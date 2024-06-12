<%@ Page Language="VB" AutoEventWireup="false" CodeFile="mi-cuenta.aspx.vb" Inherits="mi_cuenta" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
	                    <strong class="text-underline">CREAR CUENTA</strong>
	                    <div class="text-login"><p>Ingresa tu dirección de correo y una<br>contraseña para crear tu cuenta</p></div>
	            	</div>
	                <div  class="form-general"> 
                         <div class="form-group">
	                    <label for="exampleInputEmail1">nombre completo</label>
	                    <asp:TextBox ID="txtnombre" runat="server" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                  <div class="form-group">
	                    <label for="exampleInputEmail1">correo electrónico</label>
	                    <asp:TextBox ID="txtUsuario" runat="server" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                  <div class="form-group">
	                    <label for="exampleInputPassword1">contraseña (al menos 6 caracteres)</label>
	                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                  <div class="form-group">
	                    <label for="exampleInputPassword1">confirmar contraseña</label>
                          <asp:TextBox ID="txtConfirma" runat="server" TextMode="Password" CssClass ="form-control"></asp:TextBox>
	                  </div>
                        <asp:Panel ID="pnlCFDI" runat="server" Visible ="false" >
                         <div class="form-group">
	                    <label for="exampleInputEmail1">rfc (opcional)</label>
	                    <asp:TextBox ID="txtRFC" runat="server" CssClass ="form-control" MaxLength="13"></asp:TextBox>
	                  </div>
                         <div class="form-group">
	                    <label for="exampleInputEmail1">uso de cfdi (opcional)</label>
	                      <asp:DropDownList ID="ddlUsoCFDI" runat="server" Width="100%" AutoPostBack="false" CssClass="form-control"></asp:DropDownList>
	                  </div>
                        </asp:Panel>
	                  <div class="form-group">
						<div class="checkbox-2">
						    <label>
                                <asp:CheckBox ID="chkOfertas" runat="server" Text ="Quiero recibir ofertas y promociones" Visible ="false"  />
						     </label>
					  	</div>
					  </div>

	                 
                        <asp:Button ID="btnEntrar" runat="server" Text="crear cuenta"  CssClass ="btn btn-general-3"  OnClientClick ="ShowProgress();"/>
	                  <div class="avisos"> 
	                  	<p>al crear una cuenta, estás de acuerdo con nuestros <a href="ayuda.aspx?Men=preguntas%20frecuentes&Sub=Términos%20y%20Condiciones">términos y condiciones</a>.</p>
	                  </div>
                        <a  class ="btn btn-general-4" href="catalogo.aspx">ingresar</a>
	                  <asp:Button ID="btnIngresar" runat="server" Text="ingresar"  CssClass ="btn btn-general-4" Visible ="false" />

	                </div>
	            </div>
	        </div>
	    </div>
	</div>
                         </ContentTemplate>
                      <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName="Click" ></asp:AsyncPostBackTrigger>
             <%--   <asp:PostBackTrigger ControlID="btnEntrar"  />--%>
               
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
    </asp:Content>


