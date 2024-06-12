Imports Newtonsoft.Json
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Openpay.Entities
    Public Class ExchangeRate
        <JsonProperty(PropertyName:="from")>
        Public Property fromCurrency As String
        <JsonProperty(PropertyName:="to")>
        Public Property toCurrency As String
        <JsonProperty(PropertyName:="date")>
        Public Property [Date] As DateTime?
        <JsonProperty(PropertyName:="value")>
        Public Property value As Decimal
    End Class
End Namespace

