
Imports System.Data

Partial Class misdatoshnd
    Inherits System.Web.UI.Page

    Public objdatos As New Cls_Funciones
    Public ssql As String

    Private Sub misdatoshnd_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnMenuPreferencias()
        Dim fila As DataRow

        If Not IsPostBack Then

            ''El RFC y datos generales

            ssql = "SELECT ISNULL(cvRFC,''),ISNULL(cvNombreCompleto,'') as Nombre,ISNULL(cvTelefono1,'') Tel1, ISNULL(cvTelefono2,'') Tel2,ISNULL(cvmail,'') Mail FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtRFC As New DataTable
            dtRFC = objdatos.fnEjecutarConsulta(ssql)
            If dtRFC.Rows.Count > 0 Then
                txtRFC.Text = dtRFC.Rows(0)(0)
                txtNombre.Text = dtRFC.Rows(0)("Nombre")
                txtTelefono.Text = dtRFC.Rows(0)("Tel1")
                txtCelular.Text = dtRFC.Rows(0)("Tel2")
                txtemail.Text = dtRFC.Rows(0)("Mail")
            End If

            ''La empresa, si esque aplica
            ssql = "SELECT ISNULL(cvEmpresa,'') FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtEmpresa As New DataTable
            dtEmpresa = objdatos.fnEjecutarConsulta(ssql)
            If dtEmpresa.Rows.Count > 0 Then
                txtEmpresa.Text = dtEmpresa.Rows(0)(0)

            End If


            'End If
        End If
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
        ssql = "UPDATE config.Usuarios SET cvNombreCompleto=" & "'" & txtNombre.Text & "',cvRFC=" & "'" & txtRFC.Text & "',cvTelefono1=" & "'" & txtTelefono.Text & "' , cvTelefono2=" & "'" & txtCelular.Text & "',cvMail='" & txtemail.Text & "' where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
        objdatos.fnEjecutarInsert(ssql)

        Try
            ssql = "UPDATE config.Usuarios SET cvEmpresa=" & "'" & txtEmpresa.Text & "' where cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C'"
            objdatos.fnEjecutarInsert(ssql)
        Catch ex As Exception

        End Try



        objdatos.Mensaje("Datos guardados", Me.Page)
        Response.Redirect("preferencias.aspx")
    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("preferencias.aspx")
    End Sub
End Class
