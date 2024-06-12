Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Openpay.Entities
Imports Openpay.Entities.Request

Namespace Openpay
    Public Class ChargeService
        Inherits OpenpayResourceService(Of ChargeRequest, Charge)

        Public Sub New(ByVal api_key As String, ByVal merchant_id As String, ByVal Optional production As Boolean = False)
            MyBase.New(api_key, merchant_id, production)
            ResourceName = "charges"
        End Sub

        Friend Sub New(ByVal opHttpClient As OpenpayHttpClient)
            MyBase.New(opHttpClient)
            ResourceName = "charges"
        End Sub

        Public Function Refund(ByVal charge_id As String, ByVal description As String) As Charge
            Return Me.Refund(Nothing, charge_id, description)
        End Function

        Public Function Refund(ByVal charge_id As String, ByVal description As String, ByVal amount As Decimal?) As Charge
            Return Me.Refund(Nothing, charge_id, description, amount)
        End Function

        Public Function Refund(ByVal customer_id As String, ByVal charge_id As String, ByVal description As String) As Charge
            Return Me.Refund(customer_id, charge_id, description, Nothing)
        End Function

        Public Function Refund(ByVal customer_id As String, ByVal charge_id As String, ByVal description As String, ByVal amount As Decimal?) As Charge
            If charge_id Is Nothing Then Throw New ArgumentNullException("charge_id cannot be null")
            Dim ep As String = GetEndPoint(customer_id, charge_id) & "/refund"
            Dim request As RefundRequest = New RefundRequest()
            request.Description = description
            If amount IsNot Nothing Then request.Amount = amount
            Return Me.httpClient.Post(Of Charge)(ep, request)
        End Function

        Public Function Capture(ByVal charge_id As String, ByVal amount As Decimal?) As Charge
            Return Me.Capture(Nothing, charge_id, amount)
        End Function

        Public Function Capture(ByVal customer_id As String, ByVal charge_id As String, ByVal amount As Decimal?) As Charge
            If charge_id Is Nothing Then Throw New ArgumentNullException("charge_id cannot be null")
            Dim ep As String = GetEndPoint(customer_id, charge_id) & "/capture"
            Dim request As CaptureRequest = New CaptureRequest()
            request.Amount = amount
            Return Me.httpClient.Post(Of Charge)(ep, request)
        End Function

        Public Overloads Function Create(ByVal charge_request As ChargeRequest) As Charge
            Return MyBase.Create(Nothing, charge_request)
        End Function

        Public Overloads Function Create(ByVal customer_id As String, ByVal charge_request As ChargeRequest) As Charge
            Return MyBase.Create(customer_id, charge_request)
        End Function

        Public Function CancelByMerchant(ByVal merchant_id As String, ByVal charge_id As String, ByVal charge_request As ChargeRequest) As Charge
            Return MyBase.CancelByMerchant(merchant_id, charge_id, charge_request)
        End Function

        Public Overloads Function [Get](ByVal customer_id As String, ByVal charge_id As String) As Charge
            Return MyBase.[Get](customer_id, charge_id)
        End Function

        Public Overloads Function [Get](ByVal charge_id As String) As Charge
            Return MyBase.[Get](Nothing, charge_id)
        End Function

        Public Overloads Function List(ByVal customer_id As String, ByVal Optional filters As SearchParams = Nothing) As List(Of Charge)
            Return MyBase.List(customer_id, filters)
        End Function

        Public Overloads Function List(ByVal Optional filters As SearchParams = Nothing) As List(Of Charge)
            Return MyBase.List(Nothing, filters)
        End Function
    End Class
End Namespace
