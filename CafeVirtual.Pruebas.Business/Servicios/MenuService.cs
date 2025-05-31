//using DocumentFormat.OpenXml.Office2016.Drawing.Charts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class MenuService: IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRolService _rolService;
        private readonly IRepository<TblMenu> _repositorio;
        private readonly IRepository<TblRol> _repositorioRol;
        private readonly IRepository<TblRoleMenu> _repositorioRoleMenu;
        private readonly ILogService _logService;

        public MenuService(IUnitOfWork unitOfWork, ILogService logService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));            
            _repositorio = _unitOfWork.Repository<TblMenu>();
            _repositorioRol = _unitOfWork.Repository<TblRol>();
            _repositorioRoleMenu = _unitOfWork.Repository<TblRoleMenu>();
            _logService = logService;
        }

        public async Task<ResponseViewModel<List<TblMenu>>> ObtenerMenus()
        {
            try
            {
                var query = _repositorio.ObtenerQueryable();
                
                query = query.OrderBy(c => c.IdMenu);     
                
                var data = await query.ToListAsync();

                return ResponseHelper.CrearRespuestaExito(data, "Menus obtenidos exitosamente.");
            }
            catch (Exception ex)
            {
                _logService.LogError("Ocurrio un error en ObtenerMenus", ex);
                return ResponseHelper.CrearRespuestaError<List<TblMenu>>("Hubo un error al obtener los menus. Por favor intente de nuevo");
            }
        }

        public async Task<List<TblMenu>> ObtenerMenusPorRolAsync(int idRol)
        {
            return await _unitOfWork.RepositorioRol.ObtenerMenusPorRolAsync(idRol);            
        }        
    }
}
