using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Datos
{
    public class ProductoDatos
    {
        Conexion conexion = new Conexion();

        //  obtener la lista de objetos

        public List<Producto> ObtenerProductos()
        {
            List<Producto> lista = new List<Producto>();
            SqlConnection con = conexion.ObtenerConexion();

            if (con != null)
            {
                try
                {
                    con.Open(); // Aabrimos conexion 
                    string query = "SELECT * FROM Productos";
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string tipo = reader["TipoProducto"].ToString();
                        Producto prod;

                        if (tipo == "Celular")
                        {
                            Celular cel = new Celular();
                            cel.Almacenamiento = reader["Almacenamiento"] != DBNull.Value ? Convert.ToInt32(reader["Almacenamiento"]) : 0;
                            cel.Ram = reader["Ram"] != DBNull.Value ? Convert.ToInt32(reader["Ram"]) : 0;
                            prod = cel;
                        }
                        else
                        {
                            prod = new Accesorio();
                        }

                        prod.Id = Convert.ToInt32(reader["Id"]);
                        prod.Marca = reader["Marca"].ToString();
                        prod.Modelo = reader["Modelo"].ToString();
                        prod.Precio = Convert.ToDecimal(reader["Precio"]);
                        prod.Stock = Convert.ToInt32(reader["Stock"]);

                        lista.Add(prod);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    conexion.CerrarConexion(con);
                }
            }

            return lista;
        }

        // insertar un producto
        public bool Insertar(Producto obj)
        {
            bool respuesta = false;
            SqlConnection con = new Conexion().ObtenerConexion();

            try
            {
                con.Open();
                string query = "INSERT INTO Productos (TipoProducto, Marca, Modelo, Precio, Stock, Almacenamiento, Ram) " +
                               "VALUES (@tipo, @marca, @modelo, @precio, @stock, @alm, @ram)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@marca", obj.Marca);
                cmd.Parameters.AddWithValue("@modelo", obj.Modelo);
                cmd.Parameters.AddWithValue("@precio", obj.Precio);
                cmd.Parameters.AddWithValue("@stock", obj.Stock);

                if (obj is Celular)
                {
                    Celular cel = (Celular)obj;
                    cmd.Parameters.AddWithValue("@tipo", "Celular");
                    cmd.Parameters.AddWithValue("@alm", cel.Almacenamiento);
                    cmd.Parameters.AddWithValue("@ram", cel.Ram);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@tipo", "Accesorio");
                    cmd.Parameters.AddWithValue("@alm", DBNull.Value);
                    cmd.Parameters.AddWithValue("@ram", DBNull.Value);
                }

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) respuesta = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }

            return respuesta;
        }

        // obtener todo para el data grid view
        public System.Data.DataTable ObtenerTodos()
        {
            System.Data.DataTable tabla = new System.Data.DataTable();
            SqlConnection con = new Conexion().ObtenerConexion();

            try
            {
                con.Open();
                string query = "SELECT Id, TipoProducto, Marca, Modelo, Precio, Stock, Ram, Almacenamiento FROM Productos";
                SqlCommand cmd = new SqlCommand(query, con);

                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                adaptador.Fill(tabla);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }

            return tabla;
        }
        // descontar stock
        public bool DescontarStock(int id, int cantidad)
        {
            bool respuesta = false;
            
            SqlConnection con = new Conexion().ObtenerConexion();

            try
            {
                con.Open();
                string query = "UPDATE Productos SET Stock = Stock - @cantidad WHERE Id = @id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cantidad", cantidad);
                cmd.Parameters.AddWithValue("@id", id);

                int filas = cmd.ExecuteNonQuery();
                if (filas > 0) respuesta = true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al descontar stock: " + ex.Message);
            }
            finally
            {
                if (con.State == System.Data.ConnectionState.Open) con.Close();
            }

            return respuesta;
        }
    }
}