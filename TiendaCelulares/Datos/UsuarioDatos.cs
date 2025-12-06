using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Datos
{
    public class UsuarioDatos
    {
        Conexion conexion = new Conexion();

        public Usuario Loguear(string usuario, string clave)
        {
            Usuario usu = null;
            SqlConnection con = conexion.ObtenerConexion();

            if (con != null)
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @user AND Clave = @pass";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@user", usuario);
                    cmd.Parameters.AddWithValue("@pass", clave);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        usu = new Usuario();
                        usu.Id = Convert.ToInt32(reader["Id"]);
                        usu.NombreUsuario = reader["NombreUsuario"].ToString();
                        usu.Rol = reader["Rol"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    //por si la moca muestre el error real
                    throw new Exception("Error en Loguear: " + ex.Message);
                }

                finally
                {
                    conexion.CerrarConexion(con); 
                }
            }
            return usu;
        }
    }
}
