<%@ page language="VB" autoeventwireup="false" inherits="misdatosla, App_Web_fy50zy4w" masterpagefile="~/Main.master" enableeventvalidation="false" %>

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
            
        <div class='col-xs-12 col-sm-10 stl-1-p contenido'>
				<div class='form-general blk-genericos'>
					<fieldset>
 						<div class="tit-bloque">Edita tus datos</div>
 						<div class='row'>
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Nombre </label>
						        	 <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Apellidos</label>
						          <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Cédula fiscal</label>
						        	 <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
                            <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Cédula de identificación</label>
						        	 <asp:TextBox ID="txtCveIdentificacion" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>

                               <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Fecha de nacimiento</label>
                                  
                                 
    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" ></asp:TextBox>
    
                                   
						        	 
						    	</div>
						    </div>

						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Email</label>
						        	 <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" TextMode = "Email"></asp:TextBox>
						    	</div>
						    </div>
                         <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Sexo</label>
                                       <asp:DropDownList ID="ddlSexo" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control" ></asp:DropDownList>
						        	 <asp:TextBox ID="txtTel2" runat="server" CssClass="form-control" Visible ="false"  ></asp:TextBox>
						    	</div>
						    </div>
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Teléfono 1</label>

						        	 <asp:TextBox ID="txtTel1" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
						 
						    <div class='col-xs-12'>
                                  <asp:Button ID="btnGuardar" runat="server" Text="guardar"  CssClass ="btn btn-general-2 btn-left"/>
                                 <asp:Button ID="btnCancelar" runat="server" Text="cancelar"  CssClass ="btn btn-general-1 btn-left"/>
		
							</div>
						</div>
					</fieldset>
			     </div>
			</div>
        </div>
    </div>
 
    </asp:Content>
