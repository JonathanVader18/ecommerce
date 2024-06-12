<%@ page language="VB" autoeventwireup="false" inherits="mi_cuentahnd, App_Web_3bsbwt4p" %>

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
        <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/detectizr/2.2.0/detectizr.min.js"></script>

    <script type ="text/javascript" >
         function justNumbers(e)
        {
        var keynum = window.event ? window.event.keyCode : e.which;
        if ((keynum == 8) )
        return true;
         
        return /\d/.test(String.fromCharCode(keynum));
        }
       function pulsar(e) { 
 var tecla = (document.all) ? e.keyCode :e.which; 
  return (tecla!=13); 
        } 

       
    </script>
    <script type ="text/javascript">
function stopRKey(evt) {
   var evt = (evt) ? evt : ((event) ? event : null);
   var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
   if ((evt.keyCode == 13) && (node.type=="text")) {return false;}
}
document.onkeypress = stopRKey;
</script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                    <span style="border-width: 0px; position: fixed; padding: 50px; background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                    <img src="LOADER_mm.gif">
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="main-container">
                    <div class="act-altcuenta">
                        <div class="col-xs-12">
                            <!-- <img src="img/header/logo.png" class="img-responsive cent-img"> -->
                            <!--  <div class="col-xs-12 text-center">
	                    <strong class="text-underline">ACCESO INTERNO</strong>
	                    <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div>
	            </div> -->
                            <div class="marco-form">
                                <div class="col-xs-12 text-center">
                                    <strong class="text-underline">CREAR CUENTA</strong>
                                    <div class="text-login">
                                        <p>
                                            Ingresa tu dirección de correo y una<br>
                                            contraseña para crear tu cuenta
                                        </p>
                                    </div>
                                </div>
                                <div class="form-general">
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Nombre completo</label>
                                        <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                       <div class="form-group">
                                        <label for="exampleInputEmail1">Empresa</label>
                                        <asp:TextBox ID="txtEmpresa" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputEmail1">RTN o Identidad *</label>
                                        <asp:TextBox ID="txtRFC" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>


                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Dirección *</label>
                                        <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Ciudad</label>
                                        <asp:TextBox ID="txtMunicipio" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>


                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Código Postal</label>
                                        <asp:TextBox ID="txtCP" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                     <div class="form-group">
                                        <label for="exampleInputEmail1">Teléfono</label>
                                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                      <div class="form-group">
                                        <label for="exampleInputEmail1">Teléfono celular</label>
                                        <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">País *</label>
                                        <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        <asp:TextBox ID="txtPais" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label for="exampleInputEmail1">Ciudad *</label>
                                        <asp:DropDownList ID="ddlEstados" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                                    </div>



                                    <div class="form-group">
                                        <label for="exampleInputEmail1">correo electrónico *</label>
                                        <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputPassword1">contraseña (al menos 6 caracteres) *</label>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label for="exampleInputPassword1">confirmar contraseña</label>
                                        <asp:TextBox ID="txtConfirma" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <div class="checkbox-2">
                                            <label>
                                                <asp:CheckBox ID="chkOfertas" runat="server" Text="Quiero recibir ofertas y promociones" />
                                            </label>
                                        </div>
                                    </div>


                                    <asp:Button ID="btnEntrar" runat="server" Text="crear cuenta" CssClass="btn btn-general-3" />
                                    <div class="avisos">
                                        <p>al crear una cuenta, estás de acuerdo con nuestros <a href="ayuda.aspx?Men=preguntas%20frecuentes&Sub=Términos%20y%20Condiciones">términos y condiciones</a>.</p>
                                    </div>
                                    <asp:Button ID="btnIngresar" runat="server" Text="iniciar sesión" CssClass="btn btn-general-4" />

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName="Click"></asp:AsyncPostBackTrigger>
                <%--   <asp:PostBackTrigger ControlID="btnEntrar"  />--%>
                <asp:AsyncPostBackTrigger ControlID="ddlPais" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
            </Triggers>
        </asp:UpdatePanel>
        <script type="text/javascript">


            function ShowProgress() {
                document.getElementById('<% Response.Write(ResultsUpdatePanel.ClientID) %>').style.display = "inline";
            }


        </script>
    </form>
</body>
    </html>
