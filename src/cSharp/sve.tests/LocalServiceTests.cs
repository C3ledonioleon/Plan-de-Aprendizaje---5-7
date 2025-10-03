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
    public class LocalServiceTests
    {
        private readonly Mock<ILocalRepository> _mockRepo;
        private readonly LocalService _localService;

        public LocalServiceTests()
        {
            _mockRepo = new Mock<ILocalRepository>();
            _localService = new LocalService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaLocalDto()
        {
            // Arrange
            var locales = new List<Local>
            {
                new Local { IdLocal = 1, Nombre = "Local1", Direccion = "Calle 1", CapacidadTotal = 100 },
                new Local { IdLocal = 2, Nombre = "Local2", Direccion = "Calle 2", CapacidadTotal = 200 }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(locales);

            // Act
            var resultado = _localService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Local1", resultado[0].Nombre);
            Assert.Equal("Local2", resultado[1].Nombre);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaLocal()
        {
            // Arrange
            var local = new Local { IdLocal = 1, Nombre = "Local1", Direccion = "Calle 1", CapacidadTotal = 100 };
            _mockRepo.Setup(r => r.GetById(1)).Returns(local);

            // Act
            var resultado = _localService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Local1", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Local)null);

            // Act
            var resultado = _localService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarLocal_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new LocalCreateDto { Nombre = "Local1", Direccion = "Calle 1", CapacidadTotal = 100 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Local>())).Returns(1);

            // Act
            var idNuevo = _localService.AgregarLocal(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Local>()), Times.Once);
        }

        [Fact]
        public void ActualizarLocal_RetornaTrue()
        {
            // Arrange
            var updateDto = new LocalUpdateDto { Nombre = "Local1 Actualizado", Direccion = "Calle 1", CapacidadTotal = 120 };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Local>())).Returns(true);

            // Act
            var resultado = _localService.ActualizarLocal(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Local>()), Times.Once);
        }

        [Fact]
        public void EliminarLocal_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _localService.EliminarLocal(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
