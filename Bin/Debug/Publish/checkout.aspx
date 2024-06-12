<%@ page language="VB" autoeventwireup="false" inherits="checkout, App_Web_3bsbwt4p" %>

<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Realizar Pago.</title>


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




    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/css/bootstrap.min.css" integrity="sha384-Vkoo8x4CGsO3+Hhxv8T/Q5PaXtkKtu6ug5TOeNV6gBiFeWPGFN9MuhOf23Q9Ifjh" crossorigin="anonymous" />
    <link href="//cdn.jsdelivr.net/npm/@sweetalert2/theme-dark@3/dark.css" rel="stylesheet" />

    <script src="https://code.jquery.com/jquery-3.4.1.slim.min.js" integrity="sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js" integrity="sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js" integrity="sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdn.conekta.io/js/latest/conekta.js"></script>
    <script src="//cdn.jsdelivr.net/npm/sweetalert2@9/dist/sweetalert2.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bowser/1.9.4/bowser.min.js"></script>
    


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

</head>
<body>


     <div class="main-container" id="Principal">
		<div class="act-altcuenta">
			<div class="col-xs-12 col-sm-5">
                <img src="images/stop_negro.png" class="img-responsive cent-img" style="width:120px;height:60px;">
                 <strong class="text-underline">REALIZAR PAGO</strong>
	                    <div class="text-login"><p>Ingresa sus datos para procesar el cobro</p></div>
	          
	           <!--  <div class="col-xs-12 text-center">
	                    <strong class="text-underline">ACCESO INTERNO</strong>
	                    <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div>
	            </div> -->
	            <div class="marco-form" >
 <form action="" method="POST" id="card-form">
                   
                          <div class="form-group">
                            <asp:Label ID="lblNombre" runat="server" Text="Nombre Completo"></asp:Label>
                            <input value="" data-conekta="card[name]" class="form-control" placeholder="Nombre Completo." />
                        </div>
                          <div class="form-group">
                            <asp:Label ID="lblTarjeta" runat="server" Text="Número de Tarjeta"></asp:Label>
                            <input value="" data-conekta="card[number]" class="form-control" placeholder="Número de Tarjeta" maxlength="20" />
                        </div>
                         <div class="form-group">
                            <asp:Label ID="lblFechaExpiracion" runat="server" Text="Fecha de Expiración (MM/AAAA)"></asp:Label>
                            <div class="row">
                                <div class="col-4 col-md-4">
                                    <input value="" data-conekta="card[exp_month]" class="form-control" placeholder="MM" maxlength="2" />
                                </div>
                                <div class="col-4 col-md-4">
                                    <input value="" data-conekta="card[exp_year]" class="form-control" placeholder="AAAA" maxlength="4" />
                                </div>
    </div>
 </div>
      <div class="form-group">
           <div class="row">
                                <div class="col-4 col-md-4">
            <asp:Label ID="Label1" runat="server" Text="CVC"></asp:Label>
                                    </div>

                               </div>
                               <div class="row">
                                <div class="col-4 col-md-4">
                                    <input type="text" size="4" data-conekta="card[cvc]" class="form-control" placeholder="CVC" />
                                </div>

                               </div>

                         </div>
                       
                   
                    <div class="row mt-2 justify-content-end">
                        <button type="submit" class="btn btn-success">Pagar</button>
                    </div>
                </form>
                    </div>
                <div class="text-login"><p>Servicio proporcionado por Conekta</p></div>
                <img src="images/Visa-MasterCard.png" style="width:90px;height:35px">
                <img src="images/logoConekta.png" style="width:130px;height:35px">
                <img src="images/secure.png" style="width:130px;height:45px">
                </div>
            </div>
         </div>
      <div class="container">

       <%-- <div class="card" id="Principal">
            <div class="card-header">
                <h3>Realizar Pagos</h3>

            </div>


            <div class="card-body">
               

            </div>
        </div>--%>
        <form id="form1" runat="server">
            <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
            <asp:UpdateProgress ID="updateProgress1" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                        <span style="border-width: 0px; position: fixed; padding: 50px; background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                        <img src="../LOADER_mm.gif" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="ResultsUpdatePanel" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:TextBox ID="txtToken" runat="server" Enabled="false"></asp:TextBox>
                    <asp:Button ID="btnConfirmar" runat="server" Text="Pagar" CssClass="btn btn-link" OnClick="btnConfirmar_Click" />
                    <asp:Label ID="lblError" runat="server" Text="" CssClass="text-danger"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnConfirmar" EventName="Click"></asp:AsyncPostBackTrigger>
                </Triggers>
            </asp:UpdatePanel>
        </form>
    </div>
    <script type="text/javascript">
        $('#txtToken').hide();
        $('#btnConfirmar').hide();
       // Conekta.setPblicKey("key_Be3XK8qh8WzRr7eCrfHh9qA");
        Conekta.setPublicKey("key_Yv9AUUaNdjS662VCYqf9gYw");
        Conekta.getPublicKey();
        Conekta.setLanguage("es");
        Conekta.getLanguage();


        var conektaSuccessResponseHandler = function (token) {
            document.getElementById("<%=txtToken.ClientID%>").value = token.id;
            var $form = $("#card-form");
            $("#<%=txtToken.ClientID%>").hide();
            $('#Descripcion').show();
            $form[0].reset();
            var objBoton = document.getElementById('<%=btnConfirmar.ClientID%>');
            objBoton.click();
        };
        var conektaErrorResponseHandler = function (response) {
            var $form = $("#card-form");
            if (bowser.name != "Internet Explorer") {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: response.message_to_purchaser
                });
            } else {
                alert(response.message_to_purchaser);
            }
            $('#Descripcion').hide();
            $form.find("button").prop("disabled", false);
        };


        //jQuery para que genere el token después de dar click en submit
        $(function () {
            $("#card-form").submit(function (event) {
                var $form = $(this);
                // Previene hacer submit más de una vez
                $form.find("button").prop("disabled", true);
                Conekta.Token.create($form, conektaSuccessResponseHandler, conektaErrorResponseHandler);
                return false;
            });
        });
        function FnConfirmar() {
            while ($('#txtToken').val == "");
            return confirm("Se confirmara el pago.");
        }
    </script>
</body>

</html>
