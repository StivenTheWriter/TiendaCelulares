using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; // Agregado para ConnectionState
using System.Data.SqlClient;

namespace TiendaCelulares.Datos
{
    public class Conexion
    {


        private string connectionString = "Data Source=PCDESTIVEN\\SQLEXPRESS;Initial Catalog=TiendaCelularesDb;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection ObtenerConexion()
        {
            // Solo devolvemos el objeto no se  abre aquii.
           
            return new SqlConnection(connectionString);
        }

        public void CerrarConexion(SqlConnection con)
        {
            if (con != null && con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
}