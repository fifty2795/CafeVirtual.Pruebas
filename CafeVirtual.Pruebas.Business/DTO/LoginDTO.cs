using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.DTO
{
    public class LoginDTO
    {
        public int IdUsuario { get; set; }

        public string NombreCompleto { get; set; }

        public string Email { get; set; }

        public string RutaImagen { get; set; }

        public int IdRol { get; set; }

        public string RolNombre { get; set; }
    }
}
