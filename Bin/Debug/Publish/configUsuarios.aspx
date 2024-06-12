<%@ page language="VB" autoeventwireup="false" inherits="configUsuarios, App_Web_3bsbwt4p" %>


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
		<h1 class="tit-seccion">Administrar Usuarios Vendedores</h1>
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
                                           <label for="exampleInputEmail1">Nombre</label>
                                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                   
                                </div>

                                <div class="row ">
                                    <div class="col-xs-12 col-sm-6 ">
                                         <label for="exampleInputEmail1">Empleado SAP</label>
                                        <asp:DropDownList ID="ddlVendedorSAP" runat="server" AutoPostBack="True"></asp:DropDownList>
                                    </div>
                                   
                                </div>
                              
                        <br /><br />
                        <div class="row ">
                            <div class="col-xs-12 col-sm-6 ">
                                <label for="exampleInputEmail1">Usuario</label>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="col-xs-12 col-sm-3 ">
                                <label for="exampleInputEmail1">Contraseña</label>
                                <asp:TextBox ID="txtPass" runat="server" CssClass="form-control sprin" TextMode ="Password" ></asp:TextBox>
                                <asp:Label ID="lblPass" runat="server" Text="*****" Visible ="false"></asp:Label>
                            </div>
                              <div class="col-xs-12 col-sm-3 ">
                                <label for="exampleInputEmail1">Correo electrónico</label>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control sprin"></asp:TextBox>
                            </div>
                            
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-3 ">
                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-general-7" OnClientClick="ShowProgress();" />
                            </div>
                        </div>
                              <br />
                          <div class="legend">Usuarios:</div>
                         <div class="tit-bloque">
                        </div>
                                <div class="row ">
                                    <div class="col-xs-12 col-sm-12 ">
                                        <table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width: 100%">
                                        <thead>
                                        <tr>
                                            <th>Id</th>
                                            <th>Nombre</th>
                                             <th>Usuario</th>
                                            <th>Empleado SAP</th>
                                            <th>Correo</th>
                                          
                                        </tr>
                                    </thead>
                                            <tbody>
                                                <asp:Panel ID="pnlRegistros" runat="server"></asp:Panel>


                                            </tbody>
                                        </table>

                                      
                                    </div>
                                </div>
                                 <div class="row ">
                                       <div class="col-xs-12 col-sm-4 ">
                                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" cssclass="btn btn-general-4" OnClientClick ="ShowProgress();"/>
   <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" cssclass="btn btn-general-3" OnClientClick ="ShowProgress();"/>
                                           <asp:Button ID="btnRegresar" runat="server" Text="Volver al menú" cssclass="btn btn-general-4" OnClientClick ="ShowProgress();"/>
                                          </div>

                                    

                                     </div>





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
    
  <script type="text/javascript" src="js/plugins/datepicker.js"></script>
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
    </script>
</body>
</html>

