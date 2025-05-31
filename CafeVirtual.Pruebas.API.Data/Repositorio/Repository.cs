using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CafeVirtual.Pruebas.API.Data.Interfaces;
using System.Linq.Expressions;

namespace CafeVirtual.Pruebas.API.Data.Repositorio
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly MvcContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(MvcContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IQueryable<T> ObtenerQueryable()
        {
            return _dbSet.AsQueryable();
        }

        //public IQueryable<T> ObtenerQueryable(Expression<Func<T, bool>>? filtro = null, params Expression<Func<T, object>>[] includes)
        //{
        //    IQueryable<T> query = _dbSet.AsQueryable();

        //    if (filtro != null)
        //    {
        //        query = query.Where(filtro);
        //    }

        //    foreach (var include in includes)
        //    {
        //        query = query.Include(include);
        //    }

        //    return query;
        //}

        public async Task<T?> ObtenerByIdAsync(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<T> AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);

            return entidad;
        }

        public T Actualizar(T entidad)
        {
            _dbSet.Update(entidad);

            return entidad;
        }

        public async Task<T> EliminarAsync(int id)
        {
            var entidad = await _dbSet.FindAsync(id);

            _dbSet.Remove(entidad);

            return entidad;
        }
    }
}
