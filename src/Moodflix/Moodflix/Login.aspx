    <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Moodflix.Login" %>

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
            <nav class="navbar navbar-expand-lg navbar-dark">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">
                <asp:ImageButton ID="ImageButton1" CssClass="logo" ImageUrl="Imagenes/moodflix-07.png" Width="200px"
                    OnClick="ImageButton1_OnClick" runat="server" CausesValidation="False" />
            </a>

            <div class="d-flex align-items-center ms-auto">
                <asp:LinkButton CssClass="nav-link me-3" ID="LinkInitRegistro" Text="Registrarse"
                    OnClick="LinkInitRegistro_OnClick" runat="server" CausesValidation="False">
                </asp:LinkButton>

                <asp:DropDownList ID="ddlIdiomas" runat="server"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlIdiomas_SelectedIndexChanged"
                CssClass="form-select form-select-sm w-auto">
                <asp:ListItem Text="Español" Value="ES" />
                 <asp:ListItem Text="English" Value="EN" />
                </asp:DropDownList>
            </div>
        </div>
    </nav>
            <!-- CONTENT -->
        
        
        

            <main >
                <div class="container  rounded shadow p-3 w-25 my-5">
                    <div class=" row">
                        <label id="lblEmail" class="form-label" runat="server">Correo electrónico</label>
                    </div>
                    <div class=" mx-1 mb-3 row ">
                        <input type="email" id="txtEmail" class="form-control inputTextBox" runat="server" required="required"/>
                    


                    </div>


                
                    <div class=" row">
                        <label  id="lblContraseña" runat="server" class="form-label col">Contraseña</label>
                    </div>
                    <div class=" mx-1 mb-3 row">
                            <input type="password" id="txtPassword" class="form-control inputTextBox" runat="server" required="required"/>
                        
                    </div>

                
                    <div class="row mx-5 my-3">

                            <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" OnClick="Button1_OnClick" Text="Ingresar" />

                    
                    </div>
                    <div class="row my-1">
                        <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="alert alert-danger" Visible="false">
                            <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
                        </asp:Panel>
                    </div>
                
                </div>

            </main>
        
            <!-- MODAL -->
            <div class="modal fade" id="modalInconsistenciaBD" tabindex="-1" role="dialog" aria-labelledby="modalInconsistenciaBDLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalInconsistenciaBDLabel">¡ADVERTENCIA! La base de datos se encuentra en un estado inconsistente.</h5>
                        </div>
                        <div class="modal-body">
                            <asp:Label ID="lblInformacionRegistros" runat="server"></asp:Label>
                            <asp:Label ID="lblInformacionColumnas" runat="server"></asp:Label>
                            <p>Seleccione una opción:</p>
                            <asp:Button ID="btnRecomponerDV" runat="server" CssClass="btn btn-primary mb-2" Text="Recomponer el dígito verificador" OnClick="btnRecomponerDV_Click" />
                            <asp:Button ID="btnRestaurarBackup" runat="server" CssClass="btn btn-secondary" Text="Restaurar desde un backup" OnClick="btnRestaurarBackup_Click" />
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Salir</button>
                        </div>
                    </div>
                </div>
            </div>
        

            <asp:ScriptManager ID="ScriptManager1" runat="server">
                <Scripts>
                    <asp:ScriptReference Path="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js" />
                    <asp:ScriptReference Path="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js" />
                </Scripts>
            </asp:ScriptManager>
        

        </form>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/Scripts/bootstrap.js") %>
    </asp:PlaceHolder>



    </body>
    </html>
