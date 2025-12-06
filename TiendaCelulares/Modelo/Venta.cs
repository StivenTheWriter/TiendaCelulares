using System;
using System.Collections.Generic;

namespace TiendaCelulares.Modelo
{
    public class Venta
    {
        public int IdVenta { get; set; }

        public int IdCliente { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        // esto guarda lo que se compro
        public List<DetalleVenta> Detalles { get; set; }

        public Venta()
        {
            Detalles = new List<DetalleVenta>();
        }
    }
}