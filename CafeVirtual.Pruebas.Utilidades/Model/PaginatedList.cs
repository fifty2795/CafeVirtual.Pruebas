using Microsoft.EntityFrameworkCore;
using CafeVirtual.Pruebas.Utilidades.Interfaces;
using System.Linq.Expressions;

namespace CafeVirtual.Pruebas.Utilidades.Model
{
    //public class PaginatedList<T> : List<T>, IPaginatedList
    public class PaginatedList<T> : IPaginatedList
    {
        public List<T> Items { get; set; } = new List<T>(); // Propiedad Serializable
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public int TotalCount { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = items;
            //AddRange(items);
        }

        public PaginatedList()
        {
        }

        public bool HasPreviousPage
        {
            get
            {
                return PageIndex > 1;
            }
        }

        public bool HasNextPage
        {
            get
            {
                return PageIndex < TotalPages;
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count;
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }
}
