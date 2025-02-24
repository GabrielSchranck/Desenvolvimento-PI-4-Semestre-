using BookAPI.Data;
using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Clientes
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BookDbContext _context;

        public ClienteRepository(BookDbContext context)
        {
            this._context = context;
        }

        public void Create(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }
    }
}
