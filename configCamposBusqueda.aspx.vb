Imports System.Data
Imports Telerik.Web.UI
Partial Class configCamposBusqueda
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

            ''Los criterios existentes
            ssql = "select ciIdCampo as Id,cvCampoSAP as Campo from config.CamposBusqueda WHERE cvEstatus ='ACTIVO'"
            Dim dtCriterios As New DataTable
            dtCriterios = objDatos.fnEjecutarConsulta(ssql)
            rgvCriterios.DataSource = dtCriterios
            rgvCriterios.DataBind()
        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        If rcbAtributo.SelectedValue = "-Seleccione-" Then
            objDatos.Mensaje("Elija un campo de la lista", Me.Page)
            Exit Sub
        End If
        ssql = "INSERT INTO Config.CamposBusqueda(cvCampoSAP,cvEstatus) VALUES(" _
            & "'" & rcbAtributo.SelectedValue & "','ACTIVO')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado", Me.Page)
        ''Los criterios existentes
        ssql = "select ciIdCampo as Id,cvCampoSAP as Campo from config.CamposBusqueda WHERE cvEstatus ='ACTIVO'"
        Dim dtCriterios As New DataTable
        dtCriterios = objDatos.fnEjecutarConsulta(ssql)
        rgvCriterios.DataSource = dtCriterios
        rgvCriterios.DataBind()
        rcbAtributo.SelectedValue = "-Seleccione-"
    End Sub
    Protected Sub rgvCriterios_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles rgvCriterios.ItemCommand
        Try
            Dim Item As GridEditableItem
            Item = e.Item
            Session("IdCriterio") = rgvCriterios.Items(Item.ItemIndex)("Id").Text
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnAgregar0_Click(sender As Object, e As EventArgs) Handles btnAgregar0.Click
        ssql = "DELETE FROM Config.CamposBusqueda WHERE ciIdCampo=" _
          & "'" & Session("IdCriterio") & "'"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Eliminado!", Me.Page)
        ''Los criterios existentes
        ssql = "select ciIdCampo as Id,cvCampoSAP as Campo from config.CamposBusqueda WHERE cvEstatus ='ACTIVO'"
        Dim dtCriterios As New DataTable
        dtCriterios = objDatos.fnEjecutarConsulta(ssql)
        rgvCriterios.DataSource = dtCriterios
        rgvCriterios.DataBind()
        rcbAtributo.SelectedValue = "-Seleccione-"
    End Sub
End Class
