<%@ Page Language="VB" AutoEventWireup="false" CodeFile="configbannersboss.aspx.vb" Inherits="configbannersboss" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <meta charset="utf-8">
        <meta http-equiv="x-ua-compatible" content="ie=edge">
        <title></title>
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width, initial-scale=1">

        <link rel="apple-touch-icon" href="apple-touch-icon.png">
        <!-- Place favicon.ico in the root directory -->
        <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">
        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700" rel="stylesheet">
        <link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet">
        <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <link rel="stylesheet" href="css/style.css">
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>

     <!-- Para las tablas necesito esto -->
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.16/css/jquery.dataTables.min.css"/>
    <script type="text/javascript" charset="utf8" src="//code.jquery.com/jquery-1.12.4.js"></script>
    <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.16/js/jquery.dataTables.min.js"></script>

    </head>
<body class="gtk">
    <div class="main-container">
		<h1 class="tit-seccion">Administrar Banners</h1>
	</div>
   
    <div class="wrappercon">

        <div class="main-container">

            <div class="col-xs-12 col-sm-12 stl-1-p contenido no-padding ">
                <div class="col-xs-12 col-sm-12 no-padding">
                    <div class="blk-genericos">
                        <div class="tit-bloque">
                        </div>
                        <form id="form1" runat="server">
                            <div>
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

                        <div class="row ">
                            <div class="col-xs-12 col-sm-9 ">
                                <div class="col-xs-4 col-sm-3">
                                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-general-4" OnClientClick="ShowProgress();" />
                                </div>
                                <div class="col-xs-4 col-sm-3">
                                    <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-general-4" OnClientClick="ShowProgress();" />
                                </div>
                                <div class="col-xs-4 col-sm-3">
                                    <asp:Button ID="btnRegresar" runat="server" Text="Volver al menú" CssClass="btn btn-general-4" OnClientClick="ShowProgress();" />
                                </div>
                            </div>
                        </div>
                         <div class="row ">
                                    <div class="col-xs-12 col-sm-6 ">
                                        <div class="form-group">
                                           <label for="exampleInputEmail1">Selecciona el tipo de banner a modificar</label>
                                            <asp:DropDownList ID="ddlTiposBanners" runat="server" AutoPostBack="True"></asp:DropDownList>
                                        </div>
                                    </div>
                                  
                                </div>


                                <div class="row ">
                                    <div class="col-xs-12 col-sm-6 ">
                                        <div class="form-group">
                                           <label for="exampleInputEmail1">Nombre</label>
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-4 ">
                                        <asp:CheckBox ID="chkActivo" runat="server" Checked="True" Text="Activo" />
                                    </div>
                                </div>
                          <div class="row ">
                              <div class="col-xs-12 col-sm-6 ">
                                   <div class="form-group">
                                        <label for="exampleInputEmail1">Ubicación del banner</label>
                                        <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="True"></asp:DropDownList>
                                       </div>
                                  </div>
                              </div>

                                <div class="row ">
                                    <div class="col-xs-12 col-sm-12">
                                         <label for="exampleInputEmail1">Seleccionar imagen</label>
                                        <asp:FileUpload ID="FileUpload1" runat="server" Width="390px" />
                                    </div>
                                  

                                </div>
                        <div class="row ">
                            <div class="col-xs-3 col-sm-3 ">
                                <asp:Button ID="Button1" runat="server" Text="subir imagen" CssClass="btn btn-general-3" OnClientClick="ShowProgress();" />
                            </div>
                        </div>
                              
                        <div class="row ">

                            <div class="col-xs-12 col-sm-6 ">
                                <asp:Image ID="imgBanner" runat="server" Height="100%" Width="100%" Visible="false" />
                            </div>


                        </div>
                        <div class="row ">
                            <div class="col-xs-3 col-sm-3 ">
                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-general-4" OnClientClick="ShowProgress();" />
                            </div>
                        </div>

                        <br /><br />
<asp:Panel ID="pnldet" runat="server" visible ="false" >
 <div class="row ">
                            <div class="col-xs-12 col-sm-6 ">
                                <label for="exampleInputEmail1">Texto sobre el banner</label>
                                <asp:TextBox ID="txtTexto" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-sm-3 ">
                                <label for="exampleInputEmail1">Orden/prioridad</label>
                                <asp:TextBox ID="txtOrden" runat="server" CssClass="form-control sprin"></asp:TextBox>
                            </div>
                           
                        </div>
