using Microsoft.AspNetCore.Mvc;
using Parcial.Business;

namespace Parcial.Controllers;

[ApiController]
[Route("[controller]")]
public class VehiculoController : ControllerBase
{
    private readonly VehiculoService _service;
    private readonly ILogger<VehiculoController> _logger;

    public VehiculoController(VehiculoService service, ILogger<VehiculoController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] PostRequestDto v)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(v.patente))
                return BadRequest("La patente es obligatoria");

            var error = await _service.CrearAsync(v);
            if (error is not null)
                return NotFound(error);
            return Ok("Creado con exito");
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar registrar el vehiculo");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var vehiculos = await _service.ObtenerTodosAsync();
            return Ok(vehiculos);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var vehiculo = await _service.ObtenerPorIdAsync(id);
            if (vehiculo is null)
                return NotFound($"El vehiculo con Id {id} no se encuentra");

            return Ok(vehiculo);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar buscar el vehiculo");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> ModificarVehiculo(int id, [FromBody] PutRequestDto v)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(v.patente))
                return BadRequest("El numero de patente es obligatorio");

            var error = await _service.ModificarAsync(id, v);
            if (error is not null)
                return NotFound(error);

            return Ok("Actualizado Correctamente");
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar actualizar el vehiculo");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado = await _service.EliminarAsync(id);
            if (!eliminado)
                return NotFound($"El vehiculo con Id {id} no se encuentra");

            return Ok("El vehiculo a sido eliminado correctamente");
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar eliminar el vehiculo");
        }
    }

    [HttpGet("buscar/{patente}")]
    public async Task<IActionResult> GetById(string patente)
    {
        try
        {
            var vehiculo = await _service.ObtenerPorPatenteAsync(patente);
            if (vehiculo is null)
                return NotFound($"El vehiculo con patente {patente} no se encuentra");

            return Ok(vehiculo);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar buscar el vehiculo");
        }
    }

    [HttpGet("estacionados")]
    public async Task<IActionResult> VehiculosEstacionados()
    {
        try
        {
            var vehiculos = await _service.ObtenerEstacionadosAsync();
            return Ok(vehiculos);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar buscar el vehiculo");
        }
    }

    [HttpPatch("retirar/{id}")]
    public async Task<IActionResult> RetirarVehiculo(int id)
    {
        try
        {
            var vehiculo = await _service.RetirarAsync(id);
            if (vehiculo is null)
                return NotFound($"El vehiculo con Id {id} ya fue retirado");

            return Ok(vehiculo);
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar retirar el vehiculo");
        }
    }

    [HttpGet("cantidad")]
    public async Task<IActionResult> CantidadDeAutos()
    {
        try
        {
            var cantidad = await _service.ObtenerCantidadAsync();
            return Ok($"La cantidad de vehiculos en el estacionamiento es: {cantidad}");
        }
        catch (Exception)
        {
            return StatusCode(500, "Ocurrio un error al intentar ver la cantidad de vehiculos");
        }
    }
}


