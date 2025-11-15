using Moq;
using sveCore.Servicio.IRepositories;
using sveServices.Services;
using Xunit;

namespace sveTest
{
    public class OrdenServiceTests
    {
        private readonly Mock<IOrdenRepository> _mockRepo;
        private readonly OrdenService _service;

        public OrdenServiceTests()
        {
            _mockRepo = new Mock<IOrdenRepository>();
            _service = new OrdenService(_mockRepo.Object);
        }

        [Fact]
        public void CancelarOrden_DeberiaRetornarTrue()
        {
            // Arrange
            int ordenId = 1;

            _mockRepo.Setup(r => r.CancelarOrden(ordenId))
                     .Returns(true);

            // Act
            var resultado = _service.CancelarOrden(ordenId);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.CancelarOrden(ordenId), Times.Once);
        }

        [Fact]
        public void PagarOrden_DeberiaRetornarTrue()
        {
            // Arrange
            int ordenId = 1;

            _mockRepo.Setup(r => r.PagarOrden(ordenId))
                     .Returns(true);

            // Act
            var resultado = _service.PagarOrden(ordenId);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.PagarOrden(ordenId), Times.Once);
        }
    }
}
