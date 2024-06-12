<%@ page language="VB" autoeventwireup="false" inherits="Plantillas, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
     <div class="main-container">
		<h1 class="tit-seccion">Plantillas</h1>
	</div>
    <div class="wrappercon">
        <div class="main-container">
            
            <div class="col-xs-8 no-padding">
<table style="width: 100%;">
        <tr>
            <td><telerik:radgrid ID="rgvDocumentos" runat="server" Skin="Silk">
                 <MasterTableView ShowFooter="true" FooterStyle-HorizontalAlign="Right">
                  <Columns>
                                                           <telerik:GridTemplateColumn>
                                                               <ItemTemplate>
                                                                   <asp:CheckBox ID="chkSelect" runat="server" />
                                                               </ItemTemplate>
                                                           </telerik:GridTemplateColumn>
                      </Columns>    
                 </MasterTableView>
                </telerik:radgrid></td>
            
        </tr>
        <tr>
            <td> </td>
            <td></td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
               
            </div>
 <div class="col-xs-4 no-padding">
     <asp:Button ID="btnVer" CssClass ="btn btn-general-3" runat="server" Text="Ver" visible="true" />
     <asp:Button ID="btnUsar" CssClass ="btn btn-general-3" runat="server" Text="Usar" visible="true" />
     <asp:Button ID="btnCerrar" CssClass ="btn btn-general-3" runat="server" Text="Finalizar" visible="true" OnClientClick ="return confirm('¿Seguro de cerrar el documento?');" />
                    </div>

            <div class="col-xs-12">
                <telerik:radgrid ID="rgvPartidas" runat="server" Skin="Silk" Visible ="False" ShowFooter="True" >
<GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>

                 <MasterTableView FooterStyle-HorizontalAlign="Right" AutoGenerateColumns ="false" >
                 <Columns>
                       <telerik:GridBoundColumn DataField="cvItemCode" UniqueName="cvItemCode" HeaderText="CodProd"></telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="cvItemName" UniqueName="cvItemName" HeaderText="Producto"></telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cfCantidad" UniqueName="cfCantidad" HeaderText="Cant" DataFormatString="{0:#,###,###.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ><HeaderStyle HorizontalAlign="Right" /><ItemStyle HorizontalAlign="Right" /></telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="cfPRecio" UniqueName="cfPRecio" HeaderText="Precio" DataFormatString="{0:#,###,###.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" ><HeaderStyle HorizontalAlign="Right" /><ItemStyle HorizontalAlign="Right" /></telerik:GridBoundColumn>
                     <telerik:GridBoundColumn DataField="Total" UniqueName="Total" HeaderText="Total" DataFormatString="{0:#,###,###.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" Aggregate ="Sum"  ><HeaderStyle HorizontalAlign="Right" /><ItemStyle HorizontalAlign="Right" /></telerik:GridBoundColumn>
                 </Columns>

<FooterStyle HorizontalAlign="Right"></FooterStyle>
                 </MasterTableView>
                </telerik:radgrid>
                </div>
        </div>
    </div>

</asp:Content>