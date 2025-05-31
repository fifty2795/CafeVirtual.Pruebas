using CafeVirtual.Pruebas.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace CafeVirtual.Pruebas.API.Models
{
    public class ProductoViewModel
    {
        public int? IdProducto { get; set; }

        public int IdProveedor { get; set; }        

        public string Nombre { get; set; }
        
        public string Detalle { get; set; }
        
        public decimal Precio { get; set; }  
        
        public int Cantidad { get; set; }
        
        public bool Activo { get; set; }
    }
}
