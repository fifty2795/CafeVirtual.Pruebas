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
    public class Repositorio_RoleMenus : IRoleMenus
    {
        private readonly MvcContext _dbContext;
        private readonly ILogService _logService;

        public Repositorio_RoleMenus(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task AgregarRangoAsync(List<TblRoleMenu> configuraciones)
        {
            try
            {
                if (configuraciones != null && configuraciones.Any())
                {
                    await _dbContext.TblRoleMenus.AddRangeAsync(configuraciones);
                }
            }
            catch (Exception ex)
            {
                _logService.LogError("Error al agregar un rango de RoleMenus - AgregarRangoAsync", ex);
                throw;
            }
        }
    }
}
