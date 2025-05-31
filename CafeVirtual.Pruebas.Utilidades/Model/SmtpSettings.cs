using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Utilidades.Model
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string FromEmail { get; set; }
        public string AppPassword { get; set; }
    }
}
