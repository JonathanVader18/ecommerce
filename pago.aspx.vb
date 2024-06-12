Imports System.Data
Partial Class pago_pago
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub pago_pago_Load(sender As Object, e As EventArgs) Handles Me.Load

        fnTotales()
        fnMetodosPagoCliente()

        Try
            Dim fila As DataRow
            ''Cargamos los paises
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
            ddlPais.SelectedValue = "0"

            ''Cargamos los estados
            ssql = objDatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)
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

        End Try
    End Sub
    Public Sub fnTotales()
        lblSubTotal.Text = CDbl(Session("ImporteSubTotal")).ToString("$ ###,###,###.#0")
        lblEnvio.Text = CDbl(Session("ImporteEnvio")).ToString("$ ###,###,###.#0")
        lblDescuento.Text = CDbl(Session("ImporteDescuento")).ToString("$ ###,###,###.#0")
        lblTotal.Text = (CDbl(Session("ImporteSubTotal")) + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString("$ ###,###,###.#0")
    End Sub
    Protected Sub chkMismaDireccion_CheckedChanged(sender As Object, e As EventArgs) Handles chkMismaDireccion.CheckedChanged
        Try

            txtNombreF.Text = Session("NombreEnvio")
            txtCalleF.Text = Session("CalleEnvio")
            txtNumExtF.Text = Session("NumExtEnvio")
            txtNumIntF.Text = Session("NumIntEnvio")
            txtColoniaF.Text = Session("ColoniaEnvio")
            txtCPF.Text = Session("CPEnvio")
            txtCiudadF.Text = Session("CiudadEnvio")
            txtEstadoF.Text = Session("EstadoEnvio")
            txtMunicipioF.Text = Session("MunicipioEnvio")
            ddlPais.SelectedValue = Session("PaisEnvio")


            ''Cargamos los estados
            Dim fila As DataRow
            ssql = objDatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)
            fila = dtEstado.NewRow
            fila("Clave") = "0"
            fila("Descripcion") = "-Seleccione-"
            dtEstado.Rows.Add(fila)
            ddlEstados.DataSource = dtEstado
            ddlEstados.DataTextField = "Descripcion"
            ddlEstados.DataValueField = "Clave"
            ddlEstados.DataBind()
            ddlEstados.SelectedValue = "0"
            ddlEstados.SelectedValue = Session("EstadoEnvio")
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        If chkFactura.Checked = False Then
            Session("IdTarjetaPago") = txtNumTarjeta.Text
            Response.Redirect("resumen.aspx")
            Exit Sub
        End If
        If txtCalleF.Text = "" Then
            objDatos.Mensaje("Debe indicar la calle", Me.Page)
            Exit Sub
        Else
            Session("CalleF") = txtCalleF.Text
        End If
        If txtColoniaF.Text = "" Then
            objDatos.Mensaje("Debe indicar la colonia", Me.Page)
            Exit Sub
        Else
            Session("ColoniaF") = txtColoniaF.Text
        End If
        If txtCiudadF.Text = "" Then
            objDatos.Mensaje("Debe indicar la ciudad", Me.Page)
            Exit Sub
        Else
            Session("CiudadF") = txtCiudadF.Text
        End If

        If txtMunicipioF.Text = "" Then
            objDatos.Mensaje("Debe indicar el Municipio", Me.Page)
            Exit Sub
        Else
            Session("MunicipioF") = txtMunicipioF.Text
        End If

        If txtNumExtF.Text = "" Then
            objDatos.Mensaje("Debe indicar número exterior", Me.Page)
            Exit Sub
        Else
            Session("NumExtF") = txtNumExtF.Text
        End If
        If txtEstadoF.Text = "" Then
            objDatos.Mensaje("Debe indicar el estado", Me.Page)
            Exit Sub
        Else
            Session("EstadoF") = txtEstadoF.Text
        End If

        If txtCPF.Text = "" Then
            objDatos.Mensaje("Debe indicar el código postal", Me.Page)
            Exit Sub
        Else
            Session("CPF") = txtCPF.Text
        End If

        Session("NombreF") = txtNombreF.Text
        Session("NumIntF") = txtNumIntF.Text

        ''Cual tarjeta seleccionó?
        Session("IdTarjetaPago") = txtNumTarjeta.Text

        Response.Redirect("resumen.aspx")
    End Sub
    Public Sub fnMetodosPagoCliente()
        Dim sHtmlEncabezado As String = ""
        Dim sHtmlBanner As String = ""

        ssql = "SELECT * from config.MetodosPago WHERE cvUser=" & "'" & Session("UserB2C") & "' AND cvCardCode=" & "'" & Session("CardCodeUserB2C") & "'"
        Dim dtMetodos As New DataTable
        dtMetodos = objDatos.fnEjecutarConsulta(ssql)

        For i = 0 To dtMetodos.Rows.Count - 1 Step 1
            sHtmlBanner = sHtmlBanner & "<div class='col-xs-2 no-padding'> "
            sHtmlBanner = sHtmlBanner & " <img src='img/masterCard.png' class='img-responsive'>"
            sHtmlBanner = sHtmlBanner & "</div>"
            sHtmlBanner = sHtmlBanner & "<div class='col-xs-10'>"
            sHtmlBanner = sHtmlBanner & " <p>Terminación:" & CStr(dtMetodos.Rows(0)("cvTarjeta")).Substring(CStr(dtMetodos.Rows(0)("cvTarjeta")).Length - 4, 4) & " </p></br>"
            'sHtmlBanner = sHtmlBanner & " <p>Exp. " & CStr(dtMetodos.Rows(0)("cvVence")) & " </p></br>"
            ' sHtmlBanner = sHtmlBanner & " <p>Dirección:" & Session("CalleEnvio") & " " & Session("NumExtEnvio") & " </p></br></div>"
            sHtmlBanner = sHtmlBanner & "</div>"
        Next


        sHtmlEncabezado = sHtmlEncabezado & sHtmlBanner
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlMetodosPago.Controls.Clear()
        pnlMetodosPago.Controls.Add(literal)
    End Sub
    Protected Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        If txtNumTarjeta.Text = "" Then
            objDatos.Mensaje("Debe proporcionar los últimos 4 de la tarjeta a registrar (Solo como referencia)", Me.Page)
            Exit Sub
        Else
            If txtNumTarjeta.Text.Length < 4 Then
                objDatos.Mensaje("Debe proporcionar 4 dígitos en el número de la tarjeta a registrar (Solo como referencia)", Me.Page)
                Exit Sub
            End If
        End If
        'If txtAñoExp.Text = "" Then
        '    objDatos.Mensaje("Debe proporcionar el año en que expira la tarjeta a registrar", Me.Page)
        '    Exit Sub
        'End If
        'If txtMesExp.Text = "" Then
        '    objDatos.Mensaje("Debe proporcionar el mes en que expira la tarjeta a registrar", Me.Page)
        '    Exit Sub
        'End If
        'If txtCodSeguridad.Text = "" Then
        '    objDatos.Mensaje("Debe proporcionar el código de seguridad la tarjeta a registrar", Me.Page)
        '    Exit Sub
        'End If
        'If txtNombre.Text = "" Then
        '    objDatos.Mensaje("Debe proporcionar el nombre del titular de la tarjeta a registrar", Me.Page)
        '    Exit Sub
        'End If

        ssql = "SELECT * from config.MetodosPago WHERE cvUser=" & "'" & Session("UserB2C") & "' AND cvCardCode=" & "'" & Session("CardCodeUserB2C") & "'"
        Dim dtMetodos As New DataTable
        dtMetodos = objDatos.fnEjecutarConsulta(ssql)
        Dim iRegistros As Int16
        iRegistros = dtMetodos.Rows.Count

        ssql = "INSERT INTO config.MetodosPago (cvUser,cvCardCode,cvTarjeta,cvVence,cvCvv,cvTitular,cdFecha) VALUES( " _
            & "'" & Session("UserB2C") & "'," _
            & "'" & Session("CardCodeUserB2C") & "'," _
            & "'" & txtNumTarjeta.Text & "'," _
            & "'/'," _
            & "''," _
            & "'" & Session("CardCodeUserB2C") & "',GETDATE())"
        objDatos.fnEjecutarInsert(ssql)

        ssql = "SELECT * from config.MetodosPago WHERE cvUser=" & "'" & Session("UserB2C") & "' AND cvCardCode=" & "'" & Session("CardCodeUserB2C") & "'"
        dtMetodos = New DataTable
        dtMetodos = objDatos.fnEjecutarConsulta(ssql)

        If iRegistros = dtMetodos.Rows.Count Then
            ''No se registró
        Else
            objDatos.Mensaje("Forma de pago registrada correctamente", Me.Page)
            'txtNombre.Text = ""
            txtNumTarjeta.Text = ""
            'txtAñoExp.Text = ""
            'txtMesExp.Text = ""
            'txtCodSeguridad.Text = ""
            fnMetodosPagoCliente()
        End If
    End Sub
    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            ''Cargamos los estados
            Dim fila As DataRow
            ssql = objDatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objDatos.fnEjecutarConsultaSAP(ssql)
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

        End Try
    End Sub
    Protected Sub chkFactura_CheckedChanged(sender As Object, e As EventArgs) Handles chkFactura.CheckedChanged
        If chkFactura.Checked = True Then
            pnlFacturacion.Enabled = True
        Else
            pnlFacturacion.Enabled = False
        End If
    End Sub
End Class
