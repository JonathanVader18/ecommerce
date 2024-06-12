
Imports System.Data
Partial Class blog
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String
    Private Sub blog_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "blog.aspx"
        If Not IsPostBack Then
            fnCargaCategorias()
            fnEntradasRecientes()
            If Request.QueryString.Count = 0 Then
                fnCargaEntradas(0)
            Else
                If Request.QueryString("cat") <> "" Then
                    Try
                        fnCargaEntradas(Request.QueryString("cat"))
                    Catch ex As Exception

                    End Try

                End If
            End If

        End If
    End Sub
    Public Sub fnCargaEntradas(Categoria As Int16)
        If Categoria = 0 Then
            ssql = "select ciIdEntrada,ciIdCategoria,cvCategoria ,cvTitulo ,cvImagenPal ,cdFecha,cvDescripcion from blog.Blog_Hdr order by cdFecha desc"
        Else
            ssql = "select ciIdEntrada,ciIdCategoria,cvCategoria ,cvTitulo ,cvImagenPal ,cdFecha,cvDescripcion from blog.Blog_Hdr  where ciIdCategoria=" & Categoria & " order by cdFecha desc"
        End If

        Dim dtEntradas As New DataTable
        dtEntradas = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        For i = 0 To dtEntradas.Rows.Count - 1 Step 1
            If i = 0 Then
                ''Primer registro
                sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 entrada e-principal'>"
            Else
                sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-12 col-md-6  entrada e-secundaria'>"
            End If
            ''La imagen
            sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 no-padding'>"
            sHtmlBanner = sHtmlBanner & "  <img src='" & dtEntradas.Rows(i)("cvImagenPal") & "' class='img-responsive' />"
            sHtmlBanner = sHtmlBanner & "</div>"

            ''Cuerpo de la nota
            sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 no-padding'>"

            sHtmlBanner = sHtmlBanner & " <div class='data-e'>"
            sHtmlBanner = sHtmlBanner & "  <a href='blog.aspx?cat=" & dtEntradas.Rows(i)("ciIdCategoria") & "' class='categoria'>" & dtEntradas.Rows(i)("cvCategoria") & "</a>"
            sHtmlBanner = sHtmlBanner & "  <time class='fecha-publicacion'>" & CDate(dtEntradas.Rows(i)("cdFecha")).ToShortDateString & "</time> "
            sHtmlBanner = sHtmlBanner & " </div>"

            ''Titulo
            sHtmlBanner = sHtmlBanner & "<h2 class='titulo'>" & dtEntradas.Rows(i)("cvTitulo") & "</h2>"
            sHtmlBanner = sHtmlBanner & "<div class='descripcion'>"
            sHtmlBanner = sHtmlBanner & " <p>" & dtEntradas.Rows(i)("cvDescripcion") & "</p>"
            sHtmlBanner = sHtmlBanner & "</div>"

            sHtmlBanner = sHtmlBanner & "<div>"
            sHtmlBanner = sHtmlBanner & "   <a class='btn' href='blog-interno.aspx?entrada=" & dtEntradas.Rows(i)("ciIdEntrada") & "'>LEER MÁS </a>"
            sHtmlBanner = sHtmlBanner & "</div>"


            sHtmlBanner = sHtmlBanner & "</div>"


            sHtmlBanner = sHtmlBanner & "</div>"
        Next
        Dim literal As New LiteralControl(sHtmlBanner)
        pnlEntradas.Controls.Clear()
        pnlEntradas.Controls.Add(literal)
    End Sub

    Public Sub fnCargaCategorias()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select ciIdCategoria, cvCategoria  from config .CategoriasBlog where cvEstatus ='ACTIVO'"
        Dim dtCategorias As New DataTable
        dtCategorias = objDatos.fnEjecutarConsulta(ssql)
        For i = 0 To dtCategorias.Rows.Count - 1 Step 1
            sHtmlBanner = sHtmlBanner & "<li><a href='blog.aspx?cat=" & dtCategorias.Rows(i)("ciIdCategoria") & "' class='' >" & dtCategorias.Rows(i)("cvCategoria") & "</a></li>"
        Next
        Dim literal As New LiteralControl(sHtmlBanner)
        pnlCat.Controls.Clear()
        pnlCat.Controls.Add(literal)
    End Sub

    Public Sub fnEntradasRecientes()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select top 4 ciIdEntrada,ciIdCategoria,cvCategoria ,cvTitulo ,cvImagenPal ,cdFecha,cvDescripcion from blog.Blog_Hdr order by cdFecha desc"
        Dim dtEntradas As New DataTable
        dtEntradas = objDatos.fnEjecutarConsulta(ssql)

        For i = 0 To dtEntradas.Rows.Count - 1 Step 1
            sHtmlBanner = sHtmlBanner & "<div class='lat-entrada'> "
            sHtmlBanner = sHtmlBanner & "  <a href='blog.aspx?cat=" & dtEntradas.Rows(i)("ciIdCategoria") & "'>"
            sHtmlBanner = sHtmlBanner & "   <div class='imagen'><img src='" & dtEntradas.Rows(i)("cvImagenPal") & "' class='img-responsive' > </div>"
            sHtmlBanner = sHtmlBanner & "   <div class='textos'> "
            sHtmlBanner = sHtmlBanner & "    <span class='title'>" & dtEntradas.Rows(i)("cvTitulo") & "</span> "
            sHtmlBanner = sHtmlBanner & "    <time>" & CDate(dtEntradas.Rows(i)("cdFecha")).ToShortDateString & "</time> "
            sHtmlBanner = sHtmlBanner & "   </div>"
            sHtmlBanner = sHtmlBanner & " </a>"
            sHtmlBanner = sHtmlBanner & "</div>"
        Next
        Dim literal As New LiteralControl(sHtmlBanner)
        pnlEntradasRecientes.Controls.Clear()
        pnlEntradasRecientes.Controls.Add(literal)
    End Sub
End Class
