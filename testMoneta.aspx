<%@ Page Language="VB" AutoEventWireup="false" CodeFile="testMoneta.aspx.vb" Inherits="testMoneta" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
        .auto-style2 {
            width: 184px;
        }
        .auto-style3 {
            height: 26px;
            width: 184px;
        }
        .auto-style4 {
            width: 100%;
            height: 179px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="auto-style4">
                <tr>
                    <td>&nbsp;</td>
                    <td style="text-align: right">
                        <asp:Label ID="Label1" runat="server" Text="Código Moneta(OTP)"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCodMoneta" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style2" >
                        <asp:Label ID="Label4" runat="server" Text="SystemDate"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="245px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1"></td>
                    <td class="auto-style1" style="text-align: right">
                        <asp:Label ID="Label2" runat="server" Text="Importe"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtImporte" runat="server"></asp:TextBox>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="Label3" runat="server" Text="MerchantId"></asp:Label>
                    </td>
                    <td class="auto-style1">
                        <asp:TextBox ID="txtMerchantId" runat="server" ReadOnly="True" Width="305px">400c8a816d45cbaa016d4b60138c0004</asp:TextBox>
                    </td>
                    <td class="auto-style1"></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Procesar" Width="254px" />
                    </td>
                    <td class="auto-style2">
                        <asp:Label ID="Label5" runat="server" Text="AuditNumber"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAudit" runat="server" ReadOnly="True">123456</asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="Button2" runat="server" Text="Nueva Prueba" Width="254px" />
                    </td>
                    <td>
                        <asp:Label ID="lblMensaje" runat="server" Text="Respuesta WS:"></asp:Label>
                    </td>
                    <td class="auto-style2">
                        <asp:Label ID="Label6" runat="server" Text="Orig Trans Id"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrigTransId" runat="server" ReadOnly="True" Width="300px">1234567890123456789012345678601</asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td colspan="3" style ="height :450px">
                        <asp:TextBox ID="txtRespuesta" runat="server" Height="100%" TextMode="MultiLine" Width="100%"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
