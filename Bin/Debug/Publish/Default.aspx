﻿<%@ page language="VB" autoeventwireup="false" inherits="_Default, App_Web_3bsbwt4p" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
  
     <!-- ==========================
    	Meta Tags 
    =========================== -->
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    
    <!-- ==========================
    	Title 
    =========================== -->
    <title>Tienda en línea</title>
        
    <!-- ==========================
    	Fonts 
    =========================== -->
    <link href='https://fonts.googleapis.com/css?family=Source+Sans+Pro:400,200,200italic,300,300italic,400italic,600,600italic,700,700italic,900,900italic&amp;subset=latin,latin-ext' rel='stylesheet' type='text/css'/>
    <link href='https://fonts.googleapis.com/css?family=Raleway:400,100,200,300,500,600,700,900,800' rel='stylesheet' type='text/css'/>

    <!-- ==========================
    	CSS 
    =========================== -->
    <link href="assets/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
    <link href="http://code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/dragtable.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/owl.carousel.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/animate.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/color-switcher.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/custom.css" rel="stylesheet" type="text/css"/>
    <link href="assets/css/color/red.css" id="maincolor" rel="stylesheet" type="text/css"/>
    
    <!-- ==========================
    	JS 
    =========================== -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
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
                        <li><a href="checkout.aspx">Confirmar</a></li>
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
    <header class="navbar navbar-transparent navbar-fixed-top">
    	<div class="container">
            <div class="navbar-header">
                <a href="default.aspx" class="navbar-brand"><span><asp:Label runat="server" ID="lblNombreLargo"></asp:Label></span><asp:Label runat="server" ID="lblNombreCorto"></asp:Label></a>
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse"><i class="fa fa-bars"></i></button>
            </div>
            <div class="navbar-collapse collapse">
            	<p class="navbar-text hidden-xs hidden-sm"><asp:Label runat="server" ID="lblSlogan"></asp:Label></p>
                <asp:Panel runat="server" ID="pnlMenu">
                </asp:Panel>
            	<ul class="nav navbar-nav navbar-right">
                    <li class="dropdown">
                        <a href="default.aspx" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="300" data-close-others="true">Inicio</a>
                      	<ul class="dropdown-menu">
                        <%--	<li><a href="index.html">Homepage 1</a></li>
                            <li><a href="homepage-2.html">Homepage 2</a></li>
                            <li><a href="homepage-3.html">Homepage 3<span class="label label-warning pull-right">Updated</span></a></li>
                            <li><a href="homepage-4.html">Homepage 4</a></li>
                            <li><a href="homepage-5.html">Homepage 5<span class="label label-danger pull-right">New</span></a></li>
                            <li><a href="homepage-6.html">Homepage 6<span class="label label-danger pull-right">New</span></a></li>--%>
                      	</ul>
                    </li>
                  
                  <%--  <li class="dropdown megamenu">
                    	<a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="300" data-close-others="true">Pages</a>
                      	<ul class="dropdown-menu">
                            <li class="col-sm-4">
                            	<ul class="list-unstyled">
                                	<li class="title">Eshop</li>
                                    <li><a href="products.html">Products</a></li>
                                    <li><a href="cart.html">Cart</a></li>
                                    <li><a href="checkout.html">Checkout</a></li>
                                    <li><a href="compare.html">Compare</a></li>
                                    <li><a href="single-product.html">One Product</a></li>
                                    <li><a href="stores.html">Stores</a></li>
                                    <li><a href="about-shop.html">About Shop</a></li>
                                    <li><a href="lookbook.html">Lookbook</a></li>
                                </ul>
                            </li>
                            <li class="col-sm-4">
                            	<ul class="list-unstyled">
                                	<li class="title">Account</li>
                                    <li><a href="my-account.html">My Account<span class="label label-warning pull-right">Updated</span></a></li>
                                    <li><a href="profile.html">Profile</a></li>
                                    <li><a href="orders.html">Ordres</a></li>
                                    <li><a href="wishlist.html">Wishlist</a></li>
                                    <li><a href="address.html">Address</a></li>
                                    <li><a href="warranty-claims.html">Warranty Claims<span class="label label-danger pull-right">New</span></a></li>
                                    <li><a href="signup.html">Sign Up</a></li>
                                    <li><a href="signin.html">Sign In</a></li>
                                    <li><a href="lost-password.html">Lost Password</a></li>
                                </ul>
                            </li>
                            <li class="col-sm-4">
                            	<ul class="list-unstyled">
                                	<li class="title">Other Pages</li>
                                    <li><a href="blog.html">Blog</a></li>
                                    <li><a href="single-post.html">One Blog Post</a></li>
                                    <li><a href="single-order.html">Order Detail</a></li>
                                    <li><a href="downloads.html">Downloads<span class="label label-danger pull-right">New</span></a></li>
                                    <li><a href="faq.html">FAQ</a></li>
                                    <li><a href="privacy-policy.html">Privacy Policy</a></li>
                                    <li><a href="terms-conditions.html">Terms & Conditions</a></li>
                                    <li><a href="404.html">404 Error</a></li>
                                    <li><a href="email-template.html" target="_blank">Email Template</a></li>
                                </ul>
                            </li>
                      	</ul>
                    </li>--%>
                   <%-- <li class="dropdown megamenu">
                    	<a href="#" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="300" data-close-others="true">Components<span class="label label-danger pull-right">New</span></a>
                      	<ul class="dropdown-menu">
                            <li class="col-sm-4">
                            	<ul class="list-unstyled">
                                    <li><a href="components.html#headings">Headings</a></li>
                                    <li><a href="components.html#paragraphs">Paragraphs</a></li>
                                    <li><a href="components.html#lists">Lists</a></li>
                                    <li><a href="components.html#tabs">Tabs</a></li>
                                    <li><a href="components.html#accordition">Accordition</a></li>
                                </ul>
                            </li>
                            <li class="col-sm-4">
                            	<ul class="list-unstyled">
                                    <li><a href="components.html#collapse">Collapse</a></li>
                                    <li><a href="components.html#buttons">Buttons</a></li>
                                    <li><a href="components.html#tables">Tables</a></li>
                                    <li><a href="components.html#grids">Grids</a></li>
                                    <li><a href="components.html#responsive-video-audio">Responsive Video &amp; Audio</a></li>
                                </ul>
                            </li>
                            <li class="col-sm-4">
                            	<ul class="list-unstyled">
                                    <li><a href="components.html#alerts">Alerts</a></li>
                                    <li><a href="components.html#forms">Forms</a></li>
                                    <li><a href="components.html#labels">Labels</a></li>
                                    <li><a href="components.html#paginations">Paginations</a></li>
                                    <li><a href="components.html#carousels">Carousels</a></li>
                                </ul>
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
                                    	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt=""/>
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
                                        <input type="text" class="form-control" placeholder="Search ...">
                                        <span class="input-group-btn">
                                            <button class="btn btn-primary" type="button">Search</button>
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
    	JUMBOTRON - START 
    =========================== -->
    <section class="content jumbotron jumbotron-full-height">
        <div id="homepage-2-carousel" class="nav-inside">
            
            <div class="item slide-7">
                <div class="slide-mask"></div>
                <div class="slide-body">
                    <div class="container">
                        <h1>Bienvenidos a <span class="color"><asp:Label runat="server" ID="lblNombreEmpresa"></asp:Label></span></h1>
                        <h2><asp:Label runat="server" ID="lblSlogan2"></asp:Label></h2>
                     <%--   <a href="https://wrapbootstrap.com/theme/umarket-modern-responsive-ecommerce-WB054TF88?ref=themejumbo" class="btn btn-default btn-lg">Show More</a>--%>
                        <a href="https://wrapbootstrap.com/theme/umarket-modern-responsive-ecommerce-WB054TF88?ref=themejumbo" class="btn btn-inverse btn-lg">Comprar Ahora</a>
                    </div>
                </div>
            </div>
            <div class="item slide-2">
                <div class="slide-mask"></div>
                <div class="slide-body">
                    <div class="container">
                    	<h1 class="grey-background">Novedades</h1>
                        <div><h2 class="color-background">Tendencias</h2></div>
                        <ul class="list-unstyled">
                        	<li><i class="fa fa-check"></i>Increíbles promociones</li>
                            <li><i class="fa fa-check"></i>Servicio al cliente</li>
                             <li><i class="fa fa-check"></i>Tu aliado ideal</li>
                           
                        </ul>
                    </div>
                </div>
            </div>
                        
        </div>
    </section>
    <!-- ==========================
    	JUMBOTRON - END 
    =========================== -->
    
    <!-- ==========================
    	SERVICES - START 
    =========================== -->
    <section class="content services services-3x border-top border-bottom">
        <div class="container">
        	<div class="row row-no-padding">
            
            	<!-- SERVICE - START -->
                <div class="col-xs-12 col-sm-4">
                    <div class="service">
                        <i class="fa fa-star"></i>
                        <h3>TENDENCIA DENIM</h3>
                        <p>Va con TODO!</p>
                    </div>
                </div>
                <!-- SERVICE - END -->
                
                <!-- SERVICE - START -->
                <div class="col-xs-6 col-sm-4">
                    <div class="service">
                        <i class="fa fa-heart"></i>
                        <h3>LINEA CURVY</h3>
                        <p>La belleza viene en todos los tamaños.</p>
                    </div>
                </div>
                <!-- SERVICE - END -->
                
                <!-- SERVICE - START -->
                <div class="col-xs-6 col-sm-4">
                    <div class="service">
                        <i class="fa fa-rocket"></i>
                        <h3>ACCESORIOS</h3>
                        <p>Los complementos perfectos para tus outfit</p>
                    </div>
                </div>
                <!-- SERVICE - END -->
                
            </div>
            
        </div>
    </section>
    <!-- ==========================
    	SERVICES - END 
    =========================== -->
    
    <!-- ==========================
    	CATEGORIES - START 
    =========================== -->
    <section class="content categories">
        <div class="row row-no-padding">
        
            <!-- CATEGORY - START -->
            <div class="col-xs-4">
                <div class="category">
                    <a href="products.html">
                        <img src="assets/images/categories/category-2.jpg" class="img-responsive" alt="">
                        <div class="category-mask"></div>
                        <h3 class="category-title">Caballeros <span>Collection</span></h3>
                    </a>
                </div>
            </div>
            <!-- CATEGORY - END -->
            
            <!-- CATEGORY - START -->
            <div class="col-xs-4">
                <div class="category">
                    <a href="products.html">
                        <img src="assets/images/categories/category-3.jpg" class="img-responsive" alt="">
                        <div class="category-mask"></div>
                        <h3 class="category-title">Mujer <span>Collection</span></h3>
                    </a>
                </div>
            </div>
            <!-- CATEGORY - END -->
            
            <!-- CATEGORY - START -->
            <div class="col-xs-4">
                <div class="category">
                    <a href="products.html">
                        <img src="assets/images/categories/category-4.jpg" class="img-responsive" alt="">
                        <div class="category-mask"></div>
                        <h3 class="category-title">Accesorios <span>Collection</span></h3>
                    </a>
                </div>
            </div>
            <!-- CATEGORY - END -->
                            
        </div>
        
    </section>
    <!-- ==========================
    	CATEGORIES - END 
    =========================== -->
    
    <!-- ==========================
    	GRID PRODUCTS - START 
    =========================== -->
 <%--   <section class="content grid-products border-top">
        <div class="container">
        	 <div class="row">
             	
                <div class="col-xs-6 col-sm-3">
                    <article class="product-item">
                        <img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                        <h3><a href="single-product.html">Sunny Tank Selected Femme</a></h3>
                        <div class="product-rating">
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                        </div>
                        <span class="price">
                        	<del><span class="amount">$36.00</span></del>
                            <ins><span class="amount">$30.00</span></ins>
                        </span>
                    </article>
                </div>
                
                <div class="col-xs-6 col-sm-3">
                    <article class="product-item">
                        <img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                        <h3><a href="single-product.html">Sunny Tank Selected Femme</a></h3>
                        <div class="product-rating">
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                            <i class="fa fa-star"></i>
                        </div>
                        <span class="price">
                        	<del><span class="amount">$36.00</span></del>
                            <ins><span class="amount">$30.00</span></ins>
                        </span>
                    </article>
                </div>
                
                <div class="col-xs-12 col-sm-3">
                    <ul class="list-unstyled small-product">
                    	
               			<!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                                <del><span class="amount">$36.00</span></del>
                            	<ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                            	<del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                            	<del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                            	<del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                    </ul>
                    
                </div>
                
                <div class="col-xs-12 col-sm-3">
                    <ul class="list-unstyled small-product">
                    	
               			<!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                                <del><span class="amount">$36.00</span></del>
                            	<ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                            	<del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                            	<del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                            	<del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                    </ul>
                    
                </div>
             </div>     	
        </div>
    </section>--%>
    <!-- ==========================
    	GRID PRODUCTS - END 
    =========================== -->
    
    <!-- ==========================
    	NOTIFICATION - START 
    =========================== -->
    <%--<section class="content pattern notification">
        <div class="container">
        	<div class="row">
            	<div class="col-sm-4">
                    <span>The new amazing collection is here!</span>
                    <a href="#" class="btn btn-default btn-lg">Let's Go</a>
                </div>
                <div class="col-sm-8">
                    <h3>Summer Collection 2015</h3>
                    <p>Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Vestibulum tortor quam, feugiat vitae, ultricies eget.</p>
        		</div>
            </div>
        </div>
    </section>--%>
     <!-- ==========================
    	NOTIFICATION - END 
    =========================== -->
    
    <!-- ==========================
    	RECENT BLOG POSTS - START 
    =========================== -->
   <%-- <section class="content recent-blog-posts">
        <div class="container">
        	<div class="section-title">
                <h2>Latest from blog</h2>
                <p>Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue.</p>
            </div>
        	<div class="row">
            	
                <!-- BLOG POST - START -->
                <div class="col-xs-6 col-sm-3">
                    <article class="post">
                    	<img src="assets/images/blog/blog-1.jpg" class="img-responsive" alt="">
                        <h3><a href="single-post.html">How to pickup shoes</a></h3>
                        <span class="date">01/12/2015</span>
                    </article>
                </div>
                <!-- BLOG POST - END -->
                
                <!-- BLOG POST - START -->
                <div class="col-xs-6 col-sm-3">
                    <article class="post">
                    	<img src="assets/images/blog/blog-2.jpg" class="img-responsive" alt="">
                        <h3><a href="single-post.html">Fine mens gloves</a></h3>
                        <span class="date">24/11/2015</span>
                    </article>
                </div>
                <!-- BLOG POST - END -->
                
                <!-- BLOG POST - START -->
                <div class="col-xs-6 col-sm-3">
                    <article class="post">
                    	<img src="assets/images/blog/blog-3.jpg" class="img-responsive" alt="">
                        <h3><a href="single-post.html">Sunglasses for a beach</a></h3>
                        <span class="date">10/11/2015</span>
                    </article>
                </div>
                <!-- BLOG POST - END -->
                
                <!-- BLOG POST - START -->
                <div class="col-xs-6 col-sm-3">
                    <article class="post">
                    	<img src="assets/images/blog/blog-4.jpg" class="img-responsive" alt="">
                        <h3><a href="single-post.html">Pyjamas for a good night</a></h3>
                        <span class="date">19/10/2015</span>
                    </article>
                </div>
                <!-- BLOG POST - END -->
                
            </div>
        </div>
    </section>--%>
     <!-- ==========================
    	RECENT BLOG POSTS - END 
    =========================== -->
    
    <!-- ==========================
    	BRANDS - START 
    =========================== -->
   <%-- <section class="content brands pattern border-top border-bottom">
        <div class="container">
        	<div id="brands-carousel">
            	<div class="item"><a href="lookbook.html"><img src="assets/images/clients/1.png" class="img-responsive" alt=""></a></div>
                <div class="item"><a href="lookbook.html"><img src="assets/images/clients/2.png" class="img-responsive" alt=""></a></div>
                <div class="item"><a href="lookbook.html"><img src="assets/images/clients/3.png" class="img-responsive" alt=""></a></div>
                <div class="item"><a href="lookbook.html"><img src="assets/images/clients/4.png" class="img-responsive" alt=""></a></div>
                <div class="item"><a href="lookbook.html"><img src="assets/images/clients/5.png" class="img-responsive" alt=""></a></div>
                <div class="item"><a href="lookbook.html"><img src="assets/images/clients/6.png" class="img-responsive" alt=""></a></div>
                <div class="item"><a href="lookbook.html"><img src="assets/images/clients/7.png" class="img-responsive" alt=""></a></div>
            </div>
        </div>
    </section>--%>
    <!-- ==========================
    	BRANDS - END 
    =========================== -->
        
    <!-- ==========================
    	SMALL PRODUCTS - START 
    =========================== -->
   <%-- <section class="content small-products">
        <div class="container">
        	<div class="row">
            	
                <!-- COLUMN - START -->
                <div class="col-sm-4">
                    <h2>On Sale Products</h2>
                    <ul class="list-unstyled small-product">
                    	
               			<!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <span class="price">
                                <del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <span class="price">
                                <del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <span class="price">
                                <del><span class="amount">$36.00</span></del>
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                    </ul>
                </div>
                <!-- COLUMN - END -->
                
                <!-- COLUMN - START -->
                <div class="col-sm-4">
                    <h2>Featured Products</h2>
                    <ul class="list-unstyled small-product">
                    	
               			<!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <span class="price">
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <span class="price">
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <span class="price">
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                    </ul>
                </div>
                <!-- COLUMN - END -->
                
                <!-- COLUMN - START -->
                <div class="col-sm-4">
                    <h2>Top Rated Products</h2>
                    <ul class="list-unstyled small-product">
                    	
               			<!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                        <li class="clearfix">
                        	<a href="single-product.html">
                            	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                            	<h3>Sunny Tank Selected Femme</h3>
                            </a>
                            <div class="product-rating">
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                                <i class="fa fa-star"></i>
                            </div>
                            <span class="price">
                                <ins><span class="amount">$30.00</span></ins>
                            </span>
                        </li>
                        <!-- PRODUCT - END -->
                        
                    </ul>
                </div>
                <!-- COLUMN - END -->
                
            </div>
        </div>
    </section>--%>
     <!-- ==========================
    	SMALL PRODUCTS - END 
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
                            <li><i class="fa fa-envelope"></i><asp:Label runat="server" ID="lblCorreoTienda"></asp:Label></li>
                            <li><i class="fa fa-phone"></i> <asp:Label runat="server" ID="lblTelefono2"></asp:Label></li>
                    <%--        <li><i class="fa fa-map-marker"></i> 40°44'00.9"N 73°59'43.4"W</li>--%>
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
               <%-- <div class="col-sm-3 col-xs-6">
                    <div class="footer-widget footer-widget-twitter">
                    	<h4>Recent tweets</h4>
                        <div id="twitter-wrapper"></div>
                	</div>
                </div>
                <div class="col-sm-3 col-xs-6">
                	<div class="footer-widget footer-widget-facebook">
                    	<h4>Facebook Page</h4>
                        <ul class="list-unstyled row row-no-padding">
                        	<li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_01.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_02.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_03.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_04.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_01.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_02.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_03.jpg" class="img-responsive" alt=""></a></li>
                            <li class="col-xs-3"><a href="#"><img src="assets/images/avatar/avatar_04.jpg" class="img-responsive" alt=""></a></li>
                        </ul>
                        <p>45,500 Likes  <a href="#" class="btn btn-default btn-sm"><i class="fa fa-thumbs-up"></i>Like</a></p>
                	</div>
                </div>--%>
            </div>
            <div class="footer-bottom">
            	<div class="row">
                    <div class="col-sm-6">
                        <p class="copyright">© Tecnologías de información al extremo.</p>
                      
                    </div>
                    <div class="col-sm-6">
                        <ul class="list-inline payment-methods">
                        	<%--<li><i class="fa fa-cc-amex"></i></li>
                            <li><i class="fa fa-cc-diners-club"></i></li>
                            <li><i class="fa fa-cc-discover"></i></li>
                            <li><i class="fa fa-cc-jcb"></i></li>--%>
                            <li><i class="fa fa-cc-mastercard"></i></li>
                            <li><i class="fa fa-cc-paypal"></i></li>
                           <%-- <li><i class="fa fa-cc-stripe"></i></li>--%>
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
