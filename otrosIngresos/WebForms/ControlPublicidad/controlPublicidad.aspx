<%@ Page Title="Control de Publicidad" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="controlPublicidad.aspx.cs" 
    Inherits="otrosIngresos.Documents.controlPublicidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server" >
    <style type="text/css">
        .vl {
            border-left: 6px solid #ED1556;
            height: 600px;
        }
        
        .auto-style2 {
            width: 106px;
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
    <script src="../Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    
    <script type = "text/javascript">

        $(function ()
        {
            $(".cldrDatePicker").datepicker(
                {
                    dateFormat: "dd/MM/yy"
                });
        });

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
            //var equipo = '<%= ddlEquipo.SelectedItem.Text %>';
            //var moneda = '<%= ddlMoneda.SelectedItem.Text %>';
            
            //return confirm('Seleccionó Equipo: ' + equipo + ' con Moneda: ' + moneda + ', ¿Es correcto?');
            return confirm('AVISO: ¡Favor de verificar que el EQUIPO y MONEDA seleccionados sean correctos!');
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
                    <asp:UpdatePanel ID="upDatos" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:RadioButton ID="rbGenerar" runat="server" GroupName="OrdenInsercion" AutoPostBack="true" 
                                            OnCheckedChanged="rbGenerar_CheckedChanged" Font-Bold="true" Font-Size="Smaller" />
                                        <asp:Label ID="lbGenerar" runat="server" Text="Generar PDF" Font-Bold="true" Font-Size="Small" />&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbSubir" runat="server" GroupName="OrdenInsercion" AutoPostBack="true" Checked="true" 
                                            OnCheckedChanged="rbSubir_CheckedChanged" Font-Bold="true" Font-Size="Smaller" />
                                        <asp:Label ID="lbSubir" runat="server" Text="Subir Acuerdo" Font-Bold="true" Font-Size="Small" />
                                    </td>
                                </tr>
                                 <tr>
                                    <td>
                                        Equipo:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEquipo" DataTextField="FullName" DataValueField="ID" 
                                            OnSelectedIndexChanged="ddlEquipo_SelectedIndexChanged" AutoPostBack="true" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlEquipo" 
                                            CssClass="field-validation-error" ErrorMessage="Equipo requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbPais" runat="server" Text="Pais:" CssClass="labelForm" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlPais" DataTextField="FullName" DataValueField="ID" OnLoad="ddlPais_Load" 
                                            OnSelectedIndexChanged="ddlPais_SelectedIndexChanged" AutoPostBack="true" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Moneda:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMoneda" DataTextField="FullName" DataValueField="ID" OnLoad="ddlMoneda_Load" 
                                            OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" AutoPostBack="true" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMoneda" 
                                            CssClass="field-validation-error" ErrorMessage="Tipo de moneda requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Mes:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMes" DataTextField="FullName" DataValueField="ID" OnLoad="ddlMes_Load" Enabled="false" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlMes" CssClass="field-validation-error" 
                                            ErrorMessage="Mes requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbTipoAcuerdo" text="Tipo de acuerdo:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTipoAcuerdo" DataTextField="FullName" DataValueField="ID" OnLoad="ddlTipoAcuerdo_Load" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlTipoAcuerdo" CssClass="field-validation-error" 
                                            ErrorMessage="Tipo de acuerdo requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbComprobante" text="Comprobante:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlComprobante" DataTextField="FullName" DataValueField="ID" OnLoad="ddlComprobante_Load" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lbNumeroFacturas" text="Número de Facturas:" Font-Size="Small" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList runat="server" ID="ddlNumeroFacturas" DataTextField="FullName" DataValueField="ID" OnLoad="ddlNumeroFacturas_Load" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlComprobante" CssClass="field-validation-error" 
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
                                        <asp:DropDownList runat="server" ID="ddlIVA" DataTextField="FullName" DataValueField="ID" OnLoad="ddlIVA_Load" AutoPostBack="true" />
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td />
                                    <td>
                                        <asp:GridView runat="server" ID="gvGrupoCanal" OnRowDataBound ="gvGrupoCanal_RowDataBound" OnPageIndexChanging="gvGrupoCanal_PageIndexChanging" 
                                            AutoGenerateColumns="false" EmptyDataText="No se encontró información." ClientIDMode="Static"
                                            Cellpadding="2" ForeColor="#000333" GridLines="Both" Style="text-align: left; font-size: x-small"
                                            Font-Names="Trebuchet MS, Tahoma, Verdana, Arial" Font-Size="X-Small" >
                                            
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
                                        <asp:DropDownList runat="server" ID="ddlTipoPaquete" DataTextField="FullName" DataValueField="ID" OnLoad="ddlTipoPaquete_Load" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlTipoPaquete" CssClass="field-validation-error" 
                                            ErrorMessage="Tipo de paquete o diseño requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbFechaInicio" text="Fecha de Inicio:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaInicio" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbFechaTermino" text="Fecha de Termino:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaTermino" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    </td>
                                </tr>
                                <!--tr>
                                    <td>
                                        Fecha Venta:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFechaVenta" runat="server" CssClass="cldrDatePicker" Font-Size="Small" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtFechaVenta" CssClass="field-validation-error" 
                                            ErrorMessage="Fecha de venta requerido." />
                                    </td>
                                </tr-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbFormaPago" text="Forma de Pago:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlFormaPago" DataTextField="FullName" DataValueField="ID" OnLoad="ddlFormaPago_Load" 
                                            OnSelectedIndexChanged="ddlFormaPago_SelectedIndexChanged" AutoPostBack="true" />
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:Label runat="server" ID="lbEmail" Text="Email: " Visible="false"/>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:DropDownList runat="server" ID="ddlEmail" DataTextField="FullName" DataValueField="ID" OnLoad="ddlEmail_Load" AutoPostBack="true" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Comentarios<br /> Adicionales:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtComentarios" TextMode="MultiLine" Rows="6" Width="350px" Height="100px" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFolioFactura" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCantidadFacturas" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtFacturados" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMonedaFacturado" DataTextField="FullName" DataValueField="ID" OnLoad="ddlMonedaFacturado_Load" 
                                            Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtImporteporFacturar" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMontoFacturado" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMontoFacturadoMXN" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtMontoCobrado" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFechaCobro" runat="server" TextMode ="Date" Visible="false" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <!--<td>
                                        Estatus CXC:
                                        </td>-->
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEstatusCXC" DataTextField="FullName" DataValueField="ID" OnLoad="ddlEstatusCXC_Load" 
                                            Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbEstatus" runat="server" Text="Estatus:" Font-Size="Small" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEstatus" DataTextField="FullName" DataValueField="ID" OnLoad="ddlEstatus_Load" Visible="false" />
                                    </td>
                                </tr>

                                <tr>
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbOrden" Font-Size="Small" OnCheckedChanged="cbOrden_CheckedChanged" AutoPostBack="true" Visible="false" />
                                        <asp:Label runat="server" ID="lbOrden" Font-Size="Small" Text="Cargar Orden de Inserción" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:UpdatePanel ID="upAcuerdo" runat="server" Visible="false" >
                                            <ContentTemplate>
                                                <fieldset>
                                                    <asp:FileUpload ID="fuSubirOrden" runat="server" Font-Size="Smaller" />
                                                    <asp:Button runat="server" ID="btnOrden" Text="Subir Orden" OnClick="btnOrden_Click" />
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
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <p style="color:#ED1556" >
                                            <b>(*)&nbsp;</b>Campos obligatorios.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td>
                    <div class="vl"></div>
                </td>
                <td>
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
                                    <td>
                                        <asp:Label runat="server" ID="lbCliente" Text="Captura Cliente: " />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCliente" Font-Size="Small" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbDestino" Text="Destino:" Visible="false" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDestino" Font-Size="Small" Visible="false" />
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
                                                            <asp:Label runat="server" ID="lbRazonsocial" Text="Razón Social:" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox runat="server" ID="txtRazonSocial" Font-Size="Small" />
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
                                </tr>
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
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="lbFiltrar" Text="Tipo de contrato:" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlListado" DataTextField="FullName" DataValueField="ID" 
                                                OnSelectedIndexChanged="ddlListado_SelectedIndexChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlServicio" DataTextField="FullName" DataValueField="ID" 
                                            OnSelectedIndexChanged="ddlServicio_SelectedIndexChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:Label runat="server" ID="lbIDHotel" Text="ID o Nombre:" Visible="false" /><br>
                                        <asp:TextBox runat="server" ID="txtServicio" OnTextChanged="txtServicio_TextChanged" AutoPostBack="true" Width="300px" 
                                            Enabled="false" Visible="false" /><br>
                                        <asp:Label runat="server" ID="lbMensaje1" Text="Usar comas (,) como separador para busquedas avanzadas" Font-Size="Smaller" 
                                            Font-Bold="True" Visible="false" /><br>
                                        <asp:Label runat="server" ID="lbMensaje2" Text="Ejemplo: 100101, Hotel1, Hotel2, 123456, etc." Font-Size="Smaller" 
                                            Font-Bold="True" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" Text="Buscar" OnClick="btnBuscar_Click" Font-Size="Smaller" />
                                    </td>
                                </tr>
                            </table>

                            <table>
                                <tr>
                                    <td>
                                        <div class="scrolling-table-container">
                                            <asp:GridView runat="server" ID="gvHotel" OnPageIndexChanging="gvHotel_PageIndexChanging"
                                                AutoGenerateColumns="false" EmptyDataText="No se encontró información." ClientIDMode="Static" 
                                                Cellpadding="2" ForeColor="#000333" GridLines="Both" Style="text-align: left"
                                                Width="600px" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial"
                                                CssClass="table table-striped table-hover table-condensed small-top-margin" >

                                                <RowStyle Backcolor=#D8D9DB />
                                                <AlternatingRowStyle BackColor="White" />

                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox Text="Todo" ID="cbSeleccionarTodo" runat="server" OnClick="checkAll(this);" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="cbSeleccionar" runat="server" OnClick="checkClick(this);" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="IdHotel" HeaderText="ID Hotel" />
                                                    <asp:BoundField DataField="Hotel" HeaderText="Hotel" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td />
                                </tr>
                                <tr>
                                    <td />
                                    <td />
                                    <td />
                                    <td>
                                        <asp:CheckBox runat="server" ID="cbFondoMarketing" Font-Size="Small" OnCheckedChanged="cbFondoMarketing_CheckedChanged" AutoPostBack="true"/>
                                        <asp:Label runat="server" ID="lbFondoMarketing" Font-Size="Small" Text="Fondo de Marketing"/>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td>
                                        <div class="scrolling-table-container">
                                            <asp:GridView runat="server" ID="gvFondoMarketing" OnPageIndexChanging="gvFondoMarketing_PageIndexChanging"
                                                AutoGenerateColumns="false" EmptyDataText="No se encontró información." ClientIDMode="Static"
                                                Cellpadding="2" ForeColor="#000333" GridLines="Both" Style="text-align: left"
                                                Width="600px" Font-Names="Trebuchet MS, Tahoma, Verdana, Arial"
                                                CssClass="table table-striped table-hover table-condensed small-top-margin" >
                                                
                                                <RowStyle Backcolor=#D8D9DB />
                                                <AlternatingRowStyle BackColor="White" />
                                                
                                                <Columns>
                                                    <asp:BoundField DataField="IdHotel" HeaderText="ID Hotel" ItemStyle-Font-Size="X-Small" />
                                                    <asp:BoundField DataField="Hotel" HeaderText="Hotel" ItemStyle-Font-Size="X-Small" />
                                                    <asp:TemplateField HeaderText="Ene">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtEne" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Feb">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFeb" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Mar">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMar" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Abr">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAbr" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="May">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtMay" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Jun">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtJun" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Jul">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtJul" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ago">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtAgo" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sep">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSep" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Oct">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtOct" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nov">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtNov" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Dic">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtDic" runat="server" Width="25px" Font-Size="X-Small" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                </tr>
                            </table>
                            
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table>
                        <tr>
                            <td />
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Text="Guardar" OnClick="BtnGuardar_Click" Font-Size="Smaller" 
                                    OnClientClick="return confirmMsg();" />
                            </td>
                            <td>
                                <asp:Button ID="btnVistaPrevia" runat="server" Text="Vista Previa Archivo PDF" OnClick="btnVistaPrevia_Click" Font-Size="Smaller" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
