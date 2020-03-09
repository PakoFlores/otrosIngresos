<%@ Page Title="Administrar" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminAgregarActualizar.aspx.cs" 
    Inherits="otrosIngresos.Account.AdminAgregarActualizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!--<title>jQuery UI Datepicker - Default functionality</title>-->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="~/Scripts/jquery-1.12.4.js"></script>
    <script src="~/Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="~/Scripts/autoLogoff.js"></script>

    <script>
        function confirmMsg() {
            return confirm('AVISO: ¿Esta seguro de realizar la alta/actualización del perfil?');
        }
    </script>

    <hgroup class="title">
        <h1><%: Title %> : Agregar/Actualizar Usuario</h1>
    </hgroup>

    <section id="AgregarActualizar" >
        <div dir="ltr">
            <div dir="ltr">
                <asp:Label ID="lbSesion" runat="server" Font-Bold="true" ForeColor=#00395A Font-Size="Smaller" />
            </div>
            <asp:Button runat="server" ID="btnRegresar" Text="Regresar" OnClick="btnRegresar_Click" Font-Size="Smaller" CausesValidation="false" />
        </div>
        <asp:PlaceHolder runat="server" ID="phAgregarActualizar" >
            <asp:UpdatePanel ID="upAgregarActualizar" runat="server">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lbUsuario" runat="server" Text="Usuario:" Font-Bold="true" ForeColor=#00395A Font-Size="Small" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlUsuario" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" 
                                    OnSelectedIndexChanged="ddlUsuario_SelectedIndexChanged" />&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlUsuario" 
                                    CssClass="field-validation-error" ErrorMessage="Usuario requerido." />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbPerfil" runat="server" Text="Perfil:" Font-Bold="true" ForeColor=#00395A Font-Size="Small" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlPerfil" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" />&nbsp;
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlPerfil" 
                                    CssClass="field-validation-error" ErrorMessage="Perfil requerido." />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:PlaceHolder>
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnAgregarActualizar" runat="server" CommandName="AgregarActualizar" Text="Agregar/Actualizar" 
                        OnClick="btnAgregarActualizar_Click" Font-Size="Smaller" OnClientClick="return confirmMsg()" />
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
