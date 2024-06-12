Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    <JsonObject(MemberSerialization.OptIn)>
    Public Class Store
        <JsonProperty(PropertyName:="reference")>
        Public Property Reference As String
        <JsonProperty(PropertyName:="barcode_url")>
        Public Property BarcodeURL As String
    End Class
End Namespace
