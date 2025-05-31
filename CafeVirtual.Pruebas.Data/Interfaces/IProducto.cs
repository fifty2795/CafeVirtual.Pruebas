
namespace CafeVirtual.Pruebas.Data.Interfaces
{
    public interface IProducto
    {
        Task<bool> EliminarProductoVentaByProveedor(int idProveedor);        

        Task<decimal> ObtenerPrecio(int idProducto);

        Task<int> ObtenerCantidad(int idProducto);
    }
}
