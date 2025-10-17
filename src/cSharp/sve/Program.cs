using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
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

var cliente = app.MapGroup("/api/clientes");
cliente.WithTags("Cliente "); // 👈 CORREGIDO: antes decía "clientes"

cliente.MapPost("/",(IClienteService clienteService, ClienteCreateDto cliente) =>

{
    var id = clienteService.AgregarCliente(cliente);
    return Results.Created($"api/cliente/{id}",cliente);
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

cliente.MapPut("/", (IClienteService clienteService, ClienteUpdateDto cliente, int Id) =>
{
    var actualizado = clienteService.ActualizarCliente(Id, cliente);
    if (actualizado == 0)

        return Results.NotFound();

    return Results.NoContent();
});


// === Entradas Controller =====
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

entradas.MapPut("/", (IEntradaService entradaService, EntradaUpdateDto entrada, int Id) =>
{
    var actualizado = entradaService.ActualizarEntrada(Id, entrada);
    if (actualizado == 0)
        return Results.NotFound();
    return Results.NoContent();
});



app.Run();