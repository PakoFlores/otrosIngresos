using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace testotrosIngresos1.Documents.MarketingDeal
{
    public partial class MarketingDealConsulta : System.Web.UI.Page
    {
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
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Documents/MarketingDeal/marketingDeal", false);
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default", false);
        }

        protected void gvMarketingDeal_Load(object sender, EventArgs e)
        {
        }

        protected void gvMarketingDeal_PageIndexChanging(object sender, EventArgs e)
        {
        }

        protected void gvMarketingDeal_RowCommand(object sender, EventArgs e)
        {
        }

        private void mensajeError(string Mensaje)
        {
            string script = @"<script type='text/javascript'> alert('" + Mensaje + "');</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alerta", script, false);
        }
    }
}