
Imports System.Data

Partial Class misdatos
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ssql = "UPDATE config.Usuarios SET cvNombreCompleto=" & "'" & txtNombre.Text & "',cvRFC=" & "'" & txtRFC.Text & "',cvTelefono1=" & "'" & txtTel1.Text & "' , cvTelefono2=" & "'" & txtTel2.Text & "',cvMail='" & txtemail.Text & "' where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Datos guardados", Me.Page)
        Response.Redirect("preferencias.aspx")
    End Sub

    Private Sub misdatos_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Cargamos todo
        If Not IsPostBack Then
            ssql = "SELECT ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
            Dim dtDatos As New DataTable
            dtDatos = objDatos.fnEjecutarConsulta(ssql)
            If dtDatos.Rows.Count > 0 Then
                If CStr(dtDatos.Rows(0)("Mail")) = "" Then
                    txtemail.Text = Session("UserB2C")
                Else
                    txtemail.Text = dtDatos.Rows(0)("Mail")
                End If

                txtRFC.Text = dtDatos.Rows(0)("RFC")
                txtTel1.Text = dtDatos.Rows(0)("Tel1")
                txtTel2.Text = dtDatos.Rows(0)("Tel2")
                txtNombre.Text = dtDatos.Rows(0)("Nombre")
            End If
        End If

    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("preferencias.aspx")
    End Sub
End Class
