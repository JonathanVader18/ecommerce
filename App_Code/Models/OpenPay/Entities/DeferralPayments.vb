Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports Newtonsoft.Json

Namespace Openpay.Entities
    Public Class DeferralPayments
        Inherits JsonObject

        Public Sub New(ByVal Payments As Integer)
            Me.Payments = Payments
        End Sub

        <JsonProperty(PropertyName:="payments")>
        Public Property Payments As Integer
    End Class
End Namespace

