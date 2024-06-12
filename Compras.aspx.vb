Imports System.Data
Partial Class Compras
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub Compras_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnLlenarListado()
        If Request.QueryString.Count > 0 Then
            If Request.QueryString("action") = "v" Then 'Ver
                fnVer()
            End If
        End If
    End Sub

    Public Sub fnLlenarListado()

        ssql = objDatos.fnObtenerQuery("ComprasPendientes")
        Dim dtDocumentos As New DataTable

        ssql = ssql.Replace("[%0]", Session("slpCode"))
        dtDocumentos = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtml As String = ""
        For i = 0 To dtDocumentos.Rows.Count - 1 Step 1
            sHtml = sHtml & "<tr>"
            sHtml = sHtml & "<td class='mas'><a href='compras.aspx?Id=" & dtDocumentos.Rows(i)("No") & "&action=v'><i class='fa fa-chevron-right' aria-hidden='true'></i></a></td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("Fecha") & "</td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("No") & "</td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("CveCliente") & "</td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("Cliente") & "</td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("No") & "</td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("DocTotal") & "</td>"
            sHtml = sHtml & "<td>" & dtDocumentos.Rows(i)("docTotalFC") & "</td>"
            sHtml = sHtml & "<td><a href='compras.aspx?Id=" & dtDocumentos.Rows(i)("No") & "&action=v'><i class='fa fa-arrow-circle-down' aria-hidden='true'></i></a></td>" ''Abrir

            sHtml = sHtml & "</tr>"
        Next

        Dim literal As New LiteralControl(sHtml)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub

    Public Function fnProcesarSAP(idCarrito As Int64, TipoDoc As String)
        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus FROM Tienda.Pedido_HDR WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objDatos.fnEjecutarConsulta(ssql)

        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & idCarrito & "'"
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
                    objDatos.Mensaje(TipoDoc & " procesado correctamente ", Me.Page)
                End If


            Else
                objDatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)
            End If

        Catch ex As Exception
            objDatos.Mensaje("No se ha podido establecer conexión con SAP", Me.Page)
        End Try

    End Function

    Protected Sub btnCotizacion_Click(sender As Object, e As EventArgs) Handles btnCotizacion.Click
        Dim iNoPedido As Int64 = 0

        iNoPedido = Request.QueryString("Id")
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            fnProcesarSAP(iNoPedido, "COTIZACION")
            fnLlenarListado()
            rgvPartidas.Visible = False
        End If

    End Sub
    Protected Sub btnPedido_Click(sender As Object, e As EventArgs) Handles btnPedido.Click
        Dim iNoPedido As Int64 = 0

        iNoPedido = Request.QueryString("Id")
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            fnProcesarSAP(iNoPedido, "PEDIDO")
            fnLlenarListado()
            rgvPartidas.Visible = False
        End If


    End Sub
    Protected Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
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
            btnPedido.Visible = True
            btnCerrar.Visible = True
            btnCotizacion.Visible = True
        End If
    End Sub
    Protected Sub btnCerrar_Click(sender As Object, e As EventArgs) Handles btnCerrar.Click
        Dim iNoPedido As Int64 = 0

        iNoPedido = Request.QueryString("Id")
        If iNoPedido = 0 Then
            objDatos.Mensaje("Debe elegir primero un documento de la lista", Me.Page)
            Exit Sub
        Else
            ssql = "UPDATE Tienda.Pedido_HDR  SET cvEstatus='CERRADO' WHERE ciNoPedido=  " & "'" & iNoPedido & "'"
            objDatos.fnEjecutarInsert(ssql)
            objDatos.Mensaje("Se ha cerrado el documento", Me.Page)
            fnLlenarListado()
            rgvPartidas.Visible = False
        End If
    End Sub
End Class
