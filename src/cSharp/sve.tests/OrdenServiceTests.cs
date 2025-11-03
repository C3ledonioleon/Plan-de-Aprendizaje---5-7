using Xunit;
using Moq;

using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveServices.Services;
using sveCore.DTOs;

namespace sve.Tests.Services
{
    public class OrdenServiceTests
    {
        private readonly Mock<IOrdenRepository> _ordenRepositoryMock;
        private readonly OrdenService _ordenService;

        public OrdenServiceTests()
        {
            _ordenRepositoryMock = new Mock<IOrdenRepository>();
            _ordenService = new OrdenService(_ordenRepositoryMock.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeOrdenesDto()
        {
            // Arrange
            var ordenes = new List<Orden>
            {
                new Orden { IdOrden = 1, IdCliente = 2, IdTarifa = 3, Total = 100, Fecha = DateTime.Now, Estado = EstadoOrden.Creada },
                new Orden { IdOrden = 2, IdCliente = 3, IdTarifa = 4, Total = 200, Fecha = DateTime.Now.AddDays(1), Estado = EstadoOrden.Pagada }
            };

            _ordenRepositoryMock.Setup(r => r.GetAll()).Returns(ordenes);

            // Act
            var resultado = _ordenService.ObtenerTodo();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, o => Assert.IsType<OrdenDto>(o));
            _ordenRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarOrdenSiExiste()
        {
            // Arrange
            var orden = new Orden { IdOrden = 1, Total = 500 };
            _ordenRepositoryMock.Setup(r => r.GetById(1)).Returns(orden);

            // Act
            var resultado = _ordenService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado!.IdOrden);
            Assert.Equal(500, resultado.Total);
            _ordenRepositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void AgregarOrden_DeberiaAgregarYRetornarId()
        {
            // Arrange
            var dto = new OrdenCreateDto
            {
                IdCliente = 2,
                IdTarifa = 5,
                Total = 150,
                Fecha = DateTime.Now
            };

            _ordenRepositoryMock.Setup(r => r.Add(It.IsAny<Orden>())).Returns(1);

            // Act
            var resultado = _ordenService.AgregarOrden(dto);

            // Assert
            Assert.Equal(1, resultado);
            _ordenRepositoryMock.Verify(r => r.Add(It.Is<Orden>(o =>
                o.IdCliente == dto.IdCliente &&
                o.IdTarifa == dto.IdTarifa &&
                o.Estado == EstadoOrden.Creada
            )), Times.Once);
        }

        [Fact]
        public void ActualizarOrden_DeberiaActualizarYRetornarResultado()
        {
            // Arrange
            var dto = new OrdenUpdateDto
            {
                IdCliente = 3,
                IdTarifa = 7,
                Total = 200,
                Fecha = DateTime.Now,
                Estado = EstadoOrden.Pagada
            };

            _ordenRepositoryMock.Setup(r => r.Update(It.IsAny<Orden>())).Returns(1);

            // Act
            var resultado = _ordenService.ActualizarOrden(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _ordenRepositoryMock.Verify(r => r.Update(It.Is<Orden>(o =>
                o.IdOrden == 1 &&
                o.Estado == EstadoOrden.Pagada
            )), Times.Once);
        }

        [Fact]
        public void EliminarOrden_DeberiaEliminarYRetornarResultado()
        {
            // Arrange
            _ordenRepositoryMock.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _ordenService.EliminarOrden(1);

            // Assert
            Assert.Equal(1, resultado);
            _ordenRepositoryMock.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void CancelarOrden_DeberiaActualizarEstadoACancelada()
        {
            // Arrange
            var orden = new Orden { IdOrden = 1, Estado = EstadoOrden.Creada };
            _ordenRepositoryMock.Setup(r => r.GetById(1)).Returns(orden);
            _ordenRepositoryMock.Setup(r => r.Update(It.IsAny<Orden>())).Returns(1);

            // Act
            var resultado = _ordenService.CancelarOrden(1);

            // Assert
            Assert.True(resultado);
            _ordenRepositoryMock.Verify(r => r.Update(It.Is<Orden>(o => o.Estado == EstadoOrden.Cancelada)), Times.Once);
        }

        [Fact]
        public void CancelarOrden_DeberiaRetornarFalseSiNoExiste()
        {
            // Arrange
            _ordenRepositoryMock.Setup(r => r.GetById(1)).Returns((Orden?)null);

            // Act
            var resultado = _ordenService.CancelarOrden(1);

            // Assert
            Assert.False(resultado);
            _ordenRepositoryMock.Verify(r => r.Update(It.IsAny<Orden>()), Times.Never);
        }

        [Fact]
        public void PagarOrden_DeberiaActualizarEstadoAPagada()
        {
            // Arrange
            var orden = new Orden { IdOrden = 1, Estado = EstadoOrden.Creada };
            _ordenRepositoryMock.Setup(r => r.GetById(1)).Returns(orden);
            _ordenRepositoryMock.Setup(r => r.Update(It.IsAny<Orden>())).Returns(1);

            // Act
            var resultado = _ordenService.PagarOrden(1);

            // Assert
            Assert.True(resultado);
            _ordenRepositoryMock.Verify(r => r.Update(It.Is<Orden>(o => o.Estado == EstadoOrden.Pagada)), Times.Once);
        }

        [Fact]
        public void PagarOrden_DeberiaRetornarFalseSiNoExiste()
        {
            // Arrange
            _ordenRepositoryMock.Setup(r => r.GetById(1)).Returns((Orden?)null);

            // Act
            var resultado = _ordenService.PagarOrden(1);

            // Assert
            Assert.False(resultado);
            _ordenRepositoryMock.Verify(r => r.Update(It.IsAny<Orden>()), Times.Never);
        }
    }
}


