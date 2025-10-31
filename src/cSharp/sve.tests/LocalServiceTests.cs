using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;
using Xunit;

namespace sve.Tests.Services
{
    public class LocalServiceTests
    {
        private readonly Mock<ILocalRepository> _localRepositoryMock;
        private readonly LocalService _localService;

        public LocalServiceTests()
        {
            _localRepositoryMock = new Mock<ILocalRepository>();
            _localService = new LocalService(_localRepositoryMock.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeLocalesDto()
        {
            // Arrange
            var locales = new List<Local>
            {
                new Local { IdLocal = 1, Nombre = "Teatro Central", Direccion = "Calle 123", CapacidadTotal = 200 },
                new Local { IdLocal = 2, Nombre = "Cine Plaza", Direccion = "Av. 45", CapacidadTotal = 300 }
            };

            _localRepositoryMock.Setup(r => r.GetAll()).Returns(locales);

            // Act
            var resultado = _localService.ObtenerTodo();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, item => Assert.IsType<LocalDto>(item));
            _localRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarLocalSiExiste()
        {
            // Arrange
            var local = new Local { IdLocal = 1, Nombre = "Teatro Central" };
            _localRepositoryMock.Setup(r => r.GetById(1)).Returns(local);

            // Act
            var resultado = _localService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado!.IdLocal);
            Assert.Equal("Teatro Central", resultado.Nombre);
            _localRepositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void AgregarLocal_DeberiaAgregarYRetornarId()
        {
            // Arrange
            var dto = new LocalCreateDto
            {
                Nombre = "Nuevo Teatro",
                Direccion = "Calle Nueva 456",
                CapacidadTotal = 250
            };

            _localRepositoryMock.Setup(r => r.Add(It.IsAny<Local>())).Returns(1);

            // Act
            var resultado = _localService.AgregarLocal(dto);

            // Assert
            Assert.Equal(1, resultado);
            _localRepositoryMock.Verify(r => r.Add(It.Is<Local>(l =>
                l.Nombre == dto.Nombre &&
                l.Direccion == dto.Direccion &&
                l.CapacidadTotal == dto.CapacidadTotal
            )), Times.Once);
        }

        [Fact]
        public void ActualizarLocal_DeberiaActualizarYRetornarResultado()
        {
            // Arrange
            var dto = new LocalUpdateDto
            {
                Nombre = "Teatro Actualizado",
                Direccion = "Av. Principal 999",
                CapacidadTotal = 400
            };

            _localRepositoryMock.Setup(r => r.Update(It.IsAny<Local>())).Returns(1);

            // Act
            var resultado = _localService.ActualizarLocal(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _localRepositoryMock.Verify(r => r.Update(It.Is<Local>(l =>
                l.IdLocal == 1 &&
                l.Nombre == dto.Nombre &&
                l.Direccion == dto.Direccion &&
                l.CapacidadTotal == dto.CapacidadTotal
            )), Times.Once);
        }

        [Fact]
        public void EliminarLocal_DeberiaEliminarYRetornarResultado()
        {
            // Arrange
            _localRepositoryMock.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _localService.EliminarLocal(1);

            // Assert
            Assert.Equal(1, resultado);
            _localRepositoryMock.Verify(r => r.Delete(1), Times.Once);
        }
    }
}


