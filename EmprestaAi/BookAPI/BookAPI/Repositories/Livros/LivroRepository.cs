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

		public async Task CreateAsync(Livro livro)
		{
			if(livro != null)
			{
				await _dbContext.AddAsync(livro);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(Livro livro)
		{
			if(livro != null)
			{
				_dbContext.Remove(livro);
				await _dbContext.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<Livro>> GetAllAsync()
		{
			return await _dbContext.Livros.ToListAsync();
		}

		public async Task UpdateAsync(Livro livro)
		{
			if(livro != null)
			{
				var newBook = await _dbContext.Livros.FirstOrDefaultAsync(l => l.Id == livro.Id);

				if(newBook != null)
				{
					newBook.Valor = livro.Valor;
					newBook.Quantidade = livro.Quantidade;
					newBook.Custo = livro.Custo;

					_dbContext.Livros.Update(newBook);

					await _dbContext.SaveChangesAsync();
				}
			}
		}
	}
}
