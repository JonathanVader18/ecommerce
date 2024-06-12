<%@ page language="VB" autoeventwireup="false" inherits="factura_modal, App_Web_fy50zy4w" %>
<%@ MasterType VirtualPath="~/Main.master" %>
<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>--%>



    <form id="form1" runat="server">
          
 <div id="mfp-build-tool" class="extencion factura-modal">
   <button title="Close (Esc)" type="button" class="mfp-close">×</button>

<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <div>
             <div class="col-xs-2 data-cliente col-sm-2 f-right"> <asp:Button ID="btnImprimir" CssClass ="btn btn-general-3" runat="server" Text="imprimir" visible="false"  /></div>
              <div class="col-xs-2 data-cliente col-sm-2 f-right"> <asp:Button ID="btnExcel" CssClass ="btn btn-general-3" runat="server" Text="exportar excel" visible="false"  /></div>
             </div><br />
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
  <div class="col-xs-12 data-cliente">
      <div class="col-xs-3 data-clientecol-sm-3 f-center"> <asp:Button ID="btnCargar" CssClass ="btn btn-general-3" runat="server" Text="cargar en carrito" visible="false" /></div>    
                <div class="col-xs-3 data-clientecol-sm-3 f-center"> <asp:Button ID="btnConvertir" CssClass ="btn btn-general-3" runat="server" Text="convertir a pedido" visible="false" /></div>
          <div class="col-xs-3 data-clientecol-sm-3 f-center"> <asp:Button ID="btnCerrar" CssClass ="btn btn-general-3" runat="server" Text="cancelar" visible="false"  OnClientClick ="return confirm('¿Seguro de cancelar el documento?');" /></div>
   <div class="col-xs-3 data-clientecol-sm-3 f-center"> <asp:Button ID="btnDuplicar" CssClass ="btn btn-general-3" runat="server" Text="duplicar" visible="false"  /></div>
            </div>

          <asp:TextBox ID="TextBox1" runat="server" visible ="false"></asp:TextBox>
          <asp:TextBox ID="txtDate" runat="server" ReadOnly="True" Width="245px" visible ="false"> </asp:TextBox>
         <asp:TextBox ID="txtMerchantId" runat="server" ReadOnly="True" Width="305px" visible="false">400c8a816d45cbaa016d4b60138c0004</asp:TextBox>
         <asp:TextBox ID="txtAudit" runat="server" ReadOnly="True" visible="false">123556</asp:TextBox>
         <asp:TextBox ID="txtOrigTransId" runat="server" ReadOnly="True" Width="300px" visible="false">1234567890123456789012345688601</asp:TextBox>
