using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.Interfaces
{
    public interface IClienteService
    {
        Task<ResponseViewModel<List<TblCliente>>> ObtenerClientes();

        Task<ResponseViewModel<PaginatedList<TblCliente>>> ObtenerClientes(string busqueda, int pageNumber, int pageSize);

        Task<ResponseViewModel<TblCliente>> ObtenerClienteById(int idCliente);

        Task<ResponseViewModel<TblCliente>> AgregarCliente(TblCliente cliente);

        Task<ResponseViewModel<TblCliente>> ActualizarCliente(TblCliente cliente);

        Task<ResponseViewModel<TblCliente>> EliminarCliente(int idCliente);

        Task<ResponseViewModel<bool>> EliminarClienteVenta(int idCliente);
    }
}
