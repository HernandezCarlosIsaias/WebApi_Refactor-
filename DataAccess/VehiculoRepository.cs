using Microsoft.EntityFrameworkCore;

using Parcial.Entities;

namespace Parcial.DataAccess;

public class VehiculoRepository
{
    private readonly AppDbContext _context;

    public VehiculoRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<List<Vehiculo>> ObtenerTodosAsync() =>
        _context.Vehiculos.ToListAsync();

    public Task<Vehiculo?> ObtenerPorIdAsync(int id) =>
        _context.Vehiculos.FirstOrDefaultAsync(v => v.Id == id);

    public Task<Vehiculo?> ObtenerPorPatenteAsync(string patente) =>
        _context.Vehiculos.FirstOrDefaultAsync(v => v.Patente == patente);

    public Task<bool> ExistePatenteAsync(string patente, int? idExcluir = null) =>
        _context.Vehiculos.AnyAsync(v => v.Patente == patente && v.Id != idExcluir);

    public Task<List<Vehiculo>> ObtenerEstacionadosAsync() =>
        _context.Vehiculos.Where(v => !v.Retirado).ToListAsync();

    public Task<int> ContarEstacionadosAsync() =>
        _context.Vehiculos.CountAsync(v => !v.Retirado);

    public async Task AgregarAsync(Vehiculo vehiculo)
    {
        _context.Vehiculos.Add(vehiculo);
        await _context.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EliminarAsync(Vehiculo vehiculo)
    {
        _context.Vehiculos.Remove(vehiculo);
        await _context.SaveChangesAsync();
    }
}

