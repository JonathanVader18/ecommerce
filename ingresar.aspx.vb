
Imports System.Data

Partial Class ingresar
    Inherits System.Web.UI.Page
    Public objdatos As New Cls_Funciones
    Public ssql As String = ""
    Private Sub ingresar_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserB2C") = "" Then

        Else
            Response.Redirect("pagoindex.aspx")
        End If
    End Sub
    Protected Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''Hacemos el login
        If txtUser.Text = "" Then
            objdatos.Mensaje("Especifique un usuario", Me.Page)
            Exit Sub
        End If
        If txtPass.Text = "" Then
            objdatos.Mensaje("Especifique una contraseña", Me.Page)
            Exit Sub
        End If
        ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUser.Text & "' AND cvPass=" & "'" & txtPass.Text & "' and cvTipoAcceso='B2C' "
        Dim dtLogin As New DataTable
        dtLogin = objdatos.fnEjecutarConsulta(ssql)
        If dtLogin.Rows.Count > 0 Then
            Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
            Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
            Session("Cliente") = dtLogin.Rows(0)("cvCardCode")

            ''en base al cliente, obtenemos cual es su lista de precios
            ssql = objdatos.fnObtenerQuery("ListaPrecioscliente")
            ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
            Dim dtLista As New DataTable
            dtLista = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtLista.Rows.Count > 0 Then
                Session("ListaPrecios") = dtLista.Rows(0)(0)
            Else
                Session("ListaPrecios") = "1"
            End If

            Response.Redirect("pagoindex.aspx")


        Else
            objdatos.Mensaje("Acceso incorrecto", Me.Page)
            Exit Sub
        End If
    End Sub
End Class
