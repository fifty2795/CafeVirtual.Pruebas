using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Data.Repositorio
{
    public class Repositorio_Usuario: IUsuario
    {
        private readonly MvcContext _dbContext;
        private readonly ILogService _logService;

        public Repositorio_Usuario(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task<TblUsuario?> ObtenerUsuarioByCredenciales(string email, string password)
        {
            return await _dbContext.TblUsuarios.Include(u => u.IdRolNavigation).Where(u => u.Email == email.Trim() &&
                                                                                      u.Password == password.Trim() &&
                                                                                      u.Activo == true &&
                                                                                      u.IdRolNavigation.Activo == true).FirstOrDefaultAsync();            
        }
    }
}
