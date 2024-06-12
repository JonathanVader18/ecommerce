Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    <JsonObject(MemberSerialization.OptIn)>
    Public Class Address
        <JsonProperty(PropertyName:="postal_code")>
        Public Property PostalCode As String
        <JsonProperty(PropertyName:="line1")>
        Public Property Line1 As String
        <JsonProperty(PropertyName:="line2", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Line2 As String
        <JsonProperty(PropertyName:="line3", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Line3 As String
        <JsonProperty(PropertyName:="city")>
        Public Property City As String
        <JsonProperty(PropertyName:="state")>
        Public Property State As String
        <JsonProperty(PropertyName:="country_code")>
        Public Property CountryCode As String
    End Class
End Namespace

