Imports System.Data
Partial Class club_del_vino
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub club_del_vino_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("Page") = "club-del-vino.aspx"
        ''Obtenemos el título de la página
        ssql = "select cvNombre from config.Menus where cvLink ='club-del-vino.aspx' and cvTipoMenu ='Header'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            lblTitulo.Text = dtTitulo.Rows(0)(0)
        End If
        fnCargaPagina()
        fnCargaProductos()
    End Sub
    Public Sub fnCargaPagina()
        ssql = "select cvHtml, ISNULL(cvImagenBanner,'') as Banner from config.Paginas where cvNombre ='" & lblTitulo.Text & "'"
        Dim dtTitulo As New DataTable
        dtTitulo = objDatos.fnEjecutarConsulta(ssql)
        If dtTitulo.Rows.Count > 0 Then
            Dim sHTMLEncabezado As String = dtTitulo.Rows(0)(0)
            Dim literal As New LiteralControl(sHTMLEncabezado)
            pnlContenido.Controls.Clear()
            pnlContenido.Controls.Add(literal)

            If dtTitulo.Rows(0)("Banner") <> "" Then
                sHTMLEncabezado = "<div class=""pag-cont-banner"" style=""background-image: url('" & dtTitulo.Rows(0)("Banner") & "')""></div>"
                literal = New LiteralControl(sHTMLEncabezado)
                pnlBanner.Controls.Clear()
                pnlBanner.Controls.Add(literal)
            End If
        End If
    End Sub

    Public Sub fnCargaProductos()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        Dim dtArticulosPromo As New DataTable
        ssql = objDatos.fnObtenerQuery("ClubVino")
        dtArticulosPromo = objDatos.fnEjecutarConsultaSAP(ssql)

        If dtArticulosPromo.Rows.Count > 0 Then
            sHtmlEncabezado = sHtmlEncabezado & "<div class='wrappercon naranja'>"
            sHtmlEncabezado = sHtmlEncabezado & " <div class='seccion'>"
            sHtmlEncabezado = sHtmlEncabezado & "  <div class='main-container'>"
            sHtmlEncabezado = sHtmlEncabezado & "   <span class='linea top'></span>"
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='sec-tit'></div>"
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='feature-1'>"

            For y = 0 To dtArticulosPromo.Rows.Count - 1 Step 1
                '  objDatos.fnLog("Entra Barras2", dtArticulosPromo.Rows.Count)

                Dim sImagen As String = "ImagenPal"
                sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 col-sm-6 col-md-3 no-padding c-product'>"
                sHtmlBanner = sHtmlBanner & "<a href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "'>"
                sHtmlBanner = sHtmlBanner & " <div class='preview'>"
                sHtmlBanner = sHtmlBanner & "  <div class='img'>"
                sHtmlBanner = sHtmlBanner & "   <img src='" & sImagen & "' class='img-responsive'>"
                If objDatos.fnArticuloOferta(dtArticulosPromo.Rows(y)("ItemCode")) = "SI" Then
                    sHtmlBanner = sHtmlBanner & "    <span class='b-oferta'>" & "oferta" & "</span>"
                End If

                Dim sPintaPrev As String = "SI"
                Dim sPintaFav As String = "SI"
                Dim sPintaCompra As String = "SI"

                ssql = "select ISNULL(cvMenuCatalogo,'SI') as Menu,ISNULL(cvPrevDetalle,'')Interior,ISNULL(cvPrevFavorito,'')Favorito,ISNULL(cvPrevCompra,'')Comprar from [config].[Parametrizaciones_Plantilla]"
                Dim dtPintaCat As New DataTable
                dtPintaCat = objDatos.fnEjecutarConsulta(ssql)
                If dtPintaCat.Rows.Count > 0 Then
                    sPintaPrev = dtPintaCat.Rows(0)("Interior")
                    sPintaFav = dtPintaCat.Rows(0)("Favorito")
                    sPintaCompra = dtPintaCat.Rows(0)("Comprar")
                End If

                If sPintaCompra = "SI" Or sPintaPrev = "SI" Or sPintaFav = "SI" Then
                    sHtmlBanner = sHtmlBanner & "     <div class='action-products'>"
                Else
                    sHtmlBanner = sHtmlBanner & "     <div>"
                End If

                If sPintaCompra = "SI" Then
                    sHtmlBanner = sHtmlBanner & "      <a class='item view' href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "'></a>"
                End If
                If sPintaFav = "SI" Then
                    sHtmlBanner = sHtmlBanner & "      <a class='item favorite preview-popup' href='elegir-favoritos.aspx?code=" & dtArticulosPromo.Rows(y)("ItemCode") & "&name=" & dtArticulosPromo.Rows(y)("ItemName") & "'></a>"
                End If
                If sPintaCompra = "SI" Then
                    sHtmlBanner = sHtmlBanner & "      <a class='item bag preview-popup' href='preview-popup.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "&Modo=Add'></a>"
                End If



                sHtmlBanner = sHtmlBanner & "     </div>"
                sHtmlBanner = sHtmlBanner & "  </div>"



                sHtmlBanner = sHtmlBanner & "  <a class='info'href='producto-interior.aspx?Code=" & dtArticulosPromo.Rows(y)("ItemCode") & "'>"
                ''Nos traemos los datos a mostrar de acuerdo a la plantilla "PROMO"
                ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='BARRAS' order by T1.ciOrden "
                Dim dtCamposPlantilla As New DataTable
                dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)

                For x = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1
                    ' objDatos.fnLog("Entra Barras2 campos plantilla", dtCamposPlantilla.Rows(x)("Campo"))
                    '  objDatos.fnLog("Entra Barras2 campos plantilla", dtCamposPlantilla.Rows.Count)
                    If dtCamposPlantilla.Rows(x)("Tipo") = "Cadena" Then
                        Dim sValorMostrar As String = ""
                        sValorMostrar = CStr(dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo")))
                        'If sValorMostrar.Length > 30 Then
                        '    sValorMostrar = sValorMostrar.Substring(0, 30)
                        'End If
                        If dtCamposPlantilla.Rows(x)("Resaltado") = "SI" Then
                            sHtmlBanner = sHtmlBanner & "   <div class='n-product'>" & sValorMostrar & "</div>"
                        Else
                            sHtmlBanner = sHtmlBanner & "   <div class='d-product'>" & sValorMostrar & "</div>"
                        End If
                    Else
                        If dtCamposPlantilla.Rows(x)("Tipo") = "Precio" Then
                            Dim dPrecioActual As Double
                            If CInt(Session("slpCode")) <> 0 Then

                                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("ItemCode"), Session("ListaPrecios"))
                            Else
                                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("ItemCode"))
                            End If
                            If Session("Cliente") <> "" Then
                                dPrecioActual = objDatos.fnPrecioActual(dtArticulosPromo.Rows(y)("ItemCode"), Session("ListaPrecios"))
                            End If
                            sHtmlBanner = sHtmlBanner & "   <span class='p-product'>" & dPrecioActual.ToString("$ ###,###,###.#0") & "</span>"
                        Else
                            If dtCamposPlantilla.Rows(x)("Tipo") = "Imagen" Then
                                If dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo")) Is DBNull.Value Then
                                    sImagen = "images/no-image.png"
                                Else
                                    sImagen = dtArticulosPromo.Rows(y)(dtCamposPlantilla.Rows(x)("Campo"))
                                End If

                                sHtmlBanner = sHtmlBanner.Replace("ImagenPal", sImagen)

                            End If
                        End If
                    End If
                    '  objDatos.fnLog("Entra Barras2 articulo: ", dtArticulosPromo.Rows(y)("ItemCode"))
                Next
                sHtmlBanner = sHtmlBanner & "  </a>"
                sHtmlBanner = sHtmlBanner & " </div>"
                sHtmlBanner = sHtmlBanner & "  </a>"
                sHtmlBanner = sHtmlBanner & "</div>"
            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & "</div></div></div> </div>"

            Dim literal As New LiteralControl(sHtmlEncabezado)
            pnlProductos.Visible = True
            pnlProductos.Controls.Clear()
            pnlProductos.Controls.Add(literal)
        End If







    End Sub
End Class
