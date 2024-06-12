Imports Newtonsoft.Json
Imports Openpay.Entities
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text

Namespace Openpay
    Public Class OpenpayHttpClient
        Private Shared ReadOnly api_endpoint As String = "https://api.openpay.mx/v1/"
        Private Shared ReadOnly api_endpoint_sandbox As String = "https://sandbox-api.openpay.mx/v1/"
        Private Shared ReadOnly user_agent As String = "Openpay .NET v1"
        Private Shared ReadOnly encoding As Encoding = Encoding.UTF8
        Private _isProduction As Boolean = False
        Public Property TimeoutSeconds As Integer
        Public Property MerchantId As String
        Public Property APIEndpoint As String
        Public Property APIKey As String

        Public Sub New(ByVal api_key As String, ByVal merchant_id As String, ByVal Optional production As Boolean = False)
            If String.IsNullOrEmpty(api_endpoint_sandbox) Then Throw New ArgumentNullException("api_key")
            If String.IsNullOrEmpty(merchant_id) Then Throw New ArgumentNullException("merchant_id")
            MerchantId = merchant_id
            APIKey = api_key
            TimeoutSeconds = 120
            production = production
        End Sub

        Public Property Production As Boolean
            Get
                Return _isProduction
            End Get
            Set(ByVal value As Boolean)
                APIEndpoint = If(value, api_endpoint, api_endpoint_sandbox)
                _isProduction = value
            End Set
        End Property

        Protected Overridable Function SetupRequest(ByVal method As String, ByVal url As String) As WebRequest
            Dim req As WebRequest = CType(WebRequest.Create(url), WebRequest)
            'Dim req As WebRequest = CType(WebRequest.Create(api_endpoint_sandbox & "/charges"), WebRequest)
            req.Method = method

            If TypeOf req Is HttpWebRequest Then
                CType(req, HttpWebRequest).UserAgent = user_agent
            End If

            Dim authInfo As String = APIKey & ":"
            authInfo = Convert.ToBase64String(Encoding.[Default].GetBytes(authInfo))
            req.Headers("Authorization") = "Basic " & authInfo
            req.PreAuthenticate = False
            req.Timeout = TimeoutSeconds * 1000
            If method = "POST" OrElse method = "PUT" Then req.ContentType = "application/json"
            Return req
        End Function

        Protected Function GetResponseAsString(ByVal response As WebResponse) As String
            Using sr As StreamReader = New StreamReader(response.GetResponseStream(), encoding)
                Return sr.ReadToEnd()
            End Using
        End Function

        Public Function Post(Of T)(ByVal endpoint As String, ByVal obj As JsonObject) As T
            Dim json = DoRequest(endpoint, HttpMethod.POST, obj.ToJson())
            Return JsonConvert.DeserializeObject(Of T)(json)
        End Function

        Public Sub Post(Of T)(ByVal endpoint As String)
            DoRequest(endpoint, HttpMethod.POST, Nothing)
        End Sub

        Public Function [Get](Of T)(ByVal endpoint As String) As T
            Dim json = DoRequest(endpoint, HttpMethod.[GET], Nothing)
            Return JsonConvert.DeserializeObject(Of T)(json)
        End Function

        Public Function Put(Of T)(ByVal endpoint As String, ByVal obj As JsonObject) As T
            Dim json = DoRequest(endpoint, HttpMethod.PUT, obj.ToJson())
            Return JsonConvert.DeserializeObject(Of T)(json)
        End Function

        Public Function Cancel(Of T)(ByVal endpoint As String, ByVal obj As JsonObject) As T
            Dim json = DoRequest(endpoint, HttpMethod.POST, obj.ToJson())
            Return JsonConvert.DeserializeObject(Of T)(json)
        End Function

        Public Sub Cancel(Of T)(ByVal endpoint As String)
            DoRequest(endpoint, HttpMethod.POST, Nothing)
        End Sub

        Public Sub Delete(ByVal endpoint As String)
            DoRequest(endpoint, HttpMethod.DELETE, Nothing)
        End Sub

        Protected Overridable Function DoRequest(ByVal path As String, ByVal method As HttpMethod, ByVal body As String) As String
            Dim result As String = Nothing
            Dim endpoint As String = APIEndpoint & MerchantId & path
            Console.WriteLine("Request to: " & endpoint)
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            Dim req As WebRequest = SetupRequest(method.ToString(), endpoint)

            If body IsNot Nothing Then
                Dim bytes As Byte() = encoding.GetBytes(body.ToString())
                req.ContentLength = bytes.Length

                Using st As Stream = req.GetRequestStream()
                    st.Write(bytes, 0, bytes.Length)
                End Using
            End If

            Try

                Using resp As WebResponse = CType(req.GetResponse(), WebResponse)
                    result = GetResponseAsString(resp)
                End Using

            Catch wexc As WebException

                If wexc.Response IsNot Nothing Then
                    Dim json_error As String = GetResponseAsString(wexc.Response)
                    Dim status_code As HttpStatusCode = HttpStatusCode.BadRequest
                    Dim resp As HttpWebResponse = TryCast(wexc.Response, HttpWebResponse)
                    If resp IsNot Nothing Then status_code = resp.StatusCode
                    '   If CInt(status_code) <= 500 Then Throw OpenpayException.GetFromJSON(status_code, json_error)
                    If CInt(status_code) <= 500 Then result = json_error
                End If

                'Throw
            End Try

            Return result
        End Function

        Public Enum HttpMethod
            [GET]
            POST
            DELETE
            PUT
        End Enum
    End Class
End Namespace

