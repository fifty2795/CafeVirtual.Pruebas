using Microsoft.AspNetCore.Mvc;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Cliente.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using AutoMapper;

namespace CafeVirtual.Pruebas.Cliente.Controllers
{    
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMenuService _menuService;
        private readonly IRolService _rol;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;        

        public UsuarioController(IUsuarioService usuarioService, IMenuService menuService, IRolService rol, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _usuarioService = usuarioService;
            _menuService = menuService;
            _rol = rol;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        //[Authorize(Roles = "Administrador")]
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

            var response = await _usuarioService.ObtenerUsuarios(txtBusqueda, pageNumber, pageSize);

            return View(response);
        }

        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AgregarUsuario()
        {
            var response = await _rol.ObtenerRoles(null);

            var model = new AgregarUsuarioViewModel
            {
                Roles = response.Data.Select(p => new SelectListItem
                {
                    Value = p.IdRol.ToString(),
                    Text = p.Nombre
                }).ToList()
            };

            return View(model);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> AgregarUsuario([FromForm] TblUsuario usuario, IFormFile imagenPerfil)
        {
            // Guardar imagen 
            if (imagenPerfil != null && imagenPerfil.Length > 0)
            {
                usuario = await GuardarAvatar(usuario, imagenPerfil);
            }

            var response = await _usuarioService.AgregarUsuario(usuario);

            return Json(response);
        }

        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EditarUsuario(int id)
        {
            var responseUsuario = await _usuarioService.ObtenerUsuarioById(id);

            var responseRoles = await _rol.ObtenerRoles(null);            

            var usuario = new TblUsuario();

            usuario.IdUsuario = responseUsuario.Data.IdUsuario;
            usuario.IdRol = responseUsuario.Data.IdRol;
            usuario.Nombre = responseUsuario.Data.Nombre;
            usuario.ApellidoPaterno = responseUsuario.Data.ApellidoPaterno;
            usuario.ApellidoMaterno = responseUsuario.Data.ApellidoMaterno;
            usuario.Email = responseUsuario.Data.Email;
            usuario.Password = responseUsuario.Data.Password;
            usuario.Activo = responseUsuario.Data.Activo;
            usuario.FechaCreacion = responseUsuario.Data.FechaCreacion;

            var model = new AgregarUsuarioViewModel
            {
                Usuario = usuario,
                Roles = responseRoles.Data.Select(p => new SelectListItem
                {
                    Value = p.IdRol.ToString(),
                    Text = p.Nombre,
                    Selected = p.IdRol == responseUsuario.Data.IdRol
                }).ToList()
            };

            return View(model);
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> EditarUsuario([FromForm] TblUsuario usuario, IFormFile imagenPerfil)
        {
            // Guardar imagen 
            if (imagenPerfil != null && imagenPerfil.Length > 0)
            {
                usuario = await GuardarAvatar(usuario, imagenPerfil);
            }           

            // Actualizar Usuario
            var response = await _usuarioService.ActualizarUsuario(usuario);            

            var usuarioActualizado = await _usuarioService.ObtenerUsuarioById(usuario.IdUsuario);

            response.Data.RutaImagen = usuarioActualizado.Data.RutaImagen;

            // Create Claims
            int idUsuarioClaim = Convert.ToInt32(User.FindFirst("IdUsuario")?.Value);

            if (usuarioActualizado.Data.IdUsuario == idUsuarioClaim)
            {
                if (response.Success && User.Identity.IsAuthenticated)
                {
                    var token = User.FindFirst("AccessToken")?.Value;
                    var menus = User.FindFirst("Menus")?.Value;

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuarioActualizado.Data.Nombre + " " + usuarioActualizado.Data.ApellidoPaterno),
                            new Claim(ClaimTypes.Email, usuarioActualizado.Data.Email),
                            new Claim(ClaimTypes.Role, usuarioActualizado.Data.IdRolNavigation.Nombre),
                            new Claim("IdUsuario", usuarioActualizado.Data.IdUsuario.ToString()),
                            new Claim("IdRol", usuarioActualizado.Data.IdRol.ToString()),
                            new Claim("FotoUrl", string.IsNullOrEmpty(usuarioActualizado.Data.RutaImagen) ? "images/avatar-default.png" : usuarioActualizado.Data.RutaImagen),
                            new Claim("AccessToken", token),
                            new Claim("Menus", menus)
                        };                    

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
            }

            return Json(response);
        }

        //[Authorize(Roles = "Administrador,Usuario,Ventas")]
        public async Task<IActionResult> EditarUsuarioParcial()
        {            
            int idUsuario = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "IdUsuario").Value);
            
