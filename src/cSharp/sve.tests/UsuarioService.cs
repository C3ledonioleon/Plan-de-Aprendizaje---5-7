using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace sve.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _mockUsuarioRepository;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly UsuarioService _usuarioService;

        public UsuarioServiceTests()
        {
            _mockUsuarioRepository = new Mock<IUsuarioRepository>();
            _mockConfiguration = new Mock<IConfiguration>();
            
            // Configuración básica para JWT
            var configurationSection = new Mock<IConfigurationSection>();
            configurationSection.Setup(x => x.Value).Returns("mi-clave-secreta-super-segura-y-larga-para-test");
            _mockConfiguration.Setup(x => x.GetSection("JwtSettings:SecretKey")).Returns(configurationSection.Object);
            
            var issuerSection = new Mock<IConfigurationSection>();
            issuerSection.Setup(x => x.Value).Returns("test-issuer");
            _mockConfiguration.Setup(x => x.GetSection("JwtSettings:Issuer")).Returns(issuerSection.Object);
            
            var audienceSection = new Mock<IConfigurationSection>();
            audienceSection.Setup(x => x.Value).Returns("test-audience");
            _mockConfiguration.Setup(x => x.GetSection("JwtSettings:Audience")).Returns(audienceSection.Object);
            
            var expirationSection = new Mock<IConfigurationSection>();
            expirationSection.Setup(x => x.Get<int>()).Returns(60);
            _mockConfiguration.Setup(x => x.GetSection("JwtSettings:TokenExpirationMinutes")).Returns(expirationSection.Object);

            _usuarioService = new UsuarioService(_mockUsuarioRepository.Object, _mockConfiguration.Object);
        }

        [Fact]
        public void Register_WhenEmailNotExists_ShouldReturnUserId()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Contraseña = "password123",
                Rol = RolUsuario.Usuario
            };

            _mockUsuarioRepository.Setup(x => x.GetByEmail(registerDto.Email)).Returns((Usuario)null);
            _mockUsuarioRepository.Setup(x => x.Add(It.IsAny<Usuario>())).Returns(1);

            // Act
            var result = _usuarioService.Register(registerDto);

            // Assert
            Assert.Equal(1, result);
            _mockUsuarioRepository.Verify(x => x.GetByEmail(registerDto.Email), Times.Once);
            _mockUsuarioRepository.Verify(x => x.Add(It.Is<Usuario>(u => 
                u.Username == registerDto.Username && 
                u.Email == registerDto.Email &&
                u.Rol == registerDto.Rol
            )), Times.Once);
        }

        [Fact]
        public void Register_WhenEmailExists_ShouldThrowException()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                Email = "existing@example.com",
                Contraseña = "password123",
                Rol = RolUsuario.Usuario
            };

            var existingUser = new Usuario { Email = registerDto.Email };
            _mockUsuarioRepository.Setup(x => x.GetByEmail(registerDto.Email)).Returns(existingUser);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _usuarioService.Register(registerDto));
            Assert.Equal("El email ya está registrado.", exception.Message);
        }

        [Fact]
        public void Login_WithValidCredentials_ShouldReturnTokens()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Contraseña = "password123"
            };

            var hashedPassword = HashPassword("password123");
            var usuario = new Usuario
            {
                Id = 1,
                Username = "testuser",
                Email = loginDto.Email,
                Password = hashedPassword,
                Rol = RolUsuario.Usuario
            };

            _mockUsuarioRepository.Setup(x => x.GetByEmail(loginDto.Email)).Returns(usuario);
            _mockUsuarioRepository.Setup(x => x.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()));

            // Act
            var result = _usuarioService.Login(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);
            _mockUsuarioRepository.Verify(x => x.GetByEmail(loginDto.Email), Times.Once);
            _mockUsuarioRepository.Verify(x => x.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void Login_WithInvalidEmail_ShouldThrowException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "nonexistent@example.com",
                Contraseña = "password123"
            };

            _mockUsuarioRepository.Setup(x => x.GetByEmail(loginDto.Email)).Returns((Usuario)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _usuarioService.Login(loginDto));
            Assert.Equal("Usuario no encontrado.", exception.Message);
        }

        [Fact]
        public void Login_WithInvalidPassword_ShouldThrowException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Email = "test@example.com",
                Contraseña = "wrongpassword"
            };

            var usuario = new Usuario
            {
                Id = 1,
                Email = loginDto.Email,
                Password = HashPassword("correctpassword"),
                Rol = RolUsuario.Usuario
            };

            _mockUsuarioRepository.Setup(x => x.GetByEmail(loginDto.Email)).Returns(usuario);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _usuarioService.Login(loginDto));
            Assert.Equal("Contraseña incorrecta.", exception.Message);
        }

        [Fact]
        public void Refresh_WithValidRefreshToken_ShouldReturnNewTokens()
        {
            // Arrange
            var refreshToken = "valid-refresh-token";
            var usuario = new Usuario
            {
                Id = 1,
                Email = "test@example.com",
                Rol = RolUsuario.Usuario
            };

            _mockUsuarioRepository.Setup(x => x.GetByRefreshToken(refreshToken)).Returns(usuario);
            _mockUsuarioRepository.Setup(x => x.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()));

            // Act
            var result = _usuarioService.Refresh(refreshToken);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);
            _mockUsuarioRepository.Verify(x => x.GetByRefreshToken(refreshToken), Times.Once);
            _mockUsuarioRepository.Verify(x => x.UpdateRefreshToken(usuario.Email, It.IsAny<string>(), It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void Refresh_WithInvalidRefreshToken_ShouldThrowException()
        {
            // Arrange
            var refreshToken = "invalid-refresh-token";
            _mockUsuarioRepository.Setup(x => x.GetByRefreshToken(refreshToken)).Returns((Usuario)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _usuarioService.Refresh(refreshToken));
            Assert.Equal("Token inválido o expirado.", exception.Message);
        }

        [Fact]
        public void Logout_ShouldUpdateRefreshTokenToNull()
        {
            // Arrange
            var email = "test@example.com";
            _mockUsuarioRepository.Setup(x => x.UpdateRefreshToken(email, null, It.IsAny<DateTime>()));

            // Act
            _usuarioService.Logout(email);

            // Assert
            _mockUsuarioRepository.Verify(x => x.UpdateRefreshToken(email, null, It.IsAny<DateTime>()), Times.Once);
        }

        [Fact]
        public void GetById_WithValidId_ShouldReturnUsuario()
        {
            // Arrange
            var usuarioId = 1;
            var expectedUsuario = new Usuario { Id = usuarioId, Username = "testuser" };
            _mockUsuarioRepository.Setup(x => x.GetById(usuarioId)).Returns(expectedUsuario);

            // Act
            var result = _usuarioService.GetById(usuarioId);

            // Assert
            Assert.Equal(expectedUsuario, result);
            _mockUsuarioRepository.Verify(x => x.GetById(usuarioId), Times.Once);
        }

        [Fact]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var usuarioId = 999;
            _mockUsuarioRepository.Setup(x => x.GetById(usuarioId)).Returns((Usuario)null);

            // Act
            var result = _usuarioService.GetById(usuarioId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void UpdateRol_WithValidUsuario_ShouldUpdateRol()
        {
            // Arrange
            var usuarioId = 1;
            var nuevoRol = RolUsuario.Administrador;
            var usuario = new Usuario 
            { 
                Id = usuarioId, 
                Username = "testuser", 
                Rol = RolUsuario.Usuario 
            };

            _mockUsuarioRepository.Setup(x => x.GetById(usuarioId)).Returns(usuario);
            _mockUsuarioRepository.Setup(x => x.Update(It.IsAny<Usuario>())).Returns(1);

            // Act
            var result = _usuarioService.UpdateRol(usuarioId, nuevoRol);

            // Assert
            Assert.Equal(1, result);
            _mockUsuarioRepository.Verify(x => x.GetById(usuarioId), Times.Once);
            _mockUsuarioRepository.Verify(x => x.Update(It.Is<Usuario>(u => u.Rol == nuevoRol)), Times.Once);
        }

        [Fact]
        public void UpdateRol_WithInvalidUsuario_ShouldThrowException()
        {
            // Arrange
            var usuarioId = 999;
            var nuevoRol = RolUsuario.Administrador;
            _mockUsuarioRepository.Setup(x => x.GetById(usuarioId)).Returns((Usuario)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _usuarioService.UpdateRol(usuarioId, nuevoRol));
            Assert.Equal("Usuario no encontrado.", exception.Message);
        }

        [Fact]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "test@example.com",
                Rol = RolUsuario.Administrador
            };

            // Act
            var token = _usuarioService.GetType().GetMethod("GenerateJwtToken", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_usuarioService, new object[] { usuario }) as string;

            // Assert
            Assert.NotNull(token);
            
            // Verificar que el token es un JWT válido
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            Assert.NotNull(jwtToken);
            
            // Verificar claims
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);
            Assert.Equal(usuario.Email, emailClaim?.Value);
            
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            Assert.Equal(usuario.Rol.ToString(), roleClaim?.Value);
        }

        [Fact]
        public void HashPassword_ShouldReturnConsistentHash()
        {
            // Arrange
            var password = "testpassword";

            // Act
            var hash1 = _usuarioService.GetType().GetMethod("HashPassword", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_usuarioService, new object[] { password }) as string;

            var hash2 = _usuarioService.GetType().GetMethod("HashPassword", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(_usuarioService, new object[] { password }) as string;

            // Assert
            Assert.NotNull(hash1);
            Assert.NotNull(hash2);
            Assert.Equal(hash1, hash2);
            Assert.NotEqual(password, hash1);
        }

        // Método helper para hashear contraseñas en los tests
        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }
    }
}

