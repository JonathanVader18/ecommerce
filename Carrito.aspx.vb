
Imports System.Data
Imports System.Data.OleDb
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class Carrito
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Public sCaracterMoneda As String = ""

    Public Function fnObtenerMoneda(ItemCode As String) As String
        Dim ssql As String = ""
        ''Posibles monedas en la lista de precios
        ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
        ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
        dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
        Dim sMoneda As String = ""
        If Request.QueryString("Moneda") <> "" Then
            sMoneda = Request.QueryString("Moneda")
        End If
        If dtMonedas.Rows.Count > 0 Then
            sMoneda = dtMonedas.Rows(0)(0)
            If dtMonedas.Rows.Count > 1 Then
                ''El articulo se puede vender en mas de una moneda
                ''Llenamos y mostramos combo de moneda
            End If
        End If
        ' Session("Moneda") = sMoneda
        Return sMoneda
    End Function






    Private Sub Carrito_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim sPop As String()
        Dim fila As DataRow



        If Not IsPostBack Then


            If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Then
                Session("TC") = "1"
                ssql = objDatos.fnObtenerQuery("GetBalanceCustomer")
                If ssql <> "" Then
                    ssql = ssql.Replace("[%0]", Session("Cliente"))
                    Dim dtSaldoCliente As New DataTable
                    dtSaldoCliente = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtSaldoCliente.Rows.Count > 0 Then
                        If CDbl(dtSaldoCliente.Rows(0)(0)) + CDbl(Session("TotalCarrito")) > 0 Then
                            ''Saldo > a límite de crédito

                            btnPedido.Enabled = False
                            btnCotizar.Visible = True

                            objDatos.Mensaje("El cliente no cuenta con crédito disponible para realizar el pedido.", Me.Page)
                        Else

                        End If
                    End If
                End If
            End If
            If objDatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Or objDatos.fnObtenerCliente.ToUpper.Contains("MANIJ") Then
                pnlSuje.Visible = True
            End If

            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtclienteSeg As New DataTable
            dtclienteSeg = objDatos.fnEjecutarConsulta(ssql)
            If dtclienteSeg.Rows.Count > 0 Then
                If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("SEGU") Or CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("BOSS") Then
                    lblCarrito.Text = "CARRITO DE COMPRAS"
                    If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("BOSS") Then
                        btnPagar.Visible = True
                    End If

                End If

                If CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("STOP CAT") Then


                    btnPagar.Visible = True
                    If Session("RazonSocial") <> "" Then
                        objDatos.fnLog("Delta razon soc", Session("RazonSocial"))
                        fnCalculaDescuentoSalama()
                    Else
                        Session("usrInvitado") = "SI"
                    End If


                End If

                If (CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("HAWK") Or CStr(dtclienteSeg.Rows(0)(0)).ToUpper.Contains("ALTURA")) And Session("Cliente") <> "" And Session("CardCodeUserB2C") = "" Then
                    ''Mostramos el panel Hwk y llenamos el tipo de envio
                    pnlHawk.Visible = True
                    pnlDireccionEntrega.Visible = False
                    ssql = objDatos.fnObtenerQuery("TiposEnvio")

                    Dim dtTiposEnvio As New DataTable
                    dtTiposEnvio = objDatos.fnEjecutarConsultaSAP(ssql)
                    ddlTipodeEnvio.DataSource = dtTiposEnvio
                    ddlTipodeEnvio.DataTextField = "Descripcion"
                    ddlTipodeEnvio.DataValueField = "Codigo"
                    ddlTipodeEnvio.DataBind()


                End If
            End If


            ssql = "SELECT ISNULL(cvCaracterMoneda,'') FROM config.Parametrizaciones "
            Dim dtCaracter As New DataTable
            dtCaracter = objDatos.fnEjecutarConsulta(ssql)
            If dtCaracter.Rows.Count > 0 Then
                sCaracterMoneda = dtCaracter.Rows(0)(0)
            End If



            ''Cargamos los paises
            ssql = objDatos.fnObtenerQuery("Paises")
            Dim dtPais As New DataTable
            dtPais = objDatos.fnEjecutarConsultaSAP(ssql)
            fila = dtPais.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtPais.Rows.Add(fila)
            ddlPais.DataSource = dtPais
            ddlPais.DataTextField = "Descripcion"
            ddlPais.DataValueField = "Clave"
            ddlPais.DataBind()
            ' ddlPais.SelectedValue = "0"

            Dim dtEstado As New DataTable
            Try
                ''Cargamos los estados

                ssql = objDatos.fnObtenerQuery("Estados")
                ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)

                dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)

                objDatos.fnLog("pais index", "")

                fila = dtEstado.NewRow
                fila("Clave") = "0"
                fila("Descripcion") = "-Seleccione-"
                dtEstado.Rows.Add(fila)
                ddlEstados.DataSource = dtEstado
                ddlEstados.DataTextField = "Descripcion"
                ddlEstados.DataValueField = "Clave"
                ddlEstados.DataBind()
                ddlEstados.SelectedValue = "0"
            Catch ex As Exception
                objDatos.fnLog("Cargar estados", ex.Message)
            End Try
            Session("ActualizaCarrito") = "NO"
            ''Revisamos si tenemos campos adicionales x pintar

            'ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            'Dim dtcliente As New DataTable
            'dtcliente = objDatos.fnEjecutarConsulta(ssql)
            'If dtcliente.Rows.Count > 0 Then
            '    If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
            '        pnlMoneta.Visible = True
            '    End If
            'End If

            Dim dtAdicionales As New DataTable


            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ''B2B
                ssql = "SELECT * FROM config.Carrito_Adicionales where cvEstatus = 'ACTIVO' ANd cvModulo ='B2B' "
                dtAdicionales = objDatos.fnEjecutarConsulta(ssql)

                If dtAdicionales.Rows.Count > 0 Then
                    For i = 0 To dtAdicionales.Rows.Count - 1 Step 1
                        If dtAdicionales.Rows(i)("cvOpcion") = "Forma de entrega" Then
                            pnlTransporting.Visible = True
                            lblAdicional.Text = dtAdicionales.Rows(i)("cvTexto")

                            Dim sQueryadicional As String = ""
                            sQueryadicional = objDatos.fnObtenerQuery(dtAdicionales.Rows(i)("cvQuery"))
                            If sQueryadicional <> "" Then
                                Dim dtTrans As New DataTable
                                dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                                ddlTransporting.DataSource = dtTrans
                                ddlTransporting.DataValueField = "Codigo"
                                ddlTransporting.DataTextField = "Descripcion"
                                ddlTransporting.DataBind()
                            End If


                        End If

                        If dtAdicionales.Rows(i)("cvOpcion") = "Fecha de entrega" Then
                            pnlFechaEntrega.Visible = True
                            lblFechaEntrega.Text = dtAdicionales.Rows(i)("cvTexto")
                        End If


                    Next
                End If
            End If

            If Session("RazonSocial") <> "" Then
                If CStr(objDatos.fnObtenerCliente()).Contains("AIO") Or CStr(objDatos.fnObtenerCliente()).Contains("PMK") Or CStr(objDatos.fnObtenerCliente()).Contains("SUJEA") Then
                    pnlTemplate.Visible = True

                End If
                If CStr(objDatos.fnObtenerCliente()).Contains("SUJEA") Or CStr(objDatos.fnObtenerCliente()).Contains("MANIJ") Then
                    pnlOtros.Visible = True
                End If


            End If

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) <> 0 Then
                ''Vendedores


                ''Obtenemos el nombre de la empresa
                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                Dim dtcliente As New DataTable
                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                If dtcliente.Rows.Count > 0 Then
                    If CStr(dtcliente.Rows(0)(0)).Contains("TAQ") Then
                        Response.Redirect("captura-pedido.aspx")
                    End If

                End If

                If objDatos.fnObtenerCliente().ToUpper.Contains("SUJEAU") Then
                    ''Cargamos las paqueterías
                    Dim sQueryadicional As String = ""
                    Try

                        sQueryadicional = objDatos.fnObtenerQuery("Paqueterias")
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            Dim filap As DataRow
                            filap = dtTrans.NewRow
                            filap("Codigo") = 0
                            filap("Descripcion") = "-Seleccione-"
                            dtTrans.Rows.Add(filap)

                            ddlPaqueteria.DataSource = dtTrans
                            ddlPaqueteria.DataValueField = "Codigo"
                            ddlPaqueteria.DataTextField = "Descripcion"
                            ddlPaqueteria.DataBind()


                            ''Obtenemos la paqueteria default del cliente para que aparezca seleccionada
                            sQueryadicional = objDatos.fnObtenerQuery("PaqueteriaCliente")
                            sQueryadicional = sQueryadicional.Replace("[%0]", Session("Cliente"))
                            objDatos.fnLog("Paq cliente:", sQueryadicional.Replace("'", ""))
                            Dim dtPaq As New DataTable
                            dtPaq = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            If dtPaq.Rows.Count > 0 Then
                                ddlPaqueteria.SelectedValue = dtPaq.Rows(0)(0)
                            Else
                                objDatos.fnLog("Paq cliente:", sQueryadicional.Replace("'", ""))
                                ddlPaqueteria.SelectedValue = 0
                            End If
                        End If
                    Catch ex As Exception
                        objDatos.fnLog("Paqueterias:", ex.Message.Replace("'", ""))
                    End Try

                    pnlPaqueteria.Visible = True
                    ''Forma de envio
                    pnlTipoEnvio.Visible = True

                    Try
                        sQueryadicional = objDatos.fnObtenerQuery("TipoEnvio")
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            Dim filae As DataRow
                            filae = dtTrans.NewRow
                            filae("Codigo") = "0"
                            filae("Descripcion") = "-Seleccione-"
                            dtTrans.Rows.Add(filae)

                            ddlTipoEnvio.DataSource = dtTrans
                            ddlTipoEnvio.DataValueField = "Codigo"
                            ddlTipoEnvio.DataTextField = "Descripcion"
                            ddlTipoEnvio.DataBind()
                            ddlTipoEnvio.SelectedValue = "0"
                        End If

                    Catch ex As Exception
                        objDatos.fnLog("Tipo Envio:", ex.Message.Replace("'", ""))
                    End Try

                    Try
                        sQueryadicional = objDatos.fnObtenerQuery("UsoCFDI")
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)


                            ddlUsoCFDI.DataSource = dtTrans
                            ddlUsoCFDI.DataValueField = "Usage"
                            ddlUsoCFDI.DataTextField = "Descr"
                            ddlUsoCFDI.DataBind()

                            ''Obtenemos el uso de cfdi default del cliente para que aparezca seleccionada
                            sQueryadicional = objDatos.fnObtenerQuery("UsoCFDIcliente")
                            sQueryadicional = sQueryadicional.Replace("[%0]", Session("Cliente"))
                            Dim dtPaq As New DataTable
                            dtPaq = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            If dtPaq.Rows.Count > 0 Then
                                ddlUsoCFDI.SelectedValue = dtPaq.Rows(0)(0)
                            End If

                        End If

                    Catch ex As Exception
                        objDatos.fnLog("Uso de CFDI:", ex.Message.Replace("'", ""))
                    End Try



                End If
                ssql = "SELECT * FROM config.Carrito_Adicionales where cvEstatus = 'ACTIVO' ANd cvModulo ='Vendedores' "
                dtAdicionales = objDatos.fnEjecutarConsulta(ssql)

                For i = 0 To dtAdicionales.Rows.Count - 1 Step 1
                    If dtAdicionales.Rows(i)("cvOpcion") = "Proyecto" Then
                        pnlProyecto.Visible = True
                        lblAdicional2.Text = dtAdicionales.Rows(i)("cvTexto")

                        Dim sQueryadicional As String = ""
                        sQueryadicional = objDatos.fnObtenerQuery(dtAdicionales.Rows(i)("cvQuery"))
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            ddlProyecto.DataSource = dtTrans
                            ddlProyecto.DataValueField = "Codigo"
                            ddlProyecto.DataTextField = "Descripcion"
                            ddlProyecto.DataBind()
                        End If


                    End If

                    If dtAdicionales.Rows(i)("cvOpcion") = "Forma de entrega" Then
                        pnlTransporting.Visible = True
                        lblAdicional.Text = dtAdicionales.Rows(i)("cvTexto")

                        Dim sQueryadicional As String = ""
                        sQueryadicional = objDatos.fnObtenerQuery(dtAdicionales.Rows(i)("cvQuery"))
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            ddlTransporting.DataSource = dtTrans
                            ddlTransporting.DataValueField = "Codigo"
                            ddlTransporting.DataTextField = "Descripcion"
                            ddlTransporting.DataBind()
                        End If


                    End If

                    If dtAdicionales.Rows(i)("cvOpcion") = "Almacen" Then
                        pnlAlmacen.Visible = True
                        lblAdicionalAlm.Text = dtAdicionales.Rows(i)("cvTexto")

                        Dim sQueryadicional As String = ""
                        sQueryadicional = objDatos.fnObtenerQuery(dtAdicionales.Rows(i)("cvQuery"))
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            ddlAlmacen.DataSource = dtTrans
                            ddlAlmacen.DataValueField = "Codigo"
                            ddlAlmacen.DataTextField = "Descripcion"
                            ddlAlmacen.DataBind()
                        End If


                    End If


                    If dtAdicionales.Rows(i)("cvOpcion") = "Otros" Then
                        pnlOtros.Visible = True
                        lblAdicionalOtros.Text = dtAdicionales.Rows(i)("cvTexto")




                    End If




                    If dtAdicionales.Rows(i)("cvOpcion") = "Estado Pedido" Then
                        pnlEstadoPedido.Visible = True
                        lblEstadoPedido.Text = dtAdicionales.Rows(i)("cvTexto")

                        Dim sQueryadicional As String = ""
                        sQueryadicional = objDatos.fnObtenerQuery(dtAdicionales.Rows(i)("cvQuery"))
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objDatos.fnEjecutarConsultaSAP(sQueryadicional)
                            ddlEstadoPedido.DataSource = dtTrans
                            ddlEstadoPedido.DataValueField = "Codigo"
                            ddlEstadoPedido.DataTextField = "Descripcion"
                            ddlEstadoPedido.DataBind()
                        End If


                    End If

                    If dtAdicionales.Rows(i)("cvOpcion") = "Fecha de entrega" Then
                        pnlFechaEntrega.Visible = True
                        lblFechaEntrega.Text = dtAdicionales.Rows(i)("cvTexto")
                    End If





                Next
            End If



        End If
        'ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnPedido)

        ssql = "SELECT * FROM config.Carrito_Adicionales where cvEstatus = 'ACTIVO' ANd cvModulo ='B2B' "
        Dim dtAdicionalesB2B = objDatos.fnEjecutarConsulta(ssql)
        If dtAdicionalesB2B.Rows.Count > 0 And Session("Cliente") <> "" Then
            If dtAdicionalesB2B.Rows(0)("cvOpcion") = "Otros" Then
                pnlOtros.Visible = True
                lblAdicionalOtros.Text = dtAdicionalesB2B.Rows(0)("cvTexto")

            End If
        End If


        If pnlHawk.Visible = True Then
            pnlDireccionEntrega.Visible = False
        End If


        Dim sTallaColor As String = ""
        ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
        Dim dtTallasColores As New DataTable
        dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
        If dtTallasColores.Rows.Count > 0 Then
            If dtTallasColores.Rows(0)(0) = "SI" Then
                sTallaColor = "SI"
            End If
        End If

        If Session("PopUpItem") <> "" Then
            sPop = CStr(Session("PopUpItem")).Split("-")

            Dim dPrecioActual As Double = 0
            If CInt(Session("slpCode")) <> 0 Then

                dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), Session("ListaPrecios"))
            Else
                dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"))
            End If


            Dim partidaAdic As New Cls_Pedido.Partidas
            partidaAdic.ItemCode = sPop(0)
            partidaAdic.Cantidad = 1
            partidaAdic.Precio = dPrecioActual
            Session("Partidas").add(partidaAdic)
        End If
        ''Revisamos si no viene un action
        If Request.QueryString.Count > 0 Then
            Dim icontador As Int16 = 0
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If (Partida.ItemCode = Request.QueryString("item") Or Partida.Linea = Request.QueryString("Lin")) And Request.QueryString("Action") = "d" Then
                    Partida.ItemCode = "BORRAR"

                    Try
                        Dim cookie As HttpCookie
                        cookie = Request.Cookies("carrito")
                        cookie.Value = cookie.Value.Replace(Request.QueryString("item"), "BORRAR")
                        Response.Cookies.Add(cookie)
                    Catch ex As Exception

                    End Try


                End If
            Next
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")

                If Partida.ItemCode <> "FLETE-ESTAFETA" And Partida.ItemCode <> "BORRAR" Then
                    icontador = icontador + 1
                End If
            Next
            If icontador = 0 Then
                For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                    If Partida.ItemCode = "FLETE-ESTAFETA" Then
                        Partida.ItemCode = "BORRAR"
                        Session("ImporteEnvio") = 0
                    End If
                Next
            Else
                'fnAgregaFletesSeguros_StopCatalogo()
            End If
            Response.Redirect("carrito.aspx")
        End If
        objDatos.fnLog("Carrito load", "Despues de action")
        ''Cargamos lo que haya en el carrito




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
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Dim TotalImpuestos As Double = 0
        Dim fTasaImpuesto As Double = 0
        Try


            objDatos.fnLog("Carrito load", "For de partidas")
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then


                    ''Evaluamos el impuesto
                    ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", Partida.ItemCode)
                        ssql = ssql.Replace("[%1]", Session("Cliente"))
                        Dim dtIVA As New DataTable
                        dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            If objDatos.fnObtenerDBMS = "HANA" Then
                                ssql = objDatos.fnObtenerQuery("GetRateTax")
                                If dtIVA.Rows(0)(0) Is DBNull.Value Then
                                    fTasaImpuesto = 0

                                Else
                                    ssql = ssql.Replace("[%0]", dtIVA.Rows(0)(0))
                                End If

                            Else
                                ssql = "select ISNULL(rate,0) from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                            End If

                            Dim dtTasa As New DataTable
                            dtTasa = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtTasa.Rows.Count > 0 Then
                                fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                            Else
                                fTasaImpuesto = 0
                            End If

                        End If

                    End If


                    x = x + 1
                    objDatos.fnLog("Carrito load", Partida.ItemCode)
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            Dim dtExiste As New DataTable
                            Dim ssqlExiste As String = ssql.Replace("[%0]", "'" & Partida.Generico & "'")
                            dtExiste = objDatos.fnEjecutarConsultaSAP(ssqlExiste)
                            If dtExiste.Rows.Count = 0 Then
                                Partida.Generico = Partida.ItemCode
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                            End If
                            objDatos.fnLog("Carga load carrito Talla color SI ", ssql.Replace("'", ""))
                            Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)
                            objDatos.fnLog("Info-prod", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            If dtFoto.Rows(0)(0) <> "" Then
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-responsive'>"
                                            Else
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                            End If

                                        Else
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                        End If
                                    Else
                                        Dim iband As Int16 = 0
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & ".jpg") Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & ".jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-1.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-1.jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-2.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-2.jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-3.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-3.jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If

                                        If iband = 0 Then
                                            If dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                                sHtmlBanner = sHtmlBanner & " <img src=" & "'images/no-image.png' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            Else
                                                If dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) <> "" Then
                                                    If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo"))) Then
                                                        sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                                    Else
                                                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                                    End If

                                                Else
                                                    sHtmlBanner = sHtmlBanner & " <img src=" & "'images/no-image.png' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                                End If
                                            End If

                                        End If


                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                End If



                                'sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                sHtmlBanner = sHtmlBanner & "</div>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                        'ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                                        'ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                        'Dim dtItemName As New DataTable
                                        'dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)

                                        objDatos.fnLog("Carrito load itemNAme", Partida.ItemName)
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-3 info-producto'> " & sCampos & "</div>"

                    End If
                    objDatos.fnLog("Carrito load", "Antes de precio")

                    If Partida.Precio = 0 Then
                        objDatos.fnLog("Carrito load", "Precio cero")
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, (Session("ListaPrecios")))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If
                    objDatos.fnLog("Carrito load", "Precio Actual:" & Partida.Precio)
                    ' sHtmlBanner = sHtmlBanner & "</div>"
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If


                    objDatos.fnLog("Carrito-TC", Session("TC"))
                    Try
                        If Session("TC") = "" Then
                            Session("TC") = "1"
                        End If
                    Catch ex As Exception
                        Session("TC") = "1"
                    End Try

                    Dim sMonedasistema As String = ""
                    If objDatos.fnObtenerDBMS = "HANA" Then
                        ssql = objDatos.fnObtenerQuery("MonedasConf")
                    Else
                        ssql = "select MainCurncy  from OADM "
                    End If

                    Dim dtMoneda As New DataTable
                    dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtMoneda.Rows.Count > 0 Then
                        sMonedasistema = dtMoneda.Rows(0)(0)
                    End If

                    ssql = "SELECT ISNULL(cvMonedaPortal,'') from config.parametrizaciones"
                    Dim dtMonedaPortal As New DataTable
                    dtMonedaPortal = objDatos.fnEjecutarConsulta(ssql)
                    If dtMonedaPortal.Rows.Count > 0 Then
                        If dtMonedaPortal.Rows(0)(0) <> "" Then
                            sMonedasistema = dtMonedaPortal.Rows(0)(0)
                        End If
                    End If
                    Session("Moneda") = Partida.Moneda
                    objDatos.fnLog("Carrito load", "antes partida descuento")
                    If Partida.Descuento > 0 Then

                        If Partida.Moneda <> sMonedasistema Then
                            objDatos.fnLog("Carrito load", "moneda dif a sMonedaSistema")
                            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                                Session("Moneda") = Partida.Moneda
                                If Session("Cliente") <> "" Then
                                    Session("TC") = 1
                                Else
                                    Session("Moneda") = sMonedasistema
                                End If

                            Else
                                Session("Moneda") = sMonedasistema
                            End If

                            Dim sDiferenciaPrecio As Double = 0
                            sDiferenciaPrecio = (Partida.Precio * CDbl(Session("TC"))) - (precioConDescuento * CDbl(Session("TC")))

                            objDatos.fnLog("Carrito load Acumula descto1 diferencia:", sDiferenciaPrecio)

                            objDatos.fnLog("Carrito load Acumula descto1:", (sDiferenciaPrecio * Partida.Cantidad))

                            objDatos.fnLog("Carrito load Acumula descto1 Precio x TC:", (Partida.Precio * CDbl(Session("TC"))))
                            objDatos.fnLog("Carrito load Acumula descto1 Precio tras desc x TC:", (precioConDescuento * CDbl(Session("TC"))))

                            TotDescuento = TotDescuento + (sDiferenciaPrecio * Partida.Cantidad)
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                        Else
                            objDatos.fnLog("Carrito load Acumula descto2:", ((Partida.Precio - precioConDescuento) * Partida.Cantidad))
                            TotDescuento = TotDescuento + ((Partida.Precio - precioConDescuento) * Partida.Cantidad)
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                        End If
                    Else
                        If Partida.Moneda = "" Then
                            Partida.Moneda = sMonedasistema
                        End If
                        If Partida.Moneda <> sMonedasistema Then
                            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                                Session("Moneda") = Partida.Moneda
                                If Session("Cliente") <> "" Then
                                    Session("TC") = 1
                                Else
                                    ''Si convertimos de USD a MXP, expresamos la moneda del carrito en la moneda de Sistem
                                    Session("Moneda") = sMonedasistema
                                End If
                            Else
                                Session("Moneda") = sMonedasistema
                            End If
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                        Else

                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                        End If


                    End If

                    objDatos.fnLog("Carrito load", precioConDescuento)
                    objDatos.fnLog("Carrito load partida.precio", Partida.Precio)

                    If Partida.Descuento > 0 Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-precio'><span class='precio-con-descuento'>" & sCaracterMoneda & " " & precioConDescuento.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</span><div class='precio-original descuento'>" & sCaracterMoneda & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-precio'><div class='precio-original'>" & sCaracterMoneda & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    End If

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div  class='precio' id='#txt" & x & "'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-total'><div class='prec-simul'> " & sCaracterMoneda & " " & (Partida.Cantidad * precioConDescuento).ToString("###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    If Partida.Mts2 > 0 Then
                        ' Mts <sup>2</sup>
                        lblColAdicional.Text = "Mts<sup>2</sup>"
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-total'><div class='prec-simul'> " & (Partida.Cantidad * Partida.Mts2).ToString("###,###.#0") & "</div></div>"
                    End If


                    If Partida.Moneda <> sMonedasistema Then
                        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                            Session("Moneda") = Partida.Moneda
                            '   Session("TC") = 1
                            Session("Moneda") = sMonedasistema
                            If Session("Cliente") <> "" Then
                                Session("TC") = 1
                                Session("Moneda") = Partida.Moneda
                                '      Partida.Moneda = Session("Moneda")
                            Else
                                ''Si convertimos de USD a MXP, expresamos la moneda del carrito en la moneda de Sistem
                                Session("Moneda") = sMonedasistema
                            End If
                        Else
                            Session("Moneda") = sMonedasistema
                        End If

                        objDatos.fnLog("Carrito-TC", "Partida moneda <> moneda: " & Partida.Moneda & " <> " & Session("Moneda"))
                        objDatos.fnLog("Carrito", "SubTotal a acumular1:" & (Partida.Cantidad * (Partida.Precio * CDbl(Session("TC")))))
                        ''Multiplicamos el precio por el tipo de cambio
                        sSubTotal = sSubTotal + (Partida.Cantidad * (Partida.Precio * CDbl(Session("TC"))))
                    Else
                        objDatos.fnLog("Carrito", "SubTotal a acumular2:" & (Partida.Cantidad * Partida.Precio))
                        sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    End If


                    If Partida.ItemCode <> "FLETE-ESTAFETA" Then
                        ''Aqui van los botones de Action Cart
                        sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                        'PopUp('','Agregado al carrito','Aceptar');
                        If sTallaColor = "SI" Then
                            sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & Partida.Linea & "'>Editar</a></div>"

                        Else
                            If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA_US") Then
                                sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & Partida.Linea & "'>Edit</a></div>"
                            Else
                                sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & Partida.Linea & "'>Editar</a></div>"
                            End If




                        End If
                        '  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'  href='PopUp('','Agregado al carrito','Aceptar');'>Editar</a></div>"

                        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("ALTURA_US") Then
                            sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='carrito.aspx?item=" & Partida.ItemCode & "&Action=d" & "&Lin=" & Partida.Linea & "'>Remove</a></div>"
                        Else
                            sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='carrito.aspx?item=" & Partida.ItemCode & "&Action=d" & "&Lin=" & Partida.Linea & "'>Quitar</a></div>"
                        End If


                        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("STOP CAT") And Session("Razon Social") <> "" Then
                            sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart preview-popup' href='elegir-favoritos.aspx?code=" & Partida.ItemCode & "&name=" & Partida.ItemName & "'>Mover a favoritos</a></div>"
                        End If

                        'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
                        sHtmlBanner = sHtmlBanner & "</div>  "

                    End If

                    sHtmlBanner = sHtmlBanner & "</div>  "

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If
                objDatos.fnLog("Carrito", "Arma")

            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objDatos.fnLog("Carrito load", ex.Message)
        End Try
        Try
            objDatos.fnLog("Carrito load", "SubTotales")

            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
            Dim literal As New LiteralControl(sHtmlEncabezado)
            pnlPartidas.Controls.Clear()
            pnlPartidas.Controls.Add(literal)
            lblSubTotal.Text = sCaracterMoneda & " " & sSubTotal.ToString(" ###,###,###.#0") & " " & Session("Moneda")
            If TotDescuento = 0 Then
                lblDescuento.Text = ""
            Else
                lblDescuento.Text = sCaracterMoneda & " " & TotDescuento.ToString(" ###,###,###.#0") & " " & Session("Moneda")
            End If
            If Session("Cliente") <> "" And (CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("ALTU") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK")) Then
                Session("ImporteEnvio") = 0
            End If

            If CDbl(Session("ImporteEnvio")) > 0 Then
                objDatos.fnLog("Carrito load", "antes importe envio >0")
                lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                lblEnviotxt.Visible = True
                lblEnvio.Visible = True
            End If

            If TotDescuento < 0 Then
                TotDescuento = TotDescuento * -1
            End If

            Session("ImporteSubTotal") = sSubTotal

        Catch ex As Exception
            objDatos.fnLog("Carrito load ex8", ex.Message.Replace("'", ""))
        End Try
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            objDatos.fnLog("Carrito load", "antes envio")
            If lblEnvio.Text = "" Then
                lblEnviotxt.Visible = False
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                Descuento = CDbl(lblDescuento.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If

            objDatos.fnLog("Carrito envio", Envio)
            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception
            Envio = CDbl(Session("ImporteEnvio"))
        End Try
        Try
            objDatos.fnLog("Carrito load", "antes log total carrito")
            TotalImpuestos = TotalImpuestos + (Envio)
            lblTotal.Text = sCaracterMoneda & " " & (sSubTotal + Envio - TotDescuento).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            Session("TotalCarrito") = (sSubTotal + Envio - TotDescuento)
            Session("ImporteDescuento") = TotDescuento

            objDatos.fnLog("Carrito", "Total carrito:" & Session("TotalCarrito"))
            objDatos.fnLog("Carrito", "Total carrito ImporteDescuento:" & Session("ImporteDescuento"))


            ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
            Dim dtTipoPrecio As New DataTable
            dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
            If dtTipoPrecio.Rows.Count > 0 Then
                If dtTipoPrecio.Rows(0)(0) = "NO" Or Session("Cliente") <> "" Then
                    ''Calculamos el IVA
                    'Dim fIVA As Double = 0
                    'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                    objDatos.fnLog("Carrito load", "antes pnlImpuestos")
                    pnlImpuestos.Visible = True
                    lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                    Session("TotalImpuestos") = TotalImpuestos
                End If

                'If dtTipoPrecio.Rows(0)(0) = "SI" Then
                '    pnlImpuestos.Visible = True
                '    ' Session("TotalCarrito") = sSubTotal
                '    TotalImpuestos = (CDbl(Session("TotalCarrito"))) / (1 + fTasaImpuesto)
                '    lblSubTotal.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                '    lblTotal.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                '    Session("TotalCarrito") = TotalImpuestos
                '    TotalImpuestos = TotalImpuestos * fTasaImpuesto
                '    Session("TotalImpuestos") = TotalImpuestos
                '    lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                '    lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                'End If



            End If

            objDatos.fnLog("Carrito load", "antes totales")
            If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) And Session("Cliente") <> "" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                objDatos.fnLog("Carrito load", "antes total carrito")
                TotalImpuestos = CDbl(Session("TotalCarrito")) * fTasaImpuesto
                lblSubTotal.Text = sCaracterMoneda & " " & CDbl(Session("TotalCarrito")).ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                objDatos.fnLog("Carrito load", "antes impuestos")
                Session("TotalImpuestos") = TotalImpuestos

                objDatos.fnLog("Carrito load", "antes pintar imp y total")
                lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            End If





            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente3 As New DataTable
            dtcliente3 = objDatos.fnEjecutarConsulta(ssql)
            If dtcliente3.Rows.Count > 0 Then
                If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SEGURIT") Or CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("HAWK") Or CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("ALTURA") Or CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SUJEA") Then
                    pnlImpuestos.Visible = True
                    ' Session("TotalCarrito") = sSubTotal
                    TotalImpuestos = (CDbl(Session("TotalCarrito"))) / (1 + fTasaImpuesto)
                    lblSubTotal.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    lblTotal.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    Session("TotalCarrito") = TotalImpuestos
                    TotalImpuestos = TotalImpuestos * fTasaImpuesto
                    Session("TotalImpuestos") = TotalImpuestos
                    lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")


                End If
                If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("STOP CAT") Then
                    pnlImpuestos.Visible = False 'Solo subtotal, envio y descuento
                    ' Session("TotalCarrito") = sSubTotal
                    TotalImpuestos = (CDbl(Session("TotalCarrito"))) / (1 + fTasaImpuesto)
                    lblSubTotal.Text = sCaracterMoneda & " " & sSubTotal.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    lblTotal.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito"))).ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    'Session("TotalCarrito") = TotalImpuestos
                    'TotalImpuestos = TotalImpuestos * fTasaImpuesto
                    'Session("TotalImpuestos") = TotalImpuestos
                    'lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                    'lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                End If
            End If

            If CStr(Session("LeyendaDescuento")) <> "" Then
                lblCarrito.Text = "Carrito de compras (" & Session("LeyendaDescuento") & ")"
                lblDesctxt.Visible = True
            End If

            'If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then

            '    fnAgregaFletesSeguros_StopCatalogo()

            'End If

            '  objDatos.fnLog("Carrito", "slpCode y demas")

            If Session("UserB2C") <> "" Then
                pnlGuardarCarrito.Visible = True
                btnGuardar.Visible = True
                btnProcesar.Visible = True
            End If

            If CInt(Session("slpCode")) = 0 And Session("Cliente") = "" Then
                btnProcesar.Visible = True
            Else
                If CInt(Session("slpCode")) <> 0 Then

                    ''Vendedores
                    pnlSeparador.Visible = True
                    ''Revisamos en la tabla parametrizaciones, si debemos ocultar algún boton

                    ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='OFERTA'"
                    Dim dtBotonOferta As New DataTable
                    dtBotonOferta = objDatos.fnEjecutarConsulta(ssql)
                    If dtBotonOferta.Rows.Count > 0 Then
                        If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                            btnCotizar.Visible = True
                            '   pnlBotonCot.Visible = True
                        Else
                            btnCotizar.Visible = False
                            ' pnlBotonCot.Visible = False
                        End If
                    Else
                        ''Por default lo pintamos
                        btnCotizar.Visible = True
                        ' pnlBotonCot.Visible = True

                    End If

                    btnGuardar.Visible = True

                    ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='PEDIDO'"
                    Dim dtBotonPedido As New DataTable
                    dtBotonPedido = objDatos.fnEjecutarConsulta(ssql)
                    If dtBotonPedido.Rows.Count > 0 Then
                        If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                            btnPedido.Visible = True
                        Else
                            btnPedido.Visible = False
                        End If
                    Else
                        ''Por default lo pintamos
                        btnPedido.Visible = True
                    End If


                    btnProcesar.Visible = False
                    If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("ALTURA") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SUJEA") Then
                        btnGuardarPlantilla.Visible = False
                    Else
                        btnGuardarPlantilla.Visible = True
                    End If


                    pnlProcesar.Visible = False
                    pnlGuardarCarrito.Visible = True
                    If Session("UserB2C") <> "" Then
                        pnlDireccionEntrega.Visible = False
                    Else
                        pnlDireccionEntrega.Visible = True
                    End If

                    If Not IsPostBack Then
                        ''Cargamos las direcciones del cliente seleccionado
                        ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                        ssql = ssql.Replace("[%0]", Session("Cliente"))
                        Dim dtdirecciones As New DataTable
                        dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
                        ddlDirecciones.DataSource = dtdirecciones
                        ddlDirecciones.DataTextField = "DireccionDes"
                        ddlDirecciones.DataValueField = "Direccion"
                        ddlDirecciones.DataBind()

                        Try
                            ''Obtenemos el detalle de la dirección de envio
                            ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                            ssql = ssql.Replace("[%0]", Session("Cliente")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)
                            Dim dtDetalleDireccion As New DataTable
                            dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtDetalleDireccion.Rows.Count > 0 Then
                                Session("CalleEnvio") = dtDetalleDireccion.Rows(0)("Calle")
                                Session("ColoniaEnvio") = dtDetalleDireccion.Rows(0)("Colonia")
                                Session("Ciudadenvio") = dtDetalleDireccion.Rows(0)("Ciudad")

                                Session("NumExtEnvio") = dtDetalleDireccion.Rows(0)("Numero")
                                Session("CPEnvio") = dtDetalleDireccion.Rows(0)("CP")
                                Session("EstadoEnvio") = dtDetalleDireccion.Rows(0)("Estado")
                                'ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                                '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                                ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
                                Dim dtLogin As New DataTable
                                dtLogin = objDatos.fnEjecutarConsulta(ssql)
                                If dtLogin.Rows.Count > 0 Then
                                    'txtTelefono.Text = dtLogin.Rows(0)("Tel1")
                                    'txtRFC.Text = dtLogin.Rows(0)("RFC")
                                    'txtNombre.Text = dtLogin.Rows(0)("cvNombreCompleto")
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    End If

                Else
                    '   pnlPayPal.Visible = True
                    ' btnProcesar.Visible = True
                End If

                If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                    pnlPayPal.Visible = False
                    pnlSeparador.Visible = True
                    ''B2B

                    ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='OFERTA'"
                    Dim dtBotonOferta As New DataTable
                    dtBotonOferta = objDatos.fnEjecutarConsulta(ssql)
                    If dtBotonOferta.Rows.Count > 0 Then
                        If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                            btnCotizar.Visible = True
                            '  pnlBotonCot.Visible = True
                        Else
                            btnCotizar.Visible = False
                            ' pnlBotonCot.Visible = False
                        End If
                    Else
                        ''Por default lo pintamos
                        btnCotizar.Visible = True
                        '  pnlBotonCot.Visible = True
                    End If


                    btnGuardar.Visible = True
                    ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='PEDIDO'"
                    Dim dtBotonPedido As New DataTable
                    dtBotonPedido = objDatos.fnEjecutarConsulta(ssql)
                    If dtBotonPedido.Rows.Count > 0 Then
                        If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                            btnPedido.Visible = True
                        Else
                            btnPedido.Visible = False
                        End If
                    Else
                        ''Por default lo pintamos
                        btnPedido.Visible = True
                    End If

                    btnProcesar.Visible = False
                    btnGuardarPlantilla.Visible = True
                    pnlProcesar.Visible = False
                    pnlGuardarCarrito.Visible = True
                    '  pnlDireccionEntrega.Visible = True
                    If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("ALTURA") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SUJEA") Then
                        btnGuardarPlantilla.Visible = False
                    Else
                        btnGuardarPlantilla.Visible = True
                    End If
                    If Not IsPostBack Then
                        ''Cargamos las direcciones del cliente seleccionado
                        ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                        ssql = ssql.Replace("[%0]", Session("Cliente"))
                        Dim dtdirecciones As New DataTable
                        dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
                        ddlDirecciones.DataSource = dtdirecciones
                        ddlDirecciones.DataTextField = "DireccionDes"
                        ddlDirecciones.DataValueField = "Direccion"
                        ddlDirecciones.DataBind()

                        Try
                            ''Obtenemos el detalle de la dirección de envio
                            ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                            ssql = ssql.Replace("[%0]", Session("Cliente")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)
                            Dim dtDetalleDireccion2 As New DataTable
                            dtDetalleDireccion2 = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtDetalleDireccion2.Rows.Count > 0 Then
                                Session("CalleEnvio") = dtDetalleDireccion2.Rows(0)("Calle")
                                Session("ColoniaEnvio") = dtDetalleDireccion2.Rows(0)("Colonia")
                                Session("Ciudadenvio") = dtDetalleDireccion2.Rows(0)("Ciudad")

                                Session("NumExtEnvio") = dtDetalleDireccion2.Rows(0)("Numero")
                                Session("CPEnvio") = dtDetalleDireccion2.Rows(0)("CP")
                                Session("EstadoEnvio") = dtDetalleDireccion2.Rows(0)("Estado")
                                'ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                                '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                                ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
                                Dim dtLogin As New DataTable
                                dtLogin = objDatos.fnEjecutarConsulta(ssql)
                                If dtLogin.Rows.Count > 0 Then
                                    'txtTelefono.Text = dtLogin.Rows(0)("Tel1")
                                    'txtRFC.Text = dtLogin.Rows(0)("RFC")
                                    'txtNombre.Text = dtLogin.Rows(0)("cvNombreCompleto")
                                End If
                            End If
                        Catch ex As Exception

                        End Try
                    End If


                    ssql = "SELECT ISNULL(cvImprime,'NO') FROM [config].[ParametrizacionesB2B]"
                    Dim dtImprimeB2B As New DataTable
                    dtImprimeB2B = objDatos.fnEjecutarConsulta(ssql)
                    If dtImprimeB2B.Rows.Count > 0 Then
                        If dtImprimeB2B.Rows(0)(0) = "NO" Then
                            btnImprimir.Visible = False

                        Else
                            btnImprimir.Visible = True
                        End If
                    End If

                    ''Si el cliente es BOSS,Revisamos el saldo para forzar a que el cliente pague su pedido
                    ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
                    Dim dtclienteNom As New DataTable
                    dtclienteNom = objDatos.fnEjecutarConsulta(ssql)
                    If dtclienteNom.Rows.Count > 0 Then
                        If CStr(dtclienteNom.Rows(0)(0)).ToUpper.Contains("BOSS") Then

                            ssql = objDatos.fnObtenerQuery("GetBalanceCustomer")
                            If ssql <> "" Then
                                ssql = ssql.Replace("[%0]", Session("Cliente"))
                                Dim dtSaldoCliente As New DataTable
                                dtSaldoCliente = objDatos.fnEjecutarConsultaSAP(ssql)
                                If dtSaldoCliente.Rows.Count > 0 Then
                                    If CDbl(dtSaldoCliente.Rows(0)(0)) > 0 Then
                                        ''Saldo > a límite de crédito
                                        btnCotizar.Visible = False
                                        btnPedido.Visible = False
                                        btnPagar.Visible = True
                                        objDatos.Mensaje("Usted no cuenta con crédito disponible para realizar el pedido. Deberá pagarlo en línea para que podamos procesarlo", Me.Page)
                                    End If
                                End If
                            End If

                        End If
                    End If


                End If
            End If

            If Session("Cliente") <> "" And Session("UserB2C") = "" Then
                pnlDireccionEntrega.Visible = True
                btnGuardar.Visible = False
            End If

            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "

            dtcliente3 = objDatos.fnEjecutarConsulta(ssql)
            If dtcliente3.Rows.Count > 0 Then
                If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SEGURIT") Then
                    pnlDireccionEntrega.Visible = False
                End If
            End If

            If objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK") Then
                pnlOtraDireccion.Visible = False
            End If

            If Descuento > 0 Then
                lblDesctxt.Visible = True
            End If
            If pnlHawk.Visible = True Then
                pnlDireccionEntrega.Visible = False
            End If
            If CStr(objDatos.fnObtenerCliente().ToUpper.Contains("STOP CAT")) Then
                btnPedido.Visible = False
                btnCotizar.Visible = False

            End If

        Catch ex As Exception
            objDatos.fnLog("Carrito load ex9", ex.Message.Replace("'", ""))
        End Try






    End Sub


    Public Sub fnCalculaDescuentoSalama()

        Dim iMontoCarrito As Double = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then

                Else
                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If

                    If objDatos.fnArticuloDescuentoDelta(Partida.ItemCode) = 0 Then
                        iMontoCarrito = iMontoCarrito + (Partida.Precio * Partida.Cantidad)
                    Else
                        objDatos.fnLog("Descuentos", Partida.ItemCode & " no suma")
                    End If

                End If

            End If
        Next

        Dim fDescuento As Double = 0

        Dim dtDescuento As New DataTable
        objDatos.fnLog("Calcula desct carrito", iMontoCarrito)
        dtDescuento = objDatos.fnCalculaDescuentoDelta(iMontoCarrito)
        If dtDescuento.Rows.Count > 0 Then
            fDescuento = dtDescuento.Rows(0)("DescActual")
            If CDbl(dtDescuento.Rows(0)("SigDescto")) = 0 Then
                Session("LeyendaDescuento") = "Felicidades! Haz alcanzado el descuento más alto."
            Else
                Session("LeyendaDescuento") = "Descto actual:" & fDescuento.ToString("##.#0") & "- Necesitas: " & CDbl(dtDescuento.Rows(0)("Leyenda")).ToString("###,###,###.#0") & " para un mejor descuento:" & CDbl(dtDescuento.Rows(0)("SigDescto")).ToString("##.#0")
            End If

        End If
        objDatos.fnLog("Asigna desct carrito", fDescuento)
        fnAsignaDescuentoDelta(fDescuento)

    End Sub
    Public Sub fnAsignaDescuentoDelta(Descuento As Double)
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then

                Else

                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.Descuento = Descuento
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    Else
                        Partida.Descuento = Descuento
                    End If
                    Dim iDescEspecialDelta As Double = 0
                    iDescEspecialDelta = objDatos.fnObtenerDescuentoEspecialDelta(Partida.ItemCode)

                    If iDescEspecialDelta > 0 Then
                        ''Tiene descuento especial, lo reemplazamos
                        Partida.Descuento = iDescEspecialDelta
                    End If

                End If

            End If
        Next
    End Sub
    Public Sub fnAgregaFletesSeguros_StopCatalogo()
        ''Asignamos los descuentos
        If Session("RazonSocial") <> "" Then
            fnCalculaDescuentoSalama()
        End If

        objDatos.fnLog("Fletes STOP", "Entra")
        Dim iCantPiezasTotales As Int16 = 0
        Dim fMontoCarrito As Double = 0
        Dim iExisteFlete As Int16 = 0
        objDatos.fnLog("fnAgregaFletesSeguros_StopCatalogo", "Entra")
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                If Partida.ItemCode = "FLETE-ESTAFETA" Then
                    iExisteFlete = 1
                Else
                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If
                    iCantPiezasTotales = iCantPiezasTotales + Partida.Cantidad
                    objDatos.fnLog("Fletes STOP", "Monto Carrito: " & Partida.TotalLinea)
                    fMontoCarrito = fMontoCarrito + ((Partida.Precio * Partida.Cantidad) * (1 - (Partida.Descuento / 100)))
                    'fMontoCarrito = fMontoCarrito + Partida.TotalLinea
                End If

            End If
        Next

        If fMontoCarrito = 0 Then

        End If
        objDatos.fnLog("Fletes STOP", "Monto Carrito: " & fMontoCarrito)
        objDatos.fnLog("Fletes STOP", "Piezas: " & iCantPiezasTotales)
        ''Teniendo las piezas totales y el importe, determinamos el monto del flete
        ''-Reglas-------------------
        ''--Cada 70 prendas se cargan 60 Pesos de flete
        ''--Por cada Mil pesos se cargan al concepto de Flete 15 pesos
        Dim iMontoFleteGratis As Double = 20000000
        If Session("RazonSocial") = "" Then
            iMontoFleteGratis = 999
        Else
            ''Por promo o estrategia, Delta manejará flete gratis al alcanzar 899 para sus socios B2B
            iMontoFleteGratis = objDatos.fnPromoFleteDeltaSocios()
        End If

        Dim iPiezasFlete As Int16 = 70
        Dim iMontoPorSeguro As Double = 15 ' / 1.16
        Dim iMontoPorFlete As Double = 60 ' / 1.16
        Dim iMultiploCompraSeguro As Double = 1000

        Dim iMontoFleteYSeguros As Double = 0
        Dim iMontoFlete As Double = 0
        Dim iMontoSeguro As Double = 0

        Dim sResultadoDivFlete As String()
        Dim sResultadoDivSeguro As String()

        sResultadoDivFlete = CStr(iCantPiezasTotales / iPiezasFlete).Split(".")
        sResultadoDivSeguro = CStr(fMontoCarrito / iMultiploCompraSeguro).Split(".")


        ''Primero determinar el flete x piezas
        If fMontoCarrito < iMontoFleteGratis Then
            If CInt(sResultadoDivFlete(0)) < 1 Then
                iMontoFlete = iMontoPorFlete
            Else
                iMontoFlete = iMontoPorFlete * (CInt(sResultadoDivFlete(0)) + 1)
            End If
        Else
            ''Flete gratis, cargamos 1 centavo y lo que se acumule
            iMontoFlete = 0.0

            ''Si son mas de 70 piezas (un segundo multiplo, cargamos monto extra de flete)


            '   MsgBox(sResultadoDiv(0))
            If CInt(sResultadoDivFlete(0)) > 1 Then
                iMontoFlete = iMontoFlete + (iMontoPorFlete * (CInt(sResultadoDivFlete(0)) - 1))
            End If

        End If
        iMontoSeguro = CDbl(sResultadoDivSeguro(0)) * iMontoPorSeguro

        If iMontoFlete = 0 Then
            iMontoSeguro = 0
        End If
        'txtFlete.Text = iMontoFlete
        'txtSeguro.Text = iMontoSeguro
        'txtFleteTotal.Text = iMontoFlete + iMontoSeguro



        ''Cargamos el flete o lo modificamos
        'If iExisteFlete = 1 Then
        '    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
        '        If Partida.ItemCode <> "BORRAR" Then
        '            If Partida.ItemCode = "FLETE-ESTAFETA" Then
        '                Partida.Precio = iMontoFlete + iMontoSeguro
        '            End If
        '        End If
        '    Next
        'Else
        '    Dim partidaFlete As New Cls_Pedido.Partidas
        '    partidaFlete.ItemCode = "FLETE-ESTAFETA"
        '    partidaFlete.Cantidad = 1
        '    partidaFlete.Precio = iMontoFlete + iMontoSeguro
        '    Session("Partidas").add(partidaFlete)
        'End If

        Session("ImporteEnvio") = iMontoFlete + iMontoSeguro
        lblCarrito.Text = "Carrito de compras (" & Session("LeyendaDescuento") & ")"

    End Sub

    Public Sub fnCargaCarrito(Linea As Int16, Cantidad As Int16)
        Session("ActualizaCarrito") = "SI"
        Exit Sub
        ClientScript.RegisterStartupScript(Me.Page.GetType(), "Ventana", "window.opener.location.href='carrito.aspx'; ", True)

        Dim sPop As String()

        Dim sTallaColor As String = ""
        ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
        Dim dtTallasColores As New DataTable
        dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
        If dtTallasColores.Rows.Count > 0 Then
            If dtTallasColores.Rows(0)(0) = "SI" Then
                sTallaColor = "SI"
            End If
        End If


        If Session("PopUpItem") <> "" Then
            sPop = CStr(Session("PopUpItem")).Split("-")

            Dim dPrecioActual As Double = 0
            If CInt(Session("slpCode")) <> 0 Then

                dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), Session("ListaPrecios"))
            Else
                dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"))
            End If
            If Session("Cliente") <> "" Then
                dPrecioActual = objDatos.fnPrecioActual(Request.QueryString("code"), Session("ListaPrecios"))
            End If

            Dim partidaAdic As New Cls_Pedido.Partidas
            partidaAdic.ItemCode = sPop(0)
            partidaAdic.Cantidad = 1
            partidaAdic.Precio = dPrecioActual
            Session("Partidas").Add(partidaAdic)
        End If

        ''Cargamos lo que haya en el carrito

        '''Preparamos el encabezado del Grid
        'Dim sHtmlBanner As String = ""
        'Dim sHtmlEncabezado As String = ""

        'ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
        '        & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
        '        & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        'Dim dtCamposPlantilla As New DataTable
        'dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        'Dim sImagen As String = "ImagenPal"
        'Dim sSubTotal As Double = 0
        'Try
        '    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
        '        If Partida.ItemCode <> "BORRAR" Then
        '            sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
        '            If dtCamposPlantilla.Rows.Count > 0 Then
        '                Dim sCampos As String = ""
        '                For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

        '                    ssql = objDatos.fnObtenerQuery("Info-Producto")
        '                    ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
        '                    Dim dtGeneral As New DataTable
        '                    dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

        '                    If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
        '                        sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "
        '                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
        '                        sHtmlBanner = sHtmlBanner & "</div>"
        '                    Else
        '                        If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

        '                            If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Or dtCamposPlantilla.Rows(i)("Campo") = "U_descripBreve" Then
        '                                sCampos = sCampos & Partida.ItemName & " <br>"
        '                            Else
        '                                sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
        '                            End If

        '                        End If


        '                    End If
        '                Next
        '                sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
        '                sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 info-producto'> " & sCampos & "</div>"

        '            End If
        '            ' sHtmlBanner = sHtmlBanner & "</div>"
        '            If Partida.Moneda <> sMonedasistema Then
        '                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
        '                    Session("Moneda") = Partida.Moneda
        '                    '   Session("TC") = 1
        '                    If Session("Cliente") <> "" Then
        '                        '  Session("TC") = 1
        '                    Else
        '                        ''Si convertimos de USD a MXP, expresamos la moneda del carrito en la moneda de Sistem
        '                        Session("Moneda") = sMonedasistema
        '                    End If
        '                Else
        '                    Session("Moneda") = sMonedasistema
        '                End If

        '                objDatos.fnLog("Carrito-TC", "Partida moneda <> moneda: " & Partida.Moneda & " <> " & Session("Moneda"))
        '                ''Multiplicamos el precio por el tipo de cambio
        '                sSubTotal = sSubTotal + (Partida.Cantidad * (Partida.Precio * CDbl(Session("TC"))))
        '            Else
        '                sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
        '            End If

        '            sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3'><div class='prec-simul'>" & Partida.Precio.ToString(" ###,###,###.#0") & "</div></div>"
        '            sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'><div class='precio'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
        '            sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'><div class='precio'>" & Partida.Descuento.ToString("###,###,###.#0") & "</div></div>"
        '            sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3'><div class='prec-simul'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div></div>"
        '            '  sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)

        '            If Partida.ItemCode <> "FLETE-ESTAFETA" Then
        '                ''Aqui van los botones de Action Cart
        '                sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"

        '                If sTallaColor = "SI" Then
        '                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "'>Editar</a></div>"

        '                Else
        '                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "'>Editar</a></div>"

        '                End If
        '                sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='carrito.aspx?item=" & Partida.ItemCode & "&Action=d'>Quitar</a></div>"
        '                sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
        '                'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
        '                sHtmlBanner = sHtmlBanner & "</div>   </div>"
        '            End If


        '            sHtmlBanner = sHtmlBanner & " </div> "
        '        End If

        '    Next
        'Catch ex As Exception
        '    '   Response.Redirect("index.aspx")
        'End Try



        'sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        'Dim literal As New LiteralControl(sHtmlEncabezado)


        'Try
        '    ' pnlPartidas.Controls.Clear()
        '    pnlPartidas.Controls.Add(literal)


        '    lblSubTotal.Text = sCaracterMoneda & " " & sSubTotal.ToString("###,###,###.#0")
        '    Session("ImporteSubTotal") = sSubTotal
        '    Dim Envio As Double = 0
        '    Dim Descuento As Double = 0
        '    Try
        '        If lblEnvio.Text = "" Then
        '            Envio = 0
        '        Else
        '            Envio = CDbl(lblEnvio.Text.Replace(sCaracterMoneda, ""))
        '        End If

        '        If lblDescuento.Text = "" Then
        '            Descuento = 0
        '        Else
        '            Descuento = CDbl(lblDescuento.Text.Replace(sCaracterMoneda, ""))
        '        End If


        '        Session("ImporteEnvio") = Envio
        '        Session("ImporteDescuento") = Descuento
        '    Catch ex As Exception

        '    End Try
        '    lblTotal.Text = (sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0")
        '    Session("TotalCarrito") = lblTotal.Text.Replace(sCaracterMoneda, "")

        '    If CInt(Session("slpCode")) <> 0 Then
        '        pnlSeparador.Visible = True
        '        btnCotizar.Visible = True
        '        btnGuardar.Visible = True
        '        btnPedido.Visible = True
        '        btnProcesar.Visible = False
        '        btnGuardarPlantilla.Visible = True
        '        pnlProcesar.Visible = False
        '        pnlGuardarCarrito.Visible = True
        '        '  pnlDireccionEntrega.Visible = True
        '        ''Cargamos las direcciones del cliente seleccionado
        '        ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
        '        ssql = ssql.Replace("[%0]", Session("Cliente"))
        '        Dim dtdirecciones As New DataTable
        '        dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
        '        ddlDirecciones.DataSource = dtdirecciones
        '        ddlDirecciones.DataTextField = "Direccion"
        '        ddlDirecciones.DataValueField = "Direccion"
        '        ddlDirecciones.DataBind()


        '    Else
        '        pnlPayPal.Visible = True
        '        ' btnProcesar.Visible = True
        '    End If
        'Catch ex As Exception
        '    objDatos.Mensaje(ex.Message, Me.Page)
        'End Try




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
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Dim TotalImpuestos As Double = 0
        Dim fTasaImpuesto As Double = 0
        Try


            objDatos.fnLog("Carrito", "For de partidas")
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then


                    ''Evaluamos el impuesto
                    ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", Partida.ItemCode)
                        ssql = ssql.Replace("[%1]", Session("Cliente"))
                        Dim dtIVA As New DataTable
                        dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            If objDatos.fnObtenerDBMS = "HANA" Then
                                ssql = objDatos.fnObtenerQuery("GetRateTax")
                                If dtIVA.Rows(0)(0) Is DBNull.Value Then
                                    fTasaImpuesto = 0

                                Else
                                    ssql = ssql.Replace("[%0]", dtIVA.Rows(0)(0))
                                End If

                            Else
                                ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                            End If

                            Dim dtTasa As New DataTable
                            dtTasa = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtTasa.Rows.Count > 0 Then
                                fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                            Else
                                fTasaImpuesto = 0
                            End If

                        End If

                    End If


                    x = x + 1
                    objDatos.fnLog("Carrito", Partida.ItemCode)
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objDatos.fnObtenerQuery("Info-Producto")
                            Dim dtExiste As New DataTable
                            Dim ssqlExiste As String = ssql.Replace("[%0]", "'" & Partida.Generico & "'")
                            dtExiste = objDatos.fnEjecutarConsultaSAP(ssqlExiste)
                            If dtExiste.Rows.Count = 0 Then
                                Partida.Generico = Partida.ItemCode
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                            End If
                            objDatos.fnLog("Carga carrito Talla color SI ", ssql.Replace("'", ""))
                            Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)
                            objDatos.fnLog("Info-prod", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objDatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objDatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            If dtFoto.Rows(0)(0) <> "" Then
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-responsive'>"
                                            Else
                                                sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                            End If

                                        Else
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                        End If
                                    Else
                                        Dim iband As Int16 = 0
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & ".jpg") Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & ".jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-1.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-1.jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-2.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-2.jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If
                                        If File.Exists(Server.MapPath("~") & "\images\products\" & Partida.ItemCode & "-3.jpg") And iband = 0 Then
                                            sHtmlBanner = sHtmlBanner & " <img src=" & "'" & "images/products/" & Partida.ItemCode & "-3.jpg" & "'  alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            iband = 1
                                        End If

                                        If iband = 0 Then
                                            If dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) Is DBNull.Value Then
                                                sHtmlBanner = sHtmlBanner & " <img src=" & "'images/no-image.png' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                            Else
                                                If dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) <> "" Then
                                                    If File.Exists(Server.MapPath("~") & "\images\products\" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo"))) Then
                                                        sHtmlBanner = sHtmlBanner & "   <img src='images/products/" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                                    Else
                                                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                                    End If

                                                Else
                                                    sHtmlBanner = sHtmlBanner & " <img src=" & "'images/no-image.png' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                                End If
                                            End If

                                        End If


                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive' style='height: 80px;'>"
                                End If



                                'sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' class='img-responsive'>"
                                sHtmlBanner = sHtmlBanner & "</div>"
                            Else
                                If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                    If dtCamposPlantilla.Rows(i)("Campo") = "ItemName" Then
                                        'ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                                        'ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")
                                        'Dim dtItemName As New DataTable
                                        'dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)

                                        objDatos.fnLog("Carrito itemNAme", Partida.ItemName)
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-3 info-producto'> " & sCampos & "</div>"

                    End If
                    objDatos.fnLog("Carrito", "Antes de precio")

                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = objDatos.fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If
                    ' sHtmlBanner = sHtmlBanner & "</div>"
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If


                    objDatos.fnLog("Carrito-TC", Session("TC"))

                    Dim sMonedasistema As String = ""
                    If objDatos.fnObtenerDBMS = "HANA" Then
                        ssql = objDatos.fnObtenerQuery("MonedasConf")
                    Else
                        ssql = "select MainCurncy  from OADM "
                    End If

                    Dim dtMoneda As New DataTable
                    dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtMoneda.Rows.Count > 0 Then
                        sMonedasistema = dtMoneda.Rows(0)(0)
                    End If

                    ssql = "SELECT ISNULL(cvMonedaPortal,'') from config.parametrizaciones"
                    Dim dtMonedaPortal As New DataTable
                    dtMonedaPortal = objDatos.fnEjecutarConsulta(ssql)
                    If dtMonedaPortal.Rows.Count > 0 Then
                        If dtMonedaPortal.Rows(0)(0) <> "" Then
                            sMonedasistema = dtMonedaPortal.Rows(0)(0)
                        End If
                    End If

                    If Partida.Descuento > 0 Then

                        If Partida.Moneda <> sMonedasistema Then
                            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                                Session("Moneda") = Partida.Moneda
                                If Session("Cliente") <> "" Then
                                    'Session("TC") = 1
                                Else
                                    Session("Moneda") = sMonedasistema
                                End If

                            Else
                                Session("Moneda") = sMonedasistema
                            End If

                            TotDescuento = TotDescuento + (Partida.Precio * CDbl(Session("TC")) - (precioConDescuento * CDbl(Session("TC"))) * Partida.Cantidad)
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                        Else
                            TotDescuento = TotDescuento + ((Partida.Precio - precioConDescuento) * Partida.Cantidad)
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                        End If
                    Else
                        If Partida.Moneda = "" Then
                            Partida.Moneda = sMonedasistema
                        End If
                        If Partida.Moneda <> sMonedasistema Then
                            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                                Session("Moneda") = Partida.Moneda
                                If Session("Cliente") <> "" Then
                                    '  Session("TC") = 1
                                Else
                                    ''Si convertimos de USD a MXP, expresamos la moneda del carrito en la moneda de Sistem
                                    Session("Moneda") = sMonedasistema
                                End If
                            Else
                                Session("Moneda") = sMonedasistema
                            End If
                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                        Else

                            TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                        End If


                    End If

                    objDatos.fnLog("Carrito", precioConDescuento)
                    objDatos.fnLog("Carrito partida.precio", Partida.Precio)

                    If Partida.Descuento > 0 Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-precio'><span class='precio-con-descuento'>" & sCaracterMoneda & " " & precioConDescuento.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</span><div class='precio-original descuento'>" & sCaracterMoneda & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-precio'><div class='precio-original'>" & sCaracterMoneda & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    End If

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div  class='precio' id='#txt" & x & "'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-total'><div class='prec-simul'> " & sCaracterMoneda & " " & (Partida.Cantidad * precioConDescuento).ToString("###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    If Partida.Mts2 > 0 Then
                        ' Mts <sup>2</sup>
                        lblColAdicional.Text = "Mts<sup>2</sup>"
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-total'><div class='prec-simul'> " & (Partida.Cantidad * Partida.Mts2).ToString("###,###.#0") & "</div></div>"
                    End If


                    If Partida.Moneda <> sMonedasistema Then
                        If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                            Session("Moneda") = Partida.Moneda
                            '   Session("TC") = 1
                            If Session("Cliente") <> "" Then
                                '  Session("TC") = 1
                            Else
                                ''Si convertimos de USD a MXP, expresamos la moneda del carrito en la moneda de Sistem
                                Session("Moneda") = sMonedasistema
                            End If
                        Else
                            Session("Moneda") = sMonedasistema
                        End If

                        objDatos.fnLog("Carrito-TC", "Partida moneda <> moneda: " & Partida.Moneda & " <> " & Session("Moneda"))
                        ''Multiplicamos el precio por el tipo de cambio
                        sSubTotal = sSubTotal + (Partida.Cantidad * (Partida.Precio * CDbl(Session("TC"))))
                    Else
                        sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    End If


                    If Partida.ItemCode <> "FLETE-ESTAFETA" Then
                        ''Aqui van los botones de Action Cart
                        sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                        'PopUp('','Agregado al carrito','Aceptar');
                        If sTallaColor = "SI" Then
                            sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & Partida.Linea & "'>Editar</a></div>"

                        Else
                            sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & Partida.Linea & "'>Editar</a></div>"


                        End If
                        '  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'  href='PopUp('','Agregado al carrito','Aceptar');'>Editar</a></div>"
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='carrito.aspx?item=" & Partida.ItemCode & "&Action=d" & "&Lin=" & Partida.Linea & "'>Quitar</a></div>"
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart preview-popup' href='elegir-favoritos.aspx?code=" & Partida.ItemCode & "&name=" & Partida.ItemName & "'>Mover a favoritos</a></div>"
                        'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
                        sHtmlBanner = sHtmlBanner & "</div>  "

                    End If

                    sHtmlBanner = sHtmlBanner & "</div>  "

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If
                objDatos.fnLog("Carrito", "Arma")

            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objDatos.fnLog("Carrito load", ex.Message)
        End Try

        objDatos.fnLog("Carrito", "SubTotales")

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlPartidas.Controls.Clear()
        pnlPartidas.Controls.Add(literal)
        lblSubTotal.Text = sCaracterMoneda & " " & sSubTotal.ToString(" ###,###,###.#0") & " " & Session("Moneda")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = sCaracterMoneda & " " & TotDescuento.ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If
        If CDbl(Session("ImporteEnvio")) > 0 Then
            lblEnvio.Text = sCaracterMoneda & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
            lblEnviotxt.Visible = True
            lblEnvio.Visible = True
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                lblEnviotxt.Visible = False
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                Descuento = CDbl(lblDescuento.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), ""))
            End If
            If objDatos.fnObtenerCliente().ToUpper.Contains("STOP CAT") And Envio = 0 Then
                fnAgregaFletesSeguros_StopCatalogo()
                Envio = Session("ImporteEnvio")
            End If
            objDatos.fnLog("Carrito envio", Envio)
            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception
            Envio = CDbl(Session("ImporteEnvio"))
        End Try
        TotalImpuestos = TotalImpuestos + (Envio * 0.16)
        lblTotal.Text = sCaracterMoneda & " " & (sSubTotal + Envio - TotDescuento).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        Session("TotalCarrito") = (sSubTotal + Envio - TotDescuento)
        Session("ImporteDescuento") = TotDescuento


        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Or Session("Cliente") <> "" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                Session("TotalImpuestos") = TotalImpuestos
            End If
        End If
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente3 As New DataTable
        dtcliente3 = objDatos.fnEjecutarConsulta(ssql)
        If dtcliente3.Rows.Count > 0 Then
            If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SEGURIT") Or CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("HAWK") Or CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SUJEA") Then
                pnlImpuestos.Visible = True
                TotalImpuestos = CDbl(Session("TotalCarrito")) / (1 + fTasaImpuesto)
                lblSubTotal.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                lblTotal.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                Session("TotalCarrito") = TotalImpuestos
                TotalImpuestos = TotalImpuestos * fTasaImpuesto
                Session("TotalImpuestos") = TotalImpuestos
                lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
                lbltotalImp.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")


            End If
        End If

        If CStr(Session("LeyendaDescuento")) <> "" Then
            lblCarrito.Text = "Carrito de compras (" & Session("LeyendaDescuento") & ")"
            lblDesctxt.Visible = True
        End If

        'If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then

        '    fnAgregaFletesSeguros_StopCatalogo()

        'End If

        '  objDatos.fnLog("Carrito", "slpCode y demas")

        If Session("UserB2C") <> "" Then
            pnlGuardarCarrito.Visible = True
            btnGuardar.Visible = True
            btnProcesar.Visible = True
        End If

        If CInt(Session("slpCode")) = 0 And Session("Cliente") = "" Then
            btnProcesar.Visible = True
        Else
            If CInt(Session("slpCode")) <> 0 Then

                ''Vendedores
                pnlSeparador.Visible = True
                ''Revisamos en la tabla parametrizaciones, si debemos ocultar algún boton

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objDatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '   pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    ' pnlBotonCot.Visible = True

                End If

                btnGuardar.Visible = True

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objDatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If


                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False
                pnlGuardarCarrito.Visible = True
                If Session("UserB2C") <> "" Then
                    pnlDireccionEntrega.Visible = False
                Else
                    pnlDireccionEntrega.Visible = True
                End If

                If Not IsPostBack Then
                    ''Cargamos las direcciones del cliente seleccionado
                    ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                    ssql = ssql.Replace("[%0]", Session("Cliente"))
                    Dim dtdirecciones As New DataTable
                    dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
                    ddlDirecciones.DataSource = dtdirecciones
                    ddlDirecciones.DataTextField = "Direccion"
                    ddlDirecciones.DataValueField = "Direccion"
                    ddlDirecciones.DataBind()

                    Try
                        ''Obtenemos el detalle de la dirección de envio
                        ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                        ssql = ssql.Replace("[%0]", Session("Cliente")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)
                        Dim dtDetalleDireccion As New DataTable
                        dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtDetalleDireccion.Rows.Count > 0 Then
                            Session("CalleEnvio") = dtDetalleDireccion.Rows(0)("Calle")
                            Session("ColoniaEnvio") = dtDetalleDireccion.Rows(0)("Colonia")
                            Session("Ciudadenvio") = dtDetalleDireccion.Rows(0)("Ciudad")

                            Session("NumExtEnvio") = dtDetalleDireccion.Rows(0)("Numero")
                            Session("CPEnvio") = dtDetalleDireccion.Rows(0)("CP")
                            Session("EstadoEnvio") = dtDetalleDireccion.Rows(0)("Estado")
                            'ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                            '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                            ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
                            Dim dtLogin As New DataTable
                            dtLogin = objDatos.fnEjecutarConsulta(ssql)
                            If dtLogin.Rows.Count > 0 Then
                                'txtTelefono.Text = dtLogin.Rows(0)("Tel1")
                                'txtRFC.Text = dtLogin.Rows(0)("RFC")
                                'txtNombre.Text = dtLogin.Rows(0)("cvNombreCompleto")
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                End If

            Else
                '   pnlPayPal.Visible = True
                ' btnProcesar.Visible = True
            End If

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                pnlPayPal.Visible = False
                pnlSeparador.Visible = True
                ''B2B

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objDatos.fnEjecutarConsulta(ssql)
                If dtBotonOferta.Rows.Count > 0 Then
                    If dtBotonOferta.Rows(0)(0) = "ACTIVO" Then
                        btnCotizar.Visible = True
                        '  pnlBotonCot.Visible = True
                    Else
                        btnCotizar.Visible = False
                        ' pnlBotonCot.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnCotizar.Visible = True
                    '  pnlBotonCot.Visible = True
                End If


                btnGuardar.Visible = True
                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='PEDIDO'"
                Dim dtBotonPedido As New DataTable
                dtBotonPedido = objDatos.fnEjecutarConsulta(ssql)
                If dtBotonPedido.Rows.Count > 0 Then
                    If dtBotonPedido.Rows(0)(0) = "ACTIVO" Then
                        btnPedido.Visible = True
                    Else
                        btnPedido.Visible = False
                    End If
                Else
                    ''Por default lo pintamos
                    btnPedido.Visible = True
                End If

                btnProcesar.Visible = False
                btnGuardarPlantilla.Visible = True
                pnlProcesar.Visible = False
                pnlGuardarCarrito.Visible = True
                '  pnlDireccionEntrega.Visible = True

                If Not IsPostBack Then
                    ''Cargamos las direcciones del cliente seleccionado
                    ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                    ssql = ssql.Replace("[%0]", Session("Cliente"))
                    Dim dtdirecciones As New DataTable
                    dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
                    ddlDirecciones.DataSource = dtdirecciones
                    ddlDirecciones.DataTextField = "Direccion"
                    ddlDirecciones.DataValueField = "Direccion"
                    ddlDirecciones.DataBind()

                    Try
                        ''Obtenemos el detalle de la dirección de envio
                        ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                        ssql = ssql.Replace("[%0]", Session("Cliente")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)
                        Dim dtDetalleDireccion2 As New DataTable
                        dtDetalleDireccion2 = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtDetalleDireccion2.Rows.Count > 0 Then
                            Session("CalleEnvio") = dtDetalleDireccion2.Rows(0)("Calle")
                            Session("ColoniaEnvio") = dtDetalleDireccion2.Rows(0)("Colonia")
                            Session("Ciudadenvio") = dtDetalleDireccion2.Rows(0)("Ciudad")

                            Session("NumExtEnvio") = dtDetalleDireccion2.Rows(0)("Numero")
                            Session("CPEnvio") = dtDetalleDireccion2.Rows(0)("CP")
                            Session("EstadoEnvio") = dtDetalleDireccion2.Rows(0)("Estado")
                            'ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                            '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                            ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
                            Dim dtLogin As New DataTable
                            dtLogin = objDatos.fnEjecutarConsulta(ssql)
                            If dtLogin.Rows.Count > 0 Then
                                'txtTelefono.Text = dtLogin.Rows(0)("Tel1")
                                'txtRFC.Text = dtLogin.Rows(0)("RFC")
                                'txtNombre.Text = dtLogin.Rows(0)("cvNombreCompleto")
                            End If
                        End If
                    Catch ex As Exception

                    End Try
                End If


                ssql = "SELECT ISNULL(cvImprime,'NO') FROM [config].[ParametrizacionesB2B]"
                Dim dtImprimeB2B As New DataTable
                dtImprimeB2B = objDatos.fnEjecutarConsulta(ssql)
                If dtImprimeB2B.Rows.Count > 0 Then
                    If dtImprimeB2B.Rows(0)(0) = "NO" Then
                        btnImprimir.Visible = False

                    Else
                        btnImprimir.Visible = True
                    End If
                End If

                ''Si el cliente es BOSS,Revisamos el saldo para forzar a que el cliente pague su pedido
                ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
                Dim dtclienteNom As New DataTable
                dtclienteNom = objDatos.fnEjecutarConsulta(ssql)
                If dtclienteNom.Rows.Count > 0 Then
                    If CStr(dtclienteNom.Rows(0)(0)).ToUpper.Contains("BOSS") Then

                        ssql = objDatos.fnObtenerQuery("GetBalanceCustomer")
                        If ssql <> "" Then
                            ssql = ssql.Replace("[%0]", Session("Cliente"))
                            Dim dtSaldoCliente As New DataTable
                            dtSaldoCliente = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtSaldoCliente.Rows.Count > 0 Then
                                If CDbl(dtSaldoCliente.Rows(0)(0)) > 0 Then
                                    ''Saldo > a límite de crédito
                                    btnCotizar.Visible = False
                                    btnPedido.Visible = False
                                    btnPagar.Visible = True
                                    objDatos.Mensaje("Usted no cuenta con crédito disponible para realizar el pedido. Deberá pagarlo en línea para que podamos procesarlo", Me.Page)
                                End If
                            End If
                        End If

                    End If
                End If


            End If
        End If

        If Session("Cliente") <> "" And Session("UserB2C") = "" Then
            pnlDireccionEntrega.Visible = True
            btnGuardar.Visible = False
        End If

        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "

        dtcliente3 = objDatos.fnEjecutarConsulta(ssql)
        If dtcliente3.Rows.Count > 0 Then
            If CStr(dtcliente3.Rows(0)(0)).ToUpper.Contains("SEGURIT") Then
                pnlDireccionEntrega.Visible = False
            End If
        End If
        If Descuento > 0 Then
            lblDesctxt.Visible = True
        End If
        If pnlHawk.Visible = True Then
            pnlDireccionEntrega.Visible = False
        End If
    End Sub
    Protected Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
        Try

            Session("TotalCarrito") = lblTotal.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), "")
            Session("ImporteSubTotal") = lblSubTotal.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), "")
            If lblDescuento.Text = "" Then
                Session("ImporteDescuento") = 0
            Else
                Session("ImporteDescuento") = lblDescuento.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), "")
            End If

            If lblEnvio.Text = "" Then
                Session("ImporteEnvio") = 0
            Else
                Session("ImporteEnvio") = lblEnvio.Text.Replace(sCaracterMoneda, "").Replace(Session("Moneda"), "")
            End If

            If objDatos.fnObtenerCliente().ToUpper.Contains("STOP CAT") And Session("ImporteEnvio") = 0 Then
                fnAgregaFletesSeguros_StopCatalogo()
            End If

        Catch ex As Exception

        End Try




        ssql = "SELECT ISNULL(cvClienteLAT,'NO') FROM config.Parametrizaciones "
        Dim dtClienteLAT As New DataTable
        dtClienteLAT = objDatos.fnEjecutarConsulta(ssql)
        If dtClienteLAT.Rows.Count = 0 Then
            Response.Redirect("pagoindex.aspx")
        Else
            If dtClienteLAT.Rows(0)(0) = "SI" Then
                ssql = "select ISNULL(cvLigaPagoIndex,'') from config.parametrizaciones"
                Dim dtOtro As New DataTable
                dtOtro = objDatos.fnEjecutarConsulta(ssql)
                If dtOtro.Rows.Count > 0 Then
                    If dtOtro.Rows(0)(0) <> "" Then
                        Response.Redirect(dtOtro.Rows(0)(0))
                    Else
                        Response.Redirect("pagoindexla.aspx")
                    End If

                Else
                    Response.Redirect("pagoindexla.aspx")
                End If

            Else
                Response.Redirect("pagoindex.aspx")
            End If
        End If


    End Sub

    Public Sub fnGuardaCarrito(TipoDoc As String)

        Try
            If txtDate.Text = "" And pnlFechaEntrega.Visible = True Then
                objDatos.Mensaje("Debe indicar la fecha de entrega", Me.Page)
                Exit Sub
            End If


            If txtCodMoneta.Text = "" And pnlMoneta.Visible = True Then
                objDatos.Mensaje("Debe indicar el código Moneta para pagar el pedido", Me.Page)
                Exit Sub
            End If

            If txtRemitente.Text = "" And pnlHawk.Visible = True Then
                objDatos.Mensaje("Debe indicar la fecha de envío", Me.Page)
                Exit Sub
            End If
            If txtDestinatario.Text = "" And pnlHawk.Visible = True Then
                objDatos.Mensaje("Debe indicar el destinatario", Me.Page)
                Exit Sub
            End If

            If txtAtencion.Text = "" And pnlHawk.Visible = True Then
                objDatos.Mensaje("Debe indicar con atención a quién se dirige el documento", Me.Page)
                Exit Sub
            End If

            If ddlTipoEnvio.Visible = True And ddlTipoEnvio.SelectedValue = "0" Then
                objDatos.Mensaje("Debe seleccionar el tipo de envío", Me.Page)
                Exit Sub
            End If

            If ddlPaqueteria.Visible = True And ddlPaqueteria.SelectedValue = "0" Then
                objDatos.Mensaje("Debe seleccionar la paquetería", Me.Page)
                Exit Sub
            End If
        Catch ex As Exception

        End Try


        ''Obtenemos el tipo de cambio de hoy
        objDatos.fnLog("Cotizacion", "Va a obtener el tipo de cambio")
        ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
        Dim dtTc As New DataTable
        dtTc = objDatos.fnEjecutarConsultaSAP(ssql)
        Dim iTC As Double = 1
        If dtTc.Rows.Count > 0 Then
            iTC = dtTc.Rows(0)(0)
        End If

        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
        Dim dtId As New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)

        objDatos.fnLog("Cotizacion", "Antes de IdCarrito")
        Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))

        If Session("UserB2C") <> "" Then
            Session("UserTienda") = Session("UserB2C")
        End If

        Dim miVarSlpCode As Int16 = 0
        ssql = "SELECT isnull(slpCode, 0) FROM OCRD WHERE cardCode= " & " '" & Session("Cliente") & "'"
        Dim dtEmpVentas As New DataTable
        dtEmpVentas = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtEmpVentas.Rows.Count > 0 Then
            If CInt(dtEmpVentas.Rows(0)(0)) <> 0 Then
                miVarSlpCode = CInt(dtEmpVentas.Rows(0)(0))

            End If
        End If

        If CInt(Session("SlpCode")) <> 0 Then
            miVarSlpCode = Session("SlpCode")
        End If


        ssql = "INSERT INTO Tienda.Pedido_HDR ( ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus) VALUES(" _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & Session("UserTienda") & "'," _
            & "'" & Session("NombreuserTienda") & "'," _
            & "'" & miVarSlpCode & "'," _
            & "'" & Session("Cliente") & "'," _
            & "'" & Session("RazonSocial") & "',GETDATE(),''," _
            & "'" & Session("ListaPrecios") & "'," _
            & "'" & TipoDoc & "','ABIERTO')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.fnLog("Cotizacion", "Insertó Hdr en Carrito")
        ''Ahora las lineas
        Dim iTotal As Double = 0
        Dim iContadorPArtidas As Int16 = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                iContadorPArtidas = iContadorPArtidas + 1
                ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_det"
                Dim dtIdLineas As New DataTable
                dtIdLineas = objDatos.fnEjecutarConsulta(ssql)
                objDatos.fnLog("Cotizacion-lineas", Partida.ItemCode)
                ssql = "INSERT INTO Tienda.Pedido_det (ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                  & "'" & dtId.Rows(0)(0) & "'," _
                  & "'" & Session("UserTienda") & "'," _
                  & "'" & Partida.ItemCode & "'," _
                  & "'" & Partida.ItemName.Replace("'", "") & "'," _
                  & "'" & Partida.Cantidad & "'," _
                  & "'" & Partida.Precio.ToString.Replace(",", ".") & "'," _
                  & "'" & Partida.Descuento.ToString.Replace(",", ".") & "')"

                objDatos.fnLog("Cotizacion", ssql.Replace("'", ""))
                objDatos.fnEjecutarInsert(ssql)
                iTotal = iTotal + (Partida.Precio * Partida.Cantidad)
            End If
        Next
        objDatos.fnLog("Cotizacion", "Insertó las lineas")

        ssql = "UPDATE Tienda.Pedido_HDR  SET cfTotal=" & "'" & iTotal.ToString.Replace(",", ".") & "',cfTipoCambio=" & "'" & iTC.ToString.Replace(",", ".") & "',cfTotalFC=" & "'" & (iTotal * iTC).ToString.Replace(",", ".") & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
        objDatos.fnEjecutarInsert(ssql)
        If (objDatos.fnObtenerCliente().ToUpper.Contains("SUJEA") Or objDatos.fnObtenerCliente().ToUpper.Contains("MANIJ")) Then
            ''Lo sacamos por motor
            ssql = "UPDATE Tienda.Pedido_HDR  SET cvMotor='SI' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
            objDatos.fnEjecutarInsert(ssql)

            ssql = "UPDATE Tienda.Pedido_HDR  SET cvComentarios='" & txtOtros.Text & "',cvTipoEnvio='" & ddlTipoEnvio.SelectedValue & "',cvPaqueteria='" & ddlPaqueteria.SelectedValue & "',cvUsoCFDI='" & ddlUsoCFDI.SelectedValue & "',cvOC='" & txtOC_.Text & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
            objDatos.fnEjecutarInsert(ssql)



        End If

        Dim sDireccion As String = ""
        If ddlDirecciones.Items.Count > 0 Then
            sDireccion = ddlDirecciones.SelectedValue
            ssql = "UPDATE Tienda.Pedido_HDR  SET cvDireccion=" & "'" & sDireccion & "',cvIdDireccion='' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
            objDatos.fnEjecutarInsert(ssql)
        End If
        Try
            If (objDatos.fnObtenerCliente().ToUpper.Contains("SUJEA") Or objDatos.fnObtenerCliente().ToUpper.Contains("MANIJ")) And txtDireccion.Text <> "" Then
                Dim sResultadoActualizarDireccion As String = ""
                Dim oCompany As New SAPbobsCOM.Company
                oCompany = objDatos.fnConexion_SAP
                If oCompany.Connected Then
                    sResultadoActualizarDireccion = fnAgregarDireccionCliente(Session("Cliente"), oCompany)
                End If

                If sResultadoActualizarDireccion <> "ERROR" Then
                    sDireccion = sResultadoActualizarDireccion
                Else
                    sDireccion = "ERR-" & txtDireccion.Text
                End If

                ssql = "UPDATE Tienda.Pedido_HDR  SET cvDireccion=" & "'" & sDireccion & "',cvIdDireccion='' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
                objDatos.fnEjecutarInsert(ssql)
            End If

        Catch ex As Exception

        End Try




        objDatos.fnLog("Cotizacion", "Terminó en tablas")

        lblMensaje.Text = "Carrito guardado"
        ' objDatos.Mensaje("Compra procesada", Me.Page)
        If TipoDoc <> "CARRITO" Then
            ssql = "SELECT ISNULL(cvUsaMotor,'NO') FROM config.parametrizaciones "
            Dim dtUsaMotor As New DataTable
            dtUsaMotor = objDatos.fnEjecutarConsulta(ssql)
            If dtUsaMotor.Rows.Count > 0 Then
                If dtUsaMotor.Rows(0)(0) = "NO" Then
                    objDatos.fnLog("Cotizacion", "Va a procesar SAP")
                    If iContadorPArtidas > 30 And (objDatos.fnObtenerCliente().ToUpper.Contains("SUJEA") Or objDatos.fnObtenerCliente().ToUpper.Contains("MANIJ")) And 1 = 2 Then ''Forzamos que de momento no entre
                        ''Lo mandamos a motor
                        objDatos.Mensaje(TipoDoc & " se ha procesado correctamente", Me.Page)
                        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                        Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                        Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
                    Else
                        fnProcesarSAP(iIdCarrito, TipoDoc)
                    End If

                Else
                    objDatos.Mensaje(TipoDoc & " se ha procesado correctamente", Me.Page)
                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                    Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                    Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
                End If
            Else
                objDatos.fnLog("Cotizacion", "Va a procesar SAP")
                fnProcesarSAP(iIdCarrito, TipoDoc)
            End If

        End If
        btnCotizar.Enabled = False
        btnPedido.Enabled = False
        btnGuardar.Enabled = False

    End Sub

    Public Function fnAsignaAlmacen(itemCode As String)
        Dim almacen As String = ""

        ssql = "select cvWhsCode  from config.Existencias where cvEstatus ='ACTIVO' order by ciOrden "
        Dim dtAlmacenes As New DataTable
        dtAlmacenes = objDatos.fnEjecutarConsulta(ssql)

        ''El primer almacen
        If dtAlmacenes.Rows.Count > 0 Then
            ssql = "SELECT OnHand FROM OITW WHERE itemCode=" & "'" & itemCode & "' AND whsCode=" & "'" & dtAlmacenes.Rows(0)(0) & "'"
            Dim dtExistencias As New DataTable
            dtExistencias = objDatos.fnEjecutarConsultaSAP(ssql)
            objDatos.fnLog("Asigna almacen", ssql.Replace("'", ""))
            If dtExistencias.Rows.Count > 0 Then
                If CDbl(dtExistencias.Rows(0)(0)) = 0 Then
                    ''Revisamos el segundo
                    ssql = "SELECT OnHand FROM OITW WHERE itemCode=" & "'" & itemCode & "' AND whsCode=" & "'" & dtAlmacenes.Rows(1)(0) & "'"
                    Dim dtExistencias2 As New DataTable
                    dtExistencias2 = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtExistencias2.Rows.Count > 0 Then
                        If dtExistencias2.Rows(0)(0) > 0 Then
                            almacen = dtAlmacenes.Rows(1)(0)
                        Else
                            ''si no hay existencia en el almacén 2, tomamos el 1
                            almacen = dtAlmacenes.Rows(0)(0)
                        End If

                    End If

                Else
                    almacen = dtAlmacenes.Rows(0)(0)
                End If
            End If

        Else
            ''Se define por otro método
            ssql = objDatos.fnObtenerQuery("Almacenes")
            ssql = ssql.Replace("[%0]", Session("Cliente"))
            dtAlmacenes = objDatos.fnEjecutarConsulta(ssql)
            If dtAlmacenes.Rows.Count > 0 Then
                almacen = dtAlmacenes.Rows(0)(0)
            End If
        End If

        Return almacen
    End Function

    Public Function fnAgregarDireccionCliente(CardCode As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        If txtDireccion.Text.Length > 30 Then
            sIdDireccion = txtDireccion.Text.Substring(0, 30)
        Else
            sIdDireccion = txtDireccion.Text
        End If
        objDatos.fnLog("Direccion cliente", CardCode)
        Dim oProspecto As SAPbobsCOM.BusinessPartners
        oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
        objDatos.fnLog("Direccion cliente", "1")
        oProspecto.GetByKey(CardCode)
        oProspecto.Addresses.Add()
        objDatos.fnLog("Direccion cliente", "2")
        oProspecto.Addresses.TypeOfAddress = "S"
        oProspecto.Addresses.AddressName = "Envio-" & sIdDireccion
        oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo

        objDatos.fnLog("Direccion cliente", "3")
        oProspecto.Addresses.County = txtMunicipio.Text
        Try
            oProspecto.Addresses.Block = txtColonia.Text
        Catch ex As Exception

        End Try

        oProspecto.Addresses.Street = txtDireccion.Text
        Try
            oProspecto.Addresses.State = ddlEstados.SelectedValue
        Catch ex As Exception

        End Try
        Try
            oProspecto.Addresses.Country = ddlPais.SelectedValue
        Catch ex As Exception

        End Try

        oProspecto.Addresses.ZipCode = txtCP.Text
        objDatos.fnLog("Direccion cliente", "4")
        If oProspecto.Update() <> 0 Then
            sIdDireccion = "ERROR"

        End If
        sIdDireccion = "Envio-" & sIdDireccion
        Return sIdDireccion
    End Function
    Public Function fnProcesarSAP(idCarrito As Int64, TipoDoc As String)

        Session("TIPODOC") = TipoDoc
        Session("PDF_Correo") = ""

        Dim sVieneDE As String = ""

        If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
            sVieneDE = " - C"
        Else
            sVieneDE = " - V"
        End If
        ssql = "select ISNULL(cvPreciosMasIVA,'NO') from config.parametrizaciones"
        Dim dtPreciosIVA As New DataTable
        dtPreciosIVA = objDatos.fnEjecutarConsulta(ssql)
        Dim sPreciosMasIVA As String = "NO"
        If dtPreciosIVA.Rows.Count > 0 Then
            sPreciosMasIVA = dtPreciosIVA.Rows(0)(0)
        End If

        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus,ISNULL(cfTotal,0) as cfTotal FROM Tienda.Pedido_HDR WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objDatos.fnEjecutarConsulta(ssql)

        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtPartidas As New DataTable
        dtPartidas = objDatos.fnEjecutarConsulta(ssql)
        Dim sArticulosSinStock As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Dim sCardCode As String = ""
        Dim iBandArtSinExistencias As Int16 = 0
        Try
            oCompany = objDatos.fnConexion_SAP
            If oCompany.Connected Then


                If TipoDoc = "COTIZACION" Then
                    objDatos.fnLog("Cotizacion", "Crea objeto Company")
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)


                    'ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                    'Dim dtcliente As New DataTable
                    'dtcliente = objDatos.fnEjecutarConsulta(ssql)
                    'If dtcliente.Rows.Count > 0 Then
                    '    If dtcliente.Rows(0)(0) = "Bacán" Then
                    '        oDoctoVentas.DocDueDate = Now.Date.AddDays(1)

                    '    End If
                    'End If

                    ' oDoctoVentas.Series = 59
                End If
                objDatos.fnLog("Cotizacion", "Antes tipo slpcode")
                If TipoDoc = "COTIZACION" Then
                    If CInt(Session("slpCode")) <> 0 Then
                        ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='OFERTA'  and cvTipo='Vendedores'"
                    Else
                        If Session("Cliente") <> "" And CInt(Session("slpCode")) = 0 Then

                        End If
                        ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='OFERTA' and cvTipo='B2B'"
                    End If


                End If
                If TipoDoc = "PEDIDO" Then
                    If CInt(Session("slpCode")) <> 0 Then
                        ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='PEDIDO' and cvTipo='Vendedores'"
                    Else
                        ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='PEDIDO' and cvTipo='B2B' "
                    End If


                End If

                objDatos.fnLog("Cotizacion", "Query serie: " & ssql.Replace("'", ""))
                Dim dtSerie As New DataTable
                dtSerie = objDatos.fnEjecutarConsulta(ssql)
                If dtSerie.Rows.Count > 0 Then
                    If CStr(dtSerie.Rows(0)(0)) <> "" Then
                        oDoctoVentas.Series = dtSerie.Rows(0)(0)
                    End If
                End If

                objDatos.fnLog("Cotizacion", "Agente SAP: " & CInt(Session("slpCode")))
                sCardCode = dtEncabezado.Rows(0)("cvCveCliente")
                Dim MislpCode As Int16
                'If CInt(Session("slpCode")) = 0 Then
                '    ssql = "SELECT isnull(slpCode, 0) FROM OCRD WHERE cardCode= " & " '" & sCardCode & "'"
                '    Dim dtEmpVentas As New DataTable
                '    dtEmpVentas = objDatos.fnEjecutarConsultaSAP(ssql)
                '    If dtEmpVentas.Rows.Count > 0 Then
                '        If CInt(dtEmpVentas.Rows(0)(0)) <> 0 Then
                '            MislpCode = CInt(dtEmpVentas.Rows(0)(0))
                '            oDoctoVentas.SalesPersonCode = CInt(dtEmpVentas.Rows(0)(0))
                '        End If
                '    End If

                'Else
                '    MislpCode = CInt(Session("slpCode"))
                '    oDoctoVentas.SalesPersonCode = CInt(Session("slpCode"))

                'End If
                objDatos.fnLog("Cotizacion", "cvCveCliente: " & dtEncabezado.Rows(0)("cvCveCliente"))
                oDoctoVentas.CardCode = dtEncabezado.Rows(0)("cvCveCliente")
                oDoctoVentas.DocDate = Now.Date

                Try
                    If pnlPaqueteria.Visible = True Then
                        oDoctoVentas.TransportationCode = ddlPaqueteria.SelectedValue
                    End If
                Catch ex As Exception

                End Try


                If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("AIO") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then

                    ssql = "SELECT isnull(U_B1SYS_MainUsage, 'G01') FROM OCRD WHERE cardCode= " & " '" & sCardCode & "'"
                    Dim dtUsoCFDI As New DataTable
                    dtUsoCFDI = objDatos.fnEjecutarConsultaSAP(ssql)

                    If dtUsoCFDI.Rows.Count > 0 Then
                        oDoctoVentas.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = dtUsoCFDI.Rows(0)(0)
                    End If

                    oDoctoVentas.UserFields.Fields.Item("U_MetodoPago").Value = "PPD"
                    oDoctoVentas.NumAtCard = "Desde Internet"
                End If
                If pnlFechaEntrega.Visible = True Then
                    oDoctoVentas.DocDueDate = txtDate.Text
                Else
                    oDoctoVentas.DocDueDate = Now.Date
                End If

                If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("SUJEA") Or CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("MANIJ") Then
                    If txtOtros.Text <> "" Then
                        Try
                            oDoctoVentas.NumAtCard = txtOC_.Text
                            oDoctoVentas.UserFields.Fields.Item("U_Comentarios").Value = "Desde Internet:" & txtOtros.Text & sVieneDE
                            oDoctoVentas.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = ddlUsoCFDI.SelectedValue
                        Catch ex As Exception

                        End Try

                    End If
                Else
                    If txtOtros.Text <> "" Then
                        oDoctoVentas.Comments = "Desde Internet:" & txtOtros.Text & sVieneDE
                    Else
                        oDoctoVentas.Comments = "Desde Internet " & sVieneDE
                    End If
                End If





                Try
                    If ddlTipoEnvio.Visible = True Then
                        oDoctoVentas.UserFields.Fields.Item("U_Tipo_Envio").Value = ddlTipoEnvio.SelectedValue
                    End If

                Catch ex As Exception

                End Try
                Try
                    If ddlTransporting.Visible = True Then
                        oDoctoVentas.TransportationCode = ddlTransporting.SelectedValue
                    End If

                Catch ex As Exception

                End Try



                If pnlHawk.Visible = True Then
                    Try
                        oDoctoVentas.UserFields.Fields.Item("U_TipoEnvio").Value = ddlTipodeEnvio.SelectedValue
                        oDoctoVentas.UserFields.Fields.Item("U_Destinat_envio").Value = txtDestinatario.Text
                        oDoctoVentas.UserFields.Fields.Item("U_Remitente").Value = txtRemitente.Text
                        oDoctoVentas.UserFields.Fields.Item("U_Atencion").Value = txtAtencion.Text
                        oDoctoVentas.NumAtCard = txtRef.Text
                    Catch ex As Exception

                    End Try


                End If

                Try
                    If ddlProyecto.Visible = True Then
                        If txtProyecto.Text = "" Then
                            oDoctoVentas.Project = ddlProyecto.SelectedValue
                        Else
                            'Agregamos nuevo proyecto
                            Dim oCmpSrv As SAPbobsCOM.CompanyService
                            Dim projectService As SAPbobsCOM.IProjectsService
                            Dim project As SAPbobsCOM.IProject

                            oCmpSrv = oCompany.GetCompanyService
                            projectService = oCmpSrv.GetBusinessService(SAPbobsCOM.ServiceTypes.ProjectsService)

                            project = projectService.GetDataInterface(SAPbobsCOM.ProjectsServiceDataInterfaces.psProject)

                            ssql = "select * from OPRJ where PrjCode like 'EC-%' order by PrjCode desc "
                            Dim codProyecto As String = ""
                            Dim dtProyectos As New DataTable
                            dtProyectos = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtProyectos.Rows.Count = 0 Then
                                ''Es el primero
                                codProyecto = "EC-00001"
                            Else

                                codProyecto = dtProyectos.Rows(0)(0)
                                Dim sCod As String() = codProyecto.Split("-")
                                Dim iCons As Int16 = 0
                                iCons = CInt(sCod(1))
                                iCons = iCons + 1
                                codProyecto = "EC-" + padLeft(iCons, 5)
                            End If
                            project.Code = codProyecto
                            project.Name = txtProyecto.Text
                            projectService.AddProject(project)

                            oDoctoVentas.Project = codProyecto
                        End If

                    End If

                Catch ex As Exception

                End Try

                Try
                    If ddlEstadoPedido.Visible = True Then
                        oDoctoVentas.UserFields.Fields.Item("U_Estado2").Value = ddlEstadoPedido.SelectedValue
                    End If

                Catch ex As Exception

                End Try


                Try
                    ''Obtener RFC

                    oDoctoVentas.UserFields.Fields.Item("U_FacNit").Value = "7268540-9"
                    oDoctoVentas.UserFields.Fields.Item("U_Vendedor").Value = "Vendedor"
                    oDoctoVentas.UserFields.Fields.Item("U_FacNom").Value = dtEncabezado.Rows(0)("cvCliente")
                Catch ex As Exception

                End Try



                Try

                    Dim sQuerySuc As String = ""
                    sQuerySuc = objDatos.fnObtenerQuery("ObtenerSucursal")
                    If sQuerySuc <> "" Then
                        Try
                            If CInt(Session("slpCode")) = 0 Then
                                sQuerySuc = sQuerySuc.Replace("[%0]", MislpCode)
                            Else
                                sQuerySuc = sQuerySuc.Replace("[%0]", CInt(Session("slpCode")))
                            End If

                            sQuerySuc = sQuerySuc.Replace("[%1]", dtEncabezado.Rows(0)("cvCliente"))
                        Catch ex As Exception

                        End Try

                        objDatos.fnLog("Sucursal", sQuerySuc.Replace("'", ""))
                        Dim sSucursalVend As String = ""
                        Dim dtSucursalVend As New DataTable
                        dtSucursalVend = objDatos.fnEjecutarConsultaSAP(sQuerySuc)
                        If dtSucursalVend.Rows.Count > 0 Then
                            sSucursalVend = dtSucursalVend.Rows(0)(0)
                            oDoctoVentas.UserFields.Fields.Item("U_SUCURSAL").Value = sSucursalVend
                        End If

                    End If


                Catch ex As Exception

                End Try
                Dim iLinea As Int16 = 0
                Dim sArticulosPedido As String = ""
                objDatos.fnLog("Cotizacion", "Antes lineas")
                Dim dtKits As New DataTable
                dtKits.Columns.Add("Kit")
                dtKits.Columns.Add("Procesado")

                Try
                    If ddlDirecciones.Items.Count > 0 Then
                        oDoctoVentas.ShipToCode = ddlDirecciones.SelectedValue
                    End If
                    If txtDireccion.Text <> "" Then
                        Dim sResultadoActualizarDireccion As String = ""
                        sResultadoActualizarDireccion = fnAgregarDireccionCliente(sCardCode, oCompany)
                        If sResultadoActualizarDireccion <> "ERROR" Then
                            oDoctoVentas.ShipToCode = sResultadoActualizarDireccion
                        Else
                            oDoctoVentas.Address = txtDireccion.Text
                        End If

                    End If
                Catch ex As Exception

                End Try

                ''Revisamos si el vendedor tiene dimensiones (Hawk)
                Dim sUsaDimensiones As String = "NO"
                Dim ssqlDim As String = ""
                Dim dtDimensiones As New DataTable

                ssqlDim = objDatos.fnObtenerQuery("DimensionesVendedor")
                If ssqlDim <> "" Then
                    ssqlDim = ssqlDim.Replace("[%0]", MislpCode)
                    objDatos.fnLog("Dimensiones", ssqlDim.Replace("'", ""))

                    dtDimensiones = objDatos.fnEjecutarConsultaSAP(ssqlDim)
                    If dtDimensiones.Rows.Count > 0 Then
                        sUsaDimensiones = "SI"
                    End If
                End If
                Dim sIVA As String = ""
                Dim sMonedaDocumento As String = ""
                For i = 0 To dtPartidas.Rows.Count - 1 Step 1

                    ''Validacion Precio
                    Dim dPrecioCarrito As Double = 0
                    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                        If Partida.ItemCode <> "BORRAR" Then
                            If Partida.ItemCode = dtPartidas.Rows(i)("cvItemCode") And Partida.Descuento = dtPartidas.Rows(i)("cfDescuento") Then
                                dPrecioCarrito = Partida.Precio
                            End If
                        End If
                    Next



                    oDoctoVentas.Lines.Add()
                    oDoctoVentas.Lines.SetCurrentLine(iLinea)
                    If fnRevisaExistencias(dtPartidas.Rows(i)("cvItemCode")) = 0 Then
                        sArticulosSinStock = sArticulosSinStock & dtPartidas.Rows(i)("cvItemCode") & vbCrLf
                    End If
                    oDoctoVentas.Lines.ItemCode = dtPartidas.Rows(i)("cvItemCode")
                    oDoctoVentas.Lines.ItemDescription = dtPartidas.Rows(i)("cvItemName")
                    oDoctoVentas.Lines.Quantity = dtPartidas.Rows(i)("cfCantidad")

                    If fnRevisaExistencias(dtPartidas.Rows(i)("cvItemCode")) <= 0 Then
                        iBandArtSinExistencias = 1
                    End If

                    ''---DIMENSIONES (HAWK)
                    If sUsaDimensiones = "SI" Then
                        If dtDimensiones.Rows(0)("Dim1") <> "" Then
                            oDoctoVentas.Lines.CostingCode = dtDimensiones.Rows(0)("Dim1")
                        End If
                        If dtDimensiones.Rows(0)("Dim2") <> "" Then
                            oDoctoVentas.Lines.CostingCode2 = dtDimensiones.Rows(0)("Dim2")
                        End If
                        If dtDimensiones.Rows(0)("Dim3") <> "" Then
                            oDoctoVentas.Lines.CostingCode3 = dtDimensiones.Rows(0)("Dim3")
                        End If



                    End If
                    ''---DIMENSIONES (HAWK)


                    If pnlOtros.Visible = True Then
                        Try
                            oDoctoVentas.Lines.UserFields.Fields.Item("U_OTROS").Value = txtOtros.Text
                        Catch ex As Exception

                        End Try

                    End If

                    ''Vemos si tenemos centro de costos
                    Dim sQueryCC As String = ""
                    sQueryCC = objDatos.fnObtenerQuery("ObtenerCentroCostos")
                    If sQueryCC <> "" Then
                        Try
                            objDatos.fnLog("CentroCostos", sQueryCC.Replace("'", ""))
                            If CInt(Session("slpCode")) = 0 Then
                                sQueryCC = sQueryCC.Replace("[%0]", MislpCode)
                            Else
                                sQueryCC = sQueryCC.Replace("[%0]", Session("slpCode"))
                            End If

                            sQueryCC = sQueryCC.Replace("[%1]", dtPartidas.Rows(i)("cvCveCliente"))
                        Catch ex As Exception

                        End Try


                        Dim sCentroCostos As String = ""
                        Dim dtCentro As New DataTable
                        dtCentro = objDatos.fnEjecutarConsultaSAP(sQueryCC)
                        If dtCentro.Rows.Count > 0 Then
                            sCentroCostos = dtCentro.Rows(0)(0)
                            oDoctoVentas.Lines.CostingCode = sCentroCostos
                        End If

                    End If


                    ''Validamos si algun itemcode es un kit, para actualizarlo despues
                    ssql = "SELECT Father,ChildNum ,code,Price ,PriceList  from ITT1 where father='" & dtPartidas.Rows(i)("cvItemCode") & "'"
                    Dim dtEsKit As New DataTable
                    dtEsKit = objDatos.fnEjecutarConsultaSAP(ssql)
                    If dtEsKit.Rows.Count > 0 Then
                        Dim filaKit As DataRow
                        filaKit = dtKits.NewRow
                        filaKit("Kit") = dtPartidas.Rows(i)("cvItemCode")
                        filaKit("Procesado") = "NO"
                        dtKits.Rows.Add(filaKit)
                    End If

                    '   oDoctoVentas.Lines.Price = dtPartidas.Rows(i)("cfPrecio")
                    oDoctoVentas.Lines.Currency = fnObtenerMoneda(dtPartidas.Rows(i)("cvItemCode"))
                    sMonedaDocumento = fnObtenerMoneda(dtPartidas.Rows(i)("cvItemCode"))
                    Dim PrecioBD As Double = 0
                    PrecioBD = CDbl(dtPartidas.Rows(i)("cfPrecio"))

                    If Session("RazonSocial") <> "" And CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                        ''---B2B y vendedores Hawk
                        oDoctoVentas.Lines.PriceAfterVAT = PrecioBD * (1 - (CDbl(dtPartidas.Rows(i)("cfDescuento")) / 100))
                    Else

                        If sPreciosMasIVA = "SI" Then


                            If PrecioBD < dPrecioCarrito Then
                                PrecioBD = dPrecioCarrito
                            End If


                            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("AIO") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("PMK") Or CStr(objDatos.fnObtenerCliente).ToUpper.Contains("SUJEA") Then
                                oDoctoVentas.Lines.Price = PrecioBD
                            Else
                                oDoctoVentas.Lines.PriceAfterVAT = PrecioBD
                            End If


                        Else
                            oDoctoVentas.Lines.Price = PrecioBD
                        End If
                    End If



                    objDatos.fnLog("Cotizacion", "Antes IVA")
                    ''Obtener Indicador de IVA
                    ssql = objDatos.fnObtenerQuery("ObtenerIVA")
                    If ssql <> "" Then

                        ssql = ssql.Replace("[%0]", dtPartidas.Rows(i)("cvItemCode"))
                        ssql = ssql.Replace("[%1]", sCardCode)
                        Dim dtIVA As New DataTable
                        dtIVA = objDatos.fnEjecutarConsultaSAP(ssql)

                        objDatos.fnLog("Impuestos", ssql.Replace("'", ""))


                        If dtIVA.Rows.Count > 0 Then
                            sIVA = dtIVA.Rows(0)(0)
                            objDatos.fnLog("Asigna Impuestos", dtIVA.Rows(0)(0))

                            oDoctoVentas.Lines.TaxCode = dtIVA.Rows(0)(0)
                        End If

                    End If


                    If CDbl(dtPartidas.Rows(i)("cfDescuento")) > 0 Then
                        objDatos.fnLog("Asigna descuento", dtPartidas.Rows(i)("cfDescuento"))
                        oDoctoVentas.Lines.DiscountPercent = dtPartidas.Rows(i)("cfDescuento")
                    End If
                    ' oDoctoVentas.Lines.LineTotal = dtPartidas.Rows(i)("cfPrecio") * (1 - (dtPartidas.Rows(i)("cfDescuento") / 100))
                    Dim sAlmacen As String = ""
                    sAlmacen = fnAsignaAlmacen(dtPartidas.Rows(i)("cvItemCode"))
                    objDatos.fnLog("Asigna almacen", sAlmacen)

                    If objDatos.fnObtenerCliente.ToUpper.Contains("ZEYCO") And Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then

                    Else

                        If sAlmacen <> "" And CInt(Session("slpCode")) <> 0 Or (Session("RazonSocial") <> "" And sAlmacen <> "") Then
                            oDoctoVentas.Lines.WarehouseCode = sAlmacen
                        End If

                    End If


                    If ddlAlmacen.Visible = True Then
                        oDoctoVentas.Lines.WarehouseCode = ddlAlmacen.SelectedValue
                    End If

                    sArticulosPedido = sArticulosPedido & String.Format("{0,200} {1,25}{2}{2}",
                                dtPartidas.Rows(i)("cvItemName"), dtPartidas.Rows(i)("cfCantidad"), vbCrLf)
                    iLinea = iLinea + 1
                Next

                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("HAWK") Then
                    oDoctoVentas.DocCurrency = sMonedaDocumento

                End If


                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP_CAT") Then
                    If Session("ImporteEnvio") > 0 Then
                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.ItemCode = "FLETE"
                        oDoctoVentas.Lines.Quantity = 1
                        oDoctoVentas.Lines.UnitPrice = CDbl(Session("ImporteEnvio"))
                        '    oDoctoVentas.Lines.WarehouseCode = "WEB"
                        If sIVA <> "" Then
                            oDoctoVentas.Lines.TaxCode = sIVA
                        End If

                    End If
                End If
                objDatos.fnLog("Cotizacion", "antes del Add")
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    objDatos.fnLog("Cotizacion", "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    objDatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""), Me.Page)

                    ''Si cae en error, desbloqueamos los botones
                    btnCotizar.Enabled = True
                    btnPedido.Enabled = True
                Else
                    ''Todo bien

                    ''Matamos la cookie de carrito
                    Try
                        Dim cookie As HttpCookie
                        cookie = HttpContext.Current.Request.Cookies("carrito")
                        cookie.Value = ""
                        HttpContext.Current.Response.Cookies.Add(cookie)
                    Catch ex As Exception

                    End Try

                    ''Continuamos
                    Dim sDocnum As String = ""
                    Dim dtDoc As New DataTable
                    Session("DocEntry") = oCompany.GetNewObjectKey


                    'ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                    'Dim dtcliente As New DataTable
                    'dtcliente = objDatos.fnEjecutarConsulta(ssql)
                    'If dtcliente.Rows.Count > 0 Then
                    '    If CStr(dtcliente.Rows(0)(0)).Contains("STOP") Then
                    '        fnGeneraPagoSAP(sCardCode, Session("DocEntry"), lbltotalImp.Text.Replace("MXP", ""), "", idCarrito, sDocnum)
                    '    End If
                    'End If


                    ''Revisamos si hubo un kit , para re-abrir el documento y actualizar precios
                    Dim sTablaDetalle As String = ""
                    If dtKits.Rows.Count > 0 Then

                        If TipoDoc = "COTIZACION" Then
                            sTablaDetalle = "QUT1"
                            oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                        End If
                        If TipoDoc = "PEDIDO" Then
                            sTablaDetalle = "RDR1"
                            oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
                        End If
                        Try
                            objDatos.fnLog("Actualizar documento", "Va a obtener")
                            oDoctoVentas.GetByKey(Session("DocEntry"))
                            ssql = "Select * from " & sTablaDetalle & " WHERE DocEntry= " & "'" & Session("DocEntry") & "'"

                            objDatos.fnLog("Actualizar documento SQL: ", ssql.Replace("'", ""))

                            Dim dtLineasDoctoSAP As New DataTable
                            dtLineasDoctoSAP = objDatos.fnEjecutarConsultaSAP(ssql)
                            For i = 0 To dtLineasDoctoSAP.Rows.Count - 1 Step 1
                                For x = 0 To dtKits.Rows.Count - 1 Step 1
                                    If dtLineasDoctoSAP.Rows(i)("ItemCode") = dtKits.Rows(x)("Kit") And dtKits.Rows(x)("Procesado") = "NO" Then
                                        objDatos.fnLog("Actualizar documento", "Hay kit: " & dtKits.Rows(x)("Kit"))
                                        ''Sacamos los hijos de ese kit
                                        dtKits.Rows(x)("Procesado") = "SI"
                                        ssql = "select Father,ChildNum ,code,Price ,PriceList  from ITT1 where father='" & dtKits.Rows(x)("Kit") & "'"
                                        Dim dtLineasKit As New DataTable
                                        dtLineasKit = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtLineasKit.Rows.Count > 1 Then
                                            For z = 0 To dtLineasKit.Rows.Count - 1 Step 1
                                                oDoctoVentas.Lines.SetCurrentLine(i + z + 1)

                                                Dim PrecioNuevo As Double = 0
                                                PrecioNuevo = objDatos.fnPrecioActual(dtLineasKit.Rows(z)("Code"), dtLineasKit.Rows(z)("PriceList"))
                                                objDatos.fnLog("Actualizar documento", "Línea: " & (i + z + 1) & " Hijo: " & dtLineasKit.Rows(z)("Code") & " Precio: " & PrecioNuevo)
                                                oDoctoVentas.Lines.UnitPrice = PrecioNuevo

                                            Next
                                        End If

                                    End If
                                Next

                            Next
                            objDatos.fnLog("Actualizar documento", "Antes de UPDATE")
                            If oDoctoVentas.Update() <> 0 Then
                                objDatos.fnLog("Actualizar documento ERROR: ", oCompany.GetLastErrorDescription.Replace("'", ""))
                            End If
                        Catch ex As Exception

                        End Try

                    End If




                    If TipoDoc = "PEDIDO" Then
                        sTablaDetalle = "RDR1"
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumOrdenVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)

                    End If
                    If TipoDoc = "COTIZACION" Then
                        sTablaDetalle = "QUT1"
                        ssql = objDatos.fnObtenerQuery("ObtenerDocNumOfertaVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objDatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If dtDoc.Rows.Count > 0 Then
                        sDocnum = dtDoc.Rows(0)(0)
                        ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                        objDatos.fnEjecutarInsert(ssql)

                    End If
                    objDatos.Mensaje(TipoDoc & " procesado correctamente ", Me.Page)
                    Try
                        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                        Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                        Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

                        If sArticulosSinStock <> "" Then
                            ''Tenemos articulos sin stock...notificamos
                            ''Obtenemos correo de notificacion
                            ssql = "select ISNULL(cvNotificaExistencia,'NO'),ISNULL(cvCorreoNotificaExistencia,'') FROM config.parametrizaciones"
                            Dim dtNotifica As New DataTable
                            dtNotifica = objDatos.fnEjecutarConsulta(ssql)
                            If dtNotifica.Rows.Count > 0 Then
                                If dtNotifica.Rows(0)(0) <> "NO" Then
                                    Dim sBody As String = ""
                                    sBody = "Se ha generado un " & TipoDoc & " desde el portal de Ecommerce: " & sDocnum & vbCrLf
                                    sBody = sBody & "Sin embargo, los siguientes artículos no tienen stock:" & vbCrLf
                                    sBody = sBody & sArticulosSinStock
                                    If dtNotifica.Rows(0)(1) <> "" Then
                                        objDatos.fnEnviarCorreo(dtNotifica.Rows(0)(1), sBody, "Notificaciones ECommerce: Artículos sin stock")
                                    End If

                                End If
                            End If
                        End If

                        ''Enviamos correo
                        Dim text As String
                        If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") Then
                            If (Session("RazonSocial") <> "") Then
                                text = MensajeHTML(Server.MapPath("~") & "\correo_A_B2B.html")
                            End If

                        Else
                            text = MensajeHTML(Server.MapPath("~") & "\correo_A_B2B.html")
                        End If



                        ssql = "select ISNULL(cvImprimeDocumento,'NO') FROM config.parametrizaciones"
                        Dim dtImprime As New DataTable
                        dtImprime = objDatos.fnEjecutarConsulta(ssql)
                        If dtImprime.Rows.Count > 0 Then
                            If dtImprime.Rows(0)(0) = "SI" Then
                                pnlImprimir.Visible = True
                                btnImprimir.Visible = True
                            End If
                        End If

                        'If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                        '    pnlImprimir.Visible = False
                        '    btnImprimir.Visible = False
                        'End If

                        Dim sDestinatario As String = ""
                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente2 As New DataTable
                        dtcliente2 = objDatos.fnEjecutarConsulta(ssql)

                        objDatos.fnLog("Confirmacion-cliente", dtcliente2.Rows(0)(0))

                        Try
                            text = text.Replace("{nombrecliente}", dtcliente2.Rows(0)(0))
                            text = text.Replace("{nombrecliente2}", Session("RazonSocial"))
                            text = text.Replace("{enviara}", "")
                            text = text.Replace("{totalDoc}", dtEncabezado.Rows(0)("cfTotal"))
                            text = text.Replace("{direccionenvio}", "" & "</br> " & Session("Comentarios"))
                            text = text.Replace("{metodoenvio}", "")
                            text = text.Replace("{numpedido}", dtDoc.Rows(0)(0))
                            text = text.Replace("{fechapedido}", Now.Date.ToShortDateString)
                            text = text.Replace("{TipoDoc}", TipoDoc)

                            If Session("Cliente") <> "" Then

                                If iBandArtSinExistencias = 1 Then
                                    text = text.Replace("{LeyendaExistencia}", "* Solicita tiempo de entrega ítem sin existencias")
                                Else
                                    text = text.Replace("{LeyendaExistencia}", "")
                                End If

                            End If
                        Catch ex As Exception
                            objDatos.fnLog("Ex en Text", ex.Message.Replace("'", ""))
                        End Try


                        If sTablaDetalle = "" Then
                            sTablaDetalle = "RDR1"
                        End If

                        Dim sDBMS As String = "SQL"

                        If objDatos.fnObtenerDBMS = "HANA" Then
                            sDBMS = "HANA"
                            ssql = objDatos.fnObtenerQuery("GetItemsMail")
                            ssql = ssql.Replace("[%0]", sTablaDetalle)
                            ssql = ssql.Replace("[%1]", Session("DocEntry"))
                        Else
                            If sMonedaDocumento = "USD" Then
                                ssql = "SELECT  itemCode as cvItemCode, Dscription as cvItemName,Quantity as cfCantidad,Price as cfPrecio,DiscPrcnt as cfDescuento,VatSumFrgn as VatSum , TotalFrgn AS LineTotal,Currency  FROM " & sTablaDetalle & " WHERE docEntry=" & "'" & Session("DocEntry") & "'"
                            Else
                                ssql = "SELECT  itemCode as cvItemCode, Dscription as cvItemName,Quantity as cfCantidad,Price as cfPrecio,DiscPrcnt as cfDescuento,VatSum ,LineTotal,Currency  FROM " & sTablaDetalle & " WHERE docEntry=" & "'" & Session("DocEntry") & "'"
                            End If

                        End If

                        Dim dtPartidasSAP As New DataTable
                        dtPartidasSAP = objDatos.fnEjecutarConsultaSAP(ssql)
                        objDatos.fnLog("query lineas mail", ssql.Replace("'", ""))
                        ''Ahora las líneas
                        text = text.Replace("{lineas}", fnGeneraHTMLPartidas(dtPartidasSAP))

                        'If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                        '    ''En B2B con PedidoCot, quitamos las líneas
                        '    text = text.Replace("{lineas}", "")
                        'Else
                        '    text = text.Replace("{lineas}", fnGeneraHTMLPartidas(dtPartidasSAP))
                        'End If

                        Dim sMontoIVA As Double = 0
                        Dim sSubTotal As Double = 0
                        For iContLineas As Int16 = 0 To dtPartidasSAP.Rows.Count - 1 Step 1
                            sMontoIVA = sMontoIVA + dtPartidasSAP.Rows(iContLineas)("VatSum")
                            sSubTotal = sSubTotal + dtPartidasSAP.Rows(iContLineas)("LineTotal")
                        Next

                        Try
                            text = text.Replace("{totImpuesto}", sMontoIVA.ToString("###,###,###.#0"))
                            text = text.Replace("{Total}", (sSubTotal + sMontoIVA).ToString("###,###,###.#0"))
                            text = text.Replace("{subtotal}", sSubTotal.ToString("###,###,###.#0"))
                            text = text.Replace("{totalconImpuesto}", (sSubTotal + sMontoIVA).ToString("###,###,###.#0"))
                        Catch ex As Exception

                        End Try


                        objDatos.fnLog("Confirmacion", "Antes de enviar correo")

                        ''Determinamos si hay un PDF a generar, para anexarlo a un posible correo
                        If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And Session("RazonSocial") <> "" Then
                            ''Hay crystal report, generamos el PDF para guardar
                            Try
                                fnGeneraPDF_Correo(Session("DocEntry"))
                            Catch ex As Exception
                                objDatos.fnLog("Genera PDF Correo EX", ex.Message.Replace("'", ""))
                            End Try



                        End If
                        ''Revisamos si notificamos a alguien de la empresa
                        ssql = "select ISNULL(cvCorreoNotifica,'') FROM config.parametrizaciones_b2c "
                        Dim dtCorreoInterno As New DataTable
                        dtCorreoInterno = objDatos.fnEjecutarConsulta(ssql)
                        If dtCorreoInterno.Rows.Count > 0 Then
                            If dtCorreoInterno.Rows(0)(0) <> "" Then
                                objDatos.fnLog("Correo-Interno:", dtCorreoInterno.Rows(0)(0))
                                objDatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, Session("PDF_Correo"), sDocnum & "- Pedido desde B2B")

                                'If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                                '    objDatos.fnLog("Correo-SiExiste", Server.MapPath("~") & "PedidoCot.rpt")
                                '    fnCreaPDF(Session("DocEntry"), dtCorreoInterno.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")
                                'Else
                                '    objDatos.fnLog("Correo-NO Existe", Server.MapPath("~") & "\PedidoCot.rpt")

                                'End If


                            End If
                        End If




                        ''Revisamos si hay que mandar el correo al cliente
                        If Session("slpCode") <> "0" Or Session("Cliente") <> "" Then
                            ssql = "select ISNULL(cvEnviaCorreoCliente,'NO') FROM config.parametrizaciones"
                            Dim dtEnviaCorreoCliente As New DataTable
                            dtEnviaCorreoCliente = objDatos.fnEjecutarConsulta(ssql)
                            If dtEnviaCorreoCliente.Rows.Count > 0 Then
                                If dtEnviaCorreoCliente.Rows(0)(0) = "SI" Then
                                    ssql = ""
                                    ssql = objDatos.fnObtenerQuery("Correocliente")
                                    If ssql <> "" Then
                                        ssql = ssql.Replace("[%0]", sCardCode)
                                        Dim dtCorreo As New DataTable
                                        dtCorreo = objDatos.fnEjecutarConsultaSAP(ssql)
                                        If dtCorreo.Rows.Count > 0 Then
                                            If dtCorreo.Rows(0)(0) <> "" Then
                                                ''Enviar el PDF solo si es cliente, aplica solo para cotización
                                                objDatos.fnLog("Correo-Cliente:", dtCorreo.Rows(0)(0))
                                                objDatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, Session("PDF_Correo"), sDocnum & "- Pedido desde B2B")
                                                'If System.IO.File.Exists(Server.MapPath("~") & "PedidoCot.rpt") And (Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0) Then
                                                '    fnCreaPDF(Session("DocEntry"), dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")
                                                'Else
                                                '    objDatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")


                                                'End If
                                                ''Mandamos notificar al vendedor
                                                If sDBMS = "HANA" Then
                                                    ssql = " SELECT ""SlpCode""  FROM ""#BDSAP#"".""OCRD"" WHERE ""CardCode=""  " & "'" & sCardCode & "'"
                                                Else
                                                    ssql = "SELECT isnull(slpCode, 0) FROM OCRD WHERE cardCode= " & " '" & sCardCode & "'"
                                                End If

                                                Dim dtVendedor As New DataTable
                                                dtVendedor = objDatos.fnEjecutarConsultaSAP(ssql)
                                                If dtVendedor.Rows.Count > 0 Then
                                                    If CInt(dtVendedor.Rows(0)(0)) <> 0 Then
                                                        If sDBMS = "HANA" Then
                                                            ssql = objDatos.fnObtenerQuery("ObtenerCorreoVendedor")
                                                            ssql = ssql.Replace("[%0]", CInt(dtVendedor.Rows(0)(0)))
                                                        Else
                                                            ssql = "select ISNULL(email,'') from OHEM where salesPrson =" & "'" & CInt(dtVendedor.Rows(0)(0)) & "'"
                                                            ssql = "select ISNULL(email,'') from OSLP where slpcode =" & "'" & CInt(dtVendedor.Rows(0)(0)) & "'"

                                                            ssql = objDatos.fnObtenerQuery("ObtenerCorreoVendedor")
                                                            ssql = ssql.Replace("[%0]", CInt(dtVendedor.Rows(0)(0)))

                                                        End If

                                                        dtCorreo = New DataTable
                                                        dtCorreo = objDatos.fnEjecutarConsultaSAP(ssql)
                                                        If dtCorreo.Rows.Count > 0 Then
                                                            If dtCorreo.Rows(0)(0) <> "" Then

                                                                objDatos.fnLog("Correo-Vendedor:", dtCorreo.Rows(0)(0))
                                                                objDatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, Session("PDF_Correo"), sDocnum & "- Pedido desde B2B")

                                                                'If System.IO.File.Exists(Server.MapPath("~") & "PedidoCot.rpt") Then
                                                                '    fnCreaPDF(Session("DocEntry"), dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Compra Registrada")
                                                                'Else
                                                                '    objDatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, sDocnum & "- Nueva Cotización Registrada Cliente:" & sCardCode)
                                                                'End If


                                                            End If
                                                        End If
                                                    End If
                                                End If



                                            End If
                                        End If
                                    End If

                                End If
                            End If
                        End If


                    Catch ex As Exception

                    End Try




                    Dim sComando As String
                    sComando = "<script type='text/javascript'> var opciones='left=100,top=100,width=650,height=450';window.open('vistaPrevia.aspx','Ventana',opciones);</script> "

                    'sComando = sComando & "<link rel = 'stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css' integrity='sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u' crossorigin='anonymous'>"
                    'sComando = sComando & "<link href = 'https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css' rel='stylesheet' integrity='sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN' crossorigin='anonymous'>"
                    'sComando = sComando & "<link href = 'https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700' rel='stylesheet'>"
                    'sComando = sComando & "<link href = 'https://fonts.googleapis.com/css?family=Roboto:400,500,700' rel='stylesheet'>"
                    'sComando = sComando & "<link rel = 'stylesheet' href='css/jquery.bootstrap-touchspin.min.css'>"
                    'sComando = sComando & "<link rel = 'stylesheet' href='css/style.css'>"

                    ' fndescargaPDF(Session("DocEntry"))

                    'Response.Flush()
                    'Response.End()
                    'HttpContext.Current.ApplicationInstance.CompleteRequest()
                    ''   ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnPedido)
                    '' Response.Write(sComando)





                End If



            Else
                objDatos.Mensaje("No se ha podido establecer conexión - reintente por favor", Me.Page)
            End If

        Catch ex As Exception
            objDatos.Mensaje("No se ha podido establecer conexión - reintente por favor EX:" & ex.Message, Me.Page)
        End Try

    End Function

    Public Sub fnGeneraPagoSAP(Cliente As String, docEntry As Int64, Importe As Double, TipoPago As String, idCarrito As String, DocNum As String)


        ''Registramos el pago en una tabla Temporal. Así, por cualquier error, podemos recuperar el detalle y registrarlo en SAP
        Dim dtIdPago As New DataTable
        ssql = "SELECT ISNULL(MAX(ciIdRel),0) + 1 FROM Tienda.Pagos "
        dtIdPago = objDatos.fnEjecutarConsulta(ssql)

        If dtIdPago.Rows.Count > 0 Then
            ssql = "INSERT INTO Tienda.Pagos(ciIdRel,cvTipoPago,cvDocEntry,cvDocnum,cvCliente,cfImporte,cdFechaSAP) VALUES(" _
                & "'" & dtIdPago.Rows(0)(0) & "'," _
                & "'" & TipoPago & "'," _
                & "'" & docEntry & "'," _
                & "'" & DocNum & "'," _
                & "'" & Cliente & "'," _
                & "'" & CStr(Importe).Replace(",", ".") & "', GETDATE())"
            objDatos.fnLog("Tienda Pagos", ssql.Replace("'", ""))
            objDatos.fnEjecutarInsert(ssql)
        End If


        ''Registramos el pago en SAP

        ssql = "SELECT ISNULL(cvPagoBorrador,'NO') FROM Config.Parametrizaciones_B2C "
        Dim dtPagoBorrador As New DataTable
        dtPagoBorrador = objDatos.fnEjecutarConsulta(ssql)



        Dim oPymt As SAPbobsCOM.Payments
        Dim oCompany As New SAPbobsCOM.Company
        oCompany = objDatos.fnConexion_SAP

        If dtPagoBorrador.Rows.Count > 0 Then
            If dtPagoBorrador.Rows(0)(0) = "SI" Then
                oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPaymentsDrafts)
            Else
                oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
            End If

        Else
            oPymt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments)
        End If


        oPymt.DocDate = Now.Date



        oPymt.CardCode = Cliente
        oPymt.JournalRemarks = ""
        oPymt.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments
        oPymt.Invoices.Add()
        oPymt.Invoices.SetCurrentLine(0)


        oPymt.Invoices.DocEntry = docEntry
        oPymt.Invoices.SumApplied = Importe 'SUM OF INVOICE
        oPymt.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice

        objDatos.fnLog("Aplicar PAGO", "antes de importes")
        oPymt.TransferSum = Importe
        oPymt.TransferDate = Now.Date
        oPymt.TransferReference = "" & CInt(Session("NoPedido"))
        oPymt.TransferAccount = fnObtenerCuenta()

        objDatos.fnLog("Aplicar PAGO", "Antes del add")

        If oPymt.Add() <> 0 Then
            ''Error al log
            objDatos.fnLog("Aplicar PAGO", "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
        Else
            objDatos.fnLog("Aplicar PAGO", "Creó Pago en SAP")
            ''Actualizamos
            Dim dtDocnum As New DataTable
            ssql = "SELECT Docnum FROM ORCT WHERE docEntry in (SELECT MAX(DocEntry) FROM ORCT )"
            dtDocnum = New DataTable
            dtDocnum = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtDocnum.Rows.Count > 0 Then

                ssql = "UPDATE Tienda.Pagos SET cvProcesadoSAP='SI', cvDocNumSAP=" & "'" & dtDocnum.Rows(0)(0) & "',cdFechaSAP=GETDATE() WHERE ciIdRel=" & "'" & dtIdPago.Rows(0)(0) & "'"
                ' objDatos.fnLog("Aplicar PAGO", ssql.Replace("'", ""))
                objDatos.fnEjecutarInsert(ssql)
            Else
                objDatos.fnLog("Aplicar PAGO", "No obtiene DocNum PAGO")
            End If


        End If
    End Sub
    Public Function fnObtenerCuenta() As String
        Dim cuenta As String = ""
        ssql = "SELECT cvCuenta FROM config.Parametrizaciones_B2C "
        Dim dtCuenta As New DataTable
        dtCuenta = objDatos.fnEjecutarConsulta(ssql)
        If dtCuenta.Rows.Count > 0 Then
            cuenta = dtCuenta.Rows(0)(0)
        End If

        Return cuenta
    End Function

    Public Function padLeft(Valor As String, NumCeros As Int16) As String
        Dim sCadena As String = ""

        Dim iNumCeros As Int16
        iNumCeros = NumCeros - Valor.Length

        For i = 1 To iNumCeros Step 1
            sCadena = sCadena & "0"
        Next

        Return sCadena + Valor

    End Function

    Public Sub fnCreaPDF(DocEntry As Int32, correo As String, cuerpo As String, asunto As String)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objDatos.fnLog("Al imprimir", "Antes RPT")
        reporte.Load(Server.MapPath("~") & "PedidoCot.rpt")

        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            reporte.SetParameterValue("DocKey@", Session("DocEntry"))

            If objDatos.fnObtenerCliente.ToUpper.Contains("HAWK") Then
                reporte.SetParameterValue("ObjectId@", 23)
            End If
            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If

        Dim sDocnum As String = ""

        Try
            Dim sDoc As String()
            sDoc = asunto.Split("-")
            sDocnum = sDoc(0).Trim

        Catch ex As Exception

        End Try

        objDatos.fnLog("Al imprimir", "Sale de asignarle la BD")
        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        objDatos.fnLog("Al imprimir", "LogInfo")
        ' reporte.Refresh()
        Try
            objDatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
            reporte.SetParameterValue("DocKey@", DocEntry)
            If objDatos.fnObtenerCliente.ToUpper.Contains("HAWK") Then
                reporte.SetParameterValue("ObjectId@", 23)
            End If
            Dim sArchivo As String = ""
            If sDocnum <> "" Then
                sArchivo = Server.MapPath("~") & "PED-" & sDocnum & ".pdf"
            Else
                sArchivo = Server.MapPath("~") & "PED-" & DocEntry & ".pdf"
            End If

            objDatos.fnLog("Exportar en:", sArchivo)
            Try
                reporte.ExportToDisk(ExportFormatType.PortableDocFormat, sArchivo)
                reporte.Dispose()
            Catch ex As Exception

            End Try


            objDatos.fnEnviarCorreo(correo, cuerpo, sArchivo, asunto)
        Catch ex As Exception
            objDatos.fnLog("Exportar ERROR:", ex.Message.Replace("'", ""))
        End Try


        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub

    Public Function fnGeneraHTMLPartidas(dtPArtidas As DataTable) As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objDatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If
        'sLigaSitio 
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objDatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0

        Dim sIVA As Double = 0

        Try
            For x = 0 To dtPArtidas.Rows.Count - 1 Step 1
                objDatos.fnLog("Correo B2B", dtPArtidas.Rows.Count)
                sHtmlBanner = sHtmlBanner & " <tr>"
                '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                If dtCamposPlantilla.Rows.Count > 0 Then
                    Dim sCampos As String = ""
                    For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                        ssql = objDatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtPArtidas.Rows(x)("cvItemCode") & "'")
                        Dim dtGeneral As New DataTable
                        dtGeneral = objDatos.fnEjecutarConsultaSAP(ssql)

                        If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                            sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;vertical-align: middle;text-align:center;'> "

                            Dim iband As Int16 = 0
                            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtPArtidas.Rows(x)("cvItemCode")).Replace("-", "") & ".jpg") Then
                                sLigaSitio = sLigaSitio & "images/products/"
                                sHtmlBanner = sHtmlBanner & "   <img  src='" & sLigaSitio & "" & CStr(dtPArtidas.Rows(x)("cvItemCode")).Replace("-", "") & ".jpg" & "'  alt='logo' style='max-width:50px;max-height:100px'>"
                                iband = 1
                            End If
                            If File.Exists(Server.MapPath("~") & "\images\products\" & CStr(dtPArtidas.Rows(x)("cvItemCode")) & "-1.jpg") And iband = 0 Then
                                sLigaSitio = sLigaSitio & "images/products/"
                                sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & "" & CStr(dtPArtidas.Rows(x)("cvItemCode")).Replace("-", "") & "-1.jpg" & "' alt='logo' style='max-width:50px;max-height:100px'>"
                                iband = 1
                            End If

                            If iband = 0 Then
                                sLigaSitio = sLigaSitio & "images/products/"
                                sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-width:50px;max-height:100px'>"
                            End If


                            sHtmlBanner = sHtmlBanner & "</td>"
                        Else
                            If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                If sCampos = "" Then
                                    ''Si es el primer valor que va a enlazar, lo ponemos en strong
                                    Dim sCaracterEsp As String = ""
                                    If fnRevisaExistencias(dtPArtidas.Rows(x)("cvItemCode")) <= 0 Then
                                        sCaracterEsp = " *"
                                    End If
                                    sCampos = sCampos & "<strong style='font-size: 13px;color:#000000;'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & sCaracterEsp & "</strong> <br>"
                                Else
                                    sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                End If

                            Else
                                '  sCampos = sCampos & "$ " & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & ""
                            End If


                        End If
                    Next
                    sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:left;font-size: 10px;'>" & sCampos & "</td>"

                End If
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & dtPArtidas.Rows(x)("Currency") & " " & CDbl(dtPArtidas.Rows(x)("cfPrecio")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & CDbl(dtPArtidas.Rows(x)("cfCantidad")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & dtPArtidas.Rows(x)("Currency") & " " & (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio"))).ToString(" ###,###,###.#0") & "</td>"

                sHtmlBanner = sHtmlBanner & "</tr>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                sSubTotal = sSubTotal + (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio")))  'dtPartidas.Rows(i)("cfPrecio")

                sIVA = sIVA + CDbl(dtPArtidas.Rows(x)("cfPrecio"))
                ''Aqui van los botones de Action Cart
                'sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                ''sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Editar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Quitar</a></div>"
                ''  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Mover a favoritos</a></div>"
                '' sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'>Guardar</a></div>"

                ''sHtmlBanner = sHtmlBanner & "</div>"

                'sHtmlBanner = sHtmlBanner & " </div> "


            Next



            sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
        End Try
        Return sHtmlEncabezado
    End Function
    Protected Function MensajeHTML(ArchivoHTML As [String]) As String
        Dim Cuerpo As [String] = ""
        Try
            Dim File As New System.IO.StreamReader(ArchivoHTML)

            Dim Line As [String]
            Dim text As String = System.IO.File.ReadAllText(ArchivoHTML)

            Cuerpo = text

            File.Close()
        Catch ex As Exception
            objDatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function


    Public Sub fnGeneraPDF_Correo(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        Dim iTipoDocumento As String = ""
        If Session("TIPODOC") = "PEDIDO" Then
            reporte.Load(Server.MapPath("~") & "\Pedido.rpt")
            iTipoDocumento = "17"
        Else
            reporte.Load(Server.MapPath("~") & "\PedidoCot.rpt")
            iTipoDocumento = "23"
        End If


        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then

            If objDatos.fnObtenerCliente.ToUpper.Contains("HAWK") Then
                reporte.SetParameterValue("ObjectId@", iTipoDocumento)
                reporte.SetParameterValue("DocKey@", DocEntry)
            Else
                reporte.SetParameterValue("DocKey@", DocEntry)
            End If
            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If



        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next
        reporte.Refresh()

        If objDatos.fnObtenerCliente.ToUpper.Contains("HAWK") Then
            reporte.SetParameterValue("ObjectId@", iTipoDocumento)
            reporte.SetParameterValue("DocKey@", DocEntry)
        Else
            reporte.SetParameterValue("DocKey@", DocEntry)
        End If

        Dim sArchivo As String = ""
        sArchivo = Server.MapPath("~") & "\PDF\PED-" & DocEntry & ".pdf"

        Session("PDF_Correo") = sArchivo

        objDatos.fnLog("GeneraPDF_Correo", sArchivo)
        Try
            reporte.ExportToDisk(ExportFormatType.PortableDocFormat, sArchivo)
            reporte.Dispose()
        Catch ex As Exception
            objDatos.fnLog("GeneraPDF_Correo_EX", ex.Message.Replace("'", ""))
            Session("PDF_Correo") = ""
        End Try

    End Sub

    Public Sub fndescargaPDF(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        Dim iTipoDocumento As String = ""

        Try
            If Session("TIPODOC") = "PEDIDO" Then
                reporte.Load(Server.MapPath("~") & "\Pedido.rpt")
                iTipoDocumento = "17"
            Else
                reporte.Load(Server.MapPath("~") & "\PedidoCot.rpt")
                iTipoDocumento = "23"
            End If


            Dim ssql As String
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objDatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then

                If objDatos.fnObtenerCliente.ToUpper.Contains("HAWK") Or objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("ALTURA") Or objDatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Then
                    reporte.SetParameterValue("ObjectId@", iTipoDocumento)
                    reporte.SetParameterValue("DocKey@", DocEntry)
                Else
                    reporte.SetParameterValue("@DocKey", DocEntry)
                End If
                reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

                crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
                crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
                crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
                crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


            End If



            CrTables = reporte.Database.Tables
            For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
                crtableLogoninfo = CrTable.LogOnInfo
                crtableLogoninfo.ConnectionInfo = crConnectionInfo
                CrTable.ApplyLogOnInfo(crtableLogoninfo)
            Next
            reporte.Refresh()


            If objDatos.fnObtenerCliente.ToUpper.Contains("HAWK") Or objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("ALTURA") Or objDatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Then
                reporte.SetParameterValue("ObjectId@", iTipoDocumento)
                reporte.SetParameterValue("DocKey@", DocEntry)
            Else

                reporte.SetParameterValue("@DocKey", DocEntry)
            End If
            reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "PED-" & DocEntry)
            reporte.Dispose()
            'Response.Flush()
            'Response.End()
            'Response.Clear()
        Catch ex As Exception

        End Try


    End Sub

    Public Function fnRevisaExistencias(itemCode As String) As Double
        Dim existencia As Double = 0
        ''Existencia 
        ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
        Dim dtExistencia As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtExistencia.Rows.Count > 0 Then
            existencia = CDbl(dtExistencia.Rows(0)(0))
        End If
        Return existencia
    End Function
    Public Sub fnActivaTimer()
        '     Timer1.Enabled = True
    End Sub
    Protected Sub btnCotizar_Click(sender As Object, e As EventArgs) Handles btnCotizar.Click
        fnGuardaCarrito("COTIZACION")
    End Sub
    Protected Sub btnPedido_Click(sender As Object, e As EventArgs) Handles btnPedido.Click
        fnGuardaCarrito("PEDIDO")
    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        fnGuardaCarrito("CARRITO")
    End Sub
    Protected Sub btnGuardarPlantilla_Click(sender As Object, e As EventArgs) Handles btnGuardarPlantilla.Click
        pnlPlantilla.Visible = True
        pnlDireccionEntrega.Visible = False
    End Sub
    Protected Sub btnGuardarPlantillaDet_Click(sender As Object, e As EventArgs) Handles btnGuardarPlantillaDet.Click
        If txtNombrePlantilla.Text = "" Then
            objDatos.Mensaje("Debe indicar un nombre para la plantilla", Me.Page)
            Exit Sub
        Else
            ssql = "select ISNULL(MAX(ciIdPlantilla),0) + 1 FROM  Tienda.Plantilla_HDR"
            Dim dtId As New DataTable
            dtId = objDatos.fnEjecutarConsulta(ssql)

            Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))

            Dim sTallaColor As String = ""
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                End If
            End If


            ssql = "INSERT INTO Tienda.Plantilla_HDR ( ciIdPlantilla,cvUsuario,cvAgente,ciIdAgenteSAP,cvNombrePlantilla,cdFecha,cvComentarios) VALUES(" _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & Session("UserTienda") & "'," _
                & "'" & Session("NombreuserTienda") & "'," _
                & "'" & Session("SlpCode") & "'," _
                & "'" & txtNombrePlantilla.Text & "',GETDATE(),'')"
            objDatos.fnEjecutarInsert(ssql)

            Dim itemCodePlantilla As String = ""

            ''Ahora las lineas
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Plantilla_det"
                    Dim dtIdLineas As New DataTable
                    dtIdLineas = objDatos.fnEjecutarConsulta(ssql)

                    If sTallaColor = "SI" Then
                        itemCodePlantilla = Partida.Generico
                    Else
                        itemCodePlantilla = Partida.ItemName
                    End If


                    ssql = "INSERT INTO Tienda.Plantilla_det (ciIdRelacion,ciIdPlantilla,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                      & "'" & dtIdLineas.Rows(0)(0) & "'," _
                      & "'" & dtId.Rows(0)(0) & "'," _
                      & "'" & Session("UserTienda") & "'," _
                      & "'" & Partida.ItemCode & "'," _
                      & "'" & itemCodePlantilla & "'," _
                      & "'" & Partida.Cantidad.ToString.Replace(",", ".") & "'," _
                      & "'" & Partida.Precio.ToString.Replace(",", ".") & "'," _
                      & "'" & Partida.Descuento.ToString.Replace(",", ".") & "')"
                    objDatos.fnEjecutarInsert(ssql)
                End If
            Next
            lblMensaje.Text = "Plantilla Guardada"
            objDatos.Mensaje("Plantilla Guardada", Me.Page)
            txtNombrePlantilla.Text = ""
            pnlPlantilla.Visible = False

        End If
    End Sub

    Protected Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        'Dim sComando As String
        'sComando = "<script type='text/javascript'> var opciones='left=100,top=100,width=650,height=450';window.open('vistaPrevia.aspx','Ventana',opciones);</script> "
        'Response.Write(sComando)

        fndescargaPDF(Session("DocEntry"))
        ' fnCreaPDF(Session("DocEntry"))
    End Sub

    Private Sub btnPagar_Click(sender As Object, e As EventArgs) Handles btnPagar.Click
        Session("VieneB2B") = "SI"

        ssql = objDatos.fnObtenerQuery("Correocliente")
        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", Session("Cliente"))

            objDatos.fnLog("btnPagar ", "  correo: " & ssql.Replace("'", ""))

            Dim dtCorreo As New DataTable
            dtCorreo = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtCorreo.Rows.Count > 0 Then
                Session("CorreoClienteB2B") = dtCorreo.Rows(0)(0)
                Session("CorreoInvitado") = dtCorreo.Rows(0)(0)
            End If
        End If
        objDatos.fnLog("btnPagar ", "  correo: " & Session("CorreoInvitado"))


        ''Validamos la dirección de envio que ha seleccionado o capturado
        If txtDireccion.Text <> "" Then
            If txtCP.Text = "" Then
                objDatos.Mensaje("De indicar  el código postal", Me.Page)
                Exit Sub
            End If
            ''Nueva direccion
            Session("CalleEnvio") = txtDireccion.Text
            Session("CiudadEnvio") = txtMunicipio.Text
            Session("ColoniaEnvio") = ""
            Session("MunicipioEnvio") = txtMunicipio.Text
            Session("EstadoEnvio") = ddlEstados.SelectedValue
            Session("CPEnvio") = txtCP.Text
        End If

        Response.Redirect("resumen.aspx")
    End Sub

    Private Sub ddlDirecciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDirecciones.SelectedIndexChanged
        Try
            ''Obtenemos el detalle de la dirección de envio
            ssql = objDatos.fnObtenerQuery("DetalleDireccion")
            ssql = ssql.Replace("[%0]", Session("Cliente")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)
            Dim dtDetalleDireccion As New DataTable
            dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtDetalleDireccion.Rows.Count > 0 Then
                Session("NombreDireccionEnvio") = ""
                Session("CalleEnvio") = dtDetalleDireccion.Rows(0)("Calle")
                Session("ColoniaEnvio") = dtDetalleDireccion.Rows(0)("Colonia")
                Session("Ciudadenvio") = dtDetalleDireccion.Rows(0)("Ciudad")

                Session("NumExtEnvio") = dtDetalleDireccion.Rows(0)("Numero")
                Session("CPEnvio") = dtDetalleDireccion.Rows(0)("CP")
                Session("EstadoEnvio") = dtDetalleDireccion.Rows(0)("Estado")
                'ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
                Dim dtLogin As New DataTable
                dtLogin = objDatos.fnEjecutarConsulta(ssql)
                If dtLogin.Rows.Count > 0 Then
                    'txtTelefono.Text = dtLogin.Rows(0)("Tel1")
                    'txtRFC.Text = dtLogin.Rows(0)("RFC")
                    'txtNombre.Text = dtLogin.Rows(0)("cvNombreCompleto")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Function CargarCarritoTemplate(Cantidad As String, Articulo As String) As String

        HttpContext.Current.Session("AgregaCarrito") = "SI"
        Dim partida As New Cls_Pedido.Partidas
        Dim ssql As String
        Dim objDatos As New Cls_Funciones
        ' Session("Partidas") = New List(Of Cls_Pedido.Partidas)
        objDatos.fnLog("CargarCarrito", "entra:" & Articulo)
        Dim dPrecioActual As Double = 0
        HttpContext.Current.Session("ErrorExistencia") = ""
        HttpContext.Current.Session("ProductoVista") = Articulo


        'If Request.Cookies("Cliente").Value IsNot Nothing Then
        '    HttpContext.Current.Session("Cliente") = Request.Cookies("Cliente").Value
        'End If
        'If Request.Cookies("ListaPrecios").Value IsNot Nothing Then
        '    HttpContext.Current.Session("ListaPrecios") = Request.Cookies("ListaPrecios").Value
        'End If
        Try

            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(HttpContext.Current.Session("ProductoVista"))
            If fDescuento = 0 Then
                fDescuento = objDatos.fnObtenerDescuentoEspecialDelta(HttpContext.Current.Session("ProductoVista"))
            End If
            partida.Cantidad = Cantidad

            ' objDatos.fnLog("CargarCarrito cant:", Cantidad)
            ''Revisamos si hay que mostrar tallas y colores
            Dim sTallaColor As String = ""
            ssql = "SELECT ISNULL(cvManejoTallas,'NO') from config.parametrizaciones "
            Dim dtTallasColores As New DataTable
            dtTallasColores = objDatos.fnEjecutarConsulta(ssql)
            If dtTallasColores.Rows.Count > 0 Then
                If dtTallasColores.Rows(0)(0) = "SI" Then
                    sTallaColor = "SI"
                    '    objDatos.fnLog("CargarCarrito talla color:", sTallaColor)
                    Articulo = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.Generico = HttpContext.Current.Session("ProductoVista")
                    ''Cambiamos
                    partida.Precio = HttpContext.Current.Session("PrecioCodeTallaColor")
                    partida.ItemCode = HttpContext.Current.Session("ItemCodeTallaColor")
                    partida.TotalLinea = partida.Cantidad * CDbl(HttpContext.Current.Session("PrecioCodeTallaColor"))

                    partida.Descuento = fDescuento
                Else

                    partida.ItemCode = Articulo


                    If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then
                        If HttpContext.Current.Session("ListaPrecios") Is Nothing Then
                            dPrecioActual = objDatos.fnPrecioActual(Articulo)
                        Else
                            dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                        End If

                    Else
                        dPrecioActual = objDatos.fnPrecioActual(Articulo)

                    End If
                    objDatos.fnLog("CargarCarrito", "Antes de evaluar")
                    If HttpContext.Current.Session("Cliente") <> "" Then
                        ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                        ssql = ssql.Replace("[%0]", HttpContext.Current.Session("Cliente"))
                        objDatos.fnLog("ListaPrecios", ssql.Replace("'", ""))
                        Dim dtLista As New DataTable
                        dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtLista.Rows.Count > 0 Then
                            HttpContext.Current.Session("ListaPrecios") = dtLista.Rows(0)(0)
                        Else
                            HttpContext.Current.Session("ListaPrecios") = "1"
                        End If
                        dPrecioActual = objDatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))

                        If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                            'B2B sin IVA
                            dPrecioActual = dPrecioActual / 1.16
                        End If

                        partida.Descuento = objDatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                    End If


                    partida.Precio = dPrecioActual
                    partida.TotalLinea = partida.Cantidad * partida.Precio




                End If
            End If

            ''Evaluamos el stock
            Dim existencia As Double = 0
            ''Existencia 
            ssql = objDatos.fnObtenerQuery("ExistenciaSAP")
            Dim dtExistencia As New DataTable
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            objDatos.fnLog("existencia", ssql.Replace("'", ""))
            dtExistencia = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtExistencia.Rows.Count > 0 Then
                existencia = CDbl(dtExistencia.Rows(0)(0))
            End If
            Try
                For Each Partida2 As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    If Partida2.ItemCode <> "BORRAR" Then
                        If Partida2.ItemCode = Articulo Then
                            existencia = existencia - Partida2.Cantidad
                        End If
                    End If
                Next
            Catch ex As Exception

            End Try


            ssql = "SELECT ISNULL(cvVendeSinStock,'SI') from Config.Parametrizaciones"
            Dim dtVendesinStock As New DataTable
            dtVendesinStock = objDatos.fnEjecutarConsulta(ssql)
            If dtVendesinStock.Rows.Count > 0 Then
                If dtVendesinStock.Rows(0)(0) = "NO" Then


                    If existencia - Cantidad < 0 Then
                        HttpContext.Current.Response.Write("<script language=javascript>alert('No se puede vender este artículo, dado que No cuenta con suficiente stock');</script>")
                        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("SALAMA") Then

                            HttpContext.Current.Session("ErrorExistencia") = "No hay existencia del artículo seleccionado." _
                                & " Si no encontraste disponibilidad, contacta a un representante en el teléfono: 449 205 0883 en nuestros horarios de servicio."
                            Exit Function
                        Else
                            If CInt(HttpContext.Current.Session("slpCode")) > 0 And CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Then
                                ''en vendedores dejamos pasar la existencia
                            Else
                                HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                                Exit Function
                            End If

                        End If


                    End If
                Else
                    If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("BOSS") Then
                        If existencia - Cantidad < 0 Then
                            HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo no se cuenta con toda la existencia para surtir la cantidad seleccionada, el tiempo de resurtido es de 7 días hábiles."
                        End If
                    End If


                End If
            End If

            objDatos.fnLog("Carga Carrito Cliente:", HttpContext.Current.Session("Cliente"))
            objDatos.fnLog("Carga Carrito ListaPrecios:", HttpContext.Current.Session("ListaPrecios"))
            If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") And HttpContext.Current.Session("Cliente") <> "" Then


                ''Posibles monedas en la lista de precios
                ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
                ssql = objDatos.fnObtenerQuery("MonedasListaPrecios")
                Dim dtMonedas As New DataTable
                ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                ssql = ssql.Replace("[%1]", "'" & HttpContext.Current.Session("ListaPrecios") & "'")
                dtMonedas = objDatos.fnEjecutarConsultaSAP(ssql)
                Dim sMoneda As String = ""

                If dtMonedas.Rows.Count > 0 Then
                    sMoneda = dtMonedas.Rows(0)(0)
                    objDatos.fnLog("Carga carrito moneda:", sMoneda)
                    partida.Moneda = sMoneda
                    For Each PartidaMoneda As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                        If PartidaMoneda.ItemCode <> "BORRAR" Then
                            objDatos.fnLog("Carga carrito moneda-IF:", PartidaMoneda.Moneda)
                            If PartidaMoneda.Moneda <> "" Then

                                If PartidaMoneda.Moneda <> sMoneda Then
                                    HttpContext.Current.Session("ErrorExistencia") = "El artículo se agregó al carrito, sin embargo se quitará del carrito porque no se pueden combinar artículos en USD y MXP."
                                    Exit Function
                                End If

                            End If
                        End If

                    Next

                End If


            End If



            partida.ItemCode = Articulo
            Dim iLinea As Int16 = 0
            Try
                For Each PartidaCont As Cls_Pedido.Partidas In HttpContext.Current.Session("Partidas")
                    iLinea = iLinea + 1
                Next
            Catch ex As Exception

            End Try

            partida.Linea = iLinea


            '     objDatos.fnLog("CargarCarrito ", "ItemName")

            ''Ahora el itemName

            ssql = objDatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & partida.ItemCode & "'")

            'If sTallaColor = "SI" Then
            '    ssql = ssql.Replace("[%0]", "'" & HttpContext.Current.Session("ItemCodeTallaColor") & "'")
            'Else
            '    ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            'End If



            If CDbl(HttpContext.Current.Session("Mts2")) > 0 Then
                partida.Mts2 = HttpContext.Current.Session("Mts2")
            End If


            Dim dtItemName As New DataTable
            dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count = 0 Then
                partida.ItemName = "ND"
            Else
                partida.ItemName = dtItemName.Rows(0)(0)
            End If
            objDatos.fnLog("CargarCarrito ", "Add Partida: " & partida.ItemName)

            HttpContext.Current.Session("Partidas").add(partida)

            objDatos.fnLog("CargarCarrito despues de add itemcode ", partida.ItemCode)
            '  Response.Redirect("carrito.aspx")
        Catch ex As Exception
            objDatos.fnLog("Error en carga", ex.Message)
        End Try


        ''una vez que cargamos al carrito, validamos si es STOP Catalogo, para ver si por la cantidad de prendas no tenemos que cargar seguro o flete
        Try
            If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("STOP CAT") Then
                'Dim objLocal As New producto_interior
                'objLocal.fnAgregaFletesSeguros_StopCatalogo()
            End If
        Catch ex As Exception

        End Try




        Dim result As String = "Entró:" & Articulo

        Return result
    End Function
    Private Sub btnSubir_Click(sender As Object, e As EventArgs) Handles btnSubir.Click
        If FileUpload1.HasFile Then
            Dim archivo As String
            archivo = Server.MapPath("~") & "\" & FileUpload1.FileName

            FileUpload1.SaveAs(archivo)

            Dim conStr As String = ""
            If Path.GetExtension(FileUpload1.FileName).Trim() = ".xls" Then
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & archivo & ";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
            ElseIf Path.GetExtension(FileUpload1.FileName).Trim() = ".xlsx" Then
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & archivo & ";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=2"""
            End If

            Dim query As String = "SELECT * FROM [Hoja1$]"
            Dim conn As OleDbConnection
            conn = New OleDbConnection(conStr)

            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim cmd As OleDbCommand = New OleDbCommand(query, conn)
            Dim da As OleDbDataAdapter = New OleDbDataAdapter(cmd)
            Dim ds As DataSet = New DataSet()
            da.Fill(ds)
            Dim dtDatos As New DataTable
            dtDatos = ds.Tables(0)

            For i = 0 To dtDatos.Rows.Count - 1 Step 1

                ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & dtDatos.Rows(i)(0) & "'")

                Dim dtItemName As New DataTable
                dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtItemName.Rows.Count = 0 Then
                    objDatos.fnLog("Carrito btnSubir", "no encontrado")
                Else
                    objDatos.fnLog("Carrito btnSubir", "subiendo: " & dtDatos.Rows(i)(0))
                    CargarCarritoTemplate(dtDatos.Rows(i)(1), dtDatos.Rows(i)(0))
                End If

                'Dim partida As New Cls_Pedido.Partidas
                'Session("Partidas").add(partida)
            Next
            objDatos.fnLog("Carrito btnSubir", "Termina")
            Response.Redirect("carrito.aspx")
        End If
    End Sub

    Private Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged

    End Sub
End Class
