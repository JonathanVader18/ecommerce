Imports Openpay.Entities
Imports Openpay.Entities.Request
Imports Openpay.Utils
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay
    Public MustInherit Class OpenpayResourceService(Of T As JsonObject, R As OpenpayResourceObject)
        Protected httpClient As OpenpayHttpClient
        Private Shared ReadOnly filter_date_format As String = "yyyy-MM-dd"
        Private Shared ReadOnly filter_amount_format As String = "0.00"

        Public Sub New(ByVal api_key As String, ByVal merchant_id As String, ByVal Optional production As Boolean = False)
            Me.httpClient = New OpenpayHttpClient(api_key, merchant_id, production)
        End Sub

        Friend Sub New(ByVal opHttpClient As OpenpayHttpClient)
            Me.httpClient = opHttpClient
        End Sub

        Protected Property ResourceName As String

        Protected Function GetEndPoint(ByVal customer_id As String, ByVal Optional resource_id As String = Nothing) As String
            Dim ep As String = "/" & ResourceName.ToLower()

            If customer_id IsNot Nothing Then
                ep = String.Format("/customers/{0}" & ep, customer_id)
            End If

            If resource_id IsNot Nothing Then
                ep = ep & "/" & resource_id
            End If

            Return ep
        End Function

        Protected Function GetEndPointMerchant(ByVal merchant_id As String, ByVal Optional resource_id As String = Nothing) As String
            Dim ep As String = "/" & ResourceName.ToLower()

            If resource_id IsNot Nothing Then
                ep += "/" & resource_id
            End If

            Return ep
        End Function

        Protected Overridable Function Create(ByVal customer_id As String, ByVal obj As T) As R
            If obj Is Nothing Then Throw New ArgumentNullException("The object to create is null")
            Dim ep As String = GetEndPoint(customer_id)
            Return Me.httpClient.Post(Of R)(ep, obj)
        End Function

        Protected Function Update(ByVal customer_id As String, ByVal obj As R) As R
            If String.IsNullOrEmpty(obj.Id) Then Throw New ArgumentNullException("resource_id")
            If obj Is Nothing Then Throw New ArgumentNullException("Object is null")
            Dim ep As String = GetEndPoint(customer_id, obj.Id)
            Return Me.httpClient.Put(Of R)(ep, obj)
        End Function

        Protected Overridable Function Cancel(ByVal customer_id As String, ByVal charge_id As String, ByVal obj As T) As R
            If obj Is Nothing Then Throw New ArgumentNullException("The object to create is null")
            Dim ep As String = GetEndPoint(customer_id, charge_id) & "/cancel"
            Return Me.httpClient.Cancel(Of R)(ep, obj)
        End Function

        Protected Overridable Function CancelByMerchant(ByVal merchant_id As String, ByVal charge_id As String, ByVal obj As T) As R
            If obj Is Nothing Then Throw New ArgumentNullException("The object to create is null")
            Dim ep As String = GetEndPointMerchant(merchant_id, charge_id) & "/cancel"
            Return Me.httpClient.Cancel(Of R)(ep, obj)
        End Function

        Protected Overridable Sub Delete(ByVal customer_id As String, ByVal resource_id As String)
            If String.IsNullOrEmpty(resource_id) Then Throw New ArgumentNullException("The id of the object cannot be null")
            Dim ep As String = GetEndPoint(customer_id, resource_id)
            Me.httpClient.Delete(ep)
        End Sub

        Protected Overridable Function [Get](ByVal customer_id As String, ByVal resource_id As String) As R
            Dim ep As String = GetEndPoint(customer_id, resource_id)
            Return Me.httpClient.[Get](Of R)(ep)
        End Function

        Protected Function List(ByVal customer_id As String, ByVal searchParams As SearchParams) As List(Of R)
            Dim url As String = GetEndPoint(customer_id)
            url = url & BuildParams(searchParams)
            Return Me.httpClient.[Get](Of List(Of R))(url)
        End Function

        Protected Function BuildParams(ByVal searchParams As SearchParams) As String
            Dim url_params As String = String.Empty

            If searchParams IsNot Nothing Then
                If searchParams.Offset < 0 Then Throw New ArgumentOutOfRangeException("offset")
                If searchParams.Limit < 1 OrElse searchParams.Limit > 100 Then Throw New ArgumentOutOfRangeException("limit")
                url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "limit", searchParams.Limit.ToString())
                url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "offset", searchParams.Offset.ToString())
                If searchParams.OrderId IsNot Nothing Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "order_id", searchParams.OrderId)
                If searchParams.Status IsNot Nothing Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "status", searchParams.Status.ToString())
                If searchParams.Creation <> DateTime.MinValue Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "creation", searchParams.Creation.ToString(filter_date_format))
                If searchParams.CreationGte <> DateTime.MinValue Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "creation[gte]", searchParams.CreationGte.ToString(filter_date_format))
                If searchParams.CreationLte <> DateTime.MinValue Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "creation[lte]", searchParams.CreationLte.ToString(filter_date_format))
                If searchParams.Amount > 0 Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "amount", searchParams.Amount.ToString(filter_amount_format))
                If searchParams.AmountGte > 0 Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "amount[gte]", searchParams.AmountGte.ToString(filter_amount_format))
                If searchParams.AmountLte > 0 Then url_params = ParameterBuilder.ApplyParameterToUrl(url_params, "amount[lte]", searchParams.AmountLte.ToString(filter_amount_format))
            End If

            Return url_params
        End Function
    End Class
End Namespace

