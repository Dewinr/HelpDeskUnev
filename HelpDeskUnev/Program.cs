using HelpDeskUnev.Data;
using HelpDeskUnev.Models;
using HelpDeskUnev.Services;
using HelpDeskUnev.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 1. Configuración con la nueva contraseña de MySQL (199905Dw@) y versión explícita 8.0
        var connectionString = "Server=mysql9001.site4now.net;Database=db_acfc90_dewinrh;Uid=acfc90_dewinrh;Pwd=199905Dw@;SslMode=None;AllowPublicKeyRetrieval=True;Character Set=utf8mb4;";
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 35));

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));

        builder.Services.AddTransient<EmailService>();

        // 2. Configurar Autenticación por Cookies y Microsoft OAuth
        builder.Services.AddAuthentication("CookieAuth")
              .AddCookie("CookieAuth", options =>
              {
                  options.Cookie.Name = "HelpDeskUnev.Cookie";
                  options.LoginPath = "/Login/Index";
                  options.AccessDeniedPath = "/Login/AccesoDenegado";
                  options.ExpireTimeSpan = TimeSpan.FromHours(8);
              })
              .AddMicrosoftAccount(options =>
              {
                  options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "TU_CLIENT_ID_DE_AZURE";
                  options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "TU_CLIENT_SECRET_DE_AZURE";
              });

        // Ignorar ciclos de referencia al serializar a JSON
        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        var app = builder.Build();

        // 3. Configurar el pipeline HTTP
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
        }

        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // 4. Asegurar la creación de tablas y sembrado seguro de usuarios iniciales
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                // Crea la base de datos/tablas si no existen en el servidor MySQL
                context.Database.EnsureCreated();

                // Sembrar Cliente por defecto
                if (!context.Usuarios.Any(u => u.Matricula == "2024-3300026"))
                {
                    var cliente = new Cliente
                    {
                        Nombre = "Dewin",
                        Apellido = "Rivera",
                        Correo = "d.rivera@unev.edu.do",
                        Matricula = "2024-3300026",
                        ContrasenaHash = PasswordHelper.HashPassword("123456"),
                        EsActivo = true
                    };

                    context.Usuarios.Add(cliente);
                    context.SaveChanges();
                }

                // Sembrar Administrador por defecto usando la clase Tecnico
                if (!context.Usuarios.Any(u => u.Matricula == "admin"))
                {
                    var admin = new Tecnico
                    {
                        Nombre = "Soporte",
                        Apellido = "Admin",
                        Correo = "admin@unev.edu.do",
                        Matricula = "admin",
                        ContrasenaHash = PasswordHelper.HashPassword("admin123"),
                        EsActivo = true
                    };

                    context.Usuarios.Add(admin);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Ocurrió un error al sembrar la base de datos MySQL.");
            }
        }

        // 5. Configurar ruta por defecto hacia el Login
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Login}/{action=Index}/{id?}");

        app.Run();
    }
}