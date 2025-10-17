using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Tests.Services
{
    public class EntradaServiceTestsTest
    {
        private readonly Mock<IEntradaRepository> _mockRepo;
        private readonly EntradaService _service;

        public EntradaServiceTestsTest()
        {
            _mockRepo = new Mock<IEntradaRepository>();
            _service = new EntradaService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeEntradaDto()
        {
            // Arrange
            var entradas = new List<Entrada>
            {
                new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 10, IdTarifa = 5, Estado = EstadoEntrada.Activa },
                new Entrada { IdEntrada = 2, Precio = 200, IdOrden = 20, IdTarifa = 6, Estado = EstadoEntrada.Anulada }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(entradas);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, r => Assert.IsType<EntradaDto>(r));
            Assert.Equal(100, resultado.First().Precio);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarEntrada()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Precio = 150 };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(150, resultado.Precio);
        }

        [Fact]
        public void AgregarEntrada_DeberiaLlamarAddYRetornarId()
        {
            // Arrange
            var dto = new EntradaCreateDto
            {
                Precio = 120,
                IdOrden = 3,
                IdTarifa = 2,
                IdCliente = 1,
                IdFuncion = 4
            };

            _mockRepo.Setup(r => r.Add(It.IsAny<Entrada>())).Returns(1);

            // Act
            var resultado = _service.AgregarEntrada(dto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.IsAny<Entrada>()), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void ActualizarEntrada_DeberiaRetornar1CuandoExiste()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Precio = 100 };
            var updateDto = new EntradaUpdateDto
            {
                Precio = 200,
                IdOrden = 2,
                IdTarifa = 3,
                Estado = EstadoEntrada.Activa
            };

            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);
            _mockRepo.Setup(r => r.Update(It.IsAny<Entrada>())).Returns(1);

            // Act
            var resultado = _service.ActualizarEntrada(1, updateDto);

            // Assert
            Assert.Equal(1, resultado);
            _mockRepo.Verify(r => r.Update(It.Is<Entrada>(e => e.Precio == 200)), Times.Once);
        }

        [Fact]
        public void ActualizarEntrada_DeberiaRetornar0SiNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Entrada?)null);
            var dto = new EntradaUpdateDto();

            // Act
            var resultado = _service.ActualizarEntrada(99, dto);

            // Assert
            Assert.Equal(0, resultado);
            _mockRepo.Verify(r => r.Update(It.IsAny<Entrada>()), Times.Never);
        }

        [Fact]
        public void AnularEntrada_DeberiaCambiarEstadoYRetornarTrue()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Estado = EstadoEntrada.Activa };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);
            _mockRepo.Setup(r => r.Update(It.IsAny<Entrada>())).Returns(1);

            // Act
            var resultado = _service.AnularEntrada(1);

            // Assert
            Assert.True(resultado);
            Assert.Equal(EstadoEntrada.Anulada, entrada.Estado);
        }

        [Fact]
        public void AnularEntrada_DeberiaRetornarFalseSiNoExiste()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(5)).Returns((Entrada?)null);

            // Act
            var resultado = _service.AnularEntrada(5);

            // Assert
            Assert.False(resultado);
            _mockRepo.Verify(r => r.Update(It.IsAny<Entrada>()), Times.Never);
        }

        [Fact]
        public void EliminarEntrada_DeberiaRetornarResultadoDelRepositorio()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarEntrada(1);

            // Assert
            Assert.Equal(1, resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}


