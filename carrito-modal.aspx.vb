
Imports System.Data
Imports System.IO

Partial Class carrito_modal
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub carrito_modal_Load(sender As Object, e As EventArgs) Handles Me.Load

        fnCargaCarrito()
    End Sub
    Public Sub fnCargaCarrito()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Try
            ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Menu' order by T1.ciOrden "
            Dim dtCamposPlantilla As New DataTable
            dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
            Dim sImagen As String = "ImagenPal"
            Dim iCartContent As Int16 = 0
            Dim iContador As Int16 = 0

            Dim sHtmlImagen As String = ""
            Dim sHtmlPRecio As String = ""
            Dim sHtmlCantidad As String = ""
            Dim sHtmlAtributos As String = ""
            Dim sCampos As String = ""
            Dim dtTallasColores As New DataTable
            Dim sTallaColor As String = "NO"

            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If
            End If
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    sHtmlBanner = sHtmlBanner & " <li> "
                    sHtmlBanner = sHtmlBanner & "  <div class='div-sdiviped'>"
                    sHtmlBanner = sHtmlBanner & "   <div class='row-cart'>"
                    If dtCamposPlantilla.Rows.Count > 0 Then

                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1


                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            Dim precio As Double = 0
                            If sTallaColor = "SI" Then
                                objDatos.fnLog("Carga carrito Talla color SI ", Partida.Generico)



                                Dim dtExiste As New DataTable
                                Dim ssqlExiste As String = ssql.Replace("[%0]", "'" & Partida.Generico & "'")
                                dtExiste = objDatos.fnEjecutarConsultaSAP(ssqlExiste)
                                If dtExiste.Rows.Count = 0 Then

                                    ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                Else
                                    ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                                End If
                                objDatos.fnLog("Carga carrito Talla color SI ", ssql.Replace("'", ""))
                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")


                            End If

                            'ssql = objDatos.fnObtenerQuery("Info-Producto")
                            'ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)



                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then

                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        'objDatos.fnlog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            If dtFoto.Rows(0)(0) <> "" Then
                                                sHtmlImagen = sHtmlImagen & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                            Else
                                                sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                            End If

                                        Else
                                            sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                        End If
                                        sHtmlBanner = sHtmlBanner & " <div class='image-cart text-center'> <a href='producto-interior.aspx?Code=" & Partida.Generico & "'>"
                                        sHtmlBanner = sHtmlBanner & sHtmlImagen
                                        sHtmlBanner = sHtmlBanner & "</a></div>"
                                    Else
                                        Dim iband As Int16 = 0
                                        sHtmlBanner = sHtmlBanner & " <div class='image-cart text-center'> <a href='producto-interior.aspx?Code=" & Partida.ItemCode & "'>"
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & ".jpg") Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & ".jpg" & "'  alt='productos' title='productos' class='img-thumbnail'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-1.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-1.jpg" & "'  alt='productos' title='productos' class='img-thumbnail'>"
                                            iband = 1
                                        End If

                                        If iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                        End If

                                        sHtmlBanner = sHtmlBanner & "</a></div>"
                                    End If
                                End If


                            Else
                                If iCartContent = 0 Then
                                    iCartContent = 1
                                    sHtmlBanner = sHtmlBanner & "<div class='cart-button text-center'>"
                                End If
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                        sHtmlBanner = sHtmlBanner & " <div class='product-name text-left'> <a href='#'>" & Partida.ItemName & "</a></div>"
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    Else
                                        sHtmlBanner = sHtmlBanner & " <div class='product-name text-left'> <a href='#'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</a></div>"
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If



                                Else
                                    Dim dPrecioActual As Double
                                    If Partida.Precio = 0 Then

                                        If CInt(Session("slpCode")) <> 0 Then

                                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                                        Else
                                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                                        End If

                                        If Session("Cliente") <> "" Then
                                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                                        End If
                                    Else
                                        dPrecioActual = Partida.Precio
                                    End If
                                    sHtmlBanner = sHtmlBanner & " <strong class='text-right'>x-" & Partida.Cantidad & " </strong>"
                                    sHtmlBanner = sHtmlBanner & " <span class='cart-price text-right'>$" & dPrecioActual.ToString("###,###,###.#0") & "</span>"
                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & "</div>"
                        iCartContent = 0
                    End If



                    sHtmlBanner = sHtmlBanner & " </div></li> "
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        Catch ex As Exception
            objDatos.fnLog("ex modal", ex.Message.Replace("'", ""))
            Response.Redirect("index.aspx")
        End Try


        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlCarrito.Controls.Clear()
        pnlCarrito.Controls.Add(literal)

    End Sub
End Class
