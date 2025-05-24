using BookAPI.Entities.Livros;

namespace BookAPI.Repositories.Livros
{
    public interface ILivroImagemRepository
    {
        Task SaveImage(FotoLivro fotoLivro);
    }
}
