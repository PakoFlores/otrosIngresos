<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="outletActualizar.aspx.cs" 
    Inherits="otrosIngresos.Documents.Outlet.outletActualizar" %>

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

    <script>
        function checkClick(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original
                        inputList[i].checked = false;
                    }
                }
            }
        }

        function confirmMsg() {
            return confirm('AVISO: ¡Favor de verificar que el EQUIPO y MONEDA seleccionados sean correctos!');
        }
    </script>

    <hgroup class="title">
        <h1><%: Title %></h1>
    </hgroup>

    <section id="FormularioAcuerdos" >
        <div dir="rtl">
            <div dir="ltr">
                <asp:Label ID="lbSesion" runat="server" Font-Bold="true" ForeColor=#00395A Font-Size="Smaller" />
                <asp:TextBox runat="server" ID="txtRespuesta" BorderColor="White" ForeColor="Black" Font-Size="Smaller" Visible="false"/>
            </div>
            <asp:Button runat="server" ID="btnRegresar" Text="Regresar" OnClick="btnRegresar_Click" Font-Size="Smaller" CausesValidation="false" />
        </div>
        <table>
            <tr>
                <td>
                    <asp:PlaceHolder runat="server" ID="phBasico" Visible="false" >
                        <table>
                            <tr>
                                <td>
                                    Equipo:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlEquipo" DataTextField="FullName" DataValueField="ID" 
                                        OnSelectedIndexChanged="ddlEquipo_SelectedIndexChanged" AutoPostBack="true" Enabled="false" />&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlEquipo" 
                                        CssClass="field-validation-error" ErrorMessage="Equipo requerido." />
                                    <asp:Label runat="server" ID="lbEquipo" Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                    <td>
                                        <asp:Label ID="lbPais" runat="server" Text="Pais:" CssClass="labelForm" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPais" DataTextField="FullName" DataValueField="ID" 
                                            AutoPostBack="true" Visible="false" />
                                    </td>
                            </tr>
                            <tr>
                                <td>
                                    Moneda:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMoneda" DataTextField="FullName" DataValueField="ID" />&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMoneda" 
                                        CssClass="field-validation-error" ErrorMessage="Tipo de moneda requerido." />
                                    <asp:Label runat="server" ID="lbMoneda" Text="" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mes:
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlMes" DataTextField="FullName" DataValueField="ID" Enabled="false" />&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlMes" 
                                        CssClass="field-validation-error" ErrorMessage="Mes requerido." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbTipoAcuerdo" text="Tipo de acuerdo:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTipoAcuerdo" DataTextField="FullName" DataValueField="ID" />&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipoAcuerdo" 
                                        CssClass="field-validation-error" ErrorMessage="Tipo de acuerdo requerido." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbComprobante" text="Comprobante:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlComprobante" DataTextField="FullName" DataValueField="ID" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label runat="server" ID="lbNumeroFacturas" text="Número de Facturas:" Font-Size="Small" />
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:DropDownList runat="server" ID="ddlNumeroFacturas" DataTextField="FullName" DataValueField="ID" />&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlComprobante" CssClass="field-validation-error" 
                                        ErrorMessage="Tipo de acuerdo requerido." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha Facturación:&nbsp;<b style="color:#ED1556";>*</b>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaFactura" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                </td>
                            </tr>
                            <tr>
                                    <td>
                                        Precio sin IVA:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbPrecioSinIVA" Width="100px" Font-Size="Small" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lbIVA" Text="IVA: " />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList runat="server" ID="ddlIVA" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" />
                                    </td>
                            </tr>

                            <tr>
                                    <td />
                                    <td>
                                        <asp:GridView runat="server" ID="gvGrupoCanal" OnRowDataBound ="gvGrupoCanal_RowDataBound" OnPageIndexChanging="gvGrupoCanal_PageIndexChanging" 
                                            AutoGenerateColumns="false" EmptyDataText="No se encontró información." ClientIDMode="Static"
                                            Cellpadding="2" ForeColor="#000333" GridLines="Both" Style="text-align: left; font-size: x-small"
                                            Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" Font-Size="X-Small" Visible="false" >
                                            
                                            <RowStyle Backcolor=#D8D9DB />
                                            <AlternatingRowStyle BackColor="White" />
                                            
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="ID" runat="server" Text='<%# Bind("ID") %>' Font-Size="0.8em" />
                                                    </ItemTemplate>
                                                </asp:TemplateField> 
                                                <asp:BoundField DataField="Canal" HeaderText="Canal" ItemStyle-Font-Size="X-Small" />
                                                <asp:TemplateField HeaderText="Monto">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtMonto" runat="server" Width="50px" OnTextChanged="txtMonto_TextChanged" Font-Size="X-Small" AutoPostBack="true" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbTipoPaquete" text="Paquete/Diseño:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlTipoPaquete" DataTextField="FullName" DataValueField="ID" />&nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlTipoPaquete" 
                                        CssClass="field-validation-error" ErrorMessage="Tipo de paquete o diseño requerido." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbFechaInicio" text="Fecha de Inicio:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaInicio" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtFechaInicio" 
                                        CssClass="field-validation-error" ErrorMessage="Fecha de inicio requerido." />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbFechaTermino" text="Fecha de Termino:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaTermino" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica"/>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtFechaTermino" 
                                        CssClass="field-validation-error" ErrorMessage="Fecha de termino requerido." />
                                </td>
                            </tr>
                            <!--tr>
                                <td>
                                    Fecha Venta:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaVenta" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                </td>
                            </tr-->
                            <tr>
                                <td>
                                    <asp:Label runat="server" ID="lbFormaPago" text="Forma de Pago:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlFormaPago" DataTextField="FullName" DataValueField="ID" 
                                        OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged" AutoPostBack="true" />
                                    &nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lbEmail" Text="Email: " Visible="false"/>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList runat="server" ID="ddlEmail" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Comentarios<br /> Adicionales:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="5" Width="300px" Height="100px" Font-Size="Small" />
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td>
                                        <asp:CheckBox runat="server" ID="cbOrden" Font-Size="Small" OnCheckedChanged="cbOrden_CheckedChanged" AutoPostBack="true" />
                                        <asp:Label runat="server" ID="lbOrden" Font-Size="Small" Text="Cargar Orden de Inserción"/>
                                    </td>
                                <td>
                                    <asp:UpdatePanel ID="upOrden" runat="server">
                                        <ContentTemplate>
                                            <fieldset>
                                                <p><b style="color:#ED1556";>Cargar archivo antes de guardar.</b></p>
                                                <p>Control de Acuerdos:&nbsp;<b style="color:#ED1556";>*</b></p>
                                                <asp:FileUpload ID="fuSubirOrden" runat="server" Font-Size="Small" />
                                                <asp:Button runat="server" ID="btnOrden" Text="Subir Acuerdo" Visible="true" OnClick="btnOrden_Click" />
                                                <asp:Label ID="lbSubirOrden" runat="server" Font-Bold="true" ForeColor=#ED1556 />
                                                <asp:Label ID="lbRutaOrden" runat="server" Visible="false" />
                                                <asp:Label ID="lbRutaOrdenDet" runat="server" Visible="false" />
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
                                    <asp:CheckBox runat="server" ID="cbFactura" Font-Size="Small" OnCheckedChanged="cbFactura_CheckedChanged" AutoPostBack="true" Visible="false" />
                                    <asp:Label runat="server" ID="lbFactura" Font-Size="Small" Text="Cargar Factura" Visible="false" />
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
                                        <asp:CheckBox runat="server" ID="cbPago" Font-Size="Small" OnCheckedChanged="cbPago_CheckedChanged" AutoPostBack="true" Visible="false" />
                                        <asp:Label runat="server" ID="lbPago" Font-Size="Small" Text="Cargar Pago" Visible="false" />
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
                            <!--tr>
                                <td>
                                    <asp:Button runat="server" ID="btnGenerar" Text="Generar PDF" Visible="false" />
                                </td>
                            </tr-->
                        </table>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phFinanzasDestino" Visible="false">
                        <table>
                            <tr>
                                <td>
                                    Folio de Factura:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFolioFacturaDestino" Font-Size="Small" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha de Cobro:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaCobroDestino" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbEstatus" runat="server" Text="Estatus:" Font-Size="Small" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddlEstatus" DataTextField="FullName" DataValueField="ID" />
                                </td>
                            </tr>
                        </table>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder runat="server" ID="phFinanzas" Visible="false">
                    <table>
                        <tr>
                            <td>
                                Folio de Factura:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFolioFactura" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Facturados:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtFacturados" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Moneda Factura:
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlMonedaFacturado" DataTextField="FullName" DataValueField="ID" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Importe por facturar con IVA:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtImporteporFacturar" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Monto Facturado:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMontoFacturado" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Monto Facturado sin IVA (MXN):
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMontoFacturadoMXN" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Monto Cobrado:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtMontoCobrado" Font-Size="Small" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fecha de Cobro:
                            </td>
                            <td>
                                <asp:TextBox ID="txtFechaCobro" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbEstatusCXC" runat="server" Text="Estatus CXC:" Font-Size="Small" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" ID="ddlEstatusCXC" DataTextField="FullName" DataValueField="ID" />
                            </td>
                        </tr>
                    </table>
                    </asp:PlaceHolder>
                </td>
                <td>
                    <div class="vl" />
                </td>
                <td>
                    <asp:PlaceHolder runat="server" ID="phComplemento" Visible="false">
                    <asp:UpdatePanel ID="upHotel" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbClienteRFC" runat="server" Text="Cliente o RFC:" CssClass="labelForm" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlCliente" DataTextField="FullName" DataValueField="ID" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlCliente_SelectedIndexChanged" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlCliente" 
                                            CssClass="field-validation-error" ErrorMessage="Cliente requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbCliente" Text="Captura Cliente:" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCliente" Font-Size="Small" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbRazonsocial" Text="Razón Social:"  Visible="false" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtRazonSocial" Font-Size="Small"  Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:UpdatePanel ID="upCliente" runat="server" Visible="false" >
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbDestino" Text="Destino:" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDestino" Font-Size="Small" />
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbRFCCliente" Text="RFC:" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtRFCCliente" Font-Size="Small" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbDireccionfiscal" Text="Dirección Fiscal:" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtDireccionFiscal" Font-Size="Small" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label runat="server" ID="lbCodigoPostal" Text="Código Postal:" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtCodigoPostal" Font-Size="Small" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                <tr>
                                    <td />
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:UpdatePanel ID="upContactos" runat="server" Visible="false" >
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="text-align:center;">
                                                            <asp:Label ID="lbContactoC" runat="server" Text="Contacto Contabilidad" CssClass="labelForm" />
                                                        </td>
                                                        <td style="text-align:center;">
                                                            <asp:Label ID="lbContactoM" runat="server" Text="Contacto Marketing" CssClass="labelForm" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbNombreC" runat="server" Text="Nombre: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtNombreC" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbCargoC" runat="server" Text="Cargo: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCargoC" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbEmailC" runat="server" Text="E-mail: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtEmailC" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbTelefonoC" runat="server" Text="Teléfono: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtTelefonoC" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbNombreM" runat="server" Text="Nombe: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtNombreM" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbCargoM" runat="server" Text="Cargo: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtCargoM" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbEmailM" runat="server" Text="E-mail: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtEmailM" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lbTelefonoM" runat="server" Text="Teléfono: " CssClass="labelForm" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtTelefonoM" CssClass="textBoxForm" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                </tr>
                            </table>

                            <asp:UpdatePanel ID="UpdateHotel" runat="server" >
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lbFiltrar" Text="Tipo de contrato:" Visible="false" />
                                            </td>
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlListado" DataTextField="FullName" DataValueField="ID" 
                                                        OnSelectedIndexChanged="ddlListado_SelectedIndexChanged" AutoPostBack="true" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <asp:DropDownList runat="server" ID="ddlServicio" DataTextField="FullName" DataValueField="ID" 
                                                    OnSelectedIndexChanged="ddlServicio_SelectedIndexChanged" AutoPostBack="true" Visible="false" />
                                        
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <asp:Label runat="server" ID="lbIDHotel" Text="ID o Nombre:" Visible="false" /><br>
                                                <asp:TextBox runat="server" ID="txtServicio" OnTextChanged="txtServicio_TextChanged" AutoPostBack="true" 
                                                    Width="300px" Enabled="false" Visible="false" /><br>
                                                <asp:Label runat="server" ID="lbMensaje1" Text="Usar comas (,) como separador para busquedas avanzadas" 
                                                    Font-Size="Smaller" Font-Bold="True" Visible="false" /><br>
                                                <asp:Label runat="server" ID="lbMensaje2" Text="Ejemplo: 100101, Hotel1, Hotel2, 123456, etc." 
                                                    Font-Size="Smaller" Font-Bold="True" Visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td />
                                            <td>
                                                <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" Text="Buscar" OnClick="btnBuscar_Click" 
                                                    Font-Size="Small" Visible="false" />
                                            </td>
                                        </tr>    
                                    </table>

                                    <table>
                                        <tr>
                                            <td>
                                                <div class="scrolling-table-container" >
                                                    <asp:GridView runat="server" ID="gvServicio" 
                                                        AutoGenerateColumns="false" EmptyDataText="No se encontró información." ClientIDMode="Static" 
                                                        Cellpadding="2" ForeColor="#000333" GridLines="Both" Style="text-align: left"
                                                        Width="600px" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" 
                                                        CssClass="table table-striped table-hover table-condensed small-top-margin" >

                                                        <RowStyle Backcolor=#D8D9DB />
                                                        <AlternatingRowStyle BackColor="White" />
                                                
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox Text="Todo" ID="cbSeleccionarTodo" runat="server" OnClick ="checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbSeleccionar" runat="server" OnClick ="checkClick(this);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="IdCP" HeaderText="IdCP" Visible="false" />
                                                            <asp:BoundField DataField="ID" HeaderText="ID" />
                                                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                                            <asp:BoundField DataField="TipoServicio" HeaderText="TipoServicio" />
                                                        </Columns>
                                                    </asp:GridView>
                                            
                                                    <asp:GridView runat="server" ID="gvHotel" 
                                                        AutoGenerateColumns="false" EmptyDataText="No se encontró información." ClientIDMode="Static" 
                                                        Cellpadding="2" ForeColor="#000333" GridLines="Both" Style="text-align: left"
                                                        Width="600px" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" 
                                                        CssClass="table table-striped table-hover table-condensed small-top-margin" Visible="false" >

                                                        <RowStyle Backcolor=#D8D9DB />
                                                        <AlternatingRowStyle BackColor="White" />

                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox Text="Todo" ID="cbSeleccionarTodo" runat="server" OnClick ="checkAll(this);" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="cbSeleccionar" runat="server" OnClick ="checkClick(this);" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="IdHotel" HeaderText="ID Hotel" />
                                                            <asp:BoundField DataField="Hotel" HeaderText="Hotel" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td dir="rtl">
                                                <asp:Button ID="btnAgregar" runat="server" CommandName="Agregar" Text="Agregar" OnClick="btnAgregar_Click" 
                                                    Font-Size="Small" Visible="false" />
                                                <asp:Button ID="btnModificar" runat="server" CommandName="Modificar" Text="Modificar" OnClick="btnModificar_Click" 
                                                    Font-Size="Small" />
                                                <asp:Button ID="btnCancelar" runat="server" CommandName="Cancelar" Text="Cancelar" OnClick="btnCancelar_Click" 
                                                    Font-Size="Small" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </asp:PlaceHolder>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Text="Guardar" OnClick="btnGuardar_Click" Font-Size="Small"
                                    OnClientClick="return confirmMsg()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
