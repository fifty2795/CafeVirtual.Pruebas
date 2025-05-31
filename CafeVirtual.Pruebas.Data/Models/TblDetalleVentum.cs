using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblDetalleVentum
{
    public int IdVentaDetalle { get; set; }

    public int IdVenta { get; set; }

    public int IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal SubTotal { get; set; }

    public decimal Iva { get; set; }

    public decimal Total { get; set; }

    public virtual TblProducto? IdProductoNavigation { get; set; }

    public virtual TblVentum? IdVentaNavigation { get; set; }
}
