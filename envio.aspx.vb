Imports System.Data
Imports System.IO

Partial Class pago_envio
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Public sCaracterMoneda As String = ""
    Private Sub pago_envio_Load(sender As Object, e As EventArgs) Handles Me.Load
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
        If Not IsPostBack Then

        End If
        ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
        Dim dtCaracter As New DataTable
        dtCaracter = objDatos.fnEjecutarConsulta(ssql)
        If dtCaracter.Rows.Count > 0 Then
            sCaracterMoneda = dtCaracter.Rows(0)(0)
        End If

        fnMetodosEnvio()
        fnCargarCarrito()
        fnTotales()
        If Not IsPostBack Then
            Session("PesoPaquete") = 0
        End If
    End Sub
    Public Sub fnMetodosEnvio()
        Dim sHtmlEncabezado As String = ""
        Dim sHtml As String = ""
        Dim MinCompra As Double = 0
        Dim dtMetodos As New DataTable

        ''Revisamos si en las parametrizaciones tiene alguna paquetería configurada
        ssql = "SELECT ISNULL(cvPaqueteria,'') FROM config.parametrizaciones"
        Dim dtPaq As New DataTable
        dtPaq = objDatos.fnEjecutarConsulta(ssql)
        If dtPaq.Rows.Count > 0 Then
            If dtPaq.Rows(0)(0) <> "" Then
                ''Tenemos paqueteria
                Session("NombrePaqueteria") = dtPaq.Rows(0)(0)
                fnCargaPaqueteria()


            Else
                ''Tiene configurados metodos de envio propios, por minimo de compra, etc
                ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
                Dim dtcliente As New DataTable
                dtcliente = objDatos.fnEjecutarConsulta(ssql)
                If dtcliente.Rows.Count > 0 Then
                    If CStr(dtcliente.Rows(0)(0)).Contains("Salama") Then
                        ssql = "select ISNULL(cfMinimoCompraB2C,0)  from config .Parametrizaciones  "
                        Dim dtminCompra As New DataTable
                        dtminCompra = objDatos.fnEjecutarConsulta(ssql)
                        If dtminCompra.Rows.Count > 0 Then
                            MinCompra = dtminCompra.Rows(0)(0)
                            If CDbl(Session("TotalCarrito")) < MinCompra Then
                                ''No ha alcanzado el minimo de compra, le habilitamos la opción de cobro
                                ssql = "select * from config.MetodosEnvio WHERE cvImporte >0  "
                            Else
                                ssql = "select * from config.MetodosEnvio WHERE cvImporte =0  "
                            End If
                        End If
                    Else
                        If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                            ssql = "Select ciIdRel as ciIdMetodoEnvio, cfImporte as cvImporte,'MXP' as cvMoneda, cvPaqueteria as cvClave , cvTipoServicio as cvDescripcion from config.Paqueterias_costos where cvEstado=" & "'" & Session("EstadoEnvio") & "' and cvPoblacion=" & "'" & Session("PoblacionEnvio") & "'"
                        Else
                            ''Cargamos los métodos de envio
                            ssql = "select * from config.MetodosEnvio  "
                        End If

                    End If
                End If



                dtMetodos = objDatos.fnEjecutarConsulta(ssql)
                sHtmlEncabezado = sHtmlEncabezado & ""
                Dim radio As New RadioButton
                Dim sImporte As Double = 0
                For i = 0 To dtMetodos.Rows.Count - 1 Step 1




                    radio.ID = "rb" & dtMetodos.Rows(i)("ciIdMetodoEnvio")
                    If CDbl(dtMetodos.Rows(i)("cvImporte")) = 0 Then
                        radio.Text = dtMetodos.Rows(i)("cvClave") & "(" & dtMetodos.Rows(i)("cvDescripcion") & ") - Sin Costo de envío"
                    Else
                        radio.Text = dtMetodos.Rows(i)("cvClave") & "(" & dtMetodos.Rows(i)("cvDescripcion") & ") - " & CDbl(dtMetodos.Rows(i)("cvImporte")).ToString("###,###,###.#0") & " " & dtMetodos.Rows(i)("cvMoneda")
                    End If
                    sImporte = CDbl(dtMetodos.Rows(i)("cvImporte"))
                    radio.AutoPostBack = True
                    AddHandler radio.CheckedChanged, AddressOf Checked
                    Dim literal As New LiteralControl("</br>")

                    If dtMetodos.Rows.Count = 1 Then
                        radio.Checked = True
                        Session("MetodoEnvio") = dtMetodos.Rows(i)("ciIdMetodoEnvio")
                    End If
                    pnlMetodosEnvio.Controls.Add(radio)
                    pnlMetodosEnvio.Controls.Add(literal)
                Next
                If CDbl(Session("ImporteEnvio")) > 0 Then
                    ''Ya viene con monto el envio
                    radio = New RadioButton
                    radio.ID = "rb99"
                    radio.Text = "Envío calculado por consumo"
                    radio.AutoPostBack = True
                    radio.Checked = True
                    AddHandler radio.CheckedChanged, AddressOf Checked
                    pnlMetodosEnvio.Controls.Clear()
                    pnlMetodosEnvio.Controls.Add(radio)
                Else
                    If dtMetodos.Rows.Count = 1 Then
                        Session("ImporteEnvio") = sImporte

                        fnTotales()
                    End If
                End If

            End If
        End If


        'sHtml = sHtml & "<div class='col-xs-12'>"
        'sHtml = sHtml & "  <a href='envio.aspx?op=" & dtMetodos.Rows(i)("ciIdMetodoEnvio") & "'><div class='radio'>"
        'sHtml = sHtml & "  <label>"
        'sHtml = sHtml & "   <input type='radio' name='optionsRadios' id='optionsRadios" & (i + 1) & "' value='option" & (i + 1) & "' checked> " & dtMetodos.Rows(i)("cvClave") & "(" & dtMetodos.Rows(i)("cvDescripcion") & ") - $ " & CDbl(dtMetodos.Rows(i)("cvImporte")).ToString("###,###,###.#0") & " " & dtMetodos.Rows(i)("cvMoneda")
        'sHtml = sHtml & "   </label>  <asp:RadioButton ID='rb" & (i + 1) & "' runat='server' AutoPostBack ='true' Text='" & dtMetodos.Rows(i)("cvClave") & "(" & dtMetodos.Rows(i)("cvDescripcion") & ") - $ " & CDbl(dtMetodos.Rows(i)("cvImporte")).ToString("###,###,###.#0") & " " & dtMetodos.Rows(i)("cvMoneda") & "' />"
        'sHtml = sHtml & " </div></a>"
        'sHtml = sHtml & "</div>"


    End Sub
    Public Sub fnCargaPaqueteria()
        Dim objPaq As New Cls_Paqueteria
        Dim BearerToken As String = objPaq.FnObtenerToken()
        Session("UsaPaqueteria") = "SI"
        objDatos.fnLog("Paqueteria", "Entra")
        objDatos.fnLog("Paqueteria-Token", BearerToken.Replace("'", ""))
        Session("BearerToken") = BearerToken

        objDatos.fnLog("Paqueteria", "Antes de modelo cotizacion")

        Dim destino As New Address
        Dim ListaPaquetes As New List(Of Paquete)

        Try
            destino = fnCrearDestinoPaqueteria()


            ListaPaquetes = fnCrearPaquetesEnvios()

            objDatos.fnLog("Paquetes:", ListaPaquetes.Count)
            objDatos.fnLog("destino:", destino.addressLine)
            objDatos.fnLog("destino:", destino.city)
            objDatos.fnLog("destino:", destino.postalCode)

        Catch ex As Exception
            objDatos.fnLog("Armado de objetos", ex.Message.Replace("'", ""))
        End Try


        Dim Cotizacion As New CotizacionRequest
        Cotizacion = objPaq.FnCreaModeloCotizacion(destino, ListaPaquetes)
        Cotizacion.destino.attentionName = Session("NombreEnvio")
        Dim telefono As New Phone
        telefono.number = Session("TelefonoEnvio")
        Cotizacion.destino.phone = telefono

        Session("CotizacionRequest") = Cotizacion
        objDatos.fnLog("Paqueteria", "Antes de respuesta cotizacion")
        Dim RespuestaCotizacion As New CotizacionResponse
        RespuestaCotizacion = objPaq.FnCotizar(Cotizacion, BearerToken)


        ''Pintar Resultados
        Dim ResponseCotizacion As New List(Of Cotizacion_Response)
        ResponseCotizacion = objPaq.FnRespuestaCotizacion(RespuestaCotizacion)

        objDatos.fnLog("Paqueteria", "Response cotizacion:" & ResponseCotizacion.Count)


        For Each TipoEnvio As Cotizacion_Response In ResponseCotizacion
            Dim radio As New RadioButton
            radio.ID = "rb" & TipoEnvio.codigo

            radio.Text = TipoEnvio.codigo & "(" & TipoEnvio.descripcion & ") - " & TipoEnvio.monto.ToString("###,###,##.0#") & " " & TipoEnvio.moneda
            radio.ToolTip = TipoEnvio.monto & "-" & TipoEnvio.moneda
            radio.AutoPostBack = True
            AddHandler radio.CheckedChanged, AddressOf Checked

            Dim literal As New LiteralControl("</br>")
            pnlMetodosEnvio.Controls.Add(radio)
            pnlMetodosEnvio.Controls.Add(literal)
        Next

        objDatos.fnLog("Peso Paquete", Session("PesoPaquete"))
    End Sub

    Public Function fnCrearDestinoPaqueteria() As Address
        Dim destino As New Address
        destino.addressLine = Session("CalleEnvio")
        destino.numExt = Session("NumExtEnvio")
        destino.numInt = Session("NumInteriorEnvio")
        destino.colonia = Session("ColoniaEnvio")
        destino.postalCode = Session("CPEnvio")
        destino.city = Session("CiudadEnvio")
        destino.estado = Session("EstadoEnvio")
        destino.countryCode = Session("PaisEnvio")

        Return destino
    End Function

    Public Function fnCrearPaquetesEnvios() As List(Of Paquete)
        Dim LstPaquetes As New List(Of Paquete)
        Dim Paquete As New Paquete

        Dim TipoPaquete As New PackagingType
        TipoPaquete.code = "02"
        TipoPaquete.description = "Paquete"

        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                Paquete = New Paquete
                Paquete.description = Partida.ItemCode & "-" & Partida.ItemName
                Paquete.packageWeight = fnPesoArticulo(Partida.ItemCode, Partida.Cantidad)
                Paquete.dimensions = fnDimensionesArticulo(Partida.ItemCode, Partida.Cantidad)
                Paquete.packagingType = TipoPaquete
                LstPaquetes.Add(Paquete)
            End If
        Next

        Return LstPaquetes
    End Function

    Public Function fnPesoArticulo(ItemCode As String, cantidad As Int16) As PackageWeight
        Dim Peso As New PackageWeight

        ssql = objDatos.fnObtenerQuery("GetPesoItem")
        ssql = ssql.Replace("[%0]", ItemCode)

        Dim dtDetallePeso As New DataTable
        dtDetallePeso = objDatos.fnEjecutarConsultaSAP(ssql)

        If dtDetallePeso.Rows.Count > 0 Then

            Dim UnitOfMes As New UnitOfMeasurement
            UnitOfMes.code = dtDetallePeso.Rows(0)("UnidadMedida")

            Peso.unitOfMeasurement = UnitOfMes
            Peso.weight = CDbl(dtDetallePeso.Rows(0)("Peso")) * cantidad
            Session("PesoPaquete") = Session("PesoPaquete") + (CDbl(dtDetallePeso.Rows(0)("Peso")) * cantidad)
        End If

        Return Peso
    End Function

    Public Function fnDimensionesArticulo(ItemCode As String, cantidad As Int16) As Dimensions
        Dim Dimensiones As New Dimensions

        ssql = objDatos.fnObtenerQuery("GetDimensionsItem")
        ssql = ssql.Replace("[%0]", ItemCode)

        Dim dtDetalleDimensiones As New DataTable
        dtDetalleDimensiones = objDatos.fnEjecutarConsultaSAP(ssql)

        If dtDetalleDimensiones.Rows.Count > 0 Then

            Dim UnitOfMes As New UnitOfMeasurement
            UnitOfMes.code = dtDetalleDimensiones.Rows(0)("UnidadMedida")

            Dimensiones.unitOfMeasurement = UnitOfMes
            Dimensiones.height = CInt(dtDetalleDimensiones.Rows(0)("Alto")) * cantidad
            Dimensiones.width = CInt(dtDetalleDimensiones.Rows(0)("Ancho"))
            Dimensiones.length = CInt(dtDetalleDimensiones.Rows(0)("Longitud"))
        End If

        Return Dimensiones
    End Function



    Private Sub Checked(sender As Object, e As EventArgs)
        Dim ClickedRadioButton As RadioButton = DirectCast(sender, RadioButton)

        '   Display the radio button name
        ' objDatos.Mensaje(String.Format("Radio Button {0} has been Updated!", ClickedRadioButton.ID), Me.Page)
        Session("MetodoEnvio") = ClickedRadioButton.ID.Replace("rb", "")
        Session("CodeMetodo") = ClickedRadioButton.ID.Replace("rb", "")
        ''Actualizamos el monto de envio en los totales
        ssql = "select * from config.MetodosEnvio  WHERE  ciIdMetodoEnvio=" & "'" & Session("MetodoEnvio") & "'"
            Dim dtCostoEnvio As New DataTable
        dtCostoEnvio = objDatos.fnEjecutarConsulta(ssql)
        If dtCostoEnvio.Rows.Count > 0 Then
            Session("ImporteEnvio") = CDbl(dtCostoEnvio.Rows(0)("cvImporte"))

            ''PAra bacan
            If dtCostoEnvio.Rows(0)("cvClave") = "Interior" Then
                pnlComentarios.Visible = True
                'Si tiene leyenda de comentarios la pintamos
                ssql = "select ISNULL(cvLeyendaComentariosB2C,'') FROM config.parametrizaciones "
                Dim dtLeyenda As New DataTable
                dtLeyenda = objDatos.fnEjecutarConsulta(ssql)
                If dtLeyenda.Rows.Count > 0 Then
                    If dtLeyenda.Rows(0)(0) <> "" Then
                        lblLeyendaComentarios.Visible = True
                        lblLeyendaComentarios.Text = dtLeyenda.Rows(0)(0)
                    End If
                End If
            Else
                pnlComentarios.Visible = False
                lblLeyendaComentarios.Visible = False
            End If
        Else
            If CDbl(Session("ImporteEnvio")) > 0 Then
                Session("MetodoEnvio") = "Calculado por consumo"
            Else
                Session("ImporteEnvio") = 0
            End If


        End If
            For Each radio As Control In pnlMetodosEnvio.Controls
            Try
                If DirectCast(radio, RadioButton).ID <> ClickedRadioButton.ID Then
                    DirectCast(radio, RadioButton).Checked = False
                End If
            Catch ex As Exception

            End Try

        Next

        ''Revisamos si en las parametrizaciones tiene alguna paquetería configurada
        ssql = "SELECT ISNULL(cvPaqueteria,'') FROM config.parametrizaciones"
        Dim dtPaq As New DataTable
        dtPaq = objDatos.fnEjecutarConsulta(ssql)
        If dtPaq.Rows.Count > 0 Then
            If dtPaq.Rows(0)(0) <> "" Then
                ''Tenemos paqueteria, tomamos el importe cotizado con un split

                Dim sDescripcionPaqueteria As String()
                sDescripcionPaqueteria = ClickedRadioButton.Text.Split("-")
                Session("MetodoEnvio") = sDescripcionPaqueteria(0)

                Dim sImportePaqueteria As String()
                sImportePaqueteria = ClickedRadioButton.ToolTip.Split("-")
                If ClickedRadioButton.Text.ToUpper.Contains("RECOGE") Or ClickedRadioButton.Text.ToUpper.Contains("ACORDAR") Then
                    Session("ImporteEnvio") = 0
                Else
                    Session("ImporteEnvio") = CDbl(sImportePaqueteria(0).Replace(",", ""))
                End If

            End If
        End If



        fnTotales()
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


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If



        lblTotal.Text = sCaracterMoneda & " " & (sSubTotal + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString(" ###,###,###.#0") & " " & Session("Moneda")

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

                Dim sMonedasistema As String = "MXP"
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
                            sSubTotal = sSubTotal + (Partida.Cantidad * precioConDescuento)
                        End If

                    Else
                        If Partida.Descuento > 0 Then
                            sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                        Else
                            sSubTotal = sSubTotal + (Partida.Cantidad * precioConDescuento)
                        End If

                    End If
                Else
                    If Partida.Descuento > 0 Then
                        sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    Else
                        sSubTotal = sSubTotal + (Partida.Cantidad * precioConDescuento)
                    End If
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

            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                '  Descuento = CDbl(lblDescuento.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
                Descuento = TotDescuento
            End If


            Envio = CDbl(Session("ImporteEnvio"))
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try

        lblSubTotal.Text = sCaracterMoneda & " " & CDbl(Session("ImporteSubTotal")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        sSubTotal = CDbl(Session("ImporteSubTotal"))
        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = "0.00"
        Else
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            lblEnvio.Visible = True
        End If

        lblTotal.Text = sCaracterMoneda & " " & CDbl(sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0") & " " & Session("Moneda")

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
                lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & " " & Session("Moneda")
                lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                Session("TotalImpuestos") = TotalImpuestos
            End If
        End If

    End Sub
    Protected Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try
            If Session("MetodoEnvio") = "" And CDbl(Session("ImporteEnvio")) = 0 Then
                objDatos.Mensaje("Debe indicar un método de envío", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception
            Session("MetodoEnvio") = "0"
        End Try

        If pnlComentarios.Visible = True Then
            If txtComentarios.Text = "" Then
                objDatos.Mensaje("Debe indicar comentarios cuando el envío es al interior", Me.Page)
                Exit Sub
            Else
                Session("Comentarios") = txtComentarios.Text
            End If
        End If
        If Session("PaginaPago") <> "" Then
            Response.Redirect(Session("PaginaPago"))
        Else
            Response.Redirect("resumen.aspx")
        End If

    End Sub

End Class
