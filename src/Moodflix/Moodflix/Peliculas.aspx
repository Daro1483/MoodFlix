<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Peliculas.aspx.cs" Inherits="Moodflix._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>       
        <div class="row my-4">
            <h3>
                <span id="txtEmocion" runat="server"></span>
                <span class="emocion_seleccionada p-1" ID="lblEmocion" runat="server"></span>
            </h3>
        </div>
        <div class=" row" runat="server" id="cardsContainer">                        
        </div>                
    </main>
</asp:Content>
