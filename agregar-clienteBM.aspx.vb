Imports System.Data
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mime
Partial Class agregar_clienteBM
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub agregar_clienteBM_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            'Dim dtPersonas As New DataTable
            'dtPersonas.Columns.Add("Tipo")
            'dtPersonas.Columns.Add("TipoDes")

            'Session("OrigenAlta") = "BlackMandala"

            'If Request.QueryString("Origen") <> "" Then
            '    Session("OrigenAlta") = Request.QueryString("Origen")
            'End If

            'If Request.QueryString("origen") <> "" Then
            '    Session("OrigenAlta") = Request.QueryString("origen")
            'End If

            Dim dtAmbito As New DataTable
            'dtAmbito.Columns.Add("Code")
            'dtAmbito.Columns.Add("Descripcion")
            Dim filaAmbito As DataRow

            'filaAmbito = dtAmbito.NewRow
            'filaAmbito("Code") = "Locales a tatuados"
            'filaAmbito("Descripcion") = "Locales a tatuados"
            'dtAmbito.Rows.Add(filaAmbito)

            'filaAmbito = dtAmbito.NewRow
            'filaAmbito("Code") = "Locales a tuatuadores"
            'filaAmbito("Descripcion") = " Locales a tuatuadores"
            'dtAmbito.Rows.Add(filaAmbito)

            'filaAmbito = dtAmbito.NewRow
            'filaAmbito("Code") = "Locales a tatuadores y tatuados"
            'filaAmbito("Descripcion") = "Locales a tatuadores y tatuados"
            'dtAmbito.Rows.Add(filaAmbito)

            'filaAmbito = dtAmbito.NewRow
            'filaAmbito("Code") = "Nacionales a tatuados"
            'filaAmbito("Descripcion") = "Nacionales a tatuados"
            'dtAmbito.Rows.Add(filaAmbito)

            'filaAmbito = dtAmbito.NewRow
            'filaAmbito("Code") = "Nacionales a tuatuadores"
            'filaAmbito("Descripcion") = "Nacionales a tuatuadores"
            'dtAmbito.Rows.Add(filaAmbito)

            'filaAmbito = dtAmbito.NewRow
            'filaAmbito("Code") = "Nacionales a tatuadors y tatuados"
            'filaAmbito("Descripcion") = " Nacionales a tatuadors y tatuados"
            'dtAmbito.Rows.Add(filaAmbito)

            ssql = "select FldValue as Code ,Descr as Descripcion  from UFD1 where TableID ='OCRD' and FieldID =(select FieldId from CUFD where TableID ='OCRD' and AliasID ='FBM_AV')"
            dtAmbito = objdatos.fnEjecutarConsultaSAP(ssql)


            filaAmbito = dtAmbito.NewRow
            filaAmbito("Code") = "-Seleccione-"
            filaAmbito("Descripcion") = "-Seleccione-"
            dtAmbito.Rows.Add(filaAmbito)

            ddlAmbitoVentas.DataSource = dtAmbito
            ddlAmbitoVentas.DataTextField = "Descripcion"
            ddlAmbitoVentas.DataValueField = "Code"
            ddlAmbitoVentas.DataBind()
            ddlAmbitoVentas.SelectedValue = "-Seleccione-"


            'pnlFisica.Visible = True
            Dim fila As DataRow
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



            '''En base a la configuración, determinamos que tipo de SN
            'ssql = "select ISNULL(cvTipoClientes,'C') as TipoCliente from config.Parametrizaciones "
            'Dim dtTipo As New DataTable
            'dtTipo = objdatos.fnEjecutarConsulta(ssql)
            'If dtTipo.Rows.Count > 0 Then
            '    If dtTipo.Rows(0)(0) = "C" Then
            '        lblTitulo.Text = "AGREGAR CLIENTE"
            '    Else
            '        lblTitulo.Text = "AGREGAR PROSPECTO"
            '    End If
            'Else
            '    lblTitulo.Text = "SIN DEFINIR"
            'End If




        End If
    End Sub

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

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

    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        Dim javaScript As String = "<script language='JavaScript'> alert('Button Click client-side'); </script>"
        Dim iResultado As Int16 = 1

        Dim message As String
        If chkTerminos.Checked = False Then
            message = "alert('Debe aceptar aviso de privacidad');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe aceptar aviso de privacidad")

            'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub


        End If


        If txtnombre.Text = "" Then
            message = "alert('Debe proporcionar su nombre');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe proporcionar su nombre")
            '  ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub

        End If

        If txtappaterno.Text = "" Then
            message = "alert('Debe proporcionar su apellido paterno');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe proporcionar su apellido paterno")
            '  ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub

        End If



        If txtTelefono.Text = "" Then
            message = "alert('Debe proporcionar un número de cellar');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe proporcionar un número de celular")
            '  ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub

        End If






        If ddlPais.SelectedItem.Text = "" Then
            message = "alert('Debe indicar el país');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el país")
            ' ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        Else
            Session("PaisEnvio") = ddlPais.SelectedValue
        End If

        Session("ClienteCorreoExiste") = ""
        objdatos.fnLog("Cliente existe bm-inicia", Session("ClienteCorreoExiste"))

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
                ' CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El correo electrónico ya se encuentra registrado en nuestra base de datos")
                'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
                ' Exit Sub
                Session("ClienteCorreoExiste") = dtCorreo.Rows(0)("CardCode")
                objdatos.fnLog("Cliente existe bm-si encontró", Session("ClienteCorreoExiste"))
            End If
        End If



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

            oCompany = objdatos.fnConexion_SAP
            Dim sCardCode As String = "Lid-1"
            If oCompany.Connected Then
                objdatos.fnLog("Cliente", "Si Conecta")
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)



                oProspecto.CardName = txtappaterno.Text.ToUpper & " " & txtapmaterno.Text.ToUpper & " " & txtnombre.Text.ToUpper

                oProspecto.CardType = SAPbobsCOM.BoCardTypes.cLid


                If iSerie <> 0 Then

                Else
                    oProspecto.CardCode = sCardCode
                End If

                If Session("ClienteCorreoExiste") = "" Then

                    oProspecto.Series = 131
                    oProspecto.EmailAddress = txtCorreo.Text
                    ''Por default el Grupo 113
                    oProspecto.GroupCode = "119"


                    If iLista <> 0 Then
                        oProspecto.PriceListNum = iLista
                    End If

                Else
                    oProspecto.GetByKey(Session("ClienteCorreoExiste"))
                End If


                Try
                    oProspecto.Phone1 = txtTelefono.Text
                Catch ex As Exception

                End Try


                Try
                    ''Origen

                    oProspecto.UserFields.Fields.Item("U_DMS0006").Value = Session("OrigenAlta")

                Catch ex As Exception

                End Try


                ''Direccion fiscal

                oProspecto.Addresses.SetCurrentLine(0)
                oProspecto.Addresses.TypeOfAddress = "B"
                oProspecto.Addresses.AddressName = "Entrega"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                oProspecto.Addresses.Country = ddlPais.SelectedValue

                oProspecto.FederalTaxID = "XAXX010101000"

                ''Ponemos la misma en envio
                If Session("ClienteCorreoExiste") <> "" Then
                    oProspecto.Addresses.SetCurrentLine(1)
                Else
                    oProspecto.Addresses.Add()
                    oProspecto.Addresses.AddressName = "Facturación"
                    oProspecto.Addresses.TypeOfAddress = "S"

                    oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                End If

                Try
                    oProspecto.UserFields.Fields.Item("U_Email").Value = "3"
                    oProspecto.UserFields.Fields.Item("U_DocEnviar").Value = "2"

                    oProspecto.UserFields.Fields.Item("U_FBM_CCS").Value = txtCantClientes.Text
                    oProspecto.UserFields.Fields.Item("U_FBM_VI").Value = txtArtistasVende.Text
                    oProspecto.UserFields.Fields.Item("U_FBM_AV").Value = ddlAmbitoVentas.SelectedValue
                Catch ex As Exception
                    objdatos.fnLog("Agregar cliente BM", "UDF 1 " & ex.Message.Replace("'", ""))
                End Try

                Try
                    oProspecto.UserFields.Fields.Item("U_FBM_F").Value = txtFacebook.Text
                    oProspecto.UserFields.Fields.Item("U_FBM_I").Value = txtInstagram.Text
                    oProspecto.UserFields.Fields.Item("U_FBM_T").Value = txtTwitter.Text
                Catch ex As Exception
                    objdatos.fnLog("Agregar cliente BM", "UDF 2 " & ex.Message.Replace("'", ""))
                End Try

                Try
                    oProspecto.UserFields.Fields.Item("U_FAPaterno").Value = txtappaterno.Text
                    oProspecto.UserFields.Fields.Item("U_FAMaterno").Value = txtapmaterno.Text
                    oProspecto.UserFields.Fields.Item("U_FNombre").Value = txtnombre.Text
                Catch ex As Exception
                    objdatos.fnLog("Agregar cliente BM", "UDF 3 " & ex.Message.Replace("'", ""))
                End Try



                oProspecto.Website = txtSitioweb.Text
                oProspecto.Cellular = txtTelefono.Text


                If Session("ClienteCorreoExiste") = "" Then
                    iResultado = oProspecto.Add

                Else
                    iResultado = oProspecto.Update
                End If



                If iResultado <> 0 Then
                    objdatos.fnLog("Cliente BM", oCompany.GetLastErrorDescription.Replace("'", ""))
                    message = "alert('Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription & "');"
                    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    ' objdatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)

                Else
                    If Session("ClienteCorreoExiste") = "" Then
                        message = "alert('Registrado');"
                        ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    Else
                        message = "alert('Actualizado!');"
                        ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    End If




                    Try
                        Dim sNombreComercial As String = "Black Mandala"
                        Dim text As String = ""

                        text = MensajeHTML(Server.MapPath("~") & "\nuevoUsuarioBlackM.html")
                        Dim sDestinatario As String = ""

                        sDestinatario = txtCorreo.Text

                        ''Obtenemos el nombre de la empresa
                        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                        Dim dtcliente As New DataTable
                        dtcliente = objdatos.fnEjecutarConsulta(ssql)
                        text = text.Replace("{enviara}", txtnombre.Text)

                        fnEnviarCorreo(sDestinatario, text, "Bienvenido a " & sNombreComercial & "!")



                    Catch ex As Exception

                    End Try


                    Session("Cliente") = oCompany.GetNewObjectKey

                    txtnombre.Text = ""

                    txtappaterno.Text = ""
                    txtapmaterno.Text = ""
                    txtTelefono.Text = ""

                    txtCorreo.Text = ""
                    txtArtistasVende.Text = ""
                    txtCantClientes.Text = ""
                    txtFacebook.Text = ""
                    txtTwitter.Text = ""
                    txtInstagram.Text = ""
                    txtSitioweb.Text = ""
                    ddlAmbitoVentas.SelectedValue = "-Seleccione-"
                End If
            Else
                objdatos.fnLog("Cliente", "NO Conecta")

            End If
        Catch ex As Exception

        End Try
    End Sub
    Public Sub fnEnviarCorreo(Destinatario As String, Body As String, asunto As String)
        Try


            Dim dtConf As New DataTable


            Dim mimeType As New ContentType("text/html")
            Dim alternate As AlternateView = AlternateView.CreateAlternateViewFromString(Body, mimeType)

            Dim Mailmsg As New System.Net.Mail.MailMessage
            Dim archivos As String()
            Dim obj As New Net.Mail.SmtpClient
            Mailmsg.From = New System.Net.Mail.MailAddress("contacto@blackmandala.mx")
            Mailmsg.To.Add(Destinatario) '
            Mailmsg.Subject = asunto
            Mailmsg.IsBodyHtml = True
            Mailmsg.Body = Body
            obj.Host = "smtp.blackmandala.mx"
            obj.Credentials = New NetworkCredential("contacto@blackmandala.mx", "j4$M.r1A!")
            obj.Port = "25"

            obj.EnableSsl = False

            '  obj.Port = dtConf.Rows(0)("cvPuerto")
            obj.Send(Mailmsg)
            Mailmsg = Nothing
        Catch ex As Exception
            objdatos.fnLog("Envio de Correo", ex.Message)
            '  MsgBox("Ha ocurrido un problema: " & ex.Message)
        End Try

    End Sub

End Class
