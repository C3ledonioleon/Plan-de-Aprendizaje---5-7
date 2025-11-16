using Microsoft.AspNetCore.Authorization;
using sveCore.DTOs;
using sveCore.Services.IServices;
using System.Data;
using FluentValidation;

namespace sve.Endpoints
{
    public static class ClienteEndpoints
    {
        public static void MapClienteEndpoints(this WebApplication app)
        {
            var cliente = app.MapGroup("/api/clientes");
            cliente.WithTags("Cliente");
            cliente.RequireAuthorization();

            cliente.MapPost("/", (IClienteService clienteService, ClienteCreateDto cliente, IValidator<ClienteCreateDto> validator) =>
            {
                var result = validator.Validate(cliente);
                if (!result.IsValid)
                {
                    var listaErrores = result.Errors
                        .GroupBy(a => a.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return Results.ValidationProblem(listaErrores);
                }

                var id = clienteService.AgregarCliente(cliente);
                return Results.Created($"/api/clientes/{id}", new {IdCliente = id, Cliente = cliente});

            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Usuario" });

            cliente.MapGet("/", (IClienteService clienteService) =>
            {
                var lista = clienteService.ObtenerTodo();
                return Results.Ok(lista);
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador" });

            cliente.MapGet("/{clienteId}", (IClienteService clienteService, int clienteId) =>
            {
                var cliente = clienteService.ObtenerPorId(clienteId);
                if (cliente == null)
                    return Results.NotFound();
                return Results.Ok(cliente);
            }).RequireAuthorization(new AuthorizeAttribute { Roles = "Administrador,Usuario" });

            cliente.MapPut("/{clienteId}", (IClienteService clienteService, ClienteUpdateDto cliente, int Id, IValidator<ClienteUpdateDto> validator) =>
            {
                var result = validator.Validate(cliente);
                if (!result.IsValid)
                {
                    var listaErrores = result.Errors
                        .GroupBy(a => a.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    return Results.ValidationProblem(listaErrores);
                }

                var actualizado = clienteService.ActualizarCliente(Id, cliente);
                if (actualizado == 0)
                    return Results.NotFound();

                return Results.NoContent();
            }).RequireAuthorization("SoloAdmin");
        }
    }
}
