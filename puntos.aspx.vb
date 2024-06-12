
Imports System.Data
Imports System.IO
Partial Class puntos
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub puntos_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnLlenarcolumnas()
        fnLlenarListado()

        ssql = "EXEC GETPUNTOS '[%0]'"
        ssql = ssql.Replace("[%0]", Session("Cliente"))
        Dim dtPuntos As New DataTable
        dtPuntos = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtPuntos.Rows.Count = 0 Then
            lblPuntos.Text = "Puntos acumulados: 0"
        Else
            lblPuntos.Text = "Puntos acumulados: " & dtPuntos.Rows(0)(0)
        End If

    End Sub
    Public Sub fnLlenarListado()
        ssql = "select ciNoPedido as No,cvCveCliente as CveCliente,cvCliente as Cliente,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios,cvNumSAP as [#SAP],'100' as DocTotal ,'1800' docTotalFC from Tienda.Pedido_hdr WHERE cvTipoDoc='COTIZACION' and ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' AND cvEstatus<>'CERRADO'"
        ssql = objdatos.fnObtenerQuery("PuntosB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        ssql = ssql.Replace("SELECT", " SELECT TOP 300 ")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            '            sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=FacturasB2Bdetalle'></i></td>"

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

        ssql = objdatos.fnObtenerQuery("PuntosB2B")
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


        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtclienteSeg As New DataTable
        dtclienteSeg = objdatos.fnEjecutarConsulta(ssql)
        If dtclienteSeg.Rows.Count > 0 Then
            If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Then
                'sHtml = sHtml & "<tr>"
                'sHtml = sHtml & "  <th colspan=6 rowspan=''></th>"

                'sHtml = sHtml & "   <th colspan='3' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
                'sHtml = sHtml & "</tr>"
            Else
                sHtml = sHtml & "<tr>"
                sHtml = sHtml & "  <th colspan=" & iContador & " rowspan=''></th>"
                '  sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-verde txt-center'>" & sMonedaLocal & "</th>"
                ' sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
                sHtml = sHtml & "</tr>"
            End If
        End If

        sHtml = sHtml & "<tr>"
        '    sHtml = sHtml & "<th style='width:30px;'>ver</th>"

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
End Class
