<%@ page language="VB" autoeventwireup="false" inherits="ingresar, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="main-container">
		<div class="act-altcuenta">
			<div class="col-xs-12 col-sm-5">
	            <!-- <img src="img/header/logo.png" class="img-responsive cent-img"> -->
	            <div class="col-xs-12 text-center">
	                    <strong class="text-underline">INGRESO</strong>
	                    <!-- <div class="text-login"><p>Ingresa tu nombre<br>de usuario y contraseña</p></div> -->
	            </div>
	            <div class="marco-form">
	                <div  class="form-general"> 
	                  <div class="form-group">
	                    <label for="exampleInputEmail1">usuario</label>
	                    <asp:TextBox ID="txtuser" runat="server" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                  <div class="form-group">
	                    <label for="exampleInputPassword1">contraseña</label>
	                    <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass ="form-control"></asp:TextBox>
	                  </div>
	                  <div class="post-auto">
	                   
                        <a class="link-lp" href="mi-cuenta.aspx">crear cuenta</a>

                        <a class="link-lp" href="recuperar.aspx">¿olvidaste tu contraseña?</a>
	                  </div>
	                   <asp:Button ID="btnEntrar" runat="server" Text="entrar"  CssClass ="btn btn-general-2"/>
	                </div>
	            </div>
	        </div>
	    </div>
	</div>
</asp:Content>