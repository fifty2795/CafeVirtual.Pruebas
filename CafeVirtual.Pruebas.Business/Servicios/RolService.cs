//using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Business.DTO;
using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Data.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.Servicios
{
    public class RolService: IRolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TblRol> _repositorio;        
        private readonly IRepository<TblUsuario> _repositorioUsuario;        
        private readonly ILogService _logService;
        
        public RolService(IUnitOfWork unitOfWork, ILogService logService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _repositorio = _unitOfWork.Repository<TblRol>();            
            _repositorioUsuario = _unitOfWork.Repository<TblUsuario>();            
            _logService = logService;
        }

        public async Task<ResponseViewModel<List<TblRol>>> ObtenerRoles(int? idRol)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                if (idRol != null)
                {
                    query = query.Where(x => x.IdRol != idRol);
                }

                query = query.Where(x => x.Activo == true);

                query = query.OrderBy(x => x.Nombre);

                var roles = await query.ToListAsync();

                return ResponseHelper.CrearRespuestaExito(roles, "Roles obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerRoles", ex);
                return ResponseHelper.CrearRespuestaError<List<TblRol>>("Hubo un error al obtener los roles. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<PaginatedList<TblRol>>> ObtenerRoles(string? busqueda, int pageNumber, int pageSize)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                // Filtro por otros campos
                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    query = query.Where(p => p.Nombre.Contains(busqueda) || p.Descripcion.Contains(busqueda));
                }                

                query = query.OrderBy(c => c.Nombre);

                var rolesPaginados = await PaginatedList<TblRol>.CreateAsync(query, pageNumber, pageSize);

                return ResponseHelper.CrearRespuestaExito(rolesPaginados, "Roles obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerRoles con busqueda", ex);
                return ResponseHelper.CrearRespuestaError<PaginatedList<TblRol>>("Error al obtener los roles filtrados.");
            }
        }

        public async Task<ResponseViewModel<TblRol>> ObtenerRolById(int idRol)
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();

                query = query.Where(p => p.IdRol == idRol);                

                var rol = await query.FirstOrDefaultAsync();

                if (rol == null)
                {
                    return new ResponseViewModel<TblRol>
                    {
                        Success = false,
                        Message = "Rol no encontrado."
                    };
                }

                return ResponseHelper.CrearRespuestaExito(rol, "Rol obtenido exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerRolById", ex);
                return ResponseHelper.CrearRespuestaError<TblRol>("Hubo un error al obtener el rol. Por favor intente de nuevo");
            }
        }

        //public async Task<ResponseViewModel<TblRol>> AgregarRol(TblRol rol)
        //{
        //    if (rol == null)
        //    {
        //        return new ResponseViewModel<TblRol>
        //        {
        //            Success = false,
        //            Message = "El rol no puede ser nulo."
        //        };
        //    }

        //    try
        //    {
        //        var nuevoRol = await _repositorio.AgregarAsync(rol);

        //        await _unitOfWork.SaveChangesAsync();

        //        return ResponseHelper.CrearRespuestaExito(nuevoRol, "Rol agregado con éxito");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("Ocurrio un error en AgregarRol", ex);
        //        return ResponseHelper.CrearRespuestaError<TblRol>("Hubo un error al agregar el rol. Por favor intente de nuevo");
        //    }
        //}

        //public async Task<ResponseViewModel<TblRol>> ActualizarRol(TblRol rol)
        //{
        //    if (rol == null)
        //    {
        //        return new ResponseViewModel<TblRol>
        //        {
        //            Success = false,
        //            Message = "El rol no puede ser nulo."
        //        };
        //    }

        //    try
        //    {
        //        var rolActualizado = _repositorio.Actualizar(rol);

        //        await _unitOfWork.SaveChangesAsync();

        //        return ResponseHelper.CrearRespuestaExito(rolActualizado, "Rol actualizado con éxito");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logService.LogError("Ocurrio un error en ActualizarRol", ex);
        //        return ResponseHelper.CrearRespuestaError<TblRol>("Hubo un error al actualizar el rol. Por favor intente de nuevo");
        //    }
        //}

        public async Task<ResponseViewModel<TblRol>> EliminarRol(int idRol)
        {
            try
            {
                // Eliminar configuracion actual de Menus 
                await _unitOfWork.RepositorioRol.EliminarConfiguracionRoleMenus(idRol);

                var rolEliminado = await _repositorio.EliminarAsync(idRol);

                if (rolEliminado == null)
                {
                    return new ResponseViewModel<TblRol>
                    {
                        Success = false,
                        Message = "No se pudo eliminar el rol, no existe."
                    };
                }

                await _unitOfWork.SaveChangesAsync();

                return ResponseHelper.CrearRespuestaExito(rolEliminado, "Rol eliminado con éxito");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en EliminarRol", ex);
                return ResponseHelper.CrearRespuestaError<TblRol>("Hubo un error al eliminar el rol. Por favor intente de nuevo");
            }
        }

        public async Task<ResponseViewModel<bool>> AgregarRoleMenus(TblRol rol, List<int> roleMenu)
        {
            try
            {
                var configuraciones = new List<TblRoleMenu>();

                rol.FechaCreacion = DateTime.Now;

                await _repositorio.AgregarAsync(rol);                
                await _unitOfWork.SaveChangesAsync();

                foreach (var menuId in roleMenu)
                {
                    var configuracion = new TblRoleMenu
                    {
                        IdRole = rol.IdRol,
                        IdMenu = menuId,
                        Activo = true
                    };

                    configuraciones.Add(configuracion);
                }

                if (configuraciones.Any())
                {
                    await _unitOfWork.RepositorioRoleMenus.AgregarRangoAsync(configuraciones);
                    await _unitOfWork.SaveChangesAsync();
                }

                return ResponseHelper.CrearRespuestaExito(true, "Rol configurado exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en AgregarRoleMenus", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Hubo un error al configurar el rol. Por favor intente de nuevo.");
            }
        }

        public async Task<ResponseViewModel<bool>> ActualizarRoleMenus(TblRol rol, List<int> roleMenu)
        {
            try
            {
                var configuraciones = new List<TblRoleMenu>();                

                _repositorio.Actualizar(rol, nameof(TblCliente.FechaCreacion));
                await _unitOfWork.SaveChangesAsync();

                // Eliminar configuracion actual
                await _unitOfWork.RepositorioRol.EliminarConfiguracionRoleMenus(rol.IdRol);

                foreach (var menuId in roleMenu)
                {
                    var configuracion = new TblRoleMenu
                    {
                        IdRole = rol.IdRol,
                        IdMenu = menuId,
                        Activo = true
                    };

                    configuraciones.Add(configuracion);
                }

                if (configuraciones.Any())
                {
                    await _unitOfWork.RepositorioRoleMenus.AgregarRangoAsync(configuraciones);
                    await _unitOfWork.SaveChangesAsync();
                }

                return ResponseHelper.CrearRespuestaExito(true, "Rol configurado exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en AgregarRoleMenus", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Hubo un error al configurar el rol. Por favor intente de nuevo.");
            }
        }

        public async Task<ResponseViewModel<bool>> ValidarExistenUsuariosByRol(int idRol)
        {
            try
            {
                var existenUsuarios = await _unitOfWork.RepositorioUsuario.ValidarExistenUsuariosByRol(idRol);                

                if (existenUsuarios)
                {
                    return ResponseHelper.CrearRespuestaExito(true, "El rol tiene usuarios asociados.");
                }
                return ResponseHelper.CrearRespuestaError<bool>("El rol no tiene usuarios asociados.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error al validar un rol asociado a usuarios", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Error al validar roles asociados.");
            }
        }

        public async Task<ResponseViewModel<PaginatedList<UsuarioDTO>>> ObtenerUsuariosByRol(int idRol, int pageNumber, int pageSize)
        {
            try
            {
                var query = _repositorioUsuario.ObtenerQueryable()
                    .Where(x => x.IdRol == idRol)
                    .Include(x => x.IdRolNavigation); 
                
                var UsuarioDTO = query.Select(x => new UsuarioDTO
                {
                    IdUsuario = x.IdUsuario,
                    NombreCompleto = x.Nombre + " " + x.ApellidoPaterno + " " + x.ApellidoMaterno,
                    Email = x.Email,
                    IdRol = x.IdRol,
                    RolNombre = x.IdRolNavigation.Nombre
                });

                if (UsuarioDTO.Any())
                {
                    var listaPaginada = await PaginatedList<UsuarioDTO>.CreateAsync(UsuarioDTO, pageNumber, pageSize);
                    return ResponseHelper.CrearRespuestaExito(listaPaginada, "El rol tiene usuarios asociados.");
                }

                return ResponseHelper.CrearRespuestaError<PaginatedList<UsuarioDTO>>("El rol no tiene usuarios asociados.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error al obtener los usuarios asociados al rol ObtenerUsuariosByRol", ex);
                return ResponseHelper.CrearRespuestaError<PaginatedList<UsuarioDTO>>("Error al obtener usuarios asociados.");
            }          
        }

        public async Task<ResponseViewModel<bool>> ActualizarUsuarioByRol(int idUsuario, int idRol)
        {
            try
            {
                var usuarioActualizado = await _unitOfWork.RepositorioRol.ActualizarUsuarioByRol(idUsuario, idRol);

                return ResponseHelper.CrearRespuestaExito(true, "Se actualizo el rol al usuario.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ActualizarUsuarioByRol", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Error al actualizar el rol.");
            }
        }

        public async Task<ResponseViewModel<bool>> ActualizarUsuarioByRolMasivo(int idRol, int idRolNuevo)
        {
            try
            {
                var usuarioActualizado = await _unitOfWork.RepositorioRol.ActualizarUsuarioByRolMasivo(idRol, idRolNuevo);

                return ResponseHelper.CrearRespuestaExito(true, "Se actualizo el rol a los usuarios.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ActualizarUsuarioByRolMasivo", ex);
                return ResponseHelper.CrearRespuestaError<bool>("Error al actualizar el rol de forma masiva.");
            }
        }
    }
}
