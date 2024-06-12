<%@ Page Language="VB" AutoEventWireup="false" CodeFile="configDescuentos.aspx.vb" Inherits="configDescuentos" %>

<!DOCTYPE html>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<html class="no-js" lang="">
<head runat="server">
    
   <meta charset="utf-8">
        <meta http-equiv="x-ua-compatible" content="ie=edge"/>
<title></title>
<meta name="description" content="">
<meta name="viewport" content="width=device-width, initial-scale=1"/>

    

<%--<link rel="icon" type="image/png" sizes="32x32" href="favicon/favicon-32x32.png">
<link rel="icon" type="image/png" sizes="16x16" href="favicon/favicon-16x16.png">
<link rel="manifest" href="favicon/site.webmanifest">--%>

    <%--<link rel="apple-touch-icon" sizes="180x180" href="favicon/apple-touch-icon.png">
<link rel="mask-icon" href="favicon/safari-pinned-tab.svg" color="#000000">--%>
<meta name="msapplication-TileColor" content="#ffffff">
<meta name="theme-color" content="#ffffff">


  <%--      <link rel="apple-touch-icon" href="apple-touch-icon.png"/>--%>
        <!-- Place favicon.ico in the root directory -->
           <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"/>
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous"/>
        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700" rel="stylesheet"/>
        <%--<link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet"/>--%>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700&display=swap" rel="stylesheet"/>
        <link rel="stylesheet" href="css/jquery.bootstrap-touchspin.min.css"/>
        <link rel="stylesheet" href="css/style.css"/>
    <%--    <script src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>--%>
      <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/detectizr/2.2.0/detectizr.min.js"></script>

<%-- <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-zoom/1.7.21/jquery.zoom.min.js"></script>--%>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-zoom/1.7.21/jquery.zoom.min.js" integrity="sha256-bODH9inPFT93cjyz5XDGwckaBOMDrDwrfdYPVlWU2Hk=" crossorigin="anonymous"></script>

    </head>
<body class="gtk">
    <div class="main-container">
		<h1 class="tit-seccion">Administrar Descuentos</h1>
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
                                    <div class="col-xs-12 col-sm-6 ">
                                        <div class="form-group">
                                           <label for="exampleInputEmail1">Código del artículo</label>
                                            <asp:TextBox ID="txtArticulo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-4 ">
                                       <label for="exampleInputEmail1">% de Descuento</label>
                                       <asp:TextBox ID="txtDescuento" runat="server" CssClass="form-control" TextMode ="Number"></asp:TextBox>
                                    </div>
                                </div>
   <br /><br />
                               
                                <div class="row ">
                                   
                                    <div class="col-xs-12 col-sm-6 ">
                                         <label for="exampleInputEmail1">Vigencia del Descuento</label>
                                    </div>
                                </div>
                     
                        <div class="row ">
                            <div class="col-xs-12 col-sm-3 ">
                                <label for="exampleInputEmail1">Del:</label>
                                <asp:TextBox ID="txtFechaIni" runat="server" CssClass="form-control" TextMode ="Date" ></asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-sm-3 ">
                                <label for="exampleInputEmail1">Al:</label>
                                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="form-control" TextMode ="Date" ></asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-sm-3 ">
                                <br />
                               
                            </div>
                        </div>
                              <br />
                         <div class="row ">
                            <div class="col-xs-12 col-sm-3 ">
 <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-general-7" OnClientClick="ShowProgress();" />
                                </div>
                             </div>
                          <div class="legend">Artículos con Descuento:</div>
                         <div class="tit-bloque">
                        </div>
                                <div class="row ">
                                    <div >
                                        <div class="x_content">
                                           <%-- <div>
                                                <div class="input-daterange input-group " id="datepicker">
                                                    <div class="col-xs-6">
                                                        <div class="form-group">
                                                            <label>
                                                                del
							    			<input type="text" class=" start-date form-control datepicker" name="start" id="fromDate" />
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="col-xs-6">
                                                        <div class="form-group">
                                                            <label>
                                                                al
							    			<input type="text" class=" form-control datepicker" name="end" id="toDate" />
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width: 100%">
                                                <thead>
                                                    <tr>
                                                        <th>Artículo</th>
                                                        <th>Descuento</th>
                                                        <th>Del</th>
                                                        <th>Al</th>


                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:Panel ID="pnlRegistros" runat="server"></asp:Panel>


                                                </tbody>
                                            </table>
                                        </div>
                                      
                                    </div>
                                </div>
                                 <div class="row ">
                                       <div class="col-xs-12 col-sm-4 ">
                                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" cssclass="btn btn-general-4" OnClientClick ="ShowProgress();"/>
   <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" cssclass="btn btn-general-3" OnClientClick ="ShowProgress();"/>
                                           <asp:Button ID="btnRegresar" runat="server" Text="Volver al menú" cssclass="btn btn-general-4" OnClientClick ="ShowProgress();"/>
                                          </div>

                                    

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
                   
                           
                          <asp:PostBackTrigger ControlID="btnAgregar" />
                          <asp:PostBackTrigger ControlID="btnEliminar" />
                           <asp:PostBackTrigger ControlID="btnNuevo" />
                           <asp:PostBackTrigger ControlID="btnRegresar" />
                           
                

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
  <%--<script type="text/javascript" src="js/plugins/datepicker.js"></script>--%>
	<%--<script type="text/javascript">
        $(document).ready(function() {
            $('datatable-buttons').DataTable( {
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
