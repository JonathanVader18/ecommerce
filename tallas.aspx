<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tallas.aspx.vb" Inherits="tallas" MasterPageFile ="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server" >
	
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
       <div class="col-xs-12">
		<div >
            
		<h2 class="pag-cont-tit">     <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></h2>
        <asp:Panel ID="pnlContenido" runat="server"></asp:Panel>
		</div>
            
	</div>
   
 
  
   
    <div class="col-xs-12">
		<div class="main-container">
			<span class="linea top"></span>
			<div class="pag-cont-des">
               
                </div>

            <asp:Panel ID="pnlProductos" runat="server"></asp:Panel>
		</div>
	</div>
</asp:Content>