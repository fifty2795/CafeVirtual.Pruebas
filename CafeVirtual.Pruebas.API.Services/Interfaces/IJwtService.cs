using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(TblUsuario usuario);

        ClaimsPrincipal? ValidateToken(string token);
    }
}
