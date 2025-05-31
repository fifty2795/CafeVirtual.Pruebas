using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Data.Interfaces
{
    public interface IUsuario
    {
        Task<TblUsuario?> ObtenerUsuarioByCredenciales(string email, string password);
    }
}
