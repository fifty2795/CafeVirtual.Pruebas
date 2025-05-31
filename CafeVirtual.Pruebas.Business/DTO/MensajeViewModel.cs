using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeVirtual.Pruebas.Business.DTO
{
    public class MensajeViewModel
    {
        public int RemitenteId { get; set; }
        public int DestinatarioId { get; set; }
        public string Mensaje { get; set; }
        public DateTime FechaEnvio { get; set; }
        public string NombreRemitente { get; set; }
        public string FotoRemitente { get; set; } 

        public List<AdjuntoViewModel> Adjuntos { get; set; } = new();
    }

    public class AdjuntoViewModel
    {
        public int IdAdjunto { get; set; }
        public string NombreArchivo { get; set; }
        public string TipoArchivo { get; set; }
        public string? Extension { get; set; }
        public DateTime FechaCarga { get; set; }
    }

}
