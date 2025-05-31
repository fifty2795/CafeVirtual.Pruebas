using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.DTO
{
    public class ClienteDTO
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Rfc { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }        
    }

    public class ClienteInsertDto
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Rfc { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; } = true;        
    }

    public class ClienteUpdateDto
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Rfc { get; set; }
        public string Email { get; set; }
        public bool Activo { get; set; }        
    }

}
