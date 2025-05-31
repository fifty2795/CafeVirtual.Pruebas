namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class ProductoFormDTO
    {
        public int? IdProducto { get; set; } // Nullable para Creación
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Detalle { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public string NombreProveedor { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacionDateOnly => FechaCreacion.Date;
    }
}
