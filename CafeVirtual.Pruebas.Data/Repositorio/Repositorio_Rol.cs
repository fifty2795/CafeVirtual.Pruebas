using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Data.Repositorio
{
    public class Repositorio_Rol : IRol
    {
        private readonly MvcContext _dbContext;
        private readonly ILogService _logService;

        public Repositorio_Rol(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task<bool> ActualizarUsuarioByRol(int idUsuario, int idRol)
        {
            try
            {
                var usuario = await _dbContext.TblUsuarios.FirstOrDefaultAsync(p => p.IdUsuario == idUsuario);

                if (usuario != null)
                {
                    usuario.IdRol = idRol;

                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                else
                {
                    throw new Exception("Usuario no encontrado.");
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ActualizarUsuarioByRol", ex);
                return false;
            }
        }

        public async Task<bool> ActualizarUsuarioByRolMasivo(int idRol, int idRolNuevo)
        {
            try
            {
                var usuarios = await _dbContext.TblUsuarios
                    .Where(u => u.IdRol == idRol)
                    .ToListAsync();

                if (usuarios.Count == 0)
                    return true; // No hay usuarios para actualizar

                foreach (var usuario in usuarios)
                {
                    usuario.IdRol = idRolNuevo;
                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ActualizarUsuarioByRolMasivo", ex);
                return false;
            }
        }

        public async Task<List<TblMenu>> ObtenerMenusPorRolAsync(int idRol)
        {
            try
            {
                var menus = await _dbContext.TblMenus.Where(m => m.TblRoleMenus.Any(r => r.IdRole == idRol))
                    .Include(x => x.TblSubMenus)
                    .ToListAsync();

                return menus;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ActualizarUsuarioByRol", ex);
                return null;
            }
        }

        public async Task<bool> EliminarConfiguracionRoleMenus(int idRol)
        {
            try
            {
                var registros = await _dbContext.TblRoleMenus.Where(rm => rm.IdRole == idRol).ToListAsync();

                if (registros.Any())
                {
                    _dbContext.TblRoleMenus.RemoveRange(registros);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en EliminarConfiguracionRoleMenus", ex);
                return false;
            }
        }
    }
}
