using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Utilidades.Utilidades;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Utilidades.Model;
using Microsoft.EntityFrameworkCore;

namespace CafeVirtual.Pruebas.Data.Repositorio
{
    public class Repositorio_Usuario : IUsuario
    {
        private readonly MvcContext _dbContext;
        private readonly ILogService _logService;

        public Repositorio_Usuario(MvcContext dbContext, ILogService logService)
        {
            _dbContext = dbContext;
            _logService = logService;
        }

        public async Task<TblUsuario> ActualizarUsuarioForm(TblUsuario usuario)
        {
            try
            {
                var usuarioExistente = await _dbContext.TblUsuarios.FindAsync(usuario.IdUsuario);

                if (usuarioExistente == null) return usuario;

                usuarioExistente.IdUsuario = usuario.IdUsuario;
                usuarioExistente.IdRol = usuario.IdRol;
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
                usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Activo = usuario.Activo;

                if (usuario.RutaImagen != null)
                {
                    usuarioExistente.RutaImagen = usuario.RutaImagen;
                }

                await _dbContext.SaveChangesAsync();

                return usuario;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ActualizarUsuario", ex);
                throw;
            }
        }

        public async Task<bool> ActualizarUsuarioModal(TblUsuario usuario)
        {
            try
            {
                var usuarioExistente = await _dbContext.TblUsuarios.FindAsync(usuario.IdUsuario);

                if (usuarioExistente == null) return false;

                usuarioExistente.IdUsuario = usuario.IdUsuario;
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
                usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
                usuarioExistente.Email = usuario.Email;
                usuarioExistente.Password = usuario.Password;

                if (usuario.RutaImagen != null)
                {
                    usuarioExistente.RutaImagen = usuario.RutaImagen;
                }

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ActualizarUsuarioModal", ex);
                return false;
            }
        }

        public async Task<TblUsuario> ActualizarStatus(int idUsuario, string status)
        {
            try
            {
                var usuarioExistente = await _dbContext.TblUsuarios.FindAsync(idUsuario);

                usuarioExistente.Status = status;
                usuarioExistente.UltimaConexion = DateTime.UtcNow;

                _dbContext.Entry(usuarioExistente).Property(u => u.Status).IsModified = true;
                _dbContext.Entry(usuarioExistente).Property(u => u.UltimaConexion).IsModified = true;

                await _dbContext.SaveChangesAsync();

                return usuarioExistente;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ActualizarStatus", ex);
                throw;
            }
        }

        public async Task<bool> ValidarExistenUsuariosByRol(int idRol)
        {
            try
            {
                var existenUsuarios = await _dbContext.TblUsuarios.AsQueryable().AnyAsync(x => x.IdRol == idRol);

                return existenUsuarios;
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrió un error en el repositorio en ValidarExistenUsuariosByRol", ex);
                return false;
            }
        }
    }
}
