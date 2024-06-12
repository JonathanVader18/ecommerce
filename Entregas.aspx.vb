
Imports System.Data

Partial Class Entregas
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Private Sub Entregas_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnLlenarcolumnas()
        fnLlenarListado()
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("action") = "v" Then 'Ver
                fnVer()
            End If
        End If

        ssql = "SELECT cvNombre from config.menus where cvLink='Entregas.aspx'"
        Dim dtTitulo As New DataTable
        dtTitulo = objdatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            lblTitulo.Text = dtTitulo.Rows(0)(0)
        End If
    End Sub
    Public Sub fnLlenarListado()

        ssql = objdatos.fnObtenerQuery("EntregasB2B")
        Dim dtDocumentos As New DataTable
        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
        dtDocumentos = objdatos.fnEjecutarConsultaSAP(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            If CStr(objdatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then

            Else
                sHtml = sHtml & "<td class='mas'><i class='fa fa-chevron-right preview-popup' aria-hidden='true' href='factura-modal.aspx?Doc=" & dtDocumentos.Rows(i)(0) & "&QueryDoc=EntregasB2Bdetalle'></i></td>"

            End If

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

        ssql = objdatos.fnObtenerQuery("EntregasB2B")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", "'" & Session("slpCode") & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
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


        sHtml = sHtml & "<tr>"

        sHtml = sHtml & "  <th colspan=" & iContador & " rowspan=''></th>"
        sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-verde txt-center'>" & sMonedaLocal & "</th>"
        If objdatos.fnObtenerCliente().ToUpper.Contains("PMK") Then

        Else
            sHtml = sHtml & "   <th colspan='" & iCuantos & "' class='tdh-c-azul txt-center'>" & sMonedaExtranjera & "</th>"
        End If


        sHtml = sHtml & "</tr>"

        sHtml = sHtml & "<tr>"
        If CStr(objdatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then

        Else
            sHtml = sHtml & "<th style='width:30px;'>ver</th>"
        End If


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
