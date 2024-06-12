
Imports System.Data
Imports System.Security.Cryptography

Partial Class cambiar_contraseñaVend
    Inherits System.Web.UI.Page

    Public objdatos As New Cls_Funciones
    Public ssql As String = ""

    Private Sub cambiar_contraseñaVend_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Request.QueryString.Count > 0 Then
            Dim sCorreo As String = ""
            sCorreo = Desencriptar(Request.QueryString("usr"))
            ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre,cvPass,cvMail from config.Usuarios where cvMail=" & "'" & sCorreo & "'"
            Dim dtAcceso As New DataTable
            dtAcceso = objdatos.fnEjecutarConsulta(ssql)
            If dtAcceso.Rows.Count = 0 Then
                objdatos.Mensaje("Cuenta de correo no registrada", Me.Page)
                txtNuevoPass.Enabled = False
                txtConfirmaNuevoPass.Enabled = False
                btnGuardar.Enabled = False
                Exit Sub
            Else
                ''Si lo encontramos en la base
                Session("CorreoRecupera") = sCorreo
                Session("NombreRecupera") = dtAcceso.Rows(0)("Nombre")

            End If
        End If
    End Sub

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

        ssql = "UPDATE config.usuarios SET cvPass=" & "'" & txtNuevoPass.Text & "' WHERE cvMail=" & "'" & Session("CorreoRecupera") & "'"
        objdatos.fnEjecutarInsert(ssql)

        objdatos.Mensaje("Se ha realizado el cambio de la contraseña!", Me.Page)
        txtPassActual.Text = ""
        txtNuevoPass.Text = ""
        txtConfirmaNuevoPass.Text = ""
        btnRecuperar.Visible = True
        btnGuardar.Enabled = False


        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objdatos.fnEjecutarConsulta(ssql)


        Dim Mensaje As String = ""
        If dtcliente.Rows.Count > 0 Then

            Mensaje = "Hola! " & Session("NombreRecupera") & vbCrLf & dtcliente.Rows(0)(0) & " te saluda y te informa que se ha podido reestablecer tu contraseña."


            objdatos.fnEnviarCorreo(Session("CorreoRecupera"), Mensaje, dtcliente.Rows(0)(0) & "- Se ha reestablecido tu contraseña")

        End If

    End Sub

    Private Sub btnRecuperar_Click(sender As Object, e As EventArgs) Handles btnRecuperar.Click
        'ssql = "SELECT cvPass,cvMail from config.Usuarios where cvUsuario=" & "'" & Session("UserTienda") & "'"
        'Dim dtAcceso As New DataTable
        'dtAcceso = objdatos.fnEjecutarConsulta(ssql)
        'If dtAcceso.Rows.Count = 0 Then
        '    objdatos.fnEnviarCorreo(dtAcceso.Rows(0)("cvMail"), "Su contraseña del portal de vendedores es: " & dtAcceso.Rows(0)("cvPass"), "Envío de contraseña")
        '    objdatos.Mensaje("Su contraseña ha sido enviada al correo con el que lo registraron en el portal", Me.Page)
        'End If
        Response.Redirect("login.aspx")
    End Sub

    Function DESENCRIPTAR(ByVal string_desencriptar As String) As String
        Dim R As Integer
        Dim i As Integer
        R = Len(Trim(string_desencriptar))
        For i = 1 To R
            Mid(string_desencriptar, i, 1) = Chr(Asc(Mid(string_desencriptar, i, 1)) + 1)
        Next i
        Return string_desencriptar
    End Function
End Class
