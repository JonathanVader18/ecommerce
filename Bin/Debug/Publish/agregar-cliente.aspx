<%@ page language="VB" autoeventwireup="false" inherits="agregar_cliente, App_Web_fy50zy4w" masterpagefile="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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

<div class="col-xs-12">
		<div class="main-container">
			
            <div class="col-xs-12 col-sm-9 stl-1-p contenido">


                <div class="col-xs-12 col-sm-12">

                    <div class="blk-genericos">
                    </div>
                    <div class="blk-genericos extencion-2">
                        <div class="form-general">

                            <fieldset>
                                <div class="tit-bloque">
                                    <asp:Label ID="lblTitulo" runat="server" Text="Label"></asp:Label></div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">razón social</label>
                                            <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputPassword1">rfc</label>
                                            <asp:TextBox ID="txtrfc" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>


                                    </div>
                                </div>

                                  <div class="legend">PERSONA DE CONTACTO</div>
                                   <div class="row">
                                        <div class="col-xs-12 col-sm-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Nombre contacto</label>
                                            <asp:TextBox ID="txtNombreContacto" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                            <div class="col-xs-12 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Teléfono</label>
                                            <asp:TextBox ID="txtTelContacto" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Correo</label>
                                            <asp:TextBox ID="txtMailContacto" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                   </div>
                                 <div class="row">

                                 </div>

                                <div class="legend">DIRECCION FISCAL</div>

                                <div class="row">

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
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Localidad/Municipio</label>
                                            <asp:TextBox ID="txtLocalidad" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">CP</label>
                                            <asp:TextBox ID="txtCP" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">País</label>
                                            <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control" ></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Estado</label>
                                            <asp:DropDownList ID="ddlEstado" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>


                                </div>
                                <asp:Button ID="btnEntrar" runat="server" Text="crear" CssClass="btn btn-general-6" />
                                 <asp:Button ID="btnLevantarPed" runat="server" Text="levantar pedido" Visible ="false"  CssClass="btn btn-general-6" />
                            </fieldset>

                        </div>
                    </div>



                </div>

            </div>
    </div>
	</div>    
                        </ContentTemplate>
                      <Triggers>
                          
                <asp:AsyncPostBackTrigger ControlID="btnEntrar" EventName ="Click"  />
                           <asp:AsyncPostBackTrigger ControlID="btnLevantarPed" EventName ="Click"  />

                <asp:AsyncPostBackTrigger ControlID="ddlPais" EventName="SelectedIndexChanged" ></asp:AsyncPostBackTrigger>

                

            </Triggers>
                </asp:UpdatePanel>
     <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(ResultsUpdatePanel.ClientID) %>').style.display = "inline";
    }
    

    </script>
</asp:Content>