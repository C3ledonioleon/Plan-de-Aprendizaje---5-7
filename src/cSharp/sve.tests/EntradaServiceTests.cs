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
    public class EntradaServiceTests
    {
        private readonly Mock<IEntradaRepository> _mockRepo;
        private readonly EntradaService _service;

        public EntradaServiceTests()
        {
            _mockRepo = new Mock<IEntradaRepository>();
            _service = new EntradaService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeEntradas()
        {
            // Arrange
            var entradas = new List<Entrada>
            {
                new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa },
                new Entrada { IdEntrada = 2, Precio = 150, IdOrden = 1, IdTarifa = 2, Estado = EstadoEntrada.Activa }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(entradas);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(100, resultado.First().Precio);
        }

        [Fact]
        public void ObtenerPorId_EntradaExiste_DeberiaRetornarEntrada()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(100, resultado.Precio);
        }

        [Fact]
        public void AgregarEntrada_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var dto = new EntradaCreateDto { Precio = 200, IdOrden = 1, IdTarifa = 2, IdCliente = 5, IdFuncion = 3 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Entrada>())).Returns(10);

            // Act
            var resultado = _service.AgregarEntrada(dto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.Is<Entrada>(e => e.Precio == 200)), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarEntrada_EntradaExiste_DeberiaActualizarYRetornarUno()
        {
            // Arrange
            var existente = new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa };
            var dto = new EntradaUpdateDto { Precio = 150, IdOrden = 2, IdTarifa = 2, Estado = EstadoEntrada.Activa };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existente);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Entrada>())).Returns(1);

            // Act
            var resultado = _service.ActualizarEntrada(1, dto);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Entrada>(e => e.Precio == 150 && e.IdOrden == 2)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void AnularEntrada_EntradaExiste_DeberiaCambiarEstadoYRetornarUno()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Estado = EstadoEntrada.Activa };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Entrada>())).Returns(1);

            // Act
            var resultado = _service.AnularEntrada(1);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Entrada>(e => e.Estado == EstadoEntrada.Anulada)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarEntrada_DeberiaLlamarRepositorioYRetornarUno()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarEntrada(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