</asp:Panel>
                       
                              <br />
                          <div class="legend">Banners principales:</div>
                         <div class="tit-bloque">
                        </div>
                                <div class="row ">
                                    <div class="col-xs-12 col-sm-12 ">
                                        <table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width: 100%">
                                        <thead>
                                        <tr>
                                            <th>Id</th>
                                            <th>Nombre</th>
                                             <th>Imagen</th>
                                            <th>Texto</th>
                                            <th>Orden</th>
                                            <th>Estatus</th>
                                            
                                        </tr>
                                    </thead>
                                            <tbody>
                                                <asp:Panel ID="pnlRegistros" runat="server"></asp:Panel>


                                            </tbody>
                                        </table>

                                      
                                    </div>
                                </div>
                               <div class="legend">Banners secundarios:</div>
                         <div class="tit-bloque">
                        </div>
                                <div class="row ">
                                    <div class="col-xs-12 col-sm-12 ">
                                        <table id="datatable-buttons1" class="table table-striped table-bordered display" cellspacing="0" style="width: 100%">
                                        <thead>
                                        <tr>
                                            <th>Id</th>
                                            <th>Tipo</th>
                                             <th>SubTipo</th>
                                            <th>Banner</th>
                                            
                                            
                                        </tr>
                                    </thead>
                                            <tbody>
                                                <asp:Panel ID="pnlBannersCat" runat="server"></asp:Panel>


                                            </tbody>
                                        </table>

                                      
                                    </div>
                                </div>
                                 <div class="row ">
                                      

                                    

                                     </div>


<telerik:RadDropDownList ID="rcbLenguaje" runat="server" Visible ="false" > </telerik:RadDropDownList>
 <asp:CheckBox ID="chkEspecifico" runat="server" AutoPostBack="True" Text="¿Especifico?" Visible ="false"  />
                                <table style="width: 100%;">
                                                   
                                  
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>
                                            <asp:Panel ID="pnlEspecifico" runat="server" Visible="False">
                                                <table style="width: 100%;">
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label5" runat="server" Text="Artículo"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <telerik:RadDropDownList ID="rcbArticulos" runat="server" AutoPostBack="True">
                                                            </telerik:RadDropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label6" runat="server" Text="Precio Actual"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPrecioActual" runat="server" CssClass="auto-style2" Width="117px"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="auto-style12">
                                                            <asp:Label ID="Label7" runat="server" Text="Precio Promo"></asp:Label>
                                                        </td>
                                                        <td class="auto-style12">
                                                            <asp:TextBox ID="txtPrecioEspecial" runat="server" CssClass="auto-style2" Width="118px"></asp:TextBox>
                                                        </td>
                                                        <td class="auto-style12"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="Label8" runat="server" Text="%Descto"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDescto" runat="server" CssClass="auto-style2" Width="118px"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                   
                                                                   
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td>
                                         
                                        </td>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td class="auto-style3">
                                          
                                        </td>
                                   
                                    </tr>
                                   
                                </table>


                         </ContentTemplate>
                      <Triggers>
                   
                           <asp:PostBackTrigger ControlID="Button1" />
                          <asp:PostBackTrigger ControlID="btnAgregar" />
                          <asp:PostBackTrigger ControlID="btnEliminar" />
                           <asp:PostBackTrigger ControlID="btnNuevo" />
                           <asp:PostBackTrigger ControlID="btnRegresar" />
                           <asp:PostBackTrigger ControlID="ddlTiposBanners" />
                           <asp:PostBackTrigger ControlID="ddlTipo" />
                           
                

            </Triggers>
                </asp:UpdatePanel>

                            </div>


                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>

     <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(updateProgress.ClientID) %>').style.display = "inline";
    }
    

    </script>
      <script src="https://ajax.aspnetcdn.com/ajax/jquery.migrate/jquery-migrate-1.2.1.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script>
        <script src="js/jquery.bootstrap-touchspin.min.js"></script>
        <script src="js/jquery.elevatezoom.js" type="text/javascript"></script>
        <script src="js/vendors/datatables.net/js/jquery.dataTables.min.js"></script>
        <script src="js/vendors/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
        <script src="js/vendors/datatables.net-buttons/js/dataTables.buttons.min.js"></script>
        <script src="js/vendors/datatables.net-buttons-bs/js/buttons.bootstrap.min.js"></script>
        <script src="js/vendors/datatables.net-buttons/js/buttons.flash.min.js"></script>
        <script src="js/vendors/datatables.net-buttons/js/buttons.html5.min.js"></script>
        <script src="js/vendors/datatables.net-buttons/js/buttons.print.min.js"></script>
        <script src="js/vendors/datatables.net-fixedheader/js/dataTables.fixedHeader.min.js"></script>
        <script src="js/vendors/datatables.net-keytable/js/dataTables.keyTable.min.js"></script>
        <script src="js/vendors/datatables.net-responsive/js/dataTables.responsive.min.js"></script>
        <script src="js/vendors/datatables.net-responsive-bs/js/responsive.bootstrap.js"></script>
        <script src="js/vendors/datatables.net-scroller/js/dataTables.scroller.min.js"></script>
        <script src="js/vendors/jszip/dist/jszip.min.js"></script>
        <script src="js/vendors/pdfmake/build/pdfmake.min.js"></script>
        <script src="js/vendors/pdfmake/build/vfs_fonts.js"></script>
        <script src="js/plugins.js"></script>
        <script src="js/custom.js"></script>
     <script type="text/javascript" src="js/plugins/datepicker.js"></script>
  <%--<script type="text/javascript" src="js/plugins/datepicker.js"></script>
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
</body>
</html>

