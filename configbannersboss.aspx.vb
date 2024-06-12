
Imports System.Data
Imports System.IO

Partial Class configbannersboss
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub configbannersboss_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'If Session("loginAdmin") = "" Then
            '    Response.Redirect("loginadmin.aspx")
            'End If

            Dim dtTiposBanners As New DataTable
            dtTiposBanners.Columns.Add("Codigo")
            dtTiposBanners.Columns.Add("Descripcion")

            Dim filaBanner As DataRow
            filaBanner = dtTiposBanners.NewRow
            filaBanner("Codigo") = "0"
            filaBanner("Descripcion") = "-Seleccione-"
            dtTiposBanners.Rows.Add(filaBanner)

            filaBanner = dtTiposBanners.NewRow
            filaBanner("Codigo") = "B2C"
            filaBanner("Descripcion") = "B2C"
            dtTiposBanners.Rows.Add(filaBanner)

            filaBanner = dtTiposBanners.NewRow
            filaBanner("Codigo") = "B2B1"
            filaBanner("Descripcion") = "B2B-Empresa o corp"
            dtTiposBanners.Rows.Add(filaBanner)

            filaBanner = dtTiposBanners.NewRow
            filaBanner("Codigo") = "B2B2"
            filaBanner("Descripcion") = "B2B-Tienda"
            dtTiposBanners.Rows.Add(filaBanner)

            filaBanner = dtTiposBanners.NewRow
            filaBanner("Codigo") = "Vendedor"
            filaBanner("Descripcion") = "Vendedor"
            dtTiposBanners.Rows.Add(filaBanner)

            ddlTiposBanners.DataSource = dtTiposBanners
            ddlTiposBanners.DataTextField = "descripcion"
            ddlTiposBanners.DataValueField = "codigo"
            ddlTiposBanners.DataBind()

            ddlTiposBanners.SelectedValue = "0"

            Dim dtUbicacionBanners As New DataTable
            dtUbicacionBanners.Columns.Add("Codigo")
            dtUbicacionBanners.Columns.Add("Descripcion")
            Dim filaUbiBanner As DataRow
            filaUbiBanner = dtUbicacionBanners.NewRow
            filaUbiBanner("Codigo") = "0"
            filaUbiBanner("Descripcion") = "-Seleccione-"
            dtUbicacionBanners.Rows.Add(filaUbiBanner)

            filaUbiBanner = dtUbicacionBanners.NewRow
            filaUbiBanner("Codigo") = "Principal"
            filaUbiBanner("Descripcion") = "Superior (alargados)"
            dtUbicacionBanners.Rows.Add(filaUbiBanner)

            filaUbiBanner = dtUbicacionBanners.NewRow
            filaUbiBanner("Codigo") = "Inferior"
            filaUbiBanner("Descripcion") = "Pequeños (2)"
            dtUbicacionBanners.Rows.Add(filaUbiBanner)

            ddlTipo.DataSource = dtUbicacionBanners
            ddlTipo.DataTextField = "Descripcion"
            ddlTipo.DataValueField = "Codigo"
            ddlTipo.DataBind()


            Session("IdBanner") = ""
            ''Cargamos los lenguajes
            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            rcbLenguaje.DataSource = dtLenguajes
            rcbLenguaje.DataTextField = "cvLenguaje"
            rcbLenguaje.DataValueField = "ciIdLenguaje"
            rcbLenguaje.DataBind()

            ''Loas artículos
            ssql = "select ItemCode,ItemName  from OITM WHERE validFor ='Y' AND SellItem ='Y' "
            Dim dtArticulos As New DataTable
            dtArticulos = objDatos.fnEjecutarConsultaSAP(ssql)
            rcbArticulos.DataSource = dtArticulos
            rcbArticulos.DataTextField = "ItemName"
            rcbArticulos.DataValueField = "ItemCode"
            rcbArticulos.DataBind()

            '  fnCargaBanners()

            If Request.QueryString.Count > 0 Then
                Session("IdBanner") = Request.QueryString("Id")
                Session("TipoBanner") = Request.QueryString("Tipo")
                ''Obtenemos los datos para rellenar
                If Session("TipoBanner") = "Principal" Then
                    ssql = "SELECT ciId as Id,cvDescripcion as Nombre,cvRutaImagen as Imagen,cvTexto as Texto,cvLenguaje as Lenguaje,cvItemCode as Articulo, cfPrecio as Precio,cfPrecioPromo as PrecioPromo,cfDescuento as Descuento,cvEspecifico as Especifico,ciOrden as Orden,cvEstatus as estatus FROM config.Banners WHERE ciId= " & "'" & Request.QueryString("Id") & "'"
                Else
                    ssql = "select ciIdRel as Id,cvTipo as Tipo,cvSubTipo as SubTipo, cvImagen as Imagen,'' as Texto,'' as Nombre,'ACTIVO' as Estatus from config.Categorias  WHERE ciIdRel= " & "'" & Request.QueryString("Id") & "'"
                End If

                Try
                    ddlTipo.SelectedValue = Session("TipoBanner")
                    If Request.QueryString("Tipob") = "" Then
                        ddlTiposBanners.SelectedValue = "B2C"
                    Else
                        ddlTiposBanners.SelectedValue = Request.QueryString("Tipob")
                    End If
                Catch ex As Exception

                End Try
                Dim dtBanner As New DataTable
                dtBanner = objDatos.fnEjecutarConsulta(ssql)

                If dtBanner.Rows.Count > 0 Then
                    txtNombre.Text = dtBanner.Rows(0)("Nombre")
                    txtTexto.Text = CStr(dtBanner.Rows(0)("Texto")).Replace("&nbsp;", "")
                    imgBanner.ImageUrl = dtBanner.Rows(0)("Imagen")
                    imgBanner.Visible = True
                    Session("Imagen") = dtBanner.Rows(0)("Imagen")
                    If dtBanner.Rows(0)("Estatus") = "ACTIVO" Then
                        chkActivo.Checked = True
                    Else
                        chkActivo.Checked = False
                    End If
                    btnAgregar.Text = "Actualizar"
                End If
            End If
            fnCargarBannersTipo()

        End If
    End Sub

    Public Sub fnCargaBanners(Tipo As String, SubTipo As String)

        Dim sTipoLiga As String = Tipo
        If Tipo = "B2C" Then
            Tipo = ""
        End If

        ''Los banners activos
        ssql = "SELECT ciId as Id,cvDescripcion as Nombre,cvRutaImagen as Imagen,cvTexto as Texto,cvLenguaje as Lenguaje,cvItemCode as Articulo, cfPrecio as Precio,cfPrecioPromo as PrecioPromo,cfDescuento as Descuento,cvEspecifico as Especifico,ciOrden as Orden,cvEstatus as estatus FROM config.Banners WHERE cvTipo='" & Tipo & "' and cvSubTipo='" & SubTipo & "'   order by ciOrden "
        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"
        For i = 0 To dtBanners.Rows.Count - 1 Step 1

            sHTML = sHTML & "<tr>"
            Try
                If Request.QueryString.Count > 0 Then
                    If Request.QueryString("Id") = dtBanners.Rows(i)("Id") And Request.QueryString("Tipo") = "Principal" Then
                        sClass = "class='bg-danger text-white'"
                    Else
                        sClass = ""
                    End If

                Else
                    sClass = ""

                End If

            Catch ex As Exception

            End Try

            sHTML = sHTML & "<td " & sClass & "><a href='configbannersboss.aspx?Id=" & dtBanners.Rows(i)("Id") & "&Tipo=Principal&Tipob=" & sTipoLiga & "&Subtipob=" & SubTipo & "'>" & dtBanners.Rows(i)("Id") & "</a></td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Nombre") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Imagen") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Texto") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Orden") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Estatus") & "</td>"
            sHTML = sHTML & "</tr>"
        Next
        Dim literal As New LiteralControl(sHTML)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub

    Public Sub fnCargaBannersCat(Tipo As String, SubTipo As String)
        Dim sTipoLiga As String = Tipo

        ''Los banners activos
        If Tipo = "B2C" Then
            Tipo = ""
        End If
        ssql = "select ciIdRel as Id,cvTipo as Tipo,cvSubTipo as SubTipo, cvImagen as Banner,'ACTIVO' as Estatus from config.Categorias  WHERE cvTipo='" & Tipo & "' and cvSubTipo='" & SubTipo & "' "
        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"
        For i = 0 To dtBanners.Rows.Count - 1 Step 1

            sHTML = sHTML & "<tr>"
            Try
                If Request.QueryString.Count > 0 Then
                    If Request.QueryString("Id") = dtBanners.Rows(i)("Id") And Request.QueryString("Tipo") = "Cat" Then
                        sClass = "class='bg-danger text-white'"
                    Else
                        sClass = ""
                    End If

                Else
                    sClass = ""

                End If

            Catch ex As Exception

            End Try

            sHTML = sHTML & "<td " & sClass & "><a href='configbannersboss.aspx?Id=" & dtBanners.Rows(i)("Id") & "&Tipo=Cat&Tipob=" & Tipo & "&Subtipob=" & SubTipo & "'>" & dtBanners.Rows(i)("Id") & "</a></td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Tipo") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("SubTipo") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Banner") & "</td>"

            sHTML = sHTML & "</tr>"
        Next
        Dim literal As New LiteralControl(sHTML)
        pnlBannersCat.Controls.Clear()
        pnlBannersCat.Controls.Add(literal)
    End Sub


    Public Function fnCuentaBannersCat(Tipo As String, SubTipo As String) As Int16
        Dim sTipoLiga As String = Tipo
        Dim iCuantoas As Int16 = 2
        ''Los banners activos
        If Tipo = "B2C" Then
            Tipo = ""
        End If
        ssql = "select ciIdRel as Id,cvTipo as Tipo,cvSubTipo as SubTipo, cvImagen as Banner,'ACTIVO' as Estatus from config.Categorias  WHERE cvTipo='" & Tipo & "' and cvSubTipo='" & SubTipo & "' "
        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"

        iCuantoas = dtBanners.Rows.Count

        Return iCuantoas
    End Function


    Private Sub ddlTiposBanners_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTiposBanners.SelectedIndexChanged
        Try
            fnCargarBannersTipo()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If imgBanner.ImageUrl = "" Then
            objDatos.Mensaje("Elija una imagen para el banner", Me.Page)
            Exit Sub
        End If
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre que identifique el banner", Me.Page)
            Exit Sub
        End If
        If ddlTipo.SelectedValue = "0" Then
            objDatos.Mensaje("Debe indicar si es un banner superior o inferior", Me.Page)
            Exit Sub
        End If
        Dim iEspecifico As Int16 = 0
        Dim sEspecifico As String = "NO"

        Dim sEstatus As String = "NO"
        If chkEspecifico.Checked Then
            iEspecifico = 1
            sEspecifico = "SI"
        End If

        If chkActivo.Checked Then
            sEstatus = "ACTIVO"
        Else
            sEstatus = "INACTIVO"
        End If
        sEstatus = "ACTIVO"
        Dim sCategoria As String = "DESPENSAS"

        If btnAgregar.Text = "Actualizar" Then
            ''Eliminamos y dejamos el flujo natural de una inserción
            If Session("TipoBanner") = "Principal" Then
                ssql = "DELETE FROM Config.Banners where ciId=" & "'" & Session("IdBanner") & "'"
            Else
                ssql = "SELECT cvCategoria FROM  Config.Categorias where ciIdRel=" & "'" & Session("IdBanner") & "'"
                Dim dtCat As New DataTable
                dtCat = objDatos.fnEjecutarConsulta(ssql)
                If dtCat.Rows.Count > 0 Then
                    sCategoria = dtCat.Rows(0)(0)
                End If
                ssql = "DELETE FROM Config.Categorias where ciIdRel=" & "'" & Session("IdBanner") & "'"
            End If
            objDatos.fnEjecutarInsert(ssql)
        End If

        If txtPrecioEspecial.Text = "" Then
                txtPrecioEspecial.Text = "0"
            End If
        If txtDescto.Text = "" Then
            txtDescto.Text = "0"
        End If
        Dim sTipo As String = ""
        Dim sSubtipo As String = ""
        Dim sTipoBanner As String()



        sTipoBanner = ddlTiposBanners.SelectedItem.Text.Split("-")
        If sTipoBanner.Count > 1 Then
            sTipo = sTipoBanner(0)
            sSubtipo = sTipoBanner(1)
        Else
            sTipo = sTipoBanner(0)
        End If
        If sTipo = "B2C" Then
            sTipo = ""
        End If
        If sSubtipo = "Seleccione" Then
            sSubtipo = ""
        End If
        If Session("TipoBanner") = "Principal" Then
            ssql = "INSERT INTO config.Banners (cvDescripcion,cvRutaImagen,cvTexto,cvLenguaje,cvEspecifico,cvItemCode,cfPrecio,cfPrecioPromo,cfDescuento, ciOrden,cvEstatus,cvTipo,cvsubtipo) VALUES(" _
                & "'" & txtNombre.Text & "'," _
                & "'" & Session("Imagen") & "'," _
                & "'" & txtTexto.Text & "'," _
                & "'" & rcbLenguaje.SelectedItem.Text & "'," _
                & "'" & sEspecifico & "'," _
                & "'" & rcbArticulos.SelectedValue & "'," _
                & "'" & txtPrecioActual.Text & "'," _
                & "'" & txtPrecioEspecial.Text & "'," _
                & "'" & txtDescto.Text & "'," _
                & "'" & txtOrden.Text & "','" & sEstatus & "'," _
                & "'" & sTipo & "'," _
                & "'" & sSubtipo & "')"
        Else
            If fnCuentaBannersCat(sTipo, sSubtipo) < 2 Then
                ssql = "INSERT INTO config.Categorias (cvTipo,cvSubTipo,cvImagen,cvCategoria,cvEstatus,cvEnPrincipal) VALUES(" _
             & "'" & sTipo & "'," _
             & "'" & sSubtipo & "'," _
             & "'" & Session("Imagen") & "'," _
             & "'" & sCategoria & "'," _
             & "'ACTIVO'," _
             & "'SI')"
            Else
                objDatos.Mensaje("Ya existen 2 banners inferiores, primero elimine 1 antes de añadir otro", Me.Page)
                fnCargarBannersTipo()
                Exit Sub
            End If
        End If







        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Banner Guardado!", Me.Page)
        txtNombre.Text = ""
        txtOrden.Text = ""
        txtTexto.Text = ""
        imgBanner.Visible = False
        Session("IdBanner") = ""
        fnCargarBannersTipo()
    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        If Session("TipoBanner") = "Principal" Then
            ssql = "DELETE FROM Config.Banners where ciId=" & "'" & Session("IdBanner") & "'"
        Else
            ssql = "DELETE FROM Config.Categorias where ciIdRel=" & "'" & Session("IdBanner") & "'"
        End If

        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Banner Eliminado!", Me.Page)
        txtNombre.Text = ""
        txtOrden.Text = ""
        txtTexto.Text = ""
        imgBanner.Visible = False
        Session("IdBanner") = ""
        fnCargarBannersTipo()



    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Response.Redirect("configbannersboss.aspx")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FileUpload1.HasFile Then
            Dim sTipoBanner As String()
            Dim sTipo As String = ""
            Dim sSubtipo As String = ""

            Dim sCarPeta As String = "banners"

            If ddlTipo.SelectedValue = "Inferior" Then
                sCarPeta = sCarPeta & "/categorias"
            End If

            sTipoBanner = ddlTiposBanners.SelectedItem.Text.Split("-")
            If sTipoBanner.Count > 1 Then
                sTipo = sTipoBanner(0)
                sSubtipo = sTipoBanner(1)
            Else
                sTipo = sTipoBanner(0)
            End If
            If sTipo = "B2B" Then
                sCarPeta = sCarPeta & "/B2B"
            End If
            If sTipo = "Vendedor" Then
                sCarPeta = sCarPeta & "/Vendedor/"
            End If
            If sTipo = "B2C" Or sTipo = "" Then
                sCarPeta = sCarPeta & "/B2C/"
            End If
            If sSubtipo = "Empresa o corp" Then
                sCarPeta = sCarPeta & "/Empresa/"
            End If

            If sSubtipo = "Tienda" Then
                sCarPeta = sCarPeta & "/Tienda/"
            End If


            Dim fileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Images/" + sCarPeta) + fileName)
            imgBanner.ImageUrl = "Images/" + sCarPeta + fileName
            Session("Imagen") = "Images/" + sCarPeta + fileName
            imgBanner.Visible = True

            fnCargarBannersTipo()
        End If
    End Sub
    Public Sub fnCargarBannersTipo()
        Dim sTipo As String = ""
        Dim sSubtipo As String = ""
        Dim sTipoBanner As String()

        If Request.QueryString.Count > 0 Then
            sTipo = Request.QueryString("Tipob")
            sSubtipo = Request.QueryString("Subtipob")
        Else
            If ddlTiposBanners.SelectedValue = "0" Then
                pnlRegistros.Controls.Clear()
                pnlBannersCat.Controls.Clear()
                Exit Sub
            End If
            sTipoBanner = ddlTiposBanners.SelectedItem.Text.Split("-")
            If sTipoBanner.Count > 1 Then
                sTipo = sTipoBanner(0)
                sSubtipo = sTipoBanner(1)
            Else
                sTipo = sTipoBanner(0)
            End If
            If sTipo = "B2C" Then
                sTipo = ""
                sSubtipo = ""
            End If
        End If

        sSubtipo = sSubtipo.Replace("%20", " ")

        fnCargaBanners(sTipo, sSubtipo)
        fnCargaBannersCat(sTipo, sSubtipo)
    End Sub

    Private Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        Try
            Session("TipoBanner") = ddlTipo.SelectedValue
            fnCargarBannersTipo()
        Catch ex As Exception

        End Try
    End Sub
End Class
