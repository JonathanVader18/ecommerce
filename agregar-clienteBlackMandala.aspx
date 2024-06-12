<%@ Page Language="VB" AutoEventWireup="false" CodeFile="agregar-clienteBlackMandala.aspx.vb" Inherits="agregar_clienteBlackMandala" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

   <!-- Facebook Pixel Code -->
<script>
!function(f,b,e,v,n,t,s)
{if(f.fbq)return;n=f.fbq=function(){n.callMethod?
n.callMethod.apply(n,arguments):n.queue.push(arguments)};
if(!f._fbq)f._fbq=n;n.push=n;n.loaded=!0;n.version='2.0';
n.queue=[];t=b.createElement(e);t.async=!0;
t.src=v;s=b.getElementsByTagName(e)[0];
s.parentNode.insertBefore(t,s)}(window,document,'script',
'https://connect.facebook.net/en_US/fbevents.js');
fbq('init', '2013498418958686'); 
fbq('track', 'PageView');
</script>
<noscript>
<img height="1" width="1" 
src="https://www.facebook.com/tr?id=2013498418958686&ev=PageView
&noscript=1"/>
</noscript>
<!-- End Facebook Pixel Code -->


<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

      <meta http-equiv="x-ua-compatible" content="ie=edge"/>
     
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width, initial-scale=1"/>

    
    <link rel="apple-touch-icon" sizes="57x57" href="favicon/apple-touch-icon-57x57.png">
        <link rel="apple-touch-icon" sizes="60x60" href="favicon/apple-touch-icon-60x60.png">
        <link rel="apple-touch-icon" sizes="72x72" href="favicon/apple-touch-icon-72x72.png">
        <link rel="apple-touch-icon" sizes="76x76" href="favicon/apple-touch-icon-76x76.png">
        <link rel="apple-touch-icon" sizes="114x114" href="favicon/apple-touch-icon-114x114.png">
        <link rel="apple-touch-icon" sizes="120x120" href="favicon/apple-touch-icon-120x120.png">
        <link rel="apple-touch-icon" sizes="144x144" href="favicon/apple-touch-icon-144x144.png">
        <link rel="apple-touch-icon" sizes="152x152" href="favicon/apple-touch-icon-152x152.png">
        <link rel="apple-touch-icon" sizes="180x180" href="favicon/apple-touch-icon-180x180.png">
        <link rel="icon" type="image/png" sizes="32x32" href="favicon/favicon-32x32.png">
        <link rel="icon" type="image/png" sizes="192x192" href="favicon/android-chrome-192x192.png">
        <link rel="icon" type="image/png" sizes="16x16" href="favicon/favicon-16x16.png">
        <link rel="manifest" href="favicon/manifest.json">
        <link rel="mask-icon" href="favicon/safari-pinned-tab.svg" color="#c22820">
        <meta name="msapplication-TileColor" content="#ffffff">
        <meta name="msapplication-TileImage" content="/mstile-144x144.png">
        <meta name="theme-color" content="#ffffff">


  <%--      <link rel="apple-touch-icon" href="apple-touch-icon.png"/>--%>
        <!-- Place favicon.ico in the root directory -->
           <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous"/>
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous"/>
        <link href="https://fonts.googleapis.com/css?family=Montserrat:300,400,500,600,700" rel="stylesheet"/>
        <link href="https://fonts.googleapis.com/css?family=Roboto:400,500,700" rel="stylesheet"/>
        <link rel="stylesheet" href="css/jquery.bootstrap-touchspin.min.css"/>
        <link rel="stylesheet" href="css/style.css"/>
        <script src="https://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js"></script>
        <script src="js/vendor/modernizr-2.8.3.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/detectizr/2.2.0/detectizr.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
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
			
            <div class="col-xs-12 stl-1-p contenido">
                

                <div class="col-xs-12 col-sm-12">

                    <div class="blk-genericos">
                    </div>
                    <div class="blk-genericos extencion-2">
                        <div class="form-general">

                            <fieldset>
                                <%--<div class="tit-bloque">
                                    <asp:Label ID="lblTitulo" runat="server" Text="AGREGAR CLIENTE"></asp:Label></div>--%> <asp:Label ID="lblTitulo" runat="server" Text="AGREGAR CLIENTE" Visible ="false" ></asp:Label>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            
                                            <label for="exampleInputEmail1">Tipo de persona</label>
                                            <asp:DropDownList ID="ddlTipoPersona" runat="server" AutoPostBack="True"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">

                                    <asp:Panel runat="server" id="pnlFisica" Visible ="false" >

                                          <div class="col-xs-12 col-sm-4">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">* Nombre</label>
                                                <asp:TextBox ID="txtnombre" runat="server" CssClass="form-control" style="text-transform:uppercase;"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-4">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">* Apellido paterno</label>
                                                <asp:TextBox ID="txtappaterno" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                            </div>
                                        </div>
                                          <div class="col-xs-12 col-sm-4">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">Apellido materno</label>
                                                <asp:TextBox ID="txtapmaterno" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                    <asp:Panel runat="server" ID="pnlMoral" Visible="false">
                                        <div class="col-xs-12 col-sm-12">
                                            <div class="form-group">
                                                <label for="exampleInputEmail1">* Razón social</label>
                                                <asp:TextBox ID="txtRazonSocial" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputPassword1">RFC</label>
                                            <asp:TextBox ID="txtrfc" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>

                                        <div class="form-group">
                                            <label for="exampleInputPassword1">Teléfono</label>
                                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" style="text-transform:uppercase" MaxLength="20"></asp:TextBox>
                                        </div>


                                    </div>
                                    </div>
                                 <div class="row">
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputPassword1">* Correo electrónico</label>
                                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode = "Email"></asp:TextBox>
                                        </div>


                                    </div>
                                      <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputPassword1">* Confirmar correo electrónico</label>
                                            <asp:TextBox ID="txtCorreoConfirma" runat="server" CssClass="form-control" TextMode = "Email" AutoPostBack ="true"></asp:TextBox>
                                        </div>


                                    </div>
                                </div>

                                 <div class="tit-bloque">
                                <div class="legend">DIRECCIÓN FISCAL</div>
                                     </div>
                                <div class="row">

                                    <div class="col-xs-12 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Calle</label>
                                            <asp:TextBox ID="txtCalle" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Número exterior</label>
                                            <asp:TextBox ID="txtNumExt" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-4">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">Número interior</label>
                                            <asp:TextBox ID="txtNumInt" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Colonia</label>
                                            <asp:TextBox ID="txtColonia" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Ciudad</label>
                                            <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Localidad/Municipio</label>
                                            <asp:TextBox ID="txtLocalidad" runat="server" CssClass="form-control" style="text-transform:uppercase"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* CP</label>
                                            <asp:TextBox ID="txtCP" runat="server" CssClass="form-control" style="text-transform:uppercase" ></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* País</label>
                                            <asp:DropDownList ID="ddlPais" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Estado</label>
                                            <asp:DropDownList ID="ddlEstado" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>

                                      <div class="col-xs-12 col-sm-12">
                                        <div class="form-group">
                                            <label for="exampleInputEmail1">* Profesión</label>
                                            <asp:DropDownList ID="ddlEspecialidad" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control" Visible ="false" ></asp:DropDownList>
                                            <asp:TextBox ID="txtEspecialidad" runat="server" CssClass="form-control" Visible ="true"></asp:TextBox>
                                            <asp:Panel ID="pnlOtro" runat="server" Visible ="false" > <label for="exampleInputEmail1">Indique cuál</label><asp:TextBox ID="txtOtro" runat="server" CssClass="form-control" Visible ="true"></asp:TextBox></asp:Panel>
                                        </div>
                                    </div>


                                </div>
                                <div class="row">
                                        <div class="col-xs-12 col-sm-12 txt-aling-left">
                                        <p>
                                            <asp:CheckBox ID="chkRecibir" runat="server" Text ="Deseo recibir promociones e información de nuevos productos"  /></p> 
                                            </div>
                                       </div>
                                 <div class="row">
                                        <div class="col-xs-12 col-sm-12 txt-aling-left">
                                        <p>
                                            <asp:CheckBox ID="chkRevista" runat="server" Text ="Deseo recibir la revista Dental para Cual TOTALMENTE GRATIS"  AutoPostBack="True" Visible ="false" /></p> 
                                            <asp:Panel ID="pnlRevista" runat="server" Visible ="false" >
                                               <asp:DropDownList ID="ddlTipoRevista" runat="server" Width="100%" AutoPostBack="True" CssClass="form-control"></asp:DropDownList>
                                            </asp:Panel>
                                            </div>
                                       </div>

                                      <div class="row">
                                        <div class="col-xs-12 col-sm-6 txt-aling-left">
                                        <p>* Campos obligatorios</p> 
                                            </div>
                                       </div>
                                   <div class="row">
                                        <div class="col-xs-12 col-sm-6">
                                          <asp:CheckBox ID="chkTerminos" runat="server" Text ="Al darte de alta, aceptas nuestro "  /><a class="link-aviso" href="https://blackmandala.mx/aviso-privacidad/" target="_blank">aviso de privacidad</a>
                                            </div>
                                       </div>
                          
                                <asp:Panel runat="server" ID="pnlButton">
                                    <asp:Button ID="btnEntrar" runat="server" Text="crear" CssClass="btn btn-general-6" OnClientClick="ShowProgress();" />
                                </asp:Panel>
                                
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
                <asp:AsyncPostBackTrigger ControlID="ddlPais" EventName="SelectedIndexChanged" ></asp:AsyncPostBackTrigger>

                   <asp:AsyncPostBackTrigger ControlID="ddlTipoPersona" EventName="SelectedIndexChanged" ></asp:AsyncPostBackTrigger>
                            <asp:AsyncPostBackTrigger ControlID="ddlEspecialidad" EventName="SelectedIndexChanged" ></asp:AsyncPostBackTrigger>
                   <asp:AsyncPostBackTrigger ControlID="chkRevista" EventName="CheckedChanged" ></asp:AsyncPostBackTrigger>
                   <asp:AsyncPostBackTrigger ControlID="txtCorreoConfirma" EventName="TextChanged" ></asp:AsyncPostBackTrigger>
                 

            </Triggers>
                </asp:UpdatePanel>
     <script type="text/javascript">
    

    function ShowProgress()
    {
        document.getElementById('<% Response.Write(ResultsUpdatePanel.ClientID) %>').style.display = "inline";
    }
    

    </script>
    </form>
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
</body>
</html>
