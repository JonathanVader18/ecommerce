
Imports System.Data
Partial Class editar_direccionla
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones

    Private Sub editar_direccionla_Load(sender As Object, e As EventArgs) Handles Me.Load
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

                        ' txtNumExt.Text = dtDireccion.Rows(0)("cvNumExt")

                        txtMunicipio.Text = dtDireccion.Rows(0)("cvMunicipio")
                        txtCP.Text = dtDireccion.Rows(0)("cvCP")
                        ddlEstados.SelectedValue = dtDireccion.Rows(0)("cvEstado")
                        ddlPais.SelectedValue = dtDireccion.Rows(0)("cvPais")

                    End If
                End If
            End If
        End If
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

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ''Editamos
        Dim IdDireccion As String = ""
        Dim Action As String = ""

        IdDireccion = Request.QueryString("dir")
        Action = Request.QueryString("action")

        If Action = "e" Then
            ssql = "UPDATE Tienda.Direcciones_Envio SET " _
                & " cvCalle=" & "'" & txtCalle.Text & "'," _
                & " cvNumExt=" & "'0'," _
                & " cvMunicipio=" & "'" & txtMunicipio.Text & "'," _
                & " cvEstado=" & "'" & ddlEstados.SelectedValue & "'," _
                & " cvCP=" & "'" & txtCP.Text & "'," _
                & " cvPais=" & "'" & ddlPais.SelectedValue & "' " _
                & " where ciIdRel=" & "'" & IdDireccion & "'"
            objDatos.fnEjecutarInsert(ssql)

        End If
        objDatos.Mensaje("Dirección actualizada", Me.Page)
    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("direccionesla.aspx")
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
