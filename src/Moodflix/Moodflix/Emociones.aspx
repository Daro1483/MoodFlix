<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Emociones.aspx.cs" Inherits="Moodflix.Emociones" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - My ASP.NET Application</title>

    <!-- Modernizr -->
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <!-- CSS principal -->
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link rel="stylesheet" type="text/css" href="Estilos/Style.css" />
</head>

<body class="background-emociones">
    <form id="form1" runat="server">

        <!-- ============================= -->
        <!-- BARRA DE NAVEGACIÓN SUPERIOR -->
        <!-- ============================= -->
        <nav class="navbar navbar-expand-lg navbar-dark">
            <div class="container-fluid">

                <div class="navbar-nav ms-auto">

                    <!-- Usuario Anónimo -->
                    <asp:PlaceHolder ID="PlantillaUserAnonimo" runat="server">
                        <asp:LinkButton CssClass="nav-link"
                            ID="linkInitLogin"
                            Text="Inicio de sesión"
                            OnClick="linkInitLogin_OnClick"
                            runat="server" />

                        <asp:LinkButton CssClass="nav-link"
                            ID="LinkInitRegistro"
                            Text="Registrarse"
                            OnClick="LinkInitRegistro_OnClick"
                            runat="server" />
                    </asp:PlaceHolder>


                    <!-- Usuario Logueado -->
                    <asp:PlaceHolder ID="PlantillaUserRegistrado" runat="server">

                        <!-- Nombre del usuario -->
                        <asp:LinkButton CssClass="nav-link"
                            ID="LinkProfile"
                            runat="server" />

                        <!-- Idiomas -->
                        <asp:PlaceHolder ID="PlantillaIdioma" runat="server">
                            <asp:DropDownList ID="ddlIdiomas"
                                runat="server"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlIdiomas_SelectedIndexChanged"
                                CssClass="form-select form-select-sm w-auto ms-2 me-2">
                                <asp:ListItem Text="Español" Value="ES" />
                                <asp:ListItem Text="English" Value="EN" />
                            </asp:DropDownList>
                        </asp:PlaceHolder>

                        <!-- Herramientas Unificadas -->
                        <asp:PlaceHolder ID="PlantillaHerramientas" runat="server">
                            <asp:DropDownList ID="ddlHerramientas"
                                runat="server"
                                CssClass="ddlHerramientas"
                                AutoPostBack="true"
                                OnSelectedIndexChanged="ddlActions_OnSelectedIndexChanged" />
                        </asp:PlaceHolder>

                        <!-- Cerrar Sesión -->
                        <asp:LinkButton CssClass="nav-link"
                            ID="LinkLogout"
                            Text="Cierre de sesión"
                            OnClick="LinkLogout_OnClick"
                            runat="server" />

                    </asp:PlaceHolder>

                </div>
            </div>
        </nav>


        <!-- ============================= -->
        <!-- CONTENIDO PRINCIPAL           -->
        <!-- ============================= -->
        <div class="container justify-content-center">
            <div class="row">

                <!-- LOGO -->
                <div class="col col-sm-6">
                    <asp:ImageButton runat="server"
                        CssClass="logo-emociones"
                        ImageUrl="Imagenes/moodflix-02.png"
                        OnClick="OnClick" />
                </div>

                <!-- TÍTULO PRINCIPAL -->
                <div class="col col-sm-6 align-content-center">
                    <h1>
                        <asp:Label ID="lblTitulo"
                            runat="server"
                            Text="Elegí una emoción para empezar:" />
                    </h1>
                </div>

            </div>

            <!-- CONTENEDOR DE EMOCIONES -->
            <div class="row mt-2" id="emotionsContainer" runat="server"></div>
        </div>

    </form>

    <!-- Scripts Bootstrap -->
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/bootstrap.js") %>
    </asp:PlaceHolder>

</body>
</html>
