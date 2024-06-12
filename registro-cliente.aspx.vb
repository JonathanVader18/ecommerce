
Imports System.Data
Imports System.Security.Cryptography

Partial Class registro_cliente
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objdatos As New Cls_Funciones
    Public Hash As String = "PERJ840518AB0"

    Private Sub registro_cliente_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
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

            Try
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
            Catch ex As Exception

            End Try



            Dim dtPaisF As New DataTable
            ssql = objdatos.fnObtenerQuery("Paises")
            dtPaisF = objdatos.fnEjecutarConsultaSAP(ssql)
            Dim filap = dtPaisF.NewRow
            filap("Clave") = "0"
            filap("Descripcion") = "-Seleccione-"
            dtPaisF.Rows.Add(filap)
            ddlPaisF.DataSource = dtPaisF
            ddlPaisF.DataTextField = "Descripcion"
            ddlPaisF.DataValueField = "Clave"
            ddlPaisF.DataBind()
            ddlPaisF.SelectedValue = "0"

            Try
                ''Cargamos los estados
                ssql = objdatos.fnObtenerQuery("Estados")
                ssql = ssql.Replace("[%0]", ddlPaisF.SelectedValue)
                Dim dtEstadof As New DataTable
                dtEstadof = objdatos.fnEjecutarConsultaSAP(ssql)
                Dim filae = dtEstadof.NewRow
                filae("Clave") = "0"
                filae("Descripcion") = "-Seleccione-"
                dtEstadof.Rows.Add(filae)
                ddlEstadoF.DataSource = dtEstadof
                ddlEstadoF.DataTextField = "Descripcion"
                ddlEstadoF.DataValueField = "Clave"
                ddlEstadoF.DataBind()
                ddlEstadoF.SelectedValue = "0"
            Catch ex As Exception

            End Try



            Dim dtUsoCFDI As New DataTable
            dtUsoCFDI.Columns.Add("Valor")
            dtUsoCFDI.Columns.Add("Descripcion")
            fila = dtUsoCFDI.NewRow
            fila("Valor") = "G01"
            fila("Descripcion") = "Adquisición de Mercancías"
            dtUsoCFDI.Rows.Add(fila)

            fila = dtUsoCFDI.NewRow
            fila("Valor") = "G03"
            fila("Descripcion") = "Gastos en general"
            dtUsoCFDI.Rows.Add(fila)

            fila = dtUsoCFDI.NewRow
            fila("Valor") = "-1"
            fila("Descripcion") = "-Seleccione-"
            dtUsoCFDI.Rows.Add(fila)

            ddlUsoCFDI.DataSource = dtUsoCFDI
            ddlUsoCFDI.DataTextField = "Descripcion"
            ddlUsoCFDI.DataValueField = "Valor"
            ddlUsoCFDI.DataBind()
            ddlUsoCFDI.SelectedValue = "-1"
        End If
    End Sub

    Private Sub chkFactura_CheckedChanged(sender As Object, e As EventArgs) Handles chkFactura.CheckedChanged
        If chkFactura.Checked Then
            pnlRFC.Visible = True
            pnlDomicilioFiscal.Visible = True
        Else
            pnlRFC.Visible = False
            pnlDomicilioFiscal.Visible = False
        End If
    End Sub

    Private Sub chkMismoDomEnvio_CheckedChanged(sender As Object, e As EventArgs) Handles chkMismoDomEnvio.CheckedChanged
        If chkMismoDomEnvio.Checked Then
            ddlPaisF.SelectedValue = ddlPais.SelectedValue
            Try
                ''Cargamos los estados
                ssql = objdatos.fnObtenerQuery("Estados")
                ssql = ssql.Replace("[%0]", ddlPaisF.SelectedValue)
                Dim dtEstadof As New DataTable
                dtEstadof = objdatos.fnEjecutarConsultaSAP(ssql)
                Dim fila = dtEstadof.NewRow
                fila("Clave") = "0"
                fila("Descripcion") = "-Seleccione-"
                dtEstadof.Rows.Add(fila)
                ddlEstadoF.DataSource = dtEstadof
                ddlEstadoF.DataTextField = "Descripcion"
                ddlEstadoF.DataValueField = "Clave"
                ddlEstadoF.DataBind()
                ddlEstadoF.SelectedValue = ddlEstado.SelectedValue
            Catch ex As Exception

            End Try

            txtCalleF.Text = txtCalle.Text
            txtCiudadF.Text = txtCiudad.Text
            txtNumExtF.Text = txtNumExt.Text
            txtNumIntF.Text = txtNumInt.Text
            txtColoniaF.Text = txtColonia.Text
            txtCPF.Text = txtCP.Text
            txtMunicipioF.Text = txtLocalidad.Text
        Else
            txtCalleF.Text = ""
            txtCiudadF.Text = ""
            txtNumExtF.Text = ""
            txtNumIntF.Text = ""
            txtColoniaF.Text = txtColonia.Text
            txtCPF.Text = ""
            txtMunicipioF.Text = ""
        End If
    End Sub

    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        Dim message As String
        objdatos.fnLog("Registro", "BtnEntrar 1")
        If chkTerminos.Checked = False Then
            message = "alert('Debe aceptar aviso de privacidad');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe aceptar aviso de privacidad")
            Exit Sub
        End If
        objdatos.fnLog("Registro", "BtnEntrar 2")
        If txtCorreo.Text <> txtCorreoConfirma.Text Then
            message = "alert('Las cuentas de correo no coinciden');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Las cuentas de correo no coinciden")
            Exit Sub
        End If
        objdatos.fnLog("Registro", "BtnEntrar 3")
        If txtCalle.Text = "" Then
            message = "alert('Debe indicar la calle');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la calle")
            Exit Sub
        Else
            Session("CalleEnvio") = txtCalle.Text
        End If
        objdatos.fnLog("Registro", "BtnEntrar 4")
        If txtColonia.Text = "" Then
            message = "alert('Debe indicar la colonia');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la colonia")
            Exit Sub
        Else
            Session("ColoniaEnvio") = txtColonia.Text
        End If

        objdatos.fnLog("Registro", "BtnEntrar 5")
        If txtCiudad.Text = "" Then
            message = "alert('Debe indicar la ciudad');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la ciudad")
            Exit Sub
        Else
            Session("CiudadEnvio") = txtCiudad.Text
        End If

        If txtLocalidad.Text = "" Then
            message = "alert('Debe indicar la localidad/municipio');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la localidad/municipio")
            Exit Sub
        Else
            Session("MunicipioEnvio") = txtLocalidad.Text
        End If

        If txtNumExt.Text = "" Then
            message = "alert('Debe indicar número exterior');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar número exterior")
            Exit Sub
        Else
            Session("NumExtEnvio") = txtNumExt.Text
        End If
        objdatos.fnLog("Registro", "BtnEntrar 6")
        If ddlEstado.SelectedValue = "0" Then
            message = "alert('Debe indicar el estado');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el estado")
            Exit Sub
        Else
            Session("EstadoEnvio") = ddlEstado.SelectedValue
        End If
        If ddlPais.SelectedItem.Text = "" Then
            message = "alert('Debe indicar el país');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el país")
            Exit Sub
        Else
            Session("PaisEnvio") = ddlPais.SelectedValue
        End If
        objdatos.fnLog("Registro", "BtnEntrar 7")
        If txtCP.Text = "" Then
            message = "alert('Debe indicar el CP');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el CP")
            Exit Sub
        Else
            Session("CPEnvio") = txtCP.Text
        End If

        If txtTel.Text = "" Then
            message = "alert('Debe indicar el Teléfono');"
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el teléfono")
            Exit Sub
        Else
            Session("TelefonoEnvio") = txtTel.Text
        End If

        objdatos.fnLog("Registro", "Antes de factura, todo bien")
        If chkFactura.Checked Then
            If txtrfc.Text = "" Then
                message = "alert('Debe indicar el RFC');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el RFC")
                Exit Sub
            Else
                Session("RFC") = txtrfc.Text
            End If
            If ddlUsoCFDI.SelectedItem.Text = "-1" Then
                message = "alert('Debe indicar el país');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el uso de CFDI")
                Exit Sub
            Else
                Session("UsoCFDI") = ddlUsoCFDI.SelectedValue
            End If
            If txtCalleF.Text = "" Then
                message = "alert('Debe indicar la calle');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la calle fiscal")
                Exit Sub
            Else
                Session("CalleFiscal") = txtCalleF.Text
            End If

            If txtColoniaF.Text = "" Then
                message = "alert('Debe indicar la colonia');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la colonia")
                Exit Sub
            Else
                Session("ColoniaFiscal") = txtColoniaF.Text
            End If

            If txtCiudadF.Text = "" Then
                message = "alert('Debe indicar la ciudad');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la ciudad fiscal")
                Exit Sub
            Else
                Session("CiudadFiscal") = txtCiudadF.Text
            End If

            If txtMunicipioF.Text = "" Then
                message = "alert('Debe indicar la localidad/municipio');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar la localidad/municipio fiscal")
                Exit Sub
            Else
                Session("MunicipioFiscal") = txtMunicipioF.Text
            End If

            If txtNumExtF.Text = "" Then
                message = "alert('Debe indicar número exterior');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar número exterior fiscal")
                Exit Sub
            Else
                Session("NumExtFiscal") = txtNumExtF.Text
            End If
            If ddlEstadoF.SelectedValue = "0" Then
                message = "alert('Debe indicar el estado');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el estado fiscal")
                Exit Sub
            Else
                Session("EstadoFiscal") = ddlEstadoF.SelectedValue
            End If
            If ddlPaisF.SelectedItem.Text = "" Then
                message = "alert('Debe indicar el país');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el país fiscal")
                Exit Sub
            Else
                Session("PaisFiscal") = ddlPaisF.SelectedValue
            End If

            If txtCPF.Text = "" Then
                message = "alert('Debe indicar el CP');"
                CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Debe indicar el CP fiscal")
                Exit Sub
            Else
                Session("CPFiscal") = txtCPF.Text
            End If
        End If
        objdatos.fnLog("Registro", "despues de factura, todo bien")

        ''Validamos que el correo no exista
        ssql = "SELECT * FROM Tienda.Clientes where cvMail=" & "'" & txtCorreo.Text & "' and cvEstatus='EN SAP'"
        Dim dtExisteMailSAP As New DataTable
        dtExisteMailSAP = objdatos.fnEjecutarConsulta(ssql)
        If dtExisteMailSAP.Rows.Count > 0 Then
            CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "El correo que ha proporcionado ya se encuentra registrado. Comuníquese a servicio al cliente para más información")
            Exit Sub
        End If


        ''----Inserción en la base de datos
        Dim Idcliente As Int32 = 0
        Dim sRequiereFactura As String = ""
        Dim sUsoCFDI As String = ""
        Dim sRFC As String = "XAXX010101000"
        Dim sCalleNum As String = ""
        Dim sCalleNumF As String = ""

        If txtNumInt.Text <> "" Then
            sCalleNum = txtCalle.Text & " " & txtNumExt.Text & " int " & txtNumInt.Text
        Else
            sCalleNum = txtCalle.Text & " " & txtNumExt.Text
        End If

        If txtNumIntF.Text <> "" Then
            sCalleNumF = txtCalleF.Text & " " & txtNumExtF.Text & " int " & txtNumIntF.Text
        Else
            sCalleNumF = txtCalleF.Text & " " & txtNumExtF.Text
        End If


        If chkFactura.Checked Then
            sRequiereFactura = "SI"
            sRFC = txtrfc.Text
            sUsoCFDI = ddlUsoCFDI.SelectedValue
        End If

        objdatos.fnLog("Registro", "Previo a inserción")

        ssql = "SELECT ISNULL(MAX(ciIdcliente),0) + 1 from Tienda.Clientes "
        Dim dtIdCliente As New DataTable
        dtIdCliente = objdatos.fnEjecutarConsulta(ssql)
        If dtIdCliente.Rows.Count > 0 Then
            Idcliente = dtIdCliente.Rows(0)(0)
        End If

        ''Datos generales
        ssql = " INSERT INTO Tienda.Clientes(ciIdCliente, cvNombre,cvMail,cdFechaRegistro,cvRequiereFactura,cvRFC,cvUsoCFDI,cvEstatus,cvTelefono) VALUES(" _
            & "'" & Idcliente & "'," _
            & "'" & txtnombre.Text & "'," _
            & "'" & txtCorreo.Text & "',GETDATE()," _
            & "'" & sRequiereFactura & "'," _
            & "'" & sRFC & "'," _
            & "'" & sUsoCFDI & "','POR CONFIRMAR'," _
            & "'" & txtTel.Text & "')"

        ''Domicilios
        ssql = ssql & " INSERT INTO Tienda.DireccionesCliente(ciIdCliente,cvTipoDireccion,cvCallenum,cvColonia,cvCiudad,cvCP,cvReferencia,cvMunicipio,cvEstado,cvPais) VALUES(" _
            & "'" & Idcliente & "'," _
            & "'ENVIO'," _
            & "'" & sCalleNum & "'," _
            & "'" & txtColonia.Text & "'," _
            & "'" & txtCiudad.Text & "'," _
            & "'" & txtCP.Text & "'," _
            & "'" & txtReferencia.Text & "'," _
            & "'" & txtLocalidad.Text & "'," _
            & "'" & ddlEstado.SelectedValue & "'," _
            & "'" & ddlPais.SelectedValue & "')"

        If sRequiereFactura = "SI" Then
            ''Metemos el domicilio fiscal
            ssql = ssql & " INSERT INTO Tienda.DireccionesCliente(ciIdCliente,cvTipoDireccion,cvCallenum,cvColonia,cvCiudad,cvCP,cvMunicipio,cvEstado,cvPais) VALUES(" _
         & "'" & Idcliente & "'," _
         & "'FISCAL'," _
         & "'" & sCalleNumF & "'," _
         & "'" & txtColoniaF.Text & "'," _
         & "'" & txtCiudadF.Text & "'," _
         & "'" & txtCPF.Text & "'," _
         & "'" & txtMunicipioF.Text & "'," _
         & "'" & ddlEstadoF.SelectedValue & "'," _
         & "'" & ddlPaisF.SelectedValue & "')"
        End If

        objdatos.fnEjecutarInsert(ssql)
        '  objdatos.Mensaje("Se ha registrado!, en breve recibirá un correo de confirmación", Me.Page)
        CreateMessageAlertInUpdatePanel(ResultsUpdatePanel, "Se ha registrado!, en breve recibirá un correo de confirmación")



        ''Envio de correo de confirmación
        Dim text As String = ""
        Dim sLigaCorreo As String = ""
        Dim sNombreComercial As String = "STOP Catálogo"
        ''Liga publica del sitio
        ssql = "SELECT cvLigaPublica FROM config.parametrizaciones "
        Dim dtLiga As New DataTable
        dtLiga = objdatos.fnEjecutarConsulta(ssql)
        If dtLiga.Rows.Count > 0 Then
            ''Encriptamos el correo
            Dim sCorreoEncript As String = EncriptCorreo(Idcliente)
            sLigaCorreo = dtLiga.Rows(0)(0) & "confirmacionalta.aspx?confirm=" & sCorreoEncript
        End If



        text = MensajeHTML(Server.MapPath("~") & "\AltaCliente.html")
        Dim sDestinatario As String = ""
        sDestinatario = txtCorreo.Text
        text = text.Replace("{enviara}", txtnombre.Text)
        text = text.Replace("{ligacorreo}", "<a href='" & sLigaCorreo & "' target='_blank'> Clic aquí para completar el registro</a>")


        objdatos.fnEnviarCorreo(sDestinatario, text, "Bienvenido a " & sNombreComercial & "!")

        ''Limpiamos el form, redirect
        txtTel.Text = ""
        txtrfc.Text = ""
        txtReferencia.Text = ""
        txtNumIntF.Text = ""
        txtNumInt.Text = ""
        txtNumExtF.Text = ""
        txtNumExt.Text = ""
        txtnombre.Text = ""
        txtMunicipioF.Text = ""
        txtLocalidad.Text = ""
        txtCPF.Text = ""
        txtCP.Text = ""
        txtCorreoConfirma.Text = ""
        txtCorreo.Text = ""
        txtColoniaF.Text = ""
        txtColonia.Text = ""
        txtCiudadF.Text = ""
        txtCiudad.Text = ""
        txtCalleF.Text = ""
        txtCalle.Text = ""



    End Sub

    Public Function EncriptCorreo(Contenido As String) As String
        Dim resultado As String = ""
        Try
            Dim myKey As String = "PERJ840518AB0"
            Dim des As New TripleDESCryptoServiceProvider 'Algorithmo TripleDES
            Dim hashmd5 As New MD5CryptoServiceProvider 'objeto md5
            If Trim(Contenido) = "" Then
                resultado = ""
            Else
                des.Key = hashmd5.ComputeHash((New UnicodeEncoding).GetBytes(myKey))
                des.Mode = CipherMode.ECB
                Dim encrypt As ICryptoTransform = des.CreateEncryptor()
                Dim buff() As Byte = UnicodeEncoding.ASCII.GetBytes(Contenido)
                resultado = Convert.ToBase64String(encrypt.TransformFinalBlock(buff, 0, buff.Length))
            End If
            Return resultado
        Catch __unusedException1__ As Exception
            Return "Error"
        End Try
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

    Public Sub CreateMessageAlertInUpdatePanel(ByVal up As UpdatePanel, ByVal strMessage As String)
        Dim strScript As String = "alert('" & strMessage & "');"
        Dim guidKey As Guid = Guid.NewGuid()
        ScriptManager.RegisterStartupScript(up, up.GetType(), guidKey.ToString(), strScript, True)

    End Sub

    Private Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try

            ssql = objdatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
            Dim fila = dtEstado.NewRow
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

    Private Sub ddlPaisF_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaisF.SelectedIndexChanged
        Try
            ''Cargamos los estados
            ssql = objdatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPaisF.SelectedValue)
            Dim dtEstadof As New DataTable
            dtEstadof = objdatos.fnEjecutarConsultaSAP(ssql)
            Dim fila = dtEstadof.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtEstadof.Rows.Add(fila)
            ddlEstadoF.DataSource = dtEstadof
            ddlEstadoF.DataTextField = "Descripcion"
            ddlEstadoF.DataValueField = "Clave"
            ddlEstadoF.DataBind()
            ddlEstadoF.SelectedValue = "0"
        Catch ex As Exception

        End Try
    End Sub
End Class
