using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Data.Interfaces
{
    public interface IRol
    {
        Task<bool> ActualizarUsuarioByRol(int idUsuario, int idRol);

        Task<bool> ActualizarUsuarioByRolMasivo(int idRol, int idRolNuevo);

        Task<List<TblMenu>> ObtenerMenusPorRolAsync(int idRol);

        Task<bool> EliminarConfiguracionRoleMenus(int idRol);
    }
}
