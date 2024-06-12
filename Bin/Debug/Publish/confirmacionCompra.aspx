<%@ page language="VB" autoeventwireup="false" inherits="confirmacionCompra, App_Web_fy50zy4w" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12 no-padding">

        <div class="main-container">
            <div class="row">

                <div class="tit-bloque">
                    <h5></h5>
                </div>
                <%--<div class="col-xs-12">
				<ol class="breadcrumb">
				  <li><a href="envio.aspx">CONFIRMACIÓN DE COMPRA</a></li>
				  
				</ol>
			</div>--%>
                <div class="col-xs-12 col-sm-9 stl-1-p contenido">
                    <div class="tit-bloque">
                        <h3>
                            <asp:Label ID="lblTitulo" runat="server" Text=""></asp:Label></h3>
                    </div>
                    <div class="tit-bloque">
                        <h4>
                            <asp:Label ID="lblFolio" runat="server" Text="Label"></asp:Label></h4>
                    </div>
                    <br />

                    <div class="tit-bloque">
                        <h5>DETALLES DE LA OPERACIÓN</h5>
                    </div>
                    <br />
                    <br />

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
                                    <div class="blk-action-btn">
                                        <%--<a class="btn-act-blok sep-btn" href ="envio.aspx">Editar</a>--%>
                                    </div>
                                </div>

                                <asp:Panel ID="pnlMetodoPago" runat="server" Visible="false">
                                    <div class="thirt-content col-xs-12 col-sm-6">
                                        <div class="col-xs-12"><strong>Método de pago</strong></div>

                                        <div class="col-xs-2 no-padding">
                                            <img src="img/masterCard.png" class="img-responsive">
                                        </div>
                                        <div class="col-xs-10">
                                            <asp:Label ID="lblTerminacion" runat="server" Text="Terminación:2210"></asp:Label><br>
                                            <asp:Label ID="lblDireccion" runat="server" Text="Calle angosta 212-501"></asp:Label><br>
                                            <br>
                                        </div>
                                        <div class="blk-action-btn">
                                            <%--<a class="btn-act-blok sep-btn" href ="pago.aspx" >Editar</a>--%>
                                        </div>
                                    </div>
                                </asp:Panel>

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
                                <asp:Panel ID="pnlProductos" runat="server"></asp:Panel>

                            </tr>

                        </tbody>
                    </table>
                </div>
                <div class="col-xs-12 col-sm-3">
                    <div class="tit-resumen"></div>
                    <ul class="resumen">
                        <li>
                            <div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text=""></asp:Label></span></div>
                        </li>
                        <li>
                            <div>Envio:<span><asp:Label ID="lblEnvio" runat="server" Text=""></asp:Label></span></div>
                        </li>
                        <li>
                            <div>
                                <asp:Panel ID="lblDesctxt" runat="server" Visible="false">Descuento:</asp:Panel>
                                <span>
                                    <asp:Label ID="lblDescuento" runat="server" Text=""></asp:Label></span></div>
                        </li>
                    </ul>
                    <div class="total">Total:<span><asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></span></div>
                    <asp:Panel ID="pnlImpuestos" runat="server" Visible="false">
                        <div class="total"><b>Impuestos:</b><span><asp:Label ID="lblImpuestos" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>
                        <div class="total"><b>Total + Imp:</b><span><asp:Label ID="lbltotalImp" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>
                    </asp:Panel>
                    <asp:Button ID="btnMuestraPDF" CssClass="btn btn-general-3" runat="server" Text="Ver Factura" Visible="true" />
                </div>

            </div>

        </div>
    </div>
    </asp:Content>