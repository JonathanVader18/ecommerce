Imports System.Data
Imports System.IO
Imports System.Web.Services

Partial Class preview_popup
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String

    Private Sub preview_popup_Load(sender As Object, e As EventArgs) Handles Me.Load

        ssql = "select ISNULL(cvAplicaDescto,'NO') as AplicaDesc from config.Parametrizaciones "
        Dim dtParam As New DataTable
        dtParam = objDatos.fnEjecutarConsulta(ssql)
        If dtParam.Rows.Count > 0 Then
            If dtParam.Rows(0)("AplicaDesc") = "SI" Then
                If Session("Cliente") <> "" And Session("UserB2C") = "" Then
                    If CInt(Session("slpCode")) <> 0 Then
                        ''El descuento solo pudiera aplicar en el módulo de Vendedores
                        pnlDescuento.Visible = True
                    End If
                End If

            Else
                If Session("Cliente") <> "" And Session("UserB2C") = "" Then
                    If CInt(Session("slpCode")) <> 0 Then
                        ''El descuento solo pudiera aplicar en el módulo de Vendedores
                        pnlEditarsinDesc.Visible = True
                    End If
                End If

            End If
        End If
        If Session("Cliente") <> "" Then
            pnlEditarsinDesc.Visible = True
            pnlEditar.Visible = False
            pnlAgregar.Visible = False
            pnlAgregarSinDesc.Visible = False
        End If
        Session("ProductoVista") = Request.QueryString("code")
        fnCargaProductov2(Request.QueryString("code"))
        Session("sModoPop") = Request.QueryString("Modo")
        Session("sNumLineaEditar") = Request.QueryString("Lin")

        If Session("sModoPop") = "" Then
            Session("sModoPop") = Request.QueryString("Action")
        End If

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 And Session("sModoPop") = "e" Then
            pnlEditarsinDesc.Visible = True
            pnlEditar.Visible = False
            pnlAgregar.Visible = False
            pnlAgregarSinDesc.Visible = False
        Else
            If Session("sModoPop") = "Add" Then
                pnlAgregar.Visible = True

                ''Revisamos la parametrización de las existencias
                ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"
                Dim dtVendesinStock As New DataTable
                dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
                If dtVendesinStock.Rows.Count > 0 Then


                    If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then

                    Else
                        'solo para vendedores
                        If dtVendesinStock.Rows(0)(0) = "NO" Then
                            ''Evaluamos el stock
                            Dim existencia = fnRevisaExistencias(Request.QueryString("Code"))
                            If existencia = 0 Then
                                '   HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                                pnlAgregar.Visible = False
                                pnlAgregarSinDesc.Visible = False

                            End If
                        End If
                    End If
                End If


            End If





        End If

        If Session("sModoPop") = "Add" Then
            ''Por default
            pnlAgregarSinDesc.Visible = True
            pnlEditarsinDesc.Visible = False
            pnlEditar.Visible = False
            pnlAgregar.Visible = False
        End If
        Dim dtVendesinStockB2B As New DataTable
        ssql = "SELECT ISNULL(cvVendeSinStockB2B,'SI') as VendeSinStock ,ISNULL(cvCantidadStockBajoB2B,'0') as MinStock,ISNULL(cvLeyendaStockBajoB2B,'Artículo sin Stock') as LeyendaStock from Config.Parametrizaciones "
        dtVendesinStockB2B = objDatos.fnEjecutarConsulta(ssql)

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 And Session("sModoPop") = "Add" Then
            'B2B en Modo Agregar
            pnlAgregarSinDesc.Visible = True
            pnlEditarsinDesc.Visible = False
            pnlEditar.Visible = False
            pnlAgregar.Visible = False


            If dtVendesinStockB2B.Rows.Count > 0 Then
                Dim existencia As Double = 0

                If Session("ItemCodeTallaColor") <> "" Then
                    existencia = fnRevisaExistencias(Session("ItemCodeTallaColor"))
                    objDatos.fnLog("B2B Existencia", Session("ItemCodeTallaColor") & "-" & existencia)
                Else
                    existencia = fnRevisaExistencias(Request.QueryString("Code"))
                End If


                If existencia <= CDbl(dtVendesinStockB2B.Rows(0)(1)) Then
                    objDatos.fnLog("B2B Existencia ", "load")
                    'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                    lblExistencia.Text = dtVendesinStockB2B.Rows(0)("LeyendaStock") & "- Stock: " & existencia
                    If Session("LabelExistencia") <> "" Then
                        lblExistencia.Text = dtVendesinStockB2B.Rows(0)("LeyendaStock") & Session("LabelExistencia")
                    End If
                    If CStr(dtVendesinStockB2B.Rows(0)("VendeSinStock")) = "NO" And existencia = 0 Then
                        pnlAgregar.Visible = False
                        pnlCantidad.Visible = False
                        pnlAgregar.Visible = False
                        pnlAgregarSinDesc.Visible = False
                    End If
                Else
                    '   pnlAgregar.Visible = True
                End If

            End If
        End If

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 And Session("sModoPop") <> "Add" Then
            'B2B en Modo Editar
            pnlEditarsinDesc.Visible = True
            pnlAgregarSinDesc.Visible = False
            pnlEditar.Visible = False
            pnlAgregar.Visible = False

            If dtVendesinStockB2B.Rows.Count > 0 Then
                Dim existencia As Double = 0

                If Session("ItemCodeTallaColor") <> "" Then
                    existencia = fnRevisaExistencias(Session("ItemCodeTallaColor"))
                Else
                    existencia = fnRevisaExistencias(Request.QueryString("Code"))
                End If

                If existencia <= CDbl(dtVendesinStockB2B.Rows(0)(1)) Then
                    objDatos.fnLog("B2B Existencia ", "load-Editar")
                    'HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                    lblExistencia.Text = dtVendesinStockB2B.Rows(0)("LeyendaStock") & "- Stock: " & existencia
                    If CStr(dtVendesinStockB2B.Rows(0)("VendeSinStock")) = "NO" And existencia = 0 Then
                        pnlEditarsinDesc.Visible = False
                        pnlCantidad.Visible = False
                    End If


                End If

            End If
        End If


        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) <> 0 And Session("sModoPop") = "Add" Then
            'Vendedores en Modo Agregar
            If pnlDescuento.Visible = True Then
                pnlAgregar.Visible = True
                pnlAgregarSinDesc.Visible = False
            Else
                pnlAgregar.Visible = False
                pnlAgregarSinDesc.Visible = True
            End If



            pnlEditarsinDesc.Visible = False
            pnlEditar.Visible = False


        End If

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) <> 0 And Session("sModoPop") <> "Add" Then
            'Vendedores en Modo Editar

            pnlAgregar.Visible = False
            pnlAgregarSinDesc.Visible = False
            pnlEditarsinDesc.Visible = False
            pnlEditar.Visible = False


            If pnlDescuento.Visible = True Then
                pnlEditar.Visible = True
            Else
                pnlEditarsinDesc.Visible = True
            End If



        End If

        If Session("RazonSocial") = "" And CInt(Session("slpCode")) = 0 And Session("sModoPop") = "Add" Then
            'B2C
            pnlAgregarSinDesc.Visible = True
            pnlAgregar.Visible = False
            pnlEditarsinDesc.Visible = False
            pnlEditar.Visible = False

            ssql = "SELECT ISNULL(cvB2CActivo,'SI') from Config.Parametrizaciones"
            Dim dtB2C As New DataTable
            dtB2C = objDatos.fnEjecutarConsulta(ssql)
            If dtB2C.Rows.Count > 0 Then
                If dtB2C.Rows(0)(0) = "NO" Then
                    pnlCantidad.Visible = False
                    pnlAgregarSinDesc.Visible = False
                    lblPrecio.Text = ""

                End If
            End If
        End If

        If Session("RazonSocial") = "" And CInt(Session("slpCode")) = 0 And Session("sModoPop") <> "Add" Then
            'B2C
            pnlEditarsinDesc.Visible = True

            pnlAgregarSinDesc.Visible = False
            pnlAgregar.Visible = False
            pnlEditar.Visible = False


            ssql = "SELECT ISNULL(cvB2CActivo,'SI') from Config.Parametrizaciones"
            Dim dtB2C As New DataTable
            dtB2C = objDatos.fnEjecutarConsulta(ssql)
            If dtB2C.Rows.Count > 0 Then
                If dtB2C.Rows(0)(0) = "NO" Then
                    pnlCantidad.Visible = False
                    pnlAgregar.Visible = False
                End If
            End If

        End If

        Try
            AddHandler ddlAtr1.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            AddHandler ddlAtr2.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            AddHandler ddlAtr3.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
            AddHandler ddlAtr4.SelectedIndexChanged, AddressOf ddl_SelectedIndexChanged
        Catch ex As Exception

        End Try


    End Sub
    Public Function fnRevisaExistencias(itemCode As String) As Double
        Dim existencia As Double = 0
        ''Existencia 
        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
        Dim dtExistencia As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtExistencia.Rows.Count > 0 Then
            existencia = CDbl(dtExistencia.Rows(0)(0))
        End If
        Return existencia
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
                    iCantPiezasTotales = iCantPiezasTotales + Partida.Cantidad
                    objDatos.fnLog("Fletes STOP", "Monto Carrito: " & Partida.TotalLinea)
                    fMontoCarrito = fMontoCarrito + ((Partida.Precio * Partida.Cantidad) * (1 - (Partida.Descuento / 100)))
                    'fMontoCarrito = fMontoCarrito + Partida.TotalLinea
                End If

            End If
        Next

        If fMontoCarrito = 0 Then

        End If
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
            If CInt(sResultadoDivFlete(0)) < 1 Then
                iMontoFlete = iMontoPorFlete
            Else
                iMontoFlete = iMontoPorFlete * (CInt(sResultadoDivFlete(0)) + 1)
            End If
        Else
            ''Flete gratis, cargamos 1 centavo y lo que se acumule
            iMontoFlete = 0.0

            ''Si son mas de 70 piezas (un segundo multiplo, cargamos monto extra de flete)


            '   MsgBox(sResultadoDiv(0))
            If CInt(sResultadoDivFlete(0)) > 1 Then
                iMontoFlete = iMontoFlete + (iMontoPorFlete * (CInt(sResultadoDivFlete(0)) - 1))
            End If

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

    Public Sub fnCargaFichasColores(itemcode As String)
        'Exit Sub
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

    <WebMethod>
    Public Shared Function CargarCarrito(Cantidad As String, desc As String, Articulo As String) As String


        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        Dim dPrecioActual As Double = 0

        If Articulo = "" Then
            Articulo = HttpContext.Current.Session("ProductoVista")
        End If
        objDatos.fnLog("Preview articulo:", Articulo)

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
                If CDbl(desc) > CDbl(dtDescValido.Rows(0)(0)) Then
                    HttpContext.Current.Session("errDescuento") = "Ha otorgado un descuento mayor al que tiene permitido: " & CStr(CDbl(dtDescValido.Rows(0)(0)).ToString("###,###.#0")) & " % "
                    Exit Function
                End If
            End If

        End If

        If HttpContext.Current.Session("sModoPop") = "e" Then

            objDatos.fnLog("Preview :", "modo editar")

            For Each partida In HttpContext.Current.Session("Partidas")
                If partida.ItemCode = HttpContext.Current.Session("ProductoVista") Then
                    Articulo = HttpContext.Current.Session("ProductoVista")
                    partida.Cantidad = Cantidad
                    partida.Descuento = desc
                    objDatos.fnLog("Preview entra:", "Cant:" & Cantidad & " desc:" & desc)
                    Try
                        If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Articulo)
                        End If
                        If HttpContext.Current.Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                            If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                                'B2B sin IVA
                                dPrecioActual = dPrecioActual / 1.16
                            End If
                        End If

                    Catch ex As Exception

                    End Try
                    If dPrecioActual <> 0 Then
                        partida.Precio = dPrecioActual
                    End If
                End If

            Next
            HttpContext.Current.Session("ActualizaCarrito") = "SI"
            ' Dim carrito As New Carrito
            'carrito.fnCargaCarrito(HttpContext.Current.Session("Lin"), Cantidad)

        Else
            partida.ItemCode = Articulo
            partida.Cantidad = Cantidad
            ''Ahora el itemName
            Try
                If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                    dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                Else
                    dPrecioActual = objDatos.fnPrecioActual(Articulo)
                End If
                If HttpContext.Current.Session("Cliente") <> "" Then
                    dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))

                End If

                Dim descEspecial As Double = 0
                ''Revisamos si no tiene desc especial
                If HttpContext.Current.Session("Cliente") <> "" Then

                    descEspecial = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))

                    If descEspecial > 0 Then
                        desc = descEspecial
                    End If

                    If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                        'B2B sin IVA
                        dPrecioActual = dPrecioActual / 1.16
                    End If
                End If
                objDatos.fnLog("Preview descto", CDbl(desc))
                objDatos.fnLog("Preview precio", dPrecioActual)
                partida.Descuento = desc


                partida.Precio = dPrecioActual
                partida.TotalLinea = partida.Cantidad * partida.Precio

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
                                        HttpContext.Current.Session("ErrorExistencia") = "El artículo no puede cargarse al carrito porque no se pueden combinar artículos en USD y MXP."
                                        Exit Function
                                    End If

                                End If
                            End If

                        Next

                    End If


                End If





                ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                Dim dtItemName As New DataTable
                dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                partida.ItemName = dtItemName.Rows(0)(0)
            Catch ex As Exception
                objDatos.fnLog("Error carga carrito", ex.Message)
            End Try

            Dim iNumLinea As Int16 = 0
            For Each Partidacont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                iNumLinea += 1
            Next
            partida.Linea = iNumLinea
            HttpContext.Current.Session("Partidas").add(partida)
        End If

        ''una vez que cargamos al carrito, validamos si es STOP Catalogo, para ver si por la cantidad de prendas no tenemos que cargar seguro o flete

        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
            Dim objLocal As New preview_popup
            objLocal.fnAgregaFletesSeguros_StopCatalogo()
        End If


        Dim result As String = "Entró:" & Articulo

        Return result
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


            Dim IdAtributo As Int16 = 0
            For Each control As System.Web.UI.Control In pnlTallaColor.Controls

                If control.ClientID.Contains("ddl") Then
                    If control.Visible = True Then
                        IdAtributo = IdAtributo + 1

                        ssql = "select cvCampoFiltra from config.TallaColor WHERe ciIdRel=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).ToolTip & "'"
                        Dim dtCampoFiltro As New DataTable
                        dtCampoFiltro = objDatos.fnEjecutarConsulta(ssql)
                        objDatos.fnLog("SelectedIndex", ssql.Replace("'", ""))
                        If dtCampoFiltro.Rows.Count > 0 Then
                            sCondicion = sCondicion & " AND " & dtCampoFiltro.Rows(0)(0) & "=" & "'" & DirectCast(control, System.Web.UI.WebControls.DropDownList).SelectedValue & "'"
                        End If

                    End If

                End If

            Next
            Dim sCaracterMoneda As String = "$ "
            ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
            Dim dtCaracter As New DataTable
            dtCaracter = objDatos.fnEjecutarConsulta(ssql)
            If dtCaracter.Rows.Count > 0 Then
                sCaracterMoneda = dtCaracter.Rows(0)(0)
            End If


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
                            dPrecio = dPrecio * (1 + dtPorcIVA.Rows(0)(0))

                        End If
                    End If
                End If

                lblPrecio.Text = sCaracterMoneda & " " & dPrecio.ToString("###,###,###.#0")
                Session("ItemCodeTallaColor") = dtPrecio.Rows(0)("ItemCode")
                Session("PrecioCodeTallaColor") = dPrecio
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

            lblPrecio.Text = sMoneda & " " & CDbl(Session("PrecioCodeTallaColor")).ToString("###,###,###.#0")


            Session("Moneda") = sMoneda
            objDatos.fnLog("ddl_sel", sMoneda & " " & lblPrecio.Text)
            lblPrecio.Text = sCaracterMoneda & " " & dPrecio.ToString("###,###,###.#0") & " " & sMoneda





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
                        objDatos.fnLog("B2B Existencia ", "ddl rows count")
                        pnlExistencia.Visible = True

                        lblExistencia.Text = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"
                        Session("LabelExistencia") = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"
                        objDatos.fnLog("ddl existen", Session("ItemCodeTallaColor") & "-" & CDbl(dtExistencia.Rows(0)(0)))
                    End If

                Else
                    pnlExistencia.Visible = False
                End If
            End If

        Catch ex As Exception
            objDatos.fnLog("ddlSelected ex", ex.Message)
        End Try
    End Sub

    <WebMethod>
    Public Shared Function CargarCarritoSinDesc(Cantidad As String, Articulo As String) As String
        Dim objDatos As New Cls_Funciones

        objDatos.fnLog(" ACTION ", "entra")
        Articulo = HttpContext.Current.Request.QueryString("Code")
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String

        Try

            For Each PartidaCont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")

            Next

        Catch ex As Exception

            HttpContext.Current.Session("Partidas") = New List(Of Cls_Pedido.Partidas)

        End Try

        If Articulo = "" Then
            Articulo = HttpContext.Current.Request.QueryString("code")
        End If
        objDatos.fnLog(" articulo ", Articulo)
        Dim dPrecioActual As Double = 0

        Dim sTallaColor As String = ""
        Dim dtTallasColores As New DataTable

        Dim desc As Double = 0
        Dim fDescuento As Double = 0
        fDescuento = objDatos.fnDesctoB2C(Articulo)
        Dim sAction As String = ""
        Try
            sAction = HttpContext.Current.Request.QueryString("Action")
        Catch ex As Exception

        End Try
        objDatos.fnLog("editar ACTION ", sAction)
        If sAction = "e" Then
            objDatos.fnLog("editar", "entra: " & Articulo)
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    Articulo = HttpContext.Current.Session("ItemCodeTallaColor")
                    For Each partida In HttpContext.Current.Session("Partidas")
                        If partida.ItemCode = Articulo And partida.Linea = HttpContext.Current.Session("sNumLineaEditar") Then


                            objDatos.fnLog("editar", "entra: " & Articulo)
                            partida.Generico = HttpContext.Current.Session("ProductoVista")
                            ''Cambiamos
                            partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                            partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                            partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))
                            partida.Cantidad = Cantidad
                            fDescuento = objDatos.fnDesctoB2C(HttpContext.Current.Session("ProductoVista"))
                            partida.Descuento = fDescuento
                            ''Ahora el itemName

                            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                            ssql = ssql.Replace("[%0]", "'" & partida.ItemCode & "'")

                            Dim dtItemName As New DataTable
                            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtItemName.Rows.Count = 0 Then
                                partida.ItemName = "ND"
                            Else
                                partida.ItemName = dtItemName.Rows(0)(0)
                            End If

                        End If

                    Next
                Else

                    For Each partida In HttpContext.Current.Session("Partidas")
                        If partida.ItemCode = Articulo And partida.Linea = HttpContext.Current.Session("sNumLineaEditar") Then
                            partida.Cantidad = Cantidad

                        End If

                    Next
                End If
            End If


            HttpContext.Current.Session("ActualizaCarrito") = "SI"
            Dim carrito As New Carrito
            carrito.fnCargaCarrito(HttpContext.Current.Session("Lin"), Cantidad)

        Else
            ''Revisamos si hay que mostrar tallas y colores
            objDatos.fnLog("Carrito popup", "entra")
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            dtTallasColores = New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                    objDatos.fnLog("CargarCarrito talla color:", sTallaColor)
                    Articulo = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.Generico = HttpContext.Current.Session("ProductoVista")
                    ''Cambiamos
                    partida.Cantidad = Cantidad
                    partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                    partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))
                    fDescuento = objDatos.fnDesctoB2C(HttpContext.Current.Session("ProductoVista"))
                    If fDescuento = 0 Then
                        fDescuento = objDatos.fnObtenerDescuentoEspecialDelta(HttpContext.Current.Session("ProductoVista"))
                    End If
                    partida.Descuento = fDescuento
                    objDatos.fnLog("CargarCarrito talla color desc:", fDescuento)
                Else
                    objDatos.fnLog("Carrito popup", HttpContext.Current.Session("ProductoVista"))
                    Articulo = HttpContext.Current.Session("ProductoVista")
                    partida.ItemCode = Articulo
                    partida.Cantidad = Cantidad
                    If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then
                        If HttpContext.Current.Session("ListaPrecios") Is Nothing Then
                            dPrecioActual = objDatos.fnPrecioActual(Articulo)
                        Else
                            objDatos.fnLog("Carrito popup", "Obtiene Precio")
                            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                        End If

                    Else
                        dPrecioActual = objDatos.fnPrecioActual(Articulo)
                        partida.Precio = dPrecioActual
                        partida.TotalLinea = partida.Cantidad * partida.Precio
                    End If
                    If HttpContext.Current.Session("Cliente") <> "" Then
                        dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))

                        If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                            'B2B sin IVA
                            dPrecioActual = dPrecioActual / 1.16
                        End If

                        partida.Precio = dPrecioActual
                        partida.TotalLinea = partida.Cantidad * partida.Precio
                    End If

                End If
            End If

            If sTallaColor = "" Then
                objDatos.fnLog("Carrito popup", "Agregar a carrito")
                partida.ItemCode = Articulo

                '   partida.ItemCode = Articulo

                If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                    dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                Else
                    dPrecioActual = objDatos.fnPrecioActual(Articulo)
                End If
                If HttpContext.Current.Session("Cliente") <> "" Then
                    dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                    partida.Descuento = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                End If

                If desc = 0 Then
                    ''Revisamos si no tiene desc especial
                    If HttpContext.Current.Session("Cliente") <> "" Then
                        desc = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                    End If

                End If

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
                        objDatos.fnLog("existencia", ssql.Replace("'", ""))
                        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtExistencia.Rows.Count > 0 Then
                            existencia = CDbl(dtExistencia.Rows(0)(0))
                        End If
                        For Each Partida2 As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                            If Partida2.ItemCode <> "BORRAR" Then
                                If Partida2.ItemCode = Articulo Then
                                    existencia = existencia - Partida2.Cantidad
                                End If
                            End If
                        Next

                        If CInt(HttpContext.Current.Session("slpCode")) <> 0 And (CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA")) Then
                            'Vendedores Hawk pueden vender sin existencia
                        Else
                            If existencia - Cantidad < 0 Then
                                ' HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                                HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                                Exit Function
                            End If
                        End If

                    End If
                End If
            Else
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
                        ssql = ssql.Replace("[%0]", "'" & HttpContext.Current.Session("ItemCodeTallaColor") & "'")
                        objDatos.fnLog("existencia", ssql.Replace("'", ""))
                        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtExistencia.Rows.Count > 0 Then
                            existencia = CDbl(dtExistencia.Rows(0)(0))
                        End If

                        If existencia - Cantidad < 0 Then
                            ' HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                            HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                            Exit Function
                        End If
                    End If
                End If

            End If

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


            ' partida.Descuento = desc
            '   partida.Precio = dPrecioActual
            '  partida.TotalLinea = partida.Cantidad * partida.Precio

            ''Ahora el itemName
            objDatos.fnLog("Carrito popup", "Obtiene itemName")

            ssql = objDatos.fnObtenerQuery("Nombre-Producto")

            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            objDatos.fnLog("Carrito itemname", ssql.replace("'", ""))
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count = 0 Then
                partida.ItemName = "ND"
            Else
                partida.ItemName = dtItemName.Rows(0)(0)
            End If


            Dim iNumLinea As Int16 = 0
            For Each Partidacont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                iNumLinea += 1
            Next
            partida.Linea = iNumLinea





            HttpContext.Current.Session("Partidas").add(partida)
        End If
        Try
            objDatos.fnLog("Carrito popup", "Antes fletes")
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                Dim objLocal As New preview_popup
                objLocal.fnAgregaFletesSeguros_StopCatalogo()
            End If
        Catch ex As Exception
            objDatos.fnLog("EX carga:", ex.Message.Replace("'", ""))
        End Try


        Dim result As String = "Entró:" & Articulo

        Return result
    End Function



    <WebMethod>
    Public Shared Function EditarCarritoSinDesc(Cantidad As String, Articulo As String) As String
        Dim objDatos As New Cls_Funciones
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String


        Dim dPrecioActual As Double = 0

        Dim sTallaColor As String = ""
        Dim dtTallasColores As New DataTable

        Dim desc As Double = 0
        Try
            objDatos.fnLog("editar", "entra: " & Articulo)
        Catch ex As Exception

        End Try
        Articulo = HttpContext.Current.Request.QueryString("Code")

        objDatos.fnLog("editar", "entra: " & Articulo)

        '  Articulo = HttpContext.Current.Session("ProductoVista")

        If Articulo = "" Then
            Articulo = HttpContext.Current.Request.QueryString("code")
        End If

        objDatos.fnLog("editar", "entra: " & Articulo)
        ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
        dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
        If dtTallasColores.Rows.Count > 0 Then
            If dtTallasColores.Rows(0)(0) = "SI" Then

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
                        ssql = ssql.Replace("[%0]", "'" & HttpContext.Current.Session("ItemCodeTallaColor") & "'")
                        objDatos.fnLog("existencia", ssql.Replace("'", ""))
                        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtExistencia.Rows.Count > 0 Then
                            existencia = CDbl(dtExistencia.Rows(0)(0))
                        End If

                        If existencia - Cantidad < 0 Then
                            ' HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                            HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                            Exit Function
                        End If
                    End If
                End If


                For Each partida In HttpContext.Current.Session("Partidas")
                    If partida.Linea = HttpContext.Current.Session("sNumLineaEditar") Then

                        If HttpContext.Current.Session("ItemCodeTallaColor") <> "" Then
                            Articulo = HttpContext.Current.Session("ItemCodeTallaColor")
                            partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                            partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                            partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))
                        End If

                        objDatos.fnLog("editar", "entra: " & Articulo)
                        partida.Generico = HttpContext.Current.Session("ProductoVista")
                        ''Cambiamos

                        partida.Cantidad = Cantidad
                        ''Ahora el itemName

                        ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                        ssql = ssql.Replace("[%0]", "'" & partida.ItemCode & "'")

                        Dim dtItemName As New DataTable
                        dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtItemName.Rows.Count = 0 Then
                            partida.ItemName = "ND"
                        Else
                            partida.ItemName = dtItemName.Rows(0)(0)
                        End If

                    End If

                Next
            Else
                objDatos.fnLog("Num Linea:", HttpContext.Current.Session("sNumLineaEditar"))
                For Each partida In HttpContext.Current.Session("Partidas")
                    objDatos.fnLog("Num Linea:", partida.Linea & "-" & HttpContext.Current.Session("sNumLineaEditar"))
                    If partida.ItemCode = Articulo And partida.Linea = HttpContext.Current.Session("sNumLineaEditar") Then
                        partida.Cantidad = Cantidad

                    End If
                    If partida.ItemCode = Articulo Then
                        partida.Cantidad = Cantidad
                    End If

                Next
                Dim CantidadExistente As Double = 0
                Try

                    For Each PartidaCont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                        If PartidaCont.ItemCode = Articulo Then
                            CantidadExistente = CantidadExistente + PartidaCont.Cantidad
                        End If
                    Next

                Catch ex As Exception

                    HttpContext.Current.Session("Partidas") = New List(Of Cls_Pedido.Partidas)

                End Try



                Dim fDescuentoEspecial As Double = 0
                Dim fDescuentoEspecialxCantidad As Double = 0
                fDescuentoEspecial = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                fDescuentoEspecialxCantidad = objDatos.fnObtenerDescuentoPorCantidad(Articulo, HttpContext.Current.Session("ListaPrecios"), CantidadExistente)

                For Each PartidaDesc As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    If PartidaDesc.ItemCode = Articulo Then
                        If fDescuentoEspecial >= fDescuentoEspecialxCantidad Then
                            partida.Descuento = fDescuentoEspecial
                        Else
                            partida.Descuento = fDescuentoEspecialxCantidad
                        End If
                    End If
                Next

                objDatos.fnLog("Descuento a aplicar:", "Especial:" & fDescuentoEspecial & " Volumen:" & fDescuentoEspecialxCantidad)


            End If
        End If

        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
            Dim objLocal As New preview_popup
            objLocal.fnAgregaFletesSeguros_StopCatalogo()
        End If

        HttpContext.Current.Session("ActualizaCarrito") = "SI"
        Dim carrito As New Carrito
        carrito.fnCargaCarrito(HttpContext.Current.Session("Lin"), Cantidad)


        Dim result As String = "Entró:" & Articulo

        Return result
    End Function


    Public Sub fnCargaProducto(itemCode As String)
        Try
            Session("ItemActual") = itemCode


            ssql = objDatos.fnObtenerQuery("Info-Producto")
            ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
            Dim dtGeneral As New DataTable
            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtGeneral.Rows.Count = 0 Then
                Response.Redirect("Index.aspx")
                Exit Sub
            End If

            Dim sHtmlEncabezado As String = ""
            Dim sHtmlBanner As String = ""
            ''Cargamos la Imagenes
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Imagenes' order by T1.ciOrden "
            Dim dtImagenesPlantilla As New DataTable
            dtImagenesPlantilla = objDatos.fnEjecutarConsulta(ssql)
            If dtImagenesPlantilla.Rows.Count > 0 Then
                ''TumbNail
                sHtmlEncabezado = "<div class='col-xs-12 col-sm-6'>"
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-3'>"
                sHtmlEncabezado = sHtmlEncabezado & " <div class='product-nav'>"
                For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                Next
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                ''Modo Normal
                sHtmlBanner = ""
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-sm-9'>"
                sHtmlEncabezado = sHtmlEncabezado & " <div class='product-for'>"
                For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                    If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                        sHtmlBanner = sHtmlBanner & " <div><img src=" & "'images/no-image.png' class='img-responsive' alt='descrip imagen'></div>"
                    Else
                        sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                    End If

                    ' sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                Next
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"

                sHtmlEncabezado = sHtmlEncabezado & "</div>"
            End If

            ''Nombre y descripcion del articulo
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Descripcion' order by T1.ciOrden "
            Dim dtTituloPlantilla As New DataTable
            dtTituloPlantilla = objDatos.fnEjecutarConsulta(ssql)
            If dtTituloPlantilla.Rows.Count > 0 Then
                sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-12 col-sm-6 info-producto-int'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='p-descripcion'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <span class='heart'></span>" '<img src='img/home/favorite.png' class='img-responsive' alt='imagen corazon'>
                sHtmlBanner = ""
                For i = 0 To dtTituloPlantilla.Rows.Count - 1 Step 1
                    If dtTituloPlantilla.Rows(i)("Tipo") = "Cadena" Then
                        If dtTituloPlantilla.Rows(i)("Resaltado") = "SI" Then
                            sHtmlBanner = sHtmlBanner & " <h1 class='titulo'>" & dtGeneral.Rows(0)(dtTituloPlantilla.Rows(i)("Campo")) & "</h1> "
                        Else
                            sHtmlBanner = sHtmlBanner & " <div class='descripcion'>" & dtGeneral.Rows(0)(dtTituloPlantilla.Rows(i)("Campo")) & "</div> "
                        End If

                    Else

                        If dtTituloPlantilla.Rows(i)("Tipo") = "Precio" Then
                            Dim dPrecioActual As Double
                            If CInt(Session("slpCode")) <> 0 Then

                                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"))
                            Else
                                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"))
                            End If
                            If Session("Cliente") <> "" Then
                                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"))

                            End If

                            sHtmlBanner = sHtmlBanner & "  <div class='col-xs-12 no-padding sec-prec'><small class='precio-org'>" & dPrecioActual.ToString("$ ###,###,###.#0") & "</small></div>"
                        End If

                    End If


                Next
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

                sHtmlEncabezado = sHtmlEncabezado & "</div></div>"
            End If

            Dim literal As New LiteralControl(sHtmlEncabezado)
            ' pnlInfoProducto.Controls.Clear()
            '   pnlInfoProducto.Controls.Add(literal)

        Catch ex As Exception

        End Try



    End Sub
    Public Sub fnOcultarLabel(Label As String)
        For Each control As System.Web.UI.Control In pnlTallaColor.Controls
            If control.ClientID.Contains(Label) Then
                control.Visible = False
            End If
        Next
    End Sub

    Public Sub CalculaPrecio()
        Try
            Dim sValores As String = ""
            Dim IdFiltro As Int16 = 1
            Dim sCondicion As String = ""



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

            ''Obtenemos el query para calcular precio
            ssql = objDatos.fnObtenerQuery("PrecioTallaColor")
            ssql = ssql.Replace("[%0]", Request.QueryString("Code"))
            ssql = ssql.Replace("[%1]", sCondicion)
            ssql = ssql.Replace("[%2]", "'" & Session("ListaPrecios") & "'")

            Dim dtPrecio As New DataTable
            dtPrecio = objDatos.fnEjecutarConsultaSAP(ssql)

            '  objDatos.fnLog("Calcular Precios", ssql.Replace("'", ""))
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
                Session("ItemCodeGenerico") = Request.QueryString("Code")
                Session("ItemCodeTallaColor") = dtPrecio.Rows(0)("ItemCode")
                Session("PrecioCodeTallaColor") = Precio
            Else
                ''Estamos en un talla color, pero el codigo es unico, no tiene tallas y colores
                Precio = objDatos.fnPrecioActual(Request.QueryString("Code"), Session("ListaPrecios"))
                Session("ItemCodeGenerico") = Request.QueryString("Code")
                Session("ItemCodeTallaColor") = Request.QueryString("Code")
                Session("PrecioCodeTallaColor") = Precio
            End If

            If Precio = 0 Then
                Precio = objDatos.fnPrecioActual(Request.QueryString("Code"), Session("ListaPrecios"))
                Session("ItemCodeGenerico") = Request.QueryString("Code")
                Session("ItemCodeTallaColor") = Request.QueryString("Code")
                Session("PrecioCodeTallaColor") = Precio
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

            Dim sLeyendaPrecio As String = ""
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                sLeyendaPrecio = "  "
            End If

            Session("Moneda") = sMoneda

            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(Session("ItemCodeGenerico"))

            If fDescuento > 0 Then

                lblPrecio.Style.Add("text-decoration", "line-through")
                lblPreciodesc.Visible = True
                lblPreciodesc.Text = " " & (Precio * (1 - (fDescuento / 100))).ToString("###,###,###.#0") & " " & sMoneda & sLeyendaPrecio
            Else
                lblPrecio.Text = Precio.ToString("###,###,###.#0") & " " & sMoneda & sLeyendaPrecio
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
                        objDatos.fnLog("B2B Existencia ", "calcula precio rowscount")
                        pnlExistencia.Visible = True
                        lblExistencia.Text = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"

                        objDatos.fnLog("calcula precio existen", Session("ItemCodeTallaColor") & "-" & CDbl(dtExistencia.Rows(0)(0)))
                    End If

                Else
                    pnlExistencia.Visible = False
                End If
            End If

        Catch ex As Exception
            objDatos.fnLog("Calcula Precio Err", ex.Message)
        End Try
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
    <WebMethod>
    Public Shared Function fnRate(Cantidad As String) As String
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        ssql = "insert into config.Rate(cvItemCode,ciCalificacion,cdFecha)VALUES(" _
            & "'" & HttpContext.Current.Session("ProductoVista") & "'," _
            & "'" & Cantidad & "',getdate())"
        objDatos.fnEjecutarInsert(ssql)

    End Function
    Public Sub fnCargaProductov2(itemCode As String)
        Try
            Dim sCaracterMoneda As String = "$"
            objDatos.fnLog("Carga producto v2", "")
            ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
            Dim dtCaracter As New DataTable
            dtCaracter = objDatos.fnEjecutarConsulta(ssql)
            If dtCaracter.Rows.Count > 0 Then
                sCaracterMoneda = dtCaracter.Rows(0)(0)
            End If


            ssql = "SELECT ISNULL(cvCalificaProductos,'NO') FROM config.Parametrizaciones "
            Dim dtCalifica As New DataTable
            dtCalifica = objDatos.fnEjecutarConsulta(ssql)
            If dtCalifica.Rows.Count > 0 Then
                If dtCalifica.Rows(0)(0) = "SI" Then
                    pnlRating.Visible = True
                    fnCargarRating(itemCode)

                Else
                    pnlRating.Visible = False
                End If
            End If

            ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
            Dim dtMonedas As New DataTable
            ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
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
            fnCargaFichasColores(itemCode)
            Session("Moneda") = sMoneda1

            Session("ItemActual") = itemCode


            ssql = objDatos.fnObtenerQuery("Info-Producto")

            ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
            Dim dtGeneral As New DataTable
            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtGeneral.Rows.Count = 0 Then
                Response.Redirect("Index.aspx")
                Exit Sub
            End If
            Dim iband As Int16 = 0
            Dim sHtmlEncabezado As String = ""
            Dim sHtmlBanner As String = ""
            ''Cargamos la Imagenes
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Imagenes' order by T1.ciOrden "
            Dim dtImagenesPlantilla As New DataTable
            dtImagenesPlantilla = objDatos.fnEjecutarConsulta(ssql)
            If dtImagenesPlantilla.Rows.Count > 0 Then
                ''TumbNail



                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & ".jpg") Then

                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "images/products/" & itemCode & ".jpg ' class='img-responsive' alt='descrip imagen'></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-1.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "images/products/" & itemCode & "-1.jpg ' class='img-responsive' alt='descrip imagen'></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-2.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "images/products/" & itemCode & "-2.jpg ' class='img-responsive' alt='descrip imagen'></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-3.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "images/products/" & itemCode & "-3.jpg ' class='img-responsive' alt='descrip imagen'></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-4.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "images/products/" & itemCode & "-4.jpg ' class='img-responsive' alt='descrip imagen'></div>"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-5.jpg") Then
                    sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "images/products/" & itemCode & "-5.jpg ' class='img-responsive' alt='descrip imagen'></div>"
                    iband = 1
                End If


                If iband = 0 Then
                    For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                        If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                            sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src='images/no-image.png' class='img-responsive' alt='descrip imagen'></div>"
                        Else
                            If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo"))) Then
                                sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'images/products/" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                            Else
                                sHtmlBanner = sHtmlBanner & " <div class='thumbnail'><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                            End If

                        End If

                    Next
                End If

                Dim literal1 As New LiteralControl(sHtmlBanner)
                pnlThum.Controls.Clear()
                pnlThum.Controls.Add(literal1)



                ''Modo Normal
                sHtmlBanner = ""
                iband = 0
                Dim sSrcImagen As String = ""

                objDatos.fnLog("Foto", "Validando si existe: " & Server.MapPath("~") & "\images\products\" & itemCode & ".jpg")
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode.Replace("-", "") & ".jpg") Then

                    sSrcImagen = "images/products/" & itemCode.Replace("-", "") & ".jpg"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & ".jpg") And iband = 0 Then

                    sSrcImagen = "images/products/" & itemCode & ".jpg"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-1.jpg") And iband = 0 Then
                    sSrcImagen = "images/products/" & itemCode & "-1.jpg"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-2.jpg") And iband = 0 Then
                    sSrcImagen = "images/products/" & itemCode & "-2.jpg"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-3.jpg") And iband = 0 Then
                    sSrcImagen = "images/products/" & itemCode & "-3.jpg"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-4.jpg") And iband = 0 Then
                    sSrcImagen = "images/products/" & itemCode & "-4.jpg"
                    iband = 1
                End If
                If File.Exists(Server.MapPath("~") & "\images\products\" & itemCode & "-5.jpg") And iband = 0 Then
                    sSrcImagen = "images/products/" & itemCode & "-5.jpg"
                    iband = 1
                End If

                If iband = 0 Then
                    For i = 0 To dtImagenesPlantilla.Rows.Count - 1 Step 1
                        If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then

                            sHtmlBanner = sHtmlBanner & " <div><img class='zoom img-responsive' src=" & "'images/no-image.png' class='img-responsive' alt='descrip imagen' data-zoom-image=" & "'images/no-image.png' /></div>"
                        Else
                            If dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                sHtmlBanner = sHtmlBanner & " <div><img class='zoom img-responsive' src=" & "'images/products/" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'  data-zoom-image='" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "'/></div>"
                            Else
                                sHtmlBanner = sHtmlBanner & " <div><img class='zoom img-responsive' src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'  data-zoom-image='" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "'/></div>"
                            End If

                        End If

                        ' sHtmlBanner = sHtmlBanner & " <div><img src=" & "'" & dtGeneral.Rows(0)(dtImagenesPlantilla.Rows(i)("Campo")) & "' class='img-responsive' alt='descrip imagen'></div>"
                    Next
                Else
                    sHtmlBanner = sHtmlBanner & " <div><img class='zoom img-responsive' src=" & "'" & sSrcImagen & "' class='img-responsive' alt='descrip imagen'  data-zoom-image='" & sSrcImagen & "'/></div>"
                End If

                Dim literal2 As New LiteralControl(sHtmlBanner)
                pnlZoom.Controls.Clear()
                pnlZoom.Controls.Add(literal2)

            End If

            Dim sLeyendaPrecio As String = ""
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                sLeyendaPrecio = " "
            End If

            ''Nombre y descripcion del articulo
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Descripcion' order by T1.ciOrden "
            Dim dtTituloPlantilla As New DataTable
            dtTituloPlantilla = objDatos.fnEjecutarConsulta(ssql)
            If dtTituloPlantilla.Rows.Count > 0 Then

                sHtmlBanner = ""
                For i = 0 To dtTituloPlantilla.Rows.Count - 1 Step 1
                    If dtTituloPlantilla.Rows(i)("Tipo") = "Cadena" Then
                        If dtTituloPlantilla.Rows(i)("Resaltado") = "SI" Then
                            lblTitulo.Text = dtGeneral.Rows(0)(dtTituloPlantilla.Rows(i)("Campo"))

                        Else
                            lblDescripcion.Text = dtGeneral.Rows(0)(dtTituloPlantilla.Rows(i)("Campo"))

                        End If

                    Else

                        If dtTituloPlantilla.Rows(i)("Tipo") = "Precio" Then
                            Dim dPrecioActual As Double
                            If CInt(Session("slpCode")) <> 0 Then

                                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"))
                            Else
                                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"))
                            End If
                            objDatos.fnLog("PRecio AIO:", "PAso 1")
                            If Session("Cliente") <> "" Then
                                dPrecioActual = objDatos.fnPrecioActual(dtGeneral.Rows(0)("ItemCode"), Session("ListaPrecios"))
                            End If


                            If Session("Cliente") <> "" Then

                                If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                                    'B2B antes de IVA
                                    dPrecioActual = dPrecioActual / 1.16
                                    objDatos.fnLog("PRecio AIO:", dPrecioActual)
                                End If

                                Dim descEspecial As Double = 0
                                descEspecial = objDatos.fnDescuentoEspecial((dtGeneral.Rows(0)("ItemCode")), Session("Cliente"))
                                If descEspecial > 0 Then
                                    lblPreciodesc.Visible = True
                                    lblPreciodesc.Text = sCaracterMoneda & " " & (dPrecioActual * (1 - (descEspecial / 100))).ToString("###,###,###.#0") & " " & Session("Moneda") & sLeyendaPrecio
                                    lblPrecio.Font.Strikeout = True
                                End If



                            End If

                            If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("STOP CAT") Then 'And Session("RazonSocial") <> ""
                                Dim descuentoPromo As Double = 0
                                descuentoPromo = objDatos.fnObtenerDescuentoEspecialDelta((dtGeneral.Rows(0)("ItemCode")))
                                If descuentoPromo > 0 Then
                                    If dPrecioActual = 0 Then
                                        dPrecioActual = objDatos.fnPrecioActual((dtGeneral.Rows(0)("ItemCode")), Convert.ToInt16(Session("ListaPrecios")))
                                    End If
                                    lblPreciodesc.Visible = True
                                    lblPreciodesc.Text = sCaracterMoneda & " " & (dPrecioActual * (1 - (descuentoPromo / 100))).ToString("###,###,###.#0") & " " & Session("Moneda")
                                    lblPrecio.Font.Strikeout = True

                                End If

                            Else

                            End If

                            lblPrecio.Text = sCaracterMoneda & " " & dPrecioActual.ToString("###,###,###.#0") & " " & Session("Moneda") & sLeyendaPrecio
                        End If

                    End If
                Next
            End If

            ''Existencia 
            If Session("Cliente") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = "SELECT ISNULL(cvMuestraStock,'NO') FROM [config].[ParametrizacionesB2B]"
            Else
                ssql = "select ISNULL(cvMuestraExistencias,'NO') FROM config.parametrizaciones"
            End If

            objDatos.fnLog("cargaProducto", ssql.Replace("'", ""))

            Dim dtMuestra As New DataTable
            dtMuestra = objDatos.fnEjecutarConsulta(ssql)
            If dtMuestra.Rows.Count > 0 Then
                If dtMuestra.Rows(0)(0) <> "NO" Then
                    If Session("Cliente") <> "" And CInt(Session("slpCode")) = 0 Then
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAPB2B")
                    Else
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
                    End If

                    Dim dtExistencia As New DataTable
                    ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
                    dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencia.Rows.Count > 0 Then
                        objDatos.fnLog("B2B Existencia ", "cargaproducto v2")
                        lblExistencia.Text = "Stock: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N0") & "<br/>"
                    End If

                Else
                    pnlExistencia.Visible = False
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
                                        ssql = ssql.Replace("[%0]", itemCode)
                                        Dim dtDatosCombo As New DataTable
                                        dtDatosCombo = objDatos.fnEjecutarConsultaSAP(ssql)
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

                                    End If
                                End If
                                If control.ClientID.Contains("lbl") Then
                                    If control.ClientID.Contains("lbl" & "Atr" & (atr + 1)) Then
                                        DirectCast(control, System.Web.UI.WebControls.Label).Text = dtConfigTallas.Rows(atr)("cvTallaColor")

                                    End If
                                End If

                            Next
                        Next
                        objDatos.fnLog("Calcula PRecios", "Va a entrar")
                        CalculaPrecio()

                    End If
                End If
            End If
            If STallaColor = "SI" Then
                CalculaPrecio()
            End If

            If Request.QueryString("Action") = "e" Then
                Session("Action") = "e"
                Session("Lin") = Request.QueryString("Lin")
                If STallaColor = "SI" Then
                    lblPrecio.Text = sCaracterMoneda & " " & CDbl(Session("PrecioCodeTallaColor")).ToString("###,###,###.#0") & " " & Session("Moneda") & sLeyendaPrecio
                Else
                    lblPrecio.Text = sCaracterMoneda & " " & CDbl(Request.QueryString("Precio")).ToString("###,###,###.#0") & " " & Session("Moneda") & sLeyendaPrecio
                End If

                ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "document.getElementById('#Cantidad').value=" & Request.QueryString("Cant"), True)

                ssql = "select ISNULL(cvAplicaDescto,'NO') as AplicaDesc from config.Parametrizaciones "
                Dim dtParam As New DataTable
                dtParam = objDatos.fnEjecutarConsulta(ssql)
                If dtParam.Rows.Count > 0 Then
                    If dtParam.Rows(0)("AplicaDesc") = "SI" Then
                        pnlEditar.Visible = True
                        pnlEditarsinDesc.Visible = False
                        pnlAgregar.Visible = False
                        pnlAgregarSinDesc.Visible = False
                    Else
                        pnlEditarsinDesc.Visible = True
                        pnlEditar.Visible = False
                        pnlAgregar.Visible = False
                        pnlAgregarSinDesc.Visible = False
                    End If
                End If
                If CInt(Session("slpCode")) <> 0 Then
                    ''es vendedores y es editar
                    pnlAgregar.Visible = False
                    pnlAgregarSinDesc.Visible = False
                End If


                'ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "document.getElementById('btnAgregar').innerHTML='editar';", True)
            Else
                ''Si hay un usuario B2C loggeado
                If Session("UserB2C") <> "" Then
                    pnlEditarsinDesc.Visible = False
                    pnlEditar.Visible = False
                    pnlAgregar.Visible = False
                    pnlAgregarSinDesc.Visible = True
                End If

                If pnlDescuento.Visible = True Then
                    pnlAgregarSinDesc.Visible = False
                    pnlAgregar.Visible = True
                    pnlEditarsinDesc.Visible = False
                    pnlEditar.Visible = False
                End If

                If Session("Cliente") = "" Then
                    ''es B2C y es agregar
                    pnlEditarsinDesc.Visible = False
                    pnlEditar.Visible = False
                    pnlAgregar.Visible = False
                    pnlAgregarSinDesc.Visible = True
                End If

                If Session("Cliente") <> "" And CInt(Session("slpCode")) = 0 Then
                    ''es B2B y es agregar
                    pnlEditarsinDesc.Visible = False
                    pnlEditar.Visible = False
                    pnlAgregar.Visible = False
                    pnlAgregarSinDesc.Visible = True
                Else
                    If CInt(Session("slpCode")) <> 0 Then
                        ''es vendedores y es agregar
                        pnlEditarsinDesc.Visible = False
                        pnlEditar.Visible = False
                        pnlAgregar.Visible = False
                        pnlAgregarSinDesc.Visible = True
                    End If

                End If




            End If

        Catch ex As Exception

        End Try



    End Sub


    Public Sub CFunction(sender As Object, e As EventArgs)
    End Sub

    'Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
    '    Dim partida As New Cls_Pedido.Partidas
    '    partida.ItemCode = Request.QueryString("code")
    '    partida.Cantidad = txtCantidad.Text
    '    Dim dPrecioActual As Double = 0
    '    If CInt(Session("slpCode")) <> 0 Then

    '        dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), Session("ListaPrecios"))
    '    Else
    '        dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"))
    '    End If

    '    partida.Precio = dPrecioActual
    '    partida.TotalLinea = partida.Cantidad * partida.Precio

    '    ''Ahora el itemName
    '    Try
    '        ssql = objDatos.fnObtenerQuery("Nombre-Producto")
    '        ssql = ssql.Replace("[%0]", "'" & Request.QueryString("code") & "'")
    '        Dim dtItemName As New DataTable
    '        dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
    '        partida.ItemName = dtItemName.Rows(0)(0)
    '    Catch ex As Exception

    '    End Try
    '    Session("Partidas").add(partida)
    '    lblMensaje.Text = "Agregado al carrito"
    'End Sub
End Class
