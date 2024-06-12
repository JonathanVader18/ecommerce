
Imports System.Data

Partial Class tallas
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub tallas_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "tallas.aspx"
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='tallas.aspx' and cvTipoMenu ='Header'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            lblTitulo.Text = dtTitulo.Rows(0)(0)
            fnCargaPagina()
        End If
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
                'sHTMLEncabezado = dtTitulo.Rows(0)("cvHtml")
                literal = New LiteralControl(sHTMLEncabezado)

            End If
        End If
    End Sub
End Class
