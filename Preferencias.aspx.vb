
Imports System.Data

Partial Class Preferencias
    Inherits System.Web.UI.Page
    Public objDatos As New Cls_Funciones


    Private Sub Preferencias_Load(sender As Object, e As EventArgs) Handles Me.Load
        fnMenuPreferencias()

        If Not IsPostBack Then
            Dim ssql As String = ""
            ssql = "SELECT cvNombreCompleto,ISNULL(cvMail,cvUsuario) as Mail,ISNULL(cvTelefono1,'') as Tel1 , ISNULL(cvTelefono2,'') as Tel2, ISNULL(cvRFc,'') as RFC,ISNULL(cvNombreCompleto,'') as Nombre FROM config.Usuarios WHERE cvUsuario=" & "'" & Session("UserB2C") & "' and cvTipoAcceso='B2C' "
            Dim dtLogin As New DataTable
            dtLogin = objDatos.fnEjecutarConsulta(ssql)
            If dtLogin.Rows.Count > 0 Then
                lblnombre.Text = dtLogin.Rows(0)("cvNombreCompleto")

                lblMail.Text = Session("UserB2C")
                Try
                    lblRFC.Text = dtLogin.Rows(0)("RFC")
                    lbltel1.Text = dtLogin.Rows(0)("Tel1")
                    Session("RFC") = dtLogin.Rows(0)("RFC")
                Catch ex As Exception

                End Try


                ssql = "SELECT ISNULL(cvClienteLAT,'NO') FROM config.Parametrizaciones "
                Dim dtClienteLAT As New DataTable
                dtClienteLAT = objDatos.fnEjecutarConsulta(ssql)
                If dtClienteLAT.Rows.Count = 0 Then

                    Response.Redirect("pagoindex.aspx")
                Else
                    If dtClienteLAT.Rows(0)(0) = "SI" Then
                        ssql = "SELECT ISNULL(cvPaginaMiCuenta,'') FROM config.parametrizaciones_b2c "
                        Dim dtPagina As New DataTable
                        dtPagina = objDatos.fnEjecutarConsulta(ssql)
                        If dtPagina.Rows.Count > 0 Then
                            If CStr(dtPagina.Rows(0)(0)).Contains("hnd") Then
                                lblTextoRFC.Text = "RTN: "
                                pnlBotonhnd.Visible = True
                                pnlBoton.Visible = False
                                pnlBotonla.Visible = False
                            Else
                                lblTextoRFC.Text = "Cédula Fiscal: "
                                pnlBoton.Visible = False
                                pnlBotonla.Visible = True
                            End If
                        Else
                            lblTextoRFC.Text = "Cédula Fiscal: "
                            pnlBoton.Visible = False
                            pnlBotonla.Visible = True
                        End If


                    Else
                        lblTextoRFC.Text = "RFC: "
                        pnlBoton.Visible = True
                        pnlBotonla.Visible = False
                    End If
                End If



            End If
        End If
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
