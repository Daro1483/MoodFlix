<%@ Page Title="Sin Permisos"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="SinPermisos.aspx.cs"
    Inherits="Moodflix.SinPermisos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-5">

        <!-- TITULO -->
        <h2 id="lblTitulo" runat="server">No tiene permisos para acceder</h2>

        <!-- MENSAJES -->
        <p id="lblMensaje1" runat="server">
            No tiene permisos para acceder a esta sección del sistema.
        </p>

        <p id="lblMensaje2" runat="server">
            Si considera que se trata de un error, por favor contacte al webmaster.
        </p>

        <!-- BOTÓN VOLVER -->
        <asp:Button 
            ID="btnVolver" 
            runat="server" 
            Text="Volver" 
            CssClass="btn btn-primary"
            OnClick="btnVolver_Click" />

    </div>

</asp:Content>
