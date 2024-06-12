<%@ page language="VB" autoeventwireup="false" inherits="Facturas, App_Web_fy50zy4w" masterpagefile="~/Main.master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     
    


   <div class="col-xs-12">
		<div class="main-container">
			<div class="x_panel">
                    Facturas
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
	                    <table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width:100%">
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

	<%--<script type="text/javascript" src="js/plugins/datepicker.js"></script>--%>
	<%--<script type="text/javascript">
        //$(document).ready(function() {
        //    $('table').DataTable( {
        //        scrollX: true,
        //        responsive: false,
        //        bPaginate: true,
        //        lengthMenu: true,
        //        lengthMenu: [
        //            [ 10, 25, 50, -1 ],
        //            [ '10', '25', '50', 'todos' ]
        //        ],
         
        //        "language": {
        //            "lengthMenu": "Mostrar _MENU_",
        //            "zeroRecords": "No encontramos ningún registro",
        //            "info": "Página _PAGE_ de _PAGES_  total _TOTAL_ registros",
        //            "infoEmpty": "No hay registros disponibles",
        //            "infoFiltered": "(filtrado de un  _MAX_ total registros)",
        //            "paginate": {
        //                "previous": "anterior",
        //                "next": "siguiente",
        //            }
        //        }
        //    } );
        //    $('.input-daterange').datepicker({
        //        todayBtn: true,
        //        keyboardNavigation: false,
        //        forceParse: false
        //    });
        //    $('.start-date').datepicker("setDate", new Date());
        //} );
//        $(document).ready(function() {
//	$('.datepicker').datepicker();
//	$("div .divDatePicker").each(function () {
//        $(this).datepicker().on('changeDate', function (ev) {
//            $(this).datepicker("hide");
//        });
//    });
//    $(document).on('change', '#fromDate, #toDate', function () {
//        $('table').DataTable().draw();
//    });
//	$.extend($.fn.dataTable.defaults, {
//		dom: 'Bfrtip',
//		buttons: [
//		  'pageLength',
//		  'copy',
//		  'excel',
//		  'print',
//		  'csv'                         
//		],	
//        scrollX: true,
//        scrollY:'400',
//        bPaginate: true,
//        lengthMenu: true,
//        lengthMenu: [
//            [ 10, 25, 50, -1 ],
//            [ '10', '25', '50', 'todos' ]
//        ],
//         "columns": [
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" },
//		    { "width": "200px" }
//		  ],
        
//		"language": {
//            "lengthMenu": "Mostrar _MENU_",
//            "zeroRecords": "No encontramos ningún registro",
//            "info": "Página _PAGE_ de _PAGES_  total _TOTAL_ registros",
//            "infoEmpty": "No hay registros disponibles",
//            "infoFiltered": "(filtrado de un  _MAX_ total registros)",
//            "lengthMenu": "Mostrar _MENU_",
//            "zeroRecords": "No encontramos ningún registro",
//            "info": "Página _PAGE_ de _PAGES_  total _TOTAL_ registros",
//            "infoEmpty": "No hay registros disponibles",
//            "infoFiltered": "(filtrado de un  _MAX_ total registros)",
//            "search": "<i class='fa fa-search' aria-hidden='true'></i>:",
//            "paginate": {
//		        "first":      "primero",
//		        "last":       "ultimo",
//		        "next":       "siguiente",
//		        "previous":   "anterior"
//		    },
//        }
//	});

//	$.extend($.fn.dataTable.ext.search.push(
//		function (settings, data, dataIndex) {
//		    if (($('#fromDate').length > 0 && $('#fromDate').val() !== '') || ($('#toDate').length > 0 && $('#toDate').val() !== '')) {

//		        var min = new Date($('#fromDate').val()).getTime() ;
//		        var max = new Date($('#toDate').val()).getTime() ;
//		        var age = parseFloat( new Date(data[4]).getTime() ) || 0;
//		        console.log(data[4]);
//		        console.log(min+"   ----   "+max);
//		        if ( ( isNaN( min ) && isNaN( max ) ) ||
//		             ( isNaN( min ) && age <= max ) ||
//		             ( min <= age   && isNaN( max ) ) ||
//		             ( min <= age   && age <= max ) )
//		        {
//		            return true;
//		        }
//		        return false;
//		    }
//		    return true;
//		})); 

//    var table = $('table').DataTable();
//} );
    </script>--%>

   
    <div class="wrappercon">
        <div class="main-container">
            
          
 <div class="col-xs-4">
<asp:Button ID="btnVer" CssClass ="btn btn-general-3" runat="server" Text="Ver" visible="false" />
<asp:Button ID="btnConvertir" CssClass ="btn btn-general-3" runat="server" Text="Convertir a Pedido" visible="false" />
     <asp:Button ID="btnCerrar" CssClass ="btn btn-general-3" runat="server" Text="Finalizar" visible="false" OnClientClick ="return confirm('¿Seguro de cerrar el documento?');" />
                    </div>

           
    </div>

         </div>
    
</asp:Content>