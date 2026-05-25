<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Carrito.aspx.cs" Inherits="Moodflix.Carrito" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    


    <h2 id="lblTitulo" runat="server" class="mx-auto">Mi carrito</h2>
    
    <div id="carritoContainer" class="container" runat="server">
        <%--<h2 id="tituloCarrito" runat="server" class="mx-auto">Mi carrito</h2>                --%>
        <!-- Título principal -->
        <%--<h2>
            <span runat="server" id="lblTitulo"> </span>
        </h2>--%>
    </div>
    
    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="False">
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </asp:Panel>
    
    
    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPersonales" runat="server">
</asp:Content>
