<%@ page language="VB" autoeventwireup="false" inherits="pagohnd, App_Web_fy50zy4w" masterpagefile="~/Main.master" enableeventvalidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type ="text/javascript" >
         function justNumbers(e)
        {
        var keynum = window.event ? window.event.keyCode : e.which;
        if ((keynum == 8) || (keynum == 46))
        return true;
         
        return /\d/.test(String.fromCharCode(keynum));
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
            <div class ="row">
			<div class="col-xs-12">
				<ol class="breadcrumb">
				   <li><a href="envio.aspx">ENVÍO</a></li>
				  <li class="active"><a href="pagohnd.aspx">PAGO</a></li>
				  <li><a href="resumen.aspx">RESUMEN</a></li>
				</ol>
			</div>
			<div class="col-xs-12 col-sm-9 stl-1-p contenido" >
				
				
	
				<div class="col-xs-12 col-sm-12">
               
                        <div class="col-xs-6">
                            <div class="checkbox-2">
                               <label>
                                    <asp:CheckBox ID="chkFactura" runat="server" Text="¿Requiere factura?" AutoPostBack="True"  Visible ="false"/>
                               </label>
                                
                               
                            </div>
                         
                        </div>
                    </div>
                    
                    <asp:panel runat="server" ID ="pnlFacturacion" Enabled ="false" >

                   
						<div class="blk-genericos extencion-2">
                        <div id="form" class="form-general">

                      
					
                            <%--<div class="tit-bloque"><h2>Dirección de facturación</h2></div>--%>
							<div class="tit-bloque"><h2>Dirección de facturación</h2></div>
                            <div class="row">
                                <div class="col-xs-12">
                                    <div class="checkbox-2">
                                        <label>
                                            <asp:CheckBox ID="chkMismaDireccion" runat="server" Text="" AutoPostBack="True" Visible ="false"  />
                                           
                                        </label>
                                        
                                    </div>
                                </div>
                                    <div class="col-xs-12 col-sm-12">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">RTN o identidad</label>
                                        <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                              <div class="col-xs-12 col-sm-12">
							    	<div class="form-group">
								        <label for="exampleInputEmail1">Nombre y apellido *</label>
								         <asp:TextBox ID="txtNombreF" runat="server" CssClass="form-control"></asp:TextBox>
							    	</div>
							    </div>
                                 <div class="col-xs-12 col-sm-12">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Nombre de empresa </label>
								       <asp:TextBox ID="txtNombreEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
							
								<div class="col-xs-12 col-sm-12">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Dirección *</label>
								       <asp:TextBox ID="txtCalleF" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Teléfono </label>
								        <asp:TextBox ID="txtTelefonoF" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
                                 	<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Celular </label>
								        <asp:TextBox ID="txtCelularF" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
							
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Ciudad</label>
								         <asp:TextBox ID="txtMunicipioF" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Código Postal</label>
								        <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>

                                 <div class="col-xs-12 col-sm-6">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Estado/Departamento</label>
                                        <asp:DropDownList ID="ddlEstados" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        <asp:TextBox ID="txtEstadoF" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                </div>


                                 <div class="col-xs-12 col-sm-6">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">País</label>
                                        <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        <asp:TextBox ID="txtPaisF" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>
                                </div>
                               
                              
                                 <div class="col-xs-12 col-sm-12">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Comentarios *</label>
								        <asp:TextBox ID="txtComentariosF" runat="server" CssClass="form-control"></asp:TextBox><br />
                                        <asp:Label ID="lblLeyendaComentarios" runat="server" Text="" Visible ="false" ></asp:Label>
								    </div>
								</div>
                           


                            </div>
						
  </div>
                              </div>
					
                           
                    </asp:panel>
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
                <div class="nota">
					<p>Puedes revisar el pedido
antes de finalizarlo.</p>
				</div>
				
			</div>
                	</div>
		</div>
	</div>
   
    </asp:Content>