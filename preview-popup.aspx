<%@ Page Language="VB" AutoEventWireup="false" CodeFile="preview-popup.aspx.vb" Inherits="preview_popup" %>
<%@ Reference VirtualPath="~/Carrito.aspx" %>
<div id="mfp-build-tool" class="prev-modal" >
      <link rel="stylesheet" href="//netdna.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css">
    <style type ="text/css" >
        
div.stars {
  width: 270px;
  display: inline-block;
}

input.star { display: none; }

label.star {
  float: right;
  padding: 10px;
  font-size: 36px;
  color: #444;
  transition: all .2s;
}

input.star:checked ~ label.star:before {
  content: '\f005';
  color: #FD4;
  transition: all .25s;
}

input.star-5:checked ~ label.star:before {
  color: #FE7;
  text-shadow: 0 0 20px #952;
}

input.star-1:checked ~ label.star:before { color: #F62; }

label.star:hover { transform: rotate(-15deg) scale(1.3); }

label.star:before {
  content: '\f006';
  font-family: FontAwesome;
}
    </style>
    <button title="Close (Esc)" type="button" class="mfp-close" style="background:gray">×</button>
			<div class="main-container wrap-product">
			<div class="row">
				<div class="col-xs-12 col-sm-6">
					<div class="col-sm-3">
						<div class="product-nav">
                          
                            <asp:PlaceHolder runat="server" id="pnlThum"></asp:PlaceHolder>
							<%--<div class="thumbnail"><img src="img/catalogo/playera.png" class="img-responsive" alt="descrip imagen"></div>
							<div class="thumbnail"><img src="img/home/producto-1.png" class="img-responsive" alt="descrip imagen"></div>
							<div class="thumbnail"><img src="img/catalogo/playera.png" class="img-responsive" alt="descrip imagen"></div>--%>
						</div>
                        
					</div>
					<div class="col-sm-9">
						<div class="product-for">
                            <asp:PlaceHolder runat="server" id="pnlZoom"></asp:PlaceHolder>
						<%--	<div><img class="zoom img-responsive" src="img/catalogo/playera.png" class="img-responsive" alt="descrip imagen" data-zoom-image="img/catalogo/playera2.jpg"/></div>
							<div><img class="zoom img-responsive" src="img/home/producto-1.png"" class="img-responsive" alt="descrip imagen" data-zoom-image="img/catalogo/playera3.jpg"/></div>
							<div><img class="zoom img-responsive" src="img/catalogo/playera.png" class="img-responsive" alt="descrip imagen" data-zoom-image="img/catalogo/playera2.jpg"/></div>--%>
							
						</div>
					</div>
				</div>
                
				<div class="col-xs-12 col-sm-6 info-producto-int">
						<div class="p-descripcion">
							<span class="heart"></span>
							<h1 class="titulo"><asp:label runat="server" text="" id="lblTitulo"></asp:label></h1>
							<div class="descripcion"><asp:label runat="server" text="" id="lblDescripcion"></asp:label></div>
						</div>
                         <asp:PlaceHolder ID="pnlRating" runat ="server" Visible ="false" ></asp:PlaceHolder>
						<div class="col-xs-12 no-padding">
                            <form id="form1" runat="server">
                                <asp:scriptmanager runat="server" enablepagemethods="true"></asp:scriptmanager>
                                 <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                    
<asp:panel runat="server" id="pnlTallaColor" visible="false">
                               <div class="form-group">
                                <label class="col-xs-12 col-sm-2 no-padding"> <asp:Label runat ="server"  id="lblAtr1" Text ="" Visible ="false" > </asp:Label></label>
                                <div class="col-xs-12 col-sm-10">
                                    <asp:DropDownList ID="ddlAtr1" runat="server" Visible ="false" autopostback="true" Width ="100%"></asp:DropDownList> 
                                </div>
                            </div>

                              <div class="form-group">
                                <label class="col-xs-12 col-sm-2 no-padding"> <asp:Label runat ="server"  id="lblAtr2" Text ="" Visible ="false" ></asp:Label></label>
                                <div class="col-xs-12 col-sm-10">
                                    <asp:DropDownList ID="ddlAtr2" runat="server" Visible ="false" autopostback="true" Width ="100%"> </asp:DropDownList> 
                                </div>
                            </div>
                              <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding"> <asp:Label runat ="server"  id="lblAtr3" Text ="presentación" Visible ="false" ></asp:Label></label>
                                <div class="col-xs-12 col-sm-8">
                                    <asp:DropDownList ID="ddlAtr3" runat="server" Visible ="false" autopostback="true" Width ="100%"></asp:DropDownList> 
                                </div>
                            </div>

                              <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding"> <asp:Label runat ="server"  id="lblAtr4" Text ="" Visible ="false" ></asp:Label></label>
                                <div class="col-xs-12 col-sm-8">
                                    <asp:DropDownList ID="ddlAtr4" runat="server" Visible ="false" autopostback="true" Width ="100%" ></asp:DropDownList> 
                                </div>
                            </div>

                             </asp:panel>

                        <asp:panel runat="server" id="pnlfichascolor" ></asp:panel>

<div class="col-xs-12 no-padding sec-prec ">
							<small class="precio-org"><asp:label runat="server" text="" id="lblPrecio"></asp:label></small><span class="prec-promo">$99.00</span>
						</div>
                        <div class="col-xs-12 no-padding sec-prec"><div class="precio-desctoPal"> <asp:Label runat="server" ID="lblPreciodesc" Text="" Visible ="false" ></asp:Label></div></div>
                        <div class="col-xs-12 no-padding sec-prec">
                      	<div class="form-group">	
                                   <asp:panel runat="server" ID="pnlExistencia" Visible ="true"><asp:Label ID="lblExistencia" runat="server" Text=""></asp:Label></asp:panel><br />
                              
								<%--<label class="col-xs-12 col-sm-4 no-padding">medida:</label>
								<div class="col-xs-12 col-sm-8">
									<select class="form-control">
										<option>S</option>
										<option>M</option>
										<option>L</option>
										<option>XL</option>
									</select>
								</div>--%>
							</div>
                            </div>
                    </ContentTemplate>
                                        <Triggers>
                                             <asp:AsyncPostBackTrigger ControlID="ddlAtr1" EventName="SelectedIndexChanged" />
                                           <asp:AsyncPostBackTrigger ControlID="ddlAtr2" EventName="SelectedIndexChanged" />
                                             <asp:AsyncPostBackTrigger ControlID="ddlAtr3" EventName="SelectedIndexChanged" />
                                             <asp:AsyncPostBackTrigger ControlID="ddlAtr4" EventName="SelectedIndexChanged" />


                                        </Triggers>

                                   
                     </asp:UpdatePanel>

                                
                            </form>
							<%--<div class="progress">
							  <span class="starts"><img src="img/catalogo/favorito.png"></span>
							  <div class="progress-bar" role="progressbar" aria-valuenow="70"
							  aria-valuemin="0" aria-valuemax="100" style="width:70%">
							    <span class="sr-only">70% Complete</span>
							  </div>
							</div>--%>
						</div>
						
						<div class="col-xs-12 no-padding">
							<div class="form-group">
						<%--	<label class="col-xs-12 col-sm-4 no-padding">Color:</label>
        <div class="col-xs-12 col-sm-8">
         <select class="form-control">
          <option>Azul</option>
          <option>Naranja</option>
          <option>Rojo</option>
          <option>Verde</option>
         </select>--%>
        </div>
							</div>						
				
						<div class="col-xs-12 col-sm-12 no-padding">
						
						</div>
						<div class="col-xs-12 col-sm-12 no-padding">
							<div class="form-group">	
                                 <asp:Panel ID="pnlCantidad" runat="server" Visible="true">
								  <label class="col-xs-12 col-sm-4 no-padding">cantidad:</label>
                                <br />
								<div class="col-xs-12 col-sm-8">
									<input type="numeric" class="form-control sprin" value="1" id="#Cantidad">	
								</div>
                                     </asp:Panel>
							</div>
						</div>

                     <asp:Panel runat="server" ID="pnlDescuento" Visible ="false" >
                            <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding">% descuento:</label>
                                  <br />
                                <div class="col-xs-12 col-sm-8">
                                    <input type="numeric" class="form-control" value="0" id="#Desc">
                                </div>
                            </div>

                        </asp:Panel>

                    		</div>
                 
                     
						<div class="col-xs-12">
                            <asp:panel runat="server" id="pnlEditar" visible="false">
                                <a class="btn btn-general-2" id="btneditar"  onclick="PageMethods.CargarCarrito(document.getElementById('#Cantidad').value,document.getElementById('#Desc').value, '', onSucess, onError);   function onSucess(result) { PopUp('', 'Artículo editado', 'Aceptar','','','',event); } function onError(result) {}">editar</a>
                            </asp:panel>
                             <asp:panel runat="server" id="pnlEditarsinDesc" visible="false">
                                <a class="btn btn-general-2" id="btneditarSinDesc"  onclick="PageMethods.EditarCarritoSinDesc(document.getElementById('#Cantidad').value, '', onSucess, onError);   function onSucess(result) { PopUp('', 'Artículo editado', 'Aceptar','','','',event); } function onError(result) {}">editar</a>
                            </asp:panel>
                              <asp:panel runat="server" id="pnlAgregar" visible="false">
                                    <%--<a class="btn btn-general-2" id="#btnAgregar"  onclick="PageMethods.CargarCarrito(document.getElementById('#Cantidad').value,'0', '', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','','',event); } function onError(result) {}">agregar</a>--%>
                                  <a class="btn btn-general-2" id="#btnAgregar"  onclick="PageMethods.CargarCarrito(document.getElementById('#Cantidad').value,document.getElementById('#Desc').value, '', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','','',event); } function onError(result) {}">agregar</a>
                                  </asp:panel>
                                <asp:panel runat="server" id="pnlAgregarSinDesc" visible="false">
                                  <a class="btn btn-general-2" id="#btnAgregarSinDesc"  onclick="PageMethods.CargarCarritoSinDesc(document.getElementById('#Cantidad').value, '', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','','',event); } function onError(result) {alert('err' + JSON.stringify(result));}">agregar</a>
                                  </asp:panel>
				    
						</div>
				</div>
			</div>
		</div>
    


<div id="myModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title">Información</h4>
            </div>
            <div class="modal-body">
                <p>One fine body&hellip;</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
