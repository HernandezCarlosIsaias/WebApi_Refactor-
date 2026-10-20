using Microsoft.EntityFrameworkCore;
using Parcial.Entities;


namespace Parcial.DataAccess;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vehiculo> Vehiculos => Set<Vehiculo>();
    
}
