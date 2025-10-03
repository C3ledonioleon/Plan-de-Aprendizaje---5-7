using sve.DTOs;
using sve.Models;
using sve.Repositories.Contracts;
using sve.Services.Contracts;


namespace sve.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository _usuarioRepository)
        {
            _usuarioRepository = _usuarioRepository;
        }
        // 🔹 Registro
        public UsuarioDto Register(RegisterDto dto)
        {
            var usuario = new Usuario
            {
                Apodo = dto.Apodo,
                Email = dto.Email,
                contrasenia = dto.Contrasenia, // Ideal: encriptar
                Rol = RolUsuario.Cliente
            };

            _usuarioRepository.Add(usuario);

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Apodo = usuario.Apodo,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString()
            };
        }

        // 🔹 Login
        public AuthResponseDto Login(LoginDto dto)
        {
            var usuario = _usuarioRepository.GetByEmail(dto.Email);
            if (usuario == null || usuario.contrasenia != dto.Contrasenia)
                throw new Exception("Usuario o contraseña incorrectos");

            // Generar token
            var token = Guid.NewGuid().ToString(); // Para simplificar, en real usar JWT

            return new AuthResponseDto
            {
                Token = token,
                Usuario = new UsuarioDto
                {
                    IdUsuario = usuario.IdUsuario,
                    Apodo = usuario.Apodo,
                    Email = usuario.Email,
                    Rol = usuario.Rol.ToString()
                }
            };
        }

        // 🔹 Obtener perfil
        public UsuarioDto GetProfile(int usuarioId)
        {
            var usuario = _usuarioRepository.GetById(usuarioId);
            if (usuario == null) throw new Exception("Usuario no encontrado");

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Apodo = usuario.Apodo,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString()
            };
        }

        // 🔹 Obtener roles disponibles
        public List<string> GetRoles() =>
            Enum.GetNames(typeof(RolUsuario)).ToList();

        // 🔹 Asignar rol
        public UsuarioDto AsignarRol(int usuarioId, UsuarioRolDto dto)
        {
            var usuario = _usuarioRepository.GetById(usuarioId);
            if (usuario == null) throw new Exception("Usuario no encontrado");

            usuario.Rol = Enum.Parse<RolUsuario>(dto.Rol);
            _usuarioRepository.Update(usuario);

            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                Apodo = usuario.Apodo,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString()
            };
        }

        // 🔹 Refresh token (ejemplo simple)
        public string RefreshToken(RefreshTokenDto dto)
        {
            // Validar token existente, etc.
            // Para simplificar, generamos uno nuevo
            return Guid.NewGuid().ToString();
        }

        // 🔹 Logout
        public void Logout(string token)
        {
            // En una implementación real eliminar token de BD o invalidarlo
        }
        public List<UsuarioDto> ObtenerTodo()
{
    var usuarios = _usuarioRepository.GetAll();
    return usuarios.Select(u => new UsuarioDto {
        IdUsuario = u.IdUsuario,
        Apodo = u.Apodo,
        Email = u.Email,
        Rol = u.Rol.ToString()
    }).ToList();
}

public int AgregarUsuario(RegisterDto dto)
{
    var usuario = new Usuario
    {
        Apodo = dto.Apodo,
        Email = dto.Email,
        contrasenia = dto.Contrasenia,
        Rol = RolUsuario.Cliente
    };
    _usuarioRepository.Add(usuario);
    return usuario.IdUsuario;
}

public UsuarioDto? ObtenerPorId(int id)
{
    var usuario = _usuarioRepository.GetById(id);
    if (usuario == null) return null;
    return new UsuarioDto
    {
        IdUsuario = usuario.IdUsuario,
        Apodo = usuario.Apodo,
        Email = usuario.Email,
        Rol = usuario.Rol.ToString()
    };
}

public bool ActualizarUsuario(int id, UsuarioUpdateDto dto)
{
    var usuario = _usuarioRepository.GetById(id);
    if (usuario == null) return false;
    usuario.Apodo = dto.Apodo;
    usuario.Email = dto.Email;
    _usuarioRepository.Update(usuario);
    return true;
}

public bool EliminarUsuario(int id)
{
    var usuario = _usuarioRepository.GetById(id);
    if (usuario == null) return false;
    _usuarioRepository.Delete(id);
    return true;
}
    }
}
