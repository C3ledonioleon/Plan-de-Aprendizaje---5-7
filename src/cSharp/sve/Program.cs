using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySql.Data.MySqlClient;
using sveCore.DTOs;
using sveService.Services;
using sveCore.Services.IServices;
using System.Data;
using System.Text;
using FluentValidation;
using sveServicio.Validation;
using sve.Endpoints;
using sveDapper.Factories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDbConnectionFactory, RoleBasedDbConnectionFactory>();

builder.Services.AddServices();
builder.Services.AddRepositories();
builder.Services.AddScoped<IQRService, QRService>();

builder.Services.AddTransient<IValidator<ClienteCreateDto>, ClienteValidator>();
builder.Services.AddTransient<IValidator<ClienteUpdateDto>, ActualizarCliente>();

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
    var config = builder.Configuration.GetSection("JwtSettings");
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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", p => p.RequireRole("Administrador"));
    options.AddPolicy("SoloUsuario", p => p.RequireRole("Usuario"));
    options.AddPolicy("SoloOrganizador", p => p.RequireRole("Organizador"));
    options.AddPolicy("SoloMolinete", p => p.RequireRole("Molinete"));
});

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

//==== Endpoints ========
app.MapClienteEndpoints();  
app.MapEntradaEndpoints();  
app.MapEventoEndpoints();
app.MapFuncionEndpoints();
app.MapLocalEndpoints();
app.MapOrdenEndpoints();
app.MapSectorEndpoints();
app.MapTarifaEndpoints();
app.MapUsuarioEndpoints();
app.Run();