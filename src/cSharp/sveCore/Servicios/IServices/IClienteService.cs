using sveCore.DTOs;
using sveCore.Models;
using System.Collections.Generic;

namespace sveCore.Services.IServices;
    public interface IClienteService
    {
        List<ClienteDto> ObtenerTodo();            // Antes: GetAll
        Cliente? ObtenerPorId(int id);           // Antes: GetById
        int AgregarCliente(ClienteCreateDto cliente);     // Antes: Add
        int ActualizarCliente(int id, ClienteUpdateDto cliente); // Antes: Update
        int EliminarCliente(int id);            // Antes: Delete
}
