using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblMensaje
{
    public int IdMensaje { get; set; }

    public int RemitenteId { get; set; }

    public int DestinatarioId { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime FechaEnvio { get; set; }

    public bool EsLeido { get; set; }

    public virtual TblUsuario Destinatario { get; set; } = null!;

    public virtual TblUsuario Remitente { get; set; } = null!;

    public virtual ICollection<TblAdjunto> TblAdjuntos { get; set; } = new List<TblAdjunto>();
}
