Imports mercadopago
Imports mercadopago.Resources
Imports mercadopago.DataStructures.Payment
Imports mercadopago.Common

Imports System.Collections

Partial Class Default2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim mp As MP = New MP("ACCESS_TOKEN")

        Dim payment As Hashtable = mp.post("/v1/payments", "{" & """transaction_amount"": 100," & """token"": ""ff8080814c11e237014c1ff593b57b4d""," & """description"": ""Title of what you are paying for""," & """installments"": 1," & """payment_method_id"": ""visa""," & """payer"": {" & """email"": ""test_user_19653727@testuser.com""" & "}" & "}")
        Response.Write(payment)
    End Sub
    Public Shared Sub Main()
        Dim mp As MP = New MP("ACCESS_TOKEN")
        Dim payment As Hashtable = mp.post("/v1/payments", "{" & """transaction_amount"": 100," & """token"": ""ff8080814c11e237014c1ff593b57b4d""," & """description"": ""Title of what you are paying for""," & """installments"": 1," & """payment_method_id"": ""visa""," & """payer"": {" & """email"": ""test_user_19653727@testuser.com""" & "}" & "}")
        'Response.Write(payment)
    End Sub
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim mp As MP = New MP("3215552140914099", "que5OV1ia0dBByAEAdEnoiLiqw6lcq9g")
        mp.sandboxMode(True)


        Dim accessToken = mp.getAccessToken()
        Response.Write(accessToken)
        '3215552140914099-050322-63a16af32acafc4fa407837ca879492a-280977523

        'Dim preferenceResult = mp.getPreference("visa")

        'Response.Write(preferenceResult)

        'Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"",""quantity"":1,""currency_id"":""MXN"",""unit_price"":250}]}")
        Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":250}],""payer"": {""email"": ""user@email.com""},""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""},""auto_return"": ""approved""}")
        Response.Write(preference)

        Response.Write(preference.Item("response")("sandbox_init_point"))

        'Dim sComando As String
        'sComando = "<script type='text/javascript'> var opciones='left=100,top=100,width=650,height=450';window.open('" & preference.Item("response")("sandbox_init_point") & "','Ventana',opciones);</script> "
        'Response.Write(sComando)

        Dim sComando As String
        sComando = "<script type='text/javascript'> window.open('" & preference.Item("response")("sandbox_init_point") & "','_blank'); </script> "
        Response.Write(sComando)

        '280977523-3c69aa39-2eab-46c3-b7cc-a3f312b4b94c

        'Response.Write(preference.i["sandbox_init_point"]);

        Dim payment As Hashtable = mp.post("/v1/payments", "{" & """transaction_amount"": 1," & """token"": ""63a16af32acafc4fa407837ca879492a""," & """description"": ""Title of what you are paying for""," & """installments"": 1," & """payment_method_id"": ""visa""," & """payer"": {" & """email"": ""test_user_19653727@testuser.com""" & "}" & "}")
        Response.Write(payment)
    End Sub
End Class
