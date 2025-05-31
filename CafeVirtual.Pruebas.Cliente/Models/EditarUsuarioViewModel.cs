using Microsoft.AspNetCore.Mvc.Rendering;

namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class EditarUsuarioViewModel
    {
        public int IdRol { get; set; }
        
        public string Nombre { get; set; }
        
        public string Descripcion { get; set; }

        public int IdRolSeleccionado { get; set; }  // Propiedad para binding

        public List<SelectListItem> Roles { get; set; } = new();  // Lista para el dropdown
    }
}
