using Xunit;
using Moq;
using System;
using sve.Services;
using sve.Repositories.Contracts;
using sve.Models;
using sve.DTOs;
using Microsoft.Extensions.Configuration;

namespace sve.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockRepo;
        private readonly Mock<IConfiguration> _mockConfig;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            _mockRepo = new Mock<IUsuarioRepository>();
            _mockConfig = new Mock<IConfiguration>();

            // Mock para JwtSettings:SecretKey
            var mockSecretKey = new Mock<IConfigurationSection>();
            mockSecretKey.Setup(s => s.Value).Returns("MiClaveSuperSecreta1234567123456789290");
            _mockConfig.Setup(c => c.GetSection("JwtSettings:SecretKey")).Returns(mockSecretKey.Object);

            // Mock para JwtSettings:Issuer
            var mockIssuer = new Mock<IConfigurationSection>();
            mockIssuer.Setup(s => s.Value).Returns("TestIssuer");
            _mockConfig.Setup(c => c.GetSection("JwtSettings:Issuer")).Returns(mockIssuer.Object);

            // Mock para JwtSettings:Audience
            var mockAudience = new Mock<IConfigurationSection>();
            mockAudience.Setup(s => s.Value).Returns("TestAudience");
            _mockConfig.Setup(c => c.GetSection("JwtSettings:Audience")).Returns(mockAudience.Object);

            // Mock para JwtSettings:TokenExpirationMinutes
            var mockExpiration = new Mock<IConfigurationSection>();
            mockExpiration.Setup(s => s.Value).Returns("60");
            _mockConfig.Setup(c => c.GetSection("JwtSettings:TokenExpirationMinutes")).Returns(mockExpiration.Object);

            // Crear instancia del servicio usando los mocks
            _service = new UsuarioService(_mockRepo.Object, _mockConfig.Object);
        }


        [Fact]
        public void Register_EmailNoRegistrado_DeberiaAgregarUsuario()
        {
            
            var dto = new RegisterDto { Username = "usuario", Email = "test@mail.com", Contraseña = "1234" };
            _mockRepo.Setup(r => r.GetByEmail(dto.Email)).Returns((Usuario?)null);
            _mockRepo.Setup(r => r.Add(It.IsAny<Usuario>())).Returns(1);

            
            var resultado = _service.Register(dto);

           
            _mockRepo.Verify(r => r.Add(It.Is<Usuario>(u => u.Email == dto.Email)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void Register_EmailExistente_DeberiaLanzarExcepcion()
        {
            
            var dto = new RegisterDto { Username = "usuario", Email = "test@mail.com", Contraseña = "1234" };
            _mockRepo.Setup(r => r.GetByEmail(dto.Email)).Returns(new Usuario());

            var ex = Assert.Throws<Exception>(() => _service.Register(dto));
            Assert.Equal("El email ya está registrado.", ex.Message);
        }

        [Fact]
        public void Login_CredencialesCorrectas_DeberiaRetornarToken()
        {
     
            var loginDto = new LoginDto { Email = "test@mail.com", Contraseña = "1234" };
            var hashedPassword = _service.GetType()
               .GetMethod("HashPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(_service, new object[] { loginDto.Contraseña })?.ToString();

            var usuario = new Usuario { Email = loginDto.Email, Password = hashedPassword!, Rol = RolUsuario.Cliente };
            _mockRepo.Setup(r => r.GetByEmail(loginDto.Email)).Returns(usuario);
            _mockRepo.Setup(r => r.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()));

            var resultado = _service.Login(loginDto);

            Assert.NotNull(resultado.Token);
            Assert.NotNull(resultado.RefreshToken);
            _mockRepo.Verify(r => r.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void Login_UsuarioNoExistente_DeberiaLanzarExcepcion()
        {
           
            var loginDto = new LoginDto { Email = "test@mail.com", Contraseña = "1234" };
            _mockRepo.Setup(r => r.GetByEmail(loginDto.Email)).Returns((Usuario?)null);

            var ex = Assert.Throws<Exception>(() => _service.Login(loginDto));
            Assert.Equal("Usuario no encontrado.", ex.Message);
        }

        [Fact]
        public void UpdateRol_UsuarioExistente_DeberiaActualizarRol()
        {
            
            var cliente = new Usuario { IdUsuario = 1, Rol = RolUsuario.Cliente };
            _mockRepo.Setup(r => r.GetById(1)).Returns(cliente);
            _mockRepo.Setup(r => r.Update(cliente)).Returns(1);

            
            var resultado = _service.UpdateRol(1, RolUsuario.Administrador);

            
            _mockRepo.Verify(r => r.Update(It.Is<Usuario>(u => u.Rol == RolUsuario.Administrador)), Times.Once);
            Assert.Equal(1, resultado);
        }

        [Fact]
        public void UpdateRol_UsuarioNoExistente_DeberiaLanzarExcepcion()
        {
            
            _mockRepo.Setup(r => r.GetById(1)).Returns((Usuario?)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.UpdateRol(1, RolUsuario.Administrador));
            Assert.Equal("Usuario no encontrado.", ex.Message);
        }
    }
}
