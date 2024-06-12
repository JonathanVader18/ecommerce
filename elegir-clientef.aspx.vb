
Imports System.Data

Partial Class elegir_clientef
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub elegir_clientef_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then



            Session("sesBuscar") = ""
            'Thread.Sleep(5000)
            ''Llenamos los clientes
            lblNombreUsuario.Text = Session("NombreuserTienda")

            btnSeleccionar.Enabled = False
            ''En base a la configuración, determinamos si puede registrar nuevos clientes
            ssql = "select ISNULL(cvAgregaClientes,'NO') as TipoCliente from config.Parametrizaciones "
            Dim dtTipo As New DataTable
            dtTipo = objDatos.fnEjecutarConsulta(ssql)
            If dtTipo.Rows.Count > 0 Then
                If dtTipo.Rows(0)(0) = "SI" Then
                    btnCrear.Visible = True
                Else
                    btnCrear.Visible = False
                End If
            End If

            pnlClientes.Visible = True
        End If
    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        Session("Cliente") = ddlClientes.SelectedValue

        If Session("Cliente") = "" Or Session("Cliente") = "-1" Then
            objDatos.Mensaje("Seleccione un cliente de la lista", Me.Page)
            Exit Sub
        End If
        Session("RazonSocial") = ddlClientes.SelectedItem.Text
        ''en base al cliente, obtenemos cual es su lista de precios
        ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
        ssql = ssql.Replace("[%0]", ddlClientes.SelectedValue)
        Dim dtLista As New DataTable
        dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtLista.Rows.Count > 0 Then
            Session("ListaPrecios") = dtLista.Rows(0)(0)
        Else
            Session("ListaPrecios") = "1"
        End If
        Session("Pedido") = New Cls_Pedido
        Try

        Catch ex As Exception

        End Try
        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
        Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
        Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

        ''Obtenemos el nombre de la empresa
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)

        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("TAQ") Then
                Response.Redirect("captura-pedido.aspx")
            Else
                Response.Redirect("catalogo.aspx")
            End If
        Else
            Response.Redirect("catalogo.aspx")
        End If
    End Sub

    Private Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Session("RegistraNuevo") = "SI"
        ssql = "SELECT ISNULL(cvLigaAgregarCliente,'') FROM config.Parametrizaciones"
        Dim dtLiga As New DataTable
        dtLiga = objDatos.fnEjecutarConsulta(ssql)
        If dtLiga.Rows.Count > 0 Then
            If dtLiga.Rows(0)(0) <> "" Then
                Response.Redirect(dtLiga.Rows(0)(0))
            Else
                Response.Redirect("agregar-cliente.aspx")
            End If

        Else
            Response.Redirect("agregar-cliente.aspx")
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        ssql = objDatos.fnObtenerQuery("Clientes")
        ssql = ssql.Replace("[%0]", Session("slpCode"))
        Dim sCondicion As String = ""

        If txtNIT.Text <> "" Then
            sCondicion = sCondicion & " AND U_NIT like '%" & txtNIT.Text & "%'"
        End If

        If txtNombre.Text <> "" Then
            sCondicion = sCondicion & " AND CardName like '%" & txtNombre.Text & "%'"
        End If
        ssql = ssql & sCondicion
        Dim dtclientes As New DataTable
        '  lblNombreUsuario.Text = ssql
        dtclientes = objDatos.fnEjecutarConsultaSAP(ssql)

        Dim fila As DataRow
        fila = dtclientes.NewRow
        fila("CardCode") = "-1"
        fila("CardName") = ""
        dtclientes.Rows.Add(fila)

        ddlClientes.DataSource = dtclientes
        ddlClientes.DataTextField = "CardName"
        ddlClientes.DataValueField = "CardCode"
        ddlClientes.DataBind()
        ddlClientes.SelectedValue = "-1"
        If dtclientes.Rows.Count = 0 Then
            objDatos.Mensaje("No se encontraron resultados", Me.Page)
            btnSeleccionar.Enabled = False
        Else
            btnSeleccionar.Enabled = True
        End If
    End Sub

    Private Sub ddlClientes_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlClientes.SelectedIndexChanged
        Try
            btnSeleccionar.Enabled = True
        Catch ex As Exception

        End Try
    End Sub
End Class
