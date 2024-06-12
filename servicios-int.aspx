<%@ Page Language="VB" AutoEventWireup="false" CodeFile="servicios-int.aspx.vb" Inherits="servicios_int"  MasterPageFile ="~/Main.master"  %>

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
            <div class="servicio">
                <asp:Panel ID="pnlServicio" runat="server"></asp:Panel>
                <div class="col-xs-12 no-padding">
					<a class="btn int" href="servicios.aspx">regresar</a>
				</div>
            </div>
        </div>
    </div>
</asp:Content>