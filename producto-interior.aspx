<%@ Page Language="VB" AutoEventWireup="false" CodeFile="producto-interior.aspx.vb" Inherits="producto_interior" MasterPageFile ="~/Main.master" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="//netdna.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css">
    <style type ="text/css" >
        
div.stars {
  width: 270px;
  display: inline-block;
}

input.star { display: none; }

label.star {
  float: right;
  padding: 10px;
  font-size: 36px;
  color: #444;
  transition: all .2s;
}

input.star:checked ~ label.star:before {
  content: '\f005';
  color: #FD4;
  transition: all .25s;
}

input.star-5:checked ~ label.star:before {
  color: #FE7;
  text-shadow: 0 0 20px #952;
}

input.star-1:checked ~ label.star:before { color: #F62; }

label.star:hover { transform: rotate(-15deg) scale(1.3); }

label.star:before {
  content: '\f006';
  font-family: FontAwesome;
}
    </style>

  <script type ="text/javascript" >
         function justNumbers(e)
        {
        var keynum = window.event ? window.event.keyCode : e.which;
        if ((keynum == 8) || (keynum == 46))
        return true;
         
        return /\d/.test(String.fromCharCode(keynum));
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

  

  <%--      <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
            <ProgressTemplate>
                <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                    <span style="border-width: 0px; position: fixed; padding: 50px; background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                    <img src="LOADER_mm.gif">
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server">
            <ContentTemplate>--%>
     <asp:Panel ID="pnlRuta" runat="server" CssClass ="col-xs-12"></asp:Panel>

    <div class="wrappercon naranja col-xs-12">
        <div class="main-container">
            <div class="row">
                 <asp:Panel ID="pnlImagenes" runat="server"></asp:Panel>

                <asp:Panel ID="pnlInfoProducto" runat="server"></asp:Panel>
                <asp:Panel ID="pnlRating" runat ="server" Visible ="false" >
       <%--<div class="stars">
  <div >
    <input class="star star-5" id="star-5" type="radio" name="star" onclick ="PageMethods.fnRate('5');"/>
    <label class="star star-5" for="star-5"></label>
    <input class="star star-4" id="star-4" type="radio" name="star" onclick ="PageMethods.fnRate('4');"/>
    <label class="star star-4" for="star-4"></label>
    <input class="star star-3" id="star-3" type="radio" name="star" onclick ="PageMethods.fnRate('3');"/>
    <label class="star star-3" for="star-3"></label>
    <input class="star star-2" id="star-2" type="radio" name="star" onclick ="PageMethods.fnRate('2');"/>
    <label class="star star-2" for="star-2"></label>
    <input class="star star-1" id="star-1" type="radio" name="star" onclick ="PageMethods.fnRate('1');"/>
    <label class="star star-1" for="star-1"></label>
  </div>
</div>--%>
                </asp:Panel>
         
                <div class="col-xs-12 col-sm-12 info-producto-int">
                    <div class="col-xs-12 col-sm-12 no-padding">
                          <div class="col-xs-12 no-padding"><div class="precio sec-prec"> <asp:Label runat="server" ID="lblPrecio" Text=""></asp:Label></div></div>
                          <div class="col-xs-12 no-padding"><div class="precio-desctoPal"> <asp:Label runat="server" ID="lblPreciodesc" Text="" Visible ="false" ></asp:Label></div></div><br />
                        <asp:panel runat="server" ID="pnlExistencia" Visible ="true"><asp:Label ID="lblExistencia" runat="server" Text=""></asp:Label></asp:panel><br />
                        <asp:panel runat="server" ID="pnlLeyendaStock" Visible ="false"><asp:Label ID="lblLeyenda" runat="server" Text=""></asp:Label></asp:panel><br />
                          <asp:panel runat="server" ID="pnlMts" Visible ="false"><asp:Label ID="lblMts" runat="server" Text=""></asp:Label></asp:panel><br />
                           <asp:Panel runat="server" ID="pnlComprarCajas" Visible ="false" ></asp:Panel>
                        <div class="form-group">
                            <asp:Panel ID="pnlCantidad" runat="server" Visible="true">
                            <div class ="hidden"> <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control hidden" Text ="1" onkeypress="return justNumbers(event);"></asp:TextBox></div>
                            <label class="col-xs-12 col-sm-6 no-padding">cantidad:</label>
                            <div class="col-xs-12 col-sm-6">
                               
                                	<input type="numeric" class="form-control sprin" value="<% =txtCantidad.Text %>" id="#Cantidad"  onchange="SetDefault('#Cantidad');"  >	

                            </div>
                            <br>
                             </asp:Panel>
                        </div>
                        <asp:Panel runat="server" ID="pnlDataSheet" Visible ="false">
                             <div class="form-group">
                                
                                <div class="col-xs-12 col-sm-4">
                                    <asp:ImageButton ID="btnDatasheet" runat="server" ImageUrl="datasheet.png" />
                                    
                                </div>
                            </div>
                        </asp:Panel>
                         <asp:Panel runat="server" ID="pnlMts2" Visible ="false" >
                            <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding">Por Mts2:</label>
                                <div class="col-xs-12 col-sm-4">
                                    
                                    <input type="numeric" class="form-control sprin" value="1" id="#CantMts2"  onchange="SetDefaultMts('<% = CStr(Session("Mts2")).Replace(",", ".") %>');"  >	
                                    <asp:TextBox ID="txtMts" runat="server" CssClass="form-control" Text ="1" onkeypress="return justNumbers(event);" AutoPostBack="True" Visible ="false" ></asp:TextBox>
                                </div>
                            </div>

                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnlDescuento" Visible ="false" >
                            <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding">% descuento:</label>
                                <div class="col-xs-12 col-sm-4">
                                    <input type="numeric" class="form-control" value="0" id="#desc">
                                </div>
                            </div>

                        </asp:Panel>
                         <asp:Panel runat="server" ID="pnlTallaColor" Visible ="false" >
                               <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding"> <asp:Label runat ="server"  id="lblAtr1" Text ="" Visible ="false" > </asp:Label></label>
                                <div class="col-xs-12 col-sm-8">
                                    <asp:DropDownList ID="ddlAtr1" runat="server" Visible ="false" autopostback="true" Width ="80%"></asp:DropDownList> 
                                </div>
                            </div>

                              <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding"> <asp:Label runat ="server"  id="lblAtr2" Text ="" Visible ="false" ></asp:Label></label>
                                <div class="col-xs-12 col-sm-8">
                                    <asp:DropDownList ID="ddlAtr2" runat="server" Visible ="false" autopostback="true" Width ="80%"> </asp:DropDownList> 
                                </div>
                            </div>
                              <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding"> <asp:Label runat ="server"  id="lblAtr3" Text ="presentación" Visible ="false" ></asp:Label></label>
                                <div class="col-xs-12 col-sm-8">
                                    <asp:DropDownList ID="ddlAtr3" runat="server" Visible ="false" autopostback="true" Width ="80%" ></asp:DropDownList> 
                                </div>
                            </div>

                              <div class="form-group">
                                <label class="col-xs-12 col-sm-4 no-padding"> <asp:Label runat ="server"  id="lblAtr4" Text ="" Visible ="false" ></asp:Label></label>
                                <div class="col-xs-12 col-sm-8">
                                    <asp:DropDownList ID="ddlAtr4" runat="server" Visible ="false" autopostback="true" Width ="80%" ></asp:DropDownList> 
                                </div>
                            </div>

                             </asp:Panel>
                         <asp:Panel runat="server" ID="pnlFichasColor" Visible ="false" ></asp:Panel>

                        
                    </div>
                 
                    <div class="col-xs-6 col-sm-6 no-padding btn-responsive" >
                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-general-2" Visible ="false"  OnClientClick ="PopUp('', 'Agregado al carrito', 'Aceptar','','','',event);" />
                        <asp:Panel runat="server" ID="pnlAgregarConDesc" Visible ="false"  >
                            <a class="btn btn-general-2"   onclick="PageMethods.CargarCarritoDesc(document.getElementById('#Cantidad').value,document.getElementById('#desc').value, '', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','','',event); } function onError(result) {alert('err' + JSON.stringify(result));}">agregar</a>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlAgregar" Visible ="true"  >
                            <%--<a class="btn btn-general-2"   onclick="PageMethods.CargarCarrito(document.getElementById('#Cantidad').value, '', onSucess, onError);                                          function onSucess(result) { PopUp('', 'Agregado al carrito', 'Aceptar','','','',event); } function onError(result) {}">agregar</a>--%>
                             <a class="btn btn-general-2"   onclick="fnClick('#Cantidad','');">agregar</a>
                        </asp:Panel>
                        
                          <%-- <div class="fb-share-button" data-href="https://sap.bossfood.mx/ecommerce/catalogo.aspx" data-layout="button_count"></div>--%>
                        
                    </div>


                         <asp:scriptmanager runat="server" EnablePageMethods="true"></asp:scriptmanager>
                   
                 <asp:Panel ID="pnlCaracteristicas" runat="server"></asp:Panel>

                </div> 

             
    

               

                


            </div>
        </div>
    </div> 
          
    <br />
    <br />
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
    $(document).ready(function() {
        $('#example').DataTable({
       "destroy": true,
        "paging":   false,
        "ordering": false,
        "info":     false,
        "searching":     false
    } );
         });
         </script>

    <script type="text/javascript">
    $(document).ready(function() {
        $('#componentes').DataTable({
       "destroy": true,
        "paging":   false,
        "ordering": false,
        "info":     false,
        "searching":     false
    } );
         });
         </script>
   

     <script type="text/javascript">

          
 function SetDefault(Variable) {
              
                var isbn = document.getElementById(Variable).value;
              //  alert(isbn);
              
     document.getElementById("ctl00_ContentPlaceHolder1_txtCantidad").value = isbn;
             <%--  <% HttpContext.Current.Session("tempCantidad") = document.getElementById(Variable).value %> ;--%>
             
               
         }

          function SetDefaultMts(Mts2) {
              
             
            

     document.getElementById("#Cantidad").value =  Math.ceil(document.getElementById("#CantMts2").value / parseFloat(Mts2));
             <%--  <% HttpContext.Current.Session("tempCantidad") = document.getElementById(Variable).value %> ;--%>
             
               
         }

     </script>
             
        
           <%-- </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click"></asp:AsyncPostBackTrigger>
                
                <asp:AsyncPostBackTrigger ControlID="ddlAtr1" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="ddlAtr2" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="ddlAtr3" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="ddlAtr4" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>

                <%--   <asp:PostBackTrigger ControlID="btnEntrar"  />
                
            </Triggers>
        </asp:UpdatePanel>
        <script type="text/javascript">


            function ShowProgress() {
                document.getElementById('<% Response.Write(ResultsUpdatePanel.ClientID) %>').style.display = "inline";
            }


        </script>--%>
                <asp:Panel ID="pnlProductos" runat="server"></asp:Panel>
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
