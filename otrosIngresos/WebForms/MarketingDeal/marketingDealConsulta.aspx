<%@ Page Title="Marketing Deal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="marketingDealConsulta.aspx.cs" 
    Inherits="testotrosIngresos1.Documents.MarketingDeal.MarketingDealConsulta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 15px;
        }
    </style>
    <style>
        .vl {
            border-left: 6px solid #ED1556;
            height: 900px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <script type="text/javascript" src="~/Scripts/autoLogoff.js"></script>
    <script type = "text/javascript">
        function confirmMsg()
        {
            return confirm('¿Desea eliminar el registro seleccionado?');
        }
    </script>

    <hgroup class="titleD">
        <h1><%: Title %></h1>
    </hgroup>

    <section id="GridViewConsulta">
        <div dir="ltr">
            <div dir="ltr">
                <asp:Label ID="lbSesion" runat="server" Font-Bold="true" ForeColor=#00395A Font-Size="Smaller" />
            </div>
            <asp:Button ID="btnRegresar" runat="server" CommandName="Regresar" Text="Guardar y Salir" OnClick="btnRegresar_Click" Font-Size="Smaller" />
            <asp:Button ID="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar Marketing Deal" OnClick="btnAgregar_Click" Font-Size="Smaller" />
            <div>
                <asp:Image runat="server" ID="construccion" ImageUrl="~/Images/EnConstruccion.jpg" />
            </div>
        </div>
        <div>
            <asp:UpdatePanel ID="upMarketingDeal" runat="server">
                <ContentTemplate>

                    <asp:GridView runat="server" ID="gvMarketingDeal" Cellpadding="2" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" ForeColor="#000333" GridLines="Both" 
                        PageSize="20" Style="text-align: left" AutoGenerateColumns="false" EmptyDataText="No existe información disponible." Width="600px" AllowPaging="True" PageIndex="0" 
                        OnLoad="gvMarketingDeal_Load" OnPageIndexChanging="gvMarketingDeal_PageIndexChanging" OnRowCommand ="gvMarketingDeal_RowCommand" >
                        <PagerSettings Mode="NextPreviousFirstLast" PageButtonCount="15" Position="TopAndBottom" />
                        <RowStyle Backcolor=#D8D9DB ForeColor=#00395A />
                        <AlternatingRowStyle BackColor="White" ForeColor=#00395A />

                        <Columns>
                            <asp:TemplateField HeaderText="Editar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEditar" runat="server" CausesValidation="False" CommandArgument='<%# Bind("Registro") %>' 
                                        CommandName="Editar" Text="Editar" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Bind("Registro") %>' 
                                        CommandName="Eliminar" Text="Eliminar" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Registro" runat="server" Text='<%# Bind("Registro") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Equipo" HeaderText="Equipo" />
                            <asp:BoundField DataField="Hotel" HeaderText="Hotel" />
                            <asp:BoundField DataField="ID Hotel" HeaderText="ID Hotel" />
                            <asp:BoundField DataField="Estancia o Venta" HeaderText="Estancia o Venta" />
                            <asp:BoundField DataField="Tipo de Acuerdo" HeaderText="Tipo de Acuerdo" />
                            <asp:BoundField DataField="Estatus" HeaderText="Estatus" />
                            <asp:BoundField DataField="Gerente" HeaderText="Gerente" />
                            <asp:BoundField DataField="Moneda de Acuerdo" HeaderText="Moneda de Acuerdo" />
                            <asp:BoundField DataField="Fecha Inicio" HeaderText="Fecha Inicio" />
                            <asp:BoundField DataField="Fecha Fin" HeaderText="Fecha Fin" />
                            <asp:BoundField DataField="Fecha Nuevo Inicio" HeaderText="Fecha Nuevo Inicio" />
                            <asp:BoundField DataField="Fecha Nuevo Fin" HeaderText="Fecha Nuevo Fin" />
                            <asp:BoundField DataField="Estatus DACK" HeaderText="Estatus DACK" />
                            <asp:BoundField DataField="Grupal o Individual" HeaderText="Grupal o Individual" />
                            <asp:BoundField DataField="Corporativo" HeaderText="Corporativo" />
                            <asp:BoundField DataField="Especiales" HeaderText="Especiales" />
                            <asp:BoundField DataField="Parámetro Tradicionales" HeaderText="Parámetro Tradicionales" />
                            <asp:BoundField DataField="% Real del Convenio" HeaderText="% Real del Convenio" />
                            <asp:BoundField DataField="Meta 1" HeaderText="Meta 1" />
                            <asp:BoundField DataField="Retorno 1" HeaderText="Retorno 1" />
                            <asp:BoundField DataField="Meta 2" HeaderText="Meta 2" />
                            <asp:BoundField DataField="Retorno 2" HeaderText="Retorno 2" />
                            <asp:BoundField DataField="Meta 3" HeaderText="Meta 3" />
                            <asp:BoundField DataField="Retorno 3" HeaderText="Retorno 3" />
                            <asp:BoundField DataField="Meta 4" HeaderText="Meta 4" />
                            <asp:BoundField DataField="Retorno 4" HeaderText="Retorno 4" />
                            <asp:BoundField DataField="Meta 5" HeaderText="Meta 5" />
                            <asp:BoundField DataField="Retorno 5" HeaderText="Retorno 5" />
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>
</asp:Content>
