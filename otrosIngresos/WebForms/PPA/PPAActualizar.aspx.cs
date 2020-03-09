using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using otrosIngresos.Shared;

namespace otrosIngresos.Documents.PPA
{
    public partial class PPAActualizar : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        int idCorporativo = 0;
        //string savePath;
        string idPPA = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            idPPA = Request.QueryString["idPPA"];

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
                    }
                }

                DataTable dt = new DataTable();

                dt = action.Consultar("SELECT atSNombreEquipo, skNIdEquipo FROM dbo.dimEquipo WHERE atBEstatus = 1");

                ddlEquipo.DataTextField = "atSNombreEquipo";
                ddlEquipo.DataValueField = "skNIdEquipo";
                ddlEquipo.DataSource = dt;
                ddlEquipo.DataBind();

                llenarListado();
                llenarListaServicio();
                llenarDatos();

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 1)
                    ddlMoneda.SelectedValue = "44";
                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 2)
                    ddlMoneda.SelectedValue = "109";

                System.DateTime moment = System.DateTime.Now;

                dt = action.Consultar(
                    "SELECT skNIdMoneda, atSMoneda + ' - ' + atSNombreMoneda atSMoneda "
                    + "FROM dbo.dimMoneda "
                    + "WHERE atBEstatus = 1");

                ddlMoneda.DataTextField = "atSMoneda";
                ddlMoneda.DataValueField = "skNIdMoneda";
                ddlMoneda.DataSource = dt;
                ddlMoneda.DataBind();

                dt = new DataTable();

                dt.Columns.AddRange(
                    new DataColumn[3]
                    {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                    }
                    );

                dt.Rows.Add("@idPPA", "SmallInt", Convert.ToInt16(idPPA));
                dt.Rows.Add("@opcion", "SmallInt", 1);
                dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                dt = action.EjecutarSP("spfctPPAConsulta", dt);

                DataRow row = dt.Rows[0];
                DateTime fecha = System.DateTime.Now;
                string activo = row[3].ToString();

                lbEquipo.Text = row[1].ToString();
                ddlEquipo.SelectedValue = row[1].ToString();
                if (activo == "True")
                    cbActivo.Checked = true;
                else
                    cbActivo.Checked = false;
                fecha = Convert.ToDateTime(row[5].ToString());
                txtCFechaInicio.Text = fecha.ToString("yyyy-MM-dd");
                fecha = Convert.ToDateTime(row[6].ToString());
                txtCFechaFin.Text = fecha.ToString("yyyy-MM-dd");
                fecha = Convert.ToDateTime(row[7].ToString());
                txtLFechaInicio.Text = fecha.ToString("yyyy-MM-dd");
                fecha = Convert.ToDateTime(row[8].ToString());
                txtLFechaFin.Text = fecha.ToString("yyyy-MM-dd");
                lbMoneda.Text = row[9].ToString();
                ddlMoneda.SelectedValue = row[9].ToString();
                txtPrepago.Text = row[10].ToString();
                txtInteres.Text = row[11].ToString();
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
                if (ddlListado.SelectedItem.Text == "Corporativo" || ddlListado.SelectedItem.Text == "Individual")
                    tipoServicio = "Hotel";
                else
                    tipoServicio = ddlListado.SelectedItem.Text;

                DataTable dt = new DataTable();

                dt.Columns.AddRange(
                    new DataColumn[3]
                    {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                    }
                    );

                dt.Rows.Add("@idPPA", "SmallInt", Convert.ToInt16(idPPA));
                dt.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                dt.Rows.Add("@esActivo", "Bit", cbActivo.Checked);
                dt.Rows.Add("@estatusPPA", "Bit", cbActivo.Checked);

                dt.Rows.Add("@fechaInicioC", "Date", Convert.ToDateTime(txtCFechaInicio.Text));
                dt.Rows.Add("@fechaFinC", "Date", Convert.ToDateTime(txtCFechaFin.Text));
                dt.Rows.Add("@fechaInicioL", "Date", Convert.ToDateTime(txtLFechaInicio.Text));
                dt.Rows.Add("@fechaFinL", "Date", Convert.ToDateTime(txtLFechaFin.Text));

                dt.Rows.Add("@moneda", "Int", Convert.ToInt16(ddlMoneda.SelectedItem.Value));
                dt.Rows.Add("@totalPrepago", "Money", Convert.ToDecimal(txtPrepago.Text));
                dt.Rows.Add("@porcentajeInteres", "Money", Convert.ToDecimal(txtInteres.Text));
                
                /*dt.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);*/

                DataTable dtUpd = new DataTable();
                DataTable dtUpdDet = new DataTable();

                dtUpd = action.EjecutarSP("spfctPPAUpd", dt);
                DataRow rowUpd = dtUpd.Rows[0];

                if (Convert.ToInt32(Session["idPerfil"]) != 5)
                {
                    int valor = action.EjecutarQuery("UPDATE fctPPADet SET atBEstatus = 0 WHERE skNIdPPA = " + idPPA);

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
                        dt.Rows.Add("@id", "Int", Convert.ToInt32(ddlServicio.SelectedItem.Value));
                        dt.Rows.Add("@tipo", "Char", "U");
                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                        
                        dtUpdDet = action.EjecutarSP("spfctPPADet", dt);

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

                                        dt.Rows.Add("@idPPA", "SmallInt", idPPA);
                                        dt.Rows.Add("@tipoServicio", "VarChar", tipoServicio);
                                        dt.Rows.Add("@id", "Int", Convert.ToInt32(row.Cells[2].Text));
                                        dt.Rows.Add("@tipo", "Char", "U");
                                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
                                        
                                        dtUpdDet = action.EjecutarSP("spfctPPADet", dt);

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
                    if (!action.Eliminar("spfctPPADel", Convert.ToInt32(idPPA), "@idPPA", 2, Convert.ToInt32(Session["idUsuario"])))
                        ModelState.AddModelError("Control de Publicidad", errorMessage: "ERROR: No se pudo realizar la acción solicitada");
                }
                Response.Redirect("~/Documents/PPA/PPAConsulta", false);
            }

            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
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

            //llenarCliente();
            llenarListado();
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

        protected void btnModificar_Click(object sender, EventArgs e)
        {
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);

            if (equipo == 3)
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
                    + "INNER JOIN fctPPADet fCPS ON fCPS.skNId = dH.skNIdHotel AND fCPS.atsTipoServicio = 'Hotel' "
                    + "WHERE dh.atBEstatus = 1 "
                    + "AND fCPS.skNIdPPA = " + idPPA + " "
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
            int valor = action.EjecutarQuery("UPDATE fctPPADetBack SET atBEstatus = 0 WHERE skNIdPPA = " + idPPA);

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

                        dt.Rows.Add("@idPPA", "SmallInt", idPPA);
                        dt.Rows.Add("@tipoServicio", "VarChar", "Hotel");
                        dt.Rows.Add("@id", "Int", Convert.ToInt32(row.Cells[1].Text));
                        dt.Rows.Add("@tipo", "Char", "U");
                        dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                        dtUpdDet = action.EjecutarSP("spfctPPADetBack", dt);

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
            Response.Redirect("~/Documents/PPA/PPAConsulta", false);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            llenarDatos();
        }

        /*protected void btnAcuerdo_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            //string saveDir = @"Documents\ControlAcuerdos\";
            //string appPath = Request.PhysicalApplicationPath;

            if (fuSubirAcuerdo.HasFile == true)
            {
                //nombreArchivo = Path.GetFileNameWithoutExtension(fuSubirAcuerdo.FileName);
                extensionArchivo = Path.GetExtension(fuSubirAcuerdo.FileName);

                //savePath = appPath + saveDir + Server.HtmlEncode(fuSubirAcuerdo.FileName);

                if (extensionArchivo == ".pdf" || extensionArchivo == ".gif" || extensionArchivo == ".png" || extensionArchivo == ".bmp" ||
                    extensionArchivo == ".dib" || extensionArchivo == ".jpg" || extensionArchivo == ".jpe" || extensionArchivo == ".jpeg" ||
                    extensionArchivo == ".tif" || extensionArchivo == ".jfif" || extensionArchivo == ".tiff")
                {
                    if (fuSubirAcuerdo.PostedFile.ContentLength < 10000000)
                    {
                        nombreArchivo = Regex.Replace(Path.GetFileName(fuSubirAcuerdo.FileName), "[^ a-zA-Z0-9-._]", "", RegexOptions.None);
                        fuSubirAcuerdo.SaveAs(Server.MapPath("~/Documents/Files/ControlPublicidad/") + nombreArchivo);
                        lbRutaAcuerdo.Text = nombreArchivo;

                        fuSubirAcuerdo.Enabled = false;
                        btnAcuerdo.Enabled = false;

                        lbSubirAcuerdo.Text = "Archivo precargado.";
                    }
                    else
                        lbSubirAcuerdo.Text = "Tamaño de archivo no válido, excede 4MB!";
                }
                else
                    lbSubirAcuerdo.Text = "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                        "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
            }
            else
                lbSubirAcuerdo.Text = "Archivo no válido para cargar.";

            lbSubirAcuerdo.Visible = true;
        }*/

        public string validar()
        {
            string valido = "";

            if (Convert.ToDateTime(txtCFechaInicio.Text) > Convert.ToDateTime(txtCFechaFin.Text)
                || Convert.ToDateTime(txtLFechaInicio.Text) > Convert.ToDateTime(txtLFechaFin.Text))
            {
                valido = "Favor de verificar las fechas capturadas, fecha de inicio o fecha de venta no deben ser mayores a fecha de termino.";
                return valido;
            }

            /*if (lbSubirAcuerdo.Text == "Tamaño de archivo no válido, excede 4MB!" ||
                lbSubirAcuerdo.Text == "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                    "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'" ||
                lbSubirAcuerdo.Text == "Archivo no válido para cargar.")
            {
                valido = "Favor de verificar, tipo de archivo no válido.";
                return valido;
            }*/


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

                dt.Rows.Add("@idPPA", "SmallInt", Convert.ToInt16(idPPA));
                dt.Rows.Add("@opcion", "SmallInt", 2);
                dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                dt = action.EjecutarSP("spfctPPAConsulta", dt);

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
            dtSP.Rows.Add("@catalogo", "SmallInt", 0);
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
            }
            else
            {
                ddlServicio.Enabled = true;
                txtServicio.Enabled = false;
            }

            llenarListaServicio();

            txtServicio.Text = "";
            gvHotel.PageIndex = 0;
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
            /*if (idPerfil == 1 || idPerfil == 4)
            {
                phBasico.Visible = true;
                phComplemento.Visible = true;
                phFinanzas.Visible = true;
            }
            else if (idPerfil == 3)
            {
                phBasico.Visible = true;
                phComplemento.Visible = true;
                phFinanzas.Visible = false;
            }
            else if (idPerfil == 5)
            {
                phBasico.Visible = false;
                phComplemento.Visible = false;
                phFinanzas.Visible = true;
            }*/
        }
    }
}