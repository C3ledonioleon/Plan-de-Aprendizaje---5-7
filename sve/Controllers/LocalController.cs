using Microsoft.AspNetCore.Mvc;
using sve.DTOs;
using sve.Models;
using sve.Services.Contracts;

namespace sve.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LocalesController : ControllerBase
    {
        private readonly ILocalService _localService;

        public LocalesController(ILocalService localService)
        {
            _localService = localService;
        }

        [HttpPost]
        public IActionResult CrearLocal([FromBody] LocalCreateDto dto)
        {
            var id = _localService.AgregarLocal(dto);
            return CreatedAtAction(nameof(ObtenerLocal), new { idLocal = id }, dto);
        }

        [HttpGet]
        public IActionResult ObtenerLocales() => Ok(_localService.ObtenerTodo());

        [HttpGet("{idLocal}")]
        public IActionResult ObtenerLocal(int idLocal)
        {
            var local = _localService.ObtenerPorId(idLocal);
            if (local == null) return NotFound();
            return Ok(local);
        }

        [HttpPut("{idLocal}")]
        public IActionResult ActualizarLocal(int idLocal, [FromBody] LocalUpdateDto dto)
        {
            var actualizado = _localService.ActualizarLocal(idLocal, dto);
            if (!actualizado) return NotFound();
            return NoContent();
        }

        [HttpDelete("{idLocal}")]
        public IActionResult EliminarLocal(int idLocal)
        {
            var eliminado = _localService.EliminarLocal(idLocal);
            if (!eliminado) return NotFound();
            return NoContent();
        }
    }
}
