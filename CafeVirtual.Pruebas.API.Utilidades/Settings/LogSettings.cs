using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.API.Utilidades.Settings
{
    public class LogSettings
    {
        public string LogFilePath { get; set; } = string.Empty;

        public string LogFileName { get; set; } = string.Empty;

        public int TamanioMaximo { get; set; }

        public int MaximoArchivosLog { get; set; }
    }
}
