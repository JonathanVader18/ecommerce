<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Cart.aspx.vb" Inherits="Cart" %>

<!DOCTYPE html>
<html>
<head>
    <!-- ==========================
    	Meta Tags 
    =========================== -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    
    <!-- ==========================
    	Title 
    =========================== -->
    <title>uMarket - The easiest way to shop</title>
        
    <!-- ==========================
    	Fonts 
    =========================== -->
    <link href='https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,200,200italic,300,300italic,400italic,600,600italic,700,700italic,900,900italic&amp;subset=latin,latin-ext' rel='stylesheet' type='text/css'>
    <link href='https://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,900,800' rel='stylesheet' type='text/css'>

    <!-- ==========================
    	CSS 
    =========================== -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css">
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css">
    <link href="assets/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="assets/css/dragtable.css" rel="stylesheet" type="text/css">
    <link href="assets/css/owl.carousel.css" rel="stylesheet" type="text/css">
    <link href="assets/css/animate.css" rel="stylesheet" type="text/css">
    <link href="assets/css/color-switcher.css" rel="stylesheet" type="text/css">
    <link href="assets/css/custom.css" rel="stylesheet" type="text/css">
    <link href="assets/css/color/red.css" id="maincolor" rel="stylesheet" type="text/css">
    
    <!-- ==========================
    	JS 
    =========================== -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"></script>
      <script src="https://oss.maxcdn.com/libs/respond.js/1.3.0/respond.min.js"></script>
    <![endif]-->
    
