
Imports System.Data
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Facturas
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub Facturas_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnLlenarcolumnas()
        fnLlenarListado()

        If Request.QueryString.Count > 0 Then
            If Request.QueryString("action") = "v" Then 'Ver
                fnVer()
            End If
            If Request.QueryString("factura") <> "" Then

                If Request.QueryString("XML") <> "" Then
                    fnDescargarXML(Request.QueryString("factura"))
                End If
                If Request.QueryString("PDF") <> "" Then
                    If objdatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objdatos.fnObtenerCliente.ToUpper.Contains("PMK") Or objdatos.fnObtenerCliente.ToUpper.Contains("SUJEAUT") Or objdatos.fnObtenerCliente.ToUpper.Contains("MANIJA") Then
                        objdatos.fnLog("exportar pdf", "Descargar PDF")
                        fnDescargarPDF(Request.QueryString("factura"))
                    Else
                        objdatos.fnLog("exportar pdf", "Descargar PDF v2:" & objdatos.fnObtenerCliente.ToUpper)
                        fnDescargarPDFv2(Request.QueryString("factura"))
                    End If

                End If

            End If
        End If
    End Sub

    Public Sub fnDescargarXML(docEntry As Int64)
        'ssql = "SELECT DISTINCT  T0.DOCDATE, T0.CardCode, T1.CardName, T0.DocEntry, ISNULL(T2.ReportID,'') eDocNum, ISNULL(T1.E_Mail,'')  as Mail,T0.U_EnviadoPDF ,t0.DocNum, ISNULL(T1.U_emailPDF,'') as emailPDF, T0.U_EnviadoPDF, T0.U_EnviadoXML,CAST(year(T0.DocDate) AS VARCHAR(4))+'-'+RIGHT('00'++ CAST(month(T0.DocDate) AS VARCHAR(4)),2) as Fecha" _
        '                & " from    OINV T0  INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode INNER JOIN ECM2  T2 ON T0.DocEntry = T2.SrcObjAbs " _
        '                & " WHERE ISNULL(U_EnviadoPDF,'NO') ='NO' AND T1.E_Mail is not null and T2.ReportID is not null AND T0.docentry=" & "'" & docEntry & "'"


        Dim dtXML As New DataTable
        Dim rutaXML As String = ""
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objdatos.fnEjecutarConsulta(ssql)
        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
                rutaXML = "\\Stop-sql\fae\Delta"
            Else
                rutaXML = "\\Zeycoserver3\FAECFDI\ALEZEYCO\XML\"
            End If
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("HAWK") Then
                rutaXML = "\\aldogdl-sap02\Carpetas FAE B1\CFDI_SIA\XML\"
            End If
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("ALTURA") Then
                rutaXML = "\\aldogdl-sap02\Carpetas FAE B1\CFDI_ALTO\XML\"
            End If
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("SUJEAU") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("MANIJ") Then

                ssql = "SELECT ISNULL(cvRutaXML,'') as Nombre from config .Parametrizaciones  "
                Dim dtRutaXML As New DataTable
                dtRutaXML = objdatos.fnEjecutarConsulta(ssql)

                If dtRutaXML.Rows.Count = 0 Then
                    rutaXML = "\\svrwinmnjauto\XML\SUJ1211235J1\"
                Else
                    rutaXML = dtRutaXML.Rows(0)(0)
                End If

            End If
        End If


        '\\Stop-sql\fae\Salama\Factura
        ssql = "SELECT ISNULL(U_rutaCodigo,'') as ruta FROM OINV where docentry=" & "'" & docEntry & "'"
        dtXML = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtXML.Rows.Count > 0 Then

            '            File.Copy(rutaXML & "\" & dtXML.Rows(0)("Fecha") & "\" & dtXML.Rows(0)("CardCode") & "\IN\" & dtXML.Rows(0)("eDocNum") & ".xml", Server.MapPath("~") & "\FacturasTEMP\" & dtXML.Rows(0)("eDocNum") & ".xml")
            File.Copy(rutaXML & "\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), True)

        End If

        If objdatos.fnObtenerCliente.ToUpper.Contains("AIO") Then
            rutaXML = "\\AUTIMP-sap\Archivos de SAP\XML\AIO090330JP2\"

            ssql = "SELECT  Convert(varchar(7),docdate,120)+'\' + cardcode + '\RF\'  as Ruta1 , reportId + '.xml' as Ruta,T0.Docdate as Fecha FROM OINV T0 WITH(nolock) INNER JOIN ECM2 T1  WITH(nolock) ON T1.SrcObjType ='13' AND T1.SrcObjAbs =T0.docEntry WHERE T0.docentry=" & "'" & docEntry & "'"
            dtXML = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtXML.Rows.Count > 0 Then
                If CDate(dtXML.Rows(0)("Fecha")).Year >= 2023 And CDate(dtXML.Rows(0)("Fecha")).Month > 3 Then
                    rutaXML = "\\AUTIMP-sap\export\autimp-sap.autimpocc.com.mx\SBO_AIO\CFDi\AIO090330JP2\"
                End If

                '            File.Copy(rutaXML & "\" & dtXML.Rows(0)("Fecha") & "\" & dtXML.Rows(0)("CardCode") & "\IN\" & dtXML.Rows(0)("eDocNum") & ".xml", Server.MapPath("~") & "\FacturasTEMP\" & dtXML.Rows(0)("eDocNum") & ".xml")
                File.Copy(rutaXML & "\" & CStr(dtXML.Rows(0)("ruta1")) & "\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), True)

            End If
        End If
        If objdatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
            rutaXML = "\\AUTIMP-sap\Archivos de SAP\XML\PIM170316V43\"

            ssql = "SELECT  Convert(varchar(7),docdate,120)+'\' + cardcode + '\RF\'  as Ruta1 , reportId + '.xml' as Ruta,T0.Docdate as Fecha FROM OINV T0 WITH(nolock) INNER JOIN ECM2 T1  WITH(nolock) ON T1.SrcObjType ='13' AND T1.SrcObjAbs =T0.docEntry WHERE T0.docentry=" & "'" & docEntry & "'"
            dtXML = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtXML.Rows.Count > 0 Then
                If CDate(dtXML.Rows(0)("Fecha")).Year >= 2023 And CDate(dtXML.Rows(0)("Fecha")).Month > 3 Then
                    rutaXML = "\\AUTIMP-sap\export\autimp-sap.autimpocc.com.mx\SBO_PMK\CFDi\PIM170316V43\"
                End If


                '            File.Copy(rutaXML & "\" & dtXML.Rows(0)("Fecha") & "\" & dtXML.Rows(0)("CardCode") & "\IN\" & dtXML.Rows(0)("eDocNum") & ".xml", Server.MapPath("~") & "\FacturasTEMP\" & dtXML.Rows(0)("eDocNum") & ".xml")
                File.Copy(rutaXML & "\" & CStr(dtXML.Rows(0)("ruta1")) & "\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), True)

            End If
        End If
        If objdatos.fnObtenerCliente.ToUpper.Contains("SUJEAU") Then
            ' rutaXML = "\\192.168.0.201\b1_shf\192.168.0.201\SBO_SUJEAUTO\SUJ1211235J1\"

            ssql = "SELECT CONCAT(CONCAT(CONCAT(TO_VARCHAR(T0.""DocDate"",'YYYY-MM'),'\') , T0.""CardCode"") , '\RF\')  as Ruta1 , CONCAT(T1.""ReportID"" , '.xml') as Ruta,T0.""DocDate"" as Fecha FROM  ""#BDSAP#"".""OINV"" T0  INNER JOIN ""#BDSAP#"".""ECM2"" T1 ON T1.""SrcObjType"" ='13' AND T1.""SrcObjAbs"" =T0.""DocEntry"" WHERE T0.""DocEntry""=" & "'" & docEntry & "'"
            dtXML = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtXML.Rows.Count > 0 Then

                '            File.Copy(rutaXML & "\" & dtXML.Rows(0)("Fecha") & "\" & dtXML.Rows(0)("CardCode") & "\IN\" & dtXML.Rows(0)("eDocNum") & ".xml", Server.MapPath("~") & "\FacturasTEMP\" & dtXML.Rows(0)("eDocNum") & ".xml")

                If File.Exists(rutaXML & "\" & CStr(dtXML.Rows(0)("ruta1")) & "\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml")) Then
                    File.Copy(rutaXML & "\" & CStr(dtXML.Rows(0)("ruta1")) & "\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"), True)
                Else
                    objdatos.Mensaje("No se ha encontrado el XML de la factura", Me.Page)
                    Exit Sub
                End If
            End If
        End If

        Try

            Response.ClearContent()
            Context.Response.Clear()
            Context.Response.AddHeader("content-disposition", "attachment;filename=" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"))
            Response.ContentType = "text/xml"
            Response.WriteFile(Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml").Replace(".jpeg", ".xml"))
            Response.End()

            'Context.Response.Clear()
            'Context.Response.ContentType = "application/octet-stream"
            'Context.Response.AddHeader("content-disposition", "attachment;filename=" + Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml"))
            'Response.WriteFile(Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml"))


            'Dim fileInfo As File
            'File = New File
            'Response.Clear()
            'Response.ClearHeaders()
            'Response.ClearContent()
            'Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml"))
            'Response.AddHeader("Content-Length", File.Length.ToString())
            'Response.ContentType = "text/plain"
            'Response.Flush()
            'Response.TransmitFile(File.FullName)
            'Response.End()
        Catch ex As Exception

        End Try


    End Sub


    Public Sub fnDescargarPDFv2(docEntry As Int64)
        'ssql = "SELECT DISTINCT  T0.DOCDATE, T0.CardCode, T1.CardName, T0.DocEntry, ISNULL(T2.ReportID,'') eDocNum, ISNULL(T1.E_Mail,'')  as Mail,T0.U_EnviadoPDF ,t0.DocNum, ISNULL(T1.U_emailPDF,'') as emailPDF, T0.U_EnviadoPDF, T0.U_EnviadoXML,CAST(year(T0.DocDate) AS VARCHAR(4))+'-'+RIGHT('00'++ CAST(month(T0.DocDate) AS VARCHAR(4)),2) as Fecha" _
        '                & " from    OINV T0  INNER JOIN OCRD T1 ON T0.CardCode = T1.CardCode INNER JOIN ECM2  T2 ON T0.DocEntry = T2.SrcObjAbs " _
        '                & " WHERE ISNULL(U_EnviadoPDF,'NO') ='NO' AND T1.E_Mail is not null and T2.ReportID is not null AND T0.docentry=" & "'" & docEntry & "'"


        Dim dtXML As New DataTable
        Dim rutaXML As String = ""
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objdatos.fnEjecutarConsulta(ssql)
        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
                rutaXML = "\\Stop-sql\fae\Delta"
            Else
                rutaXML = "\\Zeycoserver3\FAECFDI\ALEZEYCO\XML\"
            End If
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("HAWK") Then
                rutaXML = "\\aldogdl-sap02\Carpetas FAE B1\CFDI_SIA\XML\"
            End If
            If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("ALTURA") Then
                rutaXML = "\\aldogdl-sap02\Carpetas FAE B1\CFDI_ALTO\XML\"
            End If
        End If


        '\\Stop-sql\fae\Salama\Factura
        ssql = "SELECT ISNULL(U_rutaCodigo,'') as ruta FROM OINV where docentry=" & "'" & docEntry & "'"
        dtXML = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtXML.Rows.Count > 0 Then
            Dim sRutaFActura As String = CStr(rutaXML & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".pdf")).Replace("\\Factura", "\Factura")
            objdatos.fnLog("Factura 2:", sRutaFActura)
            '            File.Copy(rutaXML & "\" & dtXML.Rows(0)("Fecha") & "\" & dtXML.Rows(0)("CardCode") & "\IN\" & dtXML.Rows(0)("eDocNum") & ".xml", Server.MapPath("~") & "\FacturasTEMP\" & dtXML.Rows(0)("eDocNum") & ".xml")
            File.Copy(sRutaFActura, Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".pdf").Replace(".jpeg", ".pdf"), True)

        End If
        Try

            Response.ClearContent()
            Context.Response.Clear()
            Context.Response.AddHeader("content-disposition", "attachment;filename=" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".pdf").Replace(".jpeg", ".pdf"))
            Response.ContentType = "text/xml"
            Response.WriteFile(Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".pdf").Replace(".jpeg", ".pdf"))
            Response.End()

            'Context.Response.Clear()
            'Context.Response.ContentType = "application/octet-stream"
            'Context.Response.AddHeader("content-disposition", "attachment;filename=" + Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml"))
            'Response.WriteFile(Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml"))


            'Dim fileInfo As File
            'File = New File
            'Response.Clear()
            'Response.ClearHeaders()
            'Response.ClearContent()
            'Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.MapPath("~") & "\FacturasTEMP\" & CStr(dtXML.Rows(0)("ruta")).Replace(".jpg", ".xml"))
            'Response.AddHeader("Content-Length", File.Length.ToString())
            'Response.ContentType = "text/plain"
            'Response.Flush()
            'Response.TransmitFile(File.FullName)
            'Response.End()
        Catch ex As Exception

        End Try


    End Sub



    Public Sub fnDescargarPDF(Docentry As Int64)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objdatos.fnLog("Al imprimir", "Antes RPT:" & Server.MapPath("~") & "\Factura.rpt")
        reporte.Load(Server.MapPath("~") & "\Factura.rpt")

        objdatos.fnLog("Al imprimir docentry ", Convert.ToInt32(Session("DocEntry")))
        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            If objdatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objdatos.fnObtenerCliente.ToUpper.Contains("PMK") Or objdatos.fnObtenerCliente.ToUpper.Contains("SUJEAU") Or objdatos.fnObtenerCliente.ToUpper.Contains("MANIJ") Then
                reporte.SetParameterValue("DocKey@", Convert.ToInt32(Session("DocEntry")))
            Else
                reporte.SetParameterValue("@DocEntry", Session("DocEntry"))
            End If

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
        Try
            Dim docnum As Int64 = Docentry
            If objdatos.fnObtenerDBMS.ToUpper = "HANA" Then
                ssql = "SELECT ""DocNum"" from ""#BDSAP#"".""OINV"" where ""DocEntry""=" & "'" & Docentry & "'"
            Else
                ssql = "SELECT docnum from OINV where docentry=" & "'" & Docentry & "'"
            End If

            Dim dtDocnum As New DataTable
            dtDocnum = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtDocnum.Rows.Count > 0 Then
                docnum = dtDocnum.Rows(0)(0)
            End If
            reporte.Refresh()
            If objdatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objdatos.fnObtenerCliente.ToUpper.Contains("PMK") Or objdatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Or objdatos.fnObtenerCliente.ToUpper.Contains("MANIJ") Then
                reporte.SetParameterValue("DocKey@", Docentry)
            Else
                reporte.SetParameterValue("@DocEntry", Docentry)
            End If

            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "FE-" & docnum)
            reporte.Dispose()

        Catch ex As Exception
            objdatos.fnLog("Exportar ERROR:", ex.Message.Replace("'", ""))
        End Try

    End Sub
    Public Sub fnLlenarListado()
        ssql = "select ciNoPedido as No,cvCveCliente as CveCliente,cvCliente as Cliente,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios,cvNumSAP as [#SAP],'100' as DocTotal ,'1800' docTotalFC from Tienda.Pedido_hdr WHERE cvTipoDoc='COTIZACION' and ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' AND cvEstatus<>'CERRADO'"
        ssql = objdatos.fnObtenerQuery("FacturasB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=FacturasB2Bdetalle'></i></td>"

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
    Public Sub fnLlenarcolumnas()

        ssql = objdatos.fnObtenerQuery("FacturasB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
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
            If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Or CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("PMK") Or CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("AUTOPARTES IMPORTADAS") Then
                'sHtml = sHtml & "<tr>"
                'sHtml = sHtml & "  <th colspan=6 rowspan=''></th>"

                'sHtml = sHtml & "   <th colspan='3' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
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
    Public Sub fnVer()
        Dim iNoPedido As Int64 = 0
        iNoPedido = Request.QueryString("Id")
        If iNoPedido = 0 Then
            objdatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ''Determinar como se va a desplegar el detalle del documento

        End If
    End Sub
End Class
