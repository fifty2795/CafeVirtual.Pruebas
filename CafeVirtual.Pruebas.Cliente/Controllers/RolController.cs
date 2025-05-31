using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Cliente.Models;
using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;

namespace CafeVirtual.Pruebas.Cliente.Controllers
{
    public class RolController : Controller
    {
        private readonly IRolService _rolService;
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public RolController(IRolService rolService, IMenuService menuService, IMapper mapper)
        {
            _rolService = rolService;
            _menuService = menuService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string? txtBusqueda, int pageNumber)
        {
            int pageSize = 5;

            ViewBag.Busqueda = txtBusqueda;

            if (string.IsNullOrEmpty(txtBusqueda))
            {
                txtBusqueda = string.Empty;
            }

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            var response = await _rolService.ObtenerRoles(txtBusqueda, pageNumber, pageSize);

            return View(response);
        }

        //public IActionResult AgregarRol()
        //{
        //    var response = new ResponseViewModel<TblRol>();

        //    response.Data = new TblRol();

        //    return View(response);
        //}

        public async Task<IActionResult> AgregarRol()
        {
            var model = new RolViewModel
            {
                FechaCreacion = DateTime.Now,
                Activo = true,
                Menus = await ObtenerMenusConSubmenus()
            };

            return View(model);
        }        

        //[HttpPost]
        //public async Task<IActionResult> AgregarRol(TblRol rol)
        //{
        //    var response = await _rolService.AgregarRol(rol);

        //    return Json(response);
        //}
        
        [HttpPost]
        public async Task<IActionResult> AgregarRol(RolViewModel rolViewModel)
        {
            var rol = _mapper.Map<TblRol>(rolViewModel);

            var response = await _rolService.AgregarRoleMenus(rol, rolViewModel.MenusSeleccionados);

            return Json(response);            
        }

        //public async Task<IActionResult> EditarRol(int id)
        //{
        //    var response = await _rolService.ObtenerRolById(id);

        //    ViewBag.IdRol = id;

        //    return View(response);
        //}
        
        public async Task<IActionResult> EditarRol(int id)
        {
            // Obtener Rol
            var response = await _rolService.ObtenerRolById(id);
            var model = _mapper.Map<RolViewModel>(response.Data);

            // Obtener Menus
            var responseMenus = await _menuService.ObtenerMenus();
            var menus = _mapper.Map<List<MenuViewModel>>(responseMenus.Data);            

            // Obtener Menus por Rol
            var menusSeleccionados = await _menuService.ObtenerMenusPorRolAsync(id);

            var lstMenusSeleccionados = new List<int>();

            foreach (var menuSeleccionado in menusSeleccionados)
            {
                lstMenusSeleccionados.Add(menuSeleccionado.IdMenu);
            }

            // Set Menus y Menus Seleccionados
            model.Menus = menus;
            model.MenusSeleccionados = lstMenusSeleccionados;

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> EditarRol(RolViewModel rolViewModel)
        {
            var rol = _mapper.Map<TblRol>(rolViewModel);

            var response = await _rolService.ActualizarRoleMenus(rol, rolViewModel.MenusSeleccionados);

            return Json(response);            
        }

        public async Task<IActionResult> EliminarRol(int id)
        {
            var responseRol = await _rolService.ObtenerRolById(id);

            ViewBag.IdRol = id;

            var lstRoles = await _rolService.ObtenerRoles(id);

            var model = new EditarUsuarioViewModel
            {
                IdRol = id,
                Nombre = responseRol.Data.Nombre,
                Descripcion = responseRol.Data.Descripcion,
                Roles = lstRoles.Data.Select(r => new SelectListItem
                {
                    Value = r.IdRol.ToString(),
                    Text = r.Nombre
                }).ToList()
                /*IdRolSeleccionado = 0*/ // o el rol actual del usuario si estás editando
            };

            return View(model);
        }

        public async Task<IActionResult> EliminarRolConfirm(int idRol)
        {
            var response = await _rolService.EliminarRol(idRol);

            return Json(response);
        }

        private async Task<List<MenuViewModel>> ObtenerMenusConSubmenus()
        {
            var response = await _menuService.ObtenerMenus();

            var menus = _mapper.Map<List<MenuViewModel>>(response.Data);

            return menus;
        }

        public async Task<IActionResult> ValidarExisteUsuarioByRol(int idRol, int pageNumber)
        {
            int pageSize = 5;

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            var existeUsuarioAsociado = await _rolService.ValidarExistenUsuariosByRol(idRol);

            if (existeUsuarioAsociado.Success)
            {
                var response = await _rolService.ObtenerUsuariosByRol(idRol, pageNumber, pageSize);

                if (response.Success)
                {
                    return Json(new
                    {
                        success = true,
                        data = response.Data,
                        message = response.Message
                    });
                }
            }

            return Json(new { success = false, message = existeUsuarioAsociado.Message });
        }

        [HttpPost]

        public async Task<IActionResult> ActualizarUsuarioByRol(int idUsuario, int idRol)
        {
            var response = await _rolService.ActualizarUsuarioByRol(idUsuario, idRol);

            return Json(response);
        }

        [HttpPost]

        public async Task<IActionResult> ActualizarUsuarioByRolMasivo(int idRol, int idRolNuevo)
        {
            var response = await _rolService.ActualizarUsuarioByRolMasivo(idRol, idRolNuevo);

            return Json(response);
        }
    }
}
