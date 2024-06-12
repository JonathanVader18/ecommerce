<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Catalogo.aspx.vb" Inherits="Catalogo"  MasterPageFile ="~/Main.master"  %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
	
	.controles{
		float:right;
		display:flex;
		position:relative;
		align-items:center;
	}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:scriptmanager runat="server" EnablePageMethods="true"></asp:scriptmanager>

<asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <span style="border-width: 0px; position: fixed;  background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                <img src="LOADER_mm.gif" style="margin:0 auto;">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server" >
                    <ContentTemplate >
    <div class="wrappercon naranja">
    <div class="main-container">

  
        <asp:Panel ID="pnlRuta" runat="server"></asp:Panel>

            <div class="op-collapse-resp">
 <button class="btn" type="button" data-toggle="collapse" data-target="#collfiltro" aria-expanded="false" aria-controls="collfiltro">
 
 </button>
</div>
              <%--AIO Y PMK <div class="col-xs-12 col-sm-4 menu-lateral" id="collfiltro">--%>
    <div class="col-xs-12 col-sm-2 menu-lateral" id="collfiltro">
<asp:Panel ID="pnlFiltros" runat="server" CssClass="subcategorias" Visible ="true" >
                <h4 class="titulo">Filtrar por:</h4>
  <div class="panel-group" id="accordion-2" role="tablist" aria-multiselectable="true">
    <div class="panel panel-gtk">   

        <asp:Panel ID="pnlFiltro1" runat="server" Visible ="false" >
      <!-- encabezado 1 nivel -->
      <div class="panel-heading">
          <h4 class="panel-title">
              <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND1" class="collapsed line-punteada " aria-expanded ="true">
               <asp:Label ID="lbl_1" runat="server" Text=""></asp:Label>
              </a>
          </h4>
      </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND1" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont">
            
                              <asp:CheckBoxList ID="chkLista_1" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            
          </div>
         </div>
        </div>
      </div>
     <!-- encabezado 1 nivel -->
     </asp:Panel>
        
          <asp:Panel ID="pnlFiltro2" runat="server" Visible ="false" >
        <div class="panel-heading">
          <h4 class="panel-title">
              <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND2" class="collapsed line-punteada" aria-expanded ="true">
                <asp:Label ID="lbl_2" runat="server" Text=""></asp:Label>
              </a>
          </h4>
      </div>

      <!-- menu 2 nivel -->
      <div id="collapse-CTIND2" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont">
            
                              <asp:CheckBoxList ID="chkLista_2" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
          
        </div>
      </div>

</asp:Panel>

          <asp:Panel ID="pnlFiltro3" runat="server" Visible ="false" >
      <!-- encabezado 1 nivel -->
      <div class="panel-heading">
          <h4 class="panel-title">
              <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND3" class="collapsed line-punteada"  aria-expanded ="true">
                 <asp:Label ID="lbl_3" runat="server" Text=""></asp:Label>
              </a>
          </h4>
      </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND3" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont-radio">
           
                              <asp:CheckBoxList ID="chkLista_3" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
         
        </div>
      </div>     

