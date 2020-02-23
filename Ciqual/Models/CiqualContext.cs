using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ciqual.Models
{
    public partial class CiqualContext : DbContext
    {
        public CiqualContext()
        {
        }

        public CiqualContext(DbContextOptions<CiqualContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aliment> Aliment { get; set; }
        public virtual DbSet<Composition> Composition { get; set; }
        public virtual DbSet<Constituant> Constituant { get; set; }
        public virtual DbSet<Famille> Famille { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Ciqual;Trusted_Connection=True;");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aliment>(entity =>
            {
                entity.HasKey(e => e.IdAliment)
                    .HasName("Aliment_PK");

                entity.Property(e => e.IdAliment).ValueGeneratedNever();

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.IdFamilleNavigation)
                    .WithMany(p => p.Aliment)
                    .HasForeignKey(d => d.IdFamille)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Aliment_Famille_FK");
            });

            modelBuilder.Entity<Composition>(entity =>
            {
                entity.HasKey(e => new { e.IdAliment, e.IdConstituant })
                    .HasName("Composition_PK");

                entity.Property(e => e.NoteConfiance)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdAlimentNavigation)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.IdAliment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Composition_Aliment_FK");

                entity.HasOne(d => d.IdConstituantNavigation)
                    .WithMany(p => p.Composition)
                    .HasForeignKey(d => d.IdConstituant)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Composition_Constituant_FK");
            });

            modelBuilder.Entity<Constituant>(entity =>
            {
                entity.HasKey(e => e.IdConstituant)
                    .HasName("Constituant_PK");

                entity.Property(e => e.IdConstituant).ValueGeneratedNever();

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(60);

                entity.Property(e => e.Unite)
                    .IsRequired()
                    .HasMaxLength(4)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Famille>(entity =>
            {
                entity.HasKey(e => e.IdFamille)
                    .HasName("Famille_PK");

                entity.Property(e => e.IdFamille).ValueGeneratedNever();

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
