using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.DTO
{
    public class UsuarioDTO
    {
        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        public string NombreCompleto { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        
        public string RutaImagen { get; set; } = string.Empty;

        public string RoleNombre { get; set; } = string.Empty;

        public string Token { get; set; }
    }
}
