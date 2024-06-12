
Imports System.Data
Imports Telerik.Web.UI

Partial Class TipoAtributos
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            ''Cargamos los lenguajes
            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            rcbLenguaje.DataSource = dtLenguajes
            rcbLenguaje.DataTextField = "cvLenguaje"
            rcbLenguaje.DataValueField = "ciIdLenguaje"
            rcbLenguaje.DataBind()

            ''Los Tipos activos
            ssql = "SELECT ciIdTipo as Id,cvTipoAtributo as Nombre,cvLenguaje as Lenguaje,ciOrden as Orden FROM config.TipoAtributos WHERE cvEstatus  ='ACTIVO' order by ciOrden "
            Dim dtTipos As New DataTable
            dtTipos = objDatos.fnEjecutarConsulta(ssql)
            rgvTipos.DataSource = dtTipos
            rgvTipos.DataBind()

        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre que identifique el Tipo de Atributo", Me.Page)
            Exit Sub
        End If
        If txtOrden.Text = "" Then
            objDatos.Mensaje("Establezca el orden del Tipo de Atributo", Me.Page)
            Exit Sub
        End If
        If btnAgregar.Text = "Actualizar" Then
            ssql = "DELETE FROM config.TipoAtributos WHERE ciIdTipo=" & "'" & Session("IdTipo") & "'"
            objDatos.fnEjecutarInsert(ssql)
            btnAgregar.Text = "Agregar"
        End If
        ssql = "SELECT ISNULL(MAX(ciIdTipo),0) + 1 fROM config.TipoAtributos "
        Dim dtId As New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)

        ssql = "INSERT INTO config.TipoAtributos (ciIdTipo,cvTipoAtributo,cvLenguaje,ciOrden,cvEstatus) VALUES(" _
              & "'" & dtId.Rows(0)(0) & "'," _
              & "'" & txtNombre.Text & "'," _
              & "'" & rcbLenguaje.SelectedItem.Text & "'," _
              & "'" & txtOrden.Text & "','ACTIVO')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado", Me.Page)

        ''Los Tipos activos
        ssql = "SELECT ciIdTipo as Id,cvTipoAtributo as Nombre,cvLenguaje as Lenguaje,ciOrden as Orden FROM config.TipoAtributos WHERE cvEstatus  ='ACTIVO' order by ciOrden "
        Dim dtTipos As New DataTable
        dtTipos = objDatos.fnEjecutarConsulta(ssql)
        rgvTipos.DataSource = dtTipos
        rgvTipos.DataBind()
        txtNombre.Text = ""
    End Sub
    Protected Sub rgvTipos_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles rgvTipos.ItemCommand
        Try
            Dim Item As GridEditableItem
            Item = e.Item
            Session("IdTipo") = rgvTipos.Items(Item.ItemIndex)("Id").Text
            txtNombre.Text = rgvTipos.Items(Item.ItemIndex)("Nombre").Text
            txtOrden.Text = rgvTipos.Items(Item.ItemIndex)("Orden").Text
            btnAgregar.Text = "Actualizar"
            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO' AND cvLenguaje=" & "'" & rgvTipos.Items(Item.ItemIndex)("Lenguaje").Text & "'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            rcbLenguaje.SelectedValue = dtLenguajes.Rows(0)(0)
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE FROM config.TipoAtributos WHERE ciIdTipo=" & "'" & Session("IdTipo") & "'"
        objDatos.fnEjecutarInsert(ssql)
        btnAgregar.Text = "Agregar"
        objDatos.Mensaje("Eliminado", Me.Page)
        ''Los Tipos activos
        ssql = "SELECT ciIdTipo as Id,cvTipoAtributo as Nombre,cvLenguaje as Lenguaje,ciOrden as Orden FROM config.TipoAtributos WHERE cvEstatus  ='ACTIVO' order by ciOrden "
        Dim dtTipos As New DataTable
        dtTipos = objDatos.fnEjecutarConsulta(ssql)
        rgvTipos.DataSource = dtTipos
        rgvTipos.DataBind()
        txtNombre.Text = ""
    End Sub
End Class
