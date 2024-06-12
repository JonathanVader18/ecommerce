Imports System.Data
Partial Class Cotizaciones
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub Cotizaciones_Load(sender As Object, e As EventArgs) Handles Me.Load

        fnLlenarcolumnas()
        fnLlenarListado()
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("Doc") <> "" Then 'Ver
                fnVer()
            End If
        End If

    End Sub
    Public Sub fnLlenarcolumnas()

        ssql = objDatos.fnObtenerQuery("CotizacionesB2B")
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


        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtclienteSeg As New DataTable
        dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Or CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("PMK") Then
                'sHtml = sHtml & "<tr>"
                'sHtml = sHtml & "  <th colspan=6 rowspan=''></th>"

                'sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
                'sHtml = sHtml & "</tr>"
            Else
                sHtml = sHtml & "<tr>"
                sHtml = sHtml & "  <th colspan=" & iContador & " rowspan=''></th>"
                sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-verde txt-center'>" & sMonedaLocal & "</th>"
                sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
                sHtml = sHtml & "</tr>"
            End If
        End If




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

        ssql = objDatos.fnObtenerQuery("CotizacionesB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
        dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            ' sHtml = sHtml & "<td><a href='cotizaciones.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "'>elegir</a></td>" '<i class='fa fa-chevron-right' aria-hidden='true'></i>
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=CotizacionesB2Bdetalle'></i></td>"

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
        iNoPedido = Request.QueryString("Id")
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            'ssql = "select cvItemCode ,cvItemName,cfCantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.pedido_det WHERE ciNoPedido= " & "'" & iNoPedido & "'"
            'Dim dtLineas As New DataTable
            'dtLineas = objDatos.fnEjecutarConsulta(ssql)
            'rgvPartidas.DataSource = dtLineas
            'rgvPartidas.DataBind()
            'rgvPartidas.Visible = True
        End If
    End Sub

    Public Sub fnVer()

        lblDoc.Text = "Cotización: " & Request.QueryString("Doc")
        lblDoc.Visible = True
        btnConvertir.Visible = True
        'btnCerrar.Visible = True

        'Dim iNoPedido As Int64 = 0
        'iNoPedido = Request.QueryString("Id")
        'If iNoPedido = 0 Then
        '    objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
        '    Exit Sub
        'Else
        '    ssql = "select cvItemCode ,cvItemName,cfCantidad,cfPrecio,cfCantidad * cfPrecio as Total from tienda.pedido_det WHERE ciNoPedido= " & "'" & iNoPedido & "'"
        '    Dim dtLineas As New DataTable
        '    dtLineas = objDatos.fnEjecutarConsulta(ssql)
        '    rgvPartidas.DataSource = dtLineas
        '    rgvPartidas.DataBind()
        '    rgvPartidas.Visible = True

        '    btnConvertir.Visible = True
        '    btnCerrar.Visible = True

        'End If
    End Sub
    Protected Sub btnConvertir_Click(sender As Object, e As EventArgs) Handles btnConvertir.Click
        Dim iNoPedido As Int64 = 0

        'For Each row As Telerik.Web.UI.GridDataItem In rgvDocumentos.Items
        '    Dim cb As CheckBox = row.FindControl("chkSelect")
        '    If cb.Checked = True Then
        '        iNoPedido = rgvDocumentos.MasterTableView.Items(row.ItemIndex).Item("No").Text
        '        Exit For

        '    End If
        'Next
        iNoPedido = Request.QueryString("Doc")
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            fnProcesarSAP(iNoPedido, "PEDIDO", 1)
        End If

    End Sub
    Public Function fnProcesarSAP(idCarrito As Int64, TipoDoc As String, Vienede As Int16)

        ssql = objDatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
        ssql = ssql.Replace("[%0]", idCarrito)
        Dim dtDocEntryInicial As New DataTable
        dtDocEntryInicial = objDatos.fnEjecutarConsultaSAP(ssql)

        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cvNumSAP FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objDatos.fnEjecutarConsulta(ssql)


        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & dtEncabezado.Rows(0)("ciNoPedido") & "'"
        Dim dtPartidas As New DataTable
        dtPartidas = objDatos.fnEjecutarConsulta(ssql)

        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Try
            oCompany = objDatos.fnConexion_SAP
            If oCompany.Connected Then
                If TipoDoc = "COTIZACION" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                End If
                oDoctoVentas.SalesPersonCode = dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.CardCode = dtEncabezado.Rows(0)("cvCveCliente")
                oDoctoVentas.DocDate = Now.Date
                oDoctoVentas.DocDueDate = Now.Date
                oDoctoVentas.Comments = "Desde Internet"

                Dim iLinea As Int16 = 0
                For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                    oDoctoVentas.Lines.Add()
                    oDoctoVentas.Lines.SetCurrentLine(iLinea)

                    oDoctoVentas.Lines.ItemCode = dtPartidas.Rows(i)("cvItemCode")
                    oDoctoVentas.Lines.ItemDescription = dtPartidas.Rows(i)("cvItemName")
                    oDoctoVentas.Lines.Quantity = dtPartidas.Rows(i)("cfCantidad")
                    oDoctoVentas.Lines.Price = dtPartidas.Rows(i)("cfPrecio")
                    oDoctoVentas.Lines.UnitPrice = dtPartidas.Rows(i)("cfPrecio")
                    If Vienede = 1 Then
                        ''Vinculamos con Oferta
                        ssql = objDatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
                        ssql = ssql.Replace("[%0]", dtEncabezado.Rows(0)("cvNumSAP"))
                        Dim dtDocEntry As New DataTable
                        dtDocEntry = objDatos.fnEjecutarConsultaSAP(ssql)
                        oDoctoVentas.Lines.BaseEntry = dtDocEntry.Rows(0)(0)
                        oDoctoVentas.Lines.BaseType = "23"
                        oDoctoVentas.Lines.BaseLine = iLinea
                    End If

                    iLinea = iLinea + 1
                Next
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    objDatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription, Me.Page)
                Else
                    ''Todo bien
                    Dim dtDoc As New DataTable

                    If TipoDoc = "PEDIDO" Then
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumOrdenVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)

                    End If
                    If TipoDoc = "COTIZACION" Then
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumOfertaVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If dtDoc.Rows.Count > 0 Then
                        ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                        objDatos.fnEjecutarInsert(ssql)

                    End If
                    objDatos.Mensaje(TipoDoc & " procesado correctamente " & dtDoc.Rows(0)(0), Me.Page)
                    lblDoc.Visible = False
                    btnConvertir.Visible = False
                    btnCerrar.Visible = False
                End If


            Else
                objDatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)
            End If

        Catch ex As Exception
            objDatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)
        End Try

    End Function
    Protected Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click

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
            ssql = "UPDATE Tienda.Pedido_HDR  SET cvEstatus='CERRADO' WHERE ciNoPedido=  " & "'" & iNoPedido & "'"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.Mensaje("Se ha cerrado el documento", Me.Page)

            fnLlenarListado()
        End If


    End Sub
End Class
