namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class RolViewModel
    {
        public int? IdRol { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaCreacionDateOnly => FechaCreacion.Date;
        public bool Activo { get; set; }

        // Para construir el árbol de menús
        public List<MenuViewModel> Menus { get; set; } = new();

        // Para recoger los IDs seleccionados al hacer POST
        public List<int> MenusSeleccionados { get; set; } = new();
        public List<int> SubMenusSeleccionados { get; set; } = new();
    }
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public List<SubMenuViewModel> SubMenus { get; set; } = new();
    }

    public class SubMenuViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }

}
