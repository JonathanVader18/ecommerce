
Imports System.IO
Imports System.Net
Imports System.Xml

Partial Class testMoneta
    Inherits System.Web.UI.Page
    Public obj As New Cls_Funciones

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim dia As String = ""
            Dim mes As String = ""
            Dim Año As String = ""
            If Now.Date.Day.ToString.Length > 1 Then
                dia = Now.Date.Day
            Else
                dia = "0" & Now.Date.Day
            End If

            If Now.Date.Month.ToString.Length > 1 Then
                mes = Now.Date.Month
            Else
                mes = "0" & Now.Date.Month
            End If
            Año = Now.Date.Year.ToString.Substring(Now.Date.Year.ToString.Length - 2, 2)
            txtDate.Text = dia & mes & Año
            If Session("Audit") <> "" Then
                txtaudit.text = Session("Audit")
            End If
            If Session("TransId") <> "" Then
                txtOrigTransId.Text = Session("TransId")
            End If

        Else
            txtrespuesta.text = ""


        End If
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If txtCodMoneta.Text = "" Then
            obj.Mensaje("Debe indicar un código Moneta", Me.Page)
            Exit Sub
        End If

        If txtImporte.Text = "" Then
            obj.Mensaje("Debe indicar el importe de la transacción", Me.Page)
            Exit Sub
        End If

        txtAudit.Text = Convert.ToInt64(txtAudit.Text) + 1
        Dim valor As Int64
        valor = txtOrigTransId.Text.Substring(txtOrigTransId.Text.Length - 3, 3)
        valor = valor + 1

        txtOrigTransId.Text = (txtOrigTransId.Text.Substring(0, txtOrigTransId.Text.Length - 3) & valor.ToString).ToString
        txtRespuesta.Text = ""

        Session("TransId") = (txtOrigTransId.Text.Substring(0, txtOrigTransId.Text.Length - 3) & valor.ToString).ToString
        Session("Audit") = (Convert.ToInt64(txtAudit.Text) + 1).tostring

        Dim valorXML As String = ""
        valorXML = fnGenerarXML()
        consumirWS(valorXML)

    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Response.Redirect("testMoneta.aspx")
    End Sub

    Public Sub consumirWS(valorXML As String)
        Dim request As HttpWebRequest = CreateWebRequest()
        Dim soapEnvelopeXml As XmlDocument = New XmlDocument()


        soapEnvelopeXml.LoadXml(valorXML)

        Using stream As Stream = request.GetRequestStream()
            soapEnvelopeXml.Save(stream)
        End Using
        Try
            Using response As WebResponse = request.GetResponse()

                Using rd As StreamReader = New StreamReader(response.GetResponseStream())
                    Dim soapResult As String = rd.ReadToEnd()
                    txtRespuesta.Text = soapResult
                End Using
            End Using

            If txtRespuesta.Text.Contains("911") Then
                Try
                    Dim valorRespuesta As String

                    valorRespuesta = fnGenerarXMLREVERSA()
                    txtRespuesta.Text = valorRespuesta
                    consumirWS(valorRespuesta)


                Catch ex2 As Exception
                    obj.Mensaje(ex2.Message, Me.Page)
                End Try
            End If
        Catch ex As Exception
            obj.Mensaje(ex.Message, Me.Page)
            Try
                Dim valorRespuesta As String

                valorRespuesta = fnGenerarXMLREVERSA()
                txtRespuesta.Text = valorRespuesta
                consumirWS(valorRespuesta)
                'soapEnvelopeXml = New XmlDocument()
                'soapEnvelopeXml.LoadXml(txtRespuesta.Text)
                'request = CreateWebRequest()
                'Using response As WebResponse = request.GetResponse()

                '    Using rd As StreamReader = New StreamReader(response.GetResponseStream())
                '        Dim soapResult As String = rd.ReadToEnd()
                '        txtRespuesta.Text = soapResult
                '    End Using
                'End Using


            Catch ex2 As Exception
                obj.Mensaje(ex2.Message, Me.Page)
            End Try


        End Try


    End Sub

    Public Shared Function CreateWebRequest() As HttpWebRequest
        Dim webRequest As HttpWebRequest = CType(webRequest.Create("http://152.151.41.75:7610/OPM/OpenAPI/services"), HttpWebRequest)
        webRequest.Headers.Add("SOAP:Action")
        webRequest.ContentType = "text/xml;charset=""utf-8"""
        webRequest.Accept = "text/xml"
        webRequest.Method = "POST"
        Return webRequest
    End Function

    Public Function fnGenerarXML() As String
        Dim sXML As String = ""

        sXML = sXML & "<?xml version='1.0' encoding='UTF-8'?> "
        sXML = sXML & " <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:open='http://trivnet.com/OpenAPI'> "
        sXML = sXML & " <soapenv:Header /> "
        sXML = sXML & " <soapenv:Body> "
        sXML = sXML & " <open:invoke> "
        sXML = sXML & " <!--Optional:--> "
        sXML = sXML & " <arg0> "
        sXML = sXML & " <!--Zero or more repetitions:--> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>serviceName</name> "
        sXML = sXML & " <singleValue>debitAndCreditAccounts</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>transactionType</name> "
        sXML = sXML & " <singleValue>02</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>systemDate</name> "
        sXML = sXML & " <singleValue>00000" & txtDate.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>countryCode</name> "
        sXML = sXML & " <singleValue>484</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>originatorTransactionId</name> "
        sXML = sXML & " <singleValue>" & txtOrigTransId.Text & txtCodMoneta.Text.Substring(txtCodMoneta.Text.Length - 4, 4) & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>auditNumber</name> "
        sXML = sXML & " <singleValue>" & txtAudit.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>accessMethod</name> "
        sXML = sXML & " <singleValue>117</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>currency</name> "
        sXML = sXML & " <singleValue>484</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>amount</name> "
        sXML = sXML & " <singleValue>" & txtImporte.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>sourceAccountId</name> "
        sXML = sXML & " <singleValue>" & txtCodMoneta.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>sourceAccountIdType</name> "
        sXML = sXML & " <singleValue>105</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>merchantId</name> "
        sXML = sXML & " <singleValue>" & txtMerchantId.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>posId</name> "
        sXML = sXML & " <singleValue>098754321098732109854321654321</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " </arg0> "
        sXML = sXML & " </open:invoke> "
        sXML = sXML & " </soapenv:Body> "
        sXML = sXML & " </soapenv:Envelope> "


        Return sXML

    End Function


    Public Function fnGenerarXMLREVERSA() As String
        Dim sXML As String = ""

        sXML = sXML & "<?xml version='1.0' encoding='UTF-8'?> "
        sXML = sXML & " <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:open='http://trivnet.com/OpenAPI'> "
        sXML = sXML & " <soapenv:Header /> "
        sXML = sXML & " <soapenv:Body> "
        sXML = sXML & " <open:invoke> "
        sXML = sXML & " <!--Optional:--> "
        sXML = sXML & " <arg0> "
        sXML = sXML & " <!--Zero or more repetitions:--> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>serviceName</name> "
        sXML = sXML & " <singleValue>debitAndCreditAccountsReversal</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>transactionType</name> "
        sXML = sXML & " <singleValue>02</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>systemDate</name> "
        sXML = sXML & " <singleValue>00000" & txtDate.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>countryCode</name> "
        sXML = sXML & " <singleValue>484</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>originalOriginatorTransactionId</name> "
        sXML = sXML & " <singleValue>" & txtOrigTransId.Text & txtCodMoneta.Text.Substring(txtCodMoneta.Text.Length - 4, 4) & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>auditNumber</name> "
        sXML = sXML & " <singleValue>123456</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>accessMethod</name> "
        sXML = sXML & " <singleValue>117</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>originalAuditNumber</name> "
        sXML = sXML & " <singleValue>" & txtAudit.Text & "</singleValue> "
        sXML = sXML & " </values> "

        sXML = sXML & " </arg0> "
        sXML = sXML & " </open:invoke> "
        sXML = sXML & " </soapenv:Body> "
        sXML = sXML & " </soapenv:Envelope> "


        Return sXML

    End Function

End Class
