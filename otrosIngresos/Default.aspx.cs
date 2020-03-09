using System;
using System.Web.UI;

namespace otrosIngresos
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idUsuario"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
            }
            else
            {
                if (Convert.ToInt32(Session["idPerfil"]) == 1)
                    phMenuAdmin.Visible = true;
                else
                    phMenuAdmin.Visible = false;
            }
        }
    }
}