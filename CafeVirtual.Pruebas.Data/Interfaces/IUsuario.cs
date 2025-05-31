using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Utilidades.Utilidades;

namespace CafeVirtual.Pruebas.Data.Interfaces
{
    public interface IUsuario
    {
        Task<TblUsuario> ActualizarUsuarioForm(TblUsuario usuario);

        Task<bool> ActualizarUsuarioModal(TblUsuario usuario);

        Task<TblUsuario> ActualizarStatus(int idUsuario, string status);

        Task<bool> ValidarExistenUsuariosByRol(int idRol);
    }
}
