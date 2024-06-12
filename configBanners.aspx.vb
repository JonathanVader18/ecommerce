
Imports System.Data
Imports System.IO
Imports Telerik.Web.UI

Partial Class configBanners
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Public Sub fnCargaBanners()
        ''Los banners activos
        ssql = "SELECT ciId as Id,cvDescripcion as Nombre,cvRutaImagen as Imagen,cvTexto as Texto,cvLenguaje as Lenguaje,cvItemCode as Articulo, cfPrecio as Precio,cfPrecioPromo as PrecioPromo,cfDescuento as Descuento,cvEspecifico as Especifico,ciOrden as Orden,cvEstatus as estatus FROM config.Banners   order by ciOrden "
        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"
        For i = 0 To dtBanners.Rows.Count - 1 Step 1

            sHTML = sHTML & "<tr>"
            Try
                If Request.QueryString.Count > 0 Then
                    If Request.QueryString("Id") = dtBanners.Rows(i)("Id") Then
                        sClass = "class='bg-danger text-white'"
                    Else
                        sClass = ""
                    End If

                Else
                    sClass = ""

                End If

            Catch ex As Exception

            End Try

            sHTML = sHTML & "<td " & sClass & "><a href='configbanners.aspx?Id=" & dtBanners.Rows(i)("Id") & "'>" & dtBanners.Rows(i)("Id") & "</a></td>"
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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("loginAdmin") = "" Then
                Response.Redirect("loginadmin.aspx")
            End If

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

            fnCargaBanners()

            If Request.QueryString.Count > 0 Then
                Session("IdBanner") = Request.QueryString("Id")
                ''Obtenemos los datos para rellenar
                ssql = "SELECT ciId as Id,cvDescripcion as Nombre,cvRutaImagen as Imagen,cvTexto as Texto,cvLenguaje as Lenguaje,cvItemCode as Articulo, cfPrecio as Precio,cfPrecioPromo as PrecioPromo,cfDescuento as Descuento,cvEspecifico as Especifico,ciOrden as Orden,cvEstatus as estatus FROM config.Banners WHERE ciId= " & "'" & Request.QueryString("Id") & "'"
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

        End If
    End Sub
    Protected Sub chkEspecifico_CheckedChanged(sender As Object, e As EventArgs) Handles chkEspecifico.CheckedChanged
        If chkEspecifico.Checked = True Then
            pnlEspecifico.Visible = True
            txtPrecioActual.Text = ""
            txtPrecioEspecial.Text = ""
            txtDescto.Text = ""
        Else
            pnlEspecifico.Visible = False
        End If
    End Sub

    Protected Sub rcbArticulos_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.DropDownListEventArgs) Handles rcbArticulos.SelectedIndexChanged
        Try
            Dim Precio As Double
            Precio = objDatos.fnPrecioActual(rcbArticulos.SelectedValue)
            txtPrecioActual.Text = Precio.ToString("###,###,###.#0")
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If FileUpload1.HasFile Then
            Dim fileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)
            FileUpload1.PostedFile.SaveAs(Server.MapPath("~/Images/") + fileName)
            imgBanner.ImageUrl = "Images/" + fileName
            Session("Imagen") = "Images/" + fileName
            imgBanner.Visible = True
        End If
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If imgBanner.ImageUrl = "" Then
            objDatos.Mensaje("Elija una imagen para el banner", Me.Page)
            Exit Sub
        End If
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre que identifique el banner", Me.Page)
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

        If btnAgregar.Text = "Actualizar" Then
            ''Eliminamos y dejamos el flujo natural de una inserción
            ssql = "DELETE FROM Config.Banners where ciId=" & "'" & Session("IdBanner") & "'"
            objDatos.fnEjecutarInsert(ssql)
        End If
        If iEspecifico = 0 Then
            ssql = "INSERT INTO config.Banners (cvDescripcion,cvRutaImagen,cvTexto,cvLenguaje,cvEspecifico,ciOrden,cvEstatus) VALUES(" _
                & "'" & txtNombre.Text & "'," _
                & "'" & Session("Imagen") & "'," _
                & "'" & txtTexto.Text & "'," _
                & "'" & rcbLenguaje.SelectedItem.Text & "'," _
                & "'" & sEspecifico & "'," _
                & "'" & txtOrden.Text & "','" & sEstatus & "')"
        Else
            If txtPrecioEspecial.Text = "" Then
                txtPrecioEspecial.Text = "0"
            End If
            If txtDescto.Text = "" Then
                txtDescto.Text = "0"
            End If
            ssql = "INSERT INTO config.Banners (cvDescripcion,cvRutaImagen,cvTexto,cvLenguaje,cvEspecifico,cvItemCode,cfPrecio,cfPrecioPromo,cfDescuento, ciOrden,cvEstatus) VALUES(" _
                & "'" & txtNombre.Text & "'," _
                & "'" & Session("Imagen") & "'," _
                & "'" & txtTexto.Text & "'," _
                & "'" & rcbLenguaje.SelectedItem.Text & "'," _
                & "'" & sEspecifico & "'," _
                & "'" & rcbArticulos.SelectedValue & "'," _
                & "'" & txtPrecioActual.Text & "'," _
                & "'" & txtPrecioEspecial.Text & "'," _
                & "'" & txtDescto.Text & "'," _
                & "'" & txtOrden.Text & "','" & sEstatus & "')"
        End If
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Banner Guardado!", Me.Page)
        txtNombre.Text = ""
        txtOrden.Text = ""
        txtTexto.Text = ""
        imgBanner.Visible = False
        Session("IdBanner") = ""
        fnCargaBanners()
    End Sub
    'Protected Sub rgvBanners_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgvBanners.ItemCommand
    '    'Try
    '    '    Dim Item As GridEditableItem
    '    '    Item = e.Item
    '    '    Session("IdBanner") = rgvBanners.Items(Item.ItemIndex)("Id").Text
    '    '    txtNombre.Text = rgvBanners.Items(Item.ItemIndex)("Nombre").Text
    '    '    txtTexto.Text = rgvBanners.Items(Item.ItemIndex)("Texto").Text.Replace("&nbsp;", "")
    '    '    imgBanner.ImageUrl = rgvBanners.Items(Item.ItemIndex)("Imagen").Text

    '    '    ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO' AND cvLenguaje=" & "'" & rgvBanners.Items(Item.ItemIndex)("Lenguaje").Text & "'"
    '    '    Dim dtLenguajes As New DataTable
    '    '    dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
    '    '    rcbLenguaje.SelectedValue = dtLenguajes.Rows(0)(0)
    '    '    txtOrden.Text = rgvBanners.Items(Item.ItemIndex)("Orden").Text
    '    '    If rgvBanners.Items(Item.ItemIndex)("Especifico").Text = "SI" Then
    '    '        chkEspecifico.Checked = True
    '    '        txtPrecioActual.Text = rgvBanners.Items(Item.ItemIndex)("Precio").Text
    '    '        txtPrecioEspecial.Text = rgvBanners.Items(Item.ItemIndex)("PrecioPromo").Text.Replace("&nbsp;", "")
    '    '        txtDescto.Text = rgvBanners.Items(Item.ItemIndex)("Descuento").Text.Replace("&nbsp;", "")
    '    '        pnlEspecifico.Visible = True
    '    '    Else
    '    '        chkEspecifico.Checked = False
    '    '        pnlEspecifico.Visible = False
    '    '    End If
    '    '    btnAgregar.Text = "Actualizar"
    '    'Catch ex As Exception
    '    'End Try
    'End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE FROM Config.Banners where ciId=" & "'" & Session("IdBanner") & "'"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Banner Eliminado!", Me.Page)
        txtNombre.Text = ""
        txtOrden.Text = ""
        txtTexto.Text = ""
        imgBanner.Visible = False
        Session("IdBanner") = ""
        fnCargaBanners()

    End Sub
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Response.Redirect("configbanners.aspx")
    End Sub
    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("menuconfig.aspx")
    End Sub
End Class
