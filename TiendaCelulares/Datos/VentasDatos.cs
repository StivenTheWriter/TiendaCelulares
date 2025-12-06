using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TiendaCelulares.Modelo;

namespace TiendaCelulares.Datos
{
    public class VentasDatos
    {
        // Cadena de conexión
        private string connectionString = "Data Source=PCDESTIVEN\\SQLEXPRESS;Initial Catalog=TiendaCelularesDb;Integrated Security=True;TrustServerCertificate=True";

        
        public bool InsertarVenta(Venta venta)
        {
            bool respuesta = false;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();

                    using (SqlTransaction transaccion = con.BeginTransaction())
                    {
                        try
                        {
                            // 1. Insertar Venta Maestra 
                            string queryVenta = "INSERT INTO Ventas (IdCliente, Total, Fecha) VALUES (@IdCliente, @Total, @Fecha); SELECT SCOPE_IDENTITY();";
                            SqlCommand cmdVenta = new SqlCommand(queryVenta, con, transaccion);
                            cmdVenta.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                            cmdVenta.Parameters.AddWithValue("@Total", venta.Total);
                            cmdVenta.Parameters.AddWithValue("@Fecha", venta.Fecha);

                            //  el ID generado de la venta
                            int idVentaGenerado = Convert.ToInt32(cmdVenta.ExecuteScalar());

                            
                            foreach (DetalleVenta item in venta.Detalles)
                            {
                                //  insertar en tabla DetalleVenta
                                string queryDetalle = "INSERT INTO DetalleVenta (IdVenta, IdProducto, Cantidad, PrecioUnitario, Subtotal) VALUES (@idVenta, @idProd, @cant, @precio, @subtotal)";
                                SqlCommand cmdDetalle = new SqlCommand(queryDetalle, con, transaccion);
                                cmdDetalle.Parameters.AddWithValue("@idVenta", idVentaGenerado);
                                cmdDetalle.Parameters.AddWithValue("@idProd", item.IdProducto);
                                cmdDetalle.Parameters.AddWithValue("@cant", item.Cantidad);
                                cmdDetalle.Parameters.AddWithValue("@precio", item.PrecioUnitario);
                                cmdDetalle.Parameters.AddWithValue("@subtotal", item.Cantidad * item.PrecioUnitario);
                                cmdDetalle.ExecuteNonQuery();

                                //  Restar Stock en tabla Productos
                              
                                string queryStock = "UPDATE Productos SET Stock = Stock - @cantidad WHERE Id = @id";
                                SqlCommand cmdStock = new SqlCommand(queryStock, con, transaccion);
                                cmdStock.Parameters.AddWithValue("@cantidad", item.Cantidad);
                                cmdStock.Parameters.AddWithValue("@id", item.IdProducto);
                                cmdStock.ExecuteNonQuery();
                            }

                            // si to sale bien guardamos los cambios
                            transaccion.Commit();
                            respuesta = true;
                        }
                        catch (Exception ex)
                        {
                            // si no , no agrego na  y se deshace to
                            transaccion.Rollback();
                            throw ex; //muestro el error
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return respuesta;
        }

       //obetener todas las ventas
        public List<Venta> ObtenerTodas()
        {
            List<Venta> lista = new List<Venta>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(connectionString))
                {
                    conexion.Open();
                    string query = "SELECT IdVenta, IdCliente, Fecha, Total FROM Ventas ORDER BY Fecha DESC";
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Venta venta = new Venta();
                        venta.IdVenta = Convert.ToInt32(reader["IdVenta"]);
                        venta.IdCliente = Convert.ToInt32(reader["IdCliente"]);
                        venta.Fecha = Convert.ToDateTime(reader["Fecha"]);
                        venta.Total = Convert.ToDecimal(reader["Total"]);
                        lista.Add(venta);
                    }
                }
            }
            catch (Exception) { }
            return lista;
        }

        //   buscar por fechas 
        public DataTable ListarVentas(DateTime desde, DateTime hasta)
        {
            DataTable tabla = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"SELECT v.Id, c.Nombre as Cliente, v.Fecha, v.Total 
                                     FROM Ventas v 
                                     INNER JOIN Clientes c ON v.IdCliente = c.IdCliente";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@desde", desde.Date);
                    cmd.Parameters.AddWithValue("@hasta", hasta.Date.AddDays(1).AddTicks(-1)); // Incluye todo el día final
                    SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                    adaptador.Fill(tabla);
                }
            }
            catch (Exception) { }
            return tabla;
        }

        
        public DataTable ObtenerDetalleVenta(int idVenta)
        {
            DataTable tabla = new DataTable();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string query = @"SELECT p.Modelo, d.PrecioUnitario, d.Cantidad, d.Subtotal 
                                     FROM DetalleVenta d 
                                     INNER JOIN Productos p ON d.IdProducto = p.Id 
                                     WHERE d.IdVenta = @idVenta";

                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@idVenta", idVenta);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
                catch { }
            }
            return tabla;
        }
    }
}