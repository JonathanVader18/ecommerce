
Imports System.Data
Imports System.IO
Imports System.Web.Services
Imports System.CollectionSystem.Collections

Imports System.Reflection
Imports ASP

Partial Class Catalogo
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Public dPrecioActual As Double = 0

    Private Sub Catalogo_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("Page") = "catalogo.aspx"
        Session("TipoDBMS") = objDatos.fnObtenerDBMS()
        If Not IsPostBack Then
            MaintainScrollPositionOnPostBack = True
            Session("CatDelta") = ""
            If Session("itemEliminado") <> "" Then
                objDatos.Mensaje(Session("itemEliminado"), Me.Page)
                Session("itemEliminado") = ""
                Session("sesBuscar") = ""
            End If
            objDatos.fnLog("catalogo", "Antes filtro")
            fnCargaFiltros()
            Session("ContPaginas") = 0
            ''Cargamos los filtros
            ssql = "select ciIdRel,cvFiltro  from [config].[Filtros] where cvEstatus ='ACTIVO' "
            Dim dtFiltros As New DataTable
            dtFiltros = objDatos.fnEjecutarConsulta(ssql)
            ddlOrden.DataSource = dtFiltros
            ddlOrden.DataTextField = "cvFiltro"
            ddlOrden.DataValueField = "ciIdRel"
            ddlOrden.DataBind()

            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("MANIJ") Then  ''Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SUJEA") 
                objDatos.fnLog("catalogo", "entra IF")
                pnlOrdenarPor.Visible = False
                pnlBuscadorAIO.Visible = True
                pnlCategorias.Visible = False
                pnlTitulo.Visible = False
                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                    pnlSubCat.Visible = False
                End If


                If Not IsPostBack Then
                    Session("AIO_OPC") = ""
                    Session("AIO_Categoria") = ""
                    Session("AIO_SubLinea") = ""
                    Session("AIO_Marca") = ""
                    Session("AIO_Modelo") = ""
                    Session("AIO_AÑO") = ""
                    objDatos.fnLog("catalogo", "Antes fnCargaCatalogos")
                    If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                        fnCargaCatalogos_PMK()
                    Else
                        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Then
                            fnCargaCatalogosAIO_PMK()
                        End If

                    End If

                    If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SUJEAUT") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("MANIJAU") Then
                        objDatos.fnLog("catalogo", "Antes fnCargaCatalogos_MANIJ")
                        Session("MANIJ_OPC") = ""
                        Session("MANIJ_Categoria") = ""
                        Session("MANIJ_SubLinea") = ""
                        Session("MANIJ_Marca") = ""
                        Session("MANIJ_Modelo") = ""
                        Session("MANIJ_AÑO") = ""
                        fnCargaCatalogos_MANIJ()
                    End If

                    If Request.QueryString.Count > 0 Then
                        Session("sesBuscar") = ""
                        If Request.QueryString(0) = "Novedades" Then

                            fnCargaBusquedaAIO_PMK("Novedades")
                        End If
                    Else

                        objDatos.fnLog("catalogo", "Antes BusquedaMANIJ_Index")

                        If Session("BusquedaAIO_Index") = "SI" Then
                            fnCargaBusquedaAIO_PMK(Session("BusquedaAIO_Criterios"))

                        Else
                            If Session("BusquedaMANIJ_Index") = "SI" Then
                                objDatos.fnLog("catalogo", " BusquedaMANIJ_Index = SI")

                                fnCargaBusquedaMANIJ(Session("BusquedaAIO_Criterios"))
                            Else
                                objDatos.fnLog("catalogo", " BusquedaMANIJ_Index = NO")
                                If Session("sesBuscar") <> "" Then
                                    fnCargaBusqueda(Session("sesBuscar"))
                                Else
                                    ''Ver salida
                                    fnCargaBusqueda("")
                                End If
                            End If


                        End If


                    End If


                End If

            Else
                If Request.QueryString.Count = 0 And Session("sesBuscar") = "" Then
                    fnCargaCatalogo("", ddlOrden.SelectedItem.Text)
                Else
                    If Request.QueryString.Count > 0 Then
                        Session("sesBuscar") = ""


                    End If
                    If Session("sesBuscar") <> "" Then
                        fnCargaBusqueda(Session("sesBuscar"))
                        '   Session("sesBuscar") = ""
                    Else
                        Session("CatDelta") = Request.QueryString("Cat")
                        fnCargaCatalogo(Request.QueryString("Cat"), ddlOrden.SelectedItem.Text)
                    End If

                End If
                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                    pnlOrdenarPor.Visible = False
                    fnCargaCatalogosDelta()
                End If

            End If









            ''Revisamos si manejará filtros
            ssql = "SELECT * FROM config.ProductosFiltro where cvEstatus='ACTIVO'"
            Dim dtUsaFiltros As New DataTable
            dtUsaFiltros = objDatos.fnEjecutarConsulta(ssql)
            If dtUsaFiltros.Rows.Count = 0 Then
                pnlFiltros.Visible = False
            End If


        Else
            Session("CondicionFiltro1") = ""
            Session("CondicionFiltro2") = ""
            Session("CondicionFiltro3") = ""
            Session("CondicionFiltro4") = ""
            Session("CondicionFiltro5") = ""
            Session("CondicionFiltro6") = ""
            Session("CondicionFiltro7") = ""
            Session("CondicionFiltro8") = ""

            AddHandler chkLista_1.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_2.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_3.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_4.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_5.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_6.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_7.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged
            AddHandler chkLista_8.SelectedIndexChanged, AddressOf chkFiltro_SelectedIndexChanged


        End If

        ''Cargamos las categorias nivel 1 y su siguiente nivel
        ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
        Dim dtPintaCat As New DataTable
        dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
        If dtPintaCat.Rows.Count > 0 Then
            If dtPintaCat.Rows(0)("Menu") = "SI" Then
                fnCargaCategorias()
            Else
                pnlTitulo.Visible = False
            End If

        Else
            fnCargaCategorias()
        End If



    End Sub
    Public Sub fnCargaCatalogos_MANIJ()
        Dim fila As DataRow
        Dim sCondicion As String = ""

        Try
            ssql = objDatos.fnObtenerQuery("FiltraCategoria")
            ssql = ssql & "  " & sCondicion
            Dim dtCategoria As New DataTable
            dtCategoria = objDatos.fnEjecutarConsultaSAP(ssql)

            Dim dataView As New DataView(dtCategoria)
            dataView.Sort = "  itmsgrpnam ASC"
            Dim dataTableCat As DataTable = dataView.ToTable()

            fila = dataTableCat.NewRow
            fila("ItmsGrpNam") = "-TODOS-"
            fila("ItmsGrpNam") = "-TODOS-"
            dataTableCat.Rows.Add(fila)
            ddlCategoria.DataSource = dataTableCat
            ddlCategoria.DataTextField = "ItmsGrpNam"
            ddlCategoria.DataValueField = "ItmsGrpNam"
            ddlCategoria.DataBind()
            ddlCategoria.SelectedValue = "-TODOS-"




            ssql = objDatos.fnObtenerQuery("FiltraMarca")
            ssql = ssql & "  " & sCondicion
            Dim dtMarca As New DataTable
            dtMarca = objDatos.fnEjecutarConsultaSAP(ssql)


            dataView = New DataView(dtMarca)
            dataView.Sort = "  U_marca ASC"
            Dim dataTableMarca As DataTable = dataView.ToTable()

            fila = dataTableMarca.NewRow
            fila("U_marca") = "-TODOS-"
            fila("U_marca") = "-TODOS-"

            dataTableMarca.Rows.Add(fila)
            ddlMarca.DataSource = dataTableMarca
            ddlMarca.DataTextField = "U_marca"
            ddlMarca.DataValueField = "U_marca"
            ddlMarca.DataBind()
            ddlMarca.SelectedValue = "-TODOS-"

            ssql = objDatos.fnObtenerQuery("FiltraModelo")
            ssql = ssql & "  " & sCondicion
            Dim dtModelo As New DataTable
            dtModelo = objDatos.fnEjecutarConsultaSAP(ssql)

            dataView = New DataView(dtModelo)
            dataView.Sort = "  U_modelo ASC"
            Dim dataTableModelo As DataTable = dataView.ToTable()

            fila = dataTableModelo.NewRow
            fila("U_modelo") = "-TODOS-"
            fila("U_modelo") = "-TODOS-"

            dataTableModelo.Rows.Add(fila)
            ddlModelo.DataSource = dataTableModelo
            ddlModelo.DataTextField = "U_modelo"
            ddlModelo.DataValueField = "U_modelo"
            ddlModelo.DataBind()
            ddlModelo.SelectedValue = "-TODOS-"


            ' ssql = "select distinct cast(U_AnioDe as varchar) + '-' + cast(U_anioHasta as varchar) as U_Anio from [@MODELOS] "
            ssql = objDatos.fnObtenerQuery("FiltraAños")
            ssql = ssql & "  " & sCondicion
            Dim dtaño As New DataTable
            dtaño = objDatos.fnEjecutarConsultaSAP(ssql)


            ''Tomamos el año menor y el mayor para hacer un ciclo en el que muestre todos los años del rango
            Dim AñoMAX As Int16
            Dim AñoMIN As Int16
            AñoMAX = dtaño.Compute("MAX(U_AnioHasta)", "")
            AñoMIN = dtaño.Compute("MIN(U_AnioDe)", "")


            Dim dtAñoMostrar As New DataTable
            dtAñoMostrar.Columns.Add("U_Anio")

            For i = AñoMIN To AñoMAX Step 1
                fila = dtAñoMostrar.NewRow
                fila("U_Anio") = i
                dtAñoMostrar.Rows.Add(fila)
            Next



            fila = dtAñoMostrar.NewRow
            fila("U_Anio") = "-TODOS-"

            dtAñoMostrar.Rows.Add(fila)
            ddlAnio.DataSource = dtAñoMostrar
            ddlAnio.DataTextField = "U_Anio"
            ddlAnio.DataValueField = "U_Anio"
            ddlAnio.DataBind()
            ddlAnio.SelectedValue = "-TODOS-"
        Catch ex As Exception

        End Try




    End Sub
    Public Sub fnCargaCatalogosAIO_PMK()
        Dim ibandVieneIndex As Int16 = 0
        Dim fila As DataRow
        Dim sCondicion As String = ""
        ssql = "SELECT distinct T1.ItmsGrpNam  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod WHERE T0.SellItem='Y'  AND T0.validfor='Y' AND itemcode in( Select Distinct U_Articulo  from [@MODELOS]) "
        ssql = "SELECT  distinct GRUPO as ItmsGrpNam FROM SAP_Tienda..TablaModelos6 order by ItmsGrpNam "
        ssql = ssql & "  " & sCondicion
        Dim dtCategoria As New DataTable
        dtCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtCategoria.NewRow
        fila("ItmsGrpNam") = "-TODOS-"
        fila("ItmsGrpNam") = "-TODOS-"

        dtCategoria.Rows.Add(fila)
        ddlCategoria.DataSource = dtCategoria
        ddlCategoria.DataTextField = "ItmsGrpNam"
        ddlCategoria.DataValueField = "ItmsGrpNam"
        ddlCategoria.DataBind()

        If Session("CatBuscar") = "" Then
            ddlCategoria.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlCategoria.SelectedValue = Session("CatBuscar")
            Try
                Session("AIO_OPC") = "Categoria"
                Session("AIO_Categoria") = ddlCategoria.SelectedValue
                objDatos.fnLog("IndexChange Cat", ddlCategoria.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_categoria", ex.Message.Replace("'", ""))
            End Try
        End If

        ssql = "select distinct u_TIE_sublinea from OITM where u_TIE_sublinea is not null "
        ssql = "select distinct subgrupo as u_TIE_sublinea from sap_tienda..tablaModelos6 where subgrupo is not null order by u_TIE_sublinea "
        ssql = ssql & "  " & sCondicion
        Dim dtsubcategoria As New DataTable
        dtsubcategoria = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtsubcategoria.NewRow
        fila("u_TIE_sublinea") = "-TODOS-"
        fila("u_TIE_sublinea") = "-TODOS-"

        dtsubcategoria.Rows.Add(fila)
        ddlSubcategoria.DataSource = dtsubcategoria
        ddlSubcategoria.DataTextField = "u_TIE_sublinea"
        ddlSubcategoria.DataValueField = "u_TIE_sublinea"
        ddlSubcategoria.DataBind()

        If Session("SubCatBuscar") = "" Then
            ddlSubcategoria.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlSubcategoria.SelectedValue = Session("SubCatBuscar")
            Try
                Session("AIO_OPC") = "SubLinea"
                Session("AIO_SubLinea") = ddlSubcategoria.SelectedValue
                objDatos.fnLog("IndexChange SubCat", ddlSubcategoria.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_sublinea", ex.Message.Replace("'", ""))
            End Try
        End If



        ssql = "select distinct ARMADORA as u_marca from SAP_TIENDA..tablaModelos6   order by u_marca "
        ssql = ssql & "  " & sCondicion
        Dim dtMarca As New DataTable
        dtMarca = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtMarca.NewRow
        fila("U_marca") = "-TODOS-"
        fila("U_marca") = "-TODOS-"

        dtMarca.Rows.Add(fila)
        ddlMarca.DataSource = dtMarca
        ddlMarca.DataTextField = "U_marca"
        ddlMarca.DataValueField = "U_marca"
        ddlMarca.DataBind()
        If Session("MarcaBuscar") = "" Then
            ddlMarca.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlMarca.SelectedValue = Session("MarcaBuscar")
            Try
                Session("AIO_OPC") = "Marca"
                Session("AIO_Marca") = ddlMarca.SelectedValue.ToString.TrimEnd
                objDatos.fnLog("IndexChange Marca", ddlMarca.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_marca", ex.Message.Replace("'", ""))
            End Try
        End If



        ssql = "select distinct modelo as U_modelo from SAP_TIENDA..tablaModelos6 where modelo is not null order by U_modelo"
        ssql = ssql & "  " & sCondicion
        Dim dtModelo As New DataTable
        dtModelo = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtModelo.NewRow
        fila("U_modelo") = "-TODOS-"
        fila("U_modelo") = "-TODOS-"

        dtModelo.Rows.Add(fila)
        ddlModelo.DataSource = dtModelo
        ddlModelo.DataTextField = "U_modelo"
        ddlModelo.DataValueField = "U_modelo"
        ddlModelo.DataBind()

        If Session("ModeloBuscar") = "" Then
            ddlModelo.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlModelo.SelectedValue = Session("ModeloBuscar")
            Try
                Session("AIO_OPC") = "Modelo"
                Session("AIO_Modelo") = ddlModelo.SelectedValue
                objDatos.fnLog("IndexChange Modelo", ddlModelo.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_modelo", ex.Message.Replace("'", ""))
            End Try
        End If





        ssql = " Select  distinct AnioDe as U_AnioDe ,AnioHasta as U_AnioHasta    FROM SAP_Tienda..tablaModelos6  "
        ssql = ssql & "  " & sCondicion
        Dim dtaño As New DataTable
        dtaño = objDatos.fnEjecutarConsultaSAP(ssql)
        ''Tomamos el año menor y el mayor para hacer un ciclo en el que muestre todos los años del rango
        Dim AñoMAX As Int16
        Dim AñoMIN As Int16
        AñoMAX = dtaño.Compute("MAX(U_AnioHasta)", "")
        AñoMIN = dtaño.Compute("MIN(U_AnioDe)", "")


        Dim dtAñoMostrar As New DataTable
        dtAñoMostrar.Columns.Add("U_Anio")

        For i = AñoMIN To AñoMAX Step 1
            fila = dtAñoMostrar.NewRow
            fila("U_Anio") = i
            dtAñoMostrar.Rows.Add(fila)
        Next



        fila = dtAñoMostrar.NewRow
        fila("U_Anio") = "-TODOS-"

        dtAñoMostrar.Rows.Add(fila)
        ddlAnio.DataSource = dtAñoMostrar
        ddlAnio.DataTextField = "U_Anio"
        ddlAnio.DataValueField = "U_Anio"
        ddlAnio.DataBind()

        If Session("AnioBuscar") = "" Then
            ddlAnio.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlAnio.SelectedValue = Session("AnioBuscar")
            Try
                Session("AIO_OPC") = "Año"
                Session("AIO_AÑO") = ddlAnio.SelectedValue
                objDatos.fnLog("IndexChange año", ddlAnio.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_año", ex.Message.Replace("'", ""))
            End Try
        End If

        objDatos.fnLog("Band viene de index:", ibandVieneIndex)
        If ibandVieneIndex = 1 Then
            FiltraCombos_AIOPMK()
        End If


    End Sub


    Public Sub fnCargaCatalogos_PMK()
        Dim fila As DataRow
        Dim ibandVieneIndex As Int16 = 0
        Dim sCondicion As String = ""
        ssql = objDatos.fnObtenerQuery("FiltraCategoria")
        ssql = ssql & "  " & sCondicion
        Dim dtCategoria As New DataTable
        dtCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtCategoria.NewRow
        fila("ItmsGrpNam") = "-TODOS-"
        fila("ItmsGrpNam") = "-TODOS-"






        dtCategoria.Rows.Add(fila)
        ddlCategoria.DataSource = dtCategoria
        ddlCategoria.DataTextField = "ItmsGrpNam"
        ddlCategoria.DataValueField = "ItmsGrpNam"
        ddlCategoria.DataBind()
        If Session("CatBuscar") = "" Then
            ddlCategoria.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlCategoria.SelectedValue = Session("CatBuscar")
            Try
                Session("AIO_OPC") = "Categoria"
                Session("AIO_Categoria") = ddlCategoria.SelectedValue
                objDatos.fnLog("IndexChange Cat", ddlCategoria.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_categoria", ex.Message.Replace("'", ""))
            End Try
        End If





        ssql = objDatos.fnObtenerQuery("FiltraMarca")
        ssql = ssql & "  " & sCondicion
        Dim dtMarca As New DataTable
        dtMarca = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtMarca.NewRow
        fila("U_marca") = "-TODOS-"
        fila("U_marca") = "-TODOS-"

        dtMarca.Rows.Add(fila)
        ddlMarca.DataSource = dtMarca
        ddlMarca.DataTextField = "U_marca"
        ddlMarca.DataValueField = "U_marca"
        ddlMarca.DataBind()
        If Session("MarcaBuscar") = "" Then
            ddlMarca.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlMarca.SelectedValue = Session("MarcaBuscar")
            Try
                Session("AIO_OPC") = "Marca"
                Session("AIO_Marca") = ddlMarca.SelectedValue.ToString.TrimEnd
                objDatos.fnLog("IndexChange Marca", ddlMarca.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_marca", ex.Message.Replace("'", ""))
            End Try
        End If


        ssql = objDatos.fnObtenerQuery("FiltraModelo")
        ssql = ssql & "  " & sCondicion
        Dim dtModelo As New DataTable
        dtModelo = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtModelo.NewRow
        fila("U_modelo") = "-TODOS-"
        fila("U_modelo") = "-TODOS-"

        dtModelo.Rows.Add(fila)
        ddlModelo.DataSource = dtModelo
        ddlModelo.DataTextField = "U_modelo"
        ddlModelo.DataValueField = "U_modelo"
        ddlModelo.DataBind()
        If Session("ModeloBuscar") = "" Then
            ddlModelo.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlModelo.SelectedValue = Session("ModeloBuscar")
            Try
                Session("AIO_OPC") = "Modelo"
                Session("AIO_Modelo") = ddlModelo.SelectedValue
                objDatos.fnLog("IndexChange Modelo", ddlModelo.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_modelo", ex.Message.Replace("'", ""))
            End Try
        End If



        ' ssql = "select distinct cast(U_AnioDe as varchar) + '-' + cast(U_anioHasta as varchar) as U_Anio from [@MODELOS] "
        ssql = objDatos.fnObtenerQuery("FiltraAños")
        ssql = ssql & "  " & sCondicion
        Dim dtaño As New DataTable
        dtaño = objDatos.fnEjecutarConsultaSAP(ssql)

        ''Tomamos el año menor y el mayor para hacer un ciclo en el que muestre todos los años del rango
        Dim AñoMAX As Int16
        Dim AñoMIN As Int16
        AñoMAX = dtaño.Compute("MAX(U_AnioHasta)", "")
        AñoMIN = dtaño.Compute("MIN(U_AnioDe)", "")


        Dim dtAñoMostrar As New DataTable
        dtAñoMostrar.Columns.Add("U_Anio")

        For i = AñoMIN To AñoMAX Step 1
            fila = dtAñoMostrar.NewRow
            fila("U_Anio") = i
            dtAñoMostrar.Rows.Add(fila)
        Next



        fila = dtAñoMostrar.NewRow
        fila("U_Anio") = "-TODOS-"

        dtAñoMostrar.Rows.Add(fila)
        ddlAnio.DataSource = dtAñoMostrar
        ddlAnio.DataTextField = "U_Anio"
        ddlAnio.DataValueField = "U_Anio"
        ddlAnio.DataBind()
        If Session("AnioBuscar") = "" Then
            ddlAnio.SelectedValue = "-TODOS-"
        Else
            ibandVieneIndex = 1
            ddlAnio.SelectedValue = Session("AnioBuscar")
            Try
                Session("AIO_OPC") = "Año"
                Session("AIO_AÑO") = ddlAnio.SelectedValue
                objDatos.fnLog("IndexChange año", ddlAnio.SelectedValue)

            Catch ex As Exception
                objDatos.fnLog("ex IndexChange_año", ex.Message.Replace("'", ""))
            End Try
        End If

        If ibandVieneIndex = 1 Then
            FiltraCombos_AIOPMK()
        End If


    End Sub
    Public Sub fnCargaCatalogosDelta()

        Dim sHtml As String = ""

        If Session("RazonSocial") = "" Then
            ssql = "select Code,ISNULL(U_Imagen,'') as Imagen, ISNULL(U_Descripcion,'') as Titulo,ISNULL(U_liga,'') as Liga from [@ECOM_CATALOGOS] WHERE U_Activo='S'  ANd U_AplicaInvitado='S'  Order by ISNULL(U_Descripcion,'') "
        Else
            ssql = "select Code,ISNULL(U_Imagen,'') as Imagen, ISNULL(U_Descripcion,'') as Titulo,ISNULL(U_liga,'') as Liga from [@ECOM_CATALOGOS] WHERE U_Activo='S' Order by ISNULL(U_Descripcion,'') "
        End If

        Dim dtCatalogos As New DataTable
        dtCatalogos = objDatos.fnEjecutarConsultaSAP(ssql)
        sHtml = ""
        Dim sParREB As String = ""

        For i = 0 To dtCatalogos.Rows.Count - 1 Step 1
            If Session("CatDelta") = "" Then
                sHtml = sHtml & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'> <div class='preview'><a href='catalogo.aspx?cat=" & dtCatalogos.Rows(i)("Code") & "'>  </a>" _
               & "<div class='img'><a href='catalogo.aspx?cat=" & dtCatalogos.Rows(i)("Code") & "'>   <img src='images/catalogos/" & dtCatalogos.Rows(i)("Imagen") & "' class='img-responsive'>     </a> " _
               & " <div class='action-products'><a href='" & dtCatalogos.Rows(i)("Liga") & "'></a><a class='item view' href='" & dtCatalogos.Rows(i)("Liga") & "'  target='_blank'></a></div> </div></div></div>"
            Else
                If Session("CatDelta") = dtCatalogos.Rows(i)("Code") Then
                    sHtml = sHtml & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'> <div class='preview'><a href='catalogo.aspx?cat=" & dtCatalogos.Rows(i)("Code") & "'>  </a>" _
             & "<div class='img'><a href='catalogo.aspx?cat=" & dtCatalogos.Rows(i)("Code") & "'>   <img src='images/catalogos/" & dtCatalogos.Rows(i)("Imagen") & "' class='img-responsive'>     </a> " _
             & " <div class='action-products'><a href='" & dtCatalogos.Rows(i)("Liga") & "'></a><a class='item view' href='" & dtCatalogos.Rows(i)("Liga") & "'  target='_blank'></a></div> </div></div></div>"
                Else
                    sHtml = sHtml & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'> <div class='preview'><a href='catalogo.aspx?cat=" & dtCatalogos.Rows(i)("Code") & "'>  </a>" _
             & "<div class='img'><a href='catalogo.aspx?cat=" & dtCatalogos.Rows(i)("Code") & "'>   <img src='images/catalogos/" & dtCatalogos.Rows(i)("Imagen") & "' class='img-responsive' style='opacity:0.2;'>     </a> " _
             & " <div class='action-products'><a href='" & dtCatalogos.Rows(i)("Liga") & "'></a><a class='item view' href='" & dtCatalogos.Rows(i)("Liga") & "' target='_blank'></a></div> </div></div></div>"

                End If
            End If

        Next

        pnlCatalogos.Visible = True
        Dim Literal As New LiteralControl(sHtml)
        pnlCatalogos.Controls.Clear()
        pnlCatalogos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaCatalogoFiltro(Categoria As String, CondicionFiltro As String)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sCondicion As String = ""
        If Categoria = "" Then
            ''Todos
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-TodosB2B")
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Todos")
            End If


        Else
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-CatB2B")
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Cat")
            End If

            ''Evaluamos los parametros que nos están enviando
            Dim iNivel As Int16 = 1
            Dim sQuery As String

            For Each Param As String In Request.QueryString
                sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
                Dim dtDatos As New DataTable
                dtDatos = objDatos.fnEjecutarConsulta(sQuery)
                If dtDatos.Rows.Count > 0 Then
                    If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ''Usan el campo de grupo como descripcion
                        'sCondicion = sCondicion & "T1.ItmsGrpNam=" & "'" & Param & "' AND"
                        sCondicion = sCondicion & "(CAST(T0." & dtDatos.Rows(0)(0) & " as varchar) =" & "'" & Request.QueryString(Param) & "' OR T1.ItmsGrpNam =" & "'" & Request.QueryString(Param) & "') AND "
                    Else
                        'sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                        If objDatos.fnObtenerDBMS() = "HANA" Then
                            sCondicion = sCondicion & "TO_VARCHAR(T0." & dtDatos.Rows(0)(0) & ")=" & "'" & Request.QueryString(Param) & "' AND "
                        Else
                            sCondicion = sCondicion & "" & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                        End If

                    End If
                End If
                iNivel = iNivel + 1
            Next


            If Categoria.ToUpper.Contains("COLECCI") Or Categoria.ToUpper.Contains("PRODUCTOS") Then
                sCondicion = ""
            Else
                sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)
            End If

            ssql = ssql.Replace("[%0]", sCondicion & CondicionFiltro.Replace(" AND ", " "))
        End If


        ''Validamos si no es una categoria Especial

        Dim ssqlEspecial As String
        ssqlEspecial = "select * from config.categoriasEsp where cvDescripcion='" & Categoria.Replace("%20", " ") & "'"
        Dim dtEspecial As New DataTable
        dtEspecial = objDatos.fnEjecutarConsulta(ssqlEspecial)
        If dtEspecial.Rows.Count > 0 Then
            ''es especial

            ssqlEspecial = objDatos.fnObtenerQuery(dtEspecial.Rows(0)("cvQuery")) & "  " & CondicionFiltro

            ''Revisamos si no hay un nivel 2

            If Request.QueryString.Count > 1 Then
                If objDatos.fnObtenerDBMS() = "HANA" Then
                    ssqlEspecial = ssqlEspecial & " AND TO_VARCHAR(" & dtEspecial.Rows(0)("cvCampoSAPNiv2") & ")=" & "'" & Request.QueryString(1) & "'"
                Else
                    ssqlEspecial = ssqlEspecial & " AND " & dtEspecial.Rows(0)("cvCampoSAPNiv2") & "=" & "'" & Request.QueryString(1) & "'"
                End If

            End If
        End If

        objDatos.fnLog("Productos-catFILTRO", ssql.Replace("'", ""))
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql.Replace("AND AND ", " AND "))
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0

        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If
        If Session("ContPaginas") + 1 = 0 Then
            lblPaginas.Text = "1/" & iTotPaginas
            lblPaginasAbajo.Text = "1/" & iTotPaginas
        Else
            lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
            lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        End If


        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Or iFinal < 0 Then
            iFinal = dtProductos.Rows.Count - 1

            Session("ContPaginas") = 0
        End If

        Dim dPrecioActual As Double = 0

        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

            '' 
            'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
            'sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            'sHtmlBanner = sHtmlBanner & " <div class='preview'>"
            'sHtmlBanner = sHtmlBanner & "  <div class='img'>"
            'sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"

            'If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            '    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
            'End If


            'Dim sPintaPrev As String = "SI"
            'Dim sPintaFav As String = "SI"
            'Dim sPintaCompra As String = "SI"

            'ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            'Dim dtPintaCat As New DataTable
            'dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            'If dtPintaCat.Rows.Count > 0 Then
            '    sPintaPrev = dtPintaCat.Rows(0)("Interior")
            '    sPintaFav = dtPintaCat.Rows(0)("Favorito")
            '    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            'End If

            'If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            '    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
            'Else
            '    sHtmlBanner = sHtmlBanner & "     <div>"
            'End If

            'If sPintaCompra = "SI" Then
            '    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
            'End If
            'If sPintaFav = "SI" Then
            '    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
            'End If
            'If sPintaCompra = "SI" Then
            '    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
            'End If



            'sHtmlBanner = sHtmlBanner & "     </div>"
            'sHtmlBanner = sHtmlBanner & "  </div>"
            'sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            '''img/home/producto-1.png
            'For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            '    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
            '        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
            '            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            '        Else
            '            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            '        End If
            '    Else
            '        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then

            '            If CInt(Session("slpCode")) <> 0 Then

            '                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            '            Else
            '                'If Session("UserB2C") = "" Then
            '                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            '                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            '                Else
            '                    If Session("UserB2C") <> "" Then
            '                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            '                    Else
            '                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            '                    End If
            '                End If

            '            End If
            '            If Session("Cliente") <> "" Then
            '                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            '            End If

            '            ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            '            'sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & " " & dPrecioActual.ToString("###,###,###.#0") & "</span>"
            '        End If
            '        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
            '            Dim iband As Int16 = 0
            '            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("-", "") & ".jpg") Then
            '                iband = 1
            '                sImagen = "images/products/" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("-", "") & ".jpg"
            '            End If
            '            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtProductos.Rows(i)("ItemCode")) & ".jpg") Then
            '                iband = 1
            '                sImagen = "images/products/" & CStr(dtProductos.Rows(i)("ItemCode")) & ".jpg"
            '            End If

            '            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
            '                iband = 1
            '                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
            '            End If

            '            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
            '                iband = 1
            '                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
            '            End If

            '            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
            '                iband = 1
            '                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
            '            End If

            '            objDatos.fnLog("imagen", sImagen)
            '            Try
            '                ' sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            '                If iband = 0 Then
            '                    If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
            '                        sImagen = "images/no-image.png"
            '                    Else
            '                        If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" Then
            '                            sImagen = "images/no-image.png"
            '                        Else
            '                            sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            '                        End If

            '                    End If
            '                End If


            '            Catch ex As Exception
            '                sImagen = "images/no-image.png"
            '            End Try

            '            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
            '        End If
            '    End If

            'Next


            'sHtmlBanner = sHtmlBanner & "  </a>"



            'sHtmlBanner = sHtmlBanner & " </div>"
            'sHtmlBanner = sHtmlBanner & "  </a>"
            'sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & "</span>"

            'sHtmlBanner = sHtmlBanner & "</div>"

            '      
            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1
            ' sHtmlBanner = sHtmlBanner & "</div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaCatalogoFiltro(CondicionFiltro As String)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""


        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = objDatos.fnObtenerQuery("Productos-TodosB2B")
        Else
            ssql = objDatos.fnObtenerQuery("Productos-Todos")
        End If


        ssql = ssql & CondicionFiltro

        ' objDatos.fnLog("Productos-cat", ssql.Replace("'", ""))
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0

        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If
        If Session("ContPaginas") + 1 = 0 Then
            lblPaginas.Text = "1/" & iTotPaginas
            lblPaginasAbajo.Text = "1/" & iTotPaginas
        Else
            lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
            lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        End If


        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Or iFinal < 0 Then
            iFinal = dtProductos.Rows.Count - 1

            Session("ContPaginas") = 0
        End If

        Dim dPrecioActual As Double = 0
        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If


            sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

            ''' 
            ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
            ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
            ''sHtmlBanner = sHtmlBanner & "  <div class='img'>"
            ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"

            ''If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
            ''End If

            ''Dim sPintaPrev As String = "SI"
            ''Dim sPintaFav As String = "SI"
            ''Dim sPintaCompra As String = "SI"

            ''ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            ''Dim dtPintaCat As New DataTable
            ''dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            ''If dtPintaCat.Rows.Count > 0 Then
            ''    sPintaPrev = dtPintaCat.Rows(0)("Interior")
            ''    sPintaFav = dtPintaCat.Rows(0)("Favorito")
            ''    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            ''End If

            ''If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
            ''Else
            ''    sHtmlBanner = sHtmlBanner & "     <div>"
            ''End If

            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
            ''End If
            ''If sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
            ''End If
            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
            ''End If



            ''sHtmlBanner = sHtmlBanner & "     </div>"
            ''sHtmlBanner = sHtmlBanner & "  </div>"
            ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''''img/home/producto-1.png
            ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
            ''        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
            ''            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        Else
            ''            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        End If
            ''    Else
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then


            ''            If CInt(Session("slpCode")) <> 0 Then

            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            Else
            ''                'If Session("UserB2C") = "" Then
            ''                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                Else
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                End If

            ''                ' Else
            ''                '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                'End If

            ''            End If
            ''            If Session("Cliente") <> "" Then
            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            End If
            ''            ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''            'sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & " " & dPrecioActual.ToString("###,###,###.#0") & "</span>"
            ''        End If
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
            ''            Dim iband As Int16 = 0
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("-", "") & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("-", "") & ".jpg"
            ''            End If
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtProductos.Rows(i)("ItemCode")) & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & CStr(dtProductos.Rows(i)("ItemCode")) & ".jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
            ''            End If

            ''            objDatos.fnLog("imagen", sImagen)
            ''            Try
            ''                ' sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                If iband = 0 Then
            ''                    If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
            ''                        sImagen = "images/no-image.png"
            ''                    Else
            ''                        If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" Then
            ''                            sImagen = "images/no-image.png"
            ''                        Else
            ''                            sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                        End If

            ''                    End If
            ''                End If


            ''            Catch ex As Exception
            ''                sImagen = "images/no-image.png"
            ''            End Try

            ''            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)


            ''        End If
            ''    End If

            ''Next


            ''sHtmlBanner = sHtmlBanner & "  </a>"


            ''sHtmlBanner = sHtmlBanner & " </div>"
            ''sHtmlBanner = sHtmlBanner & "  </a>"
            ''sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & "</span>"

            ''sHtmlBanner = sHtmlBanner & "</div>"

            '      
            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1
            ' sHtmlBanner = sHtmlBanner & "</div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaCatalogoFiltroPrecio(RangoDe As Double, RangoAL As Double)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = objDatos.fnObtenerQuery("Productos-TodosB2B")
        Else
            ssql = objDatos.fnObtenerQuery("Productos-Todos")
        End If



        ' objDatos.fnLog("Productos-cat", ssql.Replace("'", ""))
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        dtProductos.Columns.Add("Precio", GetType(Double))

        Dim dPrecioActual As Double = 0

        ''Recorremos el listado y le ponemos precio
        For i = 0 To dtProductos.Rows.Count - 1 Step 1

            If CInt(Session("slpCode")) <> 0 Then
                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            Else
                'If Session("UserB2C") = "" Then
                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                Else
                    If Session("UserB2C") <> "" Then
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                    Else
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                    End If

                End If
                If Session("Cliente") <> "" Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                End If
            End If
            dtProductos.Rows(i)("Precio") = dPrecioActual
            ' objDatos.fnLog("Filtro", dPrecioActual)
        Next
        Dim view As DataView = New DataView(dtProductos)
        view.Sort = "Precio"
        Dim dtPaso As New DataTable
        Dim foundRows As DataRow()

        foundRows = dtProductos.[Select]("Precio >= " & "'" & RangoDe & "' AND Precio <= " & "'" & RangoDe & "'", "Precio ASC")

        dtPaso = foundRows.CopyToDataTable()

        'dtProductos = view.Table
        dtProductos = New DataTable
        dtProductos = dtPaso


        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0

        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If
        If Session("ContPaginas") + 1 = 0 Then
            lblPaginas.Text = "1/" & iTotPaginas
            lblPaginasAbajo.Text = "1/" & iTotPaginas
        Else
            lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
            lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        End If


        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Or iFinal < 0 Then
            iFinal = dtProductos.Rows.Count - 1

            Session("ContPaginas") = 0
        End If

        '  Dim dPrecioActual As Double = 0
        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
            ' 
            ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
            ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
            ''sHtmlBanner = sHtmlBanner & "  <div class='img'>"
            ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"

            ''If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
            ''End If
            ''Dim sPintaPrev As String = "SI"
            ''Dim sPintaFav As String = "SI"
            ''Dim sPintaCompra As String = "SI"

            ''ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            ''Dim dtPintaCat As New DataTable
            ''dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            ''If dtPintaCat.Rows.Count > 0 Then
            ''    sPintaPrev = dtPintaCat.Rows(0)("Interior")
            ''    sPintaFav = dtPintaCat.Rows(0)("Favorito")
            ''    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            ''End If

            ''If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
            ''Else
            ''    sHtmlBanner = sHtmlBanner & "     <div>"
            ''End If

            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
            ''End If
            ''If sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
            ''End If
            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
            ''End If



            ''sHtmlBanner = sHtmlBanner & "     </div>"
            ''sHtmlBanner = sHtmlBanner & "  </div>"
            ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''''img/home/producto-1.png
            ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
            ''        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
            ''            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        Else
            ''            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        End If
            ''    Else
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then


            ''            If CInt(Session("slpCode")) <> 0 Then

            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            Else
            ''                'If Session("UserB2C") = "" Then
            ''                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                Else
            ''                    If Session("UserB2C") <> "" Then
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                    Else
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                    End If
            ''                End If

            ''                If Session("Cliente") <> "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                End If

            ''            End If

            ''            ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''            'sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & " " & dPrecioActual.ToString("###,###,###.#0") & "</span>"
            ''        End If
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
            ''            Dim iband As Int16 = 0
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & ".jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
            ''            End If
            ''            Try
            ''                sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value And iband = 0 Then
            ''                    sImagen = "images/no-image.png"
            ''                Else
            ''                    If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" And iband = 0 Then
            ''                        sImagen = "images/no-image.png"
            ''                    Else
            ''                        sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                    End If

            ''                End If

            ''            Catch ex As Exception
            ''                sImagen = "images/no-image.png"
            ''            End Try

            ''            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
            ''        End If
            ''    End If

            ''Next


            ''sHtmlBanner = sHtmlBanner & "  </a>"


            ''sHtmlBanner = sHtmlBanner & " </div>"
            ''sHtmlBanner = sHtmlBanner & "  </a>"
            ''sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & "</span>"

            ''sHtmlBanner = sHtmlBanner & "</div>"

            '      
            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1
            ' sHtmlBanner = sHtmlBanner & "</div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaCatalogoFiltroPrecio(Categoria As String, RangoDe As Double, RangoAL As Double)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""


        If Categoria = "" Then
            ''Todos
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-TodosB2B")
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Todos")
            End If


        Else

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-CatB2B")
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Cat")
            End If

            ''Evaluamos los parametros que nos están enviando
            Dim iNivel As Int16 = 1
            Dim sQuery As String
            Dim sCondicion As String = ""
            For Each Param As String In Request.QueryString
                sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
                '  objDatos.fnLog("PArams:", sQuery.Replace("'", ""))
                Dim dtDatos As New DataTable
                dtDatos = objDatos.fnEjecutarConsulta(sQuery)
                If dtDatos.Rows.Count > 0 Then
                    If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ''Usan el campo de grupo como descripcion
                        'sCondicion = sCondicion & "T1.ItmsGrpNam=" & "'" & Param & "' AND"
                        sCondicion = sCondicion & "(CAST(T0." & dtDatos.Rows(0)(0) & " as varchar) =" & "'" & Request.QueryString(Param) & "' OR T1.ItmsGrpNam =" & "'" & Request.QueryString(Param) & "') AND "
                    Else
                        sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                        'sCondicion = sCondicion & "" & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                    End If
                    ' objDatos.fnLog("condicion:", sCondicion.Replace("'", ""))
                End If
                iNivel = iNivel + 1
            Next
            sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)
            ssql = ssql.Replace("[%0]", sCondicion)
            '   objDatos.fnLog("Filtra:", ssql.Replace("'", ""))


        End If


        Dim dtPaso As New DataTable
        ' objDatos.fnLog("Productos-cat", ssql.Replace("'", ""))
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        dtProductos.Columns.Add("Precio", GetType(Double))

        dtPaso = objDatos.fnEjecutarConsultaSAP(ssql)
        dtPaso.Columns.Add("Precio", GetType(Double))
        Dim dPrecioActual As Double = 0
        ''Recorremos el listado y le ponemos precio
        For i = 0 To dtProductos.Rows.Count - 1 Step 1

            If CInt(Session("slpCode")) <> 0 Then
                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            Else
                'If Session("UserB2C") = "" Then
                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                Else
                    If Session("UserB2C") <> "" Then
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                    Else
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                    End If

                End If

            End If
            If Session("Cliente") <> "" Then
                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            End If
            dtProductos.Rows(i)("Precio") = dPrecioActual
            '  objDatos.fnLog("Filtro Asigna Precio", dPrecioActual)
        Next



        dtPaso.Rows.Clear()
        ' objDatos.fnLog("Filtro dtProductos ", dtProductos.Rows.Count)
        For Each registro As DataRow In dtProductos.Rows
            '    objDatos.fnLog("Filtro for each ", registro("Precio") & ">= " & RangoDe)
            If registro("Precio") >= RangoDe And registro("Precio") <= RangoAL Then ''
                objDatos.fnLog("Filtro for each ", "entra")
                dtPaso.ImportRow(registro)
            End If
        Next



        '  objDatos.fnLog("Filtro foundRows ", dtPaso.Rows.Count)
        'dtProductos = view.Table
        dtProductos = New DataTable
        dtProductos = dtPaso


        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0

        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If
        If Session("ContPaginas") + 1 = 0 Then
            lblPaginas.Text = "1/" & iTotPaginas
            lblPaginasAbajo.Text = "1/" & iTotPaginas
        Else
            lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas

            lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        End If


        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Or iFinal < 0 Then
            iFinal = dtProductos.Rows.Count - 1

            Session("ContPaginas") = 0
        End If


        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

            ' 
            ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
            ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
            ''sHtmlBanner = sHtmlBanner & "  <div class='img'>"
            ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"

            ''If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
            ''End If

            ''Dim sPintaPrev As String = "SI"
            ''Dim sPintaFav As String = "SI"
            ''Dim sPintaCompra As String = "SI"

            ''ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            ''Dim dtPintaCat As New DataTable
            ''dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            ''If dtPintaCat.Rows.Count > 0 Then
            ''    sPintaPrev = dtPintaCat.Rows(0)("Interior")
            ''    sPintaFav = dtPintaCat.Rows(0)("Favorito")
            ''    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            ''End If

            ''If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
            ''Else
            ''    sHtmlBanner = sHtmlBanner & "     <div>"
            ''End If

            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
            ''End If
            ''If sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
            ''End If
            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
            ''End If



            ''sHtmlBanner = sHtmlBanner & "     </div>"
            ''sHtmlBanner = sHtmlBanner & "  </div>"
            ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''''img/home/producto-1.png
            ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
            ''        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
            ''            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        Else
            ''            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        End If
            ''    Else
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then


            ''            If CInt(Session("slpCode")) <> 0 Then

            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            Else
            ''                'If Session("UserB2C") = "" Then
            ''                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                Else
            ''                    If Session("UserB2C") <> "" Then
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                    Else
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                    End If
            ''                End If

            ''                ' Else
            ''                '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                'End If

            ''            End If

            ''            ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''            'sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & " " & dPrecioActual.ToString("###,###,###.#0") & "</span>"
            ''        End If
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
            ''            Dim iband As Int16 = 0
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & ".jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
            ''            End If
            ''            Try
            ''                sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value And iband = 0 Then
            ''                    sImagen = "images/no-image.png"
            ''                Else
            ''                    If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" And iband = 0 Then
            ''                        sImagen = "images/no-image.png"
            ''                    Else
            ''                        sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                    End If

            ''                End If

            ''            Catch ex As Exception
            ''                sImagen = "images/no-image.png"
            ''            End Try

            ''            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
            ''        End If
            ''    End If

            ''Next


            ''sHtmlBanner = sHtmlBanner & "  </a>"


            ''sHtmlBanner = sHtmlBanner & " </div>"
            ''sHtmlBanner = sHtmlBanner & "  </a>"
            ''sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & "</span>"

            sHtmlBanner = sHtmlBanner & "</div>"

            '      
            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1
            ' sHtmlBanner = sHtmlBanner & "</div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub
    Public Function fnObtenerMoneda(ItemCode As String) As String
        Dim ssql As String = ""
        ''Posibles monedas en la lista de precios
        ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
        ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
        dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
        Dim sMoneda As String = ""
        If Request.QueryString("Moneda") <> "" Then
            sMoneda = Request.QueryString("Moneda")
        End If
        If dtMonedas.Rows.Count > 0 Then
            sMoneda = dtMonedas.Rows(0)(0)
            If dtMonedas.Rows.Count > 1 Then
                ''El articulo se puede vender en mas de una moneda
                ''Llenamos y mostramos combo de moneda



            End If
        End If
        Session("Moneda") = sMoneda
        Return sMoneda
    End Function
    Public Sub fnCargaCatalogo(Categoria As String)


        Dim sHtmlRuta As String = ""

        sHtmlRuta = "<ol class='breadcrumb'>"
        sHtmlRuta = sHtmlRuta & "<li><a href='catalogo.aspx'>Home</a></li>"
        sHtmlRuta = sHtmlRuta & "</ol>"
        Dim LiteralRuta As New LiteralControl(sHtmlRuta)
        pnlRuta.Controls.Clear()
        pnlRuta.Controls.Add(LiteralRuta)



        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        If Categoria = "" Then
            ''Todos

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-TodosB2B")
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Todos")
            End If


        Else
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-CatB2B")
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Cat")
            End If

            ''Evaluamos los parametros que nos están enviando
            Dim iNivel As Int16 = 1
            Dim sQuery As String
            Dim sCondicion As String = ""
            For Each Param As String In Request.QueryString
                sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
                Dim dtDatos As New DataTable
                dtDatos = objDatos.fnEjecutarConsulta(sQuery)
                If dtDatos.Rows.Count > 0 Then
                    If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ''Usan el campo de grupo como descripcion
                        'sCondicion = sCondicion & "T1.ItmsGrpNam=" & "'" & Param & "' AND"
                        sCondicion = sCondicion & "(CAST(T0." & dtDatos.Rows(0)(0) & " as varchar) =" & "'" & Request.QueryString(Param) & "' OR T1.ItmsGrpNam =" & "'" & Request.QueryString(Param) & "') AND "
                    Else
                        If objDatos.fnObtenerDBMS() = "HANA" Then
                            sCondicion = sCondicion & "TO_VARCHAR(T0." & dtDatos.Rows(0)(0) & ")=" & "'" & Request.QueryString(Param) & "' AND "
                        Else
                            sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                        End If

                        ' sCondicion = sCondicion & "" & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                    End If
                End If
                iNivel = iNivel + 1
            Next
            sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)
            ssql = ssql.Replace("[%0]", sCondicion)
            '    objDatos.fnLog("Para Filtrar:", ssql.Replace("'", ""))
        End If
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "

        Dim iTotPaginas As Int16 = 0

        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If
        If Session("ContPaginas") + 1 = 0 Then
            lblPaginas.Text = "1/" & iTotPaginas
            lblPaginasAbajo.Text = "1/" & iTotPaginas
        Else
            lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
            lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        End If


        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Or iFinal < 0 Then
            iFinal = dtProductos.Rows.Count - 1

            Session("ContPaginas") = 0
        End If
        Dim dPrecioActual As Double = 0


        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

            ''' 
            ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
            ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
            ''sHtmlBanner = sHtmlBanner & "  <div class='img'>"
            ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"

            ''If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
            ''End If

            ''Dim sPintaPrev As String = "SI"
            ''Dim sPintaFav As String = "SI"
            ''Dim sPintaCompra As String = "SI"

            ''ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            ''Dim dtPintaCat As New DataTable
            ''dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            ''If dtPintaCat.Rows.Count > 0 Then
            ''    sPintaPrev = dtPintaCat.Rows(0)("Interior")
            ''    sPintaFav = dtPintaCat.Rows(0)("Favorito")
            ''    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            ''End If

            ''If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
            ''Else
            ''    sHtmlBanner = sHtmlBanner & "     <div>"
            ''End If

            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
            ''End If
            ''If sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
            ''End If
            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
            ''End If
            ''sHtmlBanner = sHtmlBanner & "     </div>"
            ''sHtmlBanner = sHtmlBanner & "  </div>"

            ''If CInt(Session("slpCode")) <> 0 Then

            ''    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''Else
            ''    'If Session("UserB2C") = "" Then
            ''    If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''    Else
            ''        If Session("UserB2C") <> "" Then
            ''            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''        Else
            ''            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''        End If

            ''    End If

            ''    ' Else
            ''    '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''    'End If

            ''End If
            ''If Session("Cliente") <> "" Then
            ''    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''End If

            ''sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & "</span>"

            ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''''img/home/producto-1.png
            ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
            ''        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
            ''            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        Else
            ''            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
            ''        End If
            ''    Else
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then

            ''            If CInt(Session("slpCode")) <> 0 Then

            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            Else
            ''                'If Session("UserB2C") = "" Then
            ''                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                Else
            ''                    If Session("UserB2C") <> "" Then
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                    Else
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                    End If

            ''                End If

            ''                ' Else
            ''                '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                'End If

            ''            End If
            ''            If Session("Cliente") <> "" Then
            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            End If

            ''            ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))

            ''        End If
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
            ''            Dim iband As Int16 = 0
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & ".jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
            ''            End If
            ''            Try
            ''                sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value And iband = 0 Then
            ''                    sImagen = "images/no-image.png"
            ''                Else
            ''                    If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" And iband = 0 Then
            ''                        sImagen = "images/no-image.png"
            ''                    Else
            ''                        sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                    End If

            ''                End If

            ''            Catch ex As Exception
            ''                sImagen = "images/no-image.png"
            ''            End Try
            ''            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
            ''        End If
            ''    End If

            ''Next


            ''sHtmlBanner = sHtmlBanner & "  </a>"
            ''sHtmlBanner = sHtmlBanner & " </div>"

            ''sHtmlBanner = sHtmlBanner & "  </a>"

            ''sHtmlBanner = sHtmlBanner & "</div>"

            '      
            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1
            ' sHtmlBanner = sHtmlBanner & "</div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)




    End Sub


    Public Sub fnCargaCatalogo(Categoria As String, Filtro As String)
        Dim sHtmlRuta As String = ""
        sHtmlRuta = "<ol class='breadcrumb'>"
        sHtmlRuta = sHtmlRuta & "<li><a href='#'>Home</a></li>"
        sHtmlRuta = sHtmlRuta & "</ol>"
        Dim LiteralRuta1 As New LiteralControl(sHtmlRuta)
        pnlRuta.Controls.Clear()
        pnlRuta.Controls.Add(LiteralRuta1)
        Dim dPrecioActual As Double = 0

        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        If Categoria = "" Or Categoria = "Productos" Or Categoria.ToUpper = "COLECCION" Or Categoria.ToUpper = "COLECCIÓN" Then
            ''Todos
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-TodosB2B")
            Else
                objDatos.fnLog("Productos todos:", "")
                ssql = objDatos.fnObtenerQuery("Productos-Todos")
            End If


        Else
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ssql = objDatos.fnObtenerQuery("Productos-CatB2B")
                If Categoria.ToUpper.Contains("REB") Then
                    ssql = objDatos.fnObtenerQuery("Productos-CatB2B_DESCUENTOS")
                End If
            Else
                ssql = objDatos.fnObtenerQuery("Productos-Cat")
            End If

            ''Evaluamos los parametros que nos están enviando
            Dim iNivel As Int16 = 1
            Dim sQuery As String
            Dim sCondicion As String = ""
            For Each Param As String In Request.QueryString
                sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
                '  objDatos.fnLog("PArams:", sQuery.Replace("'", ""))
                Dim dtDatos As New DataTable
                dtDatos = objDatos.fnEjecutarConsulta(sQuery)
                If dtDatos.Rows.Count > 0 Then
                    If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ''Usan el campo de grupo como descripcion
                        'sCondicion = sCondicion & "T1.ItmsGrpNam=" & "'" & Param & "' AND"

                        If Session("TipoDBMS") = "HANA" Then
                            sCondicion = sCondicion & "(CAST(T0.""" & dtDatos.Rows(0)(0) & """ as varchar) =" & "'" & Request.QueryString(Param) & "' OR T1.""ItmsGrpNam"" =" & "'" & Request.QueryString(Param) & "') AND "
                        Else
                            sCondicion = sCondicion & "(CAST(T0." & dtDatos.Rows(0)(0) & " as varchar) =" & "'" & Request.QueryString(Param) & "' OR T1.ItmsGrpNam =" & "'" & Request.QueryString(Param) & "') AND "
                        End If
                    Else
                        If Session("TipoDBMS") = "HANA" Then
                            sCondicion = sCondicion & "TO_VARCHAR(T0.""" & dtDatos.Rows(0)(0) & """)=" & "'" & Request.QueryString(Param) & "' AND "
                        Else
                            sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                        End If

                        'sCondicion = sCondicion & "" & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                    End If
                    ' objDatos.fnLog("condicion:", sCondicion.Replace("'", ""))
                End If
                iNivel = iNivel + 1
            Next
            sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)
            ssql = ssql.Replace("[%0]", sCondicion)
            objDatos.fnLog("Filtra:", ssql.Replace("'", ""))


            ''Armamos la ruta de donde está


            Try
                sHtmlRuta = "<ol class='breadcrumb'>"
                sHtmlRuta = sHtmlRuta & "<li><a href='#'>Home</a></li>"
                iNivel = 1
                Dim ssqlRuta As String = ""
                Dim rutaLink As String = "Catalogo.aspx?Cat="
                For Each Param As String In Request.QueryString
                    sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
                    Dim dtDatos As New DataTable
                    dtDatos = objDatos.fnEjecutarConsulta(sQuery)
                    If dtDatos.Rows.Count > 0 Then
                        Dim sValorPintar As String = ""
                        If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then

                        Else

                            ' objDatos.fnLog("Cats", "entra")
                            If CStr(dtDatos.Rows(0)(0)).Contains("U_") Then
                                If iNivel = 1 Then
                                    ssqlRuta = objDatos.fnObtenerQuery("CampoUsuario")
                                Else
                                    ssqlRuta = objDatos.fnObtenerQuery("CampoUsuarioNiv" & iNivel)
                                End If

                                ssqlRuta = ssqlRuta.Replace("[%0]", Request.QueryString(Param))
                                Dim dtValor As New DataTable
                                dtValor = objDatos.fnEjecutarConsultaSAP(ssqlRuta)
                                ' objDatos.fnLog("Cats", ssql.Replace("'", ""))
                                If dtValor.Rows.Count > 0 Then
                                    sValorPintar = dtValor.Rows(0)(0)
                                End If
                            Else
                                sValorPintar = Request.QueryString(Param)
                            End If
                        End If

                        If iNivel = 1 Then
                            rutaLink = rutaLink & Request.QueryString(Param)
                            sHtmlRuta = sHtmlRuta & "<li><a href='" & rutaLink & "'>" & sValorPintar & "</a></li>"
                        Else
                            rutaLink = rutaLink & "&param" & iNivel - 1 & "=" & Request.QueryString(Param)
                            sHtmlRuta = sHtmlRuta & "<li><a href='" & rutaLink & "'>" & sValorPintar & "</a></li>"
                        End If

                    End If
                    iNivel = iNivel + 1
                Next
                sHtmlRuta = sHtmlRuta & "</ol>"
                Dim LiteralRuta As New LiteralControl(sHtmlRuta)
                pnlRuta.Controls.Clear()
                pnlRuta.Controls.Add(LiteralRuta)

            Catch ex As Exception

            End Try

        End If

        ''Validamos si no es una categoria Especial
        objDatos.fnLog("CatEspecial:", "")
        Dim ssqlEspecial As String
        ssqlEspecial = "select * from config.categoriasEsp where cvDescripcion='" & Categoria.Replace("%20", " ") & "'"
        Dim dtEspecial As New DataTable
        dtEspecial = objDatos.fnEjecutarConsulta(ssqlEspecial)
        If dtEspecial.Rows.Count > 0 Then
            ''es especial

            ssql = objDatos.fnObtenerQuery(dtEspecial.Rows(0)("cvQuery"))
            ''Revisamos si no hay un nivel 2

            If Request.QueryString.Count > 1 Then
                If Session("TipoDBMS") = "HANA" Then
                    ssql = ssql & " AND """ & dtEspecial.Rows(0)("cvCampoSAPNiv2") & """=" & "'" & Request.QueryString(1) & "'"
                Else
                    ssql = ssql & " AND " & dtEspecial.Rows(0)("cvCampoSAPNiv2") & "=" & "'" & Request.QueryString(1) & "'"
                End If

            End If
        End If
        If Filtro.Contains("Alfab") Or Filtro.Contains("-Sel") Then
            Dim sqlFiltro As String = "SELECT cvCampo FROM config.Filtros where cvFiltro like 'Alfab%'"
            Dim dtCampo As New DataTable
            dtCampo = objDatos.fnEjecutarConsulta(sqlFiltro)
            If dtCampo.Rows.Count > 0 Then
                If Session("TipoDBMS") = "HANA" Then
                    ssql = ssql & " Order by """ & dtCampo.Rows(0)(0) & """"
                Else
                    ssql = ssql & " Order by " & dtCampo.Rows(0)(0)
                End If

            Else
                If Session("TipoDBMS") = "HANA" Then
                    ssql = ssql & " Order by ""ItemCode"""
                Else
                    ssql = ssql & " Order by ItemCode"
                End If

            End If
            objDatos.fnLog("Antes de ejecutar consultaSAP:", ssql.Replace("'", ""))
            dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        End If

        objDatos.fnLog("DatosProd", dtProductos.Rows.Count)

        If Filtro.ToUpper.Contains("PRECIO") Then
            'objDatos.fnLog("Filtro", "Entra en Precio")
            dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
            dtProductos.Columns.Add("Precio", GetType(Double))



            ''Recorremos el listado y le ponemos precio
            For i = 0 To dtProductos.Rows.Count - 1 Step 1

                Dim descEspecial As Double = 0
                descEspecial = objDatos.fnDescuentoEspecial((dtProductos.Rows(i)("ItemCode")), Session("Cliente"))

                ' Dim dPrecioActual As Double = 0
                If CInt(Session("slpCode")) <> 0 Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                Else
                    'If Session("UserB2C") = "" Then
                    If Session("UserB2C") = "" And Session("UserB2B") = "" Then
                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                    Else
                        If Session("UserB2C") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                        End If

                    End If
                End If
                If Session("Cliente") <> "" Then
                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
                End If

                ' objDatos.fnLog("Filtro", dPrecioActual)
                If descEspecial > 0 Then
                    dPrecioActual = dPrecioActual * (1 - (descEspecial / 100))
                End If
                dtProductos.Rows(i)("Precio") = dPrecioActual
            Next

            Dim view As DataView = New DataView(dtProductos)

            ' Lo ordenamos por el campo Nombre.
            '
            view.Sort = "Precio"
            Dim dtPaso As New DataTable
            Dim foundRows As DataRow()
            If Filtro.Contains("menor a mayor") Or Filtro.ToUpper.Contains("MENOR A MAYOR") Then
                foundRows = dtProductos.[Select]("", "Precio ASC")
            Else
                If Filtro.Contains("mayor a menor") Or Filtro.Contains("MAYOR A MENOR") Then
                    foundRows = dtProductos.[Select]("", "Precio DESC")
                Else
                    foundRows = dtProductos.[Select]("", "Precio ASC")
                End If
            End If

            dtPaso = foundRows.CopyToDataTable()

            'dtProductos = view.Table
            dtProductos = New DataTable
            dtProductos = dtPaso
        End If

        If Filtro.Contains("Vendidos") Then
            '     objDatos.fnLog("Filtro", "Entra en vendidos")
            dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
            dtProductos.Columns.Add("Venta", GetType(Double))

            ''Recorremos el listado y le ponemos precio
            For i = 0 To dtProductos.Rows.Count - 1 Step 1
                ssql = objDatos.fnObtenerQuery("VentaArticulo")
                ssql = ssql.Replace("[%0]", dtProductos.Rows(i)("itemCode"))
                Dim dtVenta As New DataTable
                dtVenta = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtVenta.Rows.Count > 0 Then
                    dtProductos.Rows(i)("Venta") = CDbl(dtVenta.Rows(0)(0))
                End If

            Next
            Dim view As DataView = New DataView(dtProductos)

            ' Lo ordenamos por el campo Nombre.
            '
            view.Sort = "Venta"
            Dim dtPaso As New DataTable

            Dim foundRows As DataRow() = dtProductos.[Select]("", "Venta desc")
            dtPaso = foundRows.CopyToDataTable()

            'dtProductos = view.Table
            dtProductos = New DataTable
            dtProductos = dtPaso

        End If

        If Filtro.Contains("Marca") Then
            ' objDatos.fnLog("Filtro", "Entra en marca")
            dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
            dtProductos.Columns.Add("Marca", GetType(String))

            ''Recorremos el listado y le ponemos precio
            For i = 0 To dtProductos.Rows.Count - 1 Step 1
                ssql = objDatos.fnObtenerQuery("MarcaArticulo")
                ssql = ssql.Replace("[%0]", dtProductos.Rows(i)("itemCode"))
                Dim dtMarca As New DataTable
                dtMarca = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtMarca.Rows.Count > 0 Then
                    dtProductos.Rows(i)("Marca") = CDbl(dtMarca.Rows(0)(0))
                End If

            Next
            Dim view As DataView = New DataView(dtProductos)

            ' Lo ordenamos por el campo Nombre.
            '
            view.Sort = "Marca"
            Dim dtPaso As New DataTable

            Dim foundRows As DataRow() = dtProductos.[Select]("", "Marca asc")
            dtPaso = foundRows.CopyToDataTable()

            'dtProductos = view.Table
            dtProductos = New DataTable
            dtProductos = dtPaso

        End If

        If Filtro.Contains("Promo") Then
            '   objDatos.fnLog("Filtro", "Entra en promo")
            dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
            dtProductos.Columns.Add("Promo", GetType(String))

            ''Recorremos el listado y le ponemos precio
            For i = 0 To dtProductos.Rows.Count - 1 Step 1
                If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
                    dtProductos.Rows(i)("Promo") = "A"
                Else
                    dtProductos.Rows(i)("Promo") = "B"
                End If


            Next
            Dim view As DataView = New DataView(dtProductos)

            ' Lo ordenamos por el campo Nombre.
            '
            view.Sort = "Promo"
            Dim dtPaso As New DataTable

            Dim foundRows As DataRow() = dtProductos.[Select]("", "Promo asc")
            dtPaso = foundRows.CopyToDataTable()

            'dtProductos = view.Table
            dtProductos = New DataTable
            dtProductos = dtPaso

        End If

        ''  If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then





        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0

        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If
        Try
            If dtProductos.Rows.Count Mod 20 <> 0 Then
                If iTotPaginas * 20 < dtProductos.Rows.Count Then
                    iTotPaginas = iTotPaginas + 1
                End If

            End If
        Catch ex As Exception

        End Try
        If Session("ContPaginas") + 1 = 0 Then
            lblPaginas.Text = "1/" & iTotPaginas
            lblPaginasAbajo.Text = "1/" & iTotPaginas
        Else
            lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
            lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas

        End If

        objDatos.fnLog("Pagina", Session("ContPaginas") + 1 & "/" & iTotPaginas)
        Session("TotCatalogo") = iTotPaginas
        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        objDatos.fnLog("iFinal", iFinal)
        If iFinal > dtProductos.Rows.Count - 1 Or iFinal < 0 Then
            iFinal = dtProductos.Rows.Count - 1

            ' Session("ContPaginas") = 0
        End If

        objDatos.fnLog("FOR", "desde.." & (Session("ContPaginas") * 20) & " hasta " & iFinal)
        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            If Categoria.Replace("%20", " ").ToUpper.Contains("PLUS") Then
                Session("VienePLUS") = "SI"
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProductoPLUS(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

            Else
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
                Session("VienePLUS") = "NO"
            End If

            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1

            objDatos.fnLog("Catalogo - Armado:", dtProductos.Rows(i)("ItemCode"))

            ' sHtmlBanner = sHtmlBanner & "</div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

        objDatos.fnLog("Catalogo - Armado:", "Terminó HTML")
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)

        objDatos.fnLog("Catalogo - Armado:", "Pegó HTML")
    End Sub
    Public Function fnEsMarcaAIO_PMK(Criterio As String)
        Dim iEsmarca As Int16 = 0
        Dim ssql As String
        ssql = ""
        If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Then
            ssql = "SELECT * FROM Sap_tienda..TablaModelos6 where Armadora like '%" & Criterio & "%'"

        End If
        If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
            ssql = "SELECT * FROM [@MODELOS] where u_marca = '" & Criterio & "'"

        End If
        Dim dtDatos As New DataTable
        objDatos.fnLog("EsMarca AIO:", ssql.Replace("'", ""))
        If ssql <> "" Then
            dtDatos = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtDatos.Rows.Count > 0 Then
                iEsmarca = 1
            End If
        End If
        objDatos.fnLog("EsMarca AIO:", iEsmarca)
        Return iEsmarca
    End Function

    Public Function fnEsModeloAIO_PMK(Criterio As String)
        Dim iEsmarca As Int16 = 0
        Dim ssql As String
        ssql = ""
        If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Then
            ssql = "SELECT * FROM Sap_tienda..TablaModelos6 where Modelo like '%" & Criterio & "%'"

        End If
        If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
            ssql = "SELECT * FROM [@MODELOS] where u_Modelo like '%" & Criterio & "%'"

        End If
        Dim dtDatos As New DataTable
        objDatos.fnLog("EsModelo AIO:", ssql.Replace("'", ""))
        If ssql <> "" Then
            dtDatos = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtDatos.Rows.Count > 0 Then
                iEsmarca = 1
            End If
        End If
        objDatos.fnLog("EsModelo AIO:", iEsmarca)
        Return iEsmarca
    End Function


    Public Sub fnCargaBusqueda(Criterio As String)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Criterio = Criterio.ToUpper
        'If Criterio = "" Then
        '    ''Todos
        '    ssql = objDatos.fnObtenerQuery("Productos-Todos")
        'Else
        objDatos.fnLog("catalogo", " fnCargaBusqueda")
        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = objDatos.fnObtenerQuery("QueryBusquedaB2B")
        Else
            ssql = objDatos.fnObtenerQuery("QueryBusqueda")
        End If

        objDatos.fnLog("catalogo", " ssql= " & ssql.Replace("'", ""))
        Dim ssqlBuscar As String = ssql

        '    ''Evaluamos los parametros que nos están enviando
        '    Dim iNivel As Int16 = 1
        '    Dim sQuery As String
        '    Dim sCondicion As String = ""
        '    For Each Param As String In Request.QueryString
        '        sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
        '        Dim dtDatos As New DataTable
        '        dtDatos = objDatos.fnEjecutarConsulta(sQuery)
        '        If dtDatos.Rows.Count > 0 Then
        '            If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
        '                ''Usan el campo de grupo como descripcion
        '                'sCondicion = sCondicion & "T1.ItmsGrpNam=" & "'" & Param & "' AND"
        '                sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
        '            Else
        '                sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
        '            End If
        '        End If
        '        iNivel = iNivel + 1
        '    Next
        '    sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)

        'End If


        Dim sCriterio As String()
        Dim iContadorCriterios As Int16 = 1
        ''------
        sCriterio = Criterio.Split(" ")
        Dim sBusquedaAdicional As String = ""
        For Each valorBuscar As String In sCriterio
            objDatos.fnLog("catalogo", " Entra criterio: " & valorBuscar)
            If valorBuscar <> "" And valorBuscar.ToUpper <> "DE" Then
                iContadorCriterios = iContadorCriterios + 1

                Try
                    Dim iAño As Int16

                    objDatos.fnLog("Busqueda valor:", valorBuscar)
                    iAño = valorBuscar
                    'Si no se rompe, entonces es un año

                    If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Then
                        sBusquedaAdicional = sBusquedaAdicional & iAño & " between AnioDe and AnioHasta AND "
                    End If

                    If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                        sBusquedaAdicional = sBusquedaAdicional & iAño & " between U_AnioDe and U_AnioHasta AND "
                    End If

                Catch ex As Exception
                    If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                        Dim iEntra As Int16 = 0
                        If fnEsMarcaAIO_PMK(valorBuscar) = 1 Then
                            iEntra = 1
                            If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                                sBusquedaAdicional = sBusquedaAdicional & " (  U_Marca = '" & valorBuscar & "') AND "
                            Else
                                sBusquedaAdicional = sBusquedaAdicional & " ( Armadora like '%" & valorBuscar & "%') AND "
                            End If


                        End If

                        If fnEsModeloAIO_PMK(valorBuscar) = 1 Then
                            iEntra = 1
                            If objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                                sBusquedaAdicional = sBusquedaAdicional & " (U_Modelo like '%" & valorBuscar & "%') AND "
                            Else
                                sBusquedaAdicional = sBusquedaAdicional & " (Modelo like '%" & valorBuscar & "%') AND "
                            End If
                        End If
                        If iEntra = 0 Then
                            sBusquedaAdicional = sBusquedaAdicional & "  itemName like '%" & valorBuscar & "%' AND "
                        End If

                    Else
                        If objDatos.fnObtenerDBMS() = "HANA" Then
                            sBusquedaAdicional = sBusquedaAdicional & "  ""ItemName"" like '%" & valorBuscar & "%' AND "
                        Else
                            sBusquedaAdicional = sBusquedaAdicional & "  itemName like '%" & valorBuscar & "%' AND "
                        End If

                    End If


                End Try
            End If

        Next
        sBusquedaAdicional = sBusquedaAdicional & " 1=1  "
        If iContadorCriterios > 1 Then


            If objDatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Or objDatos.fnObtenerCliente.ToUpper.Contains("MANIJ") Then
                ssql = ssql.Replace("@SearchText", Criterio)
                sBusquedaAdicional = sBusquedaAdicional.Replace("AND  1=1", "  ")
                ssql = ssql & " OR (" & sBusquedaAdicional & ") order by ""ItemName"" desc"
            Else
                ssql = ssql.Replace("@SearchText", "'" & Criterio & "'")
                sBusquedaAdicional = sBusquedaAdicional.Replace("AND  1=1", " AND (qrygroup60='Y' or QryGroup61='Y') ")
                ssql = ssql & " OR (" & sBusquedaAdicional & ") order by ItemName desc"
            End If

        Else
            ssql = ssql.Replace("@SearchText", "''")
        End If


        ''-----

        'If Criterio.Contains(" ") Then

        '    sCriterio = Criterio.Split(" ")

        '    For Each valorBuscar As String In sCriterio

        '        If valorBuscar <> "" Then
        '            If iContadorCriterios = 1 Then
        '                ssql = ssql.Replace("@SearchText", "'" & valorBuscar & "'")
        '            Else
        '                ssql = ssql & " UNION ALL " & ssqlBuscar.Replace("@SearchText", "'" & valorBuscar & "'")
        '            End If
        '            iContadorCriterios = iContadorCriterios + 1

        '        End If

        '    Next
        '    'ssql = ssql.Substring(0, ssql.Length - 1)
        '    'ssql = ssql.Replace("|", " UNION ALL ")

        'Else
        '    If Session("TipoDBMS") = "HANA" Then
        '        ssql = ssql.Replace("@SearchText", Criterio.ToUpper)
        '    Else
        '        ssql = ssql.Replace("@SearchText", "'" & Criterio & "'")
        '    End If

        'End If

        objDatos.fnLog("Buscar", ssql.Replace("'", ""))
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)

        If dtProductos.Rows.Count = 0 Then
            lblSinResultados.Text = "No se han encontrado resultados con la búsqueda indicada"
            lblSinResultados.Visible = True
        End If
        'Session("sesBuscar") = ""
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0
        Dim sPaginas As String = ""
        sPaginas = Math.Round(dtProductos.Rows.Count / 20, 2)
        objDatos.fnLog("sPaginas", sPaginas)
        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If



        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        If sPaginas.Contains(".") Or sPaginas.Contains(",") Then
            Dim sDecimales As String()
            If sPaginas.Contains(",") Then
                sDecimales = sPaginas.Split(",")
            Else
                sDecimales = sPaginas.Split(".")
            End If

            If CInt(sDecimales(1)) > 50 Then
                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                iTotPaginas = iTotPaginas + 1
                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                Session("TotCatalogo") = Session("TotCatalogo") + 1
            End If
        End If

        lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas

        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Then
            iFinal = dtProductos.Rows.Count - 1
        End If

        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Then
                'sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProductoPMK(dtProductos.Rows(i)("ItemCode"))
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
            Else
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))


            End If


            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1

            objDatos.fnLog("Catalogo - Armado:", dtProductos.Rows(i)("ItemCode"))
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub



    Public Sub fnCargaBusquedaAIO_PMK_Original(Criterio As String)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        'If Criterio = "" Then
        '    ''Todos
        '    ssql = objDatos.fnObtenerQuery("Productos-Todos")
        'Else
        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = objDatos.fnObtenerQuery("QueryBusquedaAIOPMKB2B")
        Else
            ssql = objDatos.fnObtenerQuery("QueryBusquedaAIOPMK")
        End If
        Dim ssqlBuscar As String = ssql

        ssql = ssql & Criterio

        objDatos.fnLog("Buscar", ssql.Replace("'", ""))
        ' dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)

        If dtProductos.Rows.Count = 0 Then
            lblSinResultados.Text = "No se han encontrado resultados con la búsqueda indicada"
            lblSinResultados.Visible = True
        Else
            lblSinResultados.Visible = False
        End If
        'Session("sesBuscar") = ""
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0
        Dim sPaginas As String = ""
        sPaginas = Math.Round(dtProductos.Rows.Count / 20, 2)
        objDatos.fnLog("sPaginas", sPaginas)
        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If

        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        If sPaginas.Contains(".") Or sPaginas.Contains(",") Then
            Dim sDecimales As String()
            If sPaginas.Contains(",") Then
                sDecimales = sPaginas.Split(",")
            Else
                sDecimales = sPaginas.Split(".")
            End If

            If CInt(sDecimales(1)) > 50 Then
                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                iTotPaginas = iTotPaginas + 1
                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                Session("TotCatalogo") = Session("TotCatalogo") + 1
            End If
        End If

        lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas

        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Then
            iFinal = dtProductos.Rows.Count - 1
        End If

        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

            ''' 
            ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
            ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
            ''sHtmlBanner = sHtmlBanner & "  <div class='img'>"
            ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"

            ''If objDatos.fnArticuloOferta(dtProductos.Rows(i)("ItemCode")) = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
            ''End If


            ''Dim sPintaPrev As String = "SI"
            ''Dim sPintaFav As String = "SI"
            ''Dim sPintaCompra As String = "SI"

            ''ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
            ''Dim dtPintaCat As New DataTable
            ''dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
            ''If dtPintaCat.Rows.Count > 0 Then
            ''    sPintaPrev = dtPintaCat.Rows(0)("Interior")
            ''    sPintaFav = dtPintaCat.Rows(0)("Favorito")
            ''    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
            ''End If

            ''If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
            ''Else
            ''    sHtmlBanner = sHtmlBanner & "     <div>"
            ''End If

            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'></a>"
            ''End If
            ''If sPintaFav = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&name=" & dtProductos.Rows(i)("ItemName") & "'></a>"
            ''End If
            ''If sPintaCompra = "SI" Then
            ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "&Modo=Add'></a>"
            ''End If



            ''sHtmlBanner = sHtmlBanner & "     </div>"
            ''sHtmlBanner = sHtmlBanner & "  </div>"

            ''If CInt(Session("slpCode")) <> 0 Then

            ''    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''Else
            ''    'If Session("UserB2C") = "" Then
            ''    If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''    Else
            ''        If Session("UserB2C") <> "" Then
            ''            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''        Else
            ''            dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''        End If
            ''    End If

            ''    If Session("Cliente") <> "" Then
            ''        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''    End If

            ''    ' Else
            ''    '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''    'End If

            ''End If


            ''sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ " & dPrecioActual.ToString("###,###,###.#0") & " " & fnObtenerMoneda(dtProductos.Rows(i)("ItemCode")) & "</span>"
            ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("+", "%2b") & "'>"
            ''''img/home/producto-1.png
            ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
            ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then

            ''        Dim sValorMostrar As String = ""
            ''        If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
            ''            sValorMostrar = ""
            ''        Else
            ''            sValorMostrar = CStr(dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")))
            ''        End If

            ''        'If sValorMostrar.Length > 30 Then
            ''        '    sValorMostrar = sValorMostrar.Substring(0, 30)
            ''        'End If



            ''        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
            ''            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & sValorMostrar & "</div>"
            ''        Else
            ''            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & sValorMostrar & "</div>"
            ''        End If
            ''    Else
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then


            ''            If CInt(Session("slpCode")) <> 0 Then

            ''                dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''            Else
            ''                'If Session("UserB2C") = "" Then
            ''                If Session("UserB2C") = "" And Session("UserB2B") = "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                Else
            ''                    If Session("UserB2C") <> "" Then
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                    Else
            ''                        dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                    End If
            ''                End If

            ''                If Session("Cliente") <> "" Then
            ''                    dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"), Session("ListaPrecios"))
            ''                End If

            ''                ' Else
            ''                '  dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))
            ''                'End If

            ''            End If

            ''            ' dPrecioActual = objDatos.fnPrecioActual(dtProductos.Rows(i)("ItemCode"))

            ''        End If
            ''        If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
            ''            Dim iband As Int16 = 0
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("-", "") & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & CStr(dtProductos.Rows(i)("ItemCode")).Replace("-", "") & ".jpg"
            ''            End If
            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtProductos.Rows(i)("ItemCode")) & ".jpg") Then
            ''                iband = 1
            ''                sImagen = "images/products/" & CStr(dtProductos.Rows(i)("ItemCode")) & ".jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-1.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-1.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-2.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-2.jpg"
            ''            End If

            ''            If File.Exists(Server.MapPath("~") & "\images\products\" & dtProductos.Rows(i)("ItemCode") & "-3.jpg") And iband = 0 Then
            ''                iband = 1
            ''                sImagen = "images/products/" & dtProductos.Rows(i)("ItemCode") & "-3.jpg"
            ''            End If

            ''            objDatos.fnLog("imagen", sImagen)
            ''            Try
            ''                ' sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                If iband = 0 Then
            ''                    If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
            ''                        sImagen = "images/no-image.png"
            ''                    Else
            ''                        If dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) = "" Then
            ''                            sImagen = "images/no-image.png"
            ''                        Else
            ''                            sImagen = dtProductos.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
            ''                        End If

            ''                    End If
            ''                End If


            ''            Catch ex As Exception
            ''                sImagen = "images/no-image.png"
            ''            End Try

            ''            sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
            ''        End If
            ''    End If

            ''Next


            ''sHtmlBanner = sHtmlBanner & "  </a>"
            ''sHtmlBanner = sHtmlBanner & " </div>"

            ''sHtmlBanner = sHtmlBanner & "  </a>"

            ''sHtmlBanner = sHtmlBanner & "</div>"

            '      
            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1

            objDatos.fnLog("Catalogo - Armado:", dtProductos.Rows(i)("ItemCode"))
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaBusquedaMANIJ(Criterio As String)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        'If Criterio = "" Then
        '    ''Todos
        '    ssql = objDatos.fnObtenerQuery("Productos-Todos")
        'Else
        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = objDatos.fnObtenerQuery("QueryBusquedaAIOPMKB2B")
        Else
            objDatos.fnLog("Buscar", "QueryBusquedaAIOPMK_v2")
            ssql = objDatos.fnObtenerQuery("QueryBusquedaAIOPMK_v2")
        End If
        Dim ssqlBuscar As String = ssql

        ssql = ssql & Criterio

        If Criterio = "Novedades" Then
            Session("Novedades") = "SI"
            ssql = objDatos.fnObtenerQuery("Novedades")
        End If

        ssql = ssql & " Order by ItemName "
        objDatos.fnLog("Buscar", ssql.Replace("'", ""))
        ' dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        dtProductos = objDatos.fnEjecutarConsulta(ssql)

        If dtProductos.Rows.Count = 0 Then
            lblSinResultados.Text = "No se han encontrado resultados con la búsqueda indicada"
            lblSinResultados.Visible = True
        Else
            lblSinResultados.Visible = False
        End If
        'Session("sesBuscar") = ""
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0
        Dim sPaginas As String = ""
        sPaginas = Math.Round(dtProductos.Rows.Count / 20, 2)
        objDatos.fnLog("sPaginas", sPaginas)
        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If

        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        If sPaginas.Contains(".") Or sPaginas.Contains(",") Then
            Dim sDecimales As String()
            If sPaginas.Contains(",") Then
                sDecimales = sPaginas.Split(",")
            Else
                sDecimales = sPaginas.Split(".")
            End If

            If CInt(sDecimales(1)) > 50 Then

                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                iTotPaginas = iTotPaginas + 1
                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                Session("TotCatalogo") = Session("TotCatalogo") + 1
            End If
        End If

        lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas

        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Then
            iFinal = dtProductos.Rows.Count - 1
        End If

        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
            Else
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

                ''AIO Cargar con el HTML en la tablaModelo6
                '  sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProductoPMK(dtProductos.Rows(i)("ItemCode"))
            End If
            '  


            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1

            objDatos.fnLog("Catalogo - Armado:", dtProductos.Rows(i)("ItemCode"))
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaBusquedaAIO_PMK(Criterio As String)
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        'If Criterio = "" Then
        '    ''Todos
        '    ssql = objDatos.fnObtenerQuery("Productos-Todos")
        'Else
        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            ssql = objDatos.fnObtenerQuery("QueryBusquedaAIOPMKB2B")
        Else
            objDatos.fnLog("Buscar", "QueryBusquedaAIOPMK_v2")
            ssql = objDatos.fnObtenerQuery("QueryBusquedaAIOPMK_v2")
        End If
        Dim ssqlBuscar As String = ssql

        ssql = ssql & Criterio

        If Criterio = "Novedades" Then
            Session("Novedades") = "SI"
            ssql = objDatos.fnObtenerQuery("Novedades")
        End If

        ssql = ssql & " Order by ItemName "
        objDatos.fnLog("Buscar", ssql.Replace("'", ""))
        ' dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
        dtProductos = objDatos.fnEjecutarConsulta(ssql)

        If dtProductos.Rows.Count = 0 Then
            lblSinResultados.Text = "No se han encontrado resultados con la búsqueda indicada"
            lblSinResultados.Visible = True
        Else
            lblSinResultados.Visible = False
        End If
        'Session("sesBuscar") = ""
        '   sHtmlEncabezado = "<div class='col-xs-12 col-sm-10 stl-1-p'>"
        Dim iContador As Int16 = 0
        Dim iPuesto As Int16 = 0
        lblResultados.Text = dtProductos.Rows.Count & " resultados "
        lblResultadosAbajo.Text = dtProductos.Rows.Count & " resultados "
        Dim iTotPaginas As Int16 = 0
        Dim sPaginas As String = ""
        sPaginas = Math.Round(dtProductos.Rows.Count / 20, 2)
        objDatos.fnLog("sPaginas", sPaginas)
        If dtProductos.Rows.Count <= 20 Then
            iTotPaginas = 1
        Else
            iTotPaginas = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        End If

        Session("TotCatalogo") = CInt(Math.Round(dtProductos.Rows.Count / 20, 0))
        If sPaginas.Contains(".") Or sPaginas.Contains(",") Then
            Dim sDecimales As String()
            If sPaginas.Contains(",") Then
                sDecimales = sPaginas.Split(",")
            Else
                sDecimales = sPaginas.Split(".")
            End If

            If CInt(sDecimales(1)) > 50 Then

                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                iTotPaginas = iTotPaginas + 1
                objDatos.fnLog("sPaginas iTotPaginas", iTotPaginas)
                Session("TotCatalogo") = Session("TotCatalogo") + 1
            End If
        End If

        lblPaginas.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas
        lblPaginasAbajo.Text = Session("ContPaginas") + 1 & "/" & iTotPaginas

        ''Paginamos

        Dim iFinal As Int16 = 0
        iFinal = (Session("ContPaginas") * 20) + 19
        If iFinal > dtProductos.Rows.Count - 1 Then
            iFinal = dtProductos.Rows.Count - 1
        End If

        For i = (Session("ContPaginas") * 20) To iFinal Step 1
            '   sHtmlBanner = sHtmlBanner & "<div class='row'>"
            ''Nos traemos los datos a mostrar de acuerdo a la plantilla "INFO-Productos Relacionados"
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='INFO-Productos Relacionados' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            If iPuesto = 0 And iContador = 0 And dtProductos.Rows.Count > 4 Then
                sHtmlBanner = sHtmlBanner & "<div class='cont-p-4'>"
                iPuesto = 1
            End If

            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
            Else
                sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtProductos.Rows(i)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))

                ''AIO Cargar con el HTML en la tablaModelo6
                '  sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProductoPMK(dtProductos.Rows(i)("ItemCode"))
            End If
            '  


            If dtProductos.Rows.Count < 4 Then
                iContador = 3
            End If

            If iContador = 3 And dtProductos.Rows.Count > 4 Then
                iPuesto = 0
                iContador = -1
                sHtmlBanner = sHtmlBanner & "</div>"
            End If
            iContador = iContador + 1

            objDatos.fnLog("Catalogo - Armado:", dtProductos.Rows(i)("ItemCode"))
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        ' sHtmlEncabezado = sHtmlEncabezado & "</div>"
        '      sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"
        Dim Literal As New LiteralControl(sHtmlEncabezado)
        pnlProductos.Controls.Clear()
        pnlProductos.Controls.Add(Literal)
    End Sub

    Public Sub fnCargaFiltros()


        Dim iNivel As Int16 = 1
        Dim sQuery As String
        Dim sCondicion As String = ""
        If Request.QueryString.Count > 0 Then
            If Request.QueryString(0) <> "COLECCIÓN" And Request.QueryString(0) <> "Productos" Then
                For Each Param As String In Request.QueryString
                    sQuery = "select cvCampoSAP  from config.NivelesArticulos WHERE cvEstatus='ACTIVO' AND ciOrden=" & "'" & iNivel & "'"
                    Dim dtDatos As New DataTable
                    dtDatos = objDatos.fnEjecutarConsulta(sQuery)
                    If dtDatos.Rows.Count > 0 Then
                        If dtDatos.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                            ''Usan el campo de grupo como descripcion
                            'sCondicion = sCondicion & "T1.ItmsGrpNam=" & "'" & Param & "' AND"
                            sCondicion = sCondicion & "(CAST(T0." & dtDatos.Rows(0)(0) & " as varchar) =" & "'" & Request.QueryString(Param) & "' OR T1.ItmsGrpNam =" & "'" & Request.QueryString(Param) & "') AND "
                        Else
                            'sCondicion = sCondicion & "T0." & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                            sCondicion = sCondicion & "" & dtDatos.Rows(0)(0) & "=" & "'" & Request.QueryString(Param) & "' AND "
                        End If
                    End If
                    iNivel = iNivel + 1
                Next
            End If
        End If

        If sCondicion <> "" Then
            sCondicion = sCondicion.Substring(0, sCondicion.Length - 4)
        End If


        If Request.QueryString.Count > 0 Then
            If CStr(Request.QueryString(0).Contains("+")) Then
                sCondicion = "  U_Tie_Talla in ('EXTRA GRANDE','EXTRA EXTRA GRANDE')"
            Else
                Dim ssqlEspecial As String
                ssqlEspecial = "select * from config.categoriasEsp where cvDescripcion='" & CStr(Request.QueryString(0)).Replace("%20", " ") & "'"
                Dim dtEspecial As New DataTable
                dtEspecial = objDatos.fnEjecutarConsulta(ssqlEspecial)
                If dtEspecial.Rows.Count > 0 Then
                    ''es especial

                    sCondicion = "  " & dtEspecial.Rows(0)("cvCampoSAP") & " = '" & CStr(Request.QueryString(0)) & "'"
                    If Request.QueryString.Count > 1 Then
                        sCondicion = sCondicion & " AND " & dtEspecial.Rows(0)("cvCampoSAPNiv2") & "=" & "'" & Request.QueryString(1) & "'"
                    End If

                End If


            End If

        End If



        ''Obtenemos los filtros a mostrar
        ssql = "select cvFiltro,cvQuery,cvCampo from config.productosFiltro"
        Dim dtFiltros As New DataTable
        dtFiltros = objDatos.fnEjecutarConsulta(ssql)
        If dtFiltros.Rows.Count > 0 Then
            pnlFiltros.Visible = True
        End If
        For i = 0 To dtFiltros.Rows.Count - 1 Step 1
            '  objDatos.fnLog("Filtro ", dtFiltros.Rows(i)("cvFiltro"))
            'If i = 0 Then
            '    chkLista_1.Visible = True
            '    lbl_1.Visible = True
            '    lbl_1.Text = dtFiltros.Rows(i)("cvFiltro")
            '    ssql = objDatos.fnObtenerQuery(dtFiltros.Rows(i)("cvQuery"))
            '    Dim dtItemsFiltros As New DataTable
            '    dtItemsFiltros = objDatos.fnEjecutarConsultaSAP(ssql)
            '    For x = 0 To dtItemsFiltros.Rows.Count - 1 Step 1
            '        chkLista_1.Items.Add(dtItemsFiltros.Rows(x)(dtFiltros.Rows(i)("cvCampo")))
            '    Next
            'End If

            For Each control As System.Web.UI.Control In pnlFiltros.Controls
                Try
                    ' objDatos.fnLog("Filtro Control", control.ID)
                    If control.ID.Length > 0 Then
                        If control.ID.Contains("chkLista_" & (i + 1)) Or control.ID.Contains("pnlFiltro" & (i + 1)) Then
                            control.Visible = True
                            ssql = objDatos.fnObtenerQuery(dtFiltros.Rows(i)("cvQuery"))
                            If sCondicion <> "" Then
                                ssql = ssql & " AND " & sCondicion
                            End If


                            Dim dtItemsFiltros As New DataTable

                            objDatos.fnLog("QueryFiltro", ssql.Replace("'", ""))
                            If i = 0 Then
                                pnlFiltro1.Visible = True
                                control = chkLista_1
                            End If
                            If i = 1 Then
                                pnlFiltro2.Visible = True
                                control = chkLista_2
                            End If
                            If i = 2 Then
                                pnlFiltro3.Visible = True
                                control = chkLista_3
                            End If
                            If i = 3 Then
                                pnlFiltro4.Visible = True
                                control = chkLista_4
                            End If
                            If i = 4 Then
                                pnlFiltro5.Visible = True
                                control = chkLista_5
                            End If
                            If i = 5 Then
                                pnlFiltro6.Visible = True
                                control = chkLista_6
                            End If
                            If i = 6 Then
                                pnlFiltro7.Visible = True
                                control = chkLista_7
                            End If
                            If i = 7 Then
                                pnlFiltro8.Visible = True
                                control = chkLista_8
                            End If

                            If dtFiltros.Rows(i)("cvFiltro") = "Precio" Then
                                ssql = "SELECT cvFiltro,cfRangoInicial,cfRangoFinal FROM config.Filtros_Precio WHERE cvEstatus='ACTIVO' Order by cfRangoInicial"
                                dtItemsFiltros = objDatos.fnEjecutarConsulta(ssql)
                                For x = 0 To dtItemsFiltros.Rows.Count - 1 Step 1
                                    '    objDatos.fnLog("ItemsFiltro", dtItemsFiltros.Rows(x)(dtFiltros.Rows(i)("cvCampo")))
                                    DirectCast(control, System.Web.UI.WebControls.CheckBoxList).Items.Add(dtItemsFiltros.Rows(x)("cvFiltro"))
                                Next
                            Else
                                '  objDatos.fnLog("ItemsFiltro CON CONDICION", ssql.Replace("'", ""))

                                ssql = ssql & " Order by Orden "
                                dtItemsFiltros = objDatos.fnEjecutarConsultaSAP(ssql)

                                If dtItemsFiltros.Rows.Count = 0 Then
                                    ssql = ssql.Replace("Order by Orden ", "")
                                    dtItemsFiltros = objDatos.fnEjecutarConsultaSAP(ssql)
                                End If
                                For x = 0 To dtItemsFiltros.Rows.Count - 1 Step 1
                                    '    objDatos.fnLog("ItemsFiltro", dtItemsFiltros.Rows(x)(dtFiltros.Rows(i)("cvCampo")))
                                    DirectCast(control, System.Web.UI.WebControls.CheckBoxList).Items.Add(dtItemsFiltros.Rows(x)(dtFiltros.Rows(i)("cvCampo")))
                                Next
                            End If


                        End If


                        If i = 0 Then
                            lbl_1.Visible = True
                            lbl_1.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 1 Then
                            lbl_2.Visible = True
                            lbl_2.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 2 Then
                            lbl_3.Visible = True
                            lbl_3.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 3 Then
                            lbl_4.Visible = True
                            lbl_4.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 4 Then
                            lbl_5.Visible = True
                            lbl_5.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 5 Then
                            lbl_6.Visible = True
                            lbl_6.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 6 Then
                            lbl_7.Visible = True
                            lbl_7.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If
                        If i = 7 Then
                            lbl_8.Visible = True
                            lbl_8.Text = dtFiltros.Rows(i)("cvFiltro")
                        End If


                        'If control.ID.Contains("pnlFiltro" & (i + 1)) Then
                        '    For Each subControl In control.Controls
                        '        If subControl.ID.Contains("lbl_" & (i + 1)) Then
                        '            DirectCast(subControl, System.Web.UI.WebControls.Label).Text = dtFiltros.Rows(i)("cvFiltro")
                        '        End If
                        '    Next

                        'End If

                    End If

                Catch ex As Exception
                    '    objDatos.fnLog("Filtro Ex", ex.Message)
                End Try

            Next
        Next


        '''Por el momento, marcas
        'ssql = "select firmCode,FirmName from OMRC where firmCode <> -1"
        'Dim dtMarcas As New DataTable
        'dtMarcas = objDatos.fnEjecutarConsultaSAP(ssql)
        'For i = 0 To dtMarcas.Rows.Count - 1 Step 1
        '    chkMarcas.Items.Add(dtMarcas.Rows(i)("FirmName"))
        'Next
    End Sub

    Public Function fnCategoriasEspeciales() As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim iExisteTercerNivel As Int16 = 0
        Dim sLinkPrimerNivel As String = ""
        Dim sValorPintar As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP,cvQuery, ISNULL(cvQueryNiv2,'') as QueryNiv2,ISNULL(cvCampoSAPNiv2,'') as CampoSAPNiv2  from config.CategoriasEsp where ciEstatus =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            For i = 0 To dtPrimerNivel.Rows.Count - 1 Step 1
                sHtmlEncabezado = sHtmlEncabezado & " <div class='panel-heading'>"

                sHtmlEncabezado = sHtmlEncabezado & " <h4 class='panel'>"
                If CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper.Contains("REBAJA") Then
                    sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='color:red; font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'>" & CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper.Replace("PLUS", "<span style='font-size:16px;'>+</span>") & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#menu-collapse" & (i + 1) & "' class='collapse' aria-expanded='true'></a>"
                Else

                    sHtmlEncabezado = sHtmlEncabezado & "  <span class='link-subcategoria'  style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & dtPrimerNivel.Rows(i)("cvDescripcion") & "'>" & CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).ToUpper.Replace("PLUS", "<span style='font-size:16px;'>+</span>") & "</span>  <a data-toggle='collapse'  data-parent='#accordion' href='#menu-collapse" & (i + 1) & "' class='collapse' aria-expanded='true'></a>"
                End If


                sHtmlEncabezado = sHtmlEncabezado & "   </h4>"

                sHtmlEncabezado = sHtmlEncabezado & "   </div>"

                sLinkPrimerNivel = dtPrimerNivel.Rows(i)("cvDescripcion")



                sHtmlBanner = ""
                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                If dtPrimerNivel.Rows(i)("QueryNiv2") <> "" Then


                    Dim sExpand As String = "CLASSMODALIDAD" 'class='panel-collapse collapse in' aria-expanded='true'
                    sHtmlBanner = sHtmlBanner & " <div id='menu-collapse" & (i + 1) & "'" & sExpand & "> "


                    ' sHtmlBanner = sHtmlBanner & " <div id='menu-collapse" & (i + 1) & "' class='panel-collapse collapse'> "
                    sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "


                    Dim sCategoriaPadre As String = CStr(dtPrimerNivel.Rows(i)("cvDescripcion")).Replace(" ", "-").Replace(",", "-")

                    Dim dtSubcategoria As New DataTable
                    dtSubcategoria = objDatos.fnEjecutarConsultaSAP(objDatos.fnObtenerQuery(dtPrimerNivel.Rows(i)("QueryNiv2")))

                    For x = 0 To dtSubcategoria.Rows.Count - 1 Step 1
                        If dtSubcategoria.Rows(x)(0) Is DBNull.Value Then
                            sValorPintar = ""
                        Else
                            sValorPintar = dtSubcategoria.Rows(x)(0)
                        End If

                        If iExisteTercerNivel = 0 Then
                            '  objDatos.fnLog("Catalogo", "SoloNivel 2: " & sValorPintar)
                            sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 no-padding'> "

                            sHtmlBanner = sHtmlBanner.Replace("CLASSMODALIDAD", "class='panel-collapse collapse' aria-expanded='true'")
                            sHtmlBanner = sHtmlBanner & " <a class='active' data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtPrimerNivel.Rows(i)("CampoSAPNiv2") & "=" & dtSubcategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"

                            'If Param2 <> "" Then
                            '    If Param2 = dtSubCategoria.Rows(x)(0) Then
                            '        sHtmlBanner = sHtmlBanner.Replace("CLASSMODALIDAD", "class='panel-collapse collapse' aria-expanded='true'")
                            '        sHtmlBanner = sHtmlBanner & " <a class='active' data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtPrimerNivel.Rows(i)("CampoSAPNiv2") & "=" & dtSubcategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                            '    Else
                            '        sHtmlBanner = sHtmlBanner.Replace("CLASSMODALIDAD", "class='panel-collapse collapse'")
                            '        sHtmlBanner = sHtmlBanner & " <a data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                            '    End If

                            'Else
                            '    sHtmlBanner = sHtmlBanner & " <a data-grafica='1' href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</a>"
                            'End If


                            ' sHtmlBanner = sHtmlBanner & " </a>"
                            sHtmlBanner = sHtmlBanner & "</div>"

                        Else

                        End If

                    Next

                    sHtmlBanner = sHtmlBanner & "  </div>"
                    sHtmlBanner = sHtmlBanner & " </div>"
                End If
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            Next
        End If


        'sHtmlEncabezado = sHtmlEncabezado & "  <div class='panel-heading' role='tab' id='heading" & (i + 1) & "'>"
        'sHtmlEncabezado = sHtmlEncabezado & "   <h4 class='categoria'>"


        Return sHtmlEncabezado
    End Function

    Public Sub fnCargaCategorias()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sHtmlMenu As String = ""

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


        ssql = "Select cvHTML From config.HTML where cvTipo='Categorias-lateral'"
        Dim dtHTML As New DataTable
        dtHTML = objDatos.fnEjecutarConsulta(ssql)

        If dtHTML.Rows.Count > 0 Then
            'Dim re As StreamReader = File.OpenText("categorias.txt")
            'Dim entrada As String = ""
            'Dim texto As String = ""
            'While ((entrada = re.ReadLine()) <> Nothing)
            '    texto += entrada
            'End While
            'sHtmlMenu = sHtmlMenu & texto
            sHtmlMenu = sHtmlMenu & dtHTML.Rows(0)(0)
            Dim Literal As New LiteralControl(sHtmlMenu)
            pnlCategorias.Controls.Clear()
            pnlCategorias.Controls.Add(Literal)

            Exit Sub
        End If


        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                If Session("RazonSocial") <> "" Then
                    ssql = objDatos.fnObtenerQuery("CategoriasB2B")
                Else
                    ssql = objDatos.fnObtenerQuery("Categorias")
                End If
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(1))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                Dim sValorPintar As String = ""
                objDatos.fnLog("Categorias Cat", "Inicia")
                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objDatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                End If

                Dim sCampoPrimerNivel As String = ""
                ''Nos traemos el nivel 2 de la categoría seleccionada
                ssql = objDatos.fnObtenerQuery("Categorias-det")
                If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                    ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                    sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                Else
                    If objDatos.fnObtenerDBMS = "HANA" Then
                        objDatos.fnLog("Categorias Cat", "HANA 1")
                        ssql = ssql.Replace("[%1]", "TO_VARCHAR(T0.""" & dtPrimerNivel.Rows(0)("cvCampoSAP") & """)=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sCampoPrimerNivel = "TO_VARCHAR(T0.""" & dtPrimerNivel.Rows(0)("cvCampoSAP") & """)=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sCampoPrimerNivel = "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'"
                    End If


                    sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                End If

                '-Heading
                sHtmlEncabezado = sHtmlEncabezado & " <div class='panel#SUBCAT#'>"

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


                objDatos.fnLog("Categorias Cat", "HANA 2")
                Dim iExisteTercerNivel As Int16 = 0

                Dim sQueryTercer As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =3 AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtTercerNivel As New DataTable
                dtTercerNivel = objDatos.fnEjecutarConsulta(sQueryTercer)
                If dtTercerNivel.Rows.Count > 0 Then
                    iExisteTercerNivel = 1
                End If
                sHtmlBanner = ""
                Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2 AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtSegundoNivel As New DataTable
                dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                If dtSegundoNivel.Rows.Count > 0 Then
                    objDatos.fnLog("Categorias Cat", "HANA existe 2 nivel")
                    sHtmlEncabezado = sHtmlEncabezado.Replace("#SUBCAT#", "-heading")
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        If objDatos.fnObtenerDBMS = "HANA" Then
                            ssql = ssql.Replace("[%0]", "TO_VARCHAR(T0.""" & dtSegundoNivel.Rows(0)("cvCampoSAP") & """)")
                        Else
                            ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                        End If

                    End If
                    '  objDatos.fnLog("Segundo nivel", ssql.Replace("'", ""))

                    ' objDatos.fnLog("Evaluar tercer nivel", ssql.Replace("'", ""))
                    If iExisteTercerNivel = 1 Then

                        '     objDatos.fnLog("Query tercer nivel", sQueryTercer.Replace("'", ""))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)

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
                            objDatos.fnLog("Categorias Cat", "HANA existe 2 nivel CampoUsuarioNiv2")
                            ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv2")
                            ssql = ssql.Replace("[%0]", dtSubCategoria.Rows(x)(0))
                            Dim dtValor As New DataTable
                            dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
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
                            objDatos.fnLog("Categorias Cat", "HANA existe 3 nivel Categorias-Tercero")
                            sQueryTercer = objDatos.fnObtenerQuery("Categorias-Tercero")

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
                                    sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;font-weight: bold;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & sCategoriaPadre & "' class='collapse' aria-expanded='true'></a>"
                                Else
                                    sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & sCategoriaPadre & "' class='collapsed'></a>"
                                End If
                            Else
                                sHtmlBanner = sHtmlBanner & "      <span class='link-subcategoria' style='font-size:  13px;cursor:     pointer;' data-href='Catalogo.aspx?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "'>" & sValorPintar & "</span>  <a data-toggle='collapse' data-parent='#accordion-" & (i + 1) & "' href='#menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & sCategoriaPadre & "' class='collapsed'></a>"
                            End If

                            sHtmlBanner = sHtmlBanner & "      </h4>"
                            sHtmlBanner = sHtmlBanner & "    </div>"

                            ''Leemos el tercer nivel

                            If Param2 <> "" Then
                                If Param2 = dtSubCategoria.Rows(x)(0) Then
                                    sHtmlBanner = sHtmlBanner & " <div id='menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & sCategoriaPadre & "' class='panel-collapse collapse' aria-extended='true'>"
                                Else
                                    sHtmlBanner = sHtmlBanner & " <div id='menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & sCategoriaPadre & "' class='panel-collapse collapse '>"
                                End If

                            Else
                                sHtmlBanner = sHtmlBanner & " <div id='menu-collapse-" & sValorPintar.Replace(" ", "_").Replace(",", "_").Replace("/", "") & sCategoriaPadre & "' class='panel-collapse collapse '>"
                            End If

                            sHtmlBanner = sHtmlBanner & "  <div class='panel-body'> "

                            Dim dtTercer As New DataTable
                            dtTercer = objDatos.fnEjecutarConsultaSAP(sQueryTercer)
                            '     objDatos.fnLog("Filas tercer nivel", dtTercer.Rows.Count)

                            For y = 0 To dtTercer.Rows.Count - 1 Step 1
                                '    objDatos.fnLog("tercer nivel", dtTercer.Rows(y)(0))
                                sValorPintar = ""
                                If CStr(dtTercerNivel.Rows(0)("cvCampoSAP")).Contains("U_") Then
                                    ssql = objDatos.fnObtenerQuery("CampoUsuarioNiv3")
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
                Else
                    ''No tiene hijos
                    sHtmlEncabezado = sHtmlEncabezado.Replace("#SUBCAT#", "")
                End If
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner



                '' End If

            Next
            objDatos.fnLog("Categorias Cat", "HANA pega html categorias")
            sHtmlEncabezado = sHtmlEncabezado & fnCategoriasEspeciales()
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            pnlCategorias.Controls.Clear()
            pnlCategorias.Controls.Add(Literal)
            objDatos.fnLog("Categorias Cat", "HANA termina html categorias")
            pnlCategorias.Visible = True

        End If

    End Sub
    Protected Sub btnPagina_Click(sender As Object, e As EventArgs) Handles btnPagina.Click
        Session("ContPaginas") = Session("ContPaginas") - 1
        If Session("ContPaginas") < 1 Then
            Session("ContPaginas") = 0
        End If
        Dim iParametros As Int16 = 0
        If Request.QueryString.Count = 0 Then
            iParametros = 0
        Else
            If Request.QueryString(0) = "Novedades" And Session("Novedades") = "NO" Then
                iParametros = 0
            Else
                iParametros = 1
            End If
        End If
        Try
            If iParametros = 0 Then
                If Session("sesBuscar") <> "" Then
                    fnCargaBusqueda(Session("sesBuscar"))
                Else
                    objDatos.fnLog("btnSiguiente", "PMK")
                    If Session("BusquedaAIO_Index") = "SI" Then
                        fnCargaBusquedaAIO_PMK(Session("BusquedaAIO_Criterios"))

                    Else

                        fnCargaCatalogo("")


                    End If


                End If



            Else
                objDatos.fnLog("btnSiguiente_sesBuscar", Session("sesBuscar"))
                If Session("Novedades") = "SI" Then
                    objDatos.fnLog("btnSiguiente", "Novedades")
                    fnCargaBusquedaAIO_PMK("Novedades")
                Else


                    fnCargaCatalogo(Request.QueryString("Cat"), ddlOrden.SelectedItem.Text)

                End If


                ' fnCargaCatalogo(Request.QueryString("Cat"))

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSiguiente_Click(sender As Object, e As EventArgs) Handles btnSiguiente.Click
        Session("ContPaginas") = Session("ContPaginas") + 1
        If Session("ContPaginas") >= Session("TotCatalogo") Then
            Session("ContPaginas") = Session("TotCatalogo") - 1
        End If
        objDatos.fnLog("ContPaginas", Session("ContPaginas"))
        'If CInt(lblResultados.Text) <= 20 Then
        '    Session("ContPaginas") = 1

        'End If

        Dim iParametros As Int16 = 0
        If Request.QueryString.Count = 0 Then
            iParametros = 0
        Else
            If Request.QueryString(0) = "Novedades" And Session("Novedades") = "NO" Then
                iParametros = 0
            Else
                iParametros = 1
            End If
        End If
        Try
            If iParametros = 0 Then
                If Session("sesBuscar") <> "" Then
                    fnCargaBusqueda(Session("sesBuscar"))
                Else
                    objDatos.fnLog("btnSiguiente", "PMK")
                    If Session("BusquedaAIO_Index") = "SI" Then
                        fnCargaBusquedaAIO_PMK(Session("BusquedaAIO_Criterios"))

                    Else

                        fnCargaCatalogo("")


                    End If


                End If



            Else
                objDatos.fnLog("btnSiguiente_sesBuscar", Session("sesBuscar"))
                If Session("Novedades") = "SI" Then
                    objDatos.fnLog("btnSiguiente", "Novedades")
                    fnCargaBusquedaAIO_PMK("Novedades")
                Else


                    fnCargaCatalogo(Request.QueryString("Cat"), ddlOrden.SelectedItem.Text)

                End If


                ' fnCargaCatalogo(Request.QueryString("Cat"))

            End If
        Catch ex As Exception

        End Try

    End Sub
    Protected Sub ddlOrden_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlOrden.SelectedIndexChanged
        Try
            If Request.QueryString.Count = 0 And Session("sesBuscar") = "" Then
                fnCargaCatalogo("", ddlOrden.SelectedItem.Text)
            Else
                If Session("sesBuscar") <> "" Then
                    fnCargaBusqueda(Session("sesBuscar"))
                Else
                    fnCargaCatalogo(Request.QueryString("Cat"), ddlOrden.SelectedItem.Text)
                End If

            End If
        Catch ex As Exception
            objDatos.fnLog("Filtro-Selección", ex.Message)
        End Try
    End Sub
    Protected Sub chkMarcas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkLista_1.SelectedIndexChanged
        'Try
        '    Dim sValores As String = ""
        '    For Each elemento As ListItem In chkLista_1.Items
        '        If elemento.Selected Then
        '            sValores = sValores & "'" & elemento.Text & "',"
        '        End If
        '    Next
        '    ''Filtramos los items de acuerdo a la marca
        '    sValores = sValores.Substring(0, sValores.Length - 1)

        '    Dim IdFiltro As Int16 = 1
        '    ssql = "select cvFiltro,cvQuery,cvCampo from config.productosFiltro WHERe ciIdRel=" & "'" & IdFiltro & "'"
        '    Dim dtDatosFiltro As New DataTable
        '    dtDatosFiltro = objDatos.fnEjecutarConsulta(ssql)
        '    If dtDatosFiltro.Rows.Count > 0 Then
        '        ''Armamos la condición del campo in(
        '        Dim sCampo As String = " AND  " & dtDatosFiltro.Rows(0)("cvCampo")
        '        Dim sCondicion As String = ""

        '        sCondicion = sCampo & " IN (" & sValores & ")"

        '        Session("CondicionFiltro1") = sCondicion

        '        sCondicion = Session("CondicionFiltro1") & Session("CondicionFiltro2") & Session("CondicionFiltro3") & Session("CondicionFiltro4") & Session("CondicionFiltro5") & Session("CondicionFiltro6") & Session("CondicionFiltro7") & Session("CondicionFiltro8")
        '        If Request.QueryString.Count = 0 Then

        '            fnCargaCatalogoFiltro(sCondicion)
        '        Else

        '            fnCargaCatalogoFiltro(Request.QueryString("Cat"), sCondicion)
        '        End If

        '    End If

        'Catch ex As Exception

        'End Try
    End Sub
    Protected Sub chkFiltro_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            Dim sValores As String = ""
            Dim IdFiltro As Int16 = 1
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_1") Then
                IdFiltro = 1
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_2") Then
                IdFiltro = 2
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_3") Then
                IdFiltro = 3
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_4") Then
                IdFiltro = 4
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_5") Then
                IdFiltro = 5
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_6") Then
                IdFiltro = 6
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_7") Then
                IdFiltro = 7
            End If
            If DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).ID.Contains("_8") Then
                IdFiltro = 8
            End If

            For Each elemento As ListItem In DirectCast(sender, System.Web.UI.WebControls.CheckBoxList).Items
                If elemento.Selected Then
                    sValores = sValores & "'" & elemento.Text & "',"
                End If
            Next
            ''Filtramos los items de acuerdo a la marca
            sValores = sValores.Substring(0, sValores.Length - 1)


            ssql = "select cvFiltro,cvQuery,cvCampo,cvCampoFiltra from config.productosFiltro WHERe ciIdRel=" & "'" & IdFiltro & "'"
            Dim dtDatosFiltro As New DataTable
            dtDatosFiltro = objDatos.fnEjecutarConsulta(ssql)
            If dtDatosFiltro.Rows.Count > 0 Then

                If dtDatosFiltro.Rows(0)("cvFiltro") = "Precio" Then

                    ''Obtenemos el rango
                    ''Partimos los valores en rangos (split en la coma)
                    Dim ValoresPrecio As String()
                    ValoresPrecio = sValores.Split(",")
                    ssql = ""
                    For Each filtro As String In ValoresPrecio
                        ssql = ssql & "SELECT cvFiltro,cfRangoInicial,cfRangoFinal FROM config.Filtros_Precio WHERE cvFiltro=" & filtro & " UNION ALL "
                    Next
                    ssql = "SELECT * FROM( " & ssql.Substring(0, ssql.Length - 10) & ") X Order by cfRangoInicial "

                    objDatos.fnLog("FiltroPrecio", ssql.Replace("'", "@"))
                    Dim dtRangoFiltro As New DataTable
                    Dim FiltroDe As Double = 0
                    Dim FiltroA As Double = 0
                    dtRangoFiltro = objDatos.fnEjecutarConsulta(ssql)
                    If dtRangoFiltro.Rows.Count > 0 Then
                        FiltroDe = dtRangoFiltro.Rows(0)("cfRangoInicial")
                        FiltroA = dtRangoFiltro.Rows(dtRangoFiltro.Rows.Count - 1)("cfRangoFinal")
                    End If
                    If Request.QueryString.Count = 0 Then

                        fnCargaCatalogoFiltroPrecio(FiltroDe, FiltroA)
                        '  objDatos.fnLog("Filtra", sCondicion.Replace("'", ""))
                    Else
                        objDatos.fnLog("FiltroPrecio", " fnCargaCatalogoFiltroPrecio: " & FiltroDe & " " & FiltroA)
                        fnCargaCatalogoFiltroPrecio(Request.QueryString("Cat"), FiltroDe, FiltroA)

                    End If


                Else
                    ''Armamos la condición del campo in(
                    Dim sCampo As String = " AND  " & dtDatosFiltro.Rows(0)("cvCampoFiltra")
                    Dim sCondicion As String = ""

                    sCondicion = sCampo & " IN (" & sValores & ")"

                    Session("CondicionFiltro" & IdFiltro) = sCondicion

                    sCondicion = Session("CondicionFiltro1") & Session("CondicionFiltro2") & Session("CondicionFiltro3") & Session("CondicionFiltro4") & Session("CondicionFiltro5") & Session("CondicionFiltro6") & Session("CondicionFiltro7") & Session("CondicionFiltro8")
                    If Request.QueryString.Count = 0 Then
                        objDatos.fnLog("Filtra", sCondicion.Replace("'", ""))
                        fnCargaCatalogoFiltro(sCondicion)

                    Else

                        fnCargaCatalogoFiltro(Request.QueryString("Cat"), sCondicion)
                    End If
                End If


            End If

        Catch ex As Exception
            objDatos.fnLog("Filtro ex", ex.Message)
        End Try
    End Sub
    Protected Sub BtnAtrasAbajo_Click(sender As Object, e As EventArgs) Handles BtnAtrasAbajo.Click
        Session("ContPaginas") = Session("ContPaginas") - 1
        If Session("ContPaginas") < 1 Then
            Session("ContPaginas") = 0
        End If
        Dim iParametros As Int16 = 0
        If Request.QueryString.Count = 0 Then
            iParametros = 0
        Else
            If Request.QueryString(0) = "Novedades" And Session("Novedades") = "NO" Then
                iParametros = 0
            Else
                iParametros = 1
            End If
        End If
        Try
            If iParametros = 0 Then
                If Session("sesBuscar") <> "" Then
                    fnCargaBusqueda(Session("sesBuscar"))
                Else
                    objDatos.fnLog("btnSiguiente", "PMK")
                    If Session("BusquedaAIO_Index") = "SI" Then
                        fnCargaBusquedaAIO_PMK(Session("BusquedaAIO_Criterios"))

                    Else

                        fnCargaCatalogo("")


                    End If


                End If



            Else
                objDatos.fnLog("btnSiguiente_sesBuscar", Session("sesBuscar"))
                If Session("Novedades") = "SI" Then
                    objDatos.fnLog("btnSiguiente", "Novedades")
                    fnCargaBusquedaAIO_PMK("Novedades")
                Else


                    fnCargaCatalogo(Request.QueryString("Cat"), ddlOrden.SelectedItem.Text)

                End If


                ' fnCargaCatalogo(Request.QueryString("Cat"))

            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnSiguienteAbajo_Click(sender As Object, e As EventArgs) Handles btnSiguienteAbajo.Click
        Session("ContPaginas") = Session("ContPaginas") + 1
        If Session("ContPaginas") >= Session("TotCatalogo") Then
            Session("ContPaginas") = Session("TotCatalogo") - 1
        End If
        'If CInt(lblResultados.Text) <= 20 Then
        '    Session("ContPaginas") = 1
        'End If
        Dim iParametros As Int16 = 0
        If Request.QueryString.Count = 0 Then
            iParametros = 0
        Else
            If Request.QueryString(0) = "Novedades" And Session("Novedades") = "NO" Then
                iParametros = 0
            Else
                iParametros = 1
            End If
        End If
        Try
            If iParametros = 0 Then
                If Session("sesBuscar") <> "" Then
                    fnCargaBusqueda(Session("sesBuscar"))
                Else
                    objDatos.fnLog("btnSiguiente", "PMK")
                    If Session("BusquedaAIO_Index") = "SI" Then
                        fnCargaBusquedaAIO_PMK(Session("BusquedaAIO_Criterios"))

                    Else

                        fnCargaCatalogo("")


                    End If


                End If



            Else
                objDatos.fnLog("btnSiguiente_sesBuscar", Session("sesBuscar"))
                If Session("Novedades") = "SI" Then
                    objDatos.fnLog("btnSiguiente", "Novedades")
                    fnCargaBusquedaAIO_PMK("Novedades")
                Else


                    fnCargaCatalogo(Request.QueryString("Cat"), ddlOrden.SelectedItem.Text)

                End If


                ' fnCargaCatalogo(Request.QueryString("Cat"))

            End If
        Catch ex As Exception

        End Try
    End Sub


    <WebMethod>
    Public Shared Function CargarCarrito(Cantidad As String, Articulo As String) As String

        HttpContext.Current.Session("AgregaCarrito") = "SI"
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones
        HttpContext.Current.Session("ProductoVista") = Articulo
        objDatos.fnLog("CargarCarrito", "entra:" & Articulo)
        Dim dPrecioActual As Double = 0
        HttpContext.Current.Session("ErrorExistencia") = ""
        Try

            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(Articulo)
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
                    partida.Generico = HttpContext.Current.Session("ProductoVista")
                    ''Cambiamos
                    partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                    partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))

                    partida.Descuento = fDescuento
                Else
                    '   Articulo = HttpContext.Current.Session("ProductoVista")
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
                    objDatos.fnLog("CargarCarrito", "Antes de evaluar")
                    If HttpContext.Current.Session("Cliente") <> "" Then
                        ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                        ssql = ssql.Replace("[%0]", HttpContext.Current.Session("Cliente"))
                        objDatos.fnLog("ListaPrecios", ssql.Replace("'", ""))
                        Dim dtLista As New DataTable
                        dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtLista.Rows.Count > 0 Then
                            HttpContext.Current.Session("ListaPrecios") = dtLista.Rows(0)(0)
                        Else
                            HttpContext.Current.Session("ListaPrecios") = "1"
                        End If
                        dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                        partida.Descuento = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                    End If


                    partida.Precio = dPrecioActual
                    partida.TotalLinea = partida.Cantidad * partida.Precio




                End If
            End If

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
                        Else
                            HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                        End If

                        Exit Function
                    End If
                Else
                    If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("BOSS") Then
                        If existencia - Cantidad < 0 Then
                            HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo no se cuenta con toda la existencia para surtir la cantidad seleccionada, el tiempo de resurtido es de 7 días hábiles."
                        End If
                    End If


                End If
            End If



            partida.ItemCode = Articulo
            Dim iLinea As Int16 = 0
            For Each PartidaCont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                iLinea = iLinea + 1
            Next
            partida.Linea = iLinea


            '     objDatos.fnLog("CargarCarrito ", "ItemName")

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
            '   objDatos.fnLog("CargarCarrito ", "Add Partida: " & partida.ItemName)

            HttpContext.Current.Session("Partidas").add(partida)

            ' objDatos.fnLog("CargarCarrito itemcode ", partida.ItemCode)

        Catch ex As Exception
            objDatos.fnLog("Error en carga", ex.Message)
        End Try







        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        ''Evaluamos lo que esta seleccionado
        Dim sCondicion As String = " AND 1=1 "
        objDatos.fnLog("btnBuscar-obtener cliente", CStr(objDatos.fnObtenerCliente()).ToUpper())
        If CStr(objDatos.fnObtenerCliente()).ToUpper().Contains("PMK") Then
            If ddlCategoria.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND T1.U_Categoria=" & "'" & ddlCategoria.SelectedValue & "'"

            End If



            If ddlMarca.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND T1.u_marca=" & "'" & ddlMarca.SelectedValue & "'"

            End If

            If ddlModelo.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND T1.U_Modelo=" & "'" & ddlModelo.SelectedValue & "'"
            End If

            If ddlAnio.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND " & "'" & ddlAnio.SelectedValue & "' between U_AnioDe and U_AnioHasta"
            End If


            Session("BusquedaAIO_Index") = "SI"

        Else
            If ddlCategoria.SelectedValue <> "-TODOS-" And ddlCategoria.SelectedItem.Text <> "" Then
                '   sCondicion = sCondicion & " AND T1.ItmsGrpNam=" & "'" & ddlCategoria.SelectedValue & "'"
                sCondicion = sCondicion & " AND Grupo=" & "'" & ddlCategoria.SelectedValue & "'"
            End If

            If ddlSubcategoria.SelectedValue <> "-TODOS-" And ddlSubcategoria.SelectedItem.Text <> "" Then
                'sCondicion = sCondicion & " AND T0.U_sublinea=" & "'" & ddlSubcategoria.SelectedValue & "'"
                sCondicion = sCondicion & " AND SubGrupo=" & "'" & ddlSubcategoria.SelectedValue & "'"
            End If

            If ddlMarca.SelectedValue <> "-TODOS-" And ddlMarca.SelectedItem.Text <> "" Then
                'sCondicion = sCondicion & " AND T0.itemcode in (SELECT U_Articulo FROM [@MODELOS] where u_marca=" & "'" & ddlMarca.SelectedValue & "')"
                sCondicion = sCondicion & " AND armadora=" & "'" & ddlMarca.SelectedValue & "'"
            End If

            If ddlModelo.SelectedValue <> "-TODOS-" And ddlModelo.SelectedItem.Text <> "" Then
                sCondicion = sCondicion & " AND Modelo=" & "'" & ddlModelo.SelectedValue & "'"
            End If
            Session("BusquedaAIO_Index") = "SI"

        End If
        Session("ContPaginas") = 0
        Session("BusquedaAIO_Criterios") = sCondicion
        objDatos.fnLog("btnBuscar-condicion", sCondicion.Replace("'", ""))
        Session("sesBuscar") = ""
        Session("Novedades") = "NO"

        'If Request.QueryString.Count > 0 Then
        '    Dim isreadonly As PropertyInfo = GetType(System.Collections.Specialized.NameValueCollection).GetProperty("IsReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)

        '    ' make collection editable
        '    isreadonly.SetValue(Me.Request.QueryString, False, Nothing)

        '    ' remove
        '    Me.Request.QueryString.Remove("opc")

        'End If
        fnCargaBusquedaAIO_PMK(sCondicion)


    End Sub

    Private Sub ddlCategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategoria.SelectedIndexChanged
        Try
            Session("AIO_OPC") = "Categoria"
            Session("AIO_Categoria") = ddlCategoria.SelectedValue
            objDatos.fnLog("IndexChange Cat", ddlCategoria.SelectedValue)
            FiltraCombos_AIOPMK()
        Catch ex As Exception
            objDatos.fnLog("ex IndexChange_categoria", ex.Message.Replace("'", ""))
        End Try
    End Sub


    Private Sub ddlSubcategoria_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSubcategoria.SelectedIndexChanged
        Try
            Session("AIO_OPC") = "SubLinea"
            Session("AIO_SubLinea") = ddlSubcategoria.SelectedValue
            objDatos.fnLog("IndexChange SubCat", ddlSubcategoria.SelectedValue)
            FiltraCombos_AIOPMK()
        Catch ex As Exception
            objDatos.fnLog("ex IndexChange_sublinea", ex.Message.Replace("'", ""))
        End Try
    End Sub

    Private Sub ddlMarca_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlMarca.SelectedIndexChanged
        Try
            Session("AIO_OPC") = "Marca"
            Session("AIO_Marca") = ddlMarca.SelectedValue.ToString.TrimEnd
            objDatos.fnLog("IndexChange Marca", ddlMarca.SelectedValue)
            FiltraCombos_AIOPMK()
        Catch ex As Exception
            objDatos.fnLog("ex IndexChange_marca", ex.Message.Replace("'", ""))
        End Try
    End Sub

    Private Sub ddlModelo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlModelo.SelectedIndexChanged
        Try
            Session("AIO_OPC") = "Modelo"
            Session("AIO_Modelo") = ddlModelo.SelectedValue
            objDatos.fnLog("IndexChange Modelo", ddlModelo.SelectedValue)
            FiltraCombos_AIOPMK()
        Catch ex As Exception
            objDatos.fnLog("ex IndexChange_modelo", ex.Message.Replace("'", ""))
        End Try
    End Sub

    Public Sub FiltraCombos_AIOPMK()
        Dim sCondicion As String = " AND 1=1 "

        Dim sCondicionCategoria As String = ""
        Dim sCondicionSubLinea As String = ""
        Dim sCondicionModelo As String = ""
        Dim sCondicionMarca As String = ""
        Dim sCondicionAño As String = ""
        Dim sEmpresa As String = ""

        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then
            sEmpresa = "PMK"
        End If
        If ddlCategoria.SelectedValue <> "-TODOS-" Then
            If sEmpresa = "PMK" Then

                sCondicionCategoria = " AND U_categoria=" & "'" & ddlCategoria.SelectedValue & "'"
            Else

                '  sCondicionCategoria = " AND T1.ItmsGrpNam=" & "'" & ddlCategoria.SelectedValue & "'"
                sCondicionCategoria = " AND Grupo=" & "'" & ddlCategoria.SelectedValue & "'"
            End If
        End If

        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then

        Else
            If ddlSubcategoria.SelectedValue <> "-TODOS-" And ddlSubcategoria.SelectedValue.Length > 0 Then
                sCondicionSubLinea = " AND SubGrupo=" & "'" & ddlSubcategoria.SelectedValue & "'"
            End If
        End If


        If ddlMarca.SelectedValue <> "-TODOS-" And ddlMarca.SelectedValue.Length > 0 Then
            '  sCondicionMarca = " AND T0.itemcode in (SELECT U_Articulo FROM [@MODELOS] where u_marca=" & "'" & ddlMarca.SelectedValue & "')"
            If sEmpresa = "PMK" Then
                sCondicionMarca = " AND U_Marca=" & "'" & ddlMarca.SelectedValue & "'"
            Else
                sCondicionMarca = " AND ARMADORA=" & "'" & ddlMarca.SelectedValue & "'"
            End If

        End If

        If ddlModelo.SelectedValue <> "-TODOS-" And ddlModelo.SelectedValue.Length > 0 Then
            '  sCondicionModelo = " AND T0.itemcode in (SELECT distinct U_Articulo FROM [@MODELOS] where u_Modelo=" & "'" & ddlModelo.SelectedValue & "')"
            If sEmpresa = "PMK" Then
                sCondicionModelo = " AND U_Modelo=" & "'" & ddlModelo.SelectedValue & "'"
            Else
                sCondicionModelo = " AND Modelo=" & "'" & ddlModelo.SelectedValue & "'"
            End If

        End If

        If ddlAnio.SelectedValue <> "-TODOS-" And ddlAnio.SelectedValue.Length > 0 Then
            'sCondicionAño = " AND T0.itemcode in (SELECT distinct U_Articulo FROM [@MODELOS] where (cast(U_AnioDe As varchar) + '-' + cast(U_anioHasta as varchar))=" & "'" & ddlAnio.SelectedValue & "')"
            sCondicionAño = " AND (cast(desde As varchar) + '-' + cast(hasta as varchar))=" & "'" & ddlAnio.SelectedValue & "'"
        End If

        objDatos.fnLog("Condicion Categoria", sCondicionCategoria.Replace("'", ""))
        objDatos.fnLog("Condicion SubCategoria", sCondicionSubLinea.Replace("'", ""))
        objDatos.fnLog("Condicion Marca", sCondicionMarca.Replace("'", ""))
        objDatos.fnLog("Condicion Modelo", sCondicionModelo.Replace("'", ""))


        objDatos.fnLog("Session Categoria", Session("AIO_Categoria"))
        objDatos.fnLog("Session SubCategoria", Session("AIO_SubLinea"))
        objDatos.fnLog("Session Marca", Session("AIO_Marca"))
        objDatos.fnLog("Session Modelo", Session("AIO_Modelo"))



        Dim fila As DataRow

        Dim sOrder As String = ""

        If sEmpresa = "PMK" Then
            ssql = objDatos.fnObtenerQuery("filtraCategoria")
        Else
            ssql = "SELECT distinct T1.ItmsGrpNam  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod WHERE T0.SellItem='Y'  AND T0.validfor='Y' AND itemcode in( Select Distinct U_Articulo  from [@MODELOS]) "
            ssql = "SELECT DISTINCT Grupo as ItmsGrpNam FROM SAP_Tienda..TablaModelos6 where 1=1  "
        End If

        If Session("AIO_OPC") = "Categoria" Then
            sCondicion = sCondicionSubLinea & sCondicionMarca & sCondicionModelo
        End If
        ssql = ssql & "  " & sCondicion & " Order by ItmsGrpNam"
        objDatos.fnLog("Categoria", ssql.Replace("'", ""))
        Dim dtCategoria As New DataTable
        dtCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtCategoria.NewRow
        fila("ItmsGrpNam") = "-TODOS-"
        fila("ItmsGrpNam") = "-TODOS-"

        dtCategoria.Rows.Add(fila)
        ddlCategoria.DataSource = dtCategoria
        ddlCategoria.DataTextField = "ItmsGrpNam"
        ddlCategoria.DataValueField = "ItmsGrpNam"
        ddlCategoria.DataBind()

        Try
            ddlCategoria.SelectedValue = Session("AIO_Categoria")

        Catch ex As Exception
            ddlCategoria.SelectedValue = "-TODOS-"
            objDatos.fnLog("ex FiltraCombos_cat", ex.Message.Replace("'", "") & "-> " & Session("AIO_Categoria"))
        End Try


        If sEmpresa = "PMK" Then

        Else
            ssql = "SELECT distinct T0.U_Tie_SubLinea, T2.Name as SubLinea  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod INNER JOIN [@TIE_SubLineas] T2 on T0.U_Tie_Sublinea=T2.Code WHERE T0.SellItem='Y'  AND T0.validfor='Y' AND itemcode in( Select Distinct U_Articulo  from [@MODELOS]) "
            ssql = "SELECT DISTINCT SubGrupo as sublinea FROM SAP_Tienda..TablaModelos6 where 1=1  "
            If Session("AIO_OPC") <> "SubLinea" Then
                sCondicion = sCondicionCategoria & sCondicionMarca & sCondicionModelo
            End If

            ssql = ssql & "  " & sCondicion & " Order by sublinea"
            objDatos.fnLog("SubCategoria", ssql.Replace("'", ""))
            Dim dtsubcategoria As New DataTable
            dtsubcategoria = objDatos.fnEjecutarConsultaSAP(ssql)
            fila = dtsubcategoria.NewRow
            fila("sublinea") = "-TODOS-"
            fila("sublinea") = "-TODOS-"

            dtsubcategoria.Rows.Add(fila)
            ddlSubcategoria.DataSource = dtsubcategoria
            ddlSubcategoria.DataTextField = "sublinea"
            ddlSubcategoria.DataValueField = "sublinea"
            ddlSubcategoria.DataBind()
            Try
                ddlSubcategoria.SelectedValue = Session("AIO_SubLinea")

            Catch ex As Exception
                ddlSubcategoria.SelectedValue = "-TODOS-"
                objDatos.fnLog("ex FiltraCombos_sublinea", ex.Message.Replace("'", "") & "-> " & Session("AIO_SubLinea"))
            End Try
        End If


        If sEmpresa = "PMK" Then
            ssql = objDatos.fnObtenerQuery("FiltraMarca")
        Else
            ssql = "SELECT distinct T2.U_Marca as U_Marca  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod INNER JOIN [@MODELOS] T2 on T0.itemCode=T2.U_Articulo WHERE T0.SellItem='Y'  AND T0.validfor='Y' "
            ssql = "SELECT DISTINCT ARMADORA as u_marca FROM SAP_Tienda..TablaModelos6 where 1=1  "
        End If

        If Session("AIO_OPC") <> "Marca" Then
            sCondicion = sCondicionCategoria & sCondicionSubLinea & sCondicionModelo
        End If
        ssql = ssql & "  " & sCondicion & " Order by u_Marca "
        objDatos.fnLog("Marca", ssql.Replace("'", ""))
        Dim dtMarca As New DataTable
        dtMarca = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtMarca.NewRow
        fila("U_marca") = "-TODOS-"
        fila("U_marca") = "-TODOS-"

        dtMarca.Rows.Add(fila)
        ddlMarca.DataSource = dtMarca
        ddlMarca.DataTextField = "U_marca"
        ddlMarca.DataValueField = "U_marca"
        ddlMarca.DataBind()
        Try
            ddlMarca.SelectedValue = CStr(Session("AIO_Marca")).Trim

        Catch ex As Exception
            ddlMarca.SelectedValue = "-TODOS-"

            objDatos.fnLog("ex FiltraCombos_marca", ex.Message.Replace("'", "") & "-> " & Session("AIO_Marca"))
        End Try


        If sEmpresa = "PMK" Then
            ssql = objDatos.fnObtenerQuery("FiltraModelo")
        Else
            ssql = "SELECT distinct T2.U_Modelo as U_Modelo  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod INNER JOIN [@MODELOS] T2 on T0.itemCode=T2.U_Articulo WHERE T0.SellItem='Y'  AND T0.validfor='Y' "
            ssql = "SELECT DISTINCT modelo as u_modelo FROM SAP_Tienda..TablaModelos6 where modelo is not null  "
        End If


        If Session("AIO_OPC") <> "Modelo" Then
            sCondicion = sCondicionCategoria & sCondicionSubLinea & sCondicionMarca
        End If

        ssql = ssql & "  " & sCondicion & " Order by u_Modelo "
        objDatos.fnLog("Modelo", ssql.Replace("'", ""))
        Dim dtModelo As New DataTable
        dtModelo = objDatos.fnEjecutarConsultaSAP(ssql)
        fila = dtModelo.NewRow
        fila("U_modelo") = "-TODOS-"
        fila("U_modelo") = "-TODOS-"

        dtModelo.Rows.Add(fila)
        ddlModelo.DataSource = dtModelo
        ddlModelo.DataTextField = "U_modelo"
        ddlModelo.DataValueField = "U_modelo"
        ddlModelo.DataBind()
        Try
            ddlModelo.SelectedValue = Session("AIO_Modelo")

        Catch ex As Exception
            ddlModelo.SelectedValue = "-TODOS-"
            objDatos.fnLog("ex FiltraCombos_modelo", ex.Message.Replace("'", "") & "-> " & Session("AIO_Modelo"))
        End Try


        If sEmpresa = "PMK" Then
            ssql = objDatos.fnObtenerQuery("FiltraAños")
        Else
            ssql = "  Select  distinct cast(U_AnioDe As varchar) + '-' + cast(U_anioHasta as varchar) as U_Anio   FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod INNER JOIN [@MODELOS] T2 on T0.itemCode=T2.U_Articulo WHERE T0.SellItem='Y'  AND T0.validfor='Y' "
            ssql = "   Select  distinct AnioDe as U_AnioDe ,AnioHasta as U_AnioHasta    FROM SAP_Tienda..tablaModelos6  where (AnioDe is not null and AnioHasta is not null)  "
        End If


        If Session("AIO_OPC") <> "Año" Then
            sCondicion = sCondicionCategoria & sCondicionSubLinea & sCondicionMarca & sCondicionModelo
        End If

        ssql = ssql & "  " & sCondicion

        objDatos.fnLog("Año", ssql.Replace("'", ""))

        Dim dtaño As New DataTable
        dtaño = objDatos.fnEjecutarConsultaSAP(ssql)
        ''Tomamos el año menor y el mayor para hacer un ciclo en el que muestre todos los años del rango
        Dim AñoMAX As Int16
        Dim AñoMIN As Int16
        AñoMAX = dtaño.Compute("MAX(U_AnioHasta)", "")
        AñoMIN = dtaño.Compute("MIN(U_AnioDe)", "")


        Dim dtAñoMostrar As New DataTable
        dtAñoMostrar.Columns.Add("U_Anio")

        For i = AñoMIN To AñoMAX Step 1
            fila = dtAñoMostrar.NewRow
            fila("U_Anio") = i
            dtAñoMostrar.Rows.Add(fila)
        Next



        fila = dtAñoMostrar.NewRow
        fila("U_Anio") = "-TODOS-"

        dtAñoMostrar.Rows.Add(fila)
        ddlAnio.DataSource = dtAñoMostrar
        ddlAnio.DataTextField = "U_Anio"
        ddlAnio.DataValueField = "U_Anio"
        ddlAnio.DataBind()
        Try
            ddlAnio.SelectedValue = Session("AIO_AÑO")
        Catch ex As Exception
            ddlAnio.SelectedValue = "-TODOS-"
        End Try
    End Sub

    Private Sub ddlAnio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAnio.SelectedIndexChanged
        Try
            Session("AIO_OPC") = "Año"
            Session("AIO_AÑO") = ddlAnio.SelectedValue
            objDatos.fnLog("IndexChange año", ddlAnio.SelectedValue)
            FiltraCombos_AIOPMK()
        Catch ex As Exception
            objDatos.fnLog("ex IndexChange_año", ex.Message.Replace("'", ""))
        End Try
    End Sub
End Class
