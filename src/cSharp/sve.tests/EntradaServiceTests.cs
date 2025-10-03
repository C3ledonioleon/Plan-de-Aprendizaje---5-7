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
    public class EntradaServiceTests
    {
        private readonly Mock<IEntradaRepository> _mockRepo;
        private readonly EntradaService _entradaService;

        public EntradaServiceTests()
        {
            _mockRepo = new Mock<IEntradaRepository>();
            _entradaService = new EntradaService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaEntradaDto()
        {
            // Arrange
            var entradas = new List<Entrada>
            {
                new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa },
                new Entrada { IdEntrada = 2, Precio = 200, IdOrden = 2, IdTarifa = 2, Estado = EstadoEntrada.Activa }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(entradas);

            // Act
            var resultado = _entradaService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(100, resultado[0].Precio);
            Assert.Equal(200, resultado[1].Precio);
        }

        [Fact]
        public void ObtenerPorId_EntradaExistente_RetornaEntrada()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);

            // Act
            var resultado = _entradaService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(100, resultado.Precio);
        }

        [Fact]
        public void ObtenerPorId_EntradaNoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Entrada)null);

            // Act
            var resultado = _entradaService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarEntrada_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new EntradaCreateDto { Precio = 150, IdOrden = 1, IdTarifa = 1, IdCliente = 1, IdFuncion = 1 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Entrada>())).Returns(1);

            // Act
            var idNuevo = _entradaService.AgregarEntrada(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Entrada>()), Times.Once);
        }

        [Fact]
        public void ActualizarEntrada_EntradaExistente_RetornaTrue()
        {
            // Arrange
            var existente = new Entrada { IdEntrada = 1, Precio = 100, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa };
            var updateDto = new EntradaUpdateDto { Precio = 200, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existente);
            _mockRepo.Setup(r => r.Update(1, existente)).Returns(true);

            // Act
            var resultado = _entradaService.ActualizarEntrada(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Entrada>()), Times.Once);
        }

        [Fact]
        public void ActualizarEntrada_EntradaNoExistente_RetornaFalse()
        {
            // Arrange
            var updateDto = new EntradaUpdateDto { Precio = 200, IdOrden = 1, IdTarifa = 1, Estado = EstadoEntrada.Activa };
            _mockRepo.Setup(r => r.GetById(99)).Returns((Entrada)null);

            // Act
            var resultado = _entradaService.ActualizarEntrada(99, updateDto);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void AnularEntrada_EntradaExistente_RetornaTrue()
        {
            // Arrange
            var entrada = new Entrada { IdEntrada = 1, Estado = EstadoEntrada.Activa };
            _mockRepo.Setup(r => r.GetById(1)).Returns(entrada);
            _mockRepo.Setup(r => r.Update(1, entrada)).Returns(true);

            // Act
            var resultado = _entradaService.AnularEntrada(1);

            // Assert
            Assert.True(resultado);
            Assert.Equal(EstadoEntrada.Anulada, entrada.Estado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Entrada>()), Times.Once);
        }

        [Fact]
        public void AnularEntrada_EntradaNoExistente_RetornaFalse()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Entrada)null);

            // Act
            var resultado = _entradaService.AnularEntrada(99);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void EliminarEntrada_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _entradaService.EliminarEntrada(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
