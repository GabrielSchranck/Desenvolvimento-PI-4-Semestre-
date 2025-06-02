using BookAPI.Data;
using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Livros;
using BookModels.DTOs.Livros;
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

        public async Task AnunciarLivroAsync(LivroAnunciadoDTO livroAnunciadoDTO)
        {
            var anuncioExistente = await _dbContext.LivrosAnunciados
                .FirstOrDefaultAsync(la => la.LivroId == livroAnunciadoDTO.LivroId && la.ClienteId == livroAnunciadoDTO.ClienteId);

            bool ehTipoDiferente = false;

            var livroAnunciado = new LivroAnunciado();

            if (anuncioExistente == null)
            {
                livroAnunciado = new LivroAnunciado
                {
                    ClienteId = (int)livroAnunciadoDTO.ClienteId,
                    LivroId = (int)livroAnunciadoDTO.LivroId,
                    Tipo = (int)livroAnunciadoDTO.Tipo,
                    QuantidadeAnunciado = (int)livroAnunciadoDTO.QuantidadeAnunciado
                };
            }
            else
            {
                livroAnunciado = anuncioExistente;
                livroAnunciado.QuantidadeAnunciado += (int)livroAnunciadoDTO.QuantidadeAnunciado;

                if(livroAnunciado.Tipo != livroAnunciadoDTO.Tipo)
                {
                    ehTipoDiferente = true;
                    livroAnunciado.Id = 0;
                }
            }

            var livro = await _dbContext.Livros.FirstOrDefaultAsync(l => l.Id == livroAnunciado.LivroId);

            if (livro.Quantidade == 0) return;

            livro.Anunciado = true;
            livro.Quantidade = livro.Quantidade - (int)livroAnunciadoDTO.QuantidadeAnunciado;

            if (anuncioExistente == null || ehTipoDiferente) await _dbContext.LivrosAnunciados.AddAsync(livroAnunciado);
            else _dbContext.LivrosAnunciados.Update(livroAnunciado);

            _dbContext.UpdateRange(livro);
            _dbContext.SaveChanges();
        }

        public async Task CreateAsync(Livro livro, ClienteLivro clienteLivro)
        {
            if (livro == null || clienteLivro == null)
                return;

            var livroExistente = await _dbContext.Livros
                .FirstOrDefaultAsync(l => l.Titulo == livro.Titulo);

            if (livroExistente != null)
            {
                bool vinculoExiste = await _dbContext.ClientesLivros
                    .AnyAsync(cl => cl.ClienteId == clienteLivro.ClienteId && cl.LivroId == livroExistente.Id);

                if (!vinculoExiste)
                {
                    clienteLivro.LivroId = livroExistente.Id;
                    await _dbContext.ClientesLivros.AddAsync(clienteLivro);
                    await _dbContext.SaveChangesAsync();
                }

                return;
            }

            await _dbContext.Livros.AddAsync(livro);
            await _dbContext.SaveChangesAsync();

            clienteLivro.LivroId = livro.Id;
            await _dbContext.ClientesLivros.AddAsync(clienteLivro);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ClienteLivro clienteLivro)
        {
            var livroParaRemover = await _dbContext.Livros.Where(l => l.Id == clienteLivro.LivroId).FirstAsync();
            var itensParaRemover = await _dbContext.ClientesLivros.Where(cl => cl.ClienteId == clienteLivro.ClienteId && cl.LivroId == clienteLivro.LivroId).FirstAsync();
            var imagemLivroParaRemover = await _dbContext.FotosLivros.Where(fl => fl.LivroId == clienteLivro.LivroId).FirstAsync();

            _dbContext.ClientesLivros.RemoveRange(itensParaRemover);
            _dbContext.Livros.Remove(livroParaRemover);
            _dbContext.FotosLivros.Remove(imagemLivroParaRemover);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<LivroDTO>> GetAllAsync(int clientId)
        {
            var clientesLivro = await _dbContext.ClientesLivros
                .Where(cl => cl.ClienteId == clientId)
                .ToListAsync();

            var livrosIds = clientesLivro.Select(cl => cl.LivroId).ToList();

            if (!livrosIds.Any())
                return Enumerable.Empty<LivroDTO>();

            var livrosCliente = await _dbContext.Livros
                .Where(l => livrosIds.Contains(l.Id))
                .ToListAsync();

            var imagensLivro = await _dbContext.FotosLivros
                .Where(img => livrosIds.Contains(img.LivroId))
                .GroupBy(img => img.LivroId)
                .Select(g => g.FirstOrDefault())
                .ToListAsync();

            var livrosAnunciados = await _dbContext.LivrosAnunciados
                .Where(la => la.ClienteId == clientId && la.QuantidadeAnunciado > 0)
                .ToListAsync();

            var anunciosAgrupados = livrosAnunciados
                .GroupBy(la => la.LivroId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(la => new LivroAnunciadoDTO
                    {
                        ClienteId = clientId,
                        LivroId = la.LivroId,
                        Tipo = la.Tipo,
                        QuantidadeAnunciado = la.QuantidadeAnunciado
                    }).ToList()
                );

            var livrosDto = livrosCliente
                .Select(l => new LivroDTO
                {
                    Id = l.Id,
                    CategoriaId = l.CategoriaId,
                    Titulo = l.Titulo,
                    Valor = l.Valor,
                    Custo = l.Custo,
                    QtdPaginas = l.QtdPaginas,
                    Quantidade = l.Quantidade,
                    ClienteId = clientId,
                    UriImagemLivro = imagensLivro.FirstOrDefault(img => img.LivroId == l.Id)?.UrlImagem,
                    Anunciado = livrosAnunciados.Any(la => la.LivroId == l.Id),
                    LivrosAnunciados = anunciosAgrupados.ContainsKey(l.Id)
                        ? anunciosAgrupados[l.Id]
                        : new List<LivroAnunciadoDTO>()
                })
                .ToList();

            return livrosDto;
        }



        public async Task<IEnumerable<Categoria>> GetCategorias()
        {
            return await _dbContext.Categorias.ToListAsync();
        }

        public async Task UpdateAsync(Livro livro)
        {
            if (livro != null)
            {
                var newBook = await _dbContext.Livros.FirstOrDefaultAsync(l => l.Id == livro.Id);

                if (newBook != null)
                {
                    newBook.Titulo = livro.Titulo;
                    newBook.Valor = livro.Valor;
                    newBook.Quantidade = livro.Quantidade;
                    newBook.Custo = livro.Custo;
                    newBook.Categoria = livro.Categoria;

                    _dbContext.Livros.Update(newBook);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
