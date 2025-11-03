using Moq;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveServices.Services;
using sveCore.DTOs;

using Xunit;

namespace sve.Tests.Services
{
    public class SectorServiceTests
    {
        private readonly Mock<ISectorRepository> _sectorRepositoryMock;
        private readonly SectorService _sectorService;

        public SectorServiceTests()
        {
            _sectorRepositoryMock = new Mock<ISectorRepository>();
            _sectorService = new SectorService(_sectorRepositoryMock.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeSectorDto()
        {
            // Arrange
            var sectores = new List<Sector>
            {
                new Sector { IdSector = 1, Nombre = "Platea Baja", Capacidad = 200, IdLocal = 10 },
                new Sector { IdSector = 2, Nombre = "Palco", Capacidad = 100, IdLocal = 10 }
            };

            _sectorRepositoryMock.Setup(r => r.GetAll()).Returns(sectores);

            // Act
            var resultado = _sectorService.ObtenerTodo();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, s => Assert.IsType<SectorDto>(s));
            _sectorRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarSectorSiExiste()
        {
            // Arrange
            var sector = new Sector { IdSector = 1, Nombre = "Platea Alta" };
            _sectorRepositoryMock.Setup(r => r.GetById(1)).Returns(sector);

            // Act
            var resultado = _sectorService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado!.IdSector);
            Assert.Equal("Platea Alta", resultado.Nombre);
            _sectorRepositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void AgregarSector_DeberiaAgregarYRetornarId()
        {
            // Arrange
            var dto = new SectorCreateDto
            {
                Nombre = "Nuevo Sector",
                Capacidad = 150,
                IdLocal = 5
            };

            _sectorRepositoryMock.Setup(r => r.Add(It.IsAny<Sector>())).Returns(1);

            // Act
            var resultado = _sectorService.AgregarSector(dto);

            // Assert
            Assert.Equal(1, resultado);
            _sectorRepositoryMock.Verify(r => r.Add(It.Is<Sector>(s =>
                s.Nombre == dto.Nombre &&
                s.Capacidad == dto.Capacidad &&
                s.IdLocal == dto.IdLocal
            )), Times.Once);
        }

        [Fact]
        public void ActualizarSector_DeberiaActualizarYRetornarResultado()
        {
            // Arrange
            var dto = new SectorUpdateDto
            {
                Nombre = "Sector Actualizado",
                Capacidad = 300,
                IdLocal = 8
            };

            _sectorRepositoryMock.Setup(r => r.Update(It.IsAny<Sector>())).Returns(1);

            // Act
            var resultado = _sectorService.ActualizarSector(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _sectorRepositoryMock.Verify(r => r.Update(It.Is<Sector>(s =>
                s.IdSector == 1 &&
                s.Nombre == dto.Nombre &&
                s.Capacidad == dto.Capacidad &&
                s.IdLocal == dto.IdLocal
            )), Times.Once);
        }

        [Fact]
        public void EliminarSector_DeberiaEliminarYRetornarResultado()
        {
            // Arrange
            _sectorRepositoryMock.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _sectorService.EliminarSector(1);

            // Assert
            Assert.Equal(1, resultado);
            _sectorRepositoryMock.Verify(r => r.Delete(1), Times.Once);
        }
    }
}


