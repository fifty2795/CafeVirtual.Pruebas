namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class ProductoViewModelApi
    {
        public int IdProveedor { get; set; }

        public int IdCliente { get; set; }

        public string Nombre { get; set; }

        public string Detalle { get; set; }

        public string Precio { get; set; }

        public string Cantidad { get; set; }

        public bool Activo { get; set; }
    }
}
