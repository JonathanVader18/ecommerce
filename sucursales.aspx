<%@ Page Language="VB" AutoEventWireup="false" CodeFile="sucursales.aspx.vb" Inherits="sucursales" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
			<h2 class="pag-cont-tit"> <asp:Label ID="lblTitulo" runat="server" Text="Nuestras Tiendas"></asp:Label></h2>
		</div>
	</div>
    	<div class="col-xs-12">
			<span class="linea-2"></span>
		<div class="main-container sep-esp flex-center">

			 <asp:Panel ID="pnlsucursales" runat="server"></asp:Panel>
				<%--<div class="col-xs-12 col-sm-6 col-md-4 sucursales">
                    
					<div class="cont-img"></div>
					<div class="descripcion">
						<h2>Oficina principal chamelecón</h2>
						<p>
							lorem ipsum es simplemente el texto de relleno de las imprentas y archivos de texto relleno.
						</p>
						<p>
							tel: (504) 2565-8882 cel:(504) 3392-6530
						</p>
						<p>
							admonventas@lazarus.hn
						</p>
					</div>
					
				</div>--%>
	
		</div>
	</div>
    </asp:Content>