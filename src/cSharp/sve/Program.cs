using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using sve.DTOs;
using sve.Models;
using sve.Services;
using sve.Services.Contracts;
using System.Data;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddScoped<IQRService, QRService>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SVE API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese '{token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddScoped<QRService>();

var connectionString = builder.Configuration.GetConnectionString("myBD");

builder.Services.AddScoped<IDbConnection>(sp =>
    new MySqlConnection(connectionString));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => x.EnableTryItOutByDefault());
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();





// === Cliente Controller =====
#region Cliente 
var cliente = app.MapGroup("/api/clientes");
cliente.WithTags("Cliente "); 

cliente.MapPost("/",(IClienteService clienteService, ClienteCreateDto cliente) =>

{
    var id = clienteService.AgregarCliente(cliente);
    return Results.Created($"/api/clientes/{id}",cliente);
}

);

cliente.MapGet("/",(IClienteService clienteService ) =>
{
    var lista = clienteService.ObtenerTodo();
    return Results.Ok(lista);
});

cliente.MapGet("/{clienteId}", (IClienteService clienteService, int clienteId) =>

{
    var cliente = clienteService.ObtenerPorId(clienteId);
    if (cliente == null)
        return Results.NotFound();
    return Results.Ok (cliente);
}
);
cliente.MapPut("/{clienteId}", (IClienteService clienteService, ClienteUpdateDto cliente, int Id) =>
{
    var actualizado = clienteService.ActualizarCliente(Id, cliente);
    if (actualizado == 0)

        return Results.NotFound();

    return Results.NoContent();
});

#endregion

#region Entrada
var entradas = app.MapGroup("/api/entradas");
entradas.WithTags("Entradas");

entradas.MapPost("/", (IEntradaService entradaService, EntradaCreateDto entrada) =>
{
    var id = entradaService.AgregarEntrada(entrada);
    return Results.Created($"api/entrada/{id}", entrada);
});

entradas.MapGet("/", (IEntradaService entradaService) =>
{
    var lista = entradaService.ObtenerTodo();
    return Results.Ok(lista);
});

entradas.MapGet("/{entradaId}", (IEntradaService entradaService, int entradaId) =>
{
    var entrada = entradaService.ObtenerPorId(entradaId);
    if (entrada == null)
        return Results.NotFound();
    return Results.Ok(entrada);
});

entradas.MapPost("{entradaId}/anular", (IEntradaService entradaService, int entradaId) =>
{
    return !entradaService.AnularEntrada(entradaId)
    ? Results.NotFound($"No se encontro la entrada con ID{entradaId}") :
    Results.Ok($"La entrada con ID{entradaId} fue anulada corectamente");
});

// ==== QR ==== 

entradas.MapGet("/{entradaId}/qr", ( int entradaId, IEntradaService entradaService, IQRService qrService, IOrdenService ordenService) =>
{
      var entrada = entradaService.ObtenerPorId(entradaId);
    if (entrada == null)
        return Results.NotFound($"No se encontró la entrada con ID {entradaId}.");

       var orden = ordenService.ObtenerPorId(entrada.IdOrden);
    if (orden == null)
        return Results.BadRequest($"No se encontró la orden asociada a la entrada {entradaId}.");

    // Validar si la orden está pagada
    
    if (orden.Estado != EstadoOrden.Pagada) 
    {
        return Results.BadRequest("Error: No se puede generar el código QR porque la orden no está pagada.");
    }
    var contenido = $"Entrada {entradaId} - Sistema de Venta de Entradas";
    var qrBytes = qrService.GenerarQR(contenido);

    return Results.File(qrBytes, "image/png");
    
});


#endregion


// === EventoController === 
#region Evento
var evento = app.MapGroup ("/api/eventos");

evento.WithTags("Evento");


evento.MapPost("/",(IEventoService eventoService, EventoCreateDto evento ) =>

{
    var id = eventoService.AgregarEvento(evento);
    return Results.Created($"/api/evento{id}", evento);
} );

evento.MapGet ("/",(IEventoService eventoService )=>

{
    var evento = eventoService.ObtenerTodo();
    return Results.Ok(evento);
} );

evento.MapGet ("/{eventoId}",(int eventoId ,IEventoService eventoService) =>

{
  var evento = eventoService.ObtenerPorId(eventoId);
  if (evento == null ) return Results.NotFound();
   return Results.Ok (evento);
}
);

evento.MapPut("/{eventoId}",( int eventoId, IEventoService eventoService , EventoUpdateDto evento ) => 
{
    var actualizado = eventoService.ActualizarEvento (eventoId, evento);
    if (actualizado == 0 ) 
        return Results.NotFound ();

    return Results.NoContent ();
} );

evento.MapDelete("/{eventoId}",(int eventoId ,IEventoService eventoService ) =>


{
    var eliminado = eventoService.EliminarEvento (eventoId);
    if (eliminado == 0 ) 
        return Results.NotFound() ;

    return Results.NoContent ();
});

