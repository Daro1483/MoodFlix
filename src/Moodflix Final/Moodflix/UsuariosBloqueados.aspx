<%@ Page Title="Usuarios bloqueados"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="UsuariosBloqueados.aspx.cs"
    Inherits="Moodflix.UsuariosBloqueados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-4">
        <h3 id="titulo" runat="server">Administración de usuarios bloqueados</h3>

        <asp:GridView ID="gvUsuarios" runat="server"
              AutoGenerateColumns="False"
              CssClass="table table-striped table-bordered mt-3"
              OnRowCommand="gvUsuarios_RowCommand"
              OnRowDataBound="gvUsuarios_RowDataBound">

    <Columns>

        <asp:BoundField DataField="IDUSUARIO" HeaderText="ID" />

        <asp:BoundField DataField="USERNAME" HeaderText="Usuario" />

        <asp:BoundField DataField="EMAIL" HeaderText="Email" />

        <asp:BoundField DataField="INTENTOS" HeaderText="Intentos" />

        <asp:CheckBoxField DataField="BLOQUEADO" HeaderText="Bloqueado" />

        <asp:TemplateField HeaderText="Acciones">
            <ItemTemplate>

                <asp:Button ID="btnBloquear" runat="server"
                            Text="Bloquear"
                            CssClass="btn btn-danger btn-sm"
                            CommandName="Bloquear"
                            CommandArgument='<%# Eval("IDUSUARIO") %>' />

                <asp:Button ID="btnDesbloquear" runat="server"
                            Text="Desbloquear"
                            CssClass="btn btn-success btn-sm ms-1"
                            CommandName="Desbloquear"
                            CommandArgument='<%# Eval("IDUSUARIO") %>' />

            </ItemTemplate>
        </asp:TemplateField>

    </Columns>

</asp:GridView>

    </div>

</asp:Content>
