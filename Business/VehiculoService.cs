using Parcial.DataAccess;
using Parcial.Entities;

namespace Parcial.Business;

public class VehiculoService
{
    private readonly VehiculoRepository _repository;

    public VehiculoService(VehiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task<string?> CrearAsync(PostRequestDto dto)
    {
        if (await _repository.ExistePatenteAsync(dto.patente))
            return "La patente pertenece a otro vehiculo";

        var vehiculo = new Vehiculo
        {
            Patente = dto.patente,
            Marca = dto.marca,
            Tipo = dto.tipo
        };
        await _repository.AgregarAsync(vehiculo);
        return null;
    }

    public async Task<string?> ModificarAsync(int id, PutRequestDto dto)
    {
        var vehiculo = await _repository.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return $"El vehiculo con Id {id} no se encuentra";

        if (await _repository.ExistePatenteAsync(dto.patente, id))
            return "La patente pertenece a otro vehiculo";

        vehiculo.Patente = dto.patente;
        vehiculo.Marca = dto.marca;
        vehiculo.Tipo = dto.tipo;
        await _repository.GuardarCambiosAsync();
        return null;
    }

    public async Task<List<GetByIdresponseDto>> ObtenerTodosAsync()
    {
        var vehiculos = await _repository.ObtenerTodosAsync();
        var respuesta = new List<GetByIdresponseDto>();
        foreach (var vehiculo in vehiculos)
        {
            respuesta.Add(new GetByIdresponseDto
            {
                patente = vehiculo.Patente,
                marca = vehiculo.Marca
            });
        }
        return respuesta;
    }

    public async Task<GetByIdresponseDto?> ObtenerPorIdAsync(int id)
    {
        var vehiculo = await _repository.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return null;

        return new GetByIdresponseDto
        {
            patente = vehiculo.Patente,
            marca = vehiculo.Marca
        };
    }

    public async Task<GetByPatenteResponseDto?> ObtenerPorPatenteAsync(string patente)
    {
        var vehiculo = await _repository.ObtenerPorPatenteAsync(patente);
        if (vehiculo is null)
            return null;

        return new GetByPatenteResponseDto
        {
            patente = vehiculo.Patente,
            marca = vehiculo.Marca
        };
    }

    public async Task<List<GetEstacionadosResponseDto>> ObtenerEstacionadosAsync()
    {
        var vehiculos = await _repository.ObtenerEstacionadosAsync();
        var respuesta = new List<GetEstacionadosResponseDto>();
        foreach (var vehiculo in vehiculos)
        {
            respuesta.Add(new GetEstacionadosResponseDto
            {
                patente = vehiculo.Patente,
                marca = vehiculo.Marca
            });
        }
        return respuesta;
    }

    public async Task<RetirarResponseDto?> RetirarAsync(int id)
    {
        var vehiculo = await _repository.ObtenerPorIdAsync(id);
        if (vehiculo is null || vehiculo.Retirado)
            return null;

        vehiculo.Retirado = true;
        await _repository.GuardarCambiosAsync();
        return new RetirarResponseDto
        {
            id = vehiculo.Id,
            patente = vehiculo.Patente,
            marca = vehiculo.Marca,
            tipo = vehiculo.Tipo,
            retirado = vehiculo.Retirado
        };
    }

    public async Task<int> ObtenerCantidadAsync()
    {
        return await _repository.ContarEstacionadosAsync();
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var vehiculo = await _repository.ObtenerPorIdAsync(id);
        if (vehiculo is null)
            return false;

        await _repository.EliminarAsync(vehiculo);
        return true;
    }
}



