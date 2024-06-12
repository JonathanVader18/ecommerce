Imports System.Data
Imports Telerik.Web.UI
Partial Class configBarras
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("IdDetalleBarra") = 0
            ''Las barras
            ssql = "select ciIdBarra,cvDescripcion from config.barras WHERE cvEstatus='ACTIVO' "
            Dim dtBarras As New DataTable
            dtBarras = objDatos.fnEjecutarConsulta(ssql)
            rcbBarra.DataSource = dtBarras
            rcbBarra.DataTextField = "cvDescripcion"
            rcbBarra.DataValueField = "ciIdBarra"
            rcbBarra.DataBind()

            ssql = "select ciIdDetalle as Id,cvItemCode as CodProducto,cvItemName as Descripcion from config.barras_detalle WHERE ciIdBarra =" & "'" & rcbBarra.SelectedValue & "'"
            Dim dtProductos As New DataTable
            dtProductos = objDatos.fnEjecutarConsulta(ssql)
            rgvBarra.DataSource = dtProductos
            rgvBarra.DataBind()


            ''Los productos
            ssql = "SELECT  itemCode,ItemName FROM OITM WHERE validfor='Y' AND sellItem='Y' "
            Dim dtProd As New DataTable
            dtProd = objDatos.fnEjecutarConsultaSAP(ssql)
            rcbProducto.DataSource = dtProd
            rcbProducto.DataTextField = "ItemName"
            rcbProducto.DataValueField = "ItemCode"
            rcbProducto.DataBind()

            ''Combo plantillas
            ssql = "Select ciIdPlantilla,cvDescripcion FROM config.Plantillas WHERE cvEstatus='ACTIVO'"
            Dim dtPlantillas As New DataTable
            dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
            rcbPlantilla.DataSource = dtPlantillas
            rcbPlantilla.DataTextField = "cvDescripcion"
            rcbPlantilla.DataValueField = "ciIdPlantilla"
            rcbPlantilla.DataBind()
        End If
    End Sub
    Protected Sub btnAgregarP_Click(sender As Object, e As EventArgs) Handles btnAgregarP.Click
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre que identifique a la barra", Me.Page)
            Exit Sub
        End If
        If btnAgregarP.Text = "Actualizar Barra" Then
            ssql = "UPDATE config.barras SET " _
                          & "ciIdPlantilla='" & rcbPlantilla.SelectedValue & "'," _
                          & "cvDescripcion='" & txtNombre.Text & "'" _
                          & " WHERE ciIdBarra = '" & rcbBarra.SelectedValue & "' "
            objDatos.fnEjecutarInsert(ssql)
        Else

            ssql = "SELECT ISNULL(MAX(ciIdBarra),0) + 1 fROM config.barras "
            Dim dtId As New DataTable
            dtId = objDatos.fnEjecutarConsulta(ssql)

            ssql = "INSERT INTO config.barras (ciIdBarra,cvDescripcion,ciIdPlantilla,cvEstatus) VALUES(" _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & txtNombre.Text & "'," _
                & "'" & rcbPlantilla.SelectedValue & "','ACTIVO') "
            objDatos.fnEjecutarInsert(ssql)
        End If

        objDatos.Mensaje("Registrado", Me.Page)

        ''Las barras
        ssql = "select ciIdBarra,cvDescripcion from config.barras WHERE cvEstatus='ACTIVO' "
        Dim dtBarras As New DataTable
        dtBarras = objDatos.fnEjecutarConsulta(ssql)
        rcbBarra.DataSource = dtBarras
        rcbBarra.DataTextField = "cvDescripcion"
        rcbBarra.DataValueField = "ciIdBarra"
        rcbBarra.DataBind()


    End Sub
    Protected Sub rcbBarra_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles rcbBarra.SelectedIndexChanged
        Try
            ssql = "select ciIdDetalle as Id,cvItemCode as CodProducto,cvItemName as Descripcion from config.barras_detalle WHERE ciIdBarra =" & "'" & rcbBarra.SelectedValue & "'"
            Dim dtProductos As New DataTable
            dtProductos = objDatos.fnEjecutarConsulta(ssql)
            rgvBarra.DataSource = dtProductos
            rgvBarra.DataBind()
            txtNombre.Text = rcbBarra.SelectedItem.Text
            btnAgregarP.Text = "Actualizar Barra"

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAgregarP0_Click(sender As Object, e As EventArgs) Handles btnAgregarP0.Click
        btnAgregarP.Text = "Agregar Barra"
        txtNombre.Text = ""
    End Sub
    Protected Sub btnEliminar0_Click(sender As Object, e As EventArgs) Handles btnEliminar0.Click
        ssql = "DELETE FROM config.barras_detalle WHERE ciIdBarra=" & "'" & rcbBarra.SelectedValue & "'"
        objDatos.fnEjecutarInsert(ssql)

        ssql = "DELETE FROM config.barras WHERE ciIdBarra=" & "'" & rcbBarra.SelectedValue & "'"
        objDatos.fnEjecutarInsert(ssql)

        objDatos.Mensaje("Eliminado", Me.Page)

        ''Las barras
        ssql = "select ciIdBarra,cvDescripcion from config.barras WHERE cvEstatus='ACTIVO' "
        Dim dtBarras As New DataTable
        dtBarras = objDatos.fnEjecutarConsulta(ssql)
        rcbBarra.DataSource = dtBarras
        rcbBarra.DataTextField = "cvDescripcion"
        rcbBarra.DataValueField = "ciIdBarra"
        rcbBarra.DataBind()
        ssql = "select ciIdDetalle as Id,cvItemCode as CodProducto,cvItemName as Descripcion from config.barras_detalle WHERE ciIdBarra =" & "'" & rcbBarra.SelectedValue & "'"
        Dim dtProductos As New DataTable
        dtProductos = objDatos.fnEjecutarConsulta(ssql)
        rgvBarra.DataSource = dtProductos
        rgvBarra.DataBind()

        btnAgregarP.Text = "Agregar Barra"
        txtNombre.Text = ""


    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        ssql = "SELECT ISNULL(MAX(ciIdDetalle),0) + 1 fROM config.barras_detalle "
        Dim dtId As New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)
        ssql = "INSERT INTO config.barras_detalle (ciIdDetalle,ciIdBarra,cvItemCode,cvItemName) VALUES(" _
            & "'" & dtId.Rows(0)(0) & "', " _
            & "'" & rcbBarra.SelectedValue & "'," _
            & "'" & rcbProducto.SelectedValue & "'," _
            & "'" & rcbProducto.SelectedItem.Text & "')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado", Me.Page)
        ssql = "select ciIdDetalle as Id,cvItemCode as CodProducto,cvItemName as Descripcion from config.barras_detalle WHERE ciIdBarra =" & "'" & rcbBarra.SelectedValue & "'"
        Dim dtProductos As New DataTable
        dtProductos = objDatos.fnEjecutarConsulta(ssql)
        rgvBarra.DataSource = dtProductos
        rgvBarra.DataBind()


    End Sub
    Protected Sub rgvBarra_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgvBarra.ItemCommand
        Try
            Dim Item As GridEditableItem
            Item = e.Item
            Session("IdDetalleBarra") = rgvBarra.Items(Item.ItemIndex)("Id").Text
            rcbProducto.SelectedValue = rgvBarra.Items(Item.ItemIndex)("CodProducto").Text
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE FROM config.barras_detalle WHERE ciIdDetalle=" & "'" & Session("IdDetalleBarra") & "'"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Eliminado", Me.Page)
        ssql = "select ciIdDetalle as Id,cvItemCode as CodProducto,cvItemName as Descripcion from config.barras_detalle WHERE ciIdBarra =" & "'" & rcbBarra.SelectedValue & "'"
        Dim dtProductos As New DataTable
        dtProductos = objDatos.fnEjecutarConsulta(ssql)
        rgvBarra.DataSource = dtProductos
        rgvBarra.DataBind()
    End Sub
End Class
