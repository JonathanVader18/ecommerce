<%@ Page Language="VB" AutoEventWireup="false" CodeFile="captura-pedidozeyco.aspx.vb" Inherits="captura_pedidozeyco" MasterPageFile ="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"  > 
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
		<div class="x_panel">
                    Capturar Pedido
                  </div>
	</div>
    <div class="wrappercon">

		<div class="main-container">
            <asp:Label ID="lblUsuario" runat="server" Text="" Visible ="false"></asp:Label>
 <div class="col-xs-12 col-sm-12 stl-1-p contenido no-padding ">
                <div class="col-xs-12 col-sm-12 no-padding">
                    <div class="blk-genericos cotizacion-generico">
                        <div class="row ">
                             <div class="col-xs-12 col-sm-3 ">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Moneda</label>
                                    <asp:DropDownList ID="ddlMoneda" runat="server" AutoPostBack ="true"></asp:DropDownList>
                                  
                                </div>
                            </div>
                            <asp:Panel ID="pnlfechas" runat="server">
                            <div class="col-xs-12 col-sm-3 ">
                                <div class="form-group">
                                      <label for="exampleInputEmail1">Fecha Cotización</label>
 <asp:TextBox ID="txtFechaCot" runat="server" CssClass="form-control" TextMode="Date"  ></asp:TextBox>
                                </div>
                                </div>
                            <div class="col-xs-12 col-sm-3 ">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Fecha Vencimiento</label>
                                    <asp:TextBox ID="txtFechaVence" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-3 ">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Tiempo de Entrega</label>
                                    <asp:TextBox ID="txtTiempoEntrega" runat="server" CssClass="form-control" MaxLength ="50" ></asp:TextBox>
                                </div>
                            </div>
</asp:Panel>
                            </div>
                        <div class="legend">Selecciona un artículo:</div>
                        <div class="row ">
                            <div class="col-xs-12 col-sm-4 ">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Artículo/descripción</label>
                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" EnableTheming="True" AutoPostBack="True" ></asp:TextBox>
                                    <asp:HiddenField ID="hfCustomerId" runat="server" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-2">
                                <div class="form-group">
                                    <label for="exampleInputEmail1">Cantidad</label>
                                    <asp:TextBox runat="server" ID="txtcantidad" CssClass="form-control sprin" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                              <div class="col-xs-12 col-sm-2">
                                <div class="form-group">
                                    <asp:Label ID="lbldesc1" runat="server" Text="%Desc" CssClass="exampleInputEmail1" Visible="false"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtDesc1" CssClass="form-control" Visible="false" ></asp:TextBox>
                                     <%--<asp:RegularExpressionValidator ID="Regex2" runat="server" ValidationExpression="((\d+)+(\.\d+))$"
