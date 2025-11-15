using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using sveCore.Servicio.IRepositories;
using sveServices.Services;
using sveCore.DTOs;
using sveCore.Models;

namespace sve.Tests.Services
{
    public class UsuarioServiceTests
    {
        private readonly Mock<IUsuarioRepository> _repo;
        private readonly UsuarioService _service;

        public UsuarioServiceTests()
        {
            _repo = new Mock<IUsuarioRepository>();
            _service = new UsuarioService(_repo.Object, MockConfig());
        }

        // 🔧 Mock rápido de configuración JWT
        private IConfiguration MockConfig()
        {
            var dict = new Dictionary<string, string>
            {
                ["JwtSettings:SecretKey"] = "12345678901234567890123456789012",
                ["JwtSettings:Issuer"] = "TestIssuer",
                ["JwtSettings:Audience"] = "TestAudience",
                ["JwtSettings:TokenExpirationMinutes"] = "15"
            };

            return new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
        }

        // ----------------------------------------------------------
        //                     TESTS SIMPLIFICADOS
        // ----------------------------------------------------------

        [Fact]
        public void Register_CreaUsuarioSiEmailNoExiste()
        {
            var dto = new RegisterDto
            {
                Username = "juan",
                Email = "test@test.com",
                Contraseña = "1234"
            };

            _repo.Setup(r => r.GetByEmail(dto.Email)).Returns((Usuario?)null);
            _repo.Setup(r => r.Add(It.IsAny<Usuario>())).Returns(1);

            var result = _service.Register(dto);

            Assert.Equal(1, result);
        }

        [Fact]
        public void Register_FallaSiEmailExiste()
        {
            _repo.Setup(r => r.GetByEmail("test@test.com")).Returns(new Usuario());

            Assert.Throws<Exception>(() =>
                _service.Register(new RegisterDto { Email = "test@test.com" })
            );
        }

        [Fact]
        public void Login_RetornaTokensSiTodoCorrecto()
        {
            var dto = new LoginDto { Email = "a@a.com", Contraseña = "1234" };

            // Generamos hash igual al que usa el servicio
            var hash = _service
                        .GetType()
                        .GetMethod("HashPassword", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                        .Invoke(_service, new object[] { dto.Contraseña })!.ToString();

            _repo.Setup(r => r.GetByEmail(dto.Email)).Returns(new Usuario
            {
                Email = dto.Email,
                Password = hash,
                Rol = RolUsuario.Usuario
            });

            var result = _service.Login(dto);

            Assert.NotNull(result.Token);
            Assert.NotNull(result.RefreshToken);
        }

        [Fact]
        public void Login_FallaSiUsuarioNoExiste()
        {
            _repo.Setup(r => r.GetByEmail("no@a.com")).Returns((Usuario?)null);

            Assert.Throws<Exception>(() =>
                _service.Login(new LoginDto { Email = "no@a.com", Contraseña = "1234" })
            );
        }

        [Fact]
        public void Refresh_GeneraNuevoToken()
        {
            var usuario = new Usuario { Email = "a@a.com", Rol = RolUsuario.Usuario };

            _repo.Setup(r => r.GetByRefreshToken("REF123")).Returns(usuario);

            var result = _service.Refresh("REF123");

            Assert.NotNull(result.Token);
        }

        [Fact]
        public void UpdateRol_CambiaElRol()
        {
            var usuario = new Usuario { IdUsuario = 1, Rol = RolUsuario.Usuario };

            _repo.Setup(r => r.GetById(1)).Returns(usuario);
            _repo.Setup(r => r.Update(usuario)).Returns(1);

            var result = _service.UpdateRol(1, RolUsuario.Administrador);

            Assert.Equal(1, result);
            Assert.Equal(RolUsuario.Administrador, usuario.Rol);
        }
    }
}
