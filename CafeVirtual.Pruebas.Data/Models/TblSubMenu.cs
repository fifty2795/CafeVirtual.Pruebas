using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblSubMenu
{
    public int IdSubMenu { get; set; }

    public int IdMenu { get; set; }

    public string Nombre { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Icono { get; set; } = null!;

    public int Orden { get; set; }

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual TblMenu IdMenuNavigation { get; set; } = null!;

    public virtual ICollection<TblRolesSubMenu> TblRolesSubMenus { get; set; } = new List<TblRolesSubMenu>();
}
