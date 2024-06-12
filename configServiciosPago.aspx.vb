Imports System.Data
Imports Telerik.Web.UI
Partial Class configServiciosPago
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub
End Class
