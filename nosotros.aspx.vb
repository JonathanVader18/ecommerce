Imports System.Data
Partial Class nosotros
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub nosotros_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "nosotros.aspx"
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='nosotros.aspx' and cvTipoMenu ='Header'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            lblTitulo.Text = dtTitulo.Rows(0)(0)
        End If
        fnCargaPagina()
    End Sub
    Public Sub fnCargaPagina()
        ssql = "select cvHtml, ISNULL(cvImagenBanner,'') as Banner from config.Paginas where cvNombre ='" & lblTitulo.Text & "'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            Dim sHTMLEncabezado As String = dtTitulo.Rows(0)(0)
            Dim literal As New LiteralControl(sHTMLEncabezado)
            pnlContenido.Controls.Clear()
            pnlContenido.Controls.Add(literal)

            If dtTitulo.Rows(0)("Banner") <> "" Then
                sHTMLEncabezado = "<div class=""pag-cont-banner"" style=""background-image: url('" & dtTitulo.Rows(0)("Banner") & "')""></div>"
                literal = New LiteralControl(sHTMLEncabezado)
                pnlBanner.Controls.Clear()
                pnlBanner.Controls.Add(literal)
            End If
        End If
    End Sub
End Class
