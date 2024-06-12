Imports System.Data
Imports System.Data.SqlClient
Imports System.Xml.Serialization
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class captura_pedido
    Inherits System.Web.UI.Page
    Public objdatos As New Cls_Funciones
    Public ssql As String

    Protected Sub btnPedido_Click(sender As Object, e As EventArgs) Handles btnPedido.Click
        fnGuardaCarrito("PEDIDO")
    End Sub

    <Services.WebMethod()>
    Public Shared Function GetCustomers(prefix As String) As String()
        Dim customers As New List(Of String)()
        Dim objdatos As New Cls_Funciones
        Dim ssql As String
        'objdatos.fnLog("auto", "entra")
        Using conn As New SqlConnection()
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                'conn.ConnectionString = "Data Source=PDGDL-SQL;Initial Catalog=IP;User ID=sa;Password=P3g4dur0"
                conn.ConnectionString = "Data Source=" & dtConfSAP.Rows(0)("cvServidorSQL") & ";Initial Catalog=" & dtConfSAP.Rows(0)("cvBD") & ";User ID=" & dtConfSAP.Rows(0)("cvUserSQL") & ";Password=" & dtConfSAP.Rows(0)("cvPwdSQL")
                '   objdatos.fnLog("auto", "ConnectionString")
            End If

            ssql = objdatos.fnObtenerQuery("ArticulosAutoComplete")
            '    objdatos.fnLog("auto", ssql.Replace("'", ""))
            Using cmd As New SqlCommand()
                cmd.CommandText = ssql '"SELECT ItemCode,ItemName,(ItemCode +  '-' + ISNULL(ItemName,'')) as Descripcion FROM OITM WHERE ValidFor='Y'  AND (ItemCode like @SearchText + '%' OR ItemName like @SearchText + '%')"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        '  objdatos.fnLog("auto", sdr("Descripcion"))
                        customers.Add(String.Format("{0}@{1}", sdr("Descripcion"), sdr("ItemCode")))
                    End While
                End Using
                conn.Close()
            End Using
        End Using
        Return customers.ToArray()
    End Function


    Public Function fnExisteArticulo(Articulo As String)
        Dim iExiste As Int16 = 1

        ssql = "SELECT * from OITM where itemCode=" & "'" & Articulo & "'"
        Dim dtArticulo As New DataTable
        dtArticulo = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtArticulo.Rows.Count = 0 Then
            iExiste = 0
        End If


        Return iExiste
    End Function
    Protected Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        Dim Articulo As String = ""
        Dim sSeleccionado As String()
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        If txtcantidad.Text = "" Then
            txtcantidad.Text = "1"
        End If
        Try
            If txtSearch.Text.Contains("|") Then
                sSeleccionado = txtSearch.Text.Split("|")
                Articulo = sSeleccionado(0)
            Else
                Articulo = txtSearch.Text
                If fnExisteArticulo(txtSearch.Text) = 0 Then
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no existe, quizá no seleccionó el elemento de la lista")
                    'objDatos.Mensaje("El artículo ingresado no existe, quizá no seleccionó el elemento de la lista", Me.Page)
                    Exit Sub
                End If
            End If

            Dim partida As New Cls_Pedido.Partidas


            Dim dPrecioActual As Double = 0

            objDatos.fnLog("articulo descuento", Articulo)
            partida.ItemCode = Articulo
            partida.Cantidad = txtcantidad.Text

            ''Revisamos si aplica descuentos

            ssql = "SELECT ISNULL(cvAplicaDescto,'NO') , ISNULL(cfMaxDescto,0) FROM config.parametrizaciones "
            Dim dtDescuento As New DataTable
            dtDescuento = objDatos.fnEjecutarConsulta(ssql)

            If dtDescuento.Rows.Count > 0 Then
                If CDbl(dtDescuento.Rows(0)(1)) < CDbl(txtDesc.Text) Then
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El descuento que está otorgando es mayor al permitido")
                    '  objDatos.Mensaje("El descuento que está otorgando es mayor al permitido", Me.Page)
                    Exit Sub
                Else
                    If txtDesc1.Visible = True Then
                        If txtDesc1.Text <> "" Then
                            If CDbl(txtDesc1.Text) > 0 Then
                                objDatos.fnLog("Asigna descuento", txtDesc1.Text)
                                partida.Descuento = CDbl(txtDesc1.Text)
                            End If
                        End If

                    End If
                End If
            End If


            ' partida.Descuento = desc

            If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
            Else
                dPrecioActual = objDatos.fnPrecioActual(Articulo)
            End If

            Try
                ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                Dim dtItemName As New DataTable
                dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtItemName.Rows.Count > 0 Then
                    partida.ItemName = dtItemName.Rows(0)(0)
                End If

            Catch ex As Exception

            End Try

            If txtPrecioArt.Text <> "" Then
                Try
                    If CDbl(txtPrecioArt.Text) <> 0 Then
                        dPrecioActual = CDbl(txtPrecioArt.Text)
                    End If

                Catch ex As Exception
                    dPrecioActual = 0
                End Try
            End If
            'If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
            '    ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
            '    Dim dtTc As New DataTable
            '    dtTc = objDatos.fnEjecutarConsultaSAP(ssql)
            '    objDatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
            '    Dim iTC As Double = 1
            '    If dtTc.Rows.Count > 0 Then
            '        iTC = dtTc.Rows(0)(0)
            '    End If
            '    objDatos.fnLog("Conversion Moneda", "TC: " & iTC)
            '    If txtPrecioArt.Text <> "" Then
            '    Else
            '        dPrecioActual = dPrecioActual * iTC
            '    End If


            'Else

            'End If
            If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
                partida.MonedaGen = ddlMoneda.SelectedValue
                partida.Moneda = ddlMoneda.SelectedValue
            Else
                partida.Moneda = fnObtenerMoneda(Articulo)
                partida.MonedaGen = fnObtenerMoneda(Articulo)
            End If


            partida.Precio = dPrecioActual
            partida.TotalLinea = partida.Cantidad * partida.Precio

            If chkImpuestos.Checked = True Then
                partida.Impuesto = "SI"
            Else
                partida.Impuesto = "NO"
            End If

            Session("Partidas").add(partida)
            txtPrecioArt.Text = "0"
            txtcantidad.Text = "1"

            txtDescripcionAlt.Text = ""
            txtSearch.Text = ""
            fnCargaCarrito()

        Catch ex As Exception
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema, favor de reportarlo con esta descripción: " & ex.Message)
        End Try

    End Sub

    Public Function fnObtenerMoneda(ItemCode As String) As String
        Dim ssql As String = ""
        ''Posibles monedas en la lista de precios
        ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
        ssql = objdatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
        dtMonedas = objdatos.fnEjecutarConsultaSAP(ssql)
        Dim sMoneda As String = ""
        If Request.QueryString("Moneda") <> "" Then
            sMoneda = Request.QueryString("Moneda")
        End If
        If dtMonedas.Rows.Count > 0 Then
            sMoneda = dtMonedas.Rows(0)(0)
            If dtMonedas.Rows.Count > 1 Then
                ''El articulo se puede vender en mas de una moneda
                ''Llenamos y mostramos combo de moneda



            End If
        End If
        Session("Moneda") = sMoneda
        Return sMoneda
    End Function

    Public Sub fnCargaCarrito()
        ''Preparamos el encabezado del Grid
        Dim sHtmlBanner As String = ""
        Dim sHtmlEncabezado As String = ""
        Dim sTallaColor As String = "NO"

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Try
            objdatos.fnLog("Carrito", "For de partidas")
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    x = x + 1
                    objdatos.fnLog("Carrito", Partida.ItemCode)
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objdatos.fnObtenerQuery("Info-Producto")
                            If Partida.Generico <> "" Then
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")

                            End If
                            fnObtenerMoneda(Partida.ItemCode)
                            objdatos.fnLog("Info-prod", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objdatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objdatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-responsive'>"
                                        End If
                                    Else
                                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                End If



                                'sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                sHtmlBanner = sHtmlBanner & "</div>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                        'ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                                        'ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                        'Dim dtItemName As New DataTable
                                        'dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)

                                        objdatos.fnLog("Carrito itemNAme", Partida.ItemName)
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 info-producto'> " & sCampos & "</div>"

                    End If
                    objdatos.fnLog("Carrito", "Antes de precio")

                    If Not IsPostBack Then

                    End If
                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objdatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If

                    ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
                    Dim dtTc As New DataTable
                    dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
                    objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
                    Dim iTC As Double = 1
                    If dtTc.Rows.Count > 0 Then
                        iTC = dtTc.Rows(0)(0)
                    End If
                    objdatos.fnLog("Conversion Moneda", "TC: " & iTC)

                    'If fnObtenerMoneda(Partida.ItemCode) <> ddlMoneda.SelectedValue Then
                    Dim PrecioLista As Double = 0
                    Dim DifPrecio As Double = 0
                    Dim MonedaOriginal As String = ""

                    MonedaOriginal = fnObtenerMoneda(Partida.ItemCode)
                    PrecioLista = fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))


                    If Partida.ItemCode <> Session("ArtGenerico") Then

                        DifPrecio = Partida.Precio - PrecioLista
                        If DifPrecio < 0 Then
                            DifPrecio = DifPrecio * -1
                        End If

                        If DifPrecio > 1 Then
                            ''Diferencia mayor a 1 peso, se entiende que esta en una moneda diferente a la original y hay que dividir
                            If MonedaOriginal <> "USD" Then
                                If Partida.MonedaGen <> ddlMoneda.SelectedValue Then
                                    Partida.Precio = Partida.Precio * iTC
                                End If
                            Else
                                If Partida.MonedaGen <> ddlMoneda.SelectedValue Then
                                    Partida.Precio = Partida.Precio / iTC
                                End If
                            End If


                            Partida.Moneda = MonedaOriginal
                        Else
                            If MonedaOriginal <> "USD" Then
                                If MonedaOriginal <> ddlMoneda.SelectedValue Then
                                    Partida.Precio = Partida.Precio / iTC
                                End If
                            Else
                                If MonedaOriginal <> ddlMoneda.SelectedValue Then
                                    Partida.Precio = Partida.Precio * iTC
                                End If
                            End If


                            Partida.Moneda = MonedaOriginal
                            Partida.MonedaGen = ddlMoneda.SelectedValue
                        End If
                        objdatos.fnLog("Monedas art", fnObtenerMoneda(Partida.ItemCode) & " -  " & ddlMoneda.SelectedValue)

                        If txtSearch.Text <> "" Then
                            txtPrecioArt.Text = Partida.Precio
                        End If
                        'If Partida.MonedaGen <> ddlMoneda.SelectedValue Then  'fnObtenerMoneda(Partida.ItemCode)
                        '    If fnObtenerMoneda(Partida.ItemCode) = ddlMoneda.SelectedValue Then
                        '        Partida.Precio = Partida.Precio / iTC 'fnPrecioActual(Partida.ItemCode, Session("ListaPrecios")) * iTC
                        '    Else
                        '        Partida.Precio = Partida.Precio * iTC 'fnPrecioActual(Partida.ItemCode, Session("ListaPrecios")) * iTC
                        '    End If

                        '    Partida.Moneda = ddlMoneda.SelectedValue

                        'Else
                        '    If Partida.MonedaGen <> Partida.Moneda Then
                        '        Partida.Precio = Partida.Precio / iTC 'fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        '        Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)

                        '    Else
                        '        Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)
                        '    End If

                        'End If

                    Else
                        ''Es el generico
                        objdatos.fnLog("Monedas gen ", Partida.Moneda & " -  " & ddlMoneda.SelectedValue)

                        If Partida.Moneda = "" Then
                            Partida.Moneda = Partida.MonedaGen
                        End If
                        If Partida.MonedaGen <> ddlMoneda.SelectedValue Then
                            objdatos.fnLog(" Monedas", Partida.Moneda & " -  " & ddlMoneda.SelectedValue)


                            Partida.Precio = Partida.Precio * iTC


                            Partida.Moneda = ddlMoneda.SelectedValue

                        Else
                            If Partida.MonedaGen <> Partida.Moneda Then
                                Partida.Precio = Partida.Precio / iTC
                                Partida.Moneda = ddlMoneda.SelectedValue

                            End If

                        End If
                    End If

                    ' sHtmlBanner = sHtmlBanner & "</div>"
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If

                    If Partida.Descuento > 0 Then
                        TotDescuento = TotDescuento + (Partida.Precio - precioConDescuento)
                    End If

                    objdatos.fnLog("Carrito", precioConDescuento)
                    objdatos.fnLog("Carrito partida.precio", Partida.Precio)

                    If Partida.Descuento > 0 Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><span class='precio-con-descuento'>" & ddlMoneda.SelectedValue & " " & precioConDescuento.ToString(" ###,###,###.#0") & "</span><div class='precio-original descuento'>" & Partida.Precio.ToString("$ ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><div class='precio-original'>" & ddlMoneda.SelectedValue & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & "</div></div>"

                    End If

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div  class='precio' id='#txt" & x & "'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-total'><div class='prec-simul'> " & ddlMoneda.SelectedValue & " " & (Partida.Cantidad * precioConDescuento).ToString("###,###,###.#0") & "</div></div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart
                    sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    'PopUp('','Agregado al carrito','Aceptar');
                    If sTallaColor = "SI" Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"


                    End If
                    '  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'  href='PopUp('','Agregado al carrito','Aceptar');'>Editar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='captura-pedido.aspx?item=" & Partida.ItemCode & "&Action=d'>Quitar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart preview-popup' href='elegir-favoritos.aspx?code=" & Partida.ItemCode & "&name=" & Partida.ItemName & "'>Mover a favoritos</a></div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
                    sHtmlBanner = sHtmlBanner & "</div>   </div>"

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If
                objdatos.fnLog("Carrito", "Arma")

            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objdatos.fnLog("Carrito load", ex.Message)
        End Try

        objdatos.fnLog("Carrito", "SubTotales")

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlPartidas.Controls.Clear()
        pnlPartidas.Controls.Add(literal)
        lblSubTotal.Text = ddlMoneda.SelectedValue & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = ddlMoneda.SelectedValue & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
            Else
                Descuento = CDbl(lblDescuento.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
                Descuento = TotDescuento
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try
        lblTotal.Text = ddlMoneda.SelectedValue & " " & (sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0")
        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)


        '  objDatos.fnLog("Carrito", "slpCode y demas")

        If Session("UserB2C") <> "" Then
            pnlGuardarCarrito.Visible = False
            btnGuardar.Visible = True
            btnProcesar.Visible = True
        End If

        If CInt(Session("slpCode")) = 0 And Session("Cliente") = "" Then
            btnProcesar.Visible = True
        Else
            If CInt(Session("slpCode")) <> 0 Then

                ''Vendedores
                pnlSeparador.Visible = True
                ''Revisamos en la tabla parametrizaciones, si debemos ocultar algún boton

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '   pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    ' pnlBotonCot.Visible = True

                End If

                btnGuardar.Visible = True

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If


                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False
                pnlGuardarCarrito.Visible = False
                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            Else
                '   pnlPayPal.Visible = True
                ' btnProcesar.Visible = True
            End If

            If Session("RazonSocial") <> "" Then
                pnlPayPal.Visible = False
                pnlSeparador.Visible = True
                ''B2B

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '  pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    '  pnlBotonCot.Visible = True
                End If


                btnGuardar.Visible = True
                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If

                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False

                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            End If
        End If
    End Sub


    Public Sub fnCargaCarritoPOST()
        ''Preparamos el encabezado del Grid
        Dim sHtmlBanner As String = ""
        Dim sHtmlEncabezado As String = ""
        Dim sTallaColor As String = "NO"

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Try
            objdatos.fnLog("Carrito", "For de partidas")
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    x = x + 1
                    objdatos.fnLog("Carrito", Partida.ItemCode)
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objdatos.fnObtenerQuery("Info-Producto")
                            If Partida.Generico <> "" Then
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")

                            End If
                            fnObtenerMoneda(Partida.ItemCode)
                            objdatos.fnLog("Info-prod", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objdatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objdatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-responsive'>"
                                        End If
                                    Else
                                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                End If



                                'sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                sHtmlBanner = sHtmlBanner & "</div>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                        'ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                                        'ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                        'Dim dtItemName As New DataTable
                                        'dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)

                                        objdatos.fnLog("Carrito itemNAme", Partida.ItemName)
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 info-producto'> " & sCampos & "</div>"

                    End If
                    objdatos.fnLog("Carrito", "Antes de precio")



                    ' sHtmlBanner = sHtmlBanner & "</div>"
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If

                    If Partida.Descuento > 0 Then
                        TotDescuento = TotDescuento + (Partida.Precio - precioConDescuento)
                    End If

                    objdatos.fnLog("Carrito", precioConDescuento)
                    objdatos.fnLog("Carrito partida.precio", Partida.Precio)

                    If Partida.Descuento > 0 Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><span class='precio-con-descuento'>" & ddlMoneda.SelectedValue & " " & precioConDescuento.ToString(" ###,###,###.#0") & "</span><div class='precio-original descuento'>" & Partida.Precio.ToString("$ ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><div class='precio-original'>" & ddlMoneda.SelectedValue & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & "</div></div>"

                    End If

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div  class='precio' id='#txt" & x & "'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-total'><div class='prec-simul'> " & ddlMoneda.SelectedValue & " " & (Partida.Cantidad * precioConDescuento).ToString("###,###,###.#0") & "</div></div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart
                    sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    'PopUp('','Agregado al carrito','Aceptar');
                    If sTallaColor = "SI" Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"


                    End If
                    '  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'  href='PopUp('','Agregado al carrito','Aceptar');'>Editar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='captura-pedido.aspx?item=" & Partida.ItemCode & "&Action=d'>Quitar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart preview-popup' href='elegir-favoritos.aspx?code=" & Partida.ItemCode & "&name=" & Partida.ItemName & "'>Mover a favoritos</a></div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
                    sHtmlBanner = sHtmlBanner & "</div>   </div>"

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If
                objdatos.fnLog("Carrito", "Arma")

            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objdatos.fnLog("Carrito load", ex.Message)
        End Try

        objdatos.fnLog("Carrito", "SubTotales")

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlPartidas.Controls.Clear()
        pnlPartidas.Controls.Add(literal)
        lblSubTotal.Text = ddlMoneda.SelectedValue & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = ddlMoneda.SelectedValue & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
            Else
                Descuento = CDbl(lblDescuento.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
                Descuento = TotDescuento
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try
        lblTotal.Text = ddlMoneda.SelectedValue & " " & (sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0")
        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)


        '  objDatos.fnLog("Carrito", "slpCode y demas")

        If Session("UserB2C") <> "" Then
            pnlGuardarCarrito.Visible = False
            btnGuardar.Visible = True
            btnProcesar.Visible = True
        End If

        If CInt(Session("slpCode")) = 0 And Session("Cliente") = "" Then
            btnProcesar.Visible = True
        Else
            If CInt(Session("slpCode")) <> 0 Then

                ''Vendedores
                pnlSeparador.Visible = True
                ''Revisamos en la tabla parametrizaciones, si debemos ocultar algún boton

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '   pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    ' pnlBotonCot.Visible = True

                End If

                btnGuardar.Visible = True

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If


                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False
                pnlGuardarCarrito.Visible = False
                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            Else
                '   pnlPayPal.Visible = True
                ' btnProcesar.Visible = True
            End If

            If Session("RazonSocial") <> "" Then
                pnlPayPal.Visible = False
                pnlSeparador.Visible = True
                ''B2B

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '  pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    '  pnlBotonCot.Visible = True
                End If


                btnGuardar.Visible = True
                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If

                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False

                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            End If
        End If
    End Sub

    Public Function fnPrecioActual(itemCode As String, IdListaPrecios As Int16) As Double
        Dim ssql As String
        Dim Precio As Double = 0

        Dim DecRedondeo As Int16 = 0
        Dim iBandRedondeo As Int16 = 0
        ssql = "SELECT ISNULL(cvAplicaRedondeo,'NO'),ISNULL(ciRedondeo,0) as Digitos FROM config.parametrizaciones"
        Dim dtAplicaRedondeo As New DataTable
        dtAplicaRedondeo = objdatos.fnEjecutarConsulta(ssql)
        If dtAplicaRedondeo.Rows.Count > 0 Then
            If dtAplicaRedondeo.Rows(0)(0) = "SI" Then
                iBandRedondeo = 1
                DecRedondeo = dtAplicaRedondeo.Rows(0)(1)
            End If
        End If


        If iBandRedondeo = 1 Then
            ssql = "SELECT ROUND(ISNULL(price,'0')," & DecRedondeo & ")) FROM ITM1 WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"

        Else
            ssql = "SELECT ISNULL(price,'0') FROM ITM1 WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"
        End If

        ssql = "SELECT ISNULL(price,'0') FROM ITM1 WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"

        Dim dtPrecio As New DataTable
        dtPrecio = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtPrecio.Rows.Count > 0 Then
            Precio = dtPrecio.Rows(0)(0)
        Else
            Precio = 0
        End If

        ''Revisamos si se mostrarán precios más IVA
        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objdatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "SI" Then
                ''Obtenemos el porcentaje de IVA
                ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                Dim dtPorcIVA As New DataTable
                dtPorcIVA = objdatos.fnEjecutarConsulta(ssql)
                If dtPorcIVA.Rows.Count > 0 Then
                    Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                End If
            End If
        End If
        If iBandRedondeo = 1 Then
            Precio = Math.Round(Precio, DecRedondeo)
        End If

        objdatos.fnLog("Precio de Lista", Precio)

        ''Revisamos si la moneda natural de la lista de precios es diferente a la seleccionada, convertimos en base al tipo de cambio
        ssql = objdatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & IdListaPrecios & "'")

        objdatos.fnLog("Moneda Lista", ssql.Replace("'", ""))
        dtMonedas = objdatos.fnEjecutarConsultaSAP(ssql)
        Dim sMoneda As String = ""
        If dtMonedas.Rows.Count > 0 Then
            sMoneda = dtMonedas.Rows(0)(0)
        End If
        objdatos.fnLog("Conversion Moneda", "Moneda de la lista:" & sMoneda)
        objdatos.fnLog("Conversion Moneda", "Moneda del combo:" & ddlMoneda.SelectedValue)
        If sMoneda <> ddlMoneda.SelectedValue Then
            objdatos.fnLog("Conversion Moneda", "Entra al tipo de cambio")
            ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
            Dim dtTc As New DataTable
            dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
            objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
            Dim iTC As Double = 1
            If dtTc.Rows.Count > 0 Then
                iTC = dtTc.Rows(0)(0)
            End If
            objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
            Precio = Precio
        End If

        Return Precio
    End Function

    Public Sub fnCargaUsuario()
        ssql = "SELECT * from config.Usuarios where cvUsuario=" & "'" & lblUsuario.Text & "' "
        Dim dtAcceso As New DataTable
        dtAcceso = objdatos.fnEjecutarConsulta(ssql)
        If dtAcceso.Rows.Count > 0 Then
            Session("UserTienda") = lblUsuario.Text
            Session("slpCode") = dtAcceso.Rows(0)("ciVendedorSAP")
            Session("NombreuserTienda") = dtAcceso.Rows(0)("cvNombreCompleto")
        End If

    End Sub
    Private Sub captura_pedido_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not IsPostBack Then
                HttpContext.Current.Session("ListaPrecios") = "2"
                txtcantidad.Text = "1"
                txtCantidadGen.Text = "1"
                ''es el load, la variable debe estar llena
                If Session("UserTienda") <> "" Then
                    lblUsuario.Text = Session("UserTienda")
                Else
                    If lblUsuario.Text <> "" Then
                        fnCargaUsuario()
                    End If
                End If
                If Request.QueryString.Count > 0 Then
                    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                        If Partida.ItemCode = Request.QueryString("item") And Request.QueryString("Action") = "d" Then
                            Partida.ItemCode = "BORRAR"
                            Response.Redirect("captura-pedido.aspx")
                        End If
                    Next
                End If


                ''Las personas de contacto del cliente
                ssql = "SELECT CntctCode as Id, ISNULL(Name,'') + '( ' + ISNULL(Position,'') + ' )' + ISNULL(firstname,'') + ' ' + ISNULL(middlename,'') + ' ' + ISNULL(LastName,'') as Persona from OCPR WHERE CardCode ='" & Session("Cliente") & "'"
                objdatos.fnLog("Contacto", ssql.Replace("'", ""))
                Dim dtPersonas As New DataTable
                dtPersonas = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlPersonaContacto.DataSource = dtPersonas
                ddlPersonaContacto.DataTextField = "Persona"
                ddlPersonaContacto.DataValueField = "Id"
                ddlPersonaContacto.DataBind()


                ''Cargamos las monedas
                Dim dtMonedas As New DataTable
                dtMonedas.Columns.Add("Cve")
                dtMonedas.Columns.Add("Descripcion")
                Dim fila As DataRow
                fila = dtMonedas.NewRow
                fila("Cve") = "USD"
                fila("Descripcion") = "USD"
                dtMonedas.Rows.Add(fila)
                fila = dtMonedas.NewRow
                fila("Cve") = "MXP"
                fila("Descripcion") = "MXP"
                dtMonedas.Rows.Add(fila)
                ddlMoneda.DataSource = dtMonedas
                ddlMoneda.DataTextField = "Descripcion"
                ddlMoneda.DataValueField = "Cve"
                ddlMoneda.DataBind()

                ssql = "SELECT ISNULL(cvArticuloGEN,'') FROM config.Parametrizaciones"
                Dim dtArticuloGen As New DataTable
                dtArticuloGen = objdatos.fnEjecutarConsulta(ssql)
                If dtArticuloGen.Rows.Count > 0 Then
                    If dtArticuloGen.Rows(0)(0) <> "" Then
                        Session("ArtGenerico") = dtArticuloGen.Rows(0)(0)
                    End If
                End If
                ''Vendedores
                pnlSeparador.Visible = True
                ''Revisamos en la tabla parametrizaciones, si debemos ocultar algún boton

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '   pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    ' pnlBotonCot.Visible = True

                End If



                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If


                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False

                ssql = "select ISNULL(cvAplicaDescto,'NO') as AplicaDesc from config.Parametrizaciones "
                Dim dtParam As New DataTable
                dtParam = objdatos.fnEjecutarConsulta(ssql)
                If dtParam.Rows.Count > 0 Then
                    If dtParam.Rows(0)("AplicaDesc") = "SI" Then
                        txtDesc.Text = "0"
                        txtDesc.Visible = True
                        txtDesc1.Visible = True
                        lbldesc1.Visible = True
                        lblDesc.Visible = True
                    Else
                        lblDesc.Visible = False
                        txtDesc.Visible = False

                        txtDesc1.Visible = False
                        lbldesc1.Visible = False

                    End If
                End If
                Dim sFechaCot As String()
                Dim sFechaVence As String()

                sFechaCot = Now.Date.ToString("dd/MM/yyyy").Split("/")
                sFechaVence = Now.Date.ToString("dd/MM/yyyy").Split("/")


                txtFechaCot.Text = sFechaCot(2) & "-" & sFechaCot(1) & "-" & sFechaCot(0)
                txtFechaVence.Text = sFechaVence(2) & "-" & sFechaVence(1) & "-" & sFechaVence(0)
                fnCargaCarrito()





            Else
                If Session("UserTienda") <> "" Then
                    lblUsuario.Text = Session("UserTienda")
                Else
                    If lblUsuario.Text <> "" Then
                        fnCargaUsuario()
                    End If
                End If

                If txtSearch.Text <> "" Then

                    Dim Articulo As String = ""

                    If txtSearch.Text.Contains("|") Then
                        Dim sSeleccionado As String() = txtSearch.Text.Split("|")
                        Articulo = sSeleccionado(0)
                    Else
                        Articulo = txtSearch.Text
                    End If

                    ssql = "SELECT * FROM OITM Where itemCode=" & "'" & Articulo & "'"
                    Dim dtValida As New DataTable
                    dtValida = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtValida.Rows.Count = 0 Then
                        CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no existe, asegúrese de seleccionar el artículo de la lista")
                        Exit Sub

                    End If

                    Dim dPrecioActual As Double = 0
                    If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                        dPrecioActual = objdatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                    Else
                        dPrecioActual = objdatos.fnPrecioActual(Articulo)
                    End If

                    HttpContext.Current.Session("ListaPrecios") = "2"
                    dPrecioActual = objdatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))

                    If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
                        ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
                        Dim dtTc As New DataTable
                        dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
                        objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
                        Dim iTC As Double = 1
                        If dtTc.Rows.Count > 0 Then
                            iTC = dtTc.Rows(0)(0)
                        End If

                        If fnObtenerMoneda(Articulo) <> "USD" Then
                            objdatos.fnLog("Precio Moneda", dPrecioActual & " / " & iTC)
                            objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                            dPrecioActual = dPrecioActual / iTC
                        Else
                            objdatos.fnLog("Precio Moneda", dPrecioActual & " * " & iTC)
                            objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                            dPrecioActual = dPrecioActual * iTC
                        End If



                    Else

                    End If
                    ssql = "select onhand from OITW where itemcode='" & Articulo & "' and whscode ='00'"
                    Dim dtExistencia As New DataTable
                    dtExistencia = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        lblExistencia.Text = "Existencia: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N2")
                    End If
                    If txtPrecioArt.Text = "" Then
                        txtPrecioArt.Text = dPrecioActual
                    Else
                        If txtPrecioArt.Text = "0" Then
                            txtPrecioArt.Text = dPrecioActual
                        End If
                    End If


                End If


                ' fnCargaCarrito()
            End If
        Catch ex As Exception
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema, favor de reportarlo con esta descripción: " & ex.Message)
            Exit Sub
        End Try




    End Sub
    Protected Sub btnCotizar_Click(sender As Object, e As EventArgs) Handles btnCotizar.Click
        fnGuardaCarrito("COTIZACION")
    End Sub
    Protected Sub btnGuardarPlantilla_Click(sender As Object, e As EventArgs) Handles btnGuardarPlantilla.Click
        pnlPlantilla.Visible = True
        pnlDireccionEntrega.Visible = False
    End Sub
    Protected Sub btnGuardarPlantillaDet_Click(sender As Object, e As EventArgs) Handles btnGuardarPlantillaDet.Click
        If txtNombrePlantilla.Text = "" Then
            objdatos.Mensaje("Debe indicar un nombre para la plantilla", Me.Page)
            Exit Sub
        Else
            ssql = "select ISNULL(MAX(ciIdPlantilla),0) + 1 FROM  Tienda.Plantilla_HDR"
            Dim dtId As New DataTable
            dtId = objdatos.fnEjecutarConsulta(ssql)

            Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))

            ssql = "INSERT INTO Tienda.Plantilla_HDR ( ciIdPlantilla,cvUsuario,cvAgente,ciIdAgenteSAP,cvNombrePlantilla,cdFecha,cvComentarios) VALUES(" _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & Session("UserTienda") & "'," _
                & "'" & Session("NombreuserTienda") & "'," _
                & "'" & Session("SlpCode") & "'," _
                & "'" & txtNombrePlantilla.Text & "',GETDATE(),'')"
            objdatos.fnEjecutarInsert(ssql)

            ''Ahora las lineas
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Plantilla_det"
                    Dim dtIdLineas As New DataTable
                    dtIdLineas = objdatos.fnEjecutarConsulta(ssql)

                    ssql = "INSERT INTO Tienda.Plantilla_det (ciIdRelacion,ciIdPlantilla,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                      & "'" & dtIdLineas.Rows(0)(0) & "'," _
                      & "'" & dtId.Rows(0)(0) & "'," _
                      & "'" & Session("UserTienda") & "'," _
                      & "'" & Partida.ItemCode & "'," _
                      & "'" & Partida.ItemName & "'," _
                      & "'" & Partida.Cantidad.ToString.Replace(",", ".") & "'," _
                      & "'" & Partida.Precio.ToString.Replace(",", ".") & "'," _
                      & "'" & Partida.Descuento.ToString.Replace(",", ".") & "')"
                    objdatos.fnEjecutarInsert(ssql)
                End If
            Next
            lblMensaje.Text = "Plantilla Guardada"
            objdatos.Mensaje("Plantilla Guardada", Me.Page)
            txtNombrePlantilla.Text = ""
            pnlPlantilla.Visible = False

        End If
    End Sub

    Public Sub fnGuardaCarrito(TipoDoc As String)

        If txtFechaCot.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la fecha del documento")
            Exit Sub

        End If

        If txtFechaVence.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la fecha de vencimiento del documento")
            Exit Sub
        End If
        ''Obtenemos el tipo de cambio de hoy
        objdatos.fnLog("Cotizacion", "Va a obtener el tipo de cambio")
        ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
        Dim dtTc As New DataTable
        dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
        Dim iTC As Double = 1
        If dtTc.Rows.Count > 0 Then
            iTC = dtTc.Rows(0)(0)
        End If

        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
        Dim dtId As New DataTable
        dtId = objdatos.fnEjecutarConsulta(ssql)

        objdatos.fnLog("Cotizacion", "Antes de IdCarrito")
        Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))

        If Session("UserB2C") <> "" Then
            Session("UserTienda") = Session("UserB2C")
        End If

        ssql = "INSERT INTO Tienda.Pedido_HDR ( ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus) VALUES(" _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & Session("UserTienda") & "'," _
            & "'" & Session("NombreuserTienda") & "'," _
            & "'" & Session("SlpCode") & "'," _
            & "'" & Session("Cliente") & "'," _
            & "'" & Session("RazonSocial") & "',GETDATE(),''," _
            & "'" & Session("ListaPrecios") & "'," _
            & "'" & TipoDoc & "','ABIERTO')"
        objdatos.fnEjecutarInsert(ssql)
        objdatos.fnLog("Cotizacion", "Insertó Hdr en Carrito")
        ''Ahora las lineas
        Dim iTotal As Double = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_det"
                Dim dtIdLineas As New DataTable
                dtIdLineas = objdatos.fnEjecutarConsulta(ssql)
                objdatos.fnLog("Cotizacion-lineas", Partida.ItemCode)
                ssql = "INSERT INTO Tienda.Pedido_det (ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                  & "'" & dtIdLineas.Rows(0)(0) & "'," _
                  & "'" & dtId.Rows(0)(0) & "'," _
                  & "'" & Session("UserTienda") & "'," _
                  & "'" & Partida.ItemCode & "'," _
                  & "'" & Partida.ItemName & "'," _
                  & "'" & Partida.Cantidad & "'," _
                  & "'" & Partida.Precio.ToString.Replace(",", ".") & "'," _
                  & "'" & Partida.Descuento.ToString.Replace(",", ".") & "')"

                objdatos.fnLog("Cotizacion", ssql.Replace("'", ""))
                objdatos.fnEjecutarInsert(ssql)
                iTotal = iTotal + (Partida.Precio * Partida.Cantidad)
            End If
        Next
        objdatos.fnLog("Cotizacion", "Insertó las lineas")

        ssql = "UPDATE Tienda.Pedido_HDR  SET cfTotal=" & "'" & iTotal.ToString.Replace(",", ".") & "',cfTipoCambio=" & "'" & iTC.ToString.Replace(",", ".") & "',cfTotalFC=" & "'" & (iTotal * iTC).ToString.Replace(",", ".") & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
        objdatos.fnEjecutarInsert(ssql)

        objdatos.fnLog("Cotizacion", "Terminó en tablas")

        lblMensaje.Text = "Carrito guardado"
        ' objDatos.Mensaje("Compra procesada", Me.Page)
        If TipoDoc <> "CARRITO" Then
            objdatos.fnLog("Cotizacion", "Va a procesar SAP")
            fnProcesarSAP(iIdCarrito, TipoDoc)
        End If
        btnCotizar.Enabled = False
        btnPedido.Enabled = False
        btnGuardar.Enabled = False
        btnImprimir.Visible = True
    End Sub

    Public Function fnAsignaAlmacen(itemCode As String)
        Dim almacen As String = ""

        ssql = "select cvWhsCode  from config.Existencias where cvEstatus ='ACTIVO' order by ciOrden "
        Dim dtAlmacenes As New DataTable
        dtAlmacenes = objdatos.fnEjecutarConsulta(ssql)

        ''El primer almacen

        If dtAlmacenes.Rows.Count > 0 Then
            ssql = "SELECT OnHand FROM OITW WHERE itemCode=" & "'" & itemCode & "' ANd whsCode=" & "'" & dtAlmacenes.Rows(0)(0) & "'"
            Dim dtExistencias As New DataTable
            dtExistencias = objdatos.fnEjecutarConsultaSAP(ssql)
            objdatos.fnLog("Asigna almacen", ssql.Replace("'", ""))
            If dtExistencias.Rows.Count > 0 Then
                If CDbl(dtExistencias.Rows(0)(0)) = 0 Then
                    ''Tomamos el segundo
                    almacen = dtAlmacenes.Rows(1)(0)
                Else
                    almacen = dtAlmacenes.Rows(0)(0)
                End If
            End If

        End If

        Return almacen
    End Function
    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub

    Public Function fnProcesarSAP(idCarrito As Int64, TipoDoc As String)


        ssql = "select ISNULL(cvPreciosMasIVA,'NO') from config.parametrizaciones"
        Dim dtPreciosIVA As New DataTable
        dtPreciosIVA = objdatos.fnEjecutarConsulta(ssql)
        Dim sPreciosMasIVA As String = "NO"
        If dtPreciosIVA.Rows.Count > 0 Then
            sPreciosMasIVA = dtPreciosIVA.Rows(0)(0)
        End If

        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus FROM Tienda.Pedido_HDR WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objdatos.fnEjecutarConsulta(ssql)

        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtPartidas As New DataTable
        dtPartidas = objdatos.fnEjecutarConsulta(ssql)
        Dim sArticulosSinStock As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Dim sCardCode As String = ""
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then

                Dim sPersonaContacto As String = ""
                If txtPersonaContacto.Text <> "" Then
                    Try
                        Dim oProspecto As SAPbobsCOM.BusinessPartners

                        oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                        oProspecto.GetByKey(dtEncabezado.Rows(0)("cvCveCliente"))
                        oProspecto.ContactEmployees.Add()
                        oProspecto.ContactEmployees.FirstName = txtPersonaContacto.Text
                        oProspecto.ContactEmployees.Name = txtPersonaContacto.Text
                        oProspecto.ContactEmployees.Position = ""
                        oProspecto.ContactEmployees.E_Mail = ""
                        oProspecto.ContactEmployees.Phone1 = ""
                        oProspecto.ContactEmployees.Address = " "
                        oProspecto.ContactEmployees.Active = SAPbobsCOM.BoYesNoEnum.tYES

                        If oProspecto.Update() <> 0 Then
                            objdatos.fnLog("Actualizar contacto:", oCompany.GetLastErrorDescription)
                        Else
                            ''Todo bien
                            ssql = "SELECT MAX(CntctCode)  from OCPR where cardCode='" & dtEncabezado.Rows(0)("cvCveCliente") & "'"
                            Dim dtIdContacto As New DataTable
                            dtIdContacto = objdatos.fnEjecutarConsultaSAP(ssql)
                            If dtIdContacto.Rows.Count > 0 Then
                                sPersonaContacto = dtIdContacto.Rows(0)(0)
                            End If
                        End If


                    Catch ex As Exception

                    End Try
                End If

                If TipoDoc = "COTIZACION" Then
                    objdatos.fnLog("Cotizacion", "Crea objeto Company")
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)

                    ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                    Dim dtcliente As New DataTable
                    dtcliente = objdatos.fnEjecutarConsulta(ssql)
                    If dtcliente.Rows.Count > 0 Then
                        If dtcliente.Rows(0)(0) = "Bacán" Then
                            oDoctoVentas.DocDueDate = Now.Date.AddDays(1)

                        End If
                    End If

                    ' oDoctoVentas.Series = 59
                End If
                Try
                    oDoctoVentas.SalesPersonCode = Session("slpCode")
                Catch ex As Exception

                End Try

                If ddlPersonaContacto.Items.Count > 0 Then
                    oDoctoVentas.ContactPersonCode = ddlPersonaContacto.SelectedValue
                End If

                If sPersonaContacto <> "" Then
                    oDoctoVentas.ContactPersonCode = sPersonaContacto
                End If

                'dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.CardCode = dtEncabezado.Rows(0)("cvCveCliente")

                Dim sFechaCot As String()
                Dim sFechaVence As String()
                objdatos.fnLog("Fechas", txtFechaVence.Text)
                'sFechaCot = txtFechaCot.Text.Split("/")
                'sFechaVence = txtFechaVence.Text.Split("/")


                oDoctoVentas.DocDate = CDate(txtFechaCot.Text)
                oDoctoVentas.DocDueDate = CDate(txtFechaVence.Text)
                oDoctoVentas.DocCurrency = ddlMoneda.SelectedValue
                oDoctoVentas.Comments = "Desde Internet"

                sCardCode = dtEncabezado.Rows(0)("cvCveCliente")
                Try
                    ''Obtener RFC

                    oDoctoVentas.UserFields.Fields.Item("U_FacNit").Value = "7268540-9"
                    oDoctoVentas.UserFields.Fields.Item("U_Vendedor").Value = "Vendedor"
                    oDoctoVentas.UserFields.Fields.Item("U_FacNom").Value = dtEncabezado.Rows(0)("cvCliente")
                Catch ex As Exception

                End Try
                Try
                    oDoctoVentas.UserFields.Fields.Item("U_BXP_TIME_ENTREGA").Value = txtTiempoEntrega.Text
                Catch ex As Exception

                End Try
                Dim iLinea As Int16 = 0
                Dim sArticulosPedido As String = ""
                For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                    oDoctoVentas.Lines.Add()
                    oDoctoVentas.Lines.SetCurrentLine(iLinea)
                    If fnRevisaExistencias(dtPartidas.Rows(i)("cvItemCode")) = 0 Then
                        sArticulosSinStock = sArticulosSinStock & dtPartidas.Rows(i)("cvItemCode") & vbCrLf
                    End If
                    oDoctoVentas.Lines.ItemCode = dtPartidas.Rows(i)("cvItemCode")
                    oDoctoVentas.Lines.ItemDescription = dtPartidas.Rows(i)("cvItemName")
                    oDoctoVentas.Lines.Quantity = dtPartidas.Rows(i)("cfCantidad")
                    oDoctoVentas.Lines.Price = dtPartidas.Rows(i)("cfPrecio")
                    oDoctoVentas.Lines.Currency = ddlMoneda.SelectedValue









                    If sPreciosMasIVA = "SI" Then
                        oDoctoVentas.Lines.PriceAfterVAT = dtPartidas.Rows(i)("cfPrecio")
                    Else
                        oDoctoVentas.Lines.UnitPrice = dtPartidas.Rows(i)("cfPrecio")
                    End If

                    ''Obtener Indicador de IVA
                    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                        If Partida.ItemCode <> "BORRAR" Then
                            If Partida.ItemCode = dtPartidas.Rows(i)("cvItemCode") Then
                                If Partida.Impuesto = "SI" Then
                                    ssql = objdatos.fnObtenerQuery("ObtenerIVA")
                                Else
                                    ssql = objdatos.fnObtenerQuery("ObtenerIVA0")
                                End If
                                Exit For

                            End If
                        End If
                    Next


                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", dtPartidas.Rows(i)("cvItemCode"))
                        Dim dtIVA As New DataTable
                        dtIVA = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            oDoctoVentas.Lines.TaxCode = dtIVA.Rows(0)(0)
                        End If

                    End If

                    If CDbl(dtPartidas.Rows(i)("cfDescuento")) > 0 Then
                        oDoctoVentas.Lines.DiscountPercent = dtPartidas.Rows(i)("cfDescuento")
                    End If

                    Dim sAlmacen As String = ""
                    sAlmacen = fnAsignaAlmacen(dtPartidas.Rows(i)("cvItemCode"))
                    objdatos.fnLog("Asigna almacen", sAlmacen)
                    If sAlmacen <> "" Then
                        oDoctoVentas.Lines.WarehouseCode = sAlmacen
                    End If
                    sArticulosPedido = sArticulosPedido & String.Format("{0,200} {1,25}{2}{2}",
                                dtPartidas.Rows(i)("cvItemName"), dtPartidas.Rows(i)("cfCantidad"), vbCrLf)
                    iLinea = iLinea + 1
                Next

                Dim fEnvio As Double = 0
                ''agregamos el envio
                Try
                    fEnvio = txtEnvio.Text.Replace(",", "").Replace("$", "")

                    If fEnvio > 0 Then

                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.PriceAfterVAT = fEnvio
                        oDoctoVentas.Lines.ItemCode = "Envio"
                    End If


                Catch ex As Exception

                End Try


                objdatos.fnLog("Cotizacion", "antes del Add")
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    objdatos.fnLog("Cotizacion", "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    '  objDatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription, Me.Page)
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    btnImprimir.Visible = False
                    btnNuevo.Visible = False
                Else
                    ''Todo bien
                    btnImprimir.Visible = True
                    btnNuevo.Visible = True
                    Dim sDocnum As String = ""
                    Dim dtDoc As New DataTable
                    Session("DocEntry") = oCompany.GetNewObjectKey
                    If TipoDoc = "PEDIDO" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumOrdenVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)

                    End If
                    If TipoDoc = "COTIZACION" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumOfertaVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If dtDoc.Rows.Count > 0 Then
                        sDocnum = dtDoc.Rows(0)(0)
                        ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                        objdatos.fnEjecutarInsert(ssql)

                    End If
                    objdatos.Mensaje(TipoDoc & " procesado correctamente ", Me.Page)
                    Try
                        fnCargaCarritoPOST()


                        If sArticulosSinStock <> "" Then
                            ''Tenemos articulos sin stock...notificamos
                            ''Obtenemos correo de notificacion
                            ssql = "select ISNULL(cvNotificaExistencia,'NO'),ISNULL(cvCorreoNotificaExistencia,'') FROM config.parametrizaciones"
                            Dim dtNotifica As New DataTable
                            dtNotifica = objdatos.fnEjecutarConsulta(ssql)
                            If dtNotifica.Rows.Count > 0 Then
                                If dtNotifica.Rows(0)(0) <> "NO" Then
                                    Dim sBody As String = ""
                                    sBody = "Se ha generado un " & TipoDoc & " desde el portal de Ecommerce: " & sDocnum & vbCrLf
                                    sBody = sBody & "Sin embargo, los siguientes artículos no tienen stock:" & vbCrLf
                                    sBody = sBody & sArticulosSinStock
                                    If dtNotifica.Rows(0)(1) <> "" Then
                                        objdatos.fnEnviarCorreo(dtNotifica.Rows(0)(1), sBody, "Notificaciones ECommerce: Artículos sin stock")
                                    End If

                                End If
                            End If
                        End If

                        ''Enviamos correo
                        Dim text As String = MensajeHTML(Server.MapPath("~") & "\correo_A.html")
                        Dim sDestinatario As String = ""
                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente As New DataTable
                        dtcliente = objdatos.fnEjecutarConsulta(ssql)

                        objdatos.fnLog("Confirmacion-cliente", dtcliente.Rows(0)(0))

                        text = text.Replace("{nombrecliente}", dtcliente.Rows(0)(0))
                        text = text.Replace("{enviara}", "")
                        text = text.Replace("{direccionenvio}", "" & "</br> " & Session("Comentarios"))
                        text = text.Replace("{metodoenvio}", "")
                        text = text.Replace("{numpedido}", dtDoc.Rows(0)(0))
                        text = text.Replace("{fechapedido}", Now.Date.ToShortDateString)
                        ''Ahora las líneas
                        text = text.Replace("{lineas}", fnGeneraHTMLPartidas(dtPartidas))
                        objdatos.fnLog("Confirmacion", "Antes de enviar correo")



                        ''Revisamos si notificamos a alguien de la empresa
                        ssql = "select ISNULL(cvCorreoNotifica,'') FROM config.parametrizaciones_b2c "
                        Dim dtCorreoInterno As New DataTable
                        dtCorreoInterno = objdatos.fnEjecutarConsulta(ssql)
                        If dtCorreoInterno.Rows.Count > 0 Then
                            If dtCorreoInterno.Rows(0)(0) <> "" Then
                                objdatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, sDocnum & "- Pedido desde B2B")
                            End If
                        End If


                        ''Revisamos si hay que mandar el correo al cliente
                        If Session("slpCode") <> "0" Or Session("Cliente") <> "" Then
                            ssql = "select ISNULL(cvEnviaCorreoCliente,'NO') FROM config.parametrizaciones"
                            Dim dtEnviaCorreoCliente As New DataTable
                            dtEnviaCorreoCliente = objdatos.fnEjecutarConsulta(ssql)
                            If dtEnviaCorreoCliente.Rows.Count > 0 Then
                                If dtEnviaCorreoCliente.Rows(0)(0) = "SI" Then
                                    ssql = ""
                                    ssql = objdatos.fnObtenerQuery("Correocliente")
                                    If ssql <> "" Then
                                        ssql = ssql.Replace("[%0]", sCardCode)
                                        Dim dtCorreo As New DataTable
                                        dtCorreo = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtCorreo.Rows.Count > 0 Then
                                            If dtCorreo.Rows(0)(0) <> "" Then
                                                objdatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")

                                            End If
                                        End If
                                    End If

                                End If
                            End If
                        End If

                        pnlImprimir.Visible = True
                        btnImprimir.Visible = True

                        'ssql = "select ISNULL(cvImprimeDocumento,'NO') FROM config.parametrizaciones"
                        'Dim dtImprime As New DataTable
                        'dtImprime = objdatos.fnEjecutarConsulta(ssql)
                        'If dtImprime.Rows.Count > 0 Then
                        '    If dtImprime.Rows(0)(0) = "SI" Then
                        '        pnlImprimir.Visible = True
                        '        btnImprimir.Visible = True
                        '    End If
                        'End If
                    Catch ex As Exception

                    End Try


                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                    Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                    Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

                    Dim sComando As String
                    sComando = "<script type='text/javascript'> var opciones='left=100,top=100,width=650,height=450';window.open('vistaPrevia.aspx','Ventana',opciones);</script> "

                    'sComando = sComando & "<link rel = 'stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' integrity='sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u' crossorigin='anonymous'>"
                    'sComando = sComando & "<link href = 'https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css' rel='stylesheet' integrity='sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN' crossorigin='anonymous'>"
                    'sComando = sComando & "<link href = 'https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700' rel='stylesheet'>"
                    'sComando = sComando & "<link href = 'https://fonts.googleapis.com/css?family=Roboto:400,500,700' rel='stylesheet'>"
                    'sComando = sComando & "<link rel = 'stylesheet' href='css/jquery.bootstrap-touchspin.min.css'>"
                    'sComando = sComando & "<link rel = 'stylesheet' href='css/style.css'>"

                    ' fndescargaPDF(Session("DocEntry"))

                    'Response.Flush()
                    'Response.End()
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    ''   ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnPedido)
                    '' Response.Write(sComando)





                End If



            Else
                objdatos.Mensaje("No se ha podido establecer conexión - reintente por favor", Me.Page)
            End If

        Catch ex As Exception
            objdatos.Mensaje("No se ha podido establecer conexión - reintente por favor:" & ex.Message & "", Me.Page)
        End Try

    End Function

    Public Sub fndescargaPDF(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objdatos.fnLog("Al imprimir", "Antes RPT")
        reporte.Load(Server.MapPath("~") & "\Pedido.rpt")

        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            reporte.SetParameterValue("DocKey@", Session("DocEntry"))
            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If



        objdatos.fnLog("Al imprimir", "Sale de asignarle la BD")
        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        objdatos.fnLog("Al imprimir", "LogInfo")
        ' reporte.Refresh()

        objdatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
        reporte.SetParameterValue("DocKey@", DocEntry)
        reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "PED-" & DocEntry)
        reporte.Dispose()

        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub

    Public Function fnRevisaExistencias(itemCode As String) As Double
        Dim existencia As Double = 0
        ''Existencia 
        ssql = objdatos.fnObtenerQuery("ExistenciaSAP")
        Dim dtExistencia As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        dtExistencia = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtExistencia.Rows.Count > 0 Then
            existencia = CDbl(dtExistencia.Rows(0)(0))
        End If
        Return existencia
    End Function
    Public Function fnGeneraHTMLPartidas(dtPArtidas As DataTable) As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If
        'sLigaSitio 
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Try
            For x = 0 To dtPArtidas.Rows.Count - 1 Step 1
                objdatos.fnLog("Correo B2B", dtPArtidas.Rows.Count)
                sHtmlBanner = sHtmlBanner & " <tr>"
                '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                If dtCamposPlantilla.Rows.Count > 0 Then
                    Dim sCampos As String = ""
                    For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                        ssql = objdatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtPArtidas.Rows(x)("cvItemCode") & "'")
                        Dim dtGeneral As New DataTable
                        dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                        If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                            sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;vertical-align: middle;text-align:center;'> "
                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-height:100px;'>"
                            sHtmlBanner = sHtmlBanner & "</td>"
                        Else
                            If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                If sCampos = "" Then
                                    ''Si es el primer valor que va a enlazar, lo ponemos en strong
                                    sCampos = sCampos & "<strong style='font-size: 13px;color:#000000;'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</strong> <br>"
                                Else
                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                End If

                            Else
                                '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                            End If


                        End If
                    Next
                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:left;font-size: 10px;'>" & sCampos & "</td>"

                End If
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & CDbl(dtPArtidas.Rows(x)("cfPrecio")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & CDbl(dtPArtidas.Rows(x)("cfCantidad")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio"))).ToString(" ###,###,###.#0") & "</td>"

                sHtmlBanner = sHtmlBanner & "</tr>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                sSubTotal = sSubTotal + (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio")))  'dtPartidas.Rows(i)("cfPrecio")
                ''Aqui van los botones de Action Cart
                'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                ''sHtmlBanner = sHtmlBanner & "</div>"

                'sHtmlBanner = sHtmlBanner & " </div> "


            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try
        Return sHtmlEncabezado
    End Function
    Protected Function MensajeHTML(ArchivoHTML As [String]) As String
        Dim Cuerpo As [String] = ""
        Try
            Dim File As New System.IO.StreamReader(ArchivoHTML)

            Dim Line As [String]
            Dim text As String = System.IO.File.ReadAllText(ArchivoHTML)

            Cuerpo = text

            File.Close()
        Catch ex As Exception
            objdatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function
    Protected Sub btnCargarGen_Click(sender As Object, e As EventArgs) Handles btnCargarGen.Click
        Dim sArticulo As String()
        Dim articulo As String = ""
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        If txtCantidadGen.Text = "" Then
            txtCantidadGen.Text = "1"
        End If

        Try
            ssql = "SELECT ISNULL(cvArticuloGEN,'') FROM config.Parametrizaciones"
            Dim dtArticuloGen As New DataTable
            dtArticuloGen = objDatos.fnEjecutarConsulta(ssql)
            If dtArticuloGen.Rows.Count > 0 Then
                If dtArticuloGen.Rows(0)(0) <> "" Then
                    Session("ArtGenerico") = dtArticuloGen.Rows(0)(0)
                End If
            End If


            articulo = Session("ArtGenerico")
            Dim partida As New Cls_Pedido.Partidas


            Dim dPrecioActual As Double = 0

            objDatos.fnLog("articulo descuento", articulo)
            partida.ItemCode = articulo
            partida.Cantidad = txtCantidadGen.Text

            ''Revisamos si aplica descuentos

            ssql = "SELECT ISNULL(cvAplicaDescto,'NO') , ISNULL(cfMaxDescto,0) FROM config.parametrizaciones "
            Dim dtDescuento As New DataTable
            dtDescuento = objDatos.fnEjecutarConsulta(ssql)

            If dtDescuento.Rows.Count > 0 Then
                If CDbl(dtDescuento.Rows(0)(1)) < CDbl(txtDesc.Text) Then
                    objDatos.Mensaje("El descuento que está otorgando es mayor al permitido", Me.Page)
                    Exit Sub
                Else
                    If txtDesc.Visible = True Then
                        If txtDesc.Text <> "" Then
                            If CDbl(txtDesc.Text) > 0 Then
                                objDatos.fnLog("Asigna descuento", txtDesc.Text)
                                partida.Descuento = CDbl(txtDesc.Text)
                            End If
                        End If

                    End If
                End If
            End If


            ' partida.Descuento = desc





            If txtDescripcionAlt.Text = "" Then
                objDatos.Mensaje("Debe capturar una descripción del artículo modificable", Me.Page)
                Exit Sub
            End If
            If txtPrecio.Text = "" Then
                objDatos.Mensaje("Debe capturar el precio del artículo modificable", Me.Page)
                Exit Sub
            End If


            partida.ItemName = txtDescripcionAlt.Text
            dPrecioActual = CDbl(txtPrecio.Text)


            ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
            Dim dtTc As New DataTable
            dtTc = objDatos.fnEjecutarConsultaSAP(ssql)
            objDatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
            Dim iTC As Double = 1
            If dtTc.Rows.Count > 0 Then
                iTC = dtTc.Rows(0)(0)
            End If

            partida.MonedaGen = ddlMoneda.SelectedValue ''Usamos el nodo Generico para guardar la moneda orignal en la que fue cargado el articulo generico
            partida.Moneda = ddlMoneda.SelectedValue
            partida.Precio = dPrecioActual
            partida.TotalLinea = partida.Cantidad * dPrecioActual

            If chkImpuestos.Checked = True Then
                partida.Impuesto = "SI"
            Else
                partida.Impuesto = "NO"
            End If

            Session("Partidas").add(partida)
            fnCargaCarrito()
            txtPrecio.Text = ""
            txtCantidadGen.Text = "1"

            txtDescripcionAlt.Text = ""
            txtSearch.Text = ""

        Catch ex As Exception
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema, favor de reportarlo con esta descripción: " & ex.Message)
        End Try




    End Sub

    Private Sub ddlMoneda_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMoneda.SelectedIndexChanged
        Try
            If txtSearch.Text <> "" Then

                Dim Articulo As String = ""
                Dim sSeleccionado As String() = txtSearch.Text.Split("|")
                Articulo = sSeleccionado(0)
                Dim dPrecioActual As Double = 0
                objdatos.fnLog("DDL Monedas", ddlMoneda.SelectedValue)

                If CInt(Session("slpCode")) <> 0 Then

                    dPrecioActual = objdatos.fnPrecioActual(Articulo, Session("ListaPrecios"))
                Else
                    dPrecioActual = objdatos.fnPrecioActual(Articulo)
                End If
                objdatos.fnLog("DDL Monedas", "antes TC")
                Dim iTC As Double = 1
                ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
                Dim dtTc As New DataTable
                dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
                objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))

                If dtTc.Rows.Count > 0 Then
                    iTC = dtTc.Rows(0)(0)
                End If

                If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then

                    objdatos.fnLog("Precio Moneda", dPrecioActual & " * " & iTC)
                    objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                    dPrecioActual = dPrecioActual * iTC


                Else

                End If
                Dim DifPrecio As Double = 0
                Dim PrecioLista As Double = 0
                PrecioLista = objdatos.fnPrecioActual(Articulo, Session("ListaPrecios"))
                If txtPrecioArt.Text <> "" Or txtPrecioArt.Text <> "0" Then
                    dPrecioActual = CDbl(txtPrecioArt.Text)
                End If
                DifPrecio = dPrecioActual - PrecioLista
                If DifPrecio < 0 Then
                    DifPrecio = DifPrecio * -1
                End If
                objdatos.fnLog("DDL Monedas", "dif precio: " & DifPrecio)
                If DifPrecio > 1 Then
                    ''Diferencia mayor a 1 peso, se entiende que esta en una moneda diferente a la original y hay que dividir
                    dPrecioActual = dPrecioActual / iTC
                    txtPrecioArt.Text = dPrecioActual

                Else
                    dPrecioActual = dPrecioActual * iTC
                    txtPrecioArt.Text = dPrecioActual
                End If



            End If
        Catch ex As Exception
            objdatos.fnLog("Error DDL Monedas", ex.Message)
        End Try
        fnCargaCarrito()
    End Sub
    Protected Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        fndescargaPDF(Session("DocEntry"))
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Response.Redirect("captura-pedido.aspx")
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            Dim Articulo As String = ""

            If txtSearch.Text.Contains("|") Then
                Dim sSeleccionado As String() = txtSearch.Text.Split("|")
                Articulo = sSeleccionado(0)
            Else
                Articulo = txtSearch.Text
            End If

            ssql = "SELECT * FROM OITM Where itemCode=" & "'" & Articulo & "'"
            Dim dtValida As New DataTable
            dtValida = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtValida.Rows.Count = 0 Then
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no existe, asegúrese de seleccionar el artículo de la lista")
                Exit Sub

            End If

            Dim dPrecioActual As Double = 0
            If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                dPrecioActual = objdatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
            Else
                dPrecioActual = objdatos.fnPrecioActual(Articulo)
            End If

            HttpContext.Current.Session("ListaPrecios") = "2"
            dPrecioActual = objdatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))

            If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
                ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
                Dim dtTc As New DataTable
                dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
                objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
                Dim iTC As Double = 1
                If dtTc.Rows.Count > 0 Then
                    iTC = dtTc.Rows(0)(0)
                End If

                If fnObtenerMoneda(Articulo) <> "USD" Then
                    objdatos.fnLog("Precio Moneda", dPrecioActual & " / " & iTC)
                    objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                    dPrecioActual = dPrecioActual / iTC
                Else
                    objdatos.fnLog("Precio Moneda", dPrecioActual & " * " & iTC)
                    objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                    dPrecioActual = dPrecioActual * iTC
                End If



            Else

            End If
            ssql = "select onhand from OITW where itemcode='" & Articulo & "' and whscode ='00'"
            Dim dtExistencia As New DataTable
            dtExistencia = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtExistencia.Rows.Count > 0 Then
                lblExistencia.Text = "Existencia: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N2")
            End If
            If txtPrecioArt.Text = "" Then
                txtPrecioArt.Text = dPrecioActual
            Else
                If txtPrecioArt.Text = "0" Then
                    txtPrecioArt.Text = dPrecioActual
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
