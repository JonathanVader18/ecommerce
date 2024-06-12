Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities.Request
    Public Class SearchParams
        Public Sub New()
            Offset = 0
            Limit = 10
        End Sub

        Public Property Offset As Integer
        Public Property Limit As Integer
        Public Property Amount As Decimal
        Public Property AmountGte As Decimal
        Public Property AmountLte As Decimal
        Public Property Creation As DateTime
        Public Property CreationGte As DateTime
        Public Property CreationLte As DateTime
        Public Property OrderId As String
        Public Property Status As TransactionStatus?

        Public Sub Between(ByVal start As DateTime, ByVal [end] As DateTime)
            CreationGte = start
            CreationLte = [end]
        End Sub
    End Class
End Namespace

