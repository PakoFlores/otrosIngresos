using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using otrosIngresos.Shared;

namespace testotrosIngresos1.Documents.Outlet
{
    public partial class outletConsulta : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
                Response.Redirect("~/Account/Login.aspx");
            else
            {
                lbSesion.Text = Convert.ToString(Session["nombre"]) + " (" + Convert.ToString(Session["perfil"]) + ")  ";
                if (Convert.ToInt32(Session["idPerfil"]) == 5)
                    btnAgregar.Enabled = false;
                else
                    btnAgregar.Enabled = true;
            }

            if (!IsPostBack)
            {
                DataTable dt = new DataTable();

                dt.Columns.AddRange(
                    new DataColumn[3]
                    {
                        new DataColumn("atributo", typeof(string)),
                        new DataColumn("tipoDato", typeof(string)),
                        new DataColumn("valor", typeof(object))
                    }
                    );

                dt.Rows.Add("@vista", "VarChar", "vwOutlet");

                dt = action.EjecutarSP("spConsultarColumnas", dt);
                DataRow row = dt.Rows[0];

                Session["numCols"] = Convert.ToInt16(row[0].ToString());

                for (int i = 1; i <= Convert.ToInt16(row[0].ToString()); i++)
                    ViewState["Filter" + Convert.ToString(i)] = "ALL";

                LlenarGrid();
            }
        }

        protected void gvOutlet_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex != -1)
                gvOutlet.PageIndex = e.NewPageIndex;
        }

        protected void gvOutlet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Convert.ToInt32(Session["idPerfil"]) != 2 && Convert.ToInt32(Session["idPerfil"]) != 6)
            {
                if (e.CommandName == "Eliminar")
                {
                    int idCP = Convert.ToInt32(e.CommandArgument);
                    if (!action.Eliminar("spfctControlPublicidadDel", idCP, "@idControlPublicidad", 1, Convert.ToInt32(Session["idUsuario"])))
                        mensajeError(@"ERROR: No se puede realizar la acción solicitada, no cuenta con el permiso correspondiente.");

                    Response.Redirect("~/Documents/Outlet/outletConsulta", false);
                }
                if (e.CommandName == "Editar")
                {
                    int idCP = Convert.ToInt32(e.CommandArgument);
                    Response.Redirect("~/Documents/Outlet/outletActualizar.aspx?idCP=" + idCP, false);
                }
            }
            else
                mensajeError(@"ERROR: No se puede realizar la acción solicitada, no cuenta con el permiso correspondiente.");
        }

        protected void gvOutlet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[19].Text.Length > 0)
                {
                    string caracter = e.Row.Cells[19].Text.Substring(0, 1);

                    if (caracter == "-")
                        e.Row.ForeColor = System.Drawing.Color.Green;
                    if (caracter != "*" && caracter != "-")
                        e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Documents/Outlet/outlet", false);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default", false);
        }

        private void mensajeError(string Mensaje)
        {
            string script = @"<script type='text/javascript'>
                            alert('{0}');
                        </script>";

            script = string.Format(script, Mensaje);
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AVISO", script, false);
        }

        private void LlenarGrid()
        {
            DataTable dt = new DataTable();
            DataTable dtSP = new DataTable();

            dtSP.Columns.AddRange(
                new DataColumn[3]
                {
                    new DataColumn("atributo", typeof(string)),
                    new DataColumn("tipoDato", typeof(string)),
                    new DataColumn("valor", typeof(object))
                }
                );

            for (int i = 1; i <= Convert.ToInt16(Session["numCols"]); i++)
                dtSP.Rows.Add("@Filter" + Convert.ToString(i), "VarChar", ViewState["Filter" + Convert.ToString(i)]);

            if (Convert.ToInt32(Session["idPerfil"]) == 2)
                dtSP.Rows.Add("@nombreUsuario", "VarChar", Convert.ToString(Session["nombre"]));
            else
                dtSP.Rows.Add("@nombreUsuario", "VarChar", "");

            dtSP.Rows.Add("@atributo", "SmallInt", 0);
            dtSP.Rows.Add("@usuario", "Int", Convert.ToString(Session["idUsuario"]));

            dt = action.EjecutarSP("spConsultaVistaOutlet", dtSP);

            /*gvHotel.DataTextField = "atsNombreHotel";
            gvHotel.DataValueField = "skNIdHotel";*/
            gvOutlet.DataSource = dt;
            gvOutlet.DataBind();

            if (gvOutlet.Rows.Count > 0)
            {
                for (int i = 1; i <= Convert.ToInt16(Session["numCols"]); i++)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.Items.Clear();

                    ddl = (DropDownList)gvOutlet.HeaderRow.FindControl("ddl" + Convert.ToString(i));

                    LlenarListado(ddl, dtSP, i);
                }
            }
        }

        private void LlenarListado(DropDownList ddl, DataTable dt, int index)
        {
            DataTable dtList = new DataTable();

            dt.Rows[Convert.ToInt16(Session["numCols"]) + 1]["valor"] = index;

            dtList = action.EjecutarSP("spConsultaVistaOutlet", dt);

            ddl.DataTextField = "atributo";
            ddl.DataValueField = "atributo";
            ddl.DataSource = dtList;
            ddl.DataBind();

            ddl.Items.FindByValue(ViewState["Filter" + Convert.ToString(index)].ToString()).Selected = true;
        }

        protected void ddl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl1 = (DropDownList)sender;
            ViewState["Filter1"] = ddl1.SelectedValue;
            LlenarGrid();
        }

        protected void ddl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownList ddl2 = (DropDownList)sender;
                ViewState["Filter2"] = ddl2.SelectedValue;
                LlenarGrid();
            }
        }

        protected void ddl3_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl3 = (DropDownList)sender;
            ViewState["Filter3"] = ddl3.SelectedValue;
            LlenarGrid();
        }

        protected void ddl4_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl4 = (DropDownList)sender;
            ViewState["Filter4"] = ddl4.SelectedValue;
            LlenarGrid();
        }

        protected void ddl5_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl5 = (DropDownList)sender;
            ViewState["Filter5"] = ddl5.SelectedValue;
            LlenarGrid();
        }

        protected void ddl6_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl6 = (DropDownList)sender;
            ViewState["Filter6"] = ddl6.SelectedValue;
            LlenarGrid();
        }

        protected void ddl7_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl7 = (DropDownList)sender;
            ViewState["Filter7"] = ddl7.SelectedValue;
            LlenarGrid();
        }

        protected void ddl8_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl8 = (DropDownList)sender;
            ViewState["Filter8"] = ddl8.SelectedValue;
            LlenarGrid();
        }

        protected void ddl9_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl9 = (DropDownList)sender;
            ViewState["Filter9"] = ddl9.SelectedValue;
            LlenarGrid();
        }

        protected void ddl10_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl10 = (DropDownList)sender;
            ViewState["Filter10"] = ddl10.SelectedValue;
            LlenarGrid();
        }

        protected void ddl11_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl11 = (DropDownList)sender;
            ViewState["Filter11"] = ddl11.SelectedValue;
            LlenarGrid();
        }

        protected void ddl12_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl12 = (DropDownList)sender;
            ViewState["Filter12"] = ddl12.SelectedValue;
            LlenarGrid();
        }

        protected void ddl13_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl13 = (DropDownList)sender;
            ViewState["Filter13"] = ddl13.SelectedValue;
            LlenarGrid();
        }

        protected void ddl14_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl14 = (DropDownList)sender;
            ViewState["Filter14"] = ddl14.SelectedValue;
            LlenarGrid();
        }

        protected void ddl15_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl15 = (DropDownList)sender;
            ViewState["Filter15"] = ddl15.SelectedValue;
            LlenarGrid();
        }

        protected void ddl16_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl16 = (DropDownList)sender;
            ViewState["Filter16"] = ddl16.SelectedValue;
            LlenarGrid();
        }

        protected void ddl17_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl17 = (DropDownList)sender;
            ViewState["Filter17"] = ddl17.SelectedValue;
            LlenarGrid();
        }

        protected void ddl18_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl18 = (DropDownList)sender;
            ViewState["Filter18"] = ddl18.SelectedValue;
            LlenarGrid();
        }

        protected void ddl19_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl19 = (DropDownList)sender;
            ViewState["Filter19"] = ddl19.SelectedValue;
            LlenarGrid();
        }

        protected void ddl20_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl20 = (DropDownList)sender;
            ViewState["Filter20"] = ddl20.SelectedValue;
            LlenarGrid();
        }

        protected void ddl21_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl21 = (DropDownList)sender;
            ViewState["Filter21"] = ddl21.SelectedValue;
            LlenarGrid();
        }

        protected void ddl22_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl22 = (DropDownList)sender;
            ViewState["Filter22"] = ddl22.SelectedValue;
            LlenarGrid();
        }

        protected void ddl23_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl23 = (DropDownList)sender;
            ViewState["Filter23"] = ddl23.SelectedValue;
            LlenarGrid();
        }

        protected void ddl24_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl24 = (DropDownList)sender;
            ViewState["Filter24"] = ddl24.SelectedValue;
            LlenarGrid();
        }

        protected void ddl25_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl25 = (DropDownList)sender;
            ViewState["Filter25"] = ddl25.SelectedValue;
            LlenarGrid();
        }
    }
}