using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Utilidades.Model;
using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Business.DTO;

namespace CafeVirtual.Pruebas.Business.Servicios
{
    public class UsuarioService : IUsuarioService
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

        public async Task<ResponseViewModel<List<UsuarioViewModel>>> ObtenerUsuarios()
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                var usuarios = await query.Select(x => new UsuarioViewModel
                {
                    IdUsuario = x.IdUsuario.ToString(),
                    NombreCompleto = x.Nombre + " " + x.ApellidoPaterno,
                    Status = x.Status,
                    RutaImagen = string.IsNullOrWhiteSpace(x.RutaImagen) ? "/images/avatar-default.png" : x.RutaImagen
                }).ToListAsync();           

                return ResponseHelper.CrearRespuestaExito(usuarios, "Usuarios obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerUsuarios", ex);
                return ResponseHelper.CrearRespuestaError<List<UsuarioViewModel>>("Hubo un error al obtener los usuarios. Por favor intente de nuevo");
            }
        } 
        
        public async Task<ResponseViewModel<PaginatedList<TblUsuario>>> ObtenerUsuarios(string busqueda, int pageNumber, int pageSize)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                // Filtro por otros campos
                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    query = query.Where(p => p.Nombre.Contains(busqueda) || p.ApellidoPaterno.Contains(busqueda) || p.ApellidoMaterno.Contains(busqueda) ||
                                         p.IdRolNavigation.Nombre.Contains(busqueda)  || p.Email.Contains(busqueda));
                }

                query = query.Include(c => c.IdRolNavigation);

                query = query.OrderBy(c => c.Nombre);

                var usuariosPaginados = await PaginatedList<TblUsuario>.CreateAsync(query, pageNumber, pageSize);

                return ResponseHelper.CrearRespuestaExito(usuariosPaginados, "Usuarios obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerUsuarios", ex);
                return ResponseHelper.CrearRespuestaError<PaginatedList<TblUsuario>>("Hubo un error al obtener los usuarios. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblUsuario>> ObtenerUsuarioById(int idUsuario)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                query = query.Where(p => p.IdUsuario == idUsuario);

                query = query.Include(c => c.IdRolNavigation);

                var usuario = await query.FirstOrDefaultAsync();

                if (usuario == null)
                {
                    return new ResponseViewModel<TblUsuario>
                    {
                        Success = false,
                        Message = "Usuario no encontrado."
                    };
                }

                return ResponseHelper.CrearRespuestaExito(usuario, "Usuario obtenido exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerUsuarioById", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Hubo un error al obtener el usuario. Por favor intente de nuevo");
            }
        }      

        public async Task<ResponseViewModel<TblUsuario>> AgregarUsuario(TblUsuario usuario)
        {
            if (usuario == null)
            {
                return new ResponseViewModel<TblUsuario>
                {
                    Success = false,
                    Message = "El usuario no puede ser nulo."
                };
            }

            try
            {
                usuario.FechaCreacion = DateTime.Now;
                var nuevoUsuario = await _repositorio.AgregarAsync(usuario);

                await _unitOfWork.SaveChangesAsync();

                return ResponseHelper.CrearRespuestaExito(nuevoUsuario, "Usuario agregado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en AgregarUsuario", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Hubo un error al agregar el usuario. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblUsuario>> ActualizarUsuario(TblUsuario usuario)
        {
            if (usuario == null)
            {
                return new ResponseViewModel<TblUsuario>
                {
                    Success = false,
                    Message = "El usuario no puede ser nulo."
                };
            }

            try
            {
                var usuarioActualizado = await _unitOfWork.RepositorioUsuario.ActualizarUsuarioForm(usuario);

                return ResponseHelper.CrearRespuestaExito(usuarioActualizado, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ActualizarUsuario", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Hubo un error al actualizar el usuario. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblUsuario>> ActualizarStatus(int idUsuario, string status)
        {
            if (idUsuario <= 0)
            {
                return new ResponseViewModel<TblUsuario>
                {
                    Success = false,
                    Message = "El usuario no puede ser nulo."
                };
            }

            try
            {
                var usuarioActualizado = await _unitOfWork.RepositorioUsuario.ActualizarStatus(idUsuario, status);

                return ResponseHelper.CrearRespuestaExito(usuarioActualizado, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ActualizarStatus", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Hubo un error al actualizar el status de usuario. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblUsuario>> ActualizarUsuarioModal(TblUsuario usuario)
        {
            if (usuario == null)
            {
                return new ResponseViewModel<TblUsuario>
                {
                    Success = false,
                    Message = "El usuario no puede ser nulo."
                };
            }

            try
            {
                var usuarioUpdate = await _unitOfWork.RepositorioUsuario.ActualizarUsuarioModal(usuario);

                return ResponseHelper.CrearRespuestaExito(usuario, "Usuario actualizado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ActualizarUsuario", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Hubo un error al actualizar el usuario. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<TblUsuario>> EliminarUsuario(int idUsuario)
        {
            try
            {
                var usuarioEliminado = await _repositorio.EliminarAsync(idUsuario);

                if (usuarioEliminado == null)
                {
                    return new ResponseViewModel<TblUsuario>
                    {
                        Success = false,
                        Message = "No se pudo eliminar el usuario, no existe."
                    };
                }

                await _unitOfWork.SaveChangesAsync();

                return ResponseHelper.CrearRespuestaExito(usuarioEliminado, "Usuario eliminado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en EliminarUsuario", ex);
                return ResponseHelper.CrearRespuestaError<TblUsuario>("Hubo un error al eliminar el usuario. Por favor intente de nuevo");
            }
        }
    }
}
