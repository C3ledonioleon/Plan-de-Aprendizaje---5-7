using sve.DTOs;
using sve.Models;

namespace sve.Services.Contracts;

public interface IUsuarioService
{
    Usuario? GetById(int idUsuario);
    TokenDto Login(LoginDto loginDto);
    void Logout(string email);
    TokenDto Refresh(string refreshToken);
    int Register(RegisterDto registerDto);
    int UpdateRol(int idUsuario, RolUsuario nuevoRol);
}
