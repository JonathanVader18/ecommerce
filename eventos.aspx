<%@ Page Language="VB" AutoEventWireup="false" CodeFile="eventos.aspx.vb" Inherits="eventos" MasterPageFile ="~/Main.master"  %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="col-xs-12">
		<div class="main-container">
			<h2 class="pag-cont-tit">eventos</h2>
		</div>
	</div>
    	<div class="col-xs-12 no-padding">
            <div class="main-container">
                <div class="col-xs-12 col-sm-8 col-md-9 no-padding-right">
                    <div class='listado-eventos'>
                        <asp:Panel runat ="server" id="pnlEntradas">

                </asp:Panel>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3 panel-redes no-padding">
                    <div class='row l-eventos'>
                        <h4 class='tit-eventos'>
                            <a href='eventos.aspx'>
                                <img src='img/l-b.png' alt='' />
                                próximos eventos
                            </a>
                        </h4>
                        <div class='fechas'>
						<div class='col-xs-12'>
                             <asp:Panel runat="server" ID="pnlProximos">
                            </asp:Panel>
							
						</div>
						<div class='col-xs-12'>
							<a class='btn btn-eventos' href='eventos.aspx' > VER MÁS </a>
						</div>
					</div>
                        
                        <div class='col-xs-12'>
                            <h4>categorías
                            </h4>
                            <ul class="lat-categorias">
                                <asp:Panel runat="server" ID="pnlCat">
                                </asp:Panel>

                            </ul>
                            <h4>Entradas Recientes
                            </h4>
                            <div class="noticias-laterales">
                                <asp:Panel runat="server" ID="pnlEntradasRecientes">
                                </asp:Panel>

                            </div>

                            <h4>síguenos
                            </h4>

                        </div>
                        </div>
                    </div>
            </div>

    	</div>
    </asp:Content>