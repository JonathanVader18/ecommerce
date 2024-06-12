
Imports System.Data
Imports Telerik.Web.UI
Partial Class configCategorias
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ''Los campos de OITM
            ssql = "SELECT TOP 1 * FROM OITM "
            Dim dtColumnas As New DataTable
            Dim dtCampos As New DataTable
            Dim fila As DataRow
            dtCampos.Columns.Add("Columna")
            dtColumnas = objDatos.fnEjecutarConsultaSAP(ssql)

            For i = 0 To dtColumnas.Columns.Count - 1 Step 1
                fila = dtCampos.NewRow
                fila("Columna") = dtColumnas.Columns(i).ColumnName
                dtCampos.Rows.Add(fila)
            Next
            fila = dtCampos.NewRow
            fila("Columna") = "-Seleccione-"
            dtCampos.Rows.Add(fila)
            rcbAtributo.DataSource = dtCampos
            rcbAtributo.DataTextField = "Columna"
            rcbAtributo.DataValueField = "Columna"
            rcbAtributo.DataBind()
            rcbAtributo.SelectedValue = "-Seleccione-"


            ''Cargamos los lenguajes
            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            fila = dtLenguajes.NewRow
            fila("ciIdLenguaje") = "-1"
            fila("cvLenguaje") = "-Seleccione-"
            dtLenguajes.Rows.Add(fila)

            rcbLenguaje.DataSource = dtLenguajes
            rcbLenguaje.DataTextField = "cvLenguaje"
            rcbLenguaje.DataValueField = "ciIdLenguaje"
            rcbLenguaje.DataBind()
            rcbLenguaje.SelectedValue = "-1"


            ''Los criterios existentes
            ssql = "select ciIdNivel as Id,cvDescripcion as Descripcion,cvCampoSAP as CampoSAP,cvLenguaje as Lenguaje,ciOrden as Orden from config.NivelesArticulos WHERE cvEstatus ='ACTIVO' ORDER BY ciOrden"
            Dim dtCriterios As New DataTable
            dtCriterios = objDatos.fnEjecutarConsulta(ssql)
            rgvCategorias.DataSource = dtCriterios
            rgvCategorias.DataBind()
        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtDescripcion.Text = "" Then
            objDatos.Mensaje("Debe especificar una descripción de la categoría", Me.Page)
            Exit Sub
        End If
        If rcbLenguaje.SelectedValue = "-1" Then
            objDatos.Mensaje("Debe especificar el lenguaje", Me.Page)
            Exit Sub
        End If
        If rcbAtributo.SelectedValue = "-1" Then
            objDatos.Mensaje("Debe especificar el campo de SAP al que se asociará la categoría", Me.Page)
            Exit Sub

        End If
        If btnAgregar.Text = "Actualizar" Then
            ssql = "DELETE FROM Config.NivelesArticulos WHERE ciIdNivel=" _
                  & "'" & Session("IdCategoria") & "'"
            objDatos.fnEjecutarInsert(ssql)
        End If
        ssql = "select ISNULL(MAX(ciIdNivel),0) + 1 from config.NivelesArticulos "
        Dim dtId As New DataTable
        Dim IdCategoria As Int32 = 0
        dtId = objDatos.fnEjecutarConsulta(ssql)
        If dtId.Rows.Count > 0 Then
            IdCategoria = dtId.Rows(0)(0)
        End If
        ssql = "INSERT INTO config.NivelesArticulos(ciIdNivel,cvDescripcion,cvCampoSAP,cvLenguaje,ciOrden,cvEstatus) VALUES (" _
            & "'" & IdCategoria & "'," _
            & "'" & txtDescripcion.Text & "'," _
            & "'" & rcbAtributo.SelectedValue & "'," _
            & "'" & rcbLenguaje.SelectedItem.Text & "'," _
            & "'" & rcbOrden.SelectedValue & "','ACTIVO') "
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado!", Me.Page)
        ''Los criterios existentes
        ssql = "select ciIdNivel as Id,cvDescripcion as Descripcion,cvCampoSAP as CampoSAP,cvLenguaje as Lenguaje,ciOrden as Orden from config.NivelesArticulos WHERE cvEstatus ='ACTIVO' AND cvLenguaje=" & "'" & rcbLenguaje.SelectedItem.Text & "' ORDER BY ciOrden"
        Dim dtCriterios As New DataTable
        dtCriterios = objDatos.fnEjecutarConsulta(ssql)
        rgvCategorias.DataSource = dtCriterios
        rgvCategorias.DataBind()
        btnAgregar.Text = "Agregar Nivel"

        txtDescripcion.Text = ""
        rcbAtributo.SelectedValue = "-1"
    End Sub
    Protected Sub rcbLenguaje_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles rcbLenguaje.SelectedIndexChanged
        Try
            ''Los criterios existentes
            ssql = "select ciIdNivel as Id,cvDescripcion as Descripcion,cvCampoSAP as CampoSAP,cvLenguaje as Lenguaje,ciOrden as Orden from config.NivelesArticulos WHERE cvEstatus ='ACTIVO' AND cvLenguaje=" & "'" & rcbLenguaje.SelectedItem.Text & "' ORDER BY ciOrden"
            Dim dtCriterios As New DataTable
            dtCriterios = objDatos.fnEjecutarConsulta(ssql)
            rgvCategorias.DataSource = dtCriterios
            rgvCategorias.DataBind()
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub rgvCategorias_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgvCategorias.ItemCommand

        Try
                Dim Item As GridEditableItem
                Item = e.Item
            Session("IdCategoria") = rgvCategorias.Items(Item.ItemIndex)("Id").Text

            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO' AND cvLenguaje=" & "'" & rgvCategorias.Items(Item.ItemIndex)("Lenguaje").Text & "'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            If dtLenguajes.Rows.Count > 0 Then
                rcbLenguaje.SelectedValue = dtLenguajes.Rows(0)(0)
            End If
            rcbOrden.SelectedValue = rgvCategorias.Items(Item.ItemIndex)("Orden").Text
            txtDescripcion.Text = rgvCategorias.Items(Item.ItemIndex)("Descripcion").Text
            rcbAtributo.SelectedValue = rgvCategorias.Items(Item.ItemIndex)("CampoSAP").Text
            btnAgregar.Text = "Actualizar"
        Catch ex As Exception

            End Try

    End Sub
    Protected Sub btnAgregar0_Click(sender As Object, e As EventArgs) Handles btnAgregar0.Click
        Try
            ssql = "DELETE FROM Config.NivelesArticulos WHERE ciIdNivel=" _
                    & "'" & Session("IdCategoria") & "'"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.Mensaje("Eliminado!", Me.Page)
            ''Los criterios existentes
            ssql = "select ciIdNivel as Id,cvDescripcion as Descripcion,cvCampoSAP as CampoSAP,cvLenguaje as Lenguaje,ciOrden as Orden from config.NivelesArticulos WHERE cvEstatus ='ACTIVO' AND cvLenguaje=" & "'" & rcbLenguaje.SelectedItem.Text & "' ORDER BY ciOrden"
            Dim dtCriterios As New DataTable
            dtCriterios = objDatos.fnEjecutarConsulta(ssql)
            rgvCategorias.DataSource = dtCriterios
            rgvCategorias.DataBind()
            btnAgregar.Text = "Agregar Nivel"
        Catch ex As Exception

        End Try

    End Sub
End Class
