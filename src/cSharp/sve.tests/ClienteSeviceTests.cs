using System.Collections.Generic;
using Moq;
using Xunit;
using sve.Models;
using sve.DTOs;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Tests
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _mockRepo = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaClientesDto()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "111" },
                new Cliente { IdCliente = 2, DNI = "456", Nombre = "Ana", Telefono = "222" }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(clientes);

            // Act
            var resultado = _clienteService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Juan", resultado[0].Nombre);
            Assert.Equal("Ana", resultado[1].Nombre);
        }

        [Fact]
        public void ObtenerPorId_ClienteExistente_RetornaCliente()
        {
            // Arrange
            var cliente = new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "111" };
            _mockRepo.Setup(r => r.GetById(1)).Returns(cliente);

            // Act
            var resultado = _clienteService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Juan", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_ClienteNoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Cliente)null);

            // Act
            var resultado = _clienteService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarCliente_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new ClienteCreateDto { DNI = "123", Nombre = "Juan", Telefono = "111" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Cliente>())).Returns(1);

            // Act
            var idNuevo = _clienteService.AgregarCliente(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public void ActualizarCliente_ClienteExistente_RetornaTrue()
        {
            // Arrange
            var clienteExistente = new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "111" };
            var updateDto = new ClienteUpdateDto { DNI = "999", Nombre = "Juan Actualizado", Telefono = "555" };

            _mockRepo.Setup(r => r.GetById(1)).Returns(clienteExistente);
            _mockRepo.Setup(r => r.Update(1, clienteExistente)).Returns(true);

            // Act
            var resultado = _clienteService.ActualizarCliente(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public void ActualizarCliente_ClienteNoExistente_RetornaFalse()
        {
            // Arrange
            var updateDto = new ClienteUpdateDto { DNI = "999", Nombre = "Juan Actualizado", Telefono = "555" };
            _mockRepo.Setup(r => r.GetById(99)).Returns((Cliente)null);

            // Act
            var resultado = _clienteService.ActualizarCliente(99, updateDto);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public void EliminarCliente_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _clienteService.EliminarCliente(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
