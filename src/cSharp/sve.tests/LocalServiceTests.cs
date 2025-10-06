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
    public class LocalServiceTests
    {
        private readonly Mock<ILocalRepository> _mockRepo;
        private readonly LocalService _service;

        public LocalServiceTests()
        {
            _mockRepo = new Mock<ILocalRepository>();
            _service = new LocalService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaDeLocalDto()
        {
            // Arrange
            var locales = new List<Local>
            {
                new Local { IdLocal = 1, Nombre = "Local1", Direccion = "Calle 1", CapacidadTotal = 100 },
                new Local { IdLocal = 2, Nombre = "Local2", Direccion = "Calle 2", CapacidadTotal = 200 }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(locales);

            // Act
            var resultado = _service.ObtenerTodo();

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
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Local1", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Local?)null);

            // Act
            var resultado = _service.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarLocal_RetornaIdNuevo()
        {
            // Arrange
            var createDto = new LocalCreateDto { Nombre = "LocalNuevo", Direccion = "Calle Nueva", CapacidadTotal = 150 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Local>())).Returns(10);

            // Act
            var idNuevo = _service.AgregarLocal(createDto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.IsAny<Local>()), Times.Once);
            Assert.Equal(10, idNuevo);
        }

        [Fact]
        public void ActualizarLocal_RetornaId()
        {
            // Arrange
            var updateDto = new LocalUpdateDto { Nombre = "LocalActualizado", Direccion = "Calle Actual", CapacidadTotal = 180 };
            _mockRepo.Setup(r => r.Update(It.IsAny<Local>())).Returns(1);

            // Act
            var resultado = _service.ActualizarLocal(1, updateDto);

            // Assert
            _mockRepo.Verify(r => r.Update(It.IsAny<Local>()), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarLocal_RetornaId()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarLocal(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
