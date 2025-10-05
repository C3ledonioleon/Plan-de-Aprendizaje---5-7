using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.Services;
using sve.Repositories.Contracts;
using sve.Models;
using sve.DTOs;

namespace sve.Tests.Services
{
    public class TarifaServiceTests
    {
        private readonly Mock<ITarifaRepository> _mockRepo;
        private readonly TarifaService _service;

        public TarifaServiceTests()
        {
            _mockRepo = new Mock<ITarifaRepository>();
            _service = new TarifaService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaDeTarifaDto()
        {
            // Arrange
            var tarifas = new List<Tarifa>
            {
                new Tarifa { IdTarifa = 1, IdFuncion = 1, IdSector = 1, Precio = 100, Stock = 50, Estado = true },
                new Tarifa { IdTarifa = 2, IdFuncion = 2, IdSector = 2, Precio = 200, Stock = 30, Estado = true }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(tarifas);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(100, resultado[0].Precio);
            Assert.Equal(200, resultado[1].Precio);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaTarifaDto()
        {
            // Arrange
            var tarifa = new Tarifa { IdTarifa = 1, IdFuncion = 1, IdSector = 1, Precio = 150, Stock = 20, Estado = true };
            _mockRepo.Setup(r => r.GetById(1)).Returns(tarifa);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(150, resultado.Precio);
        }

        [Fact]
        public void ObtenerPorId_NoExiste_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Tarifa?)null);

            // Act
            var resultado = _service.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarTarifa_DeberiaRetornarIdNuevo()
        {
            // Arrange
            var createDto = new TarifaCreateDto { IdFuncion = 1, IdSector = 1, Precio = 200, Stock = 10, Estado = true };
            _mockRepo.Setup(r => r.Add(It.IsAny<Tarifa>())).Returns(5);

            // Act
            var resultado = _service.AgregarTarifa(createDto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.IsAny<Tarifa>()), Times.Once);
            Assert.Equal(5, resultado);
        }

        [Fact]
        public void ActualizarTarifa_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var updateDto = new TarifaUpdateDto { Precio = 250, Stock = 15, Estado = true };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Tarifa>())).Returns(1);

            // Act
            var resultado = _service.ActualizarTarifa(1, updateDto);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Tarifa>(t => t.Precio == 250 && t.Stock == 15)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarTarifa_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarTarifa(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
