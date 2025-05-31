using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class ProductoFormViewModel
    {
        public ProductoFormDTO Producto { get; set; } = new();
        public List<SelectListItem> Proveedores { get; set; } = new();
    }
}
