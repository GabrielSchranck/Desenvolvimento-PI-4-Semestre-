
using BookAPI.Data;
using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Livros;
using Microsoft.EntityFrameworkCore;

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
            var livroExistente = await _dbContext.Livros
                .FirstOrDefaultAsync(l => l.Id == fotoLivro.LivroId);

            if (livroExistente != null)
            {
                bool vinculoExiste = await _dbContext.FotosLivros
                    .AnyAsync(fl => fl.LivroId == livroExistente.Id);

                if (!vinculoExiste)
                {
                    await _dbContext.FotosLivros.AddAsync(fotoLivro);
                    await _dbContext.SaveChangesAsync();
                }

                return;
            }

            await _dbContext.FotosLivros.AddAsync(fotoLivro);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(FotoLivro livroFoto)
        {
            var newFotoLivro = await _dbContext.FotosLivros.Where(fl => fl.LivroId == livroFoto.LivroId).FirstOrDefaultAsync();

            if(newFotoLivro != null)
            {
                newFotoLivro.UrlImagem = livroFoto.UrlImagem;

                _dbContext.FotosLivros.Update(newFotoLivro);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
