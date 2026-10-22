using HelpDeskUnev.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskUnev.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Tecnico> Tecnicos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sembrado de datos iniciales para pruebas
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Campus Virtual", Descripcion = "Aulas virtuales y entregas" },
            new Categoria { Id = 2, Nombre = "Wifi / Redes", Descripcion = "Conectividad dentro del campus" },
            new Categoria { Id = 3, Nombre = "Correo Institucional", Descripcion = "Acceso a cuentas institucionales" }
        );
    }
}
