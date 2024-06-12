
Imports System.Data
Partial Class mi_cuentala
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub mi_cuentala_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("UserB2C") = "" Then

        Else
            Response.Redirect("pagoindexla.aspx")
        End If

        If Not IsPostBack Then
            Dim fila As DataRow

            ''Cargamos el pais
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

            Try
                ''Cargamos los estados

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
        End If
    End Sub

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub

    Protected Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''btn Crear cuenta
        If txtnombre.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese el nombre")
            Exit Sub
        End If
        If txtRFC.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese el RUT/CI")
            Exit Sub

        End If
        If txtUsuario.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese un usuario")
            Exit Sub
        End If
        If txtPassword.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese una contraseña")
            Exit Sub
        End If
        If txtConfirma.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Confirme su contraseña")
            Exit Sub
        End If
        If txtPassword.Text <> txtConfirma.Text Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Las contraseñas no coinciden")
            Exit Sub
        End If

        If txtCalle.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Favor de indicar domicilio")
            Exit Sub
        End If

        If ddlPais.Text = "" Or ddlPais.Text = "-Seleccione-" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Favor de indicar el país")
            Exit Sub
        End If

        If ddlEstados.Text = "" Or ddlEstados.Text = "-Seleccione-" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Favor de indicar el estado")
            Exit Sub
        End If
        Session("RFC") = txtRFC.Text
        Session("CalleEnvio") = txtCalle.Text
        Session("NombreUserB2C") = txtnombre.Text
        Session("PaisLogin") = ddlPais.SelectedValue
        Session("EstadoLogin") = ddlEstados.SelectedValue

        ''Todo bien
        ssql = "select ISNULL( MAX(ciIdConfig),0) + 1 from config.Usuarios "
        Dim dtConsecutivo As New DataTable
        dtConsecutivo = objDatos.fnEjecutarConsulta(ssql)

        Dim sRecibePromos As String = "NO"
        If chkOfertas.Checked = True Then
            sRecibePromos = "SI"
        End If

        ''Ahora revisamos si tenemos que mandarlo a SAP:

        ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
        Dim dtCreaSAP As New DataTable
        dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtCreaSAP.Rows.Count > 0 Then
            If dtCreaSAP.Rows(0)(0) = "SI" Then
                fnCreaClienteSAP(dtConsecutivo.Rows(0)(0))
            Else
                ssql = "INSERT INTO config.Usuarios  (ciIdConfig,cvNombreCompleto,cvUsuario,cvPass,cvTipoAcceso,cvRecibePromos,cvCardCode,cvTelefono1,cvRFC)VALUES(" _
          & "'" & dtConsecutivo.Rows(0)(0) & "'," _
          & "'" & txtnombre.Text & "'," _
          & "'" & txtUsuario.Text & "'," _
          & "'" & txtPassword.Text & "','B2C'," _
          & "'" & sRecibePromos & "',''," _
          & "'" & txtTelefono.Text & "'," _
          & "'" & txtRFC.Text & "')"
                objDatos.fnEjecutarInsert(ssql)
                'objDatos.Mensaje("Registrado correctamente. ", Me.Page)

                ''Insertamos la direccion
                ssql = "INSERT INTO Tienda.Direcciones_Envio ([cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais],[cvPredeterminado],cvCP) VALUES(" _
                        & "'" & txtUsuario.Text & "'," _
                        & "'" & txtCalle.Text & "'," _
                        & "''," _
                        & "'" & txtNumExt.Text & "'," _
                        & "''," _
                        & "''," _
                        & "'" & txtMunicipio.Text & "'," _
                        & "'" & ddlEstados.SelectedValue & "'," _
                        & "'" & ddlPais.SelectedValue & "','N'," _
                        & "'" & txtCP.Text & "')"
                objDatos.fnEjecutarInsert(ssql)


                ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUsuario.Text & "' AND cvPass=" & "'" & txtPassword.Text & "' and cvTipoAcceso='B2C' "
                Dim dtLogin As New DataTable
                dtLogin = objDatos.fnEjecutarConsulta(ssql)
                If dtLogin.Rows.Count > 0 Then
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

                End If







                Dim text As String = MensajeHTML(Server.MapPath("~") & "\nuevoUsuario.html")
                Dim sDestinatario As String = ""

                sDestinatario = txtUsuario.Text

                ''Obtenemos el nombre de la empresa
                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                Dim dtcliente As New DataTable
                dtcliente = objDatos.fnEjecutarConsulta(ssql)
                text = text.Replace("{enviara}", txtnombre.Text)


                objDatos.fnEnviarCorreo(sDestinatario, text, "Bienvenido a " & dtcliente.Rows(0)(0) & "!")


                txtUsuario.Text = ""
                txtPassword.Text = ""
                txtConfirma.Text = ""
                txtUsuario.Enabled = False
                txtPassword.Enabled = False
                txtConfirma.Enabled = False
                btnEntrar.Enabled = False


                Response.Redirect("bienvenida.aspx")
            End If
        End If




    End Sub

    Public Sub fnCreaClienteSAP(ClienteInterno As Int64)
        ''Ingresamos los valores a SAP
        Dim oProspecto As SAPbobsCOM.BusinessPartners
        Dim oCompany As New SAPbobsCOM.Company
        Try
            ''Obtenemos la configuración de como dar de alta el cliente


            ''Serie,grupo y lista de precios
            Dim iSerie As Int64 = 0
            Dim iLista As Int64 = 0
            Dim sGrupo As String = ""

            ssql = " SELECT ISNULL(cvSerieClientes ,'') as Serie, ISNULL(cvGrupo ,'') as Grupo,ISNULL(ciListaPrecios ,0) as Lista from config.Parametrizaciones_B2C "
            Dim dtParam As New DataTable
            dtParam = objDatos.fnEjecutarConsulta(ssql)
            If dtParam.Rows.Count > 0 Then
                If dtParam.Rows(0)("Serie") <> "" Then
                    iSerie = dtParam.Rows(0)("Serie")
                End If
                If dtParam.Rows(0)("Grupo") <> "" Then
                    sGrupo = dtParam.Rows(0)("Grupo")
                End If
                If dtParam.Rows(0)("Lista") <> "0" Then
                    iLista = dtParam.Rows(0)("Lista")
                End If
            End If


            ssql = " SELECT ISNULL(cvCondicionPago ,'') as CondicionPago, ISNULL(cvEmpleadoVentas ,'') as EmpleadoVentas,ISNULL(cvTerritorio ,'') as Territorio from config.Parametrizaciones_B2C "
            Dim dtParam2 As New DataTable
            dtParam2 = objDatos.fnEjecutarConsulta(ssql)

            oCompany = objDatos.fnConexion_SAP
            Dim sCardCode As String = "ClienteB2C-" & ClienteInterno
            If oCompany.Connected Then
                objDatos.fnLog("Alta Cliente", "Si Conecta")
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.CardType = SAPbobsCOM.BoCardTypes.cCustomer

                oProspecto.CardName = txtnombre.Text
                oProspecto.CardForeignName = txtnombre.Text
                oProspecto.FederalTaxID = txtRFC.Text
                oProspecto.VatIDNum = txtRFC.Text   'nnn-nn-nnnnn
                oProspecto.EmailAddress = txtUsuario.Text
                '   oProspecto.DiscountRelations = SAPbobsCOM.DiscountGroupRelationsEnum.dgrMultipliedDiscount
                '  oProspecto.EffectiveDiscount = SAPbobsCOM.DiscountGroupRelationsEnum.dgrMultipliedDiscount

                oProspecto.Phone1 = txtTelefono.Text
                Try
                    If dtParam2.Rows.Count > 0 Then
                        If dtParam2.Rows(0)("Territorio") <> "" Then
                            oProspecto.Territory = dtParam2.Rows(0)("Territorio")
                        End If
                        If dtParam2.Rows(0)("CondicionPago") <> "" Then
                            '   oProspecto.PeymentMethodCode = dtParam2.Rows(0)("CondicionPago")
                            oProspecto.PayTermsGrpCode = dtParam2.Rows(0)("CondicionPago")
                        End If
                        If dtParam2.Rows(0)("EmpleadoVentas") <> "" Then
                            oProspecto.SalesPersonCode = dtParam2.Rows(0)("EmpleadoVentas")
                        End If

                    End If

                Catch ex As Exception

                End Try


                '  oProspecto.VATRegistrationNumber = "123-12-48899"
                'If dtTipo.Rows.Count > 0 Then
                '    If dtTipo.Rows(0)(0) = "C" Then

                '    Else
                '        oProspecto.CardType = SAPbobsCOM.BoCardTypes.cLid
                '    End If
                'End If
                If iSerie <> 0 Then
                    oProspecto.Series = iSerie
                Else
                    oProspecto.CardCode = sCardCode
                End If

                If sGrupo <> "" Then
                    oProspecto.GroupCode = sGrupo
                End If
                If iLista <> 0 Then
                    oProspecto.PriceListNum = iLista
                    Session("ListaPrecios") = iLista
                End If
                objDatos.fnLog("Alta Cliente", "asignó series, grupos y listas")
                Try
                    oProspecto.UserFields.Fields.Item("U_CEO").Value = "99"
                    oProspecto.UserFields.Fields.Item("U_CANAL").Value = "6006"
                    oProspecto.UserFields.Fields.Item("U_CORE_CLOUD_IGNORE").Value = "Y"
                Catch ex As Exception

                End Try
                Try
                    If txtRFC.Text.Length < 12 Then
                        oProspecto.Properties("1") = SAPbobsCOM.BoYesNoEnum.tYES
                    Else
                        oProspecto.Properties("2") = SAPbobsCOM.BoYesNoEnum.tYES
                    End If
                    oProspecto.CreditLimit = "999999"
                    oProspecto.MaxCommitment = "999999"
                Catch ex As Exception

                End Try

                objDatos.fnLog("Alta Cliente", "Sale campos de usuario")

                '  oProspecto.EmailAddress = txtCorreo.Text

                'If txtrfc.Text.Length = 12 Then
                '    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cCompany
                'Else
                '    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cPrivate
                'End If



                '''Direccion fiscal

                oProspecto.Addresses.SetCurrentLine(0)
                oProspecto.Addresses.TypeOfAddress = "B"
                oProspecto.Addresses.AddressName = "Facturación"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                oProspecto.Addresses.ZipCode = txtCP.Text
                oProspecto.Addresses.City = txtMunicipio.Text
                oProspecto.Addresses.State = ddlEstados.SelectedValue
                oProspecto.Addresses.Street = txtCalle.Text
                oProspecto.Addresses.StreetNo = "ND"

                ''Ponemos la misma en envio
                oProspecto.Addresses.Add()
                oProspecto.Addresses.TypeOfAddress = "S"
                oProspecto.Addresses.AddressName = "Envio"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                oProspecto.Addresses.ZipCode = txtCP.Text
                oProspecto.Addresses.State = ddlEstados.SelectedValue
                oProspecto.Addresses.City = txtMunicipio.Text
                'oProspecto.Addresses.County = ""
                'oProspecto.Addresses.State = ddlEstado.SelectedValue
                oProspecto.Addresses.Street = txtCalle.Text
                oProspecto.Addresses.StreetNo = "NA"



                oProspecto.ContactEmployees.SetCurrentLine(0)
                oProspecto.ContactEmployees.FirstName = txtnombre.Text
                oProspecto.ContactEmployees.Name = "Titular"
                oProspecto.ContactEmployees.Position = ""
                oProspecto.ContactEmployees.E_Mail = txtUsuario.Text
                oProspecto.ContactEmployees.Phone1 = txttelefono.Text

                oProspecto.ContactEmployees.Address = txtCalle.Text & " "
                oProspecto.ContactEmployees.Active = SAPbobsCOM.BoYesNoEnum.tYES
                oProspecto.ContactEmployees.Add()


                'oProspecto.Addresses.ZipCode = txtCP.Text
                'If txtNumInt.Text <> "" Then
                '    oProspecto.Addresses.StreetNo = txtNumExt.Text & "-" & txtNumInt.Text
                'Else
                '    oProspecto.Addresses.StreetNo = txtNumExt.Text
                'End If
                'Try
                '    oProspecto.UserFields.Fields.Item("U_Email").Value = "3"
                '    oProspecto.UserFields.Fields.Item("U_DocEnviar").Value = "2"
                'Catch ex As Exception

                'End Try

                'Try
                '    oProspecto.Industry = ddlEspecialidad.SelectedValue
                'Catch ex As Exception

                'End Try
                If oProspecto.Add <> 0 Then
                    'message = "alert('Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription & "');"
                    'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
                    objDatos.fnLog("Alta Cliente", "Error: " & oCompany.GetLastErrorDescription.Replace("'", ""))
                    objDatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)

                Else
                    ''Todo bien
                    ssql = "select ISNULL( MAX(ciIdConfig),0) + 1 from config.Usuarios "
                    Dim dtConsecutivo As New DataTable
                    dtConsecutivo = objDatos.fnEjecutarConsulta(ssql)

                    Dim sRecibePromos As String = "NO"
                    If chkOfertas.Checked = True Then
                        sRecibePromos = "SI"
                    End If

                    ssql = "INSERT INTO config.Usuarios  (ciIdConfig,cvNombreCompleto,cvUsuario,cvPass,cvTipoAcceso,cvRecibePromos,cvCardCode,cvTelefono1,cvRFC)VALUES(" _
            & "'" & dtConsecutivo.Rows(0)(0) & "'," _
            & "'" & txtnombre.Text & "'," _
            & "'" & txtUsuario.Text & "'," _
            & "'" & txtPassword.Text & "','B2C'," _
            & "'" & sRecibePromos & "','" & oCompany.GetNewObjectKey & "'," _
            & "'" & txtTelefono.Text & "'," _
            & "'" & txtRFC.Text & "')"
                    objDatos.fnEjecutarInsert(ssql)
                    'objDatos.Mensaje("Registrado correctamente. ", Me.Page)

                    ''Insertamos la direccion
                    ssql = "INSERT INTO Tienda.Direcciones_Envio ([cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais],[cvPredeterminado],cvCP) VALUES(" _
                & "'" & txtUsuario.Text & "'," _
                & "'" & txtCalle.Text & "'," _
                & "''," _
                & "'" & txtNumExt.Text & "'," _
                & "''," _
                & "''," _
                & "'" & txtMunicipio.Text & "'," _
                & "'" & ddlEstados.SelectedValue & "'," _
                & "'" & ddlPais.SelectedValue & "','N'," _
                & "'" & txtCP.Text & "')"
                    objDatos.fnEjecutarInsert(ssql)


                    ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUsuario.Text & "' AND cvPass=" & "'" & txtPassword.Text & "' and cvTipoAcceso='B2C' "
                    Dim dtLogin As New DataTable
                    dtLogin = objDatos.fnEjecutarConsulta(ssql)
                    If dtLogin.Rows.Count > 0 Then
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

                    End If



                    Dim text As String = MensajeHTML(Server.MapPath("~") & "\nuevoUsuario.html")
                    Dim sDestinatario As String = ""

                    sDestinatario = txtUsuario.Text

                    ''Obtenemos el nombre de la empresa
                    ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                    Dim dtcliente As New DataTable
                    dtcliente = objDatos.fnEjecutarConsulta(ssql)
                    text = text.Replace("{enviara}", txtnombre.Text)


                    objDatos.fnEnviarCorreo(sDestinatario, text, "Bienvenido a " & dtcliente.Rows(0)(0) & "!")


                    txtUsuario.Text = ""
                    txtPassword.Text = ""
                    txtConfirma.Text = ""
                    txtUsuario.Enabled = False
                    txtPassword.Enabled = False
                    txtConfirma.Enabled = False
                    btnEntrar.Enabled = False

                    ssql = "UPDATE config.Usuarios SET cvCardCode='" & oCompany.GetNewObjectKey & "' WHERE ciIdConfig=" & "'" & ClienteInterno & "'"
                    objDatos.fnEjecutarInsert(ssql)
                    objDatos.fnLog("Alta Cliente", "Registrado")


                    Response.Redirect("bienvenida.aspx")


                    ' objDatos.Mensaje("Registrado! ", Me.Page)


                End If

            End If
        Catch ex As Exception
            objDatos.fnLog("Alta Cliente ex", ex.Message)
        End Try
    End Sub

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
