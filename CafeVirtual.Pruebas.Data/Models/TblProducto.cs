using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblProducto
{
    public int IdProducto { get; set; }

    public int IdProveedor { get; set; }

    public string? Nombre { get; set; }

    public string? Detalle { get; set; }

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public bool Activo { get; set; }

    public virtual TblProveedor IdProveedorNavigation { get; set; } = null!;

    public virtual ICollection<TblDetalleVentum> TblDetalleVenta { get; set; } = new List<TblDetalleVentum>();
}
