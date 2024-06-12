<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pagoindex.aspx.vb" Inherits="pago_index"  EnableEventValidation ="false" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">

         <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


                                  <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <span style="border-width: 0px; position: fixed; padding: 50px; background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                <img src="LOADER_mm.gif">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
		<div class="main-container">
			<div class="col-xs-12 no-padding">
				<ol class="breadcrumb">
				  <li><a href="active">INICIO</a></li>
				  <li><a href="#">ENVÍO</a></li>
				  <li class="#">PAGO</li>
				</ol>
			</div>
			<div class="col-xs-12 col-sm-9 stl-1-p contenido">
                <asp:placeholder runat="server" ID="pnlIngresar" Visible ="true" >
				<div class="col-xs-12 col-sm-9">
					<div class="blk-genericos">
						<div class="tit-bloque"><h2><asp:CheckBox ID="chkInvitado" runat="server" AutoPostBack="true" Checked ="true" />&nbsp;CONTINUAR COMO INVITADO</h2></div>
                        <asp:Panel ID="pnlInvitado" runat="server" Visible="true">
                            <div class="form-general">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">correo electrónico</label>
                                    <asp:TextBox ID="txtCorreoInvitado" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </asp:Panel>
					</div>
                 
				</div>
                    <asp:Panel ID="pnlRegistrar" runat="server" Visible ="false">
	<div class="col-xs-12 col-sm-9">
					<div class="blk-genericos">
						<div class="tit-bloque"><h2>INGRESA TU USUARIO Y CONTRASEÑA SI YA ESTAS REGISTRADO</h2></div>
						<div  class="form-general"> 
		                  <div class="form-group">
		                    <label for="exampleInputEmail1">correo electrónico</label>
		                   <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
		                  </div>
		                  <div class="form-group">
		                    <label for="exampleInputPassword1">contraseña</label>
		                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode ="Password" ></asp:TextBox>
		                  </div>
		                  <div>
		                    <a href="mi-cuenta.aspx">haz click aquí para registrarte</a><br>
	                        <a class="link-lp" href="recuperar.aspx">¿olvidaste tu contraseña?</a>
		                  </div>
		                 <asp:Button ID="btnEntrar" runat="server" Text="Entrar"  CssClass ="btn btn-general-6" OnClientClick ="ShowProgress();"/>

		                </div>
					</div>
				</div>
                    </asp:Panel>

                    <asp:Panel ID="pnlCheckRegalo" runat="server" Visible="false">
                        <div class="col-xs-12 col-sm-9">
                            <div class="blk-genericos">
						            <div class="tit-bloque"><h2><asp:CheckBox ID="chkRegalo" runat="server" AutoPostBack="true" Checked ="false" />&nbsp;ESTA COMPRA ES UN REGALO</h2></div>
                                </div>
                            
                            </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlRegaloDelta" runat="server" Visible ="false" >
                        <div class="col-xs-12 col-sm-9">
					<div class="blk-genericos">
						<div class="tit-bloque"><h2>SELECCIONA ALGUNO DE LOS MENSAJES PERSONALIZADOS</h2></div>
						<div  class="form-general"> 
		                  <div class="form-group">
		                    <label for="exampleInputEmail1">Tipo de mensaje</label>
		                     <asp:DropDownList ID="ddlTipoMensaje" runat="server" Width="100%" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
		                  </div>
		                  <div class="form-group">
		                    <label for="exampleInputPassword1">Mensaje</label>
		                    <asp:TextBox ID="txtmensaje" runat="server" CssClass="form-control"  ></asp:TextBox>
		                  </div>
		                  
		                

		                </div>
					</div>
				</div>

                    </asp:Panel>


                    <div class="col-xs-12 col-sm-9">
			 <div class="form-group">
						<div class="checkbox-2">
                            <asp:Panel ID="pnlOfertas" runat="server" Visible ="false" >
						    <label>
						        <asp:CheckBox ID="chkOfertas" runat="server" Text ="" />
                                   Quiero recibir ofertas y promociones
						    </label>
                                </asp:Panel>
                              <asp:Panel ID="pnlOfertasStop" runat="server" Visible ="false" >
						    <label>
						        <asp:CheckBox ID="chkOfertasStop" runat="server" Text ="" />
                                  Suscríbete aL Newsletter
						    </label>
                                </asp:Panel>
					  	</div>
					  </div>
                        </div>
                    </asp:placeholder>
                <asp:Panel ID="pnlDireccion" runat="server" visible ="false" >
				<div class="blk-genericos extencion-2">
					<div class="form-general">
					 	<fieldset>
					 		<div class="tit-bloque"><h2>DIRECCIONES DE ENVÍO </h2> </div>
                             <div class="row">
                                   <div class="col-xs-12 col-sm-6">
							    	<div class="form-group">
                                        <asp:Panel ID="pnlCombodirecciones" runat ="server" Visible ="true">
                                         <label for="exampleInputEmail1">Indique una dirección</label>
								         <asp:DropDownList ID="ddlDirecciones" runat="server"  Width ="100%" AutoPostBack="True"  CssClass="form-control"></asp:DropDownList>
                                        </asp:Panel>
								       
							    	</div>
							    </div>
                             </div>
					 		<div class="row">
							    <div class="col-xs-12 col-sm-12">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">Nombre *</label>
								         <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
							    	</div>
							    </div>
                                   <div class="col-xs-12 col-sm-4">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">RFC</label>
								         <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"  style="text-transform:uppercase;"></asp:TextBox>
							    	</div>
							    </div>
							  <div class="col-xs-12 col-sm-4">
								      <div class="form-group">
	                    <label for="exampleInputEmail1">uso de cfdi (opcional)</label>
	                      <asp:DropDownList ID="ddlUsoCFDI" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
	                  </div>
								</div>
                                 
                                     <div class="col-xs-12 col-sm-4">
                                         <div class="form-group">
