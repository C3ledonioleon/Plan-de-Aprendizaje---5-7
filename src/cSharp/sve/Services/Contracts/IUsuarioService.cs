using sve.DTOs;
using sve.Models;
using System.Collections.Generic;

namespace sve.Services.Contracts
{
    public interface IUsuarioService
    {
        List<UsuarioDto> ObtenerTodo();
        List<string> GetRoles();
        int AgregarUsuario(RegisterDto usuario);
        AuthResponseDto Login(LoginDto usuario);
        string RefreshToken(RefreshTokenDto usuario);
        void Logout(string token);
        UsuarioDto GetProfile(int Id);
        UsuarioDto AsignarRol(int Id, UsuarioRolDto usuario);
        UsuarioDto? ObtenerPorId(int id);
        bool ActualizarUsuario(int id, UsuarioUpdateDto usuario);
        bool EliminarUsuario(int id);
        
    }
}
