Imports Openpay.Entities
Imports Openpay.Entities.Request
Imports Openpay.Utils
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay
    Public Class CustomerService
        Inherits OpenpayResourceService(Of Customer, Customer)

        Public Sub New(ByVal api_key As String, ByVal merchant_id As String, ByVal Optional production As Boolean = False)
            MyBase.New(api_key, merchant_id, production)
            ResourceName = "customers"
        End Sub

        Friend Sub New(ByVal opHttpClient As OpenpayHttpClient)
            MyBase.New(opHttpClient)
            ResourceName = "customers"
        End Sub

        Public Function Create(ByVal customer As Customer) As Customer
            Return MyBase.Create(Nothing, customer)
        End Function

        Public Function Update(ByVal customer As Customer) As Customer
            Return MyBase.Update(Nothing, customer)
        End Function

        Public Sub Delete(ByVal customer_id As String)
            MyBase.Delete(Nothing, customer_id)
        End Sub

        Public Function [Get](ByVal customer_id As String) As Customer
            Return MyBase.[Get](Nothing, customer_id)
        End Function

        Public Function List(ByVal Optional filters As SearchParams = Nothing) As List(Of Customer)
            Return MyBase.List(Nothing, filters)
        End Function
    End Class
End Namespace

