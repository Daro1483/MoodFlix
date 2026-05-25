<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Bitacora.aspx.cs" Inherits="Moodflix.Bitacora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">

        <!-- TITULO -->
        <h2>
            <span runat="server" id="lblTitulo"></span>
        </h2>

        <!-- ============================= -->
        <!--     FILTROS - UNA SOLA FILA   -->
        <!-- ============================= -->
        <div class="row g-3 mb-4">

            <!-- FECHA INICIO -->
            <div class="col-md-3">
                <label for="txtFechaHoraInicio" class="form-label">
                    <span id="lblFechaHoraInicio" runat="server"></span>
                </label>
                <input type="datetime-local" id="txtFechaHoraInicio" runat="server" class="form-control" />
            </div>

            <!-- FECHA FIN -->
            <div class="col-md-3">
                <label for="txtFechaHoraFin" class="form-label">
                    <span id="lblFechaHoraFin" runat="server"></span>
                </label>
                <input type="datetime-local" id="txtFechaHoraFin" runat="server" class="form-control" />
            </div>

            <!-- USUARIO -->
            <div class="col-md-2">
                <label for="ddlUsuariosFiltro" class="form-label">
                    <span id="lblUsuarioFiltro" runat="server"></span>
                </label>
                <asp:DropDownList ID="ddlUsuariosFiltro" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <!-- MODULO -->
            <div class="col-md-2">
                <label for="ddlModulosFiltro" class="form-label">
                    <span id="lblModuloFiltro" runat="server"></span>
                </label>
                <asp:DropDownList ID="ddlModulosFiltro" runat="server" CssClass="form-select"></asp:DropDownList>
            </div>

            <!-- BOTONES -->
            <div class="col-md-2 d-flex align-items-end">

                <div class="d-flex flex-column w-100 gap-2">
                    <asp:Button ID="btnFiltrar" runat="server"
                        CssClass="btn btn-primary w-100"
                        Text="Filtrar"
                        OnClick="btnFiltrar_OnClick" />

                    <asp:Button ID="btnLimpiar" runat="server"
                        CssClass="btn btn-success w-100"
                        Text="Limpiar filtros"
                        OnClick="btnLimpiar_OnClick" />
                </div>

            </div>

        </div>

        <!-- GRID BITACORA -->
        <asp:GridView ID="gvBitacora" runat="server" AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped">

            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" />
                <asp:BoundField DataField="FECHA" HeaderText="FECHA" />
                <asp:BoundField DataField="User" HeaderText="EMAIL USUARIO" />
                <asp:BoundField DataField="MODULO" HeaderText="MODULO" />
                <asp:BoundField DataField="OPERACION" HeaderText="OPERACION" />
            </Columns>

        </asp:GridView>

    </div>

</asp:Content>
