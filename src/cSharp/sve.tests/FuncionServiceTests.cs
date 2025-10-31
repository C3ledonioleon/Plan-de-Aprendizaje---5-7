using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;
using Xunit;

namespace sve.Tests.Services
{
    public class FuncionServiceTests
    {
        private readonly Mock<IFuncionRepository> _funcionRepositoryMock;
        private readonly FuncionService _funcionService;

        public FuncionServiceTests()
        {
            _funcionRepositoryMock = new Mock<IFuncionRepository>();
            _funcionService = new FuncionService(_funcionRepositoryMock.Object);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaDeFuncionesDto()
        {
            // Arrange
            var funciones = new List<Funcion>
            {
                new Funcion { IdFuncion = 1, IdEvento = 10, IdLocal = 5, FechaHora = DateTime.Now, Estado = EstadoFuncion.Pendiente },
                new Funcion { IdFuncion = 2, IdEvento = 20, IdLocal = 6, FechaHora = DateTime.Now.AddDays(1), Estado = EstadoFuncion.Cancelada }
            };

            _funcionRepositoryMock.Setup(r => r.GetAll()).Returns(funciones);

            // Act
            var resultado = _funcionService.ObtenerTodo();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.All(resultado, item => Assert.IsType<FuncionDto>(item));
            _funcionRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ObtenerTodo_DeberiaRetornarListaVaciaCuandoNoHayFunciones()
        {
            // Arrange
            _funcionRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Funcion>());

            // Act
            var resultado = _funcionService.ObtenerTodo();

            // Assert
            Assert.NotNull(resultado);
            Assert.Empty(resultado);
            _funcionRepositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void ObtenerTodo_DeberiaMapearCorrectamenteTodosLosCampos()
        {
            // Arrange
            var fecha = new DateTime(2024, 1, 15, 20, 0, 0);
            var funcion = new Funcion 
            { 
                IdFuncion = 1, 
                IdEvento = 10, 
                IdLocal = 5, 
                FechaHora = fecha, 
                Estado = EstadoFuncion.Pendiente 
            };

            _funcionRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Funcion> { funcion });

            // Act
            var resultado = _funcionService.ObtenerTodo().First();

            // Assert
            Assert.Equal(1, resultado.IdFuncion);
            Assert.Equal(10, resultado.IdEvento);
            Assert.Equal(5, resultado.IdLocal);
            Assert.Equal(fecha, resultado.FechaHora);
            Assert.Equal(EstadoFuncion.Pendiente, resultado.Estado);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarFuncionSiExiste()
        {
            // Arrange
            var funcion = new Funcion { IdFuncion = 1, IdEvento = 10, IdLocal = 5 };
            _funcionRepositoryMock.Setup(r => r.GetById(1)).Returns(funcion);

            // Act
            var resultado = _funcionService.ObtenerPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.IdFuncion);
            Assert.Equal(10, resultado.IdEvento);
            _funcionRepositoryMock.Verify(r => r.GetById(1), Times.Once);
        }

        [Fact]
        public void ObtenerPorId_DeberiaRetornarNullSiNoExiste()
        {
            // Arrange
            _funcionRepositoryMock.Setup(r => r.GetById(999)).Returns((Funcion?)null);

            // Act
            var resultado = _funcionService.ObtenerPorId(999);

            // Assert
            Assert.Null(resultado);
            _funcionRepositoryMock.Verify(r => r.GetById(999), Times.Once);
        }

        [Fact]
        public void AgregarFuncion_DeberiaAgregarYRetornarId()
        {
            // Arrange
            var fecha = DateTime.Now.AddDays(7);
            var dto = new FuncionCreateDto
            {
                IdEvento = 10,
                IdLocal = 5,
                FechaHora = fecha
            };

            _funcionRepositoryMock.Setup(r => r.Add(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.AgregarFuncion(dto);

            // Assert
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.Add(It.Is<Funcion>(f =>
                f.IdEvento == dto.IdEvento &&
                f.IdLocal == dto.IdLocal &&
                f.FechaHora == dto.FechaHora &&
                f.Estado == EstadoFuncion.Pendiente // Siempre debe ser Pendiente
            )), Times.Once);
        }

        [Fact]
        public void AgregarFuncion_DeberiaAsignarEstadoPendientePorDefecto()
        {
            // Arrange
            var dto = new FuncionCreateDto
            {
                IdEvento = 10,
                IdLocal = 5,
                FechaHora = DateTime.Now
                // No se establece Estado explícitamente
            };

            _funcionRepositoryMock.Setup(r => r.Add(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.AgregarFuncion(dto);

            // Assert
            _funcionRepositoryMock.Verify(r => r.Add(It.Is<Funcion>(f =>
                f.Estado == EstadoFuncion.Pendiente // Siempre debe ser Pendiente
            )), Times.Once);
        }

        [Fact]
        public void ActualizarFuncion_DeberiaLlamarUpdateYRetornarResultado()
        {
            // Arrange
            var fecha = DateTime.Now.AddDays(1);
            var dto = new FuncionUpdateDto
            {
                IdEvento = 10,
                IdLocal = 5,
                FechaHora = fecha,
                Estado = EstadoFuncion.Realizada
            };

            _funcionRepositoryMock.Setup(r => r.Update(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.ActualizarFuncion(1, dto);

            // Assert
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.Update(It.IsAny<Funcion>()), Times.Once);
        }

        [Fact]
        public void ActualizarFuncion_DeberiaMapearCorrectamenteTodosLosCampos()
        {
            // Arrange
            var fecha = new DateTime(2024, 1, 20, 21, 0, 0);
            var dto = new FuncionUpdateDto
            {
                IdEvento = 15,
                IdLocal = 8,
                FechaHora = fecha,
                Estado = EstadoFuncion.Realizada
            };

            _funcionRepositoryMock.Setup(r => r.Update(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.ActualizarFuncion(1, dto);

            // Assert
            _funcionRepositoryMock.Verify(r => r.Update(It.Is<Funcion>(f =>
                f.IdFuncion == 1 &&
                f.IdEvento == dto.IdEvento &&
                f.IdLocal == dto.IdLocal &&
                f.FechaHora == dto.FechaHora &&
                f.Estado == dto.Estado
            )), Times.Once);
        }

        [Fact]
        public void EliminarFuncion_DeberiaLlamarDeleteYRetornarResultado()
        {
            // Arrange
            _funcionRepositoryMock.Setup(r => r.Delete(1)).Returns(1);

            // Act
            var resultado = _funcionService.EliminarFuncion(1);

            // Assert
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.Delete(1), Times.Once);
        }

        [Fact]
        public void EliminarFuncion_DeberiaRetornarCeroSiNoSePudoEliminar()
        {
            // Arrange
            _funcionRepositoryMock.Setup(r => r.Delete(999)).Returns(0);

            // Act
            var resultado = _funcionService.EliminarFuncion(999);

            // Assert
            Assert.Equal(0, resultado);
            _funcionRepositoryMock.Verify(r => r.Delete(999), Times.Once);
        }

        [Fact]
        public void CancelarFuncion_DeberiaActualizarEstadoACancelada()
        {
            // Arrange
            var funcion = new Funcion { IdFuncion = 1, Estado = EstadoFuncion.Pendiente };
            _funcionRepositoryMock.Setup(r => r.GetById(1)).Returns(funcion);
            _funcionRepositoryMock.Setup(r => r.Update(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.CancelarFuncion(1);

            // Assert
            Assert.Equal(1, resultado);
            _funcionRepositoryMock.Verify(r => r.GetById(1), Times.Once);
            _funcionRepositoryMock.Verify(r => r.Update(It.Is<Funcion>(f => 
                f.Estado == EstadoFuncion.Cancelada
            )), Times.Once);
        }

        [Fact]
        public void CancelarFuncion_DeberiaRetornar0SiNoExiste()
        {
            // Arrange
            _funcionRepositoryMock.Setup(r => r.GetById(1)).Returns((Funcion?)null);

            // Act
            var resultado = _funcionService.CancelarFuncion(1);

            // Assert
            Assert.Equal(0, resultado);
            _funcionRepositoryMock.Verify(r => r.GetById(1), Times.Once);
            _funcionRepositoryMock.Verify(r => r.Update(It.IsAny<Funcion>()), Times.Never);
        }

        [Fact]
        public void CancelarFuncion_DeberiaMantenerOtrosCamposAlCancelar()
        {
            // Arrange
            var fechaOriginal = new DateTime(2024, 1, 15, 20, 0, 0);
            var funcion = new Funcion 
            { 
                IdFuncion = 1, 
                IdEvento = 10, 
                IdLocal = 5, 
                FechaHora = fechaOriginal, 
                Estado = EstadoFuncion.Pendiente 
            };
            
            _funcionRepositoryMock.Setup(r => r.GetById(1)).Returns(funcion);
            _funcionRepositoryMock.Setup(r => r.Update(It.IsAny<Funcion>())).Returns(1);

            // Act
            var resultado = _funcionService.CancelarFuncion(1);

            // Assert
            _funcionRepositoryMock.Verify(r => r.Update(It.Is<Funcion>(f =>
                f.IdFuncion == 1 &&
                f.IdEvento == 10 &&
                f.IdLocal == 5 &&
                f.FechaHora == fechaOriginal && // Mantiene la fecha original
                f.Estado == EstadoFuncion.Cancelada // Solo cambia el estado
            )), Times.Once);
        }
    }
}
      
