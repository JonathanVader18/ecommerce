Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json

Namespace Openpay.Entities.Request
    <JsonObject(MemberSerialization.OptIn)>
    Friend Class CaptureRequest
        Inherits JsonObject

        <JsonProperty(PropertyName:="amount")>
        Public Property Amount As Decimal?
    End Class
End Namespace

