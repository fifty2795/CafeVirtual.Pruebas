using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.API.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public int Code { get; set; }

        public LoginData Data { get; set; }
    }

    public class LoginData
    {
        public int IdUsuario { get; set; }
        
        public int IdRol { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string RutaImagen { get; set; } = string.Empty;

        public string RoleNombre { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
