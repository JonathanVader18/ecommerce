<%@ page language="VB" autoeventwireup="false" inherits="Playground, App_Web_3bsbwt4p" %>
<%--<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>--%>
<%@ Import Namespace="mercadopago" %>
<%@ Import Namespace="System.Collections" %>

  <%--<% 
      Dim mp As MP = New MP("3215552140914099", "que5OV1ia0dBByAEAdEnoiLiqw6lcq9g")
      mp.sandboxMode(True)
      Dim accessToken = mp.getAccessToken()
      ' Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"",""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""}, ""quantity"": 1,""currency_id"":""MXN"",""unit_price"":" & lblTotal.Text.Replace("$", "") & "}]}")
      Dim preference = mp.createPreference("{""items"":[{""title"":""Pedido BACÁN:"", ""quantity"": 1,""currency_id"":""UYU"",""unit_price"":" & "1.00" & "}],""payer"": {""email"": ""CorreoComprador@gmail.com""},""back_urls"": {""success"": ""http://bacan.ga/ecommerce/confirmacion.aspx"",""failure"": ""http://www.failure.com"",""pending"": ""http://www.pending.com""},""auto_return"": ""approved""}")
        %>--%>

<!DOCTYPE html>
<html lang="en">
<head>
<meta charset="UTF-8">
<title>Disallow Bootstrap Modal from Closing</title>
<link rel="stylesheet" href="assets/css/bootstrap.min.css">
<link rel="stylesheet" href="assets/css/bootstrap-theme.min.css">
<script src="https://code.jquery.com/jquery-1.11.2.min.js"></script>

    <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
<script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>

  <script type="text/javascript" src="js/jspdf.min.js"></script>
</head>
<body>
     <form runat="server" >

              <asp:Panel ID="pnlTemplate" runat="server" Visible ="true" >
          <ul class="resumen">
              <li><asp:Label ID="Label8" runat="server" Text="cargue el template"></asp:Label></li>
                    	<li><div> <asp:FileUpload ID="fuExcel" runat="server" Width="100%" /></div>

                    	</li>
              <li> <asp:Button ID="btnSubir" runat="server" Text="subir" CssClass="btn btn-general-3" /></li>
              <li><a href="/template/template.xlsx" target="_blank">Descargar plantilla para pedidos</a></li>
              </ul>

     </asp:Panel>

         <br />
 <button id="cmd">generate PDF</button>
         <div id="content">

         <p>Testing pdf</p>
             <asp:Button ID="Button5" runat="server" Text="SEPO" />
         <asp:Button ID="Button1" runat="server" Text="Button" /><br />
              <asp:Button ID="btnPaqueteria" runat="server" Text="Generar guia paqueteria" /><br />
             <asp:TextBox ID="txtguia" runat="server" Text ="047900107"></asp:TextBox>
         <asp:Label ID="lblMensaje" runat="server" Text="Label"></asp:Label>

             <asp:Button ID="Button2" runat="server" Text="conexión SAP" />
                <asp:Button ID="Button3" runat="server" Text="Articulos sin imagen" />
             </div>
          <asp:Button ID="btnMoneta" runat="server" Text="Validar cadenas moneta" /><br />
          <asp:TextBox ID="txtCodMoneta" runat="server"  Width ="100%"></asp:TextBox><br />
         <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="245px" visible ="true"> </asp:TextBox><br />
                <asp:TextBox ID="txtMerchantId" runat="server" ReadOnly="True" Width="305px" visible="true"></asp:TextBox><br />
         <asp:TextBox ID="txtAudit" runat="server" ReadOnly="True" visible="true"></asp:TextBox><br />
         <asp:TextBox ID="txtOrigTransId" runat="server" ReadOnly="True" Width="300px" visible="true"></asp:TextBox><br />
         <asp:Button ID="btnCrystal" runat="server" Text="Crystal Report" />
         <asp:Button ID="Button4" runat="server" Text="btn Mail" />
         <br />
         <asp:TextBox ID="txtMenu1" runat="server" TextMode="MultiLine" Width="650px" Height="450px"></asp:TextBox><br />
         <asp:TextBox ID="txtmenu2" runat="server" TextMode="MultiLine" Width="650px" Height="450px"></asp:TextBox><br />
         <asp:TextBox ID="txtmenuresponsive" runat="server" TextMode="MultiLine" Width="650px" Height="450px"></asp:TextBox><br />
         <asp:Button ID="Button6" runat="server" Text="Desencriptar" />
         <asp:Button ID="Button7" runat="server" Text="Encriptar" />
          <asp:Button ID="btnImportarArticulos" runat="server" Text="Importar Artìculos" />
          <div class="row ">
                                    <div class="col-xs-12 col-sm-6 ">
                                         <label for="exampleInputEmail1">Seleccionar template</label>
                                        <asp:FileUpload ID="FileUpload1" runat="server" Width="390px" />
                                    </div>
                                   
                                </div>
           <div class="row ">
                                    <div class="col-xs-12 col-sm-2 ">
                                        <asp:Button ID="btnValidar" runat="server" Text="validar" CssClass="btn btn-general-2" />
                                    </div>
                                </div>
</form>
    <%--<a href="<% Response.Write(preference.Item("response")("init_point")) %>">Pay</a>--%>

</body>
    <script type="text/javascript" >
        var doc = new jsPDF();
var specialElementHandlers = {
    '#editor': function (element, renderer) {
        return true;
    }
};

$('#cmd').click(function () {
    doc.fromHTML($('#content').html(), 15, 15, {
        'width': 170,
            'elementHandlers': specialElementHandlers
    });
    doc.save('sample-file.pdf');
});

    </script>
</html>