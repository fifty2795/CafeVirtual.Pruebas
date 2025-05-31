using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblUsuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string? Nombre { get; set; }

    public string? ApellidoPaterno { get; set; }

    public string? ApellidoMaterno { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? RutaImagen { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string? Token { get; set; }

    public string Status { get; set; } = null!;

    public DateTime UltimaConexion { get; set; }

    public virtual TblRol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<TblMensaje> TblMensajeDestinatarios { get; set; } = new List<TblMensaje>();

    public virtual ICollection<TblMensaje> TblMensajeRemitentes { get; set; } = new List<TblMensaje>();
}
