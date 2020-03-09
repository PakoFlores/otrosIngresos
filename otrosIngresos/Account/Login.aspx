<%@ Page Title="Acceso" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="otrosIngresos.Account.Login" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script type="text/javascript" src="~/Scripts/autoLogoff.js"></script>
    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>
    <!--<section id="loginForm" >
        <h2>Use su cuenta local para ingresar.</h2>
        <asp:Login runat="server" ViewStateMode="Disabled" RenderOuterTable="true" >
            <LayoutTemplate>
                <p class="validation-summary-errors" >
                    <asp:Literal runat="server" ID="FailureText" />
                </p>
                <fieldset>
                    <legend>Log in Form</legend>
                    <ol>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="UserName">Usuario</asp:Label>
                            <asp:TextBox runat="server" ID="UserName" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName" CssClass="field-validation-error" ErrorMessage="Usuario requerido." />
                        </li>
                        <li>
                            <asp:Label runat="server" AssociatedControlID="Password">Contrase&ntilde;a</asp:Label>
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="field-validation-error" ErrorMessage="Contraseña requerida." />
                        </li>
                        <li>
                            <asp:CheckBox runat="server" ID="RememberMe" />
                            <asp:Label runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Mantener iniciada la sesión?</asp:Label>
                        </li>
                    </ol>
                    <asp:Button runat="server" CommandName="Login" Text="Iniciar sesión" />
                </fieldset>
            </LayoutTemplate>
        </asp:Login>
        <p>
            Clic 
            <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">aquí</asp:HyperLink>
            si olvidó su contrase&ntilde;a o no puede ingresar.
        </p>
    </section>
    -->
    <section id="LoginForm2">
        <h2></h2>
        <table>
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtName">Usuario</asp:Label>
                    <asp:TextBox runat="server" ID="txtName" TabIndex="1" Width="300px" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName" CssClass="field-validation-error" ErrorMessage="Usuario requerido." />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="Label2" runat="server" AssociatedControlID="txtPassword">Contrase&ntilde;a</asp:Label>
                    <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" TabIndex="2" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" CssClass="field-validation-error" ErrorMessage="Contraseña requerida." />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CheckBox runat="server" ID="cbRemember" TabIndex="3" />
                    <asp:Label ID="Label3" runat="server" AssociatedControlID="cbRemember" CssClass="checkbox">Mantener iniciada la sesión?</asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <!--<asp:Button ID="LogIn2" runat="server" Text="Iniciar sesión" />-->
                    <asp:Button ID="btnLogin2" runat="server" CommandName="Login2" Text="Iniciar sesión" OnClick="btnLogin2_Click" TabIndex="4" />
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
