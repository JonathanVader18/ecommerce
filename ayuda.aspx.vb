Imports System.Data
Partial Class ayuda
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub ayuda_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "ayuda.aspx"
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='ayuda.aspx' and cvTipoMenu in('Header','Footer')"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            fnCargaMenu(dtTitulo.Rows(0)(0))
            If Request.QueryString.Count > 0 Then
                fnCargaContenido(dtTitulo.Rows(0)(0), Request.QueryString("Men"), Request.QueryString("Sub"))
            Else
                ''Mostramos por default el primer tema de la categoría
                ssql = "select top 1 cvCategoria,cvContenidoHTML,cvMenu,cvSubMenu from config.Paginas_Detalle where cvPagina=" & "'" & dtTitulo.Rows(0)(0) & "'  "
                Dim dtCat As New DataTable
                dtCat = objDatos.fnEjecutarConsulta(ssql)
                If dtCat.Rows.Count > 0 Then
                    fnCargaContenido(dtTitulo.Rows(0)(0), dtCat.Rows(0)("cvMenu"), dtCat.Rows(0)("cvSubMenu"))
                End If

            End If
        End If
    End Sub
    Public Sub fnCargaMenu(Pagina As String)
        ssql = "Select Distinct cvMenu from config.Paginas_Menu WHERe cvPagina=" & "'" & Pagina & "'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)
        Dim sHTMLEncabezado As String = ""
        Dim sHTML As String = ""

        sHTMLEncabezado = sHTMLEncabezado & "<div class='filtros-dektop'>"
        sHTMLEncabezado = sHTMLEncabezado & " <div class='panel-group filtos-catalogo' id='accordion' role='tablist' aria-multiselectable='true'>"
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHTML = sHTML & "<div class='panel'>"
            sHTML = sHTML & " <div class='panel-heading' role='tab' id='headingMenu" & (i + 1) & "'>"
            sHTML = sHTML & "  <h4 class='categoria'> "
            sHTML = sHTML & "   <a role='button' data-toggle='collapse' data-parent='#accordion' href='#collapseMenu" & (i + 1) & "' aria-expanded='true' aria-controls='collapseMenu" & (i + 1) & "'> " & dtMenus.Rows(i)(0) & " </a>"
            sHTML = sHTML & "  </h4>"
            sHTML = sHTML & " </div>"
            sHTML = sHTML & " <div id='collapseMenu" & (i + 1) & "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='headingMenu" & (i + 1) & "'>"
            sHTML = sHTML & " <div class='panel-body'>"
            sHTML = sHTML & "  <ul class='subcategorias'>"
            ''Ahora  subMenu
            ssql = "select  cvSubMenu from config.Paginas_Menu WHERe cvPagina=" & "'" & Pagina & "' ANd cvMenu =" & "'" & dtMenus.Rows(i)(0) & "' order by ciOrden"
            Dim dtsubMenu As New DataTable
            dtsubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtsubMenu.Rows.Count - 1 Step 1
                sHTML = sHTML & "<li><a href='ayuda.aspx?Men=" & dtMenus.Rows(i)(0) & "&Sub=" & dtsubMenu.Rows(x)(0) & "'>" & dtsubMenu.Rows(x)(0) & "</a></li>"
            Next
            sHTML = sHTML & "  </ul>"
            sHTML = sHTML & " </div>" 'Div id CollapseMenu
            sHTML = sHTML & "</div>" 'Div class body
            sHTML = sHTML & "</div>" 'Div Class Panel

        Next
        sHTMLEncabezado = sHTMLEncabezado & sHTML
        sHTMLEncabezado = sHTMLEncabezado & "</div></div>"
        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlMenu.Controls.Clear()
        pnlMenu.Controls.Add(literal)
    End Sub
    Public Sub fnCargaContenido(Pagina As String, Menu As String, SubMenu As String)
        Dim sHTMLEncabezado As String = ""
        Dim sHTML As String = ""

        sHTMLEncabezado = sHTMLEncabezado & "<h2 class='tit-info'>" & SubMenu & "</h2>"
        sHTMLEncabezado = sHTMLEncabezado & " <div class='panel-group tymnyce' id='accordioninfo' role='tablist' aria-multiselectable='true'> "
        If SubMenu = "" Then
            ssql = "select top 1 cvCategoria,cvContenidoHTML from config.Paginas_Detalle where cvPagina=" & "'" & Pagina & "' AND cvMenu =" & "'" & Menu & "' order by ciOrden"
        Else
            ssql = "select cvCategoria,cvContenidoHTML from config.Paginas_Detalle where cvPagina=" & "'" & Pagina & "' AND cvMenu =" & "'" & Menu & "' AND cvSubMenu=" & "'" & SubMenu & "' order by ciOrden "
        End If

        Dim dtCategorias As New DataTable
        dtCategorias = objDatos.fnEjecutarConsulta(ssql)

        For i = 0 To dtCategorias.Rows.Count - 1 Step 1
            sHTML = sHTML & "<div class='panel'>"
            If dtCategorias.Rows.Count = 1 Then
                sHTML = sHTML & " <div class='panel' role='tab' id='thirex" & (i + 1) & "'>"
                sHTML = sHTML & "  <h4 class='categoria'> "

                sHTML = sHTML & "   <a>" & dtCategorias.Rows(i)("cvCategoria") & "</a>"
            Else
                sHTML = sHTML & " <div class='panel-heading' role='tab' id='thirex" & (i + 1) & "'>"
                sHTML = sHTML & "  <h4 class='categoria'> "

                sHTML = sHTML & "   <a role='button' data-toggle='collapse' data-parent='#accordioninfo' href='#thir" & (i + 1) & "' aria-expanded='true' aria-controls='thir" & (i + 1) & "'>" & dtCategorias.Rows(i)("cvCategoria") & "</a>"
            End If

            sHTML = sHTML & "  </h4> "
            sHTML = sHTML & " </div> "
            If dtCategorias.Rows.Count = 1 Then
                sHTML = sHTML & " <div id='thir" & (i + 1) & "' class='panel' role='tabpanel' aria-labelledby='thir" & (i + 1) & "'>"
            Else
                sHTML = sHTML & " <div id='thir" & (i + 1) & "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='thir" & (i + 1) & "'>"
            End If

            sHTML = sHTML & "  <div class='panel-body'>" & dtCategorias.Rows(i)("cvContenidoHTML") & "</div>"
            sHTML = sHTML & " </div>"
            sHTML = sHTML & "</div>"
        Next


        sHTMLEncabezado = sHTMLEncabezado & sHTML
        sHTMLEncabezado = sHTMLEncabezado & "</div>"
        Dim literal As New LiteralControl(sHTMLEncabezado)
        pnlContenido.Controls.Clear()
        pnlContenido.Controls.Add(literal)

    End Sub
End Class
