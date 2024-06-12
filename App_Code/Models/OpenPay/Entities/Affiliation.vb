Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class Affiliation
        <JsonProperty(PropertyName:="number", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Number As String
        <JsonProperty(PropertyName:="name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Name As String
        <JsonProperty(PropertyName:="merchant_name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property MerchantName As String
    End Class
End Namespace

