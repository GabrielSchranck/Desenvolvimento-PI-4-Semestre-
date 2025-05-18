
using BookAPI.Data;
using BookAPI.Entities.Livros;

namespace BookAPI.Repositories.Livros
{
    public class LivroImagemRepository : ILivroImagemRepository
    {
        private readonly BookDbContext _dbContext;

        public LivroImagemRepository( BookDbContext bookDbContext)
        {
            this._dbContext = bookDbContext;
        }

        public async Task SaveImage(FotoLivro fotoLivro)
        {
            await _dbContext.FotosLivros.AddAsync(fotoLivro);
        }
    }
}