<div class="printarea">
        <div class="tit-bloque">
            <h2>Detalle</h2>    
        </div>
	
	<div class="col-xs-8 data-cliente">
		<div><strong>Cliente:</strong><asp:Label ID="lblCliente" runat="server" Text="Label"></asp:Label></div>
		<div><strong>Documento:</strong><asp:Label ID="lblDocNum" runat="server" Text="Label"></asp:Label></div>
        <%--<div><strong>Folio Fiscal:</strong>--------</div>--%>
		<div><strong>Fecha:</strong><asp:Label ID="lblFecha" runat="server" Text="Label"></asp:Label></div>
		<div><strong>Fecha entrega:</strong><asp:Label ID="lblFechaVence" runat="server" Text="Label"></asp:Label></div>
		<div><strong>Moneda:</strong><asp:Label ID="lblMoneda" runat="server" Text="Label"></asp:Label></div>
		<div><strong>Ejecutivo de ventas:</strong><asp:Label ID="lblVendedor" runat="server" Text="Label"></asp:Label></div>
    <asp:Panel ID="pnlActualizar" runat="server" Visible ="false" >
        <div><strong>Fecha de Entrega:</strong><asp:TextBox ID="txtFechaEntrega" runat="server"  textmode="date"></asp:TextBox></div>
         <div><strong>Estatus:</strong><asp:DropDownList ID="ddlEstatus" runat="server"  Width ="50%"></asp:DropDownList></div>
        <div><asp:Button ID="btnActualizar" CssClass ="btn btn-general-3" runat="server" Text="actualizar" style="width :50%;" /></div>
        </asp:Panel>
	</div>
        <br />      
        <div><strong><asp:Label ID="lblEstatus" runat="server" Visible="true"></asp:Label></strong></div>
        <br/>
        <div class="col-xs-4 col-sm-3 f-right">
            
               <asp:Panel ID="pnlMoneta" runat="server" Visible ="false" >
                        <ul class="resumen">
                            <li><img src="codTransfer.png" border="1" width="250" height="50"></li>
                            <li><asp:Label ID="Label2" runat="server" Text="Ingrese el código Transfer"></asp:Label></li>

                            	<li><div><asp:TextBox ID="txtCodMoneta" runat="server"  Width ="100%"></asp:TextBox></div></li>
                            <li><asp:Button ID="btnPagar" CssClass ="btn btn-general-3" runat="server" Text="pagar" visible="true" OnClientClick ="ShowProgress();"/></li>
                            </ul>
                     </asp:Panel>
         </div> 
	<table id="datatable-buttons" class="table table-striped table-bordered display" cellspacing="0" style="width:100%">
        <thead>
            <asp:Panel ID="pnlColumnas" runat="server"></asp:Panel>
        </thead>
	                        <tbody>
                                <asp:Panel ID="pnlRegistros" runat="server"></asp:Panel>
	                        </tbody>
    </table>
    <div class="col-xs-12 col-sm-3 f-right">
				<ul class="resumen">
					<li><div>Subtotal:<span><asp:Label ID="lblSubTotal" runat="server" Text="0.00"></asp:Label></span></div></li>
					<li><div><asp:Panel runat="server" id="pnlAnticipoLeyenda" visible="false">Anticipo:<span><asp:Label ID="lblAnticipo" runat="server" Text="0.00"  visible="false"></asp:Label></span></asp:Panel></div></li>
					<li><div>Impuestos:<span><asp:Label ID="lblIVA" runat="server" Text="0.00"></asp:Label></span></div></li>
                   <li><div><asp:Panel runat="server" id="pnlPagadoLeyenda" visible="false">Importe pagado:" <span><asp:Label ID="lblPagado" runat="server" Text="0.00"  visible="false"></asp:Label></span></asp:Panel></div></li>
					<li><div><asp:Panel runat="server" id="pnlPendienteLeyenda" visible="false">Saldo pendiente:<span><asp:Label ID="lblSaldo" runat="server" Text="0.00"  visible="false"></asp:Label></span></asp:Panel></div></li>
				</ul>
				<div class="total">Total:<span><asp:Label ID="lblTotal" runat="server" Text="0.00"></asp:Label></span></div>
			</div>
         </div>
        <div id="myModal" class="modal fade">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
               
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

</ContentTemplate>
            <Triggers>
             <asp:AsyncPostBackTrigger ControlID="btnConvertir" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnCargar" EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnDuplicar" EventName="Click"></asp:AsyncPostBackTrigger>
 <asp:AsyncPostBackTrigger ControlID="btnActualizar" EventName="Click"></asp:AsyncPostBackTrigger>
                  <asp:AsyncPostBackTrigger ControlID="btnCerrar" EventName="Click"></asp:AsyncPostBackTrigger>

<%--                <asp:AsyncPostBackTrigger ControlID="btnImprimir" EventName="Click"></asp:AsyncPostBackTrigger>
         
                   <asp:PostBackTrigger ControlID="btnEntrar"  />
                 <asp:AsyncPostBackTrigger ControlID="btnprint"  EventName="Click"></asp:AsyncPostBackTrigger>
                <asp:AsyncPostBackTrigger ControlID="btnPagar" EventName="Click"></asp:AsyncPostBackTrigger>--%>
               
                
            </Triggers>
        </asp:UpdatePanel>

</div>
             
            
        <script type="text/javascript">


            function ShowProgress() {
                document.getElementById('<% Response.Write(ResultsUpdatePanel.ClientID) %>').style.display = "inline";
            }


        </script>

    </form>
 

    


    <script type="text/javascript">
        $('body').on('click', '.detalles-popup', function (e) {
            e.preventDefault();
            $('.detalles-popup').magnificPopup({
                type: 'inline',
                preloader: false,
                focus: '#username',
                modal: true
            });
        });

        $(document).on('click', '.popup-modal-dismiss', function (e) {
            e.preventDefault();
            $.magnificPopup.close();
        });

    </script>

<%--<asp:AsyncPostBackTrigger ControlID="btnImprimir" EventName="Click"></asp:AsyncPostBackTrigger>--%>
