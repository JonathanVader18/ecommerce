<%@ page language="VB" autoeventwireup="false" inherits="recuperarVend, App_Web_3bsbwt4p" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <meta charset="utf-8">
        <meta http-equiv="x-ua-compatible" content="ie=edge">
        <title></title>
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width, initial-scale=1">

        <link rel="apple-touch-icon" href="apple-touch-icon.png">
        <!-- Place favicon.ico in the root directory -->
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">
        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700" rel="stylesheet">
        <link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet">
        <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <link rel="stylesheet" href="css/style.css">
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>

    </head>
<body class="gtk-2">
       <form runat ="server">
     <div class="col-xs-12 col-sm-12">
            <img src="img/header/logo.png" class="img-responsive cent-img">
              <div class="main-container">
<div class="act-altcuenta text-center">
			<div class="col-xs-12 col-sm-12">
	            <!-- <img src="img/header/logo.png" class="img-responsive cent-img"> -->
	              <!--<div class="col-xs-12 text-center">
	                  <strong class="text-underline">INGRESO</strong>
	                     <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div> 
	            </div>-->
	            <div class="marco-form">
	            	<div class="col-xs-12 text-center">
	                  <strong class="text-underline">Recupera tu contraseña</strong>
	                     <div class="text-login"><p>Escribe la dirección de correo con la que se registró tu cuenta y te enviaremosuna liga para que puedas reestablecerla.</p></div> 
	            	
                     <div  class="form-general"> 
	                  <div class="form-group">
	                    <label for="exampleInputEmail1">Correo</label>
	                     <asp:TextBox ID="txtCorreo" runat="server" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                    <asp:Button ID="btnEntrar" runat="server" Text="enviar"  CssClass ="btn btn-general-3"/>
	              
	                </div>
                    </div>
	               
	            </div>
	        </div>
	    </div>
                   </div>
		
	</div>
        </form>
</body>
    </html>
