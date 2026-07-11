using DinoArgentoApi.Models;
using DinoArgentoApi.Models.Dieta;
using DinoArgentoApi.Models.Dinosaurio;
using DinoArgentoApi.Models.Periodo;
using DinoArgentoApi.Models.Role;
using DinoArgentoApi.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DinoArgentoApi.Config
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dinosaurio> Dinosaurios { get; set; }
        public DbSet<Dieta> Dietas { get; set; }
        public DbSet<Periodo> Periodos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dinosaurio>()
                .HasMany(d => d.Dietas)
                .WithMany(di => di.Dinosaurios)
                .UsingEntity<DinosaurioDieta>();

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<RoleUser>();
        }
    }
}