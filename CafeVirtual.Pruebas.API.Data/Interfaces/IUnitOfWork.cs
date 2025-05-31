using CafeVirtual.Pruebas.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;        

        IUsuario RepositorioUsuario { get; }

        IProducto RepositorioProducto { get; }         

        Task<int> SaveChangesAsync();
    }
}
