Imports Openpay.Entities
Imports Openpay.Entities.Request
Imports Openpay.Utils
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay
    Public Class CardService
        Inherits OpenpayResourceService(Of Card, Card)

        Public Sub New(ByVal api_key As String, ByVal merchant_id As String, ByVal Optional production As Boolean = False)
            MyBase.New(api_key, merchant_id, production)
            ResourceName = "cards"
        End Sub

        Friend Sub New(ByVal opHttpClient As OpenpayHttpClient)
            MyBase.New(opHttpClient)
            ResourceName = "cards"
        End Sub

        'Public Function Create(ByVal card As Card) As Card
        '    Return MyBase.Create(Nothing, card)
        'End Function

        'Public Overloads Function Create(ByVal customer_id As String, ByVal card As Card) As Card
        '    Return MyBase.Create(customer_id, card)
        'End Function

        'Public Overloads Sub Delete(ByVal customer_id As String, ByVal card_id As String)
        '    MyBase.Delete(customer_id, card_id)
        'End Sub

        'Public Sub Delete(ByVal card_id As String)
        '    MyBase.Delete(Nothing, card_id)
        'End Sub

        'Public Overloads Function [Get](ByVal customer_id As String, ByVal card_id As String) As Card
        '    Return MyBase.[Get](customer_id, card_id)
        'End Function

        'Public Function [Get](ByVal card_id As String) As Card
        '    Return MyBase.[Get](Nothing, card_id)
        'End Function

        'Public Overloads Function List(ByVal customer_id As String, ByVal Optional filters As SearchParams = Nothing) As List(Of Card)
        '    Return MyBase.List(customer_id, filters)
        'End Function

        'Public Function List(ByVal Optional filters As SearchParams = Nothing) As List(Of Card)
        '    Return MyBase.List(Nothing, filters)
        'End Function

        'Public Function Points(ByVal card_id As String) As PointsBalance
        '    Dim ep As String = GetEndPoint(Nothing, card_id) & "/points"
        '    Return Me.httpClient.[Get](Of PointsBalance)(ep)
        'End Function

        'Public Function Points(ByVal customer_id As String, ByVal card_id As String) As PointsBalance
        '    Dim ep As String = GetEndPoint(customer_id, card_id) & "/points"
        '    Return Me.httpClient.[Get](Of PointsBalance)(ep)
        'End Function
    End Class
End Namespace

