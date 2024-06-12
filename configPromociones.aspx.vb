Imports System.Data
Imports System.IO
Partial Class configPromociones
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub configPromociones_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("loginAdmin") = "" Then
                Response.Redirect("loginadmin.aspx")

            End If
            Session("IdArticuloPromo") = ""



            ''Rellenamos los articulos para el combo
            ssql = objDatos.fnObtenerQuery("Productos-Combo")
            Dim dtArticulos As New DataTable
            dtArticulos = objDatos.fnEjecutarConsultaSAP(ssql)
            ddlArticulo.DataSource = dtArticulos
            ddlArticulo.DataTextField = "Descrip"
            ddlArticulo.DataValueField = "ItemCode"
            ddlArticulo.DataBind()

            ''El nombre

            ssql = "SELECT * from Config.categoriasEsp where cvManual='PROMO' "
            Dim dtNombre As New DataTable
            dtNombre = objDatos.fnEjecutarConsulta(ssql)
            If dtNombre.Rows.Count > 0 Then
                txtCategoria.Text = dtNombre.Rows(0)("cvDescripcion")
                Session("IdCatPromo") = dtNombre.Rows(0)("ciIdNivel")
            End If
            fnCargaArticulos()
            If Request.QueryString.Count > 0 Then
                Session("IdArticuloPromo") = Request.QueryString("Id")
                ''Obtenemos los datos para rellenar

                ddlArticulo.SelectedValue = Session("IdArticuloDesc")
            Else


            End If



        End If
    End Sub
    Public Sub fnCargaArticulos()
        ''Los banners activos
        ssql = "Select cvItemCode as Articulo, cvItemname as Descripcion from config.CategoriasEsp_Articulos where ciIdCategoria='" & Session("IdCatPromo") & "' Order by cvItemCode "
        Dim dtBanners As New DataTable
        dtBanners = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"
        For i = 0 To dtBanners.Rows.Count - 1 Step 1

            sHTML = sHTML & "<tr>"
            Try
                If Request.QueryString.Count > 0 Then
                    If Request.QueryString("Id") = dtBanners.Rows(i)("Articulo") Then
                        sClass = "class='bg-danger text-white'"
                    Else
                        sClass = ""
                    End If

                Else
                    sClass = ""

                End If

            Catch ex As Exception

            End Try

            sHTML = sHTML & "<td " & sClass & "><a href='configPromociones.aspx?Id=" & dtBanners.Rows(i)("Articulo") & "'>" & dtBanners.Rows(i)("Articulo") & "</a></td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtBanners.Rows(i)("Descripcion") & "</td>"


            sHTML = sHTML & "</tr>"
        Next
        Dim literal As New LiteralControl(sHTML)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click


        ssql = "INSERT INTO config.CategoriasEsp_Articulos  (cvCategoriaEsp,cvItemCode,cvItemName,ciIdCategoria ) VALUES(" _
             & "'" & txtCategoria.Text & "'," _
             & "'" & ddlArticulo.SelectedValue & "'," _
             & "'" & ddlArticulo.SelectedItem.Text & "'," _
             & "'" & Session("IdCatPromo") & "')"

        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Artículo Guardado!", Me.Page)

        fnCargaArticulos()


    End Sub

    Private Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE config.CategoriasEsp_Articulos WHERE " _
            & "cvItemCode='" & Session("IdArticuloPromo") & "' AND " _
            & "ciIdCategoria='" & Session("IdCatPromo") & "'"

        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Se ha quitado el artículo!", Me.Page)

        fnCargaArticulos()
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("menuconfig.aspx")
    End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        ssql = "UPDATE  Config.categoriasEsp SET cvDescripcion='" & txtCategoria.Text & "'  where ciIdNivel=" & "'" & Session("IdCatPromo") & "'"
        objDatos.fnEjecutarInsert(ssql)

        objDatos.Mensaje("Nombre actualizado!", Me.Page)

        fnCargaArticulos()
    End Sub
End Class
