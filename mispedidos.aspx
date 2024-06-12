<%@ Page Language="VB" AutoEventWireup="false" CodeFile="mispedidos.aspx.vb" Inherits="mispedidos" MasterPageFile ="~/Main.master" %>

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
                                       <%-- <li>
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
					<div class="tit-bloque"><h2>pedidos</h2></div>
                   <asp:DropDownList ID="ddlFiltro" runat="server" class="select-general " AutoPostBack ="true" ></asp:DropDownList>
				
				</div>
				<table class="table" id="no-more-tables">
					<thead>
						<tr>
							<th class="text-center">articulo</th>
							<th class="text-center">fecha</th>
							<th class="text-center">orden</th>
							<th class="text-center">estatus</th>
							<th class="text-center">total</th>
							<th></th>
						</tr>	
					</thead>
					<tbody>	
						
						<tr>
                            <asp:Panel ID="pnlLista" runat="server"></asp:Panel>
						<%--	<td data-title="Articulo"><img src="<?=$base?>/img/playeras/img-<?=$i+1?>.png" class="img-responsive;">palyera scotter</td>
							<td data-title="Fecha" class="text-center" >17/09/17</td>
							<td data-title="Orden" class="text-center" >12678934</td>
							<td data-title="Estatus" class="text-center" >enviado</td>
							<td data-title="Total" class="text-center" >$100.00</td>
							<td class="esp" ><a class="btn btn-general-1" href="detalle-pedido.aspx">DETALLES</a></td>--%>
						</tr>
						
					</tbody>
					<!-- <tfoot>
						<tr>
							<td colspan="2" class="hidde-small"></td>
							<td class="text-center hidde-small">Total</td>
							<td class="text-center"></td>
							<td class="text-center" data-title="TOTAL">><strong>$</strong></td>
						</tr>
					</tfoot> -->
				</table>
			</div>

		</div>
	</div>
    </asp:Content>
