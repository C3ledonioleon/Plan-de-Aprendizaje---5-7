using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.Services;
using sve.Services.Contracts;
using sve.Repositories.Contracts;
using sve.Models;
using sve.DTOs;
using System;

namespace sve.Tests.Services
{
    public class FuncionServiceTests
    {
        private readonly Mock<IFuncionRepository> _mockRepo;
        private readonly FuncionService _service;

        public FuncionServiceTests()
        {
            _mockRepo = new Mock<IFuncionRepository>();
            _service = new FuncionService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeFunciones()
        {
            // Arrange
            var funciones = new List<Funcion>
            {
                new Funcion { IdFuncion = 1, IdEvento = 1, IdLocal = 1, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente },
                new Funcion { IdFuncion = 2, IdEvento = 1, IdLocal = 2, FechaHora = DateTime.Now.AddHours(1), Estado = EstadoFuncion.Pendiente }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(funciones);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(1, resultado.First().IdEvento);
        }

        [Fact]
        public void ObtenerPorId_Existe_DeberiaRetornarFuncion()
        {
            // Arrange
            var funcion = new Funcion { IdFuncion = 1, IdEvento = 1, IdLocal = 1, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente };
            _mockRepo.Setup(r => r.GetById(1)).Returns(funcion);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdEvento);
        }

        [Fact]
        public void ObtenerPorId_NoExiste_DeberiaRetornarNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(1)).Returns((Funcion?)null);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarFuncion_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var dto = new FuncionCreateDto { IdEvento = 1, IdLocal = 2, FechaHora = DateTime.Now };
            _mockRepo.Setup(r => r.Add(It.IsAny<Funcion>())).Returns(10);

            // Act
            var resultado = _service.AgregarFuncion(dto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.Is<Funcion>(f => f.IdEvento == 1 && f.IdLocal == 2)), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarFuncion_DeberiaLlamarRepositorioYRetornarUno()
        {
            // Arrange
            var dto = new FuncionUpdateDto { IdEvento = 1, IdLocal = 2, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _service.ActualizarFuncion(1, dto);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Funcion>(f => f.IdEvento == 1 && f.IdLocal == 2)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarFuncion_DeberiaLlamarRepositorioYRetornarUno()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarFuncion(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void CancelarFuncion_Existe_DeberiaCambiarEstadoYRetornarUno()
        {
            // Arrange
            var funcion = new Funcion { IdFuncion = 1, IdEvento = 1, IdLocal = 1, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente };
            _mockRepo.Setup(r => r.GetById(1)).Returns(funcion);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _service.CancelarFuncion(1);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Funcion>(f => f.Estado == EstadoFuncion.Cancelada)), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
