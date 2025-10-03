using Moq;
using Xunit;
using sve.Models;
using sve.DTOs;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Tests
{
    public class OrdenServiceTests
    {
        private readonly Mock<IOrdenRepository> _mockRepo;
        private readonly OrdenService _ordenService;

        public OrdenServiceTests()
        {
            _mockRepo = new Mock<IOrdenRepository>();
            _ordenService = new OrdenService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaOrdenDto()
        {
            // Arrange
            var ordenes = new List<Orden>
            {
                new Orden { IdOrden = 1, IdCliente = 1, IdTarifa = 1, Total = 100, Fecha = System.DateTime.Now, Estado = EstadoOrden.Creada },
                new Orden { IdOrden = 2, IdCliente = 2, IdTarifa = 2, Total = 200, Fecha = System.DateTime.Now, Estado = EstadoOrden.Creada }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(ordenes);

            // Act
            var resultado = _ordenService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(1, resultado[0].IdCliente);
            Assert.Equal(2, resultado[1].IdCliente);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaOrden()
        {
            // Arrange
            var orden = new Orden { IdOrden = 1, IdCliente = 1, IdTarifa = 1, Total = 100, Fecha = System.DateTime.Now, Estado = EstadoOrden.Creada };
            _mockRepo.Setup(r => r.GetById(1)).Returns(orden);

            // Act
            var resultado = _ordenService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdCliente);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Orden)null);

            // Act
            var resultado = _ordenService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarOrden_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new OrdenCreateDto { IdCliente = 1, IdTarifa = 1, Total = 100, Fecha = System.DateTime.Now };
            _mockRepo.Setup(r => r.Add(It.IsAny<Orden>())).Returns(1);

            // Act
            var idNuevo = _ordenService.AgregarOrden(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Orden>()), Times.Once);
        }

        [Fact]
        public void ActualizarOrden_RetornaTrue()
        {
            // Arrange
            var updateDto = new OrdenUpdateDto { IdCliente = 1, IdTarifa = 1, Total = 150, Fecha = System.DateTime.Now, Estado = EstadoOrden.Creada };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Orden>())).Returns(true);

            // Act
            var resultado = _ordenService.ActualizarOrden(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Orden>()), Times.Once);
        }

        [Fact]
        public void EliminarOrden_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _ordenService.EliminarOrden(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
