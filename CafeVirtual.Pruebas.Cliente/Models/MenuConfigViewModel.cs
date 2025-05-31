namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class MenuConfigViewModel
    {
        public int Id { get; set; }                     // ID del menú
        public string Nombre { get; set; }              // Nombre visible en el menú
        public string Url { get; set; }                 // Ruta a la que apunta
        public string Icono { get; set; }               // Clases de icono (ej: "fa-users")
        public int Orden { get; set; }                  // Orden en el que se muestra
        public bool Activo { get; set; }                // Si está activo
        public List<SubMenuConfigViewModel> SubMenus { get; set; } = new(); // Submenús hijos
    }

    public class SubMenuConfigViewModel
    {
        public int Id { get; set; }                     // ID del submenú
        public string Nombre { get; set; }              // Nombre visible
        public string Url { get; set; }                 // Ruta a la que apunta
        public string Icono { get; set; }               // Clases de icono (ej: "fa-users")
        public bool Activo { get; set; }                // Si está activo
    }
}
