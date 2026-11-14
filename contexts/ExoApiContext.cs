using ExoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExoApi.Contexts;

public class ExoApiContext : DbContext
{
    public ExoApiContext(DbContextOptions<ExoApiContext> options) : base(options) { }

    public DbSet<Projeto> Projetos => Set<Projeto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Projeto>(e =>
        {
            e.ToTable("Projetos");
            e.Property(p => p.DataInicio).HasColumnType("date");
        });
    }
}