
Imports System.Data
Partial Class editar_direccion
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub editar_direccion_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnMenuPreferencias()

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

            If Request.QueryString.Count > 0 Then
                objDatos.fnLog("Direcciones", "Request")
                Dim IdDireccion As String = ""
                Dim Action As String = ""

                IdDireccion = Request.QueryString("dir")
                Action = Request.QueryString("action")
                objDatos.fnLog("Direcciones", "Lee Request")
                If Action = "e" Then
                    objDatos.fnLog("Direcciones", "Action e")
                    ''Editamos la direccion, la cargamos abajo
                    ssql = "SELECT  [ciIdRel],[cvUsuario],ISNULL([cvCalle],'')cvCalle, ISNULL([cvColonia],'') cvColonia, ISNULL([cvNumExt],'') as cvNumExt, ISNULL([cvNumInt],'') cvNumInt ,ISNULL([cvCiudad],'') cvCiudad, ISNULL([cvMunicipio],'') cvMunicipio, ISNULL([cvEstado],'') cvEstado, ISNULL([cvPais],'') cvPais,cvPredeterminado,ISNULL(cvCP,'') as cvCP FROM Tienda.Direcciones_Envio WHERE cvUsuario='" & Session("UserB2C") & "' AND ciIdRel= " & "'" & IdDireccion & "'"
                    Dim dtDireccion As New DataTable
                    dtDireccion = objDatos.fnEjecutarConsulta(ssql)
                    objDatos.fnLog("Direcciones", dtDireccion.Rows.Count)
                    If dtDireccion.Rows.Count > 0 Then
                        txtCalle.Text = dtDireccion.Rows(0)("cvCalle")
                        txtColonia.Text = dtDireccion.Rows(0)("cvColonia")
                        txtNumExt.Text = dtDireccion.Rows(0)("cvNumExt")
                        txtNumInt.Text = dtDireccion.Rows(0)("cvNumInt")
                        txtCiudad.Text = dtDireccion.Rows(0)("cvCiudad")
                        txtLocalidad.Text = dtDireccion.Rows(0)("cvMunicipio")
                        txtCp.Text = dtDireccion.Rows(0)("cvCP")
                        ddlEstados.SelectedValue = dtDireccion.Rows(0)("cvEstado")
                        ddlPais.SelectedValue = dtDireccion.Rows(0)("cvPais")


                        Session("CalleEnvio") = txtCalle.Text
                        Session("NumExtEnvio") = txtNumExt.Text
                        Session("ColoniaEnvio") = txtColonia.Text
                        Session("EstadoEnvio") = ddlEstados.SelectedValue
                        Session("MunicipioEnvio") = txtLocalidad.Text
                        Session("ColoniaEnvio") = txtColonia.Text
                        Session("CPEnvio") = txtCp.Text



                    End If
                End If
            End If
        End If
    End Sub

    Public Function fnActualizaDireccionCliente(CardCode As String, Direccion As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sNuevaDireccion As String = ""
        Dim sResultado As String = ""
        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
        Else
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
        End If
        Dim sAdressName As String
        sAdressName = "Envio-" & sIdDireccion


        ''Nueva dirección
        If (txtCalle.Text & " " & txtNumExt.Text).Length > 30 Then
            sNuevaDireccion = (txtCalle.Text & " " & txtNumExt.Text).Substring(0, 30)
        Else
            sNuevaDireccion = (txtCalle.Text & " " & txtNumExt.Text)
        End If

        sNuevaDireccion = "Envio-" & sNuevaDireccion

        objDatos.fnLog("Actualizando direccion", sAdressName)


        Dim sQueryDirecciones As String = ""

        sQueryDirecciones = objDatos.fnObtenerQuery("DireccionesSN")

        If sQueryDirecciones <> "" Then
            sQueryDirecciones = sQueryDirecciones.Replace("[%0]", CardCode)


            Dim dtDirecciones As New DataTable
            dtDirecciones = objDatos.fnEjecutarConsultaSAP(sQueryDirecciones)

            If dtDirecciones.Rows.Count > 0 Then
                Dim oProspecto As SAPbobsCOM.BusinessPartners
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.GetByKey(CardCode)
                For i = 0 To dtDirecciones.Rows.Count - 1 Step 1

                    oProspecto.Addresses.SetCurrentLine(i)
                    If oProspecto.Addresses.AddressName = sAdressName Then
                        oProspecto.Addresses.AddressName = sNuevaDireccion
                        oProspecto.Addresses.Block = txtColonia.Text
                        oProspecto.Addresses.County = txtLocalidad.Text

                        oProspecto.Addresses.StreetNo = txtNumExt.Text
                        oProspecto.Addresses.Street = txtCalle.Text
                        oProspecto.Addresses.State = ddlEstados.SelectedValue
                        oProspecto.Addresses.ZipCode = txtCp.Text
                        If oProspecto.Update() <> 0 Then
                            sIdDireccion = "ERROR"

                        End If
                        Exit For
                        sIdDireccion = sAdressName
                    End If



                Next
            End If


        End If



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function


    Public Function fnEliminaDireccionCliente(CardCode As String, Direccion As String, oCompany As SAPbobsCOM.Company)

        Dim sIdDireccion As String = ""
        Dim sResultado As String = ""
        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
        Else
            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
        End If
        Dim sAdressName As String
        sAdressName = "Envio-" & sIdDireccion

        objDatos.fnLog("Actualizando direccion", sAdressName)


        Dim sQueryDirecciones As String = ""

        sQueryDirecciones = objDatos.fnObtenerQuery("DireccionesSN")

        If sQueryDirecciones <> "" Then
            sQueryDirecciones = sQueryDirecciones.Replace("[%0]", CardCode)


            Dim dtDirecciones As New DataTable
            dtDirecciones = objDatos.fnEjecutarConsultaSAP(sQueryDirecciones)

            If dtDirecciones.Rows.Count > 0 Then
                Dim oProspecto As SAPbobsCOM.BusinessPartners
                oProspecto = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oProspecto.GetByKey(CardCode)
                For i = 0 To dtDirecciones.Rows.Count - 1 Step 1

                    oProspecto.Addresses.SetCurrentLine(i)
                    If oProspecto.Addresses.AddressName = sAdressName Then
                        oProspecto.Addresses.Delete()
                        If oProspecto.Update() <> 0 Then
                            sIdDireccion = "ERROR"

                        End If
                        Exit For
                        sIdDireccion = sAdressName
                    End If



                Next
            End If


        End If



        objDatos.fnLog("Agregar direccion-return", sIdDireccion)
        Return sIdDireccion
    End Function


    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ''Editamos
        Dim IdDireccion As String = ""
        Dim Action As String = ""

        IdDireccion = Request.QueryString("dir")
        Action = Request.QueryString("action")

        If Action = "e" Then
            ssql = "UPDATE Tienda.Direcciones_Envio SET " _
                & " cvCalle=" & "'" & txtCalle.Text & "'," _
                & " cvColonia=" & "'" & txtColonia.Text & "'," _
                & " cvNumExt=" & "'" & txtNumExt.Text & "'," _
                & " cvNumInt=" & "'" & txtNumInt.Text & "'," _
                & " cvCiudad=" & "'" & txtCiudad.Text & "'," _
                & " cvMunicipio=" & "'" & txtLocalidad.Text & "'," _
                & " cvEstado=" & "'" & ddlEstados.SelectedValue & "'," _
                & " cvCP=" & "'" & txtCp.Text & "'," _
                & " cvPais=" & "'" & ddlPais.SelectedValue & "' " _
                & " where ciIdRel=" & "'" & IdDireccion & "'"
            objDatos.fnEjecutarInsert(ssql)

            ''Revisamos si tenemos que ir a SAP a editar
            ssql = "SELECT ISNULL(cvCreaClienteRegistro,'NO') FROM  config.Parametrizaciones_B2C "
            Dim dtCreaSAP As New DataTable
            dtCreaSAP = objDatos.fnEjecutarConsulta(ssql)
            If dtCreaSAP.Rows.Count > 0 Then
                If dtCreaSAP.Rows(0)(0) = "SI" Then

                    ssql = "SELECT ISNULL(cvCardCode,'') FROM Config.usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "'"
                    Dim dtClienteSAP As New DataTable
                    dtClienteSAP = objDatos.fnEjecutarConsulta(ssql)
                    Dim sCardCodeSAP As String = ""
                    If dtClienteSAP.Rows.Count > 0 Then

                        sCardCodeSAP = dtClienteSAP.Rows(0)(0)


                        Dim sIdDireccion As String = ""
                        If CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Length > 30 Then
                            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio")).Substring(0, 30)
                        Else
                            sIdDireccion = CStr(Session("CalleEnvio") & " " & Session("NumExtEnvio"))
                        End If
                        Dim sAdressName As String = ""
                        sAdressName = "Envio-" & sIdDireccion

                        ssql = objDatos.fnObtenerQuery("DetalleDireccion")
                        ssql = ssql.Replace("[%0]", sCardCodeSAP).Replace("[%1]", sAdressName)
                        Dim dtDetalleDireccion As New DataTable
                        dtDetalleDireccion = objDatos.fnEjecutarConsultaSAP(ssql)
                        If dtDetalleDireccion.Rows.Count > 0 Then
                            ''Ya existe
                            Dim oCompany As SAPbobsCOM.Company
                            oCompany = objDatos.fnConexion_SAP
                            fnActualizaDireccionCliente(sCardCodeSAP, sAdressName, oCompany)
                            objDatos.fnLog("dirección Decimal entrega", "Ya existe:" & sAdressName.Replace("'", ""))
                        End If
                    End If
                End If
            End If
        End If
        objDatos.Mensaje("Dirección actualizada", Me.Page)
    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("direcciones.aspx")
    End Sub
    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            ''Cargamos los estados

            ssql = objDatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)

            objDatos.fnLog("pais index", "")
            Dim fila As DataRow
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
    Public Sub fnMenuPreferencias()
        Dim ssql As String
        ssql = "SELECT * FROM Config.Menus where cvTipoMenu='Preferencias' "
        Dim dtMenuHeader As New DataTable
        dtMenuHeader = objDatos.fnEjecutarConsulta(ssql)
        Dim sHtmlMenu As String = ""

        For i = 0 To dtMenuHeader.Rows.Count - 1 Step 1
            sHtmlMenu = sHtmlMenu & " <li><a href='" & dtMenuHeader.Rows(i)("cvLink") & "'> " & dtMenuHeader.Rows(i)("cvNombre") & " </a></li> "
        Next
        Dim literal As New LiteralControl(sHtmlMenu)
        pnlMenuPref.Controls.Clear()
        pnlMenuPref.Controls.Add(literal)

    End Sub
End Class
