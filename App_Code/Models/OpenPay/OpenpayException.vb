Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Text

Namespace Openpay
    <JsonObject(MemberSerialization.OptIn)>
    Public Class OpenpayException
        Inherits Exception

        <JsonConstructor>
        Friend Sub New()
        End Sub

        Friend Shared Function GetFromJSON(ByVal code As HttpStatusCode, ByVal json As String) As OpenpayException
            Dim result As OpenpayException = JsonConvert.DeserializeObject(Of OpenpayException)(json)
            result.StatusCode = code
            Return result
        End Function

        <JsonProperty(PropertyName:="description")>
        Public Property Description As String
        <JsonProperty(PropertyName:="category")>
        Public Property Category As String
        <JsonProperty(PropertyName:="request_id")>
        Public Property RequestId As String
        <JsonProperty(PropertyName:="error_code")>
        Public Property ErrorCode As Integer
        Public Property StatusCode As HttpStatusCode

        Public Overrides ReadOnly Property Message As String
            Get
                Return Me.ErrorCode & ": " & Me.Description
            End Get
        End Property
    End Class
End Namespace

