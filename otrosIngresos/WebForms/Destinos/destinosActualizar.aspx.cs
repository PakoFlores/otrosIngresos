using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using otrosIngresos.Shared;

namespace otrosIngresos.Documents.Destinos
{
    public partial class destinosActualizar : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        string idDestino = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            idDestino = Request.QueryString["idD"];

            if (!IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("~/Account/Login.aspx");
                else
                {
                    if (Convert.ToInt32(Session["idPerfil"]) == 2 || Convert.ToInt32(Session["idPerfil"]) == 6)
                        Response.Redirect("~/Default.aspx");
                    else
                        lbSesion.Text = Convert.ToString(Session["nombre"]) + " (" + Convert.ToString(Session["perfil"]) + ")  ";
                }

                System.DateTime moment = System.DateTime.Now;

                DataTable dt = new DataTable();

                dt = action.Consultar("SELECT atSNombreEquipo, skNIdEquipo FROM dbo.dimEquipo WHERE atBEstatus = 1");

                ddlEquipo.DataTextField = "atSNombreEquipo";
                ddlEquipo.DataValueField = "skNIdEquipo";
                ddlEquipo.DataSource = dt;
                ddlEquipo.DataBind();

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
                    "SELECT skNIdEstatus, atSNombreEstatus "
                    + "FROM dbo.dimEstatus "
                    + "WHERE atBEstatus = 1");

