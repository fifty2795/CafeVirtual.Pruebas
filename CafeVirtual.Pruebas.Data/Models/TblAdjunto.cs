using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblAdjunto
{
    public int IdAdjunto { get; set; }

    public int IdMensaje { get; set; }

    public string NombreArchivo { get; set; } = null!;

    public string TipoArchivo { get; set; } = null!;

    public string? Extension { get; set; }

    public byte[] Contenido { get; set; } = null!;

    public DateTime FechaCarga { get; set; }

    public virtual TblMensaje IdMensajeNavigation { get; set; } = null!;
}
