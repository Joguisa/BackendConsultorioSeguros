using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BackendConsultorioSeguros.Models
{
    public partial class DBSEGUROSCHUBBContext : DbContext
    {
        public DBSEGUROSCHUBBContext()
        {
        }

        public DBSEGUROSCHUBBContext(DbContextOptions<DBSEGUROSCHUBBContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Asegurado> Asegurados { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Seguro> Seguros { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asegurado>(entity =>
            {
                entity.HasIndex(e => new { e.ClienteId, e.SeguroId }, "AseguradosUnique")
                    .IsUnique();

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Cliente)
                    .WithMany(p => p.Asegurados)
                    .HasForeignKey(d => d.ClienteId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asegurado__Clien__4222D4EF");

                entity.HasOne(d => d.Seguro)
                    .WithMany(p => p.Asegurados)
                    .HasForeignKey(d => d.SeguroId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Asegurado__Segur__4316F928");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.Property(e => e.Cedula)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NombreCliente)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Telefono)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Seguro>(entity =>
            {
                entity.Property(e => e.CodigoSeguro)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('A')");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NombreSeguro)
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Prima).HasColumnType("decimal(15, 2)");

                entity.Property(e => e.SumaAsegurada).HasColumnType("decimal(15, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
