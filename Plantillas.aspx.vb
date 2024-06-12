Imports System.Data
Imports System.Collections.Generic
Partial Class Plantillas
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub Plantillas_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' "
            Dim dtDocumentos As New DataTable
            dtDocumentos = objDatos.fnEjecutarConsulta(ssql)
            rgvDocumentos.DataSource = dtDocumentos
            rgvDocumentos.DataBind()
        End If
    End Sub
    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Dim iNoPedido As Int64 = 0

        For Each row As Telerik.Web.UI.GridDataItem In rgvDocumentos.Items
            Dim cb As CheckBox = row.FindControl("chkSelect")
            If cb.Checked = True Then
                iNoPedido = rgvDocumentos.MasterTableView.Items(row.ItemIndex).Item("No").Text
                Exit For

            End If
        Next
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ssql = "select cvItemCode ,cvItemName,cfCantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & iNoPedido & "'"
            Dim dtLineas As New DataTable
            dtLineas = objDatos.fnEjecutarConsulta(ssql)
            rgvPartidas.DataSource = dtLineas
            rgvPartidas.DataBind()
            rgvPartidas.Visible = True
        End If
    End Sub
    Protected Sub btnUsar_Click(sender As Object, e As EventArgs) Handles btnUsar.Click
        Dim iNoPedido As Int64 = 0

        For Each row As Telerik.Web.UI.GridDataItem In rgvDocumentos.Items
            Dim cb As CheckBox = row.FindControl("chkSelect")
            If cb.Checked = True Then
                iNoPedido = rgvDocumentos.MasterTableView.Items(row.ItemIndex).Item("No").Text
                Exit For

            End If
        Next
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ssql = "select cvItemCode ,ISNULL(cvItemName,'') cvItemName ,cfCantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & iNoPedido & "'"
            Dim dtLineas As New DataTable
            dtLineas = objDatos.fnEjecutarConsulta(ssql)
            Session("Partidas") = New List(Of Cls_Pedido.Partidas)
            For i = 0 To dtLineas.Rows.Count - 1 Step 1
                Dim partida As New Cls_Pedido.Partidas
                partida.ItemCode = dtLineas.Rows(i)("cvItemCode")
                partida.ItemName = dtLineas.Rows(i)("cvItemName")
                partida.Cantidad = dtLineas.Rows(i)("cfCantidad")
                Dim dPrecioActual As Double = 0
                If CInt(Session("slpCode")) <> 0 Then

                    dPrecioActual = objDatos.fnPrecioActual(dtLineas.Rows(i)("cvItemCode"), Session("ListaPrecios"))
                Else
                    dPrecioActual = objDatos.fnPrecioActual(dtLineas.Rows(i)("cvItemCode"))
                End If

                partida.Precio = dPrecioActual
                partida.TotalLinea = partida.Cantidad * partida.Precio
                Session("Partidas").add(partida)
            Next
            objDatos.Mensaje("Plantilla Cargada", Me.Page)
        End If
    End Sub
    Protected Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Dim iNoPedido As Int64 = 0

        For Each row As Telerik.Web.UI.GridDataItem In rgvDocumentos.Items
            Dim cb As CheckBox = row.FindControl("chkSelect")
            If cb.Checked = True Then
                iNoPedido = rgvDocumentos.MasterTableView.Items(row.ItemIndex).Item("No").Text
                Exit For

            End If
        Next
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ssql = "delete  tienda.plantilla_det WHERE ciIdPlantilla= " & "'" & iNoPedido & "'"
            objDatos.fnEjecutarInsert(ssql)

            ssql = "delete  tienda.plantilla_Hdr WHERE ciIdPlantilla= " & "'" & iNoPedido & "'"
            objDatos.fnEjecutarInsert(ssql)

            ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' "
            Dim dtDocumentos As New DataTable
            dtDocumentos = objDatos.fnEjecutarConsulta(ssql)
            rgvDocumentos.DataSource = dtDocumentos
            rgvDocumentos.DataBind()

        End If
    End Sub
End Class
