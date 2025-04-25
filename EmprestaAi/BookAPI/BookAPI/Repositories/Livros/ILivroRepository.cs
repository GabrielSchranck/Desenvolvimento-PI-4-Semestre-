using BookAPI.Entities.Livros;

namespace BookAPI.Repositories.Livros
{
	public interface ILivroRepository
	{
		Task<IEnumerable<Livro>> GetAllAsync();
		Task CreateAsync(Livro livro);
		Task UpdateAsync(Livro livro);
		Task DeleteAsync(Livro livro);
	}
}
