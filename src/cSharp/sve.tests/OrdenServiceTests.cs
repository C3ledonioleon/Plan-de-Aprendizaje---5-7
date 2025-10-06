using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;
using sve.DTOs;

namespace sve.Tests.Services
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
        public void ObtenerTodo_DeberiaRetornarListaDeOrdenDto()
        {
            // Arrange
            var ordenes = new List<Orden>
            {
                new Orden { IdOrden = 1, IdCliente = 1, IdTarifa = 1, Total = 100, Fecha = DateTime.Now, Estado = EstadoOrden.Creada },
                new Orden { IdOrden = 2, IdCliente = 2, IdTarifa = 2, Total = 200, Fecha = DateTime.Now, Estado = EstadoOrden.Creada }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(ordenes);

            // Act
            var resultado = _service.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(100, resultado[0].Total);
            Assert.Equal(200, resultado[1].Total);
        }

        [Fact]
        public void ObtenerPorId_Existente_DeberiaRetornarOrden()
        {
            // Arrange
            var orden = new Orden { IdOrden = 1, IdCliente = 1, IdTarifa = 1, Total = 100, Fecha = DateTime.Now, Estado = EstadoOrden.Creada };
            _mockRepo.Setup(r => r.GetById(1)).Returns(orden);

            // Act
            var resultado = _service.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(100, resultado.Total);
        }

        [Fact]
        public void ObtenerPorId_NoExiste_DeberiaRetornarNull()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetById(99)).Returns((Orden?)null);

            // Act
            var resultado = _service.ObtenerPorId(99);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void AgregarOrden_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var dto = new OrdenCreateDto { IdCliente = 1, IdTarifa = 1, Total = 150, Fecha = DateTime.Now };
            _mockRepo.Setup(r => r.Add(It.IsAny<Orden>())).Returns(10);

            // Act
            var resultado = _service.AgregarOrden(dto);

            // Assert
            _mockRepo.Verify(r => r.Add(It.IsAny<Orden>()), Times.Once);
            Assert.Equal(10, resultado);
        }

        [Fact]
        public void ActualizarOrden_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            var updateDto = new OrdenUpdateDto { IdCliente = 1, IdTarifa = 1, Total = 200, Fecha = DateTime.Now, Estado = EstadoOrden.Creada };
            _mockRepo.Setup(r => r.Update(It.IsAny<Orden>())).Returns(1);

            // Act
            var resultado = _service.ActualizarOrden(1, updateDto);

            // Assert
            _mockRepo.Verify(r => r.Update(It.IsAny<Orden>()), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void EliminarOrden_DeberiaLlamarRepositorioYRetornarId()
        {
            // Arrange
            _mockRepo.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _service.EliminarOrden(1);

            // Assert
            _mockRepo.Verify(r => r.Delete(1), Times.Once);
            Assert.Equal(1, resultado);
        }
    }
}
