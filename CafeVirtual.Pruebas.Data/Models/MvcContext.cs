using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CafeVirtual.Pruebas.Data.Models;

public partial class MvcContext : DbContext
{
    public MvcContext()
    {
    }

    public MvcContext(DbContextOptions<MvcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAdjunto> TblAdjuntos { get; set; }

    public virtual DbSet<TblCliente> TblClientes { get; set; }

    public virtual DbSet<TblDetalleVentum> TblDetalleVenta { get; set; }

    public virtual DbSet<TblMensaje> TblMensajes { get; set; }

    public virtual DbSet<TblMenu> TblMenus { get; set; }

    public virtual DbSet<TblProducto> TblProductos { get; set; }

    public virtual DbSet<TblProveedor> TblProveedors { get; set; }

    public virtual DbSet<TblRol> TblRols { get; set; }

    public virtual DbSet<TblRoleMenu> TblRoleMenus { get; set; }

    public virtual DbSet<TblRolesSubMenu> TblRolesSubMenus { get; set; }

    public virtual DbSet<TblSubMenu> TblSubMenus { get; set; }

    public virtual DbSet<TblUsuario> TblUsuarios { get; set; }

    public virtual DbSet<TblVentum> TblVenta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ALEJANDRO\\SQLEXPRESS;Database=MVC;Trusted_Connection=True; TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAdjunto>(entity =>
        {
            entity.HasKey(e => e.IdAdjunto).HasName("PK__tbl_Adju__5BEDE7C82299882D");

            entity.ToTable("tbl_Adjunto");

            entity.HasIndex(e => e.IdMensaje, "IX_tbl_Adjunto_MensajeId");

            entity.Property(e => e.IdAdjunto).HasColumnName("Id_Adjunto");
            entity.Property(e => e.Extension).HasMaxLength(10);
            entity.Property(e => e.FechaCarga)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdMensaje).HasColumnName("Id_Mensaje");
            entity.Property(e => e.NombreArchivo).HasMaxLength(255);
            entity.Property(e => e.TipoArchivo).HasMaxLength(100);

            entity.HasOne(d => d.IdMensajeNavigation).WithMany(p => p.TblAdjuntos)
                .HasForeignKey(d => d.IdMensaje)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tbl_Adjun__Id_Me__2DE6D218");
        });

