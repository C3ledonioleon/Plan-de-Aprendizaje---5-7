using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using sve.Services;
using sve_api.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddRepositories();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<QRService>();
//builder.Services.AddDbContext<SveContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("myBD"),
//        new MySqlServerVersion(new Version(8, 0, 33))
//    )
//);

var connectionString = builder.Configuration.GetConnectionString("myBD");

builder.Services.AddScoped<IDbConnection>(sp =>
    new MySqlConnection(connectionString));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
