
Imports System.Data

Partial Class PagoLA
    Inherits System.Web.UI.Page

    Public ssql As String
    Public objdatos As New Cls_Funciones

    Private Sub PagoLA_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnTotales()

        If Not IsPostBack Then
            Try
                Dim fila As DataRow
                ''Cargamos los paises
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
                ddlEstados.DataSource = dtEstado
                ddlEstados.DataTextField = "Descripcion"
                ddlEstados.DataValueField = "Clave"
                ddlEstados.DataBind()
                ddlEstados.SelectedValue = "0"

                pnlFacturacion.Enabled = True
                chkMismaDireccion.Checked = True

                Try
                    If Session("UserB2C") <> "" Then
                        ssql = "SELECT ISNULL(cvRFC,'') as RFC FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
                        Dim dtRFC As New DataTable
                        dtRFC = objdatos.fnEjecutarConsulta(ssql)
                        If dtRFC.Rows.Count > 0 Then
                            txtRFC.Text = dtRFC.Rows(0)(0)
                        End If
                    End If


                    txtNombreF.Text = Session("NombreEnvio")
                    txtCalleF.Text = Session("CalleEnvio")
                    txtNumExtF.Text = Session("NumExtEnvio")
                    txtCPF.Text = Session("CPEnvio")
                    txtEstadoF.Text = Session("EstadoEnvio")
                    txtMunicipioF.Text = Session("MunicipioEnvio")
                    ddlPais.SelectedValue = Session("PaisEnvio")


                    ''Cargamos los estados

                    ddlEstados.SelectedValue = Session("EstadoEnvio")
                Catch ex As Exception

                End Try

            Catch ex As Exception

            End Try
        End If

    End Sub

    Public Sub fnTotales()
        lblSubTotal.Text = CDbl(Session("ImporteSubTotal")).ToString("$ ###,###,###.#0")
        lblEnvio.Text = CDbl(Session("ImporteEnvio")).ToString("$ ###,###,###.#0")
        lblDescuento.Text = CDbl(Session("ImporteDescuento")).ToString("$ ###,###,###.#0")
        lblTotal.Text = (CDbl(Session("ImporteSubTotal")) + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString("$ ###,###,###.#0")
    End Sub


    Protected Sub ddlPais_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            ''Cargamos los estados
            Dim fila As DataRow
            ssql = objdatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
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
    Protected Sub chkMismaDireccion_CheckedChanged(sender As Object, e As EventArgs) Handles chkMismaDireccion.CheckedChanged
        Try
            If Session("UserB2C") <> "" Then
                ssql = "SELECT ISNULL(cvRFC,'') as RFC FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
                Dim dtRFC As New DataTable
                dtRFC = objdatos.fnEjecutarConsulta(ssql)
                If dtRFC.Rows.Count > 0 Then
                    txtRFC.Text = dtRFC.Rows(0)(0)
                End If
            End If


            txtNombreF.Text = Session("NombreEnvio")
            txtCalleF.Text = Session("CalleEnvio")
            txtNumExtF.Text = Session("NumExtEnvio")
            txtCPF.Text = Session("CPEnvio")
            txtEstadoF.Text = Session("EstadoEnvio")
            txtMunicipioF.Text = Session("MunicipioEnvio")
            ddlPais.SelectedValue = Session("PaisEnvio")

            txtRFC.Text = Session("RFC")
            ''Cargamos los estados
            Dim fila As DataRow
            ssql = objdatos.fnObtenerQuery("Estados")
            ssql = ssql.Replace("[%0]", ddlPais.SelectedValue)
            Dim dtEstado As New DataTable
            dtEstado = objdatos.fnEjecutarConsultaSAP(ssql)
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
            Response.Redirect("resumen.aspx")
            Exit Sub
        End If
        If txtCalleF.Text = "" Then
            objdatos.Mensaje("Debe indicar la calle", Me.Page)
            Exit Sub
        Else
            Session("CalleF") = txtCalleF.Text
        End If

        If txtMunicipioF.Text = "" Then
            objdatos.Mensaje("Debe indicar el Municipio", Me.Page)
            Exit Sub
        Else
            Session("MunicipioF") = txtMunicipioF.Text
        End If

        If txtNumExtF.Text = "" Then
            objdatos.Mensaje("Debe indicar número exterior", Me.Page)
            Exit Sub
        Else
            Session("NumExtF") = txtNumExtF.Text
        End If
        If txtEstadoF.Text = "" Then
            objdatos.Mensaje("Debe indicar el estado", Me.Page)
            Exit Sub
        Else
            Session("EstadoF") = txtEstadoF.Text
        End If

        If txtCPF.Text = "" Then
            objdatos.Mensaje("Debe indicar el código postal", Me.Page)
            Exit Sub
        Else
            Session("CPF") = txtCPF.Text
        End If

        Session("NombreF") = txtNombreF.Text

        Response.Redirect("resumen.aspx")
    End Sub
    Protected Sub chkFactura_CheckedChanged(sender As Object, e As EventArgs) Handles chkFactura.CheckedChanged
        If chkFactura.Checked = True Then
            pnlFacturacion.Enabled = True
        Else
            pnlFacturacion.Enabled = False
        End If
    End Sub
End Class
