using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts;

    public interface IClienteService
    {
        List<ClienteDto> ObtenerTodo();            // Antes: GetAll
        Cliente? ObtenerPorId(int id);           // Antes: GetById
        int AgregarCliente(ClienteCreateDto cliente);     // Antes: Add
        bool ActualizarCliente(int id, ClienteUpdateDto cliente); // Antes: Update
        bool EliminarCliente(int id);            // Antes: Delete
    }
