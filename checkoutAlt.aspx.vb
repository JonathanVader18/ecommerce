﻿Imports System.Data
Imports System.IO
Imports System.Net
Imports conekta
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Partial Class checkoutAlt
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Public Shared Sub getApiKey()

        ' Api.apiKey = "key_oJenrWWRxVimEaeDXs6Rsw"
        Api.apiKey = "key_0zJ2LY8nhTlzWnrRyxeRn1q"
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
        Dim sMonedasistema As String = ""
        ssql = "select MainCurncy  from OADM "
        Dim dtMoneda As New DataTable
        dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtMoneda.Rows.Count > 0 Then
            sMonedasistema = dtMoneda.Rows(0)(0)
        End If


        objDatos.fnLog("Checkout", "Entra")
        getApiKey()
        Api.version = "2.0.0"
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

                    If Partida.Moneda <> sMonedasistema Then
                        If Partida.Descuento > 0 Then
                            ItemCompra.unit_price = (Partida.Precio * (1 - (Partida.Descuento / 100)) * 100) * CDbl(Session("TC"))
                        Else
                            ItemCompra.unit_price = (Partida.Precio * 100) * CDbl(Session("TC"))
                        End If
                    Else
                        If Partida.Descuento > 0 Then
                            ItemCompra.unit_price = Partida.Precio * (1 - (Partida.Descuento / 100)) * 100
                        Else
                            ItemCompra.unit_price = Partida.Precio * 100
                        End If
                    End If

                    lstItems.Add(ItemCompra)
                End If
            Next

            If ImporteEnvio > 0 Then
                ItemCompra = New Producto
                ItemCompra.name = "Envio"
                ItemCompra.quantity = 1
                ItemCompra.unit_price = CDbl(Session("ImporteEnvio")) * 100
                lstItems.Add(ItemCompra)
                ' objDatos.fnLog("Checkout envio: ", CDbl(Session("ImporteEnvio")))
            End If

            Dim lstCharger = New List(Of Cargo)() From {
            New Cargo() With {
                .payment_method = New MetodoPago() With {
                    .Type = "card",
                    .token_id = txtToken.Text
                }
            }
        }
            If Session("TelefonoEnvio") = "" Then
                Session("TelefonoEnvio") = "3333333333"
            End If
            Dim orden = New Orden() With {
            .Currency = "MXN",
            .customer_info = New comprador With {
                .name = Session("NombreuserTienda"),
                .Phone = "+52" & Session("TelefonoEnvio"),
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
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
            objDatos.fnEnviarCorreo("jpena@tie.com.mx", json, "conekta")
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
            Dim httpResponse As HttpWebResponse = CType(ex.Response, HttpWebResponse)
            Dim encoding = ASCIIEncoding.UTF8

            Using reader = New System.IO.StreamReader(httpResponse.GetResponseStream(), encoding)
                Dim responseText As String = reader.ReadToEnd()
                Dim lstErrores = JsonConvert.DeserializeObject(Of [Error])(responseText, New JsonSerializerSettings With {
                    .NullValueHandling = NullValueHandling.Ignore
                })
                Dim sError As String = "Error " & ex.Message
                objDatos.fnLog("Checkout error", "Error " & ex.Message.Replace("'", ""))
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
End Class
