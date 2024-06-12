
Imports System.Data

Partial Class adminclientes
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Private Sub adminclientes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            'Cargamos los catálogos




            ''Cuentas contables
            ssql = "  select AcctCode as Codigo,AcctName as descripcion from OACT where GroupMask=1 and postable ='y' and FatherNum ='103001000000'"
            Dim dtCuentas As New DataTable
            dtCuentas = objdatos.fnEjecutarConsultaSAP(ssql)
            ddlCuentacontable.DataSource = dtCuentas
            ddlCuentacontable.DataTextField = "Descripcion"
            ddlCuentacontable.DataValueField = "Codigo"
            ddlCuentacontable.DataBind()

            ''Grupo de socios de negocio
            ssql = "  select GroupCode as Codigo, Groupname as Descripcion from OCRG where GroupType ='C'"
            Dim dtGrupos As New DataTable
            dtGrupos = objdatos.fnEjecutarConsultaSAP(ssql)
            ddlGrupoClientes.DataSource = dtGrupos
            ddlGrupoClientes.DataTextField = "Descripcion"
            ddlGrupoClientes.DataValueField = "Codigo"
            ddlGrupoClientes.DataBind()

            ''Lista de precios
            ssql = "  select ListNum as Codigo, ListName as Descripcion from OPLN "
            Dim dtListas As New DataTable
            dtListas = objdatos.fnEjecutarConsultaSAP(ssql)
            ddlListaPrecios.DataSource = dtListas
            ddlListaPrecios.DataTextField = "Descripcion"
            ddlListaPrecios.DataValueField = "Codigo"
            ddlListaPrecios.DataBind()

            ''Cargamos los valores actuales

            ssql = " SELECT ISNULL(cvUsaSerie ,'') as UsaSerie,ISNULL(ciSerie,0) as Serie,ISNULL(cvGrupo,'') as Grupo,ISNULL(ciListaPrecios ,0) as Lista,ISNULL(cvCuentaContable ,'') as CuentaContable from config.ParametrizacionesCliente "
            Dim dtParam As New DataTable
            dtParam = objdatos.fnEjecutarConsulta(ssql)
            If dtParam.Rows.Count > 0 Then

                ddlCuentacontable.SelectedValue = dtParam.Rows(0)("CuentaContable")
                ddlGrupoClientes.SelectedValue = dtParam.Rows(0)("Grupo")
                ddlListaPrecios.SelectedValue = dtParam.Rows(0)("Lista")
            End If

        End If
    End Sub

    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click

        ssql = "UPDATE config.ParametrizacionesCliente  SET " _
            & "cvGrupo = " & "'" & ddlGrupoClientes.SelectedValue & "'," _
            & "ciListaPrecios = " & "'" & ddlListaPrecios.SelectedValue & "'," _
            & "cvCuentaContable = " & "'" & ddlCuentacontable.SelectedValue & "'"

        objdatos.fnEjecutarInsert(ssql)
        objdatos.Mensaje("Actualizado correctamente", Me.Page)
    End Sub
End Class
