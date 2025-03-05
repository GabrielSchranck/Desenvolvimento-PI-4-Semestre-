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

        public async Task Create(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cliente>> GetAllClientAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<bool> GetByCpfAsync(string cpf)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);

            if (cliente == null)
                return false;

            return true;
        }

        public async Task<bool> GetByEmailAsync(string email)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

            if (cliente == null)
                return false;

            return true;
        }

		public async Task<Cliente> GetByIdAsync(int id)
		{
			return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
		}

		public async Task<Cliente> Login(string email, string senha)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email && c.Senha == senha);
        }
    }
}
