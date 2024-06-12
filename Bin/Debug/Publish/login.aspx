<%@ page language="VB" autoeventwireup="false" inherits="login, App_Web_3bsbwt4p" %>

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
     <div class="col-xs-12 col-sm-6">
            <img src="img/header/logo.png" class="img-responsive cent-img">
            <div class="col-xs-12 text-center">
                    <strong class="text-underline">ACCESO INTERNO</strong>
                    <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div>
            </div>
            <div class="marco-form">
                <form runat="server" >
                  <div class="form-group">
                    <label for="exampleInputEmail1">usuario</label>
                        <asp:TextBox ID="txtUser" runat="server" CssClass ="form-control "></asp:TextBox>
                  </div>
                  <div class="form-group">
                    <label for="exampleInputPassword1">contraseña</label>
                    <asp:TextBox ID="txtPass" runat="server" CssClass ="form-control " TextMode="Password"></asp:TextBox>
                            
                  </div>
                  <div class="post-auto">
                    <a class="link-lp" href="recuperarVend.aspx">¿olvidaste tu contraseña?</a>
                  </div>
                     <asp:Button ID="btnIngresar" runat="server" Text="ingresar" class="btn btn-general-2"/>
                    
                </form>
            </div>
        </div>

 </body>
</html>
