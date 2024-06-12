
Imports System.Data
Imports System.IO
Imports System.Web.Services

Partial Class Main
    Inherits System.Web.UI.MasterPage
    Public objDatos As New Cls_Funciones
    Public ssql As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' fnCargaMenu()


        ''Cargamos cookies
        Try
            objDatos.fnLog("Zeyco master 1 IsB2B", Session("IsB2B"))
            objDatos.fnLog("Zeyco master 1 RazonSocial", Session("RazonSocial"))
            objDatos.fnLog("Zeyco master 1 Cliente", Session("Cliente"))

            If objDatos.fnObtenerCliente.ToUpper.Contains("ZEYCO") Then
                objDatos.fnLog("Cookie Zeyco paso 1", "entra")

                If Request.Cookies("IsB2B").Value IsNot Nothing Then


                    If Request.Cookies("Cliente").Value IsNot Nothing Then
                        objDatos.fnLog("Cookie Zeyco paso 2", "La cookie tiene:" & Request.Cookies("Cliente").Value)
                        Try
                            Session("Cliente") = Request.Cookies("Cliente").Value
                        Catch ex As Exception
                            Try
                                Session("Cliente") = Request.Cookies("Cliente").Value
                            Catch ex4 As Exception

                            End Try

                        End Try

                        Session("IsB2B") = "SI"
                        Try
                            If CStr(Session("Cliente")).Length = 0 Then
                                objDatos.fnLog("Cookie Zeyco paso 3", "Session cliente vacia")
                                Session("Cliente") = Request.Cookies("Cliente").Value
                                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                                ssql = ssql.Replace("[%0]", Session("Cliente"))
                                'objDatos.fnlog("ListaPrecios", ssql.Replace("'", ""))
                                Dim dtLista As New DataTable
                                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                                If dtLista.Rows.Count > 0 Then
                                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                                Else
                                    Session("ListaPrecios") = "1"
                                End If



                            End If
                        Catch ex As Exception
                            objDatos.fnLog("Cookie Zeyco ex 1", ex.Message.Replace("'", ""))
                            Try
                                Session("Cliente") = Request.Cookies("Cliente").Value
                                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                                ssql = ssql.Replace("[%0]", Session("Cliente"))
                                'objDatos.fnlog("ListaPrecios", ssql.Replace("'", ""))
                                Dim dtLista As New DataTable
                                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                                If dtLista.Rows.Count > 0 Then
                                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                                Else
                                    Session("ListaPrecios") = "1"
                                End If
                            Catch ex3 As Exception
                                objDatos.fnLog("Cookie Zeyco ex 2", ex3.Message.Replace("'", ""))
                            End Try
                        End Try


                    End If


                End If





                Session("slpCode") = 0
                ssql = "SELECT CardName FROM OCRD where Cardcode=" & "'" & Session("Cliente") & "'"
                Dim dtcliente2 As New DataTable
                dtcliente2 = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtcliente2.Rows.Count > 0 Then
                    Session("RazonSocial") = dtcliente2.Rows(0)(0)
                End If
                Session("IsB2B") = "SI"


            End If




            'If Request.Cookies("RazonSocial").Value IsNot Nothing Then
            '    Session("RazonSocial") = Request.Cookies("RazonSocial").Value
            'End If

            'If Request.Cookies("Cliente").Value IsNot Nothing Then
            '    Session("Cliente") = Request.Cookies("Cliente").Value
            'End If

            'If Request.Cookies("slpcode").Value IsNot Nothing Then
            '    Session("slpCode") = Request.Cookies("slpcode").Value
            'End If
            'If Request.Cookies("ListaPrecios").Value IsNot Nothing Then
            '    Session("ListaPrecios") = Request.Cookies("ListaPrecios").Value
            'End If

            'If Request.Cookies("Carrito").Value IsNot Nothing Then
            '    Try

            '        If Session("Partidas").count = 0 Then
            '            ''Leemos la variable de sesion y vamos cargando las partidas, esta inicializada la variable de sesion
            '            fnCargaCookieCarrito(Request.Cookies("Carrito").Value)
            '        End If
            '    Catch ex As Exception
            '        ''Tronó, esta vacia y no estaba inicializada
            '        fnCargaCookieCarrito(Request.Cookies("Carrito").Value)
            '    End Try
            'End If

            'If Request.Cookies("UserTienda").Value IsNot Nothing Then
            '    Session("UserTienda") = Request.Cookies("ListaPrecios").Value
            'End If

        Catch ex As Exception

        End Try


        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)
        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("STOP") And Session("RazonSocial") = "" Then
                ''Se elimina para que en Delta para que puedan comprar como invitado
                ' Response.Redirect("loginB2B.aspx")
                ' Exit Sub
            End If
        End If
        ssql = "SELECT ISNULL(cvLigaB2B,'') FROM config.parametrizaciones "
        Dim dtLigaB2B As New DataTable
        dtLigaB2B = objDatos.fnEjecutarConsulta(ssql)
        If dtLigaB2B.Rows.Count > 0 Then
            If dtLigaB2B.Rows(0)(0) <> "" And Session("RazonSocial") = "" Then
                Response.Redirect(dtLigaB2B.Rows(0)(0))
                Exit Sub
            End If
        End If

        If objDatos.fnObtenerCliente.ToUpper.Contains("BOSS") Then
            If Now.Date.Year = 2020 And Now.Month = 12 Then
                Response.Redirect("http://www.google.com")
            End If
            If Now.Date.Year > 2020 Then
                Response.Redirect("http://www.google.com")
            End If
        End If


        'objDatos.fnlog("MasterPage", "Newsletter")

        'If Session("RazonSocial") <> "" Then
        '    lblRazonSocial.Text = Session("NombreuserTienda") & " - Cliente:" & Session("RazonSocial")
        'End If
        If CInt(Session("slpCode")) = 0 And Session("Cliente") = "" Then

        End If

        '
        'fnCargaSubCategorias()
        If CInt(Session("slpCode")) <> 0 Or Session("UserB2C") <> "" Then
            pnlListaDeseos.Visible = False
            pnlLogin.Visible = False
            pnlUsuarioLogin.Visible = True
            pnlCliente.Visible = True
            If Session("RazonSocial") = "" And Session("NombreuserTienda") <> "" And Session("UserB2C") = "" Then
                If Session("RegistraNuevo") = "" Then
                    ''Un usuario que no seleccionó cliente. Se va a elegir-cliente
                    Response.Redirect("elegir-cliente.aspx")
                End If

            End If
            lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
            lblRazonSocial.Text = Session("NombreuserTienda")

            If Session("Cliente") <> "" Then
                If Session("Generalescliente") <> "" Then
                    lblRazonSocial.Text = lblRazonSocial.Text & "<BR/>" & Session("Generalescliente")
                End If
            End If


            ' btnCerrarSesion.Visible = True
            'pnlOpciones.Visible = True

            ' fnCargaMenuUser()
            If Session("UserB2C") = "" Then
                fnCargaMenuB2B()
                fnPlantillasVendedor()
                pnlProcesarPago.Visible = False

            Else
                ''Por el momento ocultamos siempre el botón de procesar pago
                ' pnlProcesarPago.Visible = False

            End If

        Else
            If Session("Cliente") = "" Then
                ssql = "SELECT ciIdListaPrecios FROM config.DatosCliente "
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsulta(ssql)
                Session("ListaPrecios") = dtLista.Rows(0)(0)
                pnlLogin.Visible = True
            End If

        End If




        '   
        If Session("Cliente") <> "" And Session("UserB2C") = "" Then
            fnCargaMenuB2B()
            lblUsuario.Text = Session("RazonSocial")
            pnlListaDeseos.Visible = False
            pnlLogin.Visible = False
            pnlUsuarioLogin.Visible = True
            pnlCliente.Visible = True
            pnlelegir.Visible = False
            pnlProcesarPago.Visible = False
            If CInt(Session("slpCode")) <> 0 Then
                lblRazonSocial.Text = Session("RazonSocial")
                lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
                pnlelegir.Visible = True
                fnPlantillasVendedor()
            End If
        End If
        If CInt(Session("slpCode")) <> 0 Then
            lblRazonSocial.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
            lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
            If Session("Generalescliente") <> "" Then
                lblRazonSocial.Text = lblRazonSocial.Text & "<BR/>" & Session("Generalescliente")
            End If
        End If

        fnCargaCarrito()


        ''revisamos si se debemostrar una leyenda en el header
        ssql = "SELECt ISNULL(cvLeyendaMain,'NO') as cvLeyendaMain FROM config .Parametrizaciones "
        Dim dtLeyendaMain As New DataTable
        dtLeyendaMain = objDatos.fnEjecutarConsulta(ssql)
        If dtLeyendaMain.Rows.Count > 0 Then
            If dtLeyendaMain.Rows(0)(0) <> "" Then
                lblLeyendaMain.Visible = True
                If objDatos.fnObtenerCliente.ToUpper.Contains("STOP CAT") Then
                    If Session("RazonSocial") = "" Then
                        lblLeyendaMain.Text = dtLeyendaMain.Rows(0)(0)
                    End If

                Else
                    lblLeyendaMain.Text = dtLeyendaMain.Rows(0)(0)
                End If

            End If
        End If

        'objDatos.fnlog("MasterPage", "carga carrito")

        ''revisamos si se debe encender el google translator
        ssql = "SELECt ISNULL(cvTraductor,'NO') as Traductor FROM config .Parametrizaciones "
        Dim dtTituloProm As New DataTable
        dtTituloProm = objDatos.fnEjecutarConsulta(ssql)
        If dtTituloProm.Rows.Count > 0 Then
            If dtTituloProm.Rows(0)(0) = "SI" Then
                pnlTrans.Visible = True
            End If
        End If
        'objDatos.fnlog("MasterPage", "Traductor")
        ''revisamos si debe de llevar chat
        ssql = "SELECt ISNULL(cvIncluyeChat,'NO') as Chat,ISNULL(cvContenidoChat,'') as Contenido FROM config .Parametrizaciones "
        Dim dtChat As New DataTable
        dtChat = objDatos.fnEjecutarConsulta(ssql)
        If dtChat.Rows.Count > 0 Then
            If dtChat.Rows(0)(0) = "SI" Then
                Dim scriptText As String = ""
                scriptText &= dtChat.Rows(0)(1)
                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "CounterScript", scriptText, True)
            End If
        End If
        'objDatos.fnlog("MasterPage", "chat")

        ''Revisamos el copyright
        ssql = "SELECT ISNULL(cvCopyright,'') from config.Parametrizaciones "
        Dim dtCopy As New DataTable
        dtCopy = objDatos.fnEjecutarConsulta(ssql)
        If dtCopy.Rows.Count > 0 Then
            lblCopy.Text = dtCopy.Rows(0)(0)
        End If

        If Session("UserB2C") <> "" Then
            pnlelegir.Visible = False
            pnlB2C.Visible = True
            lblRazonSocial.Visible = False
            lblusuarioB2cNombre.Text = Session("NombreuserTienda")
            '    pnlListaDeseos.Visible = True
        End If

        'objDatos.fnlog("MasterPage", "copyright")

        fnPluginWhatsApp()

        Session("AgregaCarrito") = "NO"
        objDatos.fnLog("MasterPage", "Carga")
        fnCargaMenuHeader()
        objDatos.fnLog("MasterPage", "Menu Header")
        fnCargaMenuFooter()
        objDatos.fnLog("MasterPage", "Menu footer")
        fnCargaMenuResponsive()
        objDatos.fnLog("MasterPage", "Menu responsive")
        fnCargaRedesSociales()
        objDatos.fnLog("MasterPage", "Redes sociales")

        fnCargaMenuFooterResponsive()
        objDatos.fnLog("MasterPage", "Menu footer responsive")
        'fnCargaCategoriasv3()
        fnNewsletter()
        objDatos.fnLog("MasterPage", "Tipo de cambio")
        ''Revisamos si debe mostrar el tipo de cambio
        ssql = "SELECT ISNULL(cvMuestraTipoCambio,'NO') from config.Parametrizaciones "
        Dim dtMuestraTC As New DataTable
        dtMuestraTC = objDatos.fnEjecutarConsulta(ssql)
        If dtMuestraTC.Rows.Count > 0 Then
            If dtMuestraTC.Rows(0)(0) = "SI" Then
                objDatos.fnLog("MasterPage", "Tipo de cambio SI")
                ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
                Dim dtTC As New DataTable
                dtTC = objDatos.fnEjecutarConsultaSAP(ssql)
                objDatos.fnLog("MasterPage", "Tipo de cambio: " & ssql.Replace("'", ""))
                If dtTC.Rows.Count > 0 Then
                    If dtcliente.Rows.Count > 0 Then
                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("SEGURIT") Then
                            lblTC.Text = "Tasa de cambio: " & CDbl(dtTC.Rows(0)(0)).ToString("###,###,###.##0")
                        Else
                            lblTC.Text = "Tipo de cambio: " & CDbl(dtTC.Rows(0)(0)).ToString("###,###,###.####00")
                        End If
                    End If

                    Session("TC") = dtTC.Rows(0)(0)
                        End If
                    End If
        End If
        objDatos.fnLog("MasterPage", "Tipo de cambio")


        If Not IsPostBack Then


        End If

        If Session("errDescuento") <> "" Then
            objDatos.Mensaje(Session("errDescuento"), Me.Page)
            Session("errDescuento") = ""
        End If

        If Session("ErrorExistencia") <> "" Then
            objDatos.Mensaje(Session("ErrorExistencia"), Me.Page)
            Session("ErrorExistencia") = ""
        End If

        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("SEGURIT") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("STOP CAT") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("PMK") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("AIO") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("SUJEAU") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("MANIJAU") Then
                pnlListaDeseos.Visible = False
                pnlFavoritos.Visible = False
                pnlHeart.Visible = False
                pnlLogin.Visible = False
            End If



        End If

        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("SUJEAU") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("MANIJAU") Then
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Then
                pnlCrearCuenta.Visible = False
            End If

            pnlListaDeseos.Visible = False
            pnlFavoritos.Visible = False
            pnlHeart.Visible = False
            If Session("Cliente") <> "" And Session("UserB2C") = "" Then
                pnlLogin.Visible = False

            Else
                pnlLogin.Visible = True
            End If

        End If

        objDatos.fnLog("Zeyco master 2 IsB2B", Session("IsB2B"))
        objDatos.fnLog("Zeyco master 2 RazonSocial", Session("RazonSocial"))
        objDatos.fnLog("Zeyco master 2 Cliente", Session("Cliente"))

    End Sub
    Public Sub fnCargaCookieCarrito(cookieValue As String)
        Dim sLineas As String() = cookieValue.Split("@")
        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
        For Each linea In sLineas
            Dim sPartida As String() = linea.Split("-")

            Dim partida As New Cls_Pedido.Partidas
                'partida.ItemCode & "-" & partida.Cantidad & "-" & partida.Descuento & "-" & partida.Linea & "@"
                If sPartida(0) <> "BORRAR" Then
                    partida.ItemCode = sPartida(0)
                    partida.Cantidad = CDbl(sPartida(1))
                    partida.Descuento = CDbl(sPartida(2))
                partida.Linea = sPartida(3)
                ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & sPartida(0) & "'")
                objDatos.fnLog("Carga itemname:", ssql.Replace("'", ""))
                Dim dtItemName As New DataTable
                dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtItemName.Rows.Count = 0 Then
                    partida.ItemName = "ND"
                Else
                    partida.ItemName = dtItemName.Rows(0)(0)

                End If
                Session("Partidas").add(partida)

            End If



        Next

    End Sub


    Public Sub fnPluginWhatsApp()
        ssql = "SELECt ISNULL(cvTelefonoWhatsApp,'') as NumWhats FROM config .Parametrizaciones "
        Dim dtTituloProm As New DataTable
        dtTituloProm = objDatos.fnEjecutarConsulta(ssql)
        If dtTituloProm.Rows.Count > 0 Then
            If dtTituloProm.Rows(0)(0) <> "" Then

                Dim sHtml As String = ""


                sHtml = sHtml & "    <a href='" & dtTituloProm.Rows(0)(0) & "' "
                sHtml = sHtml & "     style='position: fixed; width: 60px;height: 60px;bottom: 40px;right: 40px;background-color: #25d366;color: #FFF;border-radius: 50px;text-align: center;font-size: 30px;box-shadow: 2px 2px 3px #999;z-index: 100;' target='_blank'>"
                sHtml = sHtml & "     <i class=''><img class='img-responsive' src='images/whatsapp.png'></i>"
                sHtml = sHtml & "    </a>"



                Dim literal As New LiteralControl(sHtml)
                pnlWhatsApp.Controls.Clear()
                pnlWhatsApp.Controls.Add(literal)
            End If
        End If
    End Sub

    Public Sub fnNewsletter()
        ''Obtenemos el titulo de promociones
        ssql = "SELECt ISNULL(cvTieneNewLetter,'NO') as Newsletter,cvContenidoNewLetter FROM config .Parametrizaciones "
        Dim dtTituloProm As New DataTable
        dtTituloProm = objDatos.fnEjecutarConsulta(ssql)
        If dtTituloProm.Rows.Count > 0 Then
            If dtTituloProm.Rows(0)(0) = "SI" Then
                pnlnews.Visible = True
                Dim literal As New LiteralControl(dtTituloProm.Rows(0)(1))
                pnlNewsContent.Controls.Clear()
                pnlNewsContent.Controls.Add(literal)

            Else
                pnlnews.Visible = False
            End If
        End If

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
        ' Session("Moneda") = sMoneda
        Return sMoneda
    End Function
    Public Sub fnCargaCarrito()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Try
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Menu' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            Dim iCartContent As Int16 = 0
            Dim iContador As Int16 = 0

            Dim sHtmlImagen As String = ""
            Dim sHtmlPRecio As String = ""
            Dim sHtmlCantidad As String = ""
            Dim sHtmlAtributos As String = ""
            Dim sCampos As String = ""
            Dim dtTallasColores As New DataTable
            Dim sTallaColor As String = "NO"

            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If
            End If





            objDatos.fnLog("Zeyco carga carrito", Session("Cliente"))

            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                objDatos.fnLog("Zeyco carga entra", "")
                If Partida.ItemCode <> "BORRAR" Then
                    iContador = iContador + 1
                    sHtmlBanner = sHtmlBanner & " <li> "
                    sHtmlBanner = sHtmlBanner & "  <div class='div-sdiviped'>"
                    sHtmlBanner = sHtmlBanner & "   <div class='row-cart'>"
                    objDatos.fnLog("Carga carrito", Partida.ItemCode)
                    Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)
                    If dtCamposPlantilla.Rows.Count > 0 Then

                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            Dim precio As Double = 0
                            If sTallaColor = "SI" Then
                                objDatos.fnLog("Carga carrito Talla color SI ", Partida.Generico)



                                Dim dtExiste As New DataTable
                                Dim ssqlExiste As String = ssql.Replace("[%0]", "'" & Partida.Generico & "'")
                                dtExiste = objDatos.fnEjecutarConsultaSAP(ssqlExiste)
                                If dtExiste.Rows.Count = 0 Then

                                    ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                Else
                                    ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                                End If
                                objDatos.fnLog("Carga carrito Talla color SI ", ssql.Replace("'", ""))
                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")


                            End If



                            '  'objDatos.fnlog("CatGeneral", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
                            ' 'objDatos.fnlog("CatGeneral", ssql.Replace("'", ""))
                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlImagen = sHtmlImagen & " <div class='image-cart text-center'> <a href='producto-interior.aspx?Code=" & Partida.ItemCode & "'>"

                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        'objDatos.fnlog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            If dtFoto.Rows(0)(0) <> "" Then
                                                sHtmlImagen = sHtmlImagen & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                            Else
                                                sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                            End If

                                        Else
                                            sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                        End If
                                    Else
                                        Dim iband As Int16 = 0
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & ".jpg") Then
                                            sHtmlImagen = sHtmlImagen & " <img src=" & "'" & "images/products/" & Partida.ItemCode & ".jpg" & "'  alt='productos' title='productos' class='img-thumbnail'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-1.jpg") And iband = 0 Then
                                            sHtmlImagen = sHtmlImagen & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-1.jpg" & "'  alt='productos' title='productos' class='img-thumbnail'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-2.jpg") And iband = 0 Then
                                            sHtmlImagen = sHtmlImagen & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-2.jpg" & "'  alt='productos' title='productos' class='img-thumbnail'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-3.jpg") And iband = 0 Then
                                            sHtmlImagen = sHtmlImagen & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-3.jpg" & "'  alt='productos' title='productos' class='img-thumbnail'>"
                                            iband = 1
                                        End If

                                        If iband = 0 Then
                                            If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo"))) Then
                                                sHtmlImagen = sHtmlImagen & "   <img src='images/products/" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                            Else
                                                sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                            End If

                                        End If
                                        ' sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                    End If

                                Else
                                    sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                End If


                                sHtmlImagen = sHtmlImagen & "</a></div>"
                            Else
                                If iCartContent = 0 Then
                                    iCartContent = 1
                                    ' sHtmlBanner = sHtmlBanner & "<div class='cart-content'>"
                                    ' sHtmlBanner = sHtmlBanner & " <div class='cart-button text-center'>"
                                End If
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then
                                    '     sHtmlAtributos = sHtmlAtributos & " <div class='product-name text-left'> <a href='producto-interior.aspx'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</a></div>"
                                    If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                Else
                                    Dim dPrecioActual As Double
                                    If Partida.Precio = 0 Then

                                        If CInt(Session("slpCode")) <> 0 Then

                                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                                        Else
                                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                                        End If

                                        If Session("Cliente") <> "" Then
                                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                                        End If
                                    Else
                                        dPrecioActual = Partida.Precio
                                    End If

                                    sHtmlCantidad = sHtmlCantidad & " <strong class='text-right'>x - " & Partida.Cantidad & " </strong> "
                                    sHtmlPRecio = sHtmlPRecio & " <span class='cart-price text-right'> " & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & Partida.Moneda & "</span>"
                                End If



                            End If
                        Next

                        'sHtmlBanner = sHtmlBanner & "</div>"
                        'sHtmlBanner = sHtmlBanner & "</div>"

                        iCartContent = 0
                    End If
                    sHtmlAtributos = "<div class='product-name text-left'> <a href='producto-interior.aspx'>" & sCampos & "</a></div>"

                    sHtmlBanner = sHtmlBanner & sHtmlImagen
                    sHtmlBanner = sHtmlBanner & "<div class='cart-content'>"
                    sHtmlBanner = sHtmlBanner & sHtmlAtributos
                    sHtmlBanner = sHtmlBanner & sHtmlCantidad
                    sHtmlBanner = sHtmlBanner & sHtmlPRecio
                    sHtmlBanner = sHtmlBanner & " <div class='cart-button text-center'>"
                    sHtmlBanner = sHtmlBanner & "  <button type='button' onclick=fnClickEliminar('" & Partida.ItemCode & "'); title='Eliminar' class='btn cancel-p'>"
                    sHtmlBanner = sHtmlBanner & "    <img src='img/cancel.svg' class='img-responsive'>"
                    sHtmlBanner = sHtmlBanner & "  </button>"
                    sHtmlBanner = sHtmlBanner & " </div>"

                    sHtmlBanner = sHtmlBanner & "</div>"

                    sHtmlBanner = sHtmlBanner & " </div></div></li> "
                End If

                sCampos = ""
                sHtmlImagen = ""
                sHtmlAtributos = ""
                sHtmlCantidad = ""
                sHtmlPRecio = ""

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            If iContador > 0 Then
                pnlContadorCarrito.Visible = True
            Else
                pnlContadorCarrito.Visible = False
            End If
            If iContador = 0 Then
                lblItemsCarrito.Text = ""

                lblItemsCarritoMod.Text = ""
            Else
                lblItemsCarrito.Text = iContador

                lblItemsCarritoMod.Text = iContador
            End If

        Catch ex As Exception
            objDatos.fnLog("Error en MP", ex.Message.Replace("'", ""))
            '   Response.Redirect("index.aspx")
        End Try


        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlCarrito.Controls.Clear()
        pnlCarrito.Controls.Add(literal)

    End Sub
    <WebMethod>
    Public Shared Function EliminaCarrito(Articulo As String) As String

        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones


        For Each partida In HttpContext.Current.Session("Partidas")
            If partida.ItemCode = Articulo Then
                partida.ItemCode = "BORRAR"
            End If
        Next

        Try

        Catch ex As Exception
            '      objDatos.Mensaje(ex.Message, MyPage.page)
        End Try
        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    <WebMethod>
    Public Shared Function EliminaDeseo(Articulo As String) As String

        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones


        For Each partida In HttpContext.Current.Session("WishList")
            If partida.ItemCode = Articulo Then
                partida.ItemCode = "BORRAR"
            End If
        Next

        Try

        Catch ex As Exception
            '      objDatos.Mensaje(ex.Message, MyPage.page)
        End Try
        Dim result As String = "Entró:" & Articulo

        Return result
    End Function


    Public Sub fnCargaRedesSociales()
        ssql = "Select * from config.RedesSociales where cvEstatus='ACTIVO'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = "  "


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtMenus.Rows(i)("cvLiga") & "' class='' target='_blank'><i class='" & dtMenus.Rows(i)("cvRedSocial") & "' aria-hidden='true'></i></a></li>"
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlRedes.Controls.Clear()
        pnlRedes.Controls.Add(literal)
    End Sub
    Public Sub fnCargaMenuFooter()
        Dim sHtmlEncabezado As String
        Dim sHtmlMenu As String = ""
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)
        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("HAWK") Then
                fnCargaMenuFooterHawk()
            Else
                If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("AUTOPARTES IMPORTADAS DE OCCIDENTE") Then

                    fnCargaMenuFooterAIO()
                Else
                    If CStr(objDatos.fnObtenerCliente().ToUpper.Contains("PMK")) Then
                        fnCargaMenuFooterPMK()
                    Else
                        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
                        Dim dtMenus As New DataTable
                        dtMenus = objDatos.fnEjecutarConsulta(ssql)


                        sHtmlEncabezado = " <div class='col-min-12 col-xs-6 col-md-4'> "



                        sHtmlMenu = sHtmlMenu & "<div Class='col-min-12 col-xs-6 col-md-6'> "
                        sHtmlMenu = sHtmlMenu & "<span Class='tit'><img src = 'img/header/logo.png' alt='Logotipo-marca' Class='img-responsive'></span>"

                        sHtmlMenu = sHtmlMenu & " </div>"

                        ' sHtmlMenu = sHtmlMenu & "<div Class='cont-desd-fotter' style='text-align: left;font-size: 16px;font-weight: 300;'></div></div> "

                        sHtmlMenu = sHtmlMenu & "<div Class='col-min-12 col-xs-6 col-md-6'> "
                        For i = 0 To dtMenus.Rows.Count - 1 Step 1
                            sHtmlMenu = sHtmlMenu & " <div class='col-min-12 col-xs-6 col-md-3'> "
                            sHtmlMenu = sHtmlMenu & "<span class='tit'>" & dtMenus.Rows(i)("cvSubTipo") & "</span>"
                            sHtmlMenu = sHtmlMenu & "<div class='cont-desd-fotter'> "
                            sHtmlMenu = sHtmlMenu & " <ul class='menu-generico'> "

                            ssql = "SELECT cvNombre,ISNULL(cvLink,'') as cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
                            Dim dtSubMenu As New DataTable
                            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
                            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                                Else
                                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                                    Else
                                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "'></a></li>"
                                        Else
                                            If dtSubMenu.Rows(x)("cvLink") = "" Then
                                                sHtmlMenu = sHtmlMenu & "<li><div>" & dtSubMenu.Rows(x)("cvNombre") & "</div></li>"
                                            Else
                                                sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                                            End If

                                        End If

                                    End If
                                End If


                            Next
                            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
                        Next
                        sHtmlMenu = sHtmlMenu & " </div>"

                        Dim literal As New LiteralControl(sHtmlMenu)
                        pnlFooter.Controls.Clear()
                        pnlFooter.Controls.Add(literal)
                    End If


                End If


            End If
        End If






    End Sub

    Public Sub fnCargaMenuFooterHawk()
        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        Dim sHtmlEncabezado As String
        '      sHtmlMenu = " <div class='col-xs-12 col-sm-6' style='background-color: #D1CAAE;'> "



        sHtmlMenu = sHtmlMenu & "<div Class='col-min-12 col-xs-6 col-md-6'> "
        sHtmlMenu = sHtmlMenu & "<span Class='tit' style='position: relative; overflow: hidden;'><img src = 'img/header/logo.png' alt='Logotipo-marca' class='img-responsive logo-fotter'>"
        ' sHtmlMenu = sHtmlMenu & "<img role='presentation' alt='' src='img/header/logo.png' class='logo-fotter' style='position: absolute; top: 0px; left: 0px; opacity: 0; width: 59px; height: 71px; border: none; max-width: none; max-height: none;'>"
        sHtmlMenu = sHtmlMenu & "</span><div Class='cont-desd-fotter' style='text-align: left;font-size: 16px;font-weight: 300;'></div> "
        sHtmlMenu = sHtmlMenu & "</div>"

        sHtmlMenu = sHtmlMenu & " <div class='col-min-12 col-xs-6 col-md-6'> "

        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <div class='col-min-12 col-xs-6 col-md-3'> "
            sHtmlMenu = sHtmlMenu & "<span class='tit'>" & dtMenus.Rows(i)("cvSubTipo") & "</span>"
            sHtmlMenu = sHtmlMenu & "<div class='cont-desd-fotter'> "
            sHtmlMenu = sHtmlMenu & " <ul class='menu-generico'> "

            ssql = "SELECT cvNombre,ISNULL(cvLink,'') as cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "'></a></li>"
                        Else
                            If dtSubMenu.Rows(x)("cvLink") = "" Then
                                sHtmlMenu = sHtmlMenu & "<li><div>" & dtSubMenu.Rows(x)("cvNombre") & "</div></li>"
                            Else
                                sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                            End If

                        End If

                    End If
                End If


            Next
            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
        Next
        ' sHtmlMenu = sHtmlMenu & " </div>"
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFooter.Controls.Clear()
        pnlFooter.Controls.Add(literal)




    End Sub

    Public Sub fnCargaMenuFooterAIO()
        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        Dim sHtmlEncabezado As String
        '      sHtmlMenu = " <div class='col-xs-12 col-sm-6' style='background-color: #D1CAAE;'> "



        sHtmlMenu = sHtmlMenu & "<div Class='col-min-12 col-xs-6 col-md-4'> "
        sHtmlMenu = sHtmlMenu & "<span Class='tit' style='position: relative; overflow: hidden;'><img src = 'img/header/logo.png' alt='Logotipo-marca' class='img-responsive logo-fotter'>"
        ' sHtmlMenu = sHtmlMenu & "<img role='presentation' alt='' src='img/header/logo.png' class='logo-fotter' style='position: absolute; top: 0px; left: 0px; opacity: 0; width: 59px; height: 71px; border: none; max-width: none; max-height: none;'>"
        sHtmlMenu = sHtmlMenu & "</span><div Class='cont-desd-fotter' style='text-align: left;font-size: 16px;font-weight: 300;'></div> "
        sHtmlMenu = sHtmlMenu & "</div>"

        '  sHtmlMenu = sHtmlMenu & " <div class='col-min-12'> "

        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <div class='col-min-12 col-xs-6 col-md-4'> "
            sHtmlMenu = sHtmlMenu & "<span class='tit'>" & dtMenus.Rows(i)("cvSubTipo") & "</span>"
            sHtmlMenu = sHtmlMenu & "<div class='cont-desd-fotter'> "
            sHtmlMenu = sHtmlMenu & " <ul class='menu-generico'> "

            ssql = "SELECT cvNombre,ISNULL(cvLink,'') as cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "' ></a></li>"
                        Else
                            If dtSubMenu.Rows(x)("cvLink") = "" Then
                                sHtmlMenu = sHtmlMenu & "<li><div>" & dtSubMenu.Rows(x)("cvNombre") & "</div></li>"
                            Else
                                sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                            End If

                        End If

                    End If
                End If


            Next
            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
        Next
        ' sHtmlMenu = sHtmlMenu & " </div>"
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFooter.Controls.Clear()
        pnlFooter.Controls.Add(literal)




    End Sub

    Public Sub fnCargaMenuFooterPMK()
        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        Dim sHtmlEncabezado As String
        '      sHtmlMenu = " <div class='col-xs-12 col-sm-6' style='background-color: #D1CAAE;'> "



        sHtmlMenu = sHtmlMenu & "<div Class='col-min-12 col-xs-6 col-md-3'> "
        sHtmlMenu = sHtmlMenu & "<span Class='tit' style='position: relative; overflow: hidden;'><img src = 'img/header/logo.png' alt='Logotipo-marca' class='img-responsive logo-fotter'>"
        ' sHtmlMenu = sHtmlMenu & "<img role='presentation' alt='' src='img/header/logo.png' class='logo-fotter' style='position: absolute; top: 0px; left: 0px; opacity: 0; width: 59px; height: 71px; border: none; max-width: none; max-height: none;'>"
        sHtmlMenu = sHtmlMenu & "</span><div Class='cont-desd-fotter' style='text-align: left;font-size: 16px;font-weight: 300;'></div> "
        sHtmlMenu = sHtmlMenu & "</div>"

        '  sHtmlMenu = sHtmlMenu & " <div class='col-min-12'> "

        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <div class='col-min-12 col-xs-6 col-md-3'> "
            sHtmlMenu = sHtmlMenu & "<span class='tit'>" & dtMenus.Rows(i)("cvSubTipo") & "</span>"
            sHtmlMenu = sHtmlMenu & "<div class='cont-desd-fotter'> "
            sHtmlMenu = sHtmlMenu & " <ul class='menu-generico'> "

            ssql = "SELECT cvNombre,ISNULL(cvLink,'') as cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "' class='img-responsive logo-footer'></a></li>"
                        Else
                            If dtSubMenu.Rows(x)("cvLink") = "" Then
                                sHtmlMenu = sHtmlMenu & "<li><div>" & dtSubMenu.Rows(x)("cvNombre") & "</div></li>"
                            Else
                                sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                            End If

                        End If

                    End If
                End If


            Next
            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
        Next
        ' sHtmlMenu = sHtmlMenu & " </div>"
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFooter.Controls.Clear()
        pnlFooter.Controls.Add(literal)




    End Sub

    Public Sub fnCargaMenuFooterResponsive()
        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = " <div class='panel-group' id='accordionfooter2' role='tablist' aria-multiselectable='true'> "


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & "<div class='panel'> "
            sHtmlMenu = sHtmlMenu & "<div class='panel-heading del-carret' role='tab' id='cthirex" & (i + 1) & "'>"
            sHtmlMenu = sHtmlMenu & "<h4 class='categoria'> "
            sHtmlMenu = sHtmlMenu & "<a role='button' data-toggle='collapse' data-parent='#accordionfooter2' href='#cthir" & (i + 1) & "' aria-expanded='true' aria-controls='cthir" & (i + 1) & "'>"
            sHtmlMenu = sHtmlMenu & dtMenus.Rows(i)("cvSubTipo") & " </a> </h4></div>"

            sHtmlMenu = sHtmlMenu & "<div id='cthir" & (i + 1) & "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='cthir" & (i + 1) & "'> "
            sHtmlMenu = sHtmlMenu & "  <div class='panel-body'>"
            sHtmlMenu = sHtmlMenu & "   <ul> "

            ssql = "SELECT cvNombre,cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "'></a></li>"
                        Else
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                        End If

                    End If
                End If


            Next
            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
            sHtmlMenu = sHtmlMenu & "</div>"
        Next


        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFooterResponsive.Controls.Clear()
        pnlFooterResponsive.Controls.Add(literal)
    End Sub


    Public Sub fnCargaMenuUser()
        ssql = "SELECT distinct cvSubTipo FROM Config.Menus where cvTipoMenu='Header' AND ISNULL(cvTipoDato,'') = 'User'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String = ""


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlEncabezado = " <li class='dropdown yamm-fw'><a href='#' data-toggle='dropdown' class='dropdown-toggle'>" & dtMenus.Rows(i)("cvSubTipo") & "<b class='caret'></b></a> "
            sHtmlEncabezado = sHtmlEncabezado & "<ul class='dropdown-menu'>"
            sHtmlEncabezado = sHtmlEncabezado & " <li> "
            sHtmlEncabezado = sHtmlEncabezado & "  <div class='yamm-content'> "
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "    <div  class='col-sm-12 no-padding'> "
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='col-xs-8'> "
            sHtmlEncabezado = sHtmlEncabezado & "        <ul class='nav nav-tabs tabs-left'> "



            ssql = "SELECT cvNombre,cvLink,ISNULL(cvTipoDato,'') as TipoDato FROM Config.Menus where cvTipoMenu='Header' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "'"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    End If
                End If


            Next
            ' sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
        Next

        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        sHtmlEncabezado = sHtmlEncabezado & " </ul></div></div></div></div> </li></ul></li>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlMenuUsuario.Controls.Clear()
        pnlMenuUsuario.Controls.Add(literal)
    End Sub


    Public Sub fnCargaCategoriasv3()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            '   sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-2'>"
            '  sHtmlEncabezado = sHtmlEncabezado & " <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 5
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    'sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' >" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    'sHtmlBanner = sHtmlBanner & "</li> "

                    If iContador = 0 Then
                        sHtmlBanner = sHtmlBanner & "<li class='active'><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                        sHtmlBanner = sHtmlBanner & "</li> "
                    Else
                        sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                        sHtmlBanner = sHtmlBanner & "</li> "
                    End If
                    iContador = iContador + 1
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '     sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Sub

    Public Function fnCargaCategoriasHTML(NombreCat As String) As String
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                If Session("RazonSocial") <> "" Then
                    ssql = objDatos.fnObtenerQuery("CategoriasB2B")
                Else
                    ssql = objDatos.fnObtenerQuery("Categorias")
                End If

                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(1))
                'objDatos.fnlog("Primer Cats", ssql.Replace("'", ""))
            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""

            If Session("Page") = "catalogo.aspx" Then
                sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle active' href='catalogo.aspx'>" & NombreCat & "<b class='caret'></b></a>"
            Else
                sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle' href='catalogo.aspx'>" & NombreCat & "<b class='caret'></b></a>"
            End If

            Dim sHTMLConHijos As String = ""

            sHtmlEncabezado = sHtmlEncabezado & "   <ul class='dropdown-menu' style='display: none;'>"
            sHtmlEncabezado = sHtmlEncabezado & "    <li>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div class='yamm-content'>"
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "       <div  class='col-sm-12 no-padding'>"
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SALAMA") Then
                sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-12'>"
            Else
                sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-6'>"
            End If

            sHtmlEncabezado = sHtmlEncabezado & "         <ul class='nav #SUBMENU#'>"
            Dim iContador As Int16 = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                Dim sValorPintar As String = ""
                ' 'objDatos.fnlog("Cats", "entra")
                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objDatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                    'objDatos.fnlog("Cats", ssql.Replace("'", ""))
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                '  'objDatos.fnlog("Cats 1 ", dtCategorias.Rows(i)(0))

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(1) & "'>" & sValorPintar & "</span><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> "
                    'sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(1) & "'>" & sValorPintar & "</span> SINHIJOS" & iContador
                Else
                    sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'>" & sValorPintar & "</span><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> "
                    'sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'>" & sValorPintar & "</span> SINHIJOS" & iContador
                End If

                sHtmlBanner = sHtmlBanner & "</li> "
                iContador = iContador + 1


            Next

            ''Aqui las subcategorias
            ''Categorias Especiales
            sHtmlBanner = sHtmlBanner & fnCategoriasEspecial()
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"

            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SALAMA") Then
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-12'>" 'El que agregué
            Else
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-6'>" 'El que agregué
            End If

            sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-content'>" 'El que agregué


            iContador = 1
            Dim sCampoPrimerNivel As String = ""
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='Cat-" & (iContador) & "'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-12'>" 'class='col-xs-4'
                sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"
                sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-group' id='accordion-bebidas' role='tablist' aria-multiselectable='true'>"

                ''Nos traemos el nivel 2 de la categoría seleccionada
                If Session("RazonSocial") <> "" Then
                    ssql = objDatos.fnObtenerQuery("Categorias-detB2B")
                Else
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                End If
                ssql = objDatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    If objDatos.fnObtenerDBMS = "HANA" Then

                        ssql = ssql.Replace("[%1]", "T0.""" & dtPrimerNivel.Rows(0)("cvCampoSAP") & """=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    End If

                    sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If

                Dim iExisteTercerNivel As Int16 = 0
                Dim sQueryTercer As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =3"
                Dim dtTercerNivel As New DataTable
                dtTercerNivel = objDatos.fnEjecutarConsulta(sQueryTercer)
                If dtTercerNivel.Rows.Count > 0 Then
                    iExisteTercerNivel = 1
                End If

                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    sHtmlEncabezado = sHtmlEncabezado.Replace("#SUBMENU#", "nav-tabs tabs-left")

                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    'objDatos.fnlog("Categorias_det", ssql.Replace("'", ""))
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""

                    Dim sValorPintar As String = ""


                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & iContador, "<a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> ")
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            If CStr(dtSegundoNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                If Session("RazonSocial") <> "" Then
                                    ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2B2B")
                                Else
                                    ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2")
                                End If

                                ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                                Dim dtValor As New DataTable
                                dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                ' 'objDatos.fnlog("CampoUsuarioNiv2", ssql.Replace("'", ""))
                                If dtValor.Rows.Count > 0 Then
                                    sValorPintar = dtValor.Rows(0)(0)
                                Else
                                    sValorPintar = dtSubCategoria.Rows(x)(0)
                                End If
                            Else
                                sValorPintar = dtSubCategoria.Rows(x)(0)
                            End If
                            sHtmlBanner = sHtmlBanner & "<div class='panel panel-default'>"
                            sHtmlBanner = sHtmlBanner & " <div class='panel-heading' role='tab' id='heading-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "'>"
                            sHtmlBanner = sHtmlBanner & "  <h4 class='panel-title'> "

                            sHtmlBanner = sHtmlBanner & "   <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span><a role='button' data-toggle='collapse' data-link='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "' data-parent='#accordion-bebidas' href='#collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "' aria-expanded='true' aria-controls='collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "'>"
                            sHtmlBanner = sHtmlBanner & ""
                            sHtmlBanner = sHtmlBanner & "   </a>"
                            sHtmlBanner = sHtmlBanner & "  </h4>"
                            sHtmlBanner = sHtmlBanner & " </div>"

                            If iExisteTercerNivel = 0 Then
                                sHtmlBanner = sHtmlBanner & " </div>" 'Ya solamente cerramos el div
                            Else
                                If Session("RazonSocial") <> "" Then
                                    sQueryTercer = objDatos.fnObtenerQuery("Categorias-TerceroB2B")
                                Else
                                    sQueryTercer = objDatos.fnObtenerQuery("Categorias-Tercero")
                                End If

                                If dtTercerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                                    sQueryTercer = sQueryTercer.Replace("[%0]", "ItmsGrpNam")
                                Else
                                    sQueryTercer = sQueryTercer.Replace("[%0]", "ISNULL(T0." & dtTercerNivel.Rows(0)("cvCampoSAP") & ",'')")
                                    sQueryTercer = sQueryTercer.Replace("[%1]", sCampoPrimerNivel)

                                End If
                                sQueryTercer = sQueryTercer.Replace("[%2]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtSubCategoria.Rows(x)(0) & "'")
                                '    'objDatos.fnlog("Query tercer nivel: " & sLinkPrimerNivel & " " & dtSubCategoria.Rows(x)(0), sQueryTercer.Replace("'", ""))


                                Dim dtTercer As New DataTable
                                dtTercer = objDatos.fnEjecutarConsultaSAP(sQueryTercer)


                                ''Preparamos la estructura para el tercer nivel

                                sHtmlBanner = sHtmlBanner & " <div id='collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='heading-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "'> "
                                sHtmlBanner = sHtmlBanner & " <div class='panel-body'> "

                                For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                    sValorPintar = ""
                                    If CStr(dtTercerNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then

                                        If Session("RazonSocial") <> "" Then
                                            ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv3B2B")
                                        Else

                                            ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv3")
                                        End If

                                        ssql = ssql.Replace("[%0]", dtTercer.Rows(y)(0))
                                        Dim dtValor As New DataTable
                                        dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtValor.Rows.Count > 0 Then
                                            sValorPintar = dtValor.Rows(0)(0)
                                        End If
                                    Else
                                        sValorPintar = dtTercer.Rows(y)(0)
                                    End If

                                    If sValorPintar <> "" Then

                                        sHtmlBanner = sHtmlBanner & "  <a href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a>"

                                    End If
                                Next

                                sHtmlBanner = sHtmlBanner & "  </div>" 'Panel-body
                                sHtmlBanner = sHtmlBanner & " </div> " ' Div collapse
                                sHtmlBanner = sHtmlBanner & "</div> " ' Div del nivel 2

                            End If
                            ' sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "'> " & sValorPintar & "</a></li>"
                        End If

                    Next



                Else

                    sHtmlEncabezado = sHtmlEncabezado.Replace("#SUBMENU#", "")
                End If

                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                iContador = iContador + 1
                'sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                sHtmlEncabezado = sHtmlEncabezado & "</div></ul></div></div>"


            Next
            Dim iValor As Int16 = 99
            For a = 1 To 99 Step 1

                sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & iValor, "")
                iValor = iValor - 1
                'For b = 0 To 9 Step 1
                '    sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & a & b, "")
                'Next

                'sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & a, "")


            Next

            'For b = 0 To 9 Step 1
            '    sHtmlEncabezado = sHtmlEncabezado.Replace(b, "")
            'Next





            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué





            sHtmlEncabezado = sHtmlEncabezado & " <div class='clearfix'></div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div></div></li></ul></li>"





            Return sHtmlEncabezado

            'Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Function

    Public Function fnCategoriasEspecial() As String
        Dim sHTML As String = ""
        ssql = "select cvDescripcion ,cvCampoSAP,cvQuery  from config.CategoriasEsp where ciEstatus =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            For i = 0 To dtPrimerNivel.Rows.Count - 1 Step 1
                If CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper = "REBAJAS" Then
                    sHTML = sHTML & "<li> <span class='link-subcategoria' style='color:red;font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'>" & CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper.Replace("PLUS", "+") & "</span><a href='#CatEsp-" & (i + 1) & "' data-link='catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'  data-toggle='tab' aria-expanded='false'>" & "</a> </li>"
                Else

                    sHTML = sHTML & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'>" & CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper.Replace("PLUS", "<span style='font-size:16px;'>+</span>") & "</span><a href='#CatEsp-" & (i + 1) & "' data-link='catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'  data-toggle='tab' aria-expanded='false'>" & "</a> </li>"
                End If

            Next
        End If
        Return sHTML
    End Function

    Public Function fnCargaCategoriasHTML_original(NombreCat As String) As String
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(1))
                ' 'objDatos.fnlog("Primer Cats", ssql.Replace("'", ""))
            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle'>" & NombreCat & "<b class='caret'></b></a>"
            sHtmlEncabezado = sHtmlEncabezado & "   <ul class='dropdown-menu' style='display: none;'>"
            sHtmlEncabezado = sHtmlEncabezado & "    <li>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div class='yamm-content'>"
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "       <div  class='col-sm-12 no-padding'>"
            sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-4'>"
            sHtmlEncabezado = sHtmlEncabezado & "         <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                Dim sValorPintar As String = ""
                ' 'objDatos.fnlog("Cats", "entra")
                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objDatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                    ' 'objDatos.fnlog("Cats", ssql.Replace("'", ""))
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                '  'objDatos.fnlog("Cats 1 ", dtCategorias.Rows(i)(0))

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                sHtmlBanner = sHtmlBanner & "<li><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & sValorPintar & "</a> "
                'sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' data-toggle='tab' aria-expanded='false'>" & sValorPintar & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                ' sHtmlBanner = sHtmlBanner & "<li><a href='#Cat-" & iContador & "' >" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                sHtmlBanner = sHtmlBanner & "</li> "

                'If iContador = 0 Then
                '    sHtmlBanner = sHtmlBanner & "<li class='active'><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                '    sHtmlBanner = sHtmlBanner & "</li> "
                'Else
                '    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                '    sHtmlBanner = sHtmlBanner & "</li> "
                'End If
                iContador = iContador + 1


            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"

            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-8'>" 'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-content'>" 'El que agregué
            ''Aqui las subcategorias
            iContador = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='Cat-" & (iContador) & "'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-12'>" 'class='col-xs-4'
                sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"

                ''Nos traemos el nivel 2 de la categoría seleccionada
                ssql = objDatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If


                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    '  'objDatos.fnlog("Categorias_det", ssql.Replace("'", ""))
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""

                    Dim sValorPintar As String = ""

                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            If CStr(dtSegundoNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2")
                                ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                                Dim dtValor As New DataTable
                                dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                '     'objDatos.fnlog("CampoUsuarioNiv2", ssql.Replace("'", ""))
                                If dtValor.Rows.Count > 0 Then
                                    sValorPintar = dtValor.Rows(0)(0)
                                End If
                            Else
                                sValorPintar = dtSubCategoria.Rows(x)(0)
                            End If
                            sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "'> " & sValorPintar & "</a></li>"
                        End If

                    Next
                End If

                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                iContador = iContador + 1
                sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"


            Next


            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué



            sHtmlEncabezado = sHtmlEncabezado & " <div class='clearfix'></div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div></div></li></ul></li>"





            Return sHtmlEncabezado

            'Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Function
    Public Function fnCategoriasEspecialResponsive() As String
        Dim sHTML As String = ""
        ssql = "select cvDescripcion ,cvCampoSAP,cvQuery  from config.CategoriasEsp where ciEstatus =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            For i = 0 To dtPrimerNivel.Rows.Count - 1 Step 1


                sHTML = sHTML & " <div class='panel-heading'>"

                sHTML = sHTML & " <h4 class='panel'>"

                'sHTML = sHTML & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "'>" & sValorPintar & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#collapse" & (i + 1) & "' class='collapsed'></a>"
                sHTML = sHTML & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'>" & CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper.Replace("PLUS", "+") & "</span> <a data-toggle='collapse'  data-parent='#accordion' href='#collapse" & (i + 1) & "' class='collapsed'></a>"

                sHTML = sHTML & "   </h4>"

                sHTML = sHTML & "   </div>"
            Next
        End If
        Return sHTML
    End Function
    Public Function fnCargaCategoriasHTMLResponsive(NombreCat As String) As String
        'Dim dtProductos As New DataTable
        'Dim sHtmlEncabezado As String = ""
        'Dim sHtmlBanner As String = ""

        'ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        'Dim dtPrimerNivel As New DataTable
        'dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        'If dtPrimerNivel.Rows.Count > 0 Then
        '    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
        '        ''Grupo Nativo de SAP
        '        ssql = objDatos.fnObtenerQuery("GrupoTodos")
        '    Else
        '        ''Traemos el distinct del campo en OITM
        '        ssql = objDatos.fnObtenerQuery("Categorias")
        '        ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

        '    End If
        '    Dim dtCategorias As New DataTable
        '    dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
        '    Dim sLinkPrimerNivel As String = ""
        '    Dim sLinkSegundoNivel As String = ""
        '    sHtmlEncabezado = sHtmlEncabezado & "  <li class='panel'>"
        '    sHtmlEncabezado = sHtmlEncabezado & "     <div class='panel-heading' role='tab' id='headingThree'>"
        '    sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' class='link-m-r' data-toggle='collapse' data-parent='#accordion' href='#collapseThree' aria-expanded='true' aria-controls='collapseThree'>" & NombreCat & "</a>"
        '    sHtmlEncabezado = sHtmlEncabezado & "     </div>"
        '    sHtmlEncabezado = sHtmlEncabezado & "     <div id='collapseThree' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingThree'>"
        '    sHtmlEncabezado = sHtmlEncabezado & "       <div class='panel-body'>"

        '    Dim iContador As Int16 = 5
        '    For i = 0 To dtCategorias.Rows.Count - 1 Step 1

        '        ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
        '        ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
        '        Dim dtCategoriaValida As New DataTable
        '        dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

        '        If dtCategoriaValida.Rows.Count > 0 Then
        '            sHtmlBanner = sHtmlBanner & "<a class='link-m-r' href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' >" & dtCategorias.Rows(i)(0) & "</a> "
        '            iContador = iContador + 1
        '        End If

        '    Next
        '    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

        '    sHtmlEncabezado = sHtmlEncabezado & " </div></div></li>"
        '    Return sHtmlEncabezado


        'End If

        ''Cargamos menu Responsive HEADER
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(1))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                Dim sValorPintar As String = ""

                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objDatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                End If

                Dim sCampoPrimerNivel As String = ""
                ''Nos traemos el nivel 2 de la categoría seleccionada
                ssql = objDatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If

                sHtmlEncabezado = sHtmlEncabezado & " <div class='panel-heading'>"

                sHtmlEncabezado = sHtmlEncabezado & " <h4 class='panel'>"
                'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-heading' role='tab' id='heading" & (i + 1) & "'>"
                'sHtmlEncabezado = sHtmlEncabezado & "   <h4 class='categoria'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "'>" & sValorPintar & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#collapse" & (i + 1) & "' class='collapsed'></a>"
                'sHtmlEncabezado = sHtmlEncabezado & "   </h4>"
                'sHtmlEncabezado = sHtmlEncabezado & "  </div>"
                'sHtmlEncabezado = sHtmlEncabezado & " <div id='collapse" & (i + 1) & "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='heading" & (i + 1) & "'>"
                'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-body'>"
                'sHtmlEncabezado = sHtmlEncabezado & "   <ul class='subcategorias'>"


                sHtmlEncabezado = sHtmlEncabezado & "   </h4>"

                sHtmlEncabezado = sHtmlEncabezado & "   </div>"



                Dim iExisteTercerNivel As Int16 = 0

                Dim sQueryTercer As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =3"
                Dim dtTercerNivel As New DataTable
                dtTercerNivel = objDatos.fnEjecutarConsulta(sQueryTercer)
                If dtTercerNivel.Rows.Count > 0 Then
                    iExisteTercerNivel = 1
                End If

                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    '  'objDatos.fnlog("Segundo nivel", ssql.Replace("'", ""))

                    ' 'objDatos.fnlog("Evaluar tercer nivel", ssql.Replace("'", ""))
                    If iExisteTercerNivel = 1 Then

                        '     'objDatos.fnlog("Query tercer nivel", sQueryTercer.Replace("'", ""))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = sHtmlBanner & " <div id='collapse" & (i + 1) & "' class='panel-collapse collapse '> "
                    sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "

                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If CStr(dtSegundoNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                            ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2")
                            ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                            Dim dtValor As New DataTable
                            dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtValor.Rows.Count > 0 Then
                                sValorPintar = dtValor.Rows(0)(0)
                            End If
                        Else
                            If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                                sValorPintar = ""
                            Else
                                sValorPintar = dtSubCategoria.Rows(x)(0)
                            End If

                        End If
                        If iExisteTercerNivel = 0 Then
                            sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 no-padding'> "
                            sHtmlBanner = sHtmlBanner & " <a data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                            sHtmlBanner = sHtmlBanner & " </a>"
                            sHtmlBanner = sHtmlBanner & "</div>"

                        Else
                            sQueryTercer = objDatos.fnObtenerQuery("Categorias-Tercero")

                            If dtTercerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                                sQueryTercer = sQueryTercer.Replace("[%0]", "ItmsGrpNam")
                            Else
                                sQueryTercer = sQueryTercer.Replace("[%0]", "ISNULL(T0." & dtTercerNivel.Rows(0)("cvCampoSAP") & ",'')")
                                sQueryTercer = sQueryTercer.Replace("[%1]", sCampoPrimerNivel)

                            End If
                            sQueryTercer = sQueryTercer.Replace("[%2]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtSubCategoria.Rows(x)(0) & "'")

                            ' 'objDatos.fnlog("Query tercer nivel: " & sLinkPrimerNivel & " " & dtSubCategoria.Rows(x)(0), sQueryTercer.Replace("'", ""))

                            ''Preparamos la estructura para el tercer nivel
                            sHtmlBanner = sHtmlBanner & " <div class='panel-group' id='accordion-" & (i + 1) & "'> "
                            sHtmlBanner = sHtmlBanner & "  <div class='panel panel-gtk'> "
                            sHtmlBanner = sHtmlBanner & "    <div class='panel-heading'> "
                            sHtmlBanner = sHtmlBanner & "      <h4 class='panel-title'> "
                            sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & "' class='collapsed'></a>"
                            sHtmlBanner = sHtmlBanner & "      </h4>"
                            sHtmlBanner = sHtmlBanner & "    </div>"

                            ''Leemos el tercer nivel

                            sHtmlBanner = sHtmlBanner & " <div id='collapse-" & sValorPintar.Replace(" ", "_").Replace("/", "") & "' class='panel-collapse collapse '>"
                            sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "

                            Dim dtTercer As New DataTable
                            dtTercer = objDatos.fnEjecutarConsultaSAP(sQueryTercer)


                            For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                '    'objDatos.fnlog("tercer nivel", dtTercer.Rows(y)(0))
                                sValorPintar = ""
                                If CStr(dtTercerNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                    ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv3")
                                    ssql = ssql.Replace("[%0]", dtTercer.Rows(y)(0))
                                    Dim dtValor As New DataTable
                                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                    If dtValor.Rows.Count > 0 Then
                                        sValorPintar = dtValor.Rows(0)(0)
                                    End If
                                Else
                                    sValorPintar = dtTercer.Rows(y)(0)
                                End If
                                If sValorPintar <> "" Then
                                    sHtmlBanner = sHtmlBanner & " <div class='col-xs-12 no-padding'>"
                                    sHtmlBanner = sHtmlBanner & "  <a data-grafica='20' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a>"
                                    sHtmlBanner = sHtmlBanner & " </div>"
                                End If

                            Next
                            sHtmlBanner = sHtmlBanner & " </div></div>"

                            sHtmlBanner = sHtmlBanner & " </div>"
                            sHtmlBanner = sHtmlBanner & "</div>"
                        End If

                    Next
                    sHtmlBanner = sHtmlBanner & "  </div>"
                    sHtmlBanner = sHtmlBanner & " </div>"
                End If
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner



                '' End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & fnCategoriasEspecialResponsive()
            Return sHtmlEncabezado
        End If

    End Function
    Public Sub fnCargaCategorias()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            '   sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-2'>"
            '  sHtmlEncabezado = sHtmlEncabezado & " <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 5
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    sHtmlBanner = sHtmlBanner & "</li> "

                    'If iContador = 0 Then
                    '    sHtmlBanner = sHtmlBanner & "<li class='active'><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                    '    sHtmlBanner = sHtmlBanner & "</li> "
                    'Else
                    '    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    '    sHtmlBanner = sHtmlBanner & "</li> "
                    'End If
                    iContador = iContador + 1
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '     sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            '  pnlCategorias.Controls.Clear()
            ' pnlCategorias.Controls.Add(Literal)
        End If
    End Sub
    Public Sub fnCargaCategoriasv2()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            sHtmlEncabezado = sHtmlEncabezado = " <div class='col-xs-2'>"
            sHtmlEncabezado = sHtmlEncabezado & "  <ul class='nav nav-tabs tabs-left'>"

            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (i + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & " </a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                    sHtmlBanner = sHtmlBanner & "</li> "

                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"


            ''Ahora subCategorias
            sHtmlBanner = ""
            sHtmlEncabezado = sHtmlEncabezado = " <div class='col-xs-10'>"
            sHtmlEncabezado = sHtmlEncabezado = "  <div class='tab-content'>"



            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlEncabezado = sHtmlEncabezado & "<div class='tab-pane' id='cat-" & (i + 1) & "'>"
                    sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-3'>"
                    sHtmlEncabezado = sHtmlEncabezado & "  <ul class='tab-submenu'>"

                    ''Nos traemos el nivel 2 de la categoría seleccionada
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                    End If


                    Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                    Dim dtSegundoNivel As New DataTable
                    dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""
                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            sHtmlBanner = sHtmlBanner & "<a href='Catalogo.aspx'><li>" & dtSubCategoria.Rows(x)(0) & "</li></a>" '?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "
                        End If

                    Next
                    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                    sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                End If

            Next
            '    sHtmlEncabezado = sHtmlEncabezado & " </div></div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Sub
    Public Sub fnCargaSubCategorias()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "Select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""

            Dim iContador As Int16 = 5
            ' sHtmlEncabezado = sHtmlEncabezado & "<div class='tab-pane active' id='principal'>"
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='cat-" & (iContador + 1) & "'>"
                    sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-3'>"
                    sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"

                    ''Nos traemos el nivel 2 de la categoría seleccionada
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                    End If


                    Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                    Dim dtSegundoNivel As New DataTable
                    dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""
                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx'>" & dtSubCategoria.Rows(x)(0) & "</a></li>" '?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "
                        End If

                    Next
                    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                    iContador = iContador + 1
                    sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & "</div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            pnlSubCategoria.Controls.Clear()
            pnlSubCategoria.Controls.Add(Literal)
        End If
    End Sub
    Public Sub fnCargaMenuHeader()
        ''Cargamos menu clasico HEADER


        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Header'  AND ISNULL(cvTipoDato,'') <> 'User' order by IsNULL(ciOrden,1) "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            If dtMenuHeader.Rows(i)("cvNombre") = "productos" Or dtMenuHeader.Rows(i)("cvNombre") = "products" Or dtMenuHeader.Rows(i)("cvNombre") = "Productos" Or dtMenuHeader.Rows(i)("cvNombre") = "tienda" Or dtMenuHeader.Rows(i)("cvNombre") = "store" Or dtMenuHeader.Rows(i)("cvNombre") = "Tienda" Or CStr(dtMenuHeader.Rows(i)("cvNombre")).ToUpper = "COLECCIÓN" Or CStr(dtMenuHeader.Rows(i)("cvNombre")).ToUpper = "COLECCION" Then

                ssql = "Select cvHTML From config.HTML where cvTipo='Categorias'"
                Dim dtHTML As New DataTable
                dtHTML = objDatos.fnEjecutarConsulta(ssql)

                If dtHTML.Rows.Count > 0 Then
                    'Dim re As StreamReader = File.OpenText("categorias.txt")
                    'Dim entrada As String = ""
                    'Dim texto As String = ""
                    'While ((entrada = re.ReadLine()) <> Nothing)
                    '    texto += entrada
                    'End While
                    'sHtmlMenu = sHtmlMenu & texto
                    objDatos.fnLog("Menu HDR", "Si encuentra las categorías")
                    sHtmlMenu = sHtmlMenu & dtHTML.Rows(0)(0)
                Else
                    objDatos.fnLog("Menu HDR", "Calcula las categorías al vuelo")
                    sHtmlMenu = sHtmlMenu & fnCargaCategoriasHTML(dtMenuHeader.Rows(i)("cvNombre"))



                End If
            Else
                ''Revisamos si tiene subMenus
                ssql = "SELECT * from config.SubMenu where cvMenu =" & "'" & dtMenuHeader.Rows(i)("cvNombre") & "'"
                Dim dtSubMenu As New DataTable
                dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
                If dtSubMenu.Rows.Count > 0 Then
                    ''Tiene subMenus

                    sHtmlMenu = sHtmlMenu & "<li class='drop-bold'>"
                    sHtmlMenu = sHtmlMenu & " <div class='dropdown-backdrop'></div><a href='#' class='dropdown-toggle' data-toggle='dropdown'>" & dtMenuHeader.Rows(i)("cvNombre") & "<b class='caret'></b> </a>"
                    sHtmlMenu = sHtmlMenu & "  <ul class='dropdown-menu drop-mini' role='menu' aria-labelledby='dropdownMenu'> "
                    For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                        sHtmlMenu = sHtmlMenu & " <li class=''><a tabindex='-1' href='" & dtSubMenu.Rows(x)("cvLink") & "'> " & dtSubMenu.Rows(x)("cvSubMenu") & " </a></li> "
                    Next
                    sHtmlMenu = sHtmlMenu & "</ul>"
                    sHtmlMenu = sHtmlMenu & "</li>"
                Else
                    If Session("Page") = dtMenuHeader.Rows(i)("cvLink") Then
                        sHtmlMenu = sHtmlMenu & " <li class='drop-bold'><a class='active' href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
                    Else
                        Try
                            If dtMenuHeader.Rows(i)("cvEstilo") = "blank" Then
                                sHtmlMenu = sHtmlMenu & " <li class='drop-bold'><a id='button' href='" & dtMenuHeader.Rows(i)("cvLink") & "' target='_blank'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
                            Else
                                sHtmlMenu = sHtmlMenu & " <li class='drop-bold'><a id='button' href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
                            End If
                        Catch ex As Exception
                            sHtmlMenu = sHtmlMenu & " <li class='drop-bold'><a id='button' href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
                        End Try


                    End If


                End If

            End If

        Next

        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)
        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("BOSS") And Session("RazonSocial") <> "" Then
                sHtmlMenu = sHtmlMenu & " <li class='drop-bold'><a id='button' href='dashb2b.aspx'>Mi Dashboard </a></li> "
            End If
        End If

        objDatos.fnLog("Menu HDR", "sale")
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuClasicoHeader.Controls.Clear()
        pnlMenuClasicoHeader.Controls.Add(literal)
    End Sub

    Public Sub fnCargaMenuResponsive()

        Dim sHTMLEncabezado As String = ""

        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Header'  AND ISNULL(cvTipoDato,'') <> 'User'  order by IsNULL(ciOrden,1) "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            If dtMenuHeader.Rows(i)("cvNombre") = "productos" Or dtMenuHeader.Rows(i)("cvNombre") = "products" Or dtMenuHeader.Rows(i)("cvNombre") = "Productos" Or CStr(dtMenuHeader.Rows(i)("cvNombre")).ToUpper = "COLECCIÓN" Or CStr(dtMenuHeader.Rows(i)("cvNombre")).ToUpper = "COLECCION" Then
                sHtmlMenu = sHtmlMenu & " <li class='panel'> "
                sHtmlMenu = sHtmlMenu & "  <div class='panel-heading' role='tab' id='headingProducts'> "
                sHtmlMenu = sHtmlMenu & "   <a class='link-m-r' role='button' data-toggle='collapse' data-parent='#accordion' href='#collapseProducts' aria-expanded='false' aria-controls='collapseProducts'> " & dtMenuHeader.Rows(i)("cvNombre")
                sHtmlMenu = sHtmlMenu & "   </a>"
                sHtmlMenu = sHtmlMenu & "  </div> "
                sHtmlMenu = sHtmlMenu & " <div id='collapseProducts' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingProducts'> "

                ssql = "Select cvHTML From config.HTML where cvTipo='Categorias-responsive'"
                Dim dtHTML As New DataTable
                dtHTML = objDatos.fnEjecutarConsulta(ssql)

                If dtHTML.Rows.Count > 0 Then
                    sHtmlMenu = sHtmlMenu & dtHTML.Rows(0)(0)
                Else
                    sHtmlMenu = sHtmlMenu & fnCargaCategoriasHTMLResponsive(dtMenuHeader.Rows(i)("cvNombre"))
                End If


                sHtmlMenu = sHtmlMenu & " </div> "
                sHtmlMenu = sHtmlMenu & "</li> "
            Else
                sHtmlMenu = sHtmlMenu & " <li class='link-m-r'><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
            End If

        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuResponsiveHeader.Controls.Clear()
        pnlMenuResponsiveHeader.Controls.Add(literal)
    End Sub
    Public Sub fnCargaMenu()
        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Lateral'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = " <ul class='cd-cart-items'> "


        Dim iCuantosFila As Int16 = 0
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1

            If iCuantosFila = 0 Then
                sHtmlMenu = sHtmlMenu & " <div class='flex-sep'> "
            End If

            iCuantosFila = iCuantosFila + 1
            sHtmlMenu = sHtmlMenu & fnGeneraOpcionMenu(dtMenus.Rows(i)("cvNombre"), dtMenus.Rows(i)("cvImagen"), i, dtMenus.Rows(i)("cvLink"), dtMenus.Rows(i)("cvEstilo"))
            If iCuantosFila = 2 Then
                iCuantosFila = 0
                sHtmlMenu = sHtmlMenu & " </div> "
            End If
        Next
        If iCuantosFila > 0 Then
            sHtmlMenu = sHtmlMenu & " </div> "
        End If

        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        sHtmlEncabezado = sHtmlEncabezado & " </ul>"

        Dim literal As New LiteralControl(sHtmlEncabezado)
        'pnlMenu.Controls.Clear()
        'pnlMenu.Controls.Add(literal)
    End Sub


    Public Function fnGeneraOpcionMenu(Nombre As String, imagen As String, valor As String, link As String, Estilo As String) As String
        Dim sHtmlMenu As String = ""
        sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 " & Estilo.ToLower & "'> "
        sHtmlMenu = sHtmlMenu & "<div class='col-xs-12 tit-pedidos'>" & Nombre & " </div> "
        sHtmlMenu = sHtmlMenu & " <div class='icono '> "
        sHtmlMenu = sHtmlMenu & " <img src='" & imagen & "' class='img-responsive'> "
        sHtmlMenu = sHtmlMenu & " <span class='data'> " & valor & "</span>"
        sHtmlMenu = sHtmlMenu & " </div>"
        sHtmlMenu = sHtmlMenu & " </li>"
        Return sHtmlMenu
    End Function
    Protected Sub btnBuscar_Click(sender As Object, e As ImageClickEventArgs) Handles btnBuscar.Click
        'If txtUser.Text <> "" Then
        '    btnIngresar_Click(sender, e)
        'Else

        'End If
        Session("BusquedaAIO_Index") = "NO"
        Session("sesBuscar") = txtBuscar.Text
        Response.Redirect("Catalogo.aspx")

    End Sub
    'Protected Sub btnCerrarSesion_Click(sender As Object, e As ImageClickEventArgs) Handles btnCerrarSesion.Click
    '    Session("UserTienda") = ""
    '    Session("NombreuserTienda") = ""
    '    Session("slpCode") = "0"
    '    Session("Cliente") = ""
    '    Session("RazonSocial") = ""
    '    Session("ListaPrecios") = ""

    '    Session("UserB2C") = ""
    '    Session("NombreUserB2C") = ""
    '    Session("NombreuserTienda") = ""
    '    Session("CardCodeUserB2C") = ""



    '    Response.Redirect("Index.aspx")
    'End Sub


    Public Sub fnCargaMenuB2B()
        ssql = "SELECT cvNombre,cvImagen,cvLink,cvEstilo,ISNULL(cvQuery,'') as Query FROM Config.Menus where cvTipoMenu='Lateral' order by ciOrden "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String = ""

        If Session("Page") = "levantar-pedido.aspx" Then
            sHtmlEncabezado = sHtmlEncabezado & " <div id='main-nav' class='speed-in'>"
        Else
            sHtmlEncabezado = sHtmlEncabezado & " <div id='main-nav'>"
        End If


        sHtmlEncabezado = sHtmlEncabezado & "         <div class='open-engine'><div id='cd-hamburger-menu'><img src='img/menu-b2b/grid.svg' class='img-responsive cuadrito-post'></div></div> "
        'sHtmlEncabezado = sHtmlEncabezado & "         <div class='open-engine'><div id='cd-hamburger-menu'><div style='font-family:Montserrat, sans-serif;font-size:12px;background:rgba(59, 63, 72, 0.8); border-color:white;'>Reportes + </div></div></div> "
        sHtmlEncabezado = sHtmlEncabezado & " <ul class='menu-b2b'> "

        sHtmlEncabezado = sHtmlEncabezado & "<div class='e-tienda'>"
        sHtmlEncabezado = sHtmlEncabezado & "<div class='sec-est'><a class='enlace icon-l-plantilla' href='levantar-pedido.aspx' data-toggle='tooltip' data-placement='bottom' title='Mis Plantillas'> </a></div> "
        sHtmlEncabezado = sHtmlEncabezado & "<div class='sec-est'><a class='enlace icon-tienda' href='catalogo.aspx' data-toggle='tooltip' data-placement='bottom' title='Tienda'>"
        sHtmlEncabezado = sHtmlEncabezado & " </a></div>"
        '  sHtmlEncabezado = sHtmlEncabezado & " <div class='sec-est'><a class='enlace icon-contacto' href='contacto.aspx' data-toggle='tooltip' data-placement='bottom' title='Contacto'></a></div>"
        sHtmlEncabezado = sHtmlEncabezado & " </div>"

        sHtmlEncabezado = sHtmlEncabezado & "<div class='enlace-int'>"

        Dim iCuantosFila As Int16 = 0
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1

            sHtmlMenu = sHtmlMenu & fnGeneraOpcionMenuB2B(dtMenus.Rows(i)("cvNombre"), dtMenus.Rows(i)("cvImagen"), dtMenus.Rows(i)("Query"), dtMenus.Rows(i)("cvLink"), dtMenus.Rows(i)("cvEstilo"))

        Next


        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        sHtmlEncabezado = sHtmlEncabezado & " </div>"

        sHtmlEncabezado = sHtmlEncabezado & "</ul>"
        sHtmlEncabezado = sHtmlEncabezado & " </div>"


        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlB2B.Controls.Clear()
        pnlB2B.Controls.Add(literal)
    End Sub

    Public Sub fnPlantillasVendedor()
        ''Cargamos las plantillas que tiene el usuario
        ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE ( ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' OR cvUsuario=" & "'" & Session("Cliente") & "')"
        Dim dtPlantillas As New DataTable
        dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        For i = 0 To dtPlantillas.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <li><a href='levantar-pedido.aspx?opc=" & dtPlantillas.Rows(i)("No") & "'>" & dtPlantillas.Rows(i)("Plantilla") & "</a> </li>"
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFavoritos.Controls.Clear()
        pnlFavoritos.Controls.Add(literal)

    End Sub

    Public Sub fnListaDeseos()
        ''Cargamos las plantillas que tiene el usuario

        Dim sHtmlMenu As String = ""



        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sHtmlImagen As String = ""
        Dim sHtmlPRecio As String = ""
        Dim sHtmlCantidad As String = ""
        Dim sHtmlAtributos As String = ""
        Dim sCampos As String = ""
        Dim iContador As Int16 = 0
        Dim iCartContent As Int16 = 0

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Menu' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"


        For Each Partida As Cls_Pedido.Partidas In Session("WishList")
            If Partida.ItemCode <> "BORRAR" Then
                iContador = iContador + 1
                sHtmlBanner = sHtmlBanner & " <li> "
                sHtmlBanner = sHtmlBanner & "  <div class='div-sdiviped'>"
                sHtmlBanner = sHtmlBanner & "   <div class='row-cart'>"
                If dtCamposPlantilla.Rows.Count > 0 Then

                    For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
                        ssql = objDatos.fnObtenerQuery("Info-Producto")

                        If Session("ItemCodeTallaColor") <> "" Then
                            ssql = ssql.Replace("[%0]", "'" & Session("ItemCodeGenerico") & "'")
                        Else
                            ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")


                        End If
                        '  'objDatos.fnlog("CatGeneral", ssql.Replace("'", ""))

                        Dim dtGeneral As New DataTable
                        dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)
                        ' 'objDatos.fnlog("CatGeneral", ssql.Replace("'", ""))
                        If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                            sHtmlImagen = sHtmlImagen & " <div class='image-cart text-center'> <a href='producto-interior.aspx?Code=" & Partida.ItemCode & "'>"
                            sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                            sHtmlImagen = sHtmlImagen & "</a></div>"
                        Else
                            If iCartContent = 0 Then
                                iCartContent = 1
                                ' sHtmlBanner = sHtmlBanner & "<div class='cart-content'>"
                                ' sHtmlBanner = sHtmlBanner & " <div class='cart-button text-center'>"
                            End If
                            If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then
                                '     sHtmlAtributos = sHtmlAtributos & " <div class='product-name text-left'> <a href='producto-interior.aspx'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</a></div>"
                                sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                            Else
                                Dim dPrecioActual As Double
                                If CInt(Session("slpCode")) <> 0 Then

                                    dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                                Else
                                    dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                                End If
                                sHtmlCantidad = sHtmlCantidad & " <strong class='text-right'>x - " & Partida.Cantidad & " </strong> "
                                sHtmlPRecio = sHtmlPRecio & " <span class='cart-price text-right'>$ " & dPrecioActual.ToString("###,###,###.#0") & "</span>"
                            End If



                        End If
                    Next

                    'sHtmlBanner = sHtmlBanner & "</div>"
                    'sHtmlBanner = sHtmlBanner & "</div>"

                    iCartContent = 0
                End If
                sHtmlAtributos = "<div class='product-name text-left'> <a href='producto-interior.aspx'>" & sCampos & "</a></div>"

                sHtmlBanner = sHtmlBanner & sHtmlImagen
                sHtmlBanner = sHtmlBanner & "<div class='cart-content'>"
                sHtmlBanner = sHtmlBanner & sHtmlAtributos
                sHtmlBanner = sHtmlBanner & sHtmlCantidad
                sHtmlBanner = sHtmlBanner & sHtmlPRecio
                sHtmlBanner = sHtmlBanner & " <div class='cart-button text-center'>"
                sHtmlBanner = sHtmlBanner & "  <button type='button' onclick=fnClickEliminarDeseo('" & Partida.ItemCode & "'); title='Eliminar' class='btn cancel-p'>"
                sHtmlBanner = sHtmlBanner & "    <img src='img/cancel.svg' class='img-responsive'>"
                sHtmlBanner = sHtmlBanner & "  </button>"
                sHtmlBanner = sHtmlBanner & " </div>"

                sHtmlBanner = sHtmlBanner & "</div>"

                sHtmlBanner = sHtmlBanner & " </div></div></li> "
            End If

            sCampos = ""
            sHtmlImagen = ""
            sHtmlAtributos = ""
            sHtmlCantidad = ""
            sHtmlPRecio = ""

        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        If iContador > 0 Then
            pnlContadorCarrito.Visible = True
        Else
            pnlContadorCarrito.Visible = False
        End If
        lblItemsCarrito.Text = iContador



        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlFavoritos.Controls.Clear()
        pnlFavoritos.Controls.Add(literal)


    End Sub

    Public Function fnGeneraOpcionMenuB2B(Nombre As String, imagen As String, valor As String, link As String, Estilo As String) As String
        Dim sHtmlMenu As String = ""

        sHtmlMenu = sHtmlMenu & " <a class='rec' href='" & link & "'> "
        sHtmlMenu = sHtmlMenu & "<div class='titulo'>" & Nombre & " </div> "
        If valor <> "" Then
            Dim dtValorMenu As New DataTable
            ssql = objDatos.fnObtenerQuery(valor)
            '      ssql = ssql.Replace("[%0]", Session("slpCode"))
            ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
            dtValorMenu = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtValorMenu.Rows.Count = 0 Then
                sHtmlMenu = sHtmlMenu & "<span class='cuant'>0</span> "
            Else
                If Estilo = "importe" Then
                    Dim importe As Double = 0
                    If dtValorMenu.Rows(0)(0) Is DBNull.Value Then
                        importe = 0
                    Else
                        importe = dtValorMenu.Rows(0)(0)
                    End If
                    sHtmlMenu = sHtmlMenu & "<span class='cuant'>" & importe.ToString("$ ###,###,###,###.#0") & "</span> "
                Else
                    If Estilo = "texto" Then
                        sHtmlMenu = sHtmlMenu & "<span class='cuant'>" & dtValorMenu.Rows(0)(0) & "</span> "
                    Else
                        sHtmlMenu = sHtmlMenu & "<span class='cuant'>" & dtValorMenu.Rows.Count & "</span> "
                    End If

                End If

            End If

        End If
        sHtmlMenu = sHtmlMenu & " <img src='" & imagen & "' class='img-responsive'/> "
        sHtmlMenu = sHtmlMenu & " </a>"

        Return sHtmlMenu
    End Function
    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        ''Hacemos el login
        If txtUser.Text = "" Then
            objDatos.Mensaje("Especifique un usuario", Me.Page)
            Exit Sub
        End If
        If txtPass.Text = "" Then
            objDatos.Mensaje("Especifique una contraseña", Me.Page)
            Exit Sub
        End If
        ssql = "SELECT * FROM Config.Parametrizaciones"
        Dim dtLogin As New DataTable
        Dim dtParam As New DataTable
        dtParam = objDatos.fnEjecutarConsulta(ssql)
        If dtParam.Rows.Count > 0 Then
            'If dtParam.Rows(0)("cvTipoLogin") = "LOCAL" Then

            'Else

            '    '  'objDatos.fnlog("Acceso SAP", ssql.Replace("'", ""))
            'End If

        End If

        ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUser.Text & "' AND cvPass=" & "'" & txtPass.Text & "' "
        dtLogin = objDatos.fnEjecutarConsulta(ssql)
        If dtLogin.Rows.Count = 0 Then
            ' objDatos.Mensaje("Acceso incorrecto", Me.Page)
            'Exit Sub
            ''Buscamos en SAP
            ''Hay que buscar en SAP
            ssql = objDatos.fnObtenerQuery("Usuario")
            ssql = ssql.Replace("[%0]", txtUser.Text).Replace("[%1]", txtPass.Text)
            dtLogin = objDatos.fnEjecutarConsultaSAP(ssql)

        End If

        If dtLogin.Rows.Count > 0 Then
            Session("usrInvitado") = "NO"
            'If dtLogin.Rows(0)("cvCardCode") = "" Then
            '    objDatos.Mensaje("Acceso incorrecto", Me.Page)
            '    Exit Sub
            'End If



            Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("Cliente") = dtLogin.Rows(0)("cvCardCode")
            Session("UserTienda") = dtLogin.Rows(0)("cvUsuario")
            If dtLogin.Rows(0)("cvTipoAcceso") = "B2B" Then
                'objDatos.fnlog("Acceso SAP", "B2B")
                Session("UserB2C") = ""

                Session("RazonSocial") = dtLogin.Rows(0)("cvNombreCompleto")
                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                'objDatos.fnlog("ListaPrecios", ssql.Replace("'", ""))
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtLista.Rows.Count > 0 Then
                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                Else
                    Session("ListaPrecios") = "1"
                End If

                'objDatos.fnlog("Acceso SAP Lista Precios", Session("ListaPrecios"))
                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
                fnCargaMenuB2B()
            Else
                Session("RazonSocial") = ""
                Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
                Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
                ' Session("Cliente") = dtLogin.Rows(0)("cvCardCode")

                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtLista.Rows.Count > 0 Then
                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                Else
                    Session("ListaPrecios") = "1"
                End If

                Response.Redirect("index.aspx")
                Exit Sub
            End If
            '''en base al cliente, obtenemos cual es su lista de precios
            'ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
            'ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))


            pnlLogin.Visible = False
            pnlUsuarioLogin.Visible = True
            pnlCliente.Visible = True
            lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
            'btnCerrarSesion.Visible = True
            'pnlOpciones.Visible = True
            '  fnCargaMenuUser()
            If dtLogin.Rows(0)("cvTipoAcceso") = "B2B" Then
                Response.Redirect("catalogo.aspx")
            Else
                Response.Redirect("index.aspx")
            End If

            pnlIconUser.Visible = False

        Else
            ''Revisamos si el acceso no es por un usuario B2C
            ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUser.Text & "' AND cvPass=" & "'" & txtPass.Text & "' and cvTipoAcceso='B2C' "
            dtLogin = New DataTable
            dtLogin = objDatos.fnEjecutarConsulta(ssql)
            If dtLogin.Rows.Count > 0 Then
                Session("RazonSocial") = ""
                Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
                Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
                ' Session("Cliente") = dtLogin.Rows(0)("cvCardCode")

                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtLista.Rows.Count > 0 Then
                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                Else
                    Session("ListaPrecios") = "1"
                End If
                Response.Redirect("index.aspx")
            Else
                objDatos.Mensaje("Acceso incorrecto", Me.Page)
                Exit Sub
            End If
            Exit Sub
        End If
    End Sub

    Private Sub btnBuscarResponsive_Click(sender As Object, e As ImageClickEventArgs) Handles btnBuscarResponsive.Click
        If txtUser.Text <> "" Then
            btnIngresar_Click(sender, e)
        Else
            Session("sesBuscar") = txtSearchResponsive.Text
            Response.Redirect("Catalogo.aspx")
        End If
    End Sub
End Class

