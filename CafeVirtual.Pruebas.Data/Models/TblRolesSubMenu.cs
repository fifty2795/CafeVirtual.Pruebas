using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblRolesSubMenu
{
    public int IdRoleSubMenus { get; set; }

    public int IdRole { get; set; }

    public int IdSubMenu { get; set; }

    public bool Activo { get; set; }

    public virtual TblRol IdRoleNavigation { get; set; } = null!;

    public virtual TblSubMenu IdSubMenuNavigation { get; set; } = null!;
}
