
Imports System.Data

Partial Class misdatosla
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub misdatosla_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Cargamos todo
        If Not IsPostBack Then


            Dim dtSexo As New DataTable
            dtSexo.Columns.Add("Clave")
            dtSexo.Columns.Add("Descripcion")

            Dim fila As DataRow
            fila = dtSexo.NewRow
            fila("Clave") = "M"
            fila("Descripcion") = "Masculino"
            dtSexo.Rows.Add(fila)

            fila = dtSexo.NewRow
            fila("Clave") = "F"
            fila("Descripcion") = "Femenino"
            dtSexo.Rows.Add(fila)

            ddlSexo.DataSource = dtSexo
            ddlSexo.DataTextField = "Descripcion"
            ddlSexo.DataValueField = "Clave"
            ddlSexo.DataBind()


            ssql = "SELECT ISNULL(cvMail,'') as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
            Dim dtDatos As New DataTable
            dtDatos = objDatos.fnEjecutarConsulta(ssql)
            If dtDatos.Rows.Count > 0 Then
                txtemail.Text = dtDatos.Rows(0)("Mail")
                txtRFC.Text = dtDatos.Rows(0)("RFC")
                txtTel1.Text = dtDatos.Rows(0)("Tel1")
                txtTel2.Text = dtDatos.Rows(0)("Tel2")
                txtNombre.Text = dtDatos.Rows(0)("Nombre")
            End If
        End If
    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ssql = "UPDATE config.Usuarios SET cvNombreCompleto=" & "'" & txtNombre.Text & "',cvRFC=" & "'" & txtRFC.Text & "',cvTelefono1=" & "'" & txtTel1.Text & "' , cvTelefono2=" & "'" & txtTel2.Text & "',cvMail='" & txtemail.Text & "' where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
        objDatos.fnEjecutarInsert(ssql)

        ''Update de datos complementarios
        ssql = "UPDATE config.Usuarios SET cdFechaNac=" & "'" & txtDate.Text & "',cvSexo=" & "'" & ddlSexo.SelectedValue & "',cvCedulaIdentificacion=" & "'" & txtTel1.Text & "' , cvApellidos=" & "'" & txtApellidos.Text & "' where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
        objDatos.fnEjecutarInsert(ssql)

        objDatos.Mensaje("Datos guardados", Me.Page)
        Response.Redirect("preferencias.aspx")
    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("preferencias.aspx")
    End Sub
End Class