</asp:Panel>

       <asp:Panel ID="pnlFiltro4" runat="server" Visible ="false" >
      <!-- encabezado 1 nivel -->
      <div class="panel-heading">
          <h4 class="panel-title">
              <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND4" class="collapsed line-punteada">
               <asp:Label ID="lbl_4" runat="server" Text=""></asp:Label>
              </a>
          </h4>
      </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND4" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont-radio">
            
                              <asp:CheckBoxList ID="chkLista_4" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
         
        </div>
      </div>   
     </asp:Panel>   

       <asp:Panel ID="pnlFiltro5" runat="server" Visible ="false" >
         <!-- encabezado 1 nivel -->
      <div class="panel-heading">
          <h4 class="panel-title">
              <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND5" class="collapsed line-punteada">
               <asp:Label ID="lbl_5" runat="server" Text=""></asp:Label>
              </a>
          </h4>
      </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND5" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont-radio">
               <asp:CheckBoxList ID="chkLista_5" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
         
        </div>
      </div>   
  </asp:Panel> 

         <asp:Panel ID="pnlFiltro6" runat="server" Visible ="false" >
        <div class="panel-heading">
            <h4 class="panel-title">
                <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND6" class="collapsed line-punteada"> <asp:Label ID="lbl_6" runat="server" Text=""></asp:Label>
                </a>
            </h4>
        </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND6" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont-radio">
           <asp:CheckBoxList ID="chkLista_6" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
         
        </div>
      </div>   
  </asp:Panel> 

          <asp:Panel ID="pnlFiltro7" runat="server" Visible ="false" >
          <div class="panel-heading">
            <h4 class="panel-title">
                <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND7" class="collapsed line-punteada"> <asp:Label ID="lbl_7" runat="server" Text=""></asp:Label>
                </a>
            </h4>
        </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND7" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont-radio">
                                          <asp:CheckBoxList ID="chkLista_7" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
         
        </div>
      </div>   

  </asp:Panel> 
      <asp:Panel ID="pnlFiltro8" runat="server" Visible ="false" >
           <div class="panel-heading">
            <h4 class="panel-title">
                <a data-toggle="collapse" data-parent="#accordion-2" href="#collapse-CTIND8" class="collapsed line-punteada"> <asp:Label ID="lbl_8" runat="server" Text=""></asp:Label>
                </a>
            </h4>
        </div>
      <!-- menu 2 nivel -->
      <div id="collapse-CTIND8" class="panel-collapse collapse ">
        <div class="panel-body">
          <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding cont-radio">
             
                              <asp:CheckBoxList ID="chkLista_8" runat="server" AutoPostBack="true"></asp:CheckBoxList>
            </div>
          </div>
         
        </div>
      </div>   

         </asp:Panel> 


    </div>
  </div>

 </asp:Panel>

       
        <asp:Panel ID="pnlTitulo" runat="server">
              <h4 class="titulo">Buscar por:</h4>
        </asp:Panel>
            

            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
               <div class="panel panel-gtk"> 
                      <asp:Panel ID="pnlCategorias" runat="server"></asp:Panel>
          <%--          <div class="filtros-responsive">
                    </div>
                    <div class="filtros-dektop">
                       
                        <div class="panel-group filtos-catalogo" id="accordion" role="tablist" aria-multiselectable="true">
                            <asp:Panel ID="pnlCategorias" runat="server"></asp:Panel>
                           

                        </div>
                    </div>
                    <br />
                        <br />--%>
                    
                    </div>
                 </div> 

      
  <asp:Panel ID="pnlBuscadorAIO" runat="server" Visible ="false">
             <div class="main-container">
	<div class="flex-inicio">
		<div class="col-xs-12 col-sm-12 ">
			<div class="col-xs-12 col-sm-12 barra-latera-busqueda">
				<div class="text-buscar">
					BUSCAR
				</div>
				<div class="form-buscador">
					<div class="">
<div role="group" class="form-group">
							<label for="i-marca" class="d-block" >Marca:</label>
							<div class="bv-no-focus-ring">
								  <asp:DropDownList ID="ddlMarca" runat="server" class="select-general " AutoPostBack ="true" ></asp:DropDownList>
							</div>
						</div> 



						
                        <asp:Panel ID="pnlSubCat" runat="server">
                            <div role="group" class="form-group">

                                <label for="i-subcategoria" class="d-block">Subcategoría:</label>
                                <div class="bv-no-focus-ring">
                                    <asp:DropDownList ID="ddlSubcategoria" runat="server" class="select-general " AutoPostBack="true"></asp:DropDownList>
                                </div>
                            </div>
                        </asp:Panel>
						
						

						<div role="group" class="form-group">
							<label for="i-modelo" class="d-block" >Modelo:</label>
							<div class="bv-no-focus-ring">
							  <asp:DropDownList ID="ddlModelo" runat="server" class="select-general " AutoPostBack ="true" ></asp:DropDownList>
							</div>
						</div> 
						<div role="group" class="form-group">
							<label for="i-year" class="d-block" >Año:</label>
							<div class="bv-no-focus-ring">
								  <asp:DropDownList ID="ddlAnio" runat="server" class="select-general " AutoPostBack ="true" ></asp:DropDownList>
							</div>
						</div> 
<div role="group" class="form-group">
							<label for="i-categoria" class="d-block" >Categoría:</label>
							<div class="bv-no-focus-ring">
								  <asp:DropDownList ID="ddlCategoria" runat="server" class="select-general " AutoPostBack ="true" ></asp:DropDownList>
							</div>
						</div> 


						
						<%--<fieldset class="form-group box-keywords" >
							<div tabindex="-1" role="group" class="bv-no-focus-ring">
								<input type="text" placeholder="Buscar..." class="form-control" > 
								
							</div>
						</fieldset> --%>
						  <asp:Button ID="btnBuscar" CssClass ="btn mb-4 btn-search btn-search2 btn-secondary" runat="server" Text="BUSCAR" visible="true" />
						<%--<button type="submit" class="btn mb-4 btn-search btn-search2 btn-secondary">BUSCAR</button></div>--%>
				</div>
			</div>
		</div>
        </div>
		</div>
                 </div>
             </asp:Panel>


    </div>
 


             <%--AIO Y PMK <div class="col-xs-12 col-sm-8 stl-1-p"> Normal:col-sm-10--%>

                <div class="col-xs-12 col-sm-10 stl-1-p">
                    <div class="controles-top">
                        <asp:Panel ID="pnlOrdenarPor" runat="server">
						<div class="order">
							<label>Ordenar por:</label>
                            <asp:DropDownList ID="ddlOrden" runat="server" class="select-general " AutoPostBack ="true" ></asp:DropDownList>
							<%--<select class="select-general ">
								<option>precio</option>
								<option>Alafetico</option>
								<option>Popularidad</option>
							</select>--%>
						</div>
