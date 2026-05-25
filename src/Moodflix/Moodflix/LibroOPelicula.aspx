<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LibroOPelicula.aspx.cs" Inherits="Moodflix.LibroOPelicula" %>

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
    <link rel="stylesheet" type="text/css" href="Estilos/Style.css"/>
</head>

<body class="background-emociones">
    <form id="form1" runat="server">

        <!-- ============================= -->
        <!-- BARRA DE NAVEGACIÓN SUPERIOR -->
        <!-- ============================= -->
        <nav class="navbar navbar-expand-lg navbar-dark">
            <div class="container-fluid">

                <div class="navbar-nav ms-auto">

                    <!-- Usuario anónimo -->
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


                    <!-- Usuario logueado -->
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

                        <!-- Herramientas -->
                        <asp:PlaceHolder ID="PlantillaHerramientas" runat="server">
                            <asp:DropDownList ID="ddlHerramientas"
                            runat="server"
                            AutoPostBack="true"
                            OnSelectedIndexChanged="ddlActions_OnSelectedIndexChanged">
                        </asp:DropDownList>

                        </asp:PlaceHolder>

                        <!-- Logout -->
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
        <main class="container">
            <div class="row align-items-center">

                <!-- Imagen principal -->
                <section class="col-12 col-xl-6">
                    <asp:Image ID="homeImage"
                        ImageUrl="Imagenes/moodflix-02.png"
                        Width="477px"
                        runat="server" />
                </section>

                <!-- Botones -->
                <aside class="col-12 col-xl-6">

                    <div class="row">
                        <div class="col-6">
                            <asp:ImageButton ID="imgbVerPeliculas"
                                ImageUrl="Imagenes/Peliculas.png"
                                Width="326px"
                                OnClick="imgbVerPeliculas_OnClick"
                                runat="server" />
                        </div>

                        <div class="col-6">
                            <asp:ImageButton ID="imgbVerLibros"
                                ImageUrl="Imagenes/Libros.png"
                                Width="326px"
                                OnClick="imgbVerLibros_OnClick"
                                runat="server" />
                        </div>
                    </div>

                    <div class="row justify-content-center mt-3">
                        <asp:ImageButton ID="imgbVerTodo"
                            ImageUrl="Imagenes/VerTodo.png"
                            Width="326px"
                            OnClick="imgbVerTodo_OnClick"
                            runat="server" />
                    </div>

                </aside>
            </div>
        </main>

    </form>

    <!-- Bootstrap -->
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/bootstrap.js") %>
    </asp:PlaceHolder>

</body>
</html>
