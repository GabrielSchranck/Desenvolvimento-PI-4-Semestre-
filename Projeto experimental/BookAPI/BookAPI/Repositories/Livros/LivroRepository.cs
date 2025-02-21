using BookAPI.Data;
using BookAPI.Entities.Livros;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories.Livros
{
	public class LivroRepository : ILivroRepository
	{
		private readonly BookDbContext _dbContext;

        public LivroRepository(BookDbContext dbContext)
        {
			this._dbContext = dbContext;
        }

        public async Task<Livro> GetItem(int id)
		{
			return await _dbContext.Livros.FirstOrDefaultAsync(l => l.Id == id);
		}
	}
}
