using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using otrosIngresos.Shared;

namespace testotrosIngresos1.Documents.Destinos
{
    public partial class DestinosConsulta : System.Web.UI.Page
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

                dt.Rows.Add("@vista", "VarChar", "vwDestino");

                dt = action.EjecutarSP("spConsultarColumnas", dt);
                DataRow row = dt.Rows[0];

                Session["numCols"] = Convert.ToInt16(row[0].ToString());

                for (int i = 1; i <= Convert.ToInt16(row[0].ToString()); i++)
                    ViewState["Filter" + Convert.ToString(i)] = "ALL";

                LlenarGrid();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Documents/ControlPublicidad/controlPublicidad", false);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default", false);
        }

        protected void gvDestinos_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            if (e.NewPageIndex != -1)
                gvDestinos.PageIndex = e.NewPageIndex;
        }

        protected void gvDestinos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (Convert.ToInt32(Session["idPerfil"]) != 2 && Convert.ToInt32(Session["idPerfil"]) != 6)
            {
                if (e.CommandName == "Eliminar")
                {
                    int idD = Convert.ToInt32(e.CommandArgument);
                    if (!action.Eliminar("spfctDestinoDel", idD, "@idDestino", 1, Convert.ToInt32(Session["idUsuario"])))
                        mensajeError(@"ERROR: No se puede realizar la acción solicitada, no cuenta con el permiso correspondiente.");

                    Response.Redirect("~/Documents/Destinos/destinosConsulta", false);
                }
                if (e.CommandName == "Editar")
                {
                    int idD = Convert.ToInt32(e.CommandArgument), tipo = 5;
                    Response.Redirect("~/Documents/ControlPublicidad/controlPublicidadActualizar.aspx?idCP=" + idD + "&tipo=" + tipo, false);
                }
            }
            else
                mensajeError(@"ERROR: No se puede realizar la acción solicitada, no cuenta con el permiso correspondiente.");
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

            dt = action.EjecutarSP("spConsultaVistaDestinos", dtSP);

            /*gvHotel.DataTextField = "atsNombreHotel";
            gvHotel.DataValueField = "skNIdHotel";*/
            gvDestinos.DataSource = dt;
            gvDestinos.DataBind();

            if (gvDestinos.Rows.Count > 0)
            { 
                for (int i = 1; i <= Convert.ToInt16(Session["numCols"]); i++)
                {
                    DropDownList ddl = new DropDownList();
                    ddl.Items.Clear();

                    ddl = (DropDownList)gvDestinos.HeaderRow.FindControl("ddl" + Convert.ToString(i));

                    LlenarListado(ddl, dtSP, i);
                }
            }
        }

        private void LlenarListado(DropDownList ddl, DataTable dt, int index)
        {
            DataTable dtList = new DataTable();

            dt.Rows[Convert.ToInt16(Session["numCols"]) + 1]["valor"] = index;

            dtList = action.EjecutarSP("spConsultaVistaDestinos", dt);

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
    }
}