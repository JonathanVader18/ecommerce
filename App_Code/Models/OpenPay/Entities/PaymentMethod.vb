Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class PaymentMethod
        <JsonProperty(PropertyName:="type")>
        Public Property Type As String
        <JsonProperty(PropertyName:="bank")>
        Public Property BankName As String
        <JsonProperty(PropertyName:="clabe")>
        Public Property CLABE As String
        <JsonProperty(PropertyName:="name")>
        Public Property Name As String
        <JsonProperty(PropertyName:="reference")>
        Public Property Reference As String
        <JsonProperty(PropertyName:="agreement")>
        Public Property Agreement As String
        <JsonProperty(PropertyName:="walmart_reference")>
        Public Property WalmartReference As String
        <JsonProperty(PropertyName:="barcode_url")>
        Public Property BarcodeURL As String
        <JsonProperty(PropertyName:="barcode_walmart_url")>
        Public Property BarcodeWalmartURL As String
        <JsonProperty(PropertyName:="payment_address")>
        Public Property PaymentAddress As String
        <JsonProperty(PropertyName:="payment_url_bip21")>
        Public Property PaymentUrlBip21 As String
        <JsonProperty(PropertyName:="amount_bitcoins")>
        Public Property AmountBitcoins As Decimal
        <JsonProperty(PropertyName:="exchange_rate")>
        Public Property ExchangeRate As ExchangeRate
        <JsonProperty(PropertyName:="url")>
        Public Property Url As String
        <JsonProperty(PropertyName:="ivr_key")>
        Public Property IvrKey As String
        <JsonProperty(PropertyName:="phone_number")>
        Public Property PhoneNumber As String
    End Class
End Namespace
