Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json

Namespace Openpay.Entities.Request
    <JsonObject(MemberSerialization.OptIn)>
    Public Class ChargeRequest
        Inherits JsonObject

        Public Sub New()
            Capture = True
            Confirm = "true"
        End Sub

        <JsonProperty(PropertyName:="method")>
        Public Property Method As String
        <JsonProperty(PropertyName:="source_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property SourceId As String
        <JsonProperty(PropertyName:="card", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Card As Card
        <JsonProperty(PropertyName:="amount")>
        Public Property Amount As Decimal
        <JsonProperty(PropertyName:="description")>
        Public Property Description As String
        <JsonProperty(PropertyName:="order_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property OrderId As String
        <JsonProperty(PropertyName:="capture", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Capture As Boolean
        <JsonProperty(PropertyName:="device_session_id", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property DeviceSessionId As String
        <JsonProperty(PropertyName:="currency", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Currency As String
        <JsonProperty(PropertyName:="due_date", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property DueDate As DateTime?
        <JsonProperty(PropertyName:="metadata", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Metadata As Dictionary(Of String, String)
        <JsonProperty(PropertyName:="customer", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Customer As Customer
        <JsonProperty(PropertyName:="use_card_points", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property UseCardPoints As String
        <JsonProperty(PropertyName:="payment_plan", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property DeferralPayments As DeferralPayments
        <JsonProperty(PropertyName:="confirm", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Confirm As String
        <JsonProperty(PropertyName:="send_email", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property SendEmail As Boolean
        <JsonProperty(PropertyName:="use_3d_secure", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Use3DSecure As Boolean
        <JsonProperty(PropertyName:="redirect_url", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property RedirectUrl As String
        <JsonProperty(PropertyName:="affiliation", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Affiliation As Affiliation
        <JsonProperty(PropertyName:="is_phone_order", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property IsPhoneOrder As Boolean
        <JsonProperty(PropertyName:="payment_options", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property PaymentOptions As String
        <JsonProperty(PropertyName:="cvv2", NullValueHandling:=NullValueHandling.Ignore)>
        Public Property Cvv2 As String
    End Class
End Namespace

