<%@ Page Language="VB" AutoEventWireup="false" CodeFile="elegir-clientef.aspx.vb" Inherits="elegir_clientef" %>

<!doctype html>
<html class="no-js" lang="">
    <head>
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

<link type="text/css" rel="Stylesheet" href="styleAuto.css" />

<script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery/jquery-1.5.js"></script> 

     <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.10/jquery-ui.js"></script>

            <style>

       .ui-button { margin-left: -1px; }

       .ui-button-icon-only .ui-button-text { padding: 0.35em; }

       .ui-autocomplete-input { margin: 0; padding: 0.48em 0 0.47em 0.45em; }


    
       </style>

 

 

       <script>

 

 

           function optionSelected(selectedValue) {

              document.title = selectedValue;

           }

 

           (function ($) {

               $.widget("ui.combobox", {

                   _create: function () {

                       var self = this,

                                  select = this.element.hide(),

                                  selected = select.children(":selected"),

                                  value = selected.val() ? selected.text() : "";

                       var input = this.input = $("<input>")

                                  .insertAfter(select)

                                  .val(value)

                                  .autocomplete({

                                      delay: 0,

                                      minLength: 0,

                                      source: function (request, response) {

                                          var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");

                                          response(select.children("option").map(function () {

                                              var text = $(this).text();

                                              if (this.value && (!request.term || matcher.test(text)))

                                                  return {

                                                      label: text.replace(

                                                                           new RegExp(

                                                                                  "(?![^&;]+;)(?!<[^<>]*)(" +

                                                                                  $.ui.autocomplete.escapeRegex(request.term) +

                                                                                  ")(?![^<>]*>)(?![^&;]+;)", "gi"

                                                                           ), "$1"),

                                                      value: text,

                                                      option: this

                                                  };

                                          }));

                                      },

                                      select: function (event, ui) {

                                          ui.item.option.selected = true;

                                          self._trigger("selected", event, {

                                              item: ui.item.option

                                          });

                            //JK

                            optionSelected(ui.item.option.value);

                                         

                                      },

                                      change: function (event, ui) {

                                          if (!ui.item) {

                                              var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),

                                                              valid = false;

                                              select.children("option").each(function () {

                                                  if ($(this).text().match(matcher)) {

                                                      this.selected = valid = true;

                                                      return false;

                                                  }

                                              });

                                              if (!valid) {

                                                  // remove invalid value, as it didn't match anything

                                                  $(this).val("");

                                                  select.val("");

                                                  input.data("autocomplete").term = "";

                                                  return false;

                                              }

                                          }

                                      }

                                  })

                                  .addClass("ui-widget ui-widget-content ui-corner-left");

 

                       input.data("autocomplete")._renderItem = function (ul, item) {

                           return $("<li></li>")

                                         .data("item.autocomplete", item)

                                         .append("<a>" + item.label + "</a>")

                                          .appendTo(ul);

                       };

 

                       this.button = $("<button type='button'>▼</button>")

                                  .attr("tabIndex", -1)

                                  .attr("title", "Mostrar Todos")

                                  .insertAfter(input)

                                  .button({

                                      icons: {

                                          primary: ""

                                      },

                                      text: false

                                  })

                                  .removeClass("ui-corner-all")

                                  .addClass("ui-corner ui-button-icon")

                                  .click(function () {

                                      // close if already visible

                                      if (input.autocomplete("widget").is(":visible")) {

                                          input.autocomplete("close");

                                          return;

                                      }

 

                                      // pass empty string as value to search for, displaying all results

                                      input.autocomplete("search", "");

                                      input.focus();

                                  });

                   },

 

                   destroy: function () {

                       this.input.remove();

                       this.button.remove();

                       this.element.show();

                       $.Widget.prototype.destroy.call(this);

                   }

               });

           })(jQuery);

 

           $(function () {

               $("#<%= ddlClientes.ClientID%>").combobox();

               $("#toggle").click(function () {

                   $("#<%= ddlClientes.ClientID%>").toggle();

               });

           });

    </script>
        

    </head>
    <body class="gtk-2" >
        

        <div class="col-xs-12 col-sm-6">
            <img src="img/header/logo.png" class="img-responsive cent-img">
            <div class="col-xs-12 text-center">
                    <strong class="text-underline">bienvenido</strong>
                    
            </div>
            
                <form runat="server" >
   
                   
<div class="marco-form">
      <asp:Panel ID="pnlClientes" runat="server" Visible ="false">
                     <div class="form-group">
                       
                    <asp:Label ID="lblNombreUsuario" runat="server" Text="Usuario"></asp:Label>
                  </div>
                      <div class="form-group">
                    <asp:Label ID="Label1" runat="server" Text="A continuación selecciona tu cliente"></asp:Label>
                  </div>
          <div>
              <div class="row ">
                  <div class="col-xs-12 ">
                      <div  class="form-group">
                      <div class="row">
                          <div class="col-xs-12" >
                              <asp:Label ID="Label2" runat="server" Text="Buscar por NIT"></asp:Label>
                          </div>
                      </div>

                      <div class="col-xs-12">
                          <asp:TextBox ID="txtNIT" runat="server" Width ="100%"></asp:TextBox>
                      </div>

                          </div>
                  </div>
              </div>
          </div>
          <div class="row ">
                  <div class="col-xs-12 ">
                      <div  class="form-group">
                      <div class="row">
                          <div class="col-xs-12" >
                              <asp:Label ID="Label3" runat="server" Text="Buscar por Nombre"></asp:Label>
                          </div>
                      </div>

                      <div class="col-xs-12">
                          <asp:TextBox ID="txtNombre" runat="server" Width ="100%"></asp:TextBox>
                      </div>

                          </div>
                  </div>
              </div>
                          
          <div class="row ">
              <div class="col-xs-12 ">
                  
                     
                              <asp:Button ID="btnBuscar" runat="server" Text="buscar" class="btn btn-general-7 b-cent"/>
                                   



                  </div>
            
          </div>
                      <br />    
              <div >

                        <asp:DropDownList ID="ddlClientes" runat="server" Visible ="true" Width ="100%"   ></asp:DropDownList>
                  </div>
                 

                  <div class="form-group">
                      
                     <asp:Button ID="btnSeleccionar" runat="server" Text="ingresar" class="btn btn-general-2 b-cent"/>
                  </div>

</asp:Panel>
    </div>
                  <br />
                     <div class="col-xs-12">
                  
                          <asp:Button ID="btnCrear" runat="server" Text="nuevo Cliente" class="btn btn-general-7 b-cent" Visible ="true" />
                       
                     </div> 
                     
          
                </form>
            
        </div>

      
    

    </body>
</html>
