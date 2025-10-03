using Moq;
using Xunit;
using sve.Models;
using sve.DTOs;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve_tests
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
        public void ObtenerTodo_RetornaListaClientesDto()
        {
            var clientes = new List<Cliente>
            {
                new Cliente { IdCliente = 1, DNI = "123", Nombre = "Juan", Telefono = "111" },
                new Cliente { IdCliente = 2, DNI = "456", Nombre = "Ana", Telefono = "222" }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(clientes);

            var resultado = _service.ObtenerTodo();

            Assert.Equal(2, resultado.Count);
        }
    }
}
