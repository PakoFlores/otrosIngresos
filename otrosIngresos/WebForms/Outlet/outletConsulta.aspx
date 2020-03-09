<%@ Page Title="Outlet" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="outletConsulta.aspx.cs" 
    Inherits="testotrosIngresos1.Documents.Outlet.outletConsulta" %>
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

    <hgroup class="titleCP">
        <h1><%: Title %></h1>
    </hgroup>

    <section id="GridViewConsulta">
        <div dir="ltr">
            <div dir="ltr">
                <asp:Label ID="lbSesion" runat="server" Font-Bold="true" ForeColor=#00395A Font-Size="Smaller" />
            </div>
            <asp:Button ID="btnRegresar" runat="server" CommandName="Regresar" Text="Guardar y Salir" OnClick="btnRegresar_Click" Font-Size="Smaller" />
            <asp:Button ID="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar orden de inserción" OnClick="btnAgregar_Click" Font-Size="Smaller" />
        </div>
        <div>
            <asp:UpdatePanel ID="upControlPublicidad" runat="server">
                <ContentTemplate>

                    <div class="scrolling-table-container">
                        <asp:GridView runat="server" ID="gvOutlet" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" ClientIDMode="Static" 
                            ForeColor="#000333" GridLines="Both" Style="text-align: left" AutoGenerateColumns="false" Cellpadding="2" 
                            EmptyDataText="No existe información disponible." Width="600px" Font-Size="0.8em" OnRowDataBound ="gvOutlet_RowDataBound"
                            OnPageIndexChanging="gvOutlet_PageIndexChanging" OnRowCommand ="gvOutlet_RowCommand"
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
                                        Mes:
                                        <asp:DropDownList ID="ddl2" runat="server" OnSelectedIndexChanged = "ddl2_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Mes") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Tipo de Acuerdo:
                                        <asp:DropDownList ID="ddl3" runat="server" OnSelectedIndexChanged = "ddl3_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Tipo de Acuerdo") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Gerente:
                                        <asp:DropDownList ID="ddl4" runat="server" OnSelectedIndexChanged = "ddl4_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Gerente") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Proveedor:
                                        <asp:DropDownList ID="ddl5" runat="server" OnSelectedIndexChanged = "ddl5_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Proveedor") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        ID:
                                        <asp:DropDownList ID="ddl6" runat="server" OnSelectedIndexChanged = "ddl6_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("ID") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Moneda de Acuerdo:
                                        <asp:DropDownList ID="ddl7" runat="server" OnSelectedIndexChanged = "ddl7_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Moneda de Acuerdo") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                    <HeaderTemplate>
                                        Precio sin IVA:
                                        <asp:DropDownList ID="ddl8" runat="server" OnSelectedIndexChanged = "ddl8_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Precio sin IVA") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="Tipo de Paquete/Diseño" HeaderText="Tipo de Paquete/ Diseño" ItemStyle-Font-Size="1.2em" />
                                
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Meses Contratados:
                                        <asp:DropDownList ID="ddl9" runat="server" OnSelectedIndexChanged = "ddl9_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Meses Contratados") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Fecha de Inicio Acciones Efectivas MKT:
                                        <asp:DropDownList ID="ddl10" runat="server" OnSelectedIndexChanged = "ddl10_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Fecha de Inicio Acciones Efectivas MKT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Fecha de Fin Acciones Efectivas MKT:
                                        <asp:DropDownList ID="ddl11" runat="server" OnSelectedIndexChanged = "ddl11_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Fecha de Fin Acciones Efectivas MKT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Meses Acciones Efectivas MKT:
                                        <asp:DropDownList ID="ddl12" runat="server" OnSelectedIndexChanged = "ddl12_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Meses Acciones Efectivas MKT") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Fecha de Venta:
                                        <asp:DropDownList ID="ddl13" runat="server" OnSelectedIndexChanged = "ddl13_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Fecha de Venta") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        RFC:
                                        <asp:DropDownList ID="ddl14" runat="server" OnSelectedIndexChanged = "ddl14_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("RFC") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Forma de Pago:
                                        <asp:DropDownList ID="ddl15" runat="server" OnSelectedIndexChanged = "ddl15_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Forma de Pago") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="Comentarios Adicionales" HeaderText="Comentarios Adicionales" ItemStyle-Font-Size="1.2em" />
                                <asp:TemplateField HeaderText="Control de Acuerdos">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="Acuerdo" runat="server" NavigateUrl='<%# Eval("Control de Acuerdos", "~/Documents/Files/Outlet/{0}") %>'
                                            Text="Ver Acuerdo" Target="_blank" ForeColor="#ED1556" Font-Underline="true" Font-Size="0.8em" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Folio de Factura:
                                        <asp:DropDownList ID="ddl16" runat="server" OnSelectedIndexChanged = "ddl16_SelectedIndexChanged" AutoPostBack = "true"
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
                                        Cantidad de Facturas:
                                        <asp:DropDownList ID="ddl17" runat="server" OnSelectedIndexChanged = "ddl17_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Cantidad de Facturas") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Cantidad Facturados:
                                        <asp:DropDownList ID="ddl18" runat="server" OnSelectedIndexChanged = "ddl18_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Cantidad Facturados") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                    <HeaderTemplate>
                                        Moneda de Factura:
                                        <asp:DropDownList ID="ddl19" runat="server" OnSelectedIndexChanged = "ddl19_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Moneda de Factura") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                    <HeaderTemplate>
                                        Importe a Facturar con IVA:
                                        <asp:DropDownList ID="ddl20" runat="server" OnSelectedIndexChanged = "ddl20_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Importe a Facturar con IVA") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                    <HeaderTemplate>
                                        Monto Facturado:
                                        <asp:DropDownList ID="ddl21" runat="server" OnSelectedIndexChanged = "ddl21_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Monto Facturado") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" >
                                    <HeaderTemplate>
                                        Monto Facturado en Pesos MX sin IVA:
                                        <asp:DropDownList ID="ddl22" runat="server" OnSelectedIndexChanged = "ddl22_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Monto Facturado en Pesos MX sin IVA") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="auto-style1" >
                                    <HeaderTemplate>
                                        Monto Cobrado:
                                        <asp:DropDownList ID="ddl23" runat="server" OnSelectedIndexChanged = "ddl23_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Monto Cobrado") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom" >
                                    <HeaderTemplate>
                                        Fecha de Cobro:
                                        <asp:DropDownList ID="ddl24" runat="server" OnSelectedIndexChanged = "ddl24_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Fecha de Cobro") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                <asp:BoundField DataField="Diferencia por Cobrar contra Facturado" DataFormatString={0:C2} 
                                    HeaderText="Diferencia por Cobrar contra Facturado" ItemStyle-Font-Size="1.2em" ItemStyle-HorizontalAlign="Right" />
                                
                                <asp:TemplateField ItemStyle-Font-Size="1.2em" HeaderStyle-VerticalAlign="Bottom">
                                    <HeaderTemplate>
                                        Estatus CXC:
                                        <asp:DropDownList ID="ddl25" runat="server" OnSelectedIndexChanged = "ddl25_SelectedIndexChanged" AutoPostBack = "true"
                                            ForeColor="#ED1556" Font-Size="1em" AppendDataBoundItems = "true">
                                            <asp:ListItem Text = "ALL" Value = "ALL"></asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Estatus CXC") %>
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
