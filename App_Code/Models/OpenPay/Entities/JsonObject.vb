Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json

Namespace Openpay.Entities
    <JsonObject(MemberSerialization.OptIn)>
    Public Class JsonObject
        Public Function ToJson() As String
            Return JsonConvert.SerializeObject(Me)
        End Function
    End Class
End Namespace