<asp:Panel ID="pnlFormaPagoHawk" runat="server" Visible="false">
                                             <label for="exampleInputEmail1">forma de pago</label>
                                             <asp:DropDownList ID="ddlFormasPago" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
 </asp:Panel>
                                         </div>
                                     </div>
                                
                                  </div>
                             <div class="row">
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
								        <label for="exampleInputEmail1">CP *</label>
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
								        <label for="exampleInputEmail1">Localidad/Municipio</label>
								         <asp:TextBox ID="txtMunicipio" runat="server" CssClass="form-control"></asp:TextBox>
                                         <asp:DropDownList ID="ddlLocalidad" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control" Visible ="false" ></asp:DropDownList>
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
				<div class="tit-resumen">RESUMEN DE COMPRA</div>
				<ul class="resumen">
					<li><div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text="0.00"></asp:Label></span></div></li>
						<li><div><asp:panel ID="lblEnviotxt" runat="server">Envio:</asp:panel><span><asp:Label ID="lblEnvio" runat="server" Text=""></asp:Label></span></div></li>
					<li><div><asp:panel ID="lblDesctxt" runat="server" visible="false" >Descuento:</asp:panel><span><asp:Label ID="lblDescuento" runat="server" Text=""></asp:Label></span></div></li>
				</ul>
				<div class="total">Total:<span><asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></span></div>
                  <asp:Panel ID="pnlImpuestos" runat="server" Visible ="false" >
                    <div class="total"><b>Impuestos:</b><span><asp:Label ID="lblImpuestos" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                    <div class="total"><b>Total + Imp:</b><span><asp:Label ID="lbltotalImp" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                </asp:Panel>
				<asp:Button ID="btnContinuar" runat="server" Text="continuar" CssClass ="btn btn-general-3" OnClientClick ="ShowProgress();"/>
				
			</div>
		</div>

 </ContentTemplate>
                      <Triggers>
                       <asp:AsyncPostBackTrigger ControlID="ddlEstados"  EventName ="SelectedIndexChanged" />
                       <asp:AsyncPostBackTrigger ControlID="ddlPais"  EventName ="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="ddlDirecciones"  EventName ="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="ddlTipoMensaje"  EventName ="SelectedIndexChanged" />
                            <%--   <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName ="Click"  />
                           <asp:AsyncPostBackTrigger ControlID="btnContinuar" EventName ="Click"  />--%>
                            <asp:AsyncPostBackTrigger ControlID="chkInvitado" EventName ="CheckedChanged"  />
                            <asp:AsyncPostBackTrigger ControlID="chkRegalo" EventName ="CheckedChanged"  />
                       <%--    <asp:PostBackTrigger ControlID="chkInvitado" />--%>
                          <asp:PostBackTrigger ControlID="btnEntrar" />
                        <%--  <asp:PostBackTrigger ControlID="ddlPais" />
                           <asp:PostBackTrigger ControlID="ddlEstados" />
                          <asp:PostBackTrigger ControlID="ddlDirecciones" />--%>
                          <asp:PostBackTrigger ControlID="btnContinuar" />
                        
                           
                

            </Triggers>
                </asp:UpdatePanel>
	</div>
    <script type="text/javascript">


        function ShowProgress() {
            document.getElementById('<% Response.Write(updateProgress.ClientID) %>').style.display = "inline";
        }


    </script>
    </asp:Content>