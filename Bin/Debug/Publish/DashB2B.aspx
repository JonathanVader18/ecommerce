<%@ page language="VB" autoeventwireup="false" inherits="DashB2B, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="con-principal">
        
    <div class="section-blue-background">
	<div class="efect post-1"><img src="img/efect1.png" class="img-responsive"></div>
	<div class="efect post-2"><img src="img/efect2.png" class="img-responsive"></div>
	<div class="efect post-3"><img src="img/efect3.png" class="img-responsive"></div>
	<div class="efect post-4"><img src="img/efect4.png" class="img-responsive"></div>
	
	<div class="container">
	<div class="flex-cubic margin-b-0">
		<div class="col-xs-12 col-sm-3 ">
			<div class="panel-cubic">
				<div class="col-xs-12">
				<asp:Label ID="lblNombre" runat="server" Text="Nombre" Font-Bold ="true" ></asp:Label>
				</div>
                
				<div class="col-xs-12">
				<asp:Label ID="lblContacto" runat="server" Text="Contacto"  Font-Bold ="true"></asp:Label>
				</div>
				<div class="col-xs-12">
				<asp:Label ID="lblDireccion" runat="server" Text="Direccion"  Font-Bold ="true"></asp:Label>
				</div>
			</div>
		</div>
		<div class="col-xs-12 col-sm-9 no-padding">
			
            <asp:Panel ID="pnlDash" runat="server"></asp:Panel>

				<%--<div class="col-xs-12 col-sm-6">
					<div class="panel-cubic flex-int-cubic">
						<label>Importe total de ventas (LTD)</label>
						<div class="number-state">1666.45<small>m</small></div>
					</div>
				</div>
				<div class="col-xs-12 col-sm-6">
					<div class="panel-cubic flex-int-cubic">
						<label>Ganancia bruta (LTD)</label>
						<div class="number-state down">1666.45<small>m</small></div>
					</div>
				</div>
				<div class="col-xs-12 col-sm-12">
					<div class="panel-cubic flex-int-cubic">
						<label>Ingresos frente a ganacia bruta para los ultímos 6 años</label>
						
					</div>
				</div>
				<div class="col-xs-12 col-sm-6">
					<div class="panel-cubic flex-int-cubic margin-b-0">
						<label>Probabilidad de abandono</label>
						<div class="number-state">1666.45<small>m</small></div>
					</div>
				</div>
				<div class="col-xs-12 col-sm-6">
					<div class="panel-cubic flex-int-cubic margin-b-0">
						<label>Tasa de oportunidad obtenida (LTD)</label>
						<div class="number-state">1666.45<small>m</small></div>
					</div>
				</div>--%>
			
		</div>
	<%--	<div class="col-xs-12 col-sm-3 no-padding">
			<div class="col-xs-12">
				<div class="panel-cubic flex-int-cubic">
					<label>Número de llamadas de servicios pendientes</label>

					<div class="number-state">1666.45<small>m</small></div>	
				</div>
			</div>
			<div class="col-xs-12">
				<div class="panel-cubic flex-int-cubic">
					<label>Importe potencial de oportunidades abiertas</label>

					<div class="number-state">1666.45<small>m</small></div>
				</div>
			</div>
			<div class="col-xs-12">
				<div class="panel-cubic flex-int-cubic margin-b-0">
					<label>Importe ponderado de todas las oportunidades abiertas</label>

					<div class="number-state">1666.45<small>m</small></div>
				</div>
			</div>
		</div>--%>
		
	</div>
</div>
</div>
    
    </div>

  
    </asp:Content>