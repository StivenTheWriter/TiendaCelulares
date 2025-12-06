using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiendaCelulares.Modelo
{
    public class Celular : Producto , IGarantia
    {
        public int Almacenamiento { get; set; }
        public int Ram { get; set; }

        public override string MostrarInformacion()
        {
            return $"{Marca} {Modelo} - {Almacenamiento}GB/{Ram}GB RAM - ${Precio}";
        }

        public string ObtenerTiempoGarantia()
        {
            return "12 Meses de Garantía";
        }

    }
}
