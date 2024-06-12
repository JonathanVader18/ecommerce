Imports Microsoft.VisualBasic

Namespace conekta
    Public Class CustomerInfo
        Public Property email As String
        Public Property phone As String
        Public Property name As String
    End Class

    Public Class Datum
        Public Property name As String
        Public Property unit_price As Integer
        Public Property quantity As Integer
        Public Property id As String
        Public Property parent_id As String
    End Class

    Public Class LineItems
        Public Property has_more As Boolean
        Public Property total As Integer
        Public Property data As IList(Of Datum)
    End Class

    Public Class PaymentMethod
        Public Property name As String
        Public Property exp_month As String
        Public Property exp_year As String
        Public Property auth_code As String
        Public Property type As String
        Public Property last4 As String
        Public Property brand As String
        Public Property issuer As String
        Public Property account_type As String
        Public Property country As String
    End Class

    Public Class Charges
        Public Property has_more As Boolean
        Public Property total As Integer
        Public Property data As IList(Of Datum)
    End Class

    Public Class OrderResponse
        Public Property livemode As Boolean
        Public Property amount As Integer
        Public Property currency As String
        Public Property payment_status As String
        Public Property amount_refunded As Integer
        Public Property customer_info As CustomerInfo
        Public Property id As String
        Public Property created_at As Integer
        Public Property updated_at As Integer
        Public Property line_items As LineItems
        Public Property charges As Charges
    End Class
End Namespace

