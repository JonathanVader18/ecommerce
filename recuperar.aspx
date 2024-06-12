<%@ Page Language="VB" AutoEventWireup="false" CodeFile="recuperar.aspx.vb" Inherits="recuperar" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-container">
		<div class="act-altcuenta">
			<div class="col-xs-12 col-sm-5">
	            <!-- <img src="img/header/logo.png" class="img-responsive cent-img"> -->
	              <!--<div class="col-xs-12 text-center">
	                  <strong class="text-underline">INGRESO</strong>
	                     <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div> 
	            </div>-->
	            <div class="marco-form">
	            	<div class="col-xs-12 text-center">
	                  <strong class="text-underline">Recupera tu contraseña</strong>
	                     <div class="text-login"><p>Escribe la dirección de correo con la que abriste tu cuenta y te enviaremos<br> tu contraseña.</p></div> 
	            	</div>
	                <div  class="form-general"> 
	                  <div class="form-group">
	                    <label for="exampleInputEmail1">correo</label>
	                     <asp:TextBox ID="txtCorreo" runat="server" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                    <asp:Button ID="btnEntrar" runat="server" Text="enviar"  CssClass ="btn btn-general-3"/>
	              
	                </div>
	            </div>
	        </div>
	    </div>
	</div>
</asp:Content>