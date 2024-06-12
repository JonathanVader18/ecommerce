<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Index.aspx.vb" Inherits="Index" MasterPageFile ="~/Main.master"  EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
</asp:ScriptManager>

   

    <asp:Panel ID="pnlBanners" runat="server"></asp:Panel>
    <asp:Panel ID="pnlBusquedaTienda" runat="server"></asp:Panel> 

 <asp:Panel ID="pnlBarras" runat="server"></asp:Panel>
   	<div class="wrappercon naranja">
		
		
		
	</div>

   <asp:UpdateProgress ID="updateProgress" runat="server" AssociatedUpdatePanelID="ResultsUpdatePanel">
        <ProgressTemplate>
            <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #FFFFFF; opacity: 0.7;">
                <span style="border-width: 0px; position: fixed;  background-color: #FFFFFF; font-size: 36px; left: 40%; top: 40%;">Procesando ...</span>
                <img src="LOADER_mm.gif" style="margin:0 auto;">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

                       

     <asp:Panel ID="pnlBuscadorAIO" runat="server" Visible ="false">
             <div class="main-container">
	<div class="flex-inicio">
		
        

        <%-- <div class="col-xs-12 col-sm-8"> aqui va en PMK--%>
      
         <div class="col-xs-12 col-sm-8">
        <asp:Panel ID="pnlDestacadosAIO" runat="server"></asp:Panel>
            <asp:Panel ID="pnlServicios" runat="server"></asp:Panel>
        <asp:Panel ID="pnlNuestrasLineas" runat="server"></asp:Panel>
        
		
		</div>  
 
    <asp:UpdatePanel ID="ResultsUpdatePanel" ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server" >
                    <ContentTemplate >
        
        <div class="col-xs-12 col-sm-4 ">


			<div class="col-xs-12 col-sm-12 barra-latera-busqueda">
				<div class="text-buscar">
					BUSCAR
				</div>
				<div class="form-buscador">
					<div class="">
<div role="group" class="form-group">
							<label for="i-marca" class="d-block" >Marca:</label>
							<div class="bv-no-focus-ring">
								  <asp:DropDownList ID="ddlMarca" runat="server" class="select-general "   autopostback="true"  ></asp:DropDownList>
							</div>
						</div> 


	
 <asp:Panel ID="pnlSubCat" runat="server">
                            <div role="group" class="form-group">

                                <label for="i-subcategoria" class="d-block">Subcategoría:</label>
                                <div class="bv-no-focus-ring">
                                    <asp:DropDownList ID="ddlSubcategoria" runat="server" class="select-general "  autopostback="true" ></asp:DropDownList>
                                </div>
                            </div>
                        </asp:Panel>

						
						
						
						<div role="group" class="form-group">
							<label for="i-modelo" class="d-block" >Modelo:</label>
							<div class="bv-no-focus-ring">
							  <asp:DropDownList ID="ddlModelo" runat="server" class="select-general "    autopostback="true" ></asp:DropDownList>
							</div>
						</div> 
						<div role="group" class="form-group">
							<label for="i-year" class="d-block" >Año:</label>
							<div class="bv-no-focus-ring">
								  <asp:DropDownList ID="ddlAnio" runat="server" class="select-general "  autopostback="true" ></asp:DropDownList>
							</div>
						</div> 
					<div role="group" class="form-group">
							<label for="i-categoria" class="d-block" >Categoría:</label>
							<div class="bv-no-focus-ring">
								  <asp:DropDownList ID="ddlCategoria" runat="server" class="select-general " autopostback="true"  ></asp:DropDownList>
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
      </ContentTemplate>
                      <Triggers>
                    
                          <asp:AsyncPostBackTrigger ControlID="ddlCategoria" EventName ="SelectedIndexChanged" />
                          <asp:AsyncPostBackTrigger ControlID="ddlSubcategoria" EventName ="SelectedIndexChanged"  />
                          <asp:AsyncPostBackTrigger ControlID="ddlMarca" EventName ="SelectedIndexChanged"  />
                          <asp:AsyncPostBackTrigger ControlID="ddlModelo" EventName ="SelectedIndexChanged"  />
                          <asp:AsyncPostBackTrigger ControlID="ddlAnio"  EventName ="SelectedIndexChanged" />

                          <asp:AsyncPostBackTrigger ControlID="btnBuscar" EventName ="Click"  />
           

                

            </Triggers>
                </asp:UpdatePanel>


 

     

       


        </div>
                 </div>
             </asp:Panel>
 

     <script type="text/javascript">
         function fnClick( Articulo) {


                     

             PageMethods.RegisterUser( Articulo, onSucess, onError);

             function onSucess(result) {
                 //  alert(result);
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
</asp:Content>