
Imports System
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class confirmacionCompra
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones


    Public sTipoDoc As String = "ORDEN DE VENTA"
    Public sAplicaPago As String = "SI"
    Public sTipoPago As String = "A CUENTA"

    Public sCaracterMoneda As String = ""
    Private Sub confirmacionCompra_Load(sender As Object, e As EventArgs) Handles Me.Load
        'If Request.QueryString.Count = 0 Then
        '    ''Entraron por error sin haber compra
        '    Response.Redirect("index.aspx")
        'End If

        If objDatos.fnObtenerCliente().ToUpper.Contains("STOP CAT") And Session("ImporteEnvio") = 0 Then
            fnAgregaFletesSeguros_StopCatalogo()
        End If


        Dim FolioTrans As String = ""
        FolioTrans = Request.QueryString(0)
        If Not IsPostBack Then

            ssql = "SELECT * FROM Tienda.pedido_hdr WHERE cvFolioInternet=" & "'" & FolioTrans & "'"
            Dim dtFolioPago As New DataTable
            dtFolioPago = objDatos.fnEjecutarConsulta(ssql)

            If dtFolioPago.Rows.Count > 0 Then
                Exit Sub
            End If
            ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
            Dim dtclienteNom As New DataTable
            dtclienteNom = objDatos.fnEjecutarConsulta(ssql)
            If dtclienteNom.Rows.Count > 0 Then
                If CStr(dtclienteNom.Rows(0)(0)).Contains("Salama") Or CStr(dtclienteNom.Rows(0)(0)).ToUpper.Contains("BOSS") Or CStr(dtclienteNom.Rows(0)(0)).ToUpper.Contains("HAWK") Then
                    btnMuestraPDF.Visible = False

                End If
            End If

            If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Or objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Then
                btnMuestraPDF.Visible = False
            End If
            ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
            Dim dtCaracter As New DataTable
            dtCaracter = objDatos.fnEjecutarConsulta(ssql)
            If dtCaracter.Rows.Count > 0 Then
                sCaracterMoneda = dtCaracter.Rows(0)(0)
            End If

            Session("ArchivoMostrar") = ""
            objDatos.fnLog("confirma", "load")

            ''Cargamos las variables de sesión de la pantalla anterior
            Dim sTelefono As String = ""
            Dim sCalleyNum As String = ""

            If Session("NumInteriorEnvio") = "" Then
                sCalleyNum = Session("CalleEnvio") & " " & Session("NumExtEnvio")
            Else
                sCalleyNum = Session("CalleEnvio") & " " & Session("NumExtEnvio") & " - " & Session("NumInteriorEnvio")
            End If

            If Session("TelefonoEnvio") <> "" Then
                sTelefono = "Tel: " & Session("TelefonoEnvio")
            End If
            objDatos.fnLog("confirma", "load-antes lbls direccion")

            lblNombreDireccion.Text = " " & Session("NombreDireccionEnvio")
            lblNombreEnvio.Text = " Enviado a: " & Session("NombreEnvio")
            lblCalleyNum.Text = " " & sCalleyNum
            lblColoniaMunic.Text = " " & Session("ColoniaEnvio") & " " & Session("CiudadEnvio") & " " & Session("MunicipioEnvio")
            lblEstadoCP.Text = " " & Session("EstadoEnvio") & " " & Session("PaisEnvio") & " " & Session("CPEnvio") & vbCrLf & sTelefono



            objDatos.fnLog("confirma", "load-antes metodo pago")
            fnMetodoPago()

            Dim sDireccionEnvio As String = ""
            sDireccionEnvio = sDireccionEnvio & Session("NombreDireccionEnvio") & "</br>"
            sDireccionEnvio = sDireccionEnvio & sCalleyNum & "</br>"
            sDireccionEnvio = sDireccionEnvio & Session("ColoniaEnvio") & " " & Session("CiudadEnvio") & " " & Session("MunicipioEnvio") & "</br>"
            sDireccionEnvio = sDireccionEnvio & Session("CPEnvio") & " " & Session("EstadoEnvio") & "</br>" & sTelefono

            objDatos.fnLog("confirma", "load-despues dir envio")
            ''El metodo de envio
            ssql = "SELECT * from config.metodosEnvio where ciIdMetodoEnvio=" & "'" & Session("MetodoEnvio") & "'"
            Dim dtMetodos As New DataTable
            dtMetodos = objDatos.fnEjecutarConsulta(ssql)
            If dtMetodos.Rows.Count > 0 Then
                lblMetodoEnvio.Text = dtMetodos.Rows(0)("cvClave") & "(" & dtMetodos.Rows(0)("cvDescripcion") & ") - " & CDbl(dtMetodos.Rows(0)("cvImporte")).ToString("###,###,###.#0") & " " & dtMetodos.Rows(0)("cvMoneda")
            Else
                lblMetodoEnvio.Text = "Costo de envío:($ " & CDbl(Session("ImporteEnvio")).ToString("###,###,###.0#") & " )"
            End If

            ''Procesamos los documentos correspondientes en SAP
            ''Guardamos el primer parametro del request
            objDatos.fnLog("confirma", "load-antes FOLIO")

            lblFolio.Text = "FOLIO: " & FolioTrans
            ''Revisamos si en el botón Finalizar de la página Resumen, se guardó todo. Sino, aqui lo insertamos
            If CInt(Session("NoPedido")) = 0 Then
                fnGuardalocales()
            End If

            Try
                ''Registramos las guias de paqueterías si es que se usaron

                If Session("UsaPaqueteria") = "SI" And CDbl(Session("ImporteEnvio")) > 0 Then
                    ''Cachamos el cotizacion_request
                    Dim Cotizacion As New CotizacionRequest
                    Dim objPaq As New Cls_Paqueteria
                    Dim FolioPedido As String = CInt(Session("NoPedido"))
                    Dim GuiaSolicitada As String = ""
                    Cotizacion = Session("CotizacionRequest")



                    Session("GuiaSolicitada") = ""
                    Dim sIdConceptoPaqueteria As String = ""
                    sIdConceptoPaqueteria = CStr(Session("MetodoEnvio")).Substring(0, 1)
                    ''Obtenemos el consecutivo de la tabla de control
                    Dim ssqlFolioGuia As String = ""
                    ssqlFolioGuia = "SELECT ciFolioSiguiente,cvCuenta FROM config.Folios_Paqueteria where   " & "'" & CInt(Session("PesoPaquete")) & "' <= ciPeso AND ciConcepto='" & sIdConceptoPaqueteria & "' order by ciPeso "
                    Dim dtFolio As New DataTable
                    dtFolio = objDatos.fnEjecutarConsulta(ssqlFolioGuia)
                    If dtFolio.Rows.Count > 0 Then
                        GuiaSolicitada = dtFolio.Rows(0)(0)
                        Cotizacion.guia = "0" & GuiaSolicitada
                        Session("CuentaPaqueteria") = dtFolio.Rows(0)("cvCuenta")
                    Else
                        'Peso mayor
                        ssqlFolioGuia = "SELECT top 1 ciFolioSiguiente,cvCuenta FROM config.Folios_Paqueteria WHERE ciConcepto='" & sIdConceptoPaqueteria & "' order by  ciPeso desc"
                        dtFolio = New DataTable
                        dtFolio = objDatos.fnEjecutarConsulta(ssqlFolioGuia)
                        If dtFolio.Rows.Count > 0 Then
                            GuiaSolicitada = dtFolio.Rows(0)(0)
                            Cotizacion.guia = "0" & GuiaSolicitada
                            Session("CuentaPaqueteria") = dtFolio.Rows(0)("cvCuenta")
                        End If
                        ' Cotizacion.guia = txtguia.Text ''consecutivo
                    End If
                    Session("GuiaSolicitada") = "0" & GuiaSolicitada
                    objDatos.fnLog("Genera guia", Cotizacion.guia & " Paquetes: " & Cotizacion.paquetes.Count)
                    Dim Guias = objPaq.FnCrearModeloGuia(Cotizacion, "2")
                    Dim GuiaRespuesta = objPaq.FnCrearGuia(Guias, Session("BearerToken"))
                    objPaq.FnRespuestaGuia(GuiaRespuesta, FolioPedido, Cotizacion.guia, sIdConceptoPaqueteria)

                End If
            Catch ex As Exception
                objDatos.fnLog("Genera guia ex", ex.Message.Replace("'", ""))
            End Try

            objDatos.fnLog("confirma", "load-antes update folio pago")
            ''Actualizamos el Folio Internet
            ssql = "UPDATE Tienda.pedido_hdr SET cvFolioInternet=" & "'" & FolioTrans & "',cvEstatus='PAGADO' WHERE ciNoPedido=" & "'" & Session("NoPedido") & "'"
            objDatos.fnEjecutarInsert(ssql)

            ''Actualizamos el cvCveCliente en base al socio de negocio configurado para el B2C


            ''Registramos SAP
            ''Obtenemos la configuracion del B2C

            ssql = "SELECT * FROM config.Parametrizaciones_B2C "
            Dim dtConfig As New DataTable
            dtConfig = objDatos.fnEjecutarConsulta(ssql)

            If dtConfig.Rows.Count > 0 Then
                sTipoDoc = dtConfig.Rows(0)("cvTipoDocumento")
                sAplicaPago = dtConfig.Rows(0)("cvAplicaPago")
                sTipoPago = dtConfig.Rows(0)("cvTipoPago")
            End If

            objDatos.fnLog("confirma", "load-antes SAP")

            ''Procesamos SAP
            fnGeneraSAP(CInt(Session("NoPedido")), sTipoDoc)

            Dim sNombredelCliente As String = ""
            ''Enviamos el correo de confirmación
            Dim text As String = MensajeHTML(Server.MapPath("~") & "\correo_A.html")

            ''Generamos variable para guardar y construir el HTML de envio de guía de paquetería
            Dim textPaquetería As String = MensajeHTML(Server.MapPath("~") & "\correo_Paqueteria.html")


            Dim sDestinatario As String = ""
            objDatos.fnLog("Confirmacion", "Sale de SAP")
            If Session("usrInvitado") = "SI" Then
                sDestinatario = Session("CorreoInvitado")
                sNombredelCliente = Session("CorreoInvitado")
            Else
                If Session("VieneB2B") = "SI" Then
                    sDestinatario = Session("CorreoClienteB2B")
                    sNombredelCliente = Session("RazonSocial")
                Else
                    ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre,cvPass,ISNULL(cvMail,'') as Mail,cvUsuario FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'  and cvTipoAcceso='B2C' "
                    Dim dtLogin As New DataTable
                    dtLogin = objDatos.fnEjecutarConsulta(ssql)
                    sDestinatario = dtLogin.Rows(0)("Mail")
                    sNombredelCliente = dtLogin.Rows(0)("Nombre")

                End If


            End If

            objDatos.fnLog("Confirmacion-destinatario", sDestinatario)
            If sDestinatario = "" Then
                sDestinatario = Session("UserB2C")
            End If
            ''Obtenemos el nombre de la empresa
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre,ISNULL(cvCliente,'') as Cliente from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)

            objDatos.fnLog("Confirmacion-cliente", dtcliente.Rows(0)(0))
            If CDbl(Session("ImporteEnvio")) > 0 Then
                If Session("NombrePaqueteria") <> "" Then
                    lblMetodoEnvio.Text = lblMetodoEnvio.Text & " Paquetería: " & Session("NombrePaqueteria") & " (Guía: " & Session("GuiaSolicitada") & " )"
                End If
            Else
                ' Session("MetodoEnvio")
                lblMetodoEnvio.Text = lblMetodoEnvio.Text & " " & Session("MetodoEnvio")
            End If
            text = text.Replace("{nombrecliente}", sNombredelCliente)
            text = text.Replace("{enviara}", lblNombreEnvio.Text)
            text = text.Replace("{direccionenvio}", sDireccionEnvio & "</br> " & Session("Comentarios"))
            text = text.Replace("{metodoenvio}", lblMetodoEnvio.Text)
            text = text.Replace("{numpedido}", Session("NoPedido"))
            text = text.Replace("{docnumSAP}", Session("DocNumCompraSAP"))
            text = text.Replace("{fechapedido}", Now.Date.ToShortDateString)
            text = text.Replace("{TotalEnvio}", CDbl(Session("TotalCarrito")).ToString("###,###,###.0#"))
            text = text.Replace("{Envio}", CDbl(Session("ImporteEnvio")).ToString("###,###,###.0#"))
            text = text.Replace("{Total}", (CDbl(Session("TotalCarrito")) - CDbl(Session("ImporteEnvio"))).ToString("###,###,###.0#"))
            text = text.Replace("{TotalImpuestos}", (CDbl(Session("TotalImpuestos"))).ToString("###,###,###.0#"))
            text = text.Replace("{TotalMasImpuestos}", (CDbl(Session("TotalImpuestos")) + CDbl(Session("TotalCarrito"))).ToString("###,###,###.0#"))
            ''Ahora las líneas
            text = text.Replace("{lineas}", fnGeneraHTMLPartidas)


            objDatos.fnLog("Confirmacion", "Antes de enviar correo")

            objDatos.fnEnviarCorreo(sDestinatario, text, dtcliente.Rows(0)(0) & "- ¡Gracias por tu compra!")


            objDatos.fnLog("Confirmacion", "Pre-Guardado de guía - Inicia proceso")
            Try
                ''----Pre-Guardado de formato para las Paqueterías desde SAP^-----

                ''        Armamos HTML de Paqueterías
                textPaquetería = textPaquetería.Replace("{nombrecliente}", sNombredelCliente)
                textPaquetería = textPaquetería.Replace("{enviara}", lblNombreEnvio.Text)
                textPaquetería = textPaquetería.Replace("{direccionenvio}", sDireccionEnvio & "</br> " & Session("Comentarios"))
                textPaquetería = textPaquetería.Replace("{metodoenvio}", lblMetodoEnvio.Text)
                textPaquetería = textPaquetería.Replace("{numpedido}", Session("NoPedido"))
                textPaquetería = textPaquetería.Replace("{fechapedido}", Now.Date.ToShortDateString)
                textPaquetería = textPaquetería.Replace("{TotalEnvio}", CDbl(Session("TotalCarrito")).ToString("###,###,###.0#"))
                textPaquetería = textPaquetería.Replace("{Envio}", CDbl(Session("ImporteEnvio")).ToString("###,###,###.0#"))
                textPaquetería = textPaquetería.Replace("{Total}", (CDbl(Session("TotalCarrito")) - CDbl(Session("ImporteEnvio"))).ToString("###,###,###.0#"))

                ''Regalo
                textPaquetería = textPaquetería.Replace("{Regalo}", "<h4>Mensaje de Regalo</h4><br>" & Session("MensajeRegalo"))
                ''Ahora las líneas
                textPaquetería = textPaquetería.Replace("{lineas}", fnGeneraHTMLPartidas)

                ''Ahora registramos en la tabla, reemplazando las comillas por arrobas

                ssql = "INSERT INTO config.EnvioGuias  (ciNoPedido,cvCorreo,cvContenido,cvEstatus) VALUES (" _
                    & "'" & Session("NoPedido") & "', " _
                    & "'" & sDestinatario & "', " _
                    & "'" & textPaquetería.Replace("'", "@") & "'," _
                    & "'PENDIENTE GUIA')"

                objDatos.fnEjecutarInsert(ssql)

                ''Validamos que ese pedido se haya guardado correctamente. lo mandamos al log para monitorear
                ssql = "select ciNoPedido,cvCorreo,cvContenido,cvEstatus from config.EnvioGuias where ciNoPedido=" & "'" & Session("NoPedido") & "'"
                Dim dtNumPedidoGuardadoGuia As New DataTable
                dtNumPedidoGuardadoGuia = objDatos.fnEjecutarConsulta(ssql)
                objDatos.fnLog("Confirmacion", "Pre-Guardado de guía - Termina proceso almacenando:" & dtNumPedidoGuardadoGuia.Rows.Count)

            Catch ex As Exception
                objDatos.fnLog("Confirmacion", "Pre-Guardado de guía - Termina proceso con error:" & ex.Message.Replace("'", ""))
            End Try
            ''----TERMINA Pre-Guardado de formato para las Paqueterías desde SAP^-----









            If CStr(dtcliente.Rows(0)("Cliente")).ToUpper = "SALAMA" Then
                objDatos.fnEnviarCorreo("pedidos@stopbasico.com", text, dtcliente.Rows(0)(0) & "- ¡Gracias por tu compra!")
            End If
            ''Revisamos si notificamos a alguien de la empresa
            ssql = "select ISNULL(cvCorreoNotifica,'') FROM config.parametrizaciones_b2c "
            Dim dtCorreoInterno As New DataTable
            dtCorreoInterno = objDatos.fnEjecutarConsulta(ssql)
            If dtCorreoInterno.Rows.Count > 0 Then
                If dtCorreoInterno.Rows(0)(0) <> "" Then
                    objDatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, dtcliente.Rows(0)(0) & "- Agradece tu compra!")
                End If
            End If

            ''Revisamos si tenemos que mandar "descargar o imprimir" un formato PDF basado en Crystal
            Try
                ssql = "SELECT ISNULL(cvFormatoImpresion,'') FROM config.parametrizaciones_b2c"
                Dim dtFormato As New DataTable
                dtFormato = objDatos.fnEjecutarConsulta(ssql)
                If dtFormato.Rows.Count > 0 Then
                    If dtFormato.Rows(0)(0) <> "" Then
                        ''si tenemos formato
                        objDatos.fnLog("Comprobante-formato", dtFormato.Rows(0)(0) & " docEntry: " & Session("DocEntry"))
                        fndescargaPDF(Session("DocEntry"), dtFormato.Rows(0)(0))
                    End If
                End If
            Catch ex As Exception
                objDatos.fnLog("Comprobante", ex.Message)
            End Try



            fnCargarCarrito()
            fnTotales()
            Try
                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
            Catch ex As Exception

            End Try

        End If




    End Sub

    Public Function fnActualizaDireccionCliente(CardCode As String, Direccion As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
        Else
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
        End If
        Dim sAdressName As String
        sAdressName = "Envio-" & sIdDireccion

        objDatos.fnLog("Actualizando direccion", sAdressName)


        Dim sQueryDirecciones As String = ""

        sQueryDirecciones = objDatos.fnObtenerQuery("DireccionesSN")

        If sQueryDirecciones <> "" Then
            sQueryDirecciones = sQueryDirecciones.Replace("[%0]", CardCode)


            Dim dtDirecciones As New DataTable
            dtDirecciones = objDatos.fnEjecutarConsultaSAP(sQueryDirecciones)

            If dtDirecciones.Rows.Count > 0 Then
                Dim oProspecto As SAPbobsCOM.BusinessPartners
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.GetByKey(CardCode)
                For i = 0 To dtDirecciones.Rows.Count - 1 Step 1

                    oProspecto.Addresses.SetCurrentLine(i)
                    If oProspecto.Addresses.AddressName = sAdressName Then
                        oProspecto.Addresses.Block = Session("ColoniaEnvio")
                        oProspecto.Addresses.County = Session("MunicipioEnvio")

                        oProspecto.Addresses.StreetNo = Session("NumExtEnvio")
                        oProspecto.Addresses.Street = CStr(Session("CalleEnvio"))
                        oProspecto.Addresses.State = Session("EstadoEnvio")
                        oProspecto.Addresses.ZipCode = Session("CPEnvio")
                        If oProspecto.Update() <> 0 Then
                            sIdDireccion = "ERROR"

                        End If
                        Exit For
                        sIdDireccion = sAdressName
                    End If



                Next
            End If


        End If



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function

    Public Sub fnAgregaFletesSeguros_StopCatalogo()



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
        Dim iMontoPorSeguro As Double = 15 ' / 1.16
        Dim iMontoPorFlete As Double = 60 ' / 1.16
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
        ' lblCarrito.Text = "Carrito de compras (" & Session("LeyendaDescuento") & ")"

    End Sub

    Public Function fnActualizaDireccionFiscalCliente(CardCode As String, Direccion As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
        Else
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
        End If
        Dim sAdressName As String
        sAdressName = "Envio-" & sIdDireccion

        sIdDireccion = ""
        objDatos.fnLog("Actualizando direccion", sAdressName)


        Dim sQueryDirecciones As String = ""

        sQueryDirecciones = objDatos.fnObtenerQuery("DireccionesSN")

        If sQueryDirecciones <> "" Then
            sQueryDirecciones = sQueryDirecciones.Replace("[%0]", CardCode)


            Dim dtDirecciones As New DataTable
            dtDirecciones = objDatos.fnEjecutarConsultaSAP(sQueryDirecciones)

            If dtDirecciones.Rows.Count > 0 Then
                Dim oProspecto As SAPbobsCOM.BusinessPartners
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.GetByKey(CardCode)
                For i = 0 To dtDirecciones.Rows.Count - 1 Step 1

                    oProspecto.Addresses.SetCurrentLine(i)
                    If oProspecto.Addresses.AddressName = "Facturación" Then
                        oProspecto.Addresses.Block = Session("ColoniaFiscal")
                        oProspecto.Addresses.County = Session("MunicipioFiscal")

                        oProspecto.Addresses.StreetNo = Session("NumExtFiscal")
                        oProspecto.Addresses.Street = CStr(Session("CalleFiscal"))
                        oProspecto.Addresses.State = Session("EstadoFiscal")
                        oProspecto.Addresses.ZipCode = Session("CPFiscal")

                        If oProspecto.Update() <> 0 Then
                            sIdDireccion = "ERROR"

                        End If
                        Exit For
                        sIdDireccion = "Facturación"
                    End If



                Next
            End If


        End If



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function


    Public Function fnAgregarDireccionCliente(CardCode As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        Try
            If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
                sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
            Else
                sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
            End If
            Dim sAdressName As String
            sAdressName = "Envio-" & sIdDireccion

            objDatos.fnLog("Agregar direccion", sAdressName)



            Dim oProspecto As SAPbobsCOM.BusinessPartners

            oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oProspecto.GetByKey(CardCode)
            oProspecto.Addresses.Add()
            oProspecto.Addresses.TypeOfAddress = "S"
            oProspecto.Addresses.AddressName = sAdressName
            oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
            oProspecto.Addresses.Block = Session("ColoniaEnvio")
            oProspecto.Addresses.County = Session("MunicipioEnvio")
            oProspecto.Addresses.ZipCode = Session("CPEnvio")

            oProspecto.Addresses.StreetNo = Session("NumExtEnvio")
            oProspecto.Addresses.Street = CStr(Session("CalleEnvio"))
            oProspecto.Addresses.State = Session("EstadoEnvio")
            '  oProspecto.Addresses.Country = ddlPais.SelectedValue
            If oProspecto.Update() <> 0 Then
                sIdDireccion = "ERROR"

            Else
                sIdDireccion = sAdressName
            End If
        Catch ex As Exception
            sIdDireccion = "ERROR"

            objDatos.fnLog("Agregar direccion-exception", ex.Message.Replace("'", ""))
        End Try



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function

    Public Sub fndescargaPDF(DocEntry As Int32, Formato As String)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objDatos.fnLog("Comprobante", Server.MapPath("~") & "\" & Formato)
        reporte.Load(Server.MapPath("~") & "\" & Formato)

        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            reporte.SetParameterValue("DocKey@", Session("DocEntry"))
            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If



        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next
        reporte.Refresh()
        reporte.SetParameterValue("DocKey@", DocEntry)
        Session("ArchivoMostrar") = "PED-" & DocEntry & ".pdf"
        reporte.ExportToDisk(ExportFormatType.PortableDocFormat, "PED-" & DocEntry & ".pdf")
        '   reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "PED-" & DocEntry)
        reporte.Dispose()
        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub


    Public Sub fnGeneraSAP(idCarrito As String, TipoDoc As String)
        Dim sCardCode As String = ""
        ssql = "select ISNULL(cvPreciosMasIVA,'NO') from config.parametrizaciones"
        Dim dtPreciosIVA As New DataTable
        dtPreciosIVA = objDatos.fnEjecutarConsulta(ssql)
        Dim sPreciosMasIVA As String = "NO"
        If dtPreciosIVA.Rows.Count > 0 Then
            sPreciosMasIVA = dtPreciosIVA.Rows(0)(0)
        End If
        objDatos.fnLog("GeneraSAP", idCarrito)


        Dim sCardCodeSAP As String = ""
        Dim dtCreaSAP As New DataTable

        If Session("usrInvitado") = "SI" Then
            ''Entró con usuario invitado, entonces revisamos en las parametrizaciónes del B2C, el CardCode de SAP que debemos usar (El genérico)
            ssql = " SELECT ISNULL(cvCardCodeSAP ,'') as ClienteGenerico from config.Parametrizaciones_B2C"
            Dim dtClienteGen As New DataTable
            dtClienteGen = objDatos.fnEjecutarConsulta(ssql)
            If dtClienteGen.Rows.Count > 0 Then
                sCardCodeSAP = dtClienteGen.Rows(0)(0)

                ''Ahora realizamos un ajuste, para cambiar el RFC/RUT en dicho cliente primero, antes de hacer los documentos en SAP
                If Session("RFCEnvio") <> "" Then
                    fnActualizaRFCSAP(sCardCodeSAP, Session("RFCEnvio"))
                Else
                    Session("RFCEnvio") = "XEXX010101000"
                    ''Mandamos al log para revisar el caso
                    objDatos.fnLog("Al intentar actualizart RFC", "Variable de session de RFC vacía, actualizada a XEXX010101000")
                End If

            End If
        Else
            ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "

            dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
            objDatos.fnLog("Genera SAP", ssql.Replace("'", ""))
            If dtCreaSAP.Rows.Count > 0 Then
                If dtCreaSAP.Rows(0)(0) = "SI" Then
                    ssql = "SELECT ISNULL(cvCardCode,'') FROM Config.usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'"
                    Dim dtClienteSAP As New DataTable
                    dtClienteSAP = objDatos.fnEjecutarConsulta(ssql)
                    objDatos.fnLog("Genera SAP", ssql.Replace("'", ""))

                    If Session("VieneB2B") = "SI" Then
                        sCardCodeSAP = Session("Cliente")
                    Else
                        sCardCodeSAP = dtClienteSAP.Rows(0)(0)
                    End If
                    If Session("RFCEnvio") <> "" Then
                        fnActualizaRFCSAP(sCardCodeSAP, Session("RFCEnvio"))
                    Else
                        Session("RFCEnvio") = "XEXX010101000"
                        ''Mandamos al log para revisar el caso
                        objDatos.fnLog("Al intentar actualizart RFC", "Variable de session de RFC vacía, actualizada a XEXX010101000")
                    End If

                    objDatos.fnLog("Genera SAP", sCardCodeSAP)
                Else
                    ''Tiene un cardCode Generico
                    ssql = "SELECT cvCardCodeSAP FROM Config.Parametrizaciones_B2C"
                    Dim dtCardCode As New DataTable
                    dtCardCode = objDatos.fnEjecutarConsulta(ssql)
                    If dtCardCode.Rows.Count > 0 Then
                        sCardCodeSAP = dtCardCode.Rows(0)(0)
                    End If

                End If
            End If
        End If


        Dim sIVA As String = ""

        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus FROM Tienda.Pedido_HDR WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objDatos.fnEjecutarConsulta(ssql)

        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtPartidas As New DataTable
        dtPartidas = objDatos.fnEjecutarConsulta(ssql)
        Dim sArticulosSinStock As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Dim sClienteSAP As String = ""
        Try
            ''Validamos primero las existencias
            Dim iBandConexistencias As Int16 = 0

            For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                If fnRevisaExistencias(dtPartidas.Rows(i)("cvItemCode")) = 0 Then
                    iBandConexistencias = 1
                End If
            Next

            oCompany = objDatos.fnConexion_SAP
            If oCompany.Connected Then
                If TipoDoc = "OFERTA DE VENTA" Then
                    objDatos.fnLog("Cotizacion", "Crea objeto Company")
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                End If
                If TipoDoc = "ORDEN DE VENTA" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                    oDoctoVentas.DocDueDate = Now.Date

                    objDatos.fnLog("Genera SAP", "Orden de venta")
                    ' oDoctoVentas.Series = 59
                End If
                If TipoDoc = "FACTURA" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)


                    'If iBandConexistencias = 0 Then
                    '    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
                    '    oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                    'Else
                    '    ''Si se desea que se realice FACTURA pero hay productos sin existencia, generamos una orden y marcamos el tipo de pago a cuenta; dado que no se puede pagar si no hay factura
                    '    'sTipoPago = "A CUENTA"
                    '    'oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                    'End If


                    oDoctoVentas.DocDueDate = Now.Date

                    ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
                    Dim dtcliente As New DataTable
                    dtcliente = objDatos.fnEjecutarConsulta(ssql)
                    If dtcliente.Rows.Count > 0 Then
                        If CStr(dtcliente.Rows(0)(0)).Contains("Salama") Then
                            sClienteSAP = "SALAMA"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            oDoctoVentas.Series = 527
                        End If
                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("BOSS") Then
                            sClienteSAP = "BOSS"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                        End If
                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("HAWK") Then
                            sClienteSAP = "HAWK"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            oDoctoVentas.Series = 83
                            '  oDoctoVentas.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocNotRelevant
                        End If

                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("ALTUR") Then
                            sClienteSAP = "ALTURA"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            '  oDoctoVentas.Series = 83
                            '  oDoctoVentas.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocNotRelevant
                        End If


                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("STOP CAT") Then
                            sClienteSAP = "STOP_CAT"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            oDoctoVentas.Series = 140
                        End If

                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("PEGADUR") Then
                            sClienteSAP = "PEGADURO"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            '  oDoctoVentas.Series = 140
                        End If

                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("AIO") Then
                            sClienteSAP = "AIO"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            '  oDoctoVentas.Series = 140
                        End If
                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("PMK") Then
                            sClienteSAP = "PMK"
                            oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                            '  oDoctoVentas.Series = 140
                        End If


                    End If

                    ' 
                End If

                ' oDoctoVentas.SalesPersonCode = dtEncabezado.Rows(0)("ciIdAgenteSAP")



                oDoctoVentas.CardCode = sCardCodeSAP
                oDoctoVentas.DocDate = Now.Date

                oDoctoVentas.NumAtCard = idCarrito
                oDoctoVentas.CardName = Session("NombreEnvio")

                Dim sDestinatario As String = ""
                If Session("usrInvitado") = "SI" Then
                    sDestinatario = Session("CorreoInvitado")
                Else
                    ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre,cvPass,ISNULL(cvMail,'') as Mail,cvUsuario FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'  and cvTipoAcceso='B2C' "
                    Dim dtLogin As New DataTable
                    dtLogin = objDatos.fnEjecutarConsulta(ssql)

                    If Session("VieneB2B") = "SI" Then
                        sDestinatario = Session("CorreoClienteB2B")
                    Else
                        sDestinatario = dtLogin.Rows(0)("Mail")
                    End If

                End If
                objDatos.fnLog("Confirmacion-destinatario", sDestinatario)
                If sDestinatario = "" Then
                    sDestinatario = Session("UserB2C")
                End If


                If sClienteSAP = "SALAMA" Or sClienteSAP = "BOSS" Or sClienteSAP = "HAWK" Or sClienteSAP = "STOP_CAT" Or sClienteSAP = "PMK" Or sClienteSAP = "AIO" Then
                    If Session("UsoCFDI") <> "" Then
                        oDoctoVentas.CardName = Session("NombreClienteFiscal")
                        oDoctoVentas.FederalTaxID = Session("RFCEnvio")
                        Try
                            If sClienteSAP = "BOSS" Then
                                oDoctoVentas.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = Session("UsoCFDI")
                            End If

                        Catch ex As Exception

                        End Try
                    End If
                End If
                ''Agregamos la direccion de envio
                Dim sAdressName As String = ""
                Try
                    If dtCreaSAP.Rows.Count > 0 Then
                        If dtCreaSAP.Rows(0)(0) = "SI" Then
                            objDatos.fnLog("dirección Decimal entrega", "inicia")
                            Dim sIdDireccion As String = ""
                            If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
                                sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
                            Else
                                sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
                            End If

                            sAdressName = "Envio-" & sIdDireccion

                            ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                            ssql = ssql.Replace("[%0]", sCardCodeSAP).Replace("[%1]", sAdressName)
                            Dim dtDetalleDireccion As New DataTable
                            dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtDetalleDireccion.Rows.Count > 0 Then
                                ''Ya existe
                                fnActualizaDireccionCliente(sCardCodeSAP, sAdressName, oCompany)
                                objDatos.fnLog("dirección Decimal entrega", "Ya existe:" & sAdressName.Replace("'", ""))
                            Else
                                ''La creamos
                                objDatos.fnLog("dirección Decimal entrega", "se va a crear")
                                sAdressName = fnAgregarDireccionCliente(sCardCodeSAP, oCompany)
                            End If
                        End If
                    Else
                        ''Es invitado

                    End If
                Catch ex As Exception

                End Try




                If sAdressName.Contains("ERROR") Or sAdressName = "" Then
                    ''No se pudo crear
                    objDatos.fnLog("dirección Decimal entrega", "NO se pudo crear")
                    Try
                        oDoctoVentas.Address2 = lblNombreEnvio.Text & " " & lblCalleyNum.Text & " " & lblColoniaMunic.Text & " " & lblEstadoCP.Text
                    Catch ex As Exception

                    End Try
                Else
                    objDatos.fnLog("dirección Decimal entrega", "Asignamos: " & sAdressName)
                    oDoctoVentas.ShipToCode = sAdressName
                End If
                If Session("CalleFiscal") <> "" Then
                    sAdressName = fnActualizaDireccionFiscalCliente(sCardCodeSAP, sAdressName, oCompany)
                    If sAdressName.Contains("ERROR") Or sAdressName = "" Then
                        ''No se pudo crear
                        objDatos.fnLog("dirección fiscal", "NO se pudo crear")
                        Try
                            oDoctoVentas.Address = Session("CalleFiscal") & " " & Session("NumExtFiscal") & " " & Session("ColoniaFiscal") & " " & Session("CiudadFiscal") & " " & Session("MunicipioFiscal") & " " & Session("CPFiscal") & " " & Session("EstadoFiscal")
                        Catch ex As Exception

                        End Try
                    Else
                        oDoctoVentas.PayToCode = sAdressName
                    End If
                End If

                Try

                    ssql = "SELECT * FROM OSHP where TrnspCode='13'"
                    Dim dtTrans As New DataTable
                    dtTrans = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtTrans.Rows.Count > 0 Then
                        oDoctoVentas.Project = 1
                        oDoctoVentas.TransportationCode = 13
                    End If

                Catch ex As Exception

                End Try
                oDoctoVentas.Comments = "Desde Internet - " & Session("Comentarios") & sDestinatario

                objDatos.fnLog("Genera SAP", "Antes cveCliente")
                sCardCode = dtEncabezado.Rows(0)("cvCveCliente")
                Try
                    ''Obtener RFC
                    ssql = "SELECT ISNULL(VatIdUnCmp ,'0'),ISNULL(CardName,'') FROM OCRD WHERE CardCode=" & "'" & sCardCode & "'"
                    Dim dtRTN As New DataTable
                    dtRTN = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtRTN.Rows.Count > 0 Then
                        oDoctoVentas.UserFields.Fields.Item("U_RTN_Cli").Value = dtRTN.Rows(0)(0)
                        oDoctoVentas.UserFields.Fields.Item("U_CardName").Value = dtRTN.Rows(0)(1)
                    Else
                        oDoctoVentas.UserFields.Fields.Item("U_RTN_Cli").Value = "7268540-9"
                    End If


                    oDoctoVentas.UserFields.Fields.Item("U_CardCode").Value = sCardCode

                Catch ex As Exception

                End Try

                Try
                    If Session("UsoCFDI") <> "" Then
                        oDoctoVentas.UserFields.Fields.Item("U_UsoCFDI").Value = Session("UsoCFDI")
                    End If
                    If Session("FormaPagoCFDI") <> "" Then
                        oDoctoVentas.UserFields.Fields.Item("U_FormaDePago").Value = Session("FormaPagoCFDI")
                    End If

                Catch ex As Exception

                End Try

                Dim sAlmacen As String = ""

                Dim iLinea As Int16 = 0
                Dim sArticulosPedido As String = ""
                objDatos.fnLog("Genera SAP", "Antes partidas")

                Try
                    If sClienteSAP = "STOP_CAT" Then
                        oDoctoVentas.UserFields.Fields.Item("U_MensajeRegalo").Value = Session("MensajeRegalo")
                    End If

                Catch ex As Exception

                End Try

                For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                    oDoctoVentas.Lines.Add()
                    oDoctoVentas.Lines.SetCurrentLine(iLinea)
                    'If fnRevisaExistencias(dtPartidas.Rows(i)("cvItemCode")) = 0 Then
                    '    sArticulosSinStock = sArticulosSinStock & dtPartidas.Rows(i)("cvItemCode") & vbCrLf
                    'End If
                    objDatos.fnLog("Genera SAP-ItemCode", dtPartidas.Rows(i)("cvItemCode"))
                    oDoctoVentas.Lines.ItemCode = dtPartidas.Rows(i)("cvItemCode")
                    oDoctoVentas.Lines.ItemDescription = dtPartidas.Rows(i)("cvItemName")
                    oDoctoVentas.Lines.Quantity = dtPartidas.Rows(i)("cfCantidad")
                    '   oDoctoVentas.Lines.Price = dtPartidas.Rows(i)("cfPrecio")
                    Try
                        If sClienteSAP = "STOP_CAT" Then
                            oDoctoVentas.Lines.UserFields.Fields.Item("U_PBASE").Value = dtPartidas.Rows(i)("cfPrecio")
                        End If
                    Catch ex As Exception

                    End Try


                    objDatos.fnLog("Precios + IVA", sPreciosMasIVA)
                    If sPreciosMasIVA = "SI" Then

                        objDatos.fnLog("Precio con IVA:", dtPartidas.Rows(i)("cfPrecio"))
                        If sClienteSAP = "SALAMA" Then
                            oDoctoVentas.Lines.UnitPrice = CDbl(dtPartidas.Rows(i)("cfPrecio")) / 1.16
                            objDatos.fnLog("Precio con IVA:", CDbl(dtPartidas.Rows(i)("cfPrecio")) / 1.16)
                        Else
                            oDoctoVentas.Lines.UnitPrice = CDbl(dtPartidas.Rows(i)("cfPrecio")) / 1.16
                            ' oDoctoVentas.Lines.PriceAfterVAT = dtPartidas.Rows(i)("cfPrecio")
                        End If

                    Else
                        objDatos.fnLog("Precio SIN IVA:", dtPartidas.Rows(i)("cfPrecio"))
                        oDoctoVentas.Lines.UnitPrice = dtPartidas.Rows(i)("cfPrecio")
                    End If

                    Try
                        If sClienteSAP.ToUpper.Contains("HAWK") Then
                            '  objDatos.fnLog("Moneda factura reserva:", dtPartidas.Rows(i)("cvMoneda"))
                            oDoctoVentas.Lines.Currency = fnbtenerMonedaCarrito(dtPartidas.Rows(i)("cvItemCode"))
                        End If
                    Catch ex As Exception

                    End Try


                    ''Dimensiones HAWK
                    Try
                        If sClienteSAP = "HAWK" Then
                            oDoctoVentas.Lines.CostingCode = "UN_TV"
                            oDoctoVentas.Lines.CostingCode2 = "TV_001"
                            oDoctoVentas.Lines.CostingCode3 = "TV_A005"

                        End If
                    Catch ex As Exception

                    End Try


                    ''Obtener Indicador de IVA
                    ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", dtPartidas.Rows(i)("cvItemCode"))
                        Dim dtIVA As New DataTable
                        dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            objDatos.fnLog("Genera SAP-Impuesto", dtIVA.Rows(0)(0))
                            oDoctoVentas.Lines.TaxCode = dtIVA.Rows(0)(0)
                            sIVA = dtIVA.Rows(0)(0)
                        End If

                    End If

                    objDatos.fnLog("Genera SAP", "Antes descuento")
                    If CDbl(dtPartidas.Rows(i)("cfDescuento")) > 0 Then
                        objDatos.fnLog("Descto:", CDbl(dtPartidas.Rows(i)("cfDescuento")))
                        oDoctoVentas.Lines.DiscountPercent = dtPartidas.Rows(i)("cfDescuento")
                    End If


                    sAlmacen = fnAsignaAlmacen(dtPartidas.Rows(i)("cvItemCode"))
                    If sAlmacen <> "" Then
                        oDoctoVentas.Lines.WarehouseCode = sAlmacen
                        objDatos.fnLog("Genera SAP-Almacen", sAlmacen)
                    End If

                    ''Vemos si tenemos centro de costos
                    Dim MislpCode As String = "-1"
                    ssql = "SELECT ISNULL(cvEmpleadoVentas,'-1') FROM config.parametrizaciones_B2C"
                    Dim dtSlp As New DataTable
                    dtSlp = objDatos.fnEjecutarConsulta(ssql)
                    If dtSlp.Rows.Count > 0 Then
                        MislpCode = dtSlp.Rows(0)(0)
                    End If

                    Dim sQueryCC As String = ""
                    sQueryCC = objDatos.fnObtenerQuery("ObtenerCentroCostos")
                    If sQueryCC <> "" Then
                        objDatos.fnLog("Genera SAP", "ObtenerCentroCostos")
                        Try
                            objDatos.fnLog("CentroCostos", sQueryCC.Replace("'", ""))
                            If CInt(Session("slpCode")) = 0 Then
                                sQueryCC = sQueryCC.Replace("[%0]", MislpCode)
                            Else
                                sQueryCC = sQueryCC.Replace("[%0]", Session("slpCode"))
                            End If

                            sQueryCC = sQueryCC.Replace("[%1]", dtPartidas.Rows(i)("cvCveCliente"))
                        Catch ex As Exception

                        End Try


                        Dim sCentroCostos As String = ""
                        Dim dtCentro As New DataTable
                        dtCentro = objDatos.fnEjecutarConsultaSAP(sQueryCC)
                        If dtCentro.Rows.Count > 0 Then
                            sCentroCostos = dtCentro.Rows(0)(0)
                            oDoctoVentas.Lines.CostingCode = sCentroCostos
                        End If

                    End If

                    Try

                        Dim sQuerySuc As String = ""
                        sQuerySuc = objDatos.fnObtenerQuery("ObtenerSucursal")
                        If sQuerySuc <> "" Then
                            objDatos.fnLog("Genera SAP", "ObtenerSucursal")
                            Try
                                If CInt(Session("slpCode")) = 0 Then
                                    sQuerySuc = sQuerySuc.Replace("[%0]", MislpCode)
                                Else
                                    sQuerySuc = sQuerySuc.Replace("[%0]", CInt(Session("slpCode")))
                                End If

                                sQuerySuc = sQuerySuc.Replace("[%1]", dtEncabezado.Rows(0)("cvCliente"))
                            Catch ex As Exception

                            End Try

                            objDatos.fnLog("Sucursal", sQuerySuc.Replace("'", ""))
                            Dim sSucursalVend As String = ""
                            Dim dtSucursalVend As New DataTable
                            dtSucursalVend = objDatos.fnEjecutarConsultaSAP(sQuerySuc)
                            If dtSucursalVend.Rows.Count > 0 Then
                                sSucursalVend = dtSucursalVend.Rows(0)(0)
                                oDoctoVentas.UserFields.Fields.Item("U_SUCURSAL").Value = sSucursalVend
                            End If

                        End If


                    Catch ex As Exception

                    End Try


                    ssql = "SELECT ISNULL(cvUsaUbicaciones,'NO') FROM config.parametrizaciones_B2C"
                    Dim dtUbica As New DataTable
                    dtUbica = objDatos.fnEjecutarConsulta(ssql)

                    If dtUbica.Rows.Count > 0 Then
                        If dtUbica.Rows(0)(0) = "SI" Then
                            Dim sQueryUbicaciones As String = ""
                            sQueryUbicaciones = objDatos.fnObtenerQuery("ObtenerUbicacion")
                            sQueryUbicaciones = sQueryUbicaciones.Replace("[%0]", dtPartidas.Rows(i)("cvItemCode"))
                            Dim dtUbicacionSAP As New DataTable
                            dtUbicacionSAP = objDatos.fnEjecutarConsultaSAP(sQueryUbicaciones)
                            If dtUbicacionSAP.Rows.Count > 0 Then
                                oDoctoVentas.Lines.BinAllocations.Add()
                                oDoctoVentas.Lines.BinAllocations.BinAbsEntry = dtUbicacionSAP.Rows(0)(0)
                            End If
                        End If
                    End If

                    Dim iLote As Int16 = 0
                    Dim iCantidad As Double
                    iCantidad = dtPartidas.Rows(i)("cfCantidad")
                    objDatos.fnLog("Genera SAP", "Lotes:" & TipoDoc)
                    If TipoDoc = "FACTURA" Then

                        Dim dtLote As New DataTable
                        ''Comenzamos el proceso para determinar los lotes
                        If objDatos.fnObtenerDBMS = "HANA" Then
                            objDatos.fnLog("Genera SAP", "Lotes: Obtener query")
                            ssql = objDatos.fnObtenerQuery("GetBatches")
                            ssql = ssql.Replace("[%0]", dtPartidas.Rows(i)("cvItemCode"))
                            ssql = ssql.Replace("[%1]", sAlmacen)
                        Else
                            ssql = "select SUM(CASE WHEN direction=1 tHEN Quantity *-1 else Quantity END) AS Cantidad,BatchNum  from IBT1 where ItemCode ='" & dtPartidas.Rows(i)("cvItemCode") & "' AND WhsCode ='" & sAlmacen & "'  Group by BatchNum order by BatchNum  "
                        End If
                        objDatos.fnLog("Genera SAP", ssql.Replace("'", ""))
                        dtLote = objDatos.fnEjecutarConsultaSAP(ssql)
                        ''Una vez que obtenemos los lotes determinamos el mas antiguo y como debe ir consumiento

                        For j = 0 To dtLote.Rows.Count - 1 Step 1
                            objDatos.fnLog("Genera SAP", "Lotes")
                            If CInt(dtLote.Rows(j)("Cantidad")) > 0 Then
                                ''Asignamos el lote
                                oDoctoVentas.Lines.BatchNumbers.Add()
                                oDoctoVentas.Lines.BatchNumbers.SetCurrentLine(iLote)
                                If CDbl(dtLote.Rows(j)("cantidad")) >= iCantidad Then
                                    '  MsgBox("Entra al lote y va a salir" & dtLote.Rows(j)("BatchNum"))
                                    ''Asignamos el lote y cantidad que necesitamos y rompemos el ciclo
                                    '  sLotes = sLotes & dtLote.Rows(j)("BatchNum") & ","
                                    oDoctoVentas.Lines.BatchNumbers.BatchNumber = dtLote.Rows(j)("BatchNum")
                                    oDoctoVentas.Lines.BatchNumbers.Quantity = iCantidad

                                    Exit For
                                Else
                                    ' MsgBox("Entra al lote" & dtLote.Rows(j)("BatchNum"))
                                    ''Tomamos lo que necesitamos del lote y buscamos en otro lote
                                    ' sLotes = sLotes & dtLote.Rows(j)("BatchNum") & ","
                                    oDoctoVentas.Lines.BatchNumbers.BatchNumber = dtLote.Rows(j)("BatchNum")
                                    oDoctoVentas.Lines.BatchNumbers.Quantity = CDbl(dtLote.Rows(j)("cantidad"))
                                    iCantidad = iCantidad - CDbl(dtLote.Rows(j)("cantidad"))


                                End If
                                If iCantidad = 0 Then
                                    Exit For
                                End If
                                iLote = iLote + 1
                                '   oEmision .Lines .BatchNumbers .Quantity 
                            End If
                        Next
                    End If


                    sArticulosPedido = sArticulosPedido & String.Format("{0,200} {1,25}{2}{2}",
                                    dtPartidas.Rows(i)("cvItemName"), dtPartidas.Rows(i)("cfCantidad"), vbCrLf)
                    iLinea = iLinea + 1
                Next

                If sClienteSAP = "SALAMA" Then
                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "FLETE"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.UnitPrice = CDbl(Session("ImporteEnvio")) / 1.16
                        oDoctoVentas.Lines.WarehouseCode = "WEB"
                        If sIVA <> "" Then
                            oDoctoVentas.Lines.TaxCode = sIVA
                        End If

                    End If
                End If

                If sClienteSAP = "STOP_CAT" Then
                    If Session("ImporteEnvio") > 0 Then

                    Else

                        fnAgregaFletesSeguros_StopCatalogo()

                    End If
                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "FLETE-ESTAFETA"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.UnitPrice = CDbl(Session("ImporteEnvio")) / 1.16
                    End If

                    '    oDoctoVentas.Lines.WarehouseCode = "WEB"
                    If sIVA <> "" Then
                        oDoctoVentas.Lines.TaxCode = sIVA
                    End If

                End If



                If sClienteSAP.ToUpper.Contains("BOSS") Then
                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "COID061"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.PriceAfterVAT = CDbl(Session("ImporteEnvio"))
                        oDoctoVentas.Lines.WarehouseCode = "01"
                        'If sIVA <> "" Then
                        '    oDoctoVentas.Lines.TaxCode = sIVA
                        'End If

                    End If
                End If

                If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                    ssql = "SELECT isnull(U_B1SYS_MainUsage, 'G01') FROM OCRD WHERE cardCode= " & " '" & sCardCodeSAP & "'"
                    Dim dtUsoCFDI As New DataTable
                    dtUsoCFDI = objDatos.fnEjecutarConsultaSAP(ssql)

                    If dtUsoCFDI.Rows.Count > 0 Then
                        oDoctoVentas.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = dtUsoCFDI.Rows(0)(0)
                    Else
                        oDoctoVentas.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "G03"
                    End If

                    oDoctoVentas.UserFields.Fields.Item("U_MetodoPago").Value = "PPD"

                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "0003"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.PriceAfterVAT = CDbl(Session("ImporteEnvio"))
                        oDoctoVentas.Lines.WarehouseCode = "01"
                        If sIVA <> "" Then
                            oDoctoVentas.Lines.TaxCode = sIVA
                        End If

                    End If
                End If


                If sClienteSAP.ToUpper.Contains("HAWK") Then
                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "Logisticas"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.PriceAfterVAT = CDbl(Session("ImporteEnvio"))
                        oDoctoVentas.Lines.WarehouseCode = sAlmacen

                        If sIVA <> "" Then
                            oDoctoVentas.Lines.TaxCode = sIVA
                        End If

                    End If
                End If

                If sClienteSAP.ToUpper.Contains("ALTURA") Then
                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "SERV"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.PriceAfterVAT = CDbl(Session("ImporteEnvio"))
                        oDoctoVentas.Lines.WarehouseCode = sAlmacen

                        If sIVA <> "" Then
                            oDoctoVentas.Lines.TaxCode = sIVA
                        End If

                    End If
                End If



                ''Logisticas

                ''Cargamos los anexos
                ''Revisamos si hay anexos por cargar
                Dim oAtt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                Try
                    ssql = "SELECT cvArchivo FROM config.Guias_Paqueteria WHERE cvPedido=" & "'" & idCarrito & "'"
                    Dim dtAnexos As New DataTable
                    dtAnexos = objDatos.fnEjecutarConsulta(ssql)
                    If dtAnexos.Rows.Count > 0 Then
                        ''si hay PDF's
                        For adj = 0 To dtAnexos.Rows.Count - 1 Step 1
                            Dim sArchivo As String = CStr(dtAnexos.Rows(adj)("cvArchivo"))
                            Dim sRuta As String = "c:\inetpub\wwwroot\ecommerce\guias"
                            sArchivo = sArchivo.Replace(sRuta, "")

                            oAtt.Lines.add()
                            oAtt.Lines.setcurrentline(adj)
                            oAtt.Lines.SourcePath = sRuta
                            oAtt.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(sArchivo)
                            oAtt.Lines.FileExtension = System.IO.Path.GetExtension(sArchivo).Substring(1)
                            oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                        Next

                        If oAtt.add() = 0 Then
                            Dim attEntry As Int32
                            attEntry = CInt(oCompany.GetNewObjectKey())

                            oDoctoVentas.AttachmentEntry = attEntry

                        Else
                            ''Registramos el error
                            objDatos.fnLog("ANEXOS ERR", oCompany.GetLastErrorDescription.Replace("'", ""))
                            objDatos.fnEnviarCorreo("ventas@evolutic.mx", oCompany.GetLastErrorDescription.Replace("'", ""), "Error anexos SAP-alcrear")

                        End If
                    End If
                Catch ex As Exception
                    objDatos.fnEnviarCorreo("ventas@evolutic.mx", ex.Message, "Error anexos SAP EX")
                End Try



                objDatos.fnLog("TipoDoc", "antes del Add")

                '   oDoctoVentas.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocNotRelevant
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    objDatos.fnLog(sTipoDoc, "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    '   objDatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription, Me.Page)
                    lblTitulo.Text = "PEDIDO PROCESADO CORRECTAMENTE"
                    objDatos.fnEnviarCorreo("ventas@evolutic.mx", "Error al crear compra en SAP", oCompany.GetLastErrorDescription.Replace("'", ""))
                Else
                    lblTitulo.Text = "PEDIDO PROCESADO CORRECTAMENTE"
                    ''Todo bien
                    Dim FolioSAP As String = ""
                    FolioSAP = oCompany.GetNewObjectKey
                    Try
                        ''Cargamos los anexos
                        ''Revisamos si hay anexos por cargar
                        'Dim oAtt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2)
                        'ssql = "SELECT cvArchivo FROM config.Guias_Paqueteria WHERE cvPedido=" & "'" & idCarrito & "'"
                        'Dim dtAnexos As New DataTable
                        'dtAnexos = objDatos.fnEjecutarConsulta(ssql)
                        'If dtAnexos.Rows.Count > 0 Then
                        '    ''si hay PDF's
                        '    For adj = 0 To dtAnexos.Rows.Count - 1 Step 1
                        '        Dim sArchivo As String = CStr(dtAnexos.Rows(adj)("cvArchivo"))
                        '        Dim sRuta As String = "c:\inetpub\wwwroot\ecommerce\guias"
                        '        sArchivo = sArchivo.Replace(sRuta, "")

                        '        oAtt.Lines.add()
                        '        oAtt.Lines.setcurrentline(adj)
                        '        oAtt.Lines.SourcePath = sRuta
                        '        oAtt.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(sArchivo)
                        '        oAtt.Lines.FileExtension = System.IO.Path.GetExtension(sArchivo).Substring(1)
                        '        oAtt.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES
                        '    Next


                        'End If
                        'If oAtt.add() = 0 Then
                        '    Dim attEntry As Int32
                        '    attEntry = CInt(oCompany.GetNewObjectKey())
                        '    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
                        '    oDoctoVentas.GetByKey(FolioSAP)
                        '    oDoctoVentas.AttachmentEntry = attEntry
                        '    If oDoctoVentas.Update() <> 0 Then
                        '        objDatos.fnLog("ANEXOS en documento ERR", oCompany.GetLastErrorDescription.Replace("'", ""))
                        '        objDatos.fnEnviarCorreo("ventas@evolutic.mx", oCompany.GetLastErrorDescription.Replace("'", ""), "Error anexos SAP")
                        '    End If
                        'Else
                        '    ''Registramos el error
                        '    objDatos.fnLog("ANEXOS ERR", oCompany.GetLastErrorDescription.Replace("'", ""))
                        '    objDatos.fnEnviarCorreo("ventas@evolutic.mx", oCompany.GetLastErrorDescription.Replace("'", ""), "Error anexos SAP-alcrear")

                        'End If
                    Catch ex As Exception

                    End Try
                    Dim sDocnum As String = ""
                    Dim dtDoc As New DataTable
                    Session("DocEntry") = FolioSAP
                    If TipoDoc = "ORDEN DE VENTA" Then
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumOrdenVentas")
                        ssql = ssql.Replace("[%0]", FolioSAP)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)

                    End If
                    If TipoDoc = "OFERTA DE VENTA" Then
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumOfertaVentas")
                        ssql = ssql.Replace("[%0]", FolioSAP)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If TipoDoc = "FACTURA" Then
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumFactura")
                        ssql = ssql.Replace("[%0]", FolioSAP)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If dtDoc.Rows.Count > 0 Then
                        sDocnum = dtDoc.Rows(0)(0)
                        ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                        objDatos.fnEjecutarInsert(ssql)

                    End If
                    If dtDoc.Rows.Count > 0 Then
                        Session("DocNumCompraSAP") = sDocnum
                        sDocnum = dtDoc.Rows(0)(0)
                        lblFolio.Text = sDocnum

                    End If
                    Dim DocEntryDocumento As Int64 = FolioSAP
                    If sAplicaPago = "SI" Then
                        objDatos.fnLog("Genera SAP", "aplica pago")
                        Dim fTotalDoc As Double = 0

                        ssql = "SELECT docTotal FROM OINV where docentry=" & "'" & FolioSAP & "'"
                        Dim dtTotal As New DataTable
                        dtTotal = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtTotal.Rows.Count > 0 Then
                            fTotalDoc = dtTotal.Rows(0)(0)
                        Else
                            fTotalDoc = Session("ImporteSubTotal") + Session("ImporteEnvio") + Session("ImporteDescuento")
                        End If


                        fnGeneraPagoSAP(sCardCodeSAP, DocEntryDocumento, fTotalDoc, sTipoPago, idCarrito, sDocnum)
                    End If
                    '  objDatos.fnLog("Confirmación", "Sale Genera Pago SAP")

                    '   objDatos.fnLog("Confirmación", "Resetea variables")



                    If sArticulosSinStock <> "" Then
                        '  objDatos.fnLog("Confirmación articulos sin stock", "Entra")
                        ''Tenemos articulos sin stock...notificamos
                        ''Obtenemos correo de notificacion
                        ssql = "select ISNULL(cvNotificaExistencia,'NO'),ISNULL(cvCorreoNotificaExistencia,'') FROM config.parametrizaciones"
                        Dim dtNotifica As New DataTable
                        dtNotifica = objDatos.fnEjecutarConsulta(ssql)
                        If dtNotifica.Rows.Count > 0 Then
                            If dtNotifica.Rows(0)(0) <> "NO" Then
                                Dim sBody As String = ""
                                sBody = "Se ha generado un " & TipoDoc & " desde el portal de Ecommerce: " & sDocnum & vbCrLf
                                sBody = sBody & "Sin embargo, los siguientes artículos no tienen stock:" & vbCrLf
                                sBody = sBody & sArticulosSinStock
                                If dtNotifica.Rows(0)(1) <> "" Then
                                    objDatos.fnEnviarCorreo(dtNotifica.Rows(0)(1), sBody, "Notificaciones ECommerce: Artículos sin stock")
                                End If

                            End If
                        End If
                    End If


                End If



            Else
                objDatos.fnEnviarCorreo("ventas@evolutic.mx", "Error al crear compra en SAP", "No se ha podido establecer conexión con SAP: " & oCompany.GetLastErrorDescription.Replace("'", ""))
                '   objDatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)
            End If

        Catch ex As Exception
            objDatos.fnEnviarCorreo("ventas@evolutic.mx", "Error al crear compra en SAP", "No se ha podido establecer conexión con SAP EX: " & ex.Message)
            '  objDatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)
        End Try
    End Sub


    Public Function fnActualizaRFCSAP(CardCode As String, RFC As String) As String

        Dim sResult As String = "OK"

        Try
            Dim oSN As SAPbobsCOM.BusinessPartners
            Dim oCompany As New SAPbobsCOM.Company
            oCompany = objDatos.fnConexion_SAP
            oSN = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
            oSN.GetByKey(CardCode)
            oSN.FederalTaxID = RFC
            If Session("TelefonoEnvio") <> "" Then
                oSN.Phone1 = CStr(Session("TelefonoEnvio")).Replace("Tel: ", "")
            End If
            Dim sDestinatario As String = ""
            If Session("usrInvitado") = "SI" Then
                sDestinatario = Session("CorreoInvitado")
            Else
                ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre,cvPass,ISNULL(cvMail,'') as Mail,cvUsuario FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'  and cvTipoAcceso='B2C' "
                Dim dtLogin As New DataTable
                dtLogin = objDatos.fnEjecutarConsulta(ssql)
                sDestinatario = dtLogin.Rows(0)("Mail")
            End If
            objDatos.fnLog("Confirmacion-destinatario", sDestinatario)
            If sDestinatario = "" Then
                sDestinatario = Session("UserB2C")
            End If
            oSN.EmailAddress = sDestinatario

            If oSN.Update <> 0 Then
                ''Un error al actualizar el RFC
                objDatos.fnLog("Al actualizar RFC Cliente", oCompany.GetLastErrorDescription.Replace("'", ""))
            Else
                ''Todo bien
            End If
        Catch ex As Exception

        End Try




        Return sResult
    End Function

    Public Sub fnGeneraPagoSAP(Cliente As String, docEntry As Int64, Importe As Double, TipoPago As String, idCarrito As String, DocNum As String)


        ''Registramos el pago en una tabla Temporal. Así, por cualquier error, podemos recuperar el detalle y registrarlo en SAP
        Dim dtIdPago As New DataTable
        ssql = "SELECT ISNULL(MAX(ciIdRel),0) + 1 FROM Tienda.Pagos "
        dtIdPago = objDatos.fnEjecutarConsulta(ssql)

        If dtIdPago.Rows.Count > 0 Then
            ssql = "INSERT INTO Tienda.Pagos(ciIdRel,cvTipoPago,cvDocEntry,cvDocnum,cvCliente,cfImporte,cdFechaSAP) VALUES(" _
                & "'" & dtIdPago.Rows(0)(0) & "'," _
                & "'" & TipoPago & "'," _
                & "'" & docEntry & "'," _
                & "'" & DocNum & "'," _
                & "'" & Cliente & "'," _
                & "'" & CStr(Importe).Replace(",", ".") & "', GETDATE())"
            objDatos.fnLog("Tienda Pagos", ssql.Replace("'", ""))
            objDatos.fnEjecutarInsert(ssql)
        End If


        ''Registramos el pago en SAP

        ssql = "SELECT ISNULL(cvPagoBorrador,'NO') FROM Config.Parametrizaciones_B2C "
        Dim dtPagoBorrador As New DataTable
        dtPagoBorrador = objDatos.fnEjecutarConsulta(ssql)



        Dim oPymt As SAPbobsCOM.Payments
        Dim oCompany As New SAPbobsCOM.Company
        oCompany = objDatos.fnConexion_SAP

        If dtPagoBorrador.Rows.Count > 0 Then
            If dtPagoBorrador.Rows(0)(0) = "SI" Then
                oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts)
            Else
                oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
            End If

        Else
            oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
        End If


        oPymt.DocDate = Now.Date


        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
            oPymt.Series = 142
        End If
        oPymt.CardCode = Cliente
        oPymt.JournalRemarks = ""
        oPymt.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments
        oPymt.Invoices.Add()
        oPymt.Invoices.SetCurrentLine(0)
        If sTipoPago = "A CUENTA" Then
            objDatos.fnLog("Aplicar PAGO", "A cuenta")
            oPymt.DocType = SAPbobsCOM.BoRcptTypes.rCustomer
        Else
            oPymt.Invoices.DocEntry = docEntry
            oPymt.Invoices.SumApplied = Importe 'SUM OF INVOICE
            oPymt.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice
        End If
        objDatos.fnLog("Aplicar PAGO", "antes de importes")
        oPymt.TransferSum = Importe
        oPymt.TransferDate = Now.Date
        oPymt.TransferReference = "Orden: " & CInt(Session("NoPedido"))
        oPymt.TransferAccount = fnObtenerCuenta()

        objDatos.fnLog("Aplicar PAGO", "Antes del add")

        If oPymt.Add() <> 0 Then
            ''Error al log
            objDatos.fnEnviarCorreo("ventas@evolutic.mx", "Error al crear PAGO en SAP", "Problema al crear el pago: " & oCompany.GetLastErrorDescription.Replace("'", ""))
            objDatos.fnLog("Aplicar PAGO", "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
        Else
            objDatos.fnLog("Aplicar PAGO", "Creó Pago en SAP")
            ''Actualizamos
            Dim dtDocnum As New DataTable
            If objDatos.fnObtenerDBMS() = "HANA" Then
                ssql = objDatos.fnObtenerQuery("GetDocNumPayment")

            Else
                ssql = "SELECT Docnum FROM ORCT WHERE docEntry in (SELECT MAX(DocEntry) FROM ORCT )"
            End If

            dtDocnum = New DataTable
            dtDocnum = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtDocnum.Rows.Count > 0 Then

                ssql = "UPDATE Tienda.Pagos SET cvProcesadoSAP='SI', cvDocNumSAP=" & "'" & dtDocnum.Rows(0)(0) & "',cdFechaSAP=GETDATE() WHERE ciIdRel=" & "'" & dtIdPago.Rows(0)(0) & "'"
                ' objDatos.fnLog("Aplicar PAGO", ssql.Replace("'", ""))
                objDatos.fnEjecutarInsert(ssql)
            Else
                objDatos.fnLog("Aplicar PAGO", "No obtiene DocNum PAGO")
            End If


        End If
    End Sub
    Public Function fnObtenerCuenta() As String
        Dim cuenta As String = ""
        ssql = "SELECT cvCuenta FROM config.Parametrizaciones_B2C "
        Dim dtCuenta As New DataTable
        dtCuenta = objDatos.fnEjecutarConsulta(ssql)
        If dtCuenta.Rows.Count > 0 Then
            cuenta = dtCuenta.Rows(0)(0)
        End If

        Return cuenta
    End Function
    Public Function fnAsignaAlmacen(itemCode As String)
        Dim almacen As String = ""

        ssql = "select cvWhsCode  from config.Existencias where cvEstatus ='ACTIVO' order by ciOrden "
        Dim dtAlmacenes As New DataTable
        dtAlmacenes = objDatos.fnEjecutarConsulta(ssql)

        ''El primer almacen

        If dtAlmacenes.Rows.Count > 0 Then

            If Session("TipoDBMS") = "HANA" Then
                ssql = objDatos.fnObtenerQuery("ValidateStock")

                ssql = ssql.Replace("[%0]", itemCode)
                ssql = ssql.Replace("[%1]", dtAlmacenes.Rows(0)(0))
                'ssql = "SELECT ""OnHand"" FROM OITW WHERE itemCode=" & "'" & itemCode & "' ANd whsCode=" & "'" & dtAlmacenes.Rows(0)(0) & "'"

            Else
                ssql = "SELECT OnHand FROM OITW WHERE itemCode=" & "'" & itemCode & "' ANd whsCode=" & "'" & dtAlmacenes.Rows(0)(0) & "'"
            End If

            Dim dtExistencias As New DataTable
            dtExistencias = objDatos.fnEjecutarConsultaSAP(ssql)
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

    Public Function fnbtenerMonedaCarrito(ItemCode As String) As String
        Dim sMoneda As String = ""
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = ItemCode Then

                    sMoneda = Partida.Moneda
                    objDatos.fnLog("Moneda Carrito", ItemCode & " -> " & sMoneda)
                    Exit For
                End If
            End If

        Next

        Return sMoneda
    End Function

    Public Sub fnGuardalocales()

        If CInt(Session("NoPedido")) > 0 Then
            Exit Sub
        End If

        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
        Dim dtId As New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)

        objDatos.fnLog("fnGuardalocales", "Antes de IdCarrito")
        Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))
        Session("NoPedido") = iIdCarrito
        objDatos.fnLog("NoPedido", Session("NoPedido"))
        Try
            ''Insertamos el pedido en tablas locales
            ''Obtenemos el tipo de cambio de hoy
            objDatos.fnLog("fnGuardalocales", "Va a obtener el tipo de cambio")
            ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
            Dim dtTc As New DataTable
            dtTc = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim iTC As Double = 1
            If dtTc.Rows.Count > 0 Then
                iTC = dtTc.Rows(0)(0)
            End If



            Dim TipoDoc As String = "ORDEN DE VENTA"
            ssql = "INSERT INTO Tienda.Pedido_HDR ( ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus) VALUES(" _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & Session("UserB2C") & "'," _
            & "'" & Session("NombreuserTienda") & "'," _
            & "'" & Session("SlpCode") & "'," _
            & "'" & Session("Cliente") & "'," _
            & "'" & Session("RazonSocial") & "',GETDATE(),''," _
            & "'" & Session("ListaPrecios") & "'," _
            & "'" & TipoDoc & "','ABIERTO')"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.fnLog("fnGuardalocales", "Insertó Hdr en Carrito")
            ''Ahora las lineas
            Dim iTotal As Double = 0
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_det"
                    Dim dtIdLineas As New DataTable
                    dtIdLineas = objDatos.fnEjecutarConsulta(ssql)

                    ssql = "INSERT INTO Tienda.Pedido_det (ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                  & "'" & dtIdLineas.Rows(0)(0) & "'," _
                  & "'" & dtId.Rows(0)(0) & "'," _
                  & "'" & Session("UserTienda") & "'," _
                  & "'" & Partida.ItemCode & "'," _
                  & "'" & Partida.ItemName & "'," _
                  & "'" & Partida.Cantidad.ToString.Replace(",", ".") & "'," _
                  & "'" & Partida.Precio.ToString.Replace(",", ".") & "'," _
                  & "'" & Partida.Descuento.ToString.Replace(",", ".") & "')"
                    objDatos.fnEjecutarInsert(ssql)
                    iTotal = iTotal + (Partida.Precio * Partida.Cantidad)
                End If
            Next
            objDatos.fnLog("fnGuardalocales", "Insertó las lineas")

            ssql = "UPDATE Tienda.Pedido_HDR  SET cfTotal=" & "'" & iTotal.ToString.Replace(",", ".") & "',cfTipoCambio=" & "'" & iTC.ToString.Replace(",", ".") & "',cfTotalFC=" & "'" & (iTotal * iTC).ToString.Replace(",", ".") & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.fnLog("fnGuardalocales", "despues de update")

            ''Registramos la direccion de envio en el pedido(local)
            ssql = "INSERT INTO [Tienda].[Pedido_Envio]([ciNoPedido],[cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais]) VALUES(" _
            & "'" & Session("NoPedido") & "'," _
            & "'" & Session("UserB2C") & "'," _
            & "'" & Session("CalleEnvio") & "'," _
            & "'" & Session("ColoniaEnvio") & "'," _
            & "'" & Session("NumExtEnvio") & "'," _
            & "'" & Session("NumIntEnvio") & "'," _
            & "'" & Session("CiudadEnvio") & "'," _
            & "'" & Session("MunicipioEnvio") & "'," _
            & "'" & Session("EstadoEnvio") & "'," _
            & "'" & Session("PaisEnvio") & "')"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.fnLog("fnGuardalocales", "despues de Pedido_Envio")

            If Session("NombreDireccionEnvio") = "Nueva Direccion" Or Session("NombreDireccionEnvio") = "" Then
                ''Insertamos
                ssql = "INSERT INTO Tienda.Direcciones_Envio ([cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais],[cvPredeterminado],cvCP) VALUES(" _
                & "'" & Session("UserB2C") & "'," _
                & "'" & Session("CalleEnvio") & "'," _
                & "'" & Session("ColoniaEnvio") & "'," _
                & "'" & Session("NumExtEnvio") & "'," _
                & "'" & Session("NumIntEnvio") & "'," _
                & "'" & Session("CiudadEnvio") & "'," _
                & "'" & Session("MunicipioEnvio") & "'," _
                & "'" & Session("EstadoEnvio") & "'," _
                & "'" & Session("PaisEnvio") & "','N'," _
                & "'" & Session("CPEnvio") & "')"
                objDatos.fnEjecutarInsert(ssql)
            End If
            objDatos.fnLog("fnGuardalocales", "despues de Direcciones_Envio")
        Catch ex As Exception
            objDatos.fnLog("fnGuardalocales ex", ex.Message)
        End Try


    End Sub

    Public Sub fnMetodoPago()
        lblTerminacion.Text = "Terminación: " & Session("IdTarjetaPago")
        lblDireccion.Text = "Dirección: " & Session("CalleEnvio") & " " & Session("NumExtEnvio") & " " & Session("Colonia") & " " & Session("CPEnvio") & " " & Session("EstadoEnvio")
        'Dim sHtmlBanner As String = ""
        'ssql = "SELECT * from config.MetodosPago WHERE cvUser=" & "'" & Session("UserB2C") & "' AND cvCardCode=" & "'" & Session("CardCodeUserB2C") & "' AND cvTarjeta=" & "'" & Session("IdTarjetaPago") & "'"
        'Dim dtMetodos As New DataTable
        'dtMetodos = objDatos.fnEjecutarConsulta(ssql)
        'For i = 0 To dtMetodos.Rows.Count - 1 Step 1
        '    sHtmlBanner = sHtmlBanner & "<div class='col-xs-2 no-padding'> "
        '    sHtmlBanner = sHtmlBanner & " <img src='img/masterCard.png' class='img-responsive'>"
        '    sHtmlBanner = sHtmlBanner & "</div>"
        '    sHtmlBanner = sHtmlBanner & "<div class='col-xs-10'>"
        '    sHtmlBanner = sHtmlBanner & " <p>Terminación:" & CStr(dtMetodos.Rows(0)("cvTarjeta")).Substring(CStr(dtMetodos.Rows(0)("cvTarjeta")).Length - 4, 4) & " </p></br>"
        '    sHtmlBanner = sHtmlBanner & " <p>Exp. " & CStr(dtMetodos.Rows(0)("cvVence")) & " </p></br>"
        '    sHtmlBanner = sHtmlBanner & " <p>Dirección:" & Session("CalleEnvio") & " " & Session("NumExtEnvio") & " </p></br></div>"
        'Next



        'Dim literal As New LiteralControl(sHtmlBanner)
        'pnlMetodoPago.Controls.Clear()
        'pnlMetodoPago.Controls.Add(literal)

    End Sub
    Public Sub fnTotales()

        Dim sSubTotal As Double = 0
        Dim TotDescuento As Double = 0
        Dim TotalImpuestos As Double = 0
        Dim fTasaImpuesto As Double = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then

                ''Evaluamos el impuesto
                ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                If ssql <> "" Then
                    ssql = ssql.Replace("[%0]", Partida.ItemCode)
                    ssql = ssql.Replace("[%1]", Session("Cliente"))
                    Dim dtIVA As New DataTable
                    dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtIVA.Rows.Count > 0 Then
                        ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                        Dim dtTasa As New DataTable
                        dtTasa = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtTasa.Rows.Count > 0 Then
                            fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                        End If

                    End If

                End If


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
                ' sHtmlBanner = sHtmlBanner & "</div>"
                Dim precio As Double = 0
                Dim precioConDescuento As Double = 0
                If Partida.Descuento > 0 Then
                    precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                Else
                    precioConDescuento = Partida.Precio
                End If

                Dim sMonedasistema As String = ""
                ssql = "select MainCurncy  from OADM "
                Dim dtMoneda As New DataTable
                dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtMoneda.Rows.Count > 0 Then
                    sMonedasistema = dtMoneda.Rows(0)(0)
                End If

                If Partida.Descuento > 0 Then

                    If Partida.Moneda <> sMonedasistema Then
                        Session("Moneda") = sMonedasistema

                        TotDescuento = TotDescuento + (Partida.Precio * CDbl(Session("TC")) - (precioConDescuento * CDbl(Session("TC"))) * Partida.Cantidad)
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                    Else
                        TotDescuento = TotDescuento + ((Partida.Precio - precioConDescuento) * Partida.Cantidad)
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                    End If
                Else
                    TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)

                End If



                If Partida.Descuento > 0 Then
                    precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                Else
                    precioConDescuento = Partida.Precio
                End If

                If Partida.Moneda <> "" Then
                    If Partida.Moneda <> sMonedasistema Then
                        Session("Moneda") = sMonedasistema
                        objDatos.fnLog("Carrito-TC", "Partida moneda <> moneda: " & Partida.Moneda & " <> " & Session("Moneda"))
                        ''Multiplicamos el precio por el tipo de cambio
                        If CDbl(Session("TC")) > 0 Then
                            sSubTotal = sSubTotal + (Partida.Cantidad * (precioConDescuento * CDbl(Session("TC"))))
                        Else
                            sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                        End If

                    Else
                        If Partida.Descuento > 0 Then
                            sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                        Else
                            sSubTotal = sSubTotal + (Partida.Cantidad * precioConDescuento)
                        End If

                    End If
                Else
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                End If



            End If
        Next
        lblSubTotal.Text = sCaracterMoneda & " " & sSubTotal.ToString(" ###,###,###.#0") & " " & Session("Moneda")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & TotDescuento.ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                '  lblEnviotxt.Text = ""
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If

            Envio = CDbl(Session("ImporteEnvio"))

            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                Descuento = TotDescuento
            End If


            ' Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try
        Envio = CDbl(Session("ImporteEnvio"))
        lblSubTotal.Text = sCaracterMoneda & " " & CDbl(Session("ImporteSubTotal")).ToString(" ###,###,###.#0") & " " & Session("Moneda")

        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            Descuento = CDbl(Session("ImporteDescuento"))
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = "0.00"
        Else
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If

        lblTotal.Text = sCaracterMoneda & " " & CDbl(sSubTotal + CDbl(Session("ImporteEnvio")) - Descuento).ToString(" ###,###,###.#0") & " " & Session("Moneda")

        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)

        '   TotalImpuestos = TotalImpuestos + (Envio * 0.16)
        TotalImpuestos = CDbl(Session("TotalImpuestos"))

        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & " " & Session("Moneda")
                lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                Session("TotalImpuestos") = TotalImpuestos
            End If
        End If
        If CDbl(Session("ImporteDescuento")) > 0 Then
            lblDesctxt.Visible = True
        End If
    End Sub
    Public Sub fnTotalesOld()
        Dim sSubTotal As Double = 0
        Dim TotDescuento As Double = 0
        Dim TotalImpuestos As Double = 0
        Dim fTasaImpuesto As Double = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then

                ''Evaluamos el impuesto
                ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                If ssql <> "" Then
                    ssql = ssql.Replace("[%0]", Partida.ItemCode)
                    ssql = ssql.Replace("[%1]", Session("Cliente"))
                    Dim dtIVA As New DataTable
                    dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtIVA.Rows.Count > 0 Then
                        ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                        Dim dtTasa As New DataTable
                        dtTasa = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtTasa.Rows.Count > 0 Then
                            fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                        End If

                    End If

                End If


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
                ' sHtmlBanner = sHtmlBanner & "</div>"
                Dim precio As Double = 0
                Dim precioConDescuento As Double = 0
                If Partida.Descuento > 0 Then
                    precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                Else
                    precioConDescuento = Partida.Precio
                End If

                Dim sMonedasistema As String = ""
                ssql = "select MainCurncy  from OADM "
                Dim dtMoneda As New DataTable
                dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtMoneda.Rows.Count > 0 Then
                    sMonedasistema = dtMoneda.Rows(0)(0)
                End If

                If Partida.Descuento > 0 Then

                    If Partida.Moneda <> sMonedasistema Then
                        Session("Moneda") = sMonedasistema

                        TotDescuento = TotDescuento + (Partida.Precio * CDbl(Session("TC")) - (precioConDescuento * CDbl(Session("TC"))) * Partida.Cantidad)
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                    Else
                        TotDescuento = TotDescuento + ((Partida.Precio - precioConDescuento) * Partida.Cantidad)
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                    End If
                Else
                    TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)

                End If



                If Partida.Descuento > 0 Then
                    precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                Else
                    precioConDescuento = Partida.Precio
                End If

                If Partida.Moneda <> sMonedasistema Then
                    Session("Moneda") = sMonedasistema
                    objDatos.fnLog("Carrito-TC", "Partida moneda <> moneda: " & Partida.Moneda & " <> " & Session("Moneda"))
                    ''Multiplicamos el precio por el tipo de cambio
                    sSubTotal = sSubTotal + (Partida.Cantidad * (Partida.Precio * CDbl(Session("TC"))))
                Else
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                End If



            End If
        Next
        lblSubTotal.Text = Session("Moneda") & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = Session("Moneda") & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                ' lblEnviotxt.Text = ""
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                Descuento = CDbl(lblDescuento.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try

        lblSubTotal.Text = Session("Moneda") & " " & CDbl(Session("ImporteSubTotal")).ToString(" ###,###,###.#0")

        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = Session("Moneda") & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0")
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = Session("Moneda") & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0")
        End If

        lblTotal.Text = Session("Moneda") & " " & CDbl(sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0")

        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)




        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = Session("Moneda") & " " & TotalImpuestos.ToString(" ###,###,###.#0")
                lbltotalImp.Text = Session("Moneda") & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0")
                Session("TotalImpuestos") = TotalImpuestos
            End If
        End If

    End Sub
    Public Sub fnMetodosEnvio()
        lblMetodoEnvio.Text = ""

    End Sub

    Protected Function MensajeHTML(ArchivoHTML As [String]) As String
        Dim Cuerpo As [String] = ""
        Try
            Dim File As New System.IO.StreamReader(ArchivoHTML)

            Dim Line As [String]
            Dim text As String = System.IO.File.ReadAllText(ArchivoHTML)

            Cuerpo = text

            File.Close()
        Catch ex As Exception
            objDatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function

    Public Function fnGeneraHTMLPartidas() As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objDatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If
        'sLigaSitio 
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Try
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    sHtmlBanner = sHtmlBanner & " <tr>"
                    '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
                            Dim sItemCodeFoto As String = ""
                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            If Partida.Generico <> "" Then
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")
                                sItemCodeFoto = Partida.Generico
                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                sItemCodeFoto = Partida.ItemCode
                            End If
                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;vertical-align: middle;text-align:center;'> "
                                'sHtmlBanner = sHtmlBanner & " <td width='260px'> "

                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            If dtFoto.Rows(0)(0) <> "" Then
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtFoto.Rows(0)(0) & "' alt='logo' style='height:100px!important;'>"
                                            Else
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='height:100px!important;'>"
                                            End If

                                        Else
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='height:100px!important;'>"
                                        End If
                                    Else
                                        Dim iband As Int16 = 0
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto.Replace("-", "") & ".jpg") Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & "images\products\" & sItemCodeFoto.Replace("-", "") & ".jpg" & "' alt='logo' style='height:100px!important;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-1.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & "images\products\" & sItemCodeFoto.Replace("-", "") & "-1.jpg" & "' alt='logo' style='height:100px!important;'>"
                                            iband = 1
                                        End If
                                        If iband = 0 Then

                                            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PEGADURO") Then
                                            Else
                                                sLigaSitio = sLigaSitio & "images/products/"
                                            End If
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-width:50px;max-height:100px'>"

                                            'sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='height:100px!important;'>"
                                        End If

                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='height:100px!important;'>"
                                End If



                                '   sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-height:100px;'>"
                                sHtmlBanner = sHtmlBanner & "</td>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then




                                    If sCampos = "" Then
                                        ''Si es el primer valor que va a enlazar, lo ponemos en strong
                                        sCampos = sCampos & "<strong style='font-size: 13px;color:#000000;'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</strong> <br>"
                                    Else
                                        If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                            sCampos = sCampos & Partida.ItemName & " <br>"
                                        Else
                                            sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                        End If
                                    End If

                                Else
                                    '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:left;font-size: 10px;'>" & sCampos & "</td>"

                    End If

                    Dim precio As Double = 0
                    If Partida.Descuento > 0 Then
                        precio = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precio = Partida.Precio
                    End If


                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Partida.Moneda & " " & precio.ToString(" ###,###,###.#0") & "</td>"
                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Partida.Cantidad.ToString(" ###,###,###.#0") & "</td>"
                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Partida.Moneda & " " & (Partida.Cantidad * precio).ToString(" ###,###,###.#0") & "</td>"

                    sHtmlBanner = sHtmlBanner & "</tr>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * precio)
                    ''Aqui van los botones de Action Cart
                    'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                    ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                    ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                    '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                    ''sHtmlBanner = sHtmlBanner & "</div>"

                    'sHtmlBanner = sHtmlBanner & " </div> "
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

            objDatos.fnLog("Html Partidas", sHtmlEncabezado.Replace("'", ""))
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try
        Return sHtmlEncabezado
    End Function

    Public Sub fnCargarCarrito()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0

        Dim TotalImpuestos As Double = 0
        Dim fTasaImpuesto As Double = 0
        Dim TotDescuento As Double = 0




        Try
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then


                    ''Evaluamos el impuesto
                    ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", Partida.ItemCode)
                        ssql = ssql.Replace("[%1]", Session("Cliente"))
                        Dim dtIVA As New DataTable
                        dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                            Dim dtTasa As New DataTable
                            dtTasa = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtTasa.Rows.Count > 0 Then
                                fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                            End If

                        End If

                    End If


                    sHtmlBanner = sHtmlBanner & " <tr>"
                    '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        Dim sItemCodeFoto As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            If Partida.Generico <> "" Then
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")
                                sItemCodeFoto = Partida.Generico
                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                sItemCodeFoto = Partida.ItemCode
                            End If
                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <td> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            If dtFoto.Rows(0)(0) <> "" Then
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' class='img-responsive'>"
                                            Else
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                            End If

                                        Else
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                        End If
                                    Else
                                        Dim iband As Int16 = 0
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto.Replace("-", "") & ".jpg") Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & "" & sItemCodeFoto.Replace("-", "") & ".jpg" & "' class='img-responsive'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & sItemCodeFoto & "-1.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & "" & sItemCodeFoto.Replace("-", "") & "-1.jpg" & "' class='img-responsive'>"
                                            iband = 1
                                        End If

                                        If iband = 0 Then
                                            If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo"))) Then

                                                sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                            Else

                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                            End If
                                        End If



                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                End If







                                ' sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"


                                sHtmlBanner = sHtmlBanner & "</td>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                Else
                                    '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <td data-title='Articulo'>" & sCampos & "</td>"

                    End If
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If

                    Dim sMonedasistema As String = ""
                    ssql = "select MainCurncy  from OADM "
                    Dim dtMoneda As New DataTable
                    dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtMoneda.Rows.Count > 0 Then
                        sMonedasistema = dtMoneda.Rows(0)(0)
                    End If

                    If Partida.Descuento > 0 Then

                        If Partida.Moneda <> sMonedasistema Then
                            Session("Moneda") = sMonedasistema

                            TotDescuento = TotDescuento + (Partida.Precio * CDbl(Session("TC")) - (precioConDescuento * CDbl(Session("TC"))) * Partida.Cantidad)
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                        Else
                            TotDescuento = TotDescuento + ((Partida.Precio - precioConDescuento) * Partida.Cantidad)
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                        End If
                    Else
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)

                    End If


                    If Partida.Descuento > 0 Then

                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                        sHtmlBanner = sHtmlBanner & "<td data-title='Precio' class='text-center' ><span class='precio-con-descuento'>" & sCaracterMoneda & " " & precioConDescuento.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</span><div class='precio-original descuento'>" & sCaracterMoneda & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</div></td>"

                    Else
                        sHtmlBanner = sHtmlBanner & " <td data-title='Precio' class='text-center' >" & sCaracterMoneda & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</td>"

                        precioConDescuento = Partida.Precio

                    End If


                    sHtmlBanner = sHtmlBanner & " <td data-title='Cantidad' class='text-center' >" & Partida.Cantidad.ToString(" ###,###,###.#0") & "</td>"
                    sHtmlBanner = sHtmlBanner & " <td data-title='Total' class='text-center' >" & sCaracterMoneda & " " & (Partida.Cantidad * precioConDescuento).ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</td>"

                    sHtmlBanner = sHtmlBanner & "</tr>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart
                    'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                    ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                    ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                    '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                    ''sHtmlBanner = sHtmlBanner & "</div>"

                    'sHtmlBanner = sHtmlBanner & " </div> "
                End If






            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(literal)
        Session("ImporteSubTotal") = sSubTotal
        lblSubTotal.Text = sCaracterMoneda & " " & CDbl(Session("ImporteSubTotal")).ToString("###,###,###.#0")
        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & CDbl(Session("ImporteDescuento")).ToString("###,###,###.#0")
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString("###,###,###.#0")
        End If
        lblTotal.Text = sCaracterMoneda & " " & (sSubTotal + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString(" ###,###,###.#0")


        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Then
                ''Calculamos el IVA
                Dim fIVA As Double = 0
                fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = Session("Moneda") & " " & TotalImpuestos.ToString(" ###,###,###.#0")
                lbltotalImp.Text = Session("Moneda") & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0")
            End If
        End If
        If TotDescuento > 0 Then
            lblDesctxt.Visible = True
        End If

    End Sub

    Private Sub btnMuestraPDF_Click(sender As Object, e As EventArgs) Handles btnMuestraPDF.Click
        Try
            If Session("ArchivoMostrar") <> "" Then
                Response.Redirect(Session("ArchivoMostrar"))
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class
