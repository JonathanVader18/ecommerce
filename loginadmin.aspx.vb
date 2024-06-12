
Imports System.Data

Partial Class loginadmin
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String
    Private Sub loginadmin_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("loginAdmin") = ""
        End If

    End Sub
    Protected Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        If txtUser.Text = "" Then
            objDatos.Mensaje("Ingresar usuario", Me.Page)
            Exit Sub
        End If
        If txtPass.Text = "" Then
            objDatos.Mensaje("Ingresar contraseña", Me.Page)
            Exit Sub
        End If

        ''Revisamos el tipo de login. si es en SAP o en la base local
        ssql = "SELECT * FROM Config.Parametrizaciones"
        Dim dtParam As New DataTable
        dtParam = objDatos.fnEjecutarConsulta(ssql)
        If dtParam.Rows.Count > 0 Then
            If dtParam.Rows(0)("cvTipoLogin") = "LOCAL" Then

            Else


            End If

            ''Login con usuario local
            ssql = "SELECT * from config.Usuarios where cvUsuario=" & "'" & txtUser.Text & "' and cvPass=" & "'" & txtPass.Text & "' and cvTipoAcceso='Admin'"
            Dim dtAcceso As New DataTable
            dtAcceso = objDatos.fnEjecutarConsulta(ssql)
            If dtAcceso.Rows.Count = 0 Then


                objDatos.Mensaje("Acceso incorrecto", Me.Page)
            Else
                ''Si inició sessión
                Session("loginAdmin") = txtUser.Text
                Session("NombreuserTienda") = dtAcceso.Rows(0)("cvNombreCompleto")


                If CStr(objDatos.fnObtenerCliente).ToUpper.Contains("ZEYCO") Then
                    Response.Redirect("menuconfig.aspx")
                Else
                    Response.Redirect("menuconfig.aspx")
                End If



            End If



        End If
    End Sub
End Class
