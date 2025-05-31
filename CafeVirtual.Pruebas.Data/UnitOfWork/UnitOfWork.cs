using CafeVirtual.Pruebas.Utilidades.Interfaces;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.Data.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MvcContext _context;
        private readonly Dictionary<Type, object> _repositories = new();

        private readonly ILogService _logService;

        public IRoleMenus RepositorioRoleMenus { get; private set; }
        public ICliente RepositorioCliente { get; private set; }
        public IProducto RepositorioProducto { get; private set; }
        public IUsuario RepositorioUsuario { get; private set; }
        public IRol RepositorioRol { get; private set; }

        public UnitOfWork(MvcContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;

            RepositorioRoleMenus = new Repositorio_RoleMenus(_context, _logService);
            RepositorioCliente = new Repositorio_Cliente(_context, _logService);
            RepositorioProducto = new Repositorio_Producto(_context, _logService);
            RepositorioUsuario = new Repositorio_Usuario(_context, _logService);
            RepositorioRol = new Repositorio_Rol(_context, _logService);

        }

        public IRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<T>(_context);
                _repositories[type] = repositoryInstance;
            }

            return (IRepository<T>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
