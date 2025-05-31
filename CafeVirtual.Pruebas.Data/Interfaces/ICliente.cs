
namespace CafeVirtual.Pruebas.Data.Interfaces
{
    public interface ICliente
    {
        Task<bool> EliminarClienteVenta(int idCliente);
    }
}
