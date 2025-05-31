using CafeVirtual.Pruebas.Business.DTO;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResponseViewModel<List<UsuarioViewModel>>> ObtenerUsuarios();

        Task<ResponseViewModel<PaginatedList<TblUsuario>>> ObtenerUsuarios(string busqueda, int pageNumber, int pageSize);

        Task<ResponseViewModel<TblUsuario>> ObtenerUsuarioById(int idUsuario);

        Task<ResponseViewModel<TblUsuario>> AgregarUsuario(TblUsuario usuario);

        Task<ResponseViewModel<TblUsuario>> ActualizarUsuario(TblUsuario usuario);

        Task<ResponseViewModel<TblUsuario>> ActualizarStatus(int idUsuario, string status);

        Task<ResponseViewModel<TblUsuario>> ActualizarUsuarioModal(TblUsuario usuario);

        Task<ResponseViewModel<TblUsuario>> EliminarUsuario(int idUsuario);
    }
}
