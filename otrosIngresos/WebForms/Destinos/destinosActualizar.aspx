<%@ Page Title="Destinos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="destinosActualizar.aspx.cs" 
    Inherits="otrosIngresos.Documents.Destinos.destinosActualizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .vl {
            border-left: 6px solid #ED1556;
            height: 600px;
        }

        .auto-style1 {
            height: 15px;
        }
    </style>
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

        function confirmMsg() {
            //var equipo = '<%= ddlEquipo.SelectedItem.Text %>';
            //var moneda = '<%= ddlMoneda.SelectedItem.Text %>';
            
            //return confirm('Seleccionó Equipo: ' + equipo + ' con Moneda: ' + moneda + ', ¿Es correcto?');
            return confirm('AVISO: ¡Favor de verificar que el EQUIPO, PAÍS y MONEDA seleccionados sean correctos!');
        }

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
        </div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upIngresos" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        Ingreso sin IVA:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtIngresoSinIVA" Width="100px" Font-Size="Small" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtIngresoSinIVA" 
                                            CssClass="field-validation-error" ErrorMessage="Ingreso sin IVA requerido." />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                            ValidationExpression="((\d+)((\.\d{1,2})?))$" CssClass="field-validation-error" 
                                            ErrorMessage="Valor no válido, solo acepta punto decimal" ControlToValidate="txtIngresoSinIVA" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Ingreso con IVA:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtIngresoConIVA" Width="100px" Font-Size="Small" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtIngresoConIVA" 
                                            CssClass="field-validation-error" ErrorMessage="Ingreso con IVA requerido." />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                            ValidationExpression="((\d+)((\.\d{1,2})?))$" CssClass="field-validation-error" 
                                            ErrorMessage="Valor no válido, solo acepta punto decimal" ControlToValidate="txtIngresoConIVA" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Ingreso sin IVA<br />en Pesos MXN:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtIngresoSinIVAMXN" Width="100px" Font-Size="Small" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtIngresoSinIVAMXN" 
                                            CssClass="field-validation-error" ErrorMessage="Ingreso sin IVA en Pesos MXN requerido." />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                                            ValidationExpression="((\d+)((\.\d{1,2})?))$" CssClass="field-validation-error" 
                                            ErrorMessage="Valor no válido, solo acepta punto decimal" ControlToValidate="txtIngresoSinIVAMXN" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Estatus:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEstatus" DataTextField="FullName" DataValueField="ID" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlEstatus" 
                                            CssClass="field-validation-error" ErrorMessage="Estatus requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Comentarios<br />Adicionales:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="6" Width="350px" Height="100px" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Fecha Facturación:&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaFactura" runat="server" CssClass="cldrDatePicker" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Folio Factura:&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFolioFactura" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Fecha de Pago:&nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaPago" runat="server" CssClass="cldrDatePicker" Font-Size="Small" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="cbOrden" Font-Size="Small" OnCheckedChanged="cbOrden_CheckedChanged" AutoPostBack="true" />
                                <asp:Label runat="server" ID="lbOrden" Font-Size="Small" Text="Cargar Orden de Inserción"/>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upOrdenInsercion" runat="server" Visible="false" >
                                    <ContentTemplate>
                                        <fieldset>
                                            <asp:FileUpload ID="fuSubirOrden" runat="server" Font-Size="Smaller" />
                                            <asp:Button runat="server" ID="btnOrden" Text="Subir Orden" OnClick="btnOrden_Click" />
                                            <asp:Label ID="lbSubirOrden" runat="server" Font-Bold="true" ForeColor=#ED1556 />
                                            <asp:Label ID="lbRutaOrden" runat="server" Visible="false" />
                                        </fieldset>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnOrden" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="cbFactura" Font-Size="Small" OnCheckedChanged="cbFactura_CheckedChanged" AutoPostBack="true" />
                                <asp:Label runat="server" ID="lbFactura" Font-Size="Small" Text="Cargar Factura"/>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upFactura" runat="server" Visible="false" >
                                    <ContentTemplate>
                                        <fieldset>
                                            <asp:FileUpload ID="fuFactura" runat="server" Font-Size="Smaller" />
                                            <asp:Button runat="server" ID="btnFactura" Text="Subir Factura" OnClick="btnFactura_Click" />
                                            <asp:Label ID="lbSubirFactura" runat="server" Font-Bold="true" ForeColor=#ED1556 />
                                            <asp:Label ID="lbRutaFactura" runat="server" Visible="false" />
                                        </fieldset>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnFactura" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox runat="server" ID="cbPago" Font-Size="Small" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" />
                                <asp:Label runat="server" ID="lbPago" Font-Size="Small" Text="Cargar Pago"/>
                            </td>
                            <td>
                                <asp:UpdatePanel ID="upPago" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <fieldset>
                                            <asp:FileUpload ID="fuPago" runat="server" Font-Size="Smaller" />
                                            <asp:Button runat="server" ID="btnPago" Text="Subir Pago" OnClick="btnPago_Click" />
                                            <asp:Label ID="lbSubirPago" runat="server" Font-Bold="true" ForeColor=#ED1556 />
                                            <asp:Label ID="lbRutaPago" runat="server" Visible="false" />
                                        </fieldset>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnPago" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                    <p style="color:#ED1556;font-size:smaller;font-weight:bold;";>
                        <b>(*)&nbsp;</b>Campos obligatorios.
                    </p>
                </td>
                <td>
                    <div class="vl"></div>
                </td>
                <td>
                    <asp:UpdatePanel ID="upCliente" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        Equipo:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEquipo" DataTextField="FullName" DataValueField="ID" 
                                            OnSelectedIndexChanged="ddlEquipo_SelectedIndexChanged" AutoPostBack="true" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlEquipo" 
                                            CssClass="field-validation-error" ErrorMessage="Equipo requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        País:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPais" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlPais" 
                                            CssClass="field-validation-error" ErrorMessage="País requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Moneda:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMoneda" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlMoneda" 
                                            CssClass="field-validation-error" ErrorMessage="Tipo de moneda requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbCliente" Text="Cliente: " />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCliente" Font-Size="Small" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtCliente"
                                            CssClass="field-validation-error" ErrorMessage="Debe capturar el nombre o identificador del cliente." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbRazonSocial" Text="Razón Social: " />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtRazonSocial" Font-Size="Small" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtRazonSocial"
                                            CssClass="field-validation-error" ErrorMessage="Debe capturar  la razón social del cliente." />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Text="Guardar" OnClick="btnGuardar_Click" Font-Size="Small" 
                                    OnClientClick="return confirmMsg();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
