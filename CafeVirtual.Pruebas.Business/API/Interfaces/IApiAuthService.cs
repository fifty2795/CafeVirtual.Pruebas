using CafeVirtual.Pruebas.Business.API.Models;
using CafeVirtual.Pruebas.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.API.Interfaces
{
    public interface IApiAuthService
    {
        Task<LoginResponse?> ObtenerTokenAsync(string email, string password);

        Task<LoginDTO?> ObtenerUsuarioConTokenAsync(string token);
    }
}
