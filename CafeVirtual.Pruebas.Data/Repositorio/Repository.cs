using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Utilidades.Utilidades;
using CafeVirtual.Pruebas.Data.Interfaces;
using CafeVirtual.Pruebas.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Data.Repositorio
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

        public async Task<T> ObtenerByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);

            return entidad;
        }

        //public T Actualizar(T entidad)
        //{
        //    _dbSet.Update(entidad);

        //    return entidad;
        //}

        public T Actualizar(T entidad, params string[] propiedadesIgnoradas)
        {
            var entry = _dbSet.Attach(entidad);
            entry.State = EntityState.Modified;

            foreach (var propiedad in propiedadesIgnoradas)
            {
                entry.Property(propiedad).IsModified = false;
            }

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
