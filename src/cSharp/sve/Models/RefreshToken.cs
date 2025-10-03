
namespace sve.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public DateTime Expira { get; set; }
        public bool EstaRevocado { get; set; } = false;

        // Relación con Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;
    }
}
