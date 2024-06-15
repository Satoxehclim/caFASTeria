using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace caFASTeria.Models;

public partial class CaFasteriaContext : DbContext
{
    public CaFasteriaContext()
    {
    }

    public CaFasteriaContext(DbContextOptions<CaFasteriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cuentum> Cuenta { get; set; }

    public virtual DbSet<Foto> Fotos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=localhost; database=caFASTeria; integrated security=true; TrustServerCertificate=Yes");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cuentum>(entity =>
        {
            entity.HasKey(e => e.IdCuenta).HasName("PK__cuenta__BBC6DF3252E02332");

            entity.ToTable("cuenta");

            entity.Property(e => e.IdCuenta).HasColumnName("idCuenta");
            entity.Property(e => e.Contrasena)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("contrasena");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Telefono)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("telefono");
            entity.Property(e => e.Usuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("usuario");
        });

        modelBuilder.Entity<Foto>(entity =>
        {
            entity.HasKey(e => e.Idfoto).HasName("PK__foto__AE55DE62662A7474");

            entity.ToTable("foto");

            entity.Property(e => e.Idfoto).HasColumnName("idfoto");
            entity.Property(e => e.Direcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("direcion");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__pedido__A9F619B7DA384728");

            entity.ToTable("pedido");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Comprador).HasColumnName("comprador");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Producto).HasColumnName("producto");

            entity.HasOne(d => d.CompradorNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Comprador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__pedido__comprado__59FA5E80");

            entity.HasOne(d => d.ProductoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.Producto)
                .HasConstraintName("FK__pedido__producto__59063A47");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__producto__07F4A1323F09BD89");

            entity.ToTable("producto");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Foto).HasColumnName("foto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");
            entity.Property(e => e.Vendedor).HasColumnName("vendedor");

            entity.HasOne(d => d.FotoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Foto)
                .HasConstraintName("FK__producto__foto__4E88ABD4");

            entity.HasOne(d => d.VendedorNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Vendedor)
                .HasConstraintName("FK__producto__vended__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
