# API en cuatro capas

## Estructura final

```text
Parcial/
  Controllers/
    VehiculoController.cs
  Business/
    VehiculoService.cs
    DTOs/
      PostRequestDto.cs
      PutRequestDto.cs
      GetByIdResponseDto.cs
      GetByPatenteResponseDto.cs
      GetEstacionadosResponseDto.cs
      RetirarResponseDto.cs
  DataAccess/
    AppDbContext.cs
    VehiculoRepository.cs
    Migrations/
      20261013175138_InitialCreate.cs
      20261013175138_InitialCreate.Designer.cs
      AppDbContextModelSnapshot.cs
  Entities/
    Vehiculo.cs
  Program.cs
```

## Responsabilidades y forma de trabajo

- Controller: recibe los DTOs de entrada, valida que la patente no esté vacía, llama al servicio y devuelve las respuestas HTTP. Usa if y try/catch; no consulta Entity Framework.
- Business: valida patentes, convierte entidades a DTOs y coordina las operaciones. Crear recibe PostRequestDto; modificar recibe PutRequestDto. Las consultas devuelven DTOs o null si no hay vehículo. Crear y modificar devuelven un mensaje de error o null cuando la operación salió bien. Eliminar devuelve bool para indicar si encontró el vehículo. La patente vacía se valida en el controller con if y BadRequest; los duplicados y vehículos inexistentes se comprueban en Business y se devuelven como mensajes. Cada acción tiene un solo catch (Exception), reservado para errores inesperados. No se utiliza throw new. No hay ResultadoOperacion, enumeraciones de estados ni interfaces adicionales.
- DataAccess: contiene el contexto, el repositorio y las migraciones. Todas las consultas y guardados de EF Core están en esta capa.
- Entities: conserva Vehiculo con sus propiedades originales. Las entidades se utilizan entre el servicio y el repositorio; las respuestas de vehículos al cliente son DTOs.

Los DTOs existentes se movieron de Controllers/DTOs a Business/DTOs para que el servicio pueda utilizarlos sin depender de la carpeta de presentación. Se conservaron sus nombres, propiedades y contratos. RetirarResponseDto agrega una respuesta explícita para retirar, con los mismos campos JSON que tenía la entidad: id, patente, marca, tipo y retirado.

## Cambios realizados

- Creados VehiculoService, VehiculoRepository y RetirarResponseDto.
- Eliminado Business/ResultadoOperacion.cs y todas sus referencias.
- Eliminada la lista estática. Todas las operaciones usan la base de datos.
- POST consulta duplicados en SQL Server.
- PUT excluye el Id del propio vehículo al buscar una patente duplicada.
- PATCH retirar consulta la base, rechaza vehículos ausentes o ya retirados y guarda Retirado = true.
- GET cantidad usa CountAsync sobre vehículos no retirados.
- AppDbContext se movió a DataAccess; las migraciones se movieron a DataAccess/Migrations. Se ajustaron namespaces y referencias a Parcial.DataAccess y Parcial.DataAccess.Migrations. No se cambió el contenido del modelo ni las operaciones Up/Down; tampoco se generaron ni ejecutaron migraciones.
- VehiculoController y Program.cs fueron modificados. Entities/Vehiculo.cs, paquetes y configuración de conexión permanecen iguales.

Se mantienen las nueve rutas y verbos originales. Se mantiene por compatibilidad el 404 ante una patente duplicada; las validaciones devuelven 400, las escrituras exitosas conservan sus mensajes y las excepciones generales devuelven los mensajes 500 existentes.

## Ejemplo de recorrido

GET /Vehiculo/estacionados -> VehiculoController.VehiculosEstacionados -> VehiculoService.ObtenerEstacionadosAsync -> VehiculoRepository.ObtenerEstacionadosAsync -> AppDbContext -> SELECT en SQL Server con Retirado = 0.

El repositorio devuelve las entidades. El servicio las recorre con foreach y arma una lista de GetEstacionadosResponseDto. El controller responde Ok con esa lista. List<T> se usa para transportar resultados de consultas; no hay una lista estática que funcione como almacenamiento.

## Inyección de dependencias

Program.cs registra:

```csharp
builder.Services.AddScoped<VehiculoRepository>();
builder.Services.AddScoped<VehiculoService>();
```

Se conserva AddDbContext<AppDbContext> con UseSqlServer y DefaultConnection. Controller, servicio y repositorio reciben sus dependencias por constructor; comparten el contexto scoped de la solicitud.





