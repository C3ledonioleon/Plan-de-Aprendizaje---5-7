using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;
using sveCore.Models;
using sveCore.DTOs;
using sveCore.Servicio.IRepositories;
using sveCore.Services.IServices;
using sveServices.Services;

namespace sveServices.Tests
{
    public class FuncionServiceTests
    {
        private readonly Mock<IFuncionRepository> _funcionRepositoryMock;
        private readonly IFuncionService _funcionService;

        public FuncionServiceTests()
        {
            _funcionRepositoryMock = new Mock<IFuncionRepository>();
            _funcionService = new FuncionService(_funcionRepositoryMock.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeFuncionDto()
        {
            // Arrange
            var funciones = new List<Funcion>
            {
                new Funcion { IdFuncion = 1, IdEvento = 10, IdLocal = 5, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente },
                new Funcion { IdFuncion = 2, IdEvento = 11, IdLocal = 6, FechaHora = DateTime.Now, Estado = EstadoFuncion.Cancelada }
            };
            _funcionRepositoryMock.Setup(r => r.GetAll()).Returns(funciones);

            // Act
            var resultado = _funcionService.ObtenerTodo();

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(EstadoFuncion.Pendiente, resultado[0].Estado);
            Assert.Equal(EstadoFuncion.Cancelada, resultado[1].Estado);
        }

        [Fact]
        public void AgregarFuncion_DeberiaLlamarAddYRetornarResultado()
        {
            // Arrange
            var dto = new FuncionCreateDto
            {
                IdEvento = 10,
                IdLocal = 5,
                FechaHora = DateTime.Now
            };

            _funcionRepositoryMock.Setup(r => r.Add(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.AgregarFuncion(dto);

            // Assert
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.Add(It.IsAny<Funcion>()), Times.Once);
        }

        [Fact]
        public void ActualizarFuncion_DeberiaLlamarUpdateYRetornarResultado()
        {
            // Arrange
            var dto = new FuncionUpdateDto
            {
                IdEvento = 10,
                IdLocal = 5,
                FechaHora = DateTime.Now.AddDays(1),
                Estado = EstadoFuncion.Pendiente // Cambiar según el valor que quieras probar
            };

            _funcionRepositoryMock.Setup(r => r.Update(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.ActualizarFuncion(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.Update(It.IsAny<Funcion>()), Times.Once);
        }

        [Fact]
        public void CancelarFuncion_DeberiaActualizarEstadoACancelada()
        {
            // Arrange
            var funcion = new Funcion
            {
                IdFuncion = 1,
                Estado = EstadoFuncion.Pendiente
            };
            _funcionRepositoryMock.Setup(r => r.GetById(1)).Returns(funcion);
            _funcionRepositoryMock.Setup(r => r.Update(funcion)).Returns(1);

            // Act
            var resultado = _funcionService.CancelarFuncion(1);

            // Assert
            Assert.Equal(EstadoFuncion.Cancelada, funcion.Estado);
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.Update(funcion), Times.Once);
        }
    }
}
