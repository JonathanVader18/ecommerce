Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class Card
        Inherits OpenpayResourceObject

        <JsonProperty(PropertyName:="token_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property TokenId As String
        <JsonProperty(PropertyName:="creation_date", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property CreationDate As DateTime?
        <JsonProperty(PropertyName:="bank_name", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property BankName As String
        <JsonProperty(PropertyName:="allows_payouts", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property AllowsPayouts As Boolean
        <JsonProperty(PropertyName:="holder_name")>
        Public Property HolderName As String
        <JsonProperty(PropertyName:="expiration_month")>
        Public Property ExpirationMonth As String
        <JsonProperty(PropertyName:="expiration_year")>
        Public Property ExpirationYear As String
        <JsonProperty(PropertyName:="address", NullValueHandling:=NullValueHandling.Ignore)>
        Public Address As Address
        <JsonProperty(PropertyName:="card_number")>
        Public Property CardNumber As String
        <JsonProperty(PropertyName:="brand", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Brand As String
        <JsonProperty(PropertyName:="allows_charges", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property AllowsCharges As Boolean
        <JsonProperty(PropertyName:="bank_code", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property BankCode As String
        <JsonProperty(PropertyName:="type", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Type As String
        <JsonProperty(PropertyName:="cvv2", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Cvv2 As String
        <JsonProperty(PropertyName:="device_session_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property DeviceSessionId As String
        <JsonProperty(PropertyName:="points_card", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property PointsCard As Boolean
        <JsonProperty(PropertyName:="points_type", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property PointsType As String
        <JsonProperty(PropertyName:="affiliation")>
        Public Property Affiliation As Affiliation
        <JsonProperty(PropertyName:="payment_options", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property PaymentOptions As String
    End Class
End Namespace

