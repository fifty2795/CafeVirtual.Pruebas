using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Utilidades.Model
{
    public class ErrorViewModel
    {
        public string Message { get; set; }

        // StackTrace para detalles del error
        public string? StackTrace { get; set; }

        // Origen del error
        public string? Source { get; set; }

        // Código de error 
        public int? ErrorCode { get; set; }

        //public ErrorViewModel(string message, string? stackTrace = null, string? source = null, int? errorCode = null)
        public ErrorViewModel(string message, int? errorCode = null)
        {
            Message = message;
            //StackTrace = stackTrace;
            //Source = source;
            ErrorCode = errorCode;
        }
    }
}