</asp:Panel>
						<div class="pag-sup">
							<span class="contador"><asp:label runat="server" text="0 Resultados" id="lblResultados"></asp:label></span>
                            <span class="paginador">
                                <asp:Button ID="btnPagina" CssClass ="btn-left" runat="server" Text="<<" /><asp:Label ID="lblPaginas" runat="server" Text=""></asp:Label> <asp:Button ID="btnSiguiente" CssClass ="btn-right" runat="server" Text=">>" /></span>
							<%--<span class="paginador"><button class="btn-left"></button>1/5<button class="btn-right">></button></span>--%>
						</div>
					</div>
                     <h4 class="titulo"><asp:Label ID="lblSinResultados" runat="server" Text="" Visible ="false" ></asp:Label></h4>
                    <div class="row">
  <asp:Panel ID="pnlCatalogos" runat="server" Visible="false"></asp:Panel>
                        </div>
                  
  <asp:Panel ID="pnlProductos" runat="server"></asp:Panel>

                    <div class="controles-top ct-reverse">
                        <div class="pag-sup">
                            <span class="contador">
                                <asp:Label runat="server" Text="0 Resultados" ID="lblResultadosAbajo"></asp:Label></span>
                            <span class="paginador">
                                <asp:Button ID="BtnAtrasAbajo" CssClass="btn-left" runat="server" Text="<<" /><asp:Label ID="lblPaginasAbajo" runat="server" Text="Label"></asp:Label>
                                <asp:Button ID="btnSiguienteAbajo" CssClass="btn-right" runat="server" Text=">>" /></span>
                            <%--<span class="paginador"><button class="btn-left"></button>1/5<button class="btn-right">></button></span>--%>
                        </div>
                    </div>
   
                    </div>

           

               
       

            </div>
      
        </div>
</ContentTemplate>
                      <Triggers>
                    
                          <asp:AsyncPostBackTrigger ControlID="ddlCategoria" EventName ="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="ddlSubcategoria" EventName ="SelectedIndexChanged"  />
                          <asp:AsyncPostBackTrigger ControlID="ddlMarca" EventName ="SelectedIndexChanged"  />
                          <asp:AsyncPostBackTrigger ControlID="ddlModelo" EventName ="SelectedIndexChanged"  />
                          <asp:AsyncPostBackTrigger ControlID="ddlAnio"  EventName ="SelectedIndexChanged" />

                          <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName ="Click"  />
                          <asp:AsyncPostBackTrigger ControlID="BtnAtrasAbajo" EventName ="Click"  />
                          <asp:AsyncPostBackTrigger ControlID="btnSiguienteAbajo" EventName ="Click"  />
                          <asp:AsyncPostBackTrigger ControlID="btnPagina" EventName ="Click"  />
                          <asp:AsyncPostBackTrigger ControlID="btnSiguiente" EventName ="Click"  />
           

                

            </Triggers>
                </asp:UpdatePanel>



    <script type="text/javascript">
	$( document ).ready(function() {
 			$cart_operaciones= '.min-shopping,.max-shopping';


		 	$("body").on("click", $cart_operaciones, function(e){
		        e.preventDefault();
		        contador = "#cantidad"+$(this).data('row');
		        opp = $(this).data('operacion');
		        mincont(contador,opp);
		      });
			});

			function mincont(contador,operacion){
				console.log(contador);
				console.log(operacion);
	        valor = $(contador).val();
	        switch(operacion){
	            case 'min':
	              valor = parseInt(valor) - 1;
	              if(valor < 1){
	                valor = 1;
	              }
	            break;
	            case 'plus':
	              valor = parseInt(valor) + 1;
	              if(valor > 200){
	                valor = 200;
	              }
	            break;
	           //put your cases here
	         }
	         $(contador).val(valor);
	      }
        
</script>
     <script type="text/javascript">
 function fnClick(Variable, Articulo) {

                //alert('entra');

     var rate_value =1;

                try {
                var rates = document.getElementsByName('tipoProducto' + Articulo);
                var rate_value;
                for(var i = 0; i < rates.length; i++){
                    if(rates[i].checked){
                        rate_value = rates[i].value;
                    }
                }
 
                } catch (error) {
 
                }
                var isbn = document.getElementById(Variable).value;
                var Cantidad = parseInt(isbn) * rate_value;
                            
                PageMethods.CargarCarrito(Cantidad, Articulo, onSucess, onError);

                function onSucess(result) {
                    //  alert(result);
                    PopUp('', 'Agregado al carrito', 'Aceptar', '', '', '', event);
                    
                }

                function onError(result) {
                    alert('Cannot process your request at the moment, please try later.');
                }

                       
            }
         </script>
     <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(updateProgress.ClientID) %>').style.display = "inline";
    }
    

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
            </div><!-- /.modal-content -->
          </div><!-- /.modal-dialog -->
        </div>
</asp:Content>