Imports System.Data
Imports System.IO
Imports System.Web.Services
Imports Cls_Pedido

Partial Class producto_interior
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String
    Public iMuestraEtiquetas As Int16 = 0
    Public sCaracterMoneda As String = ""

    Private Sub producto_interior_Load(sender As Object, e As EventArgs) Handles Me.Load



        Dim sTallaColor As String = "NO"
        If Request.QueryString("Code") = "" Then
            Response.Redirect("index.aspx")
        Else
            objDatos.fnLog("Recarga Prod ", Request.QueryString("Code"))
            Session("ProductoVista") = Request.QueryString("Code")

            ''Validacion de si se encuentra activa la compra en B2C, solicitada por Hawk

            If Session("Cliente") = "" And CInt(Session("slpCode")) = 0 Then
                ssql = "SELECT ISNULL(cvB2CActivo,'SI') from Config.Parametrizaciones"
                Dim dtB2C As New DataTable
                dtB2C = objDatos.fnEjecutarConsulta(ssql)
                If dtB2C.Rows.Count > 0 Then
                    If dtB2C.Rows(0)(0) = "NO" Then
                        pnlCantidad.Visible = False
                        pnlAgregar.Visible = False
                        Session("B2CActivo") = "NO"

                    End If
                End If

            End If

            ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
            Dim dtCaracter As New DataTable
            dtCaracter = objDatos.fnEjecutarConsulta(ssql)
            If dtCaracter.Rows.Count > 0 Then
                sCaracterMoneda = dtCaracter.Rows(0)(0)
            End If


            ssql = "select ISNULL(cvMuestraEtiquetas,'NO') as AplicaDesc from config.Parametrizaciones "
            Dim dtMuestraEtiquetas As New DataTable
            dtMuestraEtiquetas = objDatos.fnEjecutarConsulta(ssql)
            If dtMuestraEtiquetas.Rows.Count > 0 Then
                If dtMuestraEtiquetas.Rows(0)(0) = "SI" Then
                    iMuestraEtiquetas = 1
                End If
            End If

            objDatos.fnLog("Recarga Prod carga", Request.QueryString("Code"))
            fnCargaProducto(Request.QueryString("Code"))
            Try
                ssql = "select ISNULL(cvAplicaDescto,'NO') as AplicaDesc from config.Parametrizaciones "
                Dim dtParam As New DataTable
                dtParam = objDatos.fnEjecutarConsulta(ssql)
                If dtParam.Rows.Count > 0 Then
                    If dtParam.Rows(0)("AplicaDesc") = "SI" Then
                        If Session("Cliente") <> "" And Session("UserB2C") = "" Then
                            If CInt(Session("slpCode")) <> 0 Then

                                ''El descuento solo pudiera aplicar en el módulo de Vendedores
                                pnlDescuento.Visible = True
                                pnlAgregar.Visible = False
                                pnlAgregarConDesc.Visible = True
                            End If
                        End If

                    End If
                End If
            Catch ex As Exception

            End Try


            'ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            'Dim dtTallasColores As New DataTable
            'dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            'If dtTallasColores.Rows.Count > 0 Then
            '    If dtTallasColores.Rows(0)(0) = "SI" Then
            '        STallaColor = "SI"
            '    End If

            'End If



            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If

            End If



            ''Revisamos la parametrización de las existencias
            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') ,ISNULL(cvCantidadStockBajo,'0') as MinStock from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                Dim existencia As Double = 0
                ''Evaluamos el stock
                If sTallaColor = "SI" Then
                    existencia = fnRevisaExistencias(Session("ItemCodeTallaColor"))
                Else
                    existencia = fnRevisaExistencias(Request.QueryString("Code"))
                End If



                If dtVendesinStock.Rows(0)(0) = "NO" Then


                    ''Obtenemos la cantidad minima 

                    'Session("ErrorExistencia") 
                    If existencia < CDbl(dtVendesinStock.Rows(0)(1)) Then

                        ssql = "SELECT ISNULL(cvLeyendaStockBajo,'')  from Config.Parametrizaciones"
                        Dim dtLeyenda As New DataTable
                        dtLeyenda = objDatos.fnEjecutarConsulta(ssql)
                        If dtLeyenda.Rows.Count > 0 Then
                            If dtLeyenda.Rows(0)(0) <> "" Then
                                lblLeyenda.Text = dtLeyenda.Rows(0)(0)
                            Else
                                lblLeyenda.Text = ""
                            End If

                        Else
                            lblLeyenda.Text = ""
                        End If

                        'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        If existencia = 0 Then
                            pnlAgregar.Visible = False
                            pnlAgregarConDesc.Visible = False
                            pnlAgregarConDesc.Visible = False
                        Else
                            pnlAgregar.Visible = True
                        End If

                    Else
                        pnlAgregar.Visible = True
                    End If
                Else
                    If existencia < CDbl(dtVendesinStock.Rows(0)(1)) Then
                        ssql = "SELECT ISNULL(cvLeyendaStockBajo,'')  from Config.Parametrizaciones"
                        Dim dtLeyenda As New DataTable
                        dtLeyenda = objDatos.fnEjecutarConsulta(ssql)
                        If dtLeyenda.Rows.Count > 0 Then
                            If dtLeyenda.Rows(0)(0) <> "" Then
                                lblLeyenda.Text = dtLeyenda.Rows(0)(0)
                            Else
                                lblLeyenda.Text = ""
                            End If

                        Else
                            lblLeyenda.Text = ""
                        End If
                    End If
                    'Session("ErrorExistencia") = ""
                End If


            End If




        End If
        If Session("ErrorExistencia") <> "" Then
            objDatos.Mensaje(Session("ErrorExistencia"), Me.Page)
            Session("ErrorExistencia") = ""
        Else

        End If

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            pnlAgregar.Visible = True

        End If

        Session("CondicionCombo") = ""
        Session("Imagen") = ""

        If Not IsPostBack Then



            Session("tempCantidad") = 1
            AddHandler ddlAtr1.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            AddHandler ddlAtr2.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            AddHandler ddlAtr3.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            AddHandler ddlAtr4.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            Try

            Catch ex As Exception

            End Try


            ssql = "SELECT ISNULL(cvMuestraMts2,'NO') from Config.Parametrizaciones"
            Dim dtMuestraMts As New DataTable
            dtMuestraMts = objDatos.fnEjecutarConsulta(ssql)
            If dtMuestraMts.Rows.Count > 0 Then
                If dtMuestraMts.Rows(0)(0) = "SI" Then
                    ''Mostramos los mts
                    ssql = objDatos.fnObtenerQuery("Mts")
                    ssql = ssql.Replace("[%0]", Request.QueryString("Code"))
                    Dim dtMts As New DataTable
                    dtMts = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtMts.Rows.Count > 0 Then
                        pnlMts.Visible = True
                        lblMts.Text = "<b>Mts <sup>2</sup>: </b>" & CDbl(dtMts.Rows(0)(0)).ToString("###,###.#0")
                        pnlMts2.Visible = True
                        Session("Mts2") = CDbl(dtMts.Rows(0)(0))
                    End If
                End If
            End If

            ''Revisamos si el articulo se vende por caja,y pintamos los radios
            Dim sHtmlCaja As String = objDatos.fnControlesPorCaja(Request.QueryString("Code"))
            If sHtmlCaja <> "" Then
                pnlComprarCajas.Visible = True
                Const Comillas As String = """"
                Dim literalCajas = New LiteralControl(sHtmlCaja)
                pnlComprarCajas.Controls.Clear()
                pnlComprarCajas.Controls.Add(literalCajas)

                pnlAgregar.Controls.Clear()
                sHtmlCaja = "<a class='btn btn-general-2' id='#btnAgregar" & CStr(Request.QueryString("Code")).Replace(" ", "") & "' onclick=" & Comillas & "fnClick('" & "#Cantidad', '" & CStr(Request.QueryString("Code")) & "');" & Comillas & ">agregar</a>"
                Dim literalbtnAgregar = New LiteralControl(sHtmlCaja)
                pnlAgregar.Controls.Add(literalbtnAgregar)
            End If

        Else

            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If

            End If



            ''Revisamos la parametrización de las existencias
            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') ,ISNULL(cvCantidadStockBajo,'0') as MinStock from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then
                    Dim existencia As Double = 0
                    ''Evaluamos el stock
                    If sTallaColor = "SI" Then
                        existencia = fnRevisaExistencias(Session("ItemCodeTallaColor"))
                    Else
                        existencia = fnRevisaExistencias(Request.QueryString("Code"))
                    End If

                    ''Obtenemos la cantidad minima 

                    'Session("ErrorExistencia") 
                    If existencia < CDbl(dtVendesinStock.Rows(0)(1)) Then
                        'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        ssql = "SELECT ISNULL(cvLeyendaStockBajo,'')  from Config.Parametrizaciones"
                        Dim dtLeyenda As New DataTable
                        dtLeyenda = objDatos.fnEjecutarConsulta(ssql)
                        If dtLeyenda.Rows.Count > 0 Then
                            If dtLeyenda.Rows(0)(0) <> "" Then
                                lblLeyenda.Text = dtLeyenda.Rows(0)(0)
                            Else
                                lblLeyenda.Text = ""
                            End If

                        Else
                            lblLeyenda.Text = ""
                        End If
                        If existencia = 0 Then
                            pnlAgregar.Visible = False
                            pnlAgregarConDesc.Visible = False
                            pnlAgregarConDesc.Visible = False
                        End If
                    Else
                        pnlAgregar.Visible = True
                    End If
                End If
            End If


        End If



        ''Revisamos la parametrización de las existencias (B2B), Esto por si en vendedores pueden vender sin stock y en clientes no.

        If Session("Cliente") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = "SELECT ISNULL(cvVendeSinStockB2B,'SI') ,ISNULL(cvCantidadStockBajoB2B,'0') as MinStock,ISNULL(cvLeyendaStockBajoB2B,'Artículo sin Stock') as LeyendaStock from Config.Parametrizaciones "

            Dim dtVendesinStockB2B As New DataTable
            dtVendesinStockB2B = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStockB2B.Rows.Count > 0 Then
                Dim existencia As Double = 0
                If sTallaColor = "SI" Then
                    existencia = fnRevisaExistencias(Session("ItemCodeTallaColor"))
                Else
                    existencia = fnRevisaExistencias(Request.QueryString("Code"))
                End If
                If existencia <= CDbl(dtVendesinStockB2B.Rows(0)(1)) Then
                    'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                    lblLeyenda.Text = dtVendesinStockB2B.Rows(0)("LeyendaStock")
                    If existencia = 0 Then
                        pnlCantidad.Visible = False
                        pnlAgregar.Visible = False
                        pnlAgregarConDesc.Visible = False

                    End If

                End If

            End If
        End If


    End Sub

    Public Sub fnCargarRating(ItemCode As String)

        Dim ssql As String = ""
        Dim iCalificacion As Int16 = 5
        Dim sHTML As String = ""



        Dim iStar As String = 5

        ''Vemos si hay calificacion
        ssql = "SELECT ISNULL(AVG(ciCalificacion),'5') FROM config.rate where cvItemCode=" & "'" & ItemCode & "'"
        Dim dtCalif As New DataTable
        dtCalif = objDatos.fnEjecutarConsulta(ssql)
        If dtCalif.Rows.Count > 0 Then
            If CInt(dtCalif.Rows(0)(0)) > 0 Then
                iCalificacion = CInt(dtCalif.Rows(0)(0))
            End If
        End If
        ''encabezado
        sHTML = "<div class='stars' data-calificacion='" & iCalificacion & "'><div>"

        For i = 1 To 5 Step 1
            If iCalificacion = iStar Then
                sHTML = sHTML & " <input class='star star-" & iStar & "' id='star-" & iStar & "' type='radio' name='star' onclick ='PageMethods.fnRate(" & iStar & ");' checked /><label class='star star-" & iStar & "' for='star-" & iStar & "'></label>"
            Else
                sHTML = sHTML & " <input class='star star-" & iStar & "' id='star-" & iStar & "' type='radio' name='star' onclick ='PageMethods.fnRate(" & iStar & ");'/><label class='star star-" & iStar & "' for='star-" & iStar & "'></label>"
            End If


            iStar -= 1
        Next

        sHTML = sHTML & "</div></div>"
        Dim literalRating = New LiteralControl(sHTML)
        pnlRating.Controls.Clear()
        pnlRating.Controls.Add(literalRating)
    End Sub
    'Public Function fnObtenerMoneda(ItemCode As String) As String
    '    Dim ssql As String = ""
    '    ''Posibles monedas en la lista de precios
    '    ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
    '    ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
    '    Dim dtMonedas As New DataTable
    '    ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
    '    ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
    '    dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
    '    Dim sMoneda As String = ""
    '    If Request.QueryString("Moneda") <> "" Then
    '        sMoneda = Request.QueryString("Moneda")
    '    End If
    '    If dtMonedas.Rows.Count > 0 Then
    '        sMoneda = dtMonedas.Rows(0)(0)
    '        If dtMonedas.Rows.Count > 1 Then
    '            ''El articulo se puede vender en mas de una moneda
    '            ''Llenamos y mostramos combo de moneda



    '        End If
    '    End If
    '    Session("Moneda") = sMoneda
    '    Return sMoneda
    'End Function
    <WebMethod>
    Public Shared Function fnRate(Cantidad As String) As String
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        ssql = "insert into config.Rate(cvItemCode,ciCalificacion,cdFecha)VALUES(" _
            & "'" & HttpContext.Current.Session("ProductoVista") & "'," _
            & "'" & Cantidad & "',getdate())"
        objDatos.fnEjecutarInsert(ssql)

    End Function
    <WebMethod>
    Public Shared Function CargarCarritoDesc(Cantidad As String, Desc As String, Articulo As String) As String

        HttpContext.Current.Session("AgregaCarrito") = "SI"
        Articulo = HttpContext.Current.Session("ProductoVista")
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        Dim dPrecioActual As Double = 0


        ''Validamos el descuento
        ssql = objDatos.fnObtenerQuery("ValidaDescuento")
        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            ssql = ssql.Replace("[%1]", "'" & CInt(HttpContext.Current.Session("slpCode")) & "'")
            Dim dtDescValido As New DataTable
            dtDescValido = objDatos.fnEjecutarConsulta(ssql)
            If dtDescValido.Rows.Count = 0 Then
                ''Revisamos en SAP
                dtDescValido = objDatos.fnEjecutarConsultaSAP(ssql)

            End If

            If dtDescValido.Rows.Count > 0 Then
                ''Si tenemos para validar
                If CDbl(Desc) > CDbl(dtDescValido.Rows(0)(0)) Then
                    HttpContext.Current.Session("errDescuento") = "Ha otorgado un descuento mayor al que tiene permitido: " & CStr(CDbl(dtDescValido.Rows(0)(0)).ToString("###,###.#0")) & " % "
                    Exit Function
                End If
            End If

        End If

        Try
            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then
                    ''Evaluamos el stock
                    Dim existencia As Double = 0
                    ''Existencia 
                    ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                    Dim dtExistencia As New DataTable
                    ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        existencia = CDbl(dtExistencia.Rows(0)(0))
                    End If
                    If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA") Then

                    Else
                        If existencia - Cantidad <= 0 Then
                            HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        End If
                    End If

                End If
            End If


            partida.ItemCode = Articulo
            partida.Cantidad = Cantidad
            If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then
                If HttpContext.Current.Session("ListaPrecios") Is Nothing Then
                    dPrecioActual = objDatos.fnPrecioActual(Articulo)
                Else
                    dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                End If

            Else
                dPrecioActual = objDatos.fnPrecioActual(Articulo)
            End If
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

                dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))

            End If

            partida.Precio = dPrecioActual
            If Desc = 0 Then
                ''Revisamos si no tiene desc especial
                Desc = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))

            End If
            partida.Descuento = Desc
            partida.TotalLinea = partida.Cantidad * partida.Precio



            If CDbl(HttpContext.Current.Session("Mts2")) > 0 Then
                partida.Mts2 = HttpContext.Current.Session("Mts2")
            End If

            ''Ahora el itemName

            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            partida.ItemName = dtItemName.Rows(0)(0)
        Catch ex As Exception

        End Try

        If (CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA")) And HttpContext.Current.Session("Cliente") <> "" Then


            ''Posibles monedas en la lista de precios
            ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
            ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
            Dim dtMonedas As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            ssql = ssql.Replace("[%1]", "'" & HttpContext.Current.Session("ListaPrecios") & "'")
            dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sMoneda As String = ""

            If dtMonedas.Rows.Count > 0 Then
                sMoneda = dtMonedas.Rows(0)(0)
                objDatos.fnLog("Carga carrito moneda:", sMoneda)
                partida.Moneda = sMoneda
                For Each PartidaMoneda As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    If PartidaMoneda.ItemCode <> "BORRAR" Then
                        objDatos.fnLog("Carga carrito moneda-IF:", PartidaMoneda.Moneda)
                        If PartidaMoneda.Moneda <> "" Then

                            If PartidaMoneda.Moneda <> sMoneda Then
                                HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo se quitará del carrito porque no se pueden combinar artículos en USD y MXP."
                                Exit Function
                            End If

                        End If
                    End If

                Next

            End If


        End If

        Dim iNumLinea As Int16 = 0
        For Each Partidacont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
            iNumLinea += 1
        Next
        partida.Linea = iNumLinea

        HttpContext.Current.Session("Partidas").add(partida)





        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

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
    Public Shared Function CargarCarrito(Cantidad As String, Articulo As String) As String

        HttpContext.Current.Session("AgregaCarrito") = "SI"
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones
        HttpContext.Current.Session("ProductoVista") = HttpContext.Current.Request.QueryString("Code")
        Articulo = HttpContext.Current.Session("ProductoVista")
        objDatos.fnLog("CargarCarrito", "entra:" & HttpContext.Current.Session("ProductoVista"))
        Dim dPrecioActual As Double = 0
        HttpContext.Current.Session("ErrorExistencia") = ""

        Dim CantidadExistente As Double = Cantidad
        Try

            For Each PartidaCont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                If PartidaCont.ItemCode = Articulo Then
                    CantidadExistente = CantidadExistente + PartidaCont.Cantidad
                End If
            Next

        Catch ex As Exception

            HttpContext.Current.Session("Partidas") = New List(Of Cls_Pedido.Partidas)

        End Try



        Try

            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(HttpContext.Current.Session("ProductoVista"))
            If fDescuento = 0 Then
                fDescuento = objDatos.fnObtenerDescuentoEspecialDelta(HttpContext.Current.Session("ProductoVista"))
            End If

            partida.Cantidad = Cantidad

            ' objDatos.fnLog("CargarCarrito cant:", Cantidad)
            ''Revisamos si hay que mostrar tallas y colores
            Dim sTallaColor As String = ""
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                    '    objDatos.fnLog("CargarCarrito talla color:", sTallaColor)
                    Articulo = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.Generico = HttpContext.Current.Session("ProductoVista")
                    ''Cambiamos
                    partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                    partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))

                    partida.Descuento = fDescuento
                Else
                    Articulo = HttpContext.Current.Session("ProductoVista")
                    partida.ItemCode = Articulo


                    If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then
                        If HttpContext.Current.Session("ListaPrecios") Is Nothing Then
                            dPrecioActual = objDatos.fnPrecioActual(Articulo)
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                        End If

                    Else
                        dPrecioActual = objDatos.fnPrecioActual(Articulo)

                    End If
                    objDatos.fnLog("CargarCarrito", "Antes de evaluar")
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
                        dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))

                        If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                            'B2B sin IVA
                            dPrecioActual = dPrecioActual / 1.16
                        End If

                        Dim fDescuentoEspecial As Double = 0
                        Dim fDescuentoEspecialxCantidad As Double = 0
                        fDescuentoEspecial = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                        fDescuentoEspecialxCantidad = objDatos.fnObtenerDescuentoPorCantidad(HttpContext.Current.Session("ProductoVista"), HttpContext.Current.Session("ListaPrecios"), CantidadExistente)

                        If fDescuentoEspecial >= fDescuentoEspecialxCantidad Then
                            partida.Descuento = fDescuentoEspecial
                        Else
                            partida.Descuento = fDescuentoEspecialxCantidad
                        End If
                        objDatos.fnLog("Descuento a aplicar:", "Especial:" & fDescuentoEspecial & " Volumen:" & fDescuentoEspecialxCantidad)



                    End If


                    partida.Precio = dPrecioActual
                    partida.TotalLinea = partida.Cantidad * partida.Precio




                End If
            End If

            ''Evaluamos el stock
            Dim existencia As Double = 0
            ''Existencia 

            ssql = objDatos.fnObtenerQuery("ExistenciaSAP")




            Dim dtExistencia As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            objDatos.fnLog("existencia", ssql.Replace("'", ""))
            dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtExistencia.Rows.Count > 0 Then
                existencia = CDbl(dtExistencia.Rows(0)(0))
            End If
            Try
                For Each Partida2 As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    If Partida2.ItemCode <> "BORRAR" Then
                        If Partida2.ItemCode = Articulo Then
                            existencia = existencia - Partida2.Cantidad
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try


            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"

            If HttpContext.Current.Session("RazonSocial") <> "" And CInt(HttpContext.Current.Session("slpCode")) = 0 Then
                ssql = "SELECT ISNULL(cvVendeSinStockB2B,'SI') from Config.Parametrizaciones"
            End If

            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then


                    If existencia - Cantidad < 0 Then
                        HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("SALAMA") Then

                            HttpContext.Current.Session("ErrorExistencia") = "No hay existencia del artículo seleccionado." _
                                & " Si no encontraste disponibilidad, contacta a un representante en el teléfono: 449 205 0883 en nuestros horarios de servicio."
                            Exit Function
                        Else
                            If CInt(HttpContext.Current.Session("slpCode")) > 0 And (CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA")) Then
                                ''en vendedores dejamos pasar la existencia
                            Else
                                HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                                Exit Function
                            End If

                        End If


                    End If
                Else
                    If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("BOSS") Then
                        If existencia - Cantidad < 0 Then
                            HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo no se cuenta con toda la existencia para surtir la cantidad seleccionada, el tiempo de resurtido es de 7 días hábiles."
                        End If
                    End If


                End If
            End If

            objDatos.fnLog("Carga Carrito Cliente:", HttpContext.Current.Session("Cliente"))
            objDatos.fnLog("Carga Carrito ListaPrecios:", HttpContext.Current.Session("ListaPrecios"))
            Try
                If (CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA")) And HttpContext.Current.Session("Cliente") <> "" Then


                    ''Posibles monedas en la lista de precios
                    ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
                    ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
                    Dim dtMonedas As New DataTable
                    ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                    ssql = ssql.Replace("[%1]", "'" & HttpContext.Current.Session("ListaPrecios") & "'")
                    dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
                    Dim sMoneda As String = ""

                    If dtMonedas.Rows.Count > 0 Then
                        sMoneda = dtMonedas.Rows(0)(0)
                        objDatos.fnLog("Carga carrito moneda:", sMoneda)
                        partida.Moneda = sMoneda
                        For Each PartidaMoneda As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                            If PartidaMoneda.ItemCode <> "BORRAR" Then
                                objDatos.fnLog("Carga carrito moneda-IF:", PartidaMoneda.Moneda)
                                If PartidaMoneda.Moneda <> "" Then

                                    If PartidaMoneda.Moneda <> sMoneda Then
                                        HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo se quitará del carrito porque no se pueden combinar artículos en USD y MXP."
                                        Exit Function
                                    End If

                                End If
                            End If

                        Next

                    End If


                End If
            Catch ex As Exception

            End Try



            Try
                partida.ItemCode = Articulo
                Dim iLinea As Int16 = 0
                For Each PartidaCont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    iLinea = iLinea + 1
                Next
                partida.Linea = iLinea
            Catch ex As Exception

            End Try



            '     objDatos.fnLog("CargarCarrito ", "ItemName")

            ''Ahora el itemName
            Try
                ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                objDatos.fnLog("Carga itemname:", ssql.Replace("'", ""))
                Dim dtItemName As New DataTable
                dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtItemName.Rows.Count = 0 Then
                    partida.ItemName = "ND"
                Else
                    partida.ItemName = dtItemName.Rows(0)(0)
                End If
            Catch ex As Exception

            End Try


            'If sTallaColor = "SI" Then
            '    ssql = ssql.Replace("[%0]", "'" & HttpContext.Current.Session("ItemCodeTallaColor") & "'")
            'Else
            '    ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            'End If


            Try
                If CDbl(HttpContext.Current.Session("Mts2")) > 0 Then
                    partida.Mts2 = HttpContext.Current.Session("Mts2")
                End If
            Catch ex As Exception

            End Try





            '   objDatos.fnLog("CargarCarrito ", "Add Partida: " & partida.ItemName)

            HttpContext.Current.Session("Partidas").add(partida)

            Try
                Dim cookie As HttpCookie
                cookie = HttpContext.Current.Request.Cookies("carrito")
                cookie.Value = cookie.Value & partida.ItemCode & "-" & partida.Cantidad & "-" & partida.Descuento & "-" & partida.Linea & "@"
                HttpContext.Current.Response.Cookies.Add(cookie)
            Catch ex As Exception

            End Try

        Catch ex As Exception
            objDatos.fnLog("Error en carga", ex.Message)
        End Try


        ''una vez que cargamos al carrito, validamos si es STOP Catalogo, para ver si por la cantidad de prendas no tenemos que cargar seguro o flete
        Try
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                Dim objLocal As New producto_interior
                objLocal.fnAgregaFletesSeguros_StopCatalogo()
            End If
        Catch ex As Exception

        End Try




        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Public Sub fnCalculaDescuentoSalama()

        Dim iMontoCarrito As Double = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then

                Else
                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If
                    If objDatos.fnArticuloDescuentoDelta(Partida.ItemCode) = 0 Then
                        iMontoCarrito = iMontoCarrito + (Partida.Precio * Partida.Cantidad)
                    Else
                        objDatos.fnLog("Descuentos", Partida.ItemCode & " no suma")
                    End If

                End If

            End If
        Next

        Dim fDescuento As Double = 0

        Dim dtDescuento As New DataTable
        dtDescuento = objDatos.fnCalculaDescuentoDelta(iMontoCarrito)
        If dtDescuento.Rows.Count > 0 Then
            fDescuento = dtDescuento.Rows(0)("DescActual")
            If CDbl(dtDescuento.Rows(0)("SigDescto")) = 0 Then
                Session("LeyendaDescuento") = "Felicidades! Haz alcanzado el descuento más alto."
            Else
                Session("LeyendaDescuento") = "Descto actual:" & fDescuento.ToString("##.#0") & "- Necesitas: " & CDbl(dtDescuento.Rows(0)("Leyenda")).ToString("###,###,###.#0") & " para un mejor descuento:" & CDbl(dtDescuento.Rows(0)("SigDescto")).ToString("##.#0")
            End If

        End If

        fnAsignaDescuentoDelta(fDescuento)

    End Sub
    Public Sub fnAsignaDescuentoDelta(Descuento As Double)
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then

                Else

                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If

                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.Descuento = Descuento
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    Else
                        Partida.Descuento = Descuento
                    End If

                    Dim iDescEspecialDelta As Double = 0
                    iDescEspecialDelta = objDatos.fnObtenerDescuentoEspecialDelta(Partida.ItemCode)
                    If iDescEspecialDelta > 0 Then
                        ''Tiene descuento especial, lo reemplazamos
                        Partida.Descuento = iDescEspecialDelta
                        End If


                End If

            End If
        Next
    End Sub

    Public Sub fnAgregaFletesSeguros_StopCatalogo()
        ''Asignamos descuentos
        If Session("RazonSocial") <> "" Then
            fnCalculaDescuentoSalama()
        End If

        objDatos.fnLog("Fletes STOP", "Entra")
        Dim iCantPiezasTotales As Int16 = 0
        Dim fMontoCarrito As Double = 0
        Dim iExisteFlete As Int16 = 0
        objDatos.fnLog("fnAgregaFletesSeguros_StopCatalogo", "Entra")
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then
                    iExisteFlete = 1
                Else
                    iCantPiezasTotales = iCantPiezasTotales + Partida.Cantidad
                    fMontoCarrito = fMontoCarrito + ((Partida.Precio * Partida.Cantidad) * (1 - (Partida.Descuento / 100)))
                End If

            End If
        Next

        objDatos.fnLog("Fletes STOP", "Monto Carrito: " & fMontoCarrito)
        objDatos.fnLog("Fletes STOP", "Piezas: " & iCantPiezasTotales)
        ''Teniendo las piezas totales y el importe, determinamos el monto del flete
        ''-Reglas-------------------
        ''--Cada 70 prendas se cargan 60 Pesos de flete
        ''--Por cada Mil pesos se cargan al concepto de Flete 15 pesos
        Dim iMontoFleteGratis As Double = 20000000

        If Session("RazonSocial") = "" Then
            iMontoFleteGratis = 999
        Else
            ''Por promo o estrategia, Delta manejará flete gratis al alcanzar 899 para sus socios B2B
            iMontoFleteGratis = objDatos.fnPromoFleteDeltaSocios()

        End If

        objDatos.fnLog("Fletes STOP", "Flete gratis en: " & iMontoFleteGratis)
        Dim iPiezasFlete As Int16 = 70
        Dim iMontoPorSeguro As Double = 15 '/ 1.16
        Dim iMontoPorFlete As Double = 60 '/ 1.16
        Dim iMultiploCompraSeguro As Double = 1000

        Dim iMontoFleteYSeguros As Double = 0
        Dim iMontoFlete As Double = 0
        Dim iMontoSeguro As Double = 0

        Dim sResultadoDivFlete As String()
        Dim sResultadoDivSeguro As String()

        sResultadoDivFlete = CStr(iCantPiezasTotales / iPiezasFlete).Split(".")
        sResultadoDivSeguro = CStr(fMontoCarrito / iMultiploCompraSeguro).Split(".")


        ''Primero determinar el flete x piezas
        If fMontoCarrito < iMontoFleteGratis Then
            objDatos.fnLog("Fletes STOP", "Monto carrito menor a flete")
            If CInt(sResultadoDivFlete(0)) < 1 Then
                objDatos.fnLog("Fletes STOP", "sResultadoDivFlete DIV < 1")
                iMontoFlete = iMontoPorFlete
            Else
                objDatos.fnLog("Fletes STOP", "sResultadoDivFlete DIV >=1")
                iMontoFlete = iMontoPorFlete * (CInt(sResultadoDivFlete(0)) + 1)
            End If
        Else
            objDatos.fnLog("Fletes STOP", "Monto carrito mayor a flete - Es Gratis")
            ''Flete gratis, cargamos 1 centavo y lo que se acumule
            iMontoFlete = 0

            ''Si son mas de 70 piezas (un segundo multiplo, cargamos monto extra de flete)


            '   MsgBox(sResultadoDiv(0))

            If Session("RazonSocial") = "" Then
            Else
                If CInt(sResultadoDivFlete(0)) > 1 Then
                    iMontoFlete = iMontoFlete + (iMontoPorFlete * (CInt(sResultadoDivFlete(0)) - 1))
                End If
            End If


        End If

        objDatos.fnLog("Fletes STOP", "Monto flete: " & iMontoFlete)

        If Session("RazonSocial") = "" Then

        End If

        iMontoSeguro = CDbl(sResultadoDivSeguro(0)) * iMontoPorSeguro

        If iMontoFlete = 0 Then
            iMontoSeguro = 0
        End If
        'txtFlete.Text = iMontoFlete
        'txtSeguro.Text = iMontoSeguro
        'txtFleteTotal.Text = iMontoFlete + iMontoSeguro



        ''Cargamos el flete o lo modificamos
        'If iExisteFlete = 1 Then
        '    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
        '        If Partida.ItemCode <> "BORRAR" Then
        '            If Partida.ItemCode = "FLETE-ESTAFETA" Then
        '                Partida.Precio = iMontoFlete + iMontoSeguro
        '            End If
        '        End If
        '    Next
        'Else
        '    Dim partidaFlete As New Cls_Pedido.Partidas
        '    partidaFlete.ItemCode = "FLETE-ESTAFETA"
        '    partidaFlete.Cantidad = 1
        '    partidaFlete.Precio = iMontoFlete + iMontoSeguro
        '    Session("Partidas").add(partidaFlete)
        'End If

        Session("ImporteEnvio") = iMontoFlete + iMontoSeguro

    End Sub

    Public Function fnExisteEnFavoritos(itemnCode As String) As Int16
        Dim iband As Int16 = 0


        For Each partida As Cls_Pedido.Partidas In HttpContext.Current.Session("WishList")
            If partida.ItemCode = itemnCode Then
                iband = 1
                Exit For
            End If

        Next

        Return iband

    End Function

    Public Sub fnCargaProducto(itemCode As String)
        Try
            '  objDatos.fnLog("fnCargaProducto", itemCode)
            Session("CondicionCombo") = ""
            Session("Imagen") = ""
            Session("ItemCodeTallaColor") = ""
            ssql = objDatos.fnObtenerQuery("Info-Producto")
            ssql = ssql.Replace("[%0]", "'" & itemCode & "'")


            objDatos.fnLog("Info-Producto", ssql.Replace("'", ""))


            Dim dtGeneral As New DataTable
            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtGeneral.Rows.Count = 0 Then
                Response.Redirect("index.aspx")
                Exit Sub
            End If


            ''Fichas de colores
            fnCargaFichasColores(itemCode)

            Dim sHtmlEncabezado As String = ""
            Dim sHtmlBanner As String = ""

            ''Cargamos la Imagenes
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Imagenes' order by T1.ciOrden "
            Dim dtImagenesPlantilla As New DataTable
            dtImagenesPlantilla = objDatos.fnEjecutarConsulta(ssql)
            If dtImagenesPlantilla.Rows.Count > 0 Then
                ''Metemos validacion para determinar, si es una sola imagen, no hacemos el TumbNail
                Dim iCuantas As Int16 = 0
                For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                    Else
                        If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                        Else
                            iCuantas = iCuantas + 1
                        End If
                    End If
                Next

                ''TumbNail
                sHtmlEncabezado = "<div class='col-xs-12 col-sm-6'>"

                Dim itemCodeFoto As String = itemCode
                If Session("VienePLUS") = "SI" Then
                    itemCodeFoto = itemCodeFoto & "PLUS"
                End If

                If iCuantas > -1 Then
                    sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-3'>"
                    sHtmlEncabezado = sHtmlEncabezado & " <div class='product-nav'>"

                    Dim iband As Int16 = 0
                    objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & itemCodeFoto & ".jpg")
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto.Replace("-", "") & ".jpg") Then
                        '                        sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & "images/products/" & itemCode & ".jpg" & "' class='img-responsive'  alt='descrip imagen'></div>"
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src='" & "images/products/" & itemCodeFoto.Replace("-", "") & ".jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & ".jpg") Then
                        '                        sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & "images/products/" & itemCode & ".jpg" & "' class='img-responsive'  alt='descrip imagen'></div>"
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src='" & "images/products/" & itemCodeFoto & ".jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-1.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & itemCodeFoto & "-1.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-2.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & itemCodeFoto & "-2.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-3.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & itemCodeFoto & "-3.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-4.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & itemCodeFoto & "-4.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-5.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & itemCodeFoto & "-5.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                        iband = 1
                    End If

                    If iband = 0 Then
                        For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                            If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                            Else
                                If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                                Else
                                    If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo"))) Then


                                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"

                                    Else
                                        sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                                    End If




                                End If
                            End If

                        Next
                    End If


                    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                    sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                End If

                ''Modo Normal
                sHtmlBanner = ""
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-9'>"
                sHtmlEncabezado = sHtmlEncabezado & " <div class='product-for'>"
                For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1

                    ''Es null, entonces revisamos si por default no existe con el itemocode
                    Dim iband As Int16 = 0
                    objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & itemCodeFoto & ".jpg")
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & ".jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto & ".jpg" & "'  class='zoom_img img-responsive'  data-zoom-image=" & "'" & "images/products/" & itemCodeFoto & ".jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto.Replace("-", "") & ".jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto.Replace("-", "") & ".jpg" & "' data-zoom-image=" & "'" & "images/products/" & itemCodeFoto.Replace("-", "") & ".jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If

                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-1.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto & "-1.jpg" & "' class='zoom_img img-responsive' data-zoom-image=" & "'" & "images/products/" & itemCodeFoto & "-1.jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-2.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto & "-2.jpg" & "' class='zoom_img img-responsive' data-zoom-image=" & "'" & "images/products/" & itemCodeFoto & "-2.jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-3.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto & "-3.jpg" & "' class='zoom_img img-responsive' data-zoom-image=" & "'" & "images/products/" & itemCodeFoto & "-3.jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If
                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-4.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto & "-4.jpg" & "' class='zoom_img img-responsive' data-zoom-image=" & "'" & "images/products/" & itemCodeFoto & "-4.jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If

                    If File.Exists(Server.MapPath("~") & "\images\products\" & itemCodeFoto & "-5.jpg") Then
                        sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & itemCodeFoto & "-5.jpg" & "' class='zoom_img img-responsive' data-zoom-image=" & "'" & "images/products/" & itemCodeFoto & "-5.jpg" & "' alt='descrip imagen'></span></div>"
                        iband = 1
                    End If



                    If iband = 0 Then
                        If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then

                            sHtmlBanner = sHtmlBanner & " <div><img src=" & "'images/no-image.png' class='img-responsive'  alt='descrip imagen'></div>"
                        Else

                            If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo"))) Then
                                sHtmlBanner = sHtmlBanner & " <div><span class='zoom'><img  src=" & "'images/products/" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='zoom_img img-responsive'  data-zoom-image='" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "'  alt='descrip imagen'></span></div>"
                            Else
                                sHtmlBanner = sHtmlBanner & " <div><span class='zoom'><img  src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='zoom_img img-responsive'  data-zoom-image='" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "'  alt='descrip imagen'></span></div>"
                            End If


                            'If Session("Imagen") = "" Then

                            'Else
                            '    sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & Session("Imagen") & "' class='img-responsive' alt='descrip imagen'></div>"
                            'End If

                        End If
                    End If



                    ' sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                Next
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                '  sHtmlEncabezado = sHtmlEncabezado & "</div>"
            End If

            Dim literalImagen = New LiteralControl(sHtmlEncabezado)
            pnlImagenes.Controls.Clear()
            pnlImagenes.Controls.Add(literalImagen)
            sHtmlEncabezado = ""
            sHtmlBanner = ""


            ''Posibles monedas en la lista de precios
            ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
            ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
            Dim dtMonedas As New DataTable
            ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
            ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
            objDatos.fnLog("Moneda", ssql.Replace("'", ""))
            dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
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


            Dim sPintaPrev As String = "NO"
            Dim sPintaFav As String = "NO"
            Dim sPintaCompra As String = "NO"

            ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            Dim dtPintaCat As New DataTable
            dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            If dtPintaCat.Rows.Count > 0 Then
                sPintaPrev = dtPintaCat.Rows(0)("Interior")
                sPintaFav = dtPintaCat.Rows(0)("Favorito")
                sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            End If
            Dim dPrecioActual As Double
            objDatos.fnLog("fnCargaProducto", "Nombre y descripcion")
            ''Nombre y descripcion del articulo
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Descripcion' order by T1.ciOrden "
            Dim dtTituloPlantilla As New DataTable
            dtTituloPlantilla = objDatos.fnEjecutarConsulta(ssql)
            If dtTituloPlantilla.Rows.Count > 0 Then
                sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-12 col-sm-6 info-producto-int no-padding'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='p-descripcion'>"

                If sPintaFav <> "NO" Then
                    sHtmlEncabezado = sHtmlEncabezado & "  <span class='heart preview-popup' href='elegir-favoritos.aspx?code=" & dtGeneral.Rows(0)("ItemCode") & "&name=" & dtGeneral.Rows(0)("ItemName") & "'>    <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & dtGeneral.Rows(0)("ItemCode") & "&name=" & dtGeneral.Rows(0)("ItemName") & "'></a></span>"
                End If

                'sHtmlEncabezado = sHtmlEncabezado & "  <span class='heart preview-popup'><img href='elegir-favoritos.aspx?code=" & dtGeneral.Rows(0)("ItemCode") & "&name=" & dtGeneral.Rows(0)("ItemName") & "'></span>" '
                sHtmlBanner = ""
                For i = 0 To dtTituloPlantilla.Rows.Count - 1 Step 1
                    Dim sEtiqueta As String = ""
                    If iMuestraEtiquetas = 1 Then
                        sEtiqueta = "<b>" & dtTituloPlantilla.Rows(i)("Descripcion") & ": </b><br/>"
                    End If
                    If dtTituloPlantilla.Rows(i)("Tipo") = "Cadena" Then
                        If dtTituloPlantilla.Rows(i)("Resaltado") = "SI" Then
                            sHtmlBanner = sHtmlBanner & " <h1 class='titulo'>" & sEtiqueta & dtGeneral.Rows(0)(dtTituloPlantilla.Rows(i)("Campo")) & "</h1> "
                        Else
                            sHtmlBanner = sHtmlBanner & " <div class='descripcion'>" & sEtiqueta & dtGeneral.Rows(0)(dtTituloPlantilla.Rows(i)("Campo")) & "</div> "
                        End If

                    Else

                        If dtTituloPlantilla.Rows(i)("Tipo") = "Precio" And Session("B2CActivo") <> "NO" Then

                            'If CInt(Session("slpCode")) <> 0 Then

                            '    dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"), sMoneda)
                            'Else
                            '    If Session("UserB2C") = "" Then
                            '        dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"), sMoneda)
                            '    Else
                            '        dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"))
                            '    End If

                            'End If

                            If CInt(Session("slpCode")) <> 0 Then

                                dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Session("ListaPrecios"))
                            Else
                                If Session("Cliente") <> "" Then
                                    dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Session("ListaPrecios"))
                                Else
                                    dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")))
                                End If
                            End If

                            objDatos.fnLog("fnCargaProducto", "sale de precio:" & dPrecioActual)
                            'sHtmlBanner = sHtmlBanner & "  <div class='col-xs-12 no-padding'><div class='precio sec-prec'> " & dPrecioActual.ToString("$ ###,###,###.#0") & " " & sMoneda & "</div></div>"


                            If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) And Session("Cliente") <> "" Then
                                'B2B antes de IVA
                                dPrecioActual = dPrecioActual / 1.16
                            End If


                            lblPrecio.Text = sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & sMoneda

                        Else

                            If Session("Cliente") <> "" Then
                                If CInt(Session("slpCode")) <> 0 Then

                                    dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Session("ListaPrecios"))
                                Else
                                    dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")))
                                End If
                                If Session("Cliente") <> "" Then
                                    dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Session("ListaPrecios"))
                                End If


                                If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) And Session("Cliente") <> "" Then
                                    'B2B antes de IVA
                                    dPrecioActual = dPrecioActual / 1.16
                                End If


                                lblPrecio.Text = sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & sMoneda

                            End If


                        End If

                    End If


                Next



                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

                If Session("Cliente") <> "" Then
                    Dim descEspecial As Double = 0
                    descEspecial = objDatos.fnDescuentoEspecial((dtGeneral.Rows(0)("ItemCode")), Session("Cliente"))
                    If descEspecial > 0 Then
                        lblPreciodesc.Visible = True
                        lblPreciodesc.Text = sCaracterMoneda & " " & (dPrecioActual * (1 - (descEspecial / 100))).ToString("###,###,###.#0") & " " & sMoneda
                        lblPrecio.Font.Strikeout = True
                    End If

                End If
                If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("STOP CAT") Then ' And Session("RazonSocial") <> "" 
                    Dim descuentoPromo As Double = 0
                    descuentoPromo = objDatos.fnObtenerDescuentoEspecialDelta((dtGeneral.Rows(0)("ItemCode")))
                    If descuentoPromo > 0 Then
                        If dPrecioActual = 0 Then
                            dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Convert.ToInt16(Session("ListaPrecios")))
                        End If
                        lblPreciodesc.Visible = True
                        lblPreciodesc.Text = sCaracterMoneda & " " & (dPrecioActual * (1 - (descuentoPromo / 100))).ToString("###,###,###.#0") & " " & sMoneda
                        lblPrecio.Font.Strikeout = True

                    End If

                Else

                End If


                ssql = "SELECT ISNULL(cvCalificaProductos,'NO') FROM config.Parametrizaciones "
                Dim dtCalifica As New DataTable
                dtCalifica = objDatos.fnEjecutarConsulta(ssql)
                If dtCalifica.Rows.Count > 0 Then
                    If dtCalifica.Rows(0)(0) = "SI" Then
                        pnlRating.Visible = True
                        fnCargarRating(dtGeneral.Rows(0)("ItemCode"))
                        'sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-12 no-padding'> "
                        'sHtmlEncabezado = sHtmlEncabezado & " <div class='progress'> "
                        'sHtmlEncabezado = sHtmlEncabezado & " <span class='starts'><img src='img/catalogo/favorito.png'></span> "
                        'sHtmlEncabezado = sHtmlEncabezado & " <div class='progress-bar' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:70%'> "
                        'sHtmlEncabezado = sHtmlEncabezado & " <span class='sr-only'>70% Complete</span> "
                        'sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
                    Else
                        pnlRating.Visible = False
                    End If
                End If




                '  sHtmlEncabezado = sHtmlEncabezado & "</div>"
            End If


            Dim literal As New LiteralControl(sHtmlEncabezado)
            pnlInfoProducto.Controls.Clear()
            pnlInfoProducto.Controls.Add(literal)

            objDatos.fnLog("fnCargaProducto", "Muestra existencias?")
            ''Existencia 
            ssql = "select ISNULL(cvMuestraExistencias,'NO') FROM config.parametrizaciones"
            Dim dtMuestra As New DataTable
            dtMuestra = objDatos.fnEjecutarConsulta(ssql)
            If dtMuestra.Rows.Count > 0 Then
                If dtMuestra.Rows(0)(0) <> "NO" Then
                    If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAPB2B")
                    Else
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                    End If

                    Dim dtExistencia As New DataTable
                    ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
                    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        lblExistencia.Text = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"
                    End If

                Else
                    pnlExistencia.Visible = False
                End If
            End If


            objDatos.fnLog("fnCargaProducto", "Leyenda de bajo stock")
            Dim iCantidadLeyenda As Double = 0
            ''Leyenda de stock bajo
            ssql = "select ISNULL(cvLeyendaStockBajo,'NO') as Leyenda, ISNULL(cvCantidadStockBajo,0) as Cantidad FROM config.parametrizaciones"
            Dim dtLeyenda As New DataTable
            dtLeyenda = objDatos.fnEjecutarConsulta(ssql)
            lblLeyenda.Text = ""
            If dtLeyenda.Rows.Count > 0 Then

                iCantidadLeyenda = CDbl(dtLeyenda.Rows(0)(1))
                If dtLeyenda.Rows(0)(0) <> "NO" Then
                    ''Evaluamos si el stock cumple
                    If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAPB2B")
                    Else
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                    End If


                    Dim dtExistencia As New DataTable
                    ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
                    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        ''Revisamos si la cantidad para detonar la leyenda, no viene de un query
                        Dim ssqlLeyenda As String = ""
                        ssqlLeyenda = objDatos.fnObtenerQuery("LeyendaStockMinimo")
                        If ssqlLeyenda <> "" Then
                            ssqlLeyenda = ssqlLeyenda.Replace("[%0]", "'" & itemCode & "'")
                            Dim dtStockMinLeyenda As New DataTable
                            dtStockMinLeyenda = objDatos.fnEjecutarConsultaSAP(ssqlLeyenda)
                            If dtStockMinLeyenda.Rows.Count > 0 Then
                                iCantidadLeyenda = CDbl(dtStockMinLeyenda.Rows(0)(0))
                            End If
                        End If

                        If CDbl(dtExistencia.Rows(0)(0)) <= iCantidadLeyenda Then
                            pnlLeyendaStock.Visible = True
                            '  lblLeyenda.Text = dtLeyenda.Rows(0)(0)
                        End If
                    End If

                End If
            End If

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = "SELECT ISNULL(cvMuestraStock,'NO') FROM [config].[ParametrizacionesB2B]"
                Dim dtParamB2B As New DataTable
                dtParamB2B = objDatos.fnEjecutarConsulta(ssql)
                If dtParamB2B.Rows.Count > 0 Then
                    If dtParamB2B.Rows(0)(0) = "SI" Then
                        pnlExistencia.Visible = True
                        pnlLeyendaStock.Visible = True
                    Else
                        pnlExistencia.Visible = False
                        pnlLeyendaStock.Visible = False
                    End If
                End If
            End If


            Dim STallaColor As String = "NO"
            If Not IsPostBack Then
                ''Revisamos si hay que mostrar tallas y colores
                ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
                Dim dtTallasColores As New DataTable
                dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
                If dtTallasColores.Rows.Count > 0 Then
                    If dtTallasColores.Rows(0)(0) = "SI" Then
                        STallaColor = "SI"
                        pnlTallaColor.Visible = True
                        ''nos traemos los atributos de la tabla de tallaColor
                        ssql = "SELECT * FROM config.tallaColor "
                        Dim dtConfigTallas As New DataTable
                        dtConfigTallas = objDatos.fnEjecutarConsulta(ssql)
                        For atr = 0 To dtConfigTallas.Rows.Count - 1 Step 1
                            For Each control As System.Web.UI.Control In pnlTallaColor.Controls
                                If control.ClientID.Contains("Atr" & (atr + 1)) Then
                                    control.Visible = True
                                End If

                                Dim iOcultar As Int16 = 0
                                If control.ClientID.Contains("ddl") Then
                                    If DirectCast(control, System.Web.UI.WebControls.DropDownList).ClientID.Contains("Atr" & (atr + 1)) Then
                                        DirectCast(control, System.Web.UI.WebControls.DropDownList).Visible = True
                                        ''Llenamos el combo
                                        ssql = objDatos.fnObtenerQuery(dtConfigTallas.Rows(atr)("cvQuery"))
                                        If Session("VienePLUS") = "SI" Then
                                            If (dtConfigTallas.Rows(atr)("cvQuery")) = "Talla" Then
                                                ssql = objDatos.fnObtenerQuery("TallaPlus")
                                            End If
                                        End If

                                        ssql = ssql.Replace("[%0]", itemCode)
                                        ssql = ssql & " " & Session("CondicionCombo")



                                        objDatos.fnLog("Filtro Talla Color", ssql.Replace("'", ""))
                                        Dim dtDatosCombo As New DataTable
                                        dtDatosCombo = objDatos.fnEjecutarConsultaSAP(ssql)

                                        If dtDatosCombo.Rows.Count > 1 Then
                                            'Dim fila As DataRow
                                            'fila = dtDatosCombo.NewRow
                                            'fila("valor") = "-Seleccione-"
                                            'fila("Descripcion") = "-Seleccione-"
                                            'dtDatosCombo.Rows.Add(fila)
                                        End If

                                        DirectCast(control, System.Web.UI.WebControls.DropDownList).DataSource = dtDatosCombo
                                        DirectCast(control, System.Web.UI.WebControls.DropDownList).DataTextField = "descripcion"
                                        DirectCast(control, System.Web.UI.WebControls.DropDownList).DataValueField = "valor"
                                        DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip = (atr + 1)
                                        DirectCast(control, System.Web.UI.WebControls.DropDownList).DataBind()
                                        If dtDatosCombo.Rows.Count > 0 Then
                                            If dtDatosCombo.Rows(0)("descripcion") = "Ninguno" Then
                                                DirectCast(control, System.Web.UI.WebControls.DropDownList).Visible = False
                                                fnOcultarLabel("lblAtr" & (atr + 1))

                                            End If
                                        End If

                                        Try
                                            DirectCast(control, System.Web.UI.WebControls.DropDownList).SelectedValue = "-Seleccione-"
                                        Catch ex As Exception

                                        End Try

                                    End If
                                End If
                                If control.ClientID.Contains("lbl") Then
                                    If control.ClientID.Contains("lbl" & "Atr" & (atr + 1)) Then
                                        DirectCast(control, System.Web.UI.WebControls.Label).Text = dtConfigTallas.Rows(atr)("cvTallaColor")

                                    End If
                                End If

                            Next
                        Next


                        ''


                        objDatos.fnLog("Calcula PRecios", "Va a entrar")
                        CalculaPrecio()
                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente As New DataTable
                        dtcliente = objDatos.fnEjecutarConsulta(ssql)

                        If dtcliente.Rows.Count > 0 Then
                            If dtcliente.Rows(0)(0) = "Lazarus" Then
                                ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Session("ItemCodeTallaColor") & "'"
                                objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                Dim dtFoto As New DataTable
                                dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                If dtFoto.Rows.Count > 0 Then
                                    Session("Imagen") = dtFoto.Rows(0)(0)
                                    objDatos.fnLog("ddl_sel_foto", Session("Imagen"))



                                End If

                            End If
                        End If
                    End If
                End If
            Else
                ''Revisamos si hay que mostrar tallas y colores
                ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
                Dim dtTallasColores As New DataTable
                dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
                If dtTallasColores.Rows.Count > 0 Then
                    If dtTallasColores.Rows(0)(0) = "SI" Then
                        STallaColor = "SI"
                    End If
                End If

            End If
            objDatos.fnLog("fnCargaProducto", "antes de talla Color: " & STallaColor)
            If STallaColor = "SI" Then
                CalculaPrecio()
            End If

            ''Detalles, caracteristicas e información
            ''Obtenemos el query para saber como se debe mostrar esta información
            Dim dtAtr As New DataTable
            sHtmlEncabezado = ""

            'Si es un kit, ponemos al inicio una tabla con sus componentes
            sHtmlEncabezado = sHtmlEncabezado & objDatos.fnPestañaKits(dtGeneral.Rows(0)("ItemCode"))

            ssql = "select * from config.ProductoInterior where cvEstatus ='ACTIVO'"
            Dim dtPestañas As New DataTable
            dtPestañas = objDatos.fnEjecutarConsulta(ssql)
            For z = 0 To dtPestañas.Rows.Count - 1 Step 1
                ' objDatos.fnLog("Pestañas", dtPestañas.Rows(z)("cvAtributo"))
                ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                           & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                           & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-" & dtPestañas.Rows(z)("cvAtributo") & "' order by T1.ciOrden "
                Dim dtDetallesPlantilla As New DataTable
                dtDetallesPlantilla = objDatos.fnEjecutarConsulta(ssql)
                objDatos.fnLog("fnCargaProducto", "Pestañas y características")
                If dtDetallesPlantilla.Rows.Count > 0 Then
                    Dim iPinta As Int16 = 1

                    If CStr(dtPestañas.Rows(z)("cvLeyenda")).Contains("Comprometido") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) <> 0) Then
                        iPinta = 1
                    Else
                        If Not CStr(dtPestañas.Rows(z)("cvLeyenda")).Contains("Comprometido") Then
                            iPinta = 1
                        Else
                            iPinta = 0
                        End If
                    End If


                    If iPinta = 1 Then

                        sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-12 no-padding info-producto-int'>"
                        sHtmlEncabezado = sHtmlEncabezado & " <div class='Caracteristicas'> "
                        'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-group filtos-catalogo' id='accordion' role='tablist' aria-multiselectable='true'> "
                        sHtmlEncabezado = sHtmlEncabezado & "  <div class='filtos-catalogo panel-group tymnyce' id='accordioninfo' role='tablist' aria-multiselectable='true'> "
                        sHtmlEncabezado = sHtmlEncabezado & "   <div class='panel'> "
                        sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-heading' role='tab' id='heading" & z & "'> "
                        sHtmlEncabezado = sHtmlEncabezado & "     <h4 class='categoria'> "
                        sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' data-toggle='collapse' data-parent='#accordioninfo' href='#thir" & z & "' aria-expanded='true' aria-controls='thir" & z & "'> " & dtPestañas.Rows(z)("cvLeyenda") & " </a>"
                        sHtmlEncabezado = sHtmlEncabezado & "     </h4> </div>"
                        sHtmlEncabezado = sHtmlEncabezado & "   <div id='thir" & z & "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='thir" & z & "'>"
                        sHtmlBanner = ""
                        sHtmlBanner = sHtmlBanner & "<div class='panel-body'>"
                        Dim iTabla As Int16 = 0
                        For i = 0 To dtDetallesPlantilla.Rows.Count - 1 Step 1
                            If dtDetallesPlantilla.Rows(i)("Tipo") = "Link" Then
                                'objDatos.fnLog("Anexo", dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo")))
                                Dim sArchivo As String = ""
                                If dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                Else
                                    sArchivo = dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo"))
                                End If
                                sHtmlBanner = sHtmlBanner & "<a class='descargable' href='" & sArchivo & "' target='_blank'> " & dtDetallesPlantilla.Rows(i)("Descripcion") & " </a></br>"
                            Else
                                '   objDatos.fnLog("Pestaña", dtDetallesPlantilla.Rows(i)("campo"))
                                If dtDetallesPlantilla.Rows(i)("Tipo") = "Query" Then
                                    Dim sContenido As String = ""
                                    ssql = objDatos.fnObtenerQuery(dtDetallesPlantilla.Rows(i)("Campo"))
                                    ' objDatos.fnLog("Al obtener Query", ssql.Replace("'", ""))
                                    ssql = ssql.Replace("[%0]", dtGeneral.Rows(0)("ItemCode"))
                                    '  objDatos.fnLog("TipoQuery", ssql.Replace("'", ""))
                                    Dim dtDatosPestaña As New DataTable
                                    dtDatosPestaña = objDatos.fnEjecutarConsultaSAP(ssql)



                                    If dtDatosPestaña.Rows.Count > 0 Then
                                        sHtmlBanner = sHtmlBanner & "<table class='table table-striped table-bordered' style='width:100%' id='example" & z & "'>" 'class='table table-sm'
                                        sHtmlBanner = sHtmlBanner & "<thead><tr>"
                                        For col = 0 To dtDatosPestaña.Columns.Count - 1 Step 1
                                            sHtmlBanner = sHtmlBanner & "<th>" & dtDatosPestaña.Columns(col).ColumnName & "</th>"
                                        Next
                                        sHtmlBanner = sHtmlBanner & "</tr></thead><tbody>"


                                        For atr = 0 To dtDatosPestaña.Rows.Count - 1 Step 1
                                            sHtmlBanner = sHtmlBanner & "<tr>"
                                            For colRow = 0 To dtDatosPestaña.Columns.Count - 1 Step 1

                                                If dtDatosPestaña.Rows(atr)(colRow) Is DBNull.Value Then
                                                    sContenido = ""

                                                Else
                                                    sContenido = dtDatosPestaña.Rows(atr)(colRow)

                                                End If
                                                sHtmlBanner = sHtmlBanner & "<td>" & sContenido & "</td>"
                                            Next
                                            sHtmlBanner = sHtmlBanner & "</tr>"
                                        Next


                                        sHtmlBanner = sHtmlBanner & "</tbody></table>"
                                    Else
                                        If dtDatosPestaña.Rows.Count = 1 Then
                                            sHtmlBanner = sHtmlBanner & dtDatosPestaña.Rows(0)(0) & " </br>"
                                        End If
                                    End If


                                Else

                                    If dtDetallesPlantilla.Rows(i)("Tipo") = "Video" Then
                                        Dim sHtmlVideo As String = ""
                                        Dim sLigaVideo As String = ""




                                        Dim sLiga As String()
                                        If CStr(dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo"))).Length > 0 Then
                                            sLiga = CStr(dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo"))).Split("v=")

                                            Try
                                                sLigaVideo = sLiga(1)

                                            Catch ex As Exception

                                            End Try
                                            If sLigaVideo <> "" Then
                                                sHtmlVideo = "<iframe width='100%' height='315'src='https://www.youtube.com/embed/" & sLigaVideo.Replace("=", "") & "'"
                                                sHtmlVideo = sHtmlVideo & "frameborder='0' allow='accelerometer; autoplay; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe> "
                                                sHtmlBanner = sHtmlBanner & "" & sHtmlVideo & "</br>"
                                            End If
                                        End If


                                    Else
                                        Dim sPropMostrar As String = ""
                                        If dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                        Else
                                            sPropMostrar = dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo"))
                                            '    objDatos.fnLog("Pestañas", sPropMostrar)
                                        End If

                                        If sPropMostrar.Contains("*") Then
                                            Dim sRenglones As String()
                                            sRenglones = sPropMostrar.Split("*")
                                            sHtmlBanner = sHtmlBanner & "<ul>"
                                            For Each cadena As String In sRenglones
                                                If cadena.Length > 2 Then
                                                    sHtmlBanner = sHtmlBanner & "<li>" & cadena & "</li>"
                                                End If

                                            Next
                                            sHtmlBanner = sHtmlBanner & "</ul>"
                                        Else
                                            sHtmlBanner = sHtmlBanner & "" & sPropMostrar & "</br>"
                                        End If


                                    End If

                                End If

                            End If

                        Next

                        sHtmlBanner = sHtmlBanner & "</div>"
                        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                        sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
                        sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
                    End If


                End If
            Next

            sHtmlEncabezado = sHtmlEncabezado & "</div>" ''Viene de arriba
            sHtmlEncabezado = sHtmlEncabezado & "</div>"
            literal = New LiteralControl(sHtmlEncabezado)
            pnlCaracteristicas.Controls.Clear()
            pnlCaracteristicas.Controls.Add(literal)

            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtclienteSeg As New DataTable
            dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
            If dtclienteSeg.Rows.Count > 0 Then
                If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Then
                    ''Revisamos si el atributo dataSheet es dif de null
                    ssql = objDatos.fnObtenerQuery("Datasheet")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", dtGeneral.Rows(0)("ItemCode"))
                        Dim dtData As New DataTable
                        dtData = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtData.Rows.Count > 0 Then
                            If dtData.Rows(0)(0) <> "" Then
                                Session("Datasheet") = dtData.Rows(0)(0)
                                pnlDataSheet.Visible = True

                            End If
                        Else

                        End If

                    End If

                    ''Buscamos en la carpeta
                    If File.Exists(Server.MapPath("~") & "\datasheets\" & itemCode.Replace("-", "") & ".pdf") Then
                        Session("Datasheet") = Server.MapPath("~") & "\datasheets\" & itemCode.Replace("-", "") & ".pdf"
                        pnlDataSheet.Visible = True
                    End If
                End If
            End If

            'ssql = "select * from config.ProductoInterior where cvAtributo ='Detalles' and cvEstatus ='ACTIVO'"
            'dtAtr = New DataTable
            'dtAtr = objDatos.fnEjecutarConsulta(ssql)
            'sHtmlEncabezado = ""
            'If dtAtr.Rows.Count > 0 Then
            '    ''Detalles
            '    ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
            '                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
            '                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Detalles' order by T1.ciOrden "
            '    Dim dtDetallesPlantilla As New DataTable
            '    dtDetallesPlantilla = objDatos.fnEjecutarConsulta(ssql)
            '    If dtDetallesPlantilla.Rows.Count > 0 Then
            '        sHtmlEncabezado = "<div class='col-xs-12 col-sm-6 info-producto-int'>"
            '        sHtmlEncabezado = sHtmlEncabezado & " <div class='Caracteristicas'> "
            '        'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-group filtos-catalogo' id='accordion' role='tablist' aria-multiselectable='true'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "  <div class='filtos-catalogo panel-group tymnyce' id='accordioninfo' role='tablist' aria-multiselectable='true'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "   <div class='panel'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-heading' role='tab' id='headingOne'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "     <h4 class='categoria'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' data-toggle='collapse' data-parent='#accordioninfo' href='#thirOne' aria-expanded='true' aria-controls='thirOne'> " & dtAtr.Rows(0)("cvLeyenda") & " </a>"
            '        sHtmlEncabezado = sHtmlEncabezado & "     </h4> </div>"
            '        sHtmlEncabezado = sHtmlEncabezado & "   <div id='thirOne' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='thirOne'>"
            '        sHtmlBanner = ""
            '        sHtmlBanner = sHtmlBanner & "<div class='panel-body'>"
            '        For i = 0 To dtDetallesPlantilla.Rows.Count - 1 Step 1
            '            sHtmlBanner = sHtmlBanner & "" & dtGeneral.Rows(0)(dtDetallesPlantilla.Rows(i)("Campo")) & "</br>"
            '        Next
            '        sHtmlBanner = sHtmlBanner & "</div>"
            '        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '        sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
            '    End If
            'End If

            'ssql = "select * from config.ProductoInterior where cvAtributo ='Caracteristicas' and cvEstatus ='ACTIVO'"
            'dtAtr = New DataTable
            'dtAtr = objDatos.fnEjecutarConsulta(ssql)

            'If dtAtr.Rows.Count > 0 Then
            '    ''Características
            '    ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
            '                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
            '                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Caracteristicas' order by T1.ciOrden "
            '    Dim dtDetallesCaract As New DataTable
            '    dtDetallesCaract = objDatos.fnEjecutarConsulta(ssql)
            '    If dtDetallesCaract.Rows.Count > 0 Then
            '        'sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-12 col-sm-6 info-producto-int'>"
            '        'sHtmlEncabezado = sHtmlEncabezado & " <div class='caracteristicas'> "
            '        'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-group filtos-catalogo' id='accordion' role='tablist' aria-multiselectable='true'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "   <div class='panel'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-heading' role='tab' id='headingTwo'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "     <h4 class='categoria'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' data-toggle='collapse' data-parent='#accordioninfo' href='#thirTree' aria-expanded='true' aria-controls='thirTree' > " & dtAtr.Rows(0)("cvLeyenda") & "</a>"
            '        sHtmlEncabezado = sHtmlEncabezado & "     </h4> </div>"
            '        sHtmlEncabezado = sHtmlEncabezado & "   <div id='thirTree' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='thirTree'>"
            '        sHtmlBanner = ""
            '        sHtmlBanner = sHtmlBanner & "<div class='panel-body'>"
            '        For i = 0 To dtDetallesCaract.Rows.Count - 1 Step 1
            '            sHtmlBanner = sHtmlBanner & "" & dtGeneral.Rows(0)(dtDetallesCaract.Rows(i)("Campo")) & "</br>"
            '        Next
            '        sHtmlBanner = sHtmlBanner & "</div>"
            '        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '        sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
            '    End If
            'End If


            'ssql = "select * from config.ProductoInterior where cvAtributo ='Información' and cvEstatus ='ACTIVO'"
            'dtAtr = New DataTable
            'dtAtr = objDatos.fnEjecutarConsulta(ssql)
            'If dtAtr.Rows.Count > 0 Then
            '    ''Información
            '    ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
            '            & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
            '            & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Informacion' order by T1.ciOrden "
            '    Dim dtDetallesInfo As New DataTable
            '    dtDetallesInfo = objDatos.fnEjecutarConsulta(ssql)
            '    If dtDetallesInfo.Rows.Count > 0 Then
            '        'sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-12 col-sm-6 info-producto-int'>"
            '        'sHtmlEncabezado = sHtmlEncabezado & " <div class='caracteristicas'> "
            '        'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-group filtos-catalogo' id='accordion' role='tablist' aria-multiselectable='true'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "   <div class='panel'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-heading' role='tab' id='headingThree'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "     <h4 class='categoria'> "
            '        sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' data-toggle='collapse' data-parent='#accordioninfo' href='#thirFour' aria-expanded='true' aria-controls='thirFour' > " & dtAtr.Rows(0)("cvLeyenda") & " </a>"
            '        sHtmlEncabezado = sHtmlEncabezado & "     </h4> </div>"
            '        sHtmlEncabezado = sHtmlEncabezado & "   <div id='thirFour' class='panel-collapse collapse' role='tabpanel' aria-labelledby='thirFour'>"
            '        sHtmlBanner = ""
            '        sHtmlBanner = sHtmlBanner & "<div class='panel-body'>"
            '        For i = 0 To dtDetallesInfo.Rows.Count - 1 Step 1
            '            sHtmlBanner = sHtmlBanner & "" & dtGeneral.Rows(0)(dtDetallesInfo.Rows(i)("Campo")) & "</br>"
            '        Next
            '        sHtmlBanner = sHtmlBanner & "</div>"
            '        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '        sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
            '    End If

            'End If



            'sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
            'literal = New LiteralControl(sHtmlEncabezado)
            'pnlCaracteristicas.Controls.Clear()
            'pnlCaracteristicas.Controls.Add(literal)


            'objDatos.fnLog("fnCargaProducto", "Productos relacionados")
            ''Los productos relacionados
            Dim dtPath As New DataTable
            ssql = "SELECT cvCampoSAP  from config .NivelesArticulos WHERE cvEstatus ='ACTIVO' and ciOrden in(1,2,3) order by ciOrden"
            dtPath = objDatos.fnEjecutarConsulta(ssql)
            Try
                sHtmlEncabezado = ""
                'Primero las categorias
                Dim sCondicion As String = ""

                For i = 0 To dtPath.Rows.Count - 1 Step 1
                    For x = 0 To dtGeneral.Columns.Count - 1 Step 1
                        If dtPath.Rows(i)("cvCampoSAP") = dtGeneral.Columns(x).ColumnName Then



                            If dtGeneral.Rows(0)(x) Is DBNull.Value Then
                                If Session("TipoDBMS") = "HANA" Then
                                    sCondicion = sCondicion & "IFNULL(TO_VARCHAR(T0.""" & dtPath.Rows(i)("cvCampoSAP") & """),'0')= '' AND "
                                Else
                                    sCondicion = sCondicion & "ISNULL(T0." & dtPath.Rows(i)("cvCampoSAP") & ",'0')= '' AND "
                                End If

                            Else
                                If Session("TipoDBMS") = "HANA" Then
                                    sCondicion = sCondicion & "IFNULL(TO_VARCHAR(T0.""" & dtPath.Rows(i)("cvCampoSAP") & """),'0') = '" & dtGeneral.Rows(0)(x) & "' AND "
                                Else
                                    sCondicion = sCondicion & "ISNULL(T0." & dtPath.Rows(i)("cvCampoSAP") & ",'0')= '" & dtGeneral.Rows(0)(x) & "' AND "
                                End If

                            End If

                        End If
                    Next
                Next
                If sCondicion.Length > 3 Then
                    sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)
                End If
                objDatos.fnLog("Antes de info-product grupo", sCondicion.Replace("'", ""))

                ssql = objDatos.fnObtenerQuery("Info-ProductosGrupo")

                If STallaColor = "SI" Then
                    ssql = ssql.Replace("[%0]", "'" & itemCode & "'")

                Else
                    ssql = ssql.Replace("[%0]", sCondicion)

                End If

                ssql = ssql.Replace("[%2]", "'" & itemCode & "'")
                objDatos.fnLog("Productos rel", ssql.Replace("'", ""))

                Dim dtProductos As New DataTable
                dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
                sHtmlBanner = ""
                sHtmlEncabezado = sHtmlEncabezado & "<div class='seccion'>"
                sHtmlEncabezado = sHtmlEncabezado & "<div class='main-container'>"
                sHtmlEncabezado = sHtmlEncabezado & "<span class='linea top'></span>"

                If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA_US") Then
                    sHtmlEncabezado = sHtmlEncabezado & "<div class='sec-tit'>Featuring products</div>"
                Else
                    sHtmlEncabezado = sHtmlEncabezado & "<div class='sec-tit'>Productos Relacionados</div>"
                End If

                ' 
                sHtmlEncabezado = sHtmlEncabezado & " <div class='feature-1'>"
                For i = 0 To dtProductos.Rows.Count - 1 Step 1

                    sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

                    ''''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
                    ''ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    ''    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    ''    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
                    ''Dim dtCamposPlantilla As New DataTable
                    ''dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
                    ''Dim sImagen As String = "ImagenPal"
                    ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
                    ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & dtProductos.Rows(i)("ItemCode") & "'>"
                    ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
                    ''sHtmlBanner = sHtmlBanner & "  <div class='img' >"
                    ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"
                    ''If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
                    ''End If



                    ''ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
                    ''dtPintaCat = New DataTable
                    ''dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
                    ''If dtPintaCat.Rows.Count > 0 Then
                    ''    sPintaPrev = dtPintaCat.Rows(0)("Interior")
                    ''    sPintaFav = dtPintaCat.Rows(0)("Favorito")
                    ''    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
                    ''End If

                    ''If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
                    ''Else
                    ''    sHtmlBanner = sHtmlBanner & "     <div>"
                    ''End If

                    ''If sPintaCompra = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & dtProductos.Rows(i)("ItemCode") & "'></a>"
                    ''End If
                    ''If sPintaFav = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & dtProductos.Rows(i)("ItemCode") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
                    ''End If
                    ''If sPintaCompra = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & dtProductos.Rows(i)("ItemCode") & "&Modo=Add'></a>"
                    ''End If



                    ''sHtmlBanner = sHtmlBanner & "     </div>"
                    ''sHtmlBanner = sHtmlBanner & "  </div>"
                    ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & dtProductos.Rows(i)("ItemCode") & "'>"
                    ''''img/home/producto-1.png
                    ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
                    ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                    ''        Dim sValorMostrar As String
                    ''        If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
                    ''            sValorMostrar = ""
                    ''        Else
                    ''            sValorMostrar = CStr(dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")))
                    ''        End If

                    ''        If sValorMostrar.Length > 30 Then
                    ''            sValorMostrar = sValorMostrar.Substring(0, 30)
                    ''        End If

                    ''        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
                    ''            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & sValorMostrar & "</div>"
                    ''        Else
                    ''            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & sValorMostrar & "</div>"
                    ''        End If
                    ''    Else
                    ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then
                    ''            Dim dPrecioActual As Double
                    ''            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))

                    ''            Try
                    ''                If CInt(Session("slpCode")) <> 0 Then
                    ''                    If Session("ListaPrecios") <> "0" Then
                    ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                    ''                    Else
                    ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                    ''                    End If

                    ''                Else
                    ''                    If Session("UserB2C") = "" And Session("ListaPrecios") <> "0" Then
                    ''                        dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"))
                    ''                    Else
                    ''                        dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"))
                    ''                    End If


                    ''                End If
                    ''                If Session("Cliente") <> "" Then
                    ''                    dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Session("ListaPrecios"))
                    ''                End If
                    ''            Catch ex As Exception
                    ''                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"))
                    ''            End Try


                    ''            ' sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & dPrecioActual.ToString("$ ###,###,###.#0") & "</span>"
                    ''            ' lblPrecio.Text = dPrecioActual.ToString("$ ###,###,###.#0")
                    ''        End If
                    ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
                    ''            sImagen = ""
                    ''            ''Es null, entonces revisamos si por default no existe con el itemocode
                    ''            Dim iband As Int16 = 0
                    ''            objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & ".jpg")
                    ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & ".jpg") Then
                    ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & ".jpg"
                    ''                iband = 1
                    ''            End If
                    ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
                    ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
                    ''                iband = 1
                    ''            End If
                    ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
                    ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
                    ''                iband = 1
                    ''            End If
                    ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
                    ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
                    ''                iband = 1
                    ''            End If



                    ''            If iband = 0 Then
                    ''                If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
                    ''                    sImagen = "images/no-image.png"
                    ''                Else
                    ''                    sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                    ''                End If
                    ''            End If



                    ''            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
                    ''        End If
                    ''    End If
                    ''Next


                    ''sHtmlBanner = sHtmlBanner & "  </a>"
                    ''sHtmlBanner = sHtmlBanner & " </div>"
                    ''sHtmlBanner = sHtmlBanner & "  </a>"
                    ''sHtmlBanner = sHtmlBanner & "</div>"
                Next
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                sHtmlEncabezado = sHtmlEncabezado & "</div></div></div></div>"
                literal = New LiteralControl(sHtmlEncabezado)
                pnlProductos.Controls.Clear()
                pnlProductos.Controls.Add(literal)
            Catch ex As Exception
                objDatos.fnLog("Productos rel", ex.Message.Replace("'", ""))
                '   objDatos.Mensaje(ex.Message, Me.Page)
            End Try
            ''La ruta
            sHtmlEncabezado = "<div class='main-container'>"
            sHtmlEncabezado = sHtmlEncabezado & "<ol class='breadcrumb'>"
            sHtmlBanner = "<li><a href='catalogo.aspx'>Home</a></li>"
            '  objDatos.fnLog("Ruta", "PAra entrar")
            Dim sCampoRuta As String = ""
            Dim sCatLiga As String = ""
            Dim sLigaNiv2 As String = ""
            Dim sLigaNiv3 As String = ""
            For i = 0 To dtPath.Rows.Count - 1 Step 1
                '    objDatos.fnLog("Ruta Antes For", dtPath.Rows(i)("cvCampoSAP"))
                '  objDatos.fnLog("Ruta", dtGeneral.Columns.Count)
                For x = 0 To dtGeneral.Columns.Count - 1 Step 1
                    '    objDatos.fnLog("Ruta", dtGeneral.Columns(x).ColumnName)
                    If dtPath.Rows(i)("cvCampoSAP") = dtGeneral.Columns(x).ColumnName Then
                        objDatos.fnLog("Ruta", dtPath.Rows(i)("cvCampoSAP"))
                        If dtGeneral.Columns(x).ColumnName.Contains("ItmsGrpCod") Then
                            ssql = objDatos.fnObtenerQuery("Grupo")
                            ssql = ssql.Replace("[%0]", "'" & dtGeneral.Rows(0)("ItmsGrpCod") & "'")
                            Dim dtNombreGrupo As New DataTable
                            dtNombreGrupo = objDatos.fnEjecutarConsultaSAP(ssql)

                            If dtNombreGrupo.Rows.Count > 0 Then
                                sCampoRuta = dtNombreGrupo.Rows(0)(0)
                            End If

                        Else
                            Dim sValorPintar As String = ""

                            If CStr(dtPath.Rows(i)("cvCampoSAP")).Contains("U_") Then

                                If i = 0 Then
                                    If dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP")) Is DBNull.Value Then
                                        sCatLiga = ""
                                    Else
                                        ssql = objDatos.fnObtenerQuery("CampoUsuario")
                                        objDatos.fnLog("Ruta i=0", ssql.Replace("'", ""))
                                        sCatLiga = dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP"))
                                    End If
                                Else
                                    If dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP")) Is DBNull.Value Then
                                        sLigaNiv2 = ""
                                        sLigaNiv3 = ""
                                    Else
                                        objDatos.fnLog("Ruta query", "CampoUsuarioNiv" & (i + 1))
                                        ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv" & (i + 1))
                                        ssql = ssql.Replace("[%1]", dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP")))
                                        objDatos.fnLog("Ruta query", ssql.Replace("'", ""))
                                        If i = 1 Then
                                            sLigaNiv2 = "&Param2=" & dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP"))
                                        Else
                                            sLigaNiv3 = "&Param3=" & dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP"))
                                        End If
                                    End If


                                End If
                                If dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP")) Is DBNull.Value Then
                                    '    objDatos.fnLog("Ruta", dtPath.Rows(i)("cvCampoSAP") & " ES NULL")
                                Else
                                    ' sCatLiga = dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP"))
                                    If dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP")) Is DBNull.Value Then
                                        sValorPintar = ""
                                    Else
                                        ssql = ssql.Replace("[%0]", dtGeneral.Rows(0)(dtPath.Rows(i)("cvCampoSAP")))
                                        objDatos.fnLog("Ruta @", ssql.Replace("'", ""))
                                        Dim dtValor As New DataTable
                                        dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtValor.Rows.Count > 0 Then
                                            sValorPintar = dtValor.Rows(dtValor.Rows.Count - 1)(0)
                                        End If
                                    End If

                                    sCampoRuta = sValorPintar
                                End If

                            Else
                                objDatos.fnLog("Ruta", "No tiene U_")
                                If dtGeneral.Rows(0)(x) Is DBNull.Value Then
                                    objDatos.fnLog("Ruta", "DbNull")
                                    sCampoRuta = ""
                                Else
                                    sCampoRuta = dtGeneral.Rows(0)(x)
                                    objDatos.fnLog("Ruta", "CampoRuta:" & sCampoRuta)
                                    If i = 0 Then
                                        sCatLiga = sCampoRuta
                                    End If
                                    If i = 1 Then
                                        sLigaNiv2 = "&Param2=" & dtGeneral.Rows(0)(x)

                                    End If
                                    If i = 2 Then
                                        sLigaNiv3 = "&Param3=" & dtGeneral.Rows(0)(x)

                                    End If


                                End If
                            End If




                        End If


                        If i = dtPath.Rows.Count - 1 Then
                            sHtmlBanner = sHtmlBanner & "<li class='active'>" & sCampoRuta & "</li>"
                        Else
                            sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?Cat=" & sCatLiga & sLigaNiv2 & sLigaNiv3 & "'>" & sCampoRuta & "</a></li>"
                        End If

                    End If
                Next
            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & "</ol></div>"
            literal = New LiteralControl(sHtmlEncabezado)
            pnlRuta.Controls.Clear()
            pnlRuta.Controls.Add(literal)


            ''Revisamos la parametrización de las existencias
            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then
                    ''Evaluamos el stock
                    Dim existencia As Double = 0
                    If STallaColor = "SI" Then
                        fnRevisaExistencias(Session("ItemCodeTallaColor"))
                    Else
                        existencia = fnRevisaExistencias(Request.QueryString("Code"))
                    End If

                    If existencia = 0 Then
                        'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")

                        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                            pnlAgregar.Visible = True
                        Else
                            pnlAgregar.Visible = False
                            pnlAgregarConDesc.Visible = False
                            pnlAgregarConDesc.Visible = False
                        End If
                    Else
                        If CInt(Session("slpCode")) <> 0 Then
                            If pnlDescuento.Visible = True Then
                                pnlAgregarConDesc.Visible = True
                            Else
                                pnlAgregar.Visible = True
                            End If

                        End If
                    End If
                End If
            End If

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                pnlAgregar.Visible = True

            End If
        Catch ex As Exception
            objDatos.fnLog("Err carga producto", ex.Message)
        End Try
    End Sub

    Public Sub fnOcultarLabel(Label As String)
        For Each control As System.Web.UI.Control In pnlTallaColor.Controls
            If control.ClientID.Contains(Label) Then
                control.Visible = False
            End If
        Next
    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        Dim partida As New Cls_Pedido.Partidas
        partida.ItemCode = Request.QueryString("code")
        '  partida.Cantidad = txtCantidad.Text
        Dim dPrecioActual As Double = 0

        If CInt(Session("slpCode")) <> 0 Then
            dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), Session("ListaPrecios"))
        Else
            dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"))
        End If

        ''Revisamos si hay que mostrar tallas y colores
        Dim sTallaColor As String = ""
        ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
        Dim dtTallasColores As New DataTable
        dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
        If dtTallasColores.Rows.Count > 0 Then
            If dtTallasColores.Rows(0)(0) = "SI" Then
                sTallaColor = "SI"
                ''Cambiamos
                partida.Precio = Session("PrecioTallaColor")
                partida.ItemCode = Session("ItemCodeTallaColor")
            Else
                partida.Precio = dPrecioActual
            End If
        End If

        partida.Moneda = Session("Moneda")

        partida.TotalLinea = partida.Cantidad * partida.Precio

        ''Ahora el itemName
        Try
            ssql = objDatos.fnObtenerQuery("Nombre-Producto")

            fnObtenerMoneda(Session("ItemCodeTallaColor"))

            If sTallaColor = "SI" Then
                ssql = ssql.Replace("[%0]", "'" & Session("ItemCodeTallaColor") & "'")
            Else
                ssql = ssql.Replace("[%0]", "'" & Request.QueryString("code") & "'")
            End If

            '  objDatos.fnLog("ItemName Click", ssql.Replace("'", ""))
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            partida.ItemName = dtItemName.Rows(0)(0)
        Catch ex As Exception

        End Try


        Session("Partidas").add(partida)
        Dim aMP As Main = CType(Me.Master, Main)
        aMP.fnCargaCarrito()



        '  objDatos.Mensaje("Agregado al carrito", Me.Page)
    End Sub
    Public Function fnObtenerMoneda(ItemCode As String) As String
        Dim ssql As String = ""
        ''Posibles monedas en la lista de precios
        ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
        ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
        dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
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
    Protected Sub ddl_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim sValores As String = ""
            Dim IdFiltro As Int16 = 1
            Dim sCondicion As String = ""
            If DirectCast(sender, System.Web.UI.WebControls.DropDownList).ID.Contains("Atr1") Then
                IdFiltro = 1
            End If
            If DirectCast(sender, System.Web.UI.WebControls.DropDownList).ID.Contains("Atr2") Then
                IdFiltro = 2
            End If
            If DirectCast(sender, System.Web.UI.WebControls.DropDownList).ID.Contains("Atr3") Then
                IdFiltro = 3
            End If
            If DirectCast(sender, System.Web.UI.WebControls.DropDownList).ID.Contains("Atr4") Then
                IdFiltro = 4
            End If

            objDatos.fnLog("ddl_sel", "Entra")
            Dim IdAtributo As Int16 = 0

            Dim sCondicionSeleccionado As String = ""

            For Each control As System.Web.UI.Control In pnlTallaColor.Controls

                objDatos.fnLog("ddl_sel", control.ClientID)
                If control.ClientID.Contains("ddl") Then
                    If control.Visible = True Then
                        IdAtributo = IdAtributo + 1
                        If DirectCast(control, System.Web.UI.WebControls.DropDownList).ID = DirectCast(sender, System.Web.UI.WebControls.DropDownList).ID Then
                            ssql = "select cvCampoFiltra from config.TallaColor WHERe ciIdRel=" & "'" & DirectCast(sender, System.Web.UI.WebControls.DropDownList).ToolTip & "'"
                            Dim dtCampoFiltro As New DataTable
                            dtCampoFiltro = objDatos.fnEjecutarConsulta(ssql)
                            objDatos.fnLog("ddl_CondicionSeleccionado", ssql.Replace("'", ""))
                            If dtCampoFiltro.Rows.Count > 0 Then
                                Try

                                    sCondicionSeleccionado = sCondicionSeleccionado & " AND " & dtCampoFiltro.Rows(0)(0) & "=" & "'" & DirectCast(sender, System.Web.UI.WebControls.DropDownList).SelectedValue & "'"
                                    objDatos.fnLog("ddl_Condicion", sCondicionSeleccionado.Replace("'", ""))
                                Catch ex As Exception
                                    objDatos.fnLog("ddl_Condicion", ex.Message)
                                End Try
                            End If
                        End If


                    End If

                End If

            Next










            IdAtributo = 0
            For Each control As System.Web.UI.Control In pnlTallaColor.Controls

                If control.ClientID.Contains("ddl") Then
                    objDatos.fnLog("Filtro talla color for 1 ", control.ClientID)
                    If control.Visible = True Then
                        IdAtributo = IdAtributo + 1

                        ssql = "select cvCampoFiltra from config.TallaColor WHERe ciIdRel=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip & "'"
                        Dim dtCampoFiltro As New DataTable
                        dtCampoFiltro = objDatos.fnEjecutarConsulta(ssql)
                        objDatos.fnLog("ddl_sel", ssql.Replace("'", ""))
                        If dtCampoFiltro.Rows.Count > 0 Then
                            sCondicion = sCondicion & " AND " & dtCampoFiltro.Rows(0)(0) & "=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).SelectedValue & "'"
                            Session("CondicionCombo") = " AND " & dtCampoFiltro.Rows(0)(0) & "=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).SelectedValue & "'"
                        End If

                    End If

                End If

            Next
            objDatos.fnLog("ddl_sel", "Sale For 1")





            Try
                ''nos traemos los atributos de la tabla de tallaColor

                For Each control As System.Web.UI.Control In pnlTallaColor.Controls
                    Dim sTextLabel As String = ""
                    Dim iOcultar As Int16 = 0

                    ' objDatos.fnLog("Filtro talla color Ctrl", control.ID)
                    If control.ClientID.Contains("ddl") Then
                        objDatos.fnLog("Filtro talla color", control.ClientID)
                        If DirectCast(control, System.Web.UI.WebControls.DropDownList).ID <> DirectCast(sender, System.Web.UI.WebControls.DropDownList).ID And DirectCast(control, System.Web.UI.WebControls.DropDownList).Visible = True Then


                            objDatos.fnLog("Filtro talla color", "Si Entra")
                            DirectCast(control, System.Web.UI.WebControls.DropDownList).Visible = True
                            ''Llenamos el combo
                            ssql = "select cvCampoFiltra,ciIdRel,cvTallaColor,cvQuery from config.TallaColor WHERe ciIdRel=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip & "'"
                            Dim dtCampoFiltro As New DataTable
                            dtCampoFiltro = objDatos.fnEjecutarConsulta(ssql)

                            ssql = objDatos.fnObtenerQuery(dtCampoFiltro.Rows(0)("cvQuery"))
                            ssql = ssql.Replace("[%0]", Request.QueryString("Code"))
                            ssql = ssql & " " & sCondicionSeleccionado

                            objDatos.fnLog("Filtro ddl " & DirectCast(control, System.Web.UI.WebControls.DropDownList).ClientID, ssql.Replace("'", ""))
                            Dim dtDatosCombo As New DataTable
                            dtDatosCombo = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtDatosCombo.Rows.Count > 1 Then
                                'Dim fila As DataRow
                                'fila = dtDatosCombo.NewRow
                                'fila("valor") = "-Seleccione-"
                                'fila("Descripcion") = "-Seleccione-"
                                'dtDatosCombo.Rows.Add(fila)
                            End If
                            DirectCast(control, System.Web.UI.WebControls.DropDownList).DataSource = dtDatosCombo
                            DirectCast(control, System.Web.UI.WebControls.DropDownList).DataTextField = "descripcion"
                            DirectCast(control, System.Web.UI.WebControls.DropDownList).DataValueField = "valor"
                            DirectCast(control, System.Web.UI.WebControls.DropDownList).DataBind()
                            If dtDatosCombo.Rows.Count > 0 Then
                                If dtDatosCombo.Rows(0)("descripcion") = "Ninguno" Then
                                    'DirectCast(control, System.Web.UI.WebControls.DropDownList).Visible = False
                                    'fnOcultarLabel("lblAtr" & DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip)

                                End If
                            End If
                            sTextLabel = dtCampoFiltro.Rows(0)("cvTallaColor")
                        End If
                    End If
                    If control.ClientID.Contains("lbl") Then
                        Try
                            If control.ClientID.Contains("lbl" & DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip) Then
                                DirectCast(control, System.Web.UI.WebControls.Label).Text = sTextLabel

                            End If
                        Catch ex As Exception

                        End Try

                    End If

                Next

            Catch ex As Exception
                objDatos.fnLog("Filtro talla color ex ", ex.Message.Replace("'", ""))
            End Try





            ''Obtenemos el query para calcular precio
            ssql = objDatos.fnObtenerQuery("PrecioTallaColor")
            ssql = ssql.Replace("[%0]", Request.QueryString("Code"))
            ssql = ssql.Replace("[%1]", sCondicion)
            ssql = ssql.Replace("[%2]", "'" & Session("ListaPrecios") & "'")

            objDatos.fnLog("Calcular Precios SIC", ssql.Replace("'", ""))
            Dim dtPrecio As New DataTable
            dtPrecio = objDatos.fnEjecutarConsultaSAP(ssql)

            Dim dPrecio As Double = 0
            If dtPrecio.Rows.Count > 0 Then
                objDatos.fnLog("Precio", dtPrecio.Rows(0)("Precio"))
                ''Jalamos el precio y llenamos en la variable de session el articulo real de SAP
                dPrecio = CDbl(dtPrecio.Rows(0)("Precio"))
                lblPrecio.Text = CDbl(dtPrecio.Rows(0)("Precio")).ToString("###,###,###.#0")
                Session("ItemCodeTallaColor") = dtPrecio.Rows(0)("ItemCode")
                Session("PrecioCodeTallaColor") = dtPrecio.Rows(0)("Precio")
            End If

            ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
            Dim dtMonedas As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Session("ItemCodeTallaColor") & "'")
            ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
            dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
            '   objDatos.fnLog("Moneda", ssql.Replace("'", ""))
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
            objDatos.fnLog("ddl_sel", sMoneda & " " & lblPrecio.Text)
            lblPrecio.Text = sCaracterMoneda & " " & dPrecio.ToString("###,###,###.#0") & " " & sMoneda





            ''Revisamos la parametrización de las existencias
            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then
                    ''Evaluamos el stock
                    Dim existencia = fnRevisaExistencias(Session("ItemCodeTallaColor"))
                    If existencia = 0 Then
                        'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        pnlAgregar.Visible = False
                        pnlAgregarConDesc.Visible = False
                        pnlAgregarConDesc.Visible = False

                    Else
                        pnlAgregar.Visible = True
                    End If
                End If
            End If


            ''Existencia 
            ssql = "select ISNULL(cvMuestraExistencias,'NO') FROM config.parametrizaciones"
            Dim dtMuestra As New DataTable
            dtMuestra = objDatos.fnEjecutarConsulta(ssql)
            If dtMuestra.Rows.Count > 0 Then
                If dtMuestra.Rows(0)(0) = "SI" Then
                    If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAPB2B")
                    Else
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                    End If

                    Dim dtExistencia As New DataTable
                    ssql = ssql.Replace("[%0]", "'" & Session("ItemCodeTallaColor") & "'")
                    objDatos.fnLog("Existencia ddl", ssql.Replace("'", ""))
                    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        lblExistencia.Text = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"
                    End If

                Else
                    pnlExistencia.Visible = False
                End If
            End If



            ''Revisamos la imagen
            ''Obtenemos el nombre de la empresa
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)

            If dtcliente.Rows.Count > 0 Then
                If dtcliente.Rows(0)(0) = "Lazarus" Then
                    ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Session("ItemCodeTallaColor") & "'"
                    objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                    Dim dtFoto As New DataTable
                    dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtFoto.Rows.Count > 0 Then
                        Session("Imagen") = dtFoto.Rows(0)(0)
                        objDatos.fnLog("ddl_sel_foto", Session("Imagen"))

                        Dim itemcode As String = Request.QueryString("Code")
                        ssql = objDatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & itemcode & "'")


                        objDatos.fnLog("Info-Producto", ssql.Replace("'", ""))


                        Dim dtGeneral As New DataTable
                        dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtGeneral.Rows.Count = 0 Then
                            Response.Redirect("index.aspx")
                            Exit Sub
                        End If

                        Dim sHtmlEncabezado As String = ""
                        Dim shtmlbanner As String = ""
                        ''Cargamos la Imagenes
                        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Imagenes' order by T1.ciOrden "
                        Dim dtImagenesPlantilla As New DataTable
                        dtImagenesPlantilla = objDatos.fnEjecutarConsulta(ssql)
                        If dtImagenesPlantilla.Rows.Count > 0 Then
                            ''Metemos validacion para determinar, si es una sola imagen, no hacemos el TumbNail
                            Dim iCuantas As Int16 = 0
                            For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                                If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                Else
                                    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                                    Else
                                        iCuantas = iCuantas + 1
                                    End If
                                End If
                            Next

                            ''TumbNail
                            sHtmlEncabezado = "<div class='col-xs-12 col-sm-6'>"

                            If iCuantas > 1 Then
                                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-3'>"
                                sHtmlEncabezado = sHtmlEncabezado & " <div class='product-nav'>"
                                For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                                    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                    Else
                                        If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                                        Else

                                            shtmlbanner = shtmlbanner & " <div class='thumbnail'><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"


                                        End If
                                    End If

                                Next
                                sHtmlEncabezado = sHtmlEncabezado & shtmlbanner
                                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                            End If

                            ''Modo Normal
                            shtmlbanner = ""
                            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-9'>"
                            sHtmlEncabezado = sHtmlEncabezado & " <div class='product-for'>"
                            For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                                If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                    shtmlbanner = shtmlbanner & " <div><img src=" & "'images/no-image.png' class='img-responsive' alt='descrip imagen'></div>"
                                Else
                                    If Session("Imagen") = "" Then
                                        shtmlbanner = shtmlbanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                                    Else
                                        shtmlbanner = shtmlbanner & " <div><img src=" & "'" & Session("Imagen") & "' class='img-responsive' alt='descrip imagen'></div>"
                                    End If

                                End If

                                ' sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                            Next
                            sHtmlEncabezado = sHtmlEncabezado & shtmlbanner
                            sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                            sHtmlEncabezado = sHtmlEncabezado & "</div>"
                        End If

                        Dim literalImagen = New LiteralControl(sHtmlEncabezado)
                        pnlImagenes.Controls.Clear()
                        pnlImagenes.Controls.Add(literalImagen)
                        sHtmlEncabezado = ""
                        shtmlbanner = ""

                    End If





                End If
            End If


        Catch ex As Exception

        End Try
    End Sub


    Public Sub fnCargaImagenesDetalle(itemCode As String)
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim iband As Int16 = 0
        ''Cargamos la Imagenes
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Imagenes' order by T1.ciOrden "
        Dim dtImagenesPlantilla As New DataTable
        dtImagenesPlantilla = objDatos.fnEjecutarConsulta(ssql)
        If dtImagenesPlantilla.Rows.Count > 0 Then
            ''Metemos validacion para determinar, si es una sola imagen, no hacemos el TumbNail
            Dim iCuantas As Int16 = 0
            For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                'If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                'Else
                '    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                '    Else
                '        iCuantas = iCuantas + 1
                '    End If
                'End If
            Next

            ''TumbNail
            sHtmlEncabezado = "<div class='col-xs-12 col-sm-6'>"
            Dim sItemCode As String()
            sItemCode = itemCode.Split("-")
            If sItemCode.Count = 4 Then
                itemCode = sItemCode(0) & "-" & sItemCode(1) & sItemCode(3)
            End If
            Dim sItemCodeFoto As String = itemCode
            If Session("VienePLUS") = "SI" Then
                sItemCodeFoto = itemCode & "PLUS"
            End If
            If iCuantas > -1 Or 1 = 1 Then
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-3'>"
                sHtmlEncabezado = sHtmlEncabezado & " <div class='product-nav'>"


                objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & sItemCodeFoto & ".jpg")
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto.Replace("-", "") & ".jpg") Then
                    '                        sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & "images/products/" & itemCode & ".jpg" & "' class='img-responsive'  alt='descrip imagen'></div>"
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src='" & "images/products/" & sItemCodeFoto.Replace("-", "") & ".jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & ".jpg") Then
                    '                        sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & "images/products/" & itemCode & ".jpg" & "' class='img-responsive'  alt='descrip imagen'></div>"
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src='" & "images/products/" & sItemCodeFoto & ".jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-1.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & sItemCodeFoto & "-1.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-2.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & sItemCodeFoto & "-2.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-3.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & "images/products/" & sItemCodeFoto & "-3.jpg" & "' class='img-responsive' alt='descrip imagen'></div>"

                    iband = 1
                End If

                If iband = 1 Then

                End If


                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

            End If

            ''Modo Normal
            sHtmlBanner = ""
            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-9'>"
            sHtmlEncabezado = sHtmlEncabezado & " <div class='product-for'>"
            For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1

                ''Es null, entonces revisamos si por default no existe con el itemocode
                iband = 0
                objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & sItemCodeFoto & ".jpg")
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & ".jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & sItemCodeFoto & ".jpg" & "'  class='zoom_img img-responsive'  data-zoom-image=" & "'" & "images/products/" & itemCode & ".jpg" & "' alt='descrip imagen'></span></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto.Replace("-", "") & ".jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & sItemCodeFoto.Replace("-", "") & ".jpg" & "' data-zoom-image=" & "'" & "images/products/" & itemCode.Replace("-", "") & ".jpg" & "' alt='descrip imagen'></span></div>"
                    iband = 1
                End If

                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-1.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & sItemCodeFoto & "-1.jpg" & "' data-zoom-image=" & "'" & "images/products/" & itemCode & "-1.jpg" & "' alt='descrip imagen'></span></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-2.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & sItemCodeFoto & "-2.jpg" & "' data-zoom-image=" & "'" & "images/products/" & itemCode & "-2.jpg" & "' alt='descrip imagen'></span></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-3.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div><span class='zoom' ><img src=" & "'" & "images/products/" & sItemCodeFoto & "-3.jpg" & "' data-zoom-image=" & "'" & "images/products/" & itemCode & "-3.jpg" & "' alt='descrip imagen'></span></div>"
                    iband = 1
                End If



                If iband = 0 Then

                End If



                ' sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

            '  sHtmlEncabezado = sHtmlEncabezado & "</div>"
        End If
        If iband = 1 Then
            Dim literalImagen = New LiteralControl(sHtmlEncabezado)
            pnlImagenes.Controls.Clear()
            pnlImagenes.Controls.Add(literalImagen)
            sHtmlEncabezado = ""
            sHtmlBanner = ""
        End If

    End Sub

    Public Sub fnCargaFichasColores(itemcode As String)
        Exit Sub
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim iband As Int16 = 0
        Dim ItemCodeFoto As String = ""
        ' Exit Sub
        Dim dtColores As New DataTable

        ssql = objDatos.fnObtenerQuery("Color")
        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", itemcode)
            dtColores = objDatos.fnEjecutarConsultaSAP(ssql)
        End If
        ' Dim sStyleItem As String = "style='position: relative;  width: 50px;  height: 50px; overflow:hidden;'"
        Dim sStyleItem As String = " class='item-color'"
        sHtmlBanner = sHtmlBanner & "<div class='select-colores'>"
        If dtColores.Rows.Count > 0 Then

            For i = 0 To dtColores.Rows.Count - 1 Step 1
                Dim sItemCode As String()
                ssql = objDatos.fnObtenerQuery("ItemColorHijoFoto")
                Dim dtItemColorHijo As New DataTable

                If ssql <> "" Then

                    ssql = ssql.Replace("[%0]", itemcode).Replace("[%1]", dtColores.Rows(i)(0))
                    objDatos.fnLog("Fichas Colores", ssql.Replace("'", ""))
                    dtItemColorHijo = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtItemColorHijo.Rows.Count > 0 Then
                        Dim ItemCodeHijo As String = ""
                        ItemCodeHijo = dtItemColorHijo.Rows(0)(0)
                        sItemCode = ItemCodeHijo.Split("-")
                        If sItemCode.Count = 4 Then
                            ItemCodeFoto = sItemCode(0) & "-" & sItemCode(1) & sItemCode(3)

                            objDatos.fnLog("Ficha colores existe", Server.MapPath("~") & "\images\products\" & ItemCodeFoto & "-4.jpg")
                            If File.Exists(Server.MapPath("~") & "\images\products\" & ItemCodeFoto & "-4.jpg") Then
                                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                sHtmlBanner = sHtmlBanner & "</div>"

                            Else
                                objDatos.fnLog("Ficha colores existe", Server.MapPath("~") & "\images\products\" & ItemCodeFoto & "-2.jpg")
                                If File.Exists(Server.MapPath("~") & "\images\products\" & ItemCodeFoto & "-2.jpg") Then
                                    sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                    sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-2.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                    sHtmlBanner = sHtmlBanner & "</div>"
                                Else

                                    objDatos.fnLog("Ficha colores existe sin 2", Server.MapPath("~") & "\images\products\" & ItemCodeFoto & ".jpg")
                                    If File.Exists(Server.MapPath("~") & "\images\products\" & ItemCodeFoto & ".jpg") Then
                                        sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                        sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & ".jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                        sHtmlBanner = sHtmlBanner & "</div>"
                                    Else
                                        objDatos.fnLog("Ficha colores existe sin 2", Server.MapPath("~") & "\images\products\" & ItemCodeFoto & "-1.jpg")
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & ItemCodeFoto & "-1.jpg") Then
                                            sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                            sHtmlBanner = sHtmlBanner & "<img id='clip-" & (i + 1) & "' src='" & "images/products/" & ItemCodeFoto & "-1.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                            sHtmlBanner = sHtmlBanner & "</div>"
                                        Else
                                            If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & "-4.jpg") Then
                                                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                                sHtmlBanner = sHtmlBanner & "<img id='clip-1" & "' src='" & "images/products/" & itemcode & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                                sHtmlBanner = sHtmlBanner & "</div>"
                                            End If
                                        End If
                                    End If
                                End If
                            End If



                        Else
                            If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & "-4.jpg") Then
                                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                                sHtmlBanner = sHtmlBanner & "<img id='clip-1" & "' src='" & "images/products/" & itemcode & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                                sHtmlBanner = sHtmlBanner & "</div>"
                            End If
                        End If
                    End If
                End If




            Next

        Else
            If File.Exists(Server.MapPath("~") & "\images\products\" & itemcode & "-4.jpg") Then
                sHtmlBanner = sHtmlBanner & "<div " & sStyleItem & ">"
                sHtmlBanner = sHtmlBanner & "<img id='clip-" & "' src='" & "images/products/" & itemcode & "-4.jpg" & "' style='position: absolute; top: -9999px;  left: -9999px;  right: -9999px;  bottom: -9999px;  margin: auto;max-width:1000%;'> "
                sHtmlBanner = sHtmlBanner & "</div>"
            End If

        End If
        sHtmlBanner = sHtmlBanner & "</div>"
        pnlFichasColor.Visible = True
        Dim literalImagen = New LiteralControl(sHtmlBanner)
        pnlFichasColor.Controls.Clear()
        pnlFichasColor.Controls.Add(literalImagen)
        sHtmlEncabezado = ""
        sHtmlBanner = ""
    End Sub



    Public Sub CalculaPrecio()
        Try
            Dim sValores As String = ""
            Dim IdFiltro As Int16 = 1
            Dim sCondicion As String = ""

            Session("CondicionCombo") = ""

            Dim IdAtributo As Int16 = 0
            For Each control As System.Web.UI.Control In pnlTallaColor.Controls

                If control.ClientID.Contains("ddl") Then
                    If control.Visible = True Then
                        IdAtributo = IdAtributo + 1
                        '    objDatos.fnLog("CalculaPrecio Atr:", DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip)
                        ssql = "select cvCampoFiltra from config.TallaColor WHERe ciIdRel=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip & "'"
                        Dim dtCampoFiltro As New DataTable
                        dtCampoFiltro = objDatos.fnEjecutarConsulta(ssql)
                        If dtCampoFiltro.Rows.Count > 0 Then
                            sCondicion = sCondicion & " AND " & dtCampoFiltro.Rows(0)(0) & "=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).SelectedValue & "'"
                        End If

                    End If

                End If

            Next
            '    Session("CondicionCombo") = sCondicion
            ''Obtenemos el query para calcular precio
            ssql = objDatos.fnObtenerQuery("PrecioTallaColor")
            ssql = ssql.Replace("[%0]", Request.QueryString("Code"))
            ssql = ssql.Replace("[%1]", sCondicion)
            ssql = ssql.Replace("[%2]", "'" & Session("ListaPrecios") & "'")

            Dim dtPrecio As New DataTable
            dtPrecio = objDatos.fnEjecutarConsultaSAP(ssql)

            objDatos.fnLog("Calcular Precios", ssql.Replace("'", ""))

            Dim Precio As Double
            If dtPrecio.Rows.Count > 0 Then
                Precio = dtPrecio.Rows(0)("Precio")

                ''Revisamos si se mostrarán precios más IVA
                ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
                Dim dtTipoPrecio As New DataTable
                dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
                If dtTipoPrecio.Rows.Count > 0 Then
                    If dtTipoPrecio.Rows(0)(0) = "SI" Then
                        ''Obtenemos el porcentaje de IVA
                        ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                        Dim dtPorcIVA As New DataTable
                        dtPorcIVA = objDatos.fnEjecutarConsulta(ssql)
                        If dtPorcIVA.Rows.Count > 0 Then
                            Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                        End If
                    End If
                End If


                objDatos.fnLog("Calcular Precios", "Entra")
                ''Jalamos el precio y llenamos en la variable de session el articulo real de SAP
                lblPrecio.Text = Precio.ToString("###,###,###.#0")
                objDatos.fnLog("Calcular Precios Precio", CDbl(dtPrecio.Rows(0)("Precio")).ToString("###,###,###.#0"))

                If Precio = 0 Then
                    Precio = objDatos.fnPrecioActual(Request.QueryString("Code"), Convert.ToInt16(Session("ListaPrecios")))
                    'Session("ItemCodeGenerico") = Request.QueryString("Code")
                    'Session("ItemCodeTallaColor") = Request.QueryString("Code")
                    Session("PrecioCodeTallaColor") = Precio
                End If

                Session("ItemCodeGenerico") = Request.QueryString("Code")
                Session("ItemCodeTallaColor") = dtPrecio.Rows(0)("ItemCode")
                Session("PrecioCodeTallaColor") = Precio
            Else
                ''Estamos en un talla color, pero el codigo es unico, no tiene tallas y colores
                Precio = objDatos.fnPrecioActual(Request.QueryString("Code"), Convert.ToInt16(Session("ListaPrecios")))
                Session("ItemCodeGenerico") = Request.QueryString("Code")
                Session("ItemCodeTallaColor") = Request.QueryString("Code")
                Session("PrecioCodeTallaColor") = Precio
            End If





            ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
            Dim dtMonedas As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Session("ItemCodeTallaColor") & "'")
            ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
            dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
            objDatos.fnLog("Moneda", ssql.Replace("'", ""))
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

            Dim sLeyendaPrecio As String = ""
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                sLeyendaPrecio = " "
            End If
            lblPrecio.Text = sCaracterMoneda & " " & Precio.ToString("###,###,###.#0") & " " & sMoneda & sLeyendaPrecio


            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(Session("ItemCodeGenerico"))

            If fDescuento > 0 Then

                lblPrecio.Style.Add("text-decoration", "line-through")
                lblPreciodesc.Visible = True
                lblPreciodesc.Text = sCaracterMoneda & " " & (Precio * (1 - (fDescuento / 100))).ToString("###,###,###.#0") & " " & sMoneda & sLeyendaPrecio
            End If


            ''Evaluamos si para ese producto en esa talla y color hay imagenes en especial
            fnCargaImagenesDetalle(Session("ItemCodeTallaColor"))


            ''Revisamos la imagen
            ''Obtenemos el nombre de la empresa
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)

            If dtcliente.Rows.Count > 0 Then
                If dtcliente.Rows(0)(0) = "Lazarus" Then
                    ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Session("ItemCodeTallaColor") & "'"
                    objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                    Dim dtFoto As New DataTable
                    dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtFoto.Rows.Count > 0 Then
                        Session("Imagen") = dtFoto.Rows(0)(0)
                        objDatos.fnLog("ddl_sel_foto", Session("Imagen"))

                        Dim itemcode As String = Request.QueryString("Code")
                        ssql = objDatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & itemcode & "'")


                        objDatos.fnLog("Info-Producto", ssql.Replace("'", ""))


                        Dim dtGeneral As New DataTable
                        dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtGeneral.Rows.Count = 0 Then
                            Response.Redirect("index.aspx")
                            Exit Sub
                        End If

                        Dim sHtmlEncabezado As String = ""
                        Dim shtmlbanner As String = ""
                        ''Cargamos la Imagenes
                        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Imagenes' order by T1.ciOrden "
                        Dim dtImagenesPlantilla As New DataTable
                        dtImagenesPlantilla = objDatos.fnEjecutarConsulta(ssql)
                        If dtImagenesPlantilla.Rows.Count > 0 Then
                            ''Metemos validacion para determinar, si es una sola imagen, no hacemos el TumbNail
                            Dim iCuantas As Int16 = 0
                            For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                                If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                Else
                                    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                                    Else
                                        iCuantas = iCuantas + 1
                                    End If
                                End If
                            Next

                            ''TumbNail
                            sHtmlEncabezado = "<div class='col-xs-12 col-sm-6'>"

                            If iCuantas > 1 Then
                                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-3'>"
                                sHtmlEncabezado = sHtmlEncabezado & " <div class='product-nav'>"
                                For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                                    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                    Else
                                        If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) = "" Then
                                        Else

                                            shtmlbanner = shtmlbanner & " <div class='thumbnail'><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"


                                        End If
                                    End If

                                Next
                                sHtmlEncabezado = sHtmlEncabezado & shtmlbanner
                                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                            End If

                            ''Modo Normal
                            shtmlbanner = ""
                            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-9'>"
                            sHtmlEncabezado = sHtmlEncabezado & " <div class='product-for'>"
                            For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                                If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                    shtmlbanner = shtmlbanner & " <div><img src=" & "'images/no-image.png' class='img-responsive' alt='descrip imagen'></div>"
                                Else
                                    If Session("Imagen") = "" Then
                                        shtmlbanner = shtmlbanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                                    Else
                                        shtmlbanner = shtmlbanner & " <div><img src=" & "'" & Session("Imagen") & "' class='img-responsive' alt='descrip imagen'></div>"
                                    End If

                                End If

                                ' sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                            Next
                            sHtmlEncabezado = sHtmlEncabezado & shtmlbanner
                            sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                            sHtmlEncabezado = sHtmlEncabezado & "</div>"
                        End If

                        Dim literalImagen = New LiteralControl(sHtmlEncabezado)
                        pnlImagenes.Controls.Clear()
                        pnlImagenes.Controls.Add(literalImagen)
                        sHtmlEncabezado = ""
                        shtmlbanner = ""

                    End If





                End If
            End If

            ''Revisamos si debe agregar stock o no
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("ExistenciaSAPB2B")
            Else
                ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
            End If

            Dim dtExistencia As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Session("ItemCodeTallaColor") & "'")
            dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtExistencia.Rows.Count > 0 Then
                lblExistencia.Text = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"
                If CDbl(dtExistencia.Rows(0)(0)) = 0 Then
                    pnlAgregar.Visible = False
                Else
                    pnlAgregar.Visible = True
                End If
            End If


        Catch ex As Exception
            objDatos.fnLog("Calcula Precio Err", ex.Message)
        End Try
    End Sub
    Protected Sub txtMts_TextChanged(sender As Object, e As EventArgs) Handles txtMts.TextChanged
        txtCantidad.Text = txtMts.Text / Session("Mts2")
    End Sub

    Private Sub btnDatasheet_Click(sender As Object, e As ImageClickEventArgs) Handles btnDatasheet.Click
        '  Response.Redirect(Session("Datasheet"))
        System.Web.UI.ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "openModal", "window.open('" & Session("Datasheet") & "' ,'_blank');", True)
    End Sub
End Class