            var usuario = await _usuarioService.ObtenerUsuarioById(idUsuario); 

            if (usuario == null)
                return NotFound();            

            var model = new AgregarUsuarioViewModel
            {
                Usuario = usuario.Data                
            };

            return PartialView("EditarUsuarioPartialView", model);
        }

        //[Authorize(Roles = "Administrador,Usuario,Ventas")]
        [HttpPost]
        public async Task<IActionResult> GuardarUsuarioParcial([FromForm] TblUsuario usuario, IFormFile imagenPerfil)
        {
            // Guardar imagen 
            if (imagenPerfil != null && imagenPerfil.Length > 0)
            {
                usuario = await GuardarAvatar(usuario, imagenPerfil);
            }

            // Actualizar Usuario
            var response = await _usuarioService.ActualizarUsuarioModal(usuario);

            var usuarioActualizado = await _usuarioService.ObtenerUsuarioById(usuario.IdUsuario);            

            if (response.Success && User.Identity.IsAuthenticated)
            {
                response.Data = usuarioActualizado.Data;

                var token = User.FindFirst("AccessToken")?.Value;
                var menus = User.FindFirst("Menus")?.Value;

                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, usuarioActualizado.Data.Nombre + " " + usuarioActualizado.Data.ApellidoPaterno),
                            new Claim(ClaimTypes.Email, usuarioActualizado.Data.Email),
                            new Claim(ClaimTypes.Role, usuarioActualizado.Data.IdRolNavigation.Nombre),
                            new Claim("IdUsuario", usuarioActualizado.Data.IdUsuario.ToString()),
                            new Claim("IdRol", usuarioActualizado.Data.IdRol.ToString()),
                            new Claim("FotoUrl", string.IsNullOrEmpty(usuarioActualizado.Data.RutaImagen) ? "images/avatar-default.png" : usuarioActualizado.Data.RutaImagen),
                            new Claim("AccessToken", token),
                            new Claim("Menus", menus)
                        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }         
            return Json(response);
        }

        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var response = await _usuarioService.ObtenerUsuarioById(id);

            ViewBag.IdUsuario = id;

            return View(response);
        }

        //[Authorize(Roles = "Administrador")]
        public async Task<IActionResult> EliminarUsuarioConfirm(int idUsuario)
        {
            var response = await _usuarioService.EliminarUsuario(idUsuario);

            return Json(response);
        }

        //[Authorize(Roles = "Administrador")]
        public async Task<TblUsuario> GuardarAvatar(TblUsuario usuario, IFormFile imagenPerfil)
        {
            var ext = Path.GetExtension(imagenPerfil.FileName);
            var nombreArchivo = $"avatar-{Guid.NewGuid()}{ext}";
            var rutaCarpeta = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatares");

            if (usuario.IdUsuario != 0)
            {
                var usuarioObj = await _usuarioService.ObtenerUsuarioById(usuario.IdUsuario);

                var nombreArchivoActual = Path.GetFileName(usuarioObj.Data.RutaImagen);
                var rutaArchivoActual = Path.Combine(rutaCarpeta, nombreArchivoActual);

                if (nombreArchivoActual != "avatar-default.png" && System.IO.File.Exists(rutaArchivoActual))
                {
                    System.IO.File.Delete(rutaArchivoActual);
                }
            }            

            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
            using (var stream = new FileStream(rutaCompleta, FileMode.Create))
            {
                await imagenPerfil.CopyToAsync(stream);
            }

            usuario.RutaImagen = $"/images/avatares/{nombreArchivo}";

            return usuario;
        }

        //[Authorize(Roles = "Administrador")]
        public void EliminarAvatar(string ruta)
        {
            if (string.IsNullOrEmpty(ruta))
                return;

            var rutaCompleta = Path.Combine(_webHostEnvironment.WebRootPath, ruta.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (System.IO.File.Exists(rutaCompleta))
            {
                System.IO.File.Delete(rutaCompleta);
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> ExportarExcel(string? nombreUsuario)
        //{
        //    string excelName = $"Usuarios-{DateTime.Now:yyyyMMddHHmmss}.xlsx";

        //    if (string.IsNullOrEmpty(nombreUsuario))
        //    {
        //        nombreUsuario = string.Empty;
        //    }

        //    try
        //    {
        //        var lstUsuario = await _usuarioService.ObtenerUsuariosToList(nombreUsuario);

        //        using (var workbook = new XLWorkbook())
        //        {
        //            var worksheet = workbook.Worksheets.Add("Usuarios");

        //            // Añadir el encabezado
        //            worksheet.Cell(1, 1).Value = "ID";
        //            worksheet.Cell(1, 2).Value = "Nombre";
        //            worksheet.Cell(1, 3).Value = "Apellido Paterno";
        //            worksheet.Cell(1, 4).Value = "Apellido Materno";
        //            worksheet.Cell(1, 5).Value = "Identificacion";
        //            worksheet.Cell(1, 6).Value = "Cargo";
        //            worksheet.Cell(1, 7).Value = "Activo";
        //            worksheet.Cell(1, 8).Value = "Fecha Creacion";

        //            int row = 2;
        //            // Añadir los datos                    
        //            foreach (var usuario in lstUsuario)
        //            {
        //                worksheet.Cell(row, 1).Value = usuario.IdUsuario;
        //                worksheet.Cell(row, 2).Value = usuario.Nombre;
        //                worksheet.Cell(row, 3).Value = usuario.ApellidoPaterno;
        //                worksheet.Cell(row, 4).Value = usuario.ApellidoMaterno;
        //                worksheet.Cell(row, 5).Value = usuario.Identificacion;
        //                worksheet.Cell(row, 6).Value = usuario.Cargo;
        //                worksheet.Cell(row, 7).Value = usuario.Activo;
        //                worksheet.Cell(row, 8).Value = usuario.FechaCreacion.ToString();

        //                row++;
        //            }

        //            using (var stream = new MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                var content = stream.ToArray();
        //                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> ImportarExcel(IFormFile fileInput)
        //{
        //    try
        //    {
        //        if (fileInput == null || fileInput.Length == 0)
        //        {
        //            return BadRequest("No se ha proporcionado un archivo.");
        //        }

        //        var response = new ResponseViewModel<TblUsuario>();

        //        var usuarios = new List<TblUsuario>();

        //        using (var stream = new MemoryStream())
        //        {
        //            await fileInput.CopyToAsync(stream);
        //            using (var workbook = new XLWorkbook(stream))
        //            {
        //                var worksheet = workbook.Worksheet(1);
        //                var rows = worksheet.RowsUsed();

        //                var firstRowUsed = worksheet.FirstRowUsed();
        //                // Obtener el número de la primera fila utilizada
        //                int firstRowNumber = firstRowUsed.RowNumber();

        //                foreach (var row in rows)
        //                {
        //                    if (row.RowNumber() == firstRowNumber)
        //                    {
        //                        // Omitir la primera fila (encabezado)
        //                        continue;
        //                    }

        //                    var usuario = new TblUsuario();

        //                    //usuario.IdUsuario = row.Cell(1).GetValue<int>();
        //                    usuario.Nombre = row.Cell(2).GetString();
        //                    usuario.ApellidoPaterno = row.Cell(3).GetString();
        //                    usuario.ApellidoMaterno = row.Cell(4).GetString();
        //                    usuario.Identificacion = row.Cell(5).GetValue<int>();
        //                    usuario.Cargo = row.Cell(6).GetString();
        //                    usuario.Activo = row.Cell(7).GetValue<bool>();

        //                    string fecha = row.Cell(8).GetString();
        //                    usuario.FechaCreacion = Convert.ToDateTime(fecha);

        //                    usuarios.Add(usuario);
        //                }
        //            }
        //        }
        //        // Guardar los productos en la base de datos
        //        _usuarioService.AgregarUsuarios(usuarios);

        //        var lstUsuario = await _usuarioService.ObtenerUsuarios(string.Empty, 1, 5);
        //        response.DataList = lstUsuario;
        //        response.Message = "Usuarios encontrados";

        //        return PartialView("UsuariosPartialView", response);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
