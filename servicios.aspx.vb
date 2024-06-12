Imports System.Data
Partial Class servicios
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub servicios_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "servicios.aspx"
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='servicios.aspx' and cvTipoMenu ='Header'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            lblTitulo.Text = dtTitulo.Rows(0)(0)
        End If

        fnCargaServicios()
    End Sub
    Public Sub fnCargaServicios()
        Dim sHTMLEncabezado As String = ""
        Dim sHTML As String = ""
        ssql = "SELECT * FROM config.Servicios "
        Dim dtServicios As New DataTable
        dtServicios = objDatos.fnEjecutarConsulta(ssql)

        sHTMLEncabezado = sHTMLEncabezado & "<div class='col-xs-12'>"
        For i = 0 To dtServicios.Rows.Count - 1 Step 1
            sHTML = sHTML & " <div class='col-xs-12 col-sm-6 col-md-3 servicio'> "
            sHTML = sHTML & "  <img src=" & "'" & dtServicios.Rows(i)("cvImagen") & "' class='img-responsive' alt='imagen servicio'>"
            sHTML = sHTML & "  <h2>" & dtServicios.Rows(i)("cvNombre") & "</h2>"
            sHTML = sHTML & " <div class='descripcion'><p>" & dtServicios.Rows(i)("cvDescripcion") & "</p></div>"
            sHTML = sHTML & " <div class='col-xs-12 no-padding'>"
            Try
                If dtServicios.Rows(i)("cvLiga") <> "" Then
                    sHTML = sHTML & "  <a class='btn' href='" & dtServicios.Rows(i)("cvLiga") & "'>ver más</a>"
                Else
                    sHTML = sHTML & "  <a class='btn' href='servicios-int.aspx?Serv=" & dtServicios.Rows(i)("cvNombre") & "'>ver más</a>"
                End If

            Catch ex As Exception
                sHTML = sHTML & "  <a class='btn' href='servicios-int.aspx?Serv=" & dtServicios.Rows(i)("cvNombre") & "'>ver más</a>"

            End Try

            sHTML = sHTML & " </div>"
            sHTML = sHTML & "</div>"
        Next
        sHTMLEncabezado = sHTMLEncabezado & sHTML
        sHTMLEncabezado = sHTMLEncabezado & "</div>"
        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlServicios.Controls.Clear()
        pnlServicios.Controls.Add(literal)

    End Sub
End Class
