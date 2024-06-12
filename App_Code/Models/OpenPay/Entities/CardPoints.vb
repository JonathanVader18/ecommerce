Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class CardPoints
        <JsonProperty(PropertyName:="used")>
        Public Property Used As Decimal
        <JsonProperty(PropertyName:="remaining")>
        Public Property Remaining As Decimal
        <JsonProperty(PropertyName:="amount")>
        Public Property Amount As Decimal
        <JsonProperty(PropertyName:="caption", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Caption As String
    End Class
End Namespace
