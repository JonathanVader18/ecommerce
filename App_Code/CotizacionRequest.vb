Imports Microsoft.VisualBasic

Public Class CotizacionRequest
    Public Property origen As Origen
    Public Property destino As Destino
    Public Property guia As String
    Public Property paquetes As List(Of Paquete)
End Class
Public Class Address
    Public Property addressLine As String
    Public Property city As String
    Public Property postalCode As String
    Public Property countryCode As String
    Public Property stateProvinceCode As String
    Public Property numInt As String
    Public Property numExt As String
    Public Property estado As String
    Public Property colonia As String
End Class

Public Class Phone
    Public Property number As String
End Class

Public Class Origen
    Public Property name As String
    Public Property attentionName As String
    Public Property address As Address
    Public Property phone As Phone
End Class

Public Class Destino
    Public Property name As String
    Public Property attentionName As String
    Public Property address As Address
    Public Property phone As Phone
End Class

Public Class PackagingType
    Public Property code As String
    Public Property description As String
End Class

Public Class UnitOfMeasurement
    Public Property code As String
End Class

Public Class Dimensions
    Public Property unitOfMeasurement As UnitOfMeasurement
    Public Property length As String
    Public Property width As String
    Public Property height As String
End Class

Public Class PackageWeight
    Public Property unitOfMeasurement As UnitOfMeasurement
    Public Property weight As String
End Class

Public Class Paquete
    Public Property description As String
    Public Property packagingType As PackagingType
    Public Property dimensions As Dimensions
    Public Property packageWeight As PackageWeight
End Class
