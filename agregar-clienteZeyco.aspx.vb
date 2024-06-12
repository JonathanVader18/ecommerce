
Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Mail
Imports System.Net.Mime
Imports Newtonsoft.Json

Partial Class agregar_clienteZeyco
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub agregar_clienteZeyco_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim dtPersonas As New DataTable
            dtPersonas.Columns.Add("Tipo")
            dtPersonas.Columns.Add("TipoDes")

            Session("OrigenAlta") = ""

            If Request.QueryString("Origen") <> "" Then
                Session("OrigenAlta") = Request.QueryString("Origen")
            End If

            If Request.QueryString("origen") <> "" Then
                Session("OrigenAlta") = Request.QueryString("origen")
            End If



            Dim dtTipoRev As New DataTable
            dtTipoRev.Columns.Add("TipoRev")
            dtTipoRev.Columns.Add("TipoRevDes")
            Dim filaRev As DataRow
            filaRev = dtTipoRev.NewRow
            filaRev("TipoRev") = "01"
            filaRev("TipoRevDes") = "Digital"
            dtTipoRev.Rows.Add(filaRev)
            filaRev = dtTipoRev.NewRow
            filaRev("TipoRev") = "02"
            filaRev("TipoRevDes") = "Impresa"
            dtTipoRev.Rows.Add(filaRev)

            filaRev = dtTipoRev.NewRow
            filaRev("TipoRev") = "03"
            filaRev("TipoRevDes") = "Ambas"
            dtTipoRev.Rows.Add(filaRev)


            ddlTipoRevista.DataSource = dtTipoRev
            ddlTipoRevista.DataValueField = "TipoRev"
            ddlTipoRevista.DataTextField = "TipoRevDes"
            ddlTipoRevista.DataBind()



            Dim fila As DataRow
            fila = dtPersonas.NewRow
            fila("Tipo") = "F"
            fila("TipoDes") = "Física"
            dtPersonas.Rows.Add(fila)


            fila = dtPersonas.NewRow
            fila("Tipo") = "M"
            fila("TipoDes") = "Moral"
            dtPersonas.Rows.Add(fila)

            ddlTipoPersona.DataSource = dtPersonas
            ddlTipoPersona.DataTextField = "TipoDes"
            ddlTipoPersona.DataValueField = "Tipo"
            ddlTipoPersona.DataBind()

            ddlTipoPersona.SelectedValue = "F"
            pnlFisica.Visible = True

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
            ddlPais.SelectedValue = "0"

            ''Cargamos los estados
            ssql = objdatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
            fila = dtEstado.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtEstado.Rows.Add(fila)
            ddlEstado.DataSource = dtEstado
            ddlEstado.DataTextField = "Descripcion"
            ddlEstado.DataValueField = "Clave"
            ddlEstado.DataBind()
            ddlEstado.SelectedValue = "0"

            ''En base a la configuración, determinamos que tipo de SN
            ssql = "select ISNULL(cvTipoClientes,'C') as TipoCliente from config.Parametrizaciones "
            Dim dtTipo As New DataTable
            dtTipo = objdatos.fnEjecutarConsulta(ssql)
            If dtTipo.Rows.Count > 0 Then
                If dtTipo.Rows(0)(0) = "C" Then
                    lblTitulo.Text = "AGREGAR CLIENTE"
                Else
                    lblTitulo.Text = "AGREGAR PROSPECTO"
                End If
            Else
                lblTitulo.Text = "SIN DEFINIR"
            End If


            ''La especialidad
            ssql = "select IndCode as Code,IndDesc as Especialidad from OOND"
            ssql = "select Code,Name as Especialidad from [@PROFESIONES] "
            Dim dtEspecialidad As New DataTable
            dtEspecialidad = objdatos.fnEjecutarConsultaSAP(ssql)
            ddlEspecialidad.DataSource = dtEspecialidad
            ddlEspecialidad.DataTextField = "Especialidad"
            ddlEspecialidad.DataValueField = "Code"
            ddlEspecialidad.DataBind()


        End If
    End Sub
    Protected Sub ddlTipoPersona_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoPersona.SelectedIndexChanged
        Try

            If ddlTipoPersona.SelectedValue = "F" Then
                pnlFisica.Visible = True
                pnlMoral.Visible = False
            Else
                pnlFisica.Visible = False
                pnlMoral.Visible = True
            End If


        Catch ex As Exception

        End Try
    End Sub

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub

    Protected Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''Realizamos algunas validaciones
        Dim javaScript As String = "<script language='JavaScript'> alert('Button Click client-side'); </script>"

        Dim message As String
        If chkTerminos.Checked = False Then
            message = "alert('Debe aceptar aviso de privacidad');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe aceptar aviso de privacidad")

            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub


        End If

        If txtCorreo.Text <> txtCorreoConfirma.Text Then
            message = "alert('Las cuentas de correo no coinciden');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Las cuentas de correo no coinciden")


            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub


        End If


        If txtRazonSocial.Text = "" And pnlMoral.Visible = True Then
            ' Response.Write(sComando)
            'objdatos.Mensaje("Debe indicar razón social", Me.Page)
            'Exit Sub
            message = "alert('Debe indicar razón social');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar razón social")

            'ScriptManager.RegisterClientScriptBlock(Me, Page.GetType, "Validation", javaScript, False)

            'ScriptManager.RegisterClientScriptBlock(ResultsUpdatePanel, Me.GetType(), "alert", message, True)

            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        End If
        'If txtrfc.Text = "" Then
        '    'objdatos.Mensaje("Debe indicar rfc", Me.Page)
        '    message = "alert('Debe indicar rfc');"
        '    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
        '    Exit Sub
        'End If

        If txtrfc.Text = "" Then
            txtrfc.Text = "XAXX010101000"
        End If


        If txtTelefono.Text = "" Then
            message = "alert('Debe proporcionar un número de teléfono');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe proporcionar un número de teléfono")
            '  ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub

        End If

        If txtCalle.Text = "" Then
            message = "alert('Debe indicar la calle');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la calle")
            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("CalleEnvio") = txtCalle.Text
        End If
        'If txtColonia.Text = "" Then
        '    message = "alert('Debe indicar la colonia');"
        '    CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la colonia")
        '    '            ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
        '    Exit Sub
        'Else
        '    Session("ColoniaEnvio") = txtColonia.Text
        'End If

        If txtColonia.Text <> "" Then
            Session("ColoniaEnvio") = txtColonia.Text
        Else
            Session("ColoniaEnvio") = ddlColonia.SelectedItem.Text
        End If
        If txtCiudad.Text = "" Then
            message = "alert('Debe indicar la ciudad');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la ciudad")
            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("CiudadEnvio") = txtCiudad.Text
        End If

        If txtLocalidad.Text = "" Then
            message = "alert('Debe indicar la localidad/municipio');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la localidad/municipio")
            ' ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("MunicipioEnvio") = txtLocalidad.Text
        End If

        If txtNumExt.Text = "" Then
            message = "alert('Debe indicar número exterior');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar número exterior")
            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("NumExtEnvio") = txtNumExt.Text
        End If
        If ddlEstado.SelectedItem.Text = "" Then
            message = "alert('Debe indicar el estado');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el estado")
            ' ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("EstadoEnvio") = ddlEstado.SelectedValue
        End If
        If ddlPais.SelectedItem.Text = "" Then
            message = "alert('Debe indicar el país');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el país")
            ' ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("PaisEnvio") = ddlPais.SelectedValue
        End If

        If txtCP.Text = "" Then
            message = "alert('Debe indicar el CP');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el CP")
            '  ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("CPEnvio") = txtCP.Text
        End If

        ssql = "select U_CP from [@COMP_EXT_CP] WHERE U_CP=" & "'" & txtCP.Text & "'"
        Dim dtCP As New DataTable
        dtCP = objdatos.fnEjecutarConsultaSAP(ssql)
        If dtCP.Rows.Count = 0 Then
            message = "alert('El CP que ha proporcionado no es válido');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El CP que ha proporcionado no es válido")
            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        End If

        If txtCorreo.Text = "" Then
            message = "alert('Debe indicar un correo electrónico');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar un correo electrónico")
            '  ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else

            ''Primero que el email no exista
            ssql = "select * from OCRD WHERE ISNULL(E_Mail,'')=" & "'" & txtCorreo.Text & "'"
            Dim dtCorreo As New DataTable
            dtCorreo = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtCorreo.Rows.Count > 0 Then
                message = "alert('El correo electrónico ya se encuentra registrado en nuestra base de datos');"
                '   CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El correo electrónico ya se encuentra registrado en nuestra base de datos")
                'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
                Session("ClienteCorreoExiste") = dtCorreo.Rows(0)("CardCode")
                ' Exit Sub
            End If
        End If
        Dim iResultado As Int16 = 1


        ''Ingresamos los valores a SAP
        Dim oProspecto As SAPbobsCOM.BusinessPartners
        Dim oCompany As New SAPbobsCOM.Company
        Try
            ''Obtenemos la configuración de como dar de alta el cliente
            ssql = "select ISNULL(cvTipoClientes,'C') as TipoCliente from config.Parametrizaciones "
            Dim dtTipo As New DataTable
            dtTipo = objdatos.fnEjecutarConsulta(ssql)

            ''Serie,grupo y lista de precios
            Dim iSerie As Int64 = 0
            Dim iLista As Int64 = 0
            Dim sGrupo As String = ""

            ssql = " SELECT ISNULL(cvUsaSerie ,'') as UsaSerie,ISNULL(ciSerie,0) as Serie,ISNULL(cvGrupo,'') as Grupo,ISNULL(ciListaPrecios ,0) as Lista from config.ParametrizacionesCliente "
            Dim dtParam As New DataTable
            dtParam = objdatos.fnEjecutarConsulta(ssql)
            If dtParam.Rows.Count > 0 Then
                If dtParam.Rows(0)("UsaSerie") <> "" Then
                    iSerie = dtParam.Rows(0)("Serie")
                End If
                If dtParam.Rows(0)("Grupo") <> "" Then
                    sGrupo = dtParam.Rows(0)("Grupo")
                End If
                If dtParam.Rows(0)("Lista") <> "0" Then
                    iLista = dtParam.Rows(0)("Lista")
                End If
            End If

            Dim sOrigen As String = ""
            If Request.QueryString("Origen") <> "" Then
                sOrigen = Request.QueryString("Origen")
            End If

            If Request.QueryString("origen") <> "" And sOrigen <> "" Then
                sOrigen = Request.QueryString("origen")
            End If


            oCompany = objdatos.fnConexion_SAP
            Dim sCardCode As String = "Lid-1"
            If oCompany.Connected Then
                objdatos.fnLog("Cliente", "Si Conecta")
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)

                If pnlMoral.Visible = True Then
                    oProspecto.CardName = txtRazonSocial.Text.ToUpper
                Else
                    oProspecto.CardName = txtappaterno.Text.ToUpper & " " & txtapmaterno.Text.ToUpper & " " & txtnombre.Text.ToUpper


                End If



                oProspecto.CardType = SAPbobsCOM.BoCardTypes.cLid


                If iSerie <> 0 Then
                    If Session("ClienteCorreoExiste") = "" Then
                        oProspecto.Series = iSerie
                        oProspecto.EmailAddress = txtCorreo.Text
                    Else
                        oProspecto.GetByKey(Session("ClienteCorreoExiste"))



                    End If

                Else
                    oProspecto.CardCode = sCardCode
                End If

                oProspecto.FederalTaxID = txtrfc.Text

                If pnlMoral.Visible = False Then
                    oProspecto.UserFields.Fields.Item("U_FNombre").Value = txtnombre.Text.ToUpper
                    oProspecto.UserFields.Fields.Item("U_FAPaterno").Value = txtappaterno.Text.ToUpper
                    oProspecto.UserFields.Fields.Item("U_FAMaterno").Value = txtapmaterno.Text.ToUpper
                End If

                If sGrupo <> "" Then
                    oProspecto.GroupCode = sGrupo
                End If
                If iLista <> 0 Then
                    oProspecto.PriceListNum = iLista
                End If



                Try
                    oProspecto.Phone1 = txtTelefono.Text
                Catch ex As Exception

                End Try

                If txtrfc.Text.Length = 12 Then
                    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cCompany
                Else
                    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cPrivate
                End If

                ''Especialidad
                Try
                    'oProspecto.UserFields.Fields.Item("U_DMS0005").Value = ddlEspecialidad.Text.ToUpper
                    If txtOtro.Text <> "" Then
                        oProspecto.UserFields.Fields.Item("U_DMS0005").Value = txtOtro.Text.ToUpper
                        oProspecto.UserFields.Fields.Item("U_DMS0008").Value = ddlEspecialidad.SelectedValue
                    Else
                        oProspecto.UserFields.Fields.Item("U_DMS0008").Value = ddlEspecialidad.SelectedValue
                    End If

                Catch ex As Exception

                End Try


                Try
                    ''Revista
                    If chkRevista.Checked Then
                        oProspecto.Properties(63) = SAPbobsCOM.BoYesNoEnum.tYES
                        oProspecto.Properties(62) = SAPbobsCOM.BoYesNoEnum.tNO
                        oProspecto.UserFields.Fields.Item("U_DMS0004").Value = ddlTipoRevista.SelectedValue
                    Else
                        oProspecto.Properties(62) = SAPbobsCOM.BoYesNoEnum.tYES
                        oProspecto.Properties(63) = SAPbobsCOM.BoYesNoEnum.tNO
                    End If
                Catch ex As Exception

                End Try

                Try
                    ''Origen

                    oProspecto.UserFields.Fields.Item("U_DMS0006").Value = sOrigen

                Catch ex As Exception

                End Try



                Try
                    ''Recibir más información  propiedad64
                    If chkRecibir.Checked Then
                        oProspecto.Properties(64) = SAPbobsCOM.BoYesNoEnum.tYES
                    End If
                Catch ex As Exception

                End Try

                Dim sColonia As String = ""

                If txtColonia.Text <> "" Then
                    sColonia = txtColonia.Text
                Else
                    sColonia = ddlColonia.Text
                End If


                ''Direccion fiscal

                oProspecto.Addresses.SetCurrentLine(0)
                oProspecto.Addresses.TypeOfAddress = "B"
                oProspecto.Addresses.AddressName = "Entrega"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                oProspecto.Addresses.Block = sColonia
                oProspecto.Addresses.City = txtCiudad.Text.ToUpper
                oProspecto.Addresses.County = txtLocalidad.Text.ToUpper
                oProspecto.Addresses.State = ddlEstado.SelectedValue
                oProspecto.Addresses.Street = txtCalle.Text.ToUpper
                oProspecto.Addresses.ZipCode = txtCP.Text.ToUpper

                If txtNumInt.Text <> "" Then
                    oProspecto.Addresses.StreetNo = txtNumExt.Text & "-" & txtNumInt.Text
                Else
                    oProspecto.Addresses.StreetNo = txtNumExt.Text
                End If



                If Session("ClienteCorreoExiste") <> "" Then
                    oProspecto.Addresses.SetCurrentLine(1)
                Else
                    oProspecto.Addresses.Add()
                    oProspecto.Addresses.AddressName = "Facturación"
                    oProspecto.Addresses.TypeOfAddress = "S"

                    oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                End If


                oProspecto.Addresses.Block = sColonia
                oProspecto.Addresses.City = txtCiudad.Text.ToUpper
                oProspecto.Addresses.County = txtLocalidad.Text.ToUpper
                oProspecto.Addresses.State = ddlEstado.SelectedValue
                oProspecto.Addresses.Street = txtCalle.Text.ToUpper
                oProspecto.Addresses.ZipCode = txtCP.Text.ToUpper
                If txtNumInt.Text <> "" Then
                    oProspecto.Addresses.StreetNo = txtNumExt.Text.ToUpper & "-" & txtNumInt.Text.ToUpper
                Else
                    oProspecto.Addresses.StreetNo = txtNumExt.Text.ToUpper
                End If


                Try
                    oProspecto.UserFields.Fields.Item("U_Email").Value = "3"
                    oProspecto.UserFields.Fields.Item("U_DocEnviar").Value = "2"
                Catch ex As Exception

                End Try

                Try
                    '  oProspecto.Industry = ddlEspecialidad.SelectedValue
                Catch ex As Exception

                End Try

                If Session("ClienteCorreoExiste") = "" Then
                    iResultado = oProspecto.Add

                Else
                    iResultado = oProspecto.Update
                End If



                If iResultado <> 0 Then

                    objdatos.fnLog("Cliente Zeyco", oCompany.GetLastErrorDescription.Replace("'", ""))
                    message = "alert('Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription & "');"
                    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    ' objdatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)

                Else
                    Dim stipoRegistro As String = ""

                    If Session("ClienteCorreoExiste") = "" Then
                        message = "alert('Registrado');"
                        ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    Else
                        stipoRegistro = "Actualización"
                        'ssql = "UPDATE OCRD set LicTradNum=" & "'" & txtrfc.Text & "' where cardCode=" & "'" & Session("ClienteCorreoExiste") & "'"
                        'objdatos.fnEjecutarInsertSAP(ssql)

                        message = "alert('Actualizado!');"
                        ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    End If



                    objdatos.fnLog("EnvioCorreo origen:", CStr(Session("OrigenAlta")).ToUpper)
                    objdatos.fnLog("EnvioCorreo tipo registro:", stipoRegistro)
                    Try
                        Dim sNombreComercial As String = "Zeyco"
                        Dim text As String = ""
                        If CStr(Session("OrigenAlta")) <> "" Then
                            If CStr(Session("OrigenAlta")).ToUpper = "ZEYCO" Then
                                If stipoRegistro = "Actualización" Then
                                    text = MensajeHTML(Server.MapPath("~") & "\ActualizaUsuario.html")
                                Else
                                    text = MensajeHTML(Server.MapPath("~") & "\nuevoUsuario.html")
                                End If



                            End If
                            If CStr(Session("OrigenAlta")).ToUpper.Contains("DENTAL") Then
                                objdatos.fnLog("EnvioCorreo ", "nuevoUsuarioDENTAL")
                                text = MensajeHTML(Server.MapPath("~") & "\nuevoUsuarioDENTAL.html")
                            Else
                                If stipoRegistro = "Actualización" Then
                                    objdatos.fnLog("EnvioCorreo ", "ActualizaUsuario")
                                    text = MensajeHTML(Server.MapPath("~") & "\ActualizaUsuario.html")
                                Else
                                    text = MensajeHTML(Server.MapPath("~") & "\nuevoUsuario.html")
                                End If
                            End If

                        Else
                            If stipoRegistro = "Actualización" Then
                                objdatos.fnLog("EnvioCorreo ", "ActualizaUsuario2")
                                text = MensajeHTML(Server.MapPath("~") & "\ActualizaUsuario.html")
                            Else
                                text = MensajeHTML(Server.MapPath("~") & "\nuevoUsuario.html")
                            End If

                        End If
                        Dim sDestinatario As String = ""

                        sDestinatario = txtCorreo.Text

                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente As New DataTable
                        dtcliente = objdatos.fnEjecutarConsulta(ssql)
                        text = text.Replace("{enviara}", txtnombre.Text)

                        Dim asunto As String = "Bienvenido a "

                        If stipoRegistro = "Actualización" Then

                            asunto = "Gracias por actualizar sus datos en "

                        End If

                        If CStr(Session("OrigenAlta")).ToUpper <> "ZEYCO" And CStr(Session("OrigenAlta")).ToUpper <> "" Then
                            sNombreComercial = "Dental para Cual"
                            objdatos.fnLog("EnvioCorreo Dental para Cual", sNombreComercial)
                            ' objdatos.fnEnviarCorreo(sDestinatario, text, "Bienvenido a " & sNombreComercial & "!")
                            fnEnviarCorreoDENTAL(sDestinatario, text, asunto & sNombreComercial & "!")
                        Else

                            objdatos.fnEnviarCorreo(sDestinatario, text, asunto & sNombreComercial & "!")
                        End If


                    Catch ex As Exception

                    End Try


                    Session("Cliente") = oCompany.GetNewObjectKey
                    Session("RazonSocial") = txtRazonSocial.Text
                    txtnombre.Text = ""

                    txtappaterno.Text = ""
                    txtapmaterno.Text = ""

                    txtCorreoConfirma.Text = ""
                    txtRazonSocial.Text = ""
                    txtCorreo.Text = ""
                    txtrfc.Text = ""
                    txtCalle.Text = ""
                    txtColonia.Text = ""
                    txtNumExt.Text = ""
                    txtNumInt.Text = ""
                    txtLocalidad.Text = ""
                    txtCiudad.Text = ""
                    txtCP.Text = ""
                End If
            Else
                objdatos.fnLog("Cliente", "NO Conecta")

            End If
        Catch ex As Exception

        End Try
    End Sub


    Public Sub fnEnviarCorreoDENTAL(Destinatario As String, Body As String, asunto As String)
        Dim objDatos As New Cls_Funciones
        Try
            objDatos.fnLog("Envio de Correo DENTAL", "Inicia")
            Dim ssql As String
            ssql = "select cvServerSMTP,cvUsuario,cvPass,cvSSL,cvCorreo,cvPuerto from config.correos  "
            Dim dtConf As New DataTable

            dtConf = objDatos.fnEjecutarConsulta(ssql)

            Dim mimeType As New ContentType("text/html")
            Dim alternate As AlternateView = AlternateView.CreateAlternateViewFromString(Body, mimeType)
            objDatos.fnLog("Envio de Correo DENTAL dest", Destinatario)
            Dim Mailmsg As New System.Net.Mail.MailMessage
            Dim archivos As String()
            Dim obj As New Net.Mail.SmtpClient
            Mailmsg.From = New System.Net.Mail.MailAddress("contacto@dentalparacual.com")
            Mailmsg.To.Add(Destinatario) '
            objDatos.fnLog("Envio de Correo DENTAL asunto", asunto)
            Mailmsg.Subject = asunto
            Mailmsg.IsBodyHtml = True
            Mailmsg.Body = Body
            obj.Host = "mail.dentalparacual.com"
            obj.Credentials = New NetworkCredential("contacto@dentalparacual.com", "j4$M.r1A!")
            obj.Port = "25"

            obj.EnableSsl = False
            objDatos.fnLog("Envio de Correo DENTAL", "Antes de enviar")
            '  obj.Port = dtConf.Rows(0)("cvPuerto")
            obj.Send(Mailmsg)
            objDatos.fnLog("Envio de Correo DENTAL", "Enviado")
            Mailmsg = Nothing
        Catch ex As Exception
            objDatos.fnLog("Envio de Correo", ex.Message)
            '  MsgBox("Ha ocurrido un problema: " & ex.Message)
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
            objdatos.Mensaje(ex.Message, Me.Page)
        End Try


        Return Cuerpo
    End Function

    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            Dim fila As DataRow
            ssql = objdatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
            fila = dtEstado.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtEstado.Rows.Add(fila)
            ddlEstado.DataSource = dtEstado
            ddlEstado.DataTextField = "Descripcion"
            ddlEstado.DataValueField = "Clave"
            ddlEstado.DataBind()
            ddlEstado.SelectedValue = "0"

        Catch ex As Exception

        End Try
    End Sub
    Protected Sub chkRevista_CheckedChanged(sender As Object, e As EventArgs) Handles chkRevista.CheckedChanged
        If chkRevista.Checked = True Then
            pnlRevista.Visible = True
        Else
            pnlRevista.Visible = False
        End If
    End Sub

    Private Sub ddlEspecialidad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEspecialidad.SelectedIndexChanged
        Try
            objdatos.fnLog("Esp", ddlEspecialidad.SelectedItem.Text.ToUpper)
            If ddlEspecialidad.SelectedItem.Text.ToUpper = "OTRO" Then
                pnlOtro.Visible = True
                txtOtro.Text = ""
            Else
                pnlOtro.Visible = False
                txtOtro.Text = ""
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub txtCorreoConfirma_TextChanged(sender As Object, e As EventArgs) Handles txtCorreoConfirma.TextChanged
        Try
            ''Una vez que se confirma el correo, buscamos sino se encuentra en la base de datos
            Session("ClienteCorreoExiste") = ""
            If txtCorreoConfirma.Text <> "" Then
                If txtCorreoConfirma.Text = txtCorreo.Text Then
                    ssql = objdatos.fnObtenerQuery("Consultacorreo")
                    ssql = ssql.Replace("[%0]", txtCorreo.Text)
                    Dim dtExiste As New DataTable
                    dtExiste = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtExiste.Rows.Count > 0 Then
                        ''Si tenemos datos, es un correo previamente registrado.
                        ''Llenamos en variables de session
                        Session("ClienteCorreoExiste") = dtExiste.Rows(0)("CardCode")
                        ' objdatos.Mensaje("La cuenta de correo que ha proporcionado ya se encuentra registrada. Los datos que ingrese, serán actualizados", Me.Page)
                        Dim Message = "alert('La cuenta de correo que ha proporcionado ya se encuentra registrada. Los datos que ingrese, serán actualizados');"
                        CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "La cuenta de correo que ha proporcionado ya se encuentra registrada. Los datos que ingrese, serán actualizados")
                        btnEntrar.Text = "Actualizar"

                        ''Cargamos la demas informacion
                        Dim sNombre As String()
                        Dim nombre As String = ""
                        Dim sApellidop As String = ""
                        Dim sApellidom As String = ""

                        sNombre = CStr(dtExiste.Rows(0)("CardName")).Split(" ")
                        If sNombre.Count > 1 Then
                            nombre = sNombre(0)
                            sApellidop = sNombre(1)

                            If sNombre.Count > 2 Then
                                If sApellidop = "" Then
                                    sApellidop = sNombre(2)
                                Else
                                    sApellidom = sNombre(2)
                                End If

                            End If
                        End If

                        txtnombre.Text = nombre
                        txtappaterno.Text = sApellidop
                        txtapmaterno.Text = sApellidom

                        If CStr(dtExiste.Rows(0)("NombreP")) <> "" Then

                            txtnombre.Text = CStr(dtExiste.Rows(0)("NombreP"))
                            txtappaterno.Text = CStr(dtExiste.Rows(0)("ApellidoP"))
                            txtapmaterno.Text = CStr(dtExiste.Rows(0)("ApellidoM"))

                        End If


                        Try
                            ddlTipoRevista.SelectedValue = dtExiste.Rows(0)("U_DMS0004")
                            If CStr(dtExiste.Rows(0)("U_DMS0004")) <> "" Then
                                chkRevista.Visible = True
                                chkRevista_CheckedChanged(sender, e)
                            End If
                        Catch ex As Exception

                        End Try

                        Try
                            ''Revisamos la especialidad
                            ssql = "select IndCode as Code,IndDesc as Especialidad from OOND where IndDesc=" & "'" & dtExiste.Rows(0)("U_DMS0005") & "'"
                            ssql = "select Code,Name from [@PROFESIONES] WHERE code=" & "'" & dtExiste.Rows(0)("U_DMS0007") & "'"
                            Dim dtEspecialidad As New DataTable
                            dtEspecialidad = objdatos.fnEjecutarConsultaSAP(ssql)
                            If dtEspecialidad.Rows.Count > 0 Then
                                ddlEspecialidad.SelectedValue = dtEspecialidad.Rows(0)(0)
                                If ddlEspecialidad.SelectedItem.Text.ToUpper.Contains("OTRO") Then
                                    pnlOtro.Visible = True
                                    txtOtro.Visible = True
                                    txtOtro.Text = dtExiste.Rows(0)("U_DMS0005")
                                End If
                            Else
                                ssql = "select IndCode as Code,IndDesc as Especialidad from OOND where IndCode=" & "'" & dtExiste.Rows(0)("U_DMS0005") & "'"
                                ssql = "select Code,Name from [@PROFESIONES] WHERE code=" & "'" & dtExiste.Rows(0)("U_DMS0008") & "'"
                                dtEspecialidad = New DataTable
                                dtEspecialidad = objdatos.fnEjecutarConsultaSAP(ssql)
                                If dtEspecialidad.Rows.Count > 0 Then
                                    ddlEspecialidad.SelectedValue = dtEspecialidad.Rows(0)(0)
                                    If ddlEspecialidad.SelectedItem.Text.ToUpper.Contains("OTRO") Then
                                        pnlOtro.Visible = True
                                        txtOtro.Visible = True
                                        txtOtro.Text = dtExiste.Rows(0)("U_DMS0005")
                                    End If
                                Else
                                    pnlOtro.Visible = True
                                    ddlEspecialidad.SelectedValue = "11"
                                    txtOtro.Visible = True
                                    txtOtro.Text = dtExiste.Rows(0)("U_DMS0005")
                                End If


                            End If
                        Catch ex As Exception

                        End Try

                        Dim sNumExt As String()

                        Try
                            If CStr(dtExiste.Rows(0)("StreetNo")).Contains("-") Then
                                sNumExt = CStr(dtExiste.Rows(0)("StreetNo")).Split("-")
                                txtNumExt.Text = sNumExt(0)
                                txtNumInt.Text = sNumExt(1)
                            Else
                                txtNumExt.Text = CStr(dtExiste.Rows(0)("StreetNo"))
                            End If

                        Catch ex As Exception

                        End Try
                        Try
                            txtCalle.Text = dtExiste.Rows(0)("Address")
                            txtCP.Text = dtExiste.Rows(0)("ZipCode")
                            txtCiudad.Text = dtExiste.Rows(0)("City")
                            txtTelefono.Text = dtExiste.Rows(0)("Phone1")
                        Catch ex As Exception

                        End Try

                        Try
                            fnLlenacolonias(txtCP.Text)
                            ddlColonia.SelectedValue = dtExiste.Rows(0)("Block")
                        Catch ex As Exception
                            txtColonia.Text = dtExiste.Rows(0)("Block")
                        End Try

                        Try
                            txtLocalidad.Text = dtExiste.Rows(0)("County")
                            ddlPais.SelectedValue = dtExiste.Rows(0)("Country")
                            txtrfc.Text = dtExiste.Rows(0)("LicTradNum")
                            If txtrfc.Text.Length = 12 Then
                                ddlTipoPersona.SelectedValue = "M"
                                txtRazonSocial.Text = CStr(dtExiste.Rows(0)("CardName"))
                            Else
                                ddlTipoPersona.SelectedValue = "F"
                            End If


                        Catch ex As Exception

                        End Try

                        ''Cargamos los estados
                        ssql = objdatos.fnObtenerQuery("Estados")
                        ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
                        Dim dtEstado As New DataTable
                        Dim fila As DataRow
                        dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
                        fila = dtEstado.NewRow
                        fila("Clave") = "0"
                        fila("Descripcion") = "-Seleccione-"
                        dtEstado.Rows.Add(fila)
                        ddlEstado.DataSource = dtEstado
                        ddlEstado.DataTextField = "Descripcion"
                        ddlEstado.DataValueField = "Clave"
                        ddlEstado.DataBind()
                        Try
                            ddlEstado.SelectedValue = dtExiste.Rows(0)("state1")
                        Catch ex As Exception

                        End Try


                    End If
                End If
            End If
        Catch ex As Exception
            objdatos.fnLog("load agregar cliente", "ex: " & ex.Message.Replace("'", ""))
        End Try
    End Sub

    Private Sub txtCP_TextChanged(sender As Object, e As EventArgs) Handles txtCP.TextChanged
        Try
            objdatos.fnLog("CP", "evento")
            Dim CP As String = txtCP.Text
            Dim dtColonias As New DataTable
            dtColonias.Columns.Add("Codigo")
            dtColonias.Columns.Add("Descripcion")

            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            ' Dim CP = "45601"
            Dim url As String = "https://api.copomex.com/query/info_cp/" & CP & "?type=simplified&token=69cb250a-ef15-4a34-a8a3-e7bd757d3003"
            objdatos.fnLog("CP", url)
            Dim oRequest = WebRequest.Create(url)
            Dim oResponse As WebResponse = oRequest.GetResponse()
            Dim sr As StreamReader = New StreamReader(oResponse.GetResponseStream())
            Dim oDatos = JsonConvert.DeserializeObject(Of RespuestaCP)(sr.ReadToEnd().Trim())
            If oDatos.error Then
                Dim Message = "alert('El CP no es correcto:" & oDatos.error_message & "');"
                ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", Message, True)

            Else
                objdatos.fnLog("CP", "CP Correcto")
                ''Armamos el catalogo de colonias
                Dim coloniasMensaje As String = ""
                Dim fila As DataRow
                For Each colonias As String In oDatos.response.asentamiento
                    fila = dtColonias.NewRow
                    fila("Codigo") = colonias
                    fila("Descripcion") = colonias
                    dtColonias.Rows.Add(fila)

                Next
                ddlColonia.DataSource = dtColonias
                ddlColonia.DataTextField = "Descripcion"
                ddlColonia.DataValueField = "Codigo"
                ddlColonia.DataBind()
            End If

        Catch ex As Exception
            objdatos.fnLog("CP", "error ex: " & ex.Message.Replace("'", ""))
            Dim Message = "alert('El CP no es correcto, no se encuentra registrado');"
            ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", Message, True)
        End Try
    End Sub

    Public Sub fnLlenacolonias(CP As String)
        Try
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
            objdatos.fnLog("CP", "fnLlenacolonias")

            Dim dtColonias As New DataTable
            dtColonias.Columns.Add("Codigo")
            dtColonias.Columns.Add("Descripcion")
            ' Dim CP = "45601"
            Dim url As String = "https://api.copomex.com/query/info_cp/" & CP & "?type=simplified&token=69cb250a-ef15-4a34-a8a3-e7bd757d3003"
            objdatos.fnLog("CP", url)
            Dim oRequest = WebRequest.Create(url)
            Dim oResponse As WebResponse = oRequest.GetResponse()
            Dim sr As StreamReader = New StreamReader(oResponse.GetResponseStream())
            Dim oDatos = JsonConvert.DeserializeObject(Of RespuestaCP)(sr.ReadToEnd().Trim())
            If oDatos.error Then


            Else
                objdatos.fnLog("CP", "CP Correcto")
                ''Armamos el catalogo de colonias
                Dim coloniasMensaje As String = ""
                Dim fila As DataRow
                For Each colonias As String In oDatos.response.asentamiento
                    fila = dtColonias.NewRow
                    fila("Codigo") = colonias
                    fila("Descripcion") = colonias
                    dtColonias.Rows.Add(fila)

                Next
                ddlColonia.DataSource = dtColonias
                ddlColonia.DataTextField = "Descripcion"
                ddlColonia.DataValueField = "Codigo"
                ddlColonia.DataBind()
            End If

        Catch ex As Exception
            objdatos.fnLog("CP", "error ex: " & ex.Message.Replace("'", ""))
            '  Dim Message = "alert('El CP no es correcto, no se encuentra registrado');"
            '     ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", Message, True)
        End Try
    End Sub
    Public Class ResponseCP
        Public Property cp As String
        Public Property asentamiento As List(Of String)
        Public Property tipo_asentamiento As String
        Public Property municipio As String
        Public Property estado As String
        Public Property ciudad As String
        Public Property pais As String

    End Class

    Public Class RespuestaCP
        Public Property [error] As Boolean
        Public Property code_error As Integer
        Public Property error_message As Object
        Public Property response As ResponseCP
    End Class
End Class
