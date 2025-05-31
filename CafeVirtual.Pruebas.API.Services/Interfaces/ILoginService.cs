using CafeVirtual.Pruebas.API.DTO;
using CafeVirtual.Pruebas.API.Services.DTO;
using CafeVirtual.Pruebas.API.Utilidades.Response;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Interfaces
{
    public interface ILoginService
    {
        Task<ResponseViewModel<UsuarioDTO>> Login(LoginDTO request);
        
    }
}
