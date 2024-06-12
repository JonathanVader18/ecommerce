
Imports System.Data

Partial Class configUsuarios
    Inherits System.Web.UI.Page
    Public ssql As String
    Public objDatos As New Cls_Funciones


    Public Sub fnCargaUsuarios()
        ''Los banners activos
        ssql = "select ciIdConfig as Id,cvNombreCompleto as Nombre,cvUsuario as Usuario,ciVendedorSAP as VendedorSAP,cvMail as Correo from config.Usuarios where ISNULL (civendedorSAP,0) <>0 order by Nombre "
        Dim dtUsuarios As New DataTable
        dtUsuarios = objDatos.fnEjecutarConsulta(ssql)

        Dim sHTML As String = ""
        Dim sClass As String = "class='bg-danger text-white'"
        For i = 0 To dtUsuarios.Rows.Count - 1 Step 1

            sHTML = sHTML & "<tr>"
            Try
                If Request.QueryString.Count > 0 Then
                    If Request.QueryString("Id") = dtUsuarios.Rows(i)("Id") Then
                        sClass = "class='bg-danger text-white'"
                    Else
                        sClass = ""
                    End If
                Else
                    sClass = ""
                End If
            Catch ex As Exception
            End Try

            sHTML = sHTML & "<td " & sClass & "><a href='configusuarios.aspx?Id=" & dtUsuarios.Rows(i)("Id") & "'>" & dtUsuarios.Rows(i)("Id") & "</a></td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtUsuarios.Rows(i)("Nombre") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtUsuarios.Rows(i)("Usuario") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtUsuarios.Rows(i)("VendedorSAP") & "</td>"
            sHTML = sHTML & "<td " & sClass & ">" & dtUsuarios.Rows(i)("Correo") & "</td>"
            sHTML = sHTML & "</tr>"
        Next
        Dim literal As New LiteralControl(sHTML)
        pnlRegistros.Controls.Clear()
        pnlRegistros.Controls.Add(literal)
    End Sub

    Private Sub configUsuarios_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("loginAdmin") = "" Then
                Response.Redirect("loginadmin.aspx")
            End If

            ''Cargamos los vendedores de SAP

            If objDatos.fnObtenerDBMS.ToUpper.Contains("HANA") Then
                ssql = "SELECT ""SlpCode"" as Id, ""SlpName"" as Nombre FROM ""#BDSAP#"".""OSLP"" "
            Else
                ssql = "SELECT slpCode as Id, SlpName as Nombre FROM OSLP "
            End If

            If objDatos.fnObtenerDBMS = "HANA" Then
                ssql = objDatos.fnObtenerQuery("GetSalesPerson")
            End If
            Dim dtVend As New DataTable
            dtVend = objDatos.fnEjecutarConsultaSAP(ssql)
            ddlVendedorSAP.DataSource = dtVend
            ddlVendedorSAP.DataTextField = "Nombre"
            ddlVendedorSAP.DataValueField = "Id"
            ddlVendedorSAP.DataBind()


            Session("IdUserVendedor") = ""
            fnCargaUsuarios()

            If Request.QueryString.Count > 0 Then
                Session("IdUserVendedor") = Request.QueryString("Id")
                ''Obtenemos los datos para rellenar
                ssql = "select ciIdConfig as Id,ISNULL(cvNombreCompleto,'') as Nombre,cvUsuario as Usuario,ciVendedorSAP as VendedorSAP,ISNULL(cvMail,'') as Correo,ISNULL(cvPass,'') as Pass from config.Usuarios where  ciIdConfig= " & "'" & Request.QueryString("Id") & "'"
                Dim dtUser As New DataTable
                dtUser = objDatos.fnEjecutarConsulta(ssql)

                If dtUser.Rows.Count > 0 Then
                    txtNombre.Text = dtUser.Rows(0)("Nombre")
                    txtCorreo.Text = dtUser.Rows(0)("Correo")
                    txtPass.TextMode = TextBoxMode.SingleLine
                    txtPass.Text = dtUser.Rows(0)("Pass")
                    txtUsuario.Text = dtUser.Rows(0)("Usuario")
                    Try
                        ddlVendedorSAP.SelectedValue = dtUser.Rows(0)("VendedorSAP")
                    Catch ex As Exception

                    End Try
                    txtPass.TextMode = TextBoxMode.Password
                    lblPass.Visible = True
                    btnAgregar.Text = "Actualizar"
                End If
            End If

        End If
    End Sub
    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        Dim sPass As String = ""

        If txtNombre.Text = "" Then
            objDatos.Mensaje("Establezca un nombre", Me.Page)
            Exit Sub
        End If

        If txtUsuario.Text = "" Then
            objDatos.Mensaje("Establezca un usuario", Me.Page)
            Exit Sub
        End If

        If btnAgregar.Text <> "Actualizar" Then
            If txtPass.Text = "" Then
                objDatos.Mensaje("Establezca una contraseña", Me.Page)
                Exit Sub
            End If
            sPass = txtPass.Text
        Else
            If txtPass.Text = "" Then
                ssql = "select ciIdConfig as Id,ISNULL(cvNombreCompleto,'') as Nombre,cvUsuario as Usuario,ciVendedorSAP as VendedorSAP,ISNULL(cvMail,'') as Correo,ISNULL(cvPass,'') as Pass from config.Usuarios where  ciIdConfig= " & "'" & Request.QueryString("Id") & "'"
                Dim dtUser As New DataTable
                dtUser = objDatos.fnEjecutarConsulta(ssql)

                If dtUser.Rows.Count > 0 Then
                    sPass = dtUser.Rows(0)("Pass")
                End If
            Else
                sPass = txtPass.Text
            End If

        End If



            If btnAgregar.Text = "Actualizar" Then
            ''Eliminamos y dejamos el flujo natural de una inserción
            ssql = "DELETE FROM Config.Usuarios where ciIdConfig=" & "'" & Session("IdUserVendedor") & "'"
            objDatos.fnEjecutarInsert(ssql)
        End If

        ssql = "SELECT ISNULL(MAX(ciIdConfig),0) + 1 FROM Config.Usuarios "
        Dim dtId As New DataTable
        dtId = objDatos.fnEjecutarConsulta(ssql)


        ssql = "INSERT INTO config.Usuarios (ciIdConfig,cvNombreCompleto,cvUsuario,cvPass,ciVendedorSAP,cvMail,cvTipoAcceso) VALUES(" _
                & "'" & dtId.Rows(0)(0) & "'," _
                & "'" & txtNombre.Text & "'," _
                & "'" & txtUsuario.Text & "'," _
                & "'" & sPass & "'," _
                & "'" & ddlVendedorSAP.SelectedValue & "'," _
                & "'" & txtCorreo.Text & "','B2B')"


        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Usuario Guardado!", Me.Page)
        txtNombre.Text = ""
        txtUsuario.Text = ""
        txtPass.Text = ""
        txtCorreo.Text = ""
        Session("IdUserVendedor") = ""
        lblPass.Visible = False
        fnCargaUsuarios()
    End Sub
    Protected Sub btnEliminar_Click(sender As Object, e As EventArgs) Handles btnEliminar.Click
        ''Eliminamos y dejamos el flujo natural de una inserción
        ssql = "DELETE FROM Config.Usuarios where ciIdConfig=" & "'" & Session("IdUserVendedor") & "'"
        objDatos.fnEjecutarInsert(ssql)
        objDatos.Mensaje("Usuario eliminado!", Me.Page)
        fnCargaUsuarios()

    End Sub
    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        txtNombre.Text = ""
        txtUsuario.Text = ""
        txtPass.Text = ""
        txtCorreo.Text = ""
        Session("IdUserVendedor") = ""
        fnCargaUsuarios()
        lblPass.Visible = False
    End Sub
    Protected Sub ddlVendedorSAP_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlVendedorSAP.SelectedIndexChanged

    End Sub
    Protected Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("menuconfig.aspx")
    End Sub
End Class
