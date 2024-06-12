Imports System.Data
Partial Class pagoindexhnd
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub pagoindexhnd_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim fila As DataRow
        Session("Page") = "pagoindexhnd.aspx"
        If Not IsPostBack Then



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


            If Session("UserB2C") = "" Then
                pnlLogin.Visible = True
            Else
                pnlLogin.Visible = False
                ''Cargamos las direcciones del cliente seleccionado
                Dim dtdirecciones As New DataTable
                If Session("UserB2C") <> "" Then
                    ssql = "SELECT DISTINCT  [ciIdRel], ISNULL([cvCalle],'') + '  ' + ISNULL([cvNumExt],'') + '  ' + ISNULL([cvCP],'') as Direccion, [cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvColonia, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                    dtdirecciones = objDatos.fnEjecutarConsulta(ssql)
                Else
                    ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                    ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                    dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
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
                    lblTituloDir.Text = "Confirme la dirección de entrega"
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
                    If Session("UserB2C") <> "" Then
                        ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')Calle, ISNULL([cvColonia],'') Colonia, ISNULL([cvNumExt],'') as Numero, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') Estado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as CP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                        dtDetalleDireccion = objDatos.fnEjecutarConsulta(ssql)
                    Else
                        ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                        ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)

                        dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
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


            ''El RFC

            ssql = "SELECT ISNULL(cvRFC,''),ISNULL(cvNombreCompleto,'') as Nombre,ISNULL(cvTelefono1,'') Tel1, ISNULL(cvTelefono2,'') Tel2 FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtRFC As New DataTable
            dtRFC = objDatos.fnEjecutarConsulta(ssql)
            If dtRFC.Rows.Count > 0 Then
                txtRFC.Text = dtRFC.Rows(0)(0)
                txtNombre.Text = dtRFC.Rows(0)("Nombre")
                txtTelefono.Text = dtRFC.Rows(0)("Tel1")
                txtCelular.Text = dtRFC.Rows(0)("Tel2")

            End If

            ''La empresa, si esque aplica
            ssql = "SELECT ISNULL(cvEmpresa,'') FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtEmpresa As New DataTable
            dtEmpresa = objDatos.fnEjecutarConsulta(ssql)
            If dtEmpresa.Rows.Count > 0 Then
                txtNombreEmpresa.Text = dtEmpresa.Rows(0)(0)

            End If

            ''La leyenda se pasa a otra ventana

            '''Si tiene leyenda de comentarios la pintamos
            'ssql = "select ISNULL(cvLeyendaComentariosB2C,'') FROM config.parametrizaciones "
            'Dim dtLeyenda As New DataTable
            'dtLeyenda = objDatos.fnEjecutarConsulta(ssql)
            'If dtLeyenda.Rows.Count > 0 Then
            '    If dtLeyenda.Rows(0)(0) <> "" Then
            '        lblLeyendaComentarios.Visible = True
            '        lblLeyendaComentarios.Text = dtLeyenda.Rows(0)(0)
            '    End If
            'End If
        End If

        fnRecalculaTotales()

        '  fnTotales()
    End Sub

    Public Sub fnRecalculaTotales()
        Dim sSubTotal As Double = 0
        Dim x As Int16 = 0
        Dim TotDescuento As Double = 0
        Try
            objDatos.fnLog("Carrito", "For de partidas")
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    x = x + 1


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

                    If Partida.Descuento > 0 Then
                        TotDescuento = TotDescuento + (Partida.Precio - precioConDescuento)
                    End If

                    objDatos.fnLog("Carrito", precioConDescuento)
                    objDatos.fnLog("Carrito partida.precio", Partida.Precio)


                    sSubTotal = sSubTotal + (Partida.Cantidad * Partida.Precio)
                    ''Aqui van los botones de Action Cart

                End If


            Next
        Catch ex As Exception
            '   Response.Redirect("index.aspx")
            objDatos.fnLog("Carrito load", ex.Message)
        End Try

        objDatos.fnLog("Carrito", "SubTotales")


        lblSubTotal.Text = Session("Moneda") & " " & sSubTotal.ToString(" ###,###,###.#0")
        If TotDescuento = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = Session("Moneda") & " " & TotDescuento.ToString(" ###,###,###.#0")
        End If

        Session("ImporteSubTotal") = sSubTotal
        Dim Envio As Double = 0
        Dim Descuento As Double = 0
        Try
            If lblEnvio.Text = "" Then
                Envio = 0
            Else
                Envio = CDbl(lblEnvio.Text.Replace("$ ", "").Replace(Session("Moneda"), ""))
            End If


            If lblDescuento.Text = "" Then
                Descuento = 0
            Else
                Descuento = CDbl(lblDescuento.Text.Replace("$ ", "").Replace(Session("Moneda"), ""))
            End If


            Session("ImporteEnvio") = Envio
            Session("ImporteDescuento") = Descuento
        Catch ex As Exception

        End Try
        lblTotal.Text = Session("Moneda") & " " & (sSubTotal + Envio - Descuento).ToString(" ###,###,###.#0")
        Session("TotalCarrito") = (sSubTotal + Envio - Descuento)
    End Sub
    Public Sub fnTotales()

        Dim subtotal As Double = 0

        For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
            If Partida.ItemCode <> "BORRAR" Then
                subtotal = subtotal + (Partida.Cantidad * Partida.Precio)
            End If

        Next

        Session("ImporteSubTotal") = subtotal


        lblSubTotal.Text = Session("Moneda") & " " & subtotal.ToString(" ###,###,###.#0")
        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0")
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = Session("Moneda") & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0")
        End If
        lblTotal.Text = Session("Moneda") & " " & (subtotal + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString(" ###,###,###.#0")
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
    Protected Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        If chkInvitado.Checked = True And Session("UserB2C") = "" Then
            Session("usrInvitado") = "SI"
        End If

        If Session("usrInvitado") = "SI" Then
            If txtCorreoInvitado.Text = "" Then
                objDatos.Mensaje("Debe indicar una dirección de correo", Me.Page)
                Exit Sub
            Else
                Session("CorreoInvitado") = txtCorreoInvitado.Text
            End If
        End If

        If txtRFC.Text = "" Then
            objDatos.Mensaje("Debe indicar el RTN/Clave de Identidad", Me.Page)
            Exit Sub
        Else
            Session("RFC") = txtRFC.Text
        End If

        If txtCalle.Text = "" Then
            objDatos.Mensaje("Debe indicar la dirección", Me.Page)
            Exit Sub
        Else
            Session("CalleEnvio") = txtCalle.Text
        End If


        'If txtMunicipio.Text = "" Then
        '    objDatos.Mensaje("Debe indicar una referencia", Me.Page)
        '    Exit Sub
        'Else
        '    Session("MunicipioEnvio") = txtMunicipio.Text
        'End If

        If txtCelular.Text = "" Then
            objDatos.Mensaje("Debe indicar número de teléfono celular", Me.Page)
            Exit Sub
        Else
            Session("NumTel") = txtTelefono.Text
        End If
        If ddlEstados.SelectedItem.Text = "" Then
            objDatos.Mensaje("Debe indicar el departamento", Me.Page)
            Exit Sub
        Else
            Session("EstadoEnvio") = ddlEstados.SelectedItem.Text
        End If
        If ddlPais.SelectedItem.Text = "" Then
            objDatos.Mensaje("Debe indicar el País", Me.Page)
            Exit Sub
        Else
            Session("PaisEnvio") = ddlPais.SelectedItem.Text
        End If




        ''Validación solicitada por Lazarus, para determinar si la ciudad esta en el listado
        ssql = objDatos.fnObtenerQuery("CoberturaCiudad")
        ssql = ssql.Replace("[%0]", ddlEstados.SelectedItem.Text)
        Dim dtCiudadvalida As New DataTable
        dtCiudadvalida = objDatos.fnEjecutarConsultaSAP(ssql)
        If dtCiudadvalida.Rows.Count > 0 Then
            ''Y ahora validar si el monto cubre el mínimo
            ssql = objDatos.fnObtenerQuery("MinimoCompraCiudad")
            ssql = ssql.Replace("[%0]", ddlEstados.SelectedItem.Text)
            Dim dtMinCompra As New DataTable
            dtMinCompra = objDatos.fnEjecutarConsultaSAP(ssql)
            If dtMinCompra.Rows.Count > 0 Then
                If CDbl(dtMinCompra.Rows(0)(0)) > CDbl(Session("TotalCarrito")) Then
                    Dim difMinima As Double = 0
                    difMinima = CDbl(dtMinCompra.Rows(0)(0)) - CDbl(Session("TotalCarrito"))
                    objDatos.Mensaje("Para envíos a " & ddlEstados.SelectedItem.Text & " debe cubrir un mínimo de compra de: " & CDbl(dtMinCompra.Rows(0)(0)).ToString("###,###,###.#0") & " " & Session("Moneda") & ". Necesita aún: " & difMinima.ToString("###,###,###.#0") & " " & Session("Moneda"), Me.Page)
                    Exit Sub
                End If
            Else
                objDatos.Mensaje("El departamento que ha indicado en el domicilio de entrega, no se encuentra dentro de nuestra cobertura.", Me.Page)
                Exit Sub
            End If

        Else
            objDatos.Mensaje("El departamento que ha indicado en el domicilio de entrega, no se encuentra dentro de nuestra cobertura.", Me.Page)
            Exit Sub
        End If
        ''Termina validación de cobertura y mínimo de compra







        'If txtCP.Text = "" Then
        '    objDatos.Mensaje("Debe indicar el código postal", Me.Page)
        '    Exit Sub
        'Else
        '    Session("CPEnvio") = txtCP.Text
        'End If

        Session("NombreEnvio") = txtNombreEmpresa.Text
        Session("NombreuserTienda") = txtNombre.Text
        Session("NombreAdicional") = txtNombreEmpresa.Text

        '   Session("NumIntEnvio") = txtNumInt.Text




        Session("PaginaPago") = "resumen.aspx"
        Response.Redirect("envio.aspx")

    End Sub
    Protected Sub ddlDirecciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDirecciones.SelectedIndexChanged
        Try
            Try
                Try
                    Dim dtDetalleDireccion As New DataTable
                    ''Obtenemos el detalle de la dirección de envio
                    If Session("UserB2C") <> "" Then
                        ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')Calle, ISNULL([cvColonia],'') Colonia, ISNULL([cvNumExt],'') as Numero, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') Estado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as CP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                        dtDetalleDireccion = objDatos.fnEjecutarConsulta(ssql)
                    Else
                        ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                        ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)

                        dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                    End If

                    If dtDetalleDireccion.Rows.Count > 0 Then
                        txtNombre.Text = Session("NombreUserB2C")
                        txtCalle.Text = dtDetalleDireccion.Rows(0)("Calle")
                        'txtColonia.Text = dtDetalleDireccion.Rows(0)("Colonia")
                        '  txtCiudad.Text = dtDetalleDireccion.Rows(0)("Ciudad")
                        '   txtNumExt.Text = dtDetalleDireccion.Rows(0)("Numero")
                        txtCP.Text = dtDetalleDireccion.Rows(0)("CP")
                        txtEstado.Text = dtDetalleDireccion.Rows(0)("Estado")
                        txtMunicipio.Text = dtDetalleDireccion.Rows(0)("Municipio")
                        '  txt.Text = dtDetalleDireccion.Rows(0)("Pais")
                        ddlEstados.SelectedValue = dtDetalleDireccion.Rows(0)("Estado")
                        ddlPais.SelectedValue = dtDetalleDireccion.Rows(0)("cvPais")
                    End If
                Catch ex As Exception

                End Try
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
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
            pnlLogin.Visible = False
            Session("usrInvitado") = "NO"
            pnlIngresar.Visible = False
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

            Dim fila As DataRow


            ''Cargamos las direcciones del cliente seleccionado
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
            Dim dtdirecciones As New DataTable
            If Session("UserB2C") <> "" Then
                ssql = "SELECT DISTINCT  [ciIdRel], ISNULL([cvCalle],'') + '  ' + ISNULL([cvNumExt],'') + '  ' + ISNULL([cvCP],'') as Direccion, [cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvColonia, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                dtdirecciones = objDatos.fnEjecutarConsulta(ssql)
            Else
                ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
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
                lblTituloDir.Text = "Confirme la dirección de entrega"
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
                If Session("UserB2C") <> "" Then
                    ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')Calle, ISNULL([cvColonia],'') Colonia, ISNULL([cvNumExt],'') as Numero, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') Estado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as CP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                    dtDetalleDireccion = objDatos.fnEjecutarConsulta(ssql)
                Else
                    ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                    ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C")).Replace("[%1]", ddlDirecciones.SelectedItem.Text)

                    dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
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





            ''El RFC

            ssql = "SELECT ISNULL(cvRFC,''),ISNULL(cvNombreCompleto,'') as Nombre,ISNULL(cvTelefono1,'') Tel1, ISNULL(cvTelefono2,'') Tel2 FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtRFC As New DataTable
            dtRFC = objDatos.fnEjecutarConsulta(ssql)
            If dtRFC.Rows.Count > 0 Then
                txtRFC.Text = dtRFC.Rows(0)(0)
                txtNombre.Text = dtRFC.Rows(0)("Nombre")
                txtTelefono.Text = dtRFC.Rows(0)("Tel1")
                txtCelular.Text = dtRFC.Rows(0)("Tel2")

            End If

            ''La empresa, si esque aplica
            ssql = "SELECT ISNULL(cvEmpresa,'') FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
            Dim dtEmpresa As New DataTable
            dtEmpresa = objDatos.fnEjecutarConsulta(ssql)
            If dtEmpresa.Rows.Count > 0 Then
                txtNombreEmpresa.Text = dtEmpresa.Rows(0)(0)

            End If
        Else
            objDatos.Mensaje("Acceso incorrecto", Me.Page)
            Exit Sub
        End If
    End Sub
End Class
