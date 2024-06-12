<%@ page language="VB" autoeventwireup="false" inherits="Compras, App_Web_atyal0ov" masterpagefile="~/Main.master" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
   

     <div class="col-xs-12">
		<div class="main-container">
			<div class="x_panel">
                    Compras Pendientes
                  </div>
                  <div class="x_content">
                  		<div>
	         <div class="input-daterange input-group " id="datepicker">
								<div class="col-xs-6">
						  			<div class="form-group">
						  				<label>del
							    			<input type="text" class=" start-date form-control" name="start" />
							    		</label>
							    	</div>
								</div>
								<div class="col-xs-6">
						  			<div class="form-group">
						  				<label>al
							    			<input type="text" class=" form-control" name="end" />
							    		</label>
							    	</div>
							    </div>
							</div>
						</div>
	                    <table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width:100%">
	                         <thead>
	                        	<tr>
		                        	<th style="width:30px;">ver</th>
		                        	<th style="width:100px;">Fecha</th>
		                        	<th style="width:50px;">Folio</th>
                                     <th style="width:50px;">Cve Cliente</th>
                                    <th style="width:50px;">Cliente</th>
		                        	<th style="width:50px;">Orden de compra</th>
		                        	<th style="width:100px;" class="tdh-c-verde">Pesos</th>
		                        	<th style="width:100px;" class="tdh-c-azul">Dólares</th>
                                    	<th style="width:50px;">Abrir</th>
		                        	
		                        </tr>
	                        </thead>
	                        <tbody>
                                <asp:Panel ID="pnlRegistros" runat="server"></asp:Panel>
	                  

	                        </tbody>
	                    </table>
	               
                  </div>
                </div>
		</div>

	<script type="text/javascript" src="js/plugins/datepicker.js"></script>
	<script type="text/javascript">
        $(document).ready(function() {
            $('table').DataTable( {
                scrollX: true,
                responsive: false,
                bPaginate: true,
                lengthMenu: true,
                lengthMenu: [
                    [ 10, 25, 50, -1 ],
                    [ '10', '25', '50', 'todos' ]
                ],
         
                "language": {
		            "lengthMenu": "Mostrar _MENU_",
		            "zeroRecords": "No encontramos ningún registro",
		            "info": "Página _PAGE_ de _PAGES_  total _TOTAL_ records",
		            "infoEmpty": "No hay registros disponibles",
		            "infoFiltered": "(filtrado de un  _MAX_ total records)"
		        }
            } );
            $('.input-daterange').datepicker({
                todayBtn: true,
                keyboardNavigation: false,
                forceParse: false
            });
            $('.start-date').datepicker("setDate", new Date());
        } );
    </script>


    <div class="wrappercon">
        <div class="main-container">
            
         
 

            <div class="col-xs-12">
                <telerik:RadGrid ID="rgvPartidas" runat="server" Skin="Silk" Visible="False" ShowFooter="True">
                    <GroupingSettings CollapseAllTooltip="Collapse all groups"></GroupingSettings>

                    <MasterTableView FooterStyle-HorizontalAlign="Right" AutoGenerateColumns="false">
                        <Columns>
                            <telerik:GridBoundColumn DataField="cvItemCode" UniqueName="cvItemCode" HeaderText="CodProd"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="cvItemName" UniqueName="cvItemName" HeaderText="Producto"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="cfCantidad" UniqueName="cfCantidad" HeaderText="Cant" DataFormatString="{0:#,###,###.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="cfPRecio" UniqueName="cfPRecio" HeaderText="Precio" DataFormatString="{0:#,###,###.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Total" UniqueName="Total" HeaderText="Total" DataFormatString="{0:#,###,###.00}" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" Aggregate="Sum">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle HorizontalAlign="Right" />
                            </telerik:GridBoundColumn>
                        </Columns>

                        <FooterStyle HorizontalAlign="Right"></FooterStyle>
                    </MasterTableView>
                </telerik:RadGrid>
            </div>

            <div class="col-xs-4">
                <asp:Button ID="btnVer" CssClass="btn btn-general-3" runat="server" Text="Ver" Visible="false" />
                <asp:Button ID="btnCotizacion" CssClass="btn btn-general-3" runat="server" Text="Convertir a Cotización" Visible="false" />
                <asp:Button ID="btnPedido" CssClass="btn btn-general-3" runat="server" Text="Convertir a Pedido" Visible="false" />
                <asp:Button ID="btnCerrar" CssClass="btn btn-general-3" runat="server" Text="Finalizar" Visible="false" OnClientClick="return confirm('¿Seguro de cerrar el documento?');" />
            </div>

        </div>
    </div>

</asp:Content>