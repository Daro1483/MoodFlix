<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Backup.aspx.cs" Inherits="Moodflix.Backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container mt-5">

        <!-- Título principal -->
        <h2>
            <span runat="server" id="lblTitulo"></span>
        </h2>

        <!-- Generar Backup Section -->
        <div class="border p-4 mb-4 bg-white rounded">
            <h4>
                <span runat="server" id="lblGenerarBackup"></span>
            </h4>

            <div class="form-group row mt-3">
                <div class="col-sm-12">

                    <asp:Button 
                        ID="btnGenerarBackup" 
                        runat="server" 
                        CssClass="btn btn-primary"
                        OnClick="btnGenerarBackup_OnClick" />
                    
                </div>
            </div>
        </div>

        <!-- Hacer Restore Section -->
        <div class="border p-4 mb-4 bg-white rounded">

            <h4>
                <span runat="server" id="lblHacerRestore"></span>
            </h4>

            <div class="form-group row mt-3">

                <label for="txtRutaRestore" class="col-sm-2 col-form-label">
                    <span runat="server" id="lblRutaRestore"></span>
                </label>

                <div class="col-sm-6">
                    <asp:TextBox 
                        ID="txtRutaRestore" 
                        runat="server" 
                        CssClass="form-control readonly" 
                        ReadOnly="true"></asp:TextBox>
                </div>

                <div class="col-sm-2">
                    <asp:FileUpload ID="FileUploadRestore" runat="server" CssClass="d-none" />

                    <button type="button" class="btn btn-secondary"
                            onclick="document.getElementById('<%= FileUploadRestore.ClientID %>').click()">

                        <span runat="server" id="lblExaminar"></span>

                    </button>
                </div>

                <div class="col-sm-2">
                    <asp:Button 
                        ID="btnComenzarRestore" 
                        runat="server"
                        CssClass="btn btn-primary"
                        OnClick="btnComenzarRestore_OnClick" />
                </div>
            </div>

            <div class="form-group row mt-2">
                <div class="col-sm-12">
                    <asp:Label runat="server" ID="lblError" CssClass="text-danger"></asp:Label>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPersonales" runat="server">

    <script type="text/javascript">
        document.getElementById("<%= FileUploadRestore.ClientID %>").addEventListener("change", function (event) {
            var input = event.target;
            if (input.files.length > 0) {
                var filePath = input.files[0].name;
                document.getElementById("<%= txtRutaRestore.ClientID %>").value = filePath;
            }
        });
    </script>

</asp:Content>


