<%@ Page Language="VB" AutoEventWireup="false" CodeFile="resumen.aspx.vb" Inherits="pago_resumen" MasterPageFile ="~/Main.master" %>


<%@ Import Namespace="System.Collections" %>

   
    

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%-- <% 
     Dim mp As New MP("CLIENT_ID", "CLIENT_SECRET")


     Dim preference As Hashtable = mp.createPreference("{\"items\":[{\"title\":\"sdk-dotnet\",\"quantity\":1,\"currency_id\":\"ARS\",\"unit_price\":10.5}]}")
    %>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   

    <div class="col-xs-12">
		<div class="main-container">
            <div class="row">
			<div class="col-xs-12">
				<ol class="breadcrumb">
				  <li><a href="envio.aspx">ENVÍO</a></li>
				  <li><a href="pagoindex.aspx">PAGO</a></li>
				  <li class="active"><a href="resumen.aspx">RESUMEN</a></li>
				</ol>
			</div>
			<div class="col-xs-12 col-sm-9 stl-1-p contenido">
				<div class="tit-bloque"><h5>OPCIONES DE ENVÍO</h5></div><br />
                <div class="blk-genericos extencion back-gray">
                    <div class="content-blok">
                        <div class="grup-content">
                            <div class="thirt-content col-xs-12 col-sm-6">
                                <div>
                                    <strong>
                                        <asp:Label ID="lblNombreEnvio" runat="server" Text="Enviado a:Juanito Qwert Gonzales"></asp:Label></strong>
                                    <p>
                                        <asp:Label ID="lblNombreDireccion" runat="server" Text="Juanito Qwert Gonzales"></asp:Label><br>
                                        <asp:Label ID="lblCalleyNum" runat="server" Text="Calle angosta 212-501"></asp:Label><br>
                                        <asp:Label ID="lblColoniaMunic" runat="server" Text="Del carmen,Coyoacán"></asp:Label><br>
                                        <asp:Label ID="lblEstadoCP" runat="server" Text="México, 06100"></asp:Label>
                                    </p>
                                </div>
                                <div class="blk-action-btn">
                                    <a class="btn-act-blok sep-btn" href="pagoindex.aspx">Editar</a>
                                </div>
                            </div>
                            <div class="thirt-content col-xs-12 col-sm-6">
                                <%--<div class="col-xs-12"><strong>Método de pago</strong></div>
                                <asp:Panel ID="pnlMetodoPago" runat ="server" ></asp:Panel>
                                <div class="col-xs-2 no-padding">
											<img src="img/masterCard.png" class="img-responsive">
										</div>
                                <div class="col-xs-10">
											<asp:Label ID="lblTerminacion" runat="server" Text="Terminación:2210"></asp:Label><br>
											<asp:Label ID="lblDireccion" runat="server" Text="Calle angosta 212-501"></asp:Label><br>
                                    <br>
										</div>
                                <div class="blk-action-btn">
											<a class="btn-act-blok sep-btn" href ="pago.aspx" >Editar</a>
								</div>--%>
                            </div>
                        </div>
                    </div>
                </div><br /><br />
				<div class="tit-bloque"><h5>MÉTODO DE ENVÍO</h5></div>
				<div class="col-xs-12 col-sm-12">
                    <div class="blk-genericos">
                        <div class="col-xs-12">
                            <div class="radio">
                                <label>
                                    <input type="radio" name="optionsRadios" id="optionsRadios1" value="option1" checked>
                                    <asp:Label ID="lblMetodoEnvio" runat="server" Text=" 4-7 días (4 a 7 días hábiles) - $200.00"></asp:Label>
                                </label>
							</div>
						</div>
					</div>
				</div>
				<table class="table" id="no-more-tables">
						<thead>
							<tr>
								<th></th>
								<th class="text-center">artículo</th>
								<th class="text-center">precio</th>
								<th class="text-center">cantidad</th>
								<th class="text-center">total</th>
							</tr>	
						</thead>
						<tbody>	
							
							<tr>
                                <asp:Panel ID="pnlProductos" runat ="server" ></asp:Panel>
								
							</tr>
							
						</tbody>
					</table>
			</div>
                <asp:Label ID="lblEnviotxt" runat="server" Text="Envio:" Visible ="false" ></asp:Label>
                <div class="col-xs-12 col-sm-3">
                    <div class="tit-resumen">RESUMEN DE COMPRA</div>
                    <ul class="resumen">
                      	<li><div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text=""></asp:Label></span></div></li>
					<li><div>Envio:<span><asp:Label ID="lblEnvio" runat="server" Text=""></asp:Label></span></div></li>
					<li><div><asp:panel ID="lblDesctxt" runat="server" visible="false" >Descuento:</asp:panel><span><asp:Label ID="lblDescuento" runat="server" Text=""></asp:Label></span></div></li>
                    </ul>
                    <div class="total">Total:<span><asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></span></div>
                      <asp:Panel ID="pnlImpuestos" runat="server" Visible ="false" >
                    <div class="total"><b>Impuestos:</b><span><asp:Label ID="lblImpuestos" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                    <div class="total"><b>Total + Imp:</b><span><asp:Label ID="lbltotalImp" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                </asp:Panel>
                    <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar pedido" CssClass="btn btn-general-3" />
                    <%--PostBackUrl ="https://sandbox.checkout.payulatam.com/ppp-web-gateway-payu/"--%>


                    <%--   <a href="<% Response.Write(preference("response")("init_point")) %>" name="MP-Checkout" class="orange-ar-m-sq-arall">Pay</a>
        <script type="text/javascript" src="//resources.mlstatic.com/mptools/render.js"></script>--%>

            <div align="center">
           
              <input name="merchantId"    type="hidden"  value="<%= Session("MerchantId")%>"   >
              <input name="accountId"     type="hidden"  value="<%= Session("AccountId")%>"  >
              <input name="description"   type="hidden"  value="<%= Session("Description")%>"  >
              <input name="referenceCode" type="hidden"  value="<%= Session("ReferenceCode")%>" >
              <input name="amount"        type="hidden"  value="<%=Me.lblTotal.Text.Replace("$", "").Replace(" ", "").Replace("MXP", "").Replace(",", "")%>"  >
              <input name="tax"           type="hidden"  value="0"  >
              <input name="taxReturnBase" type="hidden"  value="0"  >
              <input name="currency"      type="hidden"  value="MXN" >
              <input name="signature"     type="hidden"  value="<%= Session("Firma")%>"  >
              <input name="test"          type="hidden"  value="1" >
              <input name="buyerEmail"    type="hidden"  value="ing.prosas@gmail.com" > <%-- value="<%= Session("UserB2C")%>"  --%>
              <input name="responseUrl"    type="hidden"  value="<%= Session("ResponseURL")%>" >
              <input name="confirmationUrl"    type="hidden"  value="<%= Session("ConfirmationURL")%>" >
             
            </div>
               

              <div id="PixelPayPost" > <%--method="post" action="https://pay.pixel.hn/hosted/payment/other?"--%>
               <%-- <input type="submit" value="Pagar"/>
                <br />
                <br />--%>
                <input type="hidden" name="_key" value="<%=Session("KEY") %>" /> <%--LLAVE DE PRUEBAS: 217141192 LLAVE DE PRODUCTIVO: 3128820048 https://pay.pixel.hn/hosted/payment/other?_key=217141192&_cancel=https://www.google.com/&_complete=https://www.google.com/&_amount=10.00&_order_id=Pago 001&_email=vitayazmeya@gmail.com&_first_name=Gerardo Manuel&_last_name=Garcia Hernandez&_address=Av Santa Rosalia 202&_country=Mexico&_state=Jalisco&_city=Guadalajara--%>
                <br />
                <br />
                <input type="hidden" name="_cancel" value="<%= Session("ErrorURL")%>" />
                <br />
                <br />
                <input type="hidden" name="_complete" value="<%= Session("ResponseURL")%>" />
                <br />
                <br />
                <input type="hidden" name="_order_id" value="<%= Session("NoPedidoPago")%>" />
                <br />
                <br />
                <input type="hidden" name="_callback" value="<%= Session("ResponseURL")%>" />
                <br />
                <br />
                <input type="hidden" name="_amount" value="<%=Me.lblTotal.Text.Replace("$", "").Replace(" ", "").Replace("Lps", "").Replace(",", "") %>" />
            
                <input type="hidden" name="_first_name" value="<%= Session("NombreuserTienda")%>" />
               
                <input type="hidden" name="_email" value="<%= Session("CorreoInvitado")%>" />
                <br />
                <br />
                <input type="hidden" name="_address" value="<%= Session("CalleEnvio")%>" />
                <br />
                <br />
                <input type="hidden" name="_city" value="<%= Session("CiudadEnvio") %>" />
                <br />
                <br />
                <input type="hidden" name="_state" value="<%= Session("EstadoEnvio")%>" />
                <br />
                <br />
                <input type="hidden" name="_country" value="<%= Session("PaisEnvio")%>" />
                <br />
                <br />
                <%--<input type="hidden" name="json" value="false" />--%>
            </div>

            <%-- <div align="center">
           
              <input name="merchantId"    type="hidden"  value="508029"   >
              <input name="accountId"     type="hidden"  value="512324"  >
              <input name="description"   type="hidden"  value="Compra en línea TAQ"  >
              <input name="referenceCode" type="hidden"  value="20180605" >
              <input name="amount"        type="hidden"  value="<%=Me.lblTotal.Text.Replace("$", "").Replace(" ", "")%>"  >
              <input name="tax"           type="hidden"  value="0"  >
              <input name="taxReturnBase" type="hidden"  value="0"  >
              <input name="currency"      type="hidden"  value="MXN" >
              <input name="signature"     type="hidden"  value="<%= Session("Firma")%>"  >
              <input name="test"          type="hidden"  value="1" >
              <input name="buyerEmail"    type="hidden"  value="jorge.arrubarrena@taq.com.mx" >
              <input name="responseUrl"    type="hidden"  value="http://localhost:8080/Tienda/confirmacionCompra.aspx" >
              <input name="confirmationUrl"    type="hidden"  value="http://localhost:8080/Tienda/confirmacionCompra.aspx" >
             
            </div>--%>

        </div>

		</div>
		</div>
	</div>
     
    </asp:Content>