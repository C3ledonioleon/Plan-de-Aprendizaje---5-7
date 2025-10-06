using Xunit;
using Moq;
using System.Collections.Generic;
using System.Linq;
using sve.Services;
using sve.Repositories.Contracts;
using sve.Models;
using sve.DTOs;

namespace sve.Tests.Services
{
    public class SectorServiceTests
    {
        private readonly Mock<ISectorRepository> _mockRepo;
        private readonly SectorService _service;

        public SectorServiceTests()
        {
            _mockRepo = new Mock<ISectorRepository>();
            _service = new SectorService(_mockRepo.Object);
        }

        [Fact]
        public void ObtenerTodo_RetornaListaDeSectorDto()
        {
            // Arrange
            var sectores = new List<Sector>
            {
                new Sector { IdSector = 1, Nombre = "Sector1", Capacidad = 100, IdLocal = 1 },
                new Sector { IdSector = 2, Nombre = "Sector2", Capacidad = 200, IdLocal = 1 }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(sectores);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal("Sector1", resultado[0].Nombre);
            Assert.Equal("Sector2", resultado[1].Nombre);
        }

        [Fact]
        public void ObtenerPorId_Existente_RetornaSector()
        {
            // Arrange
            var sector = new Sector { IdSector = 1, Nombre = "Sector1", Capacidad = 100, IdLocal = 1 };
            _mockRepo.Setup(r => r.GetById(1)).Returns(sector);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Sector1", resultado.Nombre);
        }

        [Fact]
        public void ObtenerPorId_NoExiste_RetornaNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Sector?)null);

            // Act
            var resultado = _service.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarSector_DeberiaRetornarIdNuevo()
        {
            // Arrange
            var createDto = new SectorCreateDto { Nombre = "Sector1", Capacidad = 150, IdLocal = 1 };
            _mockRepo.Setup(r => r.Add(It.IsAny<Sector>())).Returns(10);

            // Act
            var resultado = _service.AgregarSector(createDto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.IsAny<Sector>()), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarSector_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var updateDto = new SectorUpdateDto { Nombre = "Sector1 Actualizado", Capacidad = 200, IdLocal = 1 };
            _mockRepo.Setup(r => r.Update(It.IsAny<Sector>())).Returns(1);

            // Act
            var resultado = _service.ActualizarSector(1, updateDto);

            // Assert
            _mockRepo.Verify(r => r.Update(It.Is<Sector>(s => s.Nombre == "Sector1 Actualizado" && s.Capacidad == 200)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarSector_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarSector(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
