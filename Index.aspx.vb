Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Web.Services

Partial Class Index
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String
    Private Sub Index_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "index.aspx"
        '   MaintainScrollPositionOnPostBack = True
        If Request.QueryString("action") = "e" Then

            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode = Request.QueryString("code") Then
                    Partida.ItemCode = "BORRAR"
                    Exit For
                End If

            Next
            objDatos.Mensaje("Se ha eliminado el producto del carrito", Me.Page)
            Session("itemEliminado") = "Se ha eliminado el producto del carrito"
            Response.Redirect("catalogo.aspx")

        End If
        Try
            fnCargaBanners()
        Catch ex As Exception
            objDatos.Mensaje(ex.Message, Me.Page)
        End Try
        If objDatos.fnObtenerCliente().ToUpper.Contains("SUJEAU") Then
            Response.Redirect("catalogo.aspx")
        End If
        If objDatos.fnObtenerCliente().ToUpper.Contains("PMK") And Session("Cliente") = "" Then
            'es pmk y es B2B, redireccionar al login en caso de que el dominio sea ventas.pml
            objDatos.fnLog("Entra URI PMK:", HttpContext.Current.Request.Url.AbsoluteUri.ToUpper)
            If HttpContext.Current.Request.Url.AbsoluteUri.Contains("ventas.pmkimpulsa") Then
                '    Response.Redirect("loginb2b.aspx")
            End If
        End If
        ' Session("slpCode")
        'AIO Vendedores
        If objDatos.fnObtenerCliente().ToUpper.Contains("AIO") And CInt(Session("slpCode")) <= 0 Then
            'es aio y es vendedores, redireccionar al login en caso de que el dominio sea administracion.aio
            objDatos.fnLog("Entra URI AIO:", HttpContext.Current.Request.Url.AbsoluteUri.ToUpper)
            If HttpContext.Current.Request.Url.AbsoluteUri.Contains("administracion.aio") Then
                Response.Redirect("login.aspx")
            End If
        End If

        'AIO B2B
        If objDatos.fnObtenerCliente().ToUpper.Contains("AIO") And Session("Cliente") = "" Then
            'es aio y es vendedores, redireccionar al login en caso de que el dominio sea administracion.aio
            objDatos.fnLog("Entra URI AIO:", HttpContext.Current.Request.Url.AbsoluteUri.ToUpper)
            If Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("administracion") And Not HttpContext.Current.Request.Url.AbsoluteUri.Contains("autopartes") Then
                '   Response.Redirect("loginB2B.aspx")
                '  Response.Redirect("index.aspx")
            End If
        End If


        If Not IsPostBack Then


            Session("Pedido") = New Cls_Pedido

            Try
                If Session("Partidas").count > 0 Then
                Else
                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                End If
            Catch ex As Exception
                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
            End Try

            Try
                If Session("WishList").count > 0 Then
                Else
                    Session("WishList") = New List(Of Cls_Pedido.Partidas)
                End If
            Catch ex As Exception
                Session("WishList") = New List(Of Cls_Pedido.Partidas)
            End Try

            Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
            Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

            Try
                fnInsertaBusquedaTienda()
                fnCargaBarrasv2()
            Catch ex As Exception
                '     objDatos.fnLog("Barrasv2", ex.Message)

            End Try
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                Response.Redirect("Catalogo.aspx")
            End If
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                pnlBuscadorAIO.Visible = True
                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                    pnlSubcat.visible = False
                End If

                If Not IsPostBack Then
                    Session("AIO_OPC") = ""
                    Session("AIO_Categoria") = ""
                    Session("AIO_SubLinea") = ""
                    Session("AIO_Marca") = ""
                    Session("AIO_Modelo") = ""
                    Session("AIO_AÑO") = ""
                    If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                        fnCargaCatalogos_PMK()
                        fnCargaNoticiasAIO_PMK()
                        fnCargaNuestrosServicios_PMK()

                    Else
                        fnCargaNoticiasAIO_PMK()
                        fnCargaCatalogosAIO_PMK()
                        fnCargaNuestrasLineasAIO_PMK()
                    End If

                    ''Revisamos los productos destacados
                    fnCargaProductosDestacadosAIO_PMK()

                End If

            End If
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SUJEAU") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("MANIJAU") Then
                pnlBuscadorAIO.Visible = True
                Session("MANIJ_OPC") = ""
                Session("MANIJ_Categoria") = ""
                Session("MANIJ_SubLinea") = ""
                Session("MANIJ_Marca") = ""
                Session("MANIJ_Modelo") = ""
                Session("MANIJ_AÑO") = ""
                fnCargaCatalogos_MANIJ()
            End If


        Else
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                pnlBuscadorAIO.Visible = True


                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Then
                    'fnCargaCatalogos_PMK()
                    fnCargaNoticiasAIO_PMK()
                    fnCargaNuestrosServicios_PMK()

                Else
                    fnCargaNoticiasAIO_PMK()
                    ' fnCargaCatalogosAIO_PMK()
                    fnCargaNuestrasLineasAIO_PMK()
                End If

                ''Revisamos los productos destacados
                fnCargaProductosDestacadosAIO_PMK()


            End If
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


    Public Sub fnCargaCatalogos_PMK()
        Dim fila As DataRow
        Dim sCondicion As String = ""
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



    End Sub

    Public Sub fnCargaProductosDestacadosAIO_PMK()
        ssql = objDatos.fnObtenerQuery("ProductosDestacados")
        Dim dtProductos As New DataTable
        Dim shtml As String = ""

        shtml = "<div class='col-xs-12 col-sm-12' id='blk-productos-destacados'>"
        '  shtml = shtml & "<h3>PRODUCTOS DESTACADOS</h3>"
        shtml = shtml & "<h3>NOVEDADES</h3>"
        shtml = shtml & " <div class='carrousel-1'>"

        If ssql <> "" Then
            dtProductos = objDatos.fnEjecutarConsultaSAP(ssql)
            For i = 0 To dtProductos.Rows.Count - 1 Step 1
                shtml = shtml & " <div class='item'>"

                shtml = shtml & " <div class='col-xs-12 col-sm-6'><img src='" & dtProductos.Rows(i)("Imagen") & "' class='img-responsive' /></div>"
                shtml = shtml & "  <div class='col-xs-12 col-sm-6'>"
                shtml = shtml & "   <div class='titulo'>" & dtProductos.Rows(i)("ItemCode")
                shtml = shtml & "  </div>"

                shtml = shtml & "  <div class='descripcion'>" & dtProductos.Rows(i)("Descripcion")
                shtml = shtml & "   </div>"
                shtml = shtml & "   <a href='producto-interior.aspx?Code=" & dtProductos.Rows(i)("ItemCode") & "' class='btn-general-2'>Ver más</a>"


                shtml = shtml & "  </div>"
                shtml = shtml & " </div>"
            Next
            shtml = shtml & " </div>"
            shtml = shtml & " </div>"
            Dim literal As New LiteralControl(shtml)
            pnlDestacadosAIO.Controls.Clear()
            pnlDestacadosAIO.Controls.Add(literal)
        End If
    End Sub

    Public Sub fnCargaNuestrosServicios_PMK()
        ssql = "SELECT cvHTML from config.HTML where cvTipo='Servicios'"
        Dim dtProductos As New DataTable
        dtProductos = objDatos.fnEjecutarConsulta(ssql)


        Dim shtml As String = ""

        If dtProductos.Rows.Count > 0 Then
            shtml = dtProductos.Rows(0)(0)
        End If
        Dim literal As New LiteralControl(shtml)
        pnlServicios.Controls.Clear()
        pnlServicios.Controls.Add(literal)

    End Sub


    Public Sub fnCargaNuestrasLineasAIO_PMK()
        ssql = "Select cvImagen as Imagen, cvTitulo as Titulo, cvDescripcion as Descripcion from config.Lineas"
        Dim dtProductos As New DataTable
        Dim shtml As String = ""
        dtProductos = objDatos.fnEjecutarConsulta(ssql)
        shtml = "<div class='col-xs-12 col-sm-12' id='blk-nuestras-lineas'>"
        shtml = shtml & "<h3>NUESTRAS LÍNEAS</h3>"
        shtml = shtml & " <div class='carrousel-2'>"

        For i = 0 To dtProductos.Rows.Count - 1 Step 1
            shtml = shtml & " <div class='item'>"

            shtml = shtml & " <div class='col-xs-12'><img src='" & dtProductos.Rows(i)("Imagen") & "' class='img-responsive' /> </div>"

            shtml = shtml & " <div class='col-xs-12'><div class='titulo'>" & dtProductos.Rows(i)("Titulo") & " </div></div>"

            shtml = shtml & " <div class='col-xs-12'><div class='descripcion'>" & dtProductos.Rows(i)("Descripcion") & " </div></div>"

            shtml = shtml & " </div>"
        Next
        shtml = shtml & " </div>"
        shtml = shtml & " </div>"
        Dim literal As New LiteralControl(shtml)
        pnlNuestrasLineas.Controls.Clear()
        pnlNuestrasLineas.Controls.Add(literal)
    End Sub

    Public Sub fnCargaNoticiasAIO_PMK()
        ssql = "Select cvImagen as Imagen, cvTitulo as Titulo, cvDescripcion as Descripcion from config.Noticias"
        Dim dtProductos As New DataTable
        Dim shtml As String = ""
        dtProductos = objDatos.fnEjecutarConsulta(ssql)
        shtml = "<div class='col-xs-12 col-sm-12' id='blk-nuestras-lineas'>"
        'shtml = shtml & "<h3>NOTICIAS</h3>"
        shtml = shtml & "<h3>CONSEJOS</h3>"
        shtml = shtml & " <div class='carrousel-2'>"

        For i = 0 To dtProductos.Rows.Count - 1 Step 1
            shtml = shtml & " <div class='item'>"

            shtml = shtml & " <div class='col-xs-12'><img src='" & dtProductos.Rows(i)("Imagen") & "' class='img-responsive' /> </div>"

            shtml = shtml & " <div class='col-xs-12'><div class='titulo'>" & dtProductos.Rows(i)("Titulo") & " </div></div>"

            shtml = shtml & " <div class='col-xs-12'><div class='descripcion'>" & dtProductos.Rows(i)("Descripcion") & " </div></div>"

            shtml = shtml & " </div>"
        Next
        shtml = shtml & " </div>"
        shtml = shtml & " </div>"
        Dim literal As New LiteralControl(shtml)
        pnlNuestrasLineas.Controls.Clear()
        pnlNuestrasLineas.Controls.Add(literal)
    End Sub

    Public Sub fnCargaCatalogosAIO_PMK()
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
        ddlCategoria.SelectedValue = "-TODOS-"
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
        ddlSubcategoria.SelectedValue = "-TODOS-"

        ssql = "select distinct ARMADORA as u_marca from SAP_TIENDA..tablaModelos6   order by u_marca"
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
        ddlMarca.SelectedValue = "-TODOS-"

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
        ddlModelo.SelectedValue = "-TODOS-"


        ssql = " Select  distinct AnioDe as U_AnioDe ,AnioHasta as U_AnioHasta    FROM SAP_Tienda..tablaModelos6 where anioDe > 0 "
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



    End Sub
    Public Function fnInsertaBusquedaTienda()
        Dim sHTML As String = ""
        '<img src='images/location/location.png' style='width:25px;height:25px;'>
        Dim sHtmlEncabezado = ""
        sHtmlEncabezado = sHtmlEncabezado & " <div class='seccion'> "
        sHTML = "<div class='seccion busca-ubicacion'>" _
    & " <div class='container flex-item-cent'> " _
    & "	<div class=''> " _
    & "		Ingresa tu dirección para mostrarte lo que tenemos en tu zona " _
    & "	</div> " _
    & "	<div class=''> " _
    & "		<div> " _
    & "			<img src='images/location/location.png' style='width:25px;height:25px;'> " _
    & "			<input type='' name=''> " _
    & "		</div> " _
    & "	</div> " _
    & " </div> " _
