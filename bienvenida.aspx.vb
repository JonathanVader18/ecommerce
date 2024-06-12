
Imports System.Data

Partial Class bienvenida
    Inherits System.Web.UI.Page

    Public ssql As String = ""
    Public objDatos As New Cls_Funciones
    Private Sub bienvenida_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then

                lblUsuario.Text = "Hola, " & Session("NombreUserB2C")
                lblBienvenida.Text = "Bienvenido/a   <b> " & dtcliente.Rows(0)(0) & " </b>"

            End If
            If CStr(Session("Page")) <> "" Then
                If CStr(Session("Page")).Contains("pago") Then
                    btnRegresar.Visible = True
                End If
            End If
        End If
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Try
            If CStr(Session("Page")) <> "" Then
                If CStr(Session("Page")).Contains("pago") Then
                    Response.Redirect(CStr(Session("Page")))
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
