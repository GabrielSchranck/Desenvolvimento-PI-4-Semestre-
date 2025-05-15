using BookAPI.Data;
using BookAPI.Entities.CEPs;
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
        public async Task CreateEnderecoCliente(EnderecoCliente enderecoCliente)
        {
            await _context.EnderecosClientes.AddAsync(enderecoCliente);
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

        public Task<Cliente> GetClienteByEmailAsync(string email)
        {
            return _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId)
        {
            return await _context.Enderecos.Where(c => c.EnderecosCliente.Any(e => e.ClienteId == clienteId)).Include(c => c.EnderecosCliente).ToListAsync();
        }
        public async Task<Cliente> Login(string email, string senha)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email && c.Senha == senha);
        }
        public async Task Update(Cliente cliente)
        {
            var clienteExistente = await _context.Clientes.FindAsync(cliente.Id);

            if(clienteExistente != null)
            {
                clienteExistente.Nome = cliente.Nome;
				clienteExistente.Email = cliente.Email;
				clienteExistente.DataNascimento = cliente.DataNascimento;
				clienteExistente.Contato = cliente.Contato;

                _context.Entry(clienteExistente).Property(c => c.Nome).IsModified = true;
				_context.Entry(clienteExistente).Property(c => c.Email).IsModified = true;
				_context.Entry(clienteExistente).Property(c => c.DataNascimento).IsModified = true;
                _context.Entry(clienteExistente).Property(c => c.Contato).IsModified = true;

				await _context.SaveChangesAsync();
			}
        }
    }
}
