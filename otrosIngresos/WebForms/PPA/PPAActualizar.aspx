<%@ Page Title="PPA" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PPAActualizar.aspx.cs" 
    Inherits="otrosIngresos.Documents.PPA.PPAActualizar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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

    <section id="FormularioAcuerdos">
        <div dir="rtl">
            <div dir="ltr">
                <asp:Label ID="lbSesion" runat="server" CssClass="labelMaster" />
                <asp:TextBox runat="server" ID="txtRespuesta" BorderColor="White" ForeColor="Black" Font-Size="Smaller" Visible="false"/>
            </div>
            <asp:Button runat="server" ID="btnRegresar" Text="Regresar" OnClick="btnRegresar_Click" Font-Size="Smaller" CausesValidation="false" />
        </div>
        <table>
            <tr>
                <td>
                    <asp:UpdatePanel ID="upPPA1" runat="server">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td />
                                    <td>
                                        <asp:CheckBox ID="cbActivo" runat="server" CssClass="checkBoxForm" Font-Size="Small" />
                                        <asp:Label ID="lbActivo" runat="server" Text="Activo" Font-Size="Small" />
                                    </td>
                                </tr>
                                <!--tr>
                                    <td>
                                        <asp:Label ID="lbEstatus" runat="server" Text="Estatus:" CssClass="labelHeadForm" />
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlEstatus" DataTextField="FullName" DataValueField="ID" CssClass="dropDownListForm" />
                                    </td>
                                </tr-->
                                <tr>
                                    <td><br />
                                        <asp:Label ID="lbComercial" runat="server" Text="Comercial" CssClass="labelForm" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbCFechaInicio" runat="server" Text="Fecha de Inicio:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCFechaInicio" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCFechaInicio" CssClass="field-validation-error" 
                                            ErrorMessage="Fecha de inicio requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbCFechaFin" runat="server" Text="Fecha de Termino:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCFechaFin" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCFechaFin" CssClass="field-validation-error" 
                                            ErrorMessage="Fecha de termino requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td><br />
                                        <asp:Label ID="lbLegal" runat="server" Text="Legal" CssClass="labelForm" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbLFechaInicio" runat="server" Text="Fecha de Inicio:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLFechaInicio" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLFechaInicio" CssClass="field-validation-error" 
                                            ErrorMessage="Fecha de inicio requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbLFechaFin" runat="server" Text="Fecha de Termino:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLFechaFin" runat="server" Font-Size="Small" TextMode="Date" Font-Names="Helvetica" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLFechaFin" CssClass="field-validation-error" 
                                            ErrorMessage="Fecha de termino requerido." />
                                    </td>
                                </tr>
                                <tr>
                                    <td />
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbPrepago" runat="server" Text="Total de Prepago:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPrepago" Font-Size="Small" Width="120px" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPrepago" CssClass="field-validation-error" 
                                            ErrorMessage="Valor requerido." /><br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))$" 
                                            CssClass="field-validation-error" ErrorMessage="Solo punto decimal" ControlToValidate="txtPrepago" /><br />
                                        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtPrepago" ErrorMessage="Solo acepta valores mayores a 0"
                                            CssClass="field-validation-error" MaximumValue="1000000000000" MinimumValue="1" Type="Double" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbinteres" runat="server" Text="% de interes:" Font-Size="Small" />
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtInteres" Font-Size="Small" Width="120px" />
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtInteres" CssClass="field-validation-error" 
                                            ErrorMessage="Valor requerido." /><br />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationExpression="((\d+)((\.\d{1,2})?))" 
                                            CssClass="field-validation-error" ErrorMessage="Solo punto decimal" ControlToValidate="txtInteres" /><br />
                                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtInteres" ErrorMessage="Solo acepta valor de 0 a 100"
                                            CssClass="field-validation-error" MaximumValue="100" MinimumValue="0" Type="Double" />
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
                    <asp:UpdatePanel ID="upPPA2" runat="server">
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
                                        <asp:Label runat="server" ID="lbEquipo" Text="" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Moneda:&nbsp;<b style="color:#ED1556";>*</b>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlMoneda" DataTextField="FullName" DataValueField="ID" AutoPostBack="true" />&nbsp;
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlMoneda" 
                                            CssClass="field-validation-error" ErrorMessage="Tipo de moneda requerido." />
                                        <asp:Label runat="server" ID="lbMoneda" Text="" Visible="false" />
                                    </td>
                                </tr>
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
                                    <td>
                                    </td>
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
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnBuscar" runat="server" CommandName="Buscar" Text="Buscar" OnClick="btnBuscar_Click" Font-Size="Smaller" />
                                    </td>
                                </tr>
                            </table>

                            <table>
                                <tr>
                                    <td>
                                        <div class="scrolling-table-container">
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
                                            Font-Size="Smaller" Visible="false" />
                                        <asp:Button ID="btnModificar" runat="server" CommandName="Modificar" Text="Modificar" OnClick="btnModificar_Click" 
                                            Font-Size="Smaller" />
                                        <asp:Button ID="btnCancelar" runat="server" CommandName="Cancelar" Text="Cancelar" OnClick="btnCancelar_Click" 
                                            Font-Size="Smaller" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnGuardar" runat="server" CommandName="Guardar" Text="Guardar" OnClick="btnGuardar_Click" Font-Size="Smaller"
                                    OnClientClick="return confirmMsg()" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </section>
</asp:Content>
