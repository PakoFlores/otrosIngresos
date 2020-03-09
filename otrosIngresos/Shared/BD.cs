using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace otrosIngresos.Shared
{
    public class BD
    {
        private SqlConnection con;
        private SqlDataAdapter da;
        private SqlCommand cmd;

        public void Conectar()
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            string ruta = path + "App_Data\\conBD.txt";
            StreamReader sr = new StreamReader(ruta, Encoding.ASCII);
            string file = sr.ReadToEnd();
            string[] DB = file.Split(':');

            con = new SqlConnection("Data Source=ptdbbi01.pricetravel.com.mx;Initial Catalog=" + DB[1].ToString() + ";Persist Security Info=True;User ID=bi.App;" +
                "Password=E1NuTk7r0r2L2NLI;MultipleActiveResultSets=True");
            con.Open();
        }

        public void Desconectar()
        {
            con.Close();
        }

        public void CrearComando(string consulta)
        {
            cmd = new SqlCommand(consulta, con);
        }

        public DataTable CrearComandoSP(string consulta, DataTable tabla)
        {
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
            cmd.Connection = con; //Pass the connection object to Command
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = consulta;

            foreach (DataRow row in tabla.Rows)
                cmd.Parameters.Add(row["atributo"].ToString(), tipoDato(row["tipoDato"].ToString())).Value = row["valor"];

            //cmd.ExecuteNonQuery();

            SqlDataAdapter validarRead = new SqlDataAdapter(cmd);
            
            validarRead.Fill(dt);
            return dt;
        }

        public int CrearComandoSPDel(string consulta, int ID, string atributo, int opcion, int usuario)
        {
            SqlCommand cmd = new SqlCommand(); // Create a object of SqlCommand class
            cmd.Connection = con; //Pass the connection object to Command
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = consulta;

            cmd.Parameters.Add(atributo, SqlDbType.Int).Value = ID;
            cmd.Parameters.Add("@opcion", SqlDbType.Int).Value = opcion;
            cmd.Parameters.Add("@usuario", SqlDbType.Int).Value = usuario;

            return cmd.ExecuteNonQuery();
        }

        public void AsignarParametro(string param, SqlDbType tipo, object val)
        {
            cmd.Parameters.Add(param, tipo).Value = val;
        }

        public int EjecutarConsulta()
        {
            return cmd.ExecuteNonQuery();
        }

        public DataTable EjecutarDataTable()
        {
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public static SqlDbType tipoDato(string tDato)
        {
            switch (tDato)
            {
                case "SmallInt":
                    return SqlDbType.SmallInt;
                case "Int":
                    return SqlDbType.Int;
                case "VarChar":
                    return SqlDbType.VarChar;
                case "Money":
                    return SqlDbType.Money;
                case "Date":
                    return SqlDbType.Date;
                case "DateTime":
                    return SqlDbType.DateTime;
                case "Bit":
                    return SqlDbType.Bit;
                case "Char":
                    return SqlDbType.Char;
                default:
                    return 0;
            }
        }
    }
}