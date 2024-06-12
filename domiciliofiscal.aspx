<%@ Page Language="VB" AutoEventWireup="false" CodeFile="domiciliofiscal.aspx.vb" Inherits="domiciliofiscal" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
			<div class="col-xs-12 no-padding">
				<ol class="breadcrumb">
				  <li><a href="active">Domicilio Fiscal</a></li>
				
				</ol>
			</div>
			<div class="col-xs-12 col-sm-9 stl-1-p contenido">
                    <asp:Panel ID="pnlDireccion" runat="server" visible ="true" >
				<div class="blk-genericos extencion-2">
					<div class="form-general">
					 	<fieldset>
					 		<div class="tit-bloque"><h2>Confirmar datos de facturación </h2> </div>
                         
					 		<div class="row">
							    <div class="col-xs-12 col-sm-12">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">Nombre Fiscal *</label>
								         <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
							    	</div>
							    </div>
                                   <div class="col-xs-12 col-sm-6">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">RFC</label>
								         <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
							    	</div>
							    </div>
							  <div class="col-xs-12 col-sm-6">
								      <div class="form-group">
	                    <label for="exampleInputEmail1">uso de cfdi</label>
	                      <asp:DropDownList ID="ddlUsoCFDI" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
	                  </div>
								</div>

								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Calle *</label>
								       <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Número *</label>
								        <asp:TextBox ID="txtNumExt" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Número interior</label>
								        <asp:TextBox ID="txtNumInt" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Colonia</label>
								       <asp:TextBox ID="txtColonia" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">   
								    <div class="form-group">
								        <label for="exampleInputEmail1">Ciudad</label>
								       <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Localidad/Municipio</label>
								         <asp:TextBox ID="txtMunicipio" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">CP</label>
								        <asp:TextBox ID="txtCP" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
                                 	<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Teléfono</label>
								        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								      <div class="col-xs-12 col-sm-6">
                                     <div class="form-group">
                                         <label for="exampleInputEmail1">Estado *</label>
                                         <asp:DropDownList ID="ddlEstados" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                         <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                     </div>
                                 </div>

                                 <div class="col-xs-12 col-sm-6">
                                     <div class="form-group">
                                        <%-- <label for="exampleInputEmail1">País</label>--%>
                                         <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control" Visible ="false"></asp:DropDownList>
                                     </div>
								</div>
                           

								
							</div>
						</fieldset>
					</div>
				</div>
</asp:Panel>
					</div>
                 
				
                  <div class="col-xs-12 col-sm-3">
			
				
				<asp:Button ID="btnContinuar" runat="server" Text="continuar" CssClass ="btn btn-general-3" />
				
			</div>
                   
          
			</div>
			
		
	</div>
    </asp:Content>
