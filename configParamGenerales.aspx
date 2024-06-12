<%@ Page Language="VB" AutoEventWireup="false" CodeFile="configParamGenerales.aspx.vb" Inherits="configParamGenerales" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
 .RadDropDownList { display:inline-block !important; 
                                               width: 160px !important; }
                            .RadDropDownList_Default{color:#333;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.RadDropDownList{width:160px;line-height:1.3333em;text-align:left;display:inline-block;vertical-align:middle;white-space:nowrap;cursor:default;*zoom:1;*display:inline} .RadDropDownList { display:inline-block !important; 
                                               width: 160px !important; }
                            .RadDropDownList_Default{color:#333;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.RadDropDownList{width:160px;line-height:1.3333em;text-align:left;display:inline-block;vertical-align:middle;white-space:nowrap;cursor:default;*zoom:1;*display:inline}.RadDropDownList{width:160px;line-height:1.3333em;text-align:left;display:inline-block;vertical-align:middle;white-space:nowrap;cursor:default;*zoom:1;*display:inline}
                            .RadDropDownList_Default{color:#333;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}
    .RadDropDownList { display:inline-block !important; 
                                               width: 160px !important; }
                            .RadDropDownList_Default .rddlInner{border-radius:3px;border-color:#cdcdcd;color:#333;background-color:#e6e6e6;background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radGradientButtonSprite.png');background-image:linear-gradient(#fff,#e6e6e6)}.RadDropDownList .rddlInner{vertical-align:top}.RadDropDownList .rddlInner{padding:2px 22px 2px 5px;border-width:1px;border-style:solid;display:block;position:relative;overflow:hidden}.RadDropDownList_Default .rddlInner{border-radius:3px;border-color:#cdcdcd;color:#333;background-color:#e6e6e6;background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radGradientButtonSprite.png');background-image:linear-gradient(#fff,#e6e6e6)}.RadDropDownList .rddlInner{vertical-align:top}.RadDropDownList .rddlInner{padding:2px 22px 2px 5px;border-width:1px;border-style:solid;display:block;position:relative;overflow:hidden}.RadDropDownList .rddlInner{padding:2px 22px 2px 5px;border-width:1px;border-style:solid;display:block;position:relative;overflow:hidden}.RadDropDownList .rddlInner{vertical-align:top}.RadDropDownList_Default .rddlInner{border-radius:3px;border-color:#cdcdcd;color:#333;background-color:#e6e6e6;background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radGradientButtonSprite.png');background-image:linear-gradient(#fff,#e6e6e6)}.RadDropDownList_Default .rddlDefaultMessage{filter:alpha(opacity=80);opacity:.8}.RadDropDownList .rddlDefaultMessage{font-style:italic}.RadDropDownList .rddlFakeInput{margin:0;padding:0;width:100%;min-height:16px;display:block;overflow:hidden}.RadDropDownList_Default .rddlDefaultMessage{filter:alpha(opacity=80);opacity:.8}.RadDropDownList .rddlDefaultMessage{font-style:italic}.RadDropDownList .rddlFakeInput{margin:0;padding:0;width:100%;min-height:16px;display:block;overflow:hidden}.RadDropDownList .rddlFakeInput{margin:0;padding:0;width:100%;min-height:16px;display:block;overflow:hidden}
                            .RadDropDownList .rddlDefaultMessage{font-style:italic}.RadDropDownList_Default .rddlDefaultMessage{filter:alpha(opacity=80);opacity:.8}
                            .rddlFakeInput {
                                    height: 16px !important; 
                                    width: 80% !important;}
                            .rddlFakeInput {
                                    height: 16px !important; 
                                    width: 80% !important;}
                            .rddlFakeInput {
                                    height: 16px !important; 
                                    width: 80% !important;}.RadDropDownList_Default .rddlIcon{background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radActionsSprite.png');background-position:-22px -20px}.RadDropDownList .rddlIcon{width:16px;height:100%;border:0;background-repeat:no-repeat;position:absolute;top:0;right:0}.RadDropDownList_Default .rddlIcon{background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radActionsSprite.png');background-position:-22px -20px}.RadDropDownList .rddlIcon{width:16px;height:100%;border:0;background-repeat:no-repeat;position:absolute;top:0;right:0}.RadDropDownList .rddlIcon{width:16px;height:100%;border:0;background-repeat:no-repeat;position:absolute;top:0;right:0}.RadDropDownList_Default .rddlIcon{background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radActionsSprite.png');background-position:-22px -20px}.rddlSlide{float:left;display:none;position:absolute;overflow:hidden;z-index:7000}.rddlSlide{float:left;display:none;position:absolute;overflow:hidden;z-index:7000}.rddlSlide{float:left;display:none;position:absolute;overflow:hidden;z-index:7000}.rddlPopup_Default{border-color:#a0a0a0;color:#333;background-color:#fff;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.rddlPopup{*zoom:1;border-width:1px;border-style:solid;text-align:left;position:relative;cursor:default;width:160px;*width:158px;box-sizing:border-box}.rddlPopup_Default{border-color:#a0a0a0;color:#333;background-color:#fff;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.rddlPopup{*zoom:1;border-width:1px;border-style:solid;text-align:left;position:relative;cursor:default;width:160px;*width:158px;box-sizing:border-box}.rddlPopup{*zoom:1;border-width:1px;border-style:solid;text-align:left;position:relative;cursor:default;width:160px;*width:158px;box-sizing:border-box}
    .rddlPopup_Default{border-color:#a0a0a0;color:#333;background-color:#fff;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}
        .auto-style1 {
            width: 166px;
        }
        .auto-style2 {
            width: 26px;
        }
        .auto-style3 {
            width: 166px;
            height: 26px;
        }
        .auto-style4 {
            width: 26px;
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
    
        <table style="width:100%;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label12" runat="server" Text="Parametrizaciones Generales"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label22" runat="server" Text="Licencia de producto"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtLicencia" runat="server" Width="650px"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnAgregar0" runat="server" Text="Cargar" />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style2">
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label23" runat="server" Text="Razón Social"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtRazonSocial" runat="server" Width="650px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label15" runat="server" Text="Servidor BD"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtServidorBD" runat="server" Width="650px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label16" runat="server" Text="Servidor Licencias"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtServidorLicencias" runat="server" Width="650px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label11" runat="server" Text="Tipo de Servidor BD"></asp:Label>
                </td>
                <td class="auto-style2">
                    <telerik:RadDropDownList ID="rcbTipoBD" runat="server" Enabled="False">
                    </telerik:RadDropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label17" runat="server" Text="Version SQL"></asp:Label>
                </td>
                <td class="auto-style2">
                    <telerik:RadDropDownList ID="rcbVersionSQL" runat="server" Enabled="False">
                    </telerik:RadDropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label14" runat="server" Text="User BD"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtUserBD" runat="server" Width="170px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label18" runat="server" Text="Pass BD"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtPassBD" runat="server" Width="170px" ReadOnly="True" TextMode="Password"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label19" runat="server" Text="User SAP"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtUserSAP" runat="server" Width="170px" ReadOnly="True"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label20" runat="server" Text="Pass SAP"></asp:Label>
                </td>
                <td class="auto-style2">
                    <asp:TextBox ID="txtPassSAP" runat="server" Width="170px" ReadOnly="True" TextMode="Password"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style2">
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label21" runat="server" Text="Lenguaje"></asp:Label>
                </td>
                <td class="auto-style2">
                    <telerik:RadDropDownList ID="rcbLenguaje" runat="server" Enabled="False">
                    </telerik:RadDropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style2">
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="Label24" runat="server" Text="Logo Empresa"></asp:Label>
                </td>
                <td class="auto-style4">
                    <asp:FileUpload ID="fUpload" runat="server" />
                    <asp:Button ID="btnAgregar1" runat="server" Text="Subir Imagen" />
                </td>
                <td rowspan="3">
                    <asp:Image ID="imgLogo" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Configurar Existencias"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style2">
                    <asp:CheckBox ID="chkTodas" runat="server" Text="Todos los almacenes" AutoPostBack="True" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td colspan="2">
                    <asp:Panel ID="pnlAlmacenes" runat="server" Visible="False">
                        <table style="width:100%;">
                            <tr>
                                <td>
                                    <asp:Label ID="Label25" runat="server" Text="Seleccione Almacén"></asp:Label>
                                </td>
                                <td>
                                    <telerik:RadDropDownList ID="rcbAlmacenes" runat="server" Enabled="False">
                                    </telerik:RadDropDownList>
                                </td>
                                <td><asp:Button ID="btnAgregar2" runat="server" Text="Agregar" /></td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <telerik:RadGrid ID="rgvAlmacenes" runat="server">
                                        <GroupingSettings CollapseAllTooltip="Collapse all groups" />
                                        <MasterTableView ShowFooter="true" ShowGroupFooter="true">
                                            <Columns>
                                                <telerik:GridButtonColumn CommandName="select" FilterControlAltText="Filter select column" HeaderText="Elegir" Text="Elegir" UniqueName="select">
                                                </telerik:GridButtonColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <HeaderContextMenu FeatureGroupID="rghcMenu">
                                        </HeaderContextMenu>
                                    </telerik:RadGrid>
                                </td>
                                <td>     </td>
                            </tr>
                            <tr>
                                <td><asp:Button ID="btnAgregar3" runat="server" Text="Quitar" /></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                    
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style2">
               
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;</td>
                <td class="auto-style2">
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">
                    <asp:Button ID="btnAgregar" runat="server" Text="Guardar Configuración" />
                </td>
                <td>&nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>