using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using CafeVirtual.Pruebas.Business.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using CafeVirtual.Pruebas.Business.Servicios;
using CafeVirtual.Pruebas.Cliente.Models;
using AutoMapper;
using CafeVirtual.Pruebas.Business.API.Interfaces;

namespace Pruebas.Cliente.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IUsuarioService _usuarioService;
        private readonly IMenuService _menuService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IApiAuthService _apiAuthService;
        private readonly IMapper _mapper;

        public LoginController(ILoginService loginService, IUsuarioService usuarioService, IMenuService menuService, IWebHostEnvironment webHostEnvironment, IApiAuthService apiAuthService, IMapper mapper)
        {
            _loginService = loginService;
            _usuarioService = usuarioService;
            _menuService = menuService;
            _webHostEnvironment = webHostEnvironment;
            _apiAuthService = apiAuthService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(string email, string password)
        //{
        //    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
        //    {
        //        var response = await _loginService.ObtenerUsuario(email, password);

        //        if (response.Success && response.Data != null)
        //        {
        //            var usuario = response.Data;

        //            var claims = new List<Claim>
        //            {
        //                new Claim(ClaimTypes.Name, usuario.Nombre + ' ' + usuario.ApellidoPaterno), //+ ' ' + usuario.ApellidoMaterno),
        //                new Claim(ClaimTypes.Email, usuario.Email),
        //                new Claim(ClaimTypes.Role, usuario.IdRolNavigation.Nombre),
        //                new Claim("IdUsuario", usuario.IdUsuario.ToString()),
        //                new Claim("FotoUrl", string.IsNullOrWhiteSpace(response.Data.RutaImagen) ? "/images/avatar-default.png" : response.Data.RutaImagen)
        //            };

        //            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        //            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

        //            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        //        }
        //    }

        //    return Json(new { success = false, message = "Usuario o contraseña incorrectos." });
        //}

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var usuario = await _apiAuthService.ObtenerTokenAsync(email, password);

                if (usuario != null)
                {
                    var menus = await _menuService.ObtenerMenusPorRolAsync(usuario.Data.IdRol);
                    var menusData = _mapper.Map<List<MenuConfigViewModel>>(menus);
                    var menusJson = JsonConvert.SerializeObject(menusData);

                    var claims = new List<Claim>
                        {
                            // Identificador único para SignalR y autenticación
                            new Claim(ClaimTypes.NameIdentifier, usuario.Data.IdUsuario.ToString()),

                            // Información básica del usuario
                            new Claim(ClaimTypes.Name, usuario.Data.NombreCompleto),
                            new Claim(ClaimTypes.Email, usuario.Data.Email),
                            new Claim(ClaimTypes.Role, usuario.Data.RoleNombre),

                             // Claims adicionales
                            new Claim("IdUsuario", usuario.Data.IdUsuario.ToString()),
                            new Claim("IdRol", usuario.Data.IdRol.ToString()),
                            new Claim("FotoUrl", string.IsNullOrWhiteSpace(usuario.Data.RutaImagen) ? "/images/avatar-default.png" : usuario.Data.RutaImagen),
                            
                            // Buscar otra alternativa en donde guardar el token
                            new Claim("AccessToken", usuario.Data.Token),

                            // Menú en JSON para usar en el frontend
                            new Claim("Menus", menusJson)
                        };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }
            }
            return Json(new { success = false, message = "Usuario o contraseña incorrectos." });
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro([FromForm] TblUsuario usuario, IFormFile imagenPerfil)
        {
            usuario.IdRol = 1;
            usuario.Activo = true;
            usuario.FechaCreacion = DateTime.Now;

            // Guardar imagen 
            if (imagenPerfil != null && imagenPerfil.Length > 0)
            {
                var ext = Path.GetExtension(imagenPerfil.FileName);
                var nombreArchivo = $"avatar-{Guid.NewGuid()}{ext}";
                var rutaCarpeta = Path.Combine(_webHostEnvironment.WebRootPath, "images", "avatares");

                if (!Directory.Exists(rutaCarpeta))
                    Directory.CreateDirectory(rutaCarpeta);

                var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);
                using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                {
                    await imagenPerfil.CopyToAsync(stream);
                }

                usuario.RutaImagen = $"/images/avatares/{nombreArchivo}";
            }

            var response = await _usuarioService.AgregarUsuario(usuario);

            if (response.Success)
            {
                return Json(new { success = true, message = "¡Registro exitoso! Tu cuenta ha sido creada correctamente. Ahora puedes iniciar sesión." });
            }

            return Json(new { success = false, message = "Error al registrarse, por favor intente de nuevo." });
        }

        // Metodo para cerrar sesion
        public async Task<IActionResult> Logout()
        {
            int idUsuario = Convert.ToInt32(User.FindFirst("IdUsuario")?.Value);
            var IsAuthenticated = User.Identity.IsAuthenticated;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);            

            if (IsAuthenticated)
            {
                // Elimina todas las cookies posibles
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                // Actualiza el estado de sesion para el Chat
                var response = await _usuarioService.ActualizarStatus(idUsuario, "offline");
            }

            return RedirectToAction("Index", "Login");
        }

        // Acceder a cualquier Claim
        //var nombre = User.Identity.Name; // ClaimTypes.Name
        //var email = User.FindFirst(ClaimTypes.Email)?.Value;
        //var idUsuario = User.FindFirst("IdUsuario")?.Value;
    }
}
