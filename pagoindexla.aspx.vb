Imports System.Data
Partial Class pagoindexla
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub pagoindexla_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim fila As DataRow
        Session("Page") = "pagoindexla.aspx"
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
                pnlIngresar.Visible = True
            Else
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
                        txtNumExt.Text = dtDetalleDireccion.Rows(0)("Numero")
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
        fnTotales()
    End Sub
    Public Sub fnTotales()

        Dim subtotal As Double = 0
        If CDbl(Session("ImporteSubTotal")) = 0 Then
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    subtotal = subtotal + Partida.Cantidad * Partida.Precio
                End If

            Next

            Session("ImporteSubTotal") = subtotal
        End If



        lblSubTotal.Text = Session("Moneda") & " " & CDbl(Session("ImporteSubTotal")).ToString(" ###,###,###.#0")

        If Session("ImporteDescuento") = 0 Then
            lblDescuento.Text = ""
        Else
            lblDescuento.Text = Session("Moneda") & " " & CDbl(Session("ImporteDescuento")).ToString(" ###,###,###.#0")
        End If


        If Session("ImporteEnvio") = 0 Then
            lblEnvio.Text = ""
        Else
            lblEnvio.Text = Session("Moneda") & " " & CDbl(Session("ImporteEnvio")).ToString(" ###,###,###.#0")
        End If

        lblTotal.Text = Session("Moneda") & " " & (CDbl(Session("ImporteSubTotal")) + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString(" ###,###,###.#0")



        ssql = "select ISNULL(cvPreciosMasIva,'NO') FROM config.parametrizaciones "
        Dim dtTipoPrecio As New DataTable
        dtTipoPrecio = objDatos.fnEjecutarConsulta(ssql)
        If dtTipoPrecio.Rows.Count > 0 Then
            If dtTipoPrecio.Rows(0)(0) = "NO" Then
                ''Calculamos el IVA
                'Dim fIVA As Double = 0
                'fIVA = objDatos.fnCalculaIVA(Session("TotalCarrito"))
                pnlImpuestos.Visible = True
                lblImpuestos.Text = Session("Moneda") & " " & CDbl(Session("TotalImpuestos")).ToString(" ###,###,###.#0")
                lbltotalImp.Text = Session("Moneda") & " " & (CDbl(Session("TotalCarrito")) + CDbl(Session("TotalImpuestos"))).ToString(" ###,###,###.#0")
            End If
        End If
    End Sub

    Protected Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click

        If Session("UserB2C") = "" Then
            objDatos.Mensaje("Debe iniciar sesión o registrarse para poder continuar con la compra", Me.Page)
            Exit Sub
        End If
        'If chkInvitado.Checked = True And Session("UserB2C") = "" Then
        '    Session("usrInvitado") = "SI"
        'End If

        'If Session("usrInvitado") = "SI" Then
        '    If txtCorreoInvitado.Text = "" Then
        '        objDatos.Mensaje("Debe indicar una dirección de correo", Me.Page)
        '        Exit Sub
        '    Else
        '        Session("CorreoInvitado") = txtCorreoInvitado.Text
        '    End If
        'End If
        If txtCalle.Text = "" Then
            objDatos.Mensaje("Debe indicar el domicilio", Me.Page)
            Exit Sub
        Else
            Session("CalleEnvio") = txtCalle.Text
        End If

        Session("MunicipioEnvio") = txtMunicipio.Text  ''Localidad

        'If txtNumExt.Text = "" Then
        '    objDatos.Mensaje("Debe indicar número ", Me.Page)
        '    Exit Sub
        'Else
        '    Session("NumExtEnvio") = txtNumExt.Text
        'End If
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


        Session("CPEnvio") = txtCP.Text

        If txtNombre.Text = "" Then
            objDatos.Mensaje("Debe indicar el nombre", Me.Page)
            Exit Sub
        End If
        If txtComentarios.Text = "" Then
            'objDatos.Mensaje("Debe indicar comentarios", Me.Page)
            'Exit Sub
        End If

        Session("NombreEnvio") = txtNombre.Text

        Session("NombreDireccionEnvio") = txtNombre.Text


        If ddlDirecciones.Items.Count > 0 Then
            If ddlDirecciones.SelectedItem.Text = "-Seleccione-" Or ddlDirecciones.SelectedItem.Text = "" Then
                Session("NombreDireccionEnvio") = "Nueva Direccion"

            End If

        End If

        ''Revisamos si hay minimo de compra
        ssql = "select ISNULL(cfMinimoCompraB2C,'0') FROM config.parametrizaciones "
        Dim dtMinimo As New DataTable
        dtMinimo = objDatos.fnEjecutarConsulta(ssql)
        If dtMinimo.Rows.Count > 0 Then
            Dim MinCompra As Double = 0
            MinCompra = dtMinimo.Rows(0)(0)
            If MinCompra > CDbl(lblTotal.Text.Replace("$", "")) Then
                objDatos.Mensaje("Debe cumplir con un mínimo de compra de " & MinCompra.ToString("$ ###,###,###.0#"), Me.Page)
                Exit Sub
            End If
        End If

        Session("PaginaPago") = "resumen.aspx"
        Response.Redirect("envio.aspx")
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


            ''Cargamos las direcciones del cliente seleccionado
            Dim dtdirecciones As New DataTable
            If Session("UserB2C") <> "" Then
                ssql = "SELECT  [ciIdRel], ISNULL([cvCalle],'') + ' No ' + ISNULL([cvNumExt],'') + ' CP: ' + ISNULL([cvCP],'') as Direccion, [cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvColonia, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "'"
                dtdirecciones = objDatos.fnEjecutarConsulta(ssql)
            Else
                ssql = objDatos.fnObtenerQuery("DireccionesEntrega")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                dtdirecciones = objDatos.fnEjecutarConsultaSAP(ssql)
            End If

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
                txtNombre.Text = dtNombre.Rows(0)(0)
            End If
        Else
            objDatos.Mensaje("Acceso incorrecto", Me.Page)
            Exit Sub
        End If
    End Sub
    Protected Sub ddlDirecciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDirecciones.SelectedIndexChanged
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
                    txtNumExt.Text = dtDetalleDireccion.Rows(0)("Numero")
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
End Class
