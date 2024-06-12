Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Partial Class saldos
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub saldos_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Cargamos
        ssql = "SELECT [cvRango],[ciValorInicial],[ciValorFinal] FROM [SAP_Tienda].[config].[Saldos]"
        Dim dtSaldos As New DataTable
        dtSaldos = objDatos.fnEjecutarConsulta(ssql)
        If dtSaldos.Columns.Count > 0 Then
            dtSaldos.Columns.Add("Cuantos", GetType(Double))
            dtSaldos.Columns.Add("MonedaN", GetType(Double))
            dtSaldos.Columns.Add("MonedaE", GetType(Double))
        End If
        If dtSaldos.Rows.Count > 0 Then
            ''Inicializamos el dataTable
            For x = 0 To dtSaldos.Rows.Count - 1 Step 1
                dtSaldos.Rows(x)("Cuantos") = 0
                dtSaldos.Rows(x)("MonedaN") = 0
                dtSaldos.Rows(x)("MonedaE") = 0
            Next
            ''Obtenemos el query de saldos
            ssql = objDatos.fnObtenerQuery("SaldosB2B")
            Dim dtDocumentos As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
            ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
            dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)


            For x = 0 To dtSaldos.Rows.Count - 1 Step 1
                For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
                    Dim iDiasVencido As Int64
                    iDiasVencido = DateDiff(DateInterval.Day, CDate(dtDocumentos.Rows(i)("Vence")), Now.Date)
                    If iDiasVencido >= dtSaldos.Rows(x)("ciValorInicial") And iDiasVencido <= dtSaldos.Rows(x)("ciValorFinal") Then
                        dtSaldos.Rows(x)("Cuantos") = CInt(dtSaldos.Rows(x)("Cuantos")) + 1
                        dtSaldos.Rows(x)("MonedaN") = CDbl(dtSaldos.Rows(x)("MonedaN")) + CDbl(dtDocumentos.Rows(i)("SaldoNacional"))
                        ' dtSaldos.Rows(x)("MonedaE") = CDbl(dtSaldos.Rows(x)("MonedaE")) + CDbl(dtDocumentos.Rows(i)("SaldoExtranjera"))

                    End If
                Next
            Next
            fnCargaTablero(dtSaldos)

        End If
        fnLlenarcolumnas()
        fnLlenarListado()
    End Sub
    Public Sub fnCargaTablero(dtSaldos As DataTable)

        ssql = objDatos.fnObtenerQuery("MonedasConf")
        Dim dtMonedas As New DataTable
        dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
        Dim sMonedaLocal As String = "M.N."
        Dim sMonedaExtranjera As String = "M.E."
        If dtMonedas.Rows.Count > 0 Then
            sMonedaLocal = dtMonedas.Rows(0)("MainCurncy")
            sMonedaExtranjera = dtMonedas.Rows(0)("SysCurrncy")
        End If

        Dim sHtml As String = ""

        ''El tablero con lo vencido (Rojo)

        Dim iTotalVencidoMN As Double = 0
        Dim iTotalVencidoME As Double = 0
        For i = 0 To dtSaldos.Rows.Count - 1 Step 1
            iTotalVencidoMN = iTotalVencidoMN + CDbl(dtSaldos.Rows(i)("MonedaN"))
            iTotalVencidoME = iTotalVencidoME + CDbl(dtSaldos.Rows(i)("MonedaE"))
        Next
        sHtml = sHtml & " <div class='col-mn-12 col-xs-6 col-sm-3 col-md-3 s-sta-h danger no-padding'> "
        sHtml = sHtml & "   <div class='title'>Vencidos<i class='fa fa-chevron-down'></i></div>"
        sHtml = sHtml & "   <div class='f-sta-input b-r-white'> "
        sHtml = sHtml & "    <div class='col-xs-12 s-sta-input'> "
        sHtml = sHtml & "     <input type='text' name='txtSaldoVencidoM' id='txtSaldoVencidoM' class='form-control text-center' value='$" & iTotalVencidoMN.ToString("###,###,###,###.#0") & "'><span>" & sMonedaLocal & "</span>"
        sHtml = sHtml & "    </div>"
        ' sHtml = sHtml & "    <div class='col-xs-12 s-sta-input'>"
        ' sHtml = sHtml & "     <input type='text' name='txtSaldoVencidoME' id='txtSaldoVencidoME' class='form-control text-center' value='$" & iTotalVencidoME.ToString("###,###,###,###.#0") & "'><span>" & sMonedaExtranjera & "</span>"
        ' sHtml = sHtml & "    </div>"
        sHtml = sHtml & "  </div>"
        sHtml = sHtml & " </div>"

        For i = 0 To dtSaldos.Rows.Count - 1 Step 1
            sHtml = sHtml & " <div class='col-mn-12 col-xs-6 col-sm-3 col-md-3 s-sta-h no-padding'> "
            sHtml = sHtml & "   <div class='title'>" & dtSaldos.Rows(i)("cvRango") & "(" & dtSaldos.Rows(i)("Cuantos") & ")</div>"
            sHtml = sHtml & "   <div class='f-sta-input b-r-white'> "
            sHtml = sHtml & "    <div class='col-xs-12 s-sta-input'> "
            sHtml = sHtml & "     <input type='text' name='txtSaldoM" & (i + 1) & "' id='txtSaldoM" & (i + 1) & "' class='form-control text-center' value='$" & CDbl(dtSaldos.Rows(i)("MonedaN")).ToString("###,###,###,###.#0") & "'><span>" & sMonedaLocal & "</span>"
            sHtml = sHtml & "    </div>"
            ' sHtml = sHtml & "    <div class='col-xs-12 s-sta-input'>"
            ' sHtml = sHtml & "     <input type='text' name='txtSaldoME" & (i + 1) & "' id='txtSaldoME" & (i + 1) & "' class='form-control text-center' value='$" & CDbl(dtSaldos.Rows(i)("MonedaE")).ToString("###,###,###,###.#0") & "'><span>" & sMonedaExtranjera & "</span>"
            'sHtml = sHtml & "    </div>"
            sHtml = sHtml & "  </div>"
            sHtml = sHtml & " </div>"
        Next
        Dim literal As New LiteralControl(sHtml)
        pnlEncabezado.Controls.Clear()
        pnlEncabezado.Controls.Add(literal)

    End Sub

    Public Sub fnLlenarListado()

        ssql = objDatos.fnObtenerQuery("SaldosB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            'sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='preview-popup.aspx'></i></td>"
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=SaldosB2Bdetalle'></i></td>"

            For x = 0 To dtDocumentos.Columns.Count - 1 Step 1
                If dtDocumentos.Columns(x).ColumnName.Contains("Nacional") Then
                    sHtml = sHtml & "<td class='tdh-c-verde txt-center'>" & CDbl(dtDocumentos.Rows(i)(x)).ToString("###,###,###,###.#0") & "</td>"
                Else
                    If dtDocumentos.Columns(x).ColumnName.Contains("Extranjera") Then
                        '      sHtml = sHtml & "<td class='tdh-c-azul txt-center'>" & CDbl(dtDocumentos.Rows(i)(x)).ToString("###,###,###,###.#0") & "</td>"
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

        ssql = objDatos.fnObtenerQuery("SaldosB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
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
            If dtDocumentos.Columns(i).ColumnName.Contains("Nacional") Then
                iContador = i + 1
                Exit For
            End If
        Next

        Dim iCuantos As Int16 = 0
        For i = 0 To dtDocumentos.Columns.Count - 1 Step 1
            If dtDocumentos.Columns(i).ColumnName.Contains("Nacional") Then
                iCuantos = iCuantos + 1

            End If
        Next

        sHtml = sHtml & "<tr>"
        sHtml = sHtml & "  <th colspan=" & iContador & " rowspan=''></th>"
        sHtml = sHtml & "   <th colspan='3' class='tdh-c-verde txt-center'>" & sMonedaLocal & "</th>"
        If objDatos.fnObtenerCliente().ToUpper.Contains("PMK") Or objDatos.fnObtenerCliente().ToUpper.Contains("AIO") Then

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

    Private Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        fnDescargarPDF(Session("Cliente"))
    End Sub
    Public Sub fnDescargarPDF(Cliente As String)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objDatos.fnLog("Al imprimir", "Antes RPT:" & Server.MapPath("~") & "\saldos.rpt")
        reporte.Load(Server.MapPath("~") & "\saldos.rpt")

        objDatos.fnLog("Al imprimir saldo de cliente ", Cliente)
        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            reporte.SetParameterValue("@CardCode", Cliente)

            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If



        objDatos.fnLog("Al imprimir", "Sale de asignarle la BD")
        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        objDatos.fnLog("Al imprimir", "LogInfo")
        ' reporte.Refresh()
        Try

            reporte.Refresh()
            reporte.SetParameterValue("@CardCode", Cliente)
            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "FE-" & Cliente)
            reporte.Dispose()
            GC.Collect()
        Catch ex As Exception
            objDatos.fnLog("Exportar ERROR:", ex.Message.Replace("'", ""))
        End Try

    End Sub
End Class
