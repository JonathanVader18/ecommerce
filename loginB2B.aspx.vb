
Imports System.Data

Partial Class loginB2B
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones
    Public ssql As String

    Private Sub btnIngresar_Click(sender As Object, e As EventArgs) Handles btnIngresar.Click
        ''Hacemos el login
        If txtUser.Text = "" Then
            objDatos.Mensaje("Especifique un usuario", Me.Page)
            Exit Sub
        End If
        If txtPass.Text = "" Then
            objDatos.Mensaje("Especifique una contraseña", Me.Page)
            Exit Sub
        End If
        ssql = "SELECT * FROM Config.Parametrizaciones"
        Dim dtLogin As New DataTable
        Dim dtParam As New DataTable
        dtParam = objDatos.fnEjecutarConsulta(ssql)

        ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUser.Text & "' AND cvPass=" & "'" & txtPass.Text & "' "
        dtLogin = objDatos.fnEjecutarConsulta(ssql)
        If dtLogin.Rows.Count = 0 Then
            ''Buscamos en SAP
            ''Hay que buscar en SAP
            ssql = objDatos.fnObtenerQuery("Usuario")
            ssql = ssql.Replace("[%0]", txtUser.Text).Replace("[%1]", txtPass.Text)
            dtLogin = objDatos.fnEjecutarConsultaSAP(ssql)

        End If

        If dtLogin.Rows.Count > 0 Then
            Session("usrInvitado") = "NO"
            'If dtLogin.Rows(0)("cvCardCode") = "" Then
            '    objDatos.Mensaje("Acceso incorrecto", Me.Page)
            '    Exit Sub
            'End If

            Session("RazonSocial") = ""
            Session("UserB2C") = ""
            Session("NombreUserB2C") = ""
            Session("NombreuserTienda") = ""
            Session("CardCodeUserB2C") = ""
            Session("ImporteEnvio") = 0
            Session("ImporteDescuento") = 0


            Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
            Session("Cliente") = dtLogin.Rows(0)("cvCardCode")
            Session("UserTienda") = dtLogin.Rows(0)("cvUsuario")
            If dtLogin.Rows(0)("cvTipoAcceso") = "B2B" Then
                'objDatos.fnlog("Acceso SAP", "B2B")
                Session("UserB2C") = ""
                Session("slpCode") = 0
                Session("RazonSocial") = dtLogin.Rows(0)("cvNombreCompleto")



                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("Cliente"))
                'objDatos.fnlog("ListaPrecios", ssql.Replace("'", ""))
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtLista.Rows.Count > 0 Then
                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                Else
                    Session("ListaPrecios") = "1"
                End If

                'objDatos.fnlog("Acceso SAP Lista Precios", Session("ListaPrecios"))
                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                Session("Entrega") = New List(Of Cls_Pedido.DireccionesEntregas)
                Session("Facturacion") = New List(Of Cls_Pedido.DireccionFacturacion)

            Else
                Session("RazonSocial") = ""
                Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
                Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
                ' Session("Cliente") = dtLogin.Rows(0)("cvCardCode")

                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtLista.Rows.Count > 0 Then
                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                Else
                    Session("ListaPrecios") = "1"
                End If

                Response.Redirect("index.aspx")
                Exit Sub
            End If

            'btnCerrarSesion.Visible = True
            'pnlOpciones.Visible = True
            '  fnCargaMenuUser()
            If dtLogin.Rows(0)("cvTipoAcceso") = "B2B" Then

                ssql = "SELECT ISNULL(cvNombreComercial,'') as Nombre from config .DatosCliente  "
                Dim dtcliente As New DataTable
                dtcliente = objDatos.fnEjecutarConsulta(ssql)
                If dtcliente.Rows.Count > 0 Then
                    If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("BOSS") Then
                        ssql = objDatos.fnObtenerQuery("GetTypeCustomerBoss")
                        If ssql <> "" Then
                            ssql = ssql.Replace("[%0]", Session("Cliente"))
                            objDatos.fnLog("TipoB2B", ssql.Replace("'", ""))
                            Dim dtLista As New DataTable
                            dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                            If dtLista.Rows.Count > 0 Then
                                Session("TipoB2B") = dtLista.Rows(0)(0)
                                objDatos.fnLog("TipoB2B", Session("TipoB2B"))
                            End If
                        End If
                        Response.Redirect("index.aspx")
                    Else

                        If CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("PMK") Or CStr(dtcliente.Rows(0)(0)).ToUpper.Contains("AUTOPARTES IMPORTADAS DE OCCIDENTE") Then
                            Response.Redirect("index.aspx")
                        Else
                            If objDatos.fnObtenerCliente.ToUpper.Contains("ZEYCO") Then
                                Dim ckCliente As HttpCookie = New HttpCookie("Cliente", Session("Cliente"))
                                Response.Cookies.Add(ckCliente)

                                Session("IsB2B") = "SI"
                                objDatos.fnLog("IsB2B login", "SI")
                                Dim ckIsB2B As HttpCookie = New HttpCookie("IsB2B", "SI")
                                Response.Cookies.Add(ckIsB2B)

                                objDatos.fnLog("Cookie", Request.Cookies("Cliente").Value)
                                Session("Partidas") = New List(Of Cls_Pedido.Partidas)
                                objDatos.fnLog("Zeyco login IsB2B", Session("IsB2B"))
                                objDatos.fnLog("Zeyco login RazonSocial", Session("RazonSocial"))
                                objDatos.fnLog("Zeyco login Cliente", Session("Cliente"))

                            End If
                            Response.Redirect("catalogo.aspx")
                        End If

                    End If
                End If

            Else
                Response.Redirect("index.aspx")
            End If



        Else
            ''Revisamos si el acceso no es por un usuario B2C
            ssql = "SELECT * FROM config.Usuarios WHERE cvUsuario=" & "'" & txtUser.Text & "' AND cvPass=" & "'" & txtPass.Text & "' and cvTipoAcceso='B2C' "
            dtLogin = New DataTable
            dtLogin = objDatos.fnEjecutarConsulta(ssql)
            If dtLogin.Rows.Count > 0 Then
                Session("RazonSocial") = ""
                Session("UserB2C") = dtLogin.Rows(0)("cvUsuario")
                Session("NombreUserB2C") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("NombreuserTienda") = dtLogin.Rows(0)("cvNombreCompleto")
                Session("CardCodeUserB2C") = dtLogin.Rows(0)("cvCardCode")
                ' Session("Cliente") = dtLogin.Rows(0)("cvCardCode")

                ''en base al cliente, obtenemos cual es su lista de precios
                ssql = objDatos.fnObtenerQuery("ListaPrecioscliente")
                ssql = ssql.Replace("[%0]", Session("CardCodeUserB2C"))
                Dim dtLista As New DataTable
                dtLista = objDatos.fnEjecutarConsultaSAP(ssql)
                If dtLista.Rows.Count > 0 Then
                    Session("ListaPrecios") = dtLista.Rows(0)(0)
                Else
                    Session("ListaPrecios") = "1"
                End If
                Response.Redirect("index.aspx")
            Else
                objDatos.Mensaje("Acceso incorrecto", Me.Page)
                Exit Sub
            End If
            Exit Sub
        End If
    End Sub
End Class
