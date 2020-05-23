using Microsoft.EntityFrameworkCore;

namespace odatawebapi.Entidades
{
    public class PersonasDbContext : DbContext
    {
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<Direccion> Direccion { get; set; }
        public virtual DbSet<Telefono> Telefono { get; set; }

        public PersonasDbContext() { }

        public PersonasDbContext(DbContextOptions<PersonasDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LUCIANOP\\SQLSERVER2019;Database=OdataWebApi;User=odata;Password=odata;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Persona>(entity =>
            {
                entity.ToTable("Personas");
            });
            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.ToTable("Telefonos");
            });
            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.ToTable("Direcciones");
            });

            modelBuilder.Entity<Direccion>(entity =>
            {
                entity.HasOne(d => d.IdPersonaNavigation)
                      .WithMany(p => p.Direccion)
                      .HasForeignKey(d => d.IdPersona)
                      .HasConstraintName("FK_Direccion_IdPersona");
            });

            modelBuilder.Entity<Telefono>(entity =>
            {
                entity.HasOne(d => d.IdPersonaNavigation)
                      .WithMany(p => p.Telefono)
                      .HasForeignKey(d => d.IdPersona)
                      .HasConstraintName("FK_Telefono_IdPersona");
            });
        }
    }
}
