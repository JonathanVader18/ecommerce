
Imports System.Data

Partial Class detalle_carrito
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub detalle_carrito_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString.Count > 0 Then
            Dim iPedido As Int64
            iPedido = Request.QueryString("Ped")

            Dim sHtmlEncabezado As String = ""
            Dim sHtmlBanner As String = ""
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            Dim sSubTotal As Double = 0
            Try
                ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & iPedido & "'"
                Dim dtPartidas As New DataTable
                dtPartidas = objdatos.fnEjecutarConsulta(ssql)

                For x = 0 To dtPartidas.Rows.Count - 1 Step 1

                    sHtmlBanner = sHtmlBanner & " <tr>"
                    '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objdatos.fnObtenerQuery("Info-Producto")
                            ssql = ssql.Replace("[%0]", "'" & dtPartidas.Rows(x)("cvItemCode") & "'")
                            Dim dtGeneral As New DataTable
                            dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <td> "
                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                sHtmlBanner = sHtmlBanner & "</td>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                Else
                                    '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <td data-title='Articulo'>" & sCampos & "</td>"

                    End If
                    Dim precio As Double = 0
                    precio = objdatos.fnPrecioActual(dtPartidas.Rows(x)("cvItemCode"))
                    sHtmlBanner = sHtmlBanner & " <td data-title='precio' class='text-center' >" & precio.ToString("$ ###,###,###.#0") & "</td>"
                    sHtmlBanner = sHtmlBanner & " <td data-title='Fecha' class='text-center' >" & CDbl(dtPartidas.Rows(x)("cfCantidad")).ToString(" ###,###,###.#0") & "</td>"
                    sHtmlBanner = sHtmlBanner & " <td data-title='precio' class='text-center' >" & (CDbl(dtPartidas.Rows(x)("cfCantidad")) * precio).ToString("$ ###,###,###.#0") & "</td>"

                    sHtmlBanner = sHtmlBanner & "</tr>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                    sSubTotal = sSubTotal + (CDbl(dtPartidas.Rows(x)("cfCantidad")) * precio)
                    ''Aqui van los botones de Action Cart
                    'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                    ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                    ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                    '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                    ''sHtmlBanner = sHtmlBanner & "</div>"

                    'sHtmlBanner = sHtmlBanner & " </div> "


                Next
            Catch ex As Exception
                '   Response.Redirect("index.aspx")
            End Try

            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
            Dim literal As New LiteralControl(sHtmlEncabezado)
            pnlPartidas.Controls.Clear()
            pnlPartidas.Controls.Add(literal)
            Session("ImporteSubTotal") = sSubTotal
            lblSubtotal.Text = CDbl(Session("ImporteSubTotal")).ToString("$ ###,###,###.#0")
            If Session("ImporteDescuento") = 0 Then
                lblDescuento.Text = ""
            Else
                lblDescuento.Text = CDbl(Session("ImporteDescuento")).ToString("$ ###,###,###.#0")
            End If


            If Session("ImporteEnvio") = 0 Then
                lblenvio.Text = ""
            Else
                lblenvio.Text = CDbl(Session("ImporteEnvio")).ToString("$ ###,###,###.#0")
            End If
            lblTotal.Text = (sSubTotal + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString("$ ###,###,###.#0")




            ssql = "select (select TOP 1 cvItemCode  from Tienda.Pedido_Det where ciNoPedido =T0.ciNoPedido) as Artículo, ciNoPedido as Orden ,cdFecha as Fecha,cvEstatus as Estatus,cfTotal as Total  from tienda.Pedido_Hdr T0 WHERE " _
              & " ciNoPedido=" & "'" & iPedido & "'"
            Dim dtPedidos = New DataTable
            dtPedidos = objdatos.fnEjecutarConsulta(ssql)
            If dtPedidos.Rows.Count > 0 Then
                lblenvio.Text = ""
                lblFecha.Text = "Fecha del pedido: " & CDate(dtPedidos.Rows(0)("Fecha")).ToShortDateString
                lblEstatus.Text = "Estatus: Guardado"
                lblOrden.Text = "Orden: " & iPedido
                lblRastreo.Text = ""
            End If


        End If
    End Sub
    Protected Sub btnCargarCarrito_Click(sender As Object, e As EventArgs) Handles btnCargarCarrito.Click
        Dim dtPartidas As New DataTable
        Dim iPedido As Int64
        iPedido = Request.QueryString("Ped")
        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & iPedido & "'"
        dtPartidas = objdatos.fnEjecutarConsulta(ssql)


        Dim iLinea As Int16 = 0
        For i = 0 To dtPartidas.Rows.Count - 1 Step 1
            Dim partida As New Cls_Pedido.Partidas
            Dim objDatos As New Cls_Funciones
            partida.ItemCode = dtPartidas.Rows(i)("cvitemCode")
            partida.Cantidad = dtPartidas.Rows(i)("cfCantidad")
            Dim dPrecioActual As Double = 0
            If CInt(Session("slpCode")) <> 0 Then

                dPrecioActual = objDatos.fnPrecioActual(dtPartidas.Rows(i)("cvitemCode"), Session("ListaPrecios"))
            Else
                dPrecioActual = objDatos.fnPrecioActual(dtPartidas.Rows(i)("cvitemCode"))
            End If

            partida.Precio = dPrecioActual
            partida.TotalLinea = partida.Cantidad * partida.Precio

            ''Ahora el itemName
            Try
                partida.ItemName = dtPartidas.Rows(i)("cvitemName")
            Catch ex As Exception
            End Try


            Session("Partidas").add(partida)
        Next

        lblEstatus.Visible = True
        lblEstatus.Text = " Se han cargado los artículos al carrito "
        objdatos.Mensaje(" Se han cargado los artículos al carrito ", Me.Page)

    End Sub
End Class
