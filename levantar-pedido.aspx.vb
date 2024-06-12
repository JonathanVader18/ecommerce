Imports System.Data
Imports System.IO
Imports System.Web.Services

Partial Class levantar_pedido
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub levantar_pedido_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Request.QueryString("Plantilla") <> "" Then

                ssql = " delete tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & Request.QueryString("Plantilla") & "' and cvItemCode=" & "'" & Request.QueryString("itemcode") & "'"
                objDatos.fnEjecutarInsert(ssql)

            End If

            Session("Page") = "levantar-pedido.aspx"
            ''Cargamos las plantillas que tiene el usuario
            If CInt(Session("SlpCode")) <> 0 Then
                ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE (ciIdAgenteSAP=" & "'" & Session("SlpCode") & "')"
            Else
                ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE (cvAgente=" & "'" & Session("RazonSocial") & "')"
            End If

            Dim dtPlantillas As New DataTable
            dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
            ddlPlantillas.DataSource = dtPlantillas
            ddlPlantillas.DataTextField = "Plantilla"
            ddlPlantillas.DataValueField = "No"
            ddlPlantillas.DataBind()

            If Request.QueryString.Count > 0 Then
                If dtPlantillas.Rows.Count > 0 Then
                    ddlPlantillas.SelectedValue = Request.QueryString("opc")
                    ''El detalle del template
                    fnCargaTemplate(ddlPlantillas.SelectedValue)
                End If

            Else
                If dtPlantillas.Rows.Count > 0 Then
                    ''El detalle del template
                    fnCargaTemplate(ddlPlantillas.SelectedValue)
                End If

            End If


        End If
    End Sub

    Public Sub fnCargaTemplate(Plantilla As Int32)
        ssql = "select cvItemCode ,cvItemName,cfCantidad, cfCantidad as cantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & Plantilla & "'"
        Dim dtLineas As New DataTable
        dtLineas = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        sHtmlEncabezado = "<div class='col-xs-12 no-padding'>"

        ''Usamos el template del carrito

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"

        For x = 0 To dtLineas.Rows.Count - 1 Step 1


            ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
            Dim dtMonedas As New DataTable
            ssql = ssql.Replace("[%0]", "'" & dtLineas.Rows(x)("cvItemCode") & "'")
            ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
            objDatos.fnLog("Moneda", ssql.Replace("'", ""))
            dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sMoneda1 As String = ""
            If Request.QueryString("Moneda") <> "" Then
                sMoneda1 = Request.QueryString("Moneda")
            End If
            If dtMonedas.Rows.Count > 0 Then
                sMoneda1 = dtMonedas.Rows(0)(0)
                If dtMonedas.Rows.Count > 1 Then
                    ''El articulo se puede vender en mas de una moneda
                    ''Llenamos y mostramos combo de moneda



                End If
            End If

            Session("Moneda") = sMoneda1
            Dim sCampos As String = ""
            Dim itemcode As String = dtLineas.Rows(x)("cvItemCode")

            Dim sTallaColor As String = ""
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If
            End If

            sHtmlBanner = sHtmlBanner & "<div class='body-tabla col-xs-12 no-padding'> "
            For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Plantilla_det"
                Dim dtIdLineas As New DataTable
                dtIdLineas = objDatos.fnEjecutarConsulta(ssql)

                If sTallaColor = "SI" Then
                    itemcode = dtLineas.Rows(x)("cvItemName")

                End If

                ssql = objDatos.fnObtenerQuery("Info-Producto")
                ssql = ssql.Replace("[%0]", "'" & dtLineas.Rows(x)("cvItemCode") & "'")
                Dim dtGeneral As New DataTable
                dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                If dtGeneral.Rows.Count > 0 Then
                    Dim sSrcImagen As String = ""
                    If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                        Dim iband As Int16 = 0
                        objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & itemcode & ".jpg")
                        If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode.Replace("-", "") & ".jpg") Then

                            sSrcImagen = "images/products/" & itemcode.Replace("-", "") & ".jpg"
                            iband = 1
                        End If
                        If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & ".jpg") And iband = 0 Then

                            sSrcImagen = "images/products/" & itemcode & ".jpg"
                            iband = 1
                        End If
                        If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & "-1.jpg") And iband = 0 Then
                            sSrcImagen = "images/products/" & itemcode & "-1.jpg"
                            iband = 1
                        End If
                        If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & "-2.jpg") And iband = 0 Then
                            sSrcImagen = "images/products/" & itemcode & "-2.jpg"
                            iband = 1
                        End If
                        If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & "-3.jpg") And iband = 0 Then
                            sSrcImagen = "images/products/" & itemcode & "-3.jpg"
                            iband = 1
                        End If

                        If iband = 0 Then
                            Try
                                ' "images/products/" &
                                sSrcImagen = dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo"))
                            Catch ex As Exception

                            End Try

                        End If
                        sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-1 no-padding'> "
                        sHtmlBanner = sHtmlBanner & "   <img src='" & sSrcImagen & "' class='img-responsive'>"
                        sHtmlBanner = sHtmlBanner & "</div>"
                    Else
                        If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                            If CStr(objDatos.fnObtenerCliente).ToUpper().Contains("HAWK") Then
                                Dim existencia As Double = 0
                                ''Existencia 
                                ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                                Dim dtExistencia As New DataTable
                                ssql = ssql.Replace("[%0]", "'" & itemcode & "'")
                                dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                                If dtExistencia.Rows.Count > 0 Then
                                    existencia = CDbl(dtExistencia.Rows(0)(0))
                                End If

                                sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " Stock:" & CInt(existencia) & "  <br>"
                            Else
                                sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                            End If


                        End If


                        End If
                End If

            Next
            Dim sParametros As String
            Dim dPrecio As Double = 0
            If CInt(Session("slpCode")) <> 0 Then

                dPrecio = objDatos.fnPrecioActual(dtLineas.Rows(x)("cvItemCode"), Session("ListaPrecios"))
            Else
                dPrecio = objDatos.fnPrecioActual(dtLineas.Rows(x)("cvItemCode"))
            End If
            If Session("Cliente") <> "" Then
                dPrecio = objDatos.fnPrecioActual(dtLineas.Rows(x)("cvItemCode"), Convert.ToInt16(Session("ListaPrecios")))
            End If
            objDatos.fnLog("lev", dPrecio)
            '   sParametros = "onchange=SetDefault('#txt" & dtLineas.Rows(x)("cvItemCode") & "','" & CDbl(dtLineas.Rows(x)("cfPrecio")) & "','#sub" & dtLineas.Rows(x)("cvItemCode") & "'); return false; onpaste=this.onchange('#txt" & dtLineas.Rows(x)("cvItemCode") & "'); oninput=this.onchange('#txt" & dtLineas.Rows(x)("cvItemCode") & "');"
            sParametros = "onchange=SetDefault('#txt" & x & "','" & dPrecio & "','#sub" & x & "'); return false; onpaste=this.onchange('#txt" & x & "'); oninput=this.onchange('#txt" & x & "');"

            sHtmlBanner = sHtmlBanner & "<div class='col-xs-11 no-padding'>"
            sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-2 info-producto'>" & sCampos & "</div>"

            sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4'><div class='prec-simul'>" & dPrecio.ToString("$ ###,###,###.#0") & " " & Session("Moneda") & "</div></div>"
            sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-2 no-padding'><input type='numeric' id='#txt" & x & "' value='" & dtLineas.Rows(x)("cfCantidad") & "' class='form-control sprin' " & sParametros & "></div>"
            'sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-2 text-center no-padding'><input type='numeric' id='" & dtLineas.Rows(x)("cvItemCode") & " ' value='" & dtLineas.Rows(x)("cfCantidad") & "' class='form-control sprin' " & sParametros & "></div>"
            'sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 text-center'><div class='prec-simul' id='sub" & dtLineas.Rows(x)("cvItemCode") & "'>" & CDbl(dtLineas.Rows(x)("Total")).ToString("$ ###,###,###.#0") & "</div></div>"
            sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4'><div class='prec-simul' id='sub" & x & "'>" & (dPrecio * CDbl(dtLineas.Rows(x)("cfCantidad"))).ToString("$ ###,###,###.#0") & " " & Session("Moneda") & "</div></div>"

            sHtmlBanner = sHtmlBanner & " <div class='col-xs-12 btn-action-cart'> "
            '  sHtmlBanner = sHtmlBanner & "  <div class='col-sm-2 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?Code=" & dtLineas.Rows(x)("cvItemCode") & "'>Editar</a></div> "
            sHtmlBanner = sHtmlBanner & "  <div class='col-sm-2 no-padding'><a class='action-cart' href='levantar-pedido.aspx?Plantilla=" & Plantilla & "&itemcode=" & dtLineas.Rows(x)("cvItemCode") & "'>Quitar</a></div> "

            If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Then
                If fnRevisaExistencias(dtLineas.Rows(x)("cvItemCode")) <= 0 Then
                    sHtmlBanner = sHtmlBanner & "  <div> <a class='btn btn-general-2 dif-gene' onclick=fnClick(" & "'#txt" & x & "','" & dtLineas.Rows(x)("cvItemCode") & "'); disabled>agregar</a> </div>"

                Else
                    sHtmlBanner = sHtmlBanner & "  <div> <a class='btn btn-general-2 dif-gene' onclick=fnClick(" & "'#txt" & x & "','" & dtLineas.Rows(x)("cvItemCode") & "');>agregar</a> </div>"

                End If
            Else

                If sTallaColor = "SI" Then

                    sHtmlBanner = sHtmlBanner & "  <div> <a class='btn btn-general-2 dif-gene' onclick=fnClick(" & "'#txt" & x & "','" & dtLineas.Rows(x)("cvItemName") & "');>agregar</a> </div>"
                Else
                    sHtmlBanner = sHtmlBanner & "  <div> <a class='btn btn-general-2 dif-gene' onclick=fnClick(" & "'#txt" & x & "','" & dtLineas.Rows(x)("cvItemCode") & "');>agregar</a> </div>"
                End If




            End If
            '   sHtmlBanner = sHtmlBanner & "  <div> <a class='btn btn-general-2 dif-gene' onclick=fnClick(" & "'#txt" & x & "','" & dtLineas.Rows(x)("cvItemCode") & "');>agregar</a> </div>"
            ' sHtmlBanner = sHtmlBanner & "  <div> <a class='btn btn-general-2 dif-gene' onclick=PageMethods.RegisterUser(document.getElementById('#txt" & x & "').value, '" & dtLineas.Rows(x)("cvItemCode") & "', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','','',event); } function onError(result) {}>agregar</a> </div>"
            sHtmlBanner = sHtmlBanner & " </div>"

            sHtmlBanner = sHtmlBanner & "</div>"




            sHtmlBanner = sHtmlBanner & "</div>"
        Next

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner & "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlArticulos.Controls.Clear()
        pnlArticulos.Controls.Add(literal)

    End Sub
    Public Function fnRevisaExistencias(itemCode As String) As Double
        Dim existencia As Double = 0
        ''Existencia 
        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
        Dim dtExistencia As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")

        objDatos.fnLog("Existencia", ssql.Replace("'", ""))
        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtExistencia.Rows.Count > 0 Then
            existencia = CDbl(dtExistencia.Rows(0)(0))
        End If
        Return existencia
    End Function
    <WebMethod>
    Public Shared Function RegisterUser(Cantidad As String, Articulo As String) As String
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        Dim sTallaColor As String = ""
        Dim sItemCodeGenerico As String = Articulo
        ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
        Dim dtTallasColores As New DataTable
        dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
        If dtTallasColores.Rows.Count > 0 Then
            If dtTallasColores.Rows(0)(0) = "SI" Then
                sTallaColor = "SI"
                ssql = "select cvItemCode ,cvItemName,cfCantidad, cfCantidad as cantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & HttpContext.Current.Session("IdPlantillaCliente") & "' AND cvItemName = " & "'" & Articulo & "'"
                Dim dtLineas As New DataTable
                dtLineas = objDatos.fnEjecutarConsulta(ssql)
                If dtLineas.Rows.Count > 0 Then
                    sItemCodeGenerico = dtLineas.Rows(0)(0)
                End If


            End If
        End If

        Dim partida As New Cls_Pedido.Partidas


        partida.Cantidad = Cantidad
        If sTallaColor = "SI" Then
            partida.ItemCode = sItemCodeGenerico
            partida.Generico = Articulo
        Else
            partida.ItemCode = Articulo
        End If
        Dim dPrecioActual As Double = 0
        If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

            dPrecioActual = objDatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))
        Else
            If HttpContext.Current.Session("Cliente") <> "" And HttpContext.Current.Session("UserB2C") = "" Then
                dPrecioActual = objDatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))
            Else
                dPrecioActual = objDatos.fnPrecioActual(Articulo)
            End If

        End If

        partida.Precio = dPrecioActual
        partida.TotalLinea = partida.Cantidad * partida.Precio

        ''Ahora el itemName
        Try
            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            partida.ItemName = dtItemName.Rows(0)(0)
        Catch ex As Exception

        End Try
        Dim iNumLinea As Int16 = 0
        For Each Partidacont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
            iNumLinea += 1
        Next
        partida.Linea = iNumLinea

        If HttpContext.Current.Session("Cliente") <> "" Then
            ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
            ssql = ssql.Replace("[%0]", HttpContext.Current.Session("Cliente"))
            objDatos.fnLog("ListaPrecios", ssql.Replace("'", ""))
            Dim dtLista As New DataTable
            dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtLista.Rows.Count > 0 Then
                HttpContext.Current.Session("ListaPrecios") = dtLista.Rows(0)(0)
            Else
                HttpContext.Current.Session("ListaPrecios") = "1"
            End If
            dPrecioActual = objDatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))
            partida.Descuento = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))

            ssql = "SELECT ISNULL(cvVendeSinStockB2B,'SI') from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then
                    ''Evaluamos el stock
                    Dim existencia As Double = 0
                    ''Existencia 
                    ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                    Dim dtExistencia As New DataTable
                    If sTallaColor = "SI" Then
                        ssql = ssql.Replace("[%0]", "'" & sItemCodeGenerico & "'")
                    Else
                        ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                    End If

                    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        existencia = CDbl(dtExistencia.Rows(0)(0))
                    End If
                    If existencia - Cantidad <= 0 Then
                        HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                        Exit Function
                    End If
                End If
            End If


        End If


        HttpContext.Current.Session("Partidas").add(partida)
        'Dim myPage = TryCast(HttpContext.Current.Handler, Page)
        'Try
        '    'myPage.Page.met
        '    Dim aMP As Main = CType(myPage.Master, Main)
        '    aMP.fnCargaCarrito()
        'Catch ex As Exception
        '    '      objDatos.Mensaje(ex.Message, MyPage.page)
        'End Try
        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Public Sub fnCargar()

    End Sub


    Protected Sub ddlPlantillas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantillas.SelectedIndexChanged
        Try
            ''El detalle del template
            Session("IdPlantillaCliente") = ddlPlantillas.SelectedValue
            fnCargaTemplate(ddlPlantillas.SelectedValue)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnTodos_Click(sender As Object, e As EventArgs) Handles btnTodos.Click
        ssql = "select cvItemCode ,cvItemName,cfCantidad, cfCantidad as cantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & ddlPlantillas.SelectedValue & "'"
        Dim dtLineas As New DataTable
        dtLineas = objDatos.fnEjecutarConsulta(ssql)
        For i = 0 To dtLineas.Rows.Count - 1 Step 1
            RegisterUser(dtLineas.Rows(i)("cfCantidad"), dtLineas.Rows(i)("cvItemCode"))
        Next
        objDatos.Mensaje("Artículos agregados", Me.Page)

    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click

        ssql = "delete tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & ddlPlantillas.SelectedValue & "'"
        objDatos.fnEjecutarInsert(ssql)

        ssql = "delete tienda.plantilla_hdr WHERE ciIdPlantilla= " & "'" & ddlPlantillas.SelectedValue & "'"
        objDatos.fnEjecutarInsert(ssql)

        Response.Redirect("levantar-pedido.aspx")

    End Sub
End Class
