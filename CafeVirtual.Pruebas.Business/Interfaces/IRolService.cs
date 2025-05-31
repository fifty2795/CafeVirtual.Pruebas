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
    public interface IRolService
    {
        Task<ResponseViewModel<List<TblRol>>> ObtenerRoles(int? idRol);

        Task<ResponseViewModel<PaginatedList<TblRol>>> ObtenerRoles(string? busqueda, int pageNumber, int pageSize);

        Task<ResponseViewModel<TblRol>> ObtenerRolById(int idRol);

        //Task<ResponseViewModel<TblRol>> AgregarRol(TblRol rol);

        //Task<ResponseViewModel<TblRol>> ActualizarRol(TblRol rol);

        Task<ResponseViewModel<TblRol>> EliminarRol(int idRol);

        Task<ResponseViewModel<bool>> AgregarRoleMenus(TblRol rol, List<int> roleMenu);

        Task<ResponseViewModel<bool>> ActualizarRoleMenus(TblRol rol, List<int> roleMenu);

        Task<ResponseViewModel<bool>> ValidarExistenUsuariosByRol(int idRol);

        Task<ResponseViewModel<PaginatedList<UsuarioDTO>>> ObtenerUsuariosByRol(int idRol, int pageNumber, int pageSize);

        Task<ResponseViewModel<bool>> ActualizarUsuarioByRol(int idUsuario, int idRol);

        Task<ResponseViewModel<bool>> ActualizarUsuarioByRolMasivo(int idRol, int idRolNuevo);
    }    
}
