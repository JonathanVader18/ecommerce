Imports System.Data
Partial Class servicios_int
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub servicios_int_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='servicios.aspx' and cvTipoMenu ='Header'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            lblTitulo.Text = dtTitulo.Rows(0)(0)
        End If
        fnCargaServicio(Request.QueryString("Serv"))
    End Sub
    Public Sub fnCargaServicio(Servicio As String)
        Dim sHTMLEncabezado As String = ""
        Dim sHTML As String = ""
        ssql = "SELECT * FROM config.Servicios_detalle where cvServicio= " & "'" & Servicio & "'"
        Dim dtServicios As New DataTable
        dtServicios = objDatos.fnEjecutarConsulta(ssql)

        sHTMLEncabezado = sHTMLEncabezado & "<div class='col-xs-12'>"

        If dtServicios.Rows.Count > 0 Then
            ssql = "SELECT * FROM config.Servicios WHERE cvNombre=" & "'" & Servicio & "'"
            Dim dtServicio As New DataTable
            dtServicio = objDatos.fnEjecutarConsulta(ssql)
            If dtServicio.Rows.Count > 0 Then
                sHTML = sHTML & "  <img src=" & "'" & dtServicio.Rows(0)("cvImagen") & "' class='img-responsive' alt='imagen servicio'>"
                sHTML = sHTML & "  <h2>" & dtServicio.Rows(0)("cvNombre") & "</h2>"
                sHTML = sHTML & " <div class='descripcion int'> "
                sHTML = sHTML & " <p>" & dtServicios.Rows(0)("cvDetalleServicio") & "</p>"
                sHTML = sHTML & " </div>"
            End If

        End If


        sHTMLEncabezado = sHTMLEncabezado & sHTML
        sHTMLEncabezado = sHTMLEncabezado & "</div>"
        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlServicio.Controls.Clear()
        pnlServicio.Controls.Add(literal)
    End Sub

End Class
