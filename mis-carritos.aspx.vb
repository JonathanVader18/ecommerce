
Imports System.Data

Partial Class mis_carritos
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub mis_carritos_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnMenuPreferencias()

        If Not IsPostBack Then

            'ssql = "select (select TOP 1 cvItemCode as Articulo from Tienda.Pedido_Det where ciNoPedido =T0.ciNoPedido) as Artículo, ciNoPedido as Orden ,cdFecha as Fecha,cvEstatus as Estatus,cfTotal as Total  from tienda.Pedido_Hdr T0 WHERE " _
            '    & " cvUsuario ='" & Session("UserB2C") & "' AND cvTipoDoc='CARRITO'"
            Dim dtPedidos As New DataTable
            'dtPedidos = objDatos.fnEjecutarConsulta(ssql)
            'ddlCarritos.DataSource = dtPedidos
            'ddlCarritos.DataTextField = "Orden"
            'ddlCarritos.DataValueField = "Orden"
            'ddlCarritos.DataBind()

            ssql = "select (select TOP 1 cvItemName  from Tienda.Pedido_Det where ciNoPedido =T0.ciNoPedido) as Artículo, ciNoPedido as Orden ,cdFecha as Fecha,cvEstatus as Estatus,cfTotal as Total  from tienda.Pedido_Hdr T0 WHERE " _
                & " cvUsuario ='" & Session("UserB2C") & "' AND cvTipoDoc='CARRITO'"
            dtPedidos = New DataTable
            dtPedidos = objDatos.fnEjecutarConsulta(ssql)

            Dim sHtml As String = ""
            For i = 0 To dtPedidos.Rows.Count - 1 Step 1
                sHtml = sHtml & "<tr>"
                sHtml = sHtml & " <td data-title='Articulo'>" & dtPedidos.Rows(i)("Artículo") & "</td>"
                sHtml = sHtml & " <td data-title='Fecha' class='text-center'>" & CDate(dtPedidos.Rows(i)("Fecha")).ToShortDateString & "</td>"
                sHtml = sHtml & " <td data-title='Orden' class='text-center'>" & dtPedidos.Rows(i)("Orden") & "</td>"
                sHtml = sHtml & " <td data-title='Estatus' class='text-center'>" & dtPedidos.Rows(i)("Estatus") & "</td>"
                sHtml = sHtml & " <td data-title='Total' class='text-center'>" & CDbl(dtPedidos.Rows(i)("Total")).ToString("###,###,###.#0") & "</td>"
                sHtml = sHtml & " <td Class='esp'><a class='btn btn-general-1' href='detalle-carrito.aspx?Ped=" & dtPedidos.Rows(i)("Orden") & "'>DETALLES</a></td>"
                sHtml = sHtml & "</tr>"
            Next
            Dim literal As New LiteralControl(sHtml)
            pnlLista.Controls.Clear()
            pnlLista.Controls.Add(literal)


        End If
    End Sub

    Private Sub ddlCarritos_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCarritos.SelectedIndexChanged
        Try


            ssql = "select (select TOP 1 cvItemCode as Articulo from Tienda.Pedido_Det where ciNoPedido =T0.ciNoPedido) as Artículo, ciNoPedido as Orden ,cdFecha as Fecha,cvEstatus as Estatus,cfTotal as Total  from tienda.Pedido_Hdr T0 WHERE " _
                & " cvUsuario ='" & Session("UserB2C") & "' AND ciNoPedido=" & "'" & ddlCarritos.SelectedValue & "'"
            Dim dtPedidos As New DataTable
            dtPedidos = objDatos.fnEjecutarConsulta(ssql)

            Dim sHtml As String = ""
            For i = 0 To dtPedidos.Rows.Count - 1 Step 1
                sHtml = sHtml & "<td data-title='Articulo'>" & dtPedidos.Rows(i)("Artículo") & "</td>"
                sHtml = sHtml & "<td data-title='Fecha' class='text-center'>" & CDate(dtPedidos.Rows(i)("Fecha")).ToShortDateString & "</td>"
                sHtml = sHtml & "<td data-title='Orden' class='text-center'>" & dtPedidos.Rows(i)("Orden") & "</td>"
                sHtml = sHtml & "<td data-title='Estatus' class='text-center'>" & dtPedidos.Rows(i)("Estatus") & "</td>"
                sHtml = sHtml & "<td data-title='Total' class='text-center'>" & CDbl(dtPedidos.Rows(i)("Total")).ToString("###,###,###.#0") & "</td>"
                sHtml = sHtml & "<td Class='esp'><a class='btn btn-general-1' href='detalle-carrito.aspx?Ped=" & dtPedidos.Rows(i)("Orden") & "'>DETALLES</a></td>"
            Next
            Dim literal As New LiteralControl(sHtml)
            pnlLista.Controls.Clear()
            pnlLista.Controls.Add(literal)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub fnMenuPreferencias()
        Dim ssql As String
        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Preferencias' "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""

        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <li><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuPref.Controls.Clear()
        pnlMenuPref.Controls.Add(literal)

    End Sub
End Class
