using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.API.DTO
{
    public class ProductoDto
    {
        public int? IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public string NombreProveedor { get; set; } = string.Empty;
        public DateTime FechaCreacionDateOnly => FechaCreacion.Date;
    }

    public class ResultProductoApiDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public ProductoDto? Data { get; set; }
        public int Code { get; set; }
    }

    public class ResultProductoListApiDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<ProductoDto> Data { get; set; } = new();
        public int Code { get; set; }
    }

    public class ResultEliminarProductoApiDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public bool Data { get; set; }
        public int Code { get; set; }
    }
}