        modelBuilder.Entity<TblCliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("tbl_Cliente");

            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.Descripcion).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.Nombre).HasMaxLength(50);
            entity.Property(e => e.Rfc)
                .HasMaxLength(20)
                .HasColumnName("RFC");
        });

        modelBuilder.Entity<TblDetalleVentum>(entity =>
        {
            entity.HasKey(e => e.IdVentaDetalle);

            entity.ToTable("tbl_DetalleVenta", tb => tb.HasTrigger("trg_UpdateStock_AfterInsert"));

            entity.Property(e => e.IdVentaDetalle).HasColumnName("Id_VentaDetalle");
            entity.Property(e => e.IdProducto).HasColumnName("Id_Producto");
            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.Iva)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("IVA");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.TblDetalleVenta)
                .HasForeignKey(d => d.IdProducto)
                .HasConstraintName("FK_tbl_DetalleVenta_tbl_Producto");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.TblDetalleVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_tbl_DetalleVenta_tbl_Venta");
        });

        modelBuilder.Entity<TblMensaje>(entity =>
        {
            entity.HasKey(e => e.IdMensaje).HasName("PK__tbl_Mens__261B6934886F1B7A");

            entity.ToTable("tbl_Mensaje");

            entity.HasIndex(e => new { e.RemitenteId, e.DestinatarioId, e.FechaEnvio }, "IX_Tbl_Mensaje_ChatHistory");

            entity.Property(e => e.IdMensaje).HasColumnName("Id_Mensaje");
            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Destinatario).WithMany(p => p.TblMensajeDestinatarios)
                .HasForeignKey(d => d.DestinatarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Mensaje_Destinatario");

            entity.HasOne(d => d.Remitente).WithMany(p => p.TblMensajeRemitentes)
                .HasForeignKey(d => d.RemitenteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Mensaje_Remitente");
        });

        modelBuilder.Entity<TblMenu>(entity =>
        {
            entity.HasKey(e => e.IdMenu);

            entity.ToTable("tbl_Menus");

            entity.Property(e => e.IdMenu).HasColumnName("Id_Menu");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Icono).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Url).HasMaxLength(200);
        });

        modelBuilder.Entity<TblProducto>(entity =>
        {
            entity.HasKey(e => e.IdProducto);

            entity.ToTable("tbl_Producto");

            entity.Property(e => e.IdProducto).HasColumnName("id_Producto");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Detalle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechaCreacion");
            entity.Property(e => e.IdProveedor).HasColumnName("id_Proveedor");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.TblProductos)
                .HasForeignKey(d => d.IdProveedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Producto_tbl_Proveedor");
        });

        modelBuilder.Entity<TblProveedor>(entity =>
        {
            entity.HasKey(e => e.IdProveedor);

            entity.ToTable("tbl_Proveedor");

            entity.Property(e => e.IdProveedor).HasColumnName("id_proveedor");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TblRol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.ToTable("tbl_Rol");

            entity.Property(e => e.IdRol).HasColumnName("Id_Rol");
            entity.Property(e => e.Descripcion).HasMaxLength(100);
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("Fecha_Creacion");
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRoleMenu>(entity =>
        {
            entity.HasKey(e => e.IdRoleMenus);

            entity.ToTable("tbl_RoleMenus");

            entity.Property(e => e.IdRoleMenus).HasColumnName("Id_RoleMenus");
            entity.Property(e => e.IdMenu).HasColumnName("Id_Menu");
            entity.Property(e => e.IdRole).HasColumnName("Id_Role");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.TblRoleMenus)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_RoleMenus_tbl_Menus");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.TblRoleMenus)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_RoleMenus_tbl_Rol");
        });

        modelBuilder.Entity<TblRolesSubMenu>(entity =>
        {
            entity.HasKey(e => e.IdRoleSubMenus);

            entity.ToTable("tbl_RolesSubMenus");

            entity.Property(e => e.IdRoleSubMenus).HasColumnName("Id_RoleSubMenus");
            entity.Property(e => e.IdRole).HasColumnName("Id_Role");
            entity.Property(e => e.IdSubMenu).HasColumnName("Id_SubMenu");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.TblRolesSubMenus)
                .HasForeignKey(d => d.IdRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_RolesSubMenus_tbl_Rol");

            entity.HasOne(d => d.IdSubMenuNavigation).WithMany(p => p.TblRolesSubMenus)
                .HasForeignKey(d => d.IdSubMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_RolesSubMenus_tbl_SubMenus");
        });

        modelBuilder.Entity<TblSubMenu>(entity =>
        {
            entity.HasKey(e => e.IdSubMenu);

            entity.ToTable("tbl_SubMenus");

            entity.Property(e => e.IdSubMenu).HasColumnName("Id_SubMenu");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Icono).HasMaxLength(50);
            entity.Property(e => e.IdMenu).HasColumnName("Id_Menu");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Url).HasMaxLength(200);

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.TblSubMenus)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_SubMenus_tbl_Menus");
        });

        modelBuilder.Entity<TblUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK_tbl_usuario");

            entity.ToTable("tbl_Usuario");

            entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .HasColumnName("apellido_materno");
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .HasColumnName("apellido_paterno");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FechaCreacion)
                .HasColumnType("datetime")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .HasColumnName("password");
            entity.Property(e => e.RutaImagen)
                .HasMaxLength(500)
                .HasColumnName("rutaImagen");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("offline");
            entity.Property(e => e.UltimaConexion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.TblUsuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Usuario_tbl_Rol");
        });

        modelBuilder.Entity<TblVentum>(entity =>
        {
            entity.HasKey(e => e.IdVenta);

            entity.ToTable("tbl_Venta");

            entity.Property(e => e.IdVenta).HasColumnName("Id_Venta");
            entity.Property(e => e.FechaVenta).HasColumnType("datetime");
            entity.Property(e => e.IdCliente).HasColumnName("Id_Cliente");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.TblVenta)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_Venta_tbl_Cliente");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
