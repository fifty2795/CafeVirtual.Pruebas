using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> ObtenerQueryable();

        Task<T> ObtenerByIdAsync(int id);

        Task<T> AgregarAsync(T entidad);

        //T Actualizar(T entidad);

        T Actualizar(T entidad, params string[] propiedadesIgnoradas);

        Task<T> EliminarAsync(int id);
    }
}
