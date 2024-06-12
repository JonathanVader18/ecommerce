Imports Newtonsoft.Json.Linq
Imports System.Collections.Generic

Namespace conekta
    Public MustInherit Class Api
        Public Const baseUri As String = "https://api.conekta.io"
        Public Shared Property version As String
        Public Shared Property locale As String
        Public Shared Property apiKey As String
    End Class
End Namespace
