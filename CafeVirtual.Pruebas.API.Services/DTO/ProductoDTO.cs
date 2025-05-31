using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Services.DTO
{
    public class ProductoDTO
    {
        public int IdProducto { get; set; }

        public int IdProveedor { get; set; }

        public string Nombre { get; set; }

        public string Detalle { get; set; }

        public decimal Precio { get; set; }

        public int Cantidad { get; set; }

        public DateTime FechaCreacion { get; set; }        

        public bool Activo { get; set; }

        public string NombreProveedor { get; set; }
    }
}
