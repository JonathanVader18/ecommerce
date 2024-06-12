<%@ Page Language="VB" AutoEventWireup="false" CodeFile="rechazocompra.aspx.vb" Inherits="rechazocompra" MasterPageFile ="~/Main.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="col-xs-12">
		<div class="main-container">
			
			<div class="col-xs-12 col-sm-12 stl-1-p contenido">
				<div class="blk-genericos">
	                <div class="col-sm-12">
		                <div class="tit-bloque"><h1>Ha ocurrido un problema al procesar la transacción</h1></div>
	                </div>
 						<div class="content-blok">
						    <div class="col-xs-12 col-sm-12">
						    	<div class="form-group">
							        <label for="exampleInputPassword1">Se ha rechazado el cobro o bien, ha decidido cancelar el procesa y regresar a la tienda. Si considera que pudo ser por un error, vuelva a intentarlo.</label>
                                						        
							    </div>
							
                            
                                <a type="submit" class="btn btn-general-1 btn-left" href="resumen.aspx">intentar nuevamente</a>

							
								
						    </div>
						</div>
					
			     </div>
			</div>
		</div>
	</div>
    </asp:Content>