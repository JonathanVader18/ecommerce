
Imports System.Data
Imports System.IO

Partial Class Listadeseos
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub Listadeseos_Load(sender As Object, e As EventArgs) Handles Me.Load


        If Session("Cliente") <> "" Then
            Response.Redirect("levantar-pedido.aspx")
        End If
        ''Revisamos si no viene un action
        If Request.QueryString.Count > 0 Then
            For Each Partida As Cls_Pedido.Partidas In Session("WishList")
                If Partida.ItemCode = Request.QueryString("item") And Request.QueryString("Action") = "d" Then
                    Partida.ItemCode = "BORRAR"
                    Response.Redirect("Listadeseos.aspx")
                End If
                If Partida.ItemCode = Request.QueryString("item") And Request.QueryString("Action") = "m" Then
                    ''Lo agregamos al carrito
                    ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre,ISNULL(cvcliente,'') as Cliente from config .DatosCliente  "
                    Dim dtcliente As New DataTable
                    dtcliente = objDatos.fnEjecutarConsulta(ssql)
                    If dtcliente.Rows.Count > 0 Then

                        If CStr(dtcliente.Rows(0)(1)).Contains("Salama") Then
                            Response.Redirect("producto-interior.aspx?Code=" & Request.QueryString("item"))
                        Else
                            Session("Partidas").add(Partida)
                            objDatos.Mensaje("Agregado al carrito", Me.Page)
                        End If
                    End If


                End If
            Next
        End If


        ''Cargamos lo que haya en la lista de deseos

        ''Preparamos el encabezado del Grid
        Dim sHtmlBanner As String = ""
        Dim sHtmlEncabezado As String = ""

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Try
            For Each Partida As Cls_Pedido.Partidas In Session("WishList")
                If Partida.ItemCode <> "BORRAR" Then
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)



                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then

                                Dim iband As Int16 = 0
                                If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode.Replace("-", "") & ".jpg") Then
                                    sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "
                                    sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & "" & Partida.ItemCode.Replace("-", "") & ".jpg" & "' class='img-responsive'>"
                                    sHtmlBanner = sHtmlBanner & "</div>"
                                    iband = 1
                                End If
                                If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-1.jpg") And iband = 0 Then
                                    sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "
                                    sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & "" & Partida.ItemCode.Replace("-", "") & "-1.jpg" & "' class='img-responsive'>"
                                    sHtmlBanner = sHtmlBanner & "</div>"
                                    iband = 1
                                End If
                                If iband = 0 Then
                                    sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                    sHtmlBanner = sHtmlBanner & "</div>"
                                End If

                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 info-producto'> " & sCampos & "</div>"

                    End If
                    ' sHtmlBanner = sHtmlBanner & "</div>"

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-precio'><div class='precio sec-prec promo'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div class='precio sec-prec promo'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-total'><div class='precio sec-prec promo'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div></div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart
                    sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='listadeseos.aspx?item=" & Partida.ItemCode & "&Action=d'>Quitar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='listadeseos.aspx?item=" & Partida.ItemCode & "&Action=m'>Mover a carrito</a></div>"
                    sHtmlBanner = sHtmlBanner & "</div>"

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If

            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try



        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlLista.Controls.Clear()
        pnlLista.Controls.Add(literal)
    End Sub
End Class