</head>
<body>
      <!-- ==========================
    	SCROLL TOP - START 
    =========================== -->
    <div id="scrolltop" class="hidden-xs"><i class="fa fa-angle-up"></i></div>
    <!-- ==========================
    	SCROLL TOP - END 
    =========================== -->
    
	<!-- ==========================
    	COLOR SWITCHER - START 
    =========================== -->
	<%--<div id="color-switcher">
        <div id="toggle-switcher"><i class="fa fa-gear"></i></div>
        <span>Color Scheme:</span>
        <ul class="list-unstyled list-inline">
            <li id="red" data-toggle="tooltip" data-placement="top" title="" data-original-title="Red"></li>
            <li id="purple" data-toggle="tooltip" data-placement="top" title="" data-original-title="Purple"></li>
            <li id="yellow" data-toggle="tooltip" data-placement="top" title="" data-original-title="Yellow"></li>
            <li id="blue" data-toggle="tooltip" data-placement="top" title="" data-original-title="Blue"></li>
            <li id="dark-blue" data-toggle="tooltip" data-placement="top" title="" data-original-title="Dark Blue"></li>
            <li id="orange" data-toggle="tooltip" data-placement="top" title="" data-original-title="Orange"></li>
            <li id="green" data-toggle="tooltip" data-placement="top" title="" data-original-title="Green"></li>
            <li id="brown" data-toggle="tooltip" data-placement="top" title="" data-original-title="Brown"></li>
            <li id="dark-red" data-toggle="tooltip" data-placement="top" title="" data-original-title="Dark Red"></li>
            <li id="light-green" data-toggle="tooltip" data-placement="top" title="" data-original-title="Light Green"></li>
        </ul>
    </div>--%>
	<!-- ==========================
    	COLOR SWITCHER - END 
    =========================== -->
    
    <div id="page-wrapper"> <!-- PAGE - START -->
    
	<!-- ==========================
    	HEADER - START 
    =========================== -->
	<div class="top-header hidden-xs hidden">
    	<div class="container">
            <div class="row">
                <div class="col-sm-5">
                    <ul class="list-inline contacts">
                        <li><i class="fa fa-envelope"></i> <asp:Label runat="server" ID="lblCorreo"></asp:Label></li>
                        <li><i class="fa fa-phone"></i> <asp:Label runat="server" ID="lblTelefono"></asp:Label></li>
                    </ul>
                </div>
                <div class="col-sm-7 text-right">
                    <ul class="list-inline links">
                        <li><a href="my-account.html">Mi Cuenta</a></li>
                        <li><a href="checkout.aspx">Checkout</a></li>
                        <li><a href="wishlist.html">Lista de deseos</a></li>
                        <li><a href="signin.html">Cerrar Sesión</a></li>
                    </ul>
                  <%--  <ul class="list-inline languages hidden-sm">
                    	<li><a href="#"><img src="assets/images/flags/cz.png" alt="cs_CZ"/></a></li>
                        <li><a href="#"><img src="assets/images/flags/us.png" alt="en_US"/></a></li>
                        <li><a href="#"><img src="assets/images/flags/de.png" alt="de_DE"></a></li>
                    </ul>--%>
                </div>
            </div>
        </div>
    </div>
    <header class="navbar navbar-default navbar-static-top">
    	<div class="container">
            <div class="navbar-header">
                <a href="default.aspx" class="navbar-brand"><span><asp:Label runat="server" ID="lblNombreLargo"></asp:Label></span><asp:Label runat="server" ID="lblNombreCorto"></asp:Label></a>
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse"><i class="fa fa-bars"></i></button>
            </div>
            <div class="navbar-collapse collapse">
            	<p class="navbar-text hidden-xs hidden-sm"><asp:Label runat="server" ID="lblSlogan"></asp:Label></p>
            	<ul class="nav navbar-nav navbar-right">
                    <asp:Panel runat="server" ID="pnlMenu">
                </asp:Panel>
                   <%-- <li class="dropdown megamenu">
                        <a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="300" data-close-others="true">Eshop</a>
                        <ul class="dropdown-menu">
                           
                            <li class="hidden-xs hidden-sm col-md-3">
                                <img src="assets/images/megamenu.png" class="img-responsive center-block" alt=""/>
                            </li>
                        </ul>
                    </li>--%>
                  
                   
                    <li class="dropdown navbar-cart hidden-xs">
                    	<a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="300" data-close-others="true"><i class="fa fa-shopping-cart"></i></a>
                      	<ul class="dropdown-menu">
                        	
                            <!-- CART ITEM - START -->
                            <li>
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                                    </div>
                                    <div class="col-sm-9">
                                    	<h4><a href="single-product.html">Fusce Aliquam</a></h4>
                                        <p>1x - $20.00</p>
                                    	<a href="#" class="remove"><i class="fa fa-times-circle"></i></a>
                                    </div>
                                </div>
                            </li>
                            <!-- CART ITEM - END -->
                            
                            <!-- CART ITEM - START -->
                            <li>
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                                    </div>
                                    <div class="col-sm-9">
                                    	<h4><a href="single-product.html">Fusce Aliquam</a></h4>
                                        <p>1x - $20.00</p>
                                    	<a href="#" class="remove"><i class="fa fa-times-circle"></i></a>
                                    </div>
                                </div>
                            </li>
                            <!-- CART ITEM - END -->
                            
                            <!-- CART ITEM - START -->
                            <li>
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                                    </div>
                                    <div class="col-sm-9">
                                    	<h4><a href="single-product.html">Fusce Aliquam</a></h4>
                                        <p>1x - $20.00</p>
                                    	<a href="#" class="remove"><i class="fa fa-times-circle"></i></a>
                                    </div>
                                </div>
                            </li>
                            <!-- CART ITEM - END -->
                            
                            <!-- CART ITEM - START -->
                            <li>
                            	<div class="row">
                                	<div class="col-sm-6">
                                    	<a href="cart.aspx" class="btn btn-primary btn-block">Ver Carrito</a>
                                    </div>
                                    <div class="col-sm-6">
                                    	<a href="checkout.aspx" class="btn btn-primary btn-block">Confirmar</a>
                                    </div>
                                </div>
                            </li>
                            <!-- CART ITEM - END -->
                            
                      	</ul>
                    </li>
                    <li class="dropdown navbar-search hidden-xs">
                    	<a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-search"></i></a>
                      	<ul class="dropdown-menu">
                        	<li>
                                <form>
                                    <div class="input-group input-group-lg">
                                        <input type="text" class="form-control" placeholder="Buscar ..."/>
                                        <span class="input-group-btn">
                                            <button class="btn btn-primary" type="button">Buscar</button>
                                        </span>
                                    </div>
                                </form>
                            </li>
                      	</ul>
                    </li>
                </ul>
            </div>
        </div>
    </header>
    <!-- ==========================
    	HEADER - END 
    =========================== --> 
    
    <!-- ==========================
    	BREADCRUMB - START 
    =========================== -->
    <section class="breadcrumb-wrapper">
        <div class="container">
            <div class="row">
                <div class="col-xs-6">
                    <h2><asp:Label runat="server" ID="lblGrupo2"></asp:Label></h2>
                    <p><asp:Label runat="server" ID="lblGrupo3"></asp:Label></p>
                </div>
                <div class="col-xs-6">
                    <ol class="breadcrumb">
                        <li><a href="default.aspx">Inicio</a></li>
                        <li><a href="products.aspx">Productos</a></li>
                        <%--<li class="active">Dresses</li>--%>
                    </ol>
                </div>
            </div>
        </div>
    </section>
	<!-- ==========================
    	BREADCRUMB - END 
    =========================== -->
    
    <!-- ==========================
    	MY ACCOUNT - START 
    =========================== -->
    <section class="content account">
        <div class="container">
            <div class="row">
                <div class="col-sm-12">
                    <article class="account-content">
                        
                        <form>
                            <div class="products-order shopping-cart">
                            	<div class="table-responsive">
                                    <table class="table table-products">
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>Producto</th>
                                                <th>Precio Unitario</th>
                                                <th>Cantidad</th>
                                                <th>Subtotal</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="col-xs-1"><img src="assets/images/products/product-1.jpg" alt="" class="img-responsive"></td>
                                                <td class="col-xs-4 col-md-5"><h4><a href="single-product.html">Fusce Aliquam</a><small>M, Black, Esprit</small></h4></td>
                                                <td class="col-xs-2 text-center"><span>$30</span></td>
                                                <td class="col-xs-2 col-md-1"><div class="form-group"><input type="text" class="form-control" value="2"></div></td>
                                                <td class="col-xs-2 text-center"><span><b>$60</b></span></td>
                                                <td class="col-xs-1 text-center"><a href="" class="btn btn-primary"><i class="fa fa-times"></i></a></td>
                                            </tr>
                                            <tr>
                                                <td class="col-xs-1"><img src="assets/images/products/product-2.jpg" alt="" class="img-responsive"></td>
                                                <td class="col-xs-4 col-md-5"><h4><a href="single-product.html">Fusce Aliquam</a><small>M, Black, Esprit</small></h4></td>
                                                <td class="col-xs-2 text-center"><span>$80</span></td>
                                                <td class="col-xs-2 col-md-1"><div class="form-group"><input type="text" class="form-control" value="2"></div></td>
                                                <td class="col-xs-2 text-center"><span><b>$160</b></span></td>
                                                <td class="col-xs-1 text-center"><a href="" class="btn btn-primary"><i class="fa fa-times"></i></a></td>
                                            </tr>
                                            <tr>
                                                <td class="col-xs-1"><img src="assets/images/products/product-3.jpg" alt="" class="img-responsive"></td>
                                                <td class="col-xs-4 col-md-5"><h4><a href="single-product.html">Fusce Aliquam</a><small>M, Black, Esprit</small></h4></td>
                                                <td class="col-xs-2 text-center"><span>$95</span></td>
                                                <td class="col-xs-2 col-md-1"><div class="form-group"><input type="text" class="form-control" value="1"></div></td>
                                                <td class="col-xs-2 text-center"><span><b>$95</b></span></td>
                                                <td class="col-xs-1 text-center"><a href="" class="btn btn-primary"><i class="fa fa-times"></i></a></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            	<a href="products.html" class="btn btn-inverse">ContinUAR COMPRANDO</a>
                                <a href="" class="btn btn-inverse update-cart">ACTUALIZAR CARRITO</a>
                            </div>
                        
                            <div class="box">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <h5>Si tiene algún cupón de descuento, ingréselo abajo</h5>
                                        <div class="input-group">
                                            <input type="email" class="form-control" placeholder="Discount code">
                                            <span class="input-group-btn">
                                                <button class="btn btn-primary" type="button">Aplicar cupon</button>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-sm-offset-2">
                                        <ul class="list-unstyled order-total">
                                            <li>Subtotal<span>$315.00</span></li>
                                            <li>Descuento<span>- $25.00</span></li>
                                            <li>Total<span class="total">$290.00</span></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix">
                                <a href="checkout.aspx" class="btn btn-primary btn-lg pull-right ">Checkout</a>
                            </div>
                        </form>

                    </article>
                </div>
            </div> 
        </div>
    </section>
    <!-- ==========================
    	MY ACCOUNT - END 
    =========================== -->
        
    <!-- ==========================
    	NEWSLETTER - START 
    =========================== -->
    <%--<section class="separator separator-newsletter">
    	<div class="container">
            <div class="newsletter-left">
                <div class="newsletter-badge">
                    <span>Subsribe & Get </span>
                    <span class="price">$15</span>
                    <span>discount</span>
                </div>
            </div>
            <div class="newsletter-right">
            	<div class="row">
                    <div class="col-sm-6">
                        <div class="newsletter-body">
                            <h3>Newsletter</h3>
                            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.</p>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <form>
                            <div class="input-group input-group-lg">
                                <input type="email" class="form-control" placeholder="Enter email address">
                                <span class="input-group-btn">
                                    <button class="btn btn-primary" type="button">Sign Up</button>
                                </span>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </section>--%>
    <!-- ==========================
    	NEWSLETTER - END 
    =========================== -->
    
    <!-- ==========================
    	FOOTER - START 
    =========================== -->
  <footer class="navbar navbar-default">
    	<div class="container">
        	<div class="row">
                <div class="col-sm-3 col-xs-6">
                	<div class="footer-widget footer-widget-contacts">
                    	<h4>Contacto</h4>
                        <ul class="list-unstyled">
                            <li><i class="fa fa-envelope"></i> <asp:Label runat="server" ID="lblCorreoTienda"></asp:Label></li>
                            <li><i class="fa fa-phone"></i>  <asp:Label runat="server" ID="lblTelefono2"></asp:Label></li>
                           
                            <li class="social">
                            	<a href="#"><i class="fa fa-facebook"></i></a>
                                <a href="#"><i class="fa fa-twitter"></i></a>
                                <a href="#"><i class="fa fa-instagram"></i></a>
                                <a href="#"><i class="fa fa-linkedin"></i></a>
                                <a href="#"><i class="fa fa-tumblr"></i></a>
                            </li>
                        </ul>
                	</div>
                </div>
                <div class="col-sm-3 col-xs-6">
                	<div class="footer-widget footer-widget-links">
                    	<h4>Información</h4>
                        <ul class="list-unstyled">
                        	<li><a href="about-shop.html">Acerca de nosotros</a></li>
                            <li><a href="stores.html">Sucursales</a></li>
                            <li><a href="terms-conditions.html">Términos y condiciones</a></li>
                            <li><a href="privacy-policy.html">Políticias de privacidad</a></li>
                            <li><a href="faq.html">FAQ</a></li>
                            <li><a href="my-account.html">Mi cuenta</a></li>
                        </ul>
                	</div>
                </div>
                <div class="col-sm-3 col-xs-6">
                   
                </div>
                <div class="col-sm-3 col-xs-6">
                	
                </div>
            </div>
            <div class="footer-bottom">
            	<div class="row">
                    <div class="col-sm-6">
                        <p class="copyright">© Tecnologías de información al extremo.</p>
                      
                    </div>
                    <div class="col-sm-6">
                        <ul class="list-inline payment-methods">
                        
                            <li><i class="fa fa-cc-mastercard"></i></li>
                            <li><i class="fa fa-cc-paypal"></i></li>
                           
                            <li><i class="fa fa-cc-visa"></i></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </footer>
    <!-- ==========================
    	FOOTER - END 
    =========================== -->
    
    <!-- ==========================
    	MODAL ADVERTISING  - START 
    =========================== -->
  <%--  <div class="modal fade" tabindex="-1" role="dialog" id="modalAdvertising">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><i class="fa fa-times"></i></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                    	<div class="col-sm-8">
                        	<h3>Dicount 10%</h3>
                        	<p>Enter your email address and get 10% discount for your first purchase.</p>
                            <form>
                                <div class="input-group">
                                    <input type="email" class="form-control" placeholder="Email Address">
                                    <span class="input-group-btn">
                                        <button class="btn btn-primary" type="button">Submit</button>
                                    </span>
                                </div>
                            </form>
                            <div class="checkbox">  
                                <input id="modal-hide" type="checkbox" value="hidden">  
                                <label for="modal-hide">Don't show this popup again</label>
                            </div>
                        </div>
                        <div class="col-sm-4 hidden-xs">
                        	<img src="assets/images/lookbook-1.png" class="img-responsive" alt="">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>
	<!-- ==========================
    	MODAL ADVERTISING - END 
    =========================== -->
    
    </div> <!-- PAGE - END -->
    
   	<!-- ==========================
    	JS 
    =========================== -->        
	<script src="http://code.jquery.com/jquery-latest.min.js"></script>
    <script src="http://code.jquery.com/ui/1.11.1/jquery-ui.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?v=3.exp&amp;sensor=true"></script>
	<script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/bootstrap-hover-dropdown.min.js"></script>
    <script src="assets/js/SmoothScroll.js"></script>
    <script src="assets/js/jquery.dragtable.js"></script>
    <script src="assets/js/jquery.card.js"></script>
    <script src="assets/js/owl.carousel.min.js"></script>
    <script src="assets/js/twitterFetcher_min.js"></script>
    <script src="assets/js/jquery.mb.YTPlayer.min.js"></script>
    <script src="assets/js/color-switcher.js"></script>
    <script src="assets/js/custom.js"></script>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
