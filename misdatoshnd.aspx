<%@ Page Language="VB" AutoEventWireup="false" CodeFile="misdatoshnd.aspx.vb" Inherits="misdatoshnd"MasterPageFile ="~/Main.master"  EnableEventValidation ="false" %>

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
						        	<label for='exampleInputEmail1'>Nombre y apellido</label>
						        	 <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Empresa</label>
						          <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>RTN  o identidad</label>
						        	 <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
						    	</div>
						    </div>
                                                                                  
						    <div class='col-xs-12 col-sm-6'>
						    	<div class='form-group'>
						        	<label for='exampleInputEmail1'>Email</label>
						        	 <asp:TextBox ID="txtemail" runat="server" CssClass="form-control" TextMode = "Email"></asp:TextBox>
						    	</div>
						    </div>
                     
						<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Teléfono *</label>
								        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
                                 	<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Celular *</label>
								        <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control"></asp:TextBox>
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
