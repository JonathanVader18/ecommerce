<%@ Page Language="VB" AutoEventWireup="false" CodeFile="saldos.aspx.vb" Inherits="saldos" MasterPageFile ="~/Main.master"  %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="col-xs-12 ">
			<div class="main-container">
                <div class="col-xs-12">
                    <a href="#" class="tit-sub extra-marg">saldos</a>
                </div>
                <div class="col-xs-12 no-padding f-general sep-1">
                    <asp:Panel ID="pnlEncabezado" runat="server"></asp:Panel>
               		<%--<div class="col-mn-12 col-xs-6 col-sm-3 col-md-3 s-sta-h danger no-padding">
                        <div class="title"><asp:Label ID="lblTituloVencidos" runat="server" Text="Label"></asp:Label> <i class="fa fa-chevron-down"></i></div>
                    	<div class="f-sta-input b-r-white">
               				<div class="col-xs-12 s-sta-input">
                                   <asp:Label ID="lblVencidoMN" runat="server" Text="Label" class="form-control text-center"></asp:Label>><span>M.N.</span>
                                <input type="text" name="txtSaldoM1" id="txtSaldoM1" class="form-control text-center"  value="$12,000.00"><span>MXN</span>
							</div>
							<div class="col-xs-12 s-sta-input">
                                 <asp:Label ID="lblVencidoME" runat="server" Text="Label" class="form-control text-center"></asp:Label>><span>M.E.</span>
                                <input type="text" name="txtSaldoU1" id="txtSaldoU1" class="form-control text-center"  value="$125.00"><span>USD</span>
							</div>
                        </div>
               		</div>
                   	<div class="col-mn-12 col-xs-6 col-sm-3 col-md-3 s-sta-h no-padding">
                        <div class="title">Por vencer <br> de 0 a 15 días (4)</div>
                        <div class="f-sta-input b-r-white">
               				<div class="col-xs-12 s-sta-input">
                                <input type="text" name="txtSaldoM2" id="txtSaldoM2" class="form-control text-center" value="$1000.00"> <span>MXN</span>
							</div>
							<div class="col-xs-12 s-sta-input">
                                <input type="text" name="txtSaldoU2" id="txtSaldoU2" class="form-control text-center" value="$148.00"> <span>USD</span>
							</div>
                        </div>
                   	</div>
               		<div class="col-mn-12 col-xs-6 col-sm-3 col-md-3 s-sta-h no-padding">
                        <div class="title">Por vencer<br> de 16 a 30 días (13)</div>
                        <div class="f-sta-input b-r-white">
               				<div class="col-xs-12 s-sta-input">
                                <input type="text" name="txtSaldoM3" id="txtSaldoM3" class="form-control text-center" value="$9000.00"> <span>MXN</span>
							</div>
							<div class="col-xs-12 s-sta-input">
			                    <input type="text" name="txtSaldoU3" id="txtSaldoU3" class="form-control text-center"  value="$500.00"> <span>USD</span>
							</div>
                        </div>
               		</div>
                   	<div class="col-mn-12 col-xs-6 col-sm-3 col-md-3 s-sta-h no-padding">
                            <div class="title">Saldos por vencer<br> mas de 30 días (8)</div>
                            <div class="f-sta-input">
                   				<div class="col-xs-12 s-sta-input">
                                    <input type="text" name="txtSaldoM4" id="txtSaldoM4" class="form-control text-center" value="$10,000.00">  <span>MXN</span>
								</div>
								<div class="col-xs-12 s-sta-input">
                                    <input type="text" name="txtSaldoU4" id="txtSaldoU4" class="form-control text-center" value="$2000.00"> <span>USD</span>
								</div>
                            </div>
                   	</div>--%>
                </div>	
            </div>
		</div>
		<div class="main-container">
		<div class="sep-bloques">
			<div class="col-xs-12">
                    <a href="#" class="tit-sub">documentos pendientes</a>
                  </div>
                   <div class="x_content">
                  		<div>
	         <div class="input-daterange input-group " id="datepicker">
								<div class="col-xs-6">
						  			<div class="form-group">
						  				<label>del
							    			<input type="text" class=" start-date form-control datepicker" name="start" id="fromDate" />
							    		</label>
							    	</div>
								</div>
								<div class="col-xs-6">
						  			<div class="form-group">
						  				<label>al
							    			<input type="text" class=" form-control datepicker" name="end" id="toDate" />
							    		</label>
							    	</div>
							    </div>
							</div>
						</div>
                        <asp:Button ID="btnExportar" CssClass ="btn btn-general-5" runat="server" Text="Descargar" visible="true" />
	                    <table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width:100%">
	                       <%-- <thead>
                            <tr>
                                <th colspan="5" rowspan=""></th>
                                <th colspan="3" class="tdh-c-verde txt-center">Pesos</th>
                                <th colspan="3" class="tdh-c-azul txt-center">Dólares</th>
                            </tr>
		                        <tr>
		                        	<th style="width:30px;">ver</th>
		                        	<th style="width:80px;">Fecha</th>
                              <th style="width:80px;">Vence</th>
                              <th style="width:80px;">Documento</th>
                              <th style="width:30px;">Folio</th>
		                        	<th style="width:80px;" class="tdh-c-verde">Cargo</th>
                              <th style="width:80px;" class="tdh-c-verde">Abono</th>
                              <th style="width:80px;" class="tdh-c-verde">Saldo</th>
                              <th style="width:80px;" class="tdh-c-azul">Cargo</th>
                              <th style="width:80px;" class="tdh-c-azul">Abono</th>
                              <th style="width:80px;" class="tdh-c-azul">Saldo</th>
		                        </tr>
	                        </thead>
	                        <tbody>
	                        	<?php
	                        	for ($i=0; $i < 250 ; $i++) { 
		                            echo '<tr>
				                            <td class="mas"><i class="fa fa-chevron-right" aria-hidden="true"></i></td>
				                            <td>'.date("m/d/y").'</td>
                                    <td>'.date("m/d/y").'</td>
                                    <td>00001</td>
				                            <td>'.rand(1,50).'</td>
                                    <td class="tdh-c-verde">$107,460.06</td>
                                    <td class="tdh-c-verde">$0</td>
                                    <td class="tdh-c-verde">$0</td>
                                    <td class="tdh-c-azul">$0</td>
                                    <td class="tdh-c-azul">$107,460.06</td>
                                    <td class="tdh-c-azul">$0</td>
                                  </tr>';
	                        	}
	                          	?>
	                        </tbody>--%>

                             <thead>
		                     
	                        	
                                     <asp:Panel ID="pnlColumnas" runat="server"></asp:Panel>
		                        	
		                        	
		                        
	                        </thead>
	                        <tbody>
                                <asp:Panel ID="pnlRegistros" runat="server"></asp:Panel>
	                  

	                        </tbody>
	                    </table>
	               
                  </div>
                </div>
		</div>
	</div>
 <%-- <script type="text/javascript" src="<?=$base?>js/plugins/datepicker.js"></script>
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
                    "info": "Página _PAGE_ de _PAGES_  total _TOTAL_ registros",
                    "infoEmpty": "No hay registros disponibles",
                    "infoFiltered": "(filtrado de un  _MAX_ total registros)",
                    "paginate": {
                        "previous": "anterior",
                        "next": "siguiente",
                    }
                }
            } );
            $('.input-daterange').datepicker({
                todayBtn: true,
                keyboardNavigation: false,
                forceParse: false
            });
            $('.start-date').datepicker("setDate", new Date());
        } );
    </script>--%>
</asp:Content>
