using BookAPI.Data;
using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Enderecos
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly BookDbContext _context;

        public EnderecoRepository(BookDbContext context)
        {
            this._context = context;
        }

        public async Task Create(Entities.CEPs.Endereco cep)
        {
            await _context.Enderecos.AddAsync(cep);
            await _context.SaveChangesAsync();
        }
    }
}
