Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class Customer
        Inherits OpenpayResourceObject

        <JsonProperty(PropertyName:="external_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property ExternalId As String
        <JsonProperty(PropertyName:="name")>
        Public Property Name As String
        <JsonProperty(PropertyName:="email")>
        Public Property Email As String
        <JsonProperty(PropertyName:="last_name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property LastName As String
        <JsonProperty(PropertyName:="phone_number", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property PhoneNumber As String
        <JsonProperty(PropertyName:="address", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Address As Address
        <JsonProperty(PropertyName:="status", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Status As String
        <JsonProperty(PropertyName:="clabe", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CLABE As String
        <JsonProperty(PropertyName:="balance", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Balance As Decimal
        <JsonProperty(PropertyName:="creation_date", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CreationDate As DateTime?
        <JsonProperty(PropertyName:="requires_account", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property RequiresAccount As Boolean
        <JsonProperty(PropertyName:="store", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Store As Store
    End Class
End Namespace

