
Imports System
Imports System.Data
Imports System.Web.Services

Partial Class Main2
    Inherits System.Web.UI.MasterPage
    Public objDatos As New Cls_Funciones
    Public ssql As String

    Private Sub Main2_Load(sender As Object, e As EventArgs) Handles Me.Load

        Session("slpCode") = "-1"
        Session("NombreuserTienda") = "Empleado de Ventas"
        fnCargaMenuHeader()
        fnCargaMenuFooter()
        fnCargaMenuResponsive()
        fnCargaRedesSociales()

        fnCargaMenuFooterResponsive()
        'fnCargaCategoriasv3()
        fnNewsletter()
        '
        'fnCargaSubCategorias()
        'If CInt(Session("slpCode")) <> 0 Or Session("UserB2C") <> "" Then
        '    pnlListaDeseos.Visible = False
        '    pnlLogin.Visible = False
        '    pnlUsuarioLogin.Visible = True
        '    pnlCliente.Visible = True
        '    If Session("RazonSocial") = "" And Session("NombreuserTienda") <> "" And Session("UserB2C") = "" Then
        '        If Session("RegistraNuevo") = "" Then
        '            ''Un usuario que no seleccionó cliente. Se va a elegir-cliente
        '            '  Response.Redirect("elegir-cliente.aspx")
        '        End If

        '    End If
        '    lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
        '    ' btnCerrarSesion.Visible = True
        '    'pnlOpciones.Visible = True
        '    ' fnCargaMenuUser()
        '    If Session("UserB2C") = "" Then
        '        fnCargaMenuB2B()
        '        fnPlantillasVendedor()
        '        pnlProcesarPago.Visible = False
        '    End If

        'Else
        '    pnlLogin.Visible = True
        'End If
        ''   
        'If Session("Cliente") <> "" And Session("UserB2C") = "" Then
        '    fnCargaMenuB2B()
        '    lblUsuario.Text = Session("RazonSocial")
        '    pnlListaDeseos.Visible = False
        '    pnlLogin.Visible = False
        '    pnlUsuarioLogin.Visible = True
        '    pnlCliente.Visible = True
        '    pnlelegir.Visible = False
        '    pnlProcesarPago.Visible = False
        '    If CInt(Session("slpCode")) <> 0 Then
        '        lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
        '        pnlelegir.Visible = True
        '        fnPlantillasVendedor()
        '    End If
        'End If
        'fnCargaCarrito()
    End Sub

    Public Sub fnNewsletter()
        ''Obtenemos el titulo de promociones
        ssql = "SELECt ISNULL(cvTieneNewLetter,'NO') as Newsletter FROM config .Parametrizaciones "
        Dim dtTituloProm As New DataTable
        dtTituloProm = objDatos.fnEjecutarConsulta(ssql)
        If dtTituloProm.Rows.Count > 0 Then
            If dtTituloProm.Rows(0)(0) = "SI" Then
                pnlnews.Visible = True
            Else
                pnlnews.Visible = False
            End If
        End If

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

            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    iContador = iContador + 1
                    sHtmlBanner = sHtmlBanner & " <li> "
                    sHtmlBanner = sHtmlBanner & "  <div class='div-sdiviped'>"
                    sHtmlBanner = sHtmlBanner & "   <div class='row-cart'>"
                    If dtCamposPlantilla.Rows.Count > 0 Then

                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlImagen = sHtmlImagen & " <div class='image-cart text-center'> <a href='producto-interior.aspx?Code=" & Partida.ItemCode & "'>"
                                sHtmlImagen = sHtmlImagen & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-thumbnail'>"
                                sHtmlImagen = sHtmlImagen & "</a></div>"
                            Else
                                If iCartContent = 0 Then
                                    iCartContent = 1
                                    ' sHtmlBanner = sHtmlBanner & "<div class='cart-content'>"
                                    ' sHtmlBanner = sHtmlBanner & " <div class='cart-button text-center'>"
                                End If
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then
                                    '     sHtmlAtributos = sHtmlAtributos & " <div class='product-name text-left'> <a href='producto-interior.aspx'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</a></div>"
                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                Else
                                    Dim dPrecioActual As Double
                                    If CInt(Session("slpCode")) <> 0 Then

                                        dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                                    Else
                                        dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                                    End If
                                    sHtmlCantidad = sHtmlCantidad & " <strong class='text-right'>x - " & Partida.Cantidad & " </strong> "
                                    sHtmlPRecio = sHtmlPRecio & " <span class='cart-price text-right'>$ " & dPrecioActual.ToString("###,###,###.#0") & "</span>"
                                End If



                            End If
                        Next

                        'sHtmlBanner = sHtmlBanner & "</div>"
                        'sHtmlBanner = sHtmlBanner & "</div>"

                        iCartContent = 0
                    End If
                    sHtmlAtributos = "<div class='product-name text-left'> <a href='producto-interior.aspx'>" & sCampos & "</a></div>"

                    sHtmlBanner = sHtmlBanner & sHtmlImagen
                    sHtmlBanner = sHtmlBanner & "<div class='cart-content'>"
                    sHtmlBanner = sHtmlBanner & sHtmlAtributos
                    sHtmlBanner = sHtmlBanner & sHtmlCantidad
                    sHtmlBanner = sHtmlBanner & sHtmlPRecio
                    sHtmlBanner = sHtmlBanner & " <div class='cart-button text-center'>"
                    sHtmlBanner = sHtmlBanner & "  <button type='button' onclick=fnClickEliminar('" & Partida.ItemCode & "'); title='Eliminar' class='btn cancel-p'>"
                    sHtmlBanner = sHtmlBanner & "    <img src='img/cancel.svg' class='img-responsive'>"
                    sHtmlBanner = sHtmlBanner & "  </button>"
                    sHtmlBanner = sHtmlBanner & " </div>"

                    sHtmlBanner = sHtmlBanner & "</div>"

                    sHtmlBanner = sHtmlBanner & " </div></div></li> "
                End If

                sCampos = ""
                sHtmlImagen = ""
                sHtmlAtributos = ""
                sHtmlCantidad = ""
                sHtmlPRecio = ""

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            If iContador > 0 Then
                pnlContadorCarrito.Visible = True
            Else
                pnlContadorCarrito.Visible = False
            End If
            lblItemsCarrito.Text = iContador
        Catch ex As Exception
            Response.Redirect("index.aspx")
        End Try


        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlCarrito.Controls.Clear()
        pnlCarrito.Controls.Add(literal)

    End Sub
    <WebMethod>
    Public Shared Function EliminaCarrito(Articulo As String) As String

        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones


        For Each partida In HttpContext.Current.Session("Partidas")
            If partida.ItemCode = Articulo Then
                partida.ItemCode = "BORRAR"
            End If
        Next

        Try

        Catch ex As Exception
            '      objDatos.Mensaje(ex.Message, MyPage.page)
        End Try
        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Public Sub fnCargaRedesSociales()
        ssql = "Select * from config.RedesSociales where cvEstatus='ACTIVO'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = "  "


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtMenus.Rows(i)("cvLiga") & "' class=''><i class='" & dtMenus.Rows(i)("cvRedSocial") & "' aria-hidden='true'></i></a></li>"
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlRedes.Controls.Clear()
        pnlRedes.Controls.Add(literal)
    End Sub
    Public Sub fnCargaMenuFooter()
        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = " <div class='col-min-12 col-xs-6 col-md-3'> "


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <div class='col-min-12 col-xs-6 col-md-3'> "
            sHtmlMenu = sHtmlMenu & "<span class='tit'>" & dtMenus.Rows(i)("cvSubTipo") & "</span>"
            sHtmlMenu = sHtmlMenu & "<div class='cont-desd-fotter'> "
            sHtmlMenu = sHtmlMenu & " <ul class='menu-generico'> "
            ssql = "SELECT cvNombre,cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "'></a></li>"
                        Else
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                        End If

                    End If
                End If


            Next
            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFooter.Controls.Clear()
        pnlFooter.Controls.Add(literal)


    End Sub

    Public Sub fnCargaMenuFooterResponsive()
        ssql = "SELECT distinct cvSubTipo,IsNULL(ciOrdenSubMenu,1) FROM Config.Menus where cvTipoMenu='Footer'  order by IsNULL(ciOrdenSubMenu,1) "
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = " <div class='panel-group' id='accordionfooter2' role='tablist' aria-multiselectable='true'> "


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & "<div class='panel'> "
            sHtmlMenu = sHtmlMenu & "<div class='panel-heading del-carret' role='tab' id='cthirex" & (i + 1) & "'>"
            sHtmlMenu = sHtmlMenu & "<h4 class='categoria'> "
            sHtmlMenu = sHtmlMenu & "<a role='button' data-toggle='collapse' data-parent='#accordionfooter2' href='#cthir" & (i + 1) & "' aria-expanded='true' aria-controls='cthir" & (i + 1) & "'>"
            sHtmlMenu = sHtmlMenu & dtMenus.Rows(i)("cvSubTipo") & " </a> </h4></div>"

            sHtmlMenu = sHtmlMenu & "<div id='cthir" & (i + 1) & "' class='panel-collapse collapse' role='tabpanel' aria-labelledby='cthir" & (i + 1) & "'> "
            sHtmlMenu = sHtmlMenu & "  <div class='panel-body'>"
            sHtmlMenu = sHtmlMenu & "   <ul> "

            ssql = "SELECT cvNombre,cvLink,ISNULL(cvTipoDato,'') as TipoDato,cvImagen FROM Config.Menus where cvTipoMenu='Footer' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "' order by IsNULL(ciOrden,1)"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        If dtSubMenu.Rows(x)("TipoDato") = "imagen" Then
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'><img src='" & dtSubMenu.Rows(x)("cvImagen") & "'></a></li>"
                        Else
                            sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                        End If

                    End If
                End If


            Next
            sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
            sHtmlMenu = sHtmlMenu & "</div>"
        Next


        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFooterResponsive.Controls.Clear()
        pnlFooterResponsive.Controls.Add(literal)
    End Sub


    Public Sub fnCargaMenuUser()
        ssql = "SELECT distinct cvSubTipo FROM Config.Menus where cvTipoMenu='Header' AND ISNULL(cvTipoDato,'') = 'User'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String = ""


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1
            sHtmlEncabezado = " <li class='dropdown yamm-fw'><a href='#' data-toggle='dropdown' class='dropdown-toggle'>" & dtMenus.Rows(i)("cvSubTipo") & "<b class='caret'></b></a> "
            sHtmlEncabezado = sHtmlEncabezado & "<ul class='dropdown-menu'>"
            sHtmlEncabezado = sHtmlEncabezado & " <li> "
            sHtmlEncabezado = sHtmlEncabezado & "  <div class='yamm-content'> "
            sHtmlEncabezado = sHtmlEncabezado & "   <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "    <div  class='col-sm-12 no-padding'> "
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='col-xs-8'> "
            sHtmlEncabezado = sHtmlEncabezado & "        <ul class='nav nav-tabs tabs-left'> "



            ssql = "SELECT cvNombre,cvLink,ISNULL(cvTipoDato,'') as TipoDato FROM Config.Menus where cvTipoMenu='Header' AND  cvSubTipo=" & "'" & dtMenus.Rows(i)("cvSubTipo") & "'"
            Dim dtSubMenu As New DataTable
            dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
            For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                If dtSubMenu.Rows(x)("TipoDato") = "mailto" Then
                    sHtmlMenu = sHtmlMenu & "<li><a href=mailto:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                Else
                    If dtSubMenu.Rows(x)("TipoDato") = "tel" Then
                        sHtmlMenu = sHtmlMenu & "<li><a href=tel:" & dtSubMenu.Rows(x)("cvLink") & ">" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    Else
                        sHtmlMenu = sHtmlMenu & "<li><a href='" & dtSubMenu.Rows(x)("cvLink") & "'>" & dtSubMenu.Rows(x)("cvNombre") & "</a></li>"
                    End If
                End If


            Next
            ' sHtmlMenu = sHtmlMenu & " </ul> </div></div>"
        Next

        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        sHtmlEncabezado = sHtmlEncabezado & " </ul></div></div></div></div> </li></ul></li>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlMenuUsuario.Controls.Clear()
        pnlMenuUsuario.Controls.Add(literal)
    End Sub


    Public Sub fnCargaCategoriasv3()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            '   sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-2'>"
            '  sHtmlEncabezado = sHtmlEncabezado & " <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 5
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    'sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' >" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    'sHtmlBanner = sHtmlBanner & "</li> "

                    If iContador = 0 Then
                        sHtmlBanner = sHtmlBanner & "<li class='active'><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                        sHtmlBanner = sHtmlBanner & "</li> "
                    Else
                        sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                        sHtmlBanner = sHtmlBanner & "</li> "
                    End If
                    iContador = iContador + 1
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '     sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Sub

    Public Function fnCargaCategoriasHTML(NombreCat As String) As String
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            sHtmlEncabezado = sHtmlEncabezado & "  <li class='dropdown yamm-fw'><a href='catalogo.aspx' data-toggle='dropdown' class='dropdown-toggle'>" & NombreCat & "<b class='caret'></b></a>"
            sHtmlEncabezado = sHtmlEncabezado & "     <ul class='dropdown-menu' style='display: none;'>"
            sHtmlEncabezado = sHtmlEncabezado & "     <li>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div class='yamm-content'>"
            sHtmlEncabezado = sHtmlEncabezado & "      <div class='row'>"
            sHtmlEncabezado = sHtmlEncabezado & "       <div  class='col-sm-12 no-padding'>"
            sHtmlEncabezado = sHtmlEncabezado & "        <div class='col-xs-4'>"
            sHtmlEncabezado = sHtmlEncabezado & "         <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    ' sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' >" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    sHtmlBanner = sHtmlBanner & "<li><a href='#Cat-" & iContador & "' >" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    sHtmlBanner = sHtmlBanner & "</li> "

                    'If iContador = 0 Then
                    '    sHtmlBanner = sHtmlBanner & "<li class='active'><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                    '    sHtmlBanner = sHtmlBanner & "</li> "
                    'Else
                    '    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    '    sHtmlBanner = sHtmlBanner & "</li> "
                    'End If
                    iContador = iContador + 1
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"

            sHtmlEncabezado = sHtmlEncabezado & "<div class='col-xs-8'>" 'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-content'>" 'El que agregué
            ''Aqui las subcategorias
            iContador = 1
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='cat-" & (iContador) & "'>"
                    sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-3'>"
                    sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"

                    ''Nos traemos el nivel 2 de la categoría seleccionada
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                    End If


                    Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                    Dim dtSegundoNivel As New DataTable
                    dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                    If dtSegundoNivel.Rows.Count > 0 Then
                        If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                            ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                        Else
                            ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                        End If
                        Dim dtSubCategoria As New DataTable
                        dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                        sHtmlBanner = ""
                        For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                            If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                            Else
                                sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx'>Sub " & dtSubCategoria.Rows(x)(0) & "</a></li>"
                            End If

                        Next
                    End If

                    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                    iContador = iContador + 1
                    sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                End If

            Next


            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué
            sHtmlEncabezado = sHtmlEncabezado & " </div>"  'El que agregué



            sHtmlEncabezado = sHtmlEncabezado & " <div class='clearfix'></div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div>"
            sHtmlEncabezado = sHtmlEncabezado & " </div></div></li></ul></li>"





            Return sHtmlEncabezado

            'Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Function
    Public Function fnCargaCategoriasHTMLResponsive(NombreCat As String) As String
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            sHtmlEncabezado = sHtmlEncabezado & "  <li class='panel'>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div class='panel-heading' role='tab' id='headingThree'>"
            sHtmlEncabezado = sHtmlEncabezado & "      <a role='button' class='link-m-r' data-toggle='collapse' data-parent='#accordion' href='#collapseThree' aria-expanded='true' aria-controls='collapseThree'>" & NombreCat & "</a>"
            sHtmlEncabezado = sHtmlEncabezado & "     </div>"
            sHtmlEncabezado = sHtmlEncabezado & "     <div id='collapseThree' class='panel-collapse collapse' role='tabpanel' aria-labelledby='headingThree'>"
            sHtmlEncabezado = sHtmlEncabezado & "       <div class='panel-body'>"

            Dim iContador As Int16 = 5
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then
                    sHtmlBanner = sHtmlBanner & "<a class='link-m-r' href='Catalogo.aspx?Cat=" & dtCategorias.Rows(i)(0) & "' >" & dtCategorias.Rows(i)(0) & "</a> "
                    iContador = iContador + 1
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner

            sHtmlEncabezado = sHtmlEncabezado & " </div></div></li>"
            Return sHtmlEncabezado


        End If
    End Function
    Public Sub fnCargaCategorias()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            '   sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-2'>"
            '  sHtmlEncabezado = sHtmlEncabezado & " <ul class='nav nav-tabs tabs-left'>"
            Dim iContador As Int16 = 5
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    sHtmlBanner = sHtmlBanner & "</li> "

                    'If iContador = 0 Then
                    '    sHtmlBanner = sHtmlBanner & "<li class='active'><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                    '    sHtmlBanner = sHtmlBanner & "</li> "
                    'Else
                    '    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (iContador + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & "</a> " '<div class='col-xs-3'><ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul></div>
                    '    sHtmlBanner = sHtmlBanner & "</li> "
                    'End If
                    iContador = iContador + 1
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            '     sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            '  pnlCategorias.Controls.Clear()
            ' pnlCategorias.Controls.Add(Literal)
        End If
    End Sub
    Public Sub fnCargaCategoriasv2()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""
            sHtmlEncabezado = sHtmlEncabezado = " <div class='col-xs-2'>"
            sHtmlEncabezado = sHtmlEncabezado & "  <ul class='nav nav-tabs tabs-left'>"

            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlBanner = sHtmlBanner & "<li><a href='#cat-" & (i + 1) & "' data-toggle='tab'>" & dtCategorias.Rows(i)(0) & " </a> " '<ul class='tab-submenu'><li><a href='#'>categoria 1</a></li></ul>
                    sHtmlBanner = sHtmlBanner & "</li> "

                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
            sHtmlEncabezado = sHtmlEncabezado & " </ul></div>"


            ''Ahora subCategorias
            sHtmlBanner = ""
            sHtmlEncabezado = sHtmlEncabezado = " <div class='col-xs-10'>"
            sHtmlEncabezado = sHtmlEncabezado = "  <div class='tab-content'>"



            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlEncabezado = sHtmlEncabezado & "<div class='tab-pane' id='cat-" & (i + 1) & "'>"
                    sHtmlEncabezado = sHtmlEncabezado & " <div class='col-xs-3'>"
                    sHtmlEncabezado = sHtmlEncabezado & "  <ul class='tab-submenu'>"

                    ''Nos traemos el nivel 2 de la categoría seleccionada
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                    End If


                    Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                    Dim dtSegundoNivel As New DataTable
                    dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""
                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            sHtmlBanner = sHtmlBanner & "<a href='Catalogo.aspx'><li>" & dtSubCategoria.Rows(x)(0) & "</li></a>" '?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "
                        End If

                    Next
                    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                    sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                End If

            Next
            '    sHtmlEncabezado = sHtmlEncabezado & " </div></div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            'pnlCategorias.Controls.Clear()
            'pnlCategorias.Controls.Add(Literal)
        End If
    End Sub
    Public Sub fnCargaSubCategorias()
        Dim dtProductos As New DataTable
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "Select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =1"
        Dim dtPrimerNivel As New DataTable
        dtPrimerNivel = objDatos.fnEjecutarConsulta(ssql)
        If dtPrimerNivel.Rows.Count > 0 Then
            If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                ''Grupo Nativo de SAP
                ssql = objDatos.fnObtenerQuery("GrupoTodos")
            Else
                ''Traemos el distinct del campo en OITM
                ssql = objDatos.fnObtenerQuery("Categorias")
                ssql = ssql.Replace("[%0]", dtPrimerNivel.Rows(0)(0))

            End If
            Dim dtCategorias As New DataTable
            dtCategorias = objDatos.fnEjecutarConsultaSAP(ssql)
            Dim sLinkPrimerNivel As String = ""
            Dim sLinkSegundoNivel As String = ""

            Dim iContador As Int16 = 5
            ' sHtmlEncabezado = sHtmlEncabezado & "<div class='tab-pane active' id='principal'>"
            For i = 0 To dtCategorias.Rows.Count - 1 Step 1

                ''Ya con todas las posibles categorías, revisamos aquellas que se encuentren en config.Categorias
                ssql = "Select cvCategoria,cvImagen,cvUrl from config.categorias  WHERE cvCategoria=" & "'" & dtCategorias.Rows(i)(0) & "' AND ISNULL(cvEstatus,'ACTIVO')='ACTIVO'"
                Dim dtCategoriaValida As New DataTable
                dtCategoriaValida = objDatos.fnEjecutarConsulta(ssql)

                If dtCategoriaValida.Rows.Count > 0 Then

                    sHtmlEncabezado = sHtmlEncabezado & " <div class='tab-pane' id='cat-" & (iContador + 1) & "'>"
                    sHtmlEncabezado = sHtmlEncabezado & "  <div class='col-xs-3'>"
                    sHtmlEncabezado = sHtmlEncabezado & "   <ul class='tab-submenu'>"

                    ''Nos traemos el nivel 2 de la categoría seleccionada
                    ssql = objDatos.fnObtenerQuery("Categorias-det")
                    If dtPrimerNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%1]", "T1.ItmsGrpNam=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)("CodGrupo")
                    Else
                        ssql = ssql.Replace("[%1]", "T0." & dtPrimerNivel.Rows(0)("cvCampoSAP") & "=" & "'" & dtCategorias.Rows(i)(0) & "'")
                        sLinkPrimerNivel = dtCategorias.Rows(i)(0)
                    End If


                    Dim sQuery As String = "select cvDescripcion ,cvCampoSAP  from config.NivelesArticulos where ciOrden =2"
                    Dim dtSegundoNivel As New DataTable
                    dtSegundoNivel = objDatos.fnEjecutarConsulta(sQuery)
                    If dtSegundoNivel.Rows(0)("cvCampoSAP") = "ItmsGrpCod" Then
                        ssql = ssql.Replace("[%0]", "ItmsGrpNam")
                    Else
                        ssql = ssql.Replace("[%0]", "T0." & dtSegundoNivel.Rows(0)("cvCampoSAP"))
                    End If
                    Dim dtSubCategoria As New DataTable
                    dtSubCategoria = objDatos.fnEjecutarConsultaSAP(ssql)
                    sHtmlBanner = ""
                    For x = 0 To dtSubCategoria.Rows.Count - 1 Step 1
                        If dtSubCategoria.Rows(x)(0) Is DBNull.Value Then
                        Else
                            sHtmlBanner = sHtmlBanner & "<li><a href='Catalogo.aspx'>" & dtSubCategoria.Rows(x)(0) & "</a></li>" '?Cat=" & sLinkPrimerNivel & "&" & dtSegundoNivel.Rows(0)("cvCampoSAP") & "=" & dtSubCategoria.Rows(x)(0) & "
                        End If

                    Next
                    sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
                    iContador = iContador + 1
                    sHtmlEncabezado = sHtmlEncabezado & "</ul></div></div>"
                End If

            Next
            sHtmlEncabezado = sHtmlEncabezado & "</div>"
            Dim Literal As New LiteralControl(sHtmlEncabezado)
            pnlSubCategoria.Controls.Clear()
            pnlSubCategoria.Controls.Add(Literal)
        End If
    End Sub
    Public Sub fnCargaMenuHeader()
        ''Cargamos menu clasico HEADER


        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Header'  AND ISNULL(cvTipoDato,'') <> 'User' order by IsNULL(ciOrden,1) "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            If dtMenuHeader.Rows(i)("cvNombre") = "productos" Or dtMenuHeader.Rows(i)("cvNombre") = "products" Then
                sHtmlMenu = sHtmlMenu & fnCargaCategoriasHTML(dtMenuHeader.Rows(i)("cvNombre"))
            Else
                ''Revisamos si tiene subMenus
                ssql = "SELECT * from config.SubMenu where cvMenu =" & "'" & dtMenuHeader.Rows(i)("cvNombre") & "'"
                Dim dtSubMenu As New DataTable
                dtSubMenu = objDatos.fnEjecutarConsulta(ssql)
                If dtSubMenu.Rows.Count > 0 Then
                    ''Tiene subMenus

                    sHtmlMenu = sHtmlMenu & "<li class='drop-bold'>"
                    sHtmlMenu = sHtmlMenu & " <div class='dropdown-backdrop'></div><a href='#' class='dropdown-toggle' data-toggle='dropdown'>" & dtMenuHeader.Rows(i)("cvNombre") & "<b class='caret'></b> </a>"
                    sHtmlMenu = sHtmlMenu & "  <ul class='dropdown-menu drop-mini' role='menu' aria-labelledby='dropdownMenu'> "
                    For x = 0 To dtSubMenu.Rows.Count - 1 Step 1
                        sHtmlMenu = sHtmlMenu & " <li class=''><a tabindex='-1' href='" & dtSubMenu.Rows(x)("cvLink") & "'> " & dtSubMenu.Rows(x)("cvSubMenu") & " </a></li> "
                    Next
                    sHtmlMenu = sHtmlMenu & "</ul>"
                    sHtmlMenu = sHtmlMenu & "</li>"
                Else
                    sHtmlMenu = sHtmlMenu & " <li class='drop-bold'><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "

                End If

            End If

        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuClasicoHeader.Controls.Clear()
        pnlMenuClasicoHeader.Controls.Add(literal)
    End Sub

    Public Sub fnCargaMenuResponsive()
        ''Cargamos menu Responsive HEADER

        Dim sHTMLEncabezado As String = ""

        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Header'  AND ISNULL(cvTipoDato,'') <> 'User'  order by IsNULL(ciOrden,1) "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            If dtMenuHeader.Rows(i)("cvNombre") = "productos" Or dtMenuHeader.Rows(i)("cvNombre") = "products" Then
                sHtmlMenu = sHtmlMenu & fnCargaCategoriasHTMLResponsive(dtMenuHeader.Rows(i)("cvNombre"))
            Else
                sHtmlMenu = sHtmlMenu & " <li class='link-m-r'><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
            End If

        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuResponsiveHeader.Controls.Clear()
        pnlMenuResponsiveHeader.Controls.Add(literal)
    End Sub
    Public Sub fnCargaMenu()
        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Lateral'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String
        sHtmlEncabezado = " <ul class='cd-cart-items'> "


        Dim iCuantosFila As Int16 = 0
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1

            If iCuantosFila = 0 Then
                sHtmlMenu = sHtmlMenu & " <div class='flex-sep'> "
            End If

            iCuantosFila = iCuantosFila + 1
            sHtmlMenu = sHtmlMenu & fnGeneraOpcionMenu(dtMenus.Rows(i)("cvNombre"), dtMenus.Rows(i)("cvImagen"), i, dtMenus.Rows(i)("cvLink"), dtMenus.Rows(i)("cvEstilo"))
            If iCuantosFila = 2 Then
                iCuantosFila = 0
                sHtmlMenu = sHtmlMenu & " </div> "
            End If
        Next
        If iCuantosFila > 0 Then
            sHtmlMenu = sHtmlMenu & " </div> "
        End If

        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        sHtmlEncabezado = sHtmlEncabezado & " </ul>"

        Dim literal As New LiteralControl(sHtmlEncabezado)
        'pnlMenu.Controls.Clear()
        'pnlMenu.Controls.Add(literal)
    End Sub


    Public Function fnGeneraOpcionMenu(Nombre As String, imagen As String, valor As String, link As String, Estilo As String) As String
        Dim sHtmlMenu As String = ""
        sHtmlMenu = sHtmlMenu & " <li class='col-xs-6 " & Estilo.ToLower & "'> "
        sHtmlMenu = sHtmlMenu & "<div class='col-xs-12 tit-pedidos'>" & Nombre & " </div> "
        sHtmlMenu = sHtmlMenu & " <div class='icono '> "
        sHtmlMenu = sHtmlMenu & " <img src='" & imagen & "' class='img-responsive'> "
        sHtmlMenu = sHtmlMenu & " <span class='data'> " & valor & "</span>"
        sHtmlMenu = sHtmlMenu & " </div>"
        sHtmlMenu = sHtmlMenu & " </li>"
        Return sHtmlMenu
    End Function
    Protected Sub btnBuscar_Click(sender As Object, e As ImageClickEventArgs) Handles btnBuscar.Click
        Session("sesBuscar") = txtBuscar.Text
        Response.Redirect("Catalogo.aspx")
    End Sub
    'Protected Sub btnCerrarSesion_Click(sender As Object, e As ImageClickEventArgs) Handles btnCerrarSesion.Click
    '    Session("UserTienda") = ""
    '    Session("NombreuserTienda") = ""
    '    Session("slpCode") = "0"
    '    Session("Cliente") = ""
    '    Session("RazonSocial") = ""
    '    Session("ListaPrecios") = ""

    '    Session("UserB2C") = ""
    '    Session("NombreUserB2C") = ""
    '    Session("NombreuserTienda") = ""
    '    Session("CardCodeUserB2C") = ""



    '    Response.Redirect("Index.aspx")
    'End Sub


    Public Sub fnCargaMenuB2B()
        ssql = "SELECT cvNombre,cvImagen,cvLink,cvEstilo,ISNULL(cvQuery,'') as Query FROM Config.Menus where cvTipoMenu='Lateral'"
        Dim dtMenus As New DataTable
        dtMenus = objDatos.fnEjecutarConsulta(ssql)

        Dim sHtmlEncabezado As String = ""



        sHtmlEncabezado = sHtmlEncabezado & " <div id='main-nav'>"
        sHtmlEncabezado = sHtmlEncabezado & "         <div class='open-engine'><div id='cd-hamburger-menu'><img src='img/menu-b2b/grid.svg' class='img-responsive cuadrito-post'></div></div> "
        sHtmlEncabezado = sHtmlEncabezado & " <ul class='menu-b2b'> "

        sHtmlEncabezado = sHtmlEncabezado & "<div class='e-tienda'>"
        sHtmlEncabezado = sHtmlEncabezado & "<div class='sec-est'><a class='enlace icon-tienda' href='catalogo.aspx' data-toggle='tooltip' data-placement='bottom' title='Tienda'>"
        sHtmlEncabezado = sHtmlEncabezado & " </a></div>"
        sHtmlEncabezado = sHtmlEncabezado & " <div class='sec-est'><a class='enlace icon-contacto' href='contacto.aspx' data-toggle='tooltip' data-placement='bottom' title='Contacto'></a></div>"
        sHtmlEncabezado = sHtmlEncabezado & " </div>"
        sHtmlEncabezado = sHtmlEncabezado & "<div class='enlace-int'>"

        Dim iCuantosFila As Int16 = 0
        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1

            sHtmlMenu = sHtmlMenu & fnGeneraOpcionMenuB2B(dtMenus.Rows(i)("cvNombre"), dtMenus.Rows(i)("cvImagen"), dtMenus.Rows(i)("Query"), dtMenus.Rows(i)("cvLink"), dtMenus.Rows(i)("cvEstilo"))

        Next


        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        sHtmlEncabezado = sHtmlEncabezado & " </div>"

        sHtmlEncabezado = sHtmlEncabezado & "</ul>"
        sHtmlEncabezado = sHtmlEncabezado & " </div>"


        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlB2B.Controls.Clear()
        pnlB2B.Controls.Add(literal)
    End Sub

    Public Sub fnPlantillasVendedor()
        ''Cargamos las plantillas que tiene el usuario
        ssql = "select ciIdPlantilla as No,cvNombrePlantilla as Plantilla,Convert(varchar(10),cdFecha,120) as Fecha,cvComentarios as Comentarios from Tienda.Plantilla_hdr WHERE ( ciIdAgenteSAP=" & "'" & Session("SlpCode") & "' OR cvUsuario=" & "'" & Session("Cliente") & "')"
        Dim dtPlantillas As New DataTable
        dtPlantillas = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""
        For i = 0 To dtPlantillas.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <li><a href='levantar-pedido.aspx?opc=" & dtPlantillas.Rows(i)("No") & "'>" & dtPlantillas.Rows(i)("Plantilla") & "</a> </li>"
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlFavoritos.Controls.Clear()
        pnlFavoritos.Controls.Add(literal)

    End Sub
    Public Function fnGeneraOpcionMenuB2B(Nombre As String, imagen As String, valor As String, link As String, Estilo As String) As String
        Dim sHtmlMenu As String = ""

        sHtmlMenu = sHtmlMenu & " <a class='rec' href='" & link & "'> "
        sHtmlMenu = sHtmlMenu & "<div class='titulo'>" & Nombre & " </div> "
        If valor <> "" Then
            Dim dtValorMenu As New DataTable
            ssql = objDatos.fnObtenerQuery(valor)
            '      ssql = ssql.Replace("[%0]", Session("slpCode"))
            ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
            dtValorMenu = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtValorMenu.Rows.Count = 0 Then
                sHtmlMenu = sHtmlMenu & "<span class='cuant'>0</span> "
            Else
                If Estilo = "importe" Then
                    Dim importe As Double = 0
                    If dtValorMenu.Rows(0)(0) Is DBNull.Value Then
                        importe = 0
                    Else
                        importe = dtValorMenu.Rows(0)(0)
                    End If
                    sHtmlMenu = sHtmlMenu & "<span class='cuant'>" & importe.ToString("$ ###,###,###,###.#0") & "</span> "
                Else
                    sHtmlMenu = sHtmlMenu & "<span class='cuant'>" & dtValorMenu.Rows.Count & "</span> "
                End If

            End If

        End If
        sHtmlMenu = sHtmlMenu & " <img src='" & imagen & "' class='img-responsive'/> "
        sHtmlMenu = sHtmlMenu & " </a>"

        Return sHtmlMenu
    End Function
    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        ''Hacemos el login
        If txtUser.Text = "" Then
            objDatos.Mensaje("Especifique un usuario", Me.Page)
            Exit Sub
        End If
        If txtPass.Text = "" Then
            objDatos.Mensaje("Especifique una contraseña", Me.Page)
            Exit Sub
        End If
        ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUser.Text & "' AND cvPass=" & "'" & txtPass.Text & "' "
        Dim dtLogin As New DataTable
        dtLogin = objDatos.fnEjecutarConsulta(ssql)
        If dtLogin.Rows.Count > 0 Then
            Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
            Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
            If dtLogin.Rows(0)("cvCardCode") = "" Then
                objDatos.Mensaje("Acceso incorrecto", Me.Page)
                Exit Sub
            End If
            Session("Cliente") = dtLogin.Rows(0)("cvCardCode")
            Session("UserTienda") = dtLogin.Rows(0)("cvUsuario")
            If dtLogin.Rows(0)("cvTipoAcceso") = "B2B" Then
                Session("UserB2C") = ""

                Session("RazonSocial") = dtLogin.Rows(0)("cvNombreCompleto")
                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("Cliente"))

                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
                fnCargaMenuB2B()
            End If
            ''en base al cliente, obtenemos cual es su lista de precios
            ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
            ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
            Dim dtLista As New DataTable
            dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtLista.Rows.Count > 0 Then
                Session("ListaPrecios") = dtLista.Rows(0)(0)
            Else
                Session("ListaPrecios") = "1"
            End If

            pnlLogin.Visible = False
            pnlUsuarioLogin.Visible = True
            pnlCliente.Visible = True
            lblUsuario.Text = Session("NombreuserTienda") & " - " & Session("RazonSocial")
            'btnCerrarSesion.Visible = True
            'pnlOpciones.Visible = True
            '  fnCargaMenuUser()
            Response.Redirect("index.aspx")
            pnlIconUser.Visible = False

        Else
            objDatos.Mensaje("Acceso incorrecto", Me.Page)
            Exit Sub
        End If
    End Sub
End Class

