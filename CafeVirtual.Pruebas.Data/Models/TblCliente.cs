using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblCliente
{
    public int IdCliente { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Rfc { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<TblVentum> TblVenta { get; set; } = new List<TblVentum>();
}
