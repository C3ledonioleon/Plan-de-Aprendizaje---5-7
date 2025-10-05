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
        public UsuarioDto Register(RegisterDto usario)
        {
            var usuario = new Usuario
            {
                Apodo = usario.Apodo,
                Email = usario.Email,
                contrasenia = usario.Contrasenia, // Ideal: encriptar
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
        public UsuarioDto GetProfile(int Id)
        {
            var usuario = _usuarioRepository.GetById(Id);
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
        public UsuarioDto AsignarRol(int id, UsuarioRolDto usuario)
        {
            var usuarioExistente = _usuarioRepository.GetById(id);
            if (usuario == null) throw new Exception("Usuario no encontrado");

            usuarioExistente.Rol = usuarioExistente.Rol;
            _usuarioRepository.Update(usuarioExistente.IdUsuario, usuarioExistente);
  
            return new UsuarioDto
            {
                IdUsuario = usuarioExistente.IdUsuario,
                Apodo = usuarioExistente.Apodo,
                Email = usuarioExistente.Email,
                Rol = usuarioExistente.Rol.ToString()
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
        public int ActualizarUsuario(int id, UsuarioUpdateDto usuario)
        {
            var usuarioExistente = _usuarioRepository.GetById(id);
            if (usuario == null) return 0;
              usuarioExistente.Apodo = usuarioExistente.Apodo;
              usuarioExistente.Email = usuarioExistente.Email;
             _usuarioRepository.Update(usuarioExistente.IdUsuario, usuarioExistente);
            return 1;
        }
        public int EliminarUsuario(int id)
        {
            var usuario = _usuarioRepository.GetById(id);
            if (usuario == null) return 0;
            _usuarioRepository.Delete(id);
            return 1;
        }
            }
        }
