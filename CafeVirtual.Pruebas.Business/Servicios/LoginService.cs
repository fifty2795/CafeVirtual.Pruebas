using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.Servicios
{
    public class LoginService: ILoginService
    {

        private readonly IUnitOfWork _unitOfWork;        
        private readonly IRepository<TblUsuario> _repositorio;
        private readonly ILogService _logService;

        public LoginService(IUnitOfWork unitOfWork, ILogService logService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));            
            _repositorio = _unitOfWork.Repository<TblUsuario>();
            _logService = logService;
        }

        public async Task<ResponseViewModel<TblUsuario>> ObtenerUsuario(string email, string password)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();                

                // Filtro por otros campos
                if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
                {
                    query = query.Where(u => u.Email == email.Trim() && u.Password == password.Trim());
                }

                query = query.Include(u => u.IdRolNavigation);

                var usuario = await query.FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return ResponseHelper.CrearRespuestaError<TblUsuario>("Credenciales invalidas.");
                }

                return ResponseHelper.CrearRespuestaExito(usuario, "Usuario obtenido exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerUsuario Service:Login", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Error al validar las credenciales.");
            }
        }
    }
}
