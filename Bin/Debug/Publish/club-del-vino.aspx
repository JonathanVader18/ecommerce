<%@ page language="VB" autoeventwireup="false" inherits="club_del_vino, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server" >
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <div class="col-xs-12">
		<div >
            
		<h2 class="pag-cont-tit">     <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h2>
       
		</div>
            
	</div>
   
 <asp:panel ID="pnlBanner" runat="server" CssClass ="pag-cont-banner "  > </asp:panel>
  
   
    <div class="col-xs-12">
		<div class="main-container">
			<span class="linea top"></span>
			<div class="pag-cont-des">
                <asp:Panel ID="pnlContenido" runat="server"></asp:Panel>
                </div>

            <asp:Panel ID="pnlProductos" runat="server"></asp:Panel>
		</div>
	</div>
</asp:Content>