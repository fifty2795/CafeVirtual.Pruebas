using CafeVirtual.Pruebas.API.Services.DTO;
using CafeVirtual.Pruebas.API.Utilidades.Response;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Services.Interfaces
{
    public interface IProductoService
    {
        Task<ResponseViewModel<List<ProductoDTO>>> ObtenerProducto(string? busqueda);

        Task<ResponseViewModel<ProductoDTO>> ObtenerProductoById(int idProducto);

        Task<ResponseViewModel<ProductoDTO>> AgregarProducto(TblProducto producto);

        Task<ResponseViewModel<ProductoDTO>> EditarProducto(TblProducto producto);

        Task<ResponseViewModel<bool>> EliminarProducto(int idProducto);
    }
}
