using System;
using System.Data;
using System.Web.UI;
using otrosIngresos.Shared;

namespace otrosIngresos.Account
{
    public partial class AdminAgregarActualizar : System.Web.UI.Page
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
                    if (Convert.ToInt32(Session["idPerfil"]) != 1)
                        Response.Redirect("~/Default.aspx");
                    else
                    {
                        lbSesion.Text = Convert.ToString(Session["nombre"]) + " (" + Convert.ToString(Session["perfil"]) + ")  ";
                        //validarPerfil(Convert.ToInt32(Session["idPerfil"]));

                        LlenarListasDesplegables();
                    }
                }
            }
        }

        protected void btnAgregarActualizar_Click(object sender, EventArgs e)
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

            dtSP.Rows.Add("@idUsuario", "Int", Convert.ToUInt32(ddlUsuario.SelectedValue));
            dtSP.Rows.Add("@perfil", "SmallInt", Convert.ToUInt16(ddlPerfil.SelectedValue));
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
            dtSP.Rows.Add("@tipo", "VarChar", "A");

            dt = action.EjecutarSP("spAdministrarInsUpd", dtSP);

            if (dt.Rows.Count <= 0)
                mensajeError(@"ERROR: No se pudo guardar la información recibida");
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
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

            dtSP.Rows.Add("@idUsuario", "Int", Convert.ToUInt32(ddlUsuario.SelectedValue));
            dtSP.Rows.Add("@perfil", "SmallInt", Convert.ToUInt16(ddlPerfil.SelectedValue));
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
            dtSP.Rows.Add("@tipo", "VarChar", "");

            dt = action.EjecutarSP("spAdministrarInsUpd", dtSP);

            if (dt.Rows.Count > 0)
            {
                DataRow r = dt.Rows[0];
                ddlPerfil.SelectedValue = r[0].ToString();
            }
        }

        private void LlenarListasDesplegables()
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

            dtSP.Rows.Add("@equipo", "SmallInt", 0);
            dtSP.Rows.Add("@servicio", "VarChar", "Admin");
            dtSP.Rows.Add("@catalogo", "SmallInt", 1);
            dtSP.Rows.Add("@usuario", "Int", Convert.ToInt32(Session["idUsuario"]));
            
            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlUsuario.DataTextField = "atSNombreUsuario";
            ddlUsuario.DataValueField = "skNIdUsuario";
            ddlUsuario.DataSource = dt;
            ddlUsuario.DataBind();

            dtSP.Rows[2]["valor"] = "2";

            dt = action.EjecutarSP("spConsultarCatalogo", dtSP);

            ddlPerfil.DataTextField = "atSNombrePerfil";
            ddlPerfil.DataValueField = "skNIdPerfil";
            ddlPerfil.DataSource = dt;
            ddlPerfil.DataBind();
        }

        private void mensajeError(string Mensaje)
        {
            string script = "<script type='text/javascript'> alert('" + Mensaje + "');</script>";
            ScriptManager.RegisterStartupScript(this, typeof(Page), "AVISO", script, false);
        }
    }
}