using PrecioyViajes.Service.Implementation;
using PrecioyViajes.Dao.Model;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using otrosIngresos.Shared;

namespace otrosIngresos.Account
{
    public partial class Login : Page
    {
        actionsBD action = new actionsBD();
        private readonly UserManager userManager = new UserManager();
        private const string CookieName = "DIRECTORY";
        public int user = 0;
        public string username;

        protected void Page_Load(object sender, EventArgs e)
        {
            LinkButton lbLogout = Master.FindControl("lbLogout") as LinkButton;
            lbLogout.Visible = false;

            if (!IsPostBack)
            {
                txtName.Text = "@pricetravel.com.mx";
                txtName.Focus();
            }
            /*RegisterHyperLink.NavigateUrl = "Register";
            OpenAuthLogin.ReturnUrl = Request.QueryString["ReturnUrl"];

            var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
            if (!String.IsNullOrEmpty(returnUrl))
            {
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
            }*/
        }

        protected void btnLogin2_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string password = txtPassword.Text;

            try
            {
                /*for (int i = 0; i < 52; i++)
                {*/
                var result = userManager.Login(new LoginConfiguration
                {
                    CookieName = CookieName,
                    Remember = cbRemember.Checked,
                    Password = password,
                    Username = name,
                    System = 3, 
                    //System = i,
                    HashedPassword = "",
                });

                if (result == LoginResult.SUCCESS || result == LoginResult.ISNOTACTIVE || result == LoginResult.NOINTERNETACCESS)
                {/**/
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
                    dt.Rows.Add("@strUsuario", "VarChar", name);
                    dt.Rows.Add("@strPassword", "VarChar", "");
                    dt.Rows.Add("@usuario", "Int", 0);

                    dt = action.EjecutarSP("spValidarUsuario", dt);

                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        Session["idUsuario"] = Convert.ToInt32(row["idUsuario"].ToString());
                        Session["nombre"] = row["nombre"].ToString();
                        Session["idPerfil"] = row["idPerfil"].ToString();
                        Session["perfil"] = row["perfil"].ToString();
                        Response.Redirect("~/Default.aspx", false);
                    }
                    else
                    {
                        //ModelState.AddModelError("NewPassword", errorMessage: "ERROR");
                        mensajeError("ERROR: Favor de verificar, acceso restringido a la página, solicitar el permiso correspondiente.");
                    }
                    /**/
                }
                else
                {
                    if (result == LoginResult.USERFAILED)
                        mensajeError("ERROR: Favor de verificar, usuario no válido.");
                    if (result == LoginResult.PASSWORDFAILED)
                        mensajeError("ERROR: Favor de verificar, contraseña no válida.");
                    if (result == LoginResult.UPDATEFAILED)
                        mensajeError("ERROR: Favor de verificar, no se pudo actualizar la información.");
                    if (result == LoginResult.AGENCYNOTACTIVE)
                        mensajeError("ERROR: Favor de verificar, agencia no esta activa.");
                    if (result == LoginResult.OTHER)
                        mensajeError("ERROR: Favor de verificar, no se pudo determinar el problema.");
                    if (result == LoginResult.TOKENREQUIRED)
                        mensajeError("ERROR: Favor de verificar, token requerido para realizar la acción.");
                    if (result == LoginResult.INVALIDORGUNIT)
                        mensajeError("ERROR: Favor de verificar, organización no válida");

                }
                //}
                //return result;
            }
            catch (Exception ex)
            {
                mensajeError(ex.Message);
            }
        }

        private void mensajeError(string Mensaje)
        {
            string script = 
                @"<script type='text/javascript'> " +
                "alert('" + Mensaje + "'); "+ 
                "</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "Alerta", script, false);
        }
    }
}