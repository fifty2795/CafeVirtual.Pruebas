using CafeVirtual.Pruebas.Data.Models;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using CafeVirtual.Pruebas.API.Data.Repositorio;
using CafeVirtual.Pruebas.API.Utilidades.Interfaces;

namespace CafeVirtual.Pruebas.API.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MvcContext _context;
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly ILogService _logService;        

        public IUsuario RepositorioUsuario { get; private set; }        
        public IProducto RepositorioProducto { get; private set; }                

        public UnitOfWork(MvcContext context, ILogService logService)
        {
            _context = context;
            _logService = logService;
            
            RepositorioUsuario = new Repositorio_Usuario(_context, _logService);
            RepositorioProducto = new Repositorio_Producto(_context, _logService);            
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
