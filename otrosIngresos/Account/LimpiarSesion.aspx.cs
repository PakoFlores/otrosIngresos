using System;
using System.Data;
using System.Web;
using otrosIngresos.Shared;

namespace otrosIngresos.Account
{
    public partial class LimpiarSesion : System.Web.UI.Page
    {
        actionsBD action = new actionsBD();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["idUsuario"] == null)
                    Response.Redirect("~/Account/Login.aspx");
                else
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

                    dt.Rows.Add("@strTabla", "VarChar", "dimUsuario");
                    dt.Rows.Add("@strUsuario", "VarChar", Convert.ToString(Session["nombre"]));
                    dt.Rows.Add("@strPassword", "VarChar", "X");
                    dt.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));

                    dt = action.EjecutarSP("spValidarUsuario", dt);

                    Session.Abandon();
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                }
            }
        }
    }
}