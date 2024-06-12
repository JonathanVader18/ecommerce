Imports System.Data
Partial Class pago_index
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub pago_index_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim fila As DataRow
        Session("Page") = "pagoindex.aspx"
        If Not IsPostBack Then


            ssql = objDatos.fnObtenerQuery("UsoCFDI")
            If ssql <> "" Then
                Dim dtUso As New DataTable
                dtUso = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtUso.Rows.Count > 0 Then
                    Dim filac As DataRow
                    filac = dtUso.NewRow
                    filac("Clave") = "-1"
                    filac("Descripcion") = "-Seleccione-"
                    dtUso.Rows.Add(filac)

                    ddlUsoCFDI.DataSource = dtUso
                    ddlUsoCFDI.DataTextField = "Descripcion"
                    ddlUsoCFDI.DataValueField = "Clave"
                    ddlUsoCFDI.DataBind()
                    ddlUsoCFDI.SelectedValue = "-1"



                End If
            End If

            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre,ISNULL(cvcliente,'') as Cliente from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then
                If CStr(dtcliente.Rows(0)(0)).Contains("STOP") And Session("RazonSocial") <> "" Then
                    Response.Redirect("index.aspx")

                End If
                If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("STOP CAT") Then
                    chkInvitado.Enabled = False
                    pnlCheckRegalo.Visible = True

                    ''Llenamos los mensajes
                    ssql = objDatos.fnObtenerQuery("MensajesRegalo")
                    Dim dtMensajes As New DataTable
                    dtMensajes = objDatos.fnEjecutarConsultaSAP(ssql)
                    ddlTipoMensaje.DataSource = dtMensajes
                    ddlTipoMensaje.DataTextField = "Mensaje"
                    ddlTipoMensaje.DataValueField = "Clave"
                    ddlTipoMensaje.DataBind()


                End If
                If CStr(dtcliente.Rows(0)(1)).Contains("Salama") Then
                    pnlOfertas.Visible = False
                    pnlOfertasStop.Visible = True
                Else
                    pnlOfertas.Visible = True
                    pnlOfertasStop.Visible = False
                End If

                ''HAWK - Formas de pago
                If CStr(dtcliente.Rows(0)(1)).ToUpper.Contains("HAWK") Then
                    pnlFormaPagoHawk.Visible = True
                    ssql = objDatos.fnObtenerQuery("FormasPagoCFDI")
                    If ssql <> "" Then
                        Dim dtFormaPago As New DataTable
                        dtFormaPago = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtFormaPago.Rows.Count > 0 Then
                            Dim filac As DataRow
                            filac = dtFormaPago.NewRow
                            filac("Clave") = "-1"
                            filac("Descripcion") = "-Seleccione-"
                            dtFormaPago.Rows.Add(filac)

                            ddlFormasPago.DataSource = dtFormaPago
                            ddlFormasPago.DataTextField = "Descripcion"
                            ddlFormasPago.DataValueField = "Clave"
                            ddlFormasPago.DataBind()
                            ddlFormasPago.SelectedValue = "-1"



                        End If
                    End If
                End If

            End If
            pnlRegistrar.Visible = False
            pnlDireccion.Visible = True
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
                    '    Response.Redirect("pagoindex.aspx")
                End If
            End If



            If Session("UserB2C") = "" Then
                pnlIngresar.Visible = True
            Else
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objDatos.fnObtenerQuery("DireccionesEntrega")

                If Session("CardCodeUserB2C") = "" Then
                    ''Tiene un cardCode Generico
                    Dim ssql2 = "SELECT cvCardCodeSAP FROM Config.Parametrizaciones_B2C"
                    Dim dtCardCode As New DataTable
                    dtCardCode = objDatos.fnEjecutarConsulta(ssql2)
                    If dtCardCode.Rows.Count > 0 Then
                        Session("CardCodeUserB2C") = dtCardCode.Rows(0)(0)
                    End If
                End If
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)

                fila = dtdirecciones.NewRow
                fila("Direccion") = "-Seleccione-"
                dtdirecciones.Rows.Add(fila)

                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
                ddlDirecciones.SelectedValue = "-Seleccione-"
                ''Cargamos el nombre del cliente
                ssql = objDatos.fnObtenerQuery("NombreCliente")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtNombre As New DataTable
                dtNombre = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtNombre.Rows.Count > 0 Then
                    txtNombre.Text = dtNombre.Rows(0)(0)
                End If


            End If
            If Session("UserB2C") = "" Then
                pnlCombodirecciones.Visible = False
            End If
            objDatos.fnLog("Va a cargar", " Paises ")
            ''Cargamos los paises
            ssql = objDatos.fnObtenerQuery("Paises")
            Dim dtPais As New DataTable
            dtPais = objDatos.fnEjecutarConsultaSAP(ssql)
            'fila = dtPais.NewRow
            'fila("Clave") = "0"
            'fila("Descripcion") = "-Seleccione-"
            'dtPais.Rows.Add(fila)
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


                ''Si el cliente es PMK, cargamos las localidades
                If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then
                    ssql = objDatos.fnObtenerQuery("Poblaciones")
                    ssql = ssql.Replace("[%0]", ddlEstados.SelectedItem.Text)

                    Dim dtPoblacion = objDatos.fnEjecutarConsultaSAP(ssql)

                    objDatos.fnLog("poblaciones pmk", ssql.Replace("'", ""))

                    fila = dtPoblacion.NewRow
                    fila("Clave") = "0"
                    fila("Descripcion") = "-Seleccione-"
                    dtPoblacion.Rows.Add(fila)

                    ddlLocalidad.Visible = True
                    txtMunicipio.Visible = False
                    ddlLocalidad.DataSource = dtPoblacion
                    ddlLocalidad.DataTextField = "Descripcion"
                    ddlLocalidad.DataValueField = "Clave"
                    ddlLocalidad.DataBind()
                    ddlLocalidad.SelectedValue = "0"
                End If
            Catch ex As Exception
                objDatos.fnLog("Cargar estados", ex.Message)
            End Try
            If Session("UserB2C") = "" Then
                pnlIngresar.Visible = True
            Else
                pnlIngresar.Visible = False
                ''Cargamos las direcciones del cliente seleccionado
                Dim dtdirecciones As New DataTable


                ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "

                Dim dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
                objDatos.fnLog("Genera SAP", ssql.Replace("'", ""))
                If dtCreaSAP.Rows.Count > 0 Then
                    If dtCreaSAP.Rows(0)(0) = "SI" Then
                        ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                        ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                        dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
                    Else
                        ssql = "SELECT DISTINCT  [ciIdRel], ISNULL([cvCalle],'') + '  ' + ISNULL([cvNumExt],'') + '  ' + ISNULL([cvCP],'') as Direccion, [cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvColonia, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                        dtdirecciones = objDatos.fnEjecutarConsulta(ssql)
                    End If
                End If



                fila = dtdirecciones.NewRow
                fila("Direccion") = "-Seleccione-"
                dtdirecciones.Rows.Add(fila)

                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
                ddlDirecciones.SelectedValue = "-Seleccione-"
                ''Cargamos el nombre del cliente
                ssql = objDatos.fnObtenerQuery("NombreCliente")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtNombre As New DataTable
                dtNombre = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtNombre.Rows.Count > 0 Then
                    txtNombre.Text = dtNombre.Rows(0)(0)
                End If

                If dtdirecciones.Rows.Count = 1 Then  'Solo la de "-Seleccione-
                    ddlDirecciones.Visible = False
                    ' lblTituloDir.Text = "Confirme la dirección de entrega"
                    ''Es nuevo, viene de login
                    '       Session("RFC") = txtRFC.Text
                    txtCalle.Text = Session("CalleEnvio")
                    txtNombre.Text = Session("NombreUserB2C")

                    For i = 0 To dtPais.Rows.Count - 1 Step 1
                        If dtPais.Rows(i)("Descripcion") = Session("PaisLogin") Or dtPais.Rows(i)("Clave") = Session("PaisLogin") Then
                            ddlPais.SelectedValue = dtPais.Rows(i)("Clave")
                        End If
                    Next
                    For i = 0 To dtEstado.Rows.Count - 1 Step 1
                        If dtEstado.Rows(i)("Descripcion") = Session("EstadoLogin") Or dtEstado.Rows(i)("Clave") = Session("EstadoLogin") Then
                            ddlEstados.SelectedValue = dtEstado.Rows(i)("Clave")
                        End If
                    Next



                Else
                    ddlDirecciones.SelectedValue = dtdirecciones.Rows(0)("Direccion")
                    ''Cargamos por default la ultima
                    Dim dtDetalleDireccion As New DataTable
                    ''Obtenemos el detalle de la dirección de envio


                    ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
                    dtCreaSAP = New DataTable
                    dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
                    objDatos.fnLog("Genera SAP", ssql.Replace("'", ""))
                    If dtCreaSAP.Rows.Count > 0 Then
                        If dtCreaSAP.Rows(0)(0) = "SI" Then
                            ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                            ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)

                            dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                        Else
                            ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')Calle, ISNULL([cvColonia],'') Colonia, ISNULL([cvNumExt],'') as Numero, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') Estado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as CP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                            dtDetalleDireccion = objDatos.fnEjecutarConsulta(ssql)
                        End If
                    End If


                    If dtDetalleDireccion.Rows.Count > 0 Then
                        txtNombre.Text = Session("NombreUserB2C")
                        txtCalle.Text = dtDetalleDireccion.Rows(0)("Calle")
                        'txtColonia.Text = dtDetalleDireccion.Rows(0)("Colonia")
                        '  txtCiudad.Text = dtDetalleDireccion.Rows(0)("Ciudad")
                        ' txtNumExt.Text = dtDetalleDireccion.Rows(0)("Numero")
                        txtCP.Text = dtDetalleDireccion.Rows(0)("CP")
                        txtEstado.Text = dtDetalleDireccion.Rows(0)("Estado")
                        ' txtMunicipio.Text = dtDetalleDireccion.Rows(0)("Municipio")
                        '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                        For i = 0 To dtPais.Rows.Count - 1 Step 1
                            If dtPais.Rows(i)("Descripcion") = dtDetalleDireccion.Rows(0)("cvPais") Or dtPais.Rows(i)("Clave") = dtDetalleDireccion.Rows(0)("cvPais") Then
                                ddlPais.SelectedValue = dtPais.Rows(i)("Clave")
                            End If
                        Next
                        For i = 0 To dtEstado.Rows.Count - 1 Step 1
                            If dtEstado.Rows(i)("Descripcion") = dtDetalleDireccion.Rows(0)("Estado") Or dtEstado.Rows(i)("Clave") = dtDetalleDireccion.Rows(0)("Estado") Then
                                ddlEstados.SelectedValue = dtEstado.Rows(i)("Clave")
                            End If
                        Next

                    End If
                End If


            End If
            If Session("UserB2C") <> "" Then
                pnlDireccion.Visible = True
            Else
                pnlDireccion.Visible = True

            End If

            ''El RFC

            ssql = "SELECT ISNULL(cvRFC,''),ISNULL(cvNombreCompleto,'') as Nombre,ISNULL(cvTelefono1,'') Tel1, ISNULL(cvTelefono2,'') Tel2,ISNULL(cvCardCode,'') as CardCodeSAP FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtRFC As New DataTable
            dtRFC = objDatos.fnEjecutarConsulta(ssql)
            If dtRFC.Rows.Count > 0 Then
                txtRFC.Text = dtRFC.Rows(0)(0)
                txtNombre.Text = dtRFC.Rows(0)("Nombre")

                If CStr(dtRFC.Rows(0)(0)) = "" Then
                    'El RFC está vacio, lo buscamos en SAP
                    If CStr(dtRFC.Rows(0)("CardCodeSAP")) <> "" Then
                        ssql = objDatos.fnObtenerQuery("ConsultarRFC")
                        If ssql <> "" Then
                            ssql = ssql.Replace("[%0]", CStr(dtRFC.Rows(0)("CardCodeSAP")))
                            Dim dtRFC_SAP As New DataTable
                            dtRFC_SAP = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtRFC_SAP.Rows.Count > 0 Then
                                txtRFC.Text = dtRFC_SAP.Rows(0)(0)
                            End If

                        End If
                    End If
                End If
            End If

            ''La empresa, si esque aplica
            ssql = "SELECT ISNULL(cvEmpresa,'') FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtEmpresa As New DataTable
            dtEmpresa = objDatos.fnEjecutarConsulta(ssql)
            If dtEmpresa.Rows.Count > 0 Then
                'txtNombreEmpresa.Text = dtEmpresa.Rows(0)(0)

            End If
            If chkInvitado.Checked = True Then
                pnlInvitado.Visible = True
                If Session("UserB2C") = "" Then
                    pnlDireccion.Visible = False
                Else
                    pnlDireccion.Visible = True
                End If
                pnlRegistrar.Visible = False
                pnlDireccion.Visible = True
            Else
                pnlDireccion.Visible = False
                pnlRegistrar.Visible = True
                pnlInvitado.Visible = False
            End If

        End If

        If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then
            objDatos.fnLog("Ocultar municipio PMK", "Si entra")
            txtMunicipio.Visible = False
            ddlLocalidad.Visible = True
        End If
        fnTotales()
    End Sub

    Public Sub fnAgregaFletesSeguros_StopCatalogo()
        ''calculamos el envio

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
        '  lblCarrito.Text = "Carrito de compras (" & Session("LeyendaDescuento") & ")"
        lblEnvio.Text = CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        objDatos.fnLog("Flete Delta", lblEnvio.Text)
        chkInvitado.Enabled = False
    End Sub

    Public Sub fnTotales()


        Dim sSubTotal As Double = 0
        Dim TotDescuento As Double = 0
        Dim TotalImpuestos As Double = 0
        Dim fTasaImpuesto As Double = 0
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
                        ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                        Dim dtTasa As New DataTable
                        dtTasa = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtTasa.Rows.Count > 0 Then
                            fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                        End If

                    End If

                End If


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

                Dim sMonedasistema As String = "MXP"
                ssql = "select MainCurncy  from OADM "
                Dim dtMoneda As New DataTable
                dtMoneda = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtMoneda.Rows.Count > 0 Then
                    sMonedasistema = dtMoneda.Rows(0)(0)
                End If

                If Partida.Descuento > 0 Then

                    If Partida.Moneda <> sMonedasistema Then
                        Session("Moneda") = sMonedasistema

                        TotDescuento = TotDescuento + (Partida.Precio * CDbl(Session("TC")) - (precioConDescuento * CDbl(Session("TC"))) * Partida.Cantidad)
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto) * CDbl(Session("TC"))
                    Else
                        TotDescuento = TotDescuento + ((Partida.Precio - precioConDescuento) * Partida.Cantidad)
                        TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)
                    End If
                Else
                    TotalImpuestos = TotalImpuestos + ((precioConDescuento * Partida.Cantidad) * fTasaImpuesto)

                End If



                If Partida.Descuento > 0 Then
                    precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                Else
                    precioConDescuento = Partida.Precio
                End If

                If Partida.Moneda <> sMonedasistema Then
                    Session("Moneda") = sMonedasistema
                    objDatos.fnLog("Carrito-TC", "Partida moneda <> moneda: " & Partida.Moneda & " <> " & Session("Moneda"))
                    ''Multiplicamos el precio por el tipo de cambio
                    If CDbl(Session("TC")) > 0 Then
                        If Partida.Descuento > 0 Then
                            sSubTotal = sSubTotal + (Partida.Cantidad * (Partida.Precio * CDbl(Session("TC"))))
                        Else
                            sSubTotal = sSubTotal + (Partida.Cantidad * (precioConDescuento * CDbl(Session("TC"))))
                        End If

                    Else
                        If Partida.Descuento > 0 Then
                            sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                        Else
                            sSubTotal = sSubTotal + (Partida.Cantidad * precioConDescuento)
                        End If

                    End If

                Else
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)



                End If



            End If
        Next
        lblSubTotal.Text = Session("Moneda") & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = Session("Moneda") & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0

        If objDatos.fnObtenerCliente().ToUpper.Contains("STOP CAT") Then
            fnAgregaFletesSeguros_StopCatalogo()
        End If
        Try
            If lblEnvio.Text = "" Then
                'lblEnviotxt.Text = ""
                lblEnviotxt.Visible = False
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace("$ ", "").Replace(Session("Moneda"), ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
                lblDesctxt.Visible = False
            Else
                ' Descuento = CDbl(lblDescuento.Text.Replace("$ ", "").Replace(Session("Moneda"), ""))
                Descuento = TotDescuento
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try

        lblSubTotal.Text = "$ " & sSubTotal.ToString(" ###,###,###.#0") & " " & Session("Moneda")

        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = Session("Moneda") & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0")
            lblDesctxt.Visible = True
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        End If

        lblTotal.Text = "$ " & CDbl(sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0") & " " & Session("Moneda")

        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)




        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = "$ " & TotalImpuestos.ToString(" ###,###,###.#0") & " " & Session("Moneda")
                lbltotalImp.Text = "$ " & (CDbl(Session("TotalCarrito")) + TotalImpuestos).ToString(" ###,###,###.#0") & " " & Session("Moneda")
                Session("TotalImpuestos") = TotalImpuestos
            End If
        End If

    End Sub

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub

    Protected Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        objDatos.fnLog("btnContinuar", "Entra")
        If chkInvitado.Checked = True And Session("UserB2C") = "" Then
            Session("usrInvitado") = "SI"
            Session("CorreoInvitado") = txtCorreoInvitado.Text
        Else
            Session("usrInvitado") = "NO"
        End If
        If txtCalle.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la calle")
            '   objDatos.Mensaje("Debe indicar la calle", Me.Page)
            Exit Sub
        Else
            Session("CalleEnvio") = txtCalle.Text
        End If
        If txtColonia.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la colonia")
            '    objDatos.Mensaje("Debe indicar la colonia", Me.Page)
            Exit Sub
        Else
            Session("ColoniaEnvio") = txtColonia.Text
        End If
        If txtCiudad.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la ciudad")
            '  objDatos.Mensaje("Debe indicar la ciudad", Me.Page)
            Exit Sub
        Else
            Session("CiudadEnvio") = txtCiudad.Text
        End If

        If txtMunicipio.Visible = True Then
            If txtMunicipio.Text = "" Then
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el Municipio")
                '   objDatos.Mensaje("Debe indicar el Municipio", Me.Page)
                Exit Sub
            Else
                Session("MunicipioEnvio") = txtMunicipio.Text
            End If



        End If
        Try
            If ddlLocalidad.Visible = True And ddlLocalidad.SelectedItem.Text = "" Then
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la localidad")
                '    objDatos.Mensaje("Debe indicar la localidad", Me.Page)
                Exit Sub
            Else

                Session("MunicipioEnvio") = ddlLocalidad.SelectedItem.Text
                Session("PoblacionEnvio") = ddlLocalidad.SelectedItem.Text
            End If

        Catch ex As Exception

        End Try

        If txtNumExt.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar número exterior")
            '  objDatos.Mensaje("Debe indicar número exterior", Me.Page)
            Exit Sub
        Else
            Session("NumExtEnvio") = txtNumExt.Text
        End If
        If ddlEstados.SelectedItem.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el estado")
            ' objDatos.Mensaje("Debe indicar el estado", Me.Page)
            Exit Sub
        Else
            Session("EstadoEnvio") = ddlEstados.SelectedValue
        End If
        If ddlPais.SelectedItem.Text = "" And ddlPais.Visible = True Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el país")
            '  objDatos.Mensaje("Debe indicar el País", Me.Page)
            Exit Sub
        Else
            Session("PaisEnvio") = ddlPais.SelectedValue
        End If

        If ddlUsoCFDI.SelectedItem.Text.ToUpper <> "-SELECCIONE-" Then

            Session("UsoCFDI") = ddlUsoCFDI.SelectedValue
        End If
        If pnlFormaPagoHawk.Visible = True Then
            If ddlFormasPago.SelectedItem.Text.ToUpper <> "-SELECCIONE-" Then

                Session("FormaPagoCFDI") = ddlFormasPago.SelectedValue
            End If
        End If


        If txtRFC.Text <> "" Then

            If txtRFC.Text.Length < 12 Or txtRFC.Text.Length > 13 Or txtRFC.Text.Contains(" ") Then
                ' objDatos.Mensaje("Debe proporcionar un RFC válido", Me.Page)
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe proporcionar un RFC válido")
                Exit Sub
            End If
            Session("RFCEnvio") = txtRFC.Text.ToUpper
        End If


        If txtCP.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el código postal")
            ' objDatos.Mensaje("Debe indicar el código postal", Me.Page)
            Exit Sub
        Else
            Session("CPEnvio") = txtCP.Text
        End If
        Session("TelefonoEnvio") = txtTelefono.Text
        Session("NumInteriorEnvio") = txtNumInt.Text

        Session("NombreEnvio") = txtNombre.Text
        '   Session("NumIntEnvio") = txtNumInt.Text

        If ddlDirecciones.Items.Count > 0 Then
            If ddlDirecciones.SelectedItem.Text = "-Seleccione-" Then
                Session("NombreDireccionEnvio") = "Nueva Direccion"
            Else
                Session("NombreDireccionEnvio") = ddlDirecciones.SelectedItem.Text
            End If

        End If
        If txtmensaje.Visible = True Then
            Session("MensajeRegalo") = txtmensaje.Text
        End If

        If ddlUsoCFDI.SelectedItem.Text.ToUpper <> "-SELECCIONE-" Then
            Response.Redirect("domiciliofiscal.aspx")
        Else
            Response.Redirect("envio.aspx")
        End If


    End Sub
    Protected Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''Hacemos el login
        If txtUsuario.Text = "" Then
            objDatos.Mensaje("Especifique un usuario", Me.Page)
            Exit Sub
        End If
        If txtPassword.Text = "" Then
            objDatos.Mensaje("Especifique una contraseña", Me.Page)
            Exit Sub
        End If
        ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUsuario.Text & "' AND cvPass=" & "'" & txtPassword.Text & "' and cvTipoAcceso='B2C' "
            Dim dtLogin As New DataTable
        dtLogin = objDatos.fnEjecutarConsulta(ssql)
        If dtLogin.Rows.Count > 0 Then
            pnlCombodirecciones.Visible = True
            pnlIngresar.Visible = False
            pnlDireccion.Visible = True
            Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
            Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
            Session("Cliente") = dtLogin.Rows(0)("cvCardCode")

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


            ''Cargamos las direcciones del cliente seleccionado
            ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
            ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
            Dim dtdirecciones As New DataTable
            dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)

            Dim fila As DataRow
            fila = dtdirecciones.NewRow
            fila("Direccion") = "-Seleccione-"
            dtdirecciones.Rows.Add(fila)

            ddlDirecciones.DataSource = dtdirecciones
            ddlDirecciones.DataTextField = "Direccion"
            ddlDirecciones.DataValueField = "Direccion"
            ddlDirecciones.DataBind()
            ddlDirecciones.SelectedValue = "-Seleccione-"
            ''Cargamos el nombre del cliente
            ssql = objDatos.fnObtenerQuery("NombreCliente")
            ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
            Dim dtNombre As New DataTable
            dtNombre = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtNombre.Rows.Count > 0 Then
                txtNombre.Text = dtNombre.Rows(0)(1)
            End If
        Else
            objDatos.Mensaje("Acceso incorrecto", Me.Page)
            Exit Sub
        End If
    End Sub
    Protected Sub ddlDirecciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDirecciones.SelectedIndexChanged
        Try
            ''Obtenemos el detalle de la dirección de envio
            ssql = objDatos.fnObtenerQuery("DetalleDireccion")
            ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)
            Dim dtDetalleDireccion As New DataTable
            dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtDetalleDireccion.Rows.Count > 0 Then
                txtCalle.Text = dtDetalleDireccion.Rows(0)("Calle")
                txtColonia.Text = dtDetalleDireccion.Rows(0)("Colonia")
                txtCiudad.Text = dtDetalleDireccion.Rows(0)("Ciudad")
                txtMunicipio.Text = dtDetalleDireccion.Rows(0)("Ciudad")
                txtNumExt.Text = dtDetalleDireccion.Rows(0)("Numero")
                txtCP.Text = dtDetalleDireccion.Rows(0)("CP")
                txtEstado.Text = dtDetalleDireccion.Rows(0)("Estado")
                ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")

                ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
                Dim dtLogin As New DataTable
                dtLogin = objDatos.fnEjecutarConsulta(ssql)
                If dtLogin.Rows.Count > 0 Then
                    txtTelefono.Text = dtLogin.Rows(0)("Tel1")
                    txtRFC.Text = dtLogin.Rows(0)("RFC")
                    txtNombre.Text = dtLogin.Rows(0)("cvNombreCompleto")
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            ''Cargamos los estados
            Dim fila As DataRow
            ssql = objDatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
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
    End Sub

    Private Sub chkInvitado_CheckedChanged(sender As Object, e As EventArgs) Handles chkInvitado.CheckedChanged
        Try
            If chkInvitado.Checked = True Then
                pnlInvitado.Visible = True
                If Session("UserB2C") = "" Then
                    pnlDireccion.Visible = False
                Else
                    pnlDireccion.Visible = True
                End If
                pnlRegistrar.Visible = False
                pnlDireccion.Visible = True
            Else
                pnlDireccion.Visible = False
                pnlRegistrar.Visible = True
                pnlInvitado.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ddlEstados_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEstados.SelectedIndexChanged
        Try
            ''Si el cliente es PMK, cargamos las localidades
            If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("PMK") Then
                objDatos.fnLog("Estado PMK", "Si entra")
                Dim fila As DataRow
                ssql = objDatos.fnObtenerQuery("Poblaciones")
                ssql = ssql.Replace("[%0]", ddlEstados.SelectedItem.Text)

                Dim dtPoblacion = objDatos.fnEjecutarConsultaSAP(ssql)

                objDatos.fnLog("estado index", "")

                fila = dtPoblacion.NewRow
                fila("Clave") = "0"
                fila("Descripcion") = "-Seleccione-"
                dtPoblacion.Rows.Add(fila)

                ddlLocalidad.Visible = True
                txtMunicipio.Visible = False
                ddlLocalidad.DataSource = dtPoblacion
                ddlLocalidad.DataTextField = "Descripcion"
                ddlLocalidad.DataValueField = "Clave"
                ddlLocalidad.DataBind()
                ddlLocalidad.SelectedValue = "0"
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub chkRegalo_CheckedChanged(sender As Object, e As EventArgs) Handles chkRegalo.CheckedChanged
        Try
            If chkRegalo.Checked Then
                pnlRegaloDelta.Visible = True
            Else
                pnlRegaloDelta.Visible = False
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ddlTipoMensaje_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoMensaje.SelectedIndexChanged
        Try
            ''Llenamos los mensajes
            ssql = objDatos.fnObtenerQuery("MensajesRegalo_detalle")
            ssql = ssql.Replace("[%0]", ddlTipoMensaje.SelectedValue)
            Dim dtMensajes As New DataTable
            dtMensajes = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtMensajes.Rows.Count > 0 Then
                txtmensaje.Text = dtMensajes.Rows(0)(0)
            End If


        Catch ex As Exception

        End Try
    End Sub

    Private Sub ddlTipoMensaje_TextChanged(sender As Object, e As EventArgs) Handles ddlTipoMensaje.TextChanged

    End Sub
End Class
