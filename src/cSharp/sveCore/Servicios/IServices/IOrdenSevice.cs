using sveCore.DTOs;
using sveCore.Models;
using System.Collections.Generic;


namespace sveCore.Services.IServices;
    public interface IOrdenService
    {
        List<OrdenDto> ObtenerTodo();
        Orden? ObtenerPorId(int id);
        int AgregarOrden(OrdenCreateDto orden);
        int ActualizarOrden(int id, OrdenUpdateDto orden);
        int EliminarOrden(int id);
        bool CancelarOrden(int ordenId);
        bool PagarOrden(int ordenId);
    }
