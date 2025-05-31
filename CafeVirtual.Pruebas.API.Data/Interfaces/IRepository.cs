using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> ObtenerQueryable();        

        Task<T?> ObtenerByIdAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes);

        Task<T> AgregarAsync(T entidad);

        T Actualizar(T entidad);

        Task<T> EliminarAsync(int id);
    }
}
