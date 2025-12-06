using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaCelulares.Modelo
{
    public class Accesorio: Producto
    {
        public override string MostrarInformacion()
        {
            return $"Accesorio: {Marca} {Modelo} - ${Precio}";
        }
    }
}
