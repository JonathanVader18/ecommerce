
Imports System.Data

Partial Class menuConfig
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String = ""
    Private Sub menuConfig_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("loginAdmin") = "" Then
                Response.Redirect("loginadmin.aspx")
            End If


            Dim ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre,ISNULL(cvCliente,'') as Cliente from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then
                If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
                    btnUsuarios.Visible = False
                    btnSincronizar.Visible = True
                End If
                If CStr(dtcliente.Rows(0)(1)).Contains("Salama") Then
                    btnDescuentos.Visible = True
                End If
                If CStr(dtcliente.Rows(0)(1)).ToUpper.Contains("BOSS") Then
                    btnDescuentos.Visible = False
                    btnSincronizar.Visible = False
                    btnBanners.Visible = False
                    btnPRomos.Visible = False
                    btnDescuentos.Visible = False
                End If
                If CStr(dtcliente.Rows(0)(1)).ToUpper.Contains("ZEYCO") Then
                    btnAdminClientes.Visible = True
                    btnPRomos.Visible = False
                    btnSincronizar.Visible = True
                End If
                If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("HAWK") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("ALTU") Then
                    btnActivarB2C.Visible = True
                    btnDesactivarB2C.Visible = True
                    btnDescuentos.Visible = False
                    btnSincronizar.Visible = False
                    btnBanners.Visible = False
                    btnPRomos.Visible = False
                    btnDescuentos.Visible = False
                End If
                If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("SUJEA") Then
                    btnActivarB2C.Visible = True
                    btnDesactivarB2C.Visible = True
                    btnUsuarios.Visible = True
                    btnDescuentos.Visible = False
                    btnSincronizar.Visible = False
                    btnBanners.Visible = False
                    btnPRomos.Visible = False
                    btnDescuentos.Visible = False
                End If
            End If
        End If
    End Sub
    Protected Sub btnBanners_Click(sender As Object, e As EventArgs) Handles btnBanners.Click

        Dim sPaginaBanners As String = "configBanners.aspx"

        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("BOSS") Then
            sPaginaBanners = "configBannersBoss.aspx"
        End If

        Response.Redirect(sPaginaBanners)
    End Sub
    Protected Sub btnUsuarios_Click(sender As Object, e As EventArgs) Handles btnUsuarios.Click
        Response.Redirect("configUsuarios.aspx")
    End Sub


    Public Function fnCargaCategoriasLateral()
        Dim ssql As String = ""
        Dim objdatos As New Cls_Funciones
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        Dim Param1 As String = ""
        Dim Param2 As String = ""
        Dim Param3 As String = ""
        Dim xPar As Int16 = 0
        For Each Param As String In Request.QueryString
            If xPar = 0 Then
                Param1 = Request.QueryString(Param)
            End If
            If xPar = 1 Then
                Param2 = Request.QueryString(Param)
            End If
            If xPar = 2 Then
                Param3 = Request.QueryString(Param)
            End If
            xPar = xPar + 1
        Next

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objdatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objdatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objdatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(1))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objdatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                Dim sValorPintar As String = ""

                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objdatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objdatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                End If

                Dim sCampoPrimerNivel As String = ""
                ''Nos traemos el nivel 2 de la categoría seleccionada
                ssql = objdatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If

                sHtmlEncabezado = sHtmlEncabezado & " <div class='panel-heading'>"

                sHtmlEncabezado = sHtmlEncabezado & " <h4 class='panel'>"
                'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-heading' role='tab' id='heading" & (i + 1) & "'>"
                'sHtmlEncabezado = sHtmlEncabezado & "   <h4 class='categoria'>"


                If Param1 <> "" Then
                    If Param1 = sLinkPrimerNivel Then
                        sValorPintar = "<b>" & sValorPintar & "</b>"
                        sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "'>" & sValorPintar & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#menu-collapse" & (i + 1) & "' class='collapse' aria-expanded='true'></a>"
                    Else
                        sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "'>" & sValorPintar & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#menu-collapse" & (i + 1) & "' class='collapsed'></a>"
                    End If

                Else
                    sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "'>" & sValorPintar & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#menu-collapse" & (i + 1) & "' class='collapsed'></a>"
                End If




                'sHtmlEncabezado = sHtmlEncabezado & "   </h4>"
                'sHtmlEncabezado = sHtmlEncabezado & "  </div>"
                'sHtmlEncabezado = sHtmlEncabezado & " <div id='collapse" & (i + 1) & "' class='panel-collapse collapse in' role='tabpanel' aria-labelledby='heading" & (i + 1) & "'>"
                'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-body'>"
                'sHtmlEncabezado = sHtmlEncabezado & "   <ul class='subcategorias'>"

                sHtmlEncabezado = sHtmlEncabezado & "   </h4>"

                sHtmlEncabezado = sHtmlEncabezado & "   </div>"



                Dim iExisteTercerNivel As Int16 = 0

                Dim sQueryTercer As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =3"
                Dim dtTercerNivel As New DataTable
                dtTercerNivel = objdatos.fnEjecutarConsulta(sQueryTercer)
                If dtTercerNivel.Rows.Count > 0 Then
                    iExisteTercerNivel = 1
                End If
                sHtmlBanner = ""
                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objdatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    '  objDatos.fnLog("Segundo nivel", ssql.Replace("'", ""))

                    ' objDatos.fnLog("Evaluar tercer nivel", ssql.Replace("'", ""))
                    If iExisteTercerNivel = 1 Then

                        '     objDatos.fnLog("Query tercer nivel", sQueryTercer.Replace("'", ""))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objdatos.fnEjecutarConsultaSAP(ssql)

                    Dim sExpand As String = "CLASSMODALIDAD" 'class='panel-collapse collapse in' aria-expanded='true'
                    If Param2 <> "" Then

                        sHtmlBanner = sHtmlBanner & " <div id='menu-collapse" & (i + 1) & "'" & sExpand & "> "

                    Else
                        sHtmlBanner = sHtmlBanner & " <div id='menu-collapse" & (i + 1) & "' class='panel-collapse collapse'> "
                    End If

                    ' sHtmlBanner = sHtmlBanner & " <div id='menu-collapse" & (i + 1) & "' class='panel-collapse collapse'> "
                    sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "


                    Dim sCategoriaPadre As String = CStr(dtCategorias.Rows(i)(0)).Replace(" ", "-").Replace(",", "-")


                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If CStr(dtSegundoNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                            ssql = objdatos.fnObtenerQuery("CampoUsuarioNiv2")
                            ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                            Dim dtValor As New DataTable
                            dtValor = objdatos.fnEjecutarConsultaSAP(ssql)
                            If dtValor.Rows.Count > 0 Then
                                sValorPintar = dtValor.Rows(0)(0)
                            End If
                        Else
                            If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                                sValorPintar = ""
                            Else
                                sValorPintar = dtSubCategoria.Rows(x)(0)
                            End If

                        End If
                        If iExisteTercerNivel = 0 Then
                            '  objDatos.fnLog("Catalogo", "SoloNivel 2: " & sValorPintar)
                            sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 no-padding'> "

                            If Param2 <> "" Then
                                If Param2 = dtSubCategoria.Rows(x)(0) Then
                                    sHtmlBanner = sHtmlBanner.Replace("CLASSMODALIDAD", "class='panel-collapse collapse' aria-expanded='true'")
                                    sHtmlBanner = sHtmlBanner & " <a class='active' data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                                Else
                                    sHtmlBanner = sHtmlBanner.Replace("CLASSMODALIDAD", "class='panel-collapse collapse'")
                                    sHtmlBanner = sHtmlBanner & " <a data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                                End If

                            Else
                                sHtmlBanner = sHtmlBanner & " <a data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                            End If


                            sHtmlBanner = sHtmlBanner & " </a>"
                            sHtmlBanner = sHtmlBanner & "</div>"

                        Else
                            sQueryTercer = objdatos.fnObtenerQuery("Categorias-Tercero")

                            If dtTercerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                                sQueryTercer = sQueryTercer.Replace("[%0]", "ItmsGrpNam")
                            Else
                                sQueryTercer = sQueryTercer.Replace("[%0]", "ISNULL(T0." & dtTercerNivel.Rows(0)("cvCampoSAP") & ",'')")
                                sQueryTercer = sQueryTercer.Replace("[%1]", sCampoPrimerNivel)

                            End If
                            sQueryTercer = sQueryTercer.Replace("[%2]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtSubCategoria.Rows(x)(0) & "'")

                            '   objDatos.fnLog("Query tercer nivel: " & sLinkPrimerNivel & " " & dtSubCategoria.Rows(x)(0), sQueryTercer.Replace("'", ""))

                            ''Preparamos la estructura para el tercer nivel
                            sHtmlBanner = sHtmlBanner & " <div class='panel-group' id='accordion-" & (i + 1) & "'> "
                            sHtmlBanner = sHtmlBanner & "  <div class='panel panel-gtk'> "
                            sHtmlBanner = sHtmlBanner & "    <div class='panel-heading'> "
                            sHtmlBanner = sHtmlBanner & "      <h4 class='panel-title'> "
                            If Param2 <> "" Then
                                If Param2 = dtSubCategoria.Rows(x)(0) Then
                                    sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;font-weight: bold;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & sCategoriaPadre & "' class='collapse' aria-expanded='true'></a>"
                                Else
                                    sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & sCategoriaPadre & "' class='collapsed'></a>"
                                End If
                            Else
                                sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & sCategoriaPadre & "' class='collapsed'></a>"
                            End If

                            sHtmlBanner = sHtmlBanner & "      </h4>"
                            sHtmlBanner = sHtmlBanner & "    </div>"

                            ''Leemos el tercer nivel

                            If Param2 <> "" Then
                                If Param2 = dtSubCategoria.Rows(x)(0) Then
                                    sHtmlBanner = sHtmlBanner & " <div id='menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & sCategoriaPadre & "' class='panel-collapse collapse' aria-extended='true'>"
                                Else
                                    sHtmlBanner = sHtmlBanner & " <div id='menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & sCategoriaPadre & "' class='panel-collapse collapse '>"
                                End If

                            Else
                                sHtmlBanner = sHtmlBanner & " <div id='menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & sCategoriaPadre & "' class='panel-collapse collapse '>"
                            End If

                            sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "

                            Dim dtTercer As New DataTable
                            dtTercer = objdatos.fnEjecutarConsultaSAP(sQueryTercer)
                            '     objDatos.fnLog("Filas tercer nivel", dtTercer.Rows.Count)

                            For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                '    objDatos.fnLog("tercer nivel", dtTercer.Rows(y)(0))
                                sValorPintar = ""
                                If CStr(dtTercerNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                    ssql = objdatos.fnObtenerQuery("CampoUsuarioNiv3")
                                    ssql = ssql.Replace("[%0]", dtTercer.Rows(y)(0))
                                    Dim dtValor As New DataTable
                                    dtValor = objdatos.fnEjecutarConsultaSAP(ssql)
                                    If dtValor.Rows.Count > 0 Then
                                        sValorPintar = dtValor.Rows(0)(0)
                                    End If
                                Else
                                    sValorPintar = dtTercer.Rows(y)(0)
                                End If
                                If sValorPintar <> "" Then
                                    sHtmlBanner = sHtmlBanner & " <div class='col-xs-12 no-padding'>"
                                    If Param3 <> "" Then
                                        If Param3 = dtTercer.Rows(y)(0) Then
                                            sHtmlBanner = sHtmlBanner & " <a class='active' data-grafica='20' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a></b>"
                                        Else
                                            sHtmlBanner = sHtmlBanner & "  <a data-grafica='20' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a>"
                                        End If
                                    Else
                                        sHtmlBanner = sHtmlBanner & "  <a data-grafica='20' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a>"
                                    End If

                                    sHtmlBanner = sHtmlBanner & " </div>"
                                End If

                            Next
                            sHtmlBanner = sHtmlBanner & " </div></div>"

                            sHtmlBanner = sHtmlBanner & " </div>"
                            sHtmlBanner = sHtmlBanner & "</div>"
                        End If

                    Next

                    sHtmlBanner = sHtmlBanner & "  </div>"
                    sHtmlBanner = sHtmlBanner & " </div>"
                End If
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner



                '' End If

            Next
            Return sHtmlEncabezado
        End If

    End Function


    Public Function fnCargaCategoriasHTML(NombreCat As String) As String
        Dim ssql As String = ""
        Dim objDatos As New Cls_Funciones
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(1))
                ' objDatos.fnLog("Primer Cats", ssql.Replace("'", ""))
            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""

            If Session("Page") = "catalogo.aspx" Then

                If NombreCat = "Productos" Then
                    sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx'  data-toggle='dropdown' class='dropdown-toggle'>" & NombreCat & "<b class='caret'></b></a>"

                Else
                    sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle active'>" & NombreCat & "<b class='caret'></b></a>"
                End If
            Else
                If NombreCat = "Productos" Then
                    sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx'  data-toggle='dropdown' class='dropdown-toggle'>" & NombreCat & "<b class='caret'></b></a>"

                Else
                    sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle'>" & NombreCat & "<b class='caret'></b></a>"

                End If

            End If

            sHtmlEncabezado = sHtmlEncabezado & "   <ul class='dropdown-menu' style='display: none;'>"
            sHtmlEncabezado = sHtmlEncabezado & "    <li>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div class='yamm-content'>"
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "       <div  class='col-sm-12 no-padding'>"
            sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-4'>"
            sHtmlEncabezado = sHtmlEncabezado & "         <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                Dim sValorPintar As String = ""
                ' objDatos.fnLog("Cats", "entra")
                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objDatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                    ' objDatos.fnLog("Cats", ssql.Replace("'", ""))
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                '  objDatos.fnLog("Cats 1 ", dtCategorias.Rows(i)(0))

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(1) & "'>" & sValorPintar & "</span><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> "
                Else
                    sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'>" & sValorPintar & "</span><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> "
                End If

                sHtmlBanner = sHtmlBanner & "</li> "
                iContador = iContador + 1


            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"

            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-8'>" 'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-content'>" 'El que agregué
            ''Aqui las subcategorias
            iContador = 1
            Dim sCampoPrimerNivel As String = ""
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='Cat-" & (iContador) & "'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-12'>" 'class='col-xs-4'
                sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"
                sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-group' id='accordion-bebidas' role='tablist' aria-multiselectable='true'>"

                ''Nos traemos el nivel 2 de la categoría seleccionada
                If Session("RazonSocial") <> "" Then
                    ssql = objDatos.fnObtenerQuery("Categorias-detB2B")
                Else
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                End If

                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If

                Dim iExisteTercerNivel As Int16 = 0
                Dim sQueryTercer As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =3"
                Dim dtTercerNivel As New DataTable
                dtTercerNivel = objDatos.fnEjecutarConsulta(sQueryTercer)
                If dtTercerNivel.Rows.Count > 0 Then
                    iExisteTercerNivel = 1
                End If

                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    '  objDatos.fnLog("Categorias_det", ssql.Replace("'", ""))
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""

                    Dim sValorPintar As String = ""
                    Dim sCategoriaPadre As String = CStr(dtCategorias.Rows(i)(0)).Replace(" ", "-").Replace(",", "-")

                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            If CStr(dtSegundoNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                If Session("RazonSocial") <> "" Then
                                    ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2B2B")
                                Else
                                    ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2")
                                End If

                                ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                                Dim dtValor As New DataTable
                                dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                ' objDatos.fnLog("CampoUsuarioNiv2", ssql.Replace("'", ""))
                                If dtValor.Rows.Count > 0 Then
                                    sValorPintar = dtValor.Rows(0)(0)
                                End If
                            Else
                                sValorPintar = dtSubCategoria.Rows(x)(0)
                            End If
                            sHtmlBanner = sHtmlBanner & "<div class='panel panel-default'>"
                            sHtmlBanner = sHtmlBanner & " <div class='panel-heading' role='tab' id='heading-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-") & sCategoriaPadre & "'>"
                            sHtmlBanner = sHtmlBanner & "  <h4 class='panel-title'> "

                            sHtmlBanner = sHtmlBanner & "   <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span><a role='button' data-toggle='collapse' data-link='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "' data-parent='#accordion-bebidas' href='#collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-") & sCategoriaPadre & "' aria-expanded='true' aria-controls='collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-") & sCategoriaPadre & "'>"
                            sHtmlBanner = sHtmlBanner & ""
                            sHtmlBanner = sHtmlBanner & "   </a>"
                            sHtmlBanner = sHtmlBanner & "  </h4>"
                            sHtmlBanner = sHtmlBanner & " </div>"

                            If iExisteTercerNivel = 0 Then
                                sHtmlBanner = sHtmlBanner & " </div>" 'Ya solamente cerramos el div
                            Else
                                If Session("RazonSocial") <> "" Then
                                    sQueryTercer = objDatos.fnObtenerQuery("Categorias-TerceroB2B")
                                Else
                                    sQueryTercer = objDatos.fnObtenerQuery("Categorias-Tercero")
                                End If

                                If dtTercerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                                    sQueryTercer = sQueryTercer.Replace("[%0]", "ItmsGrpNam")
                                Else
                                    sQueryTercer = sQueryTercer.Replace("[%0]", "ISNULL(T0." & dtTercerNivel.Rows(0)("cvCampoSAP") & ",'')")
                                    sQueryTercer = sQueryTercer.Replace("[%1]", sCampoPrimerNivel)

                                End If
                                sQueryTercer = sQueryTercer.Replace("[%2]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtSubCategoria.Rows(x)(0) & "'")
                                '    objDatos.fnLog("Query tercer nivel: " & sLinkPrimerNivel & " " & dtSubCategoria.Rows(x)(0), sQueryTercer.Replace("'", ""))


                                Dim dtTercer As New DataTable
                                dtTercer = objDatos.fnEjecutarConsultaSAP(sQueryTercer)


                                ''Preparamos la estructura para el tercer nivel

                                sHtmlBanner = sHtmlBanner & " <div id='collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-") & sCategoriaPadre & "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='heading-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-") & sCategoriaPadre & "'> "
                                sHtmlBanner = sHtmlBanner & " <div class='panel-body'> "

                                For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                    sValorPintar = ""
                                    If CStr(dtTercerNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then

                                        If Session("RazonSocial") <> "" Then
                                            ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv3B2B")
                                        Else

                                            ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv3")
                                        End If

                                        ssql = ssql.Replace("[%0]", dtTercer.Rows(y)(0))
                                        Dim dtValor As New DataTable
                                        dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtValor.Rows.Count > 0 Then
                                            sValorPintar = dtValor.Rows(0)(0)
                                        End If
                                    Else
                                        sValorPintar = dtTercer.Rows(y)(0)
                                    End If

                                    If sValorPintar <> "" Then

                                        sHtmlBanner = sHtmlBanner & "  <a href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a>"

                                    End If
                                Next

                                sHtmlBanner = sHtmlBanner & "  </div>" 'Panel-body
                                sHtmlBanner = sHtmlBanner & " </div> " ' Div collapse
                                sHtmlBanner = sHtmlBanner & "</div> " ' Div del nivel 2

                            End If
                            ' sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "'> " & sValorPintar & "</a></li>"
                        End If

                    Next
                End If

                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                iContador = iContador + 1
                sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"


            Next


            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué





            sHtmlEncabezado = sHtmlEncabezado & " <div class='clearfix'></div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div></div></li></ul></li>"





            Return sHtmlEncabezado

            'Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Function

    Private Sub btnSincronizar_Click(sender As Object, e As EventArgs) Handles btnSincronizar.Click
        Dim sHTML As String
        sHTML = fnCargaCategoriasHTML("Productos")

        ssql = "Delete config.HTML where cvTipo='Categorias'"
        objDatos.fnEjecutarInsert(ssql)



        ssql = "INSERT INTO config.HTML(cvTipo,cvHTML)VALUES('Categorias','" & sHTML.Replace("'", "@") & "') "
        objDatos.fnEjecutarInsert(ssql)


        ssql = "Delete config.HTML where cvTipo='Categorias-lateral'"
        objDatos.fnEjecutarInsert(ssql)

        sHTML = fnCargaCategoriasLateral()
        ssql = "INSERT INTO config.HTML(cvTipo,cvHTML)VALUES('Categorias-lateral','" & sHTML.Replace("'", "@") & "') "
        objDatos.fnEjecutarInsert(ssql)

        ssql = "UPDATE config.html  set cvHTML = REPLACE(CVHTML,'@','''')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Proceso terminado, las categorías se han actualizado", Me.Page)

    End Sub

    Private Sub btnDescuentos_Click(sender As Object, e As EventArgs) Handles btnDescuentos.Click
        Response.Redirect("configDescuentos.aspx")
    End Sub

    Private Sub btnPRomos_Click(sender As Object, e As EventArgs) Handles btnPRomos.Click
        Response.Redirect("configPromociones.aspx")
    End Sub

    Private Sub btnAdminClientes_Click(sender As Object, e As EventArgs) Handles btnAdminClientes.Click
        Response.Redirect("adminclientes.aspx")
    End Sub

    Private Sub btnActivarB2C_Click(sender As Object, e As EventArgs) Handles btnActivarB2C.Click
        ssql = "UPDATE config.parametrizaciones set cvB2CActivo='SI' "
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Activado!", Me.Page)
    End Sub

    Private Sub btnDesactivarB2C_Click(sender As Object, e As EventArgs) Handles btnDesactivarB2C.Click
        ssql = "UPDATE config.parametrizaciones set cvB2CActivo='NO' "
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Desactivado!", Me.Page)
    End Sub
End Class
