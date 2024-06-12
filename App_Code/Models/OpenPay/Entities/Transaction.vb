Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class Transaction
        Inherits OpenpayResourceObject

        <JsonProperty(PropertyName:="creation_date")>
        Public Property CreationDate As DateTime?
        <JsonProperty(PropertyName:="amount")>
        Public Property Amount As Decimal
        <JsonProperty(PropertyName:="status")>
        Public Property Status As String
        <JsonProperty(PropertyName:="description")>
        Public Property Description As String
        <JsonProperty(PropertyName:="transaction_type")>
        Public Property TransactionType As String
        <JsonProperty(PropertyName:="operation_type")>
        Public Property OperationType As String
        <JsonProperty(PropertyName:="method")>
        Public Property Method As String
        <JsonProperty(PropertyName:="error_message")>
        Public Property ErrorMessage As String
        <JsonProperty(PropertyName:="card")>
        Public Property Card As Card
        <JsonProperty(PropertyName:="bank_account")>
        Public Property BankAccount As BankAccount
        <JsonProperty(PropertyName:="authorization")>
        Public Property Authorization As String
        <JsonProperty(PropertyName:="order_id")>
        Public Property OrderId As String
        <JsonProperty(PropertyName:="customer_id")>
        Public Property CustomerId As String
        <JsonProperty(PropertyName:="conciliated")>
        Public Property Conciliated As Boolean
        <JsonProperty(PropertyName:="due_date")>
        Public Property DueDate As DateTime?
    End Class

    Public Class Refund
        Inherits Transaction
    End Class

    Public Class Charge
        Inherits Transaction

        <JsonProperty(PropertyName:="refund")>
        Public Property Refund As Refund
        <JsonProperty(PropertyName:="payment_method")>
        Public Property PaymentMethod As PaymentMethod
        <JsonProperty(PropertyName:="card_points")>
        Public Property CardPoints As CardPoints
        <JsonProperty(PropertyName:="exchange_rate")>
        Public Property ExchangeRate As ExchangeRate
        <JsonProperty(PropertyName:="metadata")>
        Public Property Metadata As Dictionary(Of String, String)
    End Class

    Public Class Payout
        Inherits Transaction
    End Class

    Public Class Fee
        Inherits Transaction
    End Class

    Public Class Transfer
        Inherits Transaction
    End Class
End Namespace

