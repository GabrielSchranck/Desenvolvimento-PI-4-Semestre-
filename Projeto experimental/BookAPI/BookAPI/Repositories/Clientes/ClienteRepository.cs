using BookAPI.Data;
using BookAPI.Entities.Clientes;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Cliente>> GetAllClientAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente> Login(string email, string senha)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email && c.Senha == senha);
        }
    }
}
