using Microsoft.IdentityModel.Tokens;
using sveCore.DTOs;
using sveCore.Models;
using sveCore.Servicio.IRepositories;
using sveCore.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;


namespace sveServices.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public UsuarioService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public int Register(RegisterDto registerDto)
    {
        if (_usuarioRepository.GetByEmail(registerDto.Email) != null)
            throw new Exception("El email ya está registrado.");

        var usuario = new Usuario
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            Password = HashPassword(registerDto.Contraseña),
            Rol = RolUsuario.Usuario
        };
        

        return _usuarioRepository.Add(usuario);
    }

    public TokenDto Login(LoginDto loginDto)
    {
        var usuario = _usuarioRepository.GetByEmail(loginDto.Email);

        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        if (usuario.Password != HashPassword(loginDto.Contraseña))
            throw new Exception("Contraseña incorrecta.");

        string token = GenerateJwtToken(usuario);
        string refreshToken = GenerateRefreshToken();

        _usuarioRepository.UpdateRefreshToken(usuario.Email, refreshToken, DateTime.UtcNow.AddDays(7));

        return new TokenDto
        {
            Token = token,
            RefreshToken = refreshToken
        };
    }

    public TokenDto Refresh(string refreshToken)
    {
        var usuario = _usuarioRepository.GetByRefreshToken(refreshToken)
            ?? throw new Exception("Token inválido o expirado.");

        string newAccessToken = GenerateJwtToken(usuario);
        string newRefreshToken = GenerateRefreshToken();

        _usuarioRepository.UpdateRefreshToken(usuario.Email, newRefreshToken, DateTime.UtcNow.AddDays(7));

        return new TokenDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }

    public void Logout(string email)
    {
        _usuarioRepository.UpdateRefreshToken(email, null, DateTime.UtcNow);
    }

        public List<Usuario> ObtenerTodo()
    {
        return _usuarioRepository.GetAll()
            .Select(usuario => new Usuario
            {
                IdUsuario = usuario.IdUsuario,
                Username = usuario.Username,
                Email = usuario.Email,
                //Password = usuario.Password,
                Rol = usuario.Rol
            }).ToList();
    }


    public Usuario? GetById(int idUsuario)
    {
        return _usuarioRepository.GetById(idUsuario);
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:SecretKey").Value!));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol.ToString()!)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration.GetSection("JwtSettings:Issuer").Value,
            audience: _configuration.GetSection("JwtSettings:Audience").Value,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_configuration.GetSection("JwtSettings:TokenExpirationMinutes").Get<int>()),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }

    private string HashPassword(string password)
    {
        using (var sha = SHA256.Create())
        {
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }

    public int UpdateRol(int idUsuario, RolUsuario nuevoRol)
    {
        var usuario = _usuarioRepository.GetById(idUsuario);
        if (usuario == null)
            throw new Exception("Usuario no encontrado.");

        usuario.Rol = nuevoRol;

        return _usuarioRepository.Update(usuario);
    }

}
    
