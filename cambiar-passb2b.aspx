<%@ Page Language="VB" AutoEventWireup="false" CodeFile="cambiar-passb2b.aspx.vb" Inherits="cambiar_passb2b" %>

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

			<div class="col-xs-12 col-sm-10 stl-1-p contenido">
				<div class="blk-genericos">
	                <div class="col-sm-12">
		                <div class="tit-bloque"><h2>cambiar contraseña</h2></div>
	                </div>
 						<div class="content-blok">
						    <div class="col-xs-12 col-sm-12">
						    	<div class="form-group">
                                    <asp:Panel ID="pnlPass" runat="server" Visible ="false" >
                                        <label for="exampleInputPassword1">Escribe tu contraseña actual</label>
                                     <asp:TextBox ID="txtPassActual" runat="server" CssClass ="form-control" TextMode ="Password" ></asp:TextBox>

                                    </asp:Panel>
							      
							        
							    </div>
							    <div class="form-group">
							        <label for="exampleInputPassword1">Nueva contraseña</label>
							         <asp:TextBox ID="txtNuevoPass" runat="server" CssClass ="form-control" TextMode ="Password"></asp:TextBox>
							    </div>
							    <div class="form-group">
							        <label for="exampleInputPassword1">Confirmar contraseña</label>
							       <asp:TextBox ID="txtConfirmaNuevoPass" runat="server" CssClass ="form-control" TextMode ="Password"></asp:TextBox>
							    </div>
                                <asp:Button ID="btnGuardar" runat="server" Text="cambiar"  CssClass ="btn btn-general-2 btn-left"/>
                                 
                                <a type="submit" class="btn btn-general-1 btn-left" href="catalogo.aspx">Regresar</a>

							
								
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