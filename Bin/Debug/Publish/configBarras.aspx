<%@ page language="VB" autoeventwireup="false" inherits="configBarras, App_Web_r2ayzcoz" %>


<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 23px;
        }
        .auto-style2 {
            margin-left: 0px;
        }
        .auto-style3 {
            width: 159px;
        }
        .auto-style4 {
            height: 23px;
            width: 193px;
        }
        .auto-style5 {
            width: 193px;
        }
        .RadDropDownList { display:inline-block !important; 
                                               width: 160px !important; }
                            .RadDropDownList_Default{color:#333;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.RadDropDownList{width:160px;line-height:1.3333em;text-align:left;display:inline-block;vertical-align:middle;white-space:nowrap;cursor:default;*zoom:1;*display:inline}.RadDropDownList_Default .rddlInner{border-radius:3px;border-color:#cdcdcd;color:#333;background-color:#e6e6e6;background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radGradientButtonSprite.png');background-image:linear-gradient(#fff,#e6e6e6)}.RadDropDownList .rddlInner{vertical-align:top}.RadDropDownList .rddlInner{padding:2px 22px 2px 5px;border-width:1px;border-style:solid;display:block;position:relative;overflow:hidden}.RadDropDownList_Default .rddlDefaultMessage{filter:alpha(opacity=80);opacity:.8}.RadDropDownList .rddlDefaultMessage{font-style:italic}.RadDropDownList .rddlFakeInput{margin:0;padding:0;width:100%;min-height:16px;display:block;overflow:hidden}
                            .rddlFakeInput {
                                    height: 16px !important; 
                                    width: 80% !important;}.RadDropDownList_Default .rddlIcon{background-image:url('mvwres://Telerik.Web.UI, Version=2016.2.607.35, Culture=neutral, PublicKeyToken=121fae78165ba3d4/Telerik.Web.UI.Skins.Default.Common.radActionsSprite.png');background-position:-22px -20px}.RadDropDownList .rddlIcon{width:16px;height:100%;border:0;background-repeat:no-repeat;position:absolute;top:0;right:0}.rddlSlide{float:left;display:none;position:absolute;overflow:hidden;z-index:7000}.rddlPopup_Default{border-color:#a0a0a0;color:#333;background-color:#fff;font-size:12px;font-family:"Segoe UI",Arial,Helvetica,sans-serif}.rddlPopup{*zoom:1;border-width:1px;border-style:solid;text-align:left;position:relative;cursor:default;width:160px;*width:158px;box-sizing:border-box}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <table style="width:100%;">
            <tr>
                <td class="auto-style1" colspan="2">
                    <asp:Label ID="Label1" runat="server" Text="Registrar Barra de Productos"></asp:Label>
                </td>
                <td class="auto-style4"></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style1"></td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Label ID="Label2" runat="server" Text="Nombre"></asp:Label>
                </td>
                <td class="auto-style5">
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="auto-style2" Width="471px"></asp:TextBox>
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td>
                    <asp:CheckBox ID="chkActivo" runat="server" Checked="True" Text="Activo" />
                </td>
                <td>
                    <asp:Label ID="lblIdPlantilla" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Label ID="Label13" runat="server" Text="Plantilla"></asp:Label>
                </td>
                <td class="auto-style5">
                    <telerik:RadDropDownList ID="rcbPlantilla" runat="server" AutoPostBack="True">
                    </telerik:RadDropDownList>
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnAgregarP0" runat="server" Text="Nueva Barra" Width="100%" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Button ID="btnAgregarP" runat="server" Text="Agregar Barra" Width="100%" />
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1" colspan="2">
                    <asp:Label ID="Label12" runat="server" Text="Agregar Productos a la Barra"></asp:Label>
                </td>
                <td class="auto-style4"></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style1"></td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Label ID="Label14" runat="server" Text="Barra"></asp:Label>
                </td>
                <td class="auto-style5">
                    <telerik:RadDropDownList ID="rcbBarra" runat="server" AutoPostBack="True">
                    </telerik:RadDropDownList>
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td>
                    <asp:Button ID="btnEliminar0" runat="server" Text="Eliminar Barra" Width="100%" />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Label ID="Label11" runat="server" Text="Producto"></asp:Label>
                </td>
                <td class="auto-style5">
                    <telerik:RadDropDownList ID="rcbProducto" runat="server">
                    </telerik:RadDropDownList>
                </td>
                <td class="auto-style5">
                    &nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Button ID="btnAgregar" runat="server" Text="Agregar" Width="100%" />
                </td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    &nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td colspan="4">
                    <telerik:RadGrid ID="rgvBarra" runat="server">
                         <MasterTableView ShowFooter ="true" ShowGroupFooter ="true">
                                             <Columns>
                                                  <telerik:GridButtonColumn CommandName="select" FilterControlAltText="Filter select column" HeaderText="Elegir" Text="Elegir" UniqueName="select">
                                                 </telerik:GridButtonColumn>
                                                 </Columns> 
                                                    </MasterTableView>
                    </telerik:RadGrid>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">
                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" Width="100%" />
                </td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td class="auto-style5">&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
