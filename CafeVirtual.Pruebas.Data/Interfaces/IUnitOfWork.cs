using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Repositorio;

namespace CafeVirtual.Pruebas.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : class;

        IRoleMenus RepositorioRoleMenus { get; }

        ICliente RepositorioCliente { get; }

        IProducto RepositorioProducto { get; }

        IUsuario RepositorioUsuario { get; }

        IRol RepositorioRol { get; }

        Task<int> SaveChangesAsync();
    }
}
