Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json

Namespace Openpay.Entities.Request
    <JsonObject(MemberSerialization.OptIn)>
    Friend Class RefundRequest
        Inherits JsonObject

        <JsonProperty(PropertyName:="description")>
        Public Property Description As String
        <JsonProperty(PropertyName:="amount")>
        Public Property Amount As Decimal?
    End Class
End Namespace

