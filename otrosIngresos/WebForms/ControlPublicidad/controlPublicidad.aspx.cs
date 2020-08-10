using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using otrosIngresos.Shared;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace otrosIngresos.Documents
{
    public partial class controlPublicidad : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        int idCorporativo = 0;
        decimal total = 0;
        string ruta = "";
        //string savePath;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("~/Account/Login.aspx");
                else
                {
                    if (Convert.ToInt32(Session["idPerfil"]) == 5 || Convert.ToInt32(Session["idPerfil"]) == 6)
                        Response.Redirect("~/Default.aspx");
                    else
                        lbSesion.Text = Convert.ToString(Session["nombre"]) + " (" + Convert.ToString(Session["perfil"]) + ")  ";
                }

                lbPrecioSinIVA.Text = "0.00";
                DataTable dt = new DataTable();
                //dt = action.Consultar("SELECT * FROM dbo.dimCorporativo WHERE atBEstatus = 1 ORDER BY atSNombreCorporativo");
                dt = action.Consultar("SELECT * FROM dbo.dimEquipo WHERE atBEstatus = 1");

                ddlEquipo.DataTextField = "atSNombreEquipo";
                ddlEquipo.DataValueField = "skNIdEquipo";
                ddlEquipo.DataSource = dt;
                ddlEquipo.DataBind();

                llenarListasDesplegables();
                llenarListaServicio();
                llenarDatos();

                llenarGrupoCanal();

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 1)
                    ddlMoneda.SelectedValue = "44";

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 2)
                    ddlMoneda.SelectedValue = "109";

                if (rbSubir.Checked)
                {
                    upAcuerdo.Visible = true;
                    upContactos.Visible = false;
                    //btnVistaPrevia.Visible = false;
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
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlEquipo.SelectedItem.Value == "5")
                {
                    DataTable dtMedia = new DataTable();

                    dtMedia.Columns.AddRange(
                        columns: new DataColumn[3]
                        {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                        }
                        );

                    dtMedia.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                    dtMedia.Rows.Add("@pais", "SmallInt", Convert.ToInt16(ddlPais.SelectedItem.Value));
                    dtMedia.Rows.Add("@nombreCliente", "VarChar", Regex.Replace(txtCliente.Text, "[^ a-zA-Z0-9-._]", "", RegexOptions.None));
                    dtMedia.Rows.Add("@razonSocial", "VarChar", txtRazonSocial.Text);
                    dtMedia.Rows.Add("@ordenInsercion", "VarChar", lbRutaOrden.Text);

                    dtMedia.Rows.Add("@ingresoSinIVA", "Money", Convert.ToDecimal(lbPrecioSinIVA.Text));
                    dtMedia.Rows.Add("@ingresoConIVA", "Money", Convert.ToDecimal(Convert.ToDouble(lbPrecioSinIVA.Text) * (1 + (Convert.ToDouble(ddlIVA.SelectedItem.Value) / 100))));
                    dtMedia.Rows.Add("@moneda", "Int", Convert.ToInt16(ddlMoneda.SelectedItem.Value));
                    dtMedia.Rows.Add("@ingresoSinIVAMXN", "Money", Convert.ToDecimal(lbPrecioSinIVA.Text));
                    dtMedia.Rows.Add("@estatus", "SmallInt", Convert.ToInt16(ddlEstatus.SelectedItem.Value));

                    if (txtFechaFactura.Text != "")
                        dtMedia.Rows.Add("@fechaFactura", "Date", Convert.ToDateTime(txtFechaFactura.Text));
                    dtMedia.Rows.Add("@folioFactura", "VarChar", "");
                    if (txtFechaCobro.Text != "")
                        dtMedia.Rows.Add("@fechaPago", "Date", Convert.ToDateTime(txtFechaCobro.Text));
                    dtMedia.Rows.Add("@archivoFactura", "VarChar", lbRutaFactura.Text);
                    dtMedia.Rows.Add("@archivoPago", "VarChar", lbRutaPago.Text);

                    dtMedia.Rows.Add("@comentariosAdicionales", "VarChar", txtComentarios.Text);
                    dtMedia.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);
                    dtMedia.Rows.Add("@cantidadFacturas", "SmallInt", Convert.ToInt16(ddlNumeroFacturas.SelectedItem.Value));
                    dtMedia.Rows.Add("@porcentajeIVA", "SmallInt", Convert.ToInt16(ddlIVA.SelectedItem.Value));
                    dtMedia.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                    dtMedia = action.EjecutarSP("spfctDestinoIns", dtMedia);

                    if (dtMedia.Rows.Count > 0)
                    {
                        DataRow r = dtMedia.Rows[0];
                        int idD = Convert.ToInt32(r[0].ToString());

                        foreach (GridViewRow row in gvGrupoCanal.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                TextBox txtMonto = (row.Cells[0].FindControl("txtMonto") as TextBox);
                                Label lbID = (row.Cells[0].FindControl("ID") as Label);

                                if (txtMonto.Text != null)//&& txtMonto.Text != "" && Convert.ToDecimal(txtMonto.Text) != 0)
                                {
                                    dtMedia = new DataTable();

                                    dtMedia.Columns.AddRange(
                                        new DataColumn[3]
                                        {
                                            new DataColumn("atributo", typeof(string)),
                                            new DataColumn("tipoDato", typeof(string)),
                                            new DataColumn("valor", typeof(object))
                                        }
                                        );

                                    dtMedia.Rows.Add("@idControlPublicidad", "SmallInt", idD);
                                    dtMedia.Rows.Add("@idGrupoCanal", "SmallInt", lbID.Text);
                                    dtMedia.Rows.Add("@tipo", "SmallInt", 5);
                                    if (txtMonto.Text != "")
                                        dtMedia.Rows.Add("@monto", "Money", Convert.ToDecimal(txtMonto.Text));
                                    else
                                        dtMedia.Rows.Add("@monto", "Money", 0);
                                    dtMedia.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                                    dtMedia = action.EjecutarSP("spfctControlPublicidadGrupoCanalDet", dtMedia);

                                    /*if (dtMedia.Rows.Count <= 0)
                                    {
                                        mensajeError(@"ERROR: No se pudo guardar el detalle del canal de la orden de inserción.");
                                        return;
                                    }*/
                                }
                            }
                        }
                        Response.Redirect("~/WebForms/Destinos/destinosConsulta", false);
                    }
                    else
                        mensajeError(@"ERROR: No se pudo guardar la orden de inserción.");
                }

                else
                {
                    string valido = validar();
                    if (valido != "")
                    {
                        mensajeError(@"ERROR: " + valido);
                        return;
                    }

                    //lbPrecioSinIVA.Text = txtPrecioSinIVA.Text;
                    string tipoServicio = "";

                    if (rbGenerar.Checked)
                        preparaPDF("Guardar");

                    if (ddlListado.SelectedItem.Text == "Corporativo" || ddlListado.SelectedItem.Text == "Individual")
                        tipoServicio = "Hotel";
                    else
                        tipoServicio = ddlListado.SelectedItem.Text;

                    DataTable dt = new DataTable();

                    dt.Columns.AddRange(
                        columns: new DataColumn[3]
                        {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                        }
                        );

                    int idCP = 0;

                    dt.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                    dt.Rows.Add("@mes", "SmallInt", Convert.ToInt16(ddlMes.SelectedItem.Value));
                    dt.Rows.Add("@tipoAcuerdo", "SmallInt", Convert.ToInt16(ddlTipoAcuerdo.SelectedItem.Value));
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                    dt.Rows.Add("@monedaAcuerdo", "Int", Convert.ToInt16(ddlMoneda.SelectedItem.Value));

                    dt.Rows.Add("@precioSinIVA", "Money", Convert.ToDouble(lbPrecioSinIVA.Text));
                    dt.Rows.Add("@tipoPaqueteDiseno", "SmallInt", Convert.ToInt16(ddlTipoPaquete.SelectedItem.Value));
                    dt.Rows.Add("@fechaInicio", "Date", Convert.ToDateTime(txtFechaInicio.Text));
                    dt.Rows.Add("@fechaFin", "Date", Convert.ToDateTime(txtFechaTermino.Text));
                    dt.Rows.Add("@fechaVenta", "Date", System.DateTime.Now/*Convert.ToDateTime(txtFechaVenta.Text)*/);

                    dt.Rows.Add("@cliente", "Int", Convert.ToInt32(ddlCliente.SelectedItem.Value));
                    dt.Rows.Add("@formaPago", "SmallInt", Convert.ToInt16(ddlFormaPago.SelectedItem.Value));
                    dt.Rows.Add("@comentariosAdicionales", "VarChar", txtComentarios.Text);
                    if (rbSubir.Checked)
                        dt.Rows.Add("@controlAcuerdo", "VarChar", lbRutaOrden.Text /*savePath*/);
                    else
                        dt.Rows.Add("@controlAcuerdo", "VarChar", "");
                    dt.Rows.Add("@folioFactura", "VarChar", "");

                    dt.Rows.Add("@cantidadFacturas", "SmallInt", Convert.ToInt16(ddlNumeroFacturas.SelectedItem.Value));
                    dt.Rows.Add("@cantidadFacturados", "SmallInt", 0);
                    dt.Rows.Add("@monedaFacturado", "SmallInt", Convert.ToInt16(ddlMonedaFacturado.SelectedItem.Value));
                    dt.Rows.Add("@importeFacturar", "Money", 0);
                    dt.Rows.Add("@montoFacturado", "Money", 0);

                    dt.Rows.Add("@montoFacturadoMXN", "Money", 0);
                    dt.Rows.Add("@montoCobrado", "Money", 0);
                    dt.Rows.Add("@estatusCXC", "SmallInt", Convert.ToInt16(ddlEstatusCXC.SelectedItem.Value));
                    if (ddlListado.SelectedItem.Text == "Corporativo")
                        dt.Rows.Add("@grupal", "Bit", true);
                    else
                        dt.Rows.Add("@grupal", "Bit", false);
                    dt.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);

                    dt.Rows.Add("@porcentajeIVA", "SmallInt", Convert.ToInt16(ddlIVA.SelectedItem.Value));
                    if (rbGenerar.Checked)
                        dt.Rows.Add("@archivoGenerado", "VarChar", lbRutaOrden.Text);
                    else
                        dt.Rows.Add("@archivoGenerado", "VarChar", "");
                    dt.Rows.Add("@comprobante", "SmallInt", Convert.ToInt16(ddlComprobante.SelectedItem.Value));
                    dt.Rows.Add("@fechaFactura", "Date", Convert.ToDateTime(txtFechaFactura.Text));
                    dt.Rows.Add("@email", "VarChar", Convert.ToInt16(ddlEmail.SelectedItem.Value));

                    dt.Rows.Add("@archivoDetalle", "VarChar", lbRutaOrdenDet.Text);

                    dt = action.EjecutarSP("spfctControlPublicidadIns", dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow r = dt.Rows[0];
                        idCP = Convert.ToInt32(r[0].ToString());

                        foreach (GridViewRow row in gvGrupoCanal.Rows)
                        {
                            if (row.RowType == DataControlRowType.DataRow)
                            {
                                TextBox txtMonto = (row.Cells[0].FindControl("txtMonto") as TextBox);
                                Label lbID = (row.Cells[0].FindControl("ID") as Label);

                                if (txtMonto.Text != null)//&& txtMonto.Text != "" && Convert.ToDecimal(txtMonto.Text) != 0)
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

                                    dt.Rows.Add("@idControlPublicidad", "SmallInt", idCP);
                                    dt.Rows.Add("@idGrupoCanal", "SmallInt", lbID.Text);
                                    dt.Rows.Add("@tipo", "SmallInt", 1);
                                    if (txtMonto.Text != "")
                                        dt.Rows.Add("@monto", "Money", Convert.ToDecimal(txtMonto.Text));
                                    else
                                        dt.Rows.Add("@monto", "Money", 0);
                                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                                    dt = action.EjecutarSP("spfctControlPublicidadGrupoCanalDet", dt);

                                    /*if (dt.Rows.Count <= 0)
                                    {
                                        mensajeError(@"ERROR: No se pudo guardar el detalle del canal de la orden de inserción.");
                                        return;
                                    }*/
                                }
                            }
                        }

                        if (Convert.ToInt16(ddlEquipo.SelectedValue) == 3 || Convert.ToInt16(ddlEquipo.SelectedValue) == 4)
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

                            dt.Rows.Add("@idControlPublicidad", "SmallInt", idCP);
                            dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                            dt.Rows.Add("@id", "Int", ddlServicio.SelectedItem.Value);
                            dt.Rows.Add("@tipo", "Char", "I");
                            dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                            dt.Rows.Add("@clienteCaptura", "VarChar", Regex.Replace(txtCliente.Text, "[^ a-zA-Z0-9-._]", "", RegexOptions.None));

                            dt = action.EjecutarSP("spfctControlPublicidadServicioDet", dt);

                            if (dt.Rows.Count <= 0)
                            {
                                mensajeError(@"ERROR: No se pudo guardar el detalle de la orden de inserción.");
                                return;
                            }
                        }
                        else
                        {
                            foreach (GridViewRow row in gvHotel.Rows)
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

                                            dt.Rows.Add("@idControlPublicidad", "SmallInt", idCP);
                                            dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                                            dt.Rows.Add("@id", "Int", Convert.ToInt32(row.Cells[1].Text));
                                            dt.Rows.Add("@tipo", "Char", "I");
                                            dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                                            dt.Rows.Add("@clienteCaptura", "VarChar", Regex.Replace(txtCliente.Text, "[^ a-zA-Z0-9-._]", "", RegexOptions.None));

                                            dt = action.EjecutarSP("spfctControlPublicidadServicioDet", dt);

                                            if (dt.Rows.Count <= 0)
                                            {
                                                mensajeError(@"ERROR: No se pudo guardar el detalle de la orden de inserción.");
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        dt = new DataTable();

                        dt.Columns.AddRange(
                            new DataColumn[3]
                            {
                            new DataColumn("atributo", typeof(string)),
                            new DataColumn("tipoDato", typeof(string)),
                            new DataColumn("valor", typeof(object))
                            }
                            );

                        dt.Rows.Add("@idControlPublicidad", "SmallInt", idCP);
                        dt.Rows.Add("@cliente", "Int", Convert.ToInt32(ddlCliente.SelectedItem.Value));
                        dt.Rows.Add("@nombreC", "VarChar", txtNombreC.Text);
                        dt.Rows.Add("@cargoC", "VarChar", txtCargoC.Text);
                        dt.Rows.Add("@emailC", "VarChar", txtEmailC.Text);
                        dt.Rows.Add("@telefonoC", "VarChar", txtTelefonoC.Text);
                        dt.Rows.Add("@nombreM", "VarChar", txtNombreM.Text);
                        dt.Rows.Add("@cargoM", "VarChar", txtCargoM.Text);
                        dt.Rows.Add("@emailM", "VarChar", txtEmailM.Text);
                        dt.Rows.Add("@telefonoM", "VarChar", txtTelefonoM.Text);
                        dt.Rows.Add("@destino", "VarChar", txtDestino.Text);
                        dt.Rows.Add("@razonSocial", "VarChar", txtRazonSocial.Text);
                        dt.Rows.Add("@rfc", "VarChar", txtRFCCliente.Text);
                        dt.Rows.Add("@direccionFiscal", "VarChar", txtDireccionFiscal.Text);
                        dt.Rows.Add("@cp", "VarChar", txtCodigoPostal.Text);
                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                        dt = action.EjecutarSP("spfctContactoClienteDet", dt);

                        if (dt.Rows.Count <= 0)
                        {
                            mensajeError(@"ERROR: No se pudo guardar el detalle de los contactos.");
                            return;
                        }

                        Response.Redirect("~/WebForms/ControlPublicidad/controlPublicidadConsulta", false);
                    }
                    else
                        mensajeError(@"ERROR: No se pudo guardar la orden de inserción.");
                }
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default", false);
        }

        protected void ddlListado_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar("SELECT skNIdListado, atNPorcentajeIVA FROM dimEquipoListado WHERE atBEstatus = 1 AND skNIdEquipo = 1");

                ddlListado.DataTextField = "atSNombreListado";
                ddlListado.DataValueField = "skNIdListado";
                ddlListado.DataSource = dt;
                ddlListado.DataBind();
            }
        }

        protected void ddlEmail_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar("SELECT skNIdEmail, atSEmail FROM dimEquipoEmail WHERE atBEstatus = 1 AND skNIdEquipo = " + Convert.ToInt16(ddlEquipo.SelectedItem.Value));

                ddlEmail.DataTextField = "atSEmail";
                ddlEmail.DataValueField = "skNIdEmail";
                ddlEmail.DataSource = dt;
                ddlEmail.DataBind();
            }
        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

            if (rbGenerar.Checked)
            {
                upContactos.Visible = true;
                if (ddlCliente.SelectedItem.Value == "0")
                {
                    lbCliente.Visible = true;
                    txtCliente.Visible = true;
                    upCliente.Visible = true;
                }
                else
                {
                    lbCliente.Visible = false;
                    txtCliente.Visible = false;
                    upCliente.Visible = false;
                }

                if (equipo == 5)
                {
                    lbDestino.Visible = true;
                    txtDestino.Visible = true;
                }
                else
                {
                    lbDestino.Visible = false;
                    txtDestino.Visible = false;
                }
                upAcuerdo.Visible = false;
                //btnVistaPrevia.Visible = true;
                lbRutaOrden.Text = "";
                cbOrden.Visible = false;
                lbOrden.Visible = false;
                upAcuerdo.Visible = false;
                cbFactura.Visible = false;
                lbFactura.Visible = false;
                upFactura.Visible = false;
                cbPago.Visible = false;
                lbPago.Visible = false;
                upPago.Visible = false;
            }

            if (rbSubir.Checked)
            {
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

                upAcuerdo.Visible = true;
                //btnVistaPrevia.Visible = false;
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

                llenarListasDesplegables();
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
                //ddlCliente.Visible = false;
                gvHotel.Visible = false;
                lbFiltrar.Visible = false;
                ddlListado.Visible = false;
                ddlServicio.Visible = false;
                lbIDHotel.Visible = false;
                txtServicio.Visible = false;
                lbMensaje1.Visible = false;
                lbMensaje2.Visible = false;
                btnBuscar.Visible = false;
                llenarListasDesplegables();
            }
        }

        /* ** DESTINOS ** */
        protected void ddlPais_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                System.DateTime moment = System.DateTime.Now;

                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdPais, atSCodigoPais + ' - ' + atSNombrePais Pais "
                    + "FROM dbo.dimPais "
                    + "WHERE atBEstatus = 1");

                ddlPais.DataTextField = "Pais";
                ddlPais.DataValueField = "skNIdPais";
                ddlPais.DataSource = dt;
                ddlPais.DataBind();
            }
        }

        protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (Convert.ToInt16(ddlPais.SelectedValue) == 159)
                ddlMoneda.SelectedValue = "109";
            else if (Convert.ToInt16(ddlPais.SelectedValue) == 55)
                ddlMoneda.SelectedValue = "44";

            if (ddlMoneda.SelectedValue == "44")
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 AND skNIdIVA IN(0,8,19)");
            else if (ddlMoneda.SelectedValue == "109")
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 AND skNIdIVA IN(0,16)");
            else
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 --AND skNIdIVA IN(0)");

            ddlIVA.DataTextField = "atNPorcentajeIVA";
            ddlIVA.DataValueField = "skNIdIVA";
            ddlIVA.DataSource = dt;
            ddlIVA.DataBind();
        }

        protected void ddlEstatus_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdEstatus, atSNombreEstatus "
                    + "FROM dbo.dimEstatus "
                    + "WHERE atBEstatus = 1");

                ddlEstatus.DataTextField = "atSNombreEstatus";
                ddlEstatus.DataValueField = "skNIdEstatus";
                ddlEstatus.DataSource = dt;
                ddlEstatus.DataBind();
            }
        }

        protected void cbOrden_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOrden.Checked)
                upAcuerdo.Visible = true;
            else
            {
                upAcuerdo.Visible = false;
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


        protected void ddlMes_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                System.DateTime moment = System.DateTime.Now;

                DataTable dt = new DataTable();
                dt = action.Consultar("SELECT * FROM dbo.dimMes");

                ddlMes.DataTextField = "atSNombreMes";
                ddlMes.DataValueField = "skNIdMes";
                ddlMes.DataSource = dt;
                ddlMes.DataBind();
                ddlMes.SelectedValue = Convert.ToString(moment.Month);
            }
        }

        protected void ddlIVA_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1");

                ddlIVA.DataTextField = "atNPorcentajeIVA";
                ddlIVA.DataValueField = "skNIdIVA";
                ddlIVA.DataSource = dt;
                ddlIVA.DataBind();
            }
        }

        protected void ddlTipoAcuerdo_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdTipoAcuerdo, atSNombreAcuerdo "
                    + "FROM dbo.dimTipoAcuerdo "
                    + "WHERE atBEstatus = 1 AND skNIdTipoAcuerdo NOT IN(1,2,3,7,8)");

                ddlTipoAcuerdo.DataTextField = "atSNombreAcuerdo";
                ddlTipoAcuerdo.DataValueField = "skNIdTipoAcuerdo";
                ddlTipoAcuerdo.DataSource = dt;
                ddlTipoAcuerdo.DataBind();
            }
        }

        protected void ddlMoneda_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdMoneda, atSMoneda + ' - ' + atSNombreMoneda atSMoneda "
                    + "FROM dbo.dimMoneda "
                    + "WHERE atBEstatus = 1");

                ddlMoneda.DataTextField = "atSMoneda";
                ddlMoneda.DataValueField = "skNIdMoneda";
                ddlMoneda.DataSource = dt;
                ddlMoneda.DataBind();
            }
        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();

            if (ddlMoneda.SelectedValue == "44")
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 AND skNIdIVA IN(0,8,19)");
            else if (ddlMoneda.SelectedValue == "109")
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 AND skNIdIVA IN(0,16)");
            else
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 --AND skNIdIVA IN(0)");

            ddlIVA.DataTextField = "atNPorcentajeIVA";
            ddlIVA.DataValueField = "skNIdIVA";
            ddlIVA.DataSource = dt;
            ddlIVA.DataBind();
        }

        protected void ddlTipoPaquete_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdTipoPaqueteDiseno, SUBSTRING(atSNombrePaqueteDiseno, 1, 50) atSNombrePaqueteDiseno "
                    + "FROM dbo.dimTipoPaqueteDiseno "
                    + "WHERE atBEstatus = 1");

                ddlTipoPaquete.DataTextField = "atSNombrePaqueteDiseno";
                ddlTipoPaquete.DataValueField = "skNIdTipoPaqueteDiseno";
                ddlTipoPaquete.DataSource = dt;
                ddlTipoPaquete.DataBind();
            }
        }

        protected void ddlFormaPago_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdFormaPago, atSNombreFormaPago "
                    + "FROM dbo.dimFormaPago "
                    + "WHERE atBEstatus = 1");

                ddlFormaPago.DataTextField = "atSNombreFormaPago";
                ddlFormaPago.DataValueField = "skNIdFormaPago";
                ddlFormaPago.DataSource = dt;
                ddlFormaPago.DataBind();
            }

            /*ListItem i;
            foreach (DataRow r in dt.Rows)
            {
                i = new ListItem(r["atSNombreFormaPago"].ToString(), r["skNIdFormaPago"].ToString());
                ddlFormaPago.Items.Add(i);
            }*/
        }

        protected void ddlMonedaFacturado_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdMoneda, atSMoneda + ' - ' + atSNombreMoneda atSMoneda "
                    + "FROM dbo.dimMoneda "
                    + "--WHERE atBEstatus = 1");

                ddlMonedaFacturado.DataTextField = "atSMoneda";
                ddlMonedaFacturado.DataValueField = "skNIdMoneda";
                ddlMonedaFacturado.DataSource = dt;
                ddlMonedaFacturado.DataBind();
            }
        }

        protected void ddlEstatusCXC_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdEstatusCXC, atSNombreEstatusCXC "
                    + "FROM dbo.dimEstatusCXC "
                    + "WHERE atBEstatus = 1");

                ddlEstatusCXC.DataTextField = "atSNombreEstatusCXC";
                ddlEstatusCXC.DataValueField = "skNIdEstatusCXC";
                ddlEstatusCXC.DataSource = dt;
                ddlEstatusCXC.DataBind(); 
            }
        }

        protected void ddlComprobante_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdTipoComprobante, atSNombreComprobante "
                    + "FROM dbo.dimTipoComprobante "
                    + "WHERE atBEstatus = 1");

                ddlComprobante.DataTextField = "atSNombreComprobante";
                ddlComprobante.DataValueField = "skNIdTipoComprobante";
                ddlComprobante.DataSource = dt;
                ddlComprobante.DataBind();
            }
        }

        protected void ddlNumeroFacturas_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();
                dt = action.Consultar(
                    "SELECT skNIdNumeroFacturas, atSNumeroFacturas "
                    + "FROM dbo.dimNumeroFacturas "
                    + "WHERE atBEstatus = 1");

                ddlNumeroFacturas.DataTextField = "atSNumeroFacturas";
                ddlNumeroFacturas.DataValueField = "skNIdNumeroFacturas";
                ddlNumeroFacturas.DataSource = dt;
                ddlNumeroFacturas.DataBind();
            }
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
                    "SELECT TOP 1 " +
                    "   dCC.atSNombreContabilidad, " +
                    "   dCC.atSCargoContabilidad, " +
                    "   dCC.atSEmailContabilidad, " +
                    "   dCC.atSTelefonoContabilidad, " +
                    "   dCC.atSNombreMarketing, " +
                    "   dCC.atSCargoMarketing, " +
                    "   dCC.atSEmailMarketing, " +
                    "   dCC.atSTelefonoMarketing, " +
                    "   ISNULL(" +
                    "       CASE " +
                    "           WHEN dC.atSNombreCliente = '' THEN '-' " +
                    "           ELSE dC.atSNombreCliente " +
                    "       END, '-') [Cliente], " +
                    "   dC.atSRazonSocial [NombreFiscal], " +
                    "   ISNULL(" +
                    "       CASE " +
                    "            WHEN dC.atSRFC = '' THEN '-' " +
                    "            ELSE dC.atSRFC " +
                    "        END, '-') [RFC], " +
                    "    dC.atSDomicilio [DireccionFiscal], " +
                    "    ISNULL(" +
                    "        CASE " +
                    "            WHEN dC.atSCodigoPostal = '' THEN '-' " +
                    "            ELSE dC.atSCodigoPostal " +
                    "        END, '-') [CP] " +
                    "FROM dimCliente dC " +
                    "INNER JOIN dbo.dimContactoCliente dCC ON dCC.skNIdCliente = dC.skNIdCliente " +
                    "WHERE dC.skNIdCliente = " + ddlCliente.SelectedItem.Value);

                DataRow row = dt.Rows[0];

                txtNombreC.Text = row[0].ToString();
                txtCargoC.Text = row[1].ToString();
                txtEmailC.Text = row[2].ToString();
                txtTelefonoC.Text = row[3].ToString();
                txtNombreM.Text = row[4].ToString();
                txtCargoM.Text = row[5].ToString();
                txtEmailM.Text = row[6].ToString();
                txtTelefonoM.Text = row[7].ToString();
                txtCliente.Text = row[8].ToString();
                txtRazonSocial.Text = row[9].ToString();
                txtRFCCliente.Text = row[10].ToString();
                txtDireccionFiscal.Text = row[11].ToString();
                txtCodigoPostal.Text = row[12].ToString();
                //}
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
                gvHotel.PageIndex = e.NewPageIndex;
                llenarDatos();
            }
        }

        protected void gvGrupoCanal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex != -1)
            {
                gvGrupoCanal.PageIndex = e.NewPageIndex;
                llenarGrupoCanal();
            }
        }

        protected void gvGrupoCanal_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if(e.Row.Cells[2].Text != "")
                    total += Convert.ToDecimal(e.Row.Cells[2].Text);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = total.ToString();
            }
        }

        /*protected void gvGrupoCanal_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbPrecioSinIVA.Text = gvGrupoCanal.SelectedRow.Cells[2].Text;
        }*/

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenarDatos();
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
            gvGrupoCanal.Focus();
        }

        protected void btnOrden_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            try
            {
                if (fuSubirOrden.HasFile)
                {
                    extensionArchivo = Path.GetExtension(fuSubirOrden.FileName).ToLower();

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
                        lbSubirOrden.Text = "Extensión de archivo no válido, solo se aceptan archivos: <br> " +
                            "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
                }
                else
                    // Notify the user that a file was not uploaded.
                    lbSubirOrden.Text = "No se ha especificado ningún archivo para cargar.";
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }
        
        protected void btnFactura_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            try
            {
                if (fuFactura.HasFile)
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
                    // Notify the user that a file was not uploaded.
                    lbSubirFactura.Text = "No se ha especificado ningún archivo para cargar.";
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

            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            try
            {
                if (fuPago.HasFile)
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
                            
                            /*// Call a helper method routine to save the file.
                            //SaveFile(fuPago.PostedFile);
                            string path = System.AppDomain.CurrentDomain.BaseDirectory;
                            string ruta = path + "App_Data\\pathFile.txt";
                            StreamReader sr = new StreamReader(ruta, Encoding.ASCII);
                            string file1 = sr.ReadToEnd();
                            string[] pathFile = file1.Split('|');

                            // Specify the path to save the uploaded file to.
                            //string savePath = "c:\\temp\\uploads\\";
                            //string savePath = pathFile[1] + "ComprobantePago\\";
                            string savePath = "~\\Documents\\Files\\Destinos\\ComprobantePago\\";

                            // Get the name of the file to upload.
                            string fileName = fuPago.FileName;

                            // Create the path and file name to check for duplicates.
                            string pathToCheck = savePath + fileName;

                            // Create a temporary file name to use for checking duplicates.
                            string tempfileName = "";

                            // Check to see if a file already exists with the
                            // same name as the file to upload.        
                            if (System.IO.File.Exists(pathToCheck))
                            {
                                int counter = 2;
                                while (System.IO.File.Exists(pathToCheck))
                                {
                                    // if a file with this name already exists,
                                    // prefix the filename with a number.
                                    tempfileName = fileName + " (" + counter.ToString() + ")";
                                    pathToCheck = savePath + tempfileName;
                                    counter++;
                                }

                                fileName = tempfileName;

                                // Notify the user that the file name was changed.
                                lbSubirPago.Text = "Existe un archivo con el mismo nombre." +
                                    "<br />El archivo fue guardado como: " + fileName;
                                    
                        }
                            else
                            {
                                // Notify the user that the file was saved successfully.
                                lbSubirPago.Text = "Archivo precargado.";
                            }

                            // Append the name of the file to upload to the path.
                            savePath += fileName;
                            lbRutaPago.Text = fileName;

                            // Call the SaveAs method to save the uploaded
                            // file to the specified directory.
                            fuPago.SaveAs(savePath);
                            */
                        }
                        else
                            lbSubirPago.Text = "Tamaño de archivo no válido, excede 4MB!";
                    }
                    else
                        lbSubirPago.Text = "Extensión de archivo no válido, solo se aceptan archivos: <br> " +
                            "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
                }
                else
                    // Notify the user that a file was not uploaded.
                    lbSubirPago.Text = "No se ha especificado ningún archivo para cargar.";
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        public string validar()
        {
            string valido = "";

            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

            if (equipo != 5)
            {
                if (Convert.ToDateTime(txtFechaInicio.Text) > Convert.ToDateTime(txtFechaTermino.Text) || (txtFechaInicio.Text == "" && txtFechaTermino.Text == ""))
                //|| Convert.ToDateTime(txtFechaVenta.Text) > Convert.ToDateTime(txtFechaTermino.Text))
                {
                    valido = "Favor de verificar las fechas capturadas, fecha de inicio o fecha de venta no deben ser mayores a fecha de termino.";
                    return valido;
                }

                if (rbSubir.Checked)
                {
                    if (lbSubirOrden.Text != "Archivo precargado.")
                    {
                        valido = "Favor de verificar que haya seleccionado un archivo válido a cargar.";
                        return valido;
                    }
                }

                if (rbGenerar.Checked)
                {
                    if (txtNombreC.Text == "" || txtNombreM.Text == "" || txtCargoC.Text == "" || txtCargoM.Text == "" ||
                        txtEmailC.Text == "" || txtEmailM.Text == "" || txtTelefonoC.Text == "" || txtTelefonoM.Text == "")
                    {
                        valido = "Favor de capturar información válida de los contactos de contabilidad y marketing.";
                        return valido;
                    }
                }

                if (ddlListado.SelectedItem.Text == "Corporativo" || ddlListado.SelectedItem.Text == "Individual")
                {
                    CheckBox check;
                    if (gvHotel.Rows.Count > 0)
                    {
                        foreach (GridViewRow row in gvHotel.Rows)
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
                    else
                        valido = "Detalle de Hotel, Crucero, Tour, etc., no válido.";
                }
            }
            return valido;
        }

        private void llenarDatos()
        {
            DataTable dt = new DataTable();

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
                    "SELECT TOP 100 skNIdHotel IdHotel, atsNombreHotel Hotel "
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
                    "SELECT TOP 100 skNIdHotel IdHotel, atsNombreHotel Hotel "
                    + "FROM dbo.dimHotel "
                    + "WHERE atBEstatus = 1 "
                        + "AND skNIdCorporativo = " + Convert.ToString(idCorporativo)
                    + " ORDER BY atsNombreHotel");
            }

            gvHotel.DataSource = dt;
            gvHotel.DataBind();
        }

        private void llenarGrupoCanal()
        {
            DataTable dt = new DataTable();

            dt = action.Consultar(
                    "SELECT skNIdGrupoCanal ID, atSNombreGrupoCanal Canal "
                    + "FROM dbo.dimGrupoCanal "
                    + "WHERE atBEstatus = 1 ");

            gvGrupoCanal.DataSource = dt;
            gvGrupoCanal.DataBind();
        }

        private void llenarListasDesplegables()
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

            dtSP.Rows[1]["valor"] = "servicio";

            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlListado.DataTextField = "atSNombreListado";
            ddlListado.DataValueField = "skNIdListado";
            ddlListado.DataSource = dt;
            ddlListado.DataBind();

            if (ddlMoneda.SelectedValue == "44")
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 AND skNIdIVA IN(0,8,19)");
            else if (ddlMoneda.SelectedValue == "109")
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 AND skNIdIVA IN(0,16)");
            else
                dt = action.Consultar("SELECT skNIdIVA, atNPorcentajeIVA FROM dbo.dimIVA WHERE atBEstatus = 1 --AND skNIdIVA IN(0)");

            ddlIVA.DataTextField = "atNPorcentajeIVA";
            ddlIVA.DataValueField = "skNIdIVA";
            ddlIVA.DataSource = dt;
            ddlIVA.DataBind();

            dt = action.Consultar("SELECT skNIdEmail, atSEmail FROM dimEquipoEmail WHERE atBEstatus = 1 AND skNIdEquipo = " + Convert.ToInt16(ddlEquipo.SelectedItem.Value));

            ddlEmail.DataTextField = "atSEmail";
            ddlEmail.DataValueField = "skNIdEmail";
            ddlEmail.DataSource = dt;
            ddlEmail.DataBind();
        }

        private void llenarListaServicio()
        {
            DataTable dt = new DataTable();
            DataTable dtSP = new DataTable();
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

            if (equipo != 5)
            {
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

        private void mensajeError(string Mensaje)
        {
            string script = @"<script type='text/javascript'>
                            alert('{0}');
                        </script>";

            script = string.Format(script, Mensaje);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AVISO", script, false);
        }

        protected void rbGenerar_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGenerar.Checked)
            {
                //txtPrecioSinIVA.Visible = false;
                //lbPrecioSinIVA.Visible = true;
                upAcuerdo.Visible = false;
                upContactos.Visible = true;
                //lbIVA.Visible = true;
                //ddlIVA.Visible = true;
                //btnVistaPrevia.Visible = true;
                lbRutaOrden.Text = "";
                //gvGrupoCanal.Visible = true;
                cbOrden.Visible = false;
                lbOrden.Visible = false;
                upAcuerdo.Visible = false;
                cbFactura.Visible = false;
                lbFactura.Visible = false;
                upFactura.Visible = false;
                cbPago.Visible = false;
                lbPago.Visible = false;
                upPago.Visible = false;
                upCliente.Visible = true;

                if (ddlEquipo.SelectedItem.Value == "5")
                {
                    lbDestino.Visible = true;
                    txtDestino.Visible = true;
                }
                else
                {
                    lbDestino.Visible = false;
                    txtDestino.Visible = false;
                }
            }
        }

        protected void rbSubir_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSubir.Checked)
            {
                //txtPrecioSinIVA.Visible = true;
                //lbPrecioSinIVA.Visible = true;
                upAcuerdo.Visible = true;
                upContactos.Visible = false;
                //lbIVA.Visible = false;
                //ddlIVA.Visible = false;
                //btnVistaPrevia.Visible = false;
                lbRutaOrden.Text = "";
                //gvGrupoCanal.Visible = false;
                upCliente.Visible = false;
                lbDestino.Visible = false;
                txtDestino.Visible = false;

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
                else
                {
                    cbOrden.Checked = false;
                    cbOrden.Visible = false;
                    lbOrden.Visible = false;
                    cbFactura.Visible = false;
                    lbFactura.Visible = false;
                    cbPago.Visible = false;
                    lbPago.Visible = false;
                }
            }
            
        }

        protected void cldrFechaTermino_OnSelectionChanged(object sender, EventArgs e)
        {
            txtFechaTermino.Text = "";
        }

        protected void ibFechaTermino_OnClick(object sender, EventArgs e)
        {
            txtFechaTermino.Text = "";
        }

        protected void cldrFechaInicio_OnSelectionChanged(object sender, EventArgs e)
        {
            txtFechaInicio.Text = "";
        }

        protected void ibFechaInicio_OnClick(object sender, EventArgs e)
        {
            txtFechaInicio.Text = "";
        }

        protected void btnVistaPrevia_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbGenerar.Checked)
                {
                    string valido = validar();
                    if (valido != "")
                    {
                        mensajeError(@"ERROR: " + valido);
                        return;
                    }
                    preparaPDF("VistaPrevia");
                }
                if (rbSubir.Checked)
                {
                    mensajeError(@"ERROR: No es posible generar una vista previa del archivo PDF.");
                    return;
                }
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        private void preparaPDF(string accion)
        {
            try
            {
                string filenameM, filenameD, outputFileM = "", outputFileD;
                DataTable dtSP = new DataTable();

                dtSP.Columns.AddRange(
                    columns: new DataColumn[3]
                    {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                    }
                    );

                dtSP.Rows.Add("@idCliente", "Int", Convert.ToInt32(ddlCliente.SelectedItem.Value));
                dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                dtSP.Rows.Add("@cliente", "VarChar", Regex.Replace(txtCliente.Text, "[^ a-zA-Z0-9-._]", "", RegexOptions.None));
                dtSP.Rows.Add("@nombreC", "VarChar", txtNombreC.Text);
                dtSP.Rows.Add("@cargoC", "VarChar", txtCargoC.Text);
                dtSP.Rows.Add("@emailC", "VarChar", txtEmailC.Text);
                dtSP.Rows.Add("@telefonoC", "VarChar", txtTelefonoC.Text);
                dtSP.Rows.Add("@nombreM", "VarChar", txtNombreM.Text);
                dtSP.Rows.Add("@cargoM", "VarChar", txtCargoM.Text);
                dtSP.Rows.Add("@emailM", "VarChar", txtEmailM.Text);
                dtSP.Rows.Add("@telefonoM", "VarChar", txtTelefonoM.Text);
                dtSP.Rows.Add("@email", "VarChar", ddlEmail.Text);

                dtSP = action.EjecutarSP("spfctControlPublicidadPDF", dtSP);

                if (dtSP.Rows.Count > 0 && ddlEquipo.SelectedItem.Value == "5")
                {
                    dtSP.Rows[0]["Cliente"] = Regex.Replace(txtCliente.Text, "[^ a-zA-Z0-9-._]", "", RegexOptions.None);
                    dtSP.Rows[0]["NombreFiscal"] = txtRazonSocial.Text;
                    dtSP.Rows[0]["RFC"] = txtRFCCliente.Text;
                    dtSP.Rows[0]["DireccionFiscal"] = txtDireccionFiscal.Text;
                    dtSP.Rows[0]["CP"] = txtCodigoPostal.Text;
                }

                DataTable dt = new DataTable();


                dt.Columns.AddRange(
                    columns: new DataColumn[18]
                    {
                        new DataColumn("DestinoHotel", typeof(string)),
                        new DataColumn("tipoPaquete", typeof(string)),
                        new DataColumn("vigencia", typeof(string)),
                        new DataColumn("subtotal", typeof(string)),
                        new DataColumn("IVA", typeof(string)),
                        new DataColumn("total", typeof(string)),
                        new DataColumn("comentario", typeof(string)),
                        new DataColumn("pagoDescuento", typeof(string)),
                        new DataColumn("pagoDeposito", typeof(string)),
                        new DataColumn("usuario", typeof(string)),
                        new DataColumn("fechaCreacion", typeof(string)),
                        new DataColumn("comprobante", typeof(string)),
                        new DataColumn("area", typeof(string)),
                        new DataColumn("moneda", typeof(string)),
                        new DataColumn("numeroFacturas", typeof(string)),
                        new DataColumn("iva", typeof(string)),
                        new DataColumn("formafago", typeof(string)),
                        new DataColumn("fechaFactura", typeof(string))
                    }
                    );

                string ids = "", nombreCliente= "";

                if (ddlEquipo.SelectedItem.Value == "5" && txtDestino.Text != "")
                    ids = txtDestino.Text;
                else if (ddlEquipo.SelectedItem.Value == "5" && txtDestino.Text == "")
                    ids = "N/A";
                else
                {
                    int c = 0;
                    if (ddlListado.SelectedItem.Text == "Corporativo" || ddlListado.SelectedItem.Text == "Individual")
                    {
                        if (gvHotel.Rows.Count > 0)
                        {
                            foreach (GridViewRow r in gvHotel.Rows)
                            {
                                if (r.RowType == DataControlRowType.DataRow)
                                {
                                    CheckBox check = (r.Cells[0].FindControl("cbSeleccionar") as CheckBox);
                                    if (check != null)
                                    {
                                        if (check.Checked)
                                        {
                                            if (c == 0)
                                            {
                                                ids = r.Cells[1].Text;
                                                nombreCliente = r.Cells[2].Text;
                                            }
                                            else
                                            {
                                                ids = ids + ", " + r.Cells[1].Text;
                                                nombreCliente = nombreCliente + ", " + r.Cells[2].Text;
                                            }
                                            c = c + 1;
                                        }
                                    }
                                }
                            }

                            if (ids.Length > 30)
                                ids = "VARIOS";

                            if (nombreCliente.Length > 100)
                                nombreCliente = "VARIOS";
                        }
                    }
                    else
                        ids = ddlServicio.SelectedItem.Value;

                    dtSP.Rows[0]["Cliente"] = nombreCliente;
                }

                string deposito = "", descuento = "";
                if (ddlFormaPago.SelectedItem.Text == "Descuento")
                    descuento = "X";
                else if (ddlFormaPago.SelectedItem.Text == "Depósito o Transferencia")
                    deposito = "X";

                DateTime fechaI = Convert.ToDateTime(txtFechaInicio.Text);
                DateTime fechaF = Convert.ToDateTime(txtFechaTermino.Text);
                DateTime fechaFac = Convert.ToDateTime(txtFechaFactura.Text);

                dt.Rows.Add(
                    ids,
                    ddlTipoPaquete.SelectedItem.Text,
                    fechaI.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX")) + " al " 
                        + fechaF.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX")),
                    lbPrecioSinIVA.Text,
                    Convert.ToString(Convert.ToDouble(lbPrecioSinIVA.Text) * (Convert.ToDouble(ddlIVA.SelectedItem.Value) / 100)),
                    Convert.ToString(Convert.ToDouble(lbPrecioSinIVA.Text) * (1 + (Convert.ToDouble(ddlIVA.SelectedItem.Value) / 100))),
                    txtComentarios.Text,
                    descuento,
                    deposito,
                    Convert.ToString(Session["nombre"]),
                    System.DateTime.Now,
                    ddlComprobante.SelectedItem.Text,
                    ddlEquipo.SelectedItem.Text,
                    ddlMoneda.SelectedItem.Text,
                    ddlNumeroFacturas.SelectedItem.Text,
                    ddlIVA.SelectedItem.Text,
                    ddlFormaPago.SelectedItem.Text,
                    fechaFac.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX"))
                    );

                DataTable dtgv = new DataTable();
                TextBox txtMonto;
                dtgv.Columns.AddRange(
                    columns: new DataColumn[3]
                    {
                        new DataColumn("ID", typeof(string)),
                        new DataColumn("Canal", typeof(string)),
                        new DataColumn("Monto", typeof(string))
                    }
                    );


                foreach (GridViewRow r in gvGrupoCanal.Rows)
                {
                    txtMonto = (r.Cells[0].FindControl("txtMonto") as TextBox);
                    dtgv.Rows.Add(r.Cells[0].Text, r.Cells[1].Text, txtMonto.Text);
                }

                filenameM = generarPDF(dtSP, dt, dtgv, accion, 1);
                outputFileM = this.Server.MapPath("~/Documents/Files/ControlPublicidad/" + filenameM);
                filenameD = generarPDF(dtSP, dt, dtgv, accion, 2);
                outputFileD = this.Server.MapPath("~/Documents/Files/ControlPublicidad/" + filenameD);

                // Process.Start(Server.MapPath("~/Documents/Files/ControlPublicidad/" + filename));
                //abrimos el PDF con la aplicacion por defecto
                //Process.Start(outputFile);
                ///*
                //Response.Clear();
                if (accion == "VistaPrevia")
                {
                    Server.ClearError();
                    Response.ContentType = "application/pdf";
                    Response.BufferOutput = true;
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.AddHeader("content-disposition", "attachment; filename=" + filenameM);
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.WriteFile(outputFileM);
                    //Response.Write(outputFileM);
                    //Response.End();
                    Response.Flush();
                    Response.Close();
                }
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        public string generarPDF(DataTable dtSP, DataTable dt, DataTable dtgv, string accion, int i)
        {
            string nombreArchivo = "";
            string outputFile = this.Server.MapPath("~/Documents/Files/ControlPublicidad/" + nombreArchivo);
            DataRow rowSP = dtSP.Rows[0];
            DataRow row = dt.Rows[0];
            DataRow rowgv = dtgv.Rows[0];
            string phrase = "";

            //archivo temporal
            //string outputFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";

            if (i == 1)
            {
                nombreArchivo = Regex.Replace(rowSP[25].ToString(), "[^ a-zA-Z0-9-._]", "", RegexOptions.None) + "_" + System.DateTime.Now.ToString("yyyyMMdd") + "_Montos.pdf";
                outputFile = this.Server.MapPath("~/Documents/Files/ControlPublicidad/" + nombreArchivo);
                lbRutaOrden.Text = nombreArchivo;
            }
            else
            {
                nombreArchivo = Regex.Replace(rowSP[25].ToString(), "[^ a-zA-Z0-9-._]", "", RegexOptions.None) + "_" + System.DateTime.Now.ToString("yyyyMMdd") + "_Detalle.pdf";
                outputFile = this.Server.MapPath("~/Documents/Files/ControlPublicidad/" + nombreArchivo);
                lbRutaOrdenDet.Text = nombreArchivo;
            }


            //Create a standard .Net FileStream for the file, setting various flags
            //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))

            using (FileStream fs = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                //Create a new PDF document setting the size to LETTER - tamaño CARTA
                //using (Document pdfDoc = new Document(PageSize.LETTER, 40f, 20f, 20f, 20f))

                using (Document pdfDoc = new Document(PageSize.LETTER, 30f, 15f, 15, 15))
                //using (Document doc = new Document(rec))
                {
                    //Bind the PDF document to the FileStream using an iTextSharp PdfWriter
                    using (PdfWriter w = PdfWriter.GetInstance(pdfDoc, fs))
                    {

                        //margenes fuera del limite normal a lo ancho -50f
                        //pdfDoc.SetMargins(-50f, -50f, 10f, 10f);
                        //Open the document for writing
                        iTextSharp.text.Font _standardFont5 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 11, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        //Font arial = FontFactory.GetFont("Arial", 8, BaseColor.GRAY);
                        Font pinkPT = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD, new BaseColor(237, 21, 86));
                        Font standard = new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.GRAY);
                        Font standard2 = new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.GRAY);
                        Font standard3 = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD, BaseColor.GRAY);
                        Font standard4 = new Font(Font.FontFamily.HELVETICA, 6, Font.BOLD, BaseColor.GRAY);
                        Font standard5 = new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, BaseColor.GRAY);
                        Font standard6 = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD, BaseColor.GRAY);
                        Font standard7 = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD, BaseColor.WHITE);
                        Font standard8 = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.GRAY);
                        Font standard9 = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.GRAY);
                        //Font calibri = FontFactory.GetFont("Calibri", 8, BaseColor.GRAY);

                        pdfDoc.Open();

                        //Agregamos  el texto que esta dentro de la etiqueta
                        //Font LineBreak = FontFactory.GetFont("Arial", size: 16);

                        // Header Image
                        string imageURL = Server.MapPath("~/Images/Logonew_smart.png");
                        //string imageURL = "~/Images/sign_in_logo2.png";
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageURL);
                        img.ScaleToFit(140f, 80f);
                        img.SpacingBefore = 0;
                        img.SetAbsolutePosition(30, 750);

                        pdfDoc.Add(img);

                        // Paragraph
                        Paragraph pHeader = new Paragraph(string.Format("ORDEN DE PUBLICIDAD"), _standardFont5);
                        pHeader.SpacingBefore = 30;
                        pHeader.SpacingAfter = 0;
                        pHeader.Alignment = 1; //0-Left, 1 middle,2 Right
                        pdfDoc.Add(pHeader);

                        // Tabla Datos Cliente
                        PdfPTable tblCliente = new PdfPTable(21);
                        tblCliente.WidthPercentage = 100;

                        PdfPCell clClienteHeader = new PdfPCell(new Phrase("INFORMACIÓN DEL CLIENTE", standard6));
                        clClienteHeader.Colspan = 21;
                        clClienteHeader.BorderWidth = 0;
                        clClienteHeader.BorderWidthBottom = 0.1f;
                        clClienteHeader.BorderColorBottom = new BaseColor(237, 21, 86);
                        clClienteHeader.HorizontalAlignment = 1;
                        clClienteHeader.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteHeader);

                        PdfPCell clClienteC = new PdfPCell(new Phrase("Cliente:", standard));
                        clClienteC.Colspan = 3;
                        //clClienteC.MinimumHeight = 10f;
                        clClienteC.BorderWidth = 0;
                        clClienteC.BorderWidthBottom = 0.1f;
                        clClienteC.HorizontalAlignment = 0;
                        clClienteC.VerticalAlignment = 1;
                        clClienteC.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteC);

                        PdfPCell clClienteT1 = new PdfPCell(new Phrase(rowSP[1].ToString(), standard));
                        clClienteT1.Colspan = 8;
                        clClienteT1.BorderWidth = 0;
                        clClienteT1.BorderWidthBottom = 0.1f;
                        clClienteT1.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT1);

                        PdfPCell clClienteV1 = new PdfPCell(new Phrase("", standard));
                        clClienteV1.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV1);

                        PdfPCell clClientePA = new PdfPCell(new Phrase("Provider Account:", standard));
                        clClientePA.Colspan = 3;
                        clClientePA.BorderWidth = 0;
                        clClientePA.BorderWidthBottom = 0.1f;
                        clClientePA.HorizontalAlignment = 0;
                        clClientePA.VerticalAlignment = 1;
                        clClientePA.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClientePA);

                        PdfPCell clClienteT2 = new PdfPCell(new Phrase(rowSP[0].ToString(), standard));
                        clClienteT2.Colspan = 6;
                        clClienteT2.BorderWidth = 0;
                        clClienteT2.BorderWidthBottom = 0.1f;
                        clClienteT2.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT2);

                        PdfPCell clClienteNF = new PdfPCell(new Phrase("Nombre Fiscal:", standard));
                        clClienteNF.Colspan = 3;
                        clClienteNF.BorderWidth = 0;
                        clClienteNF.BorderWidthBottom = 0.1f;
                        clClienteNF.HorizontalAlignment = 0;
                        clClienteNF.VerticalAlignment = 1;
                        clClienteNF.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteNF);

                        PdfPCell clClienteT3 = new PdfPCell(new Phrase(rowSP[2].ToString(), standard));
                        clClienteT3.Colspan = 8;
                        clClienteT3.BorderWidth = 0;
                        clClienteT3.BorderWidthBottom = 0.1f;
                        clClienteT3.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT3);

                        PdfPCell clClienteV2 = new PdfPCell(new Phrase("", standard));
                        clClienteV2.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV2);

                        PdfPCell clClienteD = new PdfPCell(new Phrase("ID:", standard));
                        clClienteD.Colspan = 3;
                        clClienteD.BorderWidth = 0;
                        clClienteD.BorderWidthBottom = 0.1f;
                        clClienteD.HorizontalAlignment = 0;
                        clClienteD.VerticalAlignment = 1;
                        clClienteD.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteD);

                        PdfPCell clClienteT4 = new PdfPCell(new Phrase(row[0].ToString(), standard));
                        clClienteT4.Colspan = 6;
                        clClienteT4.BorderWidth = 0;
                        clClienteT4.BorderWidthBottom = 0.1f;
                        clClienteT4.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT4);

                        PdfPCell clClienteDF = new PdfPCell(new Phrase("Dirección Fiscal:", standard));
                        clClienteDF.Colspan = 3;
                        clClienteDF.BorderWidth = 0;
                        clClienteDF.BorderWidthBottom = 0.1f;
                        clClienteDF.HorizontalAlignment = 0;
                        clClienteDF.VerticalAlignment = 1;
                        clClienteDF.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteDF);

                        PdfPCell clClienteT5 = new PdfPCell(new Phrase(rowSP[4].ToString(), standard));
                        clClienteT5.Colspan = 8;
                        clClienteT5.BorderWidth = 0;
                        clClienteT5.BorderWidthBottom = 0.1f;
                        clClienteT5.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT5);

                        PdfPCell clClienteV3 = new PdfPCell(new Phrase("", standard));
                        clClienteV3.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV3);

                        PdfPCell clClienteRFC = new PdfPCell(new Phrase("R.F.C.:", standard));
                        clClienteRFC.Colspan = 3;
                        clClienteRFC.BorderWidth = 0;
                        clClienteRFC.BorderWidthBottom = 0.1f;
                        clClienteRFC.HorizontalAlignment = 0;
                        clClienteRFC.VerticalAlignment = 1;
                        clClienteRFC.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteRFC);

                        PdfPCell clClienteT6 = new PdfPCell(new Phrase(rowSP[3].ToString(), standard));
                        clClienteT6.Colspan = 6;
                        clClienteT6.BorderWidth = 0;
                        clClienteT6.BorderWidthBottom = 0.1f;
                        clClienteT6.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT6);

                        PdfPCell clClienteCP = new PdfPCell(new Phrase("C.P.:", standard));
                        clClienteCP.Colspan = 3;
                        clClienteCP.BorderWidth = 0;
                        clClienteCP.BorderWidthBottom = 0.1f;
                        clClienteCP.HorizontalAlignment = 0;
                        clClienteCP.VerticalAlignment = 1;
                        clClienteCP.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteCP);

                        PdfPCell clClienteT7 = new PdfPCell(new Phrase(rowSP[5].ToString(), standard));
                        clClienteT7.Colspan = 8;
                        clClienteT7.BorderWidth = 0;
                        clClienteT7.BorderWidthBottom = 0.1f;
                        clClienteT7.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT7);

                        PdfPCell clClienteV4 = new PdfPCell(new Phrase("", standard));
                        clClienteV4.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV4);

                        PdfPCell clClienteCo = new PdfPCell(new Phrase("Comprobante:", standard));
                        clClienteCo.Colspan = 3;
                        clClienteCo.BorderWidth = 0;
                        clClienteCo.BorderWidthBottom = 0.1f;
                        clClienteCo.HorizontalAlignment = 0;
                        clClienteCo.VerticalAlignment = 1;
                        clClienteCo.BackgroundColor = new BaseColor(216, 217, 219);
                        tblCliente.AddCell(clClienteCo);

                        PdfPCell clClienteT8 = new PdfPCell(new Phrase(row[11].ToString(), standard));
                        clClienteT8.Colspan = 6;
                        clClienteT8.BorderWidth = 0;
                        clClienteT8.BorderWidthBottom = 0.1f;
                        clClienteT8.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT8);

                        tblCliente.SpacingBefore = 10;
                        pdfDoc.Add(tblCliente);

                        // Tabla Datos Contactos
                        PdfPTable tblContacto = new PdfPTable(21);
                        tblContacto.WidthPercentage = 100;

                        PdfPCell clContactoHeader = new PdfPCell(new Phrase("CONTACTOS DEL CLIENTE", standard6));
                        clContactoHeader.Colspan = 21;
                        clContactoHeader.BorderWidth = 0;
                        clContactoHeader.BorderWidthBottom = 0.1f;
                        clContactoHeader.BorderColorBottom = new BaseColor(237, 21, 86);
                        clContactoHeader.HorizontalAlignment = 1;
                        clContactoHeader.VerticalAlignment = 1;
                        tblContacto.AddCell(clContactoHeader);

                        PdfPCell clContactoC = new PdfPCell(new Phrase("Contacto Contabilidad", standard));
                        clContactoC.Colspan = 10;
                        clContactoC.BorderWidth = 0;
                        clContactoC.HorizontalAlignment = 1;
                        tblContacto.AddCell(clContactoC);

                        PdfPCell clContactoV1 = new PdfPCell(new Phrase("", standard));
                        clContactoV1.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV1);

                        PdfPCell clContactoM = new PdfPCell(new Phrase("Contacto Marketing", standard));
                        clContactoM.Colspan = 10;
                        clContactoM.BorderWidth = 0;
                        clContactoM.HorizontalAlignment = 1;
                        tblContacto.AddCell(clContactoM);

                        PdfPCell clContactoN = new PdfPCell(new Phrase("Nombre:", standard));
                        clContactoN.Colspan = 3;
                        clContactoN.BorderWidth = 0;
                        clContactoN.BorderWidthBottom = 0.1f;
                        clContactoN.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoN);

                        PdfPCell clContactoT1 = new PdfPCell(new Phrase(rowSP[6].ToString(), standard));
                        clContactoT1.Colspan = 8;
                        clContactoT1.BorderWidth = 0;
                        clContactoT1.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT1);

                        PdfPCell clContactoV2 = new PdfPCell(new Phrase("", standard));
                        clContactoV2.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV2);

                        PdfPCell clContactoN2 = new PdfPCell(new Phrase("Nombre:", standard));
                        clContactoN2.Colspan = 3;
                        clContactoN2.BorderWidth = 0;
                        clContactoN2.BorderWidthBottom = 0.1f;
                        clContactoN2.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoN2);

                        PdfPCell clContactoT2 = new PdfPCell(new Phrase(rowSP[7].ToString(), standard));
                        clContactoT2.Colspan = 6;
                        clContactoT2.BorderWidth = 0;
                        clContactoT2.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT2);

                        PdfPCell clContactoCR = new PdfPCell(new Phrase("Cargo:", standard));
                        clContactoCR.Colspan = 3;
                        clContactoCR.BorderWidth = 0;
                        clContactoCR.BorderWidthBottom = 0.1f;
                        clContactoCR.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoCR);

                        PdfPCell clContactoT3 = new PdfPCell(new Phrase(rowSP[8].ToString(), standard));
                        clContactoT3.Colspan = 8;
                        clContactoT3.BorderWidth = 0;
                        clContactoT3.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT3);

                        PdfPCell clContactoV3 = new PdfPCell(new Phrase("", standard));
                        clContactoV3.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV3);

                        PdfPCell clContactoCR2 = new PdfPCell(new Phrase("Cargo:", standard));
                        clContactoCR2.Colspan = 3;
                        clContactoCR2.BorderWidth = 0;
                        clContactoCR2.BorderWidthBottom = 0.1f;
                        clContactoCR2.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoCR2);

                        PdfPCell clContactoT4 = new PdfPCell(new Phrase(rowSP[9].ToString(), standard));
                        clContactoT4.Colspan = 6;
                        clContactoT4.BorderWidth = 0;
                        clContactoT4.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT4);

                        PdfPCell clContactoE = new PdfPCell(new Phrase("E-mail:", standard));
                        clContactoE.Colspan = 3;
                        clContactoE.BorderWidth = 0;
                        clContactoE.BorderWidthBottom = 0.1f;
                        clContactoE.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoE);

                        PdfPCell clContactoT5 = new PdfPCell(new Phrase(rowSP[10].ToString(), standard));
                        clContactoT5.Colspan = 8;
                        clContactoT5.BorderWidth = 0;
                        clContactoT5.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT5);

                        PdfPCell clContactoV4 = new PdfPCell(new Phrase("", standard));
                        clContactoV4.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV4);

                        PdfPCell clContactoE2 = new PdfPCell(new Phrase("E-mail:", standard));
                        clContactoE2.Colspan = 3;
                        clContactoE2.BorderWidth = 0;
                        clContactoE2.BorderWidthBottom = 0.1f;
                        clContactoE2.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoE2);

                        PdfPCell clContactoT6 = new PdfPCell(new Phrase(rowSP[11].ToString(), standard));
                        clContactoT6.Colspan = 6;
                        clContactoT6.BorderWidth = 0;
                        clContactoT6.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT6);

                        PdfPCell clContactoTL = new PdfPCell(new Phrase("Teléfono:", standard));
                        clContactoTL.Colspan = 3;
                        clContactoTL.BorderWidth = 0;
                        clContactoTL.BorderWidthBottom = 0.1f;
                        clContactoTL.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoTL);

                        PdfPCell clContactoT7 = new PdfPCell(new Phrase(rowSP[12].ToString(), standard));
                        clContactoT7.Colspan = 8;
                        clContactoT7.BorderWidth = 0;
                        clContactoT7.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT7);

                        PdfPCell clContactoV5 = new PdfPCell(new Phrase("", standard));
                        clContactoV5.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV5);

                        PdfPCell clContactoTL2 = new PdfPCell(new Phrase("Teléfono:", standard));
                        clContactoTL2.Colspan = 3;
                        clContactoTL2.BorderWidth = 0;
                        clContactoTL2.BorderWidthBottom = 0.1f;
                        clContactoTL2.BackgroundColor = new BaseColor(216, 217, 219);
                        tblContacto.AddCell(clContactoTL2);

                        PdfPCell clContactoT8 = new PdfPCell(new Phrase(rowSP[13].ToString(), standard));
                        clContactoT8.Colspan = 6;
                        clContactoT8.BorderWidth = 0;
                        clContactoT8.BorderWidthBottom = 0.1f;
                        tblContacto.AddCell(clContactoT8);

                        tblContacto.SpacingBefore = 10;
                        pdfDoc.Add(tblContacto);

                        // Tabla Otros Datos 
                        PdfPTable tblDatos = new PdfPTable(21);
                        tblDatos.WidthPercentage = 100;

                        PdfPCell clDatosD = new PdfPCell(new Phrase("DETALLES PUBLICIDAD CONTRATADA", standard6));
                        clDatosD.Colspan = 11;
                        clDatosD.BorderWidth = 0;
                        clDatosD.BorderWidthBottom = 0.1f;
                        clDatosD.BorderColorBottom = new BaseColor(237, 21, 86);
                        clDatosD.HorizontalAlignment = 1;
                        clDatosD.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosD);

                        PdfPCell clDatosV1 = new PdfPCell(new Phrase("", standard2));
                        clDatosV1.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV1);

                        PdfPCell clDatosD2 = new PdfPCell(new Phrase("DESGLOSE DE PUBLICIDAD CONTRATADA", standard6));
                        clDatosD2.Colspan = 9;
                        clDatosD2.BorderWidth = 0;
                        clDatosD2.BorderWidthBottom = 0.1f;
                        clDatosD2.BorderColorBottom = new BaseColor(237, 21, 86);
                        clDatosD2.HorizontalAlignment = 1;
                        clDatosD2.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosD2);

                        PdfPCell clDatosA = new PdfPCell(new Phrase("Área:", standard2));
                        clDatosA.Colspan = 3;
                        clDatosA.BorderWidth = 0;
                        clDatosA.BorderWidthBottom = 0.1f;
                        clDatosA.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosA);

                        PdfPCell clDatosT1 = new PdfPCell(new Phrase(row[12].ToString(), standard2));
                        clDatosT1.Colspan = 8;
                        clDatosT1.BorderWidth = 0;
                        clDatosT1.BorderWidthBottom = 0.1f;
                        tblDatos.AddCell(clDatosT1);

                        PdfPCell clDatosV2 = new PdfPCell(new Phrase("", standard2));
                        clDatosV2.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV2);

                        PdfPCell clDatosC1 = new PdfPCell(new Phrase("MX | Pricetravel.com", standard2));
                        clDatosC1.Colspan = 4;
                        clDatosC1.BorderWidth = 0;
                        clDatosC1.BorderWidthBottom = 0.1f;
                        clDatosC1.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC1);

                        rowgv = dtgv.Rows[0];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT2 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT2.Colspan = 5;
                        clDatosT2.BorderWidth = 0;
                        clDatosT2.BorderWidthBottom = 0.1f;
                        clDatosT2.HorizontalAlignment = 1;
                        clDatosT2.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT2);

                        PdfPCell clDatosM = new PdfPCell(new Phrase("Moneda:", standard2));
                        clDatosM.Colspan = 3;
                        clDatosM.BorderWidth = 0;
                        clDatosM.BorderWidthBottom = 0.1f;
                        clDatosM.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosM);

                        PdfPCell clDatosT3 = new PdfPCell(new Phrase(row[13].ToString(), standard2));
                        clDatosT3.Colspan = 8;
                        clDatosT3.BorderWidth = 0;
                        clDatosT3.BorderWidthBottom = 0.1f;
                        tblDatos.AddCell(clDatosT3);

                        PdfPCell clDatosV3 = new PdfPCell(new Phrase("", standard2));
                        clDatosV3.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV3);

                        PdfPCell clDatosC2 = new PdfPCell(new Phrase("MX | Puntos de Venta", standard2));
                        clDatosC2.Colspan = 4;
                        clDatosC2.BorderWidth = 0;
                        clDatosC2.BorderWidthBottom = 0.1f;
                        clDatosC2.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC2);

                        rowgv = dtgv.Rows[1];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT4 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT4.Colspan = 5;
                        clDatosT4.BorderWidth = 0;
                        clDatosT4.BorderWidthBottom = 0.1f;
                        clDatosT4.HorizontalAlignment = 1;
                        clDatosT4.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT4);

                        PdfPCell clDatosFP = new PdfPCell(new Phrase("Fechas del Plan:", standard2));
                        clDatosFP.Colspan = 3;
                        clDatosFP.BorderWidth = 0;
                        clDatosFP.BorderWidthBottom = 0.1f;
                        clDatosFP.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosFP);

                        PdfPCell clDatosT5 = new PdfPCell(new Phrase(row[2].ToString(), standard2));
                        clDatosT5.Colspan = 8;
                        clDatosT5.BorderWidth = 0;
                        clDatosT5.BorderWidthBottom = 0.1f;
                        tblDatos.AddCell(clDatosT5);

                        PdfPCell clDatosV4 = new PdfPCell(new Phrase("", standard2));
                        clDatosV4.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV4);

                        PdfPCell clDatosC3 = new PdfPCell(new Phrase("MX | Contact Center", standard2));
                        clDatosC3.Colspan = 4;
                        clDatosC3.BorderWidth = 0;
                        clDatosC3.BorderWidthBottom = 0.1f;
                        clDatosC3.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC3);

                        rowgv = dtgv.Rows[2];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT6 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT6.Colspan = 5;
                        clDatosT6.BorderWidth = 0;
                        clDatosT6.BorderWidthBottom = 0.1f;
                        clDatosT6.HorizontalAlignment = 1;
                        clDatosT6.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT6);

                        PdfPCell clDatosNF = new PdfPCell(new Phrase("Número de Facturas:", standard2));
                        clDatosNF.Colspan = 3;
                        clDatosNF.BorderWidth = 0;
                        clDatosNF.BorderWidthBottom = 0.1f;
                        clDatosNF.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosNF);

                        PdfPCell clDatosT7 = new PdfPCell(new Phrase(row[14].ToString(), standard2));
                        clDatosT7.Colspan = 8;
                        clDatosT7.BorderWidth = 0;
                        clDatosT7.BorderWidthBottom = 0.1f;
                        tblDatos.AddCell(clDatosT7);

                        PdfPCell clDatosV5 = new PdfPCell(new Phrase("", standard2));
                        clDatosV5.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV5);

                        PdfPCell clDatosC4 = new PdfPCell(new Phrase("MX | Outlet", standard2));
                        clDatosC4.Colspan = 4;
                        clDatosC4.BorderWidth = 0;
                        clDatosC4.BorderWidthBottom = 0.1f;
                        clDatosC4.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC4);

                        rowgv = dtgv.Rows[3];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT8 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT8.Colspan = 5;
                        clDatosT8.BorderWidth = 0;
                        clDatosT8.BorderWidthBottom = 0.1f;
                        clDatosT8.HorizontalAlignment = 1;
                        clDatosT8.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT8);

                        PdfPCell clDatosFF = new PdfPCell(new Phrase("Fechas de Facturación:", standard2));
                        clDatosFF.Colspan = 3;
                        clDatosFF.BorderWidth = 0;
                        clDatosFF.BorderWidthBottom = 0.1f;
                        clDatosFF.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosFF);

                        PdfPCell clDatosT9 = new PdfPCell(new Phrase(row[17].ToString(), standard2));
                        clDatosT9.Colspan = 8;
                        clDatosT9.BorderWidth = 0;
                        clDatosT9.BorderWidthBottom = 0.1f;
                        tblDatos.AddCell(clDatosT9);

                        PdfPCell clDatosV6 = new PdfPCell(new Phrase("", standard2));
                        clDatosV6.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV6);

                        PdfPCell clDatosC5 = new PdfPCell(new Phrase("MX | Travelinn Medios", standard2));
                        clDatosC5.Colspan = 4;
                        clDatosC5.BorderWidth = 0;
                        clDatosC5.BorderWidthBottom = 0.1f;
                        clDatosC5.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC5);

                        rowgv = dtgv.Rows[4];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT10 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT10.Colspan = 5;
                        clDatosT10.BorderWidth = 0;
                        clDatosT10.BorderWidthBottom = 0.1f;
                        clDatosT10.HorizontalAlignment = 1;
                        clDatosT10.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT10);

                        PdfPCell clDatosV7 = new PdfPCell(new Phrase("", standard2));
                        clDatosV7.Colspan = 12;
                        clDatosV7.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV7);

                        PdfPCell clDatosC6 = new PdfPCell(new Phrase("MX | Travelinn Caravanas", standard2));
                        clDatosC6.Colspan = 4;
                        clDatosC6.BorderWidth = 0;
                        clDatosC6.BorderWidthBottom = 0.1f;
                        clDatosC6.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC6);

                        rowgv = dtgv.Rows[5];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT11 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT11.Colspan = 5;
                        clDatosT11.BorderWidth = 0;
                        clDatosT11.BorderWidthBottom = 0.1f;
                        clDatosT11.HorizontalAlignment = 1;
                        clDatosT11.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT11);

                        PdfPCell clDatosOC = new PdfPCell(new Phrase("OBSERVACIONES / COMENTARIOS", standard3));
                        clDatosOC.Colspan = 11;
                        clDatosOC.BorderWidth = 0;
                        clDatosOC.BorderWidthBottom = 0.1f;
                        clDatosOC.BorderColorBottom = new BaseColor(237, 21, 86);
                        clDatosOC.HorizontalAlignment = 1;
                        clDatosOC.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosOC);

                        PdfPCell clDatosV8 = new PdfPCell(new Phrase("", standard2));
                        clDatosV8.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV8);

                        PdfPCell clDatosC7 = new PdfPCell(new Phrase("MX | Travelinn Retos", standard2));
                        clDatosC7.Colspan = 4;
                        clDatosC7.BorderWidth = 0;
                        clDatosC7.BorderWidthBottom = 0.1f;
                        clDatosC7.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC7);

                        rowgv = dtgv.Rows[6];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT12 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT12.Colspan = 5;
                        clDatosT12.BorderWidth = 0;
                        clDatosT12.BorderWidthBottom = 0.1f;
                        clDatosT12.HorizontalAlignment = 1;
                        clDatosT12.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT12);

                        PdfPCell clDatosV9 = new PdfPCell(new Phrase(row[6].ToString(), standard2));
                        clDatosV9.Colspan = 11;
                        clDatosV9.Rowspan = 6;
                        clDatosV9.BorderWidth = 0;
                        clDatosV9.BackgroundColor = new BaseColor(216, 217, 219);
                        clDatosV9.HorizontalAlignment = 1;
                        clDatosV9.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosV9);

                        PdfPCell clDatosV9_1 = new PdfPCell(new Phrase("", standard2));
                        clDatosV9_1.Rowspan = 6;
                        clDatosV9_1.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV9_1);

                        PdfPCell clDatosC8 = new PdfPCell(new Phrase("CO | PriceTravel.co", standard2));
                        clDatosC8.Colspan = 4;
                        clDatosC8.BorderWidth = 0;
                        clDatosC8.BorderWidthBottom = 0.1f;
                        clDatosC8.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC8);

                        rowgv = dtgv.Rows[7];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT13 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT13.Colspan = 5;
                        clDatosT13.BorderWidth = 0;
                        clDatosT13.BorderWidthBottom = 0.1f;
                        clDatosT13.HorizontalAlignment = 1;
                        clDatosT13.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT13);

                        PdfPCell clDatosC9 = new PdfPCell(new Phrase("CO | Tiquetes Baratos.com", standard2));
                        clDatosC9.Colspan = 4;
                        clDatosC9.BorderWidth = 0;
                        clDatosC9.BorderWidthBottom = 0.1f;
                        clDatosC9.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC9);

                        rowgv = dtgv.Rows[8];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT14 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT14.Colspan = 5;
                        clDatosT14.BorderWidth = 0;
                        clDatosT14.BorderWidthBottom = 0.1f;
                        clDatosT14.HorizontalAlignment = 1;
                        clDatosT14.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT14);

                        PdfPCell clDatosC10 = new PdfPCell(new Phrase("CO | Puntos de Venta", standard2));
                        clDatosC10.Colspan = 4;
                        clDatosC10.BorderWidth = 0;
                        clDatosC10.BorderWidthBottom = 0.1f;
                        clDatosC10.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC10);

                        rowgv = dtgv.Rows[9];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT15 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT15.Colspan = 5;
                        clDatosT15.BorderWidth = 0;
                        clDatosT15.BorderWidthBottom = 0.1f;
                        clDatosT15.HorizontalAlignment = 1;
                        clDatosT15.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT15);

                        PdfPCell clDatosC11 = new PdfPCell(new Phrase("CO | Contact Center", standard2));
                        clDatosC11.Colspan = 4;
                        clDatosC11.BorderWidth = 0;
                        clDatosC11.BorderWidthBottom = 0.1f;
                        clDatosC11.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC11);

                        rowgv = dtgv.Rows[10];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT16 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT16.Colspan = 5;
                        clDatosT16.BorderWidth = 0;
                        clDatosT16.BorderWidthBottom = 0.1f;
                        clDatosT16.HorizontalAlignment = 1;
                        clDatosT16.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT16);

                        PdfPCell clDatosC12 = new PdfPCell(new Phrase("CO | PriceAgencies Medios", standard2));
                        clDatosC12.Colspan = 4;
                        clDatosC12.BorderWidth = 0;
                        clDatosC12.BorderWidthBottom = 0.1f;
                        clDatosC12.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC12);

                        rowgv = dtgv.Rows[11];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT17 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT17.Colspan = 5;
                        clDatosT17.BorderWidth = 0;
                        clDatosT17.BorderWidthBottom = 0.1f;
                        clDatosT17.HorizontalAlignment = 1;
                        clDatosT17.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT17);

                        PdfPCell clDatosC13 = new PdfPCell(new Phrase("CO | PriceAgencies Caravanas", standard2));
                        clDatosC13.Colspan = 4;
                        clDatosC13.BorderWidth = 0;
                        clDatosC13.BorderWidthBottom = 0.1f;
                        clDatosC13.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC13);

                        rowgv = dtgv.Rows[12];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT18 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT18.Colspan = 5;
                        clDatosT18.BorderWidth = 0;
                        clDatosT18.BorderWidthBottom = 0.1f;
                        clDatosT18.HorizontalAlignment = 1;
                        clDatosT18.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT18);

                        PdfPCell clDatosFDP = new PdfPCell(new Phrase("FORMA DE PAGO", standard3));
                        clDatosFDP.Colspan = 11;
                        clDatosFDP.BorderWidth = 0;
                        clDatosFDP.BorderWidthBottom = 0.1f;
                        clDatosFDP.BorderColorBottom = new BaseColor(237, 21, 86);
                        clDatosFDP.HorizontalAlignment = 1;
                        clDatosFDP.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosFDP);

                        PdfPCell clDatosV15 = new PdfPCell(new Phrase("", standard2));
                        clDatosV15.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV15);

                        PdfPCell clDatosC14 = new PdfPCell(new Phrase("CO | PriceAgencies Retos", standard2));
                        clDatosC14.Colspan = 4;
                        clDatosC14.BorderWidth = 0;
                        clDatosC14.BorderWidthBottom = 0.1f;
                        clDatosC14.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clDatosC14);

                        rowgv = dtgv.Rows[13];
                        if (i == 1)
                            phrase = rowgv[2].ToString();
                        else if (rowgv[2].ToString() != "" && rowgv[2].ToString() != "0")
                            phrase = "X";
                        else
                            phrase = "";

                        PdfPCell clDatosT19 = new PdfPCell(new Phrase(phrase, standard2));
                        clDatosT19.Colspan = 5;
                        clDatosT19.BorderWidth = 0;
                        clDatosT19.BorderWidthBottom = 0.1f;
                        clDatosT19.HorizontalAlignment = 1;
                        clDatosT19.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT19);

                        Chunk T1 = new Chunk("\n" + row[16].ToString(), pinkPT);
                        PdfPCell clFPagoD = new PdfPCell(new Phrase(T1));
                        clFPagoD.Colspan = 3;
                        clFPagoD.Rowspan = 3;
                        clFPagoD.BorderWidth = 0;
                        clFPagoD.HorizontalAlignment = 1;
                        clFPagoD.VerticalAlignment = 1;
                        clFPagoD.BackgroundColor = new BaseColor(216, 217, 219);
                        tblDatos.AddCell(clFPagoD);

                        string textoFP = "";
                        if (row[7].ToString() == "X")
                            textoFP = rowSP[22].ToString();
                        else if (row[8].ToString() == "X")
                            textoFP = rowSP[23].ToString();
                        else
                            textoFP = rowSP[24].ToString();

                        PdfPCell clDatosT20 = new PdfPCell(new Phrase(textoFP, standard4));
                        clDatosT20.Colspan = 8;
                        clDatosT20.Rowspan = 3;
                        clDatosT20.BorderWidth = 0;
                        tblDatos.AddCell(clDatosT20);

                        PdfPCell clDatosV16 = new PdfPCell(new Phrase("", standard2));
                        clDatosV16.Rowspan = 3;
                        clDatosV16.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV16);

                        PdfPCell clDatosC15 = new PdfPCell(new Phrase("Subtotal:", standard3));
                        clDatosC15.Colspan = 4;
                        clDatosC15.BorderWidth = 0;
                        clDatosC15.BorderWidthBottom = 0.1f;
                        clDatosC15.BackgroundColor = new BaseColor(190, 192, 194);
                        tblDatos.AddCell(clDatosC15);

                        PdfPCell clDatosT21 = new PdfPCell(new Phrase(row[3].ToString(), standard3));
                        clDatosT21.Colspan = 5;
                        clDatosT21.BorderWidth = 0;
                        clDatosT21.BorderWidthBottom = 0.1f;
                        clDatosT21.HorizontalAlignment = 1;
                        clDatosT21.VerticalAlignment = 1;
                        clDatosT21.BackgroundColor = new BaseColor(190, 192, 194);
                        tblDatos.AddCell(clDatosT21);

                        PdfPCell clDatosC16 = new PdfPCell(new Phrase("IVA " + row[15].ToString() + "%:", standard3));
                        clDatosC16.Colspan = 4;
                        clDatosC16.BorderWidth = 0;
                        clDatosC16.BorderWidthBottom = 0.1f;
                        clDatosC16.BackgroundColor = new BaseColor(190, 192, 194);
                        tblDatos.AddCell(clDatosC16);

                        PdfPCell clDatosT23 = new PdfPCell(new Phrase(row[4].ToString(), standard3));
                        clDatosT23.Colspan = 5;
                        clDatosT23.BorderWidth = 0;
                        clDatosT23.BorderWidthBottom = 0.1f;
                        clDatosT23.HorizontalAlignment = 1;
                        clDatosT23.VerticalAlignment = 1;
                        clDatosT23.BackgroundColor = new BaseColor(190, 192, 194);
                        tblDatos.AddCell(clDatosT23);

                        PdfPCell clDatosC17 = new PdfPCell(new Phrase("TOTAL " + row[13].ToString() + ":", standard3));
                        clDatosC17.Colspan = 4;
                        clDatosC17.BorderWidth = 0;
                        clDatosC17.BackgroundColor = new BaseColor(161, 160, 164);
                        tblDatos.AddCell(clDatosC17);

                        PdfPCell clDatosT25 = new PdfPCell(new Phrase(row[5].ToString(), standard3));
                        clDatosT25.Colspan = 5;
                        clDatosT25.BorderWidth = 0;
                        clDatosT25.HorizontalAlignment = 1;
                        clDatosT25.VerticalAlignment = 1;
                        clDatosT25.BackgroundColor = new BaseColor(161, 160, 164);
                        tblDatos.AddCell(clDatosT25);

                        tblDatos.SpacingBefore = 10;
                        pdfDoc.Add(tblDatos);

                        // Tabla Forma de Pago
                        PdfPTable tblFormaPago = new PdfPTable(11);
                        tblFormaPago.WidthPercentage = 100;

                        PdfPCell clFormaPagoHeader = new PdfPCell(new Phrase("INFORMACIÓN BANCARIA", standard6));
                        clFormaPagoHeader.Colspan = 11;
                        clFormaPagoHeader.BorderWidth = 0;
                        clFormaPagoHeader.BorderWidthBottom = 0.1f;
                        clFormaPagoHeader.BorderColorBottom = new BaseColor(237, 21, 86);
                        clFormaPagoHeader.HorizontalAlignment = 1;
                        clFormaPagoHeader.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFormaPagoHeader);

                        PdfPCell clFormaPagoV1 = new PdfPCell(new Phrase("", standard3));
                        clFormaPagoV1.BorderWidth = 0;
                        tblFormaPago.AddCell(clFormaPagoV1);

                        PdfPCell clFPagoM3 = new PdfPCell(new Phrase(rowSP[15].ToString() + "\n" + rowSP[16].ToString(), standard8));
                        clFPagoM3.Colspan = 10;
                        clFPagoM3.BorderWidth = 0;
                        tblFormaPago.AddCell(clFPagoM3);

                        imageURL = Server.MapPath("~/Images/logoBanco1_2.png");
                        iTextSharp.text.Image pngBBVA = iTextSharp.text.Image.GetInstance(imageURL);
                        pngBBVA.ScaleToFit(17f, 11f);
                        pngBBVA.SpacingBefore = 0;
                        pngBBVA.SetAbsolutePosition(60, 320);

                        pdfDoc.Add(pngBBVA);

                        PdfPCell clFormaPagoV2 = new PdfPCell(new Phrase("", standard3));
                        clFormaPagoV2.BorderWidth = 0;
                        tblFormaPago.AddCell(clFormaPagoV2);

                        PdfPCell clFPagoM4 = new PdfPCell(new Phrase(rowSP[17].ToString() + "\n" + rowSP[18].ToString(), standard8));
                        clFPagoM4.Colspan = 10;
                        clFPagoM4.BorderWidth = 0;
                        tblFormaPago.AddCell(clFPagoM4);

                        imageURL = Server.MapPath("~/Images/logoBanco2_2.png");
                        iTextSharp.text.Image pngS = iTextSharp.text.Image.GetInstance(imageURL);
                        pngS.ScaleToFit(14f, 12f);
                        pngS.SpacingBefore = 0;
                        pngS.SetAbsolutePosition(60, 300);

                        pdfDoc.Add(pngS);

                        PdfPCell clFormaPagoV3 = new PdfPCell(new Phrase("", standard3));
                        clFormaPagoV3.BorderWidth = 0;
                        tblFormaPago.AddCell(clFormaPagoV3);

                        PdfPCell clFPagoM5 = new PdfPCell(new Phrase(rowSP[19].ToString() + "\n" + rowSP[20].ToString(), standard8));
                        clFPagoM5.Colspan = 10;
                        clFPagoM5.BorderWidth = 0;
                        tblFormaPago.AddCell(clFPagoM5);

                        imageURL = Server.MapPath("~/Images/logoBanco3_2.png");
                        iTextSharp.text.Image pngB = iTextSharp.text.Image.GetInstance(imageURL);
                        pngB.ScaleToFit(12f, 10f);
                        pngB.SpacingBefore = 0;
                        pngB.SetAbsolutePosition(60, 280);

                        pdfDoc.Add(pngB);

                        tblFormaPago.SpacingBefore = 20;
                        pdfDoc.Add(tblFormaPago);

                        // Tabla Firmas
                        PdfPTable tblFirma = new PdfPTable(7);
                        tblFirma.WidthPercentage = 100;

                        PdfPCell clFirmaV1 = new PdfPCell(new Phrase("", standard9));
                        clFirmaV1.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV1);

                        PdfPCell clFirmaPT = new PdfPCell(new Phrase("PRICETRAVEL HOLDING", standard9));
                        clFirmaPT.Colspan = 2;
                        clFirmaPT.BorderWidth = 0;
                        clFirmaPT.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaPT);

                        PdfPCell clFirmaV2 = new PdfPCell(new Phrase("", standard9));
                        clFirmaV2.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV2);

                        PdfPCell clFirmaC = new PdfPCell(new Phrase("CLIENTE", standard9));
                        clFirmaC.Colspan = 2;
                        clFirmaC.BorderWidth = 0;
                        clFirmaC.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaC);

                        PdfPCell clFirmaV3 = new PdfPCell(new Phrase("", standard9));
                        clFirmaV3.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV3);

                        PdfPCell clFirmaV4 = new PdfPCell(new Phrase("-", standard7));
                        clFirmaV4.Colspan = 7;
                        clFirmaV4.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV4);

                        PdfPCell clFirmaN = new PdfPCell(new Phrase("Nombre:", standard9));
                        clFirmaN.BorderWidth = 0;
                        clFirmaN.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaN);

                        PdfPCell clFirmaT1 = new PdfPCell(new Phrase(row[9].ToString(), standard9));
                        clFirmaT1.Colspan = 2;
                        clFirmaT1.BorderWidth = 0;
                        clFirmaT1.BorderWidthBottom = 1;
                        clFirmaT1.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT1);

                        PdfPCell clFirmaN2 = new PdfPCell(new Phrase("Nombre:", standard9));
                        clFirmaN2.BorderWidth = 0;
                        clFirmaN2.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaN2);

                        PdfPCell clFirmaT2 = new PdfPCell(new Phrase("", standard9));
                        clFirmaT2.Colspan = 2;
                        clFirmaT2.BorderWidth = 0;
                        clFirmaT2.BorderWidthBottom = 1;
                        clFirmaT2.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT2);

                        PdfPCell clFirmaV5 = new PdfPCell(new Phrase("", standard9));
                        clFirmaV5.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV5);

                        PdfPCell clFirmaV6 = new PdfPCell(new Phrase("-", standard7));
                        clFirmaV6.Colspan = 7;
                        clFirmaV6.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV6);

                        PdfPCell clFirmaF = new PdfPCell(new Phrase("Firma:", standard9));
                        clFirmaF.BorderWidth = 0;
                        clFirmaF.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaF);

                        PdfPCell clFirmaT3 = new PdfPCell(new Phrase("", standard9));
                        clFirmaT3.Colspan = 2;
                        clFirmaT3.BorderWidth = 0;
                        clFirmaT3.BorderWidthBottom = 1;
                        clFirmaT3.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT3);

                        PdfPCell clFirmaF2 = new PdfPCell(new Phrase("Firma:", standard9));
                        clFirmaF2.BorderWidth = 0;
                        clFirmaF2.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaF2);

                        PdfPCell clFirmaT4 = new PdfPCell(new Phrase("", standard9));
                        clFirmaT4.Colspan = 2;
                        clFirmaT4.BorderWidth = 0;
                        clFirmaT4.BorderWidthBottom = 1;
                        clFirmaT4.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT4);

                        PdfPCell clFirmaV7 = new PdfPCell(new Phrase("", standard7));
                        clFirmaV7.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV7);

                        PdfPCell clFirmaV8 = new PdfPCell(new Phrase("-", standard7));
                        clFirmaV8.Colspan = 7;
                        clFirmaV8.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV8);

                        PdfPCell clFirmaFc = new PdfPCell(new Phrase("Fecha:", standard9));
                        clFirmaFc.BorderWidth = 0;
                        clFirmaFc.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaFc);

                        DateTime fecha = Convert.ToDateTime(row[10].ToString());
                        PdfPCell clFirmaT5 = new PdfPCell(new Phrase(fecha.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX")), standard9));
                        clFirmaT5.Colspan = 2;
                        clFirmaT5.BorderWidth = 0;
                        clFirmaT5.BorderWidthBottom = 1;
                        clFirmaT5.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT5);

                        PdfPCell clFirmaFc2 = new PdfPCell(new Phrase("Fecha:", standard9));
                        clFirmaFc2.BorderWidth = 0;
                        clFirmaFc2.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaFc2);

                        PdfPCell clFirmaT6 = new PdfPCell(new Phrase("", standard9));
                        clFirmaT6.Colspan = 2;
                        clFirmaT6.BorderWidth = 0;
                        clFirmaT6.BorderWidthBottom = 1;
                        clFirmaT6.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT6);

                        PdfPCell clFirmaV9 = new PdfPCell(new Phrase("", standard9));
                        clFirmaV9.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV9);

                        PdfPCell clFirmaV10 = new PdfPCell(new Phrase("-", standard7));
                        clFirmaV10.Colspan = 7;
                        clFirmaV10.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV10);

                        PdfPCell clFirmaV11 = new PdfPCell(new Phrase("-", standard7));
                        clFirmaV11.Colspan = 7;
                        clFirmaV11.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV11);

                        tblFirma.SpacingBefore = 30;
                        pdfDoc.Add(tblFirma);

                        imageURL = Server.MapPath("~/Images/Logonew_smart2.png");
                        //string imageURL = "~/Images/sign_in_logo2.png";
                        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);
                        png.ScaleToFit(70f, 60f);
                        png.SpacingBefore = 0;
                        png.SetAbsolutePosition(280, 105);

                        pdfDoc.Add(png);

                        // Texto Footer
                        PdfPTable tblFooter2 = new PdfPTable(1);
                        tblFooter2.WidthPercentage = 100;
                        PdfPCell clFooter2 = new PdfPCell(new Phrase(rowSP[21].ToString(), standard4));
                        clFooter2.BorderWidth = 0;
                        clFooter2.HorizontalAlignment = 0;
                        clFooter2.VerticalAlignment = 1;
                        tblFooter2.AddCell(clFooter2);

                        tblFooter2.SpacingBefore = 50;
                        pdfDoc.Add(tblFooter2);

                        pdfDoc.Close();


                    }
                }
            }
            return nombreArchivo;
        }

        /*
        public string generarPDF(DataTable dtSP, DataTable dt, string accion)
        {
            DataRow rowSP = dtSP.Rows[0];
            DataRow row = dt.Rows[0];
            string cliente = "";

            if (txtCliente.Text == "")
                cliente = rowSP[1].ToString();
            else
                cliente = txtCliente.Text;

            //archivo temporal
            //string outputFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
            string nombreArchivo = cliente + "_" + System.DateTime.Now.ToString("yyyyMMdd") + ".pdf";
            string outputFile = this.Server.MapPath("~/Documents/Files/ControlPublicidad/" + nombreArchivo);
            lbRutaAcuerdo.Text = nombreArchivo;

            //Create a standard .Net FileStream for the file, setting various flags
            //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))

            using (FileStream fs = new FileStream(outputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                //Create a new PDF document setting the size to LETTER - tamaño CARTA
                //using (Document pdfDoc = new Document(PageSize.LETTER, 40f, 20f, 20f, 20f))

                using (Document pdfDoc = new Document(PageSize.LETTER, 40f, 20f, 20f, 20f))
                //using (Document doc = new Document(rec))
                {
                    //Bind the PDF document to the FileStream using an iTextSharp PdfWriter
                    using (PdfWriter w = PdfWriter.GetInstance(pdfDoc, fs))
                    {
                        //margenes fuera del limite normal a lo ancho -50f
                        //pdfDoc.SetMargins(-50f, -50f, 10f, 10f);
                        //Open the document for writing
                        iTextSharp.text.Font _standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.WHITE);
                        iTextSharp.text.Font _standardFont2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _standardFont3 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.BOLD, BaseColor.MAGENTA);
                        iTextSharp.text.Font _standardFont4 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                        iTextSharp.text.Font _standardFont5 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                        iTextSharp.text.Font _standardFont6 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

                        pdfDoc.Open();

                        //Agregamos  el texto que esta dentro de la etiqueta
                        Font LineBreak = FontFactory.GetFont("Arial", size: 16);

                        // Header Image
                        string imageURL = Server.MapPath("~/Images/sign_in_logo2.png");
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageURL);
                        pdfDoc.Add(Chunk.NEWLINE);
                        img.SetAbsolutePosition(10, 670);
                        //Se pueden agregar varios solamente añadiendo varias sentencias doc.Add(…)
                        pdfDoc.Add(img);
                        img.ScaleToFit(115f, 50f);

                        // Paragraph
                        Paragraph pHeader = new Paragraph(string.Format("                                      ORDEN DE INSERCIÓN"), _standardFont5);
                        pHeader.SpacingBefore = 30;
                        pHeader.SpacingAfter = 0;
                        pHeader.Alignment = 1; //0-Left, 1 middle,2 Right
                        pdfDoc.Add(pHeader);

                        // Tabla Datos Cliente
                        PdfPTable tblCliente = new PdfPTable(21);
                        tblCliente.WidthPercentage = 100;

                        PdfPCell clClienteHeader = new PdfPCell(new Phrase("INFORMACIÓN DEL CLIENTE (Es importante que la información sea correcta y completa, ya que se usará para elaborar la factura)", _standardFont1));
                        clClienteHeader.Colspan = 21;
                        clClienteHeader.BorderWidth = 0.5f;
                        clClienteHeader.HorizontalAlignment = 1;
                        clClienteHeader.VerticalAlignment = 1;
                        clClienteHeader.BackgroundColor = new BaseColor(36, 68, 100);
                        tblCliente.AddCell(clClienteHeader);

                        PdfPCell clClienteC = new PdfPCell(new Phrase("Cliente:", _standardFont2));
                        clClienteC.Colspan = 3;
                        clClienteC.MinimumHeight = 30f;
                        clClienteC.BorderWidth = 0.5f;
                        clClienteC.HorizontalAlignment = 0;
                        clClienteC.VerticalAlignment = 1;
                        clClienteC.BackgroundColor = new BaseColor(190, 192, 194);
                        tblCliente.AddCell(clClienteC);

                        PdfPCell clClienteT1 = new PdfPCell(new Phrase(rowSP[1].ToString(), _standardFont2));
                        clClienteT1.Colspan = 7;
                        clClienteT1.BorderWidth = 0.5f;
                        clClienteT1.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT1);

                        PdfPCell clClienteV1 = new PdfPCell(new Phrase("", _standardFont2));
                        clClienteV1.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV1);

                        PdfPCell clClienteD = new PdfPCell(new Phrase("Destino / Hotel ID:", _standardFont2));
                        clClienteD.Colspan = 3;
                        clClienteD.MinimumHeight = 30f;
                        clClienteD.BorderWidth = 0.5f;
                        clClienteD.HorizontalAlignment = 0;
                        clClienteD.VerticalAlignment = 1;
                        clClienteD.BackgroundColor = new BaseColor(190, 192, 194);
                        tblCliente.AddCell(clClienteD);

                        PdfPCell clClienteT2 = new PdfPCell(new Phrase(row[0].ToString(), _standardFont4));
                        clClienteT2.Colspan = 7;
                        clClienteT2.BorderWidth = 0.5f;
                        clClienteT2.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT2);

                        PdfPCell clClienteNF = new PdfPCell(new Phrase("Nombre Fiscal:", _standardFont2));
                        clClienteNF.Colspan = 3;
                        clClienteNF.MinimumHeight = 30f;
                        clClienteNF.BorderWidth = 0.5f;
                        clClienteNF.HorizontalAlignment = 0;
                        clClienteNF.VerticalAlignment = 1;
                        clClienteNF.BackgroundColor = new BaseColor(190, 192, 194);
                        tblCliente.AddCell(clClienteNF);

                        PdfPCell clClienteT3 = new PdfPCell(new Phrase(rowSP[2].ToString(), _standardFont4));
                        clClienteT3.Colspan = 7;
                        clClienteT3.BorderWidth = 0.5f;
                        clClienteT3.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT3);

                        PdfPCell clClienteV2 = new PdfPCell(new Phrase("", _standardFont2));
                        clClienteV2.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV2);

                        PdfPCell clClienteRFC = new PdfPCell(new Phrase("R.F.C.:", _standardFont2));
                        clClienteRFC.Colspan = 3;
                        clClienteRFC.MinimumHeight = 30f;
                        clClienteRFC.BorderWidth = 0.5f;
                        clClienteRFC.HorizontalAlignment = 0;
                        clClienteRFC.VerticalAlignment = 1;
                        clClienteRFC.BackgroundColor = new BaseColor(190, 192, 194);
                        tblCliente.AddCell(clClienteRFC);

                        PdfPCell clClienteT4 = new PdfPCell(new Phrase(rowSP[3].ToString(), _standardFont4));
                        clClienteT4.Colspan = 7;
                        clClienteT4.BorderWidth = 0.5f;
                        clClienteT4.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT4);

                        PdfPCell clClienteDF = new PdfPCell(new Phrase("Dirección Fiscal:", _standardFont2));
                        clClienteDF.Colspan = 3;
                        clClienteDF.MinimumHeight = 30f;
                        clClienteDF.BorderWidth = 0.5f;
                        clClienteDF.HorizontalAlignment = 0;
                        clClienteDF.VerticalAlignment = 1;
                        clClienteDF.BackgroundColor = new BaseColor(190, 192, 194);
                        tblCliente.AddCell(clClienteDF);

                        PdfPCell clClienteT5 = new PdfPCell(new Phrase(rowSP[4].ToString(), _standardFont4));
                        clClienteT5.Colspan = 7;
                        clClienteT5.BorderWidth = 0.5f;
                        clClienteT5.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT5);

                        PdfPCell clClienteV3 = new PdfPCell(new Phrase("", _standardFont2));
                        clClienteV3.BorderWidth = 0;
                        tblCliente.AddCell(clClienteV3);

                        PdfPCell clClienteCP = new PdfPCell(new Phrase("C.P.:", _standardFont2));
                        clClienteCP.Colspan = 3;
                        clClienteCP.MinimumHeight = 30f;
                        clClienteCP.BorderWidth = 0.5f;
                        clClienteCP.HorizontalAlignment = 0;
                        clClienteCP.VerticalAlignment = 1;
                        clClienteCP.BackgroundColor = new BaseColor(190, 192, 194);
                        tblCliente.AddCell(clClienteCP);

                        PdfPCell clClienteT6 = new PdfPCell(new Phrase(rowSP[5].ToString(), _standardFont4));
                        clClienteT6.Colspan = 7;
                        clClienteT6.BorderWidth = 0.5f;
                        clClienteT6.VerticalAlignment = 1;
                        tblCliente.AddCell(clClienteT6);

                        tblCliente.SpacingBefore = 30;
                        pdfDoc.Add(tblCliente);

                        // Tabla Datos Contactos
                        PdfPTable tblContacto = new PdfPTable(7);
                        tblContacto.WidthPercentage = 100;

                        PdfPCell clContactoHeader = new PdfPCell(new Phrase("INFORMACIÓN DE CONTACTOS CON EL CLIENTE", _standardFont1));
                        clContactoHeader.Colspan = 7;
                        clContactoHeader.BorderWidth = 0.5f;
                        clContactoHeader.HorizontalAlignment = 1;
                        clContactoHeader.VerticalAlignment = 1;
                        clContactoHeader.BackgroundColor = new BaseColor(36, 68, 100);
                        tblContacto.AddCell(clContactoHeader);

                        PdfPCell clContactoC = new PdfPCell(new Phrase("Contacto Contabilidad", _standardFont2));
                        clContactoC.Colspan = 3;
                        clContactoC.BorderWidth = 0.5f;
                        clContactoC.HorizontalAlignment = 1;
                        clContactoC.BackgroundColor = new BaseColor(190, 192, 194);
                        tblContacto.AddCell(clContactoC);

                        PdfPCell clContactoV1 = new PdfPCell(new Phrase("", _standardFont2));
                        clContactoV1.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV1);

                        PdfPCell clContactoM = new PdfPCell(new Phrase("Contacto Marketing", _standardFont2));
                        clContactoM.Colspan = 3;
                        clContactoM.BorderWidth = 0.5f;
                        clContactoM.HorizontalAlignment = 1;
                        clContactoM.BackgroundColor = new BaseColor(190, 192, 194);
                        tblContacto.AddCell(clContactoM);

                        PdfPCell clContactoN = new PdfPCell(new Phrase("Nombre:", _standardFont2));
                        clContactoN.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoN);

                        PdfPCell clContactoT1 = new PdfPCell(new Phrase(rowSP[6].ToString(), _standardFont4));
                        clContactoT1.Colspan = 2;
                        clContactoT1.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT1);

                        PdfPCell clContactoV2 = new PdfPCell(new Phrase("", _standardFont2));
                        clContactoV2.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV2);

                        PdfPCell clContactoN2 = new PdfPCell(new Phrase("Nombre:", _standardFont2));
                        clContactoN2.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoN2);

                        PdfPCell clContactoT2 = new PdfPCell(new Phrase(rowSP[7].ToString(), _standardFont4));
                        clContactoT2.Colspan = 2;
                        clContactoT2.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT2);

                        PdfPCell clContactoCR = new PdfPCell(new Phrase("Cargo:", _standardFont2));
                        clContactoCR.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoCR);

                        PdfPCell clContactoT3 = new PdfPCell(new Phrase(rowSP[8].ToString(), _standardFont4));
                        clContactoT3.Colspan = 2;
                        clContactoT3.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT3);

                        PdfPCell clContactoV3 = new PdfPCell(new Phrase("", _standardFont2));
                        clContactoV3.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV3);

                        PdfPCell clContactoCR2 = new PdfPCell(new Phrase("Cargo:", _standardFont2));
                        clContactoCR2.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoCR2);

                        PdfPCell clContactoT4 = new PdfPCell(new Phrase(rowSP[9].ToString(), _standardFont4));
                        clContactoT4.Colspan = 2;
                        clContactoT4.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT4);

                        PdfPCell clContactoE = new PdfPCell(new Phrase("E-mail:", _standardFont2));
                        clContactoE.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoE);

                        PdfPCell clContactoT5 = new PdfPCell(new Phrase(rowSP[10].ToString(), _standardFont4));
                        clContactoT5.Colspan = 2;
                        clContactoT5.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT5);

                        PdfPCell clContactoV4 = new PdfPCell(new Phrase("", _standardFont2));
                        clContactoV4.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV4);

                        PdfPCell clContactoE2 = new PdfPCell(new Phrase("E-mail:", _standardFont2));
                        clContactoE2.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoE2);

                        PdfPCell clContactoT6 = new PdfPCell(new Phrase(rowSP[11].ToString(), _standardFont4));
                        clContactoT6.Colspan = 2;
                        clContactoT6.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT6);

                        PdfPCell clContactoTL = new PdfPCell(new Phrase("Teléfono:", _standardFont2));
                        clContactoTL.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoTL);

                        PdfPCell clContactoT7 = new PdfPCell(new Phrase(rowSP[12].ToString(), _standardFont4));
                        clContactoT7.Colspan = 2;
                        clContactoT7.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT7);

                        PdfPCell clContactoV5 = new PdfPCell(new Phrase("", _standardFont2));
                        clContactoV5.BorderWidth = 0;
                        tblContacto.AddCell(clContactoV5);

                        PdfPCell clContactoTL2 = new PdfPCell(new Phrase("Teléfono:", _standardFont2));
                        clContactoTL2.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoTL2);

                        PdfPCell clContactoT8 = new PdfPCell(new Phrase(rowSP[13].ToString(), _standardFont4));
                        clContactoT8.Colspan = 2;
                        clContactoT8.BorderWidth = 0.5f;
                        tblContacto.AddCell(clContactoT8);

                        tblContacto.SpacingBefore = 20;
                        pdfDoc.Add(tblContacto);

                        // Tabla Otros Datos 
                        PdfPTable tblDatos = new PdfPTable(21);
                        tblDatos.WidthPercentage = 100;

                        PdfPCell clDatosC = new PdfPCell(new Phrase("CONCEPTO", _standardFont1));
                        clDatosC.Colspan = 10;
                        clDatosC.BorderWidth = 0.5f;
                        clDatosC.HorizontalAlignment = 1;
                        clDatosC.VerticalAlignment = 1;
                        clDatosC.BackgroundColor = new BaseColor(36, 68, 100);
                        tblDatos.AddCell(clDatosC);

                        PdfPCell clDatosV1 = new PdfPCell(new Phrase("", _standardFont2));
                        clDatosV1.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV1);

                        PdfPCell clDatosD = new PdfPCell(new Phrase("DETALLES PUBLICIDAD CONTRATADA", _standardFont1));
                        clDatosD.Colspan = 10;
                        clDatosD.BorderWidth = 0.5f;
                        clDatosD.HorizontalAlignment = 1;
                        clDatosD.VerticalAlignment = 1;
                        clDatosD.BackgroundColor = new BaseColor(36, 68, 100);
                        tblDatos.AddCell(clDatosD);

                        PdfPCell clDatosT1 = new PdfPCell(new Phrase(row[1].ToString(), _standardFont2));
                        clDatosT1.Colspan = 10;
                        clDatosT1.Rowspan = 2;
                        clDatosT1.BorderWidth = 0.5f;
                        clDatosT1.HorizontalAlignment = 1;
                        clDatosT1.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT1);

                        PdfPCell clDatosV2 = new PdfPCell(new Phrase("", _standardFont2));
                        clDatosV2.BorderWidth = 0;
                        clDatosV2.Rowspan = 2;
                        tblDatos.AddCell(clDatosV2);

                        PdfPCell clDatosV = new PdfPCell(new Phrase("Vigencia:", _standardFont2));
                        clDatosV.Colspan = 3;
                        clDatosV.BorderWidth = 0.5f;
                        clDatosV.HorizontalAlignment = 0;
                        clDatosV.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosV);

                        PdfPCell clDatosT2 = new PdfPCell(new Phrase(row[2].ToString(), _standardFont4));
                        clDatosT2.Colspan = 7;
                        clDatosT2.BorderWidth = 0.5f;
                        clDatosT2.HorizontalAlignment = 1;
                        clDatosT2.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT2);

                        PdfPCell clDatosS = new PdfPCell(new Phrase("Subtotal:", _standardFont2));
                        clDatosS.Colspan = 3;
                        clDatosS.BorderWidth = 0.5f;
                        clDatosS.HorizontalAlignment = 0;
                        clDatosS.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosS);

                        PdfPCell clDatosT3 = new PdfPCell(new Phrase("$ " + row[3].ToString(), _standardFont4));
                        clDatosT3.Colspan = 7;
                        clDatosT3.BorderWidth = 0.5f;
                        clDatosT3.HorizontalAlignment = 1;
                        clDatosT3.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT3);

                        PdfPCell clDatosO = new PdfPCell(new Phrase("OBSERVACIONES / COMENTARIOS", _standardFont1));
                        clDatosO.Colspan = 10;
                        clDatosO.BorderWidth = 0.5f;
                        clDatosO.HorizontalAlignment = 1;
                        clDatosO.VerticalAlignment = 1;
                        clDatosO.BackgroundColor = new BaseColor(36, 68, 100);
                        tblDatos.AddCell(clDatosO);

                        PdfPCell clDatosV3 = new PdfPCell(new Phrase("", _standardFont2));
                        clDatosV3.BorderWidth = 0;
                        tblDatos.AddCell(clDatosV3);

                        PdfPCell clDatosIVA = new PdfPCell(new Phrase("IVA (16%):", _standardFont2));
                        clDatosIVA.Colspan = 3;
                        clDatosIVA.BorderWidth = 0.5f;
                        clDatosIVA.HorizontalAlignment = 0;
                        clDatosIVA.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosIVA);

                        PdfPCell clDatosT4 = new PdfPCell(new Phrase("$ " + row[4].ToString(), _standardFont4));
                        clDatosT4.Colspan = 7;
                        clDatosT4.BorderWidth = 0.5f;
                        clDatosT4.HorizontalAlignment = 1;
                        clDatosT4.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT4);

                        PdfPCell clDatosT5 = new PdfPCell(new Phrase(row[6].ToString(), _standardFont4));
                        clDatosT5.Colspan = 10;
                        clDatosT5.Rowspan = 2;
                        clDatosT5.BorderWidth = 0.5f;
                        clDatosT5.HorizontalAlignment = 1;
                        clDatosT5.VerticalAlignment = 1;
                        tblDatos.AddCell(clDatosT5);

                        PdfPCell clDatosV4 = new PdfPCell(new Phrase("", _standardFont2));
                        clDatosV4.BorderWidth = 0;
                        clDatosV4.Rowspan = 2;
                        tblDatos.AddCell(clDatosV4);

                        PdfPCell clDatosP = new PdfPCell(new Phrase("Precio total (MXN):", _standardFont1));
                        clDatosP.Colspan = 3;
                        clDatosP.BorderWidth = 0.5f;
                        clDatosP.HorizontalAlignment = 0;
                        clDatosP.VerticalAlignment = 1;
                        clDatosP.BackgroundColor = new BaseColor(36, 68, 100);
                        tblDatos.AddCell(clDatosP);

                        PdfPCell clDatosT6 = new PdfPCell(new Phrase("$ " + row[5].ToString(), _standardFont1));
                        clDatosT6.Colspan = 7;
                        clDatosT6.BorderWidth = 0.5f;
                        clDatosT6.HorizontalAlignment = 1;
                        clDatosT6.VerticalAlignment = 1;
                        clDatosT6.BackgroundColor = new BaseColor(36, 68, 100);
                        tblDatos.AddCell(clDatosT6);

                        PdfPCell clDatosV5 = new PdfPCell(new Phrase(".", _standardFont1));
                        clDatosV5.Colspan = 10;
                        clDatosV5.BorderWidth = 0f;
                        tblDatos.AddCell(clDatosV5);

                        tblDatos.SpacingBefore = 20;
                        pdfDoc.Add(tblDatos);

                        // Tabla Forma de Pago
                        PdfPTable tblFormaPago = new PdfPTable(7);
                        tblFormaPago.WidthPercentage = 100;

                        PdfPCell clFPagoHeader = new PdfPCell(new Phrase("FORMA DE PAGO", _standardFont1));
                        clFPagoHeader.Colspan = 7;
                        clFPagoHeader.BorderWidth = 0.5f;
                        clFPagoHeader.HorizontalAlignment = 1;
                        clFPagoHeader.VerticalAlignment = 1;
                        clFPagoHeader.BackgroundColor = new BaseColor(36, 68, 100);
                        tblFormaPago.AddCell(clFPagoHeader);

                        Font pinkPT = new Font(Font.FontFamily.HELVETICA, 7, Font.BOLD, new BaseColor(237, 21, 86));

                        Chunk T1 = new Chunk("\n(    " + row[7].ToString() + "    )\nDescuento:\n ", pinkPT);
                        PdfPCell clFPagoD = new PdfPCell(new Phrase(T1));
                        clFPagoD.Colspan = 1;
                        clFPagoD.BorderWidth = 0.5f;
                        clFPagoD.HorizontalAlignment = 1;
                        clFPagoD.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoD);

                        PdfPCell clFPagoM = new PdfPCell(new Phrase("\nEl monto total con IVA incluido será descontado directamente de la siguiente facturación " +
                            "por producción de PriceTravel dentro de la vigencia de la publicidad contratada.\n ", _standardFont4));
                        clFPagoM.Colspan = 6;
                        clFPagoM.BorderWidth = 0.5f;
                        clFPagoM.HorizontalAlignment = 0;
                        clFPagoM.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoM);

                        Chunk T2 = new Chunk("\n(    " + row[8].ToString() + "    )\nDepósito /\nTransferencia:\n ", pinkPT);
                        PdfPCell clFPagoDT = new PdfPCell(new Phrase(T2));
                        clFPagoDT.Colspan = 1;
                        clFPagoDT.BorderWidth = 0.5f;
                        clFPagoDT.HorizontalAlignment = 1;
                        clFPagoDT.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoDT);

                        PdfPCell clFPagoM2 = new PdfPCell(new Phrase("\n" + rowSP[14].ToString() + "\n ", _standardFont4));
                        clFPagoM2.Colspan = 6;
                        clFPagoM2.BorderWidth = 0.5f;
                        clFPagoM2.HorizontalAlignment = 0;
                        clFPagoM2.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoM2);

                        Chunk T3 = new Chunk("\n\n\n\nInformación Bancaria:", pinkPT);
                        PdfPCell clFPagoIB = new PdfPCell(new Phrase(T3));
                        clFPagoIB.Colspan = 1;
                        clFPagoIB.Rowspan = 3;
                        clFPagoIB.BorderWidth = 0.5f;
                        clFPagoIB.HorizontalAlignment = 1;
                        clFPagoIB.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoIB);

                        PdfPCell clFPagoM3 = new PdfPCell(new Phrase(rowSP[15].ToString() + "\n\n" + rowSP[16].ToString(), _standardFont4));
                        clFPagoM3.Colspan = 6;
                        clFPagoM3.BorderWidth = 0.5f;
                        clFPagoM3.HorizontalAlignment = 0;
                        clFPagoM3.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoM3);

                        imageURL = Server.MapPath("~/Images/logoBanco1.png");
                        iTextSharp.text.Image pngBBVA = iTextSharp.text.Image.GetInstance(imageURL);
                        pngBBVA.ScaleToFit(24f, 21f);
                        pngBBVA.SpacingBefore = 0;
                        pngBBVA.SetAbsolutePosition(122, 284);

                        pdfDoc.Add(pngBBVA);

                        PdfPCell clFPagoM4 = new PdfPCell(new Phrase(rowSP[17].ToString() + "\n\n" + rowSP[18].ToString(), _standardFont4));
                        clFPagoM4.Colspan = 6;
                        clFPagoM4.BorderWidth = 0.5f;
                        clFPagoM4.HorizontalAlignment = 0;
                        clFPagoM4.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoM4);

                        imageURL = Server.MapPath("~/Images/logoBanco2.png");
                        iTextSharp.text.Image pngS = iTextSharp.text.Image.GetInstance(imageURL);
                        pngS.ScaleToFit(24f, 21f);
                        pngS.SpacingBefore = 0;
                        pngS.SetAbsolutePosition(122, 259);

                        pdfDoc.Add(pngS);

                        PdfPCell clFPagoM5 = new PdfPCell(new Phrase(rowSP[19].ToString() + "\n\n" + rowSP[20].ToString(), _standardFont4));
                        clFPagoM5.Colspan = 6;
                        clFPagoM5.BorderWidth = 0.5f;
                        clFPagoM5.HorizontalAlignment = 0;
                        clFPagoM5.VerticalAlignment = 1;
                        tblFormaPago.AddCell(clFPagoM5);

                        imageURL = Server.MapPath("~/Images/logoBanco3.png");
                        iTextSharp.text.Image pngB = iTextSharp.text.Image.GetInstance(imageURL);
                        pngB.ScaleToFit(24f, 21f);
                        pngB.SpacingBefore = 0;
                        pngB.SetAbsolutePosition(122, 234);

                        pdfDoc.Add(pngB);

                        tblFormaPago.SpacingBefore = 20;
                        pdfDoc.Add(tblFormaPago);

                        // Tabla Firmas
                        PdfPTable tblFirma = new PdfPTable(7);
                        tblFirma.WidthPercentage = 100;

                        PdfPCell clFirmaV1 = new PdfPCell(new Phrase("", _standardFont2));
                        clFirmaV1.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV1);

                        PdfPCell clFirmaPT = new PdfPCell(new Phrase("PRICETRAVEL HOLDING", _standardFont6));
                        clFirmaPT.Colspan = 2;
                        clFirmaPT.BorderWidth = 0;
                        clFirmaPT.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaPT);

                        PdfPCell clFirmaV2 = new PdfPCell(new Phrase("", _standardFont2));
                        clFirmaV2.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV2);

                        PdfPCell clFirmaC = new PdfPCell(new Phrase("CLIENTE", _standardFont6));
                        clFirmaC.Colspan = 2;
                        clFirmaC.BorderWidth = 0;
                        clFirmaC.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaC);

                        PdfPCell clFirmaV3 = new PdfPCell(new Phrase("", _standardFont2));
                        clFirmaV3.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV3);

                        PdfPCell clFirmaV4 = new PdfPCell(new Phrase("-", _standardFont1));
                        clFirmaV4.Colspan = 7;
                        clFirmaV4.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV4);

                        PdfPCell clFirmaN = new PdfPCell(new Phrase("Nombre:", _standardFont2));
                        clFirmaN.BorderWidth = 0;
                        clFirmaN.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaN);

                        PdfPCell clFirmaT1 = new PdfPCell(new Phrase(row[9].ToString(), _standardFont4));
                        clFirmaT1.Colspan = 2;
                        clFirmaT1.BorderWidth = 0;
                        clFirmaT1.BorderWidthBottom = 1;
                        clFirmaT1.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT1);

                        PdfPCell clFirmaN2 = new PdfPCell(new Phrase("Nombre:", _standardFont2));
                        clFirmaN2.BorderWidth = 0;
                        clFirmaN2.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaN2);

                        PdfPCell clFirmaT2 = new PdfPCell(new Phrase("", _standardFont4));
                        clFirmaT2.Colspan = 2;
                        clFirmaT2.BorderWidth = 0;
                        clFirmaT2.BorderWidthBottom = 1;
                        clFirmaT2.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT2);

                        PdfPCell clFirmaV5 = new PdfPCell(new Phrase("", _standardFont2));
                        clFirmaV5.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV5);

                        PdfPCell clFirmaV6 = new PdfPCell(new Phrase("-", _standardFont1));
                        clFirmaV6.Colspan = 7;
                        clFirmaV6.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV6);

                        PdfPCell clFirmaF = new PdfPCell(new Phrase("Firma:", _standardFont2));
                        clFirmaF.BorderWidth = 0;
                        clFirmaF.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaF);

                        PdfPCell clFirmaT3 = new PdfPCell(new Phrase("", _standardFont4));
                        clFirmaT3.Colspan = 2;
                        clFirmaT3.BorderWidth = 0;
                        clFirmaT3.BorderWidthBottom = 1;
                        clFirmaT3.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT3);

                        PdfPCell clFirmaF2 = new PdfPCell(new Phrase("Firma:", _standardFont2));
                        clFirmaF2.BorderWidth = 0;
                        clFirmaF2.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaF2);

                        PdfPCell clFirmaT4 = new PdfPCell(new Phrase("", _standardFont4));
                        clFirmaT4.Colspan = 2;
                        clFirmaT4.BorderWidth = 0;
                        clFirmaT4.BorderWidthBottom = 1;
                        clFirmaT4.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT4);

                        PdfPCell clFirmaV7 = new PdfPCell(new Phrase("", _standardFont2));
                        clFirmaV7.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV7);

                        PdfPCell clFirmaV8 = new PdfPCell(new Phrase("-", _standardFont1));
                        clFirmaV8.Colspan = 7;
                        clFirmaV8.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV8);

                        PdfPCell clFirmaFc = new PdfPCell(new Phrase("Fecha:", _standardFont2));
                        clFirmaFc.BorderWidth = 0;
                        clFirmaFc.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaFc);

                        DateTime fecha = Convert.ToDateTime(row[10].ToString());
                        PdfPCell clFirmaT5 = new PdfPCell(new Phrase(fecha.ToString("dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-MX")), _standardFont4));
                        clFirmaT5.Colspan = 2;
                        clFirmaT5.BorderWidth = 0;
                        clFirmaT5.BorderWidthBottom = 1;
                        clFirmaT5.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT5);

                        PdfPCell clFirmaFc2 = new PdfPCell(new Phrase("Fecha:", _standardFont2));
                        clFirmaFc2.BorderWidth = 0;
                        clFirmaFc2.HorizontalAlignment = 2;
                        tblFirma.AddCell(clFirmaFc2);

                        PdfPCell clFirmaT6 = new PdfPCell(new Phrase("", _standardFont4));
                        clFirmaT6.Colspan = 2;
                        clFirmaT6.BorderWidth = 0;
                        clFirmaT6.BorderWidthBottom = 1;
                        clFirmaT6.HorizontalAlignment = 1;
                        tblFirma.AddCell(clFirmaT6);

                        PdfPCell clFirmaV9 = new PdfPCell(new Phrase("", _standardFont2));
                        clFirmaV9.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV9);

                        PdfPCell clFirmaV10 = new PdfPCell(new Phrase("-", _standardFont1));
                        clFirmaV10.Colspan = 7;
                        clFirmaV10.BorderWidth = 0;
                        tblFirma.AddCell(clFirmaV10);

                        PdfPCell clFirmaV11 = new PdfPCell(new Phrase("-", _standardFont1));
                        clFirmaV11.Colspan = 7;
                        clFirmaV11.BorderWidth = 0;
                        clFirmaV11.BorderWidthBottom = 1;
                        tblFirma.AddCell(clFirmaV11);

                        tblFirma.SpacingBefore = 20;
                        pdfDoc.Add(tblFirma);

                        imageURL = Server.MapPath("~/Images/sign_in_logo2.png");
                        //string imageURL = "~/Images/sign_in_logo2.png";
                        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(imageURL);
                        png.ScaleToFit(70f, 60f);
                        png.SpacingBefore = 0;
                        png.SetAbsolutePosition(280, 85);

                        pdfDoc.Add(png);

                        // Texto Footer
                        PdfPTable tblFooter2 = new PdfPTable(1);
                        tblFooter2.WidthPercentage = 100;
                        PdfPCell clFooter2 = new PdfPCell(new Phrase(rowSP[21].ToString(), _standardFont4));
                        clFooter2.BorderWidth = 0f;
                        clFooter2.HorizontalAlignment = 0;
                        clFooter2.VerticalAlignment = 1;
                        tblFooter2.AddCell(clFooter2);

                        tblFooter2.SpacingBefore = 30;
                        pdfDoc.Add(tblFooter2);

                        pdfDoc.Close();

                        //abrimos el PDF con la aplicacion por defecto
                        //Process.Start(outputFile);
                        ///*
                        //Response.Clear();
                        if (accion == "VistaPrevia")
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.WriteFile(outputFile);
                            Response.End();
                            //Response.Flush();
                            //Response.Close();
                        }
                    }
                }
            }
            return nombreArchivo;
        }
        */
    }
}