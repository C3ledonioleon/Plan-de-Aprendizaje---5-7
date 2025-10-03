using Moq;
using Xunit;
using sve.Models;
using sve.DTOs;
using sve.Repositories.Contracts;
using sve.Services;

namespace sve.Tests
{
    public class SectorServiceTests
    {
        private readonly Mock<ISectorRepository> _mockRepo;
        private readonly SectorService _sectorService;

        public SectorServiceTests()
        {
            _mockRepo = new Mock<ISectorRepository>();
            _sectorService = new SectorService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaSectorDto()
        {
            // Arrange
            var sectores = new List<Sector>
            {
                new Sector { IdSector = 1, Nombre = "A", Capacidad = 100, IdLocal = 1 },
                new Sector { IdSector = 2, Nombre = "B", Capacidad = 200, IdLocal = 1 }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(sectores);

            // Act
            var resultado = _sectorService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("A", resultado[0].Nombre);
            Assert.Equal("B", resultado[1].Nombre);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaSector()
        {
            // Arrange
            var sector = new Sector { IdSector = 1, Nombre = "A", Capacidad = 100, IdLocal = 1 };
            _mockRepo.Setup(r => r.GetById(1)).Returns(sector);

            // Act
            var resultado = _sectorService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("A", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_NoExistente_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Sector)null);

            // Act
            var resultado = _sectorService.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarSector_Correctamente_RetornaId()
        {
            // Arrange
            var createDto = new SectorCreateDto { Nombre = "C", Capacidad = 150, IdLocal = 1 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Sector>())).Returns(1);

            // Act
            var idNuevo = _sectorService.AgregarSector(createDto);

            // Assert
            Assert.Equal(1, idNuevo);
            _mockRepo.Verify(r => r.Add(It.IsAny<Sector>()), Times.Once);
        }

        [Fact]
        public void ActualizarSector_RetornaTrue()
        {
            // Arrange
            var updateDto = new SectorUpdateDto { Nombre = "C Updated", Capacidad = 180, IdLocal = 1 };
            _mockRepo.Setup(r => r.Update(1, It.IsAny<Sector>())).Returns(true);

            // Act
            var resultado = _sectorService.ActualizarSector(1, updateDto);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Update(1, It.IsAny<Sector>()), Times.Once);
        }

        [Fact]
        public void EliminarSector_RetornaTrue()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(true);

            // Act
            var resultado = _sectorService.EliminarSector(1);

            // Assert
            Assert.True(resultado);
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
        }
    }
}
