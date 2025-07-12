using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProyectoNFTs.Infraestructure.Models;

namespace ProyectoNFTs.Infraestructure.Data;

public partial class ProyectoNFTsContext : DbContext
{
    public ProyectoNFTsContext(DbContextOptions<ProyectoNFTsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Billetera> Billetera { get; set; }

    public virtual DbSet<Cliente> Cliente { get; set; }

    public virtual DbSet<FacturaDetalle> FacturaDetalle { get; set; }

    public virtual DbSet<FacturaEncabezado> FacturaEncabezado { get; set; }

    public virtual DbSet<Nft> Nft { get; set; }

    public virtual DbSet<Pais> Pais { get; set; }

    public virtual DbSet<PropietarioNft> PropietarioNft { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Tarjeta> Tarjeta { get; set; }

    public virtual DbSet<TempPropietarioNft> TempPropietarioNft { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Billetera>(entity =>
        {
            entity.HasKey(e => e.IdNft).HasName("PK_Billetera_1");

            entity.Property(e => e.IdNft).ValueGeneratedNever();

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Billetera)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Billetera_Cliente");

            entity.HasOne(d => d.IdNftNavigation).WithOne(p => p.Billetera)
                .HasForeignKey<Billetera>(d => d.IdNft)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Billetera_NFT");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.Property(e => e.IdCliente).ValueGeneratedNever();
            entity.Property(e => e.Apellido1)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Apellido2)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaNacimiento).HasColumnType("datetime");
            entity.Property(e => e.Nombre)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Sexo)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Cliente)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_Pais");
        });

        modelBuilder.Entity<FacturaDetalle>(entity =>
        {
            entity.HasKey(e => new { e.IdFactura, e.Secuencia });

            entity.ToTable(tb => tb.HasTrigger("tr_InsertPropietario"));

            entity.Property(e => e.IdNft).HasColumnName("IdNFT");
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");

            entity.HasOne(d => d.IdFacturaNavigation).WithMany(p => p.FacturaDetalle)
                .HasForeignKey(d => d.IdFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalle_FacturaEncabezado");

            entity.HasOne(d => d.IdNftNavigation).WithMany(p => p.FacturaDetalle)
                .HasForeignKey(d => d.IdNft)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaDetalle_NFT");
        });

        modelBuilder.Entity<FacturaEncabezado>(entity =>
        {
            entity.HasKey(e => e.IdFactura);

            entity.Property(e => e.IdFactura).ValueGeneratedNever();
            entity.Property(e => e.FechaFacturacion).HasColumnType("datetime");
            entity.Property(e => e.TarjetaNumero)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.FacturaEncabezado)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaEncabezado_Cliente");

            entity.HasOne(d => d.IdTarjetaNavigation).WithMany(p => p.FacturaEncabezado)
                .HasForeignKey(d => d.IdTarjeta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FacturaEncabezado_Tarjeta");
        });

        modelBuilder.Entity<Nft>(entity =>
        {
            entity.HasKey(e => e.IdNft).HasName("PK_NFT");

            entity.Property(e => e.IdNft).ValueGeneratedNever();
            entity.Property(e => e.Autor)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("numeric(18, 2)");
        });

        modelBuilder.Entity<Pais>(entity =>
        {
            entity.HasKey(e => e.IdPais);

            entity.Property(e => e.IdPais).ValueGeneratedNever();
            entity.Property(e => e.Alfa2)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.Alfa3)
                .HasMaxLength(3)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PropietarioNft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Propieta__3213E83F80722E82");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FechaPropiedad).HasColumnType("datetime");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.PropietarioNft)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK_PropietarioNft_Cliente");

            entity.HasOne(d => d.IdNftNavigation).WithMany(p => p.PropietarioNft)
                .HasForeignKey(d => d.IdNft)
                .HasConstraintName("FK_PropietarioNft_Nft");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol);

            entity.Property(e => e.IdRol).ValueGeneratedNever();
            entity.Property(e => e.DescripcionRol)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tarjeta>(entity =>
        {
            entity.HasKey(e => e.IdTarjeta);

            entity.Property(e => e.IdTarjeta).ValueGeneratedNever();
            entity.Property(e => e.Descripcion)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TempPropietarioNft>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.FechaPropiedad).HasColumnType("datetime");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Login);

            entity.Property(e => e.Login)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Apellidos)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK_Usuario_Rol");
        });
        modelBuilder.HasSequence("ReceiptNumber");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
