using Microsoft.EntityFrameworkCore;
using sve.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace sve_api.Models
{
    public class SveContext : DbContext
    {
        public SveContext(DbContextOptions<SveContext> options) : base(options)
        {
        }

        public DbSet<Entrada> Entrada { get; set; }
        public DbSet<Orden> Orden { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<Tarifa> Tarifa { get; set; }
        public DbSet<Funcion> Funcion { get; set; }
        public DbSet<Local> Local { get; set; }
        public DbSet<Sector> Sector { get; set; }
        public DbSet<Usuario> Usuario { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entrada>(entity =>
            {
                entity.HasKey(e => e.IdEntrada);

                entity.Property(e => e.IdEntrada)
                   .ValueGeneratedOnAdd();

                entity.HasOne<Orden>()
                      .WithMany(e => e.Entradas)
                      .HasForeignKey(e => e.IdOrden);

                entity.HasOne<Tarifa>()
                      .WithMany(e => e.Entradas)
                      .HasForeignKey(e => e.IdTarifa);


            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.Property(e => e.IdCliente)
                   .ValueGeneratedOnAdd();

                entity.HasOne(e => e.Usuario)
                      .WithOne(e => e.Cliente)
                      .HasForeignKey<Cliente>(e => e.IdUsuario)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Orden>(entity =>
            {
                entity.HasKey(e => e.IdOrden);

                entity.Property(e => e.IdOrden)
                   .ValueGeneratedOnAdd();

                entity.HasOne<Cliente>()
                      .WithMany(e => e.Ordenes)
                      .HasForeignKey(e => e.IdCliente);

                entity.HasOne<Tarifa>()
                      .WithMany(e => e.Ordenes)
                      .HasForeignKey(e => e.IdTarifa);
            });

            modelBuilder.Entity<Evento>(entity =>
            {
                entity.HasKey(e => e.IdEvento);
                entity.Property(e => e.IdEvento)
                   .ValueGeneratedOnAdd();

            });

            modelBuilder.Entity<Tarifa>(entity =>
            {
                entity.HasKey(e => e.IdTarifa);
                entity.Property(e => e.IdTarifa)
                   .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Funcion>(entity =>
            {
                entity.HasKey(e => e.IdFuncion);
                entity.Property(e => e.IdFuncion)
                   .ValueGeneratedOnAdd();
                entity.HasOne<Evento>()
                      .WithMany(e => e.Funciones)
                      .HasForeignKey(e => e.IdEvento);
            });
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);
                entity.Property(e => e.IdUsuario)
                   .ValueGeneratedOnAdd();
            });

           modelBuilder.Entity<Local>(entity =>
            {
                entity.HasKey(e => e.IdLocal);
                entity.Property(e => e.IdLocal)
                   .ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Sector>(entity =>
            {
                entity.HasKey(e => e.IdSector);
                entity.Property(e => e.IdSector)
                   .ValueGeneratedOnAdd();
                entity.HasOne<Local>()
                      .WithMany(e => e.Sectores)
                      .HasForeignKey(e => e.IdLocal);
            });
             

        }
    }
}
