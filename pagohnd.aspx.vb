
Imports System.Data

Partial Class pagohnd
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones
    Private Sub pagohnd_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnTotales()

        If Not IsPostBack Then
            Try
                chkFactura.Checked = True
                pnlFacturacion.Enabled = True
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

            Try
                If Session("UserB2C") <> "" Then
                    ssql = "SELECT ISNULL(cvRFC,'') as RFC FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
                    Dim dtRFC As New DataTable
                    dtRFC = objDatos.fnEjecutarConsulta(ssql)
                    If dtRFC.Rows.Count > 0 Then
                        txtRFC.Text = dtRFC.Rows(0)(0)
                    End If
                End If
                txtNombreF.Text = Session("NombreEnvio")
                txtCalleF.Text = Session("CalleEnvio")

                txtCPF.Text = Session("CPEnvio")
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
        End If


    End Sub
    Public Sub fnTotales()

        Dim subtotal As Double = 0
        If CDbl(Session("ImporteSubTotal")) = 0 Then
            For Each Partida As Cls_Pedido.Partidas In Session("Partidas")
                If Partida.ItemCode <> "BORRAR" Then
                    subtotal = subtotal + Partida.Cantidad * Partida.Precio
                End If

            Next

            Session("ImporteSubTotal") = subtotal
        End If

        lblSubTotal.Text = Session("Moneda") & " " & CDbl(Session("ImporteSubTotal")).ToString("###,###,###.#0")
        lblEnvio.Text = Session("Moneda") & " " & CDbl(Session("ImporteEnvio")).ToString("###,###,###.#0")
        lblDescuento.Text = Session("Moneda") & " " & CDbl(Session("ImporteDescuento")).ToString("###,###,###.#0")
        lblTotal.Text = Session("Moneda") & " " & (CDbl(Session("ImporteSubTotal")) + CDbl(Session("ImporteEnvio")) + CDbl(Session("ImporteDescuento"))).ToString("###,###,###.#0")
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
    Protected Sub chkMismaDireccion_CheckedChanged(sender As Object, e As EventArgs) Handles chkMismaDireccion.CheckedChanged
        Try
            If Session("UserB2C") <> "" Then
                ssql = "SELECT ISNULL(cvRFC,'') as RFC FROM config.Usuarios where cvUsuario=" & "'" & Session("UserB2C") & "'"
                Dim dtRFC As New DataTable
                dtRFC = objDatos.fnEjecutarConsulta(ssql)
                If dtRFC.Rows.Count > 0 Then
                    txtRFC.Text = dtRFC.Rows(0)(0)
                End If
            End If
            txtNombreF.Text = Session("NombreEnvio")
            txtCalleF.Text = Session("CalleEnvio")

            txtCPF.Text = Session("CPEnvio")
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
    Protected Sub chkFactura_CheckedChanged(sender As Object, e As EventArgs) Handles chkFactura.CheckedChanged
        If chkFactura.Checked = True Then
            pnlFacturacion.Enabled = True
        Else
            pnlFacturacion.Enabled = False
        End If
    End Sub
    Protected Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        If chkFactura.Checked = False Then
            Response.Redirect("resumen.aspx")
            Exit Sub
        End If
        If txtCalleF.Text = "" Then
            objDatos.Mensaje("Debe indicar el domicilio", Me.Page)
            Exit Sub
        Else
            Session("CalleF") = txtCalleF.Text
        End If

        If txtRFC.Text = "" Then
            objDatos.Mensaje("Debe indicar el RTN", Me.Page)
            Exit Sub
        Else
            Session("RFC") = txtRFC.Text
        End If


        If ddlEstados.Text = "" Or ddlEstados.Text = "-Seleccione-" Then
            objDatos.Mensaje("Debe indicar el estado", Me.Page)
            Exit Sub
        Else
            Session("EstadoF") = ddlEstados.Text
        End If

        If txtNombreF.Text = "" Then
            objDatos.Mensaje("Debe indicar el nombre", Me.Page)
            Exit Sub
        Else
            Session("NombreF") = txtNombreF.Text
        End If
        'If txtNombreEmpresa.Text = "" Then
        '    objDatos.Mensaje("Debe indicar el nombre de empresa", Me.Page)
        '    Exit Sub
        'Else
        '    Session("NombreEmpresa") = txtNombreEmpresa.Text
        'End If



        Response.Redirect("resumen.aspx")
    End Sub
End Class
