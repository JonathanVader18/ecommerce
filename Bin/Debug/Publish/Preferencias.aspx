<%@ page language="VB" autoeventwireup="false" inherits="Preferencias, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
            <div class="col-xs-12 col-sm-2">
                <div class="filtros-dektop">
                    <div class="panel-group filtos-catalogo" id="accordion" role="tablist" aria-multiselectable="true">
                        <div class="panel">
                            <div class="panel-heading" role="tab" id="headingOne">
                                <h4 class="categoria">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">mi cuenta
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                <div class="panel-body">
                                    <ul class="subcategorias">
                                        <asp:Panel ID="pnlMenuPref" runat="server"></asp:Panel>
                                     <%--   <li>
                                            <a href="preferencias.aspx">preferencias</a>
                                        </li>
                                        <li>
                                            <a href="direcciones.aspx">direcciones</a>
                                        </li>
                                      <li>
                                            <a href="mis-carritos.aspx">mis carritos</a>
                                        </li>
                                        <li>
                                            <a href="mispedidos.aspx">mis pedidos</a>
                                        </li>
                                        <li>
                                            <a href="logout.aspx">salir</a>
                                        </li>--%>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
			<div class="col-xs-12 col-sm-10 stl-1-p contenido">
				<div class="blk-genericos">
					<div class="col-sm-12">
						<div class="tit-bloque"><h2>Editar tus datos</h2></div>
					</div>
					<div class="content-blok">
						<div class="grup-content">
							<div class="singular-content col-xs-12 col-sm-12">
								<div>
									<strong>Nombre (s): </strong><asp:Label ID="lblnombre" runat="server" Text="Juan Pedro Solorzono"></asp:Label><br>
									<strong>
                                        <asp:Label ID="lblTextoRFC" runat="server" Text="Label"></asp:Label> </strong><asp:Label ID="lblRFC" runat="server" Text="PDRG90873CV0"></asp:Label><br>
									<strong>Email: </strong><asp:Label ID="lblMail" runat="server" Text="correo@midominio.com"></asp:Label><br>
									<strong>Télefono 1: </strong><asp:Label ID="lbltel1" runat="server" Text="5566445566"></asp:Label><br>
									
								</div>
								<div class="blk-action-btn">
                                    <asp:PlaceHolder ID="pnlBoton" runat="server" Visible ="true" >
                                        <a class="btn-act-blok " href="misdatos.aspx">Editar</a>
                                    </asp:PlaceHolder>
									 <asp:PlaceHolder ID="pnlBotonla" runat="server" Visible ="false" >
                                        <a class="btn-act-blok " href="misdatosla.aspx">Editar</a>
                                    </asp:PlaceHolder>
                                     <asp:PlaceHolder ID="pnlBotonhnd" runat="server" Visible ="false" >
                                        <a class="btn-act-blok " href="misdatoshnd.aspx">Editar</a>
                                    </asp:PlaceHolder>
								</div>
							</div>
							
						</div>
					</div>
				</div>
				<div class="blk-genericos">
					<div class="content-blok">
						<div class="grup-content">
							<div class="singular-content col-xs-12 col-sm-12">
								<div>
									<strong>contraseña (s): </strong>
                                    <asp:Label ID="lblPass" runat="server" Text="******" ></asp:Label><br>
								</div>
								<div class="blk-action-btn">
									<a class="btn-act-blok " href="editar-contraseña.aspx">Editar</a>
								</div>
							</div>
							
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>

    </asp:Content>
