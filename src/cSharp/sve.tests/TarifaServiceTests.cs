using Moq;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveServices.Services;
using sveCore.DTOs;

using Xunit;

namespace sve.Tests.Services
{
    public class TarifaServiceTests
    {
        private readonly Mock<ITarifaRepository> _tarifaRepositoryMock;
        private readonly TarifaService _tarifaService;

        public TarifaServiceTests()
        {
            _tarifaRepositoryMock = new Mock<ITarifaRepository>();
            _tarifaService = new TarifaService(_tarifaRepositoryMock.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeTarifaDto()
        {
            // Arrange
            var tarifas = new List<Tarifa>
            {
                new Tarifa { IdTarifa = 1, IdFuncion = 10, IdSector = 20, Precio = 500, Stock = 100, Estado = EstadoTarifa.Activa },
                new Tarifa { IdTarifa = 2, IdFuncion = 11, IdSector = 21, Precio = 800, Stock = 50, Estado = EstadoTarifa.Inactiva }
            };

            _tarifaRepositoryMock.Setup(r => r.GetAll()).Returns(tarifas);

            // Act
            var resultado = _tarifaService.ObtenerTodo();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, t => Assert.IsType<TarifaDto>(t));
            _tarifaRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarTarifaDtoSiExiste()
        {
            // Arrange
            var tarifa = new Tarifa { IdTarifa = 1, IdFuncion = 2, IdSector = 3, Precio = 1000, Stock = 20, Estado = EstadoTarifa.Activa };
            _tarifaRepositoryMock.Setup(r => r.GetById(1)).Returns(tarifa);

            // Act
            var resultado = _tarifaService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado!.IdTarifa);
            Assert.Equal(1000, resultado.Precio);
            _tarifaRepositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarNullSiNoExiste()
        {
            // Arrange
            _tarifaRepositoryMock.Setup(r => r.GetById(1)).Returns((Tarifa?)null);

            // Act
            var resultado = _tarifaService.ObtenerPorId(1);

            // Assert
            Assert.Null(resultado);
            _tarifaRepositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void AgregarTarifa_DeberiaAgregarYRetornarId()
        {
            // Arrange
            var dto = new TarifaCreateDto
            {
                IdFuncion = 1,
                IdSector = 2,
                Precio = 700,
                Stock = 80,
                Estado = EstadoTarifa.Activa
            };

            _tarifaRepositoryMock.Setup(r => r.Add(It.IsAny<Tarifa>())).Returns(1);

            // Act
            var resultado = _tarifaService.AgregarTarifa(dto);

            // Assert
            Assert.Equal(1, resultado);
            _tarifaRepositoryMock.Verify(r => r.Add(It.Is<Tarifa>(t =>
                t.IdFuncion == dto.IdFuncion &&
                t.IdSector == dto.IdSector &&
                t.Precio == dto.Precio &&
                t.Stock == dto.Stock &&
                t.Estado == dto.Estado
            )), Times.Once);
        }

        [Fact]
        public void ActualizarTarifa_DeberiaActualizarYRetornarResultado()
        {
            // Arrange
            var dto = new TarifaUpdateDto
            {
                Precio = 1200,
                Stock = 60,
                Estado = EstadoTarifa.Inactiva
            };

            _tarifaRepositoryMock.Setup(r => r.Update(It.IsAny<Tarifa>())).Returns(1);

            // Act
            var resultado = _tarifaService.ActualizarTarifa(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _tarifaRepositoryMock.Verify(r => r.Update(It.Is<Tarifa>(t =>
                t.Precio == dto.Precio &&
                t.Stock == dto.Stock &&
                t.Estado == dto.Estado
            )), Times.Once);
        }

        [Fact]
        public void EliminarTarifa_DeberiaEliminarYRetornarResultado()
        {
            // Arrange
            _tarifaRepositoryMock.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _tarifaService.EliminarTarifa(1);

            // Assert
            Assert.Equal(1, resultado);
            _tarifaRepositoryMock.Verify(r => r.Delete(1), Times.Once);
        }
    }
}


