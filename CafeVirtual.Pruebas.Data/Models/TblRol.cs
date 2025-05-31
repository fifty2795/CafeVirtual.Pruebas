using System;
using System.Collections.Generic;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class TblRol
{
    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public bool Activo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<TblRoleMenu> TblRoleMenus { get; set; } = new List<TblRoleMenu>();

    public virtual ICollection<TblRolesSubMenu> TblRolesSubMenus { get; set; } = new List<TblRolesSubMenu>();

    public virtual ICollection<TblUsuario> TblUsuarios { get; set; } = new List<TblUsuario>();
}
