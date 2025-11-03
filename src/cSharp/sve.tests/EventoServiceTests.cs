using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveServices.Services;
using sveCore.DTOs;

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
        public void ObtenerTodo_DeberiaRetornarListaDeEventoDto()
        {
            // Arrange
            var eventos = new List<Evento>
            {
                new Evento { IdEvento = 1, Nombre = "Festival", Descripcion = "Música", FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(1), Estado = EstadoEvento.Publicado },
                new Evento { IdEvento = 2, Nombre = "Conferencia", Descripcion = "Tech", FechaInicio = DateTime.Today, FechaFin = DateTime.Today.AddDays(2), Estado = EstadoEvento.Inactivo }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(eventos);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, r => Assert.IsType<EventoDto>(r));
            Assert.Equal("Festival", resultado.First().Nombre);
            Assert.Equal(EstadoEvento.Publicado, resultado.First().Estado);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarEventoCuandoExiste()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Nombre = "Recital" };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Recital", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarNullCuandoNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Evento?)null);

            // Act
            var resultado = _service.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarEvento_DeberiaLlamarAddYRetornarId()
        {
            // Arrange
            var dto = new EventoCreateDto
            {
                Nombre = "Nuevo Evento",
                Descripcion = "Descripción",
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(1)
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<Evento>())).Returns(5);

            // Act
            var resultado = _service.AgregarEvento(dto);

            // Assert
            Assert.Equal(5, resultado);
            _mockRepo.Verify(r => r.Add(It.Is<Evento>(e =>
                e.Nombre == dto.Nombre &&
                e.Descripcion == dto.Descripcion &&
                e.Estado == EstadoEvento.Inactivo
            )), Times.Once);
        }

        [Fact]
        public void ActualizarEvento_DeberiaLlamarUpdateYRetornarResultado()
        {
            // Arrange
            var dto = new EventoUpdateDto
            {
                Nombre = "Actualizado",
                Descripcion = "Nueva desc",
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(2),
                Estado = EstadoEvento.Publicado
            };
            _mockRepo.Setup(r => r.Update(It.IsAny<Evento>())).Returns(1);

            // Act
            var resultado = _service.ActualizarEvento(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _mockRepo.Verify(r => r.Update(It.Is<Evento>(e =>
                e.IdEvento == 1 &&
                e.Nombre == "Actualizado" &&
                e.Estado == EstadoEvento.Publicado
            )), Times.Once);
        }

        [Fact]
        public void EliminarEvento_DeberiaLlamarDeleteYRetornarUno()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarEvento(1);

            // Assert
            Assert.Equal(1, resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void Publicar_DeberiaCambiarEstadoAPublicadoYActualizar()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Estado = EstadoEvento.Inactivo };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);
            _mockRepo.Setup(r => r.Update(It.IsAny<Evento>())).Returns(1);

            // Act
            var resultado = _service.Publicar(1);

            // Assert
            Assert.Equal(1, resultado);
            Assert.Equal(EstadoEvento.Publicado, evento.Estado);
            _mockRepo.Verify(r => r.Update(It.Is<Evento>(e => e.Estado == EstadoEvento.Publicado)), Times.Once);
        }

        [Fact]
        public void Publicar_DeberiaRetornarCeroSiEventoNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Evento?)null);

            // Act
            var resultado = _service.Publicar(99);

            // Assert
            Assert.Equal(0, resultado);
            _mockRepo.Verify(r => r.Update(It.IsAny<Evento>()), Times.Never);
        }

        [Fact]
        public void Cancelar_DeberiaCambiarEstadoACanceladoYActualizar()
        {
            // Arrange
            var evento = new Evento { IdEvento = 2, Estado = EstadoEvento.Publicado };
            _mockRepo.Setup(r => r.GetById(2)).Returns(evento);
            _mockRepo.Setup(r => r.Update(It.IsAny<Evento>())).Returns(1);

            // Act
            var resultado = _service.Cancelar(2);

            // Assert
            Assert.Equal(1, resultado);
            Assert.Equal(EstadoEvento.Cancelado, evento.Estado);
            _mockRepo.Verify(r => r.Update(It.Is<Evento>(e => e.Estado == EstadoEvento.Cancelado)), Times.Once);
        }

        [Fact]
        public void Cancelar_DeberiaRetornarCeroSiEventoNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(5)).Returns((Evento?)null);

            // Act
            var resultado = _service.Cancelar(5);

            // Assert
            Assert.Equal(0, resultado);
            _mockRepo.Verify(r => r.Update(It.IsAny<Evento>()), Times.Never);
        }
    }
}


