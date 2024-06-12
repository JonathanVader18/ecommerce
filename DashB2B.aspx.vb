
Imports System.Data

Partial Class DashB2B
    Inherits System.Web.UI.Page

    Public ssql As String = ""
    Public objdatos As New Cls_Funciones

    Private Sub DashB2B_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            fnDatosGenerales()

            fnCreaDashboard()
        End If
    End Sub

    Public Sub fnDatosGenerales()

        ssql = objdatos.fnObtenerQuery("GetCustomerInfo")
        If ssql <> "" Then
            ssql = ssql.Replace("[%0]", Session("Cliente"))

            Dim dtGenerales As New DataTable
            dtGenerales = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtGenerales.Rows.Count > 0 Then
                lblNombre.Text = "Nombre:" & "</br>" & dtGenerales.Rows(0)("Nombre")
                lblContacto.Text = "Contacto:" & "</br>" & dtGenerales.Rows(0)("Contacto")
                lblDireccion.Text = "Direccion:" & "</br>" & dtGenerales.Rows(0)("Direccion")
            End If
        End If
    End Sub

    Public Sub fnCreaDashboard()

        Dim sHtmlEncabezado As String = ""

        ssql = "SELECT cvNombre,cvImagen,cvLink,cvEstilo,ISNULL(cvQuery,'') as Query FROM Config.Menus where cvTipoMenu='Lateral' order by ciOrden "
        Dim dtMenus As New DataTable
        dtMenus = objdatos.fnEjecutarConsulta(ssql)


        Dim sHtmlMenu As String = ""
        For i = 0 To dtMenus.Rows.Count - 1 Step 1

            sHtmlMenu = sHtmlMenu & fnGeneraOpcionMenuB2B(dtMenus.Rows(i)("cvNombre"), dtMenus.Rows(i)("cvImagen"), dtMenus.Rows(i)("Query"), dtMenus.Rows(i)("cvLink"), dtMenus.Rows(i)("cvEstilo"))

        Next
        sHtmlEncabezado = sHtmlEncabezado & sHtmlMenu
        Dim literal As New LiteralControl(sHtmlEncabezado)
        pnlDash.Controls.Clear()
        pnlDash.Controls.Add(literal)

    End Sub

    Public Function fnGeneraOpcionMenuB2B(Nombre As String, imagen As String, valor As String, link As String, Estilo As String) As String
        Dim sHtmlMenu As String = ""

        If link <> "" Then
            sHtmlMenu = sHtmlMenu & "<a href='" & link & "'>"
        End If

        sHtmlMenu = sHtmlMenu & " <div class='col-xs-12 col-sm-4'> "
        sHtmlMenu = sHtmlMenu & "  <div class='panel-cubic flex-int-cubic'> "
        sHtmlMenu = sHtmlMenu & "   <label>" & Nombre & "</label> "

        If valor <> "" Then
            Dim dtValorMenu As New DataTable
            ssql = objdatos.fnObtenerQuery(valor)
            '      ssql = ssql.Replace("[%0]", Session("slpCode"))
            ssql = ssql.Replace("[%1]", "'" & Session("Cliente") & "'")
            dtValorMenu = objdatos.fnEjecutarConsultaSAP(ssql)
            If dtValorMenu.Rows.Count = 0 Then
                sHtmlMenu = sHtmlMenu & "<div class='number-state'>0</div> "
            Else
                If Estilo = "importe" Then
                    Dim importe As Double = 0
                    If dtValorMenu.Rows(0)(0) Is DBNull.Value Then
                        importe = 0
                    Else
                        importe = dtValorMenu.Rows(0)(0)
                    End If

                    If Nombre.ToUpper.Contains("SALDO") Then
                        sHtmlMenu = sHtmlMenu & "<div class='number-state down'>" & importe.ToString("$ ###,###,###,###.#0") & "</div> "
                    Else
                        sHtmlMenu = sHtmlMenu & "<div class='number-state'>" & importe.ToString("$ ###,###,###,###.#0") & "</div> "
                    End If



                Else
                    If Estilo = "texto" Then
                        sHtmlMenu = sHtmlMenu & "<div class='number-state'>" & dtValorMenu.Rows(0)(0) & "</div> "
                    Else
                        sHtmlMenu = sHtmlMenu & "<div class='number-state'>" & dtValorMenu.Rows.Count & "</div> "
                    End If

                End If

            End If

        End If
        sHtmlMenu = sHtmlMenu & " </div> "
        sHtmlMenu = sHtmlMenu & "</div> "
        If link <> "" Then
            sHtmlMenu = sHtmlMenu & "</a>"
        End If
        Return sHtmlMenu
    End Function
    '
End Class
