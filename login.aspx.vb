Imports System.Collections.Generic
Imports System.Data
Partial Class login
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("slpCode") = "0"
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
        objDatos.fnLog("Login", "Entra")
        ''Revisamos el tipo de login. si es en SAP o en la base local
        ssql = "SELECT * FROM Config.Parametrizaciones"
        Dim dtParam As New DataTable
        dtParam = objDatos.fnEjecutarConsulta(ssql)
        If dtParam.Rows.Count > 0 Then
            If dtParam.Rows(0)("cvTipoLogin") = "LOCAL" Then

            Else


            End If
            objDatos.fnLog("Login", "Entra Param")
            ''Login con usuario local
            ssql = "SELECT * from config.Usuarios where cvUsuario=" & "'" & txtUser.Text & "' and cvPass=" & "'" & txtPass.Text & "'"
            Dim dtAcceso As New DataTable
            dtAcceso = objDatos.fnEjecutarConsulta(ssql)
            If dtAcceso.Rows.Count = 0 Then
                ''Hay que buscar en SAP
                ssql = objDatos.fnObtenerQuery("Usuario")
                ssql = ssql.Replace("[%0]", txtUser.Text).Replace("[%1]", txtPass.Text)
                Dim dtAccesoSAP As New DataTable
                dtAccesoSAP = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtAccesoSAP.Rows.Count = 0 Then
                    objDatos.Mensaje("Acceso incorrecto", Me.Page)
                Else
                    ''Si inició sessión
                    Session("UserTienda") = txtUser.Text
                    Session("NombreuserTienda") = dtAccesoSAP.Rows(0)("NombreCompleto")
                    Session("slpCode") = dtAccesoSAP.Rows(0)("VendedorSAP")


                    Response.Redirect("elegir-cliente.aspx")

                End If

                'objDatos.Mensaje("Acceso incorrecto", Me.Page)
            Else
                Session("Todosclientes") = "NO"
                Try
                    ssql = "SELECT isnull(cvTodosClientes,'NO') as Todos from config.Usuarios where cvUsuario=" & "'" & txtUser.Text & "' and cvPass=" & "'" & txtPass.Text & "'"
                    Dim dtTodos As New DataTable
                    dtTodos = objDatos.fnEjecutarConsulta(ssql)
                    If dtTodos.Rows.Count > 0 Then
                        If dtTodos.Rows(0)(0) = "SI" Then
                            Session("Todosclientes") = "SI"
                        End If

                    End If
                Catch ex As Exception

                End Try
                objDatos.fnLog("Login", "Inicia sesión")
                ''Si inició sessión
                Session("UserTienda") = txtUser.Text
                Session("NombreuserTienda") = dtAcceso.Rows(0)("cvNombreCompleto")

                If dtAcceso.Rows(0)("ciVendedorSAP") Is DBNull.Value Then
                    objDatos.Mensaje("Acceso incorrecto", Me.Page)
                    Exit Sub
                Else
                    If CInt(dtAcceso.Rows(0)("ciVendedorSAP")) = "0" Then
                        objDatos.Mensaje("Acceso incorrecto", Me.Page)
                        Exit Sub
                    End If
                End If
                objDatos.fnLog("Login", "Partidas")
                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

                Session("slpCode") = dtAcceso.Rows(0)("ciVendedorSAP")
                ' lblCliente.Visible = True
                'ddlClientes.Visible = True
                'btnSeleccionar.Visible = True
                objDatos.fnLog("Login", "Redirect")
                Response.Redirect("elegir-cliente.aspx")


            End If



        End If

    End Sub
    'Protected Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click
    '    Session("Cliente") = ddlClientes.SelectedValue
    '    Session("RazonSocial") = ddlClientes.SelectedItem.Text
    '    ''en base al cliente, obtenemos cual es su lista de precios
    '    ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
    '    ssql = ssql.Replace("[%0]", ddlClientes.SelectedValue)
    '    Dim dtLista As New DataTable
    '    dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
    '    If dtLista.Rows.Count > 0 Then
    '        Session("ListaPrecios") = dtLista.Rows(0)(0)
    '    Else
    '        Session("ListaPrecios") = "1"
    '    End If
    '    Session("Pedido") = New Cls_Pedido
    '    Try

    '    Catch ex As Exception

    '    End Try
    '    Session("Partidas") = New List(Of Cls_Pedido.Partidas)
    '    Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
    '    Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

    '    Response.Redirect("levantar-pedido.aspx")
    'End Sub
End Class