ErrorMessage="Ingrese un descuento válido" ControlToValidate="txtDesc1" />--%>


                                </div>
                            </div>
                                 <div class="col-xs-12 col-sm-2">
                                <div class="form-group">
                                    <asp:Label ID="Label2" runat="server" Text="Precio" CssClass ="exampleInputEmail1" Visible ="True"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtPrecioArt" CssClass="form-control" Visible ="True" readonly="true" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-2">
                                <div class="form-group">
                                   <asp:Label ID="Label3" runat="server" Text="" CssClass ="exampleInputEmail1" Visible ="True"></asp:Label><br />
                                    <asp:Button ID="btnCargar" CssClass="btn btn-general-3" runat="server" Text="Agregar" Visible="true" OnClientClick="ShowProgress();" />
                                </div>
                            </div>
                          
                        </div>
                         <asp:Label ID="lblExistencia" runat="server" Text="" CssClass ="exampleInputEmail1" Visible ="True"></asp:Label>
                      

                       
                    </div>
                </div>

            </div>
			<div class="col-xs-8 no-padding carrito">
				<div class="header-tabla col-xs-12 no-padding">

					<div class="producto col-xs-2">
					</div>
                    <div class="producto col-xs-10">
                        <div class="col-tabla col-xs-4">
                            artículo
                        </div>
                        <div class="col-tabla col-xs-3">
                            precio
                        </div>
                        <div class="col-tabla col-xs-2">
                            cantidad
                        </div>
                     
                        <div class="col-tabla col-xs-3">
                            total
                        </div>
                    </div>
				</div>
				<%--<div class="body-tabla col-xs-12 no-padding">--%>
               

                   <asp:Panel ID="pnlPartidas" runat="server"></asp:Panel>

				
			</div>
      
			<div class="col-xs-4 cont-resumen">
				<div class="tit-resumen">RESUMEN DE COMPRA</div>
				<ul class="resumen">
                    <asp:Label ID="lblEnvio" runat="server" Text="" Visible="false"></asp:Label>
					<li><div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text=""></asp:Label></span></div></li>
					<li><div>Envio:<span> <asp:TextBox runat="server" ID="txtEnvio" style="text-align :right ;width :100%;" Visible="True"></asp:TextBox></span></div></li>
					<li><div>Descuento:<span><asp:Label ID="lblDescuento" runat="server" Text=""></asp:Label></span></div></li>
				</ul>
				<div class="total"><b>Total:</b><span><asp:Label ID="lblTotal" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                   <asp:Panel ID="pnlImpuestos" runat="server" Visible ="false" >
                    <div class="total"><b>Impuestos:</b><span><asp:Label ID="lblImpuestos" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                    <div class="total"><b>Total + Imp:</b><span><asp:Label ID="lbltotalImp" runat="server" Text="0" Font-Bold="True"></asp:Label></span></div>  
                </asp:Panel>
                <br />  <br />

                <asp:Panel ID="pnlComentarios" runat="server" Visible ="false" >
                     <div class="total">Comentarios:</div>
                     <div class="total"><asp:TextBox ID="txtComentarios" runat="server" style="width :100%;" Visible="True" MaxLength ="49"></asp:TextBox></div>
                </asp:Panel>
          <%--      <div class="total">Persona de Contacto:</div>
                <div class="total"><asp:DropDownList ID="ddlPersonaContacto" runat="server" Width ="100%"></asp:DropDownList></div>  
                <div class="total"><asp:TextBox ID="txtPersonaContacto" runat="server" style="width :100%;" Visible="True" MaxLength ="49"></asp:TextBox></div>--%>
                
                <asp:Panel runat="server" id="pnlProcesar" Visible ="true" >
               <%-- <a href="pagoindex.aspx" class="btn btn-general-3">procesar la compra</a>--%>
  <asp:Button ID="btnProcesar" CssClass ="btn btn-general-3" runat="server" Text="procesar la compra" Visible ="false"  OnClientClick ="ShowProgress();" />
           </asp:Panel>
                  <asp:Panel runat="server" id="pnlGuardarCarrito" Visible ="false" >
                 <asp:Button ID="btnGuardar" CssClass ="btn btn-general-5" runat="server" Text="guardar carrito" visible="false" />
           </asp:Panel>

              
                <asp:Panel ID="pnlPayPal" runat="server" Visible ="false" >
                    <div id="paypal-button-container"></div>
                </asp:Panel>
                
                <asp:Panel runat="server" ID="pnlSeparador" Visible="false">
                    

                   <%-- <div class="extra-o"><span>O</span></div>--%>
                    <div class="row alinear-botones">
                    

                       
                       <div class="col-xs-12 col-sm-12"> <asp:Button ID="btnCotizar" CssClass="btn btn-general-4" runat="server" Text="cotizar" Visible="false" OnClientClick ="ShowProgress();" /></div> 
                      

                   
                        <div class="col-xs-12 col-sm-6"><asp:Button ID="btnPedido"  CssClass="btn btn-general-4" runat="server" Text="levantar pedido" Visible="true" OnClientClick ="ShowProgress();"  /></div>
                    </div>
                    <div class="extra-o"></div>
                </asp:Panel>
                
               <asp:Button ID="btnImprimir" CssClass ="btn btn-general-3" runat="server" Text="ver PDF" visible="true" />
                <asp:Button ID="btnNuevo" CssClass ="btn btn-general-3" runat="server" Text="nuevo" visible="false" />
                <asp:Panel ID="pnlImprimir" runat="server" visible="false">
                    <%--<a class="btn btn-general-3 carrito-popup" href="playground.aspx">imprimir</a>--%>
                </asp:Panel>
                 
                   <asp:Button ID="btnGuardarPlantilla" CssClass ="btn btn-general-3" runat="server" Text="guardar como plantilla" visible="false" />
                <asp:Panel ID="pnlDireccionEntrega" runat="server" Visible="true">
                    <ul class="resumen">

                        <li><span>
                            <asp:Label ID="Label1" runat="server" Text="Seleccione la dirección de entrega"></asp:Label></span></li>
                        <li>
                            <div>
                                <asp:DropDownList ID="ddlDirecciones" runat="server" Width="100%"></asp:DropDownList></div>
                        </li>

                    </ul>
                </asp:Panel>
                  <asp:Panel ID="pnlPlantilla" runat="server" Visible ="false" >
                        <ul class="resumen">
                    
					
                            <li><p style ="text-align :center ;">Nombre de la plantilla</p></li>
					<li><div><asp:TextBox runat="server" id="txtNombrePlantilla" Width ="100%" CssClass ="form-control "></asp:TextBox></div></li>
                            <li><div> <asp:Button ID="btnGuardarPlantillaDet" CssClass ="btn btn-general-3" runat="server" Text="guardar" visible="true" /></div></li>
                      
				</ul>
                  </asp:Panel>
                <div class="mensaje"><span><asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label></span></div>			
			</div>
		</div>
	</div>
                      
                        </ContentTemplate>
                      <Triggers>
                          <asp:AsyncPostBackTrigger ControlID="btnCotizar" EventName="Click" />

                             <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click"  />
                           <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />

                           <asp:AsyncPostBackTrigger ControlID="btnGuardarPlantilla" EventName="Click" />
                          <asp:AsyncPostBackTrigger ControlID="btnGuardarPlantillaDet" EventName="Click" />
                        <%--   <asp:AsyncPostBackTrigger ControlID="btnImprimir" EventName="Click" />--%>


                             <asp:AsyncPostBackTrigger ControlID="btnGuardar" EventName="Click" />
                          
                       <asp:AsyncPostBackTrigger ControlID="btnPedido" EventName="Click" />

                             <asp:AsyncPostBackTrigger ControlID="txtSearch" EventName ="TextChanged" />

                <asp:PostBackTrigger ControlID="btnImprimir" />
                        
                         
                       
                           <%-- <asp:PostBackTrigger ControlID="btnProcesar" />--%>
                          <%-- <asp:PostBackTrigger ControlID="btnCotizar" />--%>
                         
                         <%--    <asp:PostBackTrigger ControlID="btnCargar" />--%>
                         
                      <%--    <asp:PostBackTrigger ControlID ="ddlMoneda" />
                              <asp:PostBackTrigger ControlID ="txtFechaVence" />
                          <asp:PostBackTrigger ControlID ="txtFechaCot" />--%>
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
    
     <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.10.0.min.js" type="text/javascript"></script>
 <script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>
<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.2/themes/blitzer/jquery-ui.css" rel="Stylesheet" type="text/css" />

<script type="text/javascript">
    $(function () {
        $("[id$=txtSearch]").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: '<%=ResolveUrl("~/captura-pedidozeyco.aspx/GetCustomers") %>',
                    data: "{ 'prefix': '" + request.term + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        response($.map(data.d, function (item) {
                            return {
                                label: item.split('@')[0],
                                val: item.split('@')[1]
                            }
                        }))
                    },
                    error: function (response) {
                        // alert(response.responseText);
                    },
                    failure: function (response) {
                        //  alert(response.responseText);
                    }
                });
            },
            select: function (e, i) {
                $("[id$=hfCustomerId]").val(i.item.val);
            },
            minLength: 1
        });
    });
</script>
</asp:Content>