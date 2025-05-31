using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace CafeVirtual.Pruebas.API.Data.Repositorio
{
    public class Repositorio_Producto: IProducto
    {
        private readonly MvcContext _dbContext;
        private readonly ILogService _logService;

        public Repositorio_Producto(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task<List<TblProducto?>> ObtenerProducto(string? busqueda)
        {
            return await _dbContext.TblProductos.Include(u => u.IdProveedorNavigation).Where(u =>u.Activo && u.IdProveedorNavigation.Activo &&
                                                (
                                                    u.Nombre.Contains(busqueda) ||
                                                    u.Detalle.Contains(busqueda)
                                                )).OrderBy(x => x.Nombre).ToListAsync();
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

        public async Task ActualizarProducto(TblProducto producto)
        {
            try
            {
                var productoOriginal = await _dbContext.TblProductos.FirstOrDefaultAsync(p => p.IdProducto == producto.IdProducto);

                if (productoOriginal == null)
                    throw new Exception("Producto no encontrado");

                productoOriginal.Nombre = producto.Nombre;
                productoOriginal.Detalle = producto.Detalle;
                productoOriginal.Precio = producto.Precio;
                productoOriginal.Cantidad = producto.Cantidad;
                productoOriginal.Activo = producto.Activo;
                productoOriginal.IdProveedor = producto.IdProveedor;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en Repositorio Producto: ActualizarProducto", ex);
                throw;
            }
        }

        public async Task<bool> EliminarProducto(int idProducto)
        {
            try
            {
                var parametro = new SqlParameter("@IdProducto", idProducto);
                await _dbContext.Database.ExecuteSqlRawAsync("EXEC sp_EliminarProductoVentaByIdProducto @IdProducto", parametro);
                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en Repositorio Producto: EliminarProducto", ex);
                return false;
            }
        }

        public async Task ActualizarExistencias(int idProducto, int existencias)
        {
            try
            {
                var producto = await _dbContext.TblProductos.FirstOrDefaultAsync(p => p.IdProducto == idProducto);

                if (producto != null)
                {
                    producto.Cantidad = existencias;

                    _dbContext.Entry(producto).State = EntityState.Modified;

                    // Marca solo ese campo como modificado (opcional pero útil si hay tracking)
                    _dbContext.Entry(producto).Property(p => p.Cantidad).IsModified = true;
                }
                else
                {
                    throw new Exception("Producto no encontrado");
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en Repositorio Venta: ActualizarExistencias", ex);
                throw;
            }
        }
    }
}
