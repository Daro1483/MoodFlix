<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABM.aspx.cs" Inherits="Moodflix.ABM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    
    
    <div class="container mt-5">
            <%--<h2>Administrar Películas</h2>--%>
            <h2 id="lblTituloPelicula" runat="server" class="mx-auto">Administrar Películas</h2>
            <!-- Campos para Película -->
            <div class="mb-3">
                <asp:label ID="lblNombrePelicula" runat="server" Text="Nombre"></asp:label>
                <asp:TextBox ID="txtNombrePelicula" runat="server" CssClass="form-control" ValidationGroup="Peliculas"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombrePelicula" runat="server" ControlToValidate="txtNombrePelicula" ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <%--<label for="txtDescripcionPelicula">Descripción</label>--%>
                <asp:label ID="lblDescripcionPelicula" runat="server" text="Descripción"></asp:label>
                <asp:TextBox ID="txtDescripcionPelicula" runat="server" CssClass="form-control" ValidationGroup="Peliculas"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescripcionPelicula" runat="server" ControlToValidate="txtDescripcionPelicula" ErrorMessage="La descripción es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <%--<label>Fecha</label>--%>
                <asp:label ID="lblFechaPelicula" runat="server" Text="Fecha"></asp:label>
                <input type="date" id="txtFechaPelicula" runat="server" class="form-control" validationgroup="Peliculas" />
                <%--<asp:TextBox ID="txtFechaPelicula" runat="server" TextMode="DateTime" CssClass="form-control"></asp:TextBox>--%>
                <asp:RequiredFieldValidator ID="rfvFechaPelicula" runat="server" ControlToValidate="txtFechaPelicula" ErrorMessage="La fecha es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <%--<label>Precio</label>--%>
                <asp:Label ID="lblPrecioPelicula" runat="server" Text="Precio"></asp:Label>
                <asp:TextBox ID="txtPrecioPelicula" runat="server" CssClass="form-control" ValidationGroup="Peliculas"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPrecioPelicula" runat="server" ControlToValidate="txtPrecioPelicula" ErrorMessage="El precio es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <%--<label>URI</label>--%>
                <label>URI</label>
                <asp:TextBox ID="txtUriPelicula" runat="server" CssClass="form-control" ValidationGroup="Peliculas"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUriPelicula" runat="server" ControlToValidate="txtUriPelicula" ErrorMessage="El URI es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <%--<label>Emoción</label>--%>
                <asp:label ID="lblEmocionPelicula" runat="server" Text="Emoción"></asp:label>
                <asp:DropDownList ID="ddlEmocionPelicula" runat="server" CssClass="form-control" ValidationGroup="Peliculas">
                    
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvEmocionPelicula" runat="server" ControlToValidate="ddlEmocionPelicula" InitialValue="" ErrorMessage="La emoción es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />
            </div>
            <div class="mb-3">
               <%-- <label>Género</label>--%>
                <asp:Label ID="lblGeneroPelicula" runat="server" Text="Género"></asp:Label>
                <asp:TextBox ID="txtGenero" runat="server" CssClass="form-control" ValidationGroup="Peliculas"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvGenero" runat="server" ControlToValidate="txtGenero" ErrorMessage="El género es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <%--<label>Director</label>--%>
                <asp:Label ID="lblDirectorPelicula" runat="server" Text="Director"></asp:Label>
                <asp:TextBox ID="txtDirector" runat="server" CssClass="form-control" ValidationGroup="Peliculas"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDirector" runat="server" ControlToValidate="txtDirector" ErrorMessage="El director es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Peliculas" />

            </div>
            <div class="mb-3">
                <asp:Button ID="btnAgregarPelicula" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregarPelicula_OnClick" ValidationGroup="Peliculas" />
                <asp:Button ID="btnActualizarPelicula" runat="server" Text="Actualizar" CssClass="btn btn-warning" OnClick="btnActualizarPelicula_OnClick" ValidationGroup="Peliculas" />
                <asp:Button ID="btnEliminarPelicula" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminarPelicula_OnClick" ValidationGroup="Peliculas" />
            </div>
            <asp:GridView ID="gvPeliculas" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" OnSelectedIndexChanged="gvPeliculas_OnSelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" />
                    <asp:BoundField DataField="Uri" HeaderText="URI" />
                    <asp:BoundField DataField="Emocion" HeaderText="Emoción" />
                    <asp:BoundField DataField="Genero" HeaderText="Género" />
                    <asp:BoundField DataField="Director" HeaderText="Director" />
                    <asp:CommandField ShowSelectButton="true"/>
                </Columns>
            </asp:GridView>

            <%--<h2 class="mt-5">Administrar Libros</h2>--%>
            <h2 ID="lblTituloLibro" runat="server" class="mx-auto">Administrar libros</h2>    
        <!-- Campos para Libro -->
            <div class="mb-3">
                <asp:label ID="lblNombreLibro" runat="server" Text="Nombre"></asp:label>
                <asp:TextBox ID="txtNombreLibro" runat="server" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNombreLibro" runat="server" ControlToValidate="txtNombreLibro" ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:label ID="lblDescripcionLibro" runat="server" Text="Descripción"></asp:label>
                <asp:TextBox ID="txtDescripcionLibro" runat="server" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvDescripcionLibro" runat="server" ControlToValidate="txtDescripcionLibro" ErrorMessage="La descripción es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:label ID="lblFechaLibro" runat="server" Text="Fecha"></asp:label>
                <asp:TextBox ID="txtFechaLibro" runat="server" TextMode="DateTime" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFechaLibro" runat="server" ControlToValidate="txtFechaLibro" ErrorMessage="La fecha es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:label ID="lblPrecioLibro" runat="server" Text="Precio"></asp:label>
                <asp:TextBox ID="txtPrecioLibro" runat="server" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPrecioLibro" runat="server" ControlToValidate="txtPrecioLibro" ErrorMessage="El precio es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <label>URI</label>
                <asp:TextBox ID="txtUriLibro" runat="server" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUriLibro" runat="server" ControlToValidate="txtUriLibro" ErrorMessage="El URI es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:label ID="lblEmocionLibro" runat="server" Text="Emoción"></asp:label>
                <asp:DropDownList ID="ddlEmocionLibro" runat="server" CssClass="form-control" ValidationGroup="Libros">
                   
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="rfvEmocionLibro" runat="server" ControlToValidate="ddlEmocionLibro" InitialValue="" ErrorMessage="La emoción es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:label ID="lblAutorLibro" runat="server" Text="Autor"></asp:label>
                <asp:TextBox ID="txtAutor" runat="server" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAutor" runat="server" ControlToValidate="txtAutor" ErrorMessage="El autor es obligatorio." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:label ID="lblEditorialLibro" runat="server" Text="Editorial"></asp:label>
                <asp:TextBox ID="txtEditorial" runat="server" CssClass="form-control" ValidationGroup="Libros"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEditorial" runat="server" ControlToValidate="txtEditorial" ErrorMessage="La editorial es obligatoria." CssClass="text-danger" Display="Dynamic" ValidationGroup="Libros" />

            </div>
            <div class="mb-3">
                <asp:Button ID="btnAgregarLibro" runat="server" Text="Agregar" CssClass="btn btn-primary" OnClick="btnAgregarLibro_OnClick" ValidationGroup="Libros" />
                <asp:Button ID="btnActualizarLibro" runat="server" Text="Actualizar" CssClass="btn btn-warning" OnClick="btnActualizarLibro_OnClick" ValidationGroup="Libros" />
                <asp:Button ID="btnEliminarLibro" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminarLibro_OnClick" ValidationGroup="Libros" />
            </div>
            <asp:GridView ID="gvLibros" runat="server" CssClass="table table-striped" AutoGenerateColumns="false" OnSelectedIndexChanged="gvLibros_OnSelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" />
                    <asp:BoundField DataField="Uri" HeaderText="URI" />
                    <asp:BoundField DataField="Emocion" HeaderText="Emoción" />
                    <asp:BoundField DataField="Autor" HeaderText="Autor" />
                    <asp:BoundField DataField="Editorial" HeaderText="Editorial" />
                    <asp:CommandField ShowSelectButton="true"/>
                </Columns>
            </asp:GridView>
        </div>
           
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPersonales" runat="server">
</asp:Content>
