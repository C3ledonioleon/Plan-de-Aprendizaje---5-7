using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts;

    public interface IEntradaService
    {
        List<EntradaDto> ObtenerTodo();
        Entrada? ObtenerPorId(int id);
        int AgregarEntrada(EntradaCreateDto entrada);
        int ActualizarEntrada (int id, EntradaUpdateDto entrada);
        int AnularEntrada(int id);
        int EliminarEntrada(int id);
    }
