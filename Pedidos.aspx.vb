
Imports System.Data

Partial Class Pedidos
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub Pedidos_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnLlenarcolumnas()
        fnLlenarListado()
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("action") = "v" Then 'Ver
                fnVer()
            End If
        End If

    End Sub
    Public Sub fnLlenarcolumnas()

        ssql = objDatos.fnObtenerQuery("PedidosB2B")

        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")

        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
        dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""



        ssql = objDatos.fnObtenerQuery("MonedasConf")
        Dim dtMonedas As New DataTable
        dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
        Dim sMonedaLocal As String = "M.N."
        Dim sMonedaExtranjera As String = "M.E."
        If dtMonedas.Rows.Count > 0 Then
            sMonedaLocal = dtMonedas.Rows(0)("MainCurncy")
            sMonedaExtranjera = dtMonedas.Rows(0)("SysCurrncy")
        End If

        ''Preparamos los encabezados
        Dim iContador As Int16 = 0

        For i = 0 To dtDocumentos.Columns.Count - 1 Step 1
            If dtDocumentos.Columns(i).ColumnName.ToUpper.Contains("NACIONAL") Then
                iContador = i + 1
                Exit For
            End If
        Next
        Dim iCuantos As Int16 = 0
        For i = 0 To dtDocumentos.Columns.Count - 1 Step 1
            If dtDocumentos.Columns(i).ColumnName.ToUpper.Contains("NACIONAL") Then
                iCuantos = iCuantos + 1

            End If
        Next


        sHtml = sHtml & "<tr>"
        sHtml = sHtml & "  <th colspan=" & iContador & " rowspan=''></th>"
        sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-verde txt-center'>" & sMonedaLocal & "</th>"
        If objDatos.fnObtenerCliente().ToUpper.Contains("PMK") Then

        Else
            sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
        End If
        sHtml = sHtml & "</tr>"

        sHtml = sHtml & "<tr>"
        sHtml = sHtml & "<th style='width:30px;'>ver</th>"

        For i = 0 To dtDocumentos.Columns.Count - 1 Step 1
            If dtDocumentos.Columns(i).ColumnName.Contains("Nacional") Then
                sHtml = sHtml & "<th style='width:100px;' class='tdh-c-verde'>" & dtDocumentos.Columns(i).ColumnName.Replace("Nacional", "") & "</th>"
            End If
            If dtDocumentos.Columns(i).ColumnName.Contains("Extranjera") Then
                sHtml = sHtml & "<th style='width:100px;' class='tdh-c-azul'>" & dtDocumentos.Columns(i).ColumnName.Replace("Extranjera", "") & "</th>"
            End If
            If Not dtDocumentos.Columns(i).ColumnName.Contains("Extranjera") And Not dtDocumentos.Columns(i).ColumnName.Contains("Nacional") Then
                sHtml = sHtml & "<th style='width:100px;'>" & dtDocumentos.Columns(i).ColumnName & "</th>"
            End If

        Next
        sHtml = sHtml & "</tr>"
        Dim literal As New LiteralControl(sHtml)
        pnlColumnas.Controls.Clear()
        pnlColumnas.Controls.Add(literal)
    End Sub

    Public Sub fnLlenarListado()

        ssql = objDatos.fnObtenerQuery("PedidosB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")

        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")

        dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=PedidosB2Bdetalle'></i></td>"

            For x = 0 To dtDocumentos.Columns.Count - 1 Step 1
                If dtDocumentos.Columns(x).ColumnName.Contains("Nacional") Then
                    sHtml = sHtml & "<td class='tdh-c-verde txt-center'>" & CDbl(dtDocumentos.Rows(i)(x)).ToString("###,###,###,###.#0") & "</td>"
                Else
                    If dtDocumentos.Columns(x).ColumnName.Contains("Extranjera") Then
                        sHtml = sHtml & "<td class='tdh-c-azul txt-center'>" & CDbl(dtDocumentos.Rows(i)(x)).ToString("###,###,###,###.#0") & "</td>"
                    Else
                        sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)(x) & "</td>"
                    End If

                End If

            Next

            sHtml = sHtml & "</tr>"



        Next

        Dim literal As New LiteralControl(sHtml)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub
    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Dim iNoPedido As Int64 = 0

        'For Each row As Telerik.Web.UI.GridDataItem In rgvDocumentos.Items
        '    Dim cb As CheckBox = row.FindControl("chkSelect")
        '    If cb.Checked = True Then
        '        iNoPedido = rgvDocumentos.MasterTableView.Items(row.ItemIndex).Item("No").Text
        '        Exit For

        '    End If
        'Next
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ssql = "select cvItemCode ,cvItemName,cfCantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.pedido_det WHERE ciNoPedido= " & "'" & iNoPedido & "'"
            Dim dtLineas As New DataTable
            dtLineas = objDatos.fnEjecutarConsulta(ssql)
            rgvPartidas.DataSource = dtLineas
            rgvPartidas.DataBind()
            rgvPartidas.Visible = True
        End If
    End Sub
    Public Sub fnVer()
        Dim iNoPedido As Int64 = 0
        iNoPedido = Request.QueryString("Id")
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ssql = "select cvItemCode ,cvItemName,cfCantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.pedido_det WHERE ciNoPedido= " & "'" & iNoPedido & "'"
            Dim dtLineas As New DataTable
            dtLineas = objDatos.fnEjecutarConsulta(ssql)
            rgvPartidas.DataSource = dtLineas
            rgvPartidas.DataBind()
            rgvPartidas.Visible = True

            btnConvertir.Visible = True

        End If
    End Sub

End Class
