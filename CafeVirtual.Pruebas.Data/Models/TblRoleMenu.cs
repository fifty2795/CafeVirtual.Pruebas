using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblRoleMenu
{
    public int IdRoleMenus { get; set; }

    public int IdRole { get; set; }

    public int IdMenu { get; set; }

    public bool Activo { get; set; }

    public virtual TblMenu IdMenuNavigation { get; set; } = null!;

    public virtual TblRol IdRoleNavigation { get; set; } = null!;
}
