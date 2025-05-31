using CafeVirtual.Pruebas.Business.API.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.API.Interfaces
{
    public interface IApiProductoService
    {
        Task<ResultProductoListApiDTO?> ObtenerProducto(string token, string? busqueda);

        Task<ResultProductoApiDTO?> ObtenerProductoById(string token, int idProducto);

        Task<ResultProductoApiDTO?> AgregarProducto(string token, ProductoDto producto);

        Task<ResultProductoApiDTO?> EditarProducto(string token, ProductoDto producto);

        Task<ResultEliminarProductoApiDTO?> EliminarProducto(string token, int idProducto);
    }
}
