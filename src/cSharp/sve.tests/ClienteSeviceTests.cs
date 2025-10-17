using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;
using sve.DTOs;

namespace sve.Tests.Services
{
    public class ClienteServiceTestsTest
    {
        private readonly Mock<IClienteRepository> _mockRepo;
        private readonly ClienteService _service;

        public ClienteServiceTestsTest()
        {
            _mockRepo = new Mock<IClienteRepository>();
            _service = new ClienteService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeClientes()
        {
            var clientes = new List<Cliente>
            {
                new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "1111" },
                new Cliente { IdCliente = 2, DNI = "456", Nombre = "Maria", Telefono = "2222" }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(clientes);

            var resultado = _service.ObtenerTodo();

            Assert.Equal(2, resultado.Count);
            Assert.Equal("Juan", resultado.First().Nombre);
        }

        [Fact]
        public void ObtenerPorId_ClienteExiste_DeberiaRetornarCliente()
        {
            var cliente = new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "1111" };
            _mockRepo.Setup(r => r.GetById(1)).Returns(cliente);

            var resultado = _service.ObtenerPorId(1);

            Assert.NotNull(resultado);
            Assert.Equal("Juan", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_ClienteNoExiste_DeberiaRetornarNull()
        {
            _mockRepo.Setup(r => r.GetById(999)).Returns((Cliente?)null);

            var resultado = _service.ObtenerPorId(999);

            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarCliente_DeberiaLlamarRepositorioYRetornarId()
        {
            var dto = new ClienteCreateDto { DNI = "999", Nombre = "Carlos", Telefono = "1132302" };
            _mockRepo.Setup(r => r.Add(It.IsAny<Cliente>())).Returns(10);

            var resultado = _service.AgregarCliente(dto);

            _mockRepo.Verify(r => r.Add(It.Is<Cliente>(c => c.Nombre == "Carlos")), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarCliente_ClienteExiste_DeberiaActualizarYRetornarUno()
        {
            var existente = new Cliente { IdCliente = 1, DNI = "111", Nombre = "Juan", Telefono = "5555" };
            var dto = new ClienteUpdateDto { DNI = "222", Nombre = "Pedro", Telefono = "6666" };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existente);
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Cliente>())).Returns(1);

            var resultado = _service.ActualizarCliente(1, dto);

            _mockRepo.Verify(r => r.Update(1, It.Is<Cliente>(c => c.Nombre == "Pedro" && c.DNI == "222")), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void ActualizarCliente_ClienteNoExiste_DeberiaRetornarCero()
        {
            var dto = new ClienteUpdateDto { DNI = "999", Nombre = "Nadie", Telefono = "0000" };
            _mockRepo.Setup(r => r.GetById(1)).Returns((Cliente?)null);

            var resultado = _service.ActualizarCliente(1, dto);

            Assert.Equal(0, resultado);
        }

        [Fact]
        public void EliminarCliente_DeberiaLlamarRepositorioYRetornarUno()
        {
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            var resultado = _service.EliminarCliente(1);

            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
