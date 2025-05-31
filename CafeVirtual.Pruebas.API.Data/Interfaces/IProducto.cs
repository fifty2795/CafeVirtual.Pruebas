using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Data.Interfaces
{
    public interface IProducto
    {
        Task<List<TblProducto?>> ObtenerProducto(string? busqueda);

        Task<int> ObtenerCantidad(int idProducto);

        Task<bool> EliminarProducto(int idProducto);

        Task ActualizarProducto(TblProducto producto);

        Task ActualizarExistencias(int idProducto, int existencias);
    }
}
