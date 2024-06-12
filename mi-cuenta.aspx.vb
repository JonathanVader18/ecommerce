Imports System.Data
Partial Class mi_cuenta
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub mi_cuenta_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserB2C") = "" Then

        Else
            Response.Redirect("pagoindex.aspx")
        End If

        ssql = "SELECT ISNULL(cvPaginaMiCuenta,'') FROM  config.Parametrizaciones_B2C "
        Dim dtPagina As New DataTable
        dtPagina = objDatos.fnEjecutarConsulta(ssql)
        If dtPagina.Rows.Count > 0 Then
            If dtPagina.Rows(0)(0) <> "" Then
                Response.Redirect(dtPagina.Rows(0)(0))
            End If
        End If
        If Not IsPostBack Then
            ssql = objDatos.fnObtenerQuery("UsoCFDI")
            If ssql <> "" Then
                Dim dtUso As New DataTable
                dtUso = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtUso.Rows.Count > 0 Then
                    Dim fila As DataRow
                    fila = dtUso.NewRow
                    fila("Clave") = "-1"
                    fila("Descripcion") = "-Seleccione-"
                    dtUso.Rows.Add(fila)

                    ddlUsoCFDI.DataSource = dtUso
                    ddlUsoCFDI.DataTextField = "Descripcion"
                    ddlUsoCFDI.DataValueField = "Clave"
                    ddlUsoCFDI.DataBind()
                    ddlUsoCFDI.SelectedValue = "-1"



                End If
            End If
        End If
    End Sub
    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Response.Redirect("ingresar.aspx")
    End Sub
    Protected Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''btn Crear cuenta
        If txtnombre.Text = "" Then
            objDatos.Mensaje("Ingrese el nombre", Me.Page)
            Exit Sub
        End If
        If txtUsuario.Text = "" Then
            objDatos.Mensaje("Ingrese un usuario", Me.Page)
            Exit Sub
        End If
        If txtPassword.Text = "" Then
            objDatos.Mensaje("Ingrese una contraseña", Me.Page)
            Exit Sub
        End If
        If txtConfirma.Text = "" Then
            objDatos.Mensaje("Confirme su contraseña", Me.Page)
            Exit Sub
        End If
        If txtPassword.Text <> txtConfirma.Text Then
            objDatos.Mensaje("Las contraseñas no coinciden", Me.Page)
            Exit Sub
        End If
        txtRFC.Text = "XEXX010101000"
        If txtRFC.Text <> "" Then
            If txtRFC.Text.Length < 12 Then
                objDatos.Mensaje("El RFC que ha ingresado no es válido", Me.Page)
                Exit Sub
            End If
        End If

        ''Todo bien
        ssql = "select ISNULL( MAX(ciIdConfig),0) + 1 from config.Usuarios "
        Dim dtConsecutivo As New DataTable
        dtConsecutivo = objDatos.fnEjecutarConsulta(ssql)

        Dim sRecibePromos As String = "NO"
        If chkOfertas.Checked = True Then
            sRecibePromos = "SI"
        End If
        ssql = "INSERT INTO config.Usuarios  (ciIdConfig,cvNombreCompleto,cvUsuario,cvPass,cvTipoAcceso,cvRecibePromos,cvCardCode,cvRFC)VALUES(" _
            & "'" & dtConsecutivo.Rows(0)(0) & "'," _
            & "'" & txtnombre.Text & "'," _
            & "'" & txtUsuario.Text & "'," _
            & "'" & txtPassword.Text & "','B2C'," _
            & "'" & sRecibePromos & "',''," _
            & "'" & txtRFC.Text & "')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado correctamente. ", Me.Page)


        Session("RFC") = txtRFC.Text

        Session("NombreUserB2C") = txtnombre.Text





        ''Ahora revisamos si tenemos que mandarlo a SAP:

        ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
        Dim dtCreaSAP As New DataTable
        dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtCreaSAP.Rows.Count > 0 Then
            If dtCreaSAP.Rows(0)(0) = "SI" Then
                fnCreaClienteSAP(dtConsecutivo.Rows(0)(0))
            Else
                ''Obtenemos el cardCode Por default

            End If
        End If


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
    End Sub

    Public Sub fnCreaClienteSAP(ClienteInterno As Int64)
        ''Ingresamos los valores a SAP
        Dim oProspecto As SAPbobsCOM.BusinessPartners
        Dim oCompany As New SAPbobsCOM.Company
        Dim sCardCode As String = "Cliente-" & ClienteInterno
        Dim sCliente As String = ""

        Try
            ''Obtenemos la configuración de como dar de alta el cliente
            ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objDatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then
                If CStr(dtcliente.Rows(0)(0)).Contains("Salama") And Session("RazonSocial") = "" Then
                    ''B2C
                    sCliente = "SALAMA"
                    Dim iNumCeros As Int16 = 6
                    Dim sCeros As String = ""
                    objDatos.fnLog("Alta Cliente", sCliente)
                    For i = 0 To iNumCeros - ClienteInterno.ToString.Length - 1 Step 1
                        sCeros = sCeros & "0"
                    Next

                    sCardCode = "W-" & sCeros & ClienteInterno
                End If
            End If

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

            oCompany = objDatos.fnConexion_SAP

            If oCompany.Connected Then
                objDatos.fnLog("Alta Cliente", "Si Conecta")
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.CardType = SAPbobsCOM.BoCardTypes.cCustomer

                oProspecto.CardName = txtnombre.Text
                oProspecto.FederalTaxID = "123456789012"

                'If dtTipo.Rows.Count > 0 Then
                '    If dtTipo.Rows(0)(0) = "C" Then

                '    Else
                '        oProspecto.CardType = SAPbobsCOM.BoCardTypes.cLid
                '    End If
                'End If
                Dim sRFC As String = "XEXX010101000"
                If txtRFC.Text <> "" Then
                    sRFC = txtRFC.Text
                End If

                If sCliente = "SALAMA" Then
                    oProspecto.EmailAddress = txtUsuario.Text
                    oProspecto.FederalTaxID = sRFC
                    If ddlUsoCFDI.SelectedItem.Text.Contains("Seleccione") Then
                    Else
                        oProspecto.UserFields.Fields.Item("U_UsoCFDI").Value = ddlUsoCFDI.SelectedValue

                    End If
                Else
                    oProspecto.VatIDNum = "123456789012"   'nnn-nn-nnnnn
                    oProspecto.VATRegistrationNumber = "123-12-48899"
                End If
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
                Catch ex As Exception

                End Try
                objDatos.fnLog("Alta Cliente", "Sale campos de usuario")

                '  oProspecto.EmailAddress = txtCorreo.Text

                'If txtrfc.Text.Length = 12 Then
                '    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cCompany
                'Else
                '    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cPrivate
                'End If

                oProspecto.PayTermsGrpCode = -1

                '''Direccion fiscal

                oProspecto.Addresses.SetCurrentLine(0)
                oProspecto.Addresses.TypeOfAddress = "B"
                oProspecto.Addresses.AddressName = "Facturación"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                oProspecto.Addresses.Block = ""
                oProspecto.Addresses.City = "Ciudad"

                oProspecto.Addresses.Street = "Calle A"
                oProspecto.Addresses.StreetNo = "ND"

                ''Ponemos la misma en envio
                oProspecto.Addresses.Add()
                oProspecto.Addresses.TypeOfAddress = "S"
                oProspecto.Addresses.AddressName = "Envio"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                oProspecto.Addresses.Block = "Block"
                oProspecto.Addresses.City = ""
                'oProspecto.Addresses.County = ""
                'oProspecto.Addresses.State = ddlEstado.SelectedValue
                oProspecto.Addresses.Street = ""
                oProspecto.Addresses.StreetNo = "NA"
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
                    objDatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)
                    objDatos.fnLog("Alta Cliente", "Error: " & oCompany.GetLastErrorDescription.Replace("'", ""))
                    'objDatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)

                Else
                    ssql = "UPDATE config.Usuarios SET cvCardCode='" & oCompany.GetNewObjectKey & "' WHERE ciIdConfig=" & "'" & ClienteInterno & "'"
                    objDatos.fnEjecutarInsert(ssql)
                    objDatos.fnLog("Alta Cliente", "Registrado")
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

End Class
