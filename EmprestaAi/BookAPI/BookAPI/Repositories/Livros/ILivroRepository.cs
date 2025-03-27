using BookAPI.Entities.Livros;

namespace BookAPI.Repositories.Livros
{
	public interface ILivroRepository
	{
		Task<Livro> GetItem(int id);
	}
}
