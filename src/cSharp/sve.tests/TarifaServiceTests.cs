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
    public class TarifaServiceTests
    {
        private readonly Mock<ITarifaRepository> _mockRepo;
        private readonly TarifaService _tarifaService;

        public TarifaServiceTests()
        {
            _mockRepo = new Mock<ITarifaRepository>();
            _tarifaService = new TarifaService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaTarifaDto()
        {
            // Arrange
            var tarifas = new List<Tarifa>
            {
               new Tarifa { IdTarifa = 1, Precio = 100, Estado = EstadoTarifa.Activa },
               new Tarifa { IdTarifa = 2, Precio = 150, Estado = EstadoTarifa.Inactiva } // <- corregido
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(tarifas);

            // Act
            var resultado = _tarifaService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(100, resultado[0].Precio);
            Assert.Equal(150, resultado[1].Precio);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaTarifa()
        {
            // Arrange
            var tarifa = new Tarifa
            {
                IdTarifa = 1,
                IdFuncion = 1,
                IdSector = 1,
                Precio = 100,
                Stock = 50,
                Estado = EstadoTarifa.Activa
            };
            _mockRepo.Setup(r => r.GetById(1)).Returns(tarifa);

            // Act
            var resultado = _tarifaService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdTarifa);
            Assert.Equal(100, resultado.Precio);
            Assert.Equal(EstadoTarifa.Activa, resultado.Estado);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Tarifa)null);

            // Act
            var resultado = _tarifaService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarTarifa_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new TarifaCreateDto
            {
                IdFuncion = 1,
                IdSector = 1,
                Precio = 100,
                Stock = 50,
                Estado = EstadoTarifa.Activa
            };
            _mockRepo.Setup(r => r.Add(It.IsAny<Tarifa>())).Returns(1);

            // Act
            var idNuevo = _tarifaService.AgregarTarifa(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Tarifa>()), Times.Once);
        }

        [Fact]
        public void ActualizarTarifa_RetornaTrue()
        {
            // Arrange
            var tarifaExistente = new Tarifa
            {
                IdTarifa = 1,
                Precio = 100,
                Stock = 50,
                Estado = EstadoTarifa.Activa
            };
            var updateDto = new TarifaUpdateDto
            {
                Precio = 200,
                Stock = 100,
                Estado = EstadoTarifa.Inactiva
            };

            _mockRepo.Setup(r => r.GetById(1)).Returns(tarifaExistente);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Tarifa>())).Returns(true);

            // Act
            var resultado = _tarifaService.ActualizarTarifa(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Tarifa>()), Times.Once);
        }

        [Fact]
        public void EliminarTarifa_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _tarifaService.EliminarTarifa(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
