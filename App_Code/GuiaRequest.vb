Imports Microsoft.VisualBasic

Public Class GuiaRequest
    Inherits CotizacionRequest
    Public Property descripcion As String
    Public Property paqueteria As String
    Public Property guia As String
    Public Property servicio As Servicio
End Class
Public Class Servicio
    Public Property code As String
    Public Property description As String
End Class
