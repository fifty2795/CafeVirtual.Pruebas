using Microsoft.AspNetCore.Mvc.Rendering;
using CafeVirtual.Pruebas.Business.DTO;
using CafeVirtual.Pruebas.Data.Models;

namespace CafeVirtual.Pruebas.Cliente.Models
{
    public class AgregarUsuarioViewModel
    {
        public TblUsuario Usuario{ get; set; } = new();        

        public List<SelectListItem> Roles { get; set; } = new();
    }
}
