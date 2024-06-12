Imports System.Data
Partial Class Products
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ''Cargamos los datos de la empresa
            ssql = "select cvNombreComercial ,cvSlogan,cvCorreo,cvTelefono from SAP_Tienda ..DatosCliente "
            Dim dtEmpresa As New DataTable
            dtEmpresa = objdatos.fnEjecutarConsulta(ssql)
            lblCorreo.Text = dtEmpresa.Rows(0)("cvCorreo")
            lblCorreoTienda.Text = dtEmpresa.Rows(0)("cvCorreo")
            ''    lblNombreEmpresa.Text = dtEmpresa.Rows(0)("cvNombreComercial")
            lblNombreLargo.Text = dtEmpresa.Rows(0)("cvNombreComercial")
            lblSlogan.Text = dtEmpresa.Rows(0)("cvSlogan")
            '   lblSlogan2.Text = dtEmpresa.Rows(0)("cvSlogan")
            lblTelefono.Text = dtEmpresa.Rows(0)("cvTelefono")
            lblTelefono2.Text = dtEmpresa.Rows(0)("cvTelefono")


            ssql = "select distinct cvGrupo1 as Grupo1 from SAP_Tienda..Agrupadores "
            Dim scriptMenu As String
            scriptMenu = "<ul class='nav navbar-nav navbar-right'> "
            scriptMenu = scriptMenu & "<li class='dropdown megamenu'> "
            scriptMenu = scriptMenu & "<a href='#' class='dropdown-toggle' data-toggle='dropdown' data-hover='dropdown' data-delay='300' data-close-others='true'>La Tienda</a> "
            scriptMenu = scriptMenu & "<ul class='dropdown-menu'> "

            Dim dtMenu As New DataTable
            dtMenu = objdatos.fnEjecutarConsulta(ssql)
            For i = 0 To dtMenu.Rows.Count - 1 Step 1
                scriptMenu = scriptMenu & " <li class='col-sm-4 col-md-3'> "
                scriptMenu = scriptMenu & "  <ul class='list-unstyled'> "
                scriptMenu = scriptMenu & "   <li class='title'> " & dtMenu.Rows(i)("Grupo1") & "</li>"
                ssql = "select distinct cvGrupo2 as Grupo2 from SAP_Tienda..Agrupadores WHERE cvGrupo1=" & "'" & dtMenu.Rows(i)("Grupo1") & "'"
                Dim dtGrupo2 As New DataTable
                dtGrupo2 = objdatos.fnEjecutarConsulta(ssql)

                For j = 0 To dtGrupo2.Rows.Count - 1 Step 1
                    scriptMenu = scriptMenu & " <li><a href='products.aspx?Cat=" & dtGrupo2.Rows(j)("Grupo2") & "'> " & dtGrupo2.Rows(j)("Grupo2") & "</a></li>"
                Next
                scriptMenu = scriptMenu & " </ul> "
                scriptMenu = scriptMenu & " </li> "
            Next

            scriptMenu = scriptMenu & "   </ul> " '' dropdown-menu
            scriptMenu = scriptMenu & "   </li> " ''dropdown megamenu
            scriptMenu = scriptMenu & " </ul> " ''nav navbar-nav navbar-right

            pnlMenu.Controls.Add(New LiteralControl(scriptMenu))

            ''Ahora las categorias
            Dim x As Integer = 0
            Dim scriptCat As String = " <ul class='list-unstyled' id='categories' role='tablist' aria-multiselectable='true'>"
            For i = 0 To dtMenu.Rows.Count - 1 Step 1
                scriptCat = scriptCat & "<li class='panel'><a class='collapsed' role='button' data-toggle='collapse' data-parent='#categories'  href='#parent-" & (i + 1) & "' aria-expanded='true' aria-controls= '#parent-" & (i + 1) & "'><span> " & dtMenu.Rows(i)("Grupo1") & "</span> </a> "
                scriptCat = scriptCat & "<ul id='parent-" & (i + 1) & "' class='list-unstyled panel-collapse collapse' role='menu'>"
                ssql = "select distinct cvGrupo2 as Grupo2 from SAP_Tienda..Agrupadores WHERE cvGrupo1=" & "'" & dtMenu.Rows(i)("Grupo1") & "'"
                Dim dtGrupo2 As New DataTable
                dtGrupo2 = objdatos.fnEjecutarConsulta(ssql)
                For j = 0 To dtGrupo2.Rows.Count - 1 Step 1
                    scriptCat = scriptCat & " <li><a href='products.aspx?Cat=" & dtGrupo2.Rows(j)("Grupo2") & "'>  " & dtGrupo2.Rows(j)("Grupo2") & "</a></li>"
                Next
                scriptCat = scriptCat & "   </ul> "
                scriptCat = scriptCat & "   </li> "
            Next
            scriptCat = scriptCat & "   </ul> "
            pnlCat.Controls.Add(New LiteralControl(scriptCat))

            If Request.QueryString.Count > 0 Then
                lblGrupo2.Text = Request.QueryString("Cat")
            End If
        End If
    End Sub
End Class
