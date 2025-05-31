using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.DTO
{
    public class UsuarioViewModel
    {
        public string IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string RutaImagen { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public UsuarioViewModel()
        {
            // Asignar avatar por defecto si no se proporciona uno
            RutaImagen = "/images/avatar-default.png";
            Status = "offline";
        }
    }
}