& " </div> "
        sHtmlEncabezado = sHtmlEncabezado & sHTML & "</div>"

        Dim literal As New LiteralControl(sHtmlEncabezado)
        '  pnlBusquedaTienda.Controls.Clear()
        ' pnlBusquedaTienda.Controls.Add(literal)
        sHtmlEncabezado = ""
        Return sHtmlEncabezado
    End Function

    Public Function fnInsertaCategoriasTienda()
        Dim sHTML As String = ""

        Dim sHtmlEncabezado = ""
        sHtmlEncabezado = " <div class='seccion slider-products-section'> " _
                         & " <div class='main-container'>  " _
                         & " 	<div class='slider-categorias'> "

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
                'objDatos.fnlog("Primer Cats", ssql.Replace("'", ""))
            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)



            For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                Dim sValorPintar As String = ""
                ' 'objDatos.fnlog("Cats", "entra")
                If CStr(dtPrimerNivel.Rows(0)(1)).Contains("U_") Then
                    ssql = objDatos.fnObtenerQuery("CampoUsuario")
                    ssql = ssql.Replace("[%0]", dtCategorias.Rows(i)(0))
                    Dim dtValor As New DataTable
                    dtValor = objDatos.fnEjecutarConsultaSAP(ssql)
                    'objDatos.fnlog("Cats", ssql.Replace("'", ""))
                    If dtValor.Rows.Count > 0 Then
                        sValorPintar = dtValor.Rows(0)(0)
                    End If
                Else
                    sValorPintar = dtCategorias.Rows(i)(0)
                End If
                Dim sRutaImagen As String = ""
                If File.Exists(Server.MapPath("~") & "\images\categories\" & sValorPintar & ".jpg") Then
                    sRutaImagen = "images/categories/" & sValorPintar & ".jpg"
                End If
                If File.Exists(Server.MapPath("~") & "\images\categories\" & sValorPintar & ".png") Then
                    sRutaImagen = "images/categories/" & sValorPintar & ".png"
                End If
                If sRutaImagen = "" Then
                    sRutaImagen = "img/temporal1.png"
                End If
                sHTML = sHTML & "<div class='item'><a href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(1) & "' class='item-espace'><img src='" & sRutaImagen & "' class='img-responsive'></a><div class='tit-categoria'>" & sValorPintar & "</div></div>  "
            Next
        End If
        sHtmlEncabezado = sHtmlEncabezado & sHTML
        sHtmlEncabezado = sHtmlEncabezado & "   </div>"
        sHtmlEncabezado = sHtmlEncabezado & "  </div>"
        sHtmlEncabezado = sHtmlEncabezado & "</div>"

        '        sHTML = " <div class='seccion slider-products-section'> " _
        '    & " <div class='container'>  " _
        '    & " 	<div class='slider-categorias'> " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div>  " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div>  " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div>  " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & " 		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "		<div class='item'><a href='#' class='item-espace'><img src='img/temporal1.png' class='img-responsive'></a><div>categoria name</div></div> " _
        '    & "	</div> " _
        '    & " </div> " _
        '& " </div> "

        'Dim literal As New LiteralControl(sHtmlEncabezado)
        '  pnlBusquedaTienda.Controls.Clear()
        ' pnlBusquedaTienda.Controls.Add(literal)

        Return sHtmlEncabezado
    End Function

    Public Sub fnCargaBanners()


        ''Categorias
        Dim sTipoCliente As String = ""
        ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
        Dim dtclienteCat As New DataTable
        dtclienteCat = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteCat.Rows.Count > 0 Then
            If CStr(dtclienteCat.Rows(0)(0)).Contains("Salama") And Session("RazonSocial") = "" Then
                sTipoCliente = "STOP"
            Else
                sTipoCliente = CStr(dtclienteCat.Rows(0)(0)).ToUpper

            End If
        End If


        Dim dtValida As New DataTable

        If sTipoCliente.Contains("BOSS") Then
            ''Revisamos, si tenemos B2B. Checamos si hay banners especificos. Sino, mostramos todos
            If Session("RazonSocial") <> "" Then

                ''B2B, obtenemos el tipo de B2B para consultar el SubTipo
                objDatos.fnLog("TipoB2B", "Banner b2b tipo:" & Session("TipoB2B"))
                If Session("TipoB2B") <> "" Then

                    ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' and cvTipo ='B2B' and cvSubTipo='" & Session("TipoB2B") & "' Order by ciOrden "
                Else
                    ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' and cvTipo ='B2B' Order by ciOrden "
                End If



                If CInt(Session("slpCode")) <> 0 Then
                    ''Es Vendedores
                    ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' and cvTipo ='Vendedor' Order by ciOrden "

                End If
                ''Estan como B2B

                dtValida = objDatos.fnEjecutarConsulta(ssql)
                If dtValida.Rows.Count = 0 Then
                    ''No hay, mostramos lo de caja
                    ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' Order by ciOrden "
                End If
            Else
                ''No es B2B, mostramos lo de caja
                ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' and cvTipo not in ('B2B','Vendedor')  Order by ciOrden "
                dtValida = objDatos.fnEjecutarConsulta(ssql)
                If dtValida.Rows.Count = 0 Then
                    ''No tenemos la columna para el tipo= B2B, mostramos lo de caja
                    ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' Order by ciOrden "
                End If
            End If
        Else
            ssql = "Select cvDescripcion,cvRutaImagen ,ciOrden,ISNULL(cvTexto,'') as Texto,ISNULL(cvLiga,'') as Liga  from Config.banners where cvEstatus='ACTIVO' Order by ciOrden "
        End If



        objDatos.fnLog("Banners TipoB2B", ssql.Replace("'", ""))


        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlEncabezado As String
        Dim sHtmlBanner As String = ""
        sHtmlEncabezado = " <div class='wrappercon naranja'> "
        sHtmlEncabezado = sHtmlEncabezado & " <div class='seccion'> "
        'sHtmlEncabezado = sHtmlEncabezado & "  <div class='main-container'> "
        sHtmlEncabezado = sHtmlEncabezado & "  <div class='main-container-ext'> "
        If sTipoCliente.ToUpper.Contains("HAWK") Then
            '   sHtmlEncabezado = sHtmlEncabezado & " <div class='section video-home' id='index' style=''> <div id='Module-video'><div id='video' class='fullScreen loaded' style='margin-top -106px; height: 979px; width: 100%;'>  </div></div> </div> "
            sHtmlEncabezado = sHtmlEncabezado & " <div class='section video-home' id='index' style=''><div id='player-overlay'>      <video controls autoplay> <source src='img/yt1s.com - hank_480p.mp4' />           <source src='img/yt1s.com - hank_480p.webm' type='video/webm; codecs='vp8, vorbis'' /><source src='img/yt1s.com - hank_480p.ogv' type='video/ogg; codecs='theora, vorbis'' /> </video>  </div> </div> "
        End If

        sHtmlEncabezado = sHtmlEncabezado & "  <div class='slide-principal'> "

        If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Then
            sHtmlBanner = sHtmlBanner & "<div class='background-image' style='height:491px'> <video autoplay='' muted='' src='https://autopartes.aio.lat/img/home/aio%20particulas.mp4' controls=''></video><div class='caption'></div></div>"
        End If

        ''Banners
        For i = 0 To dtBanners.Rows.Count - 1 Step 1
            sHtmlBanner = sHtmlBanner & "<a class='a-link' href='" & dtBanners.Rows(i)("Liga") & "'> "
            '  sHtmlBanner = sHtmlBanner & "<div class='items'> "
            sHtmlBanner = sHtmlBanner & "<div class='background-image'> "
            sHtmlBanner = sHtmlBanner & "<img class='img-responsive' alt='slide-1' data-lazy='" & dtBanners.Rows(i)("cvRutaImagen") & "'>"
            sHtmlBanner = sHtmlBanner & "<div class='caption'> "
            If dtBanners.Rows(i)("Texto") <> "" Then
                sHtmlBanner = sHtmlBanner & "   <span class='title'> " & dtBanners.Rows(i)("Texto") & "</span>"
            End If
            If dtBanners.Rows(i)("Liga") <> "" Then
                sHtmlBanner = sHtmlBanner & "   <div class='col-xs-12 no-padding'><a class='a-link' href='" & dtBanners.Rows(i)("Liga") & "'>Ver Detalles</a></div>"
            End If

            sHtmlBanner = sHtmlBanner & "</div></div></a>  "
        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        sHtmlEncabezado = sHtmlEncabezado & "</div></div></div>"

        Dim sHTMLBannerIntermedio As String = ""
        Dim sHTMLCAT As String = ""
        Dim sHTMLPRomos As String = ""
        Dim sHTMLExtraDiv As String = ""
        Try
            If sTipoCliente.Contains("BOSS") Then
                sHtmlEncabezado = sHtmlEncabezado & fnInsertaBusquedaTienda()
                sHtmlEncabezado = sHtmlEncabezado & fnInsertaCategoriasTienda()
            End If


            If sTipoCliente = "STOP" Then
                sHTMLBannerIntermedio = "<div class='seccion white-section'><div class='main-container'> <span class='linea top'></span><div class='feature-10 slick-initialized slick-slider'> " _
                & " 		    <div class='item-sec'><img class='img-responsive' src='img/home/stop-1.jpg'> </div><div class='item-sec'><img class='img-responsive' src='img/home/stop-2.jpg'> </div><div class='item-sec'><img class='img-responsive' src='img/home/stop-3.jpg'> </div> " _
                & " </div><div class='sec-tit'><img src='img/header/logogris.png' class='img-responsive'></div></div></div> "
            End If


            If sTipoCliente = "STOP" Then
                sHTMLCAT = sHTMLCAT & "<div class='seccion gray-section'>"
            Else
                sHTMLCAT = sHTMLCAT & "<div class='seccion'>"
            End If

            sHTMLCAT = sHTMLCAT & " <div class='main-container'>"

            sHTMLCAT = sHTMLCAT & "  <span class='linea top'></span>"
            Dim sURLColeccion As String = ""
            If sTipoCliente = "STOP" Then
                ssql = "Select ISNULL(cvColeccion,''),ISNULL(cvURLColeccion,'') FROM config.parametrizaciones "
                Dim dtColeccion As New DataTable
                dtColeccion = objDatos.fnEjecutarConsulta(ssql)
                If dtColeccion.Rows.Count > 0 Then
                    sURLColeccion = dtColeccion.Rows(0)(1)
                    sHTMLCAT = sHTMLCAT & " <h2 class='tit-coleccion'>" & dtColeccion.Rows(0)(0) & "</h2>    "
                End If
            End If

            sHTMLCAT = sHTMLCAT & "   <div class='cont-banners'>"

            'sHtmlEncabezado = sHtmlEncabezado & "  <div class='slide-principal'> "
            sHtmlBanner = ""
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


                    ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

                End If
                Dim dtCategorias As New DataTable
                dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
                Dim iEntra As Int16 = 0
                For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                    ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias POR TEXTO

                    If sTipoCliente.Contains("BOSS") Then
                        If Session("RazonSocial") <> "" Then

                            ''B2B, obtenemos el tipo de B2B para consultar el SubTipo

                            If Session("TipoB2B") <> "" Then
                                ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI' and cvTipo ='B2B' and cvSubTipo='" & Session("TipoB2B") & "'"
                            Else
                                ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI' and cvTipo ='B2B' "
                            End If



                            If CInt(Session("slpCode")) <> 0 Then
                                ''Es Vendedores
                                ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI' and cvTipo='Vendedor' "

                            End If
                            ''Estan como B2B

                            dtValida = objDatos.fnEjecutarConsulta(ssql)
                            If dtValida.Rows.Count = 0 Then
                                ''No hay, mostramos lo de caja
                                ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI' "
                            End If
                        Else
                            ''No es B2B, mostramos lo de caja
                            ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI'  and cvTipo not in ('B2B','Vendedor')"
                            dtValida = objDatos.fnEjecutarConsulta(ssql)
                            If dtValida.Rows.Count = 0 Then
                                ''No tenemos la columna para el tipo= B2B, mostramos lo de caja
                                ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI' "
                            End If
                        End If
                    Else
                        ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEnPrincipal,'SI')='SI'"
                        Dim dtCategoriaValida As New DataTable
                        dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                        If dtCategoriaValida.Rows.Count > 0 Then
                            iEntra = 1
                            sHtmlBanner = sHtmlBanner & "<a class='banners-2' href='" & dtCategoriaValida.Rows(0)("cvUrl") & "'> "
                            sHtmlBanner = sHtmlBanner & "<img src='" & dtCategoriaValida.Rows(0)("cvImagen") & "' alt='banner-1' class='img-responsive'> "
                            sHtmlBanner = sHtmlBanner & "<div class='b-info'> "
                            sHtmlBanner = sHtmlBanner & "<h2 class='tit'> " & dtCategoriaValida.Rows(0)(0) & "</h2>"
                            sHtmlBanner = sHtmlBanner & "<div class='btn-next' href='" & dtCategoriaValida.Rows(0)("cvUrl") & "'><img src='img/botones/next.png' class='img-responsive'></div>"
                            ' sHtmlBanner = sHtmlBanner & "<div class='btn btn-general' href='" & dtCategoriaValida.Rows(0)("cvUrl") & "'>Ver Detalles</div>"
                            sHtmlBanner = sHtmlBanner & "</div></a>"
                        End If

                    End If



                Next

                If sTipoCliente.Contains("BOSS") Then
                    If Session("RazonSocial") <> "" Then

                        ''B2B, obtenemos el tipo de B2B para consultar el SubTipo

                        If Session("TipoB2B") <> "" Then
                            ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE  ISNULL(cvEnPrincipal,'SI')='SI' and cvTipo ='B2B' and cvSubTipo='" & Session("TipoB2B") & "'"
                        Else
                            ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE ISNULL(cvEnPrincipal,'SI')='SI' and cvTipo ='B2B' "
                        End If



                        If CInt(Session("slpCode")) <> 0 Then
                            ''Es Vendedores
                            ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE  ISNULL(cvEnPrincipal,'SI')='SI' and cvTipo='Vendedor' "

                        End If
                        ''Estan como B2B

                        dtValida = objDatos.fnEjecutarConsulta(ssql)
                        If dtValida.Rows.Count = 0 Then
                            ''No hay, mostramos lo de caja
                            ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE  ISNULL(cvEnPrincipal,'SI')='SI' "
                        End If
                    Else
                        ''No es B2B, mostramos lo de caja
                        ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE  ISNULL(cvEnPrincipal,'SI')='SI'  and cvTipo not in ('B2B','Vendedor')"
                        dtValida = objDatos.fnEjecutarConsulta(ssql)
                        If dtValida.Rows.Count = 0 Then
                            ''No tenemos la columna para el tipo= B2B, mostramos lo de caja
                            ssql = "select cvTexto,cvImagen,cvUrl from config.categorias  WHERE  ISNULL(cvEnPrincipal,'SI')='SI' "
                        End If
                    End If


                    Dim dtCategoriaValidaBoss As New DataTable
                    dtCategoriaValidaBoss = objDatos.fnEjecutarConsulta(ssql)

                    For b = 0 To dtCategoriaValidaBoss.Rows.Count - 1 Step 1
                        iEntra = 1
                        sHtmlBanner = sHtmlBanner & "<a class='banners-2' href='" & dtCategoriaValidaBoss.Rows(b)("cvUrl") & "'> "
                        sHtmlBanner = sHtmlBanner & "<img src='" & dtCategoriaValidaBoss.Rows(b)("cvImagen") & "' alt='banner-1' class='img-responsive'> "
                        sHtmlBanner = sHtmlBanner & "<div class='b-info'> "
                        sHtmlBanner = sHtmlBanner & "<h2 class='tit'> " & dtCategoriaValidaBoss.Rows(b)(0) & "</h2>"
                        sHtmlBanner = sHtmlBanner & "<div class='btn-next' href='" & dtCategoriaValidaBoss.Rows(b)("cvUrl") & "'><img src='img/botones/next.png' class='img-responsive'></div>"
                        ' sHtmlBanner = sHtmlBanner & "<div class='btn btn-general' href='" & dtCategoriaValida.Rows(0)("cvUrl") & "'>Ver Detalles</div>"
                        sHtmlBanner = sHtmlBanner & "</div></a>"
                    Next

                End If



                If iEntra = 0 Then
                    ''Las categorias no corresponden
                    For i = 0 To dtCategorias.Rows.Count - 1 Step 1
                        ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias POR CATEGORIA y PINTAMOS el TEXTO
                        ssql = "select cvCategoria,cvTexto,cvImagen,cvUrl from config.categorias  WHERE ISNULL(cvEnPrincipal,'SI')='SI'"
                        Dim dtCategoriaValida As New DataTable
                        dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)
                        iEntra = 1
                        If dtCategoriaValida.Rows.Count > 0 Then
                            sHtmlBanner = sHtmlBanner & "<a class='banners-2' href='" & dtCategoriaValida.Rows(i)("cvUrl") & "'> "
                            sHtmlBanner = sHtmlBanner & "<img src='" & dtCategoriaValida.Rows(i)("cvImagen") & "' alt='banner-1' class='img-responsive'> "
                            sHtmlBanner = sHtmlBanner & "<div class='b-info'> "
                            sHtmlBanner = sHtmlBanner & "<h2 class='tit'> " & dtCategoriaValida.Rows(i)("cvTexto") & "</h2>"
                            sHtmlBanner = sHtmlBanner & "<div class='btn btn-general' href='" & dtCategoriaValida.Rows(i)("cvUrl") & "'>ver detalles</div>"
                            sHtmlBanner = sHtmlBanner & "</div></a>"
                        End If

                    Next
                End If
                sHTMLCAT = sHTMLCAT & sHtmlBanner

            End If

            If sTipoCliente = "STOP" Then
                sHTMLExtraDiv = "<div class='col-xs-12'><a class='btn-4' href='" & sURLColeccion & "'>VER COLECCION</a></div>"
            End If
            sHTMLCAT = sHTMLCAT & "</div>" & sHTMLExtraDiv & "</div></div>" '</div>
            If sTipoCliente = "STOP" Then
                sHTMLCAT = sHTMLCAT & "<div class='seccion purple-section'>     <div class='main-container'><a class='btn-5' href='tallas.aspx'>GUIA DE TALLAS</a>    </div></div>"
            End If
            ''Promociones


            ssql = "SELECT cvCampoSAP,cvValor from config.camposPromocion "

            ''Obtenemos el query de las promociones
            ssql = objDatos.fnObtenerQuery("Promociones")
            Dim dtCampoPromo As New DataTable
            dtCampoPromo = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtCampoPromo.Rows.Count > 0 Then
                ''Evaluamos si existen articulos en promoción
                '  ssql = "SELECT *,IsNULL(ItemName,'') as Descripcion FROM OITM WHERE " & dtCampoPromo.Rows(0)("cvCampoSAP") & " = " & "'" & dtCampoPromo.Rows(0)("cvValor") & "'"
                Dim dtArticulosPromo As New DataTable
                dtArticulosPromo = objDatos.fnEjecutarConsultaSAP(ssql)
                sHtmlBanner = ""
                If dtArticulosPromo.Rows.Count > 0 Then
                    ''Obtenemos el titulo de promociones
                    ssql = "SELECt ISNULL(cvTituloPromociones,'Promociones') as Titulo FROM config .Parametrizaciones "
                    Dim dtTituloProm As New DataTable
                    Dim sTituloProm As String = "Promociones"
                    dtTituloProm = objDatos.fnEjecutarConsulta(ssql)
                    If dtTituloProm.Rows.Count > 0 Then
                        sTituloProm = dtTituloProm.Rows(0)(0)
                    End If



                    sHTMLPRomos = sHTMLPRomos & "<div class='seccion'>"
                    sHTMLPRomos = sHTMLPRomos & " <div class='main-container'>"
                    sHTMLPRomos = sHTMLPRomos & "  <span class='linea top'></span>"
                    sHTMLPRomos = sHTMLPRomos & "   <div class='sec-tit'>" & sTituloProm & "</div>"
                    sHTMLPRomos = sHTMLPRomos & "    <div class='feature-1'>"

                    For i = 0 To dtArticulosPromo.Rows.Count - 1 Step 1
                        Dim sImagen As String = "ImagenPal"
                        sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
                        sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(i)("ItemCode") & "'>"
                        sHtmlBanner = sHtmlBanner & " <div class='preview'>"
                        sHtmlBanner = sHtmlBanner & "  <div class='img'>"
                        sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"
                        sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>oferta</span>"


                        Dim sPintaPrev As String = "SI"
                        Dim sPintaFav As String = "SI"
                        Dim sPintaCompra As String = "SI"

                        ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
                        Dim dtPintaCat As New DataTable
                        dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
                        If dtPintaCat.Rows.Count > 0 Then
                            sPintaPrev = dtPintaCat.Rows(0)("Interior")
                            sPintaFav = dtPintaCat.Rows(0)("Favorito")
                            sPintaCompra = dtPintaCat.Rows(0)("Comprar")
                        End If

                        If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
                            sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
                        Else
                            sHtmlBanner = sHtmlBanner & "     <div>"
                        End If

                        If sPintaCompra = "SI" Then
                            sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(i)("ItemCode") & "'></a>"
                        End If
                        If sPintaFav = "SI" Then
                            sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & dtArticulosPromo.Rows(i)("ItemCode") & "&name=" & dtArticulosPromo.Rows(i)("ItemName") & "'></a>"
                        End If
                        If sPintaCompra = "SI" Then
                            sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & dtArticulosPromo.Rows(i)("ItemCode") & "&Modo=Add'></a>"
                        End If



                        sHtmlBanner = sHtmlBanner & "     </div>"
                        sHtmlBanner = sHtmlBanner & "  </div>"


                        sHtmlBanner = sHtmlBanner & "  <a class='info' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(i)("ItemCode") & "'>"



                        ''Nos traemos los datos a mostrar de acuerdo a la plantilla "PROMO"
                        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='PROMO' order by T1.ciOrden "
                        Dim dtCamposPlantilla As New DataTable
                        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)

                        For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1


                            If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                                Dim sValorMostrar As String = ""
                                If dtArticulosPromo.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
                                    sValorMostrar = ""
                                Else
                                    sValorMostrar = CStr(dtArticulosPromo.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")))
                                End If

                                'If sValorMostrar.Length > 30 Then
                                '    sValorMostrar = sValorMostrar.Substring(0, 30)
                                'End If
                                If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
                                    sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & sValorMostrar & "</div>"
                                Else
                                    sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & sValorMostrar & "</div>"
                                End If
                            Else
                                If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then
                                    Dim dPrecioActual As Double
                                    If CInt(Session("slpCode")) <> 0 Then

                                        dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(i)("ItemCode"), Session("ListaPrecios"))
                                    Else
                                        dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(i)("ItemCode"))
                                    End If
                                    If Session("Cliente") <> "" Then
                                        dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(i)("ItemCode"), Session("ListaPrecios"))
                                    End If
                                    sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & dPrecioActual.ToString("$ ###,###,###.#0") & "</span>"
                                Else

                                    If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then

                                        If dtArticulosPromo.Rows(i)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
                                            sImagen = "images/no-image.png"
                                        Else
                                            sImagen = dtArticulosPromo.Rows(i)(dtCamposPlantilla.Rows(x)("Campo"))
                                        End If

                                        sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)
                                    End If

                                End If
                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & "  </a>"

                        'sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtArticulosPromo.Rows(i)("ItemCode") & "</div>"
                        'sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtArticulosPromo.Rows(i)("ItemName") & "</div>"
                        'sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & "$ 250.00" & "</span>"
                        'sHtmlBanner = sHtmlBanner & "  </a>"
                        sHtmlBanner = sHtmlBanner & " </div>"

                        sHtmlBanner = sHtmlBanner & "  </a>"

                        sHtmlBanner = sHtmlBanner & "</div>"
                    Next
                    sHTMLPRomos = sHTMLPRomos & sHtmlBanner
                    sHTMLPRomos = sHTMLPRomos & "</div></div></div> </div>"

                End If
            End If

        Catch ex As Exception

        End Try
        '       sHtmlEncabezado = sHtmlEncabezado & "</div>" ''WrapperCon Naranja
        Dim sHtmlBarras As String = ""
        sHtmlBarras = fnCargaBarrasv2()
        sHtmlEncabezado = sHtmlEncabezado & sHTMLPRomos & sHTMLCAT & sHtmlBarras & sHTMLBannerIntermedio
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlBanners.Controls.Clear()
        pnlBanners.Controls.Add(literal)

    End Sub
    <WebMethod>
    Public Shared Function RegisterUser(Articulo As String) As String

        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones
        partida.ItemCode = Articulo
        partida.Cantidad = 1
        Dim dPrecioActual As Double = 0
        If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
        Else
            dPrecioActual = objDatos.fnPrecioActual(Articulo)
        End If
        If HttpContext.Current.Session("Cliente") <> "" Then
            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
        End If

        partida.Precio = dPrecioActual
        partida.TotalLinea = partida.Cantidad * partida.Precio

        ''Ahora el itemName
        Try
            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            partida.ItemName = dtItemName.Rows(0)(0)
        Catch ex As Exception

        End Try


        HttpContext.Current.Session("WishList").add(partida)

        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Public Sub fnCargaBarras()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        ssql = "SELECT * FROM config.Barras WHERE cvEstatus='ACTIVO'"
        Dim dtBarras As New DataTable
        dtBarras = objDatos.fnEjecutarConsulta(ssql)


        For i = 0 To dtBarras.Rows.Count - 1 Step 1
            sHtmlEncabezado = sHtmlEncabezado & "<div class='wrappercon naranja'>"
            sHtmlEncabezado = sHtmlEncabezado & " <div class='seccion'>"
            sHtmlEncabezado = sHtmlEncabezado & "  <div class='main-container'>"
            sHtmlEncabezado = sHtmlEncabezado & "   <span class='linea top'></span>"
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='sec-tit'>" & dtBarras.Rows(i)("cvDescripcion") & "</div>"
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='feature-2'>"

            ssql = "SELECT cvItemcode, cvItemCode as itemcode,cvItemName as ItemName from config.barras_detalle where ciIdBarra=" & "'" & dtBarras.Rows(i)("ciIdBarra") & "'"
            Dim dtArticulosPromo As New DataTable
            dtArticulosPromo = objDatos.fnEjecutarConsulta(ssql)
            For y = 0 To dtArticulosPromo.Rows.Count - 1 Step 1
                sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
                sHtmlBanner = sHtmlBanner & " <div class='cont-acciones'>"

                sHtmlBanner = sHtmlBanner & "  <div class='action-products'>"
                sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & dtArticulosPromo.Rows(y)("cvItemCode") & "'></a>"
                sHtmlBanner = sHtmlBanner & "      <div class='item view' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("cvItemCode") & "'></div>"
                sHtmlBanner = sHtmlBanner & "     </div>"
                sHtmlBanner = sHtmlBanner & "  </div>"
                sHtmlBanner = sHtmlBanner & "<a class='preview' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("cvItemCode") & "'>"
                ''Nos traemos los datos a mostrar de acuerdo a la plantilla "PROMO"
                ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='PROMO' order by T1.ciOrden "
                Dim dtCamposPlantilla As New DataTable
                dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
                sHtmlBanner = sHtmlBanner & "   <div class='img'><img src='img/home/producto-1.png'><span class='b-oferta'>oferta</span></div>"
                sHtmlBanner = sHtmlBanner & "  <div class='info' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("cvItemCode") & "'>"
                For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1


                    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
                            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
                        Else
                            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo")) & "</div>"
                        End If
                    Else
                        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then
                            Dim dPrecioActual As Double
                            If CInt(Session("slpCode")) <> 0 Then

                                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("cvItemCode"), Session("ListaPrecios"))
                            Else
                                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("cvItemCode"))
                            End If
                            If Session("Cliente") <> "" Then
                                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("cvItemCode"), Session("ListaPrecios"))
                            End If
                            sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & dPrecioActual.ToString("$ ###,###,###.#0") & "</span>"
                        Else
                            If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then


                            End If
                        End If
                    End If
                Next
                sHtmlBanner = sHtmlBanner & " </div>" ''Class info
                sHtmlBanner = sHtmlBanner & "  </a>"

                sHtmlBanner = sHtmlBanner & "</div>"
            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & "</div></div></div> </div>"
        Next
        sHtmlEncabezado = sHtmlEncabezado
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlBarras.Controls.Clear()
        pnlBarras.Controls.Add(literal)

    End Sub
    Public Function fnCargaBarrasv2()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        ssql = "SELECT * FROM config.Barras WHERE cvEstatus='ACTIVO'"
        Dim dtBarras As New DataTable
        dtBarras = objDatos.fnEjecutarConsulta(ssql)

        Try
            For i = 0 To dtBarras.Rows.Count - 1 Step 1
                '  objDatos.fnLog("Entra Barras2", dtBarras.Rows.Count)
                sHtmlEncabezado = sHtmlEncabezado & "<div class='wrappercon naranja'>"
                sHtmlEncabezado = sHtmlEncabezado & " <div class='seccion'>"
                sHtmlEncabezado = sHtmlEncabezado & "  <div class='main-container'>"
                sHtmlEncabezado = sHtmlEncabezado & "   <span class='linea top'></span>"
                sHtmlEncabezado = sHtmlEncabezado & "   <div class='sec-tit'>" & dtBarras.Rows(i)("cvDescripcion") & "</div>"
                sHtmlEncabezado = sHtmlEncabezado & "   <div class='feature-1'>"

                'ssql = "SELECT cvItemcode, cvItemCode as itemcode,cvItemName as ItemName,cvItemName as FrgnName,ISNULL(cvImagen,'') as Imagen from config.barras_detalle where ciIdBarra=" & "'" & dtBarras.Rows(i)("ciIdBarra") & "'"
                ssql = objDatos.fnObtenerQuery(dtBarras.Rows(i)("cvQuery"))

                Dim dtArticulosPromo As New DataTable
                dtArticulosPromo = objDatos.fnEjecutarConsultaSAP(ssql)
                For y = 0 To dtArticulosPromo.Rows.Count - 1 Step 1
                    sHtmlBanner = sHtmlBanner & objDatos.fnCreaFichaProducto(dtArticulosPromo.Rows(y)("ItemCode"), Server.MapPath("~"), Session("slpCode"), Session("ListaPrecios"), Session("UserB2B"), Session("UserB2C"), Session("Cliente"))
                    '  objDatos.fnLog("Entra Barras2", dtArticulosPromo.Rows.Count)

                    ''Dim sImagen As String = "ImagenPal"
                    ''sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
                    ''sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "'>"
                    ''sHtmlBanner = sHtmlBanner & " <div class='preview'>"
                    ''sHtmlBanner = sHtmlBanner & "  <div class='img'>"
                    ''sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"
                    ''If objDatos.fnArticuloOferta(dtArticulosPromo.Rows(y)("ItemCode")) = "SI" Then
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
                    ''    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "'></a>"
                    ''End If
                    ''If sPintaFav = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & dtArticulosPromo.Rows(y)("ItemCode") & "&name=" & dtArticulosPromo.Rows(y)("ItemName") & "'></a>"
                    ''End If
                    ''If sPintaCompra = "SI" Then
                    ''    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "&Modo=Add'></a>"
                    ''End If



                    ''sHtmlBanner = sHtmlBanner & "     </div>"
                    ''sHtmlBanner = sHtmlBanner & "  </div>"



                    ''sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "'>"
                    ''''Nos traemos los datos a mostrar de acuerdo a la plantilla "PROMO"
                    ''ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    ''    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    ''    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='BARRAS' order by T1.ciOrden "
                    ''Dim dtCamposPlantilla As New DataTable
                    ''dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)

                    ''For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
                    ''    ' objDatos.fnLog("Entra Barras2 campos plantilla", dtCamposPlantilla.Rows(x)("Campo"))
                    ''    '  objDatos.fnLog("Entra Barras2 campos plantilla", dtCamposPlantilla.Rows.Count)
                    ''    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                    ''        Dim sValorMostrar As String = ""
                    ''        sValorMostrar = CStr(dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo")))
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
                    ''            Dim dPrecioActual As Double
                    ''            If CInt(Session("slpCode")) <> 0 Then

                    ''                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("ItemCode"), Session("ListaPrecios"))
                    ''            Else
                    ''                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("ItemCode"))
                    ''            End If
                    ''            If Session("Cliente") <> "" Then
                    ''                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("ItemCode"), Session("ListaPrecios"))
                    ''            End If
                    ''            sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & dPrecioActual.ToString("$ ###,###,###.#0") & " " & fnObtenerMoneda(dtArticulosPromo.Rows(y)("ItemCode")) & "</span>"
                    ''        Else
                    ''            If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
                    ''                If dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
                    ''                    sImagen = "images/no-image.png"
                    ''                Else
                    ''                    sImagen = dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo"))
                    ''                End If

                    ''                sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)

                    ''            End If
                    ''        End If
                    ''    End If
                    ''    '  objDatos.fnLog("Entra Barras2 articulo: ", dtArticulosPromo.Rows(y)("ItemCode"))
                    ''Next
                    ''sHtmlBanner = sHtmlBanner & "  </a>"
                    ''sHtmlBanner = sHtmlBanner & " </div>"
                    ''sHtmlBanner = sHtmlBanner & "  </a>"
                    ''sHtmlBanner = sHtmlBanner & "</div>"
                Next
                sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                sHtmlEncabezado = sHtmlEncabezado & "</div></div></div> </div>"
            Next
            sHtmlEncabezado = sHtmlEncabezado

            '  objDatos.fnLog("El HTML de Novedades", sHtmlEncabezado.Replace("'", ""))
            'Dim literal As New LiteralControl(sHtmlEncabezado)
            'pnlBarras.Visible = True
            'pnlBarras.Controls.Clear()
            'pnlBarras.Controls.Add(literal)

            '  Dim literalDestacados As New LiteralControl(sHtmlEncabezado)
            ''   pnlBanners.Controls.Clear()
            ' pnlBanners.Controls.Add(literalDestacados)
        Catch ex As Exception
            '     objDatos.fnLog("Error en Barras", ex.Message)
        End Try

        Return sHtmlEncabezado

    End Function
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

                sCondicionCategoria = " AND U_categoria=" & "'" & ddlCategoria.SelectedValue & "' "
            Else

                '  sCondicionCategoria = " AND T1.ItmsGrpNam=" & "'" & ddlCategoria.SelectedValue & "'"
                sCondicionCategoria = " AND Grupo=" & "'" & ddlCategoria.SelectedValue & "'"
            End If
        End If

        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then

        Else
            If ddlSubcategoria.SelectedValue <> "-TODOS-" And ddlSubcategoria.SelectedValue.Length > 0 Then
                sCondicionSubLinea = " AND subgrupo=" & "'" & ddlSubcategoria.SelectedValue & "'"
            End If
        End If


        If ddlMarca.SelectedValue <> "-TODOS-" And ddlMarca.SelectedValue.Length > 0 Then
            '  sCondicionMarca = " AND T0.itemcode in (SELECT U_Articulo FROM [@MODELOS] where u_marca=" & "'" & ddlMarca.SelectedValue & "')"
            If sEmpresa = "PMK" Then
                sCondicionMarca = " AND U_Marca=" & "'" & ddlMarca.SelectedValue & "' "
            Else
                sCondicionMarca = " AND ARMADORA=" & "'" & ddlMarca.SelectedValue & "'"
            End If

        End If

        If ddlModelo.SelectedValue <> "-TODOS-" And ddlModelo.SelectedValue.Length > 0 Then
            '  sCondicionModelo = " AND T0.itemcode in (SELECT distinct U_Articulo FROM [@MODELOS] where u_Modelo=" & "'" & ddlModelo.SelectedValue & "')"
            If sEmpresa = "PMK" Then
                sCondicionModelo = " AND U_Modelo=" & "'" & ddlModelo.SelectedValue & "' "
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

        If sEmpresa = "PMK" Then
            ssql = objDatos.fnObtenerQuery("filtraCategoria")
        Else
            ssql = "SELECT distinct T1.ItmsGrpNam  FROM OITM T0 WITH(nolock) INNER JOIN OITB T1 WITH(nolock) ON T1.ItmsGrpCod=T0.ItmsGrpCod WHERE T0.SellItem='Y'  AND T0.validfor='Y' AND itemcode in( Select Distinct U_Articulo  from [@MODELOS]) "
            ssql = "SELECT DISTINCT Grupo as ItmsGrpNam FROM SAP_Tienda..TablaModelos6 where 1=1  "
        End If

        If Session("AIO_OPC") = "Categoria" Then
            sCondicion = sCondicionSubLinea & sCondicionMarca & sCondicionModelo
        End If
        ssql = ssql & "  " & sCondicion & " Order by ItmsGrpNam "
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

            ssql = ssql & "  " & sCondicion & " Order by sublinea "
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
        ssql = ssql & "  " & sCondicion & " Order by u_marca "
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
            ssql = "  Select  distinct AnioDe as U_AnioDe ,AnioHasta as U_AnioHasta    FROM SAP_Tienda..tablaModelos6 where (AnioDe is not null and AnioHasta is not null)  "
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

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click

        ''Evaluamos lo que esta seleccionado

        ''Evaluamos lo que esta seleccionado
        Dim sCondicion As String = " AND 1=1 "
        Session("CatBuscar") = ""
        Session("SubCatBuscar") = ""
        Session("MarcaBuscar") = ""
        Session("ModeloBuscar") = ""
        Session("AnioBuscar") = ""

        If CStr(objDatos.fnObtenerCliente()).ToUpper().Contains("PMK") Then
            If ddlCategoria.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND T1.U_Categoria=" & "'" & ddlCategoria.SelectedValue & "'"
                Session("CatBuscar") = ddlCategoria.SelectedValue
                objDatos.fnLog("CatBuscar", Session("CatBuscar"))
            End If



            If ddlMarca.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND T1.u_marca=" & "'" & ddlMarca.SelectedValue & "'"
                Session("MarcaBuscar") = ddlMarca.SelectedValue
                objDatos.fnLog("MarcaBuscar", Session("MarcaBuscar"))
            End If

            If ddlModelo.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND T1.U_Modelo=" & "'" & ddlModelo.SelectedValue & "'"
                Session("ModeloBuscar") = ddlModelo.SelectedValue
                objDatos.fnLog("ModeloBuscar", Session("ModeloBuscar"))
            End If

            If ddlAnio.SelectedValue <> "-TODOS-" Then
                sCondicion = sCondicion & " AND " & "'" & ddlAnio.SelectedValue & "' between U_AnioDe and U_AnioHasta"
                Session("AnioBuscar") = ddlAnio.SelectedValue
                objDatos.fnLog("AnioBuscar", Session("AnioBuscar"))
            End If

            Session("BusquedaAIO_Index") = "SI"

        Else
            If ddlCategoria.SelectedValue <> "-TODOS-" And ddlCategoria.SelectedItem.Text <> "" Then
                '   sCondicion = sCondicion & " AND T1.ItmsGrpNam=" & "'" & ddlCategoria.SelectedValue & "'"
                sCondicion = sCondicion & " AND Grupo=" & "'" & ddlCategoria.SelectedValue & "'"
                Session("CatBuscar") = ddlCategoria.SelectedValue
                objDatos.fnLog("CatBuscar", Session("CatBuscar"))
            End If

            If ddlSubcategoria.SelectedValue <> "-TODOS-" And ddlSubcategoria.SelectedItem.Text <> "" Then
                'sCondicion = sCondicion & " AND T0.U_sublinea=" & "'" & ddlSubcategoria.SelectedValue & "'"
                sCondicion = sCondicion & " AND SubGrupo=" & "'" & ddlSubcategoria.SelectedValue & "'"
                Session("SubCatBuscar") = ddlSubcategoria.SelectedValue
                objDatos.fnLog("SubCatBuscar", Session("SubCatBuscar"))
            End If

            If ddlMarca.SelectedValue <> "-TODOS-" And ddlMarca.SelectedItem.Text <> "" Then
                'sCondicion = sCondicion & " AND T0.itemcode in (SELECT U_Articulo FROM [@MODELOS] where u_marca=" & "'" & ddlMarca.SelectedValue & "')"
                sCondicion = sCondicion & " AND armadora=" & "'" & ddlMarca.SelectedValue & "'"
                Session("MarcaBuscar") = ddlMarca.SelectedValue
                objDatos.fnLog("MarcaBuscar", Session("MarcaBuscar"))
            End If

            If ddlModelo.SelectedValue <> "-TODOS-" And ddlModelo.SelectedItem.Text <> "" Then
                sCondicion = sCondicion & " AND Modelo=" & "'" & ddlModelo.SelectedValue & "'"
                Session("ModeloBuscar") = ddlModelo.SelectedValue
                objDatos.fnLog("ModeloBuscar", Session("ModeloBuscar"))
            End If

            If ddlAnio.SelectedValue <> "-TODOS-" And ddlAnio.SelectedItem.Text <> "" Then
                sCondicion = sCondicion & " AND " & "'" & ddlAnio.SelectedValue & "' between AnioDe and AnioHasta"
                Session("AnioBuscar") = ddlAnio.SelectedValue
                objDatos.fnLog("AnioBuscar", Session("AnioBuscar"))
            End If


            Session("BusquedaAIO_Index") = "SI"

        End If

        objDatos.fnLog("btnBuscar index-condicion", sCondicion.Replace("'", ""))



        Session("BusquedaAIO_Index") = "SI"
        Session("BusquedaAIO_Criterios") = sCondicion
        Response.Redirect("Catalogo.aspx")





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

    Private Sub ddlMarca_PreRender(sender As Object, e As EventArgs) Handles ddlMarca.PreRender

    End Sub
End Class
