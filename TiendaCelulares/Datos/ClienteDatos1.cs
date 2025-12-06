using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Datos
{
    public class ClienteDatos
    {
        Conexion conexion = new Conexion();

        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> lista = new List<Cliente>();

            
            string query = "SELECT IdCliente, Nombre, Telefono, Direccion FROM Clientes";

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;

                try
                {
                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"]),
                                Nombre = dr["Nombre"].ToString(),
                                //  Validaciones para que no falle si no hay nada
                                Telefono = dr["Telefono"] == DBNull.Value ? "" : dr["Telefono"].ToString(),
                                Direccion = dr["Direccion"] == DBNull.Value ? "" : dr["Direccion"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return lista;
        }

       
        public bool Guardar(Cliente objeto)
        {
            bool respuesta = false;
            string query = "INSERT INTO Clientes(Nombre, Telefono, Direccion) VALUES (@nombre, @telefono, @direccion)";

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@telefono", objeto.Telefono ?? "");
                cmd.Parameters.AddWithValue("@direccion", objeto.Direccion ?? "");
                cmd.CommandType = CommandType.Text;

                try
                {
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0) respuesta = true;
                }
                catch (Exception ex) { throw ex; }
            }
            return respuesta;
        }

        
        public bool Editar(Cliente objeto)
        {
            bool respuesta = false;
            string query = "UPDATE Clientes SET Nombre = @nombre, Telefono = @telefono, Direccion = @direccion WHERE IdCliente = @id";

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@nombre", objeto.Nombre);
                cmd.Parameters.AddWithValue("@telefono", objeto.Telefono ?? "");
                cmd.Parameters.AddWithValue("@direccion", objeto.Direccion ?? "");
                cmd.Parameters.AddWithValue("@id", objeto.IdCliente);
                cmd.CommandType = CommandType.Text;

                try
                {
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0) respuesta = true;
                }
                catch (Exception ex) { throw ex; }
            }
            return respuesta;
        }

         
        public bool Eliminar(int id)
        {
            bool respuesta = false;
            string query = "DELETE FROM Clientes WHERE IdCliente = @id";

            using (SqlConnection con = conexion.ObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.Text;

                try
                {
                    con.Open();
                    if (cmd.ExecuteNonQuery() > 0) respuesta = true;
                }
                catch (Exception ex) { throw ex; }
            }
            return respuesta;
        }
    }
}