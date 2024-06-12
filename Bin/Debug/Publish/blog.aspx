<%@ page language="VB" autoeventwireup="false" inherits="blog, App_Web_skzvnhfn" masterpagefile="~/Main.master" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="col-xs-12">
		<div class="main-container">
			<h2 class="pag-cont-tit">blog</h2>
		</div>
	</div>
	<div class="col-xs-12 no-padding">
		<div class="main-container">
			<div class="col-xs-12 col-sm-8 col-md-9 no-padding-right">
			<!-- primera nota -->
                <asp:Panel runat ="server" id="pnlEntradas">

                </asp:Panel>
                <%--<div class="col-xs-12 entrada e-principal">
                    <div class="col-xs-12 no-padding">
                        <img src="http://lorempixel.com/800/280/sports" class="img-responsive" />
                    </div>
                    <div class="col-xs-12 no-padding">
                        <div class="data-e">
                            <a href="#" class="categoria">CATEGORÍA</a>
                            <time class="fecha-publicacion">08/03/2017</time>
                        </div>
                        <h2 class="titulo">título</h2>
                        <div class="descripcion">
                            <p>Quisque id est libero. Morbi non ligula sit amet eros porta efficitur. Suspendisse mi felis, fringilla vitae scelerisque sed, varius non massa. Curabitur lobortis id nibh in maximus. Aenean egestas dictum tempus. Sed sodales lorem ac lorem fringilla malesuada. Aliquam interdum, orci a tristique fermentum, arcu elit mollis urna, at placerat odio sapien at nisl.</p>
                        </div>
                        <div>
                            <a class="btn" href="blog-interno.aspx">LEER MÁS </a>
                        </div>
                    </div>
                </div>
				

                <div class="col-xs-12 col-sm-12 col-md-6  entrada e-secundaria">
                    <div class="col-xs-12 no-padding">
                        <img src="http://lorempixel.com/325/255/sports" class="img-responsive" />
                    </div>
                    <div class="col-xs-12 no-padding">
                        <div class="data-e">
                            <a href="#" class="categoria">CATEGORÍA</a>
                            <time class="fecha-publicacion">08/03/2017</time>
                        </div>
                        <h2 class="titulo">título nota</h2>
                        <div class="descripcion">
                            <p>Quisque id est libero. Morbi non ligula sit amet eros porta efficitur. Quisque id est libero. Morbi non ligula sit amet eros porta efficitur. </p>
                        </div>
                        <div>
                            <a class="btn" href="blog-interno.aspx">LEER MÁS </a>
                        </div>
                    </div>
                </div>--%>
					
				
			</div>
			<div class="col-xs-12 col-sm-6 col-md-3 panel-redes">
				<h4>
					Entradas Recientes
				</h4>
                <div class="noticias-laterales">
                 <asp:Panel runat ="server" ID="pnlEntradasRecientes">
                 </asp:Panel>
				<%--
					<div class="lat-entrada">
						<a href="#">
							<div class="imagen"><img src="http://lorempixel.com/325/255/sports" class="img-responsive" > </div>
							<div class="textos">
								<span class="title">título nota</span>
								<time>80/09/2017</time>
							</div>
						</a>
					</div>
					<div class="lat-entrada">
						<a href="#">
							<div class="imagen"><img src="http://lorempixel.com/325/255/sports" class="img-responsive" > </div>
							<div class="textos">
								<span class="title">título nota</span>
								<time>80/09/2017</time>
							</div>
						</a>
					</div>
					<div class="lat-entrada">
						<a href="#">
							<div class="imagen"><img src="http://lorempixel.com/325/255/sports" class="img-responsive" > </div>
							<div class="textos">
								<span class="title">título nota</span>
								<time>80/09/2017</time>
							</div>
						</a>
					</div>
				--%>
                    </div>
				<h4>
					categorías
				</h4>
					<ul class="lat-categorias">
                        <asp:Panel runat ="server" ID="pnlCat">

                        </asp:Panel>
						<%--<li><a href="#" class="" >noticias</a></li>
						<li><a href="#" class="" >arte</a></li>
						<li><a href="#" class="" >eventos</a></li>
						<li><a href="#" class="" >categoría</a></li>
						<li><a href="#" class="" >notas</a></li>
						<li><a href="#" class="" >categoría</a></li>--%>
					</ul>
				<h4>
					síguenos
				</h4>
			</div>
		</div>
	</div>
</asp:Content>