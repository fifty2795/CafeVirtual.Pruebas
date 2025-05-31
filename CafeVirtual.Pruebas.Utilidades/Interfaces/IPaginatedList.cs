using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Utilidades.Interfaces
{
    public interface IPaginatedList
    {
        int PageIndex { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }
}
