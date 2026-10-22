using HelpDeskUnev.Models;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskUnev.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Maestro> Maestros { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Prioridad> Prioridades { get; set; }
        public DbSet<ComentarioTicket> ComentariosTickets { get; set; }
        public DbSet<HistorialEstados> HistorialEstados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // DESACTIVA AUTOMÁTICAMENTE EL BORRADO EN CASCADA EN TODAS LAS TABLAS
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            // Herencia de Tabla Única (TPH) incluyendo los roles del sistema
            modelBuilder.Entity<Usuario>()
                .HasDiscriminator<string>("TipoUsuario")
                .HasValue<Cliente>("Cliente")
                .HasValue<Maestro>("Maestro")
                .HasValue<Tecnico>("Tecnico");

            // Sembrado de datos iniciales
            modelBuilder.Entity<Estado>().HasData(
                new Estado { IdEstado = 1, NombreEstado = "Abierto", Descripcion = "Ticket creado por el usuario" },
                new Estado { IdEstado = 2, NombreEstado = "En Proceso", Descripcion = "Técnico trabajando en el problema" },
                new Estado { IdEstado = 3, NombreEstado = "Resuelto", Descripcion = "Solución aplicada" },
                new Estado { IdEstado = 4, NombreEstado = "Cerrado", Descripcion = "Ticket finalizado" }
            );

            modelBuilder.Entity<Prioridad>().HasData(
                new Prioridad { IdPrioridad = 1, Nivel = "Baja" },
                new Prioridad { IdPrioridad = 2, Nivel = "Media" },
                new Prioridad { IdPrioridad = 3, Nivel = "Alta" }
            );

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { IdCategoria = 1, NombreCategoria = "Campus Virtual", Descripcion = "Aulas virtuales y entregas" },
                new Categoria { IdCategoria = 2, NombreCategoria = "Wifi / Redes", Descripcion = "Conectividad dentro del campus" },
                new Categoria { IdCategoria = 3, NombreCategoria = "Correo Institucional", Descripcion = "Acceso a cuentas de correo" }
            );
        }

        // --- BLINDAJE DE CONEXIÓN PARA EVITAR ACCESO DENEGADO ---
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=mysql9001.site4now.net;Database=db_acfc90_dewinrh;Uid=acfc90_dewinrh;Pwd=199905Dw@;SslMode=None;AllowPublicKeyRetrieval=True;Character Set=utf8mb4;";
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 35)));
            }
        }
    }
}