using CafeVirtual.Pruebas.API.Utilidades.Response;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<ResponseViewModel<TblUsuario>> ObtenerUsuario(string email, string password);
    }
}
