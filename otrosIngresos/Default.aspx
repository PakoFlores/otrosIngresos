<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="otrosIngresos._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">

</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script type="text/javascript" src="~/Scripts/autoLogoff.js"></script>

    <h2>Finanzas - Otros Ingresos</h2><br />
    <div>
        <asp:PlaceHolder runat="server" ID="phMenuPrincipal" >
            <asp:Menu ID="mnPrincipal" runat="server" Orientation="Vertical">
                <Items>
                    <asp:MenuItem Text="&nbsp"/>
                    <asp:MenuItem Text="&nbsp&nbspControl de Publicidad&nbsp&nbsp" Value="item1" ImageUrl="~/Images/calendar.gif" Selectable="false" >
                        <asp:MenuItem Text="&nbspAgregar/Generar Acuerdo&nbsp" Value="subItem1" NavigateUrl="~/WebForms/ControlPublicidad/controlPublicidad" />
                        <asp:MenuItem Text="&nbspConsultar&nbsp" Value="subItem2" NavigateUrl="~/WebForms/ControlPublicidad/controlPublicidadConsulta" />
                        <asp:MenuItem Text="&nbspConsultar Destinos&nbsp" Value="subItem3" NavigateUrl="~/WebForms/Destinos/destinosConsulta" />
                    </asp:MenuItem>
                    <asp:MenuItem Text="&nbsp"/>
                    <asp:MenuItem Text="&nbsp&nbspMarketing Deal&nbsp&nbsp" Value="item3" ImageUrl="~/Images/visits.gif" Selectable="false" >
                        <asp:MenuItem Text="&nbspAgregar Marketing Deal&nbsp" Value="subItem1" NavigateUrl="~/WebForms/MarketingDeal/marketingDeal" />
                        <asp:MenuItem Text="&nbspConsultar&nbsp" Value="subItem1" NavigateUrl="~/WebForms/MarketingDeal/MarketingDealConsulta" />
                    </asp:MenuItem>
                    <asp:MenuItem Text="&nbsp"/>
                    <asp:MenuItem Text="&nbsp&nbspOutlet&nbsp&nbsp" Value="item4" ImageUrl="~/Images/sales.gif" Selectable="false" >
                        <asp:MenuItem Text="&nbspAgregar registro Outlet&nbsp" Value="subItem1" NavigateUrl="~/WebForms/Outlet/Outlet" />
                        <asp:MenuItem Text="&nbspConsultar&nbsp" Value="subItem1" NavigateUrl="~/WebForms/Outlet/OutletConsulta" />
                    </asp:MenuItem>
                    <asp:MenuItem Text="&nbsp"/>
                    <asp:MenuItem Text="&nbsp&nbspPPA&nbsp&nbsp" Value="item5" ImageUrl="~/Images/money.gif" Selectable="false" >
                        <asp:MenuItem Text="&nbspAgregar PPA&nbsp" Value="subItem1" NavigateUrl="~/WebForms/PPA/PPA" />
                        <asp:MenuItem Text="&nbspConsultar&nbsp" Value="subItem1" NavigateUrl="~/WebForms/PPA/PPAConsulta" />
                    </asp:MenuItem>
                    <asp:MenuItem Text="&nbsp"/>
                </Items>
                <StaticMenuStyle HorizontalPadding="4px" VerticalPadding="4px" />
                <StaticMenuItemStyle ItemSpacing="8px" />
                <StaticHoverStyle BackColor="#D8D9DB" />
                <DynamicHoverStyle BackColor="#D8D9DB" />
                <StaticSelectedStyle BackColor="#D8D9DB" />
                <DynamicMenuItemStyle BorderColor="#D8D9DB" BorderStyle="Solid" BorderWidth="1px" HorizontalPadding="4px" VerticalPadding="4px" />
            </asp:Menu>
        </asp:PlaceHolder>
        <asp:PlaceHolder runat="server" ID="phMenuAdmin" Visible="false">
            <asp:Menu ID="mnAdmin" runat="server" Orientation="Vertical">
                <Items>
                    <asp:MenuItem Text="&nbsp"/>
                    <asp:MenuItem Text="&nbsp&nbspAdministrar&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp" Value="item1" ImageUrl="~/Images/admin.gif" Selectable="false" >
                        <asp:MenuItem Text="&nbspAgregar/Actualizar usuario&nbsp" Value="subItem1" NavigateUrl="~/Account/AdminAgregarActualizar" />
                    </asp:MenuItem>
                    <asp:MenuItem Text="&nbsp"/>
                </Items>
                <StaticMenuStyle HorizontalPadding="4px" VerticalPadding="4px" />
                <StaticMenuItemStyle ItemSpacing="8px" />
                <StaticHoverStyle BackColor="#D8D9DB" />
                <DynamicHoverStyle BackColor="#D8D9DB" />
                <StaticSelectedStyle BackColor="#D8D9DB" />
                <DynamicMenuItemStyle BorderColor="#D8D9DB" BorderStyle="Solid" BorderWidth="1px" HorizontalPadding="4px" VerticalPadding="4px" />
            </asp:Menu>
        </asp:PlaceHolder>
    </div>
</asp:Content>
