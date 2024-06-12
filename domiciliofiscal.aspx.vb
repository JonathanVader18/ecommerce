Imports System.Data
Partial Class domiciliofiscal
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub domiciliofiscal_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim fila As DataRow
        If Not IsPostBack Then

            ssql = objDatos.fnObtenerQuery("UsoCFDI")
            If ssql <> "" Then
                Dim dtUso As New DataTable
                dtUso = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtUso.Rows.Count > 0 Then
                    Dim filac As DataRow
                    filac = dtUso.NewRow
                    filac("Clave") = "-1"
                    filac("Descripcion") = "-Seleccione-"
                    dtUso.Rows.Add(filac)

                    ddlUsoCFDI.DataSource = dtUso
                    ddlUsoCFDI.DataTextField = "Descripcion"
                    ddlUsoCFDI.DataValueField = "Clave"
                    ddlUsoCFDI.DataBind()
                    ddlUsoCFDI.SelectedValue = Session("UsoCFDI")
                    ddlUsoCFDI.Enabled = False


                End If
            End If
            txtRFC.Text = Session("RFCEnvio")
            txtRFC.ReadOnly = True

            txtNombre.Text = Session("NombreuserTienda")

            ''Cargamos los paises
            ssql = objDatos.fnObtenerQuery("Paises")
            Dim dtPais As New DataTable
            dtPais = objDatos.fnEjecutarConsultaSAP(ssql)
            'fila = dtPais.NewRow
            'fila("Clave") = "0"
            'fila("Descripcion") = "-Seleccione-"
            'dtPais.Rows.Add(fila)
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
                ddlEstados.SelectedValue = Session("EstadoEnvio")
            Catch ex As Exception
                objDatos.fnLog("Cargar estados", ex.Message)
            End Try

            txtCalle.Text = Session("CalleEnvio")
            txtNumExt.Text = Session("NumExtEnvio")
            txtNumInt.Text = Session("NumInteriorEnvio")
            txtTelefono.Text = Session("TelefonoEnvio")
            txtColonia.Text = Session("ColoniaEnvio")
            txtCiudad.Text = Session("CiudadEnvio")
            txtMunicipio.Text = Session("MunicipioEnvio")
            txtCP.Text = Session("CPEnvio")



        End If
    End Sub

    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        If txtCalle.Text = "" Then
            objDatos.Mensaje("Debe indicar la calle", Me.Page)
            Exit Sub
        Else
            Session("CalleFiscal") = txtCalle.Text
        End If
        If txtNombre.Text = "" Then
            objDatos.Mensaje("Debe indicar la razón social", Me.Page)
            Exit Sub
        Else
            Session("NombreClienteFiscal") = txtNombre.Text
        End If

        If txtColonia.Text = "" Then
            objDatos.Mensaje("Debe indicar la colonia", Me.Page)
            Exit Sub
        Else
            Session("ColoniaFiscal") = txtColonia.Text
        End If
        If txtCiudad.Text = "" Then
            objDatos.Mensaje("Debe indicar la ciudad", Me.Page)
            Exit Sub
        Else
            Session("CiudadFiscal") = txtCiudad.Text
        End If

        If txtMunicipio.Text = "" Then
            objDatos.Mensaje("Debe indicar el Municipio", Me.Page)
            Exit Sub
        Else
            Session("MunicipioFiscal") = txtMunicipio.Text
        End If

        If txtNumExt.Text = "" Then
            objDatos.Mensaje("Debe indicar número exterior", Me.Page)
            Exit Sub
        Else
            Session("NumExtFiscal") = txtNumExt.Text
        End If
        If ddlEstados.SelectedItem.Text = "" Then
            objDatos.Mensaje("Debe indicar el estado", Me.Page)
            Exit Sub
        Else
            Session("EstadoFiscal") = ddlEstados.SelectedValue
        End If
        If ddlPais.SelectedItem.Text = "" Then
            objDatos.Mensaje("Debe indicar el País", Me.Page)
            Exit Sub
        Else
            Session("PaisFiscal") = ddlPais.SelectedValue
        End If

        If ddlUsoCFDI.SelectedItem.Text.ToUpper <> "-SELECCIONE-" Then

            Session("UsoCFDI") = ddlUsoCFDI.SelectedValue
        Else
            Session("UsoCFDI") = ""
        End If

        If txtRFC.Text <> "" Then

            If txtRFC.Text.Length < 12 Or txtRFC.Text.Length > 13 Then
                objDatos.Mensaje("Debe proporcionar un RFC válido", Me.Page)
                Exit Sub
            End If
            Session("RFCEnvio") = txtRFC.Text
        End If


        If txtCP.Text = "" Then
            objDatos.Mensaje("Debe indicar el código postal", Me.Page)
            Exit Sub
        Else
            Session("CPFiscal") = txtCP.Text
        End If
        Session("TelefonoEnvio") = txtTelefono.Text
        Session("NumInteriorFiscal") = txtNumInt.Text
        Response.Redirect("envio.aspx")
    End Sub
End Class
