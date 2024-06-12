<%@ Page Language="VB" AutoEventWireup="false" CodeFile="configCategorias.aspx.vb" Inherits="configCategorias" %>

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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
    
        <table style="width:100%;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="Label12" runat="server" Text="Niveles/Categorías de productos"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label15" runat="server" Text="Descripción"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" runat="server" Width="650px"></asp:TextBox>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label11" runat="server" Text="Campo"></asp:Label>
                </td>
                <td>
                    <telerik:RadDropDownList ID="rcbAtributo" runat="server">
                    </telerik:RadDropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label14" runat="server" Text="Lenguaje"></asp:Label>
                </td>
                <td>
                    <telerik:RadDropDownList ID="rcbLenguaje" runat="server">
                    </telerik:RadDropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label ID="Label13" runat="server" Text="Orden"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="rcbOrden" runat="server">
                        <asp:ListItem>1</asp:ListItem>
                        <asp:ListItem>2</asp:ListItem>
                        <asp:ListItem>3</asp:ListItem>
                        <asp:ListItem>4</asp:ListItem>
                        <asp:ListItem>5</asp:ListItem>
                        <asp:ListItem>6</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td>
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar Nivel" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadGrid ID="rgvCategorias" runat="server">
<GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>

                         <MasterTableView ShowFooter ="true" ShowGroupFooter ="true">
                                             <Columns>
                                                  <telerik:GridButtonColumn CommandName="select" FilterControlAltText="Filter select column" HeaderText="Elegir" Text="Elegir" UniqueName="select">
                                                 </telerik:GridButtonColumn>
                                                 </Columns> 
                                                    </MasterTableView>
                         <HeaderContextMenu FeatureGroupID="rghcMenu">
                         </HeaderContextMenu>
                    </telerik:RadGrid>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Button ID="btnAgregar0" runat="server" Text="Eliminar" />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
