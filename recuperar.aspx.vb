Imports System.Data
Partial Class recuperar
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub recuperar_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click

        If txtCorreo.Text = "" Then
            objDatos.Mensaje("Ingrese la dirección de correo con la que registró su cuenta", Me.Page)
            Exit Sub
        End If
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)


        Dim Mensaje As String = ""
        If dtcliente.Rows.Count > 0 Then

            ssql = "SELECT ISNULL(cvNombreCompleto,'') as Nombre,cvPass,ISNULL(cvMail,'') as Mail,cvUsuario FROM config.Usuarios WHERE cvUsuario=" & "'" & txtCorreo.Text & "'  and cvTipoAcceso='B2C' "
            Dim dtLogin As New DataTable
            dtLogin = objDatos.fnEjecutarConsulta(ssql)

            If dtLogin.Rows.Count > 0 Then
                Mensaje = "Hola! " & dtLogin.Rows(0)(0) & vbCrLf & dtcliente.Rows(0)(0) & " te saluda y te reenvia tu contraseña para que puedas continuar comprando:" & vbCrLf & dtLogin.Rows(0)("cvPass")

                Dim sDestinatario As String = ""
                If dtLogin.Rows(0)("Mail") = "" Then
                    sDestinatario = dtLogin.Rows(0)("cvUsuario")
                Else
                    sDestinatario = dtLogin.Rows(0)("Mail")
                End If

                Dim text As String = MensajeHTML(Server.MapPath("~") & "\recuperar_A.html")

                text = text.Replace("{password}", dtLogin.Rows(0)("cvPass"))
                objDatos.fnEnviarCorreo(sDestinatario, text, dtcliente.Rows(0)(0) & "- Recupera tu contraseña")
            Else
                objDatos.Mensaje("Usuario no encontrado!", Me.Page)
                Exit Sub
            End If

        End If

        Response.Redirect("aviso.aspx")
    End Sub
    Protected Function MensajeHTML(ArchivoHTML As [String]) As String
        Dim Cuerpo As [String] = ""
        Try
            Dim File As New System.IO.StreamReader(ArchivoHTML)

            Dim Line As [String]
            Dim text As String = System.IO.File.ReadAllText(ArchivoHTML)

            Cuerpo = text

            File.Close()
        Catch ex As Exception
            objDatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function
End Class
