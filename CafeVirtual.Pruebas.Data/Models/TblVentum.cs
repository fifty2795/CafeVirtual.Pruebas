using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblVentum
{
    public int IdVenta { get; set; }

    public int IdCliente { get; set; }

    public decimal Total { get; set; }

    public DateTime FechaVenta { get; set; }

    public virtual TblCliente IdClienteNavigation { get; set; } = null!;

    public virtual ICollection<TblDetalleVentum> TblDetalleVenta { get; set; } = new List<TblDetalleVentum>();
}
