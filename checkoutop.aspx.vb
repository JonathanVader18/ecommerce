
Imports Openpay
Imports Openpay.Entities
Imports Openpay.Entities.Request

Partial Class checkoutop
    Inherits System.Web.UI.Page
    Public objdatos As New Cls_Funciones
    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        label1.Text = "Entró acá"





        Dim api = New OpenpayAPI("sk_e568c42a6c384b7ab02cd47d2e407cab", "mzdtln0bmtms6o3kck8f", False)
        api.Production = False
        Dim customer = New Customer()
        customer.Name = "jonathan "
        customer.LastName = "Peña"
        customer.PhoneNumber = "1234567896"
        customer.Email = "ventas@evolutic.mx"

        Dim request = New ChargeRequest()

        request.Method = "card"
        request.SourceId = txtToken.Text
        request.Amount = CDbl(Session("TotalCarrito"))
        request.Description = "Compra en BossFood"
        request.DeviceSessionId = txtDeviceId.Text
        request.Customer = customer



        Dim charge = api.ChargeService.Create(request)
        Try
            If charge.Status.ToUpper = "COMPLETED" Then
                Response.Redirect("confirmacioncompra.aspx?Ped=" & charge.Id)
            Else
                objdatos.Mensaje("Ha ocurrido un problema al intentar realizar el cobro:" & charge.Status & " " & charge.Description, Me.Page)
                label1.Text = charge.Status & " " & charge.Description
            End If
        Catch ex As Exception
            objdatos.Mensaje("Ha ocurrido un problema al intentar realizar el cobro:" & charge.Description, Me.Page)
            label1.Text = charge.Description
        End Try


    End Sub
End Class
