<%@ page language="VB" autoeventwireup="false" inherits="menuConfig, App_Web_3bsbwt4p" %>

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
        <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <link rel="stylesheet" href="css/style.css">
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>

    </head>
<body class="gtk-2">
    <div class="col-xs-12 col-sm-9">
        <img src="img/header/logo.png" class="img-responsive cent-img">
        <div class="col-xs-12 text-center">
            <strong class="text-underline">PANEL DE ADMINISTRACIÓN</strong>
            <div class="text-login">
                <p>Elige la sección que deseas administrar</p>
            </div>
        </div>
        <div class="marco-form text-center ">
            <div class="blk-genericos">


                <form runat="server">
                    <div class="row ">
                        <div class="col-xs-4 col-sm-4 ">
                            <asp:Button ID="btnBanners" runat="server" Text="Mis Banners" class="btn btn-general-2" />
                        </div>

                        <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnUsuarios" runat="server" Text="Usuarios Vendedores" class="btn btn-general-2" />

                        </div>
                    </div>
                    <div class="row ">

                        <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnSincronizar" runat="server" Text="Sincronizar Categorías" class="btn btn-general-2" Visible="false" />

                        </div>
                        <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnDescuentos" runat="server" Text="Administrar Descuentos" class="btn btn-general-2" Visible="false" />

                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnPRomos" runat="server" Text="Artículos Promoción" class="btn btn-general-2" Visible="true" />

                        </div>
                        <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnAdminClientes" runat="server" Text="Configuración clientes" class="btn btn-general-2" Visible="false" />

                        </div>
                    </div>
                      <div class="row ">
                           <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnActivarB2C" runat="server" Text="Activar B2C" class="btn btn-general-2" Visible="false" />

                        </div>
                           <div class="col-xs-4 col-sm-4">
                            <asp:Button ID="btnDesactivarB2C" runat="server" Text="Desactivar B2C" class="btn btn-general-2" Visible="false" />

                        </div>
 </div>
                    <div class="post-auto">
                    </div>



                </form>
            </div>
        </div>
    </div>

 </body>
</html>
