
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Xml
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class factura_modal
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub factura_modal_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim IdDocumento As String
        Dim QueryDoc As String
        IdDocumento = Request.QueryString("Doc")
        QueryDoc = Request.QueryString("QueryDoc")

        fnLlenarcolumnas(IdDocumento, QueryDoc)
        fnLlenarListado(IdDocumento, QueryDoc)
        fnLlenarTotales(IdDocumento, QueryDoc)
        If QueryDoc.Contains("Cotiza") Then
            ' btnCerrar.Visible = True
            btnConvertir.Visible = True
            btnDuplicar.Visible = False
            btnCargar.Visible = True
            btnCerrar.Visible = True
            btnImprimir.Visible = True

            ssql = objdatos.fnObtenerQuery(QueryDoc)
            Dim dtDocumentos As New DataTable
            ssql = ssql.Replace("[%0]", "'" & IdDocumento & "'")
            dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

            Try
                If dtDocumentos.Rows(0)("Estatus") = "O" Then
                    ''Abierto, se puede cancelar
                Else
                    ''Cerrado o ya cancelado, deshabilitamos el boton de cerrar
                    btnCerrar.Enabled = False
                    lblEstatus.Text = "Documento ya cerrado en SAP"
                End If
            Catch ex As Exception

            End Try

            If objdatos.fnObtenerCliente.ToUpper.Contains("ZEYCO") And Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                btnConvertir.Visible = False
                btnCerrar.Visible = False
            End If
            ssql = "select ISNULL(cvImprimeDocumento,'NO') FROM config.parametrizaciones"
            Dim dtImprime As New DataTable
            dtImprime = objdatos.fnEjecutarConsulta(ssql)
            If dtImprime.Rows.Count > 0 Then
                If dtImprime.Rows(0)(0) = "SI" Then
                    btnImprimir.Visible = True
                End If
            End If
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente2 As New DataTable
            dtcliente2 = objdatos.fnEjecutarConsulta(ssql)
            If dtcliente2.Rows.Count > 0 Then

                If CStr(dtcliente2.Rows(0)(0)).ToUpper.Contains("FERCO") Or CStr(dtcliente2.Rows(0)(0)).ToUpper.Contains("INTERGRE") Then
                    pnlActualizar.Visible = True
                    If Not IsPostBack Then
                        ssql = "SELECt ISNULL(U_Estado2,'01'),convert(varchar(10),docDueDate,120) as docDueDate FROM OQUT where docnum=" & "'" & IdDocumento & "'"
                        Dim dtEstadoActual As New DataTable
                        dtEstadoActual = objdatos.fnEjecutarConsultaSAP(ssql)

                        ssql = "select FldValue as Codigo,Descr as Descripcion from UFD1 where TableID ='OQUT' and FieldID =17  "
                        Dim dtEstado As New DataTable
                        dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
                        ddlEstatus.DataSource = dtEstado
                        ddlEstatus.DataTextField = "Descripcion"
                        ddlEstatus.DataValueField = "Codigo"
                        ddlEstatus.DataBind()
                        If dtEstadoActual.Rows.Count > 0 Then
                            ddlEstatus.SelectedValue = dtEstadoActual.Rows(0)(0)
                            Dim sFecha As String()
                            sFecha = CStr(dtEstadoActual.Rows(0)("docDueDate")).Split("-")
                            txtFechaEntrega.Text = sFecha(2) & "/" & sFecha(1) & "/" & sFecha(0)
                        End If
                    End If

                End If
            End If

        End If

        If QueryDoc.Contains("Pedido") Then
            '   btnCerrar.Visible = True
            'btnDuplicar.Visible = True
            btnImprimir.Visible = True
            btnCargar.Visible = True
            btnConvertir.Visible = False
            btnCerrar.Visible = True
            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objdatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then
                If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
                    pnlMoneta.Visible = True
                End If

                If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("FERCO") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("INTERGRE") Then
                    pnlActualizar.Visible = True
                    If Not IsPostBack Then
                        ssql = "SELECt ISNULL(U_Estado2,'01'),convert(varchar(10),docDueDate,120) as docDueDate FROM ORDR where docnum=" & "'" & IdDocumento & "'"
                        Dim dtEstadoActual As New DataTable
                        dtEstadoActual = objdatos.fnEjecutarConsultaSAP(ssql)

                        ssql = "select FldValue as Codigo,Descr as Descripcion from UFD1 where TableID ='ORDR' and FieldID =17  "
                        Dim dtEstado As New DataTable
                        dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
                        ddlEstatus.DataSource = dtEstado
                        ddlEstatus.DataTextField = "Descripcion"
                        ddlEstatus.DataValueField = "Codigo"
                        ddlEstatus.DataBind()
                        If dtEstadoActual.Rows.Count > 0 Then
                            ddlEstatus.SelectedValue = dtEstadoActual.Rows(0)(0)
                            Dim sFecha As String()
                            sFecha = CStr(dtEstadoActual.Rows(0)("docDueDate")).Split("-")
                            txtFechaEntrega.Text = sFecha(2) & "/" & sFecha(1) & "/" & sFecha(0)
                        End If
                    End If

                End If
            End If

        End If
        If Not IsPostBack Then
            Session("RefMoneta") = lblDocNum.Text
            Session("errorMoneta") = "0"
            Dim dia As String = ""
            Dim mes As String = ""
            Dim Año As String = ""
            If Now.Date.Day.ToString.Length > 1 Then
                dia = Now.Date.Day
            Else
                dia = "0" & Now.Date.Day
            End If

            If Now.Date.Month.ToString.Length > 1 Then
                mes = Now.Date.Month
            Else
                mes = "0" & Now.Date.Month
            End If
            Año = Now.Date.Year.ToString.Substring(Now.Date.Year.ToString.Length - 2, 2)
            txtDate.Text = dia & mes & Año
            Dim sRellenar As String = ""
            Dim iDif As Int16

            If lblDocNum.Text.Length >= 6 Then
                txtAudit.Text = lblDocNum.Text.Substring(0, 6)
            Else

                iDif = 6 - lblDocNum.Text.Length

                For v = 1 To iDif Step 1
                    sRellenar = sRellenar & "0"
                Next
                txtAudit.Text = sRellenar & lblDocNum.Text

            End If


            ssql = "SELECT ISNULL(cvMerchantId,'') FROM config.proveedores_pago"
            Dim dtLiga As New DataTable
            dtLiga = objdatos.fnEjecutarConsulta(ssql)
            If dtLiga.Rows.Count > 0 Then
                txtMerchantId.Text = dtLiga.Rows(0)(0)
            End If

            'If Session("Audit") <> "" Then

            '    txtAudit.Text = Session("Audit")
            'End If


            'If Session("TransId") <> "" Then
            '    txtOrigTransId.Text = Session("TransId")
            'End If
        End If

        If QueryDoc.Contains("Anticip") Then
            btnCerrar.Visible = False
            btnDuplicar.Visible = False
            btnConvertir.Visible = False
            btnImprimir.Visible = False
        End If

        If QueryDoc.Contains("Entrega") Then
            btnCerrar.Visible = False
            btnDuplicar.Visible = False
            btnConvertir.Visible = False
            btnImprimir.Visible = False
        End If
        If QueryDoc.Contains("Factura") Or QueryDoc.Contains("EstadoDe") Then
            btnCerrar.Visible = False
            btnDuplicar.Visible = False
            btnConvertir.Visible = False
            btnImprimir.Visible = False
            lblPagado.Visible = True
            pnlPagadoLeyenda.Visible = True
            pnlPendienteLeyenda.Visible = True
            lblAnticipo.Visible = True
            pnlAnticipoLeyenda.Visible = True
            lblSaldo.Visible = True
        End If


        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente3 As New DataTable
        dtcliente3 = objdatos.fnEjecutarConsulta(ssql)
        If dtcliente3.Rows.Count > 0 Then
            If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SEGURIT") Then
                btnConvertir.Visible = False
                btnCerrar.Visible = False
                '  btnImprimir.Visible = True
                lblAnticipo.Visible = False
                lblPagado.Visible = False
                lblSaldo.Visible = False

                pnlAnticipoLeyenda.Visible = False
                pnlPagadoLeyenda.Visible = False
                pnlPendienteLeyenda.Visible = False
                btnImprimir.Text = "Descargar pdf"
                If QueryDoc.Contains("Cotiza") Then
                    btnExcel.Visible = True
                End If

            End If

            If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("BOSS") Then
                btnCerrar.Visible = False
                btnImprimir.Visible = False
            End If
        End If

    End Sub

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub


    Public Sub fnLlenarcolumnas(IdDoc As String, QueryDoc As String)

        ssql = objdatos.fnObtenerQuery(QueryDoc)
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & IdDoc & "'")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        ssql = objdatos.fnObtenerQuery("MonedasConf")
        Dim dtMonedas As New DataTable
        dtMonedas = objdatos.fnEjecutarConsultaSAP(ssql)
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
        dtclienteSeg = objdatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGURI") Then
                sHtml = sHtml & "<tr>"
                sHtml = sHtml & "  <th colspan=8 rowspan=''></th>"

                sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
                sHtml = sHtml & "</tr>"
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
    Public Sub fnLlenarListado(IdDoc As String, QueryDoc As String)

        ssql = objdatos.fnObtenerQuery(QueryDoc)
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & IdDoc & "'")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx'></i></td>"

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

    Public Sub fnDuplicar(IdDoc As String, QueryDoc As String)

        ssql = objdatos.fnObtenerQuery(QueryDoc)
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & IdDoc & "'")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx'></i></td>"

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

    Public Sub fnLlenarTotales(IdDoc As String, QueryDoc As String)
        ssql = objdatos.fnObtenerQuery(QueryDoc & "Totales")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & IdDoc & "'")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtDocumentos.Rows.Count > 0 Then
            lblCliente.Text = dtDocumentos.Rows(0)("CardName")
            lblDocNum.Text = dtDocumentos.Rows(0)("DocNum")
            For x = 0 To dtDocumentos.Columns.Count - 1 Step 1
                If dtDocumentos.Columns(x).ColumnName.ToUpper = "NUMATCARD" Then
                    lblDocNum.Text = lblDocNum.Text & " ( " & dtDocumentos.Rows(0)("numatcard") & ") "
                    Exit For
                End If
            Next

            lblFecha.Text = dtDocumentos.Rows(0)("Fecha")
            lblFechaVence.Text = dtDocumentos.Rows(0)("FechaVence")
            lblMoneda.Text = dtDocumentos.Rows(0)("Moneda")
            ssql = "SELECT slpName FROM OSLP WHERE slpCode in(select slpcode FROM OCRD where cardCode='" & Session("Cliente") & "')"
            Dim dtVendedor As New DataTable
            dtVendedor = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtVendedor.Rows.Count > 0 Then
                lblVendedor.Text = dtVendedor.Rows(0)(0)
            Else
                lblVendedor.Text = ""
            End If

            lblSubTotal.Text = CDbl(dtDocumentos.Rows(0)("Subtotal")).ToString("###,###,###,###.#0")
            lblIVA.Text = CDbl(dtDocumentos.Rows(0)("Impuesto")).ToString("###,###,###,###.#0")
            lblTotal.Text = CDbl(dtDocumentos.Rows(0)("Total")).ToString("###,###,###,###.#0")
            lblPagado.Text = CDbl(dtDocumentos.Rows(0)("Pagado")).ToString("###,###,###,###.#0")
            lblSaldo.Text = (CDbl(dtDocumentos.Rows(0)("Total")) - CDbl(dtDocumentos.Rows(0)("Pagado"))).ToString("###,###,###,###.#0")

        End If
    End Sub




    Public Function fnProcesarSAP(idCarrito As Int64, TipoDoc As String, Vienede As Int16)
        Dim message As String = ""

        ssql = "SELECT ISNULL(cvUsaMotor,'NO') FROM config.parametrizaciones "
        Dim dtUsaMotor As New DataTable
        dtUsaMotor = objdatos.fnEjecutarConsulta(ssql)
        If dtUsaMotor.Rows.Count > 0 Then
            If dtUsaMotor.Rows(0)(0) = "NO" Then

            Else
                ssql = "UPDATE Tienda.Pedido_HDR SET cvConvertir ='SI',ciProcesadoSAP=3 WHERE cvNumSAP=" & "'" & idCarrito & "'"
                objdatos.fnEjecutarInsert(ssql)


                lblEstatus.Visible = True
                lblEstatus.Text = TipoDoc & " procesado correctamente "

                ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & lblEstatus.Text & "');", True)

                message = "alert('" & lblEstatus.Text & "');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert", message, True)


                btnCerrar.Visible = False
                Exit Function
            End If
        Else
            objdatos.fnLog("Cotizacion", "Va a procesar SAP")

        End If



        If TipoDoc = "COTIZACION" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
        End If
        If TipoDoc = "PEDIDO" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOrdenVentas")
        End If

        ssql = ssql.Replace("[%0]", idCarrito)
        Dim dtDocEntryInicial As New DataTable
        dtDocEntryInicial = objdatos.fnEjecutarConsultaSAP(ssql)

        If Vienede = 1 Then



            ''Si viene De...estan convirtiendo cotización a pedido. Las lineas las sacamos de la cotizacion
            ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cvNumSAP FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & idCarrito & "' AND cvTipoDoc=" & "'COTIZACION' AND 1=2"

        Else
            ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cvNumSAP FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & idCarrito & "' AND cvTipoDoc=" & "'" & TipoDoc & "'"
        End If

        Dim dtEncabezado As New DataTable
        dtEncabezado = objdatos.fnEjecutarConsulta(ssql)

        Dim iExisteLocal As Int16 = 1
        Dim dtPartidas As New DataTable

        If dtEncabezado.Rows.Count = 0 Then
            iExisteLocal = 0
            ''Es una cotización directa de SAP, entonces buscamos el detalle en SAP y no en nuestra base
            If TipoDoc = "COTIZACION" Then
                ssql = objdatos.fnObtenerQuery("ConsultaCotizacionHdr")
            End If
            If TipoDoc = "PEDIDO" Then
                ssql = objdatos.fnObtenerQuery("ConsultaPedidoHdr")
            End If


            If Vienede = 1 Then
                ''Si viene De...estan convirtiendo cotización a pedido. Obtenemos el encabezado de la cotización
                ssql = objdatos.fnObtenerQuery("ConsultaCotizacionHdr")
            End If

            ssql = ssql.Replace("[%0]", "'" & idCarrito & "'")
            dtEncabezado = New DataTable
            dtEncabezado = objdatos.fnEjecutarConsultaSAP(ssql)

            If TipoDoc = "COTIZACION" Then
                ssql = objdatos.fnObtenerQuery("ConsultaCotizacionDet")
            End If
            If TipoDoc = "PEDIDO" Then
                If Vienede = 1 Then
                    ''Si viene De...estan convirtiendo cotización a pedido. Las lineas las sacamos de la cotizacion
                    ssql = objdatos.fnObtenerQuery("ConsultaCotizacionDet")

                Else
                    ssql = objdatos.fnObtenerQuery("ConsultaPedidoDet")
                End If

            End If

            ssql = ssql.Replace("[%0]", "'" & dtEncabezado.Rows(0)("DocEntry") & "'")
            dtPartidas = objdatos.fnEjecutarConsultaSAP(ssql)
        Else
            ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & dtEncabezado.Rows(0)("ciNoPedido") & "'"
            dtPartidas = objdatos.fnEjecutarConsulta(ssql)
        End If



        Dim sCardCode As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then
                If TipoDoc = "COTIZACION" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                End If
                '  oDoctoVentas.SalesPersonCode = dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.CardCode = dtEncabezado.Rows(0)("cvCveCliente")
                oDoctoVentas.DocDate = Now.Date
                oDoctoVentas.DocDueDate = Now.Date
                oDoctoVentas.Comments = "Desde Internet"

                objdatos.fnLog("Cotizacion", "Agente SAP: " & CInt(Session("slpCode")))
                sCardCode = dtEncabezado.Rows(0)("cvCveCliente")
                Dim MislpCode As Int16
                If CInt(Session("slpCode")) = 0 Then
                    ssql = "SELECT isnull(slpCode, 0) FROM OCRD WHERE cardCode= " & " '" & sCardCode & "'"
                    Dim dtEmpVentas As New DataTable
                    dtEmpVentas = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtEmpVentas.Rows.Count > 0 Then
                        If CInt(dtEmpVentas.Rows(0)(0)) <> 0 Then
                            MislpCode = CInt(dtEmpVentas.Rows(0)(0))
                            oDoctoVentas.SalesPersonCode = CInt(dtEmpVentas.Rows(0)(0))
                        End If
                    End If

                Else
                    oDoctoVentas.SalesPersonCode = CInt(Session("slpCode"))

                End If

                Try
                    If TipoDoc = "PEDIDO" Then
                        ssql = "SELECT TrnspCode FROM ORDR WHERE DocEntry=" & "'" & dtEncabezado.Rows(0)("DocEntry") & "'"
                    End If

                    If TipoDoc = "COTIZACION" Then
                        ssql = "SELECT TrnspCode FROM OQUT WHERE DocEntry=" & "'" & dtEncabezado.Rows(0)("DocEntry") & "'"
                    End If
                    Dim dtTransport As New DataTable
                    dtTransport = objdatos.fnEjecutarConsultaSAP(ssql)
                    If Vienede = 1 Then
                    Else
                        If dtTransport.Rows.Count > 0 Then
                            oDoctoVentas.TransportationCode = dtTransport.Rows(0)(0)
                        Else
                            oDoctoVentas.TransportationCode = -1
                        End If
                    End If

                Catch ex As Exception

                End Try

                Try

                    Dim sQuerySuc As String = ""
                    sQuerySuc = objdatos.fnObtenerQuery("ObtenerSucursal")
                    If sQuerySuc <> "" Then

                        Try
                            If CInt(Session("slpCode")) = 0 Then
                                sQuerySuc = sQuerySuc.Replace("[%0]", MislpCode)
                            Else
                                sQuerySuc = sQuerySuc.Replace("[%0]", CInt(Session("slpCode")))
                            End If

                            sQuerySuc = sQuerySuc.Replace("[%1]", sCardCode)
                        Catch ex As Exception

                        End Try

                        objdatos.fnLog("Sucursal", sQuerySuc.Replace("'", ""))
                        Dim sSucursalVend As String = ""
                        Dim dtSucursalVend As New DataTable
                        dtSucursalVend = objdatos.fnEjecutarConsultaSAP(sQuerySuc)
                        If dtSucursalVend.Rows.Count > 0 Then
                            sSucursalVend = dtSucursalVend.Rows(0)(0)
                            oDoctoVentas.UserFields.Fields.Item("U_SUCURSAL").Value = sSucursalVend
                        End If


                    End If


                Catch ex As Exception

                End Try


                Dim iLinea As Int16 = 0
                For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                    oDoctoVentas.Lines.Add()
                    oDoctoVentas.Lines.SetCurrentLine(iLinea)

                    oDoctoVentas.Lines.ItemCode = dtPartidas.Rows(i)("cvItemCode")
                    oDoctoVentas.Lines.ItemDescription = dtPartidas.Rows(i)("cvItemName")
                    oDoctoVentas.Lines.Quantity = dtPartidas.Rows(i)("cfCantidad")

                    If Vienede = 1 Then
                        ''Vinculamos con Oferta
                        ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
                        ssql = ssql.Replace("[%0]", dtEncabezado.Rows(0)("cvNumSAP"))
                        Dim dtDocEntry As New DataTable
                        dtDocEntry = objdatos.fnEjecutarConsultaSAP(ssql)
                        oDoctoVentas.Lines.BaseEntry = dtDocEntry.Rows(0)(0)
                        oDoctoVentas.Lines.BaseType = "23"
                        oDoctoVentas.Lines.BaseLine = iLinea
                    Else
                        oDoctoVentas.Lines.Price = dtPartidas.Rows(i)("cfPrecio")
                        oDoctoVentas.Lines.UnitPrice = dtPartidas.Rows(i)("cfPrecio")
                    End If

                    ''Vemos si tenemos centro de costos
                    Dim sQueryCC As String = ""
                    sQueryCC = objdatos.fnObtenerQuery("ObtenerCentroCostos")
                    If sQueryCC <> "" Then
                        Try
                            objdatos.fnLog("CentroCostos", sQueryCC.Replace("'", ""))
                            If CInt(Session("slpCode")) = 0 Then
                                sQueryCC = sQueryCC.Replace("[%0]", MislpCode)
                            Else
                                sQueryCC = sQueryCC.Replace("[%0]", Session("slpCode"))
                            End If

                            sQueryCC = sQueryCC.Replace("[%1]", sCardCode)
                        Catch ex As Exception

                        End Try


                        Dim sCentroCostos As String = ""
                        Dim dtCentro As New DataTable
                        dtCentro = objdatos.fnEjecutarConsultaSAP(sQueryCC)
                        If dtCentro.Rows.Count > 0 Then
                            sCentroCostos = dtCentro.Rows(0)(0)
                            oDoctoVentas.Lines.CostingCode = sCentroCostos
                        End If

                    End If


                    iLinea = iLinea + 1
                Next
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    '  objdatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription, Me.Page)
                    '   ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", "") & "');", True)

                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema al procesar la operación-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    'Session("errorMoneta") = "1"
                    'Try
                    '    Dim valorRespuesta As String

                    '    valorRespuesta = fnGenerarXMLREVERSA()
                    '    lblEstatus.Text = valorRespuesta
                    '    consumirWS(valorRespuesta)


                    'Catch ex2 As Exception
                    '    objdatos.Mensaje(ex2.Message, Me.Page)
                    'End Try
                    objdatos.fnLog("Modal", oCompany.GetLastErrorDescription.Replace("'", ""))
                Else
                    ''Todo bien
                    Dim dtDoc As New DataTable

                    If TipoDoc = "PEDIDO" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumOrdenVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)

                    End If
                    If TipoDoc = "COTIZACION" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumOfertaVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If dtDoc.Rows.Count > 0 Then
                        If iExisteLocal <> 0 Then
                            ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                            objdatos.fnEjecutarInsert(ssql)
                        End If


                    End If
                    lblEstatus.Visible = True
                    lblEstatus.Text = TipoDoc & " procesado correctamente " & dtDoc.Rows(0)(0)

                    ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & lblEstatus.Text & "');", True)

                    message = "alert('" & lblEstatus.Text & "');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert", message, True)


                    btnCerrar.Visible = False
                    btnConvertir.Visible = False
                    btnDuplicar.Visible = False
                    btnCargar.Visible = False

                    '  lblDoc.Visible = False
                    btnConvertir.Visible = False
                    btnCerrar.Visible = False
                End If


            Else
                objdatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)

            End If

        Catch ex As Exception
            objdatos.Mensaje("Ex. No se ha podido establecer conexión con SAP", Me.Page)
            ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & "ERROR-" & ex.Message.Replace("'", "") & "');", True)
        End Try



    End Function


    Public Function fnProcesarSAPRESERVA(idCarrito As Int64, TipoDoc As String, Vienede As Int16)

        If TipoDoc = "COTIZACION" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
        End If
        If TipoDoc = "PEDIDO" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOrdenVentas")
        End If

        ssql = ssql.Replace("[%0]", idCarrito)
        Dim dtDocEntryInicial As New DataTable
        dtDocEntryInicial = objdatos.fnEjecutarConsultaSAP(ssql)

        If Vienede = 1 Then
            ''Si viene De...estan convirtiendo cotización a pedido. Las lineas las sacamos de la cotizacion
            ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cvNumSAP FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & idCarrito & "' AND cvTipoDoc=" & "'COTIZACION' AND 1=2"

        Else
            ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cvNumSAP FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & idCarrito & "' AND cvTipoDoc=" & "'" & TipoDoc & "'"
        End If

        Dim dtEncabezado As New DataTable
        dtEncabezado = objdatos.fnEjecutarConsulta(ssql)

        Dim iExisteLocal As Int16 = 1
        Dim dtPartidas As New DataTable

        If dtEncabezado.Rows.Count = 0 Then
            iExisteLocal = 0

            If TipoDoc = "PEDIDO" Then
                ssql = objdatos.fnObtenerQuery("ConsultaPedidoHdr")
            End If


            If Vienede = 1 Then
                ''Si viene De...estan convirtiendo  pedido a Factura de Reserva.
                ssql = objdatos.fnObtenerQuery("ConsultaPedidoHdr")
            End If

            ssql = ssql.Replace("[%0]", "'" & idCarrito & "'")
            dtEncabezado = New DataTable
            dtEncabezado = objdatos.fnEjecutarConsultaSAP(ssql)


            If TipoDoc = "PEDIDO" Then
                If Vienede = 1 Then
                    ''Si viene De...estan convirtiendo  pedido a Factura de Reserva. Las lineas las sacamos del documento
                    ssql = objdatos.fnObtenerQuery("ConsultaPedidoDet")
                End If

            End If

            ssql = ssql.Replace("[%0]", "'" & dtEncabezado.Rows(0)("DocEntry") & "'")
            dtPartidas = objdatos.fnEjecutarConsultaSAP(ssql)
        Else
            ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & dtEncabezado.Rows(0)("ciNoPedido") & "'"
            dtPartidas = objdatos.fnEjecutarConsulta(ssql)
        End If

        Dim message As String = ""

        Dim sCardCode As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then

                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
                    oDoctoVentas.ReserveInvoice = SAPbobsCOM.BoYesNoEnum.tYES
                    ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='FACTURA RESERVA' and cvTipo='B2B' "
                    Dim dtSerie As New DataTable
                    dtSerie = objdatos.fnEjecutarConsulta(ssql)
                    If dtSerie.Rows.Count > 0 Then
                        If dtSerie.Rows(0)(0) <> "" Then
                            oDoctoVentas.Series = dtSerie.Rows(0)(0)
                        End If

                    End If
                End If
                '  oDoctoVentas.SalesPersonCode = dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.CardCode = dtEncabezado.Rows(0)("cvCveCliente")
                oDoctoVentas.DocDate = Now.Date
                oDoctoVentas.DocDueDate = Now.Date
                oDoctoVentas.Comments = "Desde Internet"

                objdatos.fnLog("Cotizacion", "Agente SAP: " & CInt(Session("slpCode")))
                sCardCode = dtEncabezado.Rows(0)("cvCveCliente")
                Dim MislpCode As Int16
                If CInt(Session("slpCode")) = 0 Then
                    ssql = "SELECT isnull(slpCode, 0) FROM OCRD WHERE cardCode= " & " '" & sCardCode & "'"
                    Dim dtEmpVentas As New DataTable
                    dtEmpVentas = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtEmpVentas.Rows.Count > 0 Then
                        If CInt(dtEmpVentas.Rows(0)(0)) <> 0 Then
                            MislpCode = CInt(dtEmpVentas.Rows(0)(0))
                            oDoctoVentas.SalesPersonCode = CInt(dtEmpVentas.Rows(0)(0))
                        End If
                    End If

                Else
                    oDoctoVentas.SalesPersonCode = CInt(Session("slpCode"))

                End If

                Try
                    If TipoDoc = "PEDIDO" Then
                        ssql = "SELECT TrnspCode FROM ORDR WHERE DocEntry=" & "'" & dtEncabezado.Rows(0)("DocEntry") & "'"
                    End If

                    If TipoDoc = "COTIZACION" Then
                        ssql = "SELECT TrnspCode FROM OQUT WHERE DocEntry=" & "'" & dtEncabezado.Rows(0)("DocEntry") & "'"
                    End If
                    Dim dtTransport As New DataTable
                    dtTransport = objdatos.fnEjecutarConsultaSAP(ssql)

                    If dtTransport.Rows.Count > 0 Then
                        oDoctoVentas.TransportationCode = dtTransport.Rows(0)(0)
                    Else
                        oDoctoVentas.TransportationCode = -1
                    End If
                Catch ex As Exception

                End Try

                Try

                    Dim sQuerySuc As String = ""
                    sQuerySuc = objdatos.fnObtenerQuery("ObtenerSucursal")
                    If sQuerySuc <> "" Then
                        Try
                            If CInt(Session("slpCode")) = 0 Then
                                sQuerySuc = sQuerySuc.Replace("[%0]", MislpCode)
                            Else
                                sQuerySuc = sQuerySuc.Replace("[%0]", CInt(Session("slpCode")))
                            End If

                            sQuerySuc = sQuerySuc.Replace("[%1]", sCardCode)
                        Catch ex As Exception

                        End Try

                        objdatos.fnLog("Sucursal", sQuerySuc.Replace("'", ""))
                        Dim sSucursalVend As String = ""
                        Dim dtSucursalVend As New DataTable
                        dtSucursalVend = objdatos.fnEjecutarConsultaSAP(sQuerySuc)
                        If dtSucursalVend.Rows.Count > 0 Then
                            sSucursalVend = dtSucursalVend.Rows(0)(0)
                            oDoctoVentas.UserFields.Fields.Item("U_SUCURSAL").Value = sSucursalVend
                        End If

                    End If


                Catch ex As Exception

                End Try


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
                        ''Vinculamos con el pedido
                        ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOrdenVentas")
                        ssql = ssql.Replace("[%0]", dtEncabezado.Rows(0)("cvNumSAP"))
                        Dim dtDocEntry As New DataTable
                        dtDocEntry = objdatos.fnEjecutarConsultaSAP(ssql)
                        oDoctoVentas.Lines.BaseEntry = dtDocEntry.Rows(0)(0)
                        oDoctoVentas.Lines.BaseType = "17"
                        oDoctoVentas.Lines.BaseLine = iLinea
                    End If

                    ''Vemos si tenemos centro de costos
                    Dim sQueryCC As String = ""
                    sQueryCC = objdatos.fnObtenerQuery("ObtenerCentroCostos")
                    If sQueryCC <> "" Then
                        Try
                            objdatos.fnLog("CentroCostos", sQueryCC.Replace("'", ""))
                            If CInt(Session("slpCode")) = 0 Then
                                sQueryCC = sQueryCC.Replace("[%0]", MislpCode)
                            Else
                                sQueryCC = sQueryCC.Replace("[%0]", Session("slpCode"))
                            End If

                            sQueryCC = sQueryCC.Replace("[%1]", sCardCode)
                        Catch ex As Exception

                        End Try


                        Dim sCentroCostos As String = ""
                        Dim dtCentro As New DataTable
                        dtCentro = objdatos.fnEjecutarConsultaSAP(sQueryCC)
                        If dtCentro.Rows.Count > 0 Then
                            sCentroCostos = dtCentro.Rows(0)(0)
                            oDoctoVentas.Lines.CostingCode = sCentroCostos
                        End If

                    End If


                    iLinea = iLinea + 1
                Next
                Dim sArticulosSinStock As String = ""
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    '  objdatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription, Me.Page)
                    '   ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", "") & "');", True)

                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema al procesar la transacción-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    Session("errorMoneta") = "1"
                    Try
                        Dim valorRespuesta As String

                        valorRespuesta = fnGenerarXMLREVERSA()
                        lblEstatus.Text = valorRespuesta
                        consumirWS(valorRespuesta)


                    Catch ex2 As Exception
                        objdatos.Mensaje(ex2.Message, Me.Page)
                    End Try

                    objdatos.fnLog("Modal", oCompany.GetLastErrorDescription.Replace("'", ""))
                Else
                    ''Todo bien
                    Dim dtDoc As New DataTable

                    If TipoDoc = "PEDIDO" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumFacturaReserva")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)

                    End If

                    If dtDoc.Rows.Count > 0 Then
                        If iExisteLocal <> 0 Then
                            ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                            objdatos.fnEjecutarInsert(ssql)
                        End If


                    End If

                    ''Todo bien
                    Dim sDocnum As String = ""
                    sDocnum = dtDoc.Rows(0)(0)
                    Session("DocEntry") = oCompany.GetNewObjectKey


                    ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                    Dim dtcliente As New DataTable
                    dtcliente = objdatos.fnEjecutarConsulta(ssql)
                    If dtcliente.Rows.Count > 0 Then
                        If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
                            fnGeneraPagoSAP(sCardCode, Session("DocEntry"), lblSaldo.Text.Replace("MXP", ""), "", idCarrito, sDocnum)
                        End If
                    End If

                    lblEstatus.Visible = True
                    lblEstatus.Text = "Compra " & " procesada correctamente " & dtDoc.Rows(0)(0)

                    Try
                        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                        Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                        Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

                        If sArticulosSinStock <> "" Then
                            ''Tenemos articulos sin stock...notificamos
                            ''Obtenemos correo de notificacion
                            ssql = "select ISNULL(cvNotificaExistencia,'NO'),ISNULL(cvCorreoNotificaExistencia,'') FROM config.parametrizaciones"
                            Dim dtNotifica As New DataTable
                            dtNotifica = objdatos.fnEjecutarConsulta(ssql)
                            If dtNotifica.Rows.Count > 0 Then
                                If dtNotifica.Rows(0)(0) <> "NO" Then
                                    Dim sBody As String = ""
                                    sBody = "Se ha generado un " & TipoDoc & " desde el portal de Ecommerce: " & sDocnum & vbCrLf
                                    sBody = sBody & "Sin embargo, los siguientes artículos no tienen stock:" & vbCrLf
                                    sBody = sBody & sArticulosSinStock
                                    If dtNotifica.Rows(0)(1) <> "" Then
                                        objdatos.fnEnviarCorreo(dtNotifica.Rows(0)(1), sBody, "Notificaciones ECommerce: Artículos sin stock")
                                    End If

                                End If
                            End If
                        End If

                        ''Enviamos correo
                        Dim text As String
                        If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                            text = MensajeHTML(Server.MapPath("~") & "\correo_A_B2B.html")
                        Else
                            text = MensajeHTML(Server.MapPath("~") & "\correo_A.html")
                        End If

                        Dim sDestinatario As String = ""
                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente2 As New DataTable
                        dtcliente2 = objdatos.fnEjecutarConsulta(ssql)

                        objdatos.fnLog("Confirmacion-cliente", dtcliente2.Rows(0)(0))

                        text = text.Replace("{nombrecliente}", dtcliente2.Rows(0)(0))
                        text = text.Replace("{enviara}", "")
                        text = text.Replace("{direccionenvio}", "" & "</br> " & Session("Comentarios"))
                        text = text.Replace("{metodoenvio}", "")
                        text = text.Replace("{numpedido}", dtDoc.Rows(0)(0))
                        text = text.Replace("{fechapedido}", Now.Date.ToShortDateString)
                        ''Ahora las líneas
                        If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                            ''En B2B con PedidoCot, quitamos las líneas
                            text = text.Replace("{lineas}", "")
                        Else
                            text = text.Replace("{lineas}", fnGeneraHTMLPartidas(dtPartidas))
                        End If


                        objdatos.fnLog("Confirmacion", "Antes de enviar correo")

                        ''Revisamos si notificamos a alguien de la empresa
                        ssql = "select ISNULL(cvCorreoNotifica,'') FROM config.parametrizaciones_b2c "
                        Dim dtCorreoInterno As New DataTable
                        dtCorreoInterno = objdatos.fnEjecutarConsulta(ssql)
                        If dtCorreoInterno.Rows.Count > 0 Then
                            If dtCorreoInterno.Rows(0)(0) <> "" Then

                                If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                                    objdatos.fnLog("Correo-SiExiste", Server.MapPath("~") & "PedidoCot.rpt")
                                    fnCreaPDF(Session("DocEntry"), dtCorreoInterno.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")
                                Else
                                    objdatos.fnLog("Correo-NO Existe", Server.MapPath("~") & "\PedidoCot.rpt")
                                    objdatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, sDocnum & "- Pedido desde B2B")
                                End If


                            End If
                        End If


                        ''Revisamos si hay que mandar el correo al cliente
                        If Session("slpCode") <> "0" Or Session("Cliente") <> "" Then
                            ssql = "select ISNULL(cvEnviaCorreoCliente,'NO') FROM config.parametrizaciones"
                            Dim dtEnviaCorreoCliente As New DataTable
                            dtEnviaCorreoCliente = objdatos.fnEjecutarConsulta(ssql)
                            If dtEnviaCorreoCliente.Rows.Count > 0 Then
                                If dtEnviaCorreoCliente.Rows(0)(0) = "SI" Then
                                    ssql = ""
                                    ssql = objdatos.fnObtenerQuery("Correocliente")
                                    If ssql <> "" Then
                                        ssql = ssql.Replace("[%0]", sCardCode)
                                        Dim dtCorreo As New DataTable
                                        dtCorreo = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtCorreo.Rows.Count > 0 Then
                                            If dtCorreo.Rows(0)(0) <> "" Then
                                                ''Enviar el PDF solo si es cliente, aplica solo para cotización
                                                If System.IO.File.Exists(Server.MapPath("~") & "PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                                                    fnCreaPDF(Session("DocEntry"), dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")
                                                Else
                                                    objdatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")


                                                End If
                                                ''Mandamos notificar al vendedor
                                                ssql = "SELECT isnull(slpCode, 0) FROM OCRD WHERE cardCode= " & " '" & sCardCode & "'"
                                                Dim dtVendedor As New DataTable
                                                dtVendedor = objdatos.fnEjecutarConsultaSAP(ssql)
                                                If dtVendedor.Rows.Count > 0 Then
                                                    If CInt(dtVendedor.Rows(0)(0)) <> 0 Then
                                                        ssql = "select ISNULL(email,'') from OHEM where salesPrson =" & "'" & CInt(dtVendedor.Rows(0)(0)) & "'"
                                                        dtCorreo = New DataTable
                                                        dtCorreo = objdatos.fnEjecutarConsultaSAP(ssql)
                                                        If dtCorreo.Rows.Count > 0 Then
                                                            If dtCorreo.Rows(0)(0) <> "" Then
                                                                If System.IO.File.Exists(Server.MapPath("~") & "PedidoCot.rpt") Then
                                                                    fnCreaPDF(Session("DocEntry"), dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")
                                                                Else
                                                                    objdatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada Cliente:" & sCardCode)
                                                                End If


                                                            End If
                                                        End If
                                                    End If
                                                End If



                                            End If
                                        End If
                                    End If

                                End If
                            End If
                        End If



                    Catch ex As Exception

                    End Try


                    ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & lblEstatus.Text & "');", True)

                    message = "alert('" & lblEstatus.Text & "');"
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert", message, True)


                    btnCerrar.Visible = False
                    btnConvertir.Visible = False
                    btnDuplicar.Visible = False
                    btnCargar.Visible = False

                    '  lblDoc.Visible = False
                    btnConvertir.Visible = False
                    btnCerrar.Visible = False
                End If


            Else
                objdatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)

            End If

        Catch ex As Exception
            objdatos.Mensaje("Ex. No se ha podido establecer conexión con SAP", Me.Page)
            ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & "ERROR-" & ex.Message.Replace("'", "") & "');", True)
        End Try



    End Function
    Protected Function MensajeHTML(ArchivoHTML As [String]) As String
        Dim Cuerpo As [String] = ""
        Try
            Dim File As New System.IO.StreamReader(ArchivoHTML)

            Dim Line As [String]
            Dim text As String = System.IO.File.ReadAllText(ArchivoHTML)

            Cuerpo = text

            File.Close()
        Catch ex As Exception
            objdatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function
    Public Sub fnCreaPDF(DocEntry As Int32, correo As String, cuerpo As String, asunto As String)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objdatos.fnLog("Al imprimir", "Antes RPT")
        reporte.Load(Server.MapPath("~") & "PedidoCot.rpt")

        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            reporte.SetParameterValue("DocKey@", Session("DocEntry"))
            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If

        Dim sDocnum As String = ""

        Try
            Dim sDoc As String()
            sDoc = asunto.Split("-")
            sDocnum = sDoc(0).Trim

        Catch ex As Exception

        End Try

        objdatos.fnLog("Al imprimir", "Sale de asignarle la BD")
        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        objdatos.fnLog("Al imprimir", "LogInfo")
        ' reporte.Refresh()
        Try
            objdatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
            reporte.SetParameterValue("DocKey@", DocEntry)
            Dim sArchivo As String = ""
            If sDocnum <> "" Then
                sArchivo = Server.MapPath("~") & "PED-" & sDocnum & ".pdf"
            Else
                sArchivo = Server.MapPath("~") & "PED-" & DocEntry & ".pdf"
            End If

            objdatos.fnLog("Exportar en:", sArchivo)
            Try
                reporte.ExportToDisk(ExportFormatType.PortableDocFormat, sArchivo)
                reporte.Dispose()
            Catch ex As Exception

            End Try


            objdatos.fnEnviarCorreo(correo, cuerpo, sArchivo, asunto)
        Catch ex As Exception
            objdatos.fnLog("Exportar ERROR:", ex.Message.Replace("'", ""))
        End Try


        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub
    Public Function fnGeneraHTMLPartidas(dtPArtidas As DataTable) As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If
        'sLigaSitio 
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Try
            For x = 0 To dtPArtidas.Rows.Count - 1 Step 1
                objdatos.fnLog("Correo B2B", dtPArtidas.Rows.Count)
                sHtmlBanner = sHtmlBanner & " <tr>"
                '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                If dtCamposPlantilla.Rows.Count > 0 Then
                    Dim sCampos As String = ""
                    For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                        ssql = objdatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtPArtidas.Rows(x)("cvItemCode") & "'")
                        Dim dtGeneral As New DataTable
                        dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                        If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                            sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;vertical-align: middle;text-align:center;'> "
                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-width:50px;max-height:100px'>"
                            sHtmlBanner = sHtmlBanner & "</td>"
                        Else
                            If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                If sCampos = "" Then
                                    ''Si es el primer valor que va a enlazar, lo ponemos en strong
                                    sCampos = sCampos & "<strong style='font-size: 13px;color:#000000;'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</strong> <br>"
                                Else
                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                End If

                            Else
                                '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                            End If


                        End If
                    Next
                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:left;font-size: 10px;'>" & sCampos & "</td>"

                End If
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & CDbl(dtPArtidas.Rows(x)("cfPrecio")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & CDbl(dtPArtidas.Rows(x)("cfCantidad")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio"))).ToString(" ###,###,###.#0") & "</td>"

                sHtmlBanner = sHtmlBanner & "</tr>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                sSubTotal = sSubTotal + (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio")))  'dtPartidas.Rows(i)("cfPrecio")
                ''Aqui van los botones de Action Cart
                'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                ''sHtmlBanner = sHtmlBanner & "</div>"

                'sHtmlBanner = sHtmlBanner & " </div> "


            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try
        Return sHtmlEncabezado
    End Function
    Public Sub fnGeneraPagoSAP(Cliente As String, docEntry As Int64, Importe As Double, TipoPago As String, idCarrito As String, DocNum As String)


        ''Registramos el pago en una tabla Temporal. Así, por cualquier error, podemos recuperar el detalle y registrarlo en SAP
        Dim dtIdPago As New DataTable
        ssql = "SELECT ISNULL(MAX(ciIdRel),0) + 1 FROM Tienda.Pagos "
        dtIdPago = objdatos.fnEjecutarConsulta(ssql)

        If dtIdPago.Rows.Count > 0 Then
            ssql = "INSERT INTO Tienda.Pagos(ciIdRel,cvTipoPago,cvDocEntry,cvDocnum,cvCliente,cfImporte,cdFechaSAP) VALUES(" _
                & "'" & dtIdPago.Rows(0)(0) & "'," _
                & "'" & TipoPago & "'," _
                & "'" & docEntry & "'," _
                & "'" & DocNum & "'," _
                & "'" & Cliente & "'," _
                & "'" & CStr(Importe).Replace(",", ".") & "', GETDATE())"
            objdatos.fnLog("Tienda Pagos", ssql.Replace("'", ""))
            objdatos.fnEjecutarInsert(ssql)
        End If


        ''Registramos el pago en SAP

        ssql = "SELECT ISNULL(cvPagoBorrador,'NO') FROM Config.Parametrizaciones_B2C "
        Dim dtPagoBorrador As New DataTable
        dtPagoBorrador = objdatos.fnEjecutarConsulta(ssql)



        Dim oPymt As SAPbobsCOM.Payments
        Dim oCompany As New SAPbobsCOM.Company
        oCompany = objdatos.fnConexion_SAP

        If dtPagoBorrador.Rows.Count > 0 Then
            If dtPagoBorrador.Rows(0)(0) = "SI" Then
                oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts)
            Else
                oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
            End If

        Else
            oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
        End If


        oPymt.DocDate = Now.Date

        ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='PAGO RECIBIDO' and cvTipo='B2B' "
        Dim dtSerie As New DataTable
        dtSerie = objdatos.fnEjecutarConsulta(ssql)

        If dtSerie.Rows.Count > 0 Then
            oPymt.Series = dtSerie.Rows(0)(0)
        End If



        oPymt.CardCode = Cliente
        oPymt.CounterReference = "ECM"
        oPymt.JournalRemarks = "ECM"
        oPymt.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments
        oPymt.Invoices.Add()
        oPymt.Invoices.SetCurrentLine(0)


        oPymt.Invoices.DocEntry = docEntry
        oPymt.Invoices.SumApplied = Importe 'SUM OF INVOICE
        oPymt.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice

        objdatos.fnLog("Aplicar PAGO", "antes de importes")
        oPymt.TransferSum = Importe
        oPymt.TransferDate = Now.Date
        oPymt.TransferReference = Session("RefMoneta")
        oPymt.TransferAccount = fnObtenerCuenta()

        objdatos.fnLog("Aplicar PAGO", "Antes del add")

        If oPymt.Add() <> 0 Then
            ''Error al log
            objdatos.fnLog("Aplicar PAGO", "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
        Else
            objdatos.fnLog("Aplicar PAGO", "Creó Pago en SAP")
            ''Actualizamos
            Dim dtDocnum As New DataTable
            ssql = "SELECT Docnum FROM ORCT WHERE docEntry in (SELECT MAX(DocEntry) FROM ORCT )"
            dtDocnum = New DataTable
            dtDocnum = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtDocnum.Rows.Count > 0 Then

                ssql = "UPDATE Tienda.Pagos SET cvProcesadoSAP='SI', cvDocNumSAP=" & "'" & dtDocnum.Rows(0)(0) & "',cdFechaSAP=GETDATE() WHERE ciIdRel=" & "'" & dtIdPago.Rows(0)(0) & "'"
                ' objDatos.fnLog("Aplicar PAGO", ssql.Replace("'", ""))
                objdatos.fnEjecutarInsert(ssql)
            Else
                objdatos.fnLog("Aplicar PAGO", "No obtiene DocNum PAGO")
            End If


        End If
    End Sub
    Public Function fnObtenerCuenta() As String
        Dim cuenta As String = ""
        ssql = "SELECT cvCuenta FROM config.Parametrizaciones_B2C "
        Dim dtCuenta As New DataTable
        dtCuenta = objdatos.fnEjecutarConsulta(ssql)
        If dtCuenta.Rows.Count > 0 Then
            cuenta = dtCuenta.Rows(0)(0)
        End If

        Return cuenta
    End Function

    Protected Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
        Dim iNoPedido As Int64 = 0
        iNoPedido = Request.QueryString("Doc")

        Dim QueryDoc As String
        QueryDoc = Request.QueryString("QueryDoc")

        If QueryDoc.Contains("Pedido") Then
            fnProcesarSAP(iNoPedido, "PEDIDO", 0)
        Else
            fnProcesarSAP(iNoPedido, "COTIZACION", 0)
        End If
    End Sub
    Protected Sub btnConvertir_Click(sender As Object, e As EventArgs) Handles btnConvertir.Click
        Dim iNoPedido As Int64 = 0
        iNoPedido = Request.QueryString("Doc")

        ssql = "SELECT ISNULL(cvUsaMotor,'NO') FROM config.parametrizaciones "
        Dim dtUsaMotor As New DataTable
        dtUsaMotor = objdatos.fnEjecutarConsulta(ssql)
        Dim Message As String = ""
        If dtUsaMotor.Rows.Count > 0 Then
            If dtUsaMotor.Rows(0)(0) = "NO" Then

            Else
                Dim TipoDoc As String = "COTIZACION"
                ssql = "SELECT * FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & iNoPedido & "'"
                Dim dtExiste As New DataTable
                dtExiste = objdatos.fnEjecutarConsulta(ssql)
                If dtExiste.Rows.Count = 0 Then
                    ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
                    Dim dtId As New DataTable
                    dtId = objdatos.fnEjecutarConsulta(ssql)
                    ssql = "INSERT INTO Tienda.Pedido_HDR ( ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cfTotal) VALUES(" _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & Session("UserTienda") & "'," _
                & "'" & Session("NombreuserTienda") & "'," _
                & "'-1'," _
                & "'" & Session("Cliente") & "'," _
                & "'" & Session("RazonSocial") & "',GETDATE(),''," _
                & "'" & Session("ListaPrecios") & "'," _
                & "'" & TipoDoc & "','ABIERTO'," & "'" & lblTotal.Text.Replace(",", "") & "')"
                    objdatos.fnEjecutarInsert(ssql)

                    ssql = "UPDATE Tienda.Pedido_HDR SET cvConvertir ='SI',ciProcesadoSAP=3,cvNumSAP=" & "'" & iNoPedido & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
                    objdatos.fnEjecutarInsert(ssql)
                End If





                lblEstatus.Visible = True
                lblEstatus.Text = "La cotización se ha" & " procesado correctamente "

                ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('" & lblEstatus.Text & "');", True)

                Message = "alert('" & lblEstatus.Text & "');"
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "alert", Message, True)


                btnCerrar.Visible = False
                Exit Sub
            End If
        Else
            objdatos.fnLog("Cotizacion", "Va a procesar SAP")

        End If

        fnProcesarSAP(iNoPedido, "PEDIDO", 1)
    End Sub


    Public Function fnCargarCarrito(idCarrito As Int64, TipoDoc As String, Vienede As Int16)

        If TipoDoc = "COTIZACION" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
        End If
        If TipoDoc = "PEDIDO" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOrdenVentas")
        End If

        ssql = ssql.Replace("[%0]", idCarrito)
        Dim dtDocEntryInicial As New DataTable
        dtDocEntryInicial = objdatos.fnEjecutarConsultaSAP(ssql)

        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,cvNumSAP FROM Tienda.Pedido_HDR WHERE cvNumSAP=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objdatos.fnEjecutarConsulta(ssql)

        Dim iExisteLocal As Int16 = 1
        Dim dtPartidas As New DataTable

        If dtEncabezado.Rows.Count = 0 Or 1 = 1 Then 'Para que siempre entre y busque en SAP
            iExisteLocal = 0
            ''Es una cotización directa de SAP, entonces buscamos el detalle en SAP y no en nuestra base
            If TipoDoc = "COTIZACION" Then
                ssql = objdatos.fnObtenerQuery("ConsultaCotizacionHdr")
            End If
            If TipoDoc = "PEDIDO" Then
                ssql = objdatos.fnObtenerQuery("ConsultaPedidoHdr")
            End If

            ssql = ssql.Replace("[%0]", "'" & idCarrito & "'")
            dtEncabezado = New DataTable
            dtEncabezado = objdatos.fnEjecutarConsultaSAP(ssql)

            If TipoDoc = "COTIZACION" Then
                ssql = objdatos.fnObtenerQuery("ConsultaCotizacionDet")
            End If
            If TipoDoc = "PEDIDO" Then
                ssql = objdatos.fnObtenerQuery("ConsultaPedidoDet")
            End If

            ssql = ssql.Replace("[%0]", "'" & dtEncabezado.Rows(0)("DocEntry") & "'")
            dtPartidas = objdatos.fnEjecutarConsultaSAP(ssql)
        Else
            ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & dtEncabezado.Rows(0)("ciNoPedido") & "'"
            dtPartidas = objdatos.fnEjecutarConsulta(ssql)
        End If


        Try
            Dim sTallaColor As String = ""
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objdatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If
            End If

            Dim iLinea As Int16 = 0
            For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                Dim partida As New Cls_Pedido.Partidas
                Dim objDatos As New Cls_Funciones
                If sTallaColor = "SI" Then
                    ssql = objDatos.fnObtenerQuery("ObtenerItemCodeGenerico")
                    ssql = ssql.Replace("[%0]", dtPartidas.Rows(i)("cvitemCode"))
                    Dim dtItemGenerico As New DataTable
                    dtItemGenerico = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtItemGenerico.Rows.Count > 0 Then
                        partida.Generico = dtItemGenerico.Rows(0)(0)
                    Else
                        partida.Generico = dtPartidas.Rows(i)("cvitemCode")
                    End If
                End If
                partida.ItemCode = dtPartidas.Rows(i)("cvitemCode")
                partida.Cantidad = dtPartidas.Rows(i)("cfCantidad")
                partida.Moneda = dtPartidas.Rows(i)("Moneda")

                objDatos.fnLog("Carga carrito", "Pasa Moneda")
                Dim dPrecioActual As Double = 0
                If CInt(Session("slpCode")) <> 0 Then

                    dPrecioActual = objDatos.fnPrecioActual(dtPartidas.Rows(i)("cvitemCode"), Session("ListaPrecios"))
                Else
                    dPrecioActual = objDatos.fnPrecioActual(dtPartidas.Rows(i)("cvitemCode"))
                End If
                If CStr(Session("Cliente")) <> "" Then
                    partida.Descuento = objDatos.fnDescuentoEspecial(dtPartidas.Rows(i)("cvitemCode"), Session("Cliente"))
                End If


                partida.Precio = dPrecioActual
                partida.TotalLinea = partida.Cantidad * partida.Precio
                objDatos.fnLog("Carga carrito", "Pasa totallinea")
                ''Ahora el itemName
                Try
                    partida.ItemName = dtPartidas.Rows(i)("cvitemName")
                Catch ex As Exception
                End Try

                Dim iNumLinea As Int16 = 0
                For Each Partidacont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    iNumLinea += 1
                Next
                partida.Linea = iNumLinea

                ssql = "SELECT ISNULL(cvVendeSinStockB2B,'SI') from Config.Parametrizaciones"
                Dim dtVendesinStock As New DataTable
                dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
                If dtVendesinStock.Rows.Count > 0 Then
                    If dtVendesinStock.Rows(0)(0) = "NO" Then
                        ''Evaluamos el stock
                        Dim existencia As Double = 0
                        ''Existencia 
                        ssql = objDatos.fnObtenerQuery("ExistenciaSAPB2B")
                        Dim dtExistencia As New DataTable
                        ssql = ssql.Replace("[%0]", "'" & dtPartidas.Rows(i)("cvitemCode") & "'")
                        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtExistencia.Rows.Count > 0 Then
                            existencia = CDbl(dtExistencia.Rows(0)(0))
                        End If
                        If existencia - dtPartidas.Rows(i)("cfCantidad") <= 0 Then
                            lblEstatus.Text = "La(s) " & dtPartidas.Rows(i)("cfCantidad") & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                            'Exit For
                        End If
                    End If
                End If
                Session("Partidas").add(partida)
            Next

            lblEstatus.Visible = True
            lblEstatus.Text = " Se han cargado los artículos al carrito "

            btnCerrar.Visible = False
            btnConvertir.Visible = False
            btnDuplicar.Visible = False
            btnCargar.Visible = False

            '  lblDoc.Visible = False
            btnConvertir.Visible = False
            btnCerrar.Visible = False

        Catch ex As Exception
            lblEstatus.Text = "Ex. No se ha podido cargar los elementos al carrito" & ex.Message
        End Try


    End Function

    Protected Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        Dim IdDocumento As String
        Dim QueryDoc As String
        IdDocumento = Request.QueryString("Doc")
        QueryDoc = Request.QueryString("QueryDoc")
        Dim sTipo As String = ""
        If QueryDoc.Contains("Pedido") Then
            sTipo = "PEDIDO"
        Else
            sTipo = "COTIZACION"
        End If

        fnCargarCarrito(IdDocumento, sTipo, 0)

        CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Agregado al carrito")

        '  ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "javascript:history.back();alert('Agregado al carrito');", True)
        '  Response.Redirect(Request.UrlReferrer.ToString())
        'Dim aMP As Main = CType(Me.Master, Main)
        'aMP.fnCargaCarrito()
    End Sub

    Private Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Dim sCardCode As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Dim IdDocumento As String
        Dim QueryDoc As String
        IdDocumento = Request.QueryString("Doc")
        QueryDoc = Request.QueryString("QueryDoc")

        Dim TipoDoc As String = ""
        If QueryDoc.Contains("Cotiza") Then
            TipoDoc = "COTIZACION"
        End If
        If QueryDoc.Contains("Pedido") Then
            TipoDoc = "PEDIDO"
        End If

        If TipoDoc = "COTIZACION" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOfertaVentas")
        End If
        If TipoDoc = "PEDIDO" Then
            ssql = objdatos.fnObtenerQuery("ObtenerDocEntryOrdenVentas")
        End If

        Dim DocEntry As Int32 = 0
        ssql = ssql.Replace("[%0]", IdDocumento)
        Dim dtDocEntryInicial As New DataTable
        dtDocEntryInicial = objdatos.fnEjecutarConsultaSAP(ssql)

        If dtDocEntryInicial.Rows.Count > 0 Then
            DocEntry = dtDocEntryInicial.Rows(0)(0)
        End If
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then
                lblEstatus.Visible = True
                If TipoDoc = "COTIZACION" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                End If
                objdatos.fnLog("Actualizar", "Actualizando docentry:" & DocEntry & " tipo: " & TipoDoc)
                '  oDoctoVentas.SalesPersonCode = dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.GetByKey(DocEntry)
                oDoctoVentas.CancelDate = Now.Date

                ''Error
                If oDoctoVentas.Cancel() <> 0 Then
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema: " & oCompany.GetLastErrorDescription)
                    lblEstatus.Text = "Ha ocurrido un problema: " & oCompany.GetLastErrorDescription
                Else
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Documento cancelado!")
                    lblEstatus.Text = "Documento cancelado!"
                End If

            Else
                lblEstatus.Visible = True
                lblEstatus.Text = "Ha ocurrido un problema: No se ha podido conectar con SAP: " & oCompany.GetLastErrorDescription
            End If

        Catch

        End Try
    End Sub
    Public Sub fndescargaXLS(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables


        ssql = "SELECT DocEntry FROM OQUT where docnum=" & "'" & DocEntry & "'"
        Dim dtDocEntry As New DataTable
        dtDocEntry = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtDocEntry.Rows.Count > 0 Then
            DocEntry = dtDocEntry.Rows(0)(0)
        End If

        Try
            objdatos.fnLog("Al imprimir", "Antes RPT")
            reporte.Load(Server.MapPath("~") & "\Pedido.rpt")

            Dim ssql As String
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                reporte.SetParameterValue("DocKey@", DocEntry)
                reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

                crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
                crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
                crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
                crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


            End If



            objdatos.fnLog("Al imprimir", "Sale de asignarle la BD")
            CrTables = reporte.Database.Tables
            For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            objdatos.fnLog("Al imprimir", "LogInfo")
            ' reporte.Refresh()

            objdatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
            reporte.SetParameterValue("DocKey@", DocEntry)
            reporte.ExportToHttpResponse(ExportFormatType.Excel, Response, True, "PED-" & DocEntry)
            reporte.Dispose()
        Catch ex As Exception
            objdatos.fnLog("Al imprimir ex ", ex.Message.Replace("'", ""))
        End Try


        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub

    Public Sub fndescargaPDF(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        Dim IdDocumento As String
        Dim QueryDoc As String
        IdDocumento = Request.QueryString("Doc")
        QueryDoc = Request.QueryString("QueryDoc")
        Dim sTabla As String = """OQUT"""
        Dim sReporte As String = "PedidoCot.rpt"
        Dim sTipoObjeto As String = "23"
        If QueryDoc.Contains("Pedido") Then
            sTabla = """ORDR"""
            sReporte = "Pedido.rpt"
            sTipoObjeto = "17"
        End If

        If objdatos.fnObtenerDBMS.ToUpper = "HANA" Then
            ssql = "SELECT ""DocEntry"" FROM ""#BDSAP#""." & sTabla & " where ""DocNum""=" & "'" & DocEntry & "'"
        Else
            ssql = "SELECT DocEntry FROM OQUT where DocNum=" & "'" & DocEntry & "'"
        End If

        Dim dtDocEntry As New DataTable
        dtDocEntry = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtDocEntry.Rows.Count > 0 Then
            DocEntry = dtDocEntry.Rows(0)(0)
        End If

        Try
            objdatos.fnLog("Al imprimir", "Antes RPT")
            reporte.Load(Server.MapPath("~") & "\" & sReporte)

            Dim ssql As String
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                reporte.SetParameterValue("DocKey@", DocEntry)
                reporte.SetParameterValue("ObjectId@", sTipoObjeto)
                reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

                crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
                crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
                crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
                crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


            End If



            objdatos.fnLog("Al imprimir", "Sale de asignarle la BD")
            CrTables = reporte.Database.Tables
            For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next

            objdatos.fnLog("Al imprimir", "LogInfo")
            ' reporte.Refresh()

            objdatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
            reporte.SetParameterValue("DocKey@", DocEntry)
            reporte.SetParameterValue("ObjectId@", sTipoObjeto)
            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "PED-" & DocEntry)
            reporte.Dispose()
        Catch ex As Exception
            objdatos.fnLog("Al imprimir ex ", ex.Message.Replace("'", ""))
        End Try


        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub



    Private Sub btnPagar_Click(sender As Object, e As EventArgs) Handles btnPagar.Click
        Dim iNoPedido As Int64 = 0
        iNoPedido = Request.QueryString("Doc")


        Session("RefMoneta") = iNoPedido.ToString
        ' fnProcesarSAPRESERVA(iNoPedido, "PEDIDO", 1)
        If txtCodMoneta.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese un código Transfer")
            Exit Sub
        End If

        ''Validamos si la orden de venta no ha sido pagada, por el estatus = 'C'

        Dim sQuery As String = ""
        sQuery = objdatos.fnObtenerQuery("EstatusOrdenVenta")
        If sQuery <> "" Then
            Dim dtEstatus As New DataTable
            dtEstatus = objdatos.fnEjecutarConsultaSAP(sQuery.Replace("[%0]", iNoPedido))
            If dtEstatus.Rows.Count > 0 Then
                If dtEstatus.Rows(0)(0) <> "O" Then
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Esta orden de venta ya se encuentra pagada o bien ha sido cerrada en el sistema. Favor de elegir otra del listado anterior")
                    Exit Sub
                End If
            End If

        End If

        'txtAudit.Text = Convert.ToInt64(txtAudit.Text) + 1
        'Dim valor As Int64
        'valor = txtOrigTransId.Text.Substring(txtOrigTransId.Text.Length - 3, 3)
        'valor = valor + 1

        'txtOrigTransId.Text = (txtOrigTransId.Text.Substring(0, txtOrigTransId.Text.Length - 3) & valor.ToString).ToString
        'lblEstatus.Text = ""

        'Session("TransId") = (txtOrigTransId.Text.Substring(0, txtOrigTransId.Text.Length - 3) & valor.ToString).ToString
        'Session("Audit") = (Convert.ToInt64(txtAudit.Text) + 1).ToString

        Dim valorXML As String = ""
        valorXML = fnGenerarXML()
        objdatos.fnLogMoneta(txtCodMoneta.Text, valorXML)
        consumirWS(valorXML)

        ''Este IF es para validar que se procesó correctamente la OTP y que RefMoneta ahora tiene el MpsTransactionId de la operación
        If Session("RefMoneta") <> iNoPedido.ToString And Session("errorMoneta") = "0" Then
            ''
            fnProcesarSAPRESERVA(iNoPedido, "PEDIDO", 1)
        Else
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "No se pudo completar la operación, intente nuevamente. Es probable que su código transfer no sea correcto")
        End If
    End Sub
    Protected Sub btnprint_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        fndescargaPDF(Request.QueryString("Doc"))
    End Sub
    Public Sub consumirWS(valorXML As String)
        Dim request As HttpWebRequest = CreateWebRequest()
        Dim soapEnvelopeXml As XmlDocument = New XmlDocument()

        Session("errorMoneta") = "0"
        soapEnvelopeXml.LoadXml(valorXML)

        Using stream As Stream = request.GetRequestStream()
            soapEnvelopeXml.Save(stream)
        End Using
        Dim soapResult As String = ""

        Try
            Using response As WebResponse = request.GetResponse()

                Using rd As StreamReader = New StreamReader(response.GetResponseStream())
                    soapResult = rd.ReadToEnd()
                    lblEstatus.Text = soapResult
                End Using
            End Using

            If lblEstatus.Text.Contains("911") Then
                Session("errorMoneta") = "1"
                Try
                    Dim valorRespuesta As String

                    valorRespuesta = fnGenerarXMLREVERSA()
                    objdatos.fnLogMoneta(txtCodMoneta.Text & "-REVERSA", valorXML)
                    lblEstatus.Text = valorRespuesta
                    consumirWS(valorRespuesta)


                Catch ex2 As Exception
                    objdatos.Mensaje(ex2.Message, Me.Page)
                End Try
            End If
            objdatos.fnLogMoneta(txtCodMoneta.Text & "-Response", soapResult)
            If soapResult.Contains("<singleValue>0</singleValue>") Then
                ''Transacción de reversa
                '  CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "No se pudo completar la operación, intente nuevamente. Es probable que su código transfer no sea correcto")
            End If
            Try
                Dim xDoc As XmlDocument = New XmlDocument()
                xDoc.LoadXml(soapResult)
                Dim personas As XmlNodeList = xDoc.GetElementsByTagName("ns2:invokeResponse")
                Dim lista As XmlNodeList = (CType(personas(0), XmlElement)).GetElementsByTagName("return")

                Dim iEncuentra As Int16 = 0
                For Each nodo As XmlElement In lista
                    Dim i As Integer = 0
                    Dim nNombre As XmlNodeList = nodo.GetElementsByTagName("values")

                    For Each nodoValues As XmlElement In nNombre

                        For Each nodoDetalle As XmlElement In nodoValues
                            If nodoDetalle.Name = "name" Then
                                If nodoDetalle.InnerText = "mpsTransactionId" Then
                                    iEncuentra = 1
                                End If
                            End If
                            If nodoDetalle.Name = "singleValue" And iEncuentra = 1 Then
                                Session("RefMoneta") = nodoDetalle.InnerText
                                Exit For
                            End If
                        Next
                    Next
                Next
            Catch ex2 As Exception

            End Try
        Catch ex As Exception
            Session("errorMoneta") = "1"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, ex.Message)
            Try
                Dim valorRespuesta As String
                Session("errorMoneta") = "1"
                valorRespuesta = fnGenerarXMLREVERSA()
                lblEstatus.Text = valorRespuesta
                consumirWS(valorRespuesta)
                'soapEnvelopeXml = New XmlDocument()
                'soapEnvelopeXml.LoadXml(txtRespuesta.Text)
                'request = CreateWebRequest()
                'Using response As WebResponse = request.GetResponse()

                '    Using rd As StreamReader = New StreamReader(response.GetResponseStream())
                '        Dim soapResult As String = rd.ReadToEnd()
                '        txtRespuesta.Text = soapResult
                '    End Using
                'End Using


            Catch ex2 As Exception
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, ex2.Message)
            End Try


        End Try


    End Sub

    Public Shared Function CreateWebRequest() As HttpWebRequest

        Dim ssql As String = ""
        Dim sLiga As String = "http://152.151.41.75:7610/OPM/OpenAPI/services"
        Dim objDatos As New Cls_Funciones
        ssql = "SELECT ISNULL(cvUrlPostback,'') FROM config.proveedores_pago"
        Dim dtLiga As New DataTable
        dtLiga = objDatos.fnEjecutarConsulta(ssql)
        If dtLiga.Rows.Count > 0 Then
            sLiga = dtLiga.Rows(0)(0)
        End If

        Dim webRequest As HttpWebRequest = CType(webRequest.Create(sLiga), HttpWebRequest)
        webRequest.Headers.Add("SOAP:Action")
        webRequest.ContentType = "text/xml;charset=""utf-8"""
        webRequest.Accept = "text/xml"
        webRequest.Method = "POST"
        Return webRequest
    End Function

    Public Function fnGenerarXML() As String
        Dim sXML As String = ""

        Dim iDif As Int16
        Dim sRellenar As String = ""
        iDif = 35 - lblDocNum.Text.Length - txtCodMoneta.Text.Length

        For v = 1 To iDif Step 1
            sRellenar = sRellenar & "0"
        Next
        txtOrigTransId.Text = sRellenar & lblDocNum.Text & txtCodMoneta.Text

        sXML = sXML & "<?xml version='1.0' encoding='UTF-8'?> "
        sXML = sXML & " <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:open='http://trivnet.com/OpenAPI'> "
        sXML = sXML & " <soapenv:Header /> "
        sXML = sXML & " <soapenv:Body> "
        sXML = sXML & " <open:invoke> "
        sXML = sXML & " <!--Optional:--> "
        sXML = sXML & " <arg0> "
        sXML = sXML & " <!--Zero or more repetitions:--> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>serviceName</name> "
        sXML = sXML & " <singleValue>debitAndCreditAccounts</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>transactionType</name> "
        sXML = sXML & " <singleValue>02</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>systemDate</name> "
        sXML = sXML & " <singleValue>00000" & txtDate.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>countryCode</name> "
        sXML = sXML & " <singleValue>484</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>originatorTransactionId</name> "
        sXML = sXML & " <singleValue>" & txtOrigTransId.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>auditNumber</name> "
        sXML = sXML & " <singleValue>" & txtAudit.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>accessMethod</name> "
        sXML = sXML & " <singleValue>117</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>currency</name> "
        sXML = sXML & " <singleValue>484</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>amount</name> "
        sXML = sXML & " <singleValue>" & lblTotal.Text.Replace(",", "") & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>sourceAccountId</name> "
        sXML = sXML & " <singleValue>" & txtCodMoneta.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>sourceAccountIdType</name> "
        sXML = sXML & " <singleValue>105</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>merchantId</name> "
        sXML = sXML & " <singleValue>" & txtMerchantId.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>posId</name> "
        sXML = sXML & " <singleValue>098754321098732109854321654321</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " </arg0> "
        sXML = sXML & " </open:invoke> "
        sXML = sXML & " </soapenv:Body> "
        sXML = sXML & " </soapenv:Envelope> "


        Return sXML

    End Function


    Public Function fnGenerarXMLREVERSA() As String
        Dim sXML As String = ""

        sXML = sXML & "<?xml version='1.0' encoding='UTF-8'?> "
        sXML = sXML & " <soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:open='http://trivnet.com/OpenAPI'> "
        sXML = sXML & " <soapenv:Header /> "
        sXML = sXML & " <soapenv:Body> "
        sXML = sXML & " <open:invoke> "
        sXML = sXML & " <!--Optional:--> "
        sXML = sXML & " <arg0> "
        sXML = sXML & " <!--Zero or more repetitions:--> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>serviceName</name> "
        sXML = sXML & " <singleValue>debitAndCreditAccountsReversal</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>transactionType</name> "
        sXML = sXML & " <singleValue>02</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>systemDate</name> "
        sXML = sXML & " <singleValue>00000" & txtDate.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>countryCode</name> "
        sXML = sXML & " <singleValue>484</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>originalOriginatorTransactionId</name> "
        sXML = sXML & " <singleValue>" & txtOrigTransId.Text & "</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>auditNumber</name> "
        sXML = sXML & " <singleValue>123456</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>accessMethod</name> "
        sXML = sXML & " <singleValue>117</singleValue> "
        sXML = sXML & " </values> "
        sXML = sXML & " <values> "
        sXML = sXML & " <name>originalAuditNumber</name> "
        sXML = sXML & " <singleValue>" & txtAudit.Text & "</singleValue> "
        sXML = sXML & " </values> "

        sXML = sXML & " </arg0> "
        sXML = sXML & " </open:invoke> "
        sXML = sXML & " </soapenv:Body> "
        sXML = sXML & " </soapenv:Envelope> "


        Return sXML

    End Function

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Try
            Dim IdDocumento As String
            Dim DocEntry As String = ""
            Dim sTipoDoc As String = ""
            Dim QueryDoc As String
            IdDocumento = Request.QueryString("Doc")
            QueryDoc = Request.QueryString("QueryDoc")


            If QueryDoc.Contains("Cotiza") Then
                sTipoDoc = "COTIZACION"
                ssql = "SELECT DocEntry FROM OQUT where docnum=" & "'" & IdDocumento & "'"


            End If
            If QueryDoc.Contains("Pedido") Then
                sTipoDoc = "PEDIDO"
                ssql = "SELECT DocEntry FROM ORDR where docnum=" & "'" & IdDocumento & "'"
            End If
            objdatos.fnLog("Actualizar", ssql.Replace("'", ""))
            Dim dtDocEntry As New DataTable
            dtDocEntry = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtDocEntry.Rows.Count > 0 Then
                DocEntry = dtDocEntry.Rows(0)(0)
                fnActualizaDocumento(DocEntry, sTipoDoc)

            End If
            fnLlenarcolumnas(IdDocumento, QueryDoc)
            fnLlenarListado(IdDocumento, QueryDoc)
            fnLlenarTotales(IdDocumento, QueryDoc)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub fnActualizaDocumento(DocEntry As Int32, TipoDoc As String)
        Dim sCardCode As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then
                If TipoDoc = "COTIZACION" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)

                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                End If
                objdatos.fnLog("Actualizar", "Actualizando docentry:" & DocEntry & " tipo: " & TipoDoc)
                '  oDoctoVentas.SalesPersonCode = dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.GetByKey(DocEntry)
                oDoctoVentas.DocDueDate = txtFechaEntrega.Text
                oDoctoVentas.UserFields.Fields.Item("U_Estado2").Value = ddlEstatus.SelectedValue
                If oDoctoVentas.Update <> 0 Then
                    ''Error
                    lblEstatus.Text = "Ha ocurrido un problema: " & oCompany.GetLastErrorDescription
                Else
                    lblEstatus.Text = "Actualizado!"
                End If
            End If
        Catch
        End Try

    End Sub

    Private Sub factura_modal_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Private Sub btnExcel_Click(sender As Object, e As EventArgs) Handles btnExcel.Click
        fndescargaXLS(Request.QueryString("Doc"))
    End Sub
End Class
