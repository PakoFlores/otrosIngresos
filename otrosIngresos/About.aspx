<%@ Page Title="Aerca de" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="otrosIngresos.About" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <article>
        <h2>Finanzas - Otros Ingresos</h2>
        <p>v1.0</p>
        <table>
            <tr>
                <td>
                    Finanzas:
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <p>Control de Publicidad</p>
                    <p>Destinos</p>
                    <p>Marketing Deal</p>
                    <p>PPA</p>
                    <p>Outlet</p>
                </td>
            </tr>
        </table>
        <p>Team BI</p>
        <p>Copyright © <%= DateTime.Now.Year %><br>
        PriceTravel, todos los derechos reservados.
        </p>
    </article>

    <!--<aside>
        <h3>Aside Title</h3>
        <p>Use this area to provide additional information.</p>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About">About</a></li>
            <li><a runat="server" href="~/Contact">Contact</a></li>
        </ul>
    </aside>-->
</asp:Content>