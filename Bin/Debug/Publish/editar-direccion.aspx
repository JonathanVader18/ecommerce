<%@ page language="VB" autoeventwireup="false" inherits="editar_direccion, App_Web_atyal0ov" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="col-xs-12">
		<div class="main-container">
             <div class="col-xs-12 col-sm-2">
                <div class="filtros-dektop">
                    <div class="panel-group filtos-catalogo" id="accordion" role="tablist" aria-multiselectable="true">
                        <div class="panel">
                            <div class="panel-heading" role="tab" id="headingOne">
                                <h4 class="categoria">
                                    <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">mi cuenta
                                    </a>
                                </h4>
                            </div>
                            <div id="collapseOne" class="panel-collapse collapse in" role="tabpanel" aria-labelledby="headingOne">
                                <div class="panel-body">
                                    <ul class="subcategorias">
                                         <asp:Panel ID="pnlMenuPref" runat="server"></asp:Panel>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

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
            <div class="col-xs-12 col-sm-10 stl-1-p contenido">
				<div class="blk-genericos">
					<div class="tit-bloque"><h2>direcciones</h2></div>
					<div class="row">
						<div class="content-blok">
							<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Calle</label>
								        <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Número exterior</label>
								        <asp:TextBox ID="txtNumExt" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Número interior</label>
								        <asp:TextBox ID="txtNumInt" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Colonia</label>
								        <asp:TextBox ID="txtColonia" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-6">   
								    <div class="form-group">
								        <label for="exampleInputEmail1">Ciudad</label>
								         <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Localidad/Municipio</label>
								        <asp:TextBox ID="txtLocalidad" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">CP</label>
								        <asp:TextBox ID="txtCp" runat="server" CssClass="form-control"></asp:TextBox>
								    </div>
								</div>
								<div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">Estado</label>
								          <asp:DropDownList ID="ddlEstados" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
								    </div>
								</div>
                                 <div class="col-xs-12 col-sm-4">
								    <div class="form-group">
								        <label for="exampleInputEmail1">País</label>
								            <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" ></asp:DropDownList>
								    </div>
								 </div>
								<div class="col-xs-12">
                                    <asp:Button ID="btnGuardar" runat="server" Text="guardar" cssclass="btn btn-general-2 btn-left" OnClientClick ="ShowProgress();" />
                                     <asp:Button ID="btnCancelar" runat="server" Text="cancelar" cssclass="btn btn-general-1 btn-lef"  OnClientClick ="ShowProgress();" />
                                    									
								</div>
						</div>
					</div>
				</div>
				
			</div>

		</ContentTemplate>
         <Triggers>
                       
           <asp:AsyncPostBackTrigger ControlID="ddlPais" EventName ="SelectedIndexChanged" />
              
                <asp:PostBackTrigger ControlID="btnGuardar" />
                <asp:PostBackTrigger ControlID="btnCancelar" />
                        
            

                

            </Triggers>
        </asp:UpdatePanel>




            </div>
          </div>
      <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(updateProgress.ClientID) %>').style.display = "inline";
    }
    

    </script>
    </asp:Content>