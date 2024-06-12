Imports System.Data
Imports Telerik.Web.UI
Partial Class Atributos
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
            ssql = "SELECT ciIdTipo as Id,cvTipoAtributo as Nombre,cvLenguaje as Lenguaje,ciOrden as Orden FROM config.TipoAtributos WHERE cvEstatus  ='ACTIVO' and cvLenguaje=" & "'" & rcbLenguaje.SelectedItem.Text & "' order by ciOrden "
            Dim dtTipos As New DataTable
            dtTipos = objDatos.fnEjecutarConsulta(ssql)
            rcbTipo.DataSource = dtTipos
            rcbTipo.DataTextField = "Nombre"
            rcbTipo.DataValueField = "Id"
            rcbTipo.DataBind()



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
            rcbCampo.DataSource = dtCampos
            rcbCampo.DataTextField = "Columna"
            rcbCampo.DataValueField = "Columna"
            rcbCampo.DataBind()

            ''Los atributos
            ssql = "select T0.ciIdAtributo as Id,T1.cvTipoAtributo as Tipo, T0.cvDescripcion as Atributo,T0.cvCampoSAP as CampoSAP,T0.cvLenguaje as Lenguaje, T0.ciOrden as Orden from config.atributos T0 " _
                & " INNER JOIN config.TipoAtributos T1 ON T0.ciIdTipo=T1.ciIdTipo WHERE T0.cvEstatus='ACTIVO' Order by T1.CiOrden,T0.ciOrden "
            Dim dtAtributos As New DataTable
            dtAtributos = objDatos.fnEjecutarConsulta(ssql)
            rgvAtributos.DataSource = dtAtributos
            rgvAtributos.DataBind()

        End If
    End Sub
    Protected Sub rcbLenguaje_SelectedIndexChanged(sender As Object, e As DropDownListEventArgs) Handles rcbLenguaje.SelectedIndexChanged
        Try
            ''Los Tipos activos
            ssql = "SELECT ciIdTipo as Id,cvTipoAtributo as Nombre,cvLenguaje as Lenguaje,ciOrden as Orden FROM config.TipoAtributos WHERE cvEstatus  ='ACTIVO' and cvLenguaje=" & "'" & rcbLenguaje.SelectedItem.Text & "' order by ciOrden "
            Dim dtTipos As New DataTable
            dtTipos = objDatos.fnEjecutarConsulta(ssql)
            rcbTipo.DataSource = dtTipos
            rcbTipo.DataTextField = "Nombre"
            rcbTipo.DataValueField = "Id"
            rcbTipo.DataBind()

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre que identifique el Atributo", Me.Page)
            Exit Sub
        End If
        If txtOrden.Text = "" Then
            objDatos.Mensaje("Establezca el orden al Atributo", Me.Page)
            Exit Sub
        End If

        If btnAgregar.Text = "Actualizar" Then
            ssql = "DELETE FROM config.Atributos WHERE ciIdAtributo=" & "'" & Session("IdAtributo") & "'"
            objDatos.fnEjecutarInsert(ssql)
            btnAgregar.Text = "Agregar"
        End If
        ssql = "SELECT ISNULL(MAX(ciIdAtributo),0) + 1 fROM config.Atributos "
        Dim dtId As New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)

        ssql = "INSERT INTO config.Atributos (ciIdAtributo,ciIdTipo,cvDescripcion,cvCampoSAP,cvLenguaje,ciOrden,cvEstatus) VALUES(" _
              & "'" & dtId.Rows(0)(0) & "'," _
              & "'" & rcbTipo.SelectedValue & "'," _
              & "'" & txtNombre.Text & "'," _
              & "'" & rcbCampo.SelectedItem.Text & "'," _
              & "'" & rcbLenguaje.SelectedItem.Text & "'," _
              & "'" & txtOrden.Text & "','ACTIVO')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado", Me.Page)
        ''Los atributos
        ssql = "select T0.ciIdAtributo as Id,T1.cvTipoAtributo as Tipo, T0.cvDescripcion as Atributo,T0.cvCampoSAP as CampoSAP,T0.cvLenguaje as Lenguaje, T0.ciOrden as Orden from config.atributos T0 " _
            & " INNER JOIN config.TipoAtributos T1 ON T0.ciIdTipo=T1.ciIdTipo WHERE T0.cvEstatus='ACTIVO' Order by T1.CiOrden,T0.ciOrden "
        Dim dtAtributos As New DataTable
        dtAtributos = objDatos.fnEjecutarConsulta(ssql)
        rgvAtributos.DataSource = dtAtributos
        rgvAtributos.DataBind()
        txtNombre.Text = ""
        txtOrden.Text = ""

    End Sub
    Protected Sub rgvAtributos_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgvAtributos.ItemCommand
        Try
            Dim Item As GridEditableItem
            Item = e.Item
            Session("IdAtributo") = rgvAtributos.Items(Item.ItemIndex)("Id").Text
            txtNombre.Text = rgvAtributos.Items(Item.ItemIndex)("Atributo").Text
            txtOrden.Text = rgvAtributos.Items(Item.ItemIndex)("Orden").Text
            rcbCampo.SelectedValue = rgvAtributos.Items(Item.ItemIndex)("CampoSAP").Text
            btnAgregar.Text = "Actualizar"
            ''Obtenemos el Tipo
            ssql = "SELECT ciIdTipo FROM Config.TipoAtributos WHERE cvTipoAtributo=" & "'" & rgvAtributos.Items(Item.ItemIndex)("Tipo").Text & "'"
            Dim dtIdTipo As New DataTable
            dtIdTipo = objDatos.fnEjecutarConsulta(ssql)
            If dtIdTipo.Rows.Count > 0 Then
                rcbTipo.SelectedValue = dtIdTipo.Rows(0)(0)
            End If
            ssql = "SELECT ciIdLenguaje, cvLenguaje from SAP_Tienda.config.Lenguajes WHERE cvEstatus='ACTIVO' AND cvLenguaje=" & "'" & rgvAtributos.Items(Item.ItemIndex)("Lenguaje").Text & "'"
            Dim dtLenguajes As New DataTable
            dtLenguajes = objDatos.fnEjecutarConsulta(ssql)
            If dtLenguajes.Rows.Count > 0 Then
                rcbLenguaje.SelectedValue = dtLenguajes.Rows(0)(0)
            End If

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ssql = "DELETE FROM config.Atributos WHERE ciIdAtributo=" & "'" & Session("IdAtributo") & "'"
        objDatos.fnEjecutarInsert(ssql)
        btnAgregar.Text = "Agregar"
        objDatos.Mensaje("Eliminado", Me.Page)

        ''Los atributos
        ssql = "select T0.ciIdAtributo as Id,T1.cvTipoAtributo as Tipo, T0.cvDescripcion as Atributo,T0.cvCampoSAP as CampoSAP,T0.cvLenguaje as Lenguaje, T0.ciOrden as Orden from config.atributos T0 " _
            & " INNER JOIN config.TipoAtributos T1 ON T0.ciIdTipo=T1.ciIdTipo WHERE T0.cvEstatus='ACTIVO' Order by T1.CiOrden,T0.ciOrden "
        Dim dtAtributos As New DataTable
        dtAtributos = objDatos.fnEjecutarConsulta(ssql)
        rgvAtributos.DataSource = dtAtributos

        rgvAtributos.DataBind()

        txtNombre.Text = ""
        txtOrden.Text = ""
    End Sub
End Class
