<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editar-contraseña.aspx.vb" Inherits="editar_contraseña" MasterPageFile ="~/Main.master" %>

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
                                            <a href="index.aspx">salir</a>
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
		                <div class="tit-bloque"><h2>cambiar contraseña</h2></div>
	                </div>
 						<div class="content-blok">
						    <div class="col-xs-12 col-sm-6">
						    	<div class="form-group">
							        <label for="exampleInputPassword1">Escribe tu contraseña actual</label>
                                     <asp:TextBox ID="txtPassActual" runat="server" CssClass ="form-control"></asp:TextBox>
							        
							    </div>
							    <div class="form-group">
							        <label for="exampleInputPassword1">Nueva contraseña</label>
							         <asp:TextBox ID="txtNuevoPass" runat="server" CssClass ="form-control"></asp:TextBox>
							    </div>
							    <div class="form-group">
							        <label for="exampleInputPassword1">Confirmar contraseña</label>
							       <asp:TextBox ID="txtConfirmaNuevoPass" runat="server" CssClass ="form-control"></asp:TextBox>
							    </div>
                                <asp:Button ID="btnGuardar" runat="server" Text="guardar"  CssClass ="btn btn-general-2 btn-left"/>
                                <a type="submit" class="btn btn-general-1 btn-left" href="preferencias.aspx">cancelar</a>

							
								
						    </div>
						</div>
					
			     </div>
			</div>
		</div>
	</div>
    </asp:Content>