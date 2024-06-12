<%@ page language="VB" autoeventwireup="false" inherits="bienvenida, App_Web_skzvnhfn" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="col-xs-12">
		<div class="main-container con-principal">
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
                                       <%-- <li>
                                            <a href="#">métodos de pago</a>
                                        </li>--%>
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
                    <div class="col-sm-12">
                        <div class="tit-bloque">
                            <h2>TU CUENTA SE HA CREADO CON ÉXITO</h2>
                        </div>
                    </div>
                    <div class="content-blok">
                        <div class="grup-content">
                            <div class="singular-content col-xs-12 col-sm-12">
                                <div>
                                    <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label><br />
                                    <strong> <asp:Label ID="lblBienvenida" runat="server" Text="Label"></asp:Label></strong>
                                </div>

                                <div class="blk-action-btn">
									<a class="btn-act-blok " href="catalogo.aspx">ir a la tienda</a>
								</div>
                                  <div class="blk-action-btn">
                                <asp:Button ID="btnRegresar" runat="server" Text="regresar al pago" cssclass="btn-act-blok" Visible ="false"  />
 </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            </div>
         </div>

    </asp:Content>