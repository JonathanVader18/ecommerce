
Imports System.Data

Partial Class detalle_pedido
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub detalle_pedido_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim NoPedido As String = Request.QueryString("Ped")



            ''Cargamos el carrito
            Dim sHtmlBanner As String = ""
            Dim sHtmlEncabezado As String = ""

            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            Dim sSubTotal As Double = 0
            Dim sEnvio As Double = 0
            Dim sDescuento As Double = 0
            Dim sTotal As Double = 0
            Dim sCampos As String = ""
            Try
                ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & NoPedido & "'"
                Dim dtPartidas As New DataTable
                dtPartidas = objDatos.fnEjecutarConsulta(ssql)

                For x = 0 To dtPartidas.Rows.Count - 1 Step 1
                    sCampos = ""
                    sHtmlBanner = sHtmlBanner & " <tr>"
                    '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then

                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            ssql = ssql.Replace("[%0]", "'" & dtPartidas.Rows(x)("cvItemCode") & "'")
                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

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
                    precio = objDatos.fnPrecioActual(dtPartidas.Rows(x)("cvItemCode"))
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
              & " ciNoPedido=" & "'" & NoPedido & "'"
            Dim dtPedidos = New DataTable
            dtPedidos = objDatos.fnEjecutarConsulta(ssql)
            If dtPedidos.Rows.Count > 0 Then
                lblenvio.Text = ""
                lblFecha.Text = "Fecha del pedido: " & CDate(dtPedidos.Rows(0)("Fecha")).ToShortDateString
                lblEstatus.Text = "Estatus: Guardado"
                lblOrden.Text = "Orden: " & NoPedido
                lblRastreo.Text = ""
            End If
            ''Ahora el envio
            ssql = "  select cvCalle,cvColonia,cvNumExt,cvCiudad,cvEstado,cvPais from tienda.Pedido_Envio  where ciNoPedido='" & NoPedido & "'"
            Dim dtenvio As New DataTable
            dtenvio = objDatos.fnEjecutarConsulta(ssql)
            If dtenvio.Rows.Count > 0 Then
                lblEnviado.Text = "Enviado a :" & Session("NombreuserTienda")
                lblCalle.Text = dtenvio.Rows(0)("cvCalle") & " " & dtenvio.Rows(0)("cvNumExt")
                lblColoniaCiudad.Text = dtenvio.Rows(0)("cvCiudad") & " " & dtenvio.Rows(0)("cvEstado")
                lblPaisCP.Text = dtenvio.Rows(0)("cvPais")
            End If


        End If
    End Sub
End Class
