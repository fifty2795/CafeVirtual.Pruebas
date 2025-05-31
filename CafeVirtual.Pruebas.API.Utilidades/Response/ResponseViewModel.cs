using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Utilidades.Response
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }

        public int Code { get; set; }

        //public ErrorViewModel? Error { get; set; }
    }

    public class ResponseViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ErrorViewModel? Error { get; set; }
    }
}
