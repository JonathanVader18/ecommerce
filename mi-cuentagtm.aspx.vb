Imports System.Data
Partial Class mi_cuentagtm
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub mi_cuentagtm_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("UserB2C") = "" Then

        Else
            Response.Redirect("pagoindexhnd.aspx")
        End If
        If Not IsPostBack Then
            Session("ErrorCliente") = ""
            Dim fila As DataRow


        End If
    End Sub

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub


    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''btn Crear cuenta
        If txtnombre.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese el nombre")
            Exit Sub
        End If
        If txtUsuario.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese un usuario")
            Exit Sub
        End If

        If txtRFC.Text = "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Ingrese el NIT")
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


        Session("RFC") = txtRFC.Text

        Session("NombreUserB2C") = txtnombre.Text



        ''Todo bien
        ssql = "select ISNULL( MAX(ciIdConfig),0) + 1 from config.Usuarios "
        Dim dtConsecutivo As New DataTable
        dtConsecutivo = objDatos.fnEjecutarConsulta(ssql)

        Dim sRecibePromos As String = "NO"
        If chkOfertas.Checked = True Then
            sRecibePromos = "SI"
        End If
        ssql = "INSERT INTO config.Usuarios  (ciIdConfig,cvNombreCompleto,cvUsuario,cvPass,cvTipoAcceso,cvRecibePromos,cvCardCode,cvRFC,cvTelefono1,cvTelefono2)VALUES(" _
            & "'" & dtConsecutivo.Rows(0)(0) & "'," _
            & "'" & txtnombre.Text & "'," _
            & "'" & txtUsuario.Text & "'," _
            & "'" & txtPassword.Text & "','B2C'," _
            & "'" & sRecibePromos & "',''," _
            & "'" & txtRFC.Text & "'," _
            & "'','')"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Registrado correctamente. ", Me.Page)


        'ssql = "UPDATE config.usuarios SET cvEmpresa=" & "'" & txtEmpresa.Text & "' WHERE ciIdConfig=" & "'" & dtConsecutivo.Rows(0)(0) & "'"
        'objDatos.fnEjecutarInsert(ssql)

        '''Insertamos la direccion
        'ssql = "INSERT INTO Tienda.Direcciones_Envio ([cvUsuario],[cvCalle],[cvColonia],[cvNumExt],[cvNumInt],[cvCiudad],[cvMunicipio],[cvEstado],[cvPais],[cvPredeterminado],cvCP) VALUES(" _
        '        & "'" & txtUsuario.Text & "'," _
        '        & "'" & txtCalle.Text & "'," _
        '        & "''," _
        '        & "''," _
        '        & "''," _
        '        & "''," _
        '        & "'" & txtMunicipio.Text & "'," _
        '        & "'" & ddlEstados.SelectedValue & "'," _
        '        & "'" & ddlPais.SelectedValue & "','N'," _
        '        & "'" & txtCP.Text & "')"
        'objDatos.fnEjecutarInsert(ssql)


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


        ''Ahora revisamos si tenemos que mandarlo a SAP:
        Session("ErrorCliente") = ""
        ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
        Dim dtCreaSAP As New DataTable
        dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
        If dtCreaSAP.Rows.Count > 0 Then
            If dtCreaSAP.Rows(0)(0) = "SI" Then
                fnCreaClienteSAP(dtConsecutivo.Rows(0)(0))
            End If
        End If
        If Session("ErrorCliente") <> "" Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, Session("ErrorCliente"))
            ssql = "delete config.Usuarios WHERE cvUsuario=" & "'" & txtUsuario.Text & "' AND cvPass=" & "'" & txtPassword.Text & "' and cvTipoAcceso='B2C' "
            objDatos.fnEjecutarInsert(ssql)
            Session("UserB2C") = ""
            Session("NombreUserB2C") = ""
            Session("NombreuserTienda") = ""
            Session("CardCodeUserB2C") = ""
            Session("Cliente") = ""
            Exit Sub
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

            oCompany = objDatos.fnConexion_SAP
            Dim sCardCode As String = "ClienteB2C-" & ClienteInterno
            If oCompany.Connected Then
                objDatos.fnLog("Alta Cliente", "Si Conecta")
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.CardType = SAPbobsCOM.BoCardTypes.cCustomer

                oProspecto.CardName = txtnombre.Text
                oProspecto.FederalTaxID = txtRFC.Text
                oProspecto.UnifiedFederalTaxID = txtRFC.Text   'nnn-nn-nnnnn

                ssql = " SELECT ISNULL(cvEmpleadoVentas ,'-1') as Vendedor from config.Parametrizaciones_B2C "
                Dim dtVendedor As New DataTable
                dtVendedor = objDatos.fnEjecutarConsulta(ssql)
                If dtVendedor.Rows.Count > 0 Then
                    oProspecto.SalesPersonCode = dtVendedor.Rows(0)(0)
                End If


                Try
                    oProspecto.UserFields.Fields.Item("U_NIT").Value = txtRFC.Text
                Catch ex As Exception

                End Try
                '                oProspecto.VatGroupLatinAmerica = txtRFC.Text
                '  oProspecto.VATRegistrationNumber = txtRFC.Text
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

                Try
                    ''2 días Lazarus
                    ' oProspecto.PayTermsGrpCode = 7
                Catch ex As Exception

                End Try
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





                'End Try
                If oProspecto.Add <> 0 Then
                    'message = "alert('Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription & "');"
                    'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
                    objDatos.fnLog("Alta Cliente", "Error: " & oCompany.GetLastErrorDescription.Replace("'", ""))
                    '  objDatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)
                    Session("ErrorCliente") = oCompany.GetLastErrorDescription.Replace("'", "")
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

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        Response.Redirect("index.aspx")
    End Sub
End Class
