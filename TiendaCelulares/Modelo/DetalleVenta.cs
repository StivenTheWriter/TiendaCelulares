using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaCelulares.Modelo
{
   
        public class DetalleVenta
        {
            
             public int IdProducto { get; set; }
             public string Modelo { get; set; }
             public int Cantidad { get; set; }
             public decimal PrecioUnitario { get; set; }

             //se calcula el subtotal de una vez (Cantidad * Precio)
             public decimal Subtotal
             {
                get { return Cantidad * PrecioUnitario; }
             }
        }
}