evento.MapPost("/{eventoId}/publicar",(int id, IEventoService eventoService) => 

{
    var publicado = eventoService.Publicar(id);
    if(publicado == 0 ) 
     return Results.NotFound();

    return Results.Ok (new {mensaje = "Evento publicado correctamente"});
});


evento.MapPost ("/{eventoId}/cancelar", (int id , IEventoService eventoService) =>

{   
    var cancelado = eventoService.Cancelar ( id);
    if (cancelado == 0 ) 
    return Results.NotFound();
    
    return Results.Ok(new { mensaje = "Evento cancelado correctamente"});
});
#endregion 

#region Funcion
var funcion = app.MapGroup("/api/funciones");

funcion.WithTags("Funcion");

funcion.MapPost("/", (IFuncionService funcionService, FuncionCreateDto funcion) =>
{
    var id = funcionService.AgregarFuncion(funcion);
    return Results.Created($"/api/funciones/{id}", funcion);
});

funcion.MapGet("/", (IFuncionService funcionService) =>
{
    var funciones = funcionService.ObtenerTodo();
    if (funciones == null)
        return Results.NotFound();
    return Results.Ok(funciones);
});

funcion.MapGet("/{funcionId}", (int funcionId, IFuncionService funcionService) =>
{
    var funcionEncontrada = funcionService.ObtenerPorId(funcionId);
    if (funcionEncontrada == null)
        return Results.NotFound();
    return Results.Ok(funcionEncontrada);
});

funcion.MapPut("/{funcionId}", (int funcionId, IFuncionService funcionService, FuncionUpdateDto funcion) =>
{
    var actualizado = funcionService.ActualizarFuncion(funcionId, funcion);
    if (actualizado == 0)
        return Results.NotFound();
    return Results.NoContent();
});

funcion.MapDelete("/{funcionId}", (int funcionId, IFuncionService funcionService) =>
{
    var eliminado = funcionService.EliminarFuncion(funcionId);
    if (eliminado == 0)
        return Results.NotFound();
    return Results.NoContent();
});

funcion.MapPost("/{funcionId}/cancelar", (int funcionId, IFuncionService funcionService) =>
{
    var actualizado = funcionService.CancelarFuncion(funcionId);
    if (actualizado == 0)
        return Results.NotFound();
    return Results.Ok(new { mensaje = "Función cancelada correctamente" });
});
#endregion

#region Local

var local = app.MapGroup("api/locales");
local.WithTags("Local");

local.MapPost("/", (ILocalService localService, LocalCreateDto local) =>
{
    var id = localService.AgregarLocal(local);
    return Results.Created($"/api/local/{id}", local);
});

local.MapGet("/", (ILocalService localService) =>
{
    var local = localService.ObtenerTodo();
    return Results.Ok(local);
});

local.MapGet("/{localId}", (int localId, ILocalService localService) =>
{
    var localEncontrado = localService.ObtenerPorId(localId);
    if ( localEncontrado == null)
        return Results.NotFound();
    return Results.Ok(localEncontrado);
});

local.MapPut("/{localId}", (int localId , ILocalService localService,LocalUpdateDto local) =>
{
    var actualizado = localService.ActualizarLocal(localId, local);
    if (actualizado == 0) 
        return Results.NotFound();
    return Results.NoContent();
});

local.MapDelete("/{localId}", (int localId, ILocalService localService) =>

{
    var eliminado = localService.EliminarLocal(localId);
    if (eliminado == 0) return Results.NotFound();
    return Results.NoContent();
});
#endregion


#region Orden

var orden = app.MapGroup("api/ordenes");
orden.WithTags("Orden");

orden.MapPost("/", (OrdenCreateDto orden, IOrdenService ordenService) =>
{
    var ordenId = ordenService.AgregarOrden(orden);
    return Results.Ok(new { IdOrden = ordenId });
});

orden.MapGet("/",(IOrdenService ordenService)=>

{
    var ordenes = ordenService.ObtenerTodo();
    return Results.Ok(ordenes);
});

orden.MapGet("/{ordenId}", (int ordenId, IOrdenService ordenService) =>
{
    var orden = ordenService.ObtenerPorId(ordenId);
    if (orden == null)
        return Results.NotFound();
    return Results.Ok(orden);
});

orden.MapPost("/{ordenId}/pagar", (int ordenId ,IOrdenService ordenService) =>
{
    var result = ordenService.PagarOrden(ordenId);

    if (!result)
        return Results.BadRequest("No se pudo procesar el pago o la orden no existe.");

    return Results.Ok(new { mesaje = "Orden pagada y entradas emitidas" });
});


orden.MapPost ("/{ordenId}/cancelar",(int ordenId, IOrdenService ordenService )=>

{
     var result = ordenService.CancelarOrden(ordenId);

    if (!result)
        return Results.BadRequest("No se pudo cancelar la orden (puede que ya esté pagada o no exista.");

    return Results.Ok(new { mesaje = "Orden cancelada" });
}

);

