Imports Microsoft.VisualBasic

Public Class ResultadoPaquete
    Public Property numero As String
    Public Property archivo As String
    Public Property extension As String
End Class

Public Class GuiaResponse
    Public Property totalCargos As Decimal
    Public Property moneda As String
    Public Property pesoTotal As String
    Public Property numeroGuia As String
    Public Property resultadoPaquetes As List(Of ResultadoPaquete)
    Public Property mensaje As String
    Public Property [error] As Boolean
End Class