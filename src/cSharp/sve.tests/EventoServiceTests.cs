using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using sve.Models;
using sve.DTOs;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Tests
{
    public class EventoServiceTests
    {
        private readonly Mock<IEventoRepository> _mockRepo;
        private readonly EventoService _eventoService;

        public EventoServiceTests()
        {
            _mockRepo = new Mock<IEventoRepository>();
            _eventoService = new EventoService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaEventoDto()
        {
            // Arrange
            var eventos = new List<Evento>
            {
                new Evento { IdEvento = 1, Nombre = "Evento1", Descripcion = "Desc1", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1), Estado = EstadoEvento.Inactivo },
                new Evento { IdEvento = 2, Nombre = "Evento2", Descripcion = "Desc2", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(2), Estado = EstadoEvento.Inactivo }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(eventos);

            // Act
            var resultado = _eventoService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Evento1", resultado[0].Nombre);
            Assert.Equal("Evento2", resultado[1].Nombre);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaEvento()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Nombre = "Evento1", Descripcion = "Desc1", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1), Estado = EstadoEvento.Inactivo };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);

            // Act
            var resultado = _eventoService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Evento1", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Evento)null);

            // Act
            var resultado = _eventoService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarEvento_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new EventoCreateDto { Nombre = "NuevoEvento", Descripcion = "Desc", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1) };
            _mockRepo.Setup(r => r.Add(It.IsAny<Evento>())).Returns(1);

            // Act
            var idNuevo = _eventoService.AgregarEvento(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Evento>()), Times.Once);
        }

        [Fact]
        public void ActualizarEvento_RetornaTrue()
        {
            // Arrange
            var updateDto = new EventoUpdateDto { Nombre = "Actualizado", Descripcion = "Desc", FechaInicio = DateTime.Now, FechaFin = DateTime.Now.AddDays(1), Estado = EstadoEvento.Inactivo };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Evento>())).Returns(true);

            // Act
            var resultado = _eventoService.ActualizarEvento(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Evento>()), Times.Once);
        }

        [Fact]
        public void EliminarEvento_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _eventoService.EliminarEvento(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void PublicarEvento_RetornaTrue()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Estado = EstadoEvento.Inactivo };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);
            _mockRepo.Setup(r => r.Update(1, evento)).Returns(true);

            // Act
            var resultado = _eventoService.Publicar(1);

            // Assert
            Assert.True(resultado);
            Assert.Equal(EstadoEvento.Publicado, evento.Estado);
        }

        [Fact]
        public void CancelarEvento_RetornaTrue()
        {
            // Arrange
            var evento = new Evento { IdEvento = 1, Estado = EstadoEvento.Publicado };
            _mockRepo.Setup(r => r.GetById(1)).Returns(evento);
            _mockRepo.Setup(r => r.Update(1, evento)).Returns(true);

            // Act
            var resultado = _eventoService.Cancelar(1);

            // Assert
            Assert.True(resultado);
            Assert.Equal(EstadoEvento.Cancelado, evento.Estado);
        }
    }
}
