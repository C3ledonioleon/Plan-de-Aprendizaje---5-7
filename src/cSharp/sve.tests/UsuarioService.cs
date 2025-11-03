using System;
using Moq;
using Xunit;
using sveCore.Models;
using sveCore.DTOs;
using sveCore.Servicio.IRepositories;
using Microsoft.Extensions.Configuration;
using sveServices.Services;

namespace sveServices.Tests
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
             // Mock del repositorio
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();

            // Mock de IConfiguration
            _configurationMock = new Mock<IConfiguration>();

            // SecretKey (mínimo 32 caracteres)
            var secretSection = new Mock<IConfigurationSection>();
            secretSection.Setup(s => s.Value).Returns("mi_clave_secreta_muy_larga_1234567890");
            _configurationMock.Setup(c => c.GetSection("JwtSettings:SecretKey")).Returns(secretSection.Object);

            // Issuer
            var issuerSection = new Mock<IConfigurationSection>();
            issuerSection.Setup(s => s.Value).Returns("miIssuer");
            _configurationMock.Setup(c => c.GetSection("JwtSettings:Issuer")).Returns(issuerSection.Object);

            // Audience
            var audienceSection = new Mock<IConfigurationSection>();
            audienceSection.Setup(s => s.Value).Returns("miAudience");
            _configurationMock.Setup(c => c.GetSection("JwtSettings:Audience")).Returns(audienceSection.Object);

            // TokenExpirationMinutes
            var expirationSection = new Mock<IConfigurationSection>();
            expirationSection.Setup(s => s.Value).Returns("60");
            _configurationMock.Setup(c => c.GetSection("JwtSettings:TokenExpirationMinutes")).Returns(expirationSection.Object);

            // Instancia del servicio
            _usuarioService = new UsuarioService(_usuarioRepositoryMock.Object, _configurationMock.Object);
        }

        [Fact]
        public void Register_EmailNoExiste_DeberiaAgregarUsuario()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "Juan",
                Email = "juan@mail.com",
                Contraseña = "1234",
                Rol = RolUsuario.Cliente
            };

            _usuarioRepositoryMock.Setup(r => r.GetByEmail(registerDto.Email)).Returns((Usuario?)null);
            _usuarioRepositoryMock.Setup(r => r.Add(It.IsAny<Usuario>())).Returns(1);

            // Act
            int result = _usuarioService.Register(registerDto);

            // Assert
            Assert.Equal(1, result);
            _usuarioRepositoryMock.Verify(r => r.Add(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact]
        public void Register_EmailExiste_DeberiaLanzarExcepcion()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "Juan",
                Email = "juan@mail.com",
                Contraseña = "1234",
                Rol = RolUsuario.Cliente
            };

            _usuarioRepositoryMock.Setup(r => r.GetByEmail(registerDto.Email))
                .Returns(new Usuario());

            // Act & Assert
            Assert.Throws<Exception>(() => _usuarioService.Register(registerDto));
        }

        [Fact]
        public void Login_CredencialesCorrectas_DeberiaRetornarToken()
        {
            // Arrange
            var usuario = new Usuario
            {
                IdUsuario = 1,
                Username = "Juan",
                Email = "juan@mail.com",
                Password = Convert.ToHexString(System.Security.Cryptography.SHA256.Create()
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes("1234"))).ToLower(),
                Rol = RolUsuario.Cliente
            };

            var loginDto = new LoginDto
            {
                Email = "juan@mail.com",
                Contraseña = "1234"
            };

            _usuarioRepositoryMock.Setup(r => r.GetByEmail(loginDto.Email)).Returns(usuario);
            _usuarioRepositoryMock.Setup(r => r.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()));

            // Act
            var tokenDto = _usuarioService.Login(loginDto);

            // Assert
            Assert.NotNull(tokenDto.Token);
            Assert.NotNull(tokenDto.RefreshToken);
            _usuarioRepositoryMock.Verify(r => r.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void Logout_DeberiaActualizarRefreshTokenANull()
        {
            // Arrange
            string email = "juan@mail.com";
            _usuarioRepositoryMock.Setup(r => r.UpdateRefreshToken(email, null, It.IsAny<DateTime>()));

            // Act
            _usuarioService.Logout(email);

            // Assert
            _usuarioRepositoryMock.Verify(r => r.UpdateRefreshToken(email, null, It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void UpdateRol_UsuarioExistente_DeberiaActualizarRol()
        {
            // Arrange
            var usuario = new Usuario
            {
                IdUsuario = 1,
                Username = "Juan",
                Email = "juan@mail.com",
                Rol = RolUsuario.Cliente
            };

            _usuarioRepositoryMock.Setup(r => r.GetById(usuario.IdUsuario)).Returns(usuario);
            _usuarioRepositoryMock.Setup(r => r.Update(usuario)).Returns(usuario.IdUsuario);

            // Act
            int result = _usuarioService.UpdateRol(usuario.IdUsuario, RolUsuario.Administrador);

            // Assert
            Assert.Equal(usuario.IdUsuario, result);
            Assert.Equal(RolUsuario.Administrador, usuario.Rol);
            _usuarioRepositoryMock.Verify(r => r.Update(usuario), Times.Once);
        }
    }
}
