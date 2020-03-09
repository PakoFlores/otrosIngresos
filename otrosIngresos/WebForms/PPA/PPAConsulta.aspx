<%@ Page Title="PPA" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PPAConsulta.aspx.cs" 
    Inherits="testotrosIngresos1.Documents.PPA.PPAConsulta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 15px;
        }

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
            <asp:Button ID="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar registro de PPA" OnClick="btnAgregar_Click" Font-Size="Smaller" />
            <!--div>
                <asp:Image runat="server" ID="construccion" ImageUrl="~/Images/EnConstruccion.jpg" />
            </div-->
        </div>
        <div>
            <asp:UpdatePanel ID="upPPA" runat="server">
                <ContentTemplate>
                    
                    <div>
                        <asp:Menu ID="mtReporte" Width="500px" runat="server"
                            Orientation="Horizontal" StaticEnableDefaultPopOutImage="False"
                            OnMenuItemClick="mtReporte_MenuItemClick">
                            <Items>
                                <asp:MenuItem ImageUrl="~/Images/tabResSelected.jpg" Text="" Value="0" />
                                <asp:MenuItem ImageUrl="~/Images/tabExtUnselected.jpg" Text="" Value="1"  />
                            </Items>
                        </asp:Menu>
                        <asp:MultiView ID="mvReporte" runat="server" ActiveViewIndex="0" >
                            <asp:View ID="vwResumen" runat="server" >
                                <div class="scrolling-table-container">
                                    <asp:GridView runat="server" ID="gvPPA" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" ClientIDMode="Static" 
                                        ForeColor="#000333" GridLines="Both" Style="text-align: left" AutoGenerateColumns="false" Cellpadding="2" 
                                        EmptyDataText="No existe información disponible." Width="600px" Font-Size="0.8em" OnRowDataBound ="gvPPA_RowDataBound"
                                        OnPageIndexChanging="gvPPA_PageIndexChanging" OnRowCommand ="gvPPA_RowCommand"
                                        CssClass="table table-striped table-hover table-condensed small-top-margin" >
                                        
                                        <RowStyle Backcolor=#D8D9DB ForeColor=#00395A />
                                        <AlternatingRowStyle BackColor="White" ForeColor=#00395A />
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Editar" HeaderStyle-VerticalAlign="Bottom">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEditar" runat="server" CausesValidation="False" CommandArgument='<%# Bind("Registro") %>' 
                                                        CommandName="Editar" Text="Editar" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar" HeaderStyle-VerticalAlign="Bottom">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Bind("Registro") %>' 
                                                        CommandName="Eliminar" Text="Eliminar" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" 
                                                        OnClientClick="return confirmMsg();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="Registro" runat="server" Text='<%# Bind("Registro") %>' Visible="false" Font-Size="0.8em" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Equipo:
                                                    <asp:DropDownList ID="ddl1" runat="server" OnSelectedIndexChanged = "ddl1_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Equipo") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Hotel:
                                                    <asp:DropDownList ID="ddl2" runat="server" OnSelectedIndexChanged = "ddl2_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Hotel") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    ID:
                                                    <asp:DropDownList ID="ddl3" runat="server" OnSelectedIndexChanged = "ddl3_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("ID") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Gerente:
                                                    <asp:DropDownList ID="ddl4" runat="server" OnSelectedIndexChanged = "ddl4_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Gerente") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    IsActive:
                                                    <asp:DropDownList ID="ddl5" runat="server" OnSelectedIndexChanged = "ddl5_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("IsActive") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Esatus Acuerdo PPA:
                                                    <asp:DropDownList ID="ddl6" runat="server" OnSelectedIndexChanged = "ddl6_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Estatus Acuerdo PPA") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Fecha de Inicio Comercial:
                                                    <asp:DropDownList ID="ddl7" runat="server" OnSelectedIndexChanged = "ddl7_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Fecha de Inicio Comercial") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Fecha de Fin Comercial:
                                                    <asp:DropDownList ID="ddl8" runat="server" OnSelectedIndexChanged = "ddl8_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Fecha de Fin Comercial") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Fecha de Inicio Legal:
                                                    <asp:DropDownList ID="ddl9" runat="server" OnSelectedIndexChanged = "ddl9_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Fecha de Inicio Legal") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Fecha de Fin Legal:
                                                    <asp:DropDownList ID="ddl10" runat="server" OnSelectedIndexChanged = "ddl10_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Fecha de Fin Legal") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Moneda:
                                                    <asp:DropDownList ID="ddl11" runat="server" OnSelectedIndexChanged = "ddl11_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Moneda") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Total del Prepago:
                                                    <asp:DropDownList ID="ddl12" runat="server" OnSelectedIndexChanged = "ddl12_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Total del Prepago") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Porcentaje Interes:
                                                    <asp:DropDownList ID="ddl13" runat="server" OnSelectedIndexChanged = "ddl13_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Porcentaje Interes") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:View>
                            <asp:View ID="vwExtendido" runat="server">
                                <div class="scrolling-table-container">
                                    <asp:GridView runat="server" ID="gvExtendido" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" ClientIDMode="Static" 
                                        ForeColor="#000333" GridLines="Both" Style="text-align: left" AutoGenerateColumns="false" Cellpadding="2" 
                                        EmptyDataText="No existe información disponible." Width="600px" Font-Size="0.8em" OnRowDataBound ="gvExtendido_RowDataBound"
                                        OnPageIndexChanging="gvExtendido_PageIndexChanging" OnRowCommand ="gvExtendido_RowCommand"
                                        CssClass="table table-striped table-hover table-condensed small-top-margin" >
                                        
                                        <RowStyle Backcolor=#D8D9DB ForeColor=#00395A />
                                        <AlternatingRowStyle BackColor="White" ForeColor=#00395A />
                                        
                                        <Columns>
                                            <asp:TemplateField HeaderText="Editar" HeaderStyle-VerticalAlign="Bottom">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEditar" runat="server" CausesValidation="False" CommandArgument='<%# Bind("Registro") %>' 
                                                        CommandName="Editar" Text="Editar" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar" HeaderStyle-VerticalAlign="Bottom">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbEliminar" runat="server" CausesValidation="False" CommandArgument='<%# Bind("Registro") %>' 
                                                        CommandName="Eliminar" Text="Eliminar" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" 
                                                        OnClientClick="return confirmMsg();" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="Registro" runat="server" Text='<%# Bind("Registro") %>' Visible="false" Font-Size="0.8em" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Equipo:
                                                    <asp:DropDownList ID="ddl1" runat="server" OnSelectedIndexChanged = "ddl1_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Equipo") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Hotel:
                                                    <asp:DropDownList ID="ddl2" runat="server" OnSelectedIndexChanged = "ddl2_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Hotel") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    ID:
                                                    <asp:DropDownList ID="ddl3" runat="server" OnSelectedIndexChanged = "ddl3_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("ID") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Gerente:
                                                    <asp:DropDownList ID="ddl4" runat="server" OnSelectedIndexChanged = "ddl4_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Gerente") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Esatus Acuerdo PPA:
                                                    <asp:DropDownList ID="ddl5" runat="server" OnSelectedIndexChanged = "ddl5_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Estatus Acuerdo PPA") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Fecha de Inicio Comercial:
                                                    <asp:DropDownList ID="ddl6" runat="server" OnSelectedIndexChanged = "ddl6_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Fecha de Inicio Comercial") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Fecha de Fin Comercial:
                                                    <asp:DropDownList ID="ddl7" runat="server" OnSelectedIndexChanged = "ddl7_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Fecha de Fin Comercial") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    ENERO:
                                                    <asp:DropDownList ID="ddl8" runat="server" OnSelectedIndexChanged = "ddl8_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("ENERO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    FEBRERO:
                                                    <asp:DropDownList ID="ddl9" runat="server" OnSelectedIndexChanged = "ddl9_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("FEBRERO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    MARZO:
                                                    <asp:DropDownList ID="ddl10" runat="server" OnSelectedIndexChanged = "ddl10_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("MARZO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    ABRIL:
                                                    <asp:DropDownList ID="ddl11" runat="server" OnSelectedIndexChanged = "ddl11_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("ABRIL") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    MAYO:
                                                    <asp:DropDownList ID="ddl12" runat="server" OnSelectedIndexChanged = "ddl12_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("MAYO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    JUNIO:
                                                    <asp:DropDownList ID="ddl13" runat="server" OnSelectedIndexChanged = "ddl13_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("JUNIO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    JULIO:
                                                    <asp:DropDownList ID="ddl14" runat="server" OnSelectedIndexChanged = "ddl14_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("JULIO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    AGOSTO:
                                                    <asp:DropDownList ID="ddl15" runat="server" OnSelectedIndexChanged = "ddl15_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("AGOSTO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    SEPTIEMBRE:
                                                    <asp:DropDownList ID="ddl16" runat="server" OnSelectedIndexChanged = "ddl16_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("SEPTIEMBRE") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    OCTUBRE:
                                                    <asp:DropDownList ID="ddl17" runat="server" OnSelectedIndexChanged = "ddl17_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("OCTUBRE") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    NOVIEMBRE:
                                                    <asp:DropDownList ID="ddl18" runat="server" OnSelectedIndexChanged = "ddl18_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("NOVIEMBRE") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    DICIEMBRE:
                                                    <asp:DropDownList ID="ddl19" runat="server" OnSelectedIndexChanged = "ddl19_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("DICIEMBRE") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Total del Prepago:
                                                    <asp:DropDownList ID="ddl20" runat="server" OnSelectedIndexChanged = "ddl20_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Total del Prepago") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                                <HeaderTemplate>
                                                    Porcentaje Interes:
                                                    <asp:DropDownList ID="ddl21" runat="server" OnSelectedIndexChanged = "ddl21_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Porcentaje Interes") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                                <HeaderTemplate>
                                                    Moneda:
                                                    <asp:DropDownList ID="ddl22" runat="server" OnSelectedIndexChanged = "ddl22_SelectedIndexChanged" AutoPostBack = "true"
                                                        ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                                        <asp:ListItem Text = "ALL" Value = "ALL" />
                                                    </asp:DropDownList>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%# Eval("Moneda") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>
</asp:Content>
