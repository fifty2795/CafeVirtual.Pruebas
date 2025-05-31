using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using CafeVirtual.Pruebas.API.Services.DTO;
using CafeVirtual.Pruebas.API.Services.Settings;
using CafeVirtual.Pruebas.API.Utilidades.Response;
using CafeVirtual.Pruebas.API.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CafeVirtual.Pruebas.API.DTO;
using AutoMapper;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;
using CafeVirtual.Pruebas.API.Services.Interfaces;

namespace CafeVirtual.Pruebas.API.Services
{
    public class LoginService: ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TblUsuario> _repositorio;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;        
        private readonly ILogService _logService;
        private readonly IJwtService _jwtService;

        public LoginService(IUnitOfWork unitOfWork, IOptions<JwtSettings> jwtOptions, IMapper mapper, ILogService logService, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repositorio = _unitOfWork.Repository<TblUsuario>();
            _jwtSettings = jwtOptions.Value;
            _mapper = mapper;
            _logService = logService;
            _jwtService = jwtService;
        }

        public async Task<ResponseViewModel<UsuarioDTO>> Login(LoginDTO request)
        {
            var usuario = await _unitOfWork.RepositorioUsuario.ObtenerUsuarioByCredenciales(request.Email, request.Password);

            if (usuario == null)
                return ResponseHelper.CrearRespuestaError<UsuarioDTO>("Credenciales inválidas.", 401);

            usuario.Token = _jwtService.GenerateToken(usuario);

            var usuarioDto = _mapper.Map<UsuarioDTO>(usuario);

            return ResponseHelper.CrearRespuestaExito(usuarioDto, "Usuario autenticado exitosamente.");
        }
    }
}
