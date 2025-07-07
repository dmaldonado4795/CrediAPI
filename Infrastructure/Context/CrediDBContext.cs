using CrediAPI.Domain.Models;
using System.Data.Entity;

namespace CrediAPI.Infrastructure.Context
{
    public class CrediDBContext : DbContext
    {
        public CrediDBContext() : base("CrediDB")
        {
            Database.SetInitializer<CrediDBContext>(null);
        }

        public DbSet<PlazosModel> Plazos { get; set; }
        public DbSet<ProductoModel> Productos { get; set; }
        public DbSet<UsuarioModel> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UsuarioModel>()
               .ToTable("Usuarios")
               .HasIndex(u => u.Username)
               .IsUnique();

            modelBuilder.Entity<ProductoModel>()
                .ToTable("Productos")
                .HasIndex(p => p.Nombre)
                .IsUnique();

            modelBuilder.Entity<PlazosModel>()
                .ToTable("Plazos")
                .HasIndex(p => p.Meses)
                .IsUnique();
        }
    }
}