using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Data;
using otrosIngresos.Shared;


namespace otrosIngresos
{
    public partial class SiteMaster : MasterPage
    {
        actionsBD action = new actionsBD();

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        protected void lbLogout_Click(object sender, EventArgs e)
        {
            cerrarSesion();
            Response.Redirect("~/Account/Login.aspx", false);
        }

        protected void cerrarSesion()
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