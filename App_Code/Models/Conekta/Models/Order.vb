Imports Newtonsoft.Json.Linq
Imports System.Collections.Generic

Namespace conekta
    Public Class Detail
        Public Property debug_message As String
        Public Property message As String
        Public Property code As String
    End Class

    Public Class [Error]
        Public Property details As IList(Of Detail)
    End Class

    Public Class Orden
        Public Property currency As String
        Public Property payment_status As String
        Public Property livemode As String
        Public Property amount As Double
        Public Property amount_refunded As Double

        Public Property customer_info As Comprador
        Public Property line_items As List(Of Producto)
        Public Property charges As List(Of Cargo)
    End Class
End Namespace