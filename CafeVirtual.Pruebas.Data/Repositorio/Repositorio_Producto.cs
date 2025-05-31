using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Utilidades.Utilidades;
using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using Microsoft.Data.SqlClient;

namespace CafeVirtual.Pruebas.Data.Repositorio
{
    public class Repositorio_Producto : IProducto
    {
        private readonly MvcContext _dbContext = new MvcContext();
        private readonly ILogService _logService;

        public Repositorio_Producto(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task<bool> EliminarProductoVentaByProveedor(int idProveedor)
        {
            try
            {
                var parametro = new SqlParameter("@IdProveedor", idProveedor);
                await _dbContext.Database.ExecuteSqlRawAsync("EXEC sp_EliminarProductoVentaProveedor @IdProveedor", parametro);
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en EliminarProductoVentaByProveedor", ex);
                return false;
            }
        }

        public async Task<decimal> ObtenerPrecio(int idProducto)
        {
            try
            {
                var producto = await _dbContext.TblProductos.FirstOrDefaultAsync(p => p.IdProducto == idProducto);
                return producto.Precio;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en Repositorio Venta: ObtenerPrecio", ex);
                throw;
            }
        }

        public async Task<int> ObtenerCantidad(int idProducto)
        {
            try
            {
                var producto = await _dbContext.TblProductos.FirstOrDefaultAsync(p => p.IdProducto == idProducto);
                return producto?.Cantidad ?? 0;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en Repositorio Venta: ObtenerCantidad", ex);
                throw;
            }
        }
    }
}
