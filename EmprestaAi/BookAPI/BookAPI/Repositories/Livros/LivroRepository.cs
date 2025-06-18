using BookAPI.Data;
using BookAPI.Entities.Carrinhos;
using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Livros;
using BookModels.DTOs.Clientes;
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
                .FirstOrDefaultAsync(la => la.LivroId == livroAnunciadoDTO.LivroId && la.ClienteId == livroAnunciadoDTO.ClienteId && la.Tipo == livroAnunciadoDTO.Tipo);

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

                if (livroAnunciado.Tipo != livroAnunciadoDTO.Tipo)
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

        public async Task CancelarAnuncioAsync(LivroAnunciadoDTO livroAnunciadoDTO)
        {
            var anunciado = await _dbContext.LivrosAnunciados.Where(la => la.Id == livroAnunciadoDTO.Id).FirstOrDefaultAsync();
            var livro = await _dbContext.Livros.Where(l => l.Id == anunciado.LivroId).FirstOrDefaultAsync();

            anunciado.QuantidadeAnunciado -= (int)livroAnunciadoDTO.QuantidadeAnunciado;
            livro.Quantidade += (int)livroAnunciadoDTO.QuantidadeAnunciado;

            _dbContext.LivrosAnunciados.Update(anunciado);
            _dbContext.Livros.Update(livro);

            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateAsync(Livro livro, ClienteLivro clienteLivro)
        {
            if (livro == null || clienteLivro == null)
                return;

            var livroExistente = await _dbContext.Livros
                .FirstOrDefaultAsync(l => l.Titulo == livro.Titulo);

            livroExistente = null;

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
            var livrosAnunciados = await _dbContext.LivrosAnunciados.Where(la => la.LivroId == clienteLivro.LivroId).ToListAsync();

            var itensCarrinho = new List<ItemCarrinho>();

            foreach (var anuncio in livrosAnunciados)
            {
                var itemCarrinho = await _dbContext.ItemCarrinho.Where(i => i.LivroAnunciadoId == anuncio.Id).FirstOrDefaultAsync();

                if (itemCarrinho is null) continue;

                itensCarrinho.Add(itemCarrinho);
            }

            _dbContext.ClientesLivros.RemoveRange(itensParaRemover);
            _dbContext.FotosLivros.Remove(imagemLivroParaRemover);

            foreach (var item in itensCarrinho)
            {
                _dbContext.ItemCarrinho.Remove(item);
            }

            _dbContext.Livros.Remove(livroParaRemover);

            foreach (var anuncio in livrosAnunciados)
            {
                _dbContext.LivrosAnunciados.Remove(anuncio);
            }

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
                        Id = la.Id,
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

        public async Task<IEnumerable<LivroDTO>> SelecionarAnuncios()
        {
            var livros = await _dbContext.Livros
                .Include(l => l.livrosAnunciados)
                    .ThenInclude(la => la.Cliente)
                .Include(l => l.Categoria)
                .Include(l => l.FotosLivros)
                .Where(l => l.livrosAnunciados.Any(la => la.QuantidadeAnunciado > 0))
                .ToListAsync();

            var livroDTOs = livros.Select(l => new LivroDTO
            {
                Id = l.Id,
                ClienteId = l.Cliente?.Id,
                CategoriaId = l.CategoriaId,
                Titulo = l.Titulo,
                Valor = l.Valor,
                Custo = l.Custo,
                QtdPaginas = l.QtdPaginas,
                Quantidade = l.Quantidade,
                Anunciado = l.Anunciado,
                UriImagemLivro = l.FotosLivros.FirstOrDefault()?.UrlImagem,
                LivrosAnunciados = l.livrosAnunciados
                    .Where(la => la.QuantidadeAnunciado > 0)
                    .Select(la => new LivroAnunciadoDTO
                    {
                        Id = la.Id,
                        ClienteId = la.ClienteId,
                        LivroId = la.LivroId,
                        QuantidadeAnunciado = la.QuantidadeAnunciado,
                        Tipo = la.Tipo,
                        ClienteDTO = la.Cliente != null ? new ClienteDTO
                        {
                            Id = la.Cliente.Id,
                        } : null
                    }).ToList()
            }).ToList();

            return livroDTOs;
        }

        public async Task<LivroAnunciadoDTO> GetAnuncioDTO(int livroId, int tipo)
        {
            var anuncio = await _dbContext.LivrosAnunciados.Where(l => l.LivroId == livroId && l.Tipo == tipo).FirstOrDefaultAsync();
            var livro = await _dbContext.Livros.Where(l => l.Id == anuncio.LivroId).FirstOrDefaultAsync();
            var cliente = await _dbContext.Clientes.Where(c => c.Id == anuncio.ClienteId).FirstOrDefaultAsync();
            var fotoLivro = await _dbContext.FotosLivros.Where(fl => fl.LivroId == livro.Id).FirstOrDefaultAsync();
            var categoria = await _dbContext.Categorias.Where(ct => ct.Id == livro.CategoriaId).FirstOrDefaultAsync();

            return new LivroAnunciadoDTO
            {
                Id = anuncio.Id,
                ClienteDTO = new ClienteDTO
                {
                    Id = cliente.Id,
                    Nome = cliente.Nome
                },
                ClienteId = cliente.Id,
                LivroDTO = new LivroDTO
                {
                    Id = livro.Id,
                    Titulo = livro.Titulo,
                    UriImagemLivro = fotoLivro.UrlImagem,
                    categoria = categoria.NomeCategoria,
                    Valor = livro.Valor,
                    QtdPaginas = livro.QtdPaginas,
                    Quantidade = livro.Quantidade,
                    CategoriaId = livro.CategoriaId
                },
                LivroId = cliente.Id,
                QuantidadeAnunciado = anuncio.QuantidadeAnunciado,
                Tipo = anuncio.Tipo
            };
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
                    newBook.CategoriaId = livro.CategoriaId;

                    _dbContext.Livros.Update(newBook);

                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        public async Task<IEnumerable<LivroDTO>> GetAllByCategoria(int categoriaId, int livroId, int tipo)
        {
            var livros = await _dbContext.Livros
                .Include(l => l.livrosAnunciados)
                    .ThenInclude(la => la.Cliente)
                .Include(l => l.Categoria)
                .Include(l => l.FotosLivros)
                .Where(l =>
                    l.CategoriaId == categoriaId &&
                    l.Id != livroId &&
                    l.livrosAnunciados.Any(la => la.QuantidadeAnunciado > 0 && la.Tipo == tipo)
                )
                .OrderBy(r => Guid.NewGuid())
                .Take(10)
                .ToListAsync();

            var livroDTOs = livros.Select(l => new LivroDTO
            {
                Id = l.Id,
                ClienteId = l.Cliente?.Id,
                CategoriaId = l.CategoriaId,
                Titulo = l.Titulo,
                Valor = l.Valor,
                Custo = l.Custo,
                QtdPaginas = l.QtdPaginas,
                Quantidade = l.Quantidade,
                Anunciado = l.Anunciado,
                UriImagemLivro = l.FotosLivros.FirstOrDefault()?.UrlImagem,
                LivrosAnunciados = l.livrosAnunciados
                    .Where(la => la.QuantidadeAnunciado > 0 && la.Tipo == 0)
                    .Select(la => new LivroAnunciadoDTO
                    {
                        Id = la.Id,
                        ClienteId = la.ClienteId,
                        LivroId = la.LivroId,
                        QuantidadeAnunciado = la.QuantidadeAnunciado,
                        Tipo = la.Tipo,
                        ClienteDTO = la.Cliente != null ? new ClienteDTO
                        {
                            Id = la.Cliente.Id,
                        } : null
                    }).ToList()
            }).ToList();

            return livroDTOs;
        }

        public async Task<IEnumerable<LivroEmprestadoDTO>> GetLivrosEmprestados(int clienteId)
        {
            var livrosEmprestados = await _dbContext.LivrosEmprestados
                .Include(le => le.Livro)
                    .ThenInclude(l => l.FotosLivros)
                .Include(le => le.Vendedor)
                .Include(le => le.Comprador)
                .Where(le => le.CompradorId == clienteId)
                .ToListAsync();

            var livroEmprestadoDTOs = livrosEmprestados.Select(le => new LivroEmprestadoDTO
            {
                Id = le.Id,
                LivroId = le.LivroId,
                VendedorId = le.VendedorId,
                CompradorId = le.CompradorId,
                DataEmprestimo = le.DataEmprestimo,
                DataDevolucao = le.DataDevolucao,
                Devolvido = le.Devolvido,
                Livro = le.Livro != null ? new LivroDTO
                {
                    Id = le.Livro.Id,
                    Titulo = le.Livro.Titulo,
                    UriImagemLivro = le.Livro.FotosLivros.FirstOrDefault()?.UrlImagem
                } : null,
                Vendedor = le.Vendedor != null ? new ClienteDTO
                {
                    Id = le.Vendedor.Id,
                    Nome = le.Vendedor.Nome
                } : null,
                Comprador = le.Comprador != null ? new ClienteDTO
                {
                    Id = le.Comprador.Id,
                    Nome = le.Comprador.Nome
                } : null
            }).ToList();

            return livroEmprestadoDTOs;
        }

        public async Task DevolverLivro(int livroId)
        {
            var livro = await _dbContext.Livros.FindAsync(livroId);
            var livroEmprestado = await _dbContext.LivrosEmprestados.Where(le => le.LivroId == livroId).FirstOrDefaultAsync();
            var clienteLivro = await _dbContext.ClientesLivros.Where(cl => cl.LivroId == livroId).FirstOrDefaultAsync();
            var comprador = await _dbContext.Clientes.FindAsync(livroEmprestado.CompradorId);
            var vendedor = await _dbContext.Clientes.FindAsync(livroEmprestado.VendedorId);

            livro.Quantidade += 1;

            _dbContext.LivrosEmprestados.Remove(livroEmprestado);
            _dbContext.Livros.Update(livro);

            await _dbContext.SaveChangesAsync();
        }

        public async Task AdicionarComentario(ComentarioLivroDTO comentarioLivroDTO)
        {
            var cliente = await _dbContext.Clientes.FindAsync(comentarioLivroDTO.ClienteId);
            var livro = await _dbContext.Livros.FindAsync(comentarioLivroDTO.LivroId);

            var comentario = new ComentarioLivro
            {
                Id = 0,
                Cliente = cliente,
                ClienteId = cliente.Id,
                Comentario = comentarioLivroDTO.Comentario,
                DataComentario = DateTime.Now,
                Livro = livro,
                LivroId = livro.Id
            };

            await _dbContext.ComentariosLivros.AddAsync(comentario);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ComentarioLivroDTO>> GetComentarios(int livroId)
        {
            var comentarios = await _dbContext.ComentariosLivros
                .Where(cl => cl.LivroId == livroId)
                .Include(cl => cl.Cliente) // se quiser trazer o cliente
                .Include(cl => cl.Livro)   // se quiser trazer o livro
                .ToListAsync();

            if (comentarios == null || !comentarios.Any())
                return Enumerable.Empty<ComentarioLivroDTO>();

            return comentarios.Select(c => new ComentarioLivroDTO
            {
                Id = c.Id,
                ClienteId = c.ClienteId,
                LivroId = c.LivroId,
                Comentario = c.Comentario,
                DataComentario = c.DataComentario,
                ClienteDTO = c.Cliente != null ? new ClienteDTO
                {
                    Id = c.Cliente.Id,
                    Nome = c.Cliente.Nome,
                } : null,
                LivroDTO = c.Livro != null ? new LivroDTO
                {
                    Id = c.Livro.Id,
                    Titulo = c.Livro.Titulo,
                } : null
            });
        }

        public async Task ExcluirComentario(int comentarioId)
        {
            var comentario = await _dbContext.ComentariosLivros.FindAsync(comentarioId);
            _dbContext.Remove(comentario);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditarComentario(ComentarioLivroDTO comentarioLivroDTO)
        {
            var comentario = await _dbContext.ComentariosLivros.FindAsync(comentarioLivroDTO.Id);
            comentario.Comentario = comentarioLivroDTO.Comentario;

            _dbContext.ComentariosLivros.Update(comentario);
            await _dbContext.SaveChangesAsync();
        }
    }
}
