using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using otrosIngresos.Shared;

namespace otrosIngresos.Documents.Outlet
{
    public partial class outletActualizar : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        int idCorporativo = 0;
        decimal total = 0;
        string idControlP = "", tipoAcuerdo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            idControlP = Request.QueryString["idCP"];
            tipoAcuerdo = Request.QueryString["tipo"];

            if (!IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("~/Account/Login.aspx");
                else
                {
                    if (Convert.ToInt32(Session["idPerfil"]) == 2 || Convert.ToInt32(Session["idPerfil"]) == 6)
                        Response.Redirect("~/Default.aspx");
                    else
                    {
                        lbSesion.Text = Convert.ToString(Session["nombre"]) + " (" + Convert.ToString(Session["perfil"]) + ")  ";
                        validarPerfil(Convert.ToInt32(Session["idPerfil"]));
                        ddlMes.Enabled = true;
                    }
                }

                DataTable dt = new DataTable();

                dt = action.Consultar("SELECT atSNombreEquipo, skNIdEquipo FROM dbo.dimEquipo WHERE atBEstatus = 1");

                ddlEquipo.DataTextField = "atSNombreEquipo";
                ddlEquipo.DataValueField = "skNIdEquipo";
                ddlEquipo.DataSource = dt;
                ddlEquipo.DataBind();

                llenarCliente();
                llenarListado();
                llenarListaServicio();
                llenarDatos();

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 1)
                    ddlMoneda.SelectedValue = "44";
                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 2)
                    ddlMoneda.SelectedValue = "109";

                System.DateTime moment = System.DateTime.Now;

                dt = action.Consultar("SELECT atSNombreMes, skNIdMes FROM dbo.dimMes");

                ddlMes.DataTextField = "atSNombreMes";
                ddlMes.DataValueField = "skNIdMes";
                ddlMes.DataSource = dt;
                ddlMes.DataBind();
                ddlMes.SelectedValue = Convert.ToString(moment.Month);

                dt = action.Consultar(
                    "SELECT skNIdTipoAcuerdo, atSNombreAcuerdo "
                    + "FROM dbo.dimTipoAcuerdo "
                    + "WHERE atBEstatus = 1 AND skNIdTipoAcuerdo = 2");

                ddlTipoAcuerdo.DataTextField = "atSNombreAcuerdo";
                ddlTipoAcuerdo.DataValueField = "skNIdTipoAcuerdo";
                ddlTipoAcuerdo.DataSource = dt;
                ddlTipoAcuerdo.DataBind();

                dt = action.Consultar(
                   "SELECT skNIdPais, atSCodigoPais + ' - ' + atSNombrePais Pais "
                   + "FROM dbo.dimPais "
                   + "WHERE atBEstatus = 1");

                ddlPais.DataTextField = "Pais";
                ddlPais.DataValueField = "skNIdPais";
                ddlPais.DataSource = dt;
                ddlPais.DataBind();
                ddlPais.SelectedValue = Convert.ToString(moment.Month);

                dt = action.Consultar(
                    "SELECT skNIdMoneda, atSMoneda + ' - ' + atSNombreMoneda atSMoneda "
                    + "FROM dbo.dimMoneda "
                    + "WHERE atBEstatus = 1");

                ddlMoneda.DataTextField = "atSMoneda";
                ddlMoneda.DataValueField = "skNIdMoneda";
                ddlMoneda.DataSource = dt;
                ddlMoneda.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdTipoPaqueteDiseno, SUBSTRING(atSNombrePaqueteDiseno, 1, 50) atSNombrePaqueteDiseno "
                    + "FROM dbo.dimTipoPaqueteDiseno "
                    + "WHERE atBEstatus = 1");

                ddlTipoPaquete.DataTextField = "atSNombrePaqueteDiseno";
                ddlTipoPaquete.DataValueField = "skNIdTipoPaqueteDiseno";
                ddlTipoPaquete.DataSource = dt;
                ddlTipoPaquete.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdFormaPago, atSNombreFormaPago "
                    + "FROM dbo.dimFormaPago "
                    + "WHERE atBEstatus = 1");

                ddlFormaPago.DataTextField = "atSNombreFormaPago";
                ddlFormaPago.DataValueField = "skNIdFormaPago";
                ddlFormaPago.DataSource = dt;
                ddlFormaPago.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdMoneda, atSMoneda + ' - ' + atSNombreMoneda atSMoneda "
                    + "FROM dbo.dimMoneda "
                    + "--WHERE atBEstatus = 1");

                ddlMonedaFacturado.DataTextField = "atSMoneda";
                ddlMonedaFacturado.DataValueField = "skNIdMoneda";
                ddlMonedaFacturado.DataSource = dt;
                ddlMonedaFacturado.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdEstatusCXC, atSNombreEstatusCXC "
                    + "FROM dbo.dimEstatusCXC "
                    + "WHERE atBEstatus = 1");

                ddlEstatusCXC.DataTextField = "atSNombreEstatusCXC";
                ddlEstatusCXC.DataValueField = "skNIdEstatusCXC";
                ddlEstatusCXC.DataSource = dt;
                ddlEstatusCXC.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdEstatus, atSNombreEstatus "
                    + "FROM dbo.dimEstatus "
                    + "WHERE atBEstatus = 1");

                ddlEstatus.DataTextField = "atSNombreEstatus";
                ddlEstatus.DataValueField = "skNIdEstatus";
                ddlEstatus.DataSource = dt;
                ddlEstatus.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdTipoComprobante, atSNombreComprobante "
                    + "FROM dbo.dimTipoComprobante "
                    + "WHERE atBEstatus = 1");

                ddlComprobante.DataTextField = "atSNombreComprobante";
                ddlComprobante.DataValueField = "skNIdTipoComprobante";
                ddlComprobante.DataSource = dt;
                ddlComprobante.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdNumeroFacturas, atSNumeroFacturas "
                    + "FROM dbo.dimNumeroFacturas "
                    + "WHERE atBEstatus = 1");

                ddlNumeroFacturas.DataTextField = "atSNumeroFacturas";
                ddlNumeroFacturas.DataValueField = "skNIdNumeroFacturas";
                ddlNumeroFacturas.DataSource = dt;
                ddlNumeroFacturas.DataBind();

                dt = action.Consultar(
                    "SELECT skNIdIVA, atNPorcentajeIVA "
                    + "FROM dbo.dimIVA "
                    + "WHERE atBEstatus = 1");

                ddlIVA.DataTextField = "atNPorcentajeIVA";
                ddlIVA.DataValueField = "skNIdIVA";
                ddlIVA.DataSource = dt;
                ddlIVA.DataBind();

                dt = action.Consultar("SELECT skNIdEmail, atSEmail FROM dimEquipoEmail WHERE atBEstatus = 1 AND skNIdEquipo = " + Convert.ToInt16(ddlEquipo.SelectedItem.Value));

                ddlEmail.DataTextField = "atSEmail";
                ddlEmail.DataValueField = "skNIdEmail";
                ddlEmail.DataSource = dt;
                ddlEmail.DataBind();

                if (tipoAcuerdo == "1")
                {
                    dt = new DataTable();

                    dt.Columns.AddRange(
                        new DataColumn[3]
                        {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                        }
                        );

                    dt.Rows.Add("@idControlPublicidad", "SmallInt", Convert.ToInt16(idControlP));
                    dt.Rows.Add("@opcion", "SmallInt", 1);
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                    dt = action.EjecutarSP("spfctControlPublicidadConsulta", dt);

                    DataRow row = dt.Rows[0];
                    DateTime fecha = System.DateTime.Now;

                    ddlEquipo.SelectedValue = row[1].ToString();
                    llenarCliente();
                    llenarListado();
                    lbEquipo.Text = row[1].ToString();/**/
                    ddlMes.SelectedValue = row[2].ToString();
                    ddlTipoAcuerdo.SelectedValue = row[3].ToString();
                    ddlMoneda.SelectedValue = row[5].ToString();
                    lbMoneda.Text = row[5].ToString();/**/
                    lbPrecioSinIVA.Text = row[6].ToString();
                    ddlTipoPaquete.SelectedValue = row[7].ToString();
                    fecha = Convert.ToDateTime(row[8].ToString());
                    txtFechaInicio.Text = fecha.ToString("yyyy-MM-dd");
                    fecha = Convert.ToDateTime(row[9].ToString());
                    txtFechaTermino.Text = fecha.ToString("yyyy-MM-dd");
                    fecha = Convert.ToDateTime(row[10].ToString());
                    txtFechaVenta.Text = fecha.ToString("yyyy-MM-dd");
                    ddlCliente.SelectedValue = row[11].ToString();
                    ddlFormaPago.SelectedValue = row[12].ToString();
                    txtComentarios.Text = row[13].ToString();
                    lbSubirOrden.Text = row[14].ToString();
                    lbRutaOrden.Text = row[14].ToString();
                    txtFolioFactura.Text = row[15].ToString();
                    ddlNumeroFacturas.SelectedValue = row[16].ToString();
                    txtFacturados.Text = row[17].ToString();
                    ddlMonedaFacturado.SelectedValue = row[18].ToString();
                    txtImporteporFacturar.Text = row[19].ToString();
                    txtMontoFacturado.Text = row[20].ToString();
                    txtMontoFacturadoMXN.Text = row[21].ToString();
                    txtMontoCobrado.Text = row[22].ToString();
                    fecha = Convert.ToDateTime(row[23].ToString());
                    txtFechaCobro.Text = fecha.ToString("yyyy-MM-dd");
                    ddlEstatusCXC.SelectedValue = row[24].ToString();

                    if (row[25].ToString() != "")
                    {
                        lbCliente.Visible = true;
                        txtCliente.Visible = true;
                        txtCliente.Text = row[25].ToString();
                    }
                    else
                    {
                        lbCliente.Visible = false;
                        txtCliente.Visible = false;
                        txtCliente.Text = "";
                    }
                    ddlListado.SelectedValue = row[26].ToString();
                    lbRutaOrden.Text = row[27].ToString();
                    lbRutaOrdenDet.Text = row[28].ToString();
                    if (row[29].ToString() != "")
                    {
                        fecha = Convert.ToDateTime(row[29].ToString());
                        txtFechaFactura.Text = fecha.ToString("yyyy-MM-dd");
                    }
                    else
                        txtFechaFactura.Text = "";
                }
                else if (tipoAcuerdo == "5")
                {
                    dt = new DataTable();

                    dt.Columns.AddRange(
                        new DataColumn[3]
                        {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                        }
                        );

                    dt.Rows.Add("@idControlPublicidad", "SmallInt", Convert.ToInt16(idControlP));
                    dt.Rows.Add("@opcion", "SmallInt", 5);
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                    dt = action.EjecutarSP("spfctControlPublicidadConsulta", dt);

                    DataRow row = dt.Rows[0];
                    DateTime fecha = System.DateTime.Now;

                    ddlEquipo.SelectedValue = row[1].ToString();
                    //lbEquipo.Text             = row[1].ToString();/**/
                    ddlPais.SelectedValue = row[2].ToString();
                    txtCliente.Text = row[3].ToString();
                    if (row[3].ToString() != "")
                    {
                        lbCliente.Visible = true;
                        txtCliente.Visible = true;
                        txtCliente.Text = row[3].ToString();
                        lbRazonsocial.Visible = true;
                        txtRazonSocial.Visible = true;
                        txtRazonSocial.Text = row[4].ToString();
                    }
                    else
                    {
                        lbCliente.Visible = false;
                        txtCliente.Visible = false;
                        txtCliente.Text = "";
                        lbRazonsocial.Visible = false;
                        txtRazonSocial.Visible = false;
                        txtRazonSocial.Text = "";
                    }

                    lbSubirOrden.Text = row[5].ToString();
                    lbRutaOrden.Text = row[5].ToString();
                    if (lbSubirOrden.Text != "")
                    {
                        cbOrden.Checked = true;
                        upOrden.Visible = true;
                    }

                    lbPrecioSinIVA.Text = row[6].ToString();
                    //txtIngresoConIVA.Text = row[7].ToString();
                    ddlMoneda.SelectedValue = row[8].ToString();
                    //lbMoneda.Text             = row[8].ToString();/**/
                    //txtIngresoSinIVAMXN.Text = row[9].ToString();
                    ddlEstatus.SelectedValue = row[10].ToString();

                    if (row[11].ToString() != "")
                    {
                        fecha = Convert.ToDateTime(row[11].ToString());
                        txtFechaFactura.Text = fecha.ToString("yyyy-MM-dd");
                    }
                    else
                        txtFechaFactura.Text = "";

                    txtFolioFactura.Text = row[12].ToString();
                    txtFechaCobro.Text = row[13].ToString();
                    lbSubirFactura.Text = row[14].ToString();
                    lbRutaFactura.Text = row[14].ToString();
                    if (lbSubirFactura.Text != "")
                    {
                        cbFactura.Checked = true;
                        upFactura.Visible = true;
                    }
                    lbSubirPago.Text = row[15].ToString();
                    lbRutaPago.Text = row[15].ToString();
                    if (lbSubirPago.Text != "")
                    {
                        cbPago.Checked = true;
                        upPago.Visible = true;
                    }
                    txtComentarios.Text = row[16].ToString();
                    ddlNumeroFacturas.SelectedValue = row[17].ToString();
                    ddlIVA.SelectedValue = row[18].ToString();
                }

                int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

                if (equipo == 5)
                {
                    lbPais.Visible = true;
                    ddlPais.Visible = true;
                    lbTipoAcuerdo.Visible = false;
                    ddlTipoAcuerdo.Visible = false;
                    lbTipoPaquete.Visible = false;
                    ddlTipoPaquete.Visible = false;
                    lbClienteRFC.Visible = false;
                    ddlCliente.Visible = false;
                    gvHotel.Visible = false;
                    gvServicio.Visible = false;
                    lbFiltrar.Visible = false;
                    ddlListado.Visible = false;
                    ddlServicio.Visible = false;
                    lbIDHotel.Visible = false;
                    txtServicio.Visible = false;
                    lbMensaje1.Visible = false;
                    lbMensaje2.Visible = false;
                    btnBuscar.Visible = false;
                    lbFechaInicio.Visible = false;
                    txtFechaInicio.Visible = false;
                    lbFechaTermino.Visible = false;
                    txtFechaTermino.Visible = false;
                    phFinanzasDestino.Visible = true;
                    phFinanzas.Visible = false;
                    lbOrden.Visible = true;
                    cbOrden.Visible = true;
                    upOrden.Visible = true;
                    lbFactura.Visible = true;
                    cbFactura.Visible = true;
                    upFactura.Visible = true;
                    lbPago.Visible = true;
                    cbPago.Visible = true;
                    upPago.Visible = true;
                    btnModificar.Visible = false;
                }
                else
                {
                    phFinanzasDestino.Visible = false;
                    phFinanzas.Visible = true;

                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (tipoAcuerdo == "5")
                {


                    dt.Columns.AddRange(
                        new DataColumn[3]
                        {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                        }
                        );

                    dt.Rows.Add("@idDestino", "SmallInt", Convert.ToInt16(idControlP));
                    dt.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                    dt.Rows.Add("@pais", "SmallInt", Convert.ToInt16(ddlPais.SelectedItem.Value));
                    dt.Rows.Add("@nombreCliente", "VarChar", txtCliente.Text);
                    dt.Rows.Add("@razonSocial", "VarChar", txtRazonSocial.Text);

                    dt.Rows.Add("@ordenInsercion", "VarChar", lbRutaOrden.Text /*savePath*/);
                    dt.Rows.Add("@ingresoSinIVA", "Money", Convert.ToDecimal(lbPrecioSinIVA.Text));
                    dt.Rows.Add("@ingresoConIVA", "Money", Convert.ToDecimal(Convert.ToDouble(lbPrecioSinIVA.Text) * (1 + (Convert.ToDouble(ddlIVA.SelectedItem.Value) / 100))));
                    dt.Rows.Add("@moneda", "Int", Convert.ToInt16(ddlMoneda.SelectedItem.Value));
                    dt.Rows.Add("@ingresoSinIVAMXN", "Money", Convert.ToDecimal(lbPrecioSinIVA.Text));

                    dt.Rows.Add("@estatus", "SmallInt", Convert.ToInt16(ddlEstatus.SelectedItem.Value));
                    if (txtFechaFactura.Text != "")
                        dt.Rows.Add("@fechaFactura", "Date", Convert.ToDateTime(txtFechaFactura.Text));
                    dt.Rows.Add("@folioFactura", "VarChar", txtFolioFacturaDestino.Text);
                    if (txtFechaCobro.Text != "")
                        dt.Rows.Add("@fechaPago", "Date", Convert.ToDateTime(txtFechaCobroDestino.Text));
                    dt.Rows.Add("@archivoFactura", "VarChar", lbRutaFactura.Text /*savePath*/);

                    dt.Rows.Add("@archivoPago", "VarChar", lbRutaPago.Text /*savePath*/);
                    dt.Rows.Add("@comentariosAdicionales", "VarChar", txtComentarios.Text);
                    dt.Rows.Add("@cantidadFacturas", "SmallInt", Convert.ToInt16(ddlNumeroFacturas.SelectedItem.Value));
                    dt.Rows.Add("@porcentajeIva", "SmallInt", Convert.ToInt16(ddlIVA.SelectedItem.Value));
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                    /*dt.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);*/

                    DataTable dtUpd = new DataTable();
                    DataTable dtUpdDet = new DataTable();

                    dtUpd = action.EjecutarSP("spfctDestinoUpd", dt);
                    DataRow rowUpd = dtUpd.Rows[0];

                    if (Convert.ToInt32(Session["idPerfil"]) != 5)
                    {
                        if (!action.Eliminar("spfctDestinoDel", Convert.ToInt32(idControlP), "@idDestino", 2, Convert.ToInt32(Session["idUsuario"])))
                            ModelState.AddModelError("Destinos", errorMessage: "ERROR: No se pudo realizar la acción solicitada");
                    }
                    Response.Redirect("~/Documents/Destinos/destinosConsulta", false);
                }
                else
                {
                    string valido = validar();
                    if (valido != "")
                    {
                        mensajeError(@"ERROR: " + valido);
                        return;
                    }

                    string tipoServicio = "";

                    if (ddlListado.SelectedItem.Text == "Corporativo" || ddlListado.SelectedItem.Text == "Individual")
                        tipoServicio = "Hotel";
                    else
                        tipoServicio = ddlListado.SelectedItem.Text;

                    dt = new DataTable();

                    dt.Columns.AddRange(
                        new DataColumn[3]
                        {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                        }
                        );

                    dt.Rows.Add("@idControlPublicidad", "SmallInt", Convert.ToInt16(idControlP));

                    dt.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                    dt.Rows.Add("@mes", "SmallInt", Convert.ToInt16(ddlMes.SelectedItem.Value));
                    dt.Rows.Add("@tipoAcuerdo", "SmallInt", Convert.ToInt16(ddlTipoAcuerdo.SelectedItem.Value));
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                    dt.Rows.Add("@monedaAcuerdo", "Int", Convert.ToInt16(ddlMoneda.SelectedItem.Value));

                    dt.Rows.Add("@precioSinIVA", "Money", Convert.ToDecimal(lbPrecioSinIVA.Text));
                    dt.Rows.Add("@tipoPaqueteDiseno", "SmallInt", Convert.ToInt16(ddlTipoPaquete.SelectedItem.Value));
                    dt.Rows.Add("@fechaInicio", "Date", Convert.ToDateTime(txtFechaInicio.Text));
                    dt.Rows.Add("@fechaFin", "Date", Convert.ToDateTime(txtFechaTermino.Text));
                    //dt.Rows.Add("@fechaVenta", "Date", Convert.ToDateTime(txtFechaVenta.Text));

                    dt.Rows.Add("@cliente", "Int", Convert.ToInt32(ddlCliente.SelectedItem.Value));
                    dt.Rows.Add("@formaPago", "SmallInt", Convert.ToInt16(ddlFormaPago.SelectedItem.Value));
                    dt.Rows.Add("@comentariosAdicionales", "VarChar", txtComentarios.Text);
                    dt.Rows.Add("@controlAcuerdo", "VarChar", lbRutaOrden.Text /*savePath*/);
                    dt.Rows.Add("@folioFactura", "VarChar", txtFolioFactura.Text);

                    dt.Rows.Add("@cantidadFacturas", "SmallInt", Convert.ToInt16(ddlNumeroFacturas.Text));
                    dt.Rows.Add("@cantidadFacturados", "SmallInt", Convert.ToInt16(txtFacturados.Text));
                    dt.Rows.Add("@monedaFacturado", "SmallInt", Convert.ToInt16(ddlMonedaFacturado.SelectedItem.Value));
                    dt.Rows.Add("@importeFacturar", "Money", Convert.ToDecimal(txtImporteporFacturar.Text));
                    dt.Rows.Add("@montoFacturado", "Money", Convert.ToDecimal(txtMontoFacturado.Text));

                    dt.Rows.Add("@montoFacturadoMXN", "Money", Convert.ToDecimal(txtMontoFacturadoMXN.Text));
                    dt.Rows.Add("@montoCobrado", "Money", Convert.ToDecimal(txtMontoCobrado.Text));
                    if (txtFechaCobro.Text != "")
                        dt.Rows.Add("@fechaCobro", "Date", Convert.ToDateTime(txtFechaCobro.Text));
                    dt.Rows.Add("@estatusCXC", "SmallInt", Convert.ToInt16(ddlEstatusCXC.SelectedItem.Value));

                    if (ddlListado.SelectedItem.Text == "Corporativo")
                        dt.Rows.Add("@grupal", "Bit", true);
                    else
                        dt.Rows.Add("@grupal", "Bit", false);

                    dt.Rows.Add("@porcentajeIVA", "SmallInt", Convert.ToInt16(ddlIVA.SelectedItem.Value));
                    dt.Rows.Add("@archivoGenerado", "VarChar", lbRutaOrden.Text);
                    dt.Rows.Add("@comprobante", "SmallInt", Convert.ToInt16(ddlComprobante.SelectedItem.Value));
                    if (txtFechaFactura.Text != "")
                        dt.Rows.Add("@fechaFactura", "Date", Convert.ToDateTime(txtFechaFactura.Text));
                    dt.Rows.Add("@email", "VarChar", ddlEmail.Text);

                    /*dt.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);*/

                    DataTable dtUpd = new DataTable();
                    DataTable dtUpdDet = new DataTable();

                    dtUpd = action.EjecutarSP("spfctControlPublicidadUpd", dt);
                    DataRow rowUpd = dtUpd.Rows[0];

                    if (Convert.ToInt32(Session["idPerfil"]) != 5)
                    {
                        int valor = action.EjecutarQuery("UPDATE fctControlPublicidadServicioDet SET atBEstatus = 0 WHERE skNIdControlPublicidad = " + idControlP);

                        if ((Convert.ToInt16(ddlEquipo.SelectedValue) == 3 || Convert.ToInt16(ddlEquipo.SelectedValue) == 4) && ddlServicio.Visible)
                        {
                            dt = new DataTable();

                            dt.Columns.AddRange(
                                new DataColumn[3]
                                {
                                new DataColumn("atributo", typeof(string)),
                                new DataColumn("tipoDato", typeof(string)),
                                new DataColumn("valor", typeof(object))
                                }
                                );

                            dt.Rows.Add("@idControlPublicidad", "SmallInt", idControlP);
                            dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                            dt.Rows.Add("@id", "Int", Convert.ToInt32(ddlServicio.SelectedItem.Value));
                            dt.Rows.Add("@tipo", "Char", "U");
                            dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                            dt.Rows.Add("@clienteCaptura", "VarChar", txtCliente.Text);

                            dtUpdDet = action.EjecutarSP("spfctControlPublicidadServicioDet", dt);

                            if (dtUpdDet.Rows.Count <= 0)
                            {
                                mensajeError(@"ERROR: No se pudo guardar la información recibida.");
                                return;
                            }
                        }
                        else
                        {
                            foreach (GridViewRow row in gvServicio.Rows)
                            {
                                if (row.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox check = (row.Cells[0].FindControl("cbSeleccionar") as CheckBox);
                                    if (check != null)
                                    {
                                        if (check.Checked)
                                        {
                                            dt = new DataTable();

                                            dt.Columns.AddRange(
                                                new DataColumn[3]
                                                {
                                                new DataColumn("atributo", typeof(string)),
                                                new DataColumn("tipoDato", typeof(string)),
                                                new DataColumn("valor", typeof(object))
                                                }
                                                );

                                            dt.Rows.Add("@idControlPublicidad", "SmallInt", idControlP);
                                            dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                                            dt.Rows.Add("@id", "Int", Convert.ToInt32(row.Cells[2].Text));
                                            dt.Rows.Add("@tipo", "Char", "U");
                                            dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                                            dt.Rows.Add("@clienteCaptura", "VarChar", txtCliente.Text);

                                            dtUpdDet = action.EjecutarSP("spfctControlPublicidadServicioDet", dt);

                                            if (dtUpdDet.Rows.Count <= 0)
                                            {
                                                mensajeError(@"ERROR: No se pudo guardar la información recibida");
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!action.Eliminar("spfctControlPublicidadDel", Convert.ToInt32(idControlP), "@idControlPublicidad", 2, Convert.ToInt32(Session["idUsuario"])))
                            ModelState.AddModelError("Control de Publicidad", errorMessage: "ERROR: No se pudo realizar la acción solicitada");
                    }
                    Response.Redirect("~/Documents/Outlet/outletConsulta", false);
                }
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        protected void cbOrden_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOrden.Checked)
                upOrden.Visible = true;
            else
            {
                upOrden.Visible = false;
                lbRutaOrden.Text = "";
            }
        }

        protected void cbFactura_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFactura.Checked)
                upFactura.Visible = true;
            else
            {
                upFactura.Visible = false;
                lbRutaFactura.Text = "";
            }
        }

        protected void cbPago_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPago.Checked)
                upPago.Visible = true;
            else
            {
                upPago.Visible = false;
                lbRutaPago.Text = "";
            }
        }

        protected void ddlFormaPago_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt16(ddlFormaPago.SelectedValue) == 11)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar("SELECT skNIdEmail, atSEmail FROM dimEquipoEmail WHERE atBEstatus = 1 AND skNIdEquipo = " + Convert.ToInt16(ddlEquipo.SelectedItem.Value));

                ddlEmail.DataTextField = "atSEmail";
                ddlEmail.DataValueField = "skNIdEmail";
                ddlEmail.DataSource = dt;
                ddlEmail.DataBind();

                lbEmail.Visible = true;
                ddlEmail.Visible = true;
            }
            else
            {
                lbEmail.Visible = false;
                ddlEmail.Visible = false;
                //ddlEmail.Text = "";
            }
        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

            upContactos.Visible = false;
            if (ddlCliente.SelectedItem.Value == "0")
            {
                lbCliente.Visible = true;
                txtCliente.Visible = true;
            }
            else
            {
                lbCliente.Visible = false;
                txtCliente.Visible = false;
            }

            upOrden.Visible = true;
            lbRutaOrden.Text = "";

            if (ddlEquipo.SelectedItem.Value == "5")
            {
                cbOrden.Checked = true;
                cbOrden.Visible = true;
                lbOrden.Visible = true;
                cbFactura.Visible = true;
                lbFactura.Visible = true;
                cbPago.Visible = true;
                lbPago.Visible = true;
            }

            if (equipo != 5)
            {
                lbPais.Visible = false;
                ddlPais.Visible = false;
                lbTipoAcuerdo.Visible = true;
                ddlTipoAcuerdo.Visible = true;
                lbTipoPaquete.Visible = true;
                ddlTipoPaquete.Visible = true;
                lbClienteRFC.Visible = true;
                ddlCliente.Visible = true;
                gvHotel.Visible = true;
                lbFiltrar.Visible = true;
                ddlListado.Visible = true;
                ddlServicio.Visible = true;
                lbIDHotel.Visible = true;
                txtServicio.Visible = true;
                lbMensaje1.Visible = true;
                lbMensaje2.Visible = true;
                btnBuscar.Visible = true;

                if (equipo == 3 || equipo == 4)
                {
                    lbFiltrar.Visible = true;
                    ddlServicio.Visible = true;
                    lbIDHotel.Visible = false;
                    txtServicio.Visible = false;
                    lbMensaje1.Visible = false;
                    lbMensaje2.Visible = false;
                    btnBuscar.Visible = false;
                    gvHotel.Visible = false;
                }
                else if (equipo != 5)
                {
                    lbFiltrar.Visible = true;
                    if (ddlListado.SelectedItem.Value == "1")
                    {
                        ddlServicio.Visible = true;
                        lbIDHotel.Visible = false;
                        txtServicio.Visible = false;
                        lbMensaje1.Visible = false;
                        lbMensaje2.Visible = false;
                    }
                    else
                    {
                        ddlServicio.Visible = false;
                        lbIDHotel.Visible = true;
                        txtServicio.Visible = true;
                        lbMensaje1.Visible = true;
                        lbMensaje2.Visible = true;
                    }
                    btnBuscar.Visible = true;
                    gvHotel.Visible = true;

                    if (equipo == 1)
                        ddlMoneda.SelectedValue = "44";
                    if (equipo == 2)
                        ddlMoneda.SelectedValue = "109";
                }

                llenarListaServicio();
            }
            else
            {
                lbPais.Visible = true;
                ddlPais.Visible = true;
                lbTipoAcuerdo.Visible = false;
                ddlTipoAcuerdo.Visible = false;
                lbTipoPaquete.Visible = false;
                ddlTipoPaquete.Visible = false;
                lbClienteRFC.Visible = false;
                ddlCliente.Visible = false;
                gvHotel.Visible = false;
                lbFiltrar.Visible = false;
                ddlListado.Visible = false;
                ddlServicio.Visible = false;
                lbIDHotel.Visible = false;
                txtServicio.Visible = false;
                lbMensaje1.Visible = false;
                lbMensaje2.Visible = false;
                btnBuscar.Visible = false;
            }
            llenarCliente();
            llenarListado();
        }

        protected void ddlCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCliente.SelectedItem.Value == "0")
            {
                lbCliente.Visible = true;
                txtCliente.Visible = true;
            }
            else
            {
                lbCliente.Visible = false;
                txtCliente.Visible = false;
                txtCliente.Text = "";

                /*if (txtNombreC.Text == "" && txtNombreM.Text == "" && txtCargoC.Text == "" && txtCargoM.Text == "" &&
                    txtEmailC.Text == "" && txtEmailM.Text == "" && txtTelefonoC.Text == "" && txtTelefonoM.Text == "")
                {*/
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT TOP 1 atSNombreContabilidad, atSCargoContabilidad, atSEmailContabilidad, atSTelefonoContabilidad, atSNombreMarketing, atSCargoMarketing, atSEmailMarketing, atSTelefonoMarketing " +
                    "FROM dbo.dimContactoCliente " +
                    "WHERE skNIdCliente = " + ddlCliente.SelectedItem.Value);

                DataRow row = dt.Rows[0];

                txtNombreC.Text = row[0].ToString();
                txtCargoC.Text = row[1].ToString();
                txtEmailC.Text = row[2].ToString();
                txtTelefonoC.Text = row[3].ToString();
                txtNombreM.Text = row[4].ToString();
                txtCargoM.Text = row[5].ToString();
                txtEmailM.Text = row[6].ToString();
                txtTelefonoM.Text = row[7].ToString();
            }
        }

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServicio.Text = "";
            gvHotel.PageIndex = 0;
        }

        protected void txtServicio_TextChanged(object sender, EventArgs e)
        {
            ddlServicio.Text = "0";
            gvHotel.PageIndex = 0;
        }

        protected void gvHotel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex != -1)
            {
                gvServicio.PageIndex = e.NewPageIndex;
                gvServicio.DataSource = Session["Hotel"].ToString();
                llenarDatos();
            }
        }

        protected void gvGrupoCanal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex != -1)
            {
                gvGrupoCanal.PageIndex = e.NewPageIndex;
                //llenarGrupoCanal();
            }
        }

        protected void gvGrupoCanal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text != "")
                    total += Convert.ToDecimal(e.Row.Cells[2].Text);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = total.ToString();
            }
        }

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

            if (equipo == 3 || equipo == 4)
            {
                GridViewRow row = gvServicio.Rows[0];

                //llenarCliente();
                //llenarListado();
                llenarListaServicio();
                ddlServicio.SelectedValue = row.Cells[2].Text;

                lbFiltrar.Visible = true;
                lbIDHotel.Visible = false;
                txtServicio.Visible = false;
                ddlServicio.Visible = true;
                gvHotel.Visible = false;
                lbMensaje1.Visible = false;
                lbMensaje2.Visible = false;
                btnBuscar.Visible = false;
                btnAgregar.Visible = false;

            }
            else
            {
                llenarListaServicio();

                lbFiltrar.Visible = true;
                lbIDHotel.Visible = true;
                txtServicio.Visible = true;
                ddlServicio.Visible = true;
                gvHotel.Visible = true;
                lbMensaje1.Visible = true;
                lbMensaje2.Visible = true;
                btnBuscar.Visible = true;
                btnAgregar.Visible = true;

                DataTable dt = new DataTable();

                dt = action.Consultar(
                    "SELECT DISTINCT skNIdHotel IdHotel, atsNombreHotel Hotel "
                    + "FROM dbo.dimHotel dH "
                    + "INNER JOIN fctControlPublicidadServicioDet fCPS ON fCPS.skNId = dH.skNIdHotel AND fCPS.atsTipoServicio = 'Hotel' "
                    + "WHERE dh.atBEstatus = 1 "
                    + "AND fCPS.skNIdControlPublicidad = " + idControlP + " "
                    + "ORDER BY atsNombreHotel");

                gvHotel.DataSource = dt;
                gvHotel.DataBind();

                CheckBox check;
                foreach (GridViewRow row in gvHotel.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        check = (CheckBox)row.Cells[0].FindControl("cbSeleccionar");
                        if (check != null)
                            check.Checked = true;
                    }
                }
            }

            ddlListado.Visible = true;
            ddlServicio.Visible = true;
            gvServicio.Visible = false;
            btnCancelar.Visible = true;
            btnModificar.Visible = false;
            ddlEquipo.Enabled = true;
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //llenarDatos();
            lbFiltrar.Visible = false;
            ddlServicio.Visible = false;
            ddlListado.Visible = false;
            btnBuscar.Visible = false;
            lbIDHotel.Visible = false;
            txtServicio.Visible = false;
            lbMensaje1.Visible = false;
            lbMensaje2.Visible = false;
            gvServicio.Visible = true;
            gvHotel.Visible = false;
            btnBuscar.Visible = false;
            btnCancelar.Visible = false;
            btnModificar.Visible = true;
            btnAgregar.Visible = false;
            ddlEquipo.Enabled = false;
            ddlEquipo.SelectedValue = lbEquipo.Text;
            ddlMoneda.SelectedValue = lbMoneda.Text;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            int valor = action.EjecutarQuery("UPDATE fctControlPublicidadServicioDetBack SET atBEstatus = 0 WHERE skNIdControlPublicidad = " + idControlP);

            CheckBox check;
            foreach (GridViewRow row in gvHotel.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    check = (CheckBox)row.Cells[0].FindControl("cbSeleccionar");
                    if (check != null && check.Checked)
                    {
                        DataTable dt = new DataTable();
                        DataTable dtUpdDet = new DataTable();

                        dt.Columns.AddRange(
                            new DataColumn[3]
                            {
                                new DataColumn("atributo", typeof(string)),
                                new DataColumn("tipoDato", typeof(string)),
                                new DataColumn("valor", typeof(object))
                            }
                            );

                        dt.Rows.Add("@idControlPublicidad", "SmallInt", idControlP);
                        dt.Rows.Add("@tipoServicio", "VarChar", "Hotel");
                        dt.Rows.Add("@id", "Int", Convert.ToInt32(row.Cells[1].Text));
                        dt.Rows.Add("@tipo", "Char", "U");
                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                        dtUpdDet = action.EjecutarSP("spfctControlPublicidadServicioDetBack", dt);

                        if (dtUpdDet.Rows.Count <= 0)
                        {
                            mensajeError(@"ERROR: No se pudo guardar la información recibida");
                            return;
                        }
                    }
                }
            }

            //lbFiltrar.Visible = false;
            ddlServicio.Visible = false;
            btnBuscar.Visible = false;
            lbIDHotel.Visible = false;
            txtServicio.Visible = false;
            lbMensaje1.Visible = false;
            lbMensaje2.Visible = false;
            gvServicio.Visible = true;
            gvHotel.Visible = false;
            btnBuscar.Visible = false;
            btnCancelar.Visible = false;
            btnModificar.Visible = true;
            btnAgregar.Visible = false;
            //ddlEquipo.Enabled = false;

            llenarDatos();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            if (tipoAcuerdo == "1")
                Response.Redirect("~/Documents/outlet/outletConsulta", false);
            else
                Response.Redirect("~/Documents/Destinos/destinosConsulta", false);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenarDatos();
        }

        protected void btnOrden_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            //string saveDir = @"Documents\ControlAcuerdos\";
            //string appPath = Request.PhysicalApplicationPath;

            if (fuSubirOrden.HasFile == true)
            {
                //nombreArchivo = Path.GetFileNameWithoutExtension(fuSubirAcuerdo.FileName);
                extensionArchivo = Path.GetExtension(fuSubirOrden.FileName).ToLower();

                //savePath = appPath + saveDir + Server.HtmlEncode(fuSubirAcuerdo.FileName);

                if (extensionArchivo == ".pdf" || extensionArchivo == ".gif" || extensionArchivo == ".png" || extensionArchivo == ".bmp" ||
                    extensionArchivo == ".dib" || extensionArchivo == ".jpg" || extensionArchivo == ".jpe" || extensionArchivo == ".jpeg" ||
                    extensionArchivo == ".tif" || extensionArchivo == ".jfif" || extensionArchivo == ".tiff")
                {
                    if (fuSubirOrden.PostedFile.ContentLength < 10000000)
                    {
                        nombreArchivo = Regex.Replace(Path.GetFileName(fuSubirOrden.FileName), "[^ a-zA-Z0-9-._]", "", RegexOptions.None);
                        fuSubirOrden.SaveAs(Server.MapPath("~/Documents/Files/ControlPublicidad/") + nombreArchivo);
                        lbRutaOrden.Text = nombreArchivo;

                        fuSubirOrden.Enabled = false;
                        btnOrden.Enabled = false;

                        lbSubirOrden.Text = "Archivo precargado.";
                    }
                    else
                        lbSubirOrden.Text = "Tamaño de archivo no válido, excede 4MB!";
                }
                else
                    lbSubirOrden.Text = "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                        "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
            }
            else
                lbSubirOrden.Text = "Archivo no válido para cargar.";

            lbSubirOrden.Visible = true;
        }

        protected void btnFactura_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            lbSubirFactura.Text = "";

            try
            {
                if (fuFactura.HasFile == true)
                {
                    extensionArchivo = Path.GetExtension(fuFactura.FileName).ToLower();

                    if (extensionArchivo == ".pdf" || extensionArchivo == ".gif" || extensionArchivo == ".png" || extensionArchivo == ".bmp" ||
                        extensionArchivo == ".dib" || extensionArchivo == ".jpg" || extensionArchivo == ".jpe" || extensionArchivo == ".jpeg" ||
                        extensionArchivo == ".tif" || extensionArchivo == ".jfif" || extensionArchivo == ".tiff")
                    {
                        if (fuFactura.PostedFile.ContentLength < 10000000)
                        {
                            nombreArchivo = Regex.Replace(Path.GetFileName(fuFactura.FileName), "[^ a-zA-Z0-9-._]", "", RegexOptions.None);
                            fuFactura.SaveAs(Server.MapPath("~/Documents/Files/Destinos/Factura/") + nombreArchivo);

                            lbRutaFactura.Text = nombreArchivo;

                            fuFactura.Enabled = false;
                            btnFactura.Enabled = false;

                            lbSubirFactura.Text = "Archivo precargado.";
                        }
                        else
                            lbSubirFactura.Text = "Tamaño de archivo no válido, excede 4MB!";

                    }
                    else
                        lbSubirFactura.Text = "Extensión de archivo no válido, solo se aceptan archivos: <br> " +
                            "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
                }
                else
                    lbSubirFactura.Text = "Tipo de archivo no válido para cargar.";
            }

            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        protected void btnPago_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            lbSubirPago.Text = "";

            try
            {
                if (fuPago.HasFile == true)
                {
                    extensionArchivo = Path.GetExtension(fuPago.FileName).ToLower();

                    if (extensionArchivo == ".pdf" || extensionArchivo == ".gif" || extensionArchivo == ".png" || extensionArchivo == ".bmp" ||
                        extensionArchivo == ".dib" || extensionArchivo == ".jpg" || extensionArchivo == ".jpe" || extensionArchivo == ".jpeg" ||
                        extensionArchivo == ".tif" || extensionArchivo == ".jfif" || extensionArchivo == ".tiff")
                    {
                        if (fuPago.PostedFile.ContentLength < 10000000)
                        {
                            nombreArchivo = Regex.Replace(Path.GetFileName(fuPago.FileName), "[^ a-zA-Z0-9-._]", "", RegexOptions.None);
                            fuPago.SaveAs(Server.MapPath("~/Documents/Files/Destinos/ComprobantePago/") + nombreArchivo);

                            lbRutaPago.Text = nombreArchivo;

                            fuPago.Enabled = false;
                            btnPago.Enabled = false;

                            lbSubirPago.Text = "Archivo precargado.";
                        }
                        else
                            lbSubirPago.Text = "Tamaño de archivo no válido, excede 4MB!";

                    }
                    else
                        lbSubirPago.Text = "Extensión de archivo no válido, solo se aceptan archivos: <br> " +
                            "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
                }
                else
                    lbSubirPago.Text = "Tipo de archivo no válido para cargar.";
            }

            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        public string validar()
        {
            string valido = "";

            if (Convert.ToDateTime(txtFechaInicio.Text) > Convert.ToDateTime(txtFechaTermino.Text) || (txtFechaInicio.Text == "" && txtFechaTermino.Text == ""))
            {
                valido = "Favor de verificar las fechas capturadas, fecha de inicio o fecha de venta no deben ser mayores a fecha de termino.";
                return valido;
            }

            if (lbSubirOrden.Text == "Tamaño de archivo no válido, excede 4MB!" ||
                lbSubirOrden.Text == "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                    "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'" ||
                lbSubirOrden.Text == "Archivo no válido para cargar.")
            {
                valido = "Favor de verificar, tipo de archivo no válido.";
                return valido;
            }


            if (btnCancelar.Visible && ddlEquipo.SelectedValue != "3")
            {
                CheckBox check;
                foreach (GridViewRow row in gvServicio.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        check = (CheckBox)row.Cells[0].FindControl("cbSeleccionar");
                        if (check != null && check.Checked)
                        {
                            valido = "";
                            break;
                        }
                        else
                            valido = "Detalle de Hotel, Crucero, Tour, etc., no válido.";
                    }
                }
            }
            return valido;
        }

        private void llenarDatos()
        {
            DataTable dt = new DataTable();

            if (ddlServicio.Visible)
            {
                if ((txtServicio.Text.Trim()) != "")
                {
                    string Hotel = txtServicio.Text;
                    string[] captura = Hotel.Split(',');
                    string condicion = "";

                    if (captura.Count() == 1)
                        condicion = "atsNombreHotel + CAST(skNIdHotel AS varchar) LIKE ('%" + Hotel.Trim() + "%') ";
                    else
                    {
                        foreach (string palabra in captura)
                            condicion = condicion + "atsNombreHotel + CAST(skNIdHotel AS varchar) LIKE ('%" + palabra.Trim() + "%') OR ";

                        condicion = condicion.Substring(0, condicion.Length - 4);
                    }

                    dt = action.Consultar(
                        "SELECT skNIdHotel IdHotel, atsNombreHotel Hotel "
                        + "FROM dbo.dimHotel "
                        + "WHERE atBEstatus = 1 "
                        + "AND "
                        + condicion
                        + "ORDER BY atsNombreHotel");
                }
                else
                {
                    idCorporativo = Convert.ToInt32(ddlServicio.SelectedItem.Value);

                    dt = action.Consultar(
                        "SELECT TOP 1000 skNIdHotel IdHotel, atsNombreHotel Hotel "
                        + "FROM dbo.dimHotel "
                        + "WHERE atBEstatus = 1 "
                            + "AND skNIdCorporativo = " + Convert.ToString(idCorporativo)
                        + " ORDER BY atsNombreHotel");
                }

                gvHotel.DataSource = dt;
                gvHotel.DataBind();
            }

            else
            {
                dt = new DataTable();

                dt.Columns.AddRange(
                    new DataColumn[3]
                    {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                    }
                    );

                dt.Rows.Add("@idControlPublicidad", "SmallInt", Convert.ToInt16(idControlP));
                dt.Rows.Add("@opcion", "SmallInt", 2);
                dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                dt = action.EjecutarSP("spfctControlPublicidadConsulta", dt);

                gvServicio.DataSource = dt;
                gvServicio.DataBind();
            }

            CheckBox check;
            foreach (GridViewRow row in gvServicio.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    check = (CheckBox)row.Cells[0].FindControl("cbSeleccionar");
                    if (check != null)
                        check.Checked = true;
                }
            }
        }

        private void llenarCliente()
        {
            DataTable dt = new DataTable();
            DataTable dtSP = new DataTable();

            dtSP.Columns.AddRange(
                columns: new DataColumn[3]
                {
                    new DataColumn("atributo", typeof(string)),
                    new DataColumn("tipoDato", typeof(string)),
                    new DataColumn("valor", typeof(object))
                }
                );

            dtSP.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
            dtSP.Rows.Add("@servicio", "VarChar", "cliente");
            dtSP.Rows.Add("@catalogo", "SmallInt", 0);
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlCliente.DataTextField = "atSNombre";
            ddlCliente.DataValueField = "skNIdCliente";
            ddlCliente.DataSource = dt;
            ddlCliente.DataBind();
        }

        private void llenarListado()
        {
            DataTable dt = new DataTable();
            DataTable dtSP = new DataTable();

            dtSP.Columns.AddRange(
                columns: new DataColumn[3]
                {
                    new DataColumn("atributo", typeof(string)),
                    new DataColumn("tipoDato", typeof(string)),
                    new DataColumn("valor", typeof(object))
                }
                );

            dtSP.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
            dtSP.Rows.Add("@servicio", "VarChar", "servicio");
            dtSP.Rows.Add("@catalogo", "SmallInt", 0);
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlListado.DataTextField = "atSNombreListado";
            ddlListado.DataValueField = "skNIdListado";
            ddlListado.DataSource = dt;
            ddlListado.DataBind();
        }

        private void llenarListaServicio()
        {
            DataTable dt = new DataTable();
            DataTable dtSP = new DataTable();

            dtSP.Columns.AddRange(
                columns: new DataColumn[3]
                {
                    new DataColumn("atributo", typeof(string)),
                    new DataColumn("tipoDato", typeof(string)),
                    new DataColumn("valor", typeof(object))
                }
                );

            dtSP.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
            dtSP.Rows.Add("@servicio", "VarChar", "listado");
            dtSP.Rows.Add("@catalogo", "SmallInt", Convert.ToInt16(ddlListado.SelectedItem.Value));
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlServicio.DataTextField = "Nombre";
            ddlServicio.DataValueField = "Id";
            ddlServicio.DataSource = dt;
            ddlServicio.DataBind();
        }

        protected void ddlListado_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlListado.SelectedItem.Text == "Individual")
            {
                ddlServicio.Enabled = false;
                txtServicio.Enabled = true;
                ddlServicio.Visible = false;
                lbIDHotel.Visible = true;
                txtServicio.Visible = true;
                lbMensaje1.Visible = true;
                lbMensaje2.Visible = true;
            }
            else
            {
                ddlServicio.Enabled = true;
                txtServicio.Enabled = false;
                ddlServicio.Visible = true;
                lbIDHotel.Visible = false;
                txtServicio.Visible = false;
                lbMensaje1.Visible = false;
                lbMensaje2.Visible = false;
                llenarListaServicio();
            }

            txtServicio.Text = "";
            gvHotel.PageIndex = 0;
        }

        protected void txtMonto_TextChanged(object sender, EventArgs e)
        {
            //var value = "";
            decimal suma = 0;
            TextBox txtMonto;

            foreach (GridViewRow row in gvGrupoCanal.Rows)
            {
                txtMonto = (row.Cells[0].FindControl("txtMonto") as TextBox);
                //value = (sender as TextBox).Text;
                if (txtMonto.Text != "")
                    suma += Convert.ToDecimal(txtMonto.Text);
                //value += Convert.ToDecimal((sender as TextBox).Text);
            }

            lbPrecioSinIVA.Text = Convert.ToString(suma * Convert.ToDecimal(1.00));
        }

        private void mensajeError(string Mensaje)
        {
            string script = @"<script type='text/javascript'>
                            alert('{0}');
                        </script>";

            script = string.Format(script, Mensaje);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AVISO", script, false);
        }

        private void validarPerfil(int idPerfil)
        {
            if (idPerfil == 1 || idPerfil == 4)
            {
                phBasico.Visible = true;
                phComplemento.Visible = true;
                if (tipoAcuerdo == "5")
                {
                    phFinanzas.Visible = false;
                    phFinanzasDestino.Visible = true;
                }
                else
                {
                    phFinanzas.Visible = true;
                    phFinanzasDestino.Visible = false;
                }
            }
            else if (idPerfil == 3)
            {
                phBasico.Visible = true;
                phComplemento.Visible = true;
                phFinanzas.Visible = false;
                phFinanzasDestino.Visible = false;
            }
            else if (idPerfil == 5)
            {
                phBasico.Visible = false;
                phComplemento.Visible = false;
                if (tipoAcuerdo == "5")
                {
                    phFinanzas.Visible = false;
                    phFinanzasDestino.Visible = true;
                }
                else
                {
                    phFinanzas.Visible = true;
                    phFinanzasDestino.Visible = false;
                }
            }
        }
    }
}