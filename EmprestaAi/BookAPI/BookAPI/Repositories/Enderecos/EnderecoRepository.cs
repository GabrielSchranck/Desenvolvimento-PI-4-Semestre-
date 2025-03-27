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

        public async Task Create(Cep cep)
        {
            await _context.Ceps.AddAsync(cep);
            await _context.SaveChangesAsync();
        }
    }
}
