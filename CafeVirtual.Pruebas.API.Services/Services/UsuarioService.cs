using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using CafeVirtual.Pruebas.API.Utilidades.Response;
using CafeVirtual.Pruebas.API.Services.Interfaces;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;

namespace CafeVirtual.Pruebas.API.Services.Services
{
    public class UsuarioService: IUsuarioService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TblUsuario> _repositorio;
        private readonly ILogService _logService;

        public UsuarioService(IUnitOfWork unitOfWork, ILogService logService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repositorio = _unitOfWork.Repository<TblUsuario>();
            _logService = logService;
        }

        public async Task<ResponseViewModel<TblUsuario>> ObtenerUsuario(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Email y contraseña son requeridos.", 400);
            }

            try
            {
                var usuario = await _unitOfWork.RepositorioUsuario.ObtenerUsuarioByCredenciales(email, password);

                if (usuario == null)
                {
                    return ResponseHelper.CrearRespuestaError<TblUsuario>("Credenciales invalidas.", 401);
                }

                return ResponseHelper.CrearRespuestaExito(usuario, "Usuario obtenido exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerUsuario", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Error al validar las credenciales.", 500);
            }
        }
    }
}
