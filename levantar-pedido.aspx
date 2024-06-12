<%@ Page Language="VB" AutoEventWireup="false" CodeFile="levantar-pedido.aspx.vb" Inherits="levantar_pedido" MasterPageFile ="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="wrappercon">
	<div class="main-container">
        <div>
             <h1 class="tit-seccion sep-20">Elija una de sus listas</h1>
                                   <div class="col-xs-9 col-sm-4">
							    	<div class="form-group">
								       
								         <asp:DropDownList ID="ddlPlantillas" runat="server"  Width ="100%" AutoPostBack="True"  CssClass="form-control"></asp:DropDownList>
							    	</div>
							    </div>
                </div>

		<h1 class="tit-seccion sep-20">
			Levantar pedido
			<%--<div class="form-group post-form">
				<label>Filtrar por</label>
				<select class="cat-favoritos">
					<option>------------</option>
					<option>MIS FAVORITOS</option>
					<option>TOP 20</option>
					<option>Categoria 1</option>
					<option>Categoria 2</option>
					<option>Categoria 3</option>
				</select> 

                  

			</div>--%>
            <asp:Button ID="btnTodos" runat="server" Text="agregar todos" CssClass ="btn btn-general-2 txt-roboto" />
            <asp:Button ID="btnEliminar" runat="server" Text="eliminar plantilla" CssClass ="btn btn-general-2 txt-roboto" />
			
		</h1>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
</asp:ScriptManager>
		<div class="header-tabla col-xs-12 no-padding">
			<div class="producto col-xs-1 no-padding">
				
			</div>
			<div class="col-xs-11 no-padding">
				<div class="col-tabla col-xs-2">
					artículo
				</div>
				<div class="col-tabla col-xs-4">
					precio
				</div>
				<div class="col-tabla col-xs-2 text-center">
					cantidad
				</div>
				<div class="col-tabla col-xs-4">
					total
				</div>
               
           <%--     <input id="#algo" value="1" type="text" onchange="SetDefault('#algo','25','#Total'); return false;" onpaste="this.onchange('#algo');" oninput="this.onchange('#algo');"/>
                <div id="Total"></div>--%>
			</div>
			
		</div>
        <asp:Panel ID="pnlArticulos" runat="server"></asp:Panel>
	</div>
	<%--</div>--%>
        <script type="text/javascript">
          

            function SetDefault(Variable,Precio,ctrTotal) {
              
                var isbn = document.getElementById(Variable).value;
              //  alert(isbn);
              
                var Cantidad = parseInt(isbn);
                //alert( Cantidad * Precio);
                var Total = Cantidad * Precio;
                //  alert('El Total es: ' + Total);
               
                $(ctrTotal).html(format2(Total, "$") );
              
               
               
            }
            function fnClick(Variable, Articulo) {

             //   alert('entra');

                var isbn = document.getElementById(Variable).value;
                var Cantidad = parseInt(isbn);
                            
                PageMethods.RegisterUser(Cantidad, Articulo, onSucess, onError);

                function onSucess(result) {
                    //  alert(result);
                    PopUp('', 'Agregado al carrito', 'Aceptar', '', '', '', event);
                    
                }

                function onError(result) {
                    alert('Cannot process your request at the moment, please try later.');
                }

                       
            }
            function fnClickTodos(Variable, Articulo) {

                for (i = 1; i < 100; i++) {
                    var isbn = document.getElementById("#txt" + i).value;
                    var Cantidad = parseInt(isbn);
                }
              

                PageMethods.RegisterUser(Cantidad, Articulo, onSucess, onError);

                function onSucess(result) {
                    //  alert(result);
                }

                function onError(result) {
                    alert('Cannot process your request at the moment, please try later.');
                }
            }
            function format2(n, currency) {
                return currency + " " + n.toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
            }
            //$("#algo").bind("change paste keyup", function () {
            //    alert($(this).val());
            //});
       
    </script>
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
        </div>
   </asp:Content>
