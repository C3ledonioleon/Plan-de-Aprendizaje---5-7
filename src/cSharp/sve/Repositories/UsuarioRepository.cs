using System.Collections.Generic;
using System.Linq;
using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly SveContext sveContext;

        public UsuarioRepository(SveContext context)
        {
            sveContext = context;
        }

        public List<Usuario> GetAll()
        {
            return sveContext.Usuario.ToList();
        }

        public Usuario? GetById(int id)
        {
            return sveContext.Usuario.FirstOrDefault(x => x.IdUsuario == id);
        }

        public Usuario? GetByEmail(string email)
        {
            return sveContext.Usuario.FirstOrDefault(x => x.Email == email);
        }

        public int Add(Usuario usuario)
        {
            sveContext.Usuario.Add(usuario);
            sveContext.SaveChanges();
            return usuario.IdUsuario; // EF Core genera automáticamente el Id
        }

        public bool Update(Usuario usuario)
        {
            sveContext.Usuario.Update(usuario);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var usuario = sveContext.Usuario.FirstOrDefault(x => x.IdUsuario == id);
            if (usuario == null) return false;
            sveContext.Usuario.Remove(usuario);
            return sveContext.SaveChanges() > 0;
        }
    }
}
