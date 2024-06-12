Imports System.Data
Imports System.IO
Imports System.Net
Imports conekta
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Partial Class checkoutdlt
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Public Shared Sub getApiKey()

        ' Api.apiKey = "key_pnjyPmqJxGV8nxhMyz3zhA"
        Api.apiKey = "key_ezdexynHLhdUvjAyB5NzDw"
        ' Api.apiKey = "key_eYvWV7gSDkNYXsmr"
    End Sub

    Public Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Dim objDatos As New Cls_Funciones

        Dim ImporteEnvio As Double = 0


        If CDbl(Session("ImporteEnvio")) > 0 Then
            ImporteEnvio = CDbl(Session("ImporteEnvio"))

        End If


        If Session("NombreuserTienda") = "" Then
            Session("NombreuserTienda") = "Usuario Invitado"
        End If

        If Session("CorreoInvitado") = "" Then
            Session("CorreoInvitado") = "jpena@tie.com.mx"
        End If
        ServicePointManager.Expect100Continue = True
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        objDatos.fnLog("Checkout", "Entra")
        getApiKey()
        Api.version = "2.1.0"
        Dim Order As OrderResponse
        Dim lstItems = New List(Of Producto)()
        Try

            Dim ItemCompra As New Producto

            '            Dim lstItems = New List(Of Producto)() From {
            '    New Producto() With {
            '        .name = "tacos",
            '        .unit_price = 10,
            '        .quantity = 120
            '    }
            '}
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    ItemCompra = New Producto
                    ItemCompra.name = Partida.ItemName
                    ItemCompra.quantity = Partida.Cantidad

                    objDatos.fnLog("Checkout", Partida.ItemName)

                    If Partida.Descuento > 0 Then
                        ItemCompra.unit_price = (Partida.Precio) * (1 - (Partida.Descuento / 100)) * 100
                    Else
                        ItemCompra.unit_price = (Partida.Precio) * 100
                    End If
                    lstItems.Add(ItemCompra)
                End If
            Next

            If ImporteEnvio > 0 Then
                ItemCompra = New Producto
                ItemCompra.name = "Envio"
                ItemCompra.quantity = 1
                ItemCompra.unit_price = (CDbl(Session("ImporteEnvio"))) * 100
                lstItems.Add(ItemCompra)
                ' objDatos.fnLog("Checkout envio: ", CDbl(Session("ImporteEnvio")))
            End If

            Dim lstCharger = New List(Of Cargo)() From {
            New Cargo() With {
                .payment_method = New MetodoPago() With {
                    .type = "card",
                    .token_id = txtToken.Text
                }
            }
        }
            If Session("TelefonoEnvio") = "" Then
                Session("TelefonoEnvio") = "523333333334"
            End If
            Dim orden = New Orden() With {
            .currency = "MXN",
            .customer_info = New comprador With {
                .name = Session("NombreuserTienda"),
                .phone = Session("TelefonoEnvio"),
                .email = Session("CorreoInvitado")
            },
            .line_items = lstItems,
            .charges = lstCharger
        }
            ' orden.livemode = "true"
            ' orden.amount = CInt(Session("TotalCarrito"))
            objDatos.fnLog("Checkout cliente: ", Session("NombreuserTienda"))
            Dim json = JsonConvert.SerializeObject(orden)
            'lblError.Text = json
            'lblError.Visible = True
            'Exit Sub


            Dim uname = Environment.OSVersion
            Dim http As HttpWebRequest = CType(WebRequest.Create(conekta.Api.baseUri & "/orders"), HttpWebRequest)
            http.Accept = "application/vnd.conekta-v" & conekta.Api.version & "+json"
            http.UserAgent = "Conekta/v1 DotNetBindings10/Conekta::" & conekta.Api.version
            http.Method = "POST"
            Dim plainTextBytes = System.Text.Encoding.UTF8.GetBytes(conekta.Api.apiKey)
            Dim userAgent = ("{""bindings_version"":""" & Api.version & """,""lang"":"".net"",""lang_version"":""" & GetType(String).Assembly.ImageRuntimeVersion & """,""publisher"":""conekta"",""uname"":""" & uname.ToString & """}")


            http.Headers.Add("Authorization", "Basic " & System.Convert.ToBase64String(plainTextBytes) & ":")
            http.Headers.Add("Accept-Language", conekta.Api.locale)
            http.Headers.Add("X-Conekta-Client-User-Agent", userAgent)
            Dim dataBytes = Encoding.UTF8.GetBytes(json)
            '   objDatos.fnEnviarCorreo("jpena@tie.com.mx", json, "conekta")
            http.ContentLength = dataBytes.Length
            http.ContentType = "application/json"
            Dim dataStream As Stream = http.GetRequestStream()
            dataStream.Write(dataBytes, 0, dataBytes.Length)
            Dim responseConekta As WebResponse = http.GetResponse()
            Dim responseString = New StreamReader(responseConekta.GetResponseStream()).ReadToEnd()
            Order = JsonConvert.DeserializeObject(Of OrderResponse)(responseString, New JsonSerializerSettings With {
                .NullValueHandling = NullValueHandling.Ignore
            })
            objDatos.fnLog("Checkout", Order.payment_status)

            If Order.payment_status = "paid" Then
                Response.Redirect("confirmacioncompra.aspx?Ped=" & Order.id)
                Exit Sub
            End If
            'order2 = JsonConvert.DeserializeObject(Of Orden)(responseString, New JsonSerializerSettings With {
            '    .NullValueHandling = NullValueHandling.Ignore
            '})


            'Dim oCorreo = New Correo() With {
            '    .Email = "rogelio_clemente@pegaduro.com",
            '    .Body = "{JsonConvert.SerializeObject(Order)}",
            '    .Subject = "Pago Realizado Conekta",
            '    .Cco = New List(Of String)() From {
            '        "rogelioclemente31@hotmail.com",
            '        "jonathan_pena@pegaduro.com"
            '    }
            '}
            '  Dim objdatos As New Cls_Funciones
            '  objdatos.fnEnviarCorreo("jpena@tie.com.mx", JsonConvert.SerializeObject(Order), "Pago Conekta")
            'Dim request = WebRequest.Create("http://webserver.pegaduro.com.mx/APIQA/api/Email")
            'request.Method = "POST"
            'Dim _json = JsonConvert.SerializeObject(oCorreo)
            'request.ContentLength = Encoding.UTF8.GetBytes(_json).Length
            'request.ContentType = "application/json"
            'Dim stream As Stream = request.GetRequestStream()
            'stream.Write(Encoding.UTF8.GetBytes(_json), 0, Encoding.UTF8.GetBytes(_json).Length)
            'Dim respuesta = New StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd()
            lblError.Text = "Pago Realizado con éxito:" & Order.payment_status
            lblError.CssClass = "text-success"
            '  btnConfirmar.Visible = False
        Catch ex As WebException
            lblError.Visible = True
            Dim sError As String = "Error " & ex.Message
            objDatos.fnLog("Checkout error", "Error " & ex.Message.Replace("'", ""))
            Dim httpResponse As HttpWebResponse = CType(ex.Response, HttpWebResponse)
            Dim encoding = ASCIIEncoding.UTF8

            Using reader = New System.IO.StreamReader(httpResponse.GetResponseStream(), encoding)
                Dim responseText As String = reader.ReadToEnd()
                Dim lstErrores = JsonConvert.DeserializeObject(Of [Error])(responseText, New JsonSerializerSettings With {
                    .NullValueHandling = NullValueHandling.Ignore
                })

                For Each Error1 In lstErrores.details
                    sError += Error1.message
                Next

                lblError.Text = sError & vbLf & " Recargar la pagina.(" & lstItems.Count & ")"
                lblError.Visible = True
                btnConfirmar.Visible = False
            End Using
        End Try

        txtToken.Visible = False
    End Sub

    Private Sub checkoutdlt_Load(sender As Object, e As EventArgs) Handles Me.Load
        objdatos.fnLog("Checkout Delta, envio", Session("ImporteEnvio"))
        If CDbl(Session("ImporteEnvio")) = 0 Then
            fnAgregaFletesSeguros_StopCatalogo()
            objdatos.fnLog("Checkout Delta, envio", "Fue cero, entra de nuevo")
            objdatos.fnLog("Checkout Delta, confirma envio", Session("ImporteEnvio"))
        End If

    End Sub
    Public Sub fnAgregaFletesSeguros_StopCatalogo()



        objdatos.fnLog("Fletes STOP", "Entra")
        Dim iCantPiezasTotales As Int16 = 0
        Dim fMontoCarrito As Double = 0
        Dim iExisteFlete As Int16 = 0
        objdatos.fnLog("fnAgregaFletesSeguros_StopCatalogo", "Entra")
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then
                    iExisteFlete = 1
                Else
                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objdatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objdatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objdatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If
                    iCantPiezasTotales = iCantPiezasTotales + Partida.Cantidad
                    objdatos.fnLog("Fletes STOP", "Monto Carrito: " & Partida.TotalLinea)
                    fMontoCarrito = fMontoCarrito + ((Partida.Precio * Partida.Cantidad) * (1 - (Partida.Descuento / 100)))
                    'fMontoCarrito = fMontoCarrito + Partida.TotalLinea
                End If

            End If
        Next

        If fMontoCarrito = 0 Then

        End If
        objdatos.fnLog("Fletes STOP", "Monto Carrito: " & fMontoCarrito)
        objdatos.fnLog("Fletes STOP", "Piezas: " & iCantPiezasTotales)
        ''Teniendo las piezas totales y el importe, determinamos el monto del flete
        ''-Reglas-------------------
        ''--Cada 70 prendas se cargan 60 Pesos de flete
        ''--Por cada Mil pesos se cargan al concepto de Flete 15 pesos
        Dim iMontoFleteGratis As Double = 20000000

        If Session("RazonSocial") = "" Then
            iMontoFleteGratis = 999
        Else
            ''Por promo o estrategia, Delta manejará flete gratis al alcanzar 899 para sus socios B2B
            iMontoFleteGratis = objdatos.fnPromoFleteDeltaSocios()
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
End Class
