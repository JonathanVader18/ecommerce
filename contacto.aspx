<%@ Page Language="VB" AutoEventWireup="false" CodeFile="contacto.aspx.vb" Inherits="contacto" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-container">
		<h2 class="pag-cont-tit">Contacto</h2>
		<div class="col-xs-12 no-padding">
			<div id="map"></div>
		</div>
		<div class="col-xs-12 col-sm-10 autopos-2">
			<div class="gtk-form flex-contacto">
				<div class="col-xs-12 col-sm-6">
					<div class="row">
						<div class="col-xs-12">
							<div class="col-xs-12 col-sm-12">
							  <div class="form-group">
							    <label for="exampleInputName2">Nombre</label>
							    <asp:TextBox ID="txtNombre" runat="server" CssClass ="form-control"></asp:TextBox>
							  </div>
							</div>
							<div class="col-xs-12 col-sm-12">
							  <div class="form-group">
							    <label for="exampleInputName2">Correo</label>
							     <asp:TextBox ID="txtCorreo" runat="server" CssClass ="form-control"></asp:TextBox>
							  </div>
							</div>
							<div class="col-xs-12 col-sm-12">
								<div class="form-group">
									<label for="exampleInputName2" class="col-xs-12 no-padding">Mensaje</label>
									
                                     <asp:TextBox ID="txtMensaje" runat="server" CssClass ="form-control" TextMode="MultiLine"></asp:TextBox>
								</div>
							</div>
						</div>
					</div>
				</div>
				<div class="col-xs-12 col-sm-6 info-contacto">
                    <asp:Panel ID="pnlContacto" runat="server"></asp:Panel>
					
					<div class="col-xs-12">
						<div class="col-xs-12">
                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass ="btn act-4 f-left" />
						</div>
					</div>
				</div>
			</div>	
		</div>
	</div>
<%--<script type="text/javascript">
	  var image = 'images/marker.png';
	  var map;
      var markers = [];

      function initMap() {
         
          var mexico = { lat: 20.6170365, lng: -103.4503431 };
        map = new google.maps.Map(document.getElementById('map'), {
          zoom: 17,
          center: mexico,
          mapTypeId: 'roadmap',
        styles:[{"featureType":"water","elementType":"geometry","stylers":[{"color":"#e9e9e9"},{"lightness":17}]},{"featureType":"landscape","elementType":"geometry","stylers":[{"color":"#f5f5f5"},{"lightness":20}]},{"featureType":"road.highway","elementType":"geometry.fill","stylers":[{"color":"#ffffff"},{"lightness":17}]},{"featureType":"road.highway","elementType":"geometry.stroke","stylers":[{"color":"#ffffff"},{"lightness":29},{"weight":0.2}]},{"featureType":"road.arterial","elementType":"geometry","stylers":[{"color":"#ffffff"},{"lightness":18}]},{"featureType":"road.local","elementType":"geometry","stylers":[{"color":"#ffffff"},{"lightness":16}]},{"featureType":"poi","elementType":"geometry","stylers":[{"color":"#f5f5f5"},{"lightness":21}]},{"featureType":"poi.park","elementType":"geometry","stylers":[{"color":"#dedede"},{"lightness":21}]},{"elementType":"labels.text.stroke","stylers":[{"visibility":"on"},{"color":"#ffffff"},{"lightness":16}]},{"elementType":"labels.text.fill","stylers":[{"saturation":36},{"color":"#333333"},{"lightness":40}]},{"elementType":"labels.icon","stylers":[{"visibility":"off"}]},{"featureType":"transit","elementType":"geometry","stylers":[{"color":"#f2f2f2"},{"lightness":19}]},{"featureType":"administrative","elementType":"geometry.fill","stylers":[{"color":"#fefefe"},{"lightness":20}]},{"featureType":"administrative","elementType":"geometry.stroke","stylers":[{"color":"#fefefe"},{"lightness":17},{"weight":1.2}]}],

        });

        // Adds a marker at the center of the map.
        var image = 'https://developers.google.com/maps/documentation/javascript/examples/full/images/beachflag.png';
		  var beachMarker = new google.maps.Marker({
		    position: mexico,
		    map: map,
		    icon: image
		  });
      }
</script>--%>
<script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyDPP6U6OfP_jQ41Z3g6Fjk_5Tfrb2pxE5M&callback=initMap" ></script>
</asp:Content>