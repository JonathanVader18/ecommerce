Imports System.Data
Partial Class agregar_clientegtm
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objdatos As New Cls_Funciones
    Private Sub agregar_clientegtm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ''Cargamos los paises
        Dim fila As DataRow
        If Not IsPostBack Then
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


        End If
    End Sub

    Private Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
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

    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        ''Realizamos algunas validaciones
        Dim message As String = ""
        If txtRazonSocial.Text = "" Then

            message = "alert('Debe indicar razón social');"
            ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

            Exit Sub
        End If
        If txtrfc.Text = "" Then


            message = "alert('Debe indicar NIT');"
            ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
            Exit Sub
        End If

        'If txtCalle.Text = "" Then
        '    message = "alert('Debe indicar la calle');"
        '    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
        '    Exit Sub
        'Else
        '    Session("CalleEnvio") = txtCalle.Text
        'End If
        'If txtColonia.Text = "" Then
        '    message = "alert('Debe indicar la colonia');"
        '    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
        '    Exit Sub
        'Else
        '    Session("ColoniaEnvio") = txtColonia.Text
        'End If
        'If txtCiudad.Text = "" Then
        '    message = "alert('Debe indicar la ciudad');"
        '    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
        '    Exit Sub
        'Else
        '    Session("CiudadEnvio") = txtCiudad.Text
        'End If

        'If txtLocalidad.Text = "" Then
        '    objdatos.Mensaje("Debe indicar el Municipio", Me.Page)
        '    Exit Sub
        'Else
        '    Session("MunicipioEnvio") = txtLocalidad.Text
        'End If

        'If txtNumExt.Text = "" Then
        '    message = "alert('Debe indicar el número exterior');"
        '    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)
        '    Exit Sub
        'Else
        '    Session("NumExtEnvio") = txtNumExt.Text
        'End If
        'If ddlEstado.SelectedItem.Text = "" Then
        '    objdatos.Mensaje("Debe indicar el estado", Me.Page)
        '    Exit Sub
        'Else
        '    Session("EstadoEnvio") = ddlEstado.SelectedValue
        'End If
        'If ddlPais.SelectedItem.Text = "" Then
        '    objdatos.Mensaje("Debe indicar el País", Me.Page)
        '    Exit Sub
        'Else
        '    Session("PaisEnvio") = ddlPais.SelectedValue
        'End If

        'If txtCP.Text = "" Then
        '    objdatos.Mensaje("Debe indicar el código postal", Me.Page)
        '    Exit Sub
        'Else
        '    Session("CPEnvio") = txtCP.Text
        'End If
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

            ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
            Dim dtcliente As New DataTable
            dtcliente = objdatos.fnEjecutarConsulta(ssql)
            If dtcliente.Rows.Count > 0 Then
                If dtcliente.Rows(0)(0) = "TAQ" Then
                    ssql = objdatos.fnObtenerQuery("CardCodeTAQ")
                    ssql = ssql.Replace("[%0]", dtTipo.Rows(0)(0))
                    Dim dtCardCode As New DataTable
                    objdatos.fnLog("Alta Clientes", ssql.Replace("'", ""))
                    dtCardCode = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtCardCode.Rows.Count > 0 Then
                        sCardCode = CStr(dtCardCode.Rows(0)(0)).Trim
                        sCardCode = sCardCode.Replace(" ", "")
                        objdatos.fnLog("Alta Clientes", sCardCode)
                    End If
                End If
            End If

            If oCompany.Connected Then

                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                'Página Web

                If dtcliente.Rows.Count > 0 Then
                    If dtcliente.Rows(0)(0) = "TAQ" Then
                        Try
                            oProspecto.Industry = 1
                        Catch ex As Exception

                        End Try
                    End If
                End If

                oProspecto.CardName = txtRazonSocial.Text
                oProspecto.FederalTaxID = "000000000000"
                If dtTipo.Rows.Count > 0 Then
                    If dtTipo.Rows(0)(0) = "C" Then
                        oProspecto.CardType = SAPbobsCOM.BoCardTypes.cCustomer
                    Else
                        oProspecto.CardType = SAPbobsCOM.BoCardTypes.cLid
                    End If
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
                End If

                Try
                    oProspecto.UserFields.Fields.Item("U_NIT").Value = txtrfc.Text
                Catch ex As Exception

                End Try


                ''Direccion fiscal

                If txtCalle.Text <> "" Then
                    oProspecto.Addresses.SetCurrentLine(0)
                    oProspecto.Addresses.TypeOfAddress = "B"
                    oProspecto.Addresses.AddressName = "Facturación"
                    oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                    oProspecto.Addresses.Block = txtColonia.Text
                    oProspecto.Addresses.City = txtCiudad.Text
                    oProspecto.Addresses.County = txtLocalidad.Text
                    oProspecto.Addresses.State = ddlEstado.SelectedValue
                    oProspecto.Addresses.Street = txtCalle.Text
                    oProspecto.Addresses.ZipCode = txtCP.Text
                    If txtNumInt.Text <> "" Then
                        oProspecto.Addresses.StreetNo = txtNumExt.Text & "-" & txtNumInt.Text
                    Else
                        oProspecto.Addresses.StreetNo = txtNumExt.Text
                    End If


                    ''Ponemos la misma en envio
                    oProspecto.Addresses.Add()
                    oProspecto.Addresses.TypeOfAddress = "S"
                    oProspecto.Addresses.AddressName = "Entrega"
                    oProspecto.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                    oProspecto.Addresses.Block = txtColonia.Text
                    oProspecto.Addresses.City = txtCiudad.Text
                    oProspecto.Addresses.County = txtLocalidad.Text
                    oProspecto.Addresses.State = ddlEstado.SelectedValue
                    oProspecto.Addresses.Street = txtCalle.Text
                    oProspecto.Addresses.ZipCode = txtCP.Text
                    If txtNumInt.Text <> "" Then
                        oProspecto.Addresses.StreetNo = txtNumExt.Text & "-" & txtNumInt.Text
                    Else
                        oProspecto.Addresses.StreetNo = txtNumExt.Text
                    End If
                End If

                Try
                    oProspecto.UserFields.Fields.Item("U_Email").Value = "3"
                    oProspecto.UserFields.Fields.Item("U_DocEnviar").Value = "2"
                Catch ex As Exception

                End Try

                Try
                    oProspecto.SalesPersonCode = Session("slpCode")
                Catch ex As Exception

                End Try


                oProspecto.ContactEmployees.SetCurrentLine(0)
                oProspecto.ContactEmployees.FirstName = txtNombreContacto.Text
                oProspecto.ContactEmployees.Name = "Contacto"
                oProspecto.ContactEmployees.Position = ""
                oProspecto.ContactEmployees.E_Mail = txtMailContacto.Text
                oProspecto.ContactEmployees.Phone1 = txtTelContacto.Text

                oProspecto.ContactEmployees.Address = txtCalle.Text & " "
                oProspecto.ContactEmployees.Active = SAPbobsCOM.BoYesNoEnum.tYES
                oProspecto.ContactEmployees.Add()

                If oProspecto.Add <> 0 Then
                    objdatos.fnLog("Alta cliente", oCompany.GetLastErrorDescription.Replace("'", ""))
                    ' objdatos.Mensaje("Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription, Me.Page)
                    message = "alert('" & "Ha ocurrido un error al registrar: " & oCompany.GetLastErrorDescription & "');"
                    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                Else
                    btnLevantarPed.Visible = True
                    Session("Cliente") = oCompany.GetNewObjectKey
                    Session("RazonSocial") = txtRazonSocial.Text


                    message = "alert('Registrado correctamente');"
                    ScriptManager.RegisterClientScriptBlock(TryCast(sender, Control), Me.GetType(), "alert", message, True)

                    ''en base al cliente, obtenemos cual es su lista de precios
                    ssql = objdatos.fnObtenerQuery("ListaPrecioscliente")
                    ssql = ssql.Replace("[%0]", Session("Cliente"))
                    Dim dtLista As New DataTable
                    dtLista = objdatos.fnEjecutarConsultaSAP(ssql)
                    If dtLista.Rows.Count > 0 Then
                        Session("ListaPrecios") = dtLista.Rows(0)(0)
                    Else
                        Session("ListaPrecios") = "1"
                    End If
                    Session("Pedido") = New Cls_Pedido

                    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                    Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                    Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

                    txtRazonSocial.Text = ""
                    txtrfc.Text = ""
                    txtCalle.Text = ""
                    txtNumExt.Text = ""
                    txtNumInt.Text = ""
                    txtColonia.Text = ""
                    txtLocalidad.Text = ""
                    txtCP.Text = ""
                    txtCiudad.Text = ""

                End If

            End If
        Catch ex As Exception
            objdatos.fnLog("Alta cliente ex ", ex.Message)
        End Try
    End Sub

    Private Sub btnLevantarPed_Click(sender As Object, e As EventArgs) Handles btnLevantarPed.Click

        Session("Partidas") = New List(Of Cls_Pedido.Partidas)
        Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
        Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)


        ''Obtenemos el nombre de la empresa
        ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
        Dim dtcliente As New DataTable
        dtcliente = objdatos.fnEjecutarConsulta(ssql)

        If dtcliente.Rows.Count > 0 Then
            If CStr(dtcliente.Rows(0)(0)).Contains("TAQ") Then
                Response.Redirect("captura-pedido.aspx")
            Else
                Response.Redirect("catalogo.aspx")
            End If
        Else
            Response.Redirect("catalogo.aspx")
        End If
    End Sub
End Class
