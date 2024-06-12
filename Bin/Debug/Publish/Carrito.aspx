<%@ page language="VB" autoeventwireup="false" inherits="Carrito, App_Web_fy50zy4w" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"  > 
     <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

  <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <span style="border-width: 0px; position: fixed;  background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                <img src="LOADER_mm.gif" style="margin:0 auto;">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>


    <div class="main-container">
		<h1 class="tit-seccion"><asp:Label ID="lblCarrito" runat="server" Text="BOLSA DE COMPRAS"></asp:Label></h1>
        
	</div>
    <div class="wrappercon" onclick ="refreshAndClose();">
		<div class="main-container">
			<div class="col-xs-8 no-padding carrito">
				<div class="header-tabla col-xs-12 no-padding">

					<div class="producto col-xs-2">
					</div>
                    <div class="producto col-xs-10">
                        <div class="col-tabla col-xs-3">
                            Artículos
                        </div>
                        <div class="col-tabla col-xs-2">
                            Precio
                        </div>
                        <div class="col-tabla col-xs-2">
                            Cantidad
                        </div>
                     
                        <div class="col-tabla col-xs-3">
                            Total
                        </div>
                        <div class="col-tabla col-xs-2">
                            <asp:Label ID="lblColAdicional" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
				</div>
			               

                   <asp:Panel ID="pnlPartidas" runat="server"></asp:Panel>
                
				
			</div>
      
			<div class="col-xs-4 cont-resumen">
				<div class="tit-resumen">RESUMEN DE COMPRA</div>
				<ul class="resumen">
                    
					<li><div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text=""></asp:Label></span></div></li>
					<li><div><asp:panel ID="lblEnviotxt" runat="server">Envio:</asp:panel><span><asp:Label ID="lblEnvio" runat="server" Text=""></asp:Label></span></div></li>
					<li><div><asp:panel ID="lblDesctxt" runat="server" visible="false" >Descuento:</asp:panel><span><asp:Label ID="lblDescuento" runat="server" Text=""></asp:Label></span></div></li>
				</ul>
				<div class="total"><b>Total:</b><span><asp:Label ID="lblTotal" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                <asp:Panel ID="pnlImpuestos" runat="server" Visible ="false" >
                    <div class="total"><b>Impuestos:</b><span><asp:Label ID="lblImpuestos" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                    <div class="total"><b>Total + Imp:</b><span><asp:Label ID="lbltotalImp" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                </asp:Panel>
                <asp:Panel runat="server" id="pnlProcesar" Visible ="true" >
               
  <asp:Button ID="btnProcesar" CssClass ="btn btn-general-3" runat="server" Text="procesar la compra" Visible ="false"  OnClientClick ="ShowProgress();" />
           </asp:Panel>
                  <asp:Panel runat="server" id="pnlGuardarCarrito" Visible ="false" >
                 <asp:Button ID="btnGuardar" CssClass ="btn btn-general-5" runat="server" Text="guardar carrito" visible="false" />
           </asp:Panel>

              
                <asp:Panel ID="pnlPayPal" runat="server" Visible ="false" >
                    <div id="paypal-button-container"></div>
                </asp:Panel>
                
                <asp:Panel runat="server" ID="pnlSeparador" Visible="false">
                    

                   
                    <div class="row">
                    

                       <br />
                       <div class="col-xs-12"> <asp:Button ID="btnCotizar" CssClass="btn btn-general-4" runat="server" Text="Cotizar" Visible="false" OnClientClick ="ShowProgress();" /></div> 

                   
                        <div class="col-xs-12 col-sm-12"><asp:Button ID="btnPedido" OnClick ="btnPedido_Click"  CssClass="btn btn-general-4" runat="server" Text="Levantar pedido" Visible="false" OnClientClick ="ShowProgress();"  /></div>
                        <div class="col-xs-12 col-sm-12"><asp:Button ID="btnPagar"  CssClass="btn btn-general-3" runat="server" Text="Pagar" Visible="false" OnClientClick ="ShowProgress();" style="width:100%;" /></div>
                    </div>
                    <div class="extra-o"></div>
                </asp:Panel>
                
               <asp:Button ID="btnImprimir" CssClass ="btn btn-general-3" runat="server" Text="Imprimir" visible="false" />
                <asp:Panel ID="pnlImprimir" runat="server" visible="false">
                    
                </asp:Panel>
                 
                   <asp:Button ID="btnGuardarPlantilla" CssClass ="btn btn-general-3" runat="server" Text="Guardar como plantilla" visible="false" />


                  <asp:Panel ID="pnlMoneta" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="Label2" runat="server" Text="Ingrese el código Moneta"></asp:Label></li>
                            	<li><div><asp:TextBox ID="txtCodMoneta" runat="server"  Width ="100%"></asp:TextBox></div></li>
                            </ul>
                     </asp:Panel>

                 <asp:Panel ID="pnlTransporting" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="lblAdicional" runat="server" Text="a"></asp:Label></li>
                            	<li><div><asp:DropDownList ID="ddlTransporting" runat="server"  Width ="100%"></asp:DropDownList></div></li>
                            </ul>
                     </asp:Panel>

                    <asp:Panel ID="pnlFechaEntrega" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="lblFechaEntrega" runat="server" Text="a"></asp:Label></li>
                            	<li><div> <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" ></asp:TextBox></div></li>
                            </ul>
                     </asp:Panel>

                 <asp:Panel ID="pnlAlmacen" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="lblAdicionalAlm" runat="server" Text="a"></asp:Label></li>
                            	<li><div><asp:DropDownList ID="ddlAlmacen" runat="server"  Width ="100%"></asp:DropDownList></div></li>
                            </ul>
                     </asp:Panel>

                 <asp:Panel ID="pnlOtros" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="lblAdicionalOtros" runat="server" Text="a"></asp:Label></li>
                            	<li><div> <asp:TextBox ID="txtOtros" runat="server" CssClass="form-control" MaxLength ="80"></asp:TextBox></div></li>
                            </ul>
                     </asp:Panel>
                  <asp:Panel ID="pnlTipoEnvio" runat="server" Visible ="false" >
         <ul class="resumen">
             <li><asp:Label ID="Label10" runat="server" Text="Tipo Envio"></asp:Label></li>
                	<li><div><asp:DropDownList ID="ddlTipoEnvio" runat="server"  Width ="100%"></asp:DropDownList></div></li>
             </ul>
      </asp:Panel>


                 <asp:Panel ID="pnlEstadoPedido" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="lblEstadoPedido" runat="server" Text="a"></asp:Label></li>
                            	<li><div><asp:DropDownList ID="ddlEstadoPedido" runat="server"  Width ="100%"></asp:DropDownList></div></li>
                            </ul>
                     </asp:Panel>
                <asp:Panel ID="pnlPaqueteria" runat="server" Visible ="false" >
       <ul class="resumen">
           <li><asp:Label ID="Label9" runat="server" Text="Paquetería"></asp:Label></li>
              	<li><div><asp:DropDownList ID="ddlPaqueteria" runat="server"  Width ="100%"></asp:DropDownList></div></li>
           </ul>
    </asp:Panel>
                <asp:Panel ID="pnlSuje" runat="server" Visible="false">
                    <ul class="resumen">
                        <li>
                            <asp:Label ID="Label11" runat="server" Text="Orden de compra"></asp:Label></li>
                        <li>
                            <div>
                                <asp:TextBox ID="txtOC_" runat="server" CssClass="form-control" MaxLength="80"></asp:TextBox>
                            </div>
                        </li>
                       <li>
    <asp:Label ID="Label12" runat="server" Text="Uso de CFDI"></asp:Label></li>

   	<li><div><asp:DropDownList ID="ddlUsoCFDI" runat="server"  Width ="100%"></asp:DropDownList></div></li>

                        
                        
                    </ul>
                </asp:Panel>

                    <asp:Panel ID="pnlProyecto" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><asp:Label ID="lblAdicional2" runat="server" Text="a"></asp:Label></li>
                            	<li><div><asp:DropDownList ID="ddlProyecto" runat="server"  Width ="100%"></asp:DropDownList></div>

                            	</li>
                              <li>

                             
                             <div class="tit-bloque">
                                 <h4>NUEVO PROYECTO</h4>
                                 <a class="add" role="button" data-toggle="collapse" href="#collapseProyecto" aria-expanded="false" aria-controls="collapseProyecto">+ Agregar nuevo proyecto</a></div>
                             <div class="collapse" id="collapseProyecto">
                                 <asp:TextBox runat="server" ID="txtProyecto" Width="100%" CssClass="form-control "></asp:TextBox>



                             </div>
                         </li>
                            </ul>
                     </asp:Panel>
                <asp:Panel ID="pnlTemplate" runat="server" Visible ="false" >
                     <ul class="resumen">
                         <li><asp:Label ID="Label8" runat="server" Text="cargue el template"></asp:Label></li>
                            	<li><div> <asp:FileUpload ID="FileUpload1" runat="server" Width="100%" /></div>

                            	</li>
                         <li> <asp:Button ID="btnSubir" runat="server" Text="subir" CssClass="btn btn-general-3" /></li>
                         <li><a href="/template/template.xlsx" target="_blank">Descargar plantilla para pedidos</a></li>
                         </ul>

                </asp:Panel>

                 <asp:Panel ID="pnlDireccionEntrega" runat="server" Visible ="false" >
                     <ul class="resumen">

                         <li>
                             <asp:Label ID="Label1" runat="server" Text="Seleccione la dirección de entrega"></asp:Label></li>
                         <li>
                             <div>
                                 <asp:DropDownList ID="ddlDirecciones" runat="server" Width="100%"></asp:DropDownList></div>
                         </li>

                         <li>
                             <asp:Panel ID="pnlOtraDireccion" runat="server" >

                            
                             <%--  <p style="text-align: center;">Otra dirección:</p>--%>
                             <div class="tit-bloque">
                                 <h4>OTRA DIRECCION</h4>
                                 <a class="add" role="button" data-toggle="collapse" href="#collapseExample" aria-expanded="false" aria-controls="collapseExample">+ Agregar nueva dirección</a></div>
                             <div class="collapse" id="collapseExample">
                                
                                 <div><a>País:</a><asp:DropDownList ID="ddlPais" runat="server"  Width ="100%"></asp:DropDownList></div>
                                 <div><a>Estado:</a><asp:DropDownList ID="ddlEstados" runat="server"  Width ="100%"></asp:DropDownList></div>
                                  <div><a>Municipio:</a><asp:TextBox runat="server" ID="txtMunicipio" Width="100%" CssClass="form-control "></asp:TextBox></div>
                                   <div><a>Domicilio:</a><asp:TextBox runat="server" ID="txtDireccion" Width="100%" CssClass="form-control "></asp:TextBox></div>
                                 <div><a>CP:</a><asp:TextBox runat="server" ID="txtCP" Width="100%" CssClass="form-control "></asp:TextBox></div>
                                   <div><a>Colonia:</a><asp:TextBox runat="server" ID="txtColonia" Width="100%" CssClass="form-control "></asp:TextBox></div>
                                 


                             </div>
 </asp:Panel>

                         </li>
                         <li></li>

                     </ul>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlHawk" Visible ="false">
                        <ul class="resumen">
                             <li><asp:Label ID="Label3" runat="server" Text="Datos de envío"></asp:Label></li>
                             <li><div><asp:DropDownList ID="ddlTipodeEnvio" runat="server" Width="100%"></asp:DropDownList></div></li>
                             <li><asp:Label ID="Label4" runat="server" Text="Dirección de envío"></asp:Label></li>
                             <li><asp:TextBox runat="server" ID="txtDestinatario" Width="100%" CssClass="form-control "></asp:TextBox></li>
                             <li><asp:Label ID="Label5" runat="server" Text="Fecha de envío"></asp:Label></li>
                             <li><asp:TextBox runat="server" ID="txtRemitente" Width="100%" CssClass="form-control " TextMode="Date" ></asp:TextBox></li>
                             <li><asp:Label ID="Label6" runat="server" Text="Con atención a "></asp:Label></li>
                             <li><asp:TextBox runat="server" ID="txtAtencion" Width="100%" CssClass="form-control "></asp:TextBox></li>
                              <li><asp:Label ID="Label7" runat="server" Text="Número de referencia "></asp:Label></li>
                             <li><asp:TextBox runat="server" ID="txtRef" Width="100%" CssClass="form-control "></asp:TextBox></li>
                        </ul>
                </asp:Panel>
                  <asp:Panel ID="pnlPlantilla" runat="server" Visible ="false" >
                        <ul class="resumen res-dir">
                    
					
                            <li><p style ="text-align :center ;">Nombre de la plantilla</p></li>
					<li><div><asp:TextBox runat="server" id="txtNombrePlantilla" Width ="100%" CssClass ="form-control "></asp:TextBox></div></li>
                            <li><div> <asp:Button ID="btnGuardarPlantillaDet" CssClass ="btn btn-general-3" runat="server" Text="Guardar" visible="true" /></div></li>
                      
				</ul>
                  </asp:Panel>
                <div class="mensaje"><span><asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label></span></div>			
			</div>
		</div>
	</div>
                      
                        </ContentTemplate>
                      <Triggers>
                        <%--  <asp:AsyncPostBackTrigger ControlID="btnPedido" EventName="Click" />--%>
              <%--  <asp:PostBackTrigger ControlID="btnPedido" />--%>
                           <asp:PostBackTrigger ControlID="btnGuardarPlantilla" />
                          <asp:PostBackTrigger ControlID="btnGuardarPlantillaDet" />
                          <asp:PostBackTrigger ControlID="btnGuardar" />
                           <asp:PostBackTrigger ControlID="btnImprimir" />
                       <asp:PostBackTrigger ControlID="btnPedido" />
                            <asp:PostBackTrigger ControlID="btnProcesar" />
                           <asp:PostBackTrigger ControlID="btnCotizar" />
                            <asp:PostBackTrigger ControlID="ddlPais" />
                          <asp:PostBackTrigger ControlID="ddlDirecciones" />
                          <asp:PostBackTrigger ControlID="btnPagar" />
                          <asp:PostBackTrigger ControlID="btnSubir" />
                          
               <%-- <asp:PostBackTrigger ControlID="btnFullPost" />--%>
               <%-- <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName="Click" ></asp:AsyncPostBackTrigger>--%>

                

            </Triggers>
                </asp:UpdatePanel>


        <div id="myModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                <h4 class="modal-title">Información</h4>
            </div>
            <div class="modal-body">
                <p>One fine body&hellip;</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
                      

    <script type="text/javascript">
        function refreshAndClose() {
           
           
            var Actualiza = '<%= Session("ActualizaCarrito") %>';
           // alert(Actualiza);
            if (Actualiza == "SI") {
              //  alert(Actualiza);
                //location.reload(true);
            }
        
       
    }
</script>

     <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(updateProgress.ClientID) %>').style.display = "inline";
    }
    

    </script>
    

</asp:Content>