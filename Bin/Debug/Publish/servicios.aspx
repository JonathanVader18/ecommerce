<%@ page language="VB" autoeventwireup="false" inherits="servicios, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
			<h2 class="pag-cont-tit">     <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h2>
       
		</div>
	</div>
    <div class="col-xs-12">
			<span class="linea-2"></span>
		<div class="main-container sep-esp">
            <asp:Panel ID="pnlServicios" runat="server"></asp:Panel>
        </div>
      </div>
</asp:Content>