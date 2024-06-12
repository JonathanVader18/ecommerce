Imports System.Data
Imports Telerik.Web.UI

Partial Class configParamGenerales
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub
    Protected Sub chkTodas_CheckedChanged(sender As Object, e As EventArgs) Handles chkTodas.CheckedChanged
        If chkTodas.Checked = True Then
            pnlAlmacenes.Visible = False
        Else
            pnlAlmacenes.Visible = True
        End If
    End Sub
End Class
