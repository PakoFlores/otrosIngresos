using System.Data;

namespace otrosIngresos.Shared
{
    public class actionsBD
    {
        bool ejecuto = false;        
        BD bd = new BD();

        public DataTable Consultar(string instruccion)
        {
            DataTable dt = new DataTable();
            bd.Conectar();
            bd.CrearComando(instruccion);
            //bd.AsignarParametro("@tabla", SqlDbType.VarChar, tabla);
            dt = bd.EjecutarDataTable();
            bd.Desconectar();

            return dt;
        }

        public int EjecutarQuery(string instruccion)
        {
            int valor;
            bd.Conectar();
            bd.CrearComando(instruccion);
            valor = bd.EjecutarConsulta();
            bd.Desconectar();

            return valor;
        }

        public DataTable EjecutarSP(string nombreSP, DataTable tabla)
        {
            DataTable dt = new DataTable();
            bd.Conectar();
            dt = bd.CrearComandoSP(nombreSP, tabla);

            bd.Desconectar();

            return dt;
        }

        public bool Eliminar(string sp, int idCP, string atributo, int opcion, int usuario)
        {
            bd.Conectar();
            if (bd.CrearComandoSPDel(sp, idCP, atributo, opcion, usuario) > 0)
                ejecuto = true;
            else
                ejecuto = false;

            bd.Desconectar();

            return ejecuto;
        }
    }
}