
Imports System.IO
Imports System.Data
Imports System.Drawing.Printing
Imports mercadopago
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Net.Http
Imports Newtonsoft
Imports Newtonsoft.Json
Imports System.Net
Imports System.Security.Cryptography
Imports System.Data.OleDb

Partial Class Playground
    Inherits System.Web.UI.Page

    'Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    '    Dim sComando As String
    '    sComando = "<script type='text/javascript'> var opciones='left=100,top=100,width=250,height=350';window.open('vistaPrevia.aspx','Ventana',opciones);</script> "
    '    Response.Write(sComando)
    'End Sub


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim ssql As String = ""
        Dim objdatos As New Cls_Funciones

        'ssql = "SELECT * FROM OITM"
        'Dim dtdatos As New DataTable
        'dtdatos = objdatos.fnEjecutarConsultaSAP(ssql)
        'objdatos.Mensaje(dtdatos.Rows.Count & " registros", Me.Page)
        'lblMensaje.Text = dtdatos.Rows.Count & " registros"



        Dim sHTML As String
        sHTML = fnCargaCategoriasHTML("Productos")

        ssql = "Delete config.HTML where cvTipo='Categorias'"
        objdatos.fnEjecutarInsert(ssql)
        Try
            Dim tfile = New FileInfo("C:\Users\durquia2.STOPBASICO\Documents\pruebas.txt")
            Dim fichero As StreamWriter = tfile.CreateText()
            fichero.WriteLine(sHTML)
            fichero.Close()
        Catch ex As Exception

        End Try


        txtMenu1.Text = sHTML
        ssql = "INSERT INTO config.HTML(cvTipo,cvHTML)VALUES('Categorias','" & sHTML.Replace("'", "@") & "') "
        objdatos.fnEjecutarInsert(ssql)


        ssql = "Delete config.HTML where cvTipo='Categorias-lateral'"
        objdatos.fnEjecutarInsert(ssql)

        sHTML = fnCargaCategoriasLateral()
        txtmenu2.Text = sHTML
        ssql = "INSERT INTO config.HTML(cvTipo,cvHTML)VALUES('Categorias-lateral','" & sHTML.Replace("'", "@") & "') "
        objdatos.fnEjecutarInsert(ssql)


        sHTML = fnCargaCategoriasHTMLResponsive("productos")
        txtmenuresponsive.Text = sHTML
        ssql = "Delete config.HTML where cvTipo='Categorias-responsive'"
        objdatos.fnEjecutarInsert(ssql)
        ssql = "INSERT INTO config.HTML(cvTipo,cvHTML)VALUES('Categorias-responsive','" & sHTML.Replace("'", "@") & "') "
        objdatos.fnEjecutarInsert(ssql)


        ssql = "UPDATE config.html  set cvHTML = REPLACE(CVHTML,'@','''')"
        objdatos.fnEjecutarInsert(ssql)

        'Dim oCompany As New SAPbobsCOM.Company
        'Dim sCardCode As String = ""
        'Dim objdatos As New Cls_Funciones
        'Try
        '    oCompany = objDatos.fnConexion_SAP
        '    If oCompany.Connected Then
        '        objdatos.fnLog("Conexion", "Todo bien")
        '    Else
        '        objdatos.fnLog("Conexion", "No conecta")
        '    End If

        'Catch ex As Exception
        'End Try
        '''Conectamos con mercado pago en SAndBox
        'Dim mp As MP = New MP("3215552140914099", "que5OV1ia0dBByAEAdEnoiLiqw6lcq9g")
        'mp.sandboxMode(True)
        'Dim accessToken = mp.getAccessToken()
        '' Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"",""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""}, ""quantity"": 1,""currency_id"":""MXN"",""unit_price"":" & lblTotal.Text.Replace("$", "") & "}]}")
        'Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & "100.00" & "}],""payer"": {""email"": ""CorreoComprador@gmail.com""},""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""},""auto_return"": ""approved""}")
        'Dim sComando As String
        'sComando = "<script type='text/javascript'> window.open('" & preference.Item("response")("sandbox_init_point") & "','_blank'); </script> "
        'Response.Write(sComando)

    End Sub

    Public Function fnCargaCategoriasHTMLResponsive(NombreCat As String) As String
        'Dim dtProductos As New DataTable
        'Dim sHtmlEncabezado As String = ""
        'Dim sHtmlBanner As String = ""

        'ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        'Dim dtPrimerNivel As New DataTable
        'dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        'If dtPrimerNivel.Rows.Count > 0 Then
        '    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
        '        ''Grupo Nativo de SAP
        '        ssql = objDatos.fnObtenerQuery("GrupoTodos")
        '    Else
        '        ''Traemos el distinct del campo en OITM
        '        ssql = objDatos.fnObtenerQuery("Categorias")
        '        ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

        '    End If
        '    Dim dtCategorias As New DataTable
        '    dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
        '    Dim sLinkPrimerNivel As String = ""
        '    Dim sLinkSegundoNivel As String = ""
        '    sHtmlEncabezado = sHtmlEncabezado & "  <li class='panel'>"
        '    sHtmlEncabezado = sHtmlEncabezado & "     <div class='panel-heading' role='tab' id='headingThree'>"
        '    sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' class='link-m-r' data-toggle='collapse' data-parent='#accordion' href='#collapseThree' aria-expanded='true' aria-controls='collapseThree'>" & NombreCat & "</a>"
        '    sHtmlEncabezado = sHtmlEncabezado & "     </div>"
        '    sHtmlEncabezado = sHtmlEncabezado & "     <div id='collapseThree' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingThree'>"
        '    sHtmlEncabezado = sHtmlEncabezado & "       <div class='panel-body'>"

        '    Dim iContador As Int16 = 5
        '    For i = 0 To dtCategorias.Rows.Count - 1 Step 1

        '        ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
        '        ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
        '        Dim dtCategoriaValida As New DataTable
        '        dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

        '        If dtCategoriaValida.Rows.Count > 0 Then
        '            sHtmlBanner = sHtmlBanner & "<a class='link-m-r' href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' >" & dtCategorias.Rows(i)(0) & "</a> "
        '            iContador = iContador + 1
        '        End If

        '    Next
        '    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

        '    sHtmlEncabezado = sHtmlEncabezado & " </div></div></li>"
        '    Return sHtmlEncabezado


        'End If

        ''Cargamos menu Responsive HEADER
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

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
                sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "'>" & sValorPintar & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#collapse" & (i + 1) & "' class='collapsed'></a>"
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

                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objdatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    '  'objDatos.fnlog("Segundo nivel", ssql.Replace("'", ""))

                    ' 'objDatos.fnlog("Evaluar tercer nivel", ssql.Replace("'", ""))
                    If iExisteTercerNivel = 1 Then

                        '     'objDatos.fnlog("Query tercer nivel", sQueryTercer.Replace("'", ""))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objdatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = sHtmlBanner & " <div id='collapse" & (i + 1) & "' class='panel-collapse collapse '> "
                    sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "

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
                            sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 no-padding'> "
                            sHtmlBanner = sHtmlBanner & " <a data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
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

                            ' 'objDatos.fnlog("Query tercer nivel: " & sLinkPrimerNivel & " " & dtSubCategoria.Rows(x)(0), sQueryTercer.Replace("'", ""))

                            ''Preparamos la estructura para el tercer nivel
                            sHtmlBanner = sHtmlBanner & " <div class='panel-group' id='accordion-" & (i + 1) & "'> "
                            sHtmlBanner = sHtmlBanner & "  <div class='panel panel-gtk'> "
                            sHtmlBanner = sHtmlBanner & "    <div class='panel-heading'> "
                            sHtmlBanner = sHtmlBanner & "      <h4 class='panel-title'> "
                            sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_") & "' class='collapsed'></a>"
                            sHtmlBanner = sHtmlBanner & "      </h4>"
                            sHtmlBanner = sHtmlBanner & "    </div>"

                            ''Leemos el tercer nivel

                            sHtmlBanner = sHtmlBanner & " <div id='collapse-" & sValorPintar.Replace(" ", "_") & "' class='panel-collapse collapse '>"
                            sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "

                            Dim dtTercer As New DataTable
                            dtTercer = objdatos.fnEjecutarConsultaSAP(sQueryTercer)


                            For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                '    'objDatos.fnlog("tercer nivel", dtTercer.Rows(y)(0))
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
                                    sHtmlBanner = sHtmlBanner & "  <a data-grafica='20' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&param2=" & dtSubCategoria.Rows(x)(0) & "&param3=" & dtTercer.Rows(y)(0) & "'>" & sValorPintar & "</a>"
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
                If Session("RazonSocial") <> "" Then
                    ssql = objdatos.fnObtenerQuery("CategoriasB2B")
                Else
                    ssql = objdatos.fnObtenerQuery("Categorias")
                End If
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
                ''Nos traemos el nivel 2 de la categoría seleccionada
                ssql = objdatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    If objdatos.fnObtenerDBMS = "HANA" Then
                        objdatos.fnLog("Categorias Cat", "HANA 1")
                        ssql = ssql.Replace("[%1]", "TO_VARCHAR(T0.""" & dtPrimerNivel.Rows(0)("cvCampoSAP") & """)=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sCampoPrimerNivel = "TO_VARCHAR(T0.""" & dtPrimerNivel.Rows(0)("cvCampoSAP") & """)=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    End If


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
                        If objdatos.fnObtenerDBMS = "HANA" Then
                            ssql = ssql.Replace("[%0]", "TO_VARCHAR(T0.""" & dtSegundoNivel.Rows(0)("cvCampoSAP") & """)")
                        Else
                            ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                        End If

                    End If

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
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

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
                'objDatos.fnlog("Primer Cats", ssql.Replace("'", ""))
            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objdatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""

            If Session("Page") = "catalogo.aspx" Then
                sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle active' href='catalogo.aspx'>" & NombreCat & "<b class='caret'></b></a>"
            Else
                sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a data-link='catalogo.aspx?Cat=" & NombreCat & "'  data-toggle='dropdown' class='dropdown-toggle' href='catalogo.aspx'>" & NombreCat & "<b class='caret'></b></a>"
            End If

            Dim sHTMLConHijos As String = ""

            sHtmlEncabezado = sHtmlEncabezado & "   <ul class='dropdown-menu' style='display: none;'>"
            sHtmlEncabezado = sHtmlEncabezado & "    <li>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div class='yamm-content'>"
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "       <div  class='col-sm-12 no-padding'>"
            If CStr(objdatos.fnObtenerCliente).ToUpper.Contains("SALAMA") Then
                sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-12'>"
            Else
                sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-6'>"
            End If

            sHtmlEncabezado = sHtmlEncabezado & "         <ul class='nav #SUBMENU#'>"
            Dim iContador As Int16 = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                Dim sValorPintar As String = ""
                ' 'objDatos.fnlog("Cats", "entra")
                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objdatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objdatos.fnEjecutarConsultaSAP(ssql)
                    'objDatos.fnlog("Cats", ssql.Replace("'", ""))
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                '  'objDatos.fnlog("Cats 1 ", dtCategorias.Rows(i)(0))

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objdatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(1) & "'>" & sValorPintar & "</span><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> "
                    'sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(1) & "'>" & sValorPintar & "</span> SINHIJOS" & iContador
                Else
                    sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'>" & sValorPintar & "</span><a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> "
                    'sHtmlBanner = sHtmlBanner & "<li> <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'>" & sValorPintar & "</span> SINHIJOS" & iContador
                End If

                sHtmlBanner = sHtmlBanner & "</li> "
                iContador = iContador + 1


            Next

            ''Aqui las subcategorias
            ''Categorias Especiales
            '  sHtmlBanner = sHtmlBanner & fnCategoriasEspecial()
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"

            If CStr(objdatos.fnObtenerCliente).ToUpper.Contains("SALAMA") Then
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-12'>" 'El que agregué
            Else
                sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-6'>" 'El que agregué
            End If

            sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-content'>" 'El que agregué


            iContador = 1
            Dim sCampoPrimerNivel As String = ""
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objdatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                End If
                sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='Cat-" & (iContador) & "'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-12'>" 'class='col-xs-4'
                sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"
                sHtmlEncabezado = sHtmlEncabezado & "    <div class='panel-group' id='accordion-bebidas' role='tablist' aria-multiselectable='true'>"

                ''Nos traemos el nivel 2 de la categoría seleccionada
                If Session("RazonSocial") <> "" Then
                    ssql = objdatos.fnObtenerQuery("Categorias-detB2B")
                Else
                    ssql = objdatos.fnObtenerQuery("Categorias-det")
                End If
                ssql = objdatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    If objdatos.fnObtenerDBMS = "HANA" Then

                        ssql = ssql.Replace("[%1]", "T0.""" & dtPrimerNivel.Rows(0)("cvCampoSAP") & """=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    End If

                    sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If

                Dim iExisteTercerNivel As Int16 = 0
                Dim sQueryTercer As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =3"
                Dim dtTercerNivel As New DataTable
                dtTercerNivel = objdatos.fnEjecutarConsulta(sQueryTercer)
                If dtTercerNivel.Rows.Count > 0 Then
                    iExisteTercerNivel = 1
                End If

                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objdatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    sHtmlEncabezado = sHtmlEncabezado.Replace("#SUBMENU#", "nav-tabs tabs-left")

                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    'objDatos.fnlog("Categorias_det", ssql.Replace("'", ""))
                    dtSubCategoria = objdatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""

                    Dim sValorPintar As String = ""


                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & iContador, "<a href='#Cat-" & iContador & "' data-link='catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "'  data-toggle='tab' aria-expanded='false'>" & "</a> ")
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            If CStr(dtSegundoNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                If Session("RazonSocial") <> "" Then
                                    ssql = objdatos.fnObtenerQuery("CampoUsuarioNiv2B2B")
                                Else
                                    ssql = objdatos.fnObtenerQuery("CampoUsuarioNiv2")
                                End If

                                ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                                Dim dtValor As New DataTable
                                dtValor = objdatos.fnEjecutarConsultaSAP(ssql)
                                ' 'objDatos.fnlog("CampoUsuarioNiv2", ssql.Replace("'", ""))
                                If dtValor.Rows.Count > 0 Then
                                    sValorPintar = dtValor.Rows(0)(0)
                                Else
                                    sValorPintar = dtSubCategoria.Rows(x)(0)
                                End If
                            Else
                                sValorPintar = dtSubCategoria.Rows(x)(0)
                            End If
                            sHtmlBanner = sHtmlBanner & "<div class='panel panel-default'>"
                            sHtmlBanner = sHtmlBanner & " <div class='panel-heading' role='tab' id='heading-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "'>"
                            sHtmlBanner = sHtmlBanner & "  <h4 class='panel-title'> "

                            sHtmlBanner = sHtmlBanner & "   <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span><a role='button' data-toggle='collapse' data-link='Catalogo.aspx?cat=" & sLinkPrimerNivel & "&Param2=" & dtSubCategoria.Rows(x)(0) & "' data-parent='#accordion-bebidas' href='#collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "' aria-expanded='true' aria-controls='collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "'>"
                            sHtmlBanner = sHtmlBanner & ""
                            sHtmlBanner = sHtmlBanner & "   </a>"
                            sHtmlBanner = sHtmlBanner & "  </h4>"
                            sHtmlBanner = sHtmlBanner & " </div>"

                            If iExisteTercerNivel = 0 Then
                                sHtmlBanner = sHtmlBanner & " </div>" 'Ya solamente cerramos el div
                            Else
                                If Session("RazonSocial") <> "" Then
                                    sQueryTercer = objdatos.fnObtenerQuery("Categorias-TerceroB2B")
                                Else
                                    sQueryTercer = objdatos.fnObtenerQuery("Categorias-Tercero")
                                End If

                                If dtTercerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                                    sQueryTercer = sQueryTercer.Replace("[%0]", "ItmsGrpNam")
                                Else
                                    sQueryTercer = sQueryTercer.Replace("[%0]", "ISNULL(T0." & dtTercerNivel.Rows(0)("cvCampoSAP") & ",'')")
                                    sQueryTercer = sQueryTercer.Replace("[%1]", sCampoPrimerNivel)

                                End If
                                sQueryTercer = sQueryTercer.Replace("[%2]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtSubCategoria.Rows(x)(0) & "'")
                                '    'objDatos.fnlog("Query tercer nivel: " & sLinkPrimerNivel & " " & dtSubCategoria.Rows(x)(0), sQueryTercer.Replace("'", ""))


                                Dim dtTercer As New DataTable
                                dtTercer = objdatos.fnEjecutarConsultaSAP(sQueryTercer)


                                ''Preparamos la estructura para el tercer nivel

                                sHtmlBanner = sHtmlBanner & " <div id='collapse-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='heading-" & CStr(dtSubCategoria.Rows(x)(0)).Replace(" ", "-").Replace(",", "-").Replace("/", "") & "'> "
                                sHtmlBanner = sHtmlBanner & " <div class='panel-body'> "

                                For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                    sValorPintar = ""
                                    If CStr(dtTercerNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then

                                        If Session("RazonSocial") <> "" Then
                                            ssql = objdatos.fnObtenerQuery("CampoUsuarioNiv3B2B")
                                        Else

                                            ssql = objdatos.fnObtenerQuery("CampoUsuarioNiv3")
                                        End If

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



                Else

                    sHtmlEncabezado = sHtmlEncabezado.Replace("#SUBMENU#", "")
                End If

                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                iContador = iContador + 1
                'sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                sHtmlEncabezado = sHtmlEncabezado & "</div></ul></div></div>"


            Next
            Dim iValor As Int16 = 99
            For a = 1 To 99 Step 1

                sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & iValor, "")
                iValor = iValor - 1
                'For b = 0 To 9 Step 1
                '    sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & a & b, "")
                'Next

                'sHtmlEncabezado = sHtmlEncabezado.Replace("SINHIJOS" & a, "")


            Next

            'For b = 0 To 9 Step 1
            '    sHtmlEncabezado = sHtmlEncabezado.Replace(b, "")
            'Next





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
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim oCompany As New SAPbobsCOM.Company
        Dim sCardCode As String = ""
        Dim objdatos As New Cls_Funciones
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then
                objdatos.fnLog("Conexion", "Todo bien")
                lblMensaje.Text = "Conecta"
            Else
                objdatos.fnLog("Conexion", "No conecta")
                lblMensaje.Text = "NO Conecta: " & oCompany.GetLastErrorDescription
            End If

        Catch ex As Exception
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim ssql As String
        Dim objdatos As New Cls_Funciones

        ssql = "truncate table tempArt "
        objdatos.fnEjecutarInsert(ssql)

        ssql = "SELECT U_Img_1 ,ItemCode,ItemName  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod WHERE T0.SellItem='Y' AND T0.ValidFor='Y' AND U_MxStyle ='Y' AND T0.U_TIE_LINEA is not null and ISNULL (T0.U_Tie_ANO,2016) in (SELECT U_Ano FROM [Delta]..[@TEMPORADAS_PORTAL] ) AND U_TIE_TEMPORADA  IN (SELECT U_Temporada  FROM [Delta]..[@TEMPORADAS_PORTAL]) order by ItemCode"
        Dim dtArt As New DataTable
        dtArt = objdatos.fnEjecutarConsultaSAP(ssql)
        For i = 0 To dtArt.Rows.Count - 1 Step 1
            If Not File.Exists(Server.MapPath("~") & "\" & CStr(dtArt.Rows(i)("U_Img_1")).Replace("/", "\")) Then
                ''Lo registramos
                ssql = "INSERT INTO tempArt (cvItemCode,cvItemName) VALUES (" & "'" & dtArt.Rows(i)("ItemCode") & "'," & "'" & dtArt.Rows(i)("ItemName") & "')"
                objdatos.fnEjecutarInsert(ssql)
            End If
        Next
    End Sub
    Protected Sub btnMoneta_Click(sender As Object, e As EventArgs) Handles btnMoneta.Click
        Dim iDif As Int16
        Dim sRellenar As String = ""
        Dim sDocNum As String = "785689"
        iDif = 35 - sDocNum.Length - txtCodMoneta.Text.Length

        For v = 1 To iDif Step 1
            sRellenar = sRellenar & "0"
        Next
        txtOrigTransId.Text = sRellenar & sDocNum & txtCodMoneta.Text

        Dim dia As String = ""
        Dim mes As String = ""
        Dim Año As String = ""
        If Now.Date.Day.ToString.Length > 1 Then
            dia = Now.Date.Day
        Else
            dia = "0" & Now.Date.Day
        End If

        If Now.Date.Month.ToString.Length > 1 Then
            mes = Now.Date.Month
        Else
            mes = "0" & Now.Date.Month
        End If
        Año = Now.Date.Year.ToString.Substring(Now.Date.Year.ToString.Length - 2, 2)
        txtDate.Text = dia & mes & Año
        sRellenar = ""
        iDif = 0

        If sDocNum.Length >= 6 Then
            txtAudit.Text = sDocNum.Substring(0, 6)
        Else

            iDif = 6 - sDocNum.Length

            For v = 1 To iDif Step 1
                sRellenar = sRellenar & "0"
            Next
            txtAudit.Text = sRellenar & sDocNum

        End If

        Dim ssql As String = ""
        Dim objdatos As New Cls_Funciones
        ssql = "SELECT ISNULL(cvMerchantId,'') FROM config.proveedores_pago"
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)
        If dtLiga.Rows.Count > 0 Then
            txtMerchantId.Text = dtLiga.Rows(0)(0)
        End If
    End Sub

    Private Sub btnCrystal_Click(sender As Object, e As EventArgs) Handles btnCrystal.Click
        fndescargaPDF(526)
    End Sub


    Public Sub fndescargaPDF(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        Dim ssql As String = ""
        Dim objdatos As New Cls_Funciones

        ssql = "SELECT DocEntry FROM OQUT where docnum=" & "'" & DocEntry & "'"
        Dim dtDocEntry As New DataTable
        dtDocEntry = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtDocEntry.Rows.Count > 0 Then
            DocEntry = dtDocEntry.Rows(0)(0)
        End If

        Try
            objdatos.fnLog("Al imprimir", "Antes RPT")
            reporte.Load(Server.MapPath("~") & "\Pedido.rpt")


            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                reporte.SetParameterValue("DocKey@", DocEntry)
                reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

                crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
                crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
                crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
                crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


            End If



            objdatos.fnLog("Al imprimir", "Sale de asignarle la BD")
            CrTables = reporte.Database.Tables
            For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            objdatos.fnLog("Al imprimir", "LogInfo")
            ' reporte.Refresh()

            objdatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
            reporte.SetParameterValue("DocKey@", DocEntry)
            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "PED-" & DocEntry)
            reporte.Dispose()
        Catch ex As Exception
            objdatos.fnLog("Al imprimir ex ", ex.Message.Replace("'", ""))
        End Try


        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        ''Enviamos correo
        Dim text As String
        Dim ssql As String = ""
        Dim objdatos As New Cls_Funciones
        Dim sTablaDetalle As String = "RDR1"
        If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
            text = MensajeHTML(Server.MapPath("~") & "\correo_A_B2B.html")
        Else
            text = MensajeHTML(Server.MapPath("~") & "\correo_A.html")
        End If

        Dim sDestinatario As String = ""
        Dim sDocnum As String = "2262131"
        ''Obtenemos el nombre de la empresa
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente2 As New DataTable
        dtcliente2 = objdatos.fnEjecutarConsulta(ssql)

        objdatos.fnLog("Confirmacion-cliente", dtcliente2.Rows(0)(0))

        text = text.Replace("{nombrecliente}", dtcliente2.Rows(0)(0))
        text = text.Replace("{enviara}", "")
        text = text.Replace("{totalDoc}", 100)
        text = text.Replace("{direccionenvio}", "" & "</br> " & Session("Comentarios"))
        text = text.Replace("{metodoenvio}", "")
        text = text.Replace("{numpedido}", sDocnum)
        text = text.Replace("{fechapedido}", Now.Date.ToShortDateString)

        ssql = "SELECT  itemCode as cvItemCode, Dscription as cvItemName,Quantity as cfCantidad,Price as cfPrecio,DiscPrcnt as cfDescuento,VatSum ,LineTotal  FROM " & sTablaDetalle & " WHERE docEntry=" & "'667658'"
        Dim dtPartidasSAP As New DataTable
        dtPartidasSAP = objdatos.fnEjecutarConsultaSAP(ssql)
        objdatos.fnLog("LineasCorreo", dtPartidasSAP.Rows.Count)
        ''Ahora las líneas
        If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
            ''En B2B con PedidoCot, quitamos las líneas
            text = text.Replace("{lineas}", "")
        Else
            text = text.Replace("{lineas}", fnGeneraHTMLPartidas(dtPartidasSAP))
        End If

        Dim sIVA As Double = 0
        Dim sSubTotal As Double = 0
        For iContLineas As Int16 = 0 To dtPartidasSAP.Rows.Count - 1 Step 1
            sIVA = sIVA + dtPartidasSAP.Rows(iContLineas)("VatSum")
            sSubTotal = sSubTotal + dtPartidasSAP.Rows(iContLineas)("LineTotal")
        Next

        text = text.Replace("{totImpuesto}", sIVA)
        text = text.Replace("{subtotal}", sSubTotal)
        text = text.Replace("{totalconImpuesto}", sSubTotal + sIVA)
        objdatos.fnEnviarCorreo("jpena@tie.com.mx", text, sDocnum & "- Nueva Compra Registrada")

    End Sub
    Dim ssql As String = ""
    Dim objdatos As New Cls_Funciones
    Protected Function MensajeHTML(ArchivoHTML As [String]) As String
        Dim Cuerpo As [String] = ""
        Try
            Dim File As New System.IO.StreamReader(ArchivoHTML)

            Dim Line As [String]
            Dim text As String = System.IO.File.ReadAllText(ArchivoHTML)

            Cuerpo = text

            File.Close()
        Catch ex As Exception
            objdatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function
    Public Function fnGeneraHTMLPartidas(dtPArtidas As DataTable) As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If
        'sLigaSitio 
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0

        Dim sIVA As Double = 0

        Try
            For x = 0 To dtPArtidas.Rows.Count - 1 Step 1
                objdatos.fnLog("Correo B2B", dtPArtidas.Rows.Count)
                sHtmlBanner = sHtmlBanner & " <tr>"
                '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                If dtCamposPlantilla.Rows.Count > 0 Then
                    Dim sCampos As String = ""
                    For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                        ssql = objdatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtPArtidas.Rows(x)("cvItemCode") & "'")
                        Dim dtGeneral As New DataTable
                        dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                        If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                            sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;vertical-align: middle;text-align:center;'> "
                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-width:50px;max-height:100px'>"
                            sHtmlBanner = sHtmlBanner & "</td>"
                        Else
                            If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                If sCampos = "" Then
                                    ''Si es el primer valor que va a enlazar, lo ponemos en strong
                                    sCampos = sCampos & "<strong style='font-size: 13px;color:#000000;'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</strong> <br>"
                                Else
                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                End If

                            Else
                                '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                            End If


                        End If
                    Next
                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:left;font-size: 10px;'>" & sCampos & "</td>"

                End If
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & CDbl(dtPArtidas.Rows(x)("cfPrecio")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & CDbl(dtPArtidas.Rows(x)("cfCantidad")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio"))).ToString(" ###,###,###.#0") & "</td>"

                sHtmlBanner = sHtmlBanner & "</tr>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                sSubTotal = sSubTotal + (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio")))  'dtPartidas.Rows(i)("cfPrecio")

                sIVA = sIVA + CDbl(dtPArtidas.Rows(x)("cfPrecio"))
                ''Aqui van los botones de Action Cart
                'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                ''sHtmlBanner = sHtmlBanner & "</div>"

                'sHtmlBanner = sHtmlBanner & " </div> "


            Next



            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try
        Return sHtmlEncabezado
    End Function

    Private Sub btnPaqueteria_Click(sender As Object, e As EventArgs) Handles btnPaqueteria.Click
        If Session("UsaPaqueteria") = "SI" Then
            ''Cachamos el cotizacion_request
            Dim Cotizacion As New CotizacionRequest
            Dim objPaq As New Cls_Paqueteria
            Dim FolioPedido As String = "999"
            Dim GuiaSolicitada As String = ""
            Cotizacion = Session("CotizacionRequest")




            ''Obtenemos el consecutivo de la tabla de control
            Dim ssql As String = ""
            Dim sIdConceptoPaqueteria As String = ""
            sIdConceptoPaqueteria = CStr(Session("MetodoEnvio")).Substring(0, 1)
            ssql = "SELECT ciFolioSiguiente FROM config.Folios_Paqueteria "
            ssql = "SELECT ciFolioSiguiente,cvCuenta FROM config.Folios_Paqueteria where   " & "'" & CInt(Session("PesoPaquete")) & "' <= ciPeso AND ciConcepto='" & sIdConceptoPaqueteria & "' order by ciPeso "
            objdatos.fnLog("Paquete playg", ssql.Replace("'", ""))
            Dim dtFolio As New DataTable
            dtFolio = objdatos.fnEjecutarConsulta(ssql)
            If dtFolio.Rows.Count > 0 Then
                GuiaSolicitada = dtFolio.Rows(0)(0)
                Cotizacion.guia = "0" & GuiaSolicitada
            Else
                Cotizacion.guia = txtguia.Text ''consecutivo
            End If
            objdatos.fnLog("Genera guia", Cotizacion.guia & " Paquetes: " & Cotizacion.paquetes.Count)
            Dim Guias = objPaq.FnCrearModeloGuia(Cotizacion, "2")
            Dim GuiaRespuesta = objPaq.FnCrearGuia(Guias, Session("BearerToken"))
            objPaq.FnRespuestaGuia(GuiaRespuesta, FolioPedido, Cotizacion.guia, sIdConceptoPaqueteria)

        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            'objdatos.fnLog("CP", "evento")
            Dim CP = "45601"
            Dim url As String = "http://api-sepomex.hckdrk.mx/query/info_cp/" & CP & "?type=simplified"
            Dim oRequest = WebRequest.Create(url)
            Dim oResponse As WebResponse = oRequest.GetResponse()
            Dim sr As StreamReader = New StreamReader(oResponse.GetResponseStream())
            Dim oDatos = JsonConvert.DeserializeObject(Of RespuestaCP)(sr.ReadToEnd().Trim())
            If oDatos.error Then
                objdatos.Mensaje("error", Me.Page)
            Else
                Dim coloniasMensaje As String = ""
                For Each colonias As String In oDatos.response.asentamiento
                    coloniasMensaje = coloniasMensaje & colonias & ","
                Next
                objdatos.Mensaje(coloniasMensaje, Me.Page)
                '     objdatos.fnLog("CP", "CP Correcto: " & coloniasMensaje)

            End If
        Catch ex As Exception
            objdatos.fnLog("CP", "error ex: " & ex.Message.Replace("'", ""))
        End Try
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Dim contenido As String = txtmenu2.Text
            Dim hash As String = "PERJ840518AB0"
            Dim des As New TripleDESCryptoServiceProvider 'Algorithmo TripleDES
            Dim hashmd5 As New MD5CryptoServiceProvider 'objeto md5
            '     objdatos.fnLog("Desencrip", Contenido)
            Dim Resultado = ""
            If Trim(contenido) = "" Then
                Resultado = ""
            Else
                des.Key = hashmd5.ComputeHash((New UnicodeEncoding).GetBytes(hash))
                des.Mode = CipherMode.ECB
                Dim desencrypta As ICryptoTransform = des.CreateDecryptor()
                Dim buff() As Byte = Convert.FromBase64String(contenido)
                Resultado = UnicodeEncoding.ASCII.GetString(desencrypta.TransformFinalBlock(buff, 0, buff.Length))
            End If
            txtmenu2.Text = Resultado
            '   objdatos.fnLog("Desencrip resultado", Resultado)
            objdatos.Mensaje(Resultado, Me.Page)
        Catch __unusedException1__ As Exception
            objdatos.Mensaje(__unusedException1__.Message, Me.Page)
        End Try
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        txtmenu2.Text = Encriptar(txtMenu1.Text)
    End Sub

    Private Function Encriptar(ByVal texto As String) As String
        Dim myKey As String = "PERJ840518AB0"
        Dim des As New TripleDESCryptoServiceProvider 'Algorithmo TripleDES
        Dim hashmd5 As New MD5CryptoServiceProvider 'objeto md5
        If Trim(texto) = "" Then
            Encriptar = ""
        Else
            des.Key = hashmd5.ComputeHash((New UnicodeEncoding).GetBytes(myKey))
            des.Mode = CipherMode.ECB
            Dim encrypt As ICryptoTransform = des.CreateEncryptor()
            Dim buff() As Byte = UnicodeEncoding.ASCII.GetBytes(texto)
            Encriptar = Convert.ToBase64String(encrypt.TransformFinalBlock(buff, 0, buff.Length))
        End If
        Return Encriptar
    End Function

    Private Sub btnImportarArticulos_Click(sender As Object, e As EventArgs) Handles btnImportarArticulos.Click
        Dim ssql As String = ""
        ssql = "select distinct Articulo from tablaModelos6 where html is null"
        Dim dtDatos As New DataTable
        dtDatos = objdatos.fnEjecutarConsulta(ssql)

        For i = 0 To dtDatos.Rows.Count - 1 Step 1
            Dim sHTML As String = ""
            sHTML = objdatos.fnCreaFichaProducto(dtDatos.Rows(i)("Articulo"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
            ssql = "UPDATE tablaModelos6 set HTML='" & sHTML.Replace("'", "@") & "' WHERE articulo=" & "'" & dtDatos.Rows(i)("Articulo") & "'"
            objdatos.fnEjecutarInsert(ssql)

        Next
    End Sub

    Private Sub Playground_Load(sender As Object, e As EventArgs) Handles Me.Load
        '   objdatos.Mensaje(, Me.Page)
    End Sub

    Private Sub btnValidar_Click(sender As Object, e As EventArgs) Handles btnValidar.Click
        Dim rfcReceptor As String = "SEA940302HL5"
        Dim rfcEmisor As String = "LCM860204I19"
        Dim UUID As String = "B5636C71-F3FE-48E1-97F8-B14F45B297EA"
        Dim ImporteFactura As String = "1706.12"

        Dim Total As String
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        Dim oConsulta As New ConsultaCFDIService.ConsultaCFDIService
        Dim oAcuse As New ConsultaCFDIService.Acuse()


        rfcEmisor = rfcEmisor.Replace("&", "&amp;")
        rfcReceptor = rfcReceptor.Replace("&", "&amp;")


        oAcuse = oConsulta.Consulta("?re=" & rfcEmisor & "&rr=" & rfcReceptor & "&tt=" & ImporteFactura & "&id=" & UUID) 'EC609EC1-5F63-4333-A2B8-2EDC10B68075"

        Dim valido As String

        If oAcuse.CodigoEstatus <> "S - Comprobante obtenido satisfactoriamente." Or oAcuse.Estado = "Cancelado" Then
            valido = "NO"
        Else
            valido = "SI"

        End If

        'If FileUpload1.HasFile Then
        '    Dim archivo As String
        '    archivo = Server.MapPath("~") & FileUpload1.FileName
        '    lblMensaje.Text = archivo
        '    FileUpload1.SaveAs(archivo)

        '    Dim conStr As String = ""
        '    If Path.GetExtension(FileUpload1.FileName).Trim() = ".xls" Then
        '        conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & archivo & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
        '    ElseIf Path.GetExtension(FileUpload1.FileName).Trim() = ".xlsx" Then
        '        conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & archivo & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
        '    End If

        '    Dim query As String = "SELECT * FROM [Hoja1$]"
        '    Dim conn As OleDbConnection
        '    conn = New OleDbConnection(conStr)

        '    If conn.State = ConnectionState.Closed Then
        '        conn.Open()
        '    End If

        '    Dim cmd As OleDbCommand = New OleDbCommand(query, conn)
        '    Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd)
        '    Dim ds As DataSet = New DataSet()
        '    da.Fill(ds)
        '    lblMensaje.Text = ds.Tables(0).Rows.Count
        'End If

    End Sub

    Private Sub btnSubir_Click(sender As Object, e As EventArgs) Handles btnSubir.Click
        If fuExcel.HasFile Then
            Dim archivo As String
            archivo = Server.MapPath("~") & "\" & fuExcel.FileName

            fuExcel.SaveAs(archivo)

            Dim conStr As String = ""
            If Path.GetExtension(fuExcel.FileName).Trim() = ".xls" Then
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & archivo & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
            ElseIf Path.GetExtension(fuExcel.FileName).Trim() = ".xlsx" Then
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & archivo & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
            End If

            Dim query As String = "SELECT * FROM [Hoja1$]"
            Dim conn As OleDbConnection
            conn = New OleDbConnection(conStr)

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim cmd As OleDbCommand = New OleDbCommand(query, conn)
            Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd)
            Dim ds As DataSet = New DataSet()
            da.Fill(ds)
            Dim dtDatos As New DataTable
            dtDatos = ds.Tables(0)

            For i = 0 To dtDatos.Rows.Count - 1 Step 1

                ssql = objdatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & dtDatos.Rows(i)(0) & "'")

                Dim dtItemName As New DataTable
                dtItemName = objdatos.fnEjecutarConsultaSAP(ssql)
                If dtItemName.Rows.Count = 0 Then
                    objdatos.fnLog("Carrito btnSubir", "no encontrado")
                Else
                    objdatos.fnLog("Carrito btnSubir", "subiendo: " & dtDatos.Rows(i)(0))
                    CargarCarritoTemplate(dtDatos.Rows(i)(1), dtDatos.Rows(i)(0))
                End If

                'Dim partida As New Cls_Pedido.Partidas
                'Session("Partidas").add(partida)
            Next
            objdatos.fnLog("Carrito btnSubir", "Termina")
            'Response.Redirect("catalogo.aspx")
        End If
    End Sub

    Public Function CargarCarritoTemplate(Cantidad As String, Articulo As String) As String

        HttpContext.Current.Session("AgregaCarrito") = "SI"
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones
        ' Session("Partidas") = New List(Of Cls_Pedido.Partidas)
        objDatos.fnLog("CargarCarritoP", "entra:" & Articulo)
        Dim dPrecioActual As Double = 0



        Try

            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(Articulo)
            If fDescuento = 0 Then
                fDescuento = objDatos.fnObtenerDescuentoEspecialDelta(Articulo)
            End If
            partida.Cantidad = Cantidad

            ' objDatos.fnLog("CargarCarrito cant:", Cantidad)
            ''Revisamos si hay que mostrar tallas y colores
            Dim sTallaColor As String = ""
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                    '    objDatos.fnLog("CargarCarrito talla color:", sTallaColor)
                    Articulo = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.Generico = Articulo
                    ''Cambiamos
                    partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                    partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))

                    partida.Descuento = fDescuento
                Else

                    partida.ItemCode = Articulo


                    If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then
                        If HttpContext.Current.Session("ListaPrecios") Is Nothing Then
                            dPrecioActual = objDatos.fnPrecioActual(Articulo)
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                        End If

                    Else
                        dPrecioActual = objDatos.fnPrecioActual(Articulo)

                    End If
                    objDatos.fnLog("CargarCarrito", "Antes de evaluar, cliente:" & Session("Cliente"))
                    If Session("Cliente") <> "" Then
                        ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                        ssql = ssql.Replace("[%0]", Session("Cliente"))
                        objDatos.fnLog("ListaPrecios", ssql.Replace("'", ""))
                        Dim dtLista As New DataTable
                        dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtLista.Rows.Count > 0 Then
                            HttpContext.Current.Session("ListaPrecios") = dtLista.Rows(0)(0)
                        Else
                            HttpContext.Current.Session("ListaPrecios") = "1"
                        End If
                        dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))

                        If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                            'B2B sin IVA
                            dPrecioActual = dPrecioActual / 1.16
                        End If

                        partida.Descuento = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                    End If


                    partida.Precio = dPrecioActual
                    partida.TotalLinea = partida.Cantidad * partida.Precio

                    objDatos.fnLog("CargarCarritoP", "Articulo" & Articulo & " " & dPrecioActual & " " & partida.Cantidad)


                End If
            End If

            ''Evaluamos el stock
            Dim existencia As Double = 0
            ''Existencia 
            ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
            Dim dtExistencia As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            objDatos.fnLog("existencia", ssql.Replace("'", ""))
            dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtExistencia.Rows.Count > 0 Then
                existencia = CDbl(dtExistencia.Rows(0)(0))
            End If
            Try
                For Each Partida2 As Cls_Pedido.Partidas In Session("Partidas")
                    If Partida2.ItemCode <> "BORRAR" Then
                        If Partida2.ItemCode = Articulo Then
                            existencia = existencia - Partida2.Cantidad
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try
            objDatos.fnLog("CargarCarritoP", "Articulo" & Articulo & " " & dPrecioActual & " " & partida.Cantidad & " existencia: " & existencia & " ex-cantidad:" & existencia - partida.Cantidad)

            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then


                    If existencia - Cantidad < 0 Then
                        HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("SALAMA") Then

                            HttpContext.Current.Session("ErrorExistencia") = "No hay existencia del artículo seleccionado." _
                                & " Si no encontraste disponibilidad, contacta a un representante en el teléfono: 449 205 0883 en nuestros horarios de servicio."
                            Exit Function
                        Else
                            If CInt(HttpContext.Current.Session("slpCode")) > 0 And CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Then
                                ''en vendedores dejamos pasar la existencia
                            Else
                                HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                                Exit Function
                            End If

                        End If


                    End If
                Else
                    If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("BOSS") Then
                        If existencia - Cantidad < 0 Then
                            HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo no se cuenta con toda la existencia para surtir la cantidad seleccionada, el tiempo de resurtido es de 7 días hábiles."
                        End If
                    End If


                End If
            End If

            objDatos.fnLog("Carga CarritoP Cliente:", HttpContext.Current.Session("Cliente"))
            objDatos.fnLog("Carga CarritoP ListaPrecios:", HttpContext.Current.Session("ListaPrecios"))
            If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") And HttpContext.Current.Session("Cliente") <> "" Then


                ''Posibles monedas en la lista de precios
                ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
                ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
                Dim dtMonedas As New DataTable
                ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                ssql = ssql.Replace("[%1]", "'" & HttpContext.Current.Session("ListaPrecios") & "'")
                dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
                Dim sMoneda As String = ""

                If dtMonedas.Rows.Count > 0 Then
                    sMoneda = dtMonedas.Rows(0)(0)
                    objDatos.fnLog("Carga carrito moneda:", sMoneda)
                    partida.Moneda = sMoneda
                    For Each PartidaMoneda As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                        If PartidaMoneda.ItemCode <> "BORRAR" Then
                            objDatos.fnLog("Carga carrito moneda-IF:", PartidaMoneda.Moneda)
                            If PartidaMoneda.Moneda <> "" Then

                                If PartidaMoneda.Moneda <> sMoneda Then
                                    HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo se quitará del carrito porque no se pueden combinar artículos en USD y MXP."
                                    Exit Function
                                End If

                            End If
                        End If

                    Next

                End If


            End If



            partida.ItemCode = Articulo
            Dim iLinea As Int16 = 0
            Try
                For Each PartidaCont As Cls_Pedido.Partidas In Session("Partidas")
                    iLinea = iLinea + 1
                Next
            Catch ex As Exception

            End Try

            partida.Linea = iLinea


            objDatos.fnLog("CargarCarritoP Linea ", iLinea)

            ''Ahora el itemName

            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & partida.ItemCode & "'")

            'If sTallaColor = "SI" Then
            '    ssql = ssql.Replace("[%0]", "'" & HttpContext.Current.Session("ItemCodeTallaColor") & "'")
            'Else
            '    ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            'End If



            If CDbl(HttpContext.Current.Session("Mts2")) > 0 Then
                partida.Mts2 = HttpContext.Current.Session("Mts2")
            End If


            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count = 0 Then
                partida.ItemName = "ND"
            Else
                partida.ItemName = dtItemName.Rows(0)(0)
            End If
            objDatos.fnLog("CargarCarrito ", "Add Partida: " & partida.ItemName)

            Session("Partidas").add(partida)

            objDatos.fnLog("CargarCarrito despues de add itemcode ", partida.ItemCode)
            '  Response.Redirect("carrito.aspx")
        Catch ex As Exception
            objDatos.fnLog("Error en carga", ex.Message)
        End Try


        ''una vez que cargamos al carrito, validamos si es STOP Catalogo, para ver si por la cantidad de prendas no tenemos que cargar seguro o flete
        Try
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                'Dim objLocal As New producto_interior
                'objLocal.fnAgregaFletesSeguros_StopCatalogo()
            End If
        Catch ex As Exception

        End Try




        Dim result As String = "Entró:" & Articulo

        Return result
    End Function


    Public Class ResponseCP
        Public Property cp As String
        Public Property asentamiento As List(Of String)
        Public Property tipo_asentamiento As String
        Public Property municipio As String
        Public Property estado As String
        Public Property ciudad As String
        Public Property pais As String
    End Class

    Public Class RespuestaCP
        Public Property [error] As Boolean
        Public Property code_error As Integer
        Public Property error_message As Object
        Public Property response As ResponseCP
    End Class
End Class
