<%@ page language="VB" autoeventwireup="false" inherits="Products, App_Web_3bsbwt4p" %>

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
    <title>Productos</title>
        
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
                        <li><a href="checkout.html">Checkout</a></li>
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
                                    	<a href="checkout.html" class="btn btn-primary btn-block">Pagar</a>
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
    	PRODUCTS - START 
    =========================== -->
    <section class="content products">
        <div class="container">
            <h2 class="hidden">Productos</h2>
            <div class="row">
                <div class="col-sm-3">
                	<aside class="sidebar">
                		
                        <!-- WIDGET:CATEGORIES - START -->
                        <div class="widget widget-categories">
                        	<h3><a role="button" data-toggle="collapse" href="#widget-categories-collapse" aria-expanded="true" aria-controls="widget-categories-collapse">Categorías</a></h3>
                            <div class="collapse in" id="widget-categories-collapse" aria-expanded="true" role="tabpanel">
                            	<div class="widget-body">
                                    <asp:Panel runat="server" ID="pnlCat">
                                      <%--   <ul class="list-unstyled" id="categories" role="tablist" aria-multiselectable="true">
                                              </ul>--%>

                                        </asp:Panel>

                                   
                                        
                                      <%--  <li class="panel"><a class="collapsed" role="button" data-toggle="collapse" data-parent="#categories" href="#parent-1" aria-expanded="false" aria-controls="parent-1">Men<span>[12]</span></a>
                                            <ul id="parent-1" class="list-unstyled panel-collapse collapse" role="menu">
                                                <li><a href="#">Accessories</a></li>
                                                <li><a href="#">Jackets</a></li>
                                                <li><a href="#">Jumpers</a></li>
                                                <li><a href="#">Jeans</a></li>
                                                <li><a href="#">Shoes</a></li>
                                                <li><a href="#">T-Shirt & Polo Shirts</a></li>
                                                <li><a href="#">Blazers</a></li>
                                            </ul>
                                        </li>
                                        <li class="panel"><a role="button" data-toggle="collapse" data-parent="#categories" href="#parent-2" aria-expanded="true" aria-controls="parent-2">Women<span>[34]</span></a>
                                            <ul id="parent-2" class="list-unstyled panel-collapse collapse in" role="menu">
                                                <li><a href="#">Accessories</a></li>
                                                <li><a href="#">Swimwear</a></li>
                                                <li><a href="#">Basics</a></li>
                                                <li class="active"><a href="#">Dresses</a></li>
                                                <li><a href="#">Jeans</a></li>
                                                <li><a href="#">Skirts</a></li>
                                                <li><a href="#">Leggings</a></li>
                                            </ul>
                                        </li>
                                        <li class="panel"><a class="collapsed" role="button" data-toggle="collapse" data-parent="#categories" href="#parent-3" aria-expanded="false" aria-controls="parent-3">Accessories<span>[8]</span></a>
                                            <ul id="parent-3" class="list-unstyled panel-collapse collapse" role="menu">
                                                <li><a href="#">Basics</a></li>
                                                <li><a href="#">Shirts</a></li>
                                            </ul>
                                        </li>--%>
                                   
                                </div>
                        	</div>
                        </div>
                        <!-- WIDGET:CATEGORIES - END -->
                        
                        <!-- WIDGET:PRICE - START -->
                        <div class="widget widget-price">
                        	<h3><a role="button" data-toggle="collapse" href="#widget-price-collapse" aria-expanded="true" aria-controls="widget-price-collapse">Filter by price</a></h3>
                            <div class="collapse in" id="widget-price-collapse" aria-expanded="true" role="tabpanel">
                                <div class="widget-body">
                                    <div class="price-slider">  
                                        <input type="text" class="pull-left" id="amount" readonly> 
                                        <input type="text" class="pull-right" id="amount2" readonly>                       
                                        <div id="slider-range"></div>  
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- WIDGET:PRICE - END -->
                        
                        <!-- WIDGET:COLOR - START -->
                        <div class="widget widget-color">
                        	<h3><a role="button" data-toggle="collapse" href="#widget-color-collapse" aria-expanded="true" aria-controls="widget-color-collapse">Filter by color</a></h3>
                            <div class="collapse in" id="widget-color-collapse" aria-expanded="true" role="tabpanel">
                                <div class="widget-body">
                                    <div class="checkbox blue">  
                                        <input type="checkbox" value="blue" id="check-blue" checked>  
                                        <label data-toggle="tooltip" data-placement="top" title="Blue" for="check-blue"></label>
                                    </div>
                                    <div class="checkbox red"> 
                                        <input type="checkbox" value="red" id="check-red">  
                                        <label data-toggle="tooltip" data-placement="top" title="Red" for="check-red"></label>
                                    </div>
                                    <div class="checkbox green"> 
                                        <input type="checkbox" value="green" id="check-green">  
                                        <label data-toggle="tooltip" data-placement="top" title="Green" for="check-green"></label>
                                    </div>
                                    <div class="checkbox dark-gray">
                                        <input type="checkbox" value="dark-gray" id="check-dark-gray">  
                                        <label data-toggle="tooltip" data-placement="top" title="Dark Gray" for="check-dark-gray"></label>
                                    </div>
                                    <div class="checkbox dark-cyan"> 
                                        <input type="checkbox" value="dark-cyan" id="check-dark-cyan">  
                                        <label data-toggle="tooltip" data-placement="top" title="Dark Cyan" for="check-dark-cyan"></label>
                                    </div>
                                    <div class="checkbox orange">
                                        <input type="checkbox" value="orange" id="check-orange">  
                                        <label data-toggle="tooltip" data-placement="top" title="Orange" for="check-orange"></label>
                                    </div>
                                    <div class="checkbox pink">  
                                        <input type="checkbox" value="pink" id="check-pink">  
                                        <label data-toggle="tooltip" data-placement="top" title="Pink" for="check-pink"></label>
                                    </div>
                                    <div class="checkbox purple">
                                        <input type="checkbox" value="purple" id="check-purple">  
                                        <label data-toggle="tooltip" data-placement="top" title="Purple" for="check-purple"></label>
                                    </div>
                                    <div class="checkbox brown">  
                                        <input type="checkbox" value="brown" id="check-brown">  
                                        <label data-toggle="tooltip" data-placement="top" title="Brown" for="check-brown"></label>
                                    </div>
                                    <div class="checkbox yellow">
                                        <input type="checkbox" value="yellow" id="check-yellow">  
                                        <label data-toggle="tooltip" data-placement="top" title="Yellow" for="check-yellow"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- WIDGET:COLOR - END -->
                        
                        <!-- WIDGET:SIZE - START -->
                        <div class="widget widget-checkbox">
                        	<h3><a role="button" data-toggle="collapse" href="#widget-size-collapse" aria-expanded="true" aria-controls="widget-size-collapse">Filter by size</a></h3>
                            <div class="collapse in" id="widget-size-collapse" aria-expanded="true" role="tabpanel">
                                <div class="widget-body">
                                	
                                    <div class="checkbox">  
                                        <input id="check-size-xs" type="checkbox" value="size-xs" checked>  
                                        <label for="check-size-xs">XS</label>
                                        <span>[12]</span> 
                                    </div>
                                    <div class="checkbox">  
                                        <input id="check-size-s" type="checkbox" value="size-s">  
                                        <label for="check-size-s">S</label>
                                        <span>[12]</span> 
                                    </div>
                                    <div class="checkbox">  
                                        <input id="check-size-m" type="checkbox" value="size-m">  
                                        <label for="check-size-m">M</label>
                                        <span>[12]</span> 
                                    </div>
                                    <div class="checkbox">  
                                        <input id="check-size-l" type="checkbox" value="size-l">  
                                        <label for="check-size-l">L</label>
                                        <span>[12]</span> 
                                    </div>
                                    <div class="checkbox">  
                                        <input id="check-size-xl" type="checkbox" value="size-xl">  
                                        <label for="check-size-xl">XL</label>
                                        <span>[12]</span> 
                                    </div>
                                    <div class="checkbox">  
                                        <input id="check-size-xll" type="checkbox" value="size-xll">  
                                        <label for="check-size-xll">XXL</label>
                                        <span>[12]</span> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- WIDGET:SIZE - END -->
                        
                        <!-- WIDGET:DISCOUNT - START -->
                        <div class="widget widget-checkbox">
                        	<h3><a role="button" data-toggle="collapse" href="#widget-discount-collapse" aria-expanded="true" aria-controls="widget-discount-collapse">Discount</a></h3>
                            <div class="collapse in" id="widget-discount-collapse" aria-expanded="true" role="tabpanel">
                                <div class="widget-body">
                                    <div class="checkbox">  
                                        <input id="check-with-discount" type="checkbox" value="with-discount">  
                                        <label for="check-with-discount">Products with discount</label>
                                        <span>[12]</span> 
                                    </div>
                                    <div class="checkbox">  
                                        <input id="check-without-discount" type="checkbox" value="without-discount">  
                                        <label for="check-without-discount">Products without discount</label>
                                        <span>[112]</span> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- WIDGET:SIZE - END -->
                        
                	</aside>
                </div>
                <div class="col-sm-9">
                	<div class="products-header">
                    	<div class="row">
                        	<div class="col-xs-6 col-sm-4">
                                <form class="form-inline products-per-page">
                                	<div class="form-group">
                                    	<label>Show:</label>
                                  	</div>
                                  	<div class="form-group">
                                    	<select class="form-control">
                                            <option>6</option>
                                            <option selected="selected">12</option>
                                            <option>18</option>
                                            <option>24</option>
                                            <option>ALL</option>
                                        </select>
                                  	</div>
                                </form>
                            </div>
                                                        
                            <div class="col-xs-6 col-sm-8">
                            	<div class="btn-group toggle-list-grid hidden-xs" role="group">
                                    <button type="button" class="btn btn-default active" id="toggle-grid"><i class="fa fa-th"></i></button>
                                    <button type="button" class="btn btn-default" id="toggle-list"><i class="fa fa-list"></i></button>
                                </div>
                                <form class="form-inline order-by">
                                	<div class="form-group">
                                    	<label>Sort by:</label>
                                  	</div>
                                  	<div class="form-group">
                                    	<select class="form-control">
                                            <option selected="selected">Default</option>
                                            <option>Popularity</option>
                                            <option>Average rating</option>
                                            <option>Newness</option>
                                            <option>Price: low to high</option>
                                            <option>Price: high to low</option>
                                        </select>
                                  	</div>
                                </form>
                            </div>
                            
                        </div>
                    </div>
                    <div class="row grid" id="products">
                        
                        <!-- PRODUCT - START -->
                    	<div class="col-sm-4 col-xs-6">
                            <article class="product-item">
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<div class="product-overlay">
                                            <div class="product-mask"></div>
                                            <a href="single-product.html" class="product-permalink"></a>
                                        	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                                            <img src="assets/images/products/product-1b.jpg" class="img-responsive product-image-2" alt="">
                                            <div class="product-quickview">
                                                <a class="btn btn-quickview" data-toggle="modal" data-target="#product-quickview">Quick View</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                    	<div class="product-body">
                                            <h3>Ut feugiat mauris eget magna egestas</h3>
                                            <div class="product-labels">
                                                <span class="label label-info">new</span>
                                                <span class="label label-danger">sale</span>
                                            </div>
                                            <div class="product-rating">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star-o"></i>
                                                <i class="fa fa-star-o"></i>
                                            </div>
                                            <span class="price">
                                                <del><span class="amount">$36.00</span></del>
                                                <ins><span class="amount">$30.00</span></ins>
                                            </span>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                            <div class="buttons">
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-exchange"></i></a>
                                                <a href="" class="btn btn-primary btn-sm add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-heart"></i></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                    	<div class="col-sm-4 col-xs-6">
                            <article class="product-item">
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<div class="product-overlay">
                                            <div class="product-mask"></div>
                                            <a href="single-product.html" class="product-permalink"></a>
                                        	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                                            <div class="product-quickview">
                                                <a class="btn btn-quickview" data-toggle="modal" data-target="#product-quickview">Quick View</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                    	<div class="product-body">
                                            <h3>Fusce Aliquam</h3>
                                            <div class="product-labels">
                                                <span class="label label-info">new</span>
                                                <span class="label label-danger">sale</span>
                                            </div>
                                            <div class="product-rating">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star-o"></i>
                                                <i class="fa fa-star-o"></i>
                                            </div>
                                            <span class="price">
                                                <del><span class="amount">$36.00</span></del>
                                                <ins><span class="amount">$30.00</span></ins>
                                            </span>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                            <div class="buttons">
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-exchange"></i></a>
                                                <a href="" class="btn btn-primary btn-sm add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-heart"></i></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                    	<div class="col-sm-4 col-xs-6">
                            <article class="product-item">
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<div class="product-overlay">
                                            <div class="product-mask"></div>
                                            <a href="single-product.html" class="product-permalink"></a>
                                        	<img src="assets/images/products/product-3.jpg" class="img-responsive" alt="">
                                            <div class="product-quickview">
                                                <a class="btn btn-quickview" data-toggle="modal" data-target="#product-quickview">Quick View</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                    	<div class="product-body">
                                            <h3>Fusce Aliquam</h3>
                                            <div class="product-labels">
                                                <span class="label label-info">new</span>
                                                <span class="label label-danger">sale</span>
                                            </div>
                                            <div class="product-rating">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star-o"></i>
                                                <i class="fa fa-star-o"></i>
                                            </div>
                                            <span class="price">
                                                <del><span class="amount">$36.00</span></del>
                                                <ins><span class="amount">$30.00</span></ins>
                                            </span>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                            <div class="buttons">
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-exchange"></i></a>
                                                <a href="" class="btn btn-primary btn-sm add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-heart"></i></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                    	<div class="col-sm-4 col-xs-6">
                            <article class="product-item">
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<div class="product-overlay">
                                            <div class="product-mask"></div>
                                            <a href="single-product.html" class="product-permalink"></a>
                                        	<img src="assets/images/products/product-4.jpg" class="img-responsive" alt="">
                                            <div class="product-quickview">
                                                <a class="btn btn-quickview" data-toggle="modal" data-target="#product-quickview">Quick View</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                    	<div class="product-body">
                                            <h3>Fusce Aliquam</h3>
                                            <div class="product-labels">
                                                <span class="label label-info">new</span>
                                                <span class="label label-danger">sale</span>
                                            </div>
                                            <div class="product-rating">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star-o"></i>
                                                <i class="fa fa-star-o"></i>
                                            </div>
                                            <span class="price">
                                                <del><span class="amount">$36.00</span></del>
                                                <ins><span class="amount">$30.00</span></ins>
                                            </span>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                            <div class="buttons">
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-exchange"></i></a>
                                                <a href="" class="btn btn-primary btn-sm add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-heart"></i></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                    	<div class="col-sm-4 col-xs-6">
                            <article class="product-item">
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<div class="product-overlay">
                                            <div class="product-mask"></div>
                                            <a href="single-product.html" class="product-permalink"></a>
                                        	<img src="assets/images/products/product-1.jpg" class="img-responsive" alt="">
                                            <div class="product-quickview">
                                                <a class="btn btn-quickview" data-toggle="modal" data-target="#product-quickview">Quick View</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                    	<div class="product-body">
                                            <h3>Fusce Aliquam</h3>
                                            <div class="product-labels">
                                                <span class="label label-info">new</span>
                                                <span class="label label-danger">sale</span>
                                            </div>
                                            <div class="product-rating">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star-o"></i>
                                                <i class="fa fa-star-o"></i>
                                            </div>
                                            <span class="price">
                                                <del><span class="amount">$36.00</span></del>
                                                <ins><span class="amount">$30.00</span></ins>
                                            </span>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                            <div class="buttons">
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-exchange"></i></a>
                                                <a href="" class="btn btn-primary btn-sm add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-heart"></i></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <!-- PRODUCT - END -->
                        
                        <!-- PRODUCT - START -->
                    	<div class="col-sm-4 col-xs-6">
                            <article class="product-item">
                            	<div class="row">
                                	<div class="col-sm-3">
                                    	<div class="product-overlay">
                                            <div class="product-mask"></div>
                                            <a href="single-product.html" class="product-permalink"></a>
                                        	<img src="assets/images/products/product-2.jpg" class="img-responsive" alt="">
                                            <div class="product-quickview">
                                                <a class="btn btn-quickview" data-toggle="modal" data-target="#product-quickview">Quick View</a>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-9">
                                    	<div class="product-body">
                                            <h3>Fusce Aliquam</h3>
                                            <div class="product-labels">
                                                <span class="label label-info">new</span>
                                                <span class="label label-danger">sale</span>
                                            </div>
                                            <div class="product-rating">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star-o"></i>
                                                <i class="fa fa-star-o"></i>
                                            </div>
                                            <span class="price">
                                                <del><span class="amount">$36.00</span></del>
                                                <ins><span class="amount">$30.00</span></ins>
                                            </span>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                            <div class="buttons">
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-exchange"></i></a>
                                                <a href="" class="btn btn-primary btn-sm add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                                <a href="" class="btn btn-primary btn-sm"><i class="fa fa-heart"></i></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </article>
                        </div>
                        <!-- PRODUCT - END -->
                        
                    </div>
                    
                    <div class="pagination-wrapper">
                        <ul class="pagination">
                            <li><a href="#"><i class="fa fa-angle-double-left"></i></a></li>
                            <li><a href="#">1</a></li>
                            <li class="active"><a href="#">2</a></li>
                            <li><a href="#">3</a></li>
                            <li><a href="#">4</a></li>
                            <li><a href="#"><i class="fa fa-angle-double-right"></i></a></li>
                        </ul>
                    </div>
                    
                </div>
            </div>
        </div>
    </section>
    <!-- ==========================
    	PRODUCTS - END 
    =========================== -->
    
    <!-- ==========================
    	PRODUCT QUICKVIEW - START
    =========================== -->
    <div class="modal fade modal-quickview" id="product-quickview" tabindex="-1" role="dialog">
    	<div class="modal-dialog" role="document">
    		<div class="modal-content">
          		<div class="modal-header">
            		<button type="button" class="close" data-dismiss="modal" aria-label="Close"><i class="fa fa-times"></i></button>
          		</div>
                <div class="modal-body">
                    <article class="product-item product-single">
                        <div class="row">
                            <div class="col-sm-4">
                                <div class="product-carousel-wrapper hidden">
                                    <div id="product-carousel-modal">
                                        <div class="item"><img src="assets/images/products/product-1.jpg" class="img-responsive" alt=""></div>
                                        <div class="item"><img src="assets/images/products/product-2.jpg" class="img-responsive" alt=""></div>
                                        <div class="item"><img src="assets/images/products/product-3.jpg" class="img-responsive" alt=""></div>
                                        <div class="item"><img src="assets/images/products/product-4.jpg" class="img-responsive" alt=""></div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-8">
                                <h3>Fusce Aliquam</h3>
                                <div class="product-labels">
                                    <span class="label label-info">new</span>
                                    <span class="label label-danger">sale</span>
                                </div>
                                <div class="product-rating">
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star"></i>
                                    <i class="fa fa-star-o"></i>
                                    <i class="fa fa-star-o"></i>
                                </div>
                                <span class="price">
                                    <del><span class="amount">$36.00</span></del>
                                    <ins><span class="amount">$30.00</span></ins>
                                </span>
                                <ul class="list-unstyled product-info">
                                    <li><span>ID</span>U-187423</li>
                                    <li><span>Availability</span>In Stock</li>
                                    <li><span>Brand</span>Esprit</li>
                                    <li><span>Tags</span>Dress, Black, Women</li>
                                </ul>
                                <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut feugiat mauris eget magna egestas porta. Curabitur sagittis sagittis neque rutrum congue. Donec lobortis dui sagittis, ultrices nunc ornare, ultricies elit. Curabitur tristique felis pulvinar nibh porta. </p>
                                <div class="product-form clearfix">
                                    <div class="row row-no-padding">
                                        
                                        <div class="col-lg-3 col-md-4 col-sm-6">
                                            <div class="product-quantity clearfix">
                                                <a class="btn btn-default" id="modal-qty-minus">-</a>
                                                <input type="text" class="form-control" id="modal-qty" value="1">
                                                <a class="btn btn-default" id="modal-qty-plus">+</a>
                                            </div>
                                        </div>
                                        
                                        <div class="col-lg-3 col-md-4 col-sm-6">
                                            <div class="product-size">
                                                <form class="form-inline">
                                                    <div class="form-group">
                                                        <label>Size:</label>
                                                    </div>
                                                    <div class="form-group">
                                                        <select class="form-control">
                                                            <option>XS</option>
                                                            <option>S</option>
                                                            <option selected="selected">M</option>
                                                            <option>L</option>
                                                            <option>XL</option>
                                                            <option>XXL</option>
                                                        </select>
                                                    </div>
                                                </form>
                                            </div>
                                        </div>
                                        
                                        <div class="col-lg-3 col-md-4 col-sm-6">
                                            <div class="product-color">
                                                <form class="form-inline">
                                                    <div class="form-group">
                                                        <label>Color:</label>
                                                    </div>
                                                    <div class="form-group">
                                                        <select class="form-control">
                                                            <option selected="selected">Black</option>
                                                            <option>White</option>
                                                            <option>Red</option>
                                                            <option>Yellow</option>
                                                        </select>
                                                    </div>
                                                </form>
                                            </div>
                                        </div>
                                        
                                        <div class="col-lg-3 col-md-12 col-sm-6">
                                            <a href="" class="btn btn-primary add-to-cart"><i class="fa fa-shopping-cart"></i>Add to cart</a>
                                        </div>
                                        
                                    </div>
                                </div>
                                <ul class="list-inline product-links">
                                    <li><a href="#"><i class="fa fa-heart"></i>Add to wishlist</a></li>
                                    <li><a href="#"><i class="fa fa-exchange"></i>Compare</a></li>
                                    <li><a href="#"><i class="fa fa-envelope"></i>Email to friend</a></li>
                                </ul>
                            </div>
                        </div>
                    </article>
                </div>
    		</div>
    	</div>
    </div>
    <!-- ==========================
    	PRODUCT QUICKVIEW - END 
    =========================== -->
    
    <!-- ==========================
    	NEWSLETTER - START 
    =========================== -->
   <%-- <section class="separator separator-newsletter">
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
                            <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna.</p>
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
   <%-- <div class="modal fade" tabindex="-1" role="dialog" id="modalAdvertising">
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
