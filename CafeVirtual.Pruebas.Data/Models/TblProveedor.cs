using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblProveedor
{
    public int IdProveedor { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<TblProducto> TblProductos { get; set; } = new List<TblProducto>();
}
