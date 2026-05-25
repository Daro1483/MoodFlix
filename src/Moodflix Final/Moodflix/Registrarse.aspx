<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registrarse.aspx.cs" Inherits="Moodflix.Registrarse" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - My ASP.NET Application</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="Estilos/Style.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <!-- NAVBAR -->
        <nav class="navbar navbar-expand-lg navbar-dark ">
            <div class="container-fluid">
                <a class="navbar-brand" href="#">
                    <asp:ImageButton ID="ImageButton1" CssClass="logo" ImageUrl="Imagenes/moodflix-07.png" Width="200px"  runat="server" />
                </a>
                <div class="navbar-nav ms-auto">
                    <asp:LinkButton CssClass="nav-link" ID="LinkLogin" Text="Log in" OnClick="LinkLogin_OnClick" runat="server"> </asp:LinkButton>
                </div>
            </div>
        </nav>
        <!-- CONTENT -->
        
        
        

        <main >
            <div class="container  rounded shadow p-3 w-25 my-5">
                <div class="row">
                    <h3>Registrarse</h3>
                </div>
                <div class=" row">
                    <label class="form-label">Nombre de usuario</label>
                </div>
                <div class=" mx-1 mb-3 row ">
   
                    <asp:TextBox ID="txtUsername" CssClass="form-control inputTextBox" runat="server"></asp:TextBox>


                </div>
                <div class=" row">
                    <label class="form-label">Email</label>
                </div>
                <div class=" mx-1 mb-3 row ">
   
                    <asp:TextBox ID="txtEmail" TextMode="Email" CssClass="form-control inputTextBox" runat="server"></asp:TextBox>


                </div>


                
                <div class=" row">
                    <label  class="form-label col">Contraseña</label>
                </div>
                <div class=" mx-1 mb-3 row">
                    <asp:TextBox ID="txtPassword" TextMode="Password" CssClass="form-control inputTextBox" runat="server"></asp:TextBox>
                </div>
                
                
                <div class=" row">
                    <label  class="form-label col">Reintroducir Contraseña</label>
                </div>
                <div class=" mx-1 mb-3 row">
                    <asp:TextBox ID="TextBox1" TextMode="Password" CssClass="form-control inputTextBox" runat="server"></asp:TextBox>
                </div>

                
                <div class="row mx-5 my-3">

                    <asp:Button ID="btnRegistrarse" CssClass="btn btn-primary" runat="server" OnClick="btnRegistrarse_OnClick" Text="Ingresar" />

                    
                </div>
                
                <div class="row my-1">
                    <asp:Panel ID="pnlError" runat="server" CssClass="alert alert-danger" Visible="False">
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </asp:Panel>
                </div>
                

                
            </div>

        </main>
    </form>
</body>
</html>
