Imports System.Data
Imports System.IO
Imports System.Security.Cryptography
Imports mercadopago
Partial Class pago_resumen
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Public sCaracterMoneda As String = ""
    Private Sub pago_resumen_Load(sender As Object, e As EventArgs) Handles Me.Load



        fnCargarCarrito()
        fnTotales()
        If Not IsPostBack Then

            ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
            Dim dtCaracter As New DataTable
            dtCaracter = objDatos.fnEjecutarConsulta(ssql)
            If dtCaracter.Rows.Count > 0 Then
                sCaracterMoneda = dtCaracter.Rows(0)(0)
            End If

            objDatos.fnLog("Resumen", "alcanza a entrar")
            Session("TotalCompra") = 0
            Session("NoPedido") = 0
            Try
                objDatos.fnLog("Resumen load", " Antes de Totales")

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

                lblNombreDireccion.Text = " " & Session("NombreDireccionEnvio")
                lblNombreEnvio.Text = " Enviado a: " & Session("NombreEnvio")
                lblCalleyNum.Text = " " & sCalleyNum
                lblColoniaMunic.Text = " " & Session("ColoniaEnvio") & " " & Session("CiudadEnvio") & " " & Session("MunicipioEnvio")
                lblEstadoCP.Text = " " & Session("EstadoEnvio") & " " & Session("PaisEnvio") & " " & Session("CPEnvio") & vbCrLf & sTelefono

                objDatos.fnLog("Resumen load", " Antes de metodo pago")

                fnMetodoPago()
                If Session("CiudadEnvio") = "" Then
                    Session("CiudadEnvio") = "Ciudad"
                End If

                objDatos.fnLog("Resumen load", " Antes de Envio")
                ''El metodo de envio
                ssql = "SELECT * from config.metodosEnvio where ciIdMetodoEnvio=" & "'" & Session("MetodoEnvio") & "'"
                Dim dtMetodos As New DataTable
                dtMetodos = objDatos.fnEjecutarConsulta(ssql)
                If dtMetodos.Rows.Count > 0 Then
                    lblMetodoEnvio.Text = dtMetodos.Rows(0)("cvClave") & "(" & dtMetodos.Rows(0)("cvDescripcion") & ") - " & CDbl(dtMetodos.Rows(0)("cvImporte")).ToString("###,###,###.#0") & " " & dtMetodos.Rows(0)("cvMoneda")
                Else
                    lblMetodoEnvio.Text = "Costo de envío:($ " & CDbl(Session("ImporteEnvio")).ToString("###,###,###.0#") & " )"
                End If

                ' Session("NoPedido") = "100"
                objDatos.fnLog("Resumen load", " Antes de NoPedido")
                If CInt(Session("NoPedido")) = 0 Then

                    objDatos.fnLog("Resumen load", " entra  NoPedido")
                    ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
                    Dim dtId As New DataTable
                    dtId = objDatos.fnEjecutarConsulta(ssql)

                    objDatos.fnLog("Resumen load", " antes  iIdCarrito")
                    '   objDatos.fnLog("Cotizacion", "Antes de IdCarrito")
                    Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))
                    Session("NoPedidoPago") = "PED: " & iIdCarrito

                End If
                objDatos.fnLog("Resumen load", " antes  NombreuserTienda")
                If Session("NombreuserTienda") = "" Then
                    Session("NombreuserTienda") = "Usuario "
                    Session("NombreAdicional") = "Invitado"
                End If
                '  Session("NombreuserTienda") = "Usuario In"
                ''Validación necesaria para PayU y PixelPay
                objDatos.fnLog("Resumen load", " Antes de Usuarios")

                ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
                Dim dtDatosUsuario As New DataTable
                dtDatosUsuario = objDatos.fnEjecutarConsulta(ssql)
                If dtDatosUsuario.Rows.Count > 0 Then
                    Session("NombreuserTienda") = dtDatosUsuario.Rows(0)(0)
                    Session("NombreAdicional") = Session("UserB2C")
                    Session("CorreoInvitado") = Session("UserB2C")
                End If
                If Session("usrInvitado") = "NO" And Session("UserB2C") <> "" Then
                    Session("CorreoInvitado") = Session("UserB2C")
                End If

                objDatos.fnLog("Resumen load", " Antes de Proveedor: correo" & Session("CorreoInvitado"))

                'If Session("UserB2C") = "" Then
                '    Session("UserB2C") = Session("CorreoInvitado")
                'End If

                ssql = "select ISNULL(cvProveedorPagos,'') FROM config.parametrizaciones"
                Dim dtProveedor As New DataTable
                dtProveedor = objDatos.fnEjecutarConsulta(ssql)
                If dtProveedor.Rows.Count > 0 Then

                    If dtProveedor.Rows(0)(0) = "PayU" Then
                        ssql = "SELECT * FROM config.Proveedores_Pago WHERE cvEmpresa ='PayU' "
                        Dim dtDatosProveedor As New DataTable
                        dtDatosProveedor = objDatos.fnEjecutarConsulta(ssql)
                        If dtDatosProveedor.Rows.Count > 0 Then


                            btnFinalizar.PostBackUrl = dtDatosProveedor.Rows(0)("cvURLPostback")
                            Session("MerchantId") = dtDatosProveedor.Rows(0)("cvMerchantId")
                            Session("AccountId") = dtDatosProveedor.Rows(0)("cvAccountId")
                            Session("Description") = "Pedido " & Session("NoPedidoPago")
                            Session("ReferenceCode") = Now.Date.ToString("yyyyMMdd") & Session("NoPedidoPago")  ''Aqui debemos generar una referencia
                            Session("ResponseURL") = dtDatosProveedor.Rows(0)("cvURLError")
                            Session("ConfirmationURL") = dtDatosProveedor.Rows(0)("cvURLExito")
                            generarFirma()
                        End If
                    End If

                End If
            Catch ex As Exception
                objDatos.fnLog("Resumen EX", ex.Message)
            End Try
        End If



    End Sub

    Public Sub fnMetodoPago()
        'lblTerminacion.Text = "Terminación: " & Session("IdTarjetaPago")
        'lblDireccion.Text = "Dirección: " & Session("CalleEnvio") & " " & Session("NumExtEnvio") & " " & Session("Colonia") & " " & Session("CPEnvio") & " " & Session("EstadoEnvio")
        ''Dim sHtmlBanner As String = ""
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
                lblEnviotxt.Text = ""
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
                Session("ImporteEnvio") = Envio
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                lblDesctxt.Visible = True
                Descuento = TotDescuento
            End If



            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try

        lblSubTotal.Text = sCaracterMoneda & " " & CDbl(Session("ImporteSubTotal")).ToString(" ###,###,###.#0") & " " & Session("Moneda")

        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = "0.00"
        Else
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If


        Dim tenvio As Double = 0
        Dim tdescto As Double = 0

        Dim tTotal As Double = 0

        tenvio = CDbl(Session("ImporteEnvio"))
        tdescto = CDbl(Session("ImporteDescuento"))
        tTotal = sSubTotal - tdescto + tenvio

        objDatos.fnLog("Envvio-totales", tenvio)
        objDatos.fnLog("desct-totales", tdescto)
        objDatos.fnLog("subtotal-totales", sSubTotal)

        lblTotal.Text = sCaracterMoneda & " " & tTotal.ToString(" ###,###,###.#0") & " " & Session("Moneda")

        Session("TotalCarrito") = tTotal


        TotalImpuestos = TotalImpuestos + (tenvio * 0.16)
        Session("TotalImpuestos") = TotalImpuestos
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
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente3 As New DataTable
        dtcliente3 = objDatos.fnEjecutarConsulta(ssql)
        If dtcliente3.Rows.Count > 0 Then
            If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("STOP CAT") Then
                pnlImpuestos.Visible = False 'Solo subtotal, envio y descuento
                ' Session("TotalCarrito") = sSubTotal
                TotalImpuestos = (CDbl(Session("TotalCarrito"))) / (1 + fTasaImpuesto)
                lblSubTotal.Text = sCaracterMoneda & " " & sSubTotal.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                lblTotal.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito"))).ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                'Session("TotalCarrito") = TotalImpuestos
                'TotalImpuestos = TotalImpuestos * fTasaImpuesto
                'Session("TotalImpuestos") = TotalImpuestos
                'lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                'lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            End If
        End If


        If CDbl(Session("ImporteDescuento")) > 0 Then
            lblDesctxt.Visible = True
        End If
    End Sub
    Public Sub fnMetodosEnvio()
        lblMetodoEnvio.Text = ""

    End Sub
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


                                '  sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
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
                    Dim precioConDescuento As Double = 0
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
        lblSubTotal.Text = sCaracterMoneda & " " & CDbl(Session("ImporteSubTotal")).ToString(" ###,###,###.#0") & " " & Session("Moneda")

        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If

        ''Calculamos nuevamente el envio
        ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
        Dim dtclienteEnvio As New DataTable
        dtclienteEnvio = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteEnvio.Rows.Count > 0 Then
            If CStr(dtclienteEnvio.Rows(0)(0)).Contains("Salama") Then
                Session("ImporteEnvio") = fnCalculoEnvioSalama(CDbl(Session("ImporteSubTotal")) - CDbl(Session("ImporteDescuento")))
            End If
        End If

        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If


        Dim tenvio As Double = 0
        Dim tdescto As Double = 0

        Dim tTotal As Double = 0

        tenvio = CDbl(Session("ImporteEnvio"))
        tdescto = CDbl(Session("ImporteDescuento"))
        tTotal = sSubTotal - tdescto + tenvio

        objDatos.fnLog("Envvio-carrito", tenvio)
        objDatos.fnLog("desct-carrito", tdescto)
        objDatos.fnLog("subtotal-carrito", sSubTotal)

        lblTotal.Text = sCaracterMoneda & " " & tTotal.ToString(" ###,###,###.#0") & " " & Session("Moneda")

        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = sCaracterMoneda & " " & CDbl(Session("TotalImpuestos")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + CDbl(Session("TotalImpuestos"))).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            End If
        End If
        If CDbl(Session("ImporteDescuento")) > 0 Then
            lblDesctxt.Visible = True
        End If

    End Sub
    Public Function fnCalculoEnvioSalama(TotalCarrito As Double) As Double
        Dim MinCompra As Double = 0
        Dim ImporteEnvio As Double = 0


        Dim dtMetodos As New DataTable
        ssql = "select ISNULL(cfMinimoCompraB2C,0)  from config .Parametrizaciones  "
        Dim dtminCompra As New DataTable
        dtminCompra = objDatos.fnEjecutarConsulta(ssql)
        If dtminCompra.Rows.Count > 0 Then
            MinCompra = dtminCompra.Rows(0)(0)
            If TotalCarrito < MinCompra Then
                ''No ha alcanzado el minimo de compra, le habilitamos la opción de cobro
                ssql = "select * from config.MetodosEnvio WHERE cvImporte >0  "
            Else
                ssql = "select * from config.MetodosEnvio WHERE cvImporte =0  "
            End If
        End If

        dtMetodos = objDatos.fnEjecutarConsulta(ssql)
        If dtMetodos.Rows.Count > 0 Then
            ImporteEnvio = dtMetodos.Rows(0)("cvImporte")
        End If
        Return ImporteEnvio
    End Function
    Protected Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click

        ''Insertamos el pedido en tablas locales
        ''Obtenemos el tipo de cambio de hoy
        objDatos.fnLog("Resumen", "Si entra")
        ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
        Dim dtTc As New DataTable
        dtTc = objDatos.fnEjecutarConsultaSAP(ssql)
        Dim iTC As Double = 1
        If dtTc.Rows.Count > 0 Then
            iTC = dtTc.Rows(0)(0)
        End If
        If CInt(Session("NoPedido")) = 0 Or 1 = 1 Then
            ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
            Dim dtId As New DataTable
            dtId = objDatos.fnEjecutarConsulta(ssql)

            objDatos.fnLog("Resumen", "Antes de IdCarrito")
            Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))
            Session("NoPedido") = iIdCarrito
            objDatos.fnLog("Resumen", Session("NoPedido"))

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
            objDatos.fnLog("Cotizacion", "Insertó Hdr en Carrito")
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
            objDatos.fnLog(TipoDoc, "Insertó las lineas")

            ssql = "UPDATE Tienda.Pedido_HDR  SET cfTotal=" & "'" & iTotal.ToString.Replace(",", ".") & "',cfTipoCambio=" & "'" & iTC.ToString.Replace(",", ".") & "',cfTotalFC=" & "'" & (iTotal * iTC).ToString.Replace(",", ".") & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
            objDatos.fnEjecutarInsert(ssql)


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
            ''Registramos la direccion de facturacion

            ''Se debe registrar tmb en SAP las direcciones?

            objDatos.fnLog("Resumen", "Insertó")
            ''Notificamos a alguien del pedido/envio
            Dim sArticulosSinStock As String = ""
            Dim sBody As String = "Se ha generado un nuevo pedido desde el portal de eCommerce "
            ssql = "SELECT ISNULL(cvCorreoEnvios,'') from config.Parametrizaciones "
            Dim dtEnvio As New DataTable
            dtEnvio = objDatos.fnEjecutarConsulta(ssql)
            If dtEnvio.Rows.Count > 0 Then
                If dtEnvio.Rows(0)(0) <> "" Then

                    sBody = sBody & "Artículos: " & vbCrLf

                    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                        If Partida.ItemCode <> "BORRAR" Then
                            sBody = sBody & Partida.ItemCode & "-" & Partida.ItemName & " - " & Partida.Cantidad & vbCrLf
                            If fnRevisaExistencias(Partida.ItemCode) = 0 Then
                                sArticulosSinStock = sArticulosSinStock & Partida.ItemCode & vbCrLf
                            End If

                        End If

                    Next
                    sBody = sBody & " Dirección: " & vbCrLf
                    sBody = sBody & " Calle: " & Session("CalleEnvio") & " " & Session("NumExtEnvio") & vbCrLf
                    sBody = sBody & " CP: " & Session("CPEnvio") & vbCrLf
                    sBody = sBody & " Ciudad: " & Session("CiudadEnvio") & vbCrLf
                    sBody = sBody & " Estado/Departamento: " & Session("EstadoEnvio") & vbCrLf
                    sBody = sBody & " País: " & Session("PaisEnvio") & vbCrLf

                    objDatos.fnEnviarCorreo(dtEnvio.Rows(0)(0), sBody, "Nuevo pedido desde eCommerce")
                End If
            End If

            ssql = "select ISNULL(cvEnviaCorreoCliente,'NO') FROM config.parametrizaciones"
            Dim dtEnviaCorreoCliente As New DataTable
            dtEnviaCorreoCliente = objDatos.fnEjecutarConsulta(ssql)
            If dtEnviaCorreoCliente.Rows.Count > 0 Then
                If dtEnviaCorreoCliente.Rows(0)(0) = "SI" Then
                    ssql = ""
                    ssql = objDatos.fnObtenerQuery("Correocliente")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", "")
                        Dim dtCorreo As New DataTable
                        dtCorreo = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtCorreo.Rows.Count > 0 Then
                            If dtCorreo.Rows(0)(0) <> "" Then

                                sBody = sBody & "Sin embargo, los siguientes productos no cuentan con stock suficiente: " & vbCrLf & sArticulosSinStock
                                sBody = sBody & " favor de contactar con servicio al cliente para un tiempo estimado de entrega ."

                                objDatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), sBody, "Notificaciones ECommerce: Nuevo pedido ")

                            End If
                        End If
                    End If

                End If
            End If

            ''TEMPORAL

            ''Mandamos a la pagina de confirmacion Compra exitosa. Aqui nos regresará el banco despues del pago
            ' Response.Redirect("confirmacionCompra.aspx")
        End If


        ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)
        Dim sNombreEmpresa As String = ""
        If dtcliente.Rows.Count > 0 Then
            sNombreEmpresa = dtcliente.Rows(0)(0)
        End If
        objDatos.fnLog("Resumen", "Proveedores pago")
        ssql = "select ISNULL(cvProveedorPagos,'') FROM config.parametrizaciones"
        Dim dtProveedor As New DataTable
        dtProveedor = objDatos.fnEjecutarConsulta(ssql)
        If dtProveedor.Rows.Count > 0 Then

            If dtProveedor.Rows(0)(0) = "Mercado Pago" Then
                ssql = "SELECT * FROM config.Proveedores_Pago WHERE cvEmpresa ='Mercado Pago' "
                Dim dtDatosProveedor As New DataTable
                dtDatosProveedor = objDatos.fnEjecutarConsulta(ssql)
                If dtDatosProveedor.Rows.Count > 0 Then
                    ''Conectamos con mercado pago en SAndBox
                    Dim mp As MP = New MP(dtDatosProveedor.Rows(0)("cvAccountId"), dtDatosProveedor.Rows(0)("cvMerchantId"))
                    Try
                        mp.sandboxMode(True)
                        Dim accessToken = mp.getAccessToken()
                        ' Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"",""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""}, ""quantity"": 1,""currency_id"":""MXN"",""unit_price"":" & lblTotal.Text.Replace("$", "") & "}]}")
                        ' objDatos.fnLog("mercadoPago", "{""items"":[{""title"":""Pedido " & sNombreEmpresa & ":"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & lblTotal.Text.Replace("$", "").Replace(",", "") & "}],""payer"": {""email"": """ & Session("CorreoInvitado") & """}, ""back_urls"":  {""success"": """ & dtDatosProveedor.Rows(0)("cvURLExito") & """,""failure"": """ & dtDatosProveedor.Rows(0)("cvURLError") & """,""pending"": """ & dtDatosProveedor.Rows(0)("cvURLError") & """},""auto_return"": ""approved""}")

                        Dim sPreferencia As String = "{""items"":[{""title"":""Pedido " & sNombreEmpresa & ":"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & CStr(Session("TotalCompra")).Replace(",", ".") & "}],""payer"": {""email"": """ & Session("CorreoInvitado") & """},""back_urls"": {""success"": """ & dtDatosProveedor.Rows(0)("cvURLExito") & """,""failure"": """ & dtDatosProveedor.Rows(0)("cvURLError") & """,""pending"": """ & dtDatosProveedor.Rows(0)("cvURLError") & """}}"
                        Dim sPreferencia2 As String = "{""items"":[{""title"":""Pedido " & sNombreEmpresa & ":"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & Session("TotalCompra") & "}],""payer"": {""email"": """ & Session("CorreoInvitado") & """},""back_urls"": {""success"": """ & dtDatosProveedor.Rows(0)("cvURLExito") & """,""failure"": """ & dtDatosProveedor.Rows(0)("cvURLError") & """,""pending"": """ & dtDatosProveedor.Rows(0)("cvURLError") & """}}"

                        objDatos.fnLog("mercadoPago", sPreferencia)
                        Dim preference = mp.createPreference(sPreferencia)

                        ' Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & lblTotal.Text.Replace("$", "").Replace(",", "") & "}],""payer"": {""email"": ""CorreoComprador@gmail.com""},""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""},""auto_return"": ""approved""}")

                        Dim sComando As String
                        sComando = "<script type='text/javascript'> window.open('" & preference.Item("response")("sandbox_init_point") & "'); </script> "
                        Response.Write(sComando)
                    Catch ex As Exception
                        objDatos.Mensaje("Ha ocurrido un problema al contactar con Mercado Pago, Intente más tarde", Me.Page)
                    End Try


                    'Dim mp As MP = New MP("3215552140914099", "que5OV1ia0dBByAEAdEnoiLiqw6lcq9g")
                    'mp.sandboxMode(True)
                    'Dim accessToken = mp.getAccessToken()
                    '' Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"",""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""}, ""quantity"": 1,""currency_id"":""MXN"",""unit_price"":" & lblTotal.Text.Replace("$", "") & "}]}")
                    'Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & lblTotal.Text.Replace("$", "").Replace(",", "") & "}],""payer"": {""email"": ""CorreoComprador@gmail.com""},""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""},""auto_return"": ""approved""}")
                    'Dim sComando As String
                    'sComando = "<script type='text/javascript'> window.open('" & preference.Item("response")("sandbox_init_point") & "','_blank'); </script> "
                    'Response.Write(sComando)

                End If

            End If

            If dtProveedor.Rows(0)(0) = "PayU" Then
                Session("Description") = "Pedido de " & sNombreEmpresa
                Session("ReferenceCode") = Session("NoPedido")
            End If

            If dtProveedor.Rows(0)(0) = "Conekta" Then
                If sNombreEmpresa.ToUpper.Contains("SALAMA") Then
                    Response.Redirect("checkout.aspx")
                End If
                If sNombreEmpresa.ToUpper.Contains("HAWK") Then
                    Response.Redirect("checkouthwk.aspx")
                End If

                If sNombreEmpresa.ToUpper.Contains("ALT") Then
                    Response.Redirect("checkoutAlt.aspx")
                End If

                If sNombreEmpresa.ToUpper.Contains("PEGADURO") Then
                    Response.Redirect("checkoutpd.aspx")
                End If
                If sNombreEmpresa.ToUpper.Contains("AIO") Then
                    Response.Redirect("checkoutaio.aspx")
                End If
                If sNombreEmpresa.ToUpper.Contains("PMK") Then
                    Response.Redirect("checkoutpmk.aspx")
                End If
                ' Response.Redirect("checkout.aspx")
            End If

            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                Response.Redirect("checkoutdlt.aspx")
            End If

            If dtProveedor.Rows(0)(0) = "OpenPay" Then
                Response.Redirect("checkoutop.aspx")
            End If

            If dtProveedor.Rows(0)(0) = "PixelPay" Then
                objDatos.fnLog("Resumen", "PixelPay")
                ssql = "SELECT * FROM config.Proveedores_Pago WHERE cvEmpresa ='PixelPay' "
                Dim dtDatosProveedor As New DataTable
                dtDatosProveedor = objDatos.fnEjecutarConsulta(ssql)
                If dtDatosProveedor.Rows.Count > 0 Then


                    Session("KEY") = dtDatosProveedor.Rows(0)("cvKEY")
                    Session("ResponseURL") = dtDatosProveedor.Rows(0)("cvURLExito")
                    Session("ErrorURL") = dtDatosProveedor.Rows(0)("cvURLError")

                    Dim sURLPixel As String = ""
                    sURLPixel = "https://www.pixelpay.app/hosted/payment/other?_key=" & Session("KEY") _
                        & " &_cancel=" & dtDatosProveedor.Rows(0)("cvURLError") _
                        & " &_complete=" & dtDatosProveedor.Rows(0)("cvURLExito") _
                        & " &_order_id=" & "PED-" & Session("NoPedido") _
                        & " &_callback=" & dtDatosProveedor.Rows(0)("cvCampoAdicional1") _
                        & " &_amount=" & Me.lblTotal.Text.Replace(" ", "").Replace("Lps", "").Replace(",", "") _
                        & " &_first_name=" & Session("NombreuserTienda") _
                        & " &_email=" & Session("CorreoInvitado") _
                        & " &_address=" & Session("CalleEnvio")
                    Response.Redirect(sURLPixel)
                End If
            End If


        Else
            ''Lo mandamos directo a resumen de compra para que solo haga el doc en SAP

            Response.Redirect("confirmacionCompra.aspx?ped=" & Session("NoPedido"))
        End If



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

    Public Function generarFirma()
        Dim sApiKey As String

        ssql = "SELECT ISNULL(cvCampoAdicional1,'') FROM config.Proveedores_Pago WHERE cvEmpresa ='PayU' "
        Dim dtDatosProveedor As New DataTable
        dtDatosProveedor = objDatos.fnEjecutarConsulta(ssql)
        If dtDatosProveedor.Rows.Count > 0 Then
            sApiKey = dtDatosProveedor.Rows(0)(0)
        Else
            sApiKey = "4Vj8eK4rloUd272L48hsrarnUA"
        End If

        Dim sMerchantId As String = Session("MerchantId")
        Dim total As Double = 0
        If lbltotalImp.Visible = True Then
            total = lbltotalImp.Text.Replace(sCaracterMoneda, "").Replace(" ", "").Replace("MXP", "").Replace(Session("Moneda"), "").Replace(",", "")
        Else
            lblTotal.Text.Replace(sCaracterMoneda, "").Replace(" ", "").Replace("MXP", "").Replace(Session("Moneda"), "").Replace(",", "")
        End If
        Dim Texto As String = sApiKey & "~" & sMerchantId & "~" & Session("ReferenceCode") & "~" & total.ToString.Replace(",", "") & "~" & "MXN" & ""  ' Cadena original ~
        objDatos.fnLog("Texto Sign: ", Texto)
        Dim PasConMd5 As String = ""
        Dim md5 As New MD5CryptoServiceProvider
        Dim bytValue() As Byte
        Dim bytHash() As Byte
        Dim i As Integer

        bytValue = System.Text.Encoding.UTF8.GetBytes(Texto)

        bytHash = md5.ComputeHash(bytValue)
        md5.Clear()

        For i = 0 To bytHash.Length - 1
            PasConMd5 &= bytHash(i).ToString("x").PadLeft(2, "0")
        Next
        Session("Firma") = PasConMd5
        Return PasConMd5

    End Function

End Class
