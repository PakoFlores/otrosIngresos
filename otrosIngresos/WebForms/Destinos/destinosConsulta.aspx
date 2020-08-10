<%@ Page Title="Destinos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="destinosConsulta.aspx.cs" 
    Inherits="testotrosIngresos1.Documents.Destinos.DestinosConsulta" %>
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

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
            <asp:Button ID="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar registro de Destinos" OnClick="btnAgregar_Click" Font-Size="Smaller" />
        </div>
        <div>
            <asp:UpdatePanel ID="upDestinos" runat="server">
                <ContentTemplate>

                    <div class="scrolling-table-container">
                        <asp:GridView runat="server" ID="gvDestinos" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" ClientIDMode="Static" 
                            ForeColor="#000333" GridLines="Both" Style="text-align: left" AutoGenerateColumns="false" Cellpadding="2" 
                            EmptyDataText="No existe información disponible." Width="600px" Font-Size="0.8em"
                            OnPageIndexChanging="gvDestinos_PageIndexChanging" OnRowCommand ="gvDestinos_RowCommand"
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
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Equipo") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Pais:
                                        <asp:DropDownList ID="ddl2" runat="server" OnSelectedIndexChanged = "ddl2_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Pais") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Cliente:
                                        <asp:DropDownList ID="ddl3" runat="server" OnSelectedIndexChanged = "ddl3_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Cliente") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Razón Social:
                                        <asp:DropDownList ID="ddl4" runat="server" OnSelectedIndexChanged = "ddl4_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Razon Social") %>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Orden de Inserción">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="OrdenInsercion" runat="server" NavigateUrl='<%# Eval("Orden de Insercion", "~/Documents/Files/ControlPublicidad/{0}") %>'
                                            Text="Ver Orden" Target="_blank" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Ingreso sin IVA:
                                        <asp:DropDownList ID="ddl5" runat="server" OnSelectedIndexChanged = "ddl5_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Ingreso sin IVA") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Ingreso con IVA:
                                        <asp:DropDownList ID="ddl6" runat="server" OnSelectedIndexChanged = "ddl6_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Ingreso con IVA") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Tipo de Moneda:
                                        <asp:DropDownList ID="ddl7" runat="server" OnSelectedIndexChanged = "ddl7_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Moneda") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Ingreso sin IVA en Pesos MXN:
                                        <asp:DropDownList ID="ddl8" runat="server" OnSelectedIndexChanged = "ddl8_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Ingreso sin IVA en Pesos MXN") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Estatus:
                                        <asp:DropDownList ID="ddl9" runat="server" OnSelectedIndexChanged = "ddl9_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Estatus") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Mes de Facturación:
                                        <asp:DropDownList ID="ddl10" runat="server" OnSelectedIndexChanged = "ddl10_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Mes de Facturacion") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Fecha de Facturación:
                                        <asp:DropDownList ID="ddl11" runat="server" OnSelectedIndexChanged = "ddl11_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Fecha de Facturacion") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Folio de Factura:
                                        <asp:DropDownList ID="ddl12" runat="server" OnSelectedIndexChanged = "ddl12_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Folio de Factura") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Mes de Pago:
                                        <asp:DropDownList ID="ddl13" runat="server" OnSelectedIndexChanged = "ddl13_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Mes de Pago") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Fecha de Pago:
                                        <asp:DropDownList ID="ddl14" runat="server" OnSelectedIndexChanged = "ddl14_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Fecha de Pago") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:TemplateField HeaderText="Factura">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="Factura" runat="server" NavigateUrl='<%# Eval("Archivo Factura", "~/Documents/Files/Destinos/Factura/{0}") %>'
                                            Text="Ver Factura" Target="_blank" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Comprobante de Pago">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="ComprobantePago" runat="server" NavigateUrl='<%# Eval("Comprobante de Pago", "~/Documents/Files/Destinos/ComprobantePago/{0}") %>'
                                            Text="Ver Pago" Target="_blank" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Comentarios" HeaderText="Comentarios Adicionales" ItemStyle-Font-Size="1.2em" />
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Gerente:
                                        <asp:DropDownList ID="ddl15" runat="server" OnSelectedIndexChanged = "ddl15_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Gerente") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </section>
</asp:Content>
