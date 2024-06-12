Imports Microsoft.VisualBasic

Public Class CotizacionResponse
    Public Property cotizaciones As List(Of Cotizacion_Response)
    Public Property mensaje As String
End Class
Public Class Cotizacion_Response
    Public Property codigo As String
    Public Property monto As Decimal
    Public Property moneda As String
    Public Property descripcion As String
End Class
