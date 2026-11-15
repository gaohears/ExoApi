using ExoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoApi.Contexts;

public class ExoApiContext : DbContext
{
    public ExoApiContext(DbContextOptions<ExoApiContext> options) : base(options) { }

    public DbSet<Projeto> Projetos => Set<Projeto>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Projeto>(e =>
        {
            e.ToTable("Projetos");
            e.Property(p => p.DataInicio).HasColumnType("date");
        });

        modelBuilder.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios");
        });
    }
}