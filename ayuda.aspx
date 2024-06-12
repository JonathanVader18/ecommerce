<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ayuda.aspx.vb" Inherits="ayuda" MasterPageFile ="~/Main.master"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="main-container">
		<div class="row">
            <div class="col-xs-12 col-sm-2">
    <asp:Panel ID="pnlMenu" runat="server"></asp:Panel>            
            </div>
            <div class="col-xs-12 col-sm-10 stl-1-p">
                <asp:Panel ID="pnlContenido" runat="server"></asp:Panel>
            </div>
        </div>
        </div>
    
</asp:Content>