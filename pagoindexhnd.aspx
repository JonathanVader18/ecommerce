<%@ Page Language="VB" AutoEventWireup="false" CodeFile="pagoindexhnd.aspx.vb" Inherits="pagoindexhnd"  MasterPageFile ="~/Main.master"  EnableEventValidation ="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type ="text/javascript">
  function pulsar(e) { 
 var tecla = (document.all) ? e.keyCode :e.which; 
  return (tecla!=13); 
        } 
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
			<div class="col-xs-12 no-padding">
				<ol class="breadcrumb">
				  <li><a href="active">INICIO</a></li>
				  <li><a href="#">ENVÍO</a></li>
				  <li class="#">PAGO</li>
				</ol>
			</div>
			<div class="col-xs-12 col-sm-9 stl-1-p contenido">
                <asp:placeholder runat="server" ID="pnlIngresar" Visible="false">
                    <div class="col-xs-12 col-sm-6">
                        <div class="blk-genericos">
                            <div class="tit-bloque">
                                <h2>
                                    <asp:CheckBox ID="chkInvitado" runat="server" />&nbsp;CONTINUAR COMO INVITADO</h2>
                            </div>
                            <div class="form-general">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">correo</label>
                                    <asp:TextBox ID="txtCorreoInvitado" runat="server" CssClass="form-control"  onkeypress="return pulsar(event);"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <div class="checkbox-2">
                                        <label>
                                            <asp:CheckBox ID="chkOfertas" runat="server" Text=" " />
                                            Quiero recibir ofertas y promociones
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </asp:placeholder>
                <asp:PlaceHolder ID="pnlLogin" runat ="server" Visible ="true" >
                    <div class="col-xs-12 col-sm-6">
                        <div class="blk-genericos">
                            <div class="tit-bloque">
                                <h2>INGRESAR O CREAR CUENTA</h2>
                            </div>
                            <div class="form-general">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">usuario</label>
                                    <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"  onkeypress="return pulsar(event);"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="exampleInputPassword1">contraseña</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"  onkeypress="return pulsar(event);"></asp:TextBox>
                                </div>
                                <div class="post-auto">
                                    <a class="link-lp" href="mi-cuenta.aspx">crear cuenta</a><br>
                                    <a class="link-lp" href="recuperar.aspx">¿olvidaste tu contraseña?</a>
                                </div>
                                <asp:Button ID="btnEntrar" runat="server" Text="Entrar" CssClass="btn btn-general-6" />
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
				<div class="blk-genericos extencion-2">
					<div class="form-general">
					 	<fieldset>
					 		<div class="tit-bloque"><h2>DIRECCIÓN DE ENVÍO </h2> </div>
                             <div class="row">
                                   <div class="col-xs-12 col-sm-6">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">
                                            <asp:Label ID="lblTituloDir" runat="server" Text="Elija una dirección"></asp:Label></label>
								         <asp:DropDownList ID="ddlDirecciones" runat="server"  Width ="100%" AutoPostBack="True"  CssClass="form-control"></asp:DropDownList>
							    	</div>
							    </div>
                             </div>
					 		<div class="row">
                                  <div class="col-xs-12 col-sm-12">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">RTN o identidad *</label>
								         <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
							    	</div>
							    </div>
							    <div class="col-xs-12 col-sm-12">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">Nombre y apellido *</label>
								         <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
							    	</div>
							    </div>
                                 <div class="col-xs-12 col-sm-12">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Nombre de empresa </label>
								       <asp:TextBox ID="txtNombreEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
							  <%--  <div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Apellidos</label>
								        <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>--%>

								<div class="col-xs-12 col-sm-12">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Dirección *</label>
								       <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Teléfono </label>
								        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
                                 	<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Celular *</label>
								        <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
							
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Ciudad</label>
								         <asp:TextBox ID="txtMunicipio" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Código Postal</label>
								        <asp:TextBox ID="txtCP" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								      <div class="col-xs-12 col-sm-6">
                                     <div class="form-group">
                                         <label for="exampleInputEmail1">Departamento *</label>
                                         <asp:DropDownList ID="ddlEstados" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                         <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                     </div>
                                 </div>

                                 <div class="col-xs-12 col-sm-6">
                                     <div class="form-group">
                                         <label for="exampleInputEmail1">País</label>
                                         <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                     </div>
								</div>
                           

								
							</div>
						</fieldset>
					</div>
				</div>
			</div>
			<div class="col-xs-12 col-sm-3">
				<div class="tit-resumen">RESUMEN DE COMPRA</div>
				<ul class="resumen">
					<li><div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text="0.00"></asp:Label></span></div></li>
					<li><div>Envío:<span><asp:Label ID="lblEnvio" runat="server" Text="0.00"></asp:Label></span></div></li>
					<li><div>Descuento:<span><asp:Label ID="lblDescuento" runat="server" Text="0.00"></asp:Label></span></div></li>
				</ul>
				<div class="total">Total:<span><asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></span></div>
				<asp:Button ID="btnContinuar" runat="server" Text="continuar" CssClass ="btn btn-general-3" />
				
			</div>
		</div>
	</div>
    </asp:Content>