Imports System.Data
Imports System.IO
Imports System.Net
Imports conekta
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Partial Class checkout
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            '''Cargamos los datos de la empresa
            'ssql = "select cvNombreComercial ,cvSlogan,cvCorreo,cvTelefono from SAP_Tienda ..DatosCliente "
            'Dim dtEmpresa As New DataTable
            'dtEmpresa = objdatos.fnEjecutarConsulta(ssql)
            'lblCorreo.Text = dtEmpresa.Rows(0)("cvCorreo")
            'lblCorreoTienda.Text = dtEmpresa.Rows(0)("cvCorreo")
            '''    lblNombreEmpresa.Text = dtEmpresa.Rows(0)("cvNombreComercial")
            'lblNombreLargo.Text = dtEmpresa.Rows(0)("cvNombreComercial")
            'lblSlogan.Text = dtEmpresa.Rows(0)("cvSlogan")
            ''   lblSlogan2.Text = dtEmpresa.Rows(0)("cvSlogan")
            'lblTelefono.Text = dtEmpresa.Rows(0)("cvTelefono")
            'lblTelefono2.Text = dtEmpresa.Rows(0)("cvTelefono")


            'ssql = "select distinct cvGrupo1 as Grupo1 from SAP_Tienda..Agrupadores "
            'Dim scriptMenu As String
            'scriptMenu = "<ul class='nav navbar-nav navbar-right'> "
            'scriptMenu = scriptMenu & "<li class='dropdown megamenu'> "
            'scriptMenu = scriptMenu & "<a href='#' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' data-delay='300' data-close-others='true'>La Tienda</a> "
            'scriptMenu = scriptMenu & "<ul class='dropdown-menu'> "

            'Dim dtMenu As New DataTable
            'dtMenu = objdatos.fnEjecutarConsulta(ssql)
            'For i = 0 To dtMenu.Rows.Count - 1 Step 1
            '    scriptMenu = scriptMenu & " <li class='col-sm-4 col-md-3'> "
            '    scriptMenu = scriptMenu & "  <ul class='list-unstyled'> "
            '    scriptMenu = scriptMenu & "   <li class='title'> " & dtMenu.Rows(i)("Grupo1") & "</li>"
            '    ssql = "select distinct cvGrupo2 as Grupo2 from SAP_Tienda..Agrupadores WHERE cvGrupo1=" & "'" & dtMenu.Rows(i)("Grupo1") & "'"
            '    Dim dtGrupo2 As New DataTable
            '    dtGrupo2 = objdatos.fnEjecutarConsulta(ssql)

            '    For j = 0 To dtGrupo2.Rows.Count - 1 Step 1
            '        scriptMenu = scriptMenu & " <li><a href='products.aspx?Cat=" & dtGrupo2.Rows(j)("Grupo2") & "'> " & dtGrupo2.Rows(j)("Grupo2") & "</a></li>"
            '    Next
            '    scriptMenu = scriptMenu & " </ul> "
            '    scriptMenu = scriptMenu & " </li> "
            'Next

            'scriptMenu = scriptMenu & "   </ul> " '' dropdown-menu
            'scriptMenu = scriptMenu & "   </li> " ''dropdown megamenu
            'scriptMenu = scriptMenu & " </ul> " ''nav navbar-nav navbar-right

            'pnlMenu.Controls.Add(New LiteralControl(scriptMenu))


        End If
    End Sub

    Public Shared Sub getApiKey()

        Api.apiKey = "key_rQyxq6ijMsTyV3QFhZyrPw"
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

                    If Partida.Descuento > 0 Then
                        ItemCompra.unit_price = Partida.Precio * (1 - (Partida.Descuento / 100)) * 100
                    Else
                        ItemCompra.unit_price = Partida.Precio * 100
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
                    .type = "card",
                    .token_id = txtToken.Text
                }
            }
        }
            Dim orden = New Orden() With {
            .currency = "MXN",
            .customer_info = New comprador With {
                .name = Session("NombreuserTienda"),
                .phone = "+523333333333",
                .email = Session("CorreoInvitado")
            },
            .line_items = lstItems,
            .charges = lstCharger
        }
            orden.livemode = "true"
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
            Dim httpResponse As HttpWebResponse = CType(ex.Response, HttpWebResponse)
            Dim encoding = ASCIIEncoding.UTF8

            Using reader = New System.IO.StreamReader(httpResponse.GetResponseStream(), encoding)
                Dim responseText As String = reader.ReadToEnd()
                Dim lstErrores = JsonConvert.DeserializeObject(Of [Error])(responseText, New JsonSerializerSettings With {
                    .NullValueHandling = NullValueHandling.Ignore
                })
                Dim sError As String = "Error " & ex.Message

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
