Imports System.Collections.Generic
Imports System.Data
Imports System.Threading

Partial Class elegir_cliente
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub elegir_cliente_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then


            ''Obtenemos el nombre de la empresa
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)

            If dtcliente.Rows.Count > 0 Then
                If CStr(dtcliente.Rows(0)(0)).Contains("FERCO") Or CStr(dtcliente.Rows(0)(0)).Contains("INTERGRES") Then
                    Response.Redirect("elegir-clientef.aspx")

                End If

            End If

            Session("sesBuscar") = ""
            'Thread.Sleep(5000)
            ''Llenamos los clientes
            lblNombreUsuario.Text = Session("NombreuserTienda")



            If Session("Todosclientes") = "SI" Then
                ssql = objDatos.fnObtenerQuery("Clientes")
                ssql = ssql.Replace(" AND slpCode='[%0]'", "")
            Else
                ssql = objDatos.fnObtenerQuery("Clientes")
                ssql = ssql.Replace("[%0]", Session("slpCode"))

            End If
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
    'Protected Overrides Sub Render(ByVal writer As HtmlTextWriter)
    '    If Not IsPostBack Then
    '        Response.Clear()
    '        Response.ClearContent()
    '    End If

    '    MyBase.Render(writer)
    'End Sub
    Protected Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
        Session("Cliente") = ddlClientes.SelectedValue
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

        ssql = objDatos.fnObtenerQuery("Generalescliente")
        ssql = ssql.Replace("[%0]", ddlClientes.SelectedValue)
        Dim dtGenerales As New DataTable
        dtGenerales = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtGenerales.Rows.Count > 0 Then
            Session("Generalescliente") = dtGenerales.Rows(0)(0) & " - " & dtGenerales.Rows(0)(1)
        End If


        If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then

            'Dim ckRazonSocial As HttpCookie = New HttpCookie("RazonSocial", Session("RazonSocial"))
            'Response.Cookies.Add(ckRazonSocial)

            'Dim ckCliente As HttpCookie = New HttpCookie("Cliente", Session("Cliente"))
            'Response.Cookies.Add(ckCliente)

            'Dim ckslpCode As HttpCookie = New HttpCookie("slpcode", Session("slpCode"))
            'Response.Cookies.Add(ckslpCode)

            'Dim ckListaPrecios As HttpCookie = New HttpCookie("ListaPrecios", Session("ListaPrecios"))
            'Response.Cookies.Add(ckListaPrecios)

            'Dim ckCarrito As HttpCookie = New HttpCookie("Carrito", "")
            'Response.Cookies.Add(ckCarrito)

            'Dim ckNombreUserTienda As HttpCookie = New HttpCookie("NombreuserTienda", Session("NombreuserTienda"))
            'Response.Cookies.Add(ckNombreUserTienda)




            'Response.Redirect("captura-pedidozeyco.aspx")
        End If

        ''Obtenemos el nombre de la empresa
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objDatos.fnEjecutarConsulta(ssql)

        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("TAQ") Then
                Response.Redirect("captura-pedido.aspx")
            Else
                If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("BOSS") Then
                    Response.Redirect("index.aspx")
                Else
                    Response.Redirect("catalogo.aspx")
                End If

            End If
        Else
            Response.Redirect("catalogo.aspx")
        End If


    End Sub
    Protected Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Session("RegistraNuevo") = "SI"
        ssql = "SELECT ISNULL(cvLigaAgregarCliente,'') FROM config.Parametrizaciones"
        Dim dtLiga As New DataTable
        dtLiga = objDatos.fnEjecutarConsulta(ssql)
        If dtLiga.Rows.Count > 0 Then
            If dtLiga.Rows(0)(0) <> "" Then
                If objDatos.fnObtenerCliente.ToUpper.Contains("ZEYCO") Then
                    Response.Redirect(dtLiga.Rows(0)(0) & "?origen=" & Session("UserTienda"))
                Else
                    Response.Redirect(dtLiga.Rows(0)(0))
                End If

            Else
                Response.Redirect("agregar-cliente.aspx")
            End If

        Else
            Response.Redirect("agregar-cliente.aspx")
        End If

    End Sub


End Class
