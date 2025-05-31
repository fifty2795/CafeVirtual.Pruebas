using CafeVirtual.Pruebas.Utilidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Utilidades.Model
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }        

        public ErrorViewModel? Error { get; set; }
    }

    public class ResponseViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ErrorViewModel? Error { get; set; }
    }
}
