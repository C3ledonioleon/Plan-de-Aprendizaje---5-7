using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.Services;
using sve.Services.Contracts;
using sve.Repositories.Contracts;
using sve.Models;
using sve.DTOs;

namespace sve.Tests.Services
{
    public class EventoServiceTests
    {
        private readonly Mock<IEventoRepository> _mockRepo;
        private readonly EventoService _service;

        public EventoServiceTests()
        {
            _mockRepo = new Mock<IEventoRepository>();
            _service = new EventoService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeEventos()
        {
            // Arrange
            var eventos = new List<Evento>
            {
                new Evento { IdEvento = 1, Nombre = "Evento1", Descripcion = "Desc1", Estado = EstadoEvento.Inactivo },
                new Evento { IdEvento = 2, Nombre = "Evento2", Descripcion = "Desc2", Estado = EstadoEvento.Inactivo }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(eventos);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Evento1", resultado.First().Nombre);
        }

        [Fact]
        public void ObtenerPorId_Existe_DeberiaRetornarEvento()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Nombre = "Evento1", Descripcion = "Desc1", Estado = EstadoEvento.Inactivo };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Evento1", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_NoExiste_DeberiaRetornarNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(1)).Returns((Evento?)null);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarEvento_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var dto = new EventoCreateDto { Nombre = "Nuevo", Descripcion = "Desc", FechaInicio = System.DateTime.Now, FechaFin = System.DateTime.Now.AddDays(1) };
            _mockRepo.Setup(r => r.Add(It.IsAny<Evento>())).Returns(10);

            // Act
            var resultado = _service.AgregarEvento(dto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.Is<Evento>(e => e.Nombre == "Nuevo")), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarEvento_DeberiaLlamarRepositorioYRetornarUno()
        {
            // Arrange
            var dto = new EventoUpdateDto { Nombre = "Update", Descripcion = "Desc", FechaInicio = System.DateTime.Now, FechaFin = System.DateTime.Now.AddDays(1), Estado = EstadoEvento.Publicado };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Evento>())).Returns(1);

            // Act
            var resultado = _service.ActualizarEvento(1, dto);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Evento>(e => e.Nombre == "Update")), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarEvento_DeberiaLlamarRepositorioYRetornarUno()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarEvento(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void Publicar_Existe_DeberiaCambiarEstadoYRetornarUno()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Estado = EstadoEvento.Inactivo };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Evento>())).Returns(1);

            // Act
            var resultado = _service.Publicar(1);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Evento>(e => e.Estado == EstadoEvento.Publicado)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void Cancelar_Existe_DeberiaCambiarEstadoYRetornarUno()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Estado = EstadoEvento.Publicado };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Evento>())).Returns(1);

            // Act
            var resultado = _service.Cancelar(1);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Evento>(e => e.Estado == EstadoEvento.Cancelado)), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
