using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using otrosIngresos.Shared;

namespace otrosIngresos.Documents.PPA
{
    public partial class PPA : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        int idCorporativo = 0;

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

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 1)
                    ddlMoneda.SelectedValue = "44";

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 2)
                    ddlMoneda.SelectedValue = "109";
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                string valido = validar();
                if (valido != "")
                {
                    mensajeError(@"ERROR: " + valido);
                    return;
                }

                string tipoServicio = "";

                /*if (rbGenerar.Checked)
                    preparaPDF("Guardar");
                    */
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

                int idPPA = 0;

                dt.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                dt.Rows.Add("@esActivo", "Bit", cbActivo.Checked);
                if (ddlListado.SelectedItem.Text == "Corporativo")
                    dt.Rows.Add("@grupal", "Bit", true);
                else
                    dt.Rows.Add("@grupal", "Bit", false);


                dt.Rows.Add("@fechaInicioC", "Date", Convert.ToDateTime(txtCFechaInicio.Text));
                dt.Rows.Add("@fechaFinC", "Date", Convert.ToDateTime(txtCFechaFin.Text));
                dt.Rows.Add("@fechaInicioL", "Date", Convert.ToDateTime(txtLFechaInicio.Text));
                dt.Rows.Add("@fechaFinL", "Date", Convert.ToDateTime(txtLFechaFin.Text));

                dt.Rows.Add("@moneda", "SmallInt", Convert.ToInt16(ddlMoneda.SelectedItem.Value));
                dt.Rows.Add("@totalPrepago", "Money", txtPrepago.Text);
                dt.Rows.Add("@porcentajeInteres", "Money", txtInteres.Text);
                dt.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);

                dt = action.EjecutarSP("spfctPPAIns", dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    idPPA = Convert.ToInt32(r[0].ToString());

                    if (Convert.ToInt16(ddlEquipo.SelectedValue) == 3)
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

                        dt.Rows.Add("@idPPA", "SmallInt", idPPA);
                        dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                        dt.Rows.Add("@id", "Int", ddlServicio.SelectedItem.Value);
                        dt.Rows.Add("@tipo", "Char", "I");
                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                        //dt.Rows.Add("@clienteCaptura", "VarChar", txtCliente.Text);

                        dt = action.EjecutarSP("spfctPPADet", dt);

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

                                        dt.Rows.Add("@idPPA", "SmallInt", idPPA);
                                        dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                                        dt.Rows.Add("@id", "Int", Convert.ToInt32(row.Cells[1].Text));
                                        dt.Rows.Add("@tipo", "Char", "I");
                                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                                        //dt.Rows.Add("@clienteCaptura", "VarChar", txtCliente.Text);

                                        dt = action.EjecutarSP("spfctPPADet", dt);

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

                    /*dt = new DataTable();

                    dt.Columns.AddRange(
                        new DataColumn[3]
                        {
                            new DataColumn("atributo", typeof(string)),
                            new DataColumn("tipoDato", typeof(string)),
                            new DataColumn("valor", typeof(object))
                        }
                        );

                    dt.Rows.Add("@idControlPublicidad", "SmallInt", idCP);
                    dt.Rows.Add("@cliente", "Int", Convert.ToInt16(ddlCliente.SelectedItem.Value));
                    dt.Rows.Add("@nombreC", "VarChar", txtNombreC.Text);
                    dt.Rows.Add("@cargoC", "VarChar", txtCargoC.Text);
                    dt.Rows.Add("@emailC", "VarChar", txtEmailC.Text);
                    dt.Rows.Add("@telefonoC", "VarChar", txtTelefonoC.Text);
                    dt.Rows.Add("@nombreM", "VarChar", txtNombreM.Text);
                    dt.Rows.Add("@cargoM", "VarChar", txtCargoM.Text);
                    dt.Rows.Add("@emailM", "VarChar", txtEmailM.Text);
                    dt.Rows.Add("@telefonoM", "VarChar", txtTelefonoM.Text);
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                    dt = action.EjecutarSP("spfctContactoClienteDet", dt);

                    if (dt.Rows.Count <= 0)
                    {
                        mensajeError(@"ERROR: No se pudo guardar el detalle de los contactos.");
                        return;
                    }
                    */
                    Response.Redirect("~/Documents/PPA/PPAConsulta", false);
                }
                else
                    mensajeError(@"ERROR: No se pudo guardar la orden de inserción.");
            }

            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenarDatos();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);
            if (equipo == 3)
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
            else
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

        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtServicio.Text = "";
            gvHotel.PageIndex = 0;
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

        protected void gvHotel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex != -1)
            {
                gvHotel.PageIndex = e.NewPageIndex;
                llenarDatos();
            }
        }

        protected void txtServicio_TextChanged(object sender, EventArgs e)
        {
            ddlServicio.Text = "0";
            gvHotel.PageIndex = 0;
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
            dtSP.Rows.Add("@catalogo", "SmallInt", 0);
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlServicio.DataTextField = "Nombre";
            ddlServicio.DataValueField = "Id";
            ddlServicio.DataSource = dt;
            ddlServicio.DataBind();
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

        public string validar()
        {
            string valido = "";

            if (Convert.ToDateTime(txtCFechaInicio.Text) > Convert.ToDateTime(txtCFechaFin.Text) || (txtCFechaInicio.Text == "" && txtCFechaFin.Text == "") ||
                Convert.ToDateTime(txtLFechaInicio.Text) > Convert.ToDateTime(txtLFechaFin.Text) || (txtLFechaInicio.Text == "" && txtLFechaFin.Text == ""))
            {
                valido = "Favor de verificar las fechas capturadas, fecha de inicio o fecha de venta no deben ser mayores a fecha de termino.";
                return valido;
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
            return valido;
        }

        private void mensajeError(string Mensaje)
        {
            string script = 
                @"<script type='text/javascript'>
                    alert('{0}');
                </script>";

            script = string.Format(script, Mensaje);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AVISO", script, false);
        }
    }
}