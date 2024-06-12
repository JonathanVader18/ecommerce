<%@ Page Language="VB" AutoEventWireup="false" CodeFile="envio.aspx.vb" Inherits="pago_envio" MasterPageFile ="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">

            <div class="row">
    
            
			<div class="col-xs-12">
				<ol class="breadcrumb">
				  <li class="active"><a href="#">ENVÍO</a></li>
				  <li><a href="#">PAGO</a></li>
				  <li>RESUMEN</li>
				</ol>
			</div>
                <div class="col-xs-12 col-sm-9 stl-1-p contenido">

                    <div class="tit-bloque">
                        <h5>OPCIONES DE ENVÍO</h5>
                    </div>
                    <br />
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
                                </div>

                            </div>
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="tit-bloque">
                        <h5>MÉTODO DE ENVÍO</h5>
                    </div>

                    <div class="col-xs-12 col-sm-12">
                        <div class="blk-genericos">
                            <div class='col-xs-12'>
                                <div class="radio">
                                    <asp:Panel ID="pnlMetodosEnvio" runat="server">
                                    </asp:Panel>
                                </div>
                                <asp:Panel ID="pnlComentarios" runat="server" Visible="false">
                                    <div class="col-xs-12 col-sm-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Comentarios </label>
                                            <asp:TextBox ID="txtComentarios" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox><br />
                                            <asp:Label ID="lblLeyendaComentarios" runat="server" Text="" Visible="false"></asp:Label>
                                        </div>
                                    </div>
                                </asp:Panel>
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
                                    <asp:Panel ID="pnlProductos" runat="server"></asp:Panel>

                                </tr>

                            </tbody>
                        </table>
                    </div>

                </div>
                <asp:Label ID="lblEnviotxt" runat="server" Text="Envio:" CssClass ="clearfix" Visible ="false" ></asp:Label>
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
                <asp:Button ID="btnContinuar" runat="server" Text="proceder al pago" CssClass ="btn btn-general-3" />
				
			</div>


		</div>
	</div>
    </div>
    </asp:Content>