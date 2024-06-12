Imports System.Data
Partial Class sucursales
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub sucursales_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "sucursales.aspx"
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='sucursales.aspx' and cvTipoMenu ='Header'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            '   lblTitulo.Text = ""
        End If

        fnCargaSucursales()
    End Sub
    Public Sub fnCargaSucursales()
        Dim sHTMLEncabezado As String = ""
        Dim sHTML As String = ""
        ssql = "SELECT * FROM config.Servicios "
        Dim dtServicios As New DataTable
        dtServicios = objDatos.fnEjecutarConsulta(ssql)

        sHTMLEncabezado = sHTMLEncabezado & "<div class='col-xs-12'>"
        For i = 0 To dtServicios.Rows.Count - 1 Step 1
            sHTML = sHTML & " <div class='col-xs-12 col-sm-6 col-md-4 sucursales'> "
            sHTML = sHTML & "  <div class='cont-img'> <img src=" & "'" & dtServicios.Rows(i)("cvImagen") & "' class='img-responsive' alt='imagen servicio'> </div>"

            sHTML = sHTML & " <div class='descripcion'> "
            sHTML = sHTML & "  <h2>" & dtServicios.Rows(i)("cvNombre") & "</h2> <p>" & dtServicios.Rows(i)("cvDescripcion") & "</p></div>"
            'sHTML = sHTML & " <div class='col-xs-12 no-padding'>"
            'sHTML = sHTML & "  <a class='btn' href='servicios-int.aspx?Serv=" & dtServicios.Rows(i)("cvNombre") & "'>ver más</a>"
            'sHTML = sHTML & " </div>"
            sHTML = sHTML & "</div>"
        Next
        sHTMLEncabezado = sHTMLEncabezado & sHTML
        sHTMLEncabezado = sHTMLEncabezado & "</div>"
        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlsucursales.Controls.Clear()
        pnlsucursales.Controls.Add(literal)

    End Sub
End Class
