<%@ Page Title="Gestión de Perfiles" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="Moodflix.Perfiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-5">

        <!-- TITULO -->
        <h2 id="tituloPagina" runat="server" class="text-center mb-4">
            Gestión de Perfiles
        </h2>

        <!-- ========================================================= -->
        <!-- ==========   BLOQUE SUPERIOR: GESTIÓN DE PERFILES   ===== -->
        <!-- ========================================================= -->

        <div class="row">

            <!-- ROLES DISPONIBLES -->
            <div class="col-md-4">

                <asp:Label ID="lblRolesDisponibles" runat="server"
                    CssClass="form-label"
                    Text="Roles disponibles"></asp:Label>

                <asp:ListBox ID="lstRolesDisponibles" runat="server"
                    CssClass="form-control" Height="250px"></asp:ListBox>

                <asp:Label ID="lblNuevaFamilia" runat="server"
                    CssClass="form-label mt-3"
                    Text="Nombre nueva familia"></asp:Label>

                <asp:TextBox ID="txtNuevaFamilia" runat="server"
                    CssClass="form-control"></asp:TextBox>

                <asp:Button ID="btnCrearFamilia" runat="server"
                    Text="Crear familia"
                    CssClass="btn btn-primary mt-3 w-100"
                    OnClick="btnCrearFamilia_Click" />

               <asp:Button ID="btnEliminarFamilia" runat="server" 
                    Text="Eliminar familia" 
                    CssClass="btn btn-danger mt-3 w-100"          
                    OnClick="btnEliminarFamilia_Click" />



            </div>


            <!-- FAMILIAS -->
            <div class="col-md-4">

                <asp:Label ID="lblFamilias" runat="server"
                    CssClass="form-label"
                    Text="Familias"></asp:Label>

                <asp:ListBox ID="lstFamilias" runat="server"
                    CssClass="form-control"
                    Height="250px"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="lstFamilias_SelectedIndexChanged"></asp:ListBox>

            </div>


            <!-- ROLES DE LA FAMILIA -->
            <div class="col-md-4">

                <asp:Label ID="lblRolesFamilia" runat="server"
                    CssClass="form-label"
                    Text="Roles de la familia seleccionada"></asp:Label>

                <asp:ListBox ID="lstRolesFamilia" runat="server"
                    CssClass="form-control" Height="250px"></asp:ListBox>

                <div class="d-flex justify-content-between mt-3">

                    <asp:Button ID="btnAgregarRol" runat="server"
                        Text="Agregar rol"
                        CssClass="btn btn-primary w-50 me-2"
                        OnClick="btnAgregarRol_Click" />

                    <asp:Button ID="btnQuitarRol" runat="server"
                        Text="Quitar rol"
                        CssClass="btn btn-secondary w-50"
                        OnClick="btnQuitarRol_Click" />

                </div>

            </div>

        </div>

        <!-- ========================================================= -->
        <!-- == BLOQUE INFERIOR: ASIGNACIÓN DE FAMILIAS A USUARIOS == -->
        <!-- ========================================================= -->

        <hr class="my-5" />

        <div class="card shadow-sm">

            <div class="card-header bg-light">
                <h4 class="mt-2">Asignar Familias a Usuarios</h4>
            </div>

            <div class="card-body">

                <!-- SELECCIÓN DE USUARIO -->
                <asp:Label ID="lblSeleccionUsuario" runat="server"
                    CssClass="form-label"
                    Text="Seleccionar usuario"></asp:Label>

                <asp:DropDownList ID="ddlUsuarios" runat="server"
                    CssClass="form-select mb-4"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged">
                </asp:DropDownList>

                <div class="row">

                    <!-- FAMILIAS DISPONIBLES -->
                    <div class="col-md-6">

                        <asp:Label ID="lblFamiliasDisponibles" runat="server"
                            CssClass="form-label"
                            Text="Familias disponibles"></asp:Label>

                        <asp:ListBox ID="lstFamiliasDisponibles" runat="server"
                            CssClass="form-control" Height="200px"></asp:ListBox>

                        <asp:Button ID="btnAsignarFamilia" runat="server"
                            Text="Asignar familia"
                            CssClass="btn btn-primary mt-3 w-100"
                            OnClick="btnAsignarFamilia_Click" />

                    </div>

                    <!-- FAMILIAS ASIGNADAS -->
                    <div class="col-md-6">

                        <asp:Label ID="lblFamiliasUsuario" runat="server"
                            CssClass="form-label"
                            Text="Familias asignadas al usuario"></asp:Label>

                        <asp:ListBox ID="lstFamiliasUsuario" runat="server"
                            CssClass="form-control" Height="200px"></asp:ListBox>

                        <asp:Button ID="btnQuitarFamilia" runat="server"
                            Text="Quitar familia"
                            CssClass="btn btn-secondary mt-3 w-100"
                            OnClick="btnQuitarFamilia_Click" />

                    </div>

                </div>

            </div>

        </div>

    </div>

</asp:Content>
