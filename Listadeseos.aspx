<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Listadeseos.aspx.vb" Inherits="Listadeseos"MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="main-container">
		<h1 class="tit-seccion">Lista de deseos</h1>
	</div>
     <div class="wrappercon">
		<div class="main-container">
			<div class="col-xs-8 no-padding wishlist">
				<div class="header-tabla col-xs-12 no-padding">
					<div class="producto col-xs-2">
					</div>
					<div class="col-tabla col-xs-4">
						artículo
					</div>
					<div class="col-tabla col-xs-2">
						precio
					</div>
					<div class="col-tabla col-xs-2">
						cantidad
					</div>
					<div class="col-tabla col-xs-1">
						total
					</div>
				</div>
			   <asp:Panel runat="server"  ID="pnlLista">

    </asp:Panel>

				
			</div>
            </div>
         </div>
 
    </asp:Content>