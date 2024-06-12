<%@ page language="VB" autoeventwireup="false" inherits="detalle_pedido, App_Web_fy50zy4w" masterpagefile="~/Main.master" %>

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
                                        <li>
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
                                        </li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
		
            <div class="col-xs-12 col-sm-10 stl-1-p contenido">
			    <div class="blk-genericos">
					<div class="tit-bloque"><h2>Detalles de pedido</h2><a href="mispedidos.aspx" class="add">Regresar a la lista</a></div>
						<div class="content-blok">
							<div class="grup-content">
								<div class="secondary-content col-xs-12 col-sm-12">
									<div>
                                        <asp:Label ID="lblFecha" runat="server" Text="Fecha del pedido: 11/30/2015"></asp:Label><br>
										<asp:Label ID="lblOrden" runat="server" Text="Orden:127282992"></asp:Label><br>
										<asp:Label ID="lblEstatus" runat="server" Text="Estatus:Enviado"></asp:Label><br>
										<strong><asp:Label ID="lblRastreo" runat="server" Text="Número de rastreo:6729339219292" Visible ="false" ></asp:Label></strong>
									</div>
								</div>
							</div>
						</div>
				</div>
				<div class="blk-genericos back-gray">
						<div class="content-blok">
							<div class="grup-content">
								<div class="secondary-content col-xs-12 col-sm-12">
									<div>
										<strong><asp:Label ID="lblEnviado" runat="server" Text=""></asp:Label></strong>
										<p><asp:Label ID="lblCalle" runat="server" Text=""></asp:Label><br>
                                            <asp:Label ID="lblColoniaCiudad" runat="server" Text=""></asp:Label><br>
                                           <asp:Label ID="lblPaisCP" runat="server" Text=""></asp:Label></p>
									</div>
								</div>
								
							</div>
						</div>
				</div>
				<table class="table" id="no-more-tables">
					<thead>
						<tr>
							<th></th>
							<th class="text-center">articulo</th>
							<th class="text-center">precio</th>
							<th class="text-center">cantidad</th>
							<th class="text-center">total</th>
						</tr>	
					</thead>
					<tbody>	
						
						<tr>
                            <asp:Panel ID="pnlPartidas" runat="server"></asp:Panel>
							<%--<td><img src="/img/playeras/img-<?=$i+1?>.png" class="img-responsive;"></td>
							<td data-title="Articulo">
								<div class="tit-prod"><strong>nombre producto</strong></div>
								descripción corta <br>
								<br>
								Estilo: 7237-1796<br>
								Color: Gray<br>
								Tamaño: S<br>
								Cantidad: 2</td>
							<td data-title="Fecha" class="text-center" >$100.00</td>
							<td data-title="Orden" class="text-center" >1</td>
							<td data-title="Estatus" class="text-center" >$100.00</td>--%>
						</tr>
					
					</tbody>
				</table>
				<%--<div class="blk-genericos extencion">
					<div class="tit-bloque"><h2>Método de pago</h2></div>
						<div class="content-blok">
							<div class="grup-content">
								<div class="secondary-content col-xs-12 col-sm-12">
									<div class="col-xs-2 no-padding">
											<img src="<?=$base?>/img/masterCard.png" class="img-responsive">
										</div>
										<div class="col-xs-10">
											Terminación:2210<br>
											Exp. 10/2020<br>
											Dirección: calle Angosta 212-501<br>
										</div>
								</div>
							</div>
						</div>
				</div>--%>
				<div class="blk-genericos extencion-2">
					<div class="col-xs-12 col-sm-4 totales">
						<h4>RESUMEN DE COMPRA</h4>
						<hr>
							<div class="col-xs-12 no-padding">
								
									<div class="col-xs-6 label">Subtotal:</div><div class="col-xs-6 cant"><asp:Label ID="lblSubtotal" runat="server" Text="Label"></asp:Label></div>
									<div class="col-xs-6 label">Envío:</div><div class="col-xs-6 cant"><asp:Label ID="lblenvio" runat="server" Text="Label"></asp:Label></div>
									<div class="col-xs-6 label">Descuentos:</div><div class="col-xs-6 cant"><asp:Label ID="lblDescuento" runat="server" Text="Label"></asp:Label></div>
								
							</div>
						<hr>
							<div class="col-xs-12 no-padding">
								
									<div class="col-xs-8 label">Total:</div><div class="col-xs-4 cant"><asp:Label ID="lblTotal" runat="server" Text="Label"></asp:Label></div>
								
							</div>
					</div>
				</div>
			</div>

		</div>
	</div>
    </asp:Content>