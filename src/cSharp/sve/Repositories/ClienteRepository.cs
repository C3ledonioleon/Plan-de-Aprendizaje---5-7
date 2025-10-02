using sve.Models;
using sve.Repositories.Contracts;
using sve_api.Models;

namespace sve.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
       
        private readonly SveContext sveContext;

        public ClienteRepository(SveContext sveContext)
        {
        
            this.sveContext = sveContext;
        }

        public List<Cliente> GetAll()
        {
            return sveContext.Cliente.ToList();
        }

        public Cliente? GetById(int id)
        {
            return sveContext.Cliente.FirstOrDefault(x => x.IdCliente == id);
        }

        public int Add(Cliente cliente)
        {
            sveContext.Cliente.Add(cliente);
            sveContext.SaveChanges();
            return cliente.IdCliente;
        }

        public bool Update(int id, Cliente cliente)
        {
            cliente.IdCliente = id;
            sveContext.Cliente.Update(cliente);
            return sveContext.SaveChanges() > 0;
        }

        public bool Delete(int id)
        {
            var cliente = sveContext.Cliente.FirstOrDefault(x => x.IdCliente == id);
            sveContext.Cliente.Remove(cliente);
            return sveContext.SaveChanges() > 0;
        }
    }
}
