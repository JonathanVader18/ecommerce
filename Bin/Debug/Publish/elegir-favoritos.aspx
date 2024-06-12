<%@ page language="VB" autoeventwireup="false" inherits="elegir_favoritos, App_Web_3bsbwt4p" %>

   
        <div id="mfp-build-tool" class="prev-modal">
  <div class="col-xs-12">
           <%-- <img src="img/header/logo.png" class="img-responsive cent-img">--%>
            <div class="col-xs-12 text-center no-padding">
                    <strong class="text-underline">
                        <asp:Label ID="lblTitulo" runat="server" Text="seleccione entre sus plantillas guardadas"></asp:Label></strong>
                    
            </div>
            <div class="col-xs-12 text-center no-padding" >   <%--  <div class="col-xs-12 text-center" >--%>
                <form runat="server" >
            
                     <asp:scriptmanager runat="server" EnablePageMethods="true"></asp:scriptmanager>
                  <div class="form-group">
                        <asp:DropDownList ID="ddlFavoritos" runat="server" Visible ="true" Width ="100%" CssClass="form-control"></asp:DropDownList>
                  </div>
                 

                  <div class="col-xs-12 text-center no-padding">
                      
                     <asp:Button ID="btnSeleccionar" runat="server" Text="aceptar" class="btn btn-general-2"  visible="false"/>
                      <asp:Button ID="btnConfirmar" runat="server" Text="aceptar"  class="btn btn-general-2"   visible="false" />

                     <%-- PopUp('', 'Agregado a la plantilla', 'Aceptar','','','',event);--%>
                      <asp:panel runat="server" id="pnlAgregarLista" visible="false">
                          <a class="btn btn-general-2" id="#btnAgregar"  onclick="PageMethods.CargarALista('<%= ddlFavoritos.SelectedValue  %>', '', onSucess, onError);   function onSucess(result) { PopUp('', 'Agregado a la lista', 'Aceptar','','','',event);} function onError(result) {}">agregar</a>
                      </asp:panel>
                      <%-- PopUp('', 'Agregado a la plantilla', 'Aceptar','','','',event);--%>
                      <asp:panel runat="server" id="pnlAgregar" visible="true">
                          <a class="btn btn-general-2" id="#btnAgregarSinDesc"  onclick="PageMethods.CargarFavorito('', '', onSucess, onError);   function onSucess(result) { } function onError(result) {}">agregar</a>
                      </asp:panel>
                       

                       
                  </div>
                  
                </form>
            </div>
        </div>
        </div>

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

