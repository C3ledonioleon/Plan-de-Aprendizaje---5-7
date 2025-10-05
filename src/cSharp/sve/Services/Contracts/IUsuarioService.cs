using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts
{
    public interface IUsuarioService
    {
        List<UsuarioDto> ObtenerTodo();
        List<string> GetRoles();
        UsuarioDto? ObtenerPorId(int id);
        int AgregarUsuario(RegisterDto usuario);
        AuthResponseDto Login(LoginDto usuario);
        string RefreshToken(RefreshTokenDto usuario);
        void Logout(string token);
        UsuarioDto GetProfile(int id);
        UsuarioDto AsignarRol(int id, UsuarioRolDto usuario);
        int ActualizarUsuario(int id, UsuarioUpdateDto usuario);
        int EliminarUsuario(int id);
        
    }
}
