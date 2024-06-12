Imports System.Data
Imports System.Security.Cryptography

Partial Class confirmacionalta
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objdatos As New Cls_Funciones
    Public Hash As String = "PERJ840518AB0"
    Private Sub confirmacionalta_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim sCorreo As String = ""
            sCorreo = DesEncriptar(Request.QueryString("confirm"))

            ''Obtenemos el nombre del usuario
            ssql = "SELECT cvNombre,ciIdCliente,cvMail,cvEstatus from Tienda.clientes where ciIdCliente=" & "'" & sCorreo & "'"

            Dim dtCliente As New DataTable
            dtCliente = objdatos.fnEjecutarConsulta(ssql)
            If dtCliente.Rows.Count > 0 Then
                If dtCliente.Rows(0)("cvEstatus") <> "POR CONFIRMAR" Then
                    lblUsuario.Text = "Este correo de confirmación ha vencido. Es probable que ya haya hecho un registro previo con nosotros."
                    Exit Sub
                End If
                lblUsuario.Text = dtCliente.Rows(0)(0)
                lblBienvenida.Text = ""
                ssql = "UPDATE Tienda.clientes set cvEstatus='CONFIRMADO' WHERE  ciIdCliente=" & "'" & sCorreo & "'"
                objdatos.fnEjecutarInsert(ssql)
                ''Lo enviamos crear a SAP
                Dim sMensajeSAP As String = SAP_CreaCliente(dtCliente.Rows(0)("ciIdCliente"))
                If sMensajeSAP.Contains("ERROR") Then
                    ''Notificamos el problema
                Else
                    ''Enviamos correo
                    Dim Mensaje As String() = sMensajeSAP.Split("-")
                    Dim sCardCodeSAP As String = "XXXX"
                    Dim sPassSAP As String = "YYYY"
                    Dim sNombreCliente As String = dtCliente.Rows(0)(0)
                    sCardCodeSAP = Mensaje(1)
                    ssql = "SELECT [Password] FROM OCRD where cardCode=" & "'" & sCardCodeSAP & "' and cardtype='C'"
                    Dim dtPass As New DataTable
                    dtPass = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtPass.Rows.Count > 0 Then
                        sPassSAP = dtPass.Rows(0)(0)
                    End If

                    Dim text As String = ""
                    Dim sNombreComercial As String = "STOP Catálogo"

                    text = MensajeHTML(Server.MapPath("~") & "\AltaClienteconfirmado.html")
                    Dim sDestinatario As String = ""
                    sDestinatario = dtCliente.Rows(0)("cvMail")
                    text = text.Replace("{enviara}", sNombreCliente)
                    text = text.Replace("{usuario}", sCardCodeSAP)
                    text = text.Replace("{password}", sPassSAP)


                    objdatos.fnEnviarCorreo(sDestinatario, text, "Bienvenido a " & sNombreComercial & "!")


                End If
            Else
                lblUsuario.Text = "Este correo de confirmación ha vencido."
                lblBienvenida.Text = ""
            End If
        End If
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

    Protected Function SAP_CreaCliente(idCliente As Int32)
        Dim sResultado As String = "OK"

        ''Ingresamos los valores a SAP
        Dim oProspecto As SAPbobsCOM.BusinessPartners
        Dim oCompany As New SAPbobsCOM.Company
        Dim sCardCode As String = idCliente
        Dim sCliente As String = ""

        Try
            ''Obtenemos la configuración de como dar de alta el cliente
            ssql = "SELECT ISNULL(cvCliente,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objdatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then
                If CStr(dtcliente.Rows(0)(0)).Contains("Stop Catálogo") Then
                    ''
                    sCliente = "DELTA"
                    ssql = "select MAX(cast(cardCode as int))+1 from OCRD where cardtype ='C' "
                    Dim dtMaxCardcode As New DataTable
                    dtMaxCardcode = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtMaxCardcode.Rows.Count > 0 Then
                        sCardCode = dtMaxCardcode.Rows(0)(0)
                    End If


                End If
            End If

            ''Serie,grupo y lista de precios
            Dim iSerie As Int64 = 0
            Dim iLista As Int64 = 0
            Dim sGrupo As String = ""

            ssql = " SELECT ISNULL(cvSerieClientes ,'') as Serie, ISNULL(cvGrupo ,'') as Grupo,ISNULL(ciListaPrecios ,0) as Lista from config.Parametrizaciones_B2C "
            Dim dtParam As New DataTable
            dtParam = objdatos.fnEjecutarConsulta(ssql)
            If dtParam.Rows.Count > 0 Then
                If dtParam.Rows(0)("Serie") <> "" Then
                    '    iSerie = dtParam.Rows(0)("Serie")
                End If
                If dtParam.Rows(0)("Grupo") <> "" Then
                    sGrupo = dtParam.Rows(0)("Grupo")
                End If
                If dtParam.Rows(0)("Lista") <> "0" Then
                    iLista = dtParam.Rows(0)("Lista")
                End If
            End If

            ssql = "SELECT * FROM Tienda.clientes where ciIdCliente=" & "'" & idCliente & "'"
            Dim dtClienteGral As New DataTable
            dtClienteGral = objdatos.fnEjecutarConsulta(ssql)

            If dtClienteGral.Rows.Count = 0 Then
                Exit Function
            End If
            ssql = "select * from tienda.DireccionesCliente where ciIdCliente=" & "'" & idCliente & "' and cvTipoDireccion='ENVIO'"
            Dim dtDireccionesCliente As New DataTable
            dtDireccionesCliente = objdatos.fnEjecutarConsulta(ssql)

            oCompany = objdatos.fnConexion_SAP
            Dim valuePassword As Integer = CInt((996666 * Rnd()))
            If oCompany.Connected Then
                objdatos.fnLog("Alta Cliente", "Si Conecta")
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.CardType = SAPbobsCOM.BoCardTypes.cCustomer

                oProspecto.CardName = dtClienteGral.Rows(0)("cvNombre")
                oProspecto.FederalTaxID = dtClienteGral.Rows(0)("cvRFC")
                oProspecto.UserFields.Fields.Item("U_UsoCFDI").Value = dtClienteGral.Rows(0)("cvUsoCFDI")
                oProspecto.Password = valuePassword.ToString
                oProspecto.EmailAddress = dtClienteGral.Rows(0)("cvMail")
                oProspecto.Phone1 = dtClienteGral.Rows(0)("cvTelefono")
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


                If CStr(dtClienteGral.Rows(0)("cvRFC")).Length = 12 Then
                    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cCompany
                Else
                    oProspecto.CompanyPrivate = SAPbobsCOM.BoCardCompanyTypes.cPrivate
                End If
                objdatos.fnLog("Alta Cliente", "asignó series, grupos y listas")

                '  oProspecto.PayTermsGrpCode = -1

                '''Direccion fiscal

                ssql = "select * from tienda.DireccionesCliente where ciIdCliente=" & "'" & idCliente & "' and cvTipoDireccion='FISCAL'"
                Dim dtDireccionFiscalCliente As New DataTable
                dtDireccionFiscalCliente = objdatos.fnEjecutarConsulta(ssql)

                If dtDireccionFiscalCliente.Rows.Count > 0 Then
                    oProspecto.Addresses.SetCurrentLine(0)
                    oProspecto.Addresses.TypeOfAddress = "B"
                    oProspecto.Addresses.AddressName = "Facturación"
                    oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                    oProspecto.Addresses.Block = dtDireccionFiscalCliente.Rows(0)("cvColonia")
                    oProspecto.Addresses.City = dtDireccionFiscalCliente.Rows(0)("cvCiudad")
                    oProspecto.Addresses.County = dtDireccionFiscalCliente.Rows(0)("cvMunicipio")
                    oProspecto.Addresses.ZipCode = dtDireccionFiscalCliente.Rows(0)("cvCP")
                    oProspecto.Addresses.Country = dtDireccionFiscalCliente.Rows(0)("cvPais")
                    oProspecto.Addresses.State = dtDireccionFiscalCliente.Rows(0)("cvEstado")

                    oProspecto.Addresses.Street = dtDireccionFiscalCliente.Rows(0)("cvCalleNum")
                Else
                    ''Sino tiene fiscal, usamos la de envio
                    oProspecto.Addresses.SetCurrentLine(0)
                    oProspecto.Addresses.TypeOfAddress = "B"
                    oProspecto.Addresses.AddressName = "Facturación"
                    oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                    oProspecto.Addresses.Block = dtDireccionesCliente.Rows(0)("cvColonia")
                    oProspecto.Addresses.City = dtDireccionesCliente.Rows(0)("cvCiudad")
                    oProspecto.Addresses.County = dtDireccionesCliente.Rows(0)("cvMunicipio")
                    oProspecto.Addresses.ZipCode = dtDireccionesCliente.Rows(0)("cvCP")
                    oProspecto.Addresses.Country = dtDireccionesCliente.Rows(0)("cvPais")
                    oProspecto.Addresses.State = dtDireccionesCliente.Rows(0)("cvEstado")

                    oProspecto.Addresses.Street = dtDireccionesCliente.Rows(0)("cvCalleNum")

                End If

                ''Ponemos la misma en envio
                oProspecto.Addresses.Add()
                oProspecto.Addresses.TypeOfAddress = "S"
                oProspecto.Addresses.AddressName = "Envio"
                oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                oProspecto.Addresses.Block = dtDireccionesCliente.Rows(0)("cvColonia")
                oProspecto.Addresses.City = dtDireccionesCliente.Rows(0)("cvCiudad")
                oProspecto.Addresses.County = dtDireccionesCliente.Rows(0)("cvMunicipio")
                oProspecto.Addresses.ZipCode = dtDireccionesCliente.Rows(0)("cvCP")
                oProspecto.Addresses.Country = dtDireccionesCliente.Rows(0)("cvPais")
                oProspecto.Addresses.State = dtDireccionesCliente.Rows(0)("cvEstado")
                oProspecto.Addresses.Street = dtDireccionesCliente.Rows(0)("cvCalleNum")

                If oProspecto.Add <> 0 Then
                    'message = "alert('Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription & "');"
                    'ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
                    objdatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)
                    objdatos.fnLog("Alta Cliente", "Error: " & oCompany.GetLastErrorDescription.Replace("'", ""))
                    'objDatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)
                    sResultado = "ERROR-" & oCompany.GetLastErrorDescription
                Else
                    ssql = "UPDATE Tienda.clientes set cvEstatus='EN SAP' WHERE  ciIdCliente=" & "'" & idCliente & "'"
                    objdatos.fnEjecutarInsert(ssql)

                    ssql = "UPDATE Tienda.clientes set ciProcesadoSAP='1',cvCardCodeSAP='" & sCardCode & "' WHERE  ciIdCliente=" & "'" & idCliente & "'"
                    objdatos.fnEjecutarInsert(ssql)

                    sResultado = "OK-" & sCardCode
                    objdatos.fnLog("Alta Cliente", "Registrado")
                    ' objDatos.Mensaje("Registrado! ", Me.Page)


                End If

            End If
        Catch ex As Exception
            objdatos.fnLog("Alta Cliente ex", ex.Message)
        End Try
        Return sResultado
    End Function

    Private Function Desencriptar(ByVal texto As String) As String
        Dim Resultado = ""
        Try
            Dim contenido As String = texto
            Dim hash As String = "PERJ840518AB0"
            Dim des As New TripleDESCryptoServiceProvider 'Algorithmo TripleDES
            Dim hashmd5 As New MD5CryptoServiceProvider 'objeto md5
            objdatos.fnLog("Desencrip", contenido)

            If Trim(contenido) = "" Then
                Resultado = ""
            Else
                des.Key = hashmd5.ComputeHash((New UnicodeEncoding).GetBytes(hash))
                des.Mode = CipherMode.ECB
                Dim desencrypta As ICryptoTransform = des.CreateDecryptor()
                Dim buff() As Byte = Convert.FromBase64String(contenido)
                Resultado = UnicodeEncoding.ASCII.GetString(desencrypta.TransformFinalBlock(buff, 0, buff.Length))
            End If

            objdatos.fnLog("Desencrip resultado", Resultado)
            Return Resultado
        Catch __unusedException1__ As Exception
            objdatos.fnLog("Desencrip resultado ex", __unusedException1__.Message)
            ' objdatos.Mensaje(__unusedException1__.Message, Me.Page)
        End Try
    End Function
End Class
