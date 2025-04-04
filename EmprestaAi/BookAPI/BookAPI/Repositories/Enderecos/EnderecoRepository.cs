using BookAPI.Data;
using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories.Enderecos
{
	public class EnderecoRepository : IEnderecoRepository
    {
        private readonly BookDbContext _context;

        public EnderecoRepository(BookDbContext context)
        {
            this._context = context;
        }

        public async Task CreateAsync(Endereco endereco)
        {
            await _context.Enderecos.AddAsync(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task CreateEnderecoClienteAsync(EnderecoCliente enderecoCliente)
        {
            await _context.EnderecosClientes.AddAsync(enderecoCliente);
            await _context.SaveChangesAsync();
        }

		public async Task DeleteEnderecoClienteAsync(EnderecoCliente enderecoCliente)
		{
			_context.Entry(enderecoCliente).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
		}

		public async Task<bool> FindEnderecoClienteAsync(EnderecoCliente enderecoCliente)
		{
			var result = await _context.EnderecosClientes
				.FirstOrDefaultAsync(e => e.ClienteId == enderecoCliente.ClienteId && e.EnderecoId == enderecoCliente.EnderecoId && e.Numero == enderecoCliente.Numero);

            if (result != null) return true;

			return false;
		}

		public async Task<Endereco> GetByCepAsync(string cep)
        {
            return await _context.Enderecos.FirstOrDefaultAsync(e => e.CodigoCep == cep);
        }

		public Task UpdateEnderecoAsync(Endereco endereco)
		{
			throw new NotImplementedException();
		}
	}
}
