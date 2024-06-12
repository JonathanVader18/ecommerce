Imports System.Data

Partial Class editar_contraseña
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub editar_contraseña_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
    Public Sub fnMenuPreferencias()
        Dim ssql As String
        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Preferencias' "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""

        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <li><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuPref.Controls.Clear()
        pnlMenuPref.Controls.Add(literal)

    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtPassActual.Text = "" Then
            objDatos.Mensaje("Ingrese su contraseña actual", Me.Page)
            Exit Sub
        End If

        If txtNuevoPass.Text = "" Then
            objDatos.Mensaje("Ingrese su nueva contraseña", Me.Page)
            Exit Sub
        End If

        If txtNuevoPass.Text <> txtConfirmaNuevoPass.Text Then
            objDatos.Mensaje("No coinciden las contraseñas", Me.Page)
            Exit Sub
        End If

        ''Si todo bien, actualizamos el pass

        ssql = "UPDATE config.usuarios SET cvPass=" & "'" & txtNuevoPass.Text & "' WHERE cvUsuario=" & "'" & Session("UserB2C") & "'"
        objDatos.fnEjecutarInsert(ssql)

        objDatos.Mensaje("Se ha realizado el cambio de la contraseña!", Me.Page)
        txtPassActual.Text = ""
        txtNuevoPass.Text = ""
        txtConfirmaNuevoPass.Text = ""
    End Sub
End Class