                ddlEstatus.DataTextField = "atSNombreEstatus";
                ddlEstatus.DataValueField = "skNIdEstatus";
                ddlEstatus.DataSource = dt;
                ddlEstatus.DataBind();

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 1)
                {
                    ddlMoneda.SelectedValue = "44";
                    ddlPais.SelectedValue = "55";
                }

                if (Convert.ToInt16(ddlEquipo.SelectedValue) == 2)
                {
                    ddlMoneda.SelectedValue = "109";
                    ddlPais.SelectedValue = "159";
                }

                dt = action.Consultar(
                    "SELECT DISTINCT "
                    + "fD.skNIdDestino, "
                    + "fD.skNIdEquipo, "
                    + "fD.skNIdPais, "
                    + "fD.atSNombreCliente, "
                    + "fD.atSRazonSocial, "
                    + "fD.atSOrdenInsercion, "
                    + "CAST(fD.atCIngresoSinIVA AS decimal(10, 2)) atCPrecioSinIVA, "
                    + "CAST(fD.atCIngresoConIVA AS decimal(10, 2)) atCPrecioSinIVA, "
                    + "fD.skNIdMoneda, "
                    + "CAST(fD.atCIngresoPesosMXSinIVA AS decimal(10, 2)) atCPrecioSinIVA, "
                    + "fD.skNIdEstatus, "
                    + "FORMAT(fD.atDFechaFacturacion, 'dd/MMM/yy', 'en-US') FechaFactura, "
                    + "fD.atSFolioFactura, "
                    + "FORMAT(fD.atDFechaPago, 'dd/MMM/yy', 'en-US') FechaPago, "
                    + "fD.atSArchivoFactura, "
                    + "fD.atSArchivoComprobantePago, "
                    + "fD.atSComentarios, "
                    + "fD.skNIdUsuario "
                    + "FROM fctDestino fD "
                    + "WHERE fD.skNIdDestino = " + idDestino);

                DataRow row = dt.Rows[0];

                ddlEquipo.SelectedValue     = row[1].ToString();
                //lbEquipo.Text             = row[1].ToString();/**/
                ddlPais.SelectedValue       = row[2].ToString();
                txtCliente.Text             = row[3].ToString();
                txtRazonSocial.Text         = row[4].ToString();
                lbSubirOrden.Text           = row[5].ToString();
                lbRutaOrden.Text            = row[5].ToString();
                if (lbSubirOrden.Text != "")
                {
                    cbOrden.Checked = true;
                    upOrdenInsercion.Visible = true;
                }
                txtIngresoSinIVA.Text       = row[6].ToString();
                txtIngresoConIVA.Text       = row[7].ToString();
                ddlMoneda.SelectedValue     = row[8].ToString();
                //lbMoneda.Text             = row[8].ToString();/**/
                txtIngresoSinIVAMXN.Text    = row[9].ToString();
                ddlEstatus.SelectedValue    = row[10].ToString();
                txtFechaFactura.Text        = row[11].ToString();
                txtFolioFactura.Text        = row[12].ToString();
                txtFechaPago.Text           = row[13].ToString();
                lbSubirFactura.Text         = row[14].ToString();
                lbRutaFactura.Text          = row[14].ToString();
                if (lbSubirFactura.Text != "")
                {
                    cbFactura.Checked = true;
                    upFactura.Visible = true;
                }
                lbSubirPago.Text            = row[15].ToString();
                lbRutaPago.Text             = row[15].ToString();
                if (lbSubirPago.Text != "")
                {
                    cbPago.Checked = true;
                    upPago.Visible = true;
                }
                txtComentarios.Text         = row[16].ToString();
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

                DataTable dt = new DataTable();

                dt.Columns.AddRange(
                    new DataColumn[3]
                    {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                    }
                    );

                dt.Rows.Add("@idDestino", "SmallInt", Convert.ToInt16(idDestino));
                dt.Rows.Add("@equipo", "SmallInt", Convert.ToInt16(ddlEquipo.SelectedItem.Value));
                dt.Rows.Add("@pais", "SmallInt", Convert.ToInt16(ddlPais.SelectedItem.Value));
                dt.Rows.Add("@nombreCliente", "VarChar", txtCliente.Text);
                dt.Rows.Add("@razonSocial", "VarChar", txtRazonSocial.Text);

                dt.Rows.Add("@ordenInsercion", "VarChar", lbRutaOrden.Text /*savePath*/);
                dt.Rows.Add("@ingresoSinIVA", "Money", Convert.ToDecimal(txtIngresoSinIVA.Text));
                dt.Rows.Add("@ingresoConIVA", "Money", Convert.ToDecimal(txtIngresoConIVA.Text));
                dt.Rows.Add("@moneda", "Int", Convert.ToInt16(ddlMoneda.SelectedItem.Value));
                dt.Rows.Add("@ingresoSinIVAMXN", "Money", Convert.ToDecimal(txtIngresoSinIVAMXN.Text));

                dt.Rows.Add("@estatus", "SmallInt", Convert.ToInt16(ddlEstatus.SelectedItem.Value));
                if (txtFechaFactura.Text != "")
                    dt.Rows.Add("@fechaFactura", "Date", Convert.ToDateTime(txtFechaFactura.Text));
                dt.Rows.Add("@folioFactura", "VarChar", "");
                if (txtFechaPago.Text != "")
                    dt.Rows.Add("@fechaPago", "Date", Convert.ToDateTime(txtFechaPago.Text));
                dt.Rows.Add("@archivoFactura", "VarChar", lbRutaFactura.Text /*savePath*/);

                dt.Rows.Add("@archivoPago", "VarChar", lbRutaPago.Text /*savePath*/);
                dt.Rows.Add("@comentariosAdicionales", "VarChar", txtComentarios.Text);
                dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                /*dt.Rows.Add("@fechaCreacion", "DateTime", System.DateTime.Now);*/

                DataTable dtUpd = new DataTable();
                DataTable dtUpdDet = new DataTable();

                dtUpd = action.EjecutarSP("spfctDestinoUpd", dt);
                DataRow rowUpd = dtUpd.Rows[0];

                if (Convert.ToInt32(Session["idPerfil"]) != 5)
                {
                    if (!action.Eliminar("spfctDestinoDel", Convert.ToInt32(idDestino), "@idDestino", 2, Convert.ToInt32(Session["idUsuario"])))
                        ModelState.AddModelError("Destinos", errorMessage: "ERROR: No se pudo realizar la acción solicitada");
                }
                Response.Redirect("~/Documents/Destinos/destinosConsulta", false);
            }

            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        protected void ddlEquipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int equipo = Convert.ToInt32(ddlEquipo.SelectedItem.Value);
            if (equipo != 3)
            {
                if (equipo == 1)
                {
                    ddlMoneda.SelectedValue = "44";
                    ddlPais.SelectedValue = "55";
                }

                if (equipo == 2)
                {
                    ddlMoneda.SelectedValue = "109";
                    ddlPais.SelectedValue = "159";
                }
            }
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Documents/Destinos/destinosConsulta", false);
        }

        protected void btnOrden_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;
            
            if (fuSubirOrden.HasFile == true)
            {
                extensionArchivo = Path.GetExtension(fuSubirOrden.FileName);

                if (extensionArchivo == ".pdf" || extensionArchivo == ".gif" || extensionArchivo == ".png" || extensionArchivo == ".bmp" ||
                    extensionArchivo == ".dib" || extensionArchivo == ".jpg" || extensionArchivo == ".jpe" || extensionArchivo == ".jpeg" ||
                    extensionArchivo == ".tif" || extensionArchivo == ".jfif" || extensionArchivo == ".tiff")
                {
                    if (fuSubirOrden.PostedFile.ContentLength < 10000000)
                    {
                        nombreArchivo = Regex.Replace(Path.GetFileName(fuSubirOrden.FileName), "[^ a-zA-Z0-9-._]", "", RegexOptions.None);
                        fuSubirOrden.SaveAs(Server.MapPath("~/Documents/Files/Destinos/OrdenInsercion/") + nombreArchivo);
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

            if (fuFactura.HasFile == true)
            {
                extensionArchivo = Path.GetExtension(fuFactura.FileName);

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
                    lbSubirFactura.Text = "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                        "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
            }
            else
                lbSubirFactura.Text = "Archivo no válido para cargar.";

            lbSubirFactura.Visible = true;
        }

        protected void btnPago_Click(object sender, EventArgs e)
        {
            string nombreArchivo = string.Empty;
            string extensionArchivo = string.Empty;

            if (fuPago.HasFile == true)
            {
                extensionArchivo = Path.GetExtension(fuPago.FileName);

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
                    lbSubirPago.Text = "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                        "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'";
            }
            else
                lbSubirPago.Text = "Archivo no válido para cargar.";

            lbSubirPago.Visible = true;
        }

        public string validar()
        {
            string valido = "";

            /*if (txtFechaPago.Text != "" && txtFechaFactura.Text != "")
            {
                if (Convert.ToDateTime(txtFechaPago.Text) > Convert.ToDateTime(txtFechaFactura.Text))
                {
                    valido = "Favor de verificar las fechas capturadas, fecha de pago no debe ser mayor a la fecha de facturación.";
                    return valido;
                }
            }*/

            /*if (lbSubirOrden.Text == "Tamaño de archivo no válido, excede 4MB!" ||
                lbSubirOrden.Text == "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                    "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'" ||
                lbSubirOrden.Text == "Archivo no válido para cargar.")
            {
                valido = "Favor de verificar, tipo de archivo no válido.";
                return valido;
            }

            if (lbSubirFactura.Text == "Tamaño de archivo no válido, excede 4MB!" ||
                lbSubirFactura.Text == "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                    "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'" ||
                lbSubirFactura.Text == "Archivo no válido para cargar.")
            {
                valido = "Favor de verificar, tipo de archivo no válido.";
                return valido;
            }

            if (lbSubirPago.Text == "Tamaño de archivo no válido, excede 4MB!" ||
                lbSubirPago.Text == "Tipo de archivo no válido, solo se aceptan archivos: <br> " +
                    "'.pdf', '.gif', '.png', '.bmp', '.dib', '.jpg', '.jpeg', '.jpe', '.jfif', '.tif', '.tiff'" ||
                lbSubirPago.Text == "Archivo no válido para cargar.")
            {
                valido = "Favor de verificar, tipo de archivo no válido.";
                return valido;
            }*/

            return valido;
        }

        protected void cbOrden_CheckedChanged(object sender, EventArgs e)
        {
            if (cbOrden.Checked)
                upOrdenInsercion.Visible = true;
            else
            {
                upOrdenInsercion.Visible = false;
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

        private void mensajeError(string Mensaje)
        {
            string script = @"<script type='text/javascript'>
                            alert('{0}');
                        </script>";

            script = string.Format(script, Mensaje);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AVISO", script, false);
        }
    }
}