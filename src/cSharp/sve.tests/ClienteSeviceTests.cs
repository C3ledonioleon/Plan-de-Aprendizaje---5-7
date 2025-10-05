using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.Services;
using sve.Services.Contracts;
using sve.Repositories.Contracts;
using sve.Models;
using sve.DTOs;

namespace sve.Tests
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly ClienteService _service;

        public ClienteServiceTests()
        {
            _mockRepo = new Mock<IClienteRepository>();
            _service = new ClienteService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeClientes()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "1111" },
                new Cliente { IdCliente = 2, DNI = "456", Nombre = "Maria", Telefono = "2222" }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(clientes);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Juan", resultado.First().Nombre);
        }

        [Fact]
        public void ObtenerPorId_ClienteExiste_DeberiaRetornarCliente()
        {
            // Arrange
            var cliente = new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "1111" };
            _mockRepo.Setup(r => r.GetById(1)).Returns(cliente);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Juan", resultado.Nombre);
        }

        [Fact]
        public void AgregarCliente_DeberiaLlamarAlRepositorioYRetornarId()
        {
            // Arrange
            var dto = new ClienteCreateDto { DNI = "99999999", Nombre = "Carlos", Telefono = "1132302" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Cliente>())).Returns(10);

            // Act
            var resultado = _service.AgregarCliente(dto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.Is<Cliente>(c => c.Nombre == "Carlos")), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarCliente_ClienteExiste_DeberiaActualizarYRetornarUno()
        {
            // Arrange
            var existente = new Cliente { IdCliente = 1, DNI = "111", Nombre = "Juan", Telefono = "5555" };
            var dto = new ClienteUpdateDto { DNI = "222", Nombre = "Pedro", Telefono = "6666" };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existente);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Cliente>())).Returns(1);

            // Act
            var resultado = _service.ActualizarCliente(1, dto);

            // Assert
            _mockRepo.Verify(r => r.Update(1, It.Is<Cliente>(c => c.Nombre == "Pedro")), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void ActualizarCliente_ClienteNoExiste_DeberiaRetornarCero()
        {
            // Arrange
            var dto = new ClienteUpdateDto { DNI = "999", Nombre = "Nadie", Telefono = "0000" };
            _mockRepo.Setup(r => r.GetById(1)).Returns((Cliente?)null);

            // Act
            var resultado = _service.ActualizarCliente(1, dto);

            // Assert
            Assert.Equal(0, resultado);
        }

        [Fact]
        public void EliminarCliente_DeberiaLlamarRepositorioYRetornarUno()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarCliente(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
