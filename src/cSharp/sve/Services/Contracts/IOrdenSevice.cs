using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts
{
    public interface IOrdenService
    {
        List<OrdenDto> ObtenerTodo();
        Orden? ObtenerPorId(int id);
        int AgregarOrden(OrdenCreateDto orden);
        bool ActualizarOrden(int id, OrdenUpdateDto orden);
        bool EliminarOrden(int id);
    }
}
