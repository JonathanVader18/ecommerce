
Imports System.Data

Partial Class cambiar_passb2b
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtNuevoPass.Text = "" Then
            objdatos.Mensaje("Ingrese su nueva contraseña", Me.Page)
            Exit Sub
        End If

        If txtNuevoPass.Text <> txtConfirmaNuevoPass.Text Then
            objdatos.Mensaje("No coinciden las contraseñas", Me.Page)
            Exit Sub
        End If

        ''Si todo bien, actualizamos el pass

        ssql = "UPDATE OCRD SET [Password]=" & "'" & txtNuevoPass.Text & "' WHERE cardCode=" & "'" & Session("Cliente") & "'"
        objdatos.fnEjecutarInsertSAP(ssql)

        objdatos.Mensaje("Se ha realizado el cambio de la contraseña!", Me.Page)
        txtPassActual.Text = ""
        txtNuevoPass.Text = ""
        txtConfirmaNuevoPass.Text = ""

        btnGuardar.Enabled = False



    End Sub
End Class
