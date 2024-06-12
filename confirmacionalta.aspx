<%@ Page Language="VB" AutoEventWireup="false" CodeFile="confirmacionalta.aspx.vb" Inherits="confirmacionalta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

  


<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

      <meta http-equiv="x-ua-compatible" content="ie=edge"/>
     
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width, initial-scale=1"/>

    
    <link rel="apple-touch-icon" sizes="57x57" href="favicon/apple-touch-icon-57x57.png">
        <link rel="apple-touch-icon" sizes="60x60" href="favicon/apple-touch-icon-60x60.png">
        <link rel="apple-touch-icon" sizes="72x72" href="favicon/apple-touch-icon-72x72.png">
        <link rel="apple-touch-icon" sizes="76x76" href="favicon/apple-touch-icon-76x76.png">
        <link rel="apple-touch-icon" sizes="114x114" href="favicon/apple-touch-icon-114x114.png">
        <link rel="apple-touch-icon" sizes="120x120" href="favicon/apple-touch-icon-120x120.png">
        <link rel="apple-touch-icon" sizes="144x144" href="favicon/apple-touch-icon-144x144.png">
        <link rel="apple-touch-icon" sizes="152x152" href="favicon/apple-touch-icon-152x152.png">
        <link rel="apple-touch-icon" sizes="180x180" href="favicon/apple-touch-icon-180x180.png">
        <link rel="icon" type="image/png" sizes="32x32" href="favicon/favicon-32x32.png">
        <link rel="icon" type="image/png" sizes="192x192" href="favicon/android-chrome-192x192.png">
        <link rel="icon" type="image/png" sizes="16x16" href="favicon/favicon-16x16.png">
        <link rel="manifest" href="favicon/manifest.json">
        <link rel="mask-icon" href="favicon/safari-pinned-tab.svg" color="#c22820">
        <meta name="msapplication-TileColor" content="#ffffff">
        <meta name="msapplication-TileImage" content="/mstile-144x144.png">
        <meta name="theme-color" content="#ffffff">


  <%--      <link rel="apple-touch-icon" href="apple-touch-icon.png"/>--%>
        <!-- Place favicon.ico in the root directory -->
           <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"/>
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous"/>
        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700" rel="stylesheet"/>
        <link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet"/>
        <link rel="stylesheet" href="css/jquery.bootstrap-touchspin.min.css"/>
        <link rel="stylesheet" href="css/style.css"/>
        <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/detectizr/2.2.0/detectizr.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
             <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


     <div class="col-xs-12">
		<div class="main-container con-principal">
            <div class="col-xs-12 col-sm-2">
                <div class="filtros-dektop">
                    <div class="panel-group filtos-catalogo" id="accordion" role="tablist" aria-multiselectable="true">
           
                    </div>
                </div>
                </div>

            <div class="col-xs-12 col-sm-10 stl-1-p contenido">
                <div class="blk-genericos">
                    <div class="col-sm-12">
                        <div class="tit-bloque">
                             
                             <img class="logo" src="img/header/logo.png">
                              <strong> <asp:Label ID="lblBienvenida" runat="server" Text="Label"></asp:Label></strong>
                           
                        </div>
                    </div>
                    <div class="content-blok">
                        <div class="grup-content">
                            <div class="singular-content col-xs-12 col-sm-12">
                                <div>
                                    <asp:Label ID="lblUsuario" runat="server" Text="Label"></asp:Label><br />
                                   <h2>TU CUENTA SE HA CREADO CON ÉXITO. EN BREVE TE LLEGARÁ UN CORREO CON TUS ACCESOS AL PORTAL</h2>
                                </div>

                                <div class="blk-action-btn">
									<a class="btn-act-blok " href="loginb2b.aspx">Ingresar</a>
								</div>
                                  <div class="blk-action-btn">
                                <asp:Button ID="btnRegresar" runat="server" Text="regresar al pago" cssclass="btn-act-blok" Visible ="false"  />
 </div>
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