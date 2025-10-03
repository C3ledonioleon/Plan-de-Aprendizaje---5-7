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
    public class FuncionServiceTests
    {
        private readonly Mock<IFuncionRepository> _mockRepo;
        private readonly FuncionService _funcionService;

        public FuncionServiceTests()
        {
            _mockRepo = new Mock<IFuncionRepository>();
            _funcionService = new FuncionService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaFuncionDto()
        {
            // Arrange
            var funciones = new List<Funcion>
            {
                new Funcion { IdFuncion = 1, IdEvento = 10, IdLocal = 100, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente },
                new Funcion { IdFuncion = 2, IdEvento = 20, IdLocal = 200, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(funciones);

            // Act
            var resultado = _funcionService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(10, resultado[0].IdEvento);
            Assert.Equal(20, resultado[1].IdEvento);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaFuncion()
        {
            // Arrange
            var funcion = new Funcion { IdFuncion = 1, IdEvento = 10, IdLocal = 100, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente };
            _mockRepo.Setup(r => r.GetById(1)).Returns(funcion);

            // Act
            var resultado = _funcionService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(10, resultado.IdEvento);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Funcion)null);

            // Act
            var resultado = _funcionService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarFuncion_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new FuncionCreateDto { IdEvento = 10, IdLocal = 100, FechaHora = DateTime.Now };
            _mockRepo.Setup(r => r.Add(It.IsAny<Funcion>())).Returns(1);

            // Act
            var idNuevo = _funcionService.AgregarFuncion(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Funcion>()), Times.Once);
        }

        [Fact]
        public void ActualizarFuncion_RetornaTrue()
        {
            // Arrange
            var updateDto = new FuncionUpdateDto { IdEvento = 10, IdLocal = 100, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Funcion>())).Returns(true);

            // Act
            var resultado = _funcionService.ActualizarFuncion(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Funcion>()), Times.Once);
        }

        [Fact]
        public void EliminarFuncion_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _funcionService.EliminarFuncion(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void CancelarFuncion_Existente_RetornaTrue()
        {
            // Arrange
            var funcion = new Funcion { IdFuncion = 1, IdEvento = 10, IdLocal = 100, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente };
            _mockRepo.Setup(r => r.GetById(1)).Returns(funcion);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Funcion>())).Returns(true);

            // Act
            var resultado = _funcionService.CancelarFuncion(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Funcion>()), Times.Once);
        }

        [Fact]
        public void CancelarFuncion_NoExistente_RetornaFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Funcion)null);

            // Act
            var resultado = _funcionService.CancelarFuncion(99);

            // Assert
            Assert.False(resultado);
        }
    }
}
