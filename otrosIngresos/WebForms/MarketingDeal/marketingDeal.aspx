<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="marketingDeal.aspx.cs" 
    Inherits="otrosIngresos.Documents.MarketingDeal.marketingDeal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!--<title>jQuery UI Datepicker - Default functionality</title>-->
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="~/Scripts/jquery-1.12.4.js"></script>
    <script src="~/Scripts/jquery-ui.js"></script>
    <script type="text/javascript" src="~/Scripts/autoLogoff.js"></script>

    <script type = "text/javascript">

        $(function ()
        {
            $(".cldrDatePicker").datepicker(
                {
                    dateFormat: "dd/MM/yy"
                });
        });
    </script> 

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <section id="FormularioAcuerdos">
        <div dir="rtl">
            <div dir="ltr">
                <asp:Label ID="lbSesion" runat="server" Font-Bold="true" ForeColor=#00395A Font-Size="Smaller" />
            </div>
            <asp:Button runat="server" ID="btnRegresar" Text="Regresar" OnClick="btnRegresar_Click" Font-Size="Smaller" CausesValidation="false" />
            <div dir="ltr">
                <asp:Image runat="server" ID="construccion" ImageUrl="~/Images/EnConstruccion.jpg" />
            </div>
        </div>
    </section>
</asp:Content>
