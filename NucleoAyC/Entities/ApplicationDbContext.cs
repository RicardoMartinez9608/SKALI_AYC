using System;
using System.IO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace NucleoAyC.Entities
{
    public class ApplicationDbContext : IdentityDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());
        }

        //public DbSet<Persona> Personas { get; set; }
        //public DbSet<Persona_Tipo_Persona> Persona_Tipo_Persona { get; set; }
        //public DbSet<Tipo_Persona> Tipo_Persona { get; set; }

        private static string GetConnectionString()
        {
            var file = Path.GetFullPath("appsettings.json");
            var config = new ConfigurationBuilder().AddJsonFile(file, optional: false).Build();
            ConnectionParams cp = new ConnectionParams();
            config.GetSection("databaseseguridad").Bind(cp);

            return $"Server={cp.databaseServer};" +
                   $"database={cp.databaseName};" +
                   $"uid={cp.databaseUser};" +
                   $"pwd={cp.databasePass};" +
                   $"pooling=true;";
        }
    }
}