Imports System.Collections.Generic
Imports Microsoft.VisualBasic

Public Class Cls_Pedido


    Public Property CveCliente() As String
        Get
            Return m_CveCliente
        End Get
        Set(value As String)
            m_CveCliente = value
        End Set
    End Property
    Private m_CveCliente As String
    Public Property RazonSocial() As String
        Get
            Return m_RazonSocial
        End Get
        Set(value As String)
            m_RazonSocial = value
        End Set
    End Property
    Private m_RazonSocial As String

    Public Property EmpleadoVentas() As String
        Get
            Return m_EmpleadoVentas
        End Get
        Set(value As String)
            m_EmpleadoVentas = value
        End Set
    End Property
    Private m_EmpleadoVentas As String

    Public Property Usuario() As String
        Get
            Return m_Usuario
        End Get
        Set(value As String)
            m_Usuario = value
        End Set
    End Property
    Private m_Usuario As String

    Public Property NumCarrito() As String
        Get
            Return m_NumCarrito
        End Get
        Set(value As String)
            m_NumCarrito = value
        End Set
    End Property
    Private m_NumCarrito As String

    Public Property DireccionEntrega() As List(Of DireccionesEntregas)
        Get
            Return m_Entregas
        End Get
        Set(value As List(Of DireccionesEntregas))
            m_Entregas = value
        End Set
    End Property
    Private m_Entregas As List(Of DireccionesEntregas)

    Public Class Partidas
        Public Property ItemCode() As String
            Get
                Return m_ItemCode
            End Get
            Set(value As String)
                m_ItemCode = value
            End Set
        End Property
        Private m_ItemCode As String
        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(value As String)
                m_ItemName = value
            End Set
        End Property
        Private m_ItemName As String
        Public Property Precio() As Double
            Get
                Return m_Precio
            End Get
            Set(value As Double)
                m_Precio = value
            End Set
        End Property
        Private m_Precio As Double

        Public Property Descuento() As Double
            Get
                Return m_Descuento
            End Get
            Set(value As Double)
                m_Descuento = value
            End Set
        End Property
        Private m_Descuento As Double
        Public Property Cantidad() As Double
            Get
                Return m_Cantidad
            End Get
            Set(value As Double)
                m_Cantidad = value
            End Set
        End Property
        Private m_Cantidad As Double
        Public Property TotalLinea() As Double
            Get
                Return m_TotalLinea
            End Get
            Set(value As Double)
                m_TotalLinea = value
            End Set
        End Property
        Private m_TotalLinea As Double

        Public Property Moneda() As String
            Get
                Return m_Moneda
            End Get
            Set(value As String)
                m_Moneda = value
            End Set
        End Property
        Private m_Moneda As String

        Public Property MonedaGen() As String
            Get
                Return m_MonedaGen
            End Get
            Set(value As String)
                m_MonedaGen = value
            End Set
        End Property
        Private m_MonedaGen As String

        Public Property Impuesto() As String
            Get
                Return m_Impuesto
            End Get
            Set(value As String)
                m_Impuesto = value
            End Set
        End Property
        Private m_Impuesto As String

        Public Property Imagen() As String
            Get
                Return m_Imagen
            End Get
            Set(value As String)
                m_Imagen = value
            End Set
        End Property
        Private m_Imagen As String

        Public Property Generico() As String
            Get
                Return m_Generico
            End Get
            Set(value As String)
                m_Generico = value
            End Set
        End Property
        Private m_Generico As String


        Public Property Linea() As String
            Get
                Return m_Linea
            End Get
            Set(value As String)
                m_Linea = value
            End Set
        End Property
        Private m_Linea As String



        Public Property Mts2() As String
            Get
                Return m_Mts2
            End Get
            Set(value As String)
                m_Mts2 = value
            End Set
        End Property
        Private m_Mts2 As String

    End Class

    Public Class DireccionesEntregas
        Public Property Lugar() As String
            Get
                Return m_Lugar
            End Get
            Set(value As String)
                m_Lugar = value
            End Set
        End Property
        Private m_Lugar As String
        Public Property Calle() As String
            Get
                Return m_Calle
            End Get
            Set(value As String)
                m_Calle = value
            End Set
        End Property
        Private m_Calle As String

        Public Property Colonia() As String
            Get
                Return m_Colonia
            End Get
            Set(value As String)
                m_Colonia = value
            End Set
        End Property
        Private m_Colonia As String

        Public Property Municipio() As String
            Get
                Return m_Municipio
            End Get
            Set(value As String)
                m_Municipio = value
            End Set
        End Property
        Private m_Municipio As String

        Public Property Estado() As String
            Get
                Return m_Estado
            End Get
            Set(value As String)
                m_Estado = value
            End Set
        End Property
        Private m_Estado As String

        Public Property NumExt() As String
            Get
                Return m_NumExt
            End Get
            Set(value As String)
                m_NumExt = value
            End Set
        End Property
        Private m_NumExt As String

        Public Property NumInt() As String
            Get
                Return m_NumInt
            End Get
            Set(value As String)
                m_NumInt = value
            End Set
        End Property
        Private m_NumInt As String
        Public Property CP() As String
            Get
                Return m_CP
            End Get
            Set(value As String)
                m_CP = value
            End Set
        End Property
        Private m_CP As String
        Public Property TelFijo() As String
            Get
                Return m_TelFijo
            End Get
            Set(value As String)
                m_TelFijo = value
            End Set
        End Property
        Private m_TelFijo As String

        Public Property TelCelular() As String
            Get
                Return m_TelCelular
            End Get
            Set(value As String)
                m_TelCelular = value
            End Set
        End Property
        Private m_TelCelular As String

        Public Property AtencionA() As String
            Get
                Return m_AtencionA
            End Get
            Set(value As String)
                m_AtencionA = value
            End Set
        End Property
        Private m_AtencionA As String

    End Class
    Public Class DireccionFacturacion
        Public Property Lugar() As String
            Get
                Return m_Lugar
            End Get
            Set(value As String)
                m_Lugar = value
            End Set
        End Property
        Private m_Lugar As String
        Public Property Calle() As String
            Get
                Return m_Calle
            End Get
            Set(value As String)
                m_Calle = value
            End Set
        End Property
        Private m_Calle As String

        Public Property Colonia() As String
            Get
                Return m_Colonia
            End Get
            Set(value As String)
                m_Colonia = value
            End Set
        End Property
        Private m_Colonia As String

        Public Property Municipio() As String
            Get
                Return m_Municipio
            End Get
            Set(value As String)
                m_Municipio = value
            End Set
        End Property
        Private m_Municipio As String

        Public Property Estado() As String
            Get
                Return m_Estado
            End Get
            Set(value As String)
                m_Estado = value
            End Set
        End Property
        Private m_Estado As String

        Public Property NumExt() As String
            Get
                Return m_NumExt
            End Get
            Set(value As String)
                m_NumExt = value
            End Set
        End Property
        Private m_NumExt As String

        Public Property NumInt() As String
            Get
                Return m_NumInt
            End Get
            Set(value As String)
                m_NumInt = value
            End Set
        End Property
        Private m_NumInt As String
        Public Property CP() As String
            Get
                Return m_CP
            End Get
            Set(value As String)
                m_CP = value
            End Set
        End Property
        Private m_CP As String

        Public Property RFC() As String
            Get
                Return m_RFC
            End Get
            Set(value As String)
                m_RFC = value
            End Set
        End Property
        Private m_RFC As String



    End Class
End Class