#endregion

#region Sector
var sector = app.MapGroup("/api/sectores");
sector.WithTags("Sector");

sector.MapPost("/", (ISectorService sectorService, SectorCreateDto sector) =>
{
    var id = sectorService.AgregarSector(sector);
    return Results.Created($"/api/sectores/{id}", sector);
});

sector.MapGet("/", (ISectorService sectorService) =>
{
    var sectores = sectorService.ObtenerTodo();
    return Results.Ok(sectores);
});

sector.MapGet("/{sectorId}", (int sectorId,ISectorService sectorService) =>
{
    var sector = sectorService.ObtenerPorId(sectorId);
    if (sector == null)
        return Results.NotFound();
    return Results.Ok(sector);
});

sector.MapPut("/{sectorId}", (int sectorId, ISectorService sectorService, SectorUpdateDto sector) =>
{
    var actualizado = sectorService.ActualizarSector(sectorId, sector);
    if (actualizado == 0)
        return Results.NotFound();
    return Results.NoContent();
});

sector.MapDelete("/{sectorId}", (int sectorId, ISectorService sectorService) =>
{
    var eliminado = sectorService.EliminarSector(sectorId);
    if (eliminado == 0)
        return Results.NotFound();
    return Results.NoContent();
});

#endregion

#region Tarifa
var tarifa = app.MapGroup("/api/tarifas");
tarifa.WithTags("Tarifa");

tarifa.MapPost("/", (ITarifaService tarifaService, TarifaCreateDto tarifa) =>
{
    var id = tarifaService.AgregarTarifa(tarifa);
    return Results.Created($"/api/tarifas/{id}", tarifa);
});

tarifa.MapGet("/", (ITarifaService tarifaService) =>
{
    var tarifas = tarifaService.ObtenerTodo();
    return Results.Ok(tarifas);
});

tarifa.MapGet("/{tarifaId}", (int tarifaId, ITarifaService tarifaService) =>
{
    var tarifa = tarifaService.ObtenerPorId(tarifaId);
    if (tarifa == null)
        return Results.NotFound();
    return Results.Ok(tarifa);
});

tarifa.MapPut("/{tarifaId}", ( int tarifaId, ITarifaService tarifaService, TarifaUpdateDto tarifa) =>
{
    var actualizado = tarifaService.ActualizarTarifa(tarifaId, tarifa);
    if (actualizado == 0)
        return Results.NotFound();
    return Results.NoContent();
});


tarifa.MapDelete("/{tarifaId}", (int tarifaId, ITarifaService tarifaService) =>
{
    var eliminado = tarifaService.EliminarTarifa(tarifaId);
    if (eliminado == 0)
        return Results.NotFound();
    return Results.NoContent();
});

#endregion


#region Usuario
var auth = app.MapGroup("/api/Auth");
auth.WithTags("Auth");

auth.MapPost("Register", (IUsuarioService usuarioService, RegisterDto usuario) =>
{
    var id = usuarioService.Register(usuario);
    return Results.Created($"/api/usuarios/{id}", usuario);
});


auth.MapPost("Login", (IUsuarioService usuarioService, LoginDto usuario) =>
{
    var tokens = usuarioService.Login(usuario);
    return Results.Ok(tokens);
});


auth.MapPost("Refresh", (IUsuarioService usuarioService, [FromBody] string refreshToken) =>
{
    try
    {
        var tokens = usuarioService.Refresh(refreshToken);
        return Results.Ok(tokens);
    }
    catch (Exception ex)
    {
        return Results.Json(
            new { error = ex.Message }
        );
}
});


auth.MapPost("Logout", [Authorize] (IUsuarioService usuarioService, HttpContext httpContext) =>
{
    var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
    if (email != null)
    {
        usuarioService.Logout(email);
        return Results.Ok(new { message = "Sesión cerrada correctamente." });
    }
    return Results.Unauthorized();
});

auth.MapGet("/me", [Authorize] (HttpContext http ) =>
{
    var email = http.User.FindFirstValue(ClaimTypes.Email);
    var rol = http.User.FindFirstValue(ClaimTypes.Role);

    if (string.IsNullOrEmpty(email))
        return Results.Unauthorized();

    return Results.Ok(new { email, rol });
});

auth.MapGet("Roles", (IUsuarioService usuarioService) =>
{
    var roles = Enum.GetNames(typeof(RolUsuario));
    return Results.Ok(roles);
});


auth.MapPost("/{usuarioId}/rol", [Authorize(Roles = "Administrador")] (int usuarioId, [FromBody] RolUsuario nuevoRol, IUsuarioService usuarioService) =>
{
    try
    {
        var usuario = usuarioService.GetById(usuarioId);
        if (usuario == null)
            return Results.NotFound(new { error = "Usuario no encontrado." });

        usuarioService.UpdateRol(usuarioId, nuevoRol);

        return Results.Ok(new { message = $"Rol asignado: {nuevoRol}" });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
});
#endregion

app.Run();