using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using sve.DTOs;
using sve.Models;
using sve.Services;
using sve.Services.Contracts;
using System.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices();
builder.Services.AddRepositories();

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
#region 
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

// === Entradas Controller =====

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

entradas.MapPut("/{entradaId}", (IEntradaService entradaService, EntradaUpdateDto entrada, int Id) =>
{
    var actualizado = entradaService.ActualizarEntrada(Id, entrada);
    if (actualizado == 0)
        return Results.NotFound();
    return Results.NoContent();
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

local.MapGet("/", (int localId, ILocalService localService) =>
{
    var id = localService.ObtenerPorId(localId);
    if (local == null)
        return Results.NotFound();
    return Results.Ok(local);
});

local.MapPut("/", (int localId, ILocalService localService, LocalUpdateDto local) =>
{
    var actualizado = localService.ActualizarLocal(localId, local);
    if (actualizado == 0) return Results.NotFound();
    return Results.NoContent();
});

local.MapDelete("/", (int localId, ILocalService localService) =>

{
    var eliminado = localService.EliminarLocal(localId);
    if (eliminado == 0) return Results.NotFound();
    return Results.NoContent();
});
#endregion


#region Orden

var Orden = app.MapGroup("api/Ordenes");
Orden.WithTags("Orden");

Orden.MapPost("/api/ordenes", (OrdenCreateDto orden, IOrdenService ordenService) =>
{
    var ordenId = ordenService.AgregarOrden(orden);
    return Results.Ok(new { IdOrden = ordenId });
});

Orden.MapGet("/",(IOrdenService ordenService)=>

{
    var ordenes = ordenService.ObtenerTodo();
    return Results.Ok(ordenes);
});

Orden.MapGet("/", (int ordenId, IOrdenService ordenService) =>
{
    var orden = ordenService.ObtenerPorId(ordenId);
    if (orden == null)
        return Results.NotFound();
    return Results.Ok(orden);
});

Orden.MapPost("api/{ordenId:int}/pagar", (int ordenId ,IOrdenService ordenService) =>
{
    var result = ordenService.PagarOrden(ordenId);

    if (!result)
        return Results.BadRequest("No se pudo procesar el pago o la orden no existe.");

    return Results.Ok(new { mesaje = "Orden pagada y entradas emitidas" });
});




#endregion


app.Run();