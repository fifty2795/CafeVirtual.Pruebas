using CafeVirtual.Pruebas.Utilidades.Model;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.Interfaces
{
    public interface IMenuService
    {
        Task<ResponseViewModel<List<TblMenu>>> ObtenerMenus();

        Task<List<TblMenu>> ObtenerMenusPorRolAsync(int idRol);
    }
}
