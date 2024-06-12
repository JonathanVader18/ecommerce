<%@ page language="VB" autoeventwireup="false" inherits="adminclientes, App_Web_3bsbwt4p" %>

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

    
        <form runat="server">
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
			<div class="col-xs-12 col-sm-5">
	            <!-- <img src="img/header/logo.png" class="img-responsive cent-img"> -->
	           <!--  <div class="col-xs-12 text-center">
	                    <strong class="text-underline">ACCESO INTERNO</strong>
	                    <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div>
	            </div> -->
	            <div class="marco-form">
	            	<div class="col-xs-12 text-center">
	                    <strong class="text-underline">PARAMETRIZACIONES DE CLIENTES</strong>
	                    <div class="text-login"><p>Cambiar los parámetros con los que se dan de alta los clientes</p></div>
	            	</div>
	                <div  class="form-general"> 
                         <div class="form-group">
	                    <label for="exampleInputEmail1">Grupo de socios de negocio</label>
	                     <asp:DropDownList ID="ddlGrupoClientes" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
	                  </div>
	                  <div class="form-group">
	                    <label for="exampleInputEmail1">Lista de precios</label>
	                      <asp:DropDownList ID="ddlListaPrecios" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
	                  </div>
	                  <div class="form-group">
	                    <label for="exampleInputPassword1">Cuenta contable</label>
	                     <asp:DropDownList ID="ddlCuentacontable" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
	                  </div>
	                 
                     
	                

	                 
                        <asp:Button ID="btnEntrar" runat="server" Text="Guardar Cambios"  CssClass ="btn btn-general-3"  OnClientClick ="ShowProgress();"/>
	                  

	                </div>
	            </div>
	        </div>
	    </div>
	</div>
                         </ContentTemplate>
                      <Triggers>
                   <%--     <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName="Click" ></asp:AsyncPostBackTrigger>--%>
                <asp:PostBackTrigger ControlID="btnEntrar"  />
               
            </Triggers>
                </asp:UpdatePanel>
     <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(ResultsUpdatePanel.ClientID) %>').style.display = "inline";
    }
    

    </script>
    
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
            </form>
    </body>
    </html>

