
Partial Class logout
    Inherits System.Web.UI.Page

    Private Sub logout_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("UserTienda") = ""
        Session("NombreuserTienda") = ""
        Session("slpCode") = "0"
        Session("Cliente") = ""
        Session("RazonSocial") = ""
        ' Session("ListaPrecios") = ""

        Session("UserB2C") = ""
        Session("NombreUserB2C") = ""
        Session("NombreuserTienda") = ""
        Session("CardCodeUserB2C") = ""


        Session("sesBuscar") = ""
        Response.Redirect("index.aspx")

        Dim ckCliente As HttpCookie = New HttpCookie("Cliente", "")
        Response.Cookies.Add(ckCliente)
        Session("IsB2B") = "NO"
    End Sub
End Class
