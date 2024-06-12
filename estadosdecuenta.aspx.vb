
Imports System.Data
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class estadosdecuenta
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub estadosdecuenta_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnLlenarcolumnas()
        fnLlenarListado()
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtclienteSeg As New DataTable
        dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Then
                btnImprimir.Visible = True
            End If
        End If
    End Sub

    Public Sub fndescargaPDF()
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables




        Try
            objDatos.fnLog("Al imprimir", "Antes RPT")
            reporte.Load(Server.MapPath("~") & "\estadocuenta.rpt")

            Dim ssql As String
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                reporte.SetParameterValue("Cliente", Session("Cliente"))
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

            objDatos.fnLog("Al imprimir edocuenta", "cliente:" & Session("Cliente"))
            reporte.SetParameterValue("Cliente", Session("Cliente"))
            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "EdoCuenta-" & Session("Cliente"))
            reporte.Dispose()
        Catch ex As Exception
            objDatos.fnLog("Al imprimir ex ", ex.Message.Replace("'", ""))
        End Try


        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub
    Public Sub fnLlenarcolumnas()

        ssql = objDatos.fnObtenerQuery("EstadosdeCuentaB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
        dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)


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

        ''Preparamos los encabezados
        Dim iContador As Int16 = 0
        For i = 0 To dtDocumentos.Columns.Count - 1 Step 1
            If dtDocumentos.Columns(i).ColumnName.ToUpper.Contains("NACIONAL") Then
                iContador = i + 1
                Exit For
            End If
        Next

        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtclienteSeg As New DataTable
        dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Then

            Else
                sHtml = sHtml & "<tr>"
                sHtml = sHtml & "  <th colspan=" & iContador & " rowspan=''></th>"
                sHtml = sHtml & "   <th colspan='3' class='tdh-c-verde txt-center'>" & sMonedaLocal & "</th>"
                sHtml = sHtml & "   <th colspan='3' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
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

        Dim literal As New LiteralControl(sHtml)
        pnlColumnas.Controls.Clear()
        pnlColumnas.Controls.Add(literal)
    End Sub

    Public Sub fnLlenarListado()

        ssql = objDatos.fnObtenerQuery("EstadosdeCuentaB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
        dtDocumentos = objDatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true'  href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=EstadosDeCuentaB2Bdetalle'></i></td>"

            For x = 0 To dtDocumentos.Columns.Count - 1 Step 1
                If dtDocumentos.Columns(x).ColumnName.Contains("Extranjera") Or dtDocumentos.Columns(x).ColumnName.Contains("Nacional") Then
                    sHtml = sHtml & "<td>" & CDbl(dtDocumentos.Rows(i)(x)).ToString("###,###,###,###.#0") & "</td>"
                Else
                    sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)(x) & "</td>"
                End If

            Next

            sHtml = sHtml & "</tr>"



        Next

        Dim literal As New LiteralControl(sHtml)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        fndescargaPDF()
    End Sub
End Class
