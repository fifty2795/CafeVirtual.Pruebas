using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblMenu
{
    public int IdMenu { get; set; }

    public string Nombre { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Icono { get; set; } = null!;

    public int Orden { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<TblRoleMenu> TblRoleMenus { get; set; } = new List<TblRoleMenu>();

    public virtual ICollection<TblSubMenu> TblSubMenus { get; set; } = new List<TblSubMenu>();
}
