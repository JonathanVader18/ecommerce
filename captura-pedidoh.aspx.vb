
Imports System.Data
Imports System.Data.SqlClient
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Xml.Serialization
Imports System.Data.OleDb
Imports System.IO
Partial Class captura_pedidoh
    Inherits System.Web.UI.Page
    Public objdatos As New Cls_Funciones
    Public ssql As String
    Private Sub captura_pedidozeyco_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            objdatos.fnLog("Zeyco IsB2B", Session("IsB2B"))
            objdatos.fnLog("Zeyco RazonSocial", Session("RazonSocial"))
            objdatos.fnLog("Zeyco Cliente", Session("Cliente"))




            If Not IsPostBack Then
                Session("ArchivoLargo") = "NO"
                If objdatos.fnObtenerCliente.ToUpper.Contains("MANIJ") Or objdatos.fnObtenerCliente.ToUpper.Contains("SUJEA") Then
                    Session("TC") = "1"
                    ssql = objdatos.fnObtenerQuery("GetBalanceCustomer")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", Session("Cliente"))
                        Dim dtSaldoCliente As New DataTable
                        dtSaldoCliente = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtSaldoCliente.Rows.Count > 0 Then
                            If CDbl(dtSaldoCliente.Rows(0)(0)) + CDbl(Session("TotalCarrito")) > 0 Then
                                ''Saldo > a límite de crédito

                                btnPedido.Enabled = False
                                btnCotizar.Visible = True

                                objdatos.Mensaje("El cliente no cuenta con crédito disponible para realizar el pedido.", Me.Page)
                            Else

                            End If
                        End If
                    End If
                End If

                Dim fila As DataRow
                ''Cargamos los paises
                ssql = objdatos.fnObtenerQuery("Paises")
                Dim dtPais As New DataTable
                dtPais = objdatos.fnEjecutarConsultaSAP(ssql)
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

                    ssql = objdatos.fnObtenerQuery("Estados")
                    ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)

                    dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)

                    objdatos.fnLog("pais index", "")

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
                    objdatos.fnLog("Cargar estados", ex.Message)
                End Try

                If CStr(objdatos.fnObtenerCliente()).Contains("SUJEA") Or CStr(objdatos.fnObtenerCliente()).Contains("MANIJ") Then
                    pnlOtros.Visible = True
                    pnlSuje.Visible = True
                End If
                If objdatos.fnObtenerCliente().ToUpper.Contains("SUJEAU") Or CStr(objdatos.fnObtenerCliente()).Contains("MANIJ") Then
                    ''Cargamos las paqueterías
                    Dim sQueryadicional As String = ""
                    Try

                        sQueryadicional = objdatos.fnObtenerQuery("Paqueterias")
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objdatos.fnEjecutarConsultaSAP(sQueryadicional)
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
                            sQueryadicional = objdatos.fnObtenerQuery("PaqueteriaCliente")
                            sQueryadicional = sQueryadicional.Replace("[%0]", Session("Cliente"))
                            objdatos.fnLog("Paq cliente:", sQueryadicional.Replace("'", ""))
                            Dim dtPaq As New DataTable
                            dtPaq = objdatos.fnEjecutarConsultaSAP(sQueryadicional)
                            If dtPaq.Rows.Count > 0 Then
                                ddlPaqueteria.SelectedValue = dtPaq.Rows(0)(0)
                            Else
                                objdatos.fnLog("Paq cliente:", sQueryadicional.Replace("'", ""))
                                ddlPaqueteria.SelectedValue = 0
                            End If
                        End If
                    Catch ex As Exception
                        objdatos.fnLog("Paqueterias:", ex.Message.Replace("'", ""))
                    End Try

                    pnlPaqueteria.Visible = True
                    ''Forma de envio
                    pnlTipoEnvio.Visible = True

                    Try
                        sQueryadicional = objdatos.fnObtenerQuery("TipoEnvio")
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objdatos.fnEjecutarConsultaSAP(sQueryadicional)
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
                        objdatos.fnLog("Tipo Envio:", ex.Message.Replace("'", ""))
                    End Try

                    Try
                        sQueryadicional = objdatos.fnObtenerQuery("UsoCFDI")
                        If sQueryadicional <> "" Then
                            Dim dtTrans As New DataTable
                            dtTrans = objdatos.fnEjecutarConsultaSAP(sQueryadicional)


                            ddlUsoCFDI.DataSource = dtTrans
                            ddlUsoCFDI.DataValueField = "Usage"
                            ddlUsoCFDI.DataTextField = "Descr"
                            ddlUsoCFDI.DataBind()

                            ''Obtenemos el uso de cfdi default del cliente para que aparezca seleccionada
                            sQueryadicional = objdatos.fnObtenerQuery("UsoCFDIcliente")
                            sQueryadicional = sQueryadicional.Replace("[%0]", Session("Cliente"))
                            Dim dtPaq As New DataTable
                            dtPaq = objdatos.fnEjecutarConsultaSAP(sQueryadicional)
                            If dtPaq.Rows.Count > 0 Then
                                ddlUsoCFDI.SelectedValue = dtPaq.Rows(0)(0)
                            End If

                        End If

                    Catch ex As Exception
                        objdatos.fnLog("Uso de CFDI:", ex.Message.Replace("'", ""))
                    End Try



                End If
                Try
                    If Session("Partidas").count > 0 Then
                    Else
                        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                    End If
                Catch ex As Exception
                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                End Try

                '  HttpContext.Current.Session("ListaPrecios") = "2"
                txtcantidad.Text = "1"

                If Session("IsB2B") = "SI" Then
                Else
                    ''es el load, la variable debe estar llena
                    If Session("UserTienda") <> "" Then
                        lblUsuario.Text = Session("UserTienda")
                    Else
                        If lblUsuario.Text <> "" Then

                            fnCargaUsuario()


                        End If
                    End If
                End If

                If Request.QueryString.Count > 0 Then
                    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                        If Partida.ItemCode = Request.QueryString("item") And Request.QueryString("Action") = "d" Then
                            Partida.ItemCode = "BORRAR"

                            Response.Redirect("captura-pedidoh.aspx")
                        End If
                    Next
                End If


                '''Las personas de contacto del cliente
                'ssql = "SELECT CntctCode as Id, ISNULL(Name,'') + '( ' + ISNULL(Position,'') + ' )' + ISNULL(firstname,'') + ' ' + ISNULL(middlename,'') + ' ' + ISNULL(LastName,'') as Persona from OCPR WHERE CardCode ='" & Session("Cliente") & "'"
                'objdatos.fnLog("Contacto", ssql.Replace("'", ""))
                'Dim dtPersonas As New DataTable
                'dtPersonas = objdatos.fnEjecutarConsultaSAP(ssql)
                'ddlPersonaContacto.DataSource = dtPersonas
                'ddlPersonaContacto.DataTextField = "Persona"
                'ddlPersonaContacto.DataValueField = "Id"
                'ddlPersonaContacto.DataBind()


                ''Cargamos las monedas
                Dim dtMonedas As New DataTable
                dtMonedas.Columns.Add("Cve")
                dtMonedas.Columns.Add("Descripcion")

                fila = dtMonedas.NewRow
                fila("Cve") = "USD"
                fila("Descripcion") = "USD"
                dtMonedas.Rows.Add(fila)
                fila = dtMonedas.NewRow
                fila("Cve") = "MXP"
                fila("Descripcion") = "MXP"
                dtMonedas.Rows.Add(fila)
                ddlMoneda.DataSource = dtMonedas
                ddlMoneda.DataTextField = "Descripcion"
                ddlMoneda.DataValueField = "Cve"
                ddlMoneda.DataBind()
                ddlMoneda.SelectedValue = "MXP"
                ddlMoneda.Enabled = False

                ssql = "SELECT ISNULL(cvArticuloGEN,'') FROM config.Parametrizaciones"
                Dim dtArticuloGen As New DataTable
                dtArticuloGen = objdatos.fnEjecutarConsulta(ssql)
                If dtArticuloGen.Rows.Count > 0 Then
                    If dtArticuloGen.Rows(0)(0) <> "" Then
                        Session("ArtGenerico") = dtArticuloGen.Rows(0)(0)
                    End If
                End If
                ''Vendedores
                pnlSeparador.Visible = True
                ''Revisamos en la tabla parametrizaciones, si debemos ocultar algún boton

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
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


                If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                    btnPedido.Visible = False
                    btnCotizar.Visible = True
                Else
                    ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='Vendedores' and cvDocto ='PEDIDO'"
                    Dim dtBotonPedido As New DataTable
                    dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
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
                End If



                btnProcesar.Visible = False
                '  btnGuardarPlantilla.Visible = True
                'pnlProcesar.Visible = False


                If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                    txtDesc1.Visible = False
                    lbldesc1.Visible = False
                Else
                    ssql = "select ISNULL(cvAplicaDescto,'NO') as AplicaDesc from config.Parametrizaciones "
                    Dim dtParam As New DataTable
                    dtParam = objdatos.fnEjecutarConsulta(ssql)
                    If dtParam.Rows.Count > 0 Then
                        If dtParam.Rows(0)("AplicaDesc") = "SI" Then

                            txtDesc1.Visible = True
                            lbldesc1.Visible = True

                        Else


                            txtDesc1.Visible = False
                            lbldesc1.Visible = False

                        End If
                    End If
                End If

                Dim sFechaCot As String()
                Dim sFechaVence As String()

                sFechaCot = Now.Date.ToString("dd/MM/yyyy").Split("/")
                sFechaVence = Now.Date.ToString("dd/MM/yyyy").Split("/")


                txtFechaCot.Text = sFechaCot(2) & "-" & sFechaCot(1) & "-" & sFechaCot(0)
                txtFechaVence.Text = sFechaVence(2) & "-" & sFechaVence(1) & "-" & sFechaVence(0)
                fnCargaCarrito()





            Else
                If Session("IsB2B") = "SI" Then
                Else
                    ''es el load, la variable debe estar llena
                    If Session("UserTienda") <> "" Then
                        lblUsuario.Text = Session("UserTienda")
                    Else
                        If lblUsuario.Text <> "" Then

                            fnCargaUsuario()


                        End If
                    End If
                End If

                If txtSearch.Text <> "" Then

                    Dim Articulo As String = ""

                    If txtSearch.Text.Contains("|") Then
                        Dim sSeleccionado As String() = txtSearch.Text.Split("|")
                        Articulo = sSeleccionado(0)
                    Else
                        Articulo = txtSearch.Text
                    End If

                    ssql = "SELECT * FROM OITM Where itemCode=" & "'" & Articulo & "'"
                    ssql = objdatos.fnObtenerQuery("Nombre-Producto")
                    ssql = ssql.Replace("[%0]", "'" & Articulo & "'")

                    Dim dtValida As New DataTable
                    dtValida = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtValida.Rows.Count = 0 Then
                        CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no existe, asegúrese de seleccionar el artículo de la lista")
                        Exit Sub

                    End If

                    Dim dPrecioActual As Double = 0
                    If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then

                        dPrecioActual = objdatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                    Else
                        dPrecioActual = objdatos.fnPrecioActual(Articulo)
                    End If

                    ' HttpContext.Current.Session("ListaPrecios") = "2"

                    If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                        dPrecioActual = objdatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                    Else
                        ' HttpContext.Current.Session("ListaPrecios") = "2"
                    End If
                    dPrecioActual = objdatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))

                    If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
                        ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
                        Dim dtTc As New DataTable
                        dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
                        objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
                        Dim iTC As Double = 1
                        If dtTc.Rows.Count > 0 Then
                            iTC = dtTc.Rows(0)(0)
                        End If

                        If fnObtenerMoneda(Articulo) <> "USD" Then
                            objdatos.fnLog("Precio Moneda", dPrecioActual & " / " & iTC)
                            objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                            dPrecioActual = dPrecioActual / iTC
                        Else
                            objdatos.fnLog("Precio Moneda", dPrecioActual & " * " & iTC)
                            objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
                            dPrecioActual = dPrecioActual * iTC
                        End If



                    Else

                    End If

                    If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                        lblExistencia.Text = ""
                    Else
                        ssql = "select onhand from OITW where itemcode='" & Articulo & "' and whscode ='00'"
                        Dim dtExistencia As New DataTable
                        dtExistencia = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtExistencia.Rows.Count > 0 Then
                            lblExistencia.Text = "Existencia: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N2")
                        End If
                    End If

                    If txtPrecioArt.Text = "" Then
                        txtPrecioArt.Text = dPrecioActual
                    Else
                        If txtPrecioArt.Text = "0" Then
                            txtPrecioArt.Text = dPrecioActual
                        End If
                    End If


                End If


                ' fnCargaCarrito()
            End If

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                btnPedido.Visible = False
                btnCotizar.Visible = True
                pnlfechas.Visible = False
                pnlComentarios.Visible = True

            Else
                btnPedido.Visible = False
                btnCotizar.Visible = True
                lblExistencia.Text = "...."
            End If

            btnPedido.Visible = True


        Catch ex As Exception
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema, favor de reportarlo con esta descripción: " & ex.Message)
            Exit Sub
        End Try

    End Sub

    Private Sub btnPedido_Click(sender As Object, e As EventArgs) Handles btnPedido.Click
        fnGuardaCarrito("PEDIDO")
    End Sub

    Private Sub btnCotizar_Click(sender As Object, e As EventArgs) Handles btnCotizar.Click
        fnGuardaCarrito("COTIZACION")
    End Sub

    Private Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        Dim Articulo As String = ""
        Dim sSeleccionado As String()
        Dim ssql As String
        Dim objDatos As New Cls_Funciones

        If txtcantidad.Text = "" Then
            txtcantidad.Text = "1"
        End If
        Try
            If txtSearch.Text.Contains("|") Then
                sSeleccionado = txtSearch.Text.Split("|")
                Articulo = sSeleccionado(0)
            Else
                Articulo = txtSearch.Text
                If fnExisteArticulo(txtSearch.Text) = 0 Then
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no existe, quizá no seleccionó el elemento de la lista")
                    fnCargaCarrito()
                    'objDatos.Mensaje("El artículo ingresado no existe, quizá no seleccionó el elemento de la lista", Me.Page)
                    Exit Sub
                End If
            End If

            ''validacion de las existencias

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                ''B2B zeyco no revisa existencias
            Else
                If fnRevisaExistencias(Articulo) - CDbl(txtcantidad.Text) <= 0 Then
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no cuenta con la existencia suficiente")
                    'objDatos.Mensaje("El artículo ingresado no existe, quizá no seleccionó el elemento de la lista", Me.Page)
                    fnCargaCarrito()
                    Exit Sub
                End If
            End If

            Dim partida As New Cls_Pedido.Partidas


            Dim dPrecioActual As Double = 0

            objDatos.fnLog("articulo descuento", Articulo)

            partida.ItemCode = Articulo
            partida.Cantidad = txtcantidad.Text

            ''Revisamos si aplica descuentos

            If txtDesc1.Text = "" Then
                txtDesc1.Text = "0"
            End If

            Dim dDescuento As Double = 0
            Try
                dDescuento = CDbl(txtDesc1.Text)

            Catch ex As Exception
                ''Descuento invalido,
            End Try


            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then

            Else
                ssql = "SELECT ISNULL(cvAplicaDescto,'NO') , ISNULL(cfMaxDescto,0) FROM config.parametrizaciones "
                Dim dtDescuento As New DataTable
                dtDescuento = objDatos.fnEjecutarConsulta(ssql)

                If dtDescuento.Rows.Count > 0 Then
                    If CDbl(dtDescuento.Rows(0)(1)) < CDbl(txtDesc1.Text) Then
                        CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El descuento que está otorgando es mayor al permitido")
                        fnCargaCarrito()
                        '  objDatos.Mensaje("El descuento que está otorgando es mayor al permitido", Me.Page)
                        Exit Sub
                    Else
                        If txtDesc1.Visible = True Then
                            If txtDesc1.Text <> "" Then
                                If CDbl(txtDesc1.Text) > 0 Then
                                    objDatos.fnLog("Asigna descuento", txtDesc1.Text)
                                    partida.Descuento = CDbl(txtDesc1.Text)
                                End If
                            End If

                        End If
                    End If
                End If
            End If

            Dim fDescuento As Double = 0
            fDescuento = objDatos.fnDesctoB2C(Articulo)
            If fDescuento = 0 Then
                fDescuento = objDatos.fnObtenerDescuentoEspecialDelta(Articulo)
            End If
            partida.Descuento = fDescuento
            ' partida.Descuento = desc

            dPrecioActual = objDatos.fnPrecioActual(Articulo, Session("ListaPrecios"))

            Try
                ssql = objDatos.fnObtenerQuery("Nombre-Producto")
                ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
                Dim dtItemName As New DataTable
                dtItemName = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtItemName.Rows.Count > 0 Then
                    partida.ItemName = dtItemName.Rows(0)(0)
                End If

            Catch ex As Exception

            End Try

            If txtPrecioArt.Text <> "" Then
                Try
                    If CDbl(txtPrecioArt.Text) <> 0 Then
                        dPrecioActual = CDbl(txtPrecioArt.Text)
                    End If

                Catch ex As Exception
                    dPrecioActual = 0
                End Try
            End If
            'If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
            '    ssql = objDatos.fnObtenerQuery("Tipo de Cambio")
            '    Dim dtTc As New DataTable
            '    dtTc = objDatos.fnEjecutarConsultaSAP(ssql)
            '    objDatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
            '    Dim iTC As Double = 1
            '    If dtTc.Rows.Count > 0 Then
            '        iTC = dtTc.Rows(0)(0)
            '    End If
            '    objDatos.fnLog("Conversion Moneda", "TC: " & iTC)
            '    If txtPrecioArt.Text <> "" Then
            '    Else
            '        dPrecioActual = dPrecioActual * iTC
            '    End If


            'Else

            'End If
            'If fnObtenerMoneda(Articulo) <> ddlMoneda.SelectedValue Then
            '    partida.MonedaGen = ddlMoneda.SelectedValue
            '    partida.Moneda = ddlMoneda.SelectedValue
            'Else
            '    partida.Moneda = fnObtenerMoneda(Articulo)
            '    partida.MonedaGen = fnObtenerMoneda(Articulo)
            'End If

            partida.Moneda = ddlMoneda.SelectedValue

            Dim IVA As Double = 0
            IVA = fnTasaIVA(Articulo)
            'dPrecioActual = dPrecioActual * (1 + IVA)

            partida.Precio = dPrecioActual
            partida.TotalLinea = partida.Cantidad * partida.Precio


            Session("Partidas").add(partida)
            txtPrecioArt.Text = "0"
            txtcantidad.Text = "1"


            txtSearch.Text = ""
            fnCargaCarrito()

            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                btnPedido.Visible = False
                btnCotizar.Visible = True

            Else
                btnPedido.Visible = True
                btnCotizar.Visible = True

                lblExistencia.Text = "...."
            End If

        Catch ex As Exception
            objDatos.fnLog("captura-pedido ex", ex.Message.Replace("'", ""))
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ha ocurrido un problema, favor de reportarlo con esta descripción: " & ex.Message)
        End Try
    End Sub

    Private Sub btnGuardarPlantilla_Click(sender As Object, e As EventArgs) Handles btnGuardarPlantilla.Click

    End Sub

    Private Sub btnGuardarPlantillaDet_Click(sender As Object, e As EventArgs) Handles btnGuardarPlantillaDet.Click

    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        fndescargaPDF(Session("DocEntry"))
    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Response.Redirect("captura-pedidoh.aspx")
    End Sub

    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click

    End Sub

    <Services.WebMethod()>
    Public Shared Function GetCustomers(prefix As String) As String()
        Dim customers As New List(Of String)()
        Dim objdatos As New Cls_Funciones
        Dim ssql As String
        Exit Function
        'objdatos.fnLog("auto", "entra")
        Using conn As New SqlConnection()
            ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
            Dim dtConfSAP As New DataTable
            dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
            If dtConfSAP.Rows.Count > 0 Then
                'conn.ConnectionString = "Data Source=PDGDL-SQL;Initial Catalog=IP;User ID=sa;Password=P3g4dur0"
                conn.ConnectionString = "Data Source=" & dtConfSAP.Rows(0)("cvServidorSQL") & ";Initial Catalog=" & dtConfSAP.Rows(0)("cvBD") & ";User ID=" & dtConfSAP.Rows(0)("cvUserSQL") & ";Password=" & dtConfSAP.Rows(0)("cvPwdSQL")
                '   objdatos.fnLog("auto", "ConnectionString")
            End If

            ssql = objdatos.fnObtenerQuery("ArticulosAutoComplete")
            '    objdatos.fnLog("auto", ssql.Replace("'", ""))
            Using cmd As New SqlCommand()
                cmd.CommandText = ssql '"SELECT ItemCode,ItemName,(ItemCode +  '-' + ISNULL(ItemName,'')) as Descripcion FROM OITM WHERE ValidFor='Y'  AND (ItemCode like @SearchText + '%' OR ItemName like @SearchText + '%')"
                cmd.Parameters.AddWithValue("@SearchText", prefix)
                cmd.Connection = conn
                conn.Open()
                Using sdr As SqlDataReader = cmd.ExecuteReader()
                    While sdr.Read()
                        '  objdatos.fnLog("auto", sdr("Descripcion"))
                        customers.Add(String.Format("{0}@{1}", sdr("Descripcion"), sdr("ItemCode")))
                    End While
                End Using
                conn.Close()
            End Using
        End Using
        Return customers.ToArray()
    End Function
    Public Function fnObtenerMoneda(ItemCode As String) As String
        Dim ssql As String = ""
        ''Posibles monedas en la lista de precios
        ''Si la lista de precios que estamos manejando, tiene precio tmb en otra moneda, pintar combo con las posibles monedas
        ssql = objdatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & ItemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & Session("ListaPrecios") & "'")
        dtMonedas = objdatos.fnEjecutarConsultaSAP(ssql)
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
        Session("Moneda") = sMoneda
        Return sMoneda
    End Function

    Public Function fnTasaIVA(ItemCode As String) As Double
        Dim fTasaImpuesto As Double = 0

        ssql = objdatos.fnObtenerQuery("ObtenerIVA")
        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", ItemCode)
            ssql = ssql.Replace("[%1]", Session("Cliente"))
            Dim dtIVA As New DataTable
            dtIVA = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtIVA.Rows.Count > 0 Then
                If objdatos.fnObtenerDBMS = "HANA" Then
                    ssql = objdatos.fnObtenerQuery("GetRateTax")
                    If dtIVA.Rows(0)(0) Is DBNull.Value Then
                        fTasaImpuesto = 0

                    Else
                        ssql = ssql.Replace("[%0]", dtIVA.Rows(0)(0))
                    End If

                Else
                    ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                End If

                Dim dtTasa As New DataTable
                dtTasa = objdatos.fnEjecutarConsultaSAP(ssql)
                If dtTasa.Rows.Count > 0 Then
                    fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                Else
                    fTasaImpuesto = 0
                End If

            End If

        End If

        Return fTasaImpuesto

    End Function
    Public Sub fnCargaCarrito()
        ''Preparamos el encabezado del Grid
        Dim sHtmlBanner As String = ""
        Dim sHtmlEncabezado As String = ""
        Dim sTallaColor As String = "NO"

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Dim fTasaImpuesto As Double = 0
        Dim TotalImpuestos As Double = 0
        Dim sCaracterMoneda As String = "$"
        Try
            objdatos.fnLog("Carrito", "For de partidas")
            Dim totalArticulos As Int16
            Dim contador As Int16 = 0
            totalArticulos = Session("Partidas").count()

            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    x = x + 1
                    If x > 20 Then
                        lblExistencia.Text = "Mostrando 20 productos de " & totalArticulos
                        Exit For
                    End If
                    objdatos.fnLog("Carrito", Partida.ItemCode)
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objdatos.fnObtenerQuery("Info-Producto")
                            If Partida.Generico <> "" Then
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")

                            End If
                            fnObtenerMoneda(Partida.ItemCode)
                            objdatos.fnLog("Info-prod", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objdatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objdatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-responsive'>"
                                        End If
                                    Else
                                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
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

                                        objdatos.fnLog("Carrito itemNAme", Partida.ItemName)
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 info-producto'> " & sCampos & "</div>"

                    End If
                    objdatos.fnLog("Carrito", "Antes de precio")

                    If Not IsPostBack Then

                    End If
                    If Partida.Precio = 0 Then
                        Dim dPrecioActual As Double
                        If CInt(Session("slpCode")) <> 0 Then

                            dPrecioActual = fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        Else
                            dPrecioActual = objdatos.fnPrecioActual(Partida.ItemCode)
                        End If
                        If Session("Cliente") <> "" Then
                            dPrecioActual = fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                        End If
                        Partida.Precio = dPrecioActual
                        Partida.TotalLinea = Partida.Cantidad * dPrecioActual
                    End If

                    ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
                    Dim dtTc As New DataTable
                    dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
                    objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
                    Dim iTC As Double = 1
                    If dtTc.Rows.Count > 0 Then
                        iTC = dtTc.Rows(0)(0)
                    End If
                    objdatos.fnLog("Conversion Moneda", "TC: " & iTC)

                    'If fnObtenerMoneda(Partida.ItemCode) <> ddlMoneda.SelectedValue Then
                    Dim PrecioLista As Double = 0
                    Dim DifPrecio As Double = 0
                    Dim MonedaOriginal As String = ""

                    MonedaOriginal = fnObtenerMoneda(Partida.ItemCode)
                    PrecioLista = fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))


                    'If Partida.ItemCode <> Session("ArtGenerico") Then

                    '    DifPrecio = Partida.Precio - PrecioLista
                    '    If DifPrecio < 0 Then
                    '        DifPrecio = DifPrecio * -1
                    '    End If

                    '    If DifPrecio > 1 Then
                    '        ''Diferencia mayor a 1 peso, se entiende que esta en una moneda diferente a la original y hay que dividir
                    '        If MonedaOriginal <> "USD" Then
                    '            If Partida.MonedaGen <> ddlMoneda.SelectedValue Then
                    '                Partida.Precio = Partida.Precio * iTC
                    '            End If
                    '        Else
                    '            If Partida.MonedaGen <> ddlMoneda.SelectedValue Then
                    '                Partida.Precio = Partida.Precio / iTC
                    '            End If
                    '        End If


                    '        Partida.Moneda = MonedaOriginal
                    '    Else
                    '        If MonedaOriginal <> "USD" Then
                    '            If MonedaOriginal <> ddlMoneda.SelectedValue Then
                    '                Partida.Precio = Partida.Precio / iTC
                    '            End If
                    '        Else
                    '            If MonedaOriginal <> ddlMoneda.SelectedValue Then
                    '                Partida.Precio = Partida.Precio * iTC
                    '            End If
                    '        End If


                    '        Partida.Moneda = MonedaOriginal
                    '        Partida.MonedaGen = ddlMoneda.SelectedValue
                    '    End If
                    '    objdatos.fnLog("Monedas art", fnObtenerMoneda(Partida.ItemCode) & " -  " & ddlMoneda.SelectedValue)

                    '    If txtSearch.Text <> "" Then
                    '        txtPrecioArt.Text = Partida.Precio
                    '    End If
                    '    'If Partida.MonedaGen <> ddlMoneda.SelectedValue Then  'fnObtenerMoneda(Partida.ItemCode)
                    '    '    If fnObtenerMoneda(Partida.ItemCode) = ddlMoneda.SelectedValue Then
                    '    '        Partida.Precio = Partida.Precio / iTC 'fnPrecioActual(Partida.ItemCode, Session("ListaPrecios")) * iTC
                    '    '    Else
                    '    '        Partida.Precio = Partida.Precio * iTC 'fnPrecioActual(Partida.ItemCode, Session("ListaPrecios")) * iTC
                    '    '    End If

                    '    '    Partida.Moneda = ddlMoneda.SelectedValue

                    '    'Else
                    '    '    If Partida.MonedaGen <> Partida.Moneda Then
                    '    '        Partida.Precio = Partida.Precio / iTC 'fnPrecioActual(Partida.ItemCode, Session("ListaPrecios"))
                    '    '        Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)

                    '    '    Else
                    '    '        Partida.Moneda = fnObtenerMoneda(Partida.ItemCode)
                    '    '    End If

                    '    'End If

                    'Else
                    '    ''Es el generico
                    '    objdatos.fnLog("Monedas gen ", Partida.Moneda & " -  " & ddlMoneda.SelectedValue)

                    '    If Partida.Moneda = "" Then
                    '        Partida.Moneda = Partida.MonedaGen
                    '    End If
                    '    If Partida.MonedaGen <> ddlMoneda.SelectedValue Then
                    '        objdatos.fnLog(" Monedas", Partida.Moneda & " -  " & ddlMoneda.SelectedValue)


                    '        Partida.Precio = Partida.Precio * iTC


                    '        Partida.Moneda = ddlMoneda.SelectedValue

                    '    Else
                    '        If Partida.MonedaGen <> Partida.Moneda Then
                    '            Partida.Precio = Partida.Precio / iTC
                    '            Partida.Moneda = ddlMoneda.SelectedValue

                    '        End If

                    '    End If
                    'End If

                    ' sHtmlBanner = sHtmlBanner & "</div>"
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If

                    If Partida.Descuento > 0 Then
                        TotDescuento = TotDescuento + (Partida.Precio - precioConDescuento)
                    End If

                    objdatos.fnLog("Carrito", precioConDescuento)
                    objdatos.fnLog("Carrito partida.precio", Partida.Precio)


                    ssql = objdatos.fnObtenerQuery("ObtenerIVA")
                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", Partida.ItemCode)
                        ssql = ssql.Replace("[%1]", Session("Cliente"))
                        Dim dtIVA As New DataTable
                        dtIVA = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            If objdatos.fnObtenerDBMS = "HANA" Then
                                ssql = objdatos.fnObtenerQuery("GetRateTax")
                                If dtIVA.Rows(0)(0) Is DBNull.Value Then
                                    fTasaImpuesto = 0

                                Else
                                    ssql = ssql.Replace("[%0]", dtIVA.Rows(0)(0))
                                End If

                            Else
                                ssql = "select rate from OSTC where code='" & dtIVA.Rows(0)(0) & "'"
                            End If

                            Dim dtTasa As New DataTable
                            dtTasa = objdatos.fnEjecutarConsultaSAP(ssql)
                            If dtTasa.Rows.Count > 0 Then
                                fTasaImpuesto = CDbl(dtTasa.Rows(0)(0)) / 100
                            Else
                                fTasaImpuesto = 0
                            End If

                        End If

                    End If

                    TotalImpuestos = TotalImpuestos + (((precioConDescuento * Partida.Cantidad) / (1 + fTasaImpuesto)) * fTasaImpuesto)

                    If Partida.Descuento > 0 Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><span class='precio-con-descuento'>" & ddlMoneda.SelectedValue & " " & precioConDescuento.ToString(" ###,###,###.#0") & "</span><div class='precio-original descuento'>" & Partida.Precio.ToString("$ ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><div class='precio-original'>" & ddlMoneda.SelectedValue & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & "</div></div>"

                    End If

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div  class='precio' id='#txt" & x & "'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-total'><div class='prec-simul'> " & ddlMoneda.SelectedValue & " " & (Partida.Cantidad * precioConDescuento).ToString("###,###,###.#0") & "</div></div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart
                    sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    'PopUp('','Agregado al carrito','Aceptar');
                    If sTallaColor = "SI" Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"


                    End If
                    '  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'  href='PopUp('','Agregado al carrito','Aceptar');'>Editar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='captura-pedidoh.aspx?item=" & Partida.ItemCode & "&Action=d'>Quitar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart preview-popup' href='elegir-favoritos.aspx?code=" & Partida.ItemCode & "&name=" & Partida.ItemName & "'>Mover a favoritos</a></div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
                    sHtmlBanner = sHtmlBanner & "</div>   </div>"

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If
                objdatos.fnLog("Carrito", "Arma")




            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objdatos.fnLog("Carrito load", ex.Message)
        End Try

        objdatos.fnLog("Carrito", "SubTotales")

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlPartidas.Controls.Clear()
        pnlPartidas.Controls.Add(literal)
        lblSubTotal.Text = ddlMoneda.SelectedValue & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = ddlMoneda.SelectedValue & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
            Else
                Descuento = CDbl(lblDescuento.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
                Descuento = TotDescuento
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try

        lblTotal.Text = sCaracterMoneda & " " & (sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0") & " " & Session("Moneda")
        Session("TotalCarrito") = (sSubTotal + Envio - Descuento - TotalImpuestos)
        pnlImpuestos.Visible = True

        lblSubTotal.Text = sCaracterMoneda & " " & (sSubTotal + Envio - TotalImpuestos).ToString(" ###,###,###.#0") & "  " & Session("Moneda")
        lblTotal.Text = sCaracterMoneda & " " & (CDbl(Session("TotalCarrito"))).ToString(" ###,###,###.#0") & "  " & Session("Moneda")

        '   TotalImpuestos = TotalImpuestos * fTasaImpuesto
        Session("TotalImpuestos") = TotalImpuestos
        lblImpuestos.Text = sCaracterMoneda & " " & TotalImpuestos.ToString(" ###,###,###.#0") & "  " & Session("Moneda")
        lbltotalImp.Text = sCaracterMoneda & " " & ((sSubTotal + Envio - Descuento)).ToString(" ###,###,###.#0") & " " & Session("Moneda")

        '  objDatos.fnLog("Carrito", "slpCode y demas")

        If Session("UserB2C") <> "" Then
            pnlGuardarCarrito.Visible = False
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
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
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
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
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
                pnlGuardarCarrito.Visible = False
                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            Else
                '   pnlPayPal.Visible = True
                ' btnProcesar.Visible = True
            End If

            If Session("RazonSocial") <> "" Then
                pnlPayPal.Visible = False
                pnlSeparador.Visible = True
                ''B2B

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
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
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
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
                btnPedido.Visible = True
                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            End If
        End If
    End Sub


    Public Sub fnCargaCarritoPOST()
        ''Preparamos el encabezado del Grid
        Dim sHtmlBanner As String = ""
        Dim sHtmlEncabezado As String = ""
        Dim sTallaColor As String = "NO"

        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Try
            objdatos.fnLog("Carrito", "For de partidas")
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    x = x + 1
                    objdatos.fnLog("Carrito", Partida.ItemCode)
                    sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                    If dtCamposPlantilla.Rows.Count > 0 Then
                        Dim sCampos As String = ""
                        For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                            ssql = objdatos.fnObtenerQuery("Info-Producto")
                            If Partida.Generico <> "" Then
                                ssql = ssql.Replace("[%0]", "'" & Partida.Generico & "'")

                            Else
                                ssql = ssql.Replace("[%0]", "'" & Partida.ItemCode & "'")

                            End If
                            fnObtenerMoneda(Partida.ItemCode)
                            objdatos.fnLog("Info-prod", ssql.Replace("'", ""))

                            Dim dtGeneral As New DataTable
                            dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                            If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                                sHtmlBanner = sHtmlBanner & " <div class='producto col-xs-2 no-padding'> "


                                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                                Dim dtcliente As New DataTable
                                dtcliente = objdatos.fnEjecutarConsulta(ssql)

                                If dtcliente.Rows.Count > 0 Then
                                    If dtcliente.Rows(0)(0) = "Lazarus" Then
                                        ssql = "SELECT Distinct ISNULL(U_Foto1,'')   FROM [@EP_ITM1] where U_ItemCode ='" & Partida.ItemCode & "'"
                                        objdatos.fnLog("ddl_sel_Foto", ssql.Replace("'", ""))
                                        Dim dtFoto As New DataTable
                                        dtFoto = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtFoto.Rows.Count > 0 Then
                                            sHtmlBanner = sHtmlBanner & "   <img src='" & dtFoto.Rows(0)(0) & "' alt='productos' title='productos' class='img-responsive'>"
                                        End If
                                    Else
                                        sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
                                    End If

                                Else
                                    sHtmlBanner = sHtmlBanner & "   <img src='" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='productos' title='productos' class='img-responsive'>"
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

                                        objdatos.fnLog("Carrito itemNAme", Partida.ItemName)
                                        sCampos = sCampos & Partida.ItemName & " <br>"
                                    Else
                                        sCampos = sCampos & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & " <br>"
                                    End If

                                End If


                            End If
                        Next
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-10'>"
                        sHtmlBanner = sHtmlBanner & " <div class='col-tabla col-xs-4 info-producto'> " & sCampos & "</div>"

                    End If
                    objdatos.fnLog("Carrito", "Antes de precio")



                    ' sHtmlBanner = sHtmlBanner & "</div>"
                    Dim precio As Double = 0
                    Dim precioConDescuento As Double = 0
                    If Partida.Descuento > 0 Then
                        precioConDescuento = Partida.Precio * (1 - (Partida.Descuento / 100))
                    Else
                        precioConDescuento = Partida.Precio
                    End If

                    If Partida.Descuento > 0 Then
                        TotDescuento = TotDescuento + (Partida.Precio - precioConDescuento)
                    End If

                    objdatos.fnLog("Carrito", precioConDescuento)
                    objdatos.fnLog("Carrito partida.precio", Partida.Precio)

                    If Partida.Descuento > 0 Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><span class='precio-con-descuento'>" & ddlMoneda.SelectedValue & " " & precioConDescuento.ToString(" ###,###,###.#0") & "</span><div class='precio-original descuento'>" & Partida.Precio.ToString("$ ###,###,###.#0") & " " & Partida.Moneda & "</div></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-precio'><div class='precio-original'>" & ddlMoneda.SelectedValue & " " & Partida.Precio.ToString(" ###,###,###.#0") & " " & "</div></div>"

                    End If

                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2 info-cantidad'><div  class='precio' id='#txt" & x & "'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-3 info-total'><div class='prec-simul'> " & ddlMoneda.SelectedValue & " " & (Partida.Cantidad * precioConDescuento).ToString("###,###,###.#0") & "</div></div>"
                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart
                    sHtmlBanner = sHtmlBanner & "<div class='col-xs-12 btn-action-cart'>"
                    'PopUp('','Agregado al carrito','Aceptar');
                    If sTallaColor = "SI" Then
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.Generico & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"

                    Else
                        sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart preview-popup' href='preview-popup.aspx?code=" & Partida.ItemCode & "&Action=e&Cant=" & Partida.Cantidad & "&Precio=" & Partida.Precio & "&Lin=" & x & "'>Editar</a></div>"


                    End If
                    '  sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart'  href='PopUp('','Agregado al carrito','Aceptar');'>Editar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-3 no-padding'><a class='action-cart' href='captura-pedidozeyco.aspx?item=" & Partida.ItemCode & "&Action=d'>Quitar</a></div>"
                    sHtmlBanner = sHtmlBanner & "<div class='col-sm-4 no-padding'><a class='action-cart preview-popup' href='elegir-favoritos.aspx?code=" & Partida.ItemCode & "&name=" & Partida.ItemName & "'>Mover a favoritos</a></div>"
                    'sHtmlBanner = sHtmlBanner & "<div class='col-sm-2 no-padding'><a class='action-cart'>Guardar</a></div>"
                    sHtmlBanner = sHtmlBanner & "</div>   </div>"

                    sHtmlBanner = sHtmlBanner & " </div> "
                End If
                objdatos.fnLog("Carrito", "Arma")

            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objdatos.fnLog("Carrito load", ex.Message)
        End Try

        objdatos.fnLog("Carrito", "SubTotales")

        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner '& "</div>"
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlPartidas.Controls.Clear()
        pnlPartidas.Controls.Add(literal)
        lblSubTotal.Text = ddlMoneda.SelectedValue & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = ddlMoneda.SelectedValue & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
            Else
                Descuento = CDbl(lblDescuento.Text.Replace("$ ", "").Replace(ddlMoneda.SelectedValue, ""))
                Descuento = TotDescuento
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try
        lblTotal.Text = ddlMoneda.SelectedValue & " " & (sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0")
        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)


        '  objDatos.fnLog("Carrito", "slpCode y demas")

        If Session("UserB2C") <> "" Then
            pnlGuardarCarrito.Visible = False
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
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
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
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
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
                pnlGuardarCarrito.Visible = False
                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            Else
                '   pnlPayPal.Visible = True
                ' btnProcesar.Visible = True
            End If

            If Session("RazonSocial") <> "" Then
                pnlPayPal.Visible = False
                pnlSeparador.Visible = True
                ''B2B

                ssql = "select cvEstatus  from config.ParametrizacionesDoctos WHERE cvTipo ='B2B' and cvDocto ='OFERTA'"
                Dim dtBotonOferta As New DataTable
                dtBotonOferta = objdatos.fnEjecutarConsulta(ssql)
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
                dtBotonPedido = objdatos.fnEjecutarConsulta(ssql)
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

                '  pnlDireccionEntrega.Visible = True
                ''Cargamos las direcciones del cliente seleccionado
                ssql = objdatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                Dim dtdirecciones As New DataTable
                dtdirecciones = objdatos.fnEjecutarConsultaSAP(ssql)
                ddlDirecciones.DataSource = dtdirecciones
                ddlDirecciones.DataTextField = "Direccion"
                ddlDirecciones.DataValueField = "Direccion"
                ddlDirecciones.DataBind()
            End If
        End If
    End Sub

    Public Function fnPrecioActual(itemCode As String, IdListaPrecios As Int16) As Double
        Dim ssql As String
        Dim Precio As Double = 0

        Dim DecRedondeo As Int16 = 0
        Dim iBandRedondeo As Int16 = 0
        ssql = "SELECT ISNULL(cvAplicaRedondeo,'NO'),ISNULL(ciRedondeo,0) as Digitos FROM config.parametrizaciones"
        Dim dtAplicaRedondeo As New DataTable
        dtAplicaRedondeo = objdatos.fnEjecutarConsulta(ssql)
        If dtAplicaRedondeo.Rows.Count > 0 Then
            If dtAplicaRedondeo.Rows(0)(0) = "SI" Then
                iBandRedondeo = 1
                DecRedondeo = dtAplicaRedondeo.Rows(0)(1)
            End If
        End If


        If iBandRedondeo = 1 Then
            ssql = "SELECT ROUND(ISNULL(price,'0')," & DecRedondeo & ")) FROM ITM1 WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"

        Else
            ssql = "SELECT ISNULL(price,'0') FROM ITM1 WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"
        End If

        ssql = "SELECT ISNULL(price,'0') FROM ITM1 WHERE ItemCode=" & "'" & itemCode & "' AND pricelist=" & "'" & IdListaPrecios & "'"

        Dim dtPrecio As New DataTable
        dtPrecio = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtPrecio.Rows.Count > 0 Then
            Precio = dtPrecio.Rows(0)(0)
        Else
            Precio = 0
        End If

        ''Revisamos si se mostrarán precios más IVA
        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objdatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "SI" Then
                ''Obtenemos el porcentaje de IVA
                ssql = "select ISNULL(cfPorcIva,'0') FROM config.parametrizaciones "
                Dim dtPorcIVA As New DataTable
                dtPorcIVA = objdatos.fnEjecutarConsulta(ssql)
                If dtPorcIVA.Rows.Count > 0 Then
                    Precio = Precio * (1 + dtPorcIVA.Rows(0)(0))

                End If
            End If
        End If
        If iBandRedondeo = 1 Then
            Precio = Math.Round(Precio, DecRedondeo)
        End If

        objdatos.fnLog("Precio de Lista", Precio)

        ''Revisamos si la moneda natural de la lista de precios es diferente a la seleccionada, convertimos en base al tipo de cambio
        ssql = objdatos.fnObtenerQuery("MonedasListaPrecios")
        Dim dtMonedas As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        ssql = ssql.Replace("[%1]", "'" & IdListaPrecios & "'")

        objdatos.fnLog("Moneda Lista", ssql.Replace("'", ""))
        dtMonedas = objdatos.fnEjecutarConsultaSAP(ssql)
        Dim sMoneda As String = ""
        If dtMonedas.Rows.Count > 0 Then
            sMoneda = dtMonedas.Rows(0)(0)
        End If
        objdatos.fnLog("Conversion Moneda", "Moneda de la lista:" & sMoneda)
        objdatos.fnLog("Conversion Moneda", "Moneda del combo:" & ddlMoneda.SelectedValue)
        If sMoneda <> ddlMoneda.SelectedValue Then
            objdatos.fnLog("Conversion Moneda", "Entra al tipo de cambio")
            ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
            Dim dtTc As New DataTable
            dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
            objdatos.fnLog("Conversion Moneda", ssql.Replace("'", ""))
            Dim iTC As Double = 1
            If dtTc.Rows.Count > 0 Then
                iTC = dtTc.Rows(0)(0)
            End If
            objdatos.fnLog("Conversion Moneda", "TC: " & iTC)
            Precio = Precio
        End If

        Return Precio
    End Function

    Public Sub fnCargaUsuario()
        ssql = "SELECT * from config.Usuarios where cvUsuario=" & "'" & lblUsuario.Text & "' "
        Dim dtAcceso As New DataTable
        dtAcceso = objdatos.fnEjecutarConsulta(ssql)
        If dtAcceso.Rows.Count > 0 Then
            Session("UserTienda") = lblUsuario.Text
            Session("slpCode") = dtAcceso.Rows(0)("ciVendedorSAP")
            Session("NombreuserTienda") = dtAcceso.Rows(0)("cvNombreCompleto")
        End If

    End Sub

    Public Sub fnGuardaCarrito(TipoDoc As String)



        If ddlTipoEnvio.Visible = True And ddlTipoEnvio.SelectedValue = "0" Then
            objdatos.Mensaje("Debe seleccionar el tipo de envío", Me.Page)
            Exit Sub
        End If

        If ddlPaqueteria.Visible = True And ddlPaqueteria.SelectedValue = "0" Then
            objdatos.Mensaje("Debe seleccionar la paquetería", Me.Page)
            Exit Sub
        End If
        Dim iContadorPArtidas As Int32 = 0
        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                iContadorPArtidas = iContadorPArtidas + 1
            End If
        Next

        If iContadorPArtidas = 0 Then
            objdatos.Mensaje("Debe cargar artículos al documento. Si eligió un archivo de excel, asegúrese de haber hecho clic en el botón subir", Me.Page)
            Exit Sub
        End If

        ''Obtenemos el tipo de cambio de hoy
        objdatos.fnLog("Cotizacion", "Va a obtener el tipo de cambio")
        ssql = objdatos.fnObtenerQuery("Tipo de Cambio")
        Dim dtTc As New DataTable
        dtTc = objdatos.fnEjecutarConsultaSAP(ssql)
        Dim iTC As Double = 1
        If dtTc.Rows.Count > 0 Then
            iTC = dtTc.Rows(0)(0)
        End If

        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
        Dim dtId As New DataTable
        dtId = objdatos.fnEjecutarConsulta(ssql)

        objdatos.fnLog("Cotizacion", "Antes de IdCarrito")
        Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))

        If Session("UserB2C") <> "" Then
            Session("UserTienda") = Session("UserB2C")


        End If

        If Session("ArchivoLargo") = "SI" Then
            ''Cuando es archivo largo, insertamos previamente en la carga del archivo
            ssql = "UPDATE Tienda.Pedido_HDR  SET cvTipoDoc=" & "'" & TipoDoc & "', cvComentarios='" & txtOtros.Text & "',cvTipoEnvio='" & ddlTipoEnvio.SelectedValue & "',cvPaqueteria='" & ddlPaqueteria.SelectedValue & "',cvUsoCFDI='" & ddlUsoCFDI.SelectedValue & "',cvOC='" & txtOC_.Text & "' WHERE ciNoPedido=" & "'" & Session("IdCarritoGrande") & "'"
            objdatos.fnEjecutarInsert(ssql)
        Else
            ssql = "INSERT INTO Tienda.Pedido_HDR ( ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus) VALUES(" _
                       & "'" & dtId.Rows(0)(0) & "'," _
                       & "'" & dtId.Rows(0)(0) & "'," _
                       & "'" & Session("UserTienda") & "'," _
                       & "'" & Session("NombreuserTienda") & "'," _
                       & "'" & Session("SlpCode") & "'," _
                       & "'" & Session("Cliente") & "'," _
                       & "'" & Session("RazonSocial") & "',GETDATE(),''," _
                       & "'" & Session("ListaPrecios") & "'," _
                       & "'" & TipoDoc & "','ABIERTO')"
            objdatos.fnEjecutarInsert(ssql)
            objdatos.fnLog("Cotizacion", "Insertó Hdr en Carrito")
            ''Ahora las lineas
            Dim iTotal As Double = 0
            Dim ssqlLineas As String = ""
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    'ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_det"
                    'Dim dtIdLineas As New DataTable
                    'dtIdLineas = objdatos.fnEjecutarConsulta(ssql)
                    '  objdatos.fnLog("Cotizacion-lineas", Partida.ItemCode)
                    ssqlLineas = ssqlLineas & " INSERT INTO Tienda.Pedido_det (ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                      & "'" & dtId.Rows(0)(0) & "'," _
                      & "'" & Session("UserTienda") & "'," _
                      & "'" & Partida.ItemCode & "'," _
                      & "'" & Partida.ItemName & "'," _
                      & "'" & Partida.Cantidad & "'," _
                      & "'" & Partida.Precio.ToString.Replace(",", ".") & "'," _
                      & "'" & Partida.Descuento.ToString.Replace(",", ".") & "') " & vbCrLf

                    ' objdatos.fnLog("Cotizacion", ssql.Replace("'", ""))

                    iTotal = iTotal + (Partida.Precio * Partida.Cantidad)
                End If
            Next
            objdatos.fnEjecutarInsert(ssqlLineas)
            objdatos.fnLog("Cotizacion", "Insertó las lineas")
            ssql = "UPDATE Tienda.Pedido_HDR  SET cfTotal=" & "'" & iTotal.ToString.Replace(",", ".") & "',cfTipoCambio=" & "'" & iTC.ToString.Replace(",", ".") & "',cfTotalFC=" & "'" & (iTotal * iTC).ToString.Replace(",", ".") & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
            objdatos.fnEjecutarInsert(ssql)
            If (objdatos.fnObtenerCliente().ToUpper.Contains("SUJEA") Or objdatos.fnObtenerCliente().ToUpper.Contains("MANIJ")) Then
                ''Lo sacamos por motor
                ssql = "UPDATE Tienda.Pedido_HDR  SET cvMotor='SI' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
                objdatos.fnEjecutarInsert(ssql)

                ssql = "UPDATE Tienda.Pedido_HDR  SET cvComentarios='" & txtOtros.Text & "',cvTipoEnvio='" & ddlTipoEnvio.SelectedValue & "',cvPaqueteria='" & ddlPaqueteria.SelectedValue & "',cvUsoCFDI='" & ddlUsoCFDI.SelectedValue & "',cvOC='" & txtOC_.Text & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
                objdatos.fnEjecutarInsert(ssql)


                'cvTipoEnvio,cvPaqueteria,cvUsoCFDI,cvOC

            End If
            objdatos.fnLog("Cotizacion", "Terminó en tablas")

        End If





        lblMensaje.Text = "Carrito guardado"
        ' objDatos.Mensaje("Compra procesada", Me.Page)
        If TipoDoc <> "CARRITO" Then
            ssql = "SELECT ISNULL(cvUsaMotor,'NO') FROM config.parametrizaciones "
            Dim dtUsaMotor As New DataTable
            dtUsaMotor = objdatos.fnEjecutarConsulta(ssql)
            If dtUsaMotor.Rows.Count > 0 Then
                If dtUsaMotor.Rows(0)(0) = "NO" Then
                    objdatos.fnLog("Cotizacion", "Va a procesar SAP")
                    If (objdatos.fnObtenerCliente().ToUpper.Contains("SUJEA") Or objdatos.fnObtenerCliente().ToUpper.Contains("MANIJ")) Then '' And 1 = 2 Forzamos que de momento no entre
                        ''Lo mandamos a motor
                        objdatos.Mensaje(TipoDoc & " se ha procesado correctamente", Me.Page)
                        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                        Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                        Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
                    Else
                        fnProcesarSAP(iIdCarrito, TipoDoc)
                    End If

                Else
                    objdatos.Mensaje(TipoDoc & " se ha procesado correctamente", Me.Page)
                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                    Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                    Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)
                End If
            Else
                objdatos.fnLog("Cotizacion", "Va a procesar SAP")
                fnProcesarSAP(iIdCarrito, TipoDoc)
            End If

        End If

        btnCotizar.Enabled = False
        btnPedido.Enabled = False
        btnGuardar.Enabled = False
        btnImprimir.Visible = True
    End Sub

    Public Sub fnInsertaArchivoGrande(dt As DataTable)
        ssql = "select ISNULL(MAX(ciIdRelacion),0) + 1 FROM  Tienda.Pedido_hdr"
        Dim dtId As New DataTable
        dtId = objdatos.fnEjecutarConsulta(ssql)

        objdatos.fnLog("Cotizacion", "Antes de IdCarrito")
        Dim iIdCarrito As Int64 = CInt(dtId.Rows(0)(0))

        If Session("UserB2C") <> "" Then
            Session("UserTienda") = Session("UserB2C")
        End If
        Session("IdCarritoGrande") = dtId.Rows(0)(0)

        ssql = "INSERT INTO Tienda.Pedido_HDR ( ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus) VALUES(" _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & dtId.Rows(0)(0) & "'," _
            & "'" & Session("UserTienda") & "'," _
            & "'" & Session("NombreuserTienda") & "'," _
            & "'" & Session("SlpCode") & "'," _
            & "'" & Session("Cliente") & "'," _
            & "'" & Session("RazonSocial") & "',GETDATE(),''," _
            & "'" & Session("ListaPrecios") & "'," _
            & "'','ABIERTO')"
        objdatos.fnEjecutarInsert(ssql)
        objdatos.fnLog("Cotizacion", "Insertó Hdr en Carrito")
        ''Ahora las lineas
        Dim iTotal As Double = 0
        Dim ssqlLineas As String = ""
        For i = 0 To dt.Rows.Count - 1 Step 1

            Dim dtItemName As New DataTable
            ssql = objdatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & dt.Rows(i)(0) & "'")

            dtItemName = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtItemName.Rows.Count > 0 Then

                Dim precio As Double = 0
                Dim descuento As Double = 0
                Dim Articulo As String = dtId.Rows(0)(0)
                If CInt(HttpContext.Current.Session("slpCode")) <> 0 Then
                    If HttpContext.Current.Session("ListaPrecios") Is Nothing Then
                        precio = objdatos.fnPrecioActual(Articulo)
                    Else
                        precio = objdatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))
                    End If

                Else
                    precio = objdatos.fnPrecioActual(Articulo)

                End If
                If HttpContext.Current.Session("Cliente") <> "" Then

                    descuento = objdatos.fnDescuentoEspecial(Articulo, HttpContext.Current.Session("Cliente"))
                End If

                ssqlLineas = ssqlLineas & " INSERT INTO Tienda.Pedido_det (ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento) VALUES(" _
                      & "'" & dtId.Rows(0)(0) & "'," _
                      & "'" & Session("UserTienda") & "'," _
                      & "'" & dt.Rows(i)(0) & "'," _
                      & "'" & "" & "'," _
                      & "'" & dt.Rows(i)(1) & "'," _
                      & "'" & precio & "'," _
                      & "'" & descuento & "')" & vbCrLf

                ' objdatos.fnLog("Cotizacion", ssql.Replace("'", ""))

                iTotal = iTotal + (precio * dt.Rows(i)(1))
            End If


        Next
        objdatos.fnEjecutarInsert(ssqlLineas)

        Dim iTC As Double = 1
        ssql = "UPDATE Tienda.Pedido_HDR  SET cfTotal=" & "'" & iTotal.ToString.Replace(",", ".") & "',cfTipoCambio=" & "'" & 1 & "',cfTotalFC=" & "'" & (iTotal * iTC).ToString.Replace(",", ".") & "' WHERE ciNoPedido=" & "'" & dtId.Rows(0)(0) & "'"
        objdatos.fnEjecutarInsert(ssql)

        objdatos.fnLog("Cotizacion", "Insertó las lineas")
    End Sub
    Public Function fnAsignaAlmacen(itemCode As String)
        Dim almacen As String = ""

        ssql = "select cvWhsCode  from config.Existencias where cvEstatus ='ACTIVO' order by ciOrden "
        Dim dtAlmacenes As New DataTable
        dtAlmacenes = objdatos.fnEjecutarConsulta(ssql)

        ''El primer almacen

        If dtAlmacenes.Rows.Count > 0 Then
            ssql = "SELECT OnHand FROM OITW WHERE itemCode=" & "'" & itemCode & "' ANd whsCode=" & "'" & dtAlmacenes.Rows(0)(0) & "'"
            Dim dtExistencias As New DataTable
            dtExistencias = objdatos.fnEjecutarConsultaSAP(ssql)
            objdatos.fnLog("Asigna almacen", ssql.Replace("'", ""))
            If dtExistencias.Rows.Count > 0 Then
                If CDbl(dtExistencias.Rows(0)(0)) = 0 Then
                    ''Tomamos el segundo
                    almacen = dtAlmacenes.Rows(1)(0)
                Else
                    almacen = dtAlmacenes.Rows(0)(0)
                End If
            End If

        End If

        Return almacen
    End Function
    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub

    Public Function fnProcesarSAP(idCarrito As Int64, TipoDoc As String)


        ssql = "select ISNULL(cvPreciosMasIVA,'NO') from config.parametrizaciones"
        Dim dtPreciosIVA As New DataTable
        dtPreciosIVA = objdatos.fnEjecutarConsulta(ssql)
        Dim sPreciosMasIVA As String = "NO"
        If dtPreciosIVA.Rows.Count > 0 Then
            sPreciosMasIVA = dtPreciosIVA.Rows(0)(0)
        End If

        ssql = "SELECT ciIdRelacion,ciNoPedido,cvUsuario,cvAgente,ciIdAgenteSAP,cvCveCliente,cvCliente,cdFecha,cvComentarios,cvListaPrecios,cvTipoDoc,cvEstatus FROM Tienda.Pedido_HDR WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtEncabezado As New DataTable
        dtEncabezado = objdatos.fnEjecutarConsulta(ssql)

        ssql = "SELECT  ciIdRelacion,ciNoPedido,cvAgente,cvItemCode,cvItemName,cfCantidad,cfPrecio,cfDescuento FROM Tienda.Pedido_det WHERE ciNoPedido=" & "'" & idCarrito & "'"
        Dim dtPartidas As New DataTable
        dtPartidas = objdatos.fnEjecutarConsulta(ssql)
        Dim sArticulosSinStock As String = ""
        Dim oDoctoVentas As SAPbobsCOM.Documents
        Dim oCompany As New SAPbobsCOM.Company
        Dim sCardCode As String = ""
        Try
            oCompany = objdatos.fnConexion_SAP
            If oCompany.Connected Then

                Dim sPersonaContacto As String = ""
                'If txtPersonaContacto.Text <> "" Then
                '    Try
                '        Dim oProspecto As SAPbobsCOM.BusinessPartners

                '        oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                '        oProspecto.GetByKey(dtEncabezado.Rows(0)("cvCveCliente"))
                '        oProspecto.ContactEmployees.Add()
                '        oProspecto.ContactEmployees.FirstName = txtPersonaContacto.Text
                '        oProspecto.ContactEmployees.Name = txtPersonaContacto.Text
                '        oProspecto.ContactEmployees.Position = ""
                '        oProspecto.ContactEmployees.E_Mail = ""
                '        oProspecto.ContactEmployees.Phone1 = ""
                '        oProspecto.ContactEmployees.Address = " "
                '        oProspecto.ContactEmployees.Active = SAPbobsCOM.BoYesNoEnum.tYES

                '        If oProspecto.Update() <> 0 Then
                '            objdatos.fnLog("Actualizar contacto:", oCompany.GetLastErrorDescription)
                '        Else
                '            ''Todo bien
                '            ssql = "SELECT MAX(CntctCode)  from OCPR where cardCode='" & dtEncabezado.Rows(0)("cvCveCliente") & "'"
                '            Dim dtIdContacto As New DataTable
                '            dtIdContacto = objdatos.fnEjecutarConsultaSAP(ssql)
                '            If dtIdContacto.Rows.Count > 0 Then
                '                sPersonaContacto = dtIdContacto.Rows(0)(0)
                '            End If
                '        End If


                '    Catch ex As Exception

                '    End Try
                'End If

                If TipoDoc = "COTIZACION" Then
                    objdatos.fnLog("Cotizacion", "Crea objeto Company")
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations)
                End If
                If TipoDoc = "PEDIDO" Then
                    oDoctoVentas = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)

                    ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                    Dim dtcliente As New DataTable
                    dtcliente = objdatos.fnEjecutarConsulta(ssql)
                    If dtcliente.Rows.Count > 0 Then
                        If dtcliente.Rows(0)(0) = "Bacán" Then
                            oDoctoVentas.DocDueDate = Now.Date.AddDays(1)

                        End If
                    End If

                    ' oDoctoVentas.Series = 59
                End If
                Try
                    oDoctoVentas.SalesPersonCode = Session("slpCode")
                Catch ex As Exception

                End Try

                If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then

                Else
                    ssql = "select ISNULL(ciSerieDoc,'') FROM config.ParametrizacionesDoctos where cvDocto ='PEDIDO' and cvTipo='Vendedores'"

                    objdatos.fnLog("pedido", "Query serie: " & ssql.Replace("'", ""))
                    Dim dtSerie As New DataTable
                    dtSerie = objdatos.fnEjecutarConsulta(ssql)
                    If dtSerie.Rows.Count > 0 Then
                        If CStr(dtSerie.Rows(0)(0)) <> "" Then
                            oDoctoVentas.Series = dtSerie.Rows(0)(0)
                        End If
                    End If
                End If


                'If ddlPersonaContacto.Items.Count > 0 Then
                '    oDoctoVentas.ContactPersonCode = ddlPersonaContacto.SelectedValue
                'End If

                If sPersonaContacto <> "" Then
                    oDoctoVentas.ContactPersonCode = sPersonaContacto
                End If

                'dtEncabezado.Rows(0)("ciIdAgenteSAP")
                oDoctoVentas.CardCode = dtEncabezado.Rows(0)("cvCveCliente")

                Dim sFechaCot As String()
                Dim sFechaVence As String()
                objdatos.fnLog("Fechas", txtFechaVence.Text)
                'sFechaCot = txtFechaCot.Text.Split("/")
                'sFechaVence = txtFechaVence.Text.Split("/")


                oDoctoVentas.DocDate = CDate(txtFechaCot.Text)
                oDoctoVentas.DocDueDate = CDate(txtFechaVence.Text)
                oDoctoVentas.DocCurrency = ddlMoneda.SelectedValue

                Dim sComentarios As String = ""

                If pnlComentarios.Visible = True Then
                    sComentarios = txtComentarios.Text
                End If

                oDoctoVentas.Comments = "Desde Internet:" & sComentarios

                sCardCode = dtEncabezado.Rows(0)("cvCveCliente")
                Try
                    ''Obtener RFC

                    oDoctoVentas.UserFields.Fields.Item("U_FacNit").Value = "7268540-9"
                    oDoctoVentas.UserFields.Fields.Item("U_Vendedor").Value = "Vendedor"
                    oDoctoVentas.UserFields.Fields.Item("U_FacNom").Value = dtEncabezado.Rows(0)("cvCliente")
                Catch ex As Exception

                End Try
                Try
                    oDoctoVentas.UserFields.Fields.Item("U_BXP_TIME_ENTREGA").Value = txtTiempoEntrega.Text
                Catch ex As Exception

                End Try

                If ddlDirecciones.Items.Count > 0 Then
                    oDoctoVentas.ShipToCode = ddlDirecciones.SelectedValue
                End If

                Dim iLinea As Int16 = 0
                Dim sArticulosPedido As String = ""
                For i = 0 To dtPartidas.Rows.Count - 1 Step 1
                    oDoctoVentas.Lines.Add()
                    oDoctoVentas.Lines.SetCurrentLine(iLinea)
                    If fnRevisaExistencias(dtPartidas.Rows(i)("cvItemCode")) = 0 Then
                        sArticulosSinStock = sArticulosSinStock & dtPartidas.Rows(i)("cvItemCode") & vbCrLf
                    End If
                    oDoctoVentas.Lines.ItemCode = dtPartidas.Rows(i)("cvItemCode")
                    oDoctoVentas.Lines.ItemDescription = dtPartidas.Rows(i)("cvItemName")
                    oDoctoVentas.Lines.Quantity = dtPartidas.Rows(i)("cfCantidad")
                    '    oDoctoVentas.Lines.PriceAfterVAT = dtPartidas.Rows(i)("cfPrecio")
                    '   oDoctoVentas.Lines.Currency = ddlMoneda.SelectedValue









                    'If sPreciosMasIVA = "SI" Then
                    '    oDoctoVentas.Lines.PriceAfterVAT = dtPartidas.Rows(i)("cfPrecio")
                    'Else
                    '    oDoctoVentas.Lines.UnitPrice = dtPartidas.Rows(i)("cfPrecio")
                    'End If

                    ''Obtener Indicador de IVA
                    For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                        If Partida.ItemCode <> "BORRAR" Then
                            If Partida.ItemCode = dtPartidas.Rows(i)("cvItemCode") Then
                                If Partida.Impuesto = "SI" Then
                                    ssql = objdatos.fnObtenerQuery("ObtenerIVA")
                                Else
                                    ssql = objdatos.fnObtenerQuery("ObtenerIVA0")
                                End If
                                Exit For

                            End If
                        End If
                    Next


                    If ssql <> "" Then
                        ssql = ssql.Replace("[%0]", dtPartidas.Rows(i)("cvItemCode"))
                        Dim dtIVA As New DataTable
                        dtIVA = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtIVA.Rows.Count > 0 Then
                            oDoctoVentas.Lines.TaxCode = dtIVA.Rows(0)(0)
                        End If

                    End If

                    If CDbl(dtPartidas.Rows(i)("cfDescuento")) > 0 Then
                        oDoctoVentas.Lines.DiscountPercent = dtPartidas.Rows(i)("cfDescuento")
                    End If

                    Dim sAlmacen As String = ""
                    sAlmacen = fnAsignaAlmacen(dtPartidas.Rows(i)("cvItemCode"))
                    objdatos.fnLog("Asigna almacen", sAlmacen)
                    If sAlmacen <> "" Then
                        oDoctoVentas.Lines.WarehouseCode = sAlmacen
                    End If
                    sArticulosPedido = sArticulosPedido & String.Format("{0,200} {1,25}{2}{2}",
                                dtPartidas.Rows(i)("cvItemName"), dtPartidas.Rows(i)("cfCantidad"), vbCrLf)
                    iLinea = iLinea + 1
                Next

                Dim fEnvio As Double = 0
                ''agregamos el envio
                Try
                    fEnvio = txtEnvio.Text.Replace(",", "").Replace("$", "")

                    If fEnvio > 0 Then

                        oDoctoVentas.Lines.Add()
                        oDoctoVentas.Lines.SetCurrentLine(iLinea)
                        oDoctoVentas.Lines.PriceAfterVAT = fEnvio
                        oDoctoVentas.Lines.ItemCode = "Envio"
                    End If


                Catch ex As Exception

                End Try


                objdatos.fnLog("Cotizacion", "antes del Add")
                If oDoctoVentas.Add <> 0 Then
                    ''Ha ocurrido un error, regresamos el mensaje
                    objdatos.fnLog("Cotizacion", "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    '  objDatos.Mensaje("ERROR-" & oCompany.GetLastErrorDescription, Me.Page)
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "ERROR-" & oCompany.GetLastErrorDescription.Replace("'", ""))
                    btnImprimir.Visible = False
                    btnNuevo.Visible = False
                Else
                    ''Todo bien
                    btnImprimir.Visible = True
                    btnNuevo.Visible = True
                    Dim sDocnum As String = ""
                    Dim dtDoc As New DataTable
                    Session("DocEntry") = oCompany.GetNewObjectKey
                    If TipoDoc = "PEDIDO" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumOrdenVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)

                    End If
                    If TipoDoc = "COTIZACION" Then
                        ssql = objdatos.fnObtenerQuery("ObtenerDocNumOfertaVentas")
                        ssql = ssql.Replace("[%0]", oCompany.GetNewObjectKey)
                        dtDoc = objdatos.fnEjecutarConsultaSAP(ssql)
                    End If
                    If dtDoc.Rows.Count > 0 Then
                        sDocnum = dtDoc.Rows(0)(0)
                        ssql = "UPDATE Tienda.Pedido_HDR  SET ciProcesadoSAP=1, cvNumSAP=" & "'" & dtDoc.Rows(0)(0) & "' WHERE ciNoPedido=  " & "'" & idCarrito & "'"
                        objdatos.fnEjecutarInsert(ssql)

                    End If
                    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, TipoDoc & " procesado correctamente ")
                    objdatos.Mensaje(TipoDoc & " procesado correctamente ", Me.Page)
                    Try
                        fnCargaCarritoPOST()


                        If sArticulosSinStock <> "" Then
                            ''Tenemos articulos sin stock...notificamos
                            ''Obtenemos correo de notificacion
                            ssql = "select ISNULL(cvNotificaExistencia,'NO'),ISNULL(cvCorreoNotificaExistencia,'') FROM config.parametrizaciones"
                            Dim dtNotifica As New DataTable
                            dtNotifica = objdatos.fnEjecutarConsulta(ssql)
                            If dtNotifica.Rows.Count > 0 Then
                                If dtNotifica.Rows(0)(0) <> "NO" Then
                                    Dim sBody As String = ""
                                    sBody = "Se ha generado un " & TipoDoc & " desde el portal de Ecommerce: " & sDocnum & vbCrLf
                                    sBody = sBody & "Sin embargo, los siguientes artículos no tienen stock:" & vbCrLf
                                    sBody = sBody & sArticulosSinStock
                                    If dtNotifica.Rows(0)(1) <> "" Then
                                        objdatos.fnEnviarCorreo(dtNotifica.Rows(0)(1), sBody, "Notificaciones ECommerce: Artículos sin stock")
                                    End If

                                End If
                            End If
                        End If

                        ''Enviamos correo
                        Dim text As String = MensajeHTML(Server.MapPath("~") & "\correo_A_B2B.html")
                        Dim sDestinatario As String = ""
                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente As New DataTable
                        dtcliente = objdatos.fnEjecutarConsulta(ssql)

                        objdatos.fnLog("Confirmacion-cliente", dtcliente.Rows(0)(0))

                        text = text.Replace("{nombrecliente}", dtcliente.Rows(0)(0))
                        text = text.Replace("{enviara}", "")
                        text = text.Replace("{direccionenvio}", "" & "</br> " & Session("Comentarios"))
                        text = text.Replace("{metodoenvio}", "")
                        text = text.Replace("{numpedido}", dtDoc.Rows(0)(0))
                        text = text.Replace("{fechapedido}", Now.Date.ToShortDateString)
                        ''Ahora las líneas
                        text = text.Replace("{lineas}", fnGeneraHTMLPartidas(dtPartidas))
                        objdatos.fnLog("Confirmacion", "Antes de enviar correo")


                        Dim sAsuntoB2B As String = ""

                        If pnlComentarios.Visible = True Then

                            sAsuntoB2B = sDocnum & " " & sCardCode & " OFERTA DE VENTAS "
                            ''Determinamos si hay un PDF a generar, para anexarlo a un posible correo
                            If System.IO.File.Exists(Server.MapPath("~") & "\PedidoCot.rpt") And Session("RazonSocial") <> "" Then
                                ''Hay crystal report, generamos el PDF para guardar
                                Try
                                    fnGeneraPDF_Correo(Session("DocEntry"))
                                Catch ex As Exception
                                    objdatos.fnLog("Genera PDF Correo EX", ex.Message.Replace("'", ""))
                                End Try



                            End If


                        Else
                            sAsuntoB2B = sDocnum & "- Pedido desde B2B"
                        End If


                        ''Revisamos si notificamos a alguien de la empresa
                        ssql = "select ISNULL(cvCorreoNotifica,'') FROM config.parametrizaciones_b2c "
                        Dim dtCorreoInterno As New DataTable
                        dtCorreoInterno = objdatos.fnEjecutarConsulta(ssql)
                        If dtCorreoInterno.Rows.Count > 0 Then
                            If dtCorreoInterno.Rows(0)(0) <> "" Then
                                objdatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, Session("PDF_Correo"), sAsuntoB2B)

                                ' objdatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, sAsuntoB2B)

                            End If
                        End If






                        ''Revisamos si hay que mandar el correo al cliente
                        If Session("slpCode") <> "0" Or Session("Cliente") <> "" Then
                            ssql = "select ISNULL(cvEnviaCorreoCliente,'NO') FROM config.parametrizaciones"
                            Dim dtEnviaCorreoCliente As New DataTable
                            dtEnviaCorreoCliente = objdatos.fnEjecutarConsulta(ssql)
                            If dtEnviaCorreoCliente.Rows.Count > 0 Then
                                If dtEnviaCorreoCliente.Rows(0)(0) = "SI" Then
                                    ssql = ""
                                    ssql = objdatos.fnObtenerQuery("Correocliente")
                                    If ssql <> "" Then
                                        ssql = ssql.Replace("[%0]", sCardCode)
                                        Dim dtCorreo As New DataTable
                                        dtCorreo = objdatos.fnEjecutarConsultaSAP(ssql)
                                        If dtCorreo.Rows.Count > 0 Then
                                            If dtCorreo.Rows(0)(0) <> "" Then
                                                objdatos.fnEnviarCorreo(dtCorreoInterno.Rows(0)(0), text, Session("PDF_Correo"), sAsuntoB2B)
                                                ' objdatos.fnEnviarCorreo(dtCorreo.Rows(0)(0), text, sAsuntoB2B)

                                            End If
                                        End If
                                    End If

                                End If
                            End If
                        End If
                        btnGuardarPlantilla.Enabled = False
                        pnlPlantilla.Enabled = False
                        pnlImprimir.Visible = True
                        btnImprimir.Visible = True

                        'ssql = "select ISNULL(cvImprimeDocumento,'NO') FROM config.parametrizaciones"
                        'Dim dtImprime As New DataTable
                        'dtImprime = objdatos.fnEjecutarConsulta(ssql)
                        'If dtImprime.Rows.Count > 0 Then
                        '    If dtImprime.Rows(0)(0) = "SI" Then
                        '        pnlImprimir.Visible = True
                        '        btnImprimir.Visible = True
                        '    End If
                        'End If
                    Catch ex As Exception

                    End Try


                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                    Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                    Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

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
                objdatos.Mensaje("No se ha podido establecer conexión - reintente por favor", Me.Page)
            End If

        Catch ex As Exception
            objdatos.Mensaje("No se ha podido establecer conexión - reintente por favor:" & ex.Message & "", Me.Page)
        End Try

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
        dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then

            If objdatos.fnObtenerCliente.ToUpper.Contains("HAWK") Then
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

        If objdatos.fnObtenerCliente.ToUpper.Contains("HAWK") Then
            reporte.SetParameterValue("ObjectId@", iTipoDocumento)
            reporte.SetParameterValue("DocKey@", DocEntry)
        Else
            reporte.SetParameterValue("DocKey@", DocEntry)
        End If

        Dim sArchivo As String = ""
        sArchivo = Server.MapPath("~") & "\PDF\PED-" & DocEntry & ".pdf"

        Session("PDF_Correo") = sArchivo

        objdatos.fnLog("GeneraPDF_Correo", sArchivo)
        Try
            reporte.ExportToDisk(ExportFormatType.PortableDocFormat, sArchivo)
            reporte.Dispose()
        Catch ex As Exception
            objdatos.fnLog("GeneraPDF_Correo_EX", ex.Message.Replace("'", ""))
            Session("PDF_Correo") = ""
        End Try

    End Sub
    Public Sub fndescargaPDF(DocEntry As Int32)
        Dim reporte As New ReportDocument
        Dim crtableLogoninfos As New TableLogOnInfos
        Dim crtableLogoninfo As New TableLogOnInfo
        Dim crConnectionInfo As New ConnectionInfo
        Dim CrTables As Tables

        objdatos.fnLog("Al imprimir", "Antes RPT")
        Dim sParametro As String = "@DocKey"
        If pnlComentarios.Visible = True Then
            reporte.Load(Server.MapPath("~") & "\PedidoCot.rpt")
            sParametro = "DocKey@"
        Else
            reporte.Load(Server.MapPath("~") & "\Pedido.rpt")
        End If


        Dim ssql As String
        ssql = "select cvServidorSQL,cvServidorLicencias,cvUserSAP,cvPwdSAP,cvUserSQL,cvPwdSQL,cvBD,cvVersionSQL,cvVersionSAP from config.conexionSAP "
        Dim dtConfSAP As New DataTable
        dtConfSAP = objdatos.fnEjecutarConsulta(ssql)
        If dtConfSAP.Rows.Count > 0 Then
            reporte.SetParameterValue(sParametro, Session("DocEntry"))
            reporte.SetDatabaseLogon(dtConfSAP.Rows(0)("cvUserSQL"), dtConfSAP.Rows(0)("cvPwdSQL"), dtConfSAP.Rows(0)("cvServidorSQL"), dtConfSAP.Rows(0)("cvBD"))

            crConnectionInfo.ServerName = dtConfSAP.Rows(0)("cvServidorSQL")
            crConnectionInfo.DatabaseName = dtConfSAP.Rows(0)("cvBD")
            crConnectionInfo.UserID = dtConfSAP.Rows(0)("cvUserSQL")
            crConnectionInfo.Password = dtConfSAP.Rows(0)("cvPwdSQL")


        End If



        objdatos.fnLog("Al imprimir", "Sale de asignarle la BD")
        CrTables = reporte.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        objdatos.fnLog("Al imprimir", "LogInfo")
        ' reporte.Refresh()

        objdatos.fnLog("Al imprimir", "DocEntry:" & DocEntry)
        reporte.SetParameterValue(sParametro, DocEntry)
        reporte.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, True, "PED-" & DocEntry)
        reporte.Dispose()

        'Response.Flush()
        'Response.End()
        'Response.Clear()
    End Sub

    Public Function fnRevisaExistencias(itemCode As String) As Double
        Dim existencia As Double = 0
        ''Existencia 
        ssql = objdatos.fnObtenerQuery("ExistenciaSAP")
        Dim dtExistencia As New DataTable
        ssql = ssql.Replace("[%0]", "'" & itemCode & "'")
        dtExistencia = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtExistencia.Rows.Count > 0 Then
            existencia = CDbl(dtExistencia.Rows(0)(0))
        End If
        Return existencia
    End Function
    Public Function fnGeneraHTMLPartidas(dtPArtidas As DataTable) As String
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""
        Dim sLigaSitio As String = ""

        ssql = "SELECT ISNULL(cvLigaPublica,'') FROM config.Parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)

        If dtLiga.Rows.Count > 0 Then
            sLigaSitio = dtLiga.Rows(0)(0)
        End If
        'sLigaSitio 
        ssql = "select ISNULL(T2.cvCampoSAP,'') as Campo ,T2.cvDescripcion as Descripcion,T2.cvTipo as Tipo,ISNULL(T1.cvResaltado,'NO') as Resaltado  from config.Plantillas T0 INNER JOIN config.Plantillas_Detalle T1 ON T0.ciIdPlantilla = T1.ciIdPlantilla  " _
                    & " INNER JOIN config .Atributos T2 ON T1.ciIdAtributo = T2.ciIdAtributo " _
                    & " WHERE T1.cvEstatus = 'ACTIVO' and T0.cvDescripcion ='Carrito-Partidas' order by T1.ciOrden "
        Dim dtCamposPlantilla As New DataTable
        dtCamposPlantilla = objdatos.fnEjecutarConsulta(ssql)
        Dim sImagen As String = "ImagenPal"
        Dim sSubTotal As Double = 0
        Try
            For x = 0 To dtPArtidas.Rows.Count - 1 Step 1
                objdatos.fnLog("Correo B2B", dtPArtidas.Rows.Count)
                sHtmlBanner = sHtmlBanner & " <tr>"
                '  sHtmlBanner = sHtmlBanner & " <div class='body-tabla col-xs-12 no-padding'> "
                If dtCamposPlantilla.Rows.Count > 0 Then
                    Dim sCampos As String = ""
                    For i = 0 To dtCamposPlantilla.Rows.Count - 1 Step 1

                        ssql = objdatos.fnObtenerQuery("Info-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtPArtidas.Rows(x)("cvItemCode") & "'")
                        Dim dtGeneral As New DataTable
                        dtGeneral = objdatos.fnEjecutarConsultaSAP(ssql)

                        If dtCamposPlantilla.Rows(i)("Tipo") = "Imagen" Then
                            sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;vertical-align: middle;text-align:center;'> "
                            sHtmlBanner = sHtmlBanner & "   <img src='" & sLigaSitio & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "' alt='logo' style='max-height:100px;'>"
                            sHtmlBanner = sHtmlBanner & "</td>"
                        Else
                            If dtCamposPlantilla.Rows(i)("Tipo") <> "Precio" Then

                                If sCampos = "" Then
                                    ''Si es el primer valor que va a enlazar, lo ponemos en strong
                                    sCampos = sCampos & "<strong style='font-size: 13px;color:#000000;'>" & dtGeneral.Rows(0)(dtCamposPlantilla.Rows(i)("Campo")) & "</strong> <br>"
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
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & CDbl(dtPArtidas.Rows(x)("cfPrecio")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & CDbl(dtPArtidas.Rows(x)("cfCantidad")).ToString(" ###,###,###.#0") & "</td>"
                sHtmlBanner = sHtmlBanner & " <td valign='top' class='diagTextContent' style='padding-top:9px; padding-right: 18px; border-bottom:1px solid #dddddd; padding-bottom: 9px; padding-left: 5px;text-align:center;color:#000000;font-weight:600;font-size:13px;'>" & Session("Moneda") & " " & (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio"))).ToString(" ###,###,###.#0") & "</td>"

                sHtmlBanner = sHtmlBanner & "</tr>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Precio.ToString("$ ###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & Partida.Cantidad.ToString("###,###,###.#0") & "</div>"
                'sHtmlBanner = sHtmlBanner & "<div class='col-tabla col-xs-2'>" & (Partida.Cantidad * Partida.Precio).ToString("###,###,###.#0") & "</div>"
                sSubTotal = sSubTotal + (CDbl(dtPArtidas.Rows(x)("cfCantidad")) * CDbl(dtPArtidas.Rows(x)("cfPrecio")))  'dtPartidas.Rows(i)("cfPrecio")
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
            objdatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function
    Public Function fnExisteArticulo(Articulo As String)
        Dim iExiste As Int16 = 1

        ssql = "SELECT * from OITM where itemCode=" & "'" & Articulo & "'"
        ssql = objdatos.fnObtenerQuery("Nombre-Producto")
        ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
        Dim dtArticulo As New DataTable
        dtArticulo = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtArticulo.Rows.Count = 0 Then
            iExiste = 0
        End If


        Return iExiste
    End Function

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            Dim Articulo As String = ""

            If txtSearch.Text.Contains("|") Then
                Dim sSeleccionado As String() = txtSearch.Text.Split("|")
                Articulo = sSeleccionado(0)
            Else
                Articulo = txtSearch.Text
            End If

            ssql = "SELECT * FROM OITM Where itemCode=" & "'" & Articulo & "'"
            ssql = objdatos.fnObtenerQuery("Nombre-Producto")
            ssql = ssql.Replace("[%0]", "'" & Articulo & "'")
            Dim dtValida As New DataTable
            dtValida = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtValida.Rows.Count = 0 Then
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El artículo ingresado no existe, asegúrese de seleccionar el artículo de la lista")
                Exit Sub

            End If

            Dim dPrecioActual As Double = 0

            dPrecioActual = objdatos.fnPrecioActual(Articulo, HttpContext.Current.Session("ListaPrecios"))


            'HttpContext.Current.Session("ListaPrecios") = "2"
            dPrecioActual = objdatos.fnPrecioActual(Articulo, Convert.ToInt16(HttpContext.Current.Session("ListaPrecios")))

            Dim IVA As Double = 0
            IVA = fnTasaIVA(Articulo)
            IVA = 0
            dPrecioActual = dPrecioActual * (1 + IVA)

            txtPrecioArt.Text = dPrecioActual
            If Session("RazonSocial") <> "" And CInt(Session("slpCode")) = 0 Then
                txtDesc1.Visible = False
                lbldesc1.Visible = False
            Else

                ssql = "select onhand from OITW where itemcode='" & Articulo & "' and whscode ='ALCONGRE'"
                ssql = objdatos.fnObtenerQuery("ExistenciaSAP")
                Dim dtExistencia As New DataTable
                dtExistencia = objdatos.fnEjecutarConsultaSAP(ssql)
                If dtExistencia.Rows.Count > 0 Then
                    lblExistencia.Text = "Existencia: " & CDbl(dtExistencia.Rows(0)(0)).ToString("N2")
                End If

            End If


            fnCargaCarrito()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSubir_Click(sender As Object, e As EventArgs) Handles btnSubir.Click
        If FileUpload1.HasFile Then
            Dim archivo As String
            archivo = Server.MapPath("~") & "\" & FileUpload1.FileName
            Dim iContadorTotal As Int16 = 0
            Dim iContadorNoEncontrados As Int16 = 0
            Try
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
                iContadorTotal = dtDatos.Rows.Count
                If dtDatos.Rows.Count > 100 Then
                    Session("ArchivoLargo") = "SI"
                    ''Lo mandamos directo al motor, a las tablas intermedias
                    fnInsertaArchivoGrande(dtDatos)

                    ''Mostramos 10 registros como placebo para que el usuario vea que su archivo subió bien
                    For i = 0 To 9 Step 1

                        ssql = objdatos.fnObtenerQuery("Nombre-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtDatos.Rows(i)(0) & "'")

                        Dim dtItemName As New DataTable
                        dtItemName = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtItemName.Rows.Count = 0 Then
                            objdatos.fnLog("Carrito btnSubir", "no encontrado")
                            iContadorNoEncontrados = iContadorNoEncontrados + 1
                        Else
                            objdatos.fnLog("Carrito btnSubir", "subiendo: " & dtDatos.Rows(i)(0))
                            CargarCarritoTemplate(dtDatos.Rows(i)(1), dtDatos.Rows(i)(0))
                        End If

                        'Dim partida As New Cls_Pedido.Partidas
                        'Session("Partidas").add(partida)
                    Next

                Else
                    ''Camino normal
                    For i = 0 To dtDatos.Rows.Count - 1 Step 1

                        ssql = objdatos.fnObtenerQuery("Nombre-Producto")
                        ssql = ssql.Replace("[%0]", "'" & dtDatos.Rows(i)(0) & "'")

                        Dim dtItemName As New DataTable
                        dtItemName = objdatos.fnEjecutarConsultaSAP(ssql)
                        If dtItemName.Rows.Count = 0 Then
                            objdatos.fnLog("Carrito btnSubir", "no encontrado")
                            iContadorNoEncontrados = iContadorNoEncontrados + 1
                        Else
                            objdatos.fnLog("Carrito btnSubir", "subiendo: " & dtDatos.Rows(i)(0))
                            CargarCarritoTemplate(dtDatos.Rows(i)(1), dtDatos.Rows(i)(0))
                        End If

                        'Dim partida As New Cls_Pedido.Partidas
                        'Session("Partidas").add(partida)
                    Next
                End If

                objdatos.fnLog("Carrito btnSubir", "Termina")
                fnCargaCarrito()
                If iContadorNoEncontrados > 0 Then
                    lblExistencia.Text = "Artículos no encontrados:" & iContadorNoEncontrados & " de " & iContadorTotal
                Else
                    lblExistencia.Text = "Artículos leídos:" & iContadorTotal
                End If

            Catch ex As Exception
                objdatos.Mensaje("No se ha podido cargar el template, es probable que no tenga el formato esperado. Favor de revisar", Me.Page)
                objdatos.fnLog("cargar excel ex", ex.Message.Replace("'", ""))
            End Try

            ' Response.Redirect("carrito.aspx")
        End If
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
            'fDescuento = objDatos.fnDesctoB2C(HttpContext.Current.Session("ProductoVista"))
            'If fDescuento = 0 Then
            '    fDescuento = objDatos.fnObtenerDescuentoEspecialDelta(HttpContext.Current.Session("ProductoVista"))
            'End If
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

                        'If (objDatos.fnObtenerCliente.ToUpper.Contains("AIO") Or objDatos.fnObtenerCliente.ToUpper.Contains("PMK")) Then
                        '    'B2B sin IVA
                        '    dPrecioActual = dPrecioActual / 1.16
                        'End If

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
                        'If CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("SALAMA") Then

                        '    HttpContext.Current.Session("ErrorExistencia") = "No hay existencia del artículo seleccionado." _
                        '        & " Si no encontraste disponibilidad, contacta a un representante en el teléfono: 449 205 0883 en nuestros horarios de servicio."
                        '    Exit Function
                        'Else
                        '    If CInt(HttpContext.Current.Session("slpCode")) > 0 And CStr(objDatos.fnObtenerCliente()).ToUpper.Contains("HAWK") Then
                        '        ''en vendedores dejamos pasar la existencia
                        '    Else
                        '        HttpContext.Current.Session("ErrorExistencia") = "La(s) " & Cantidad & " pieza(s) del artículo seleccionado no se pudieron cargar al carrito por falta de existencia"
                        '        Exit Function
                        '    End If

                        'End If


                    End If
                Else



                End If
            End If

            objDatos.fnLog("Carga Carrito Cliente:", HttpContext.Current.Session("Cliente"))
            objDatos.fnLog("Carga Carrito ListaPrecios:", HttpContext.Current.Session("ListaPrecios"))



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




        Dim result As String = "Entró:" & Articulo

        Return result
    End Function

    Private Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged

    End Sub
End Class
