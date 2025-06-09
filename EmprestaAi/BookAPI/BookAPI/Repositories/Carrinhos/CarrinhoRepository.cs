using Azure.Core;
using BookAPI.Data;
using BookAPI.Entities.Carrinhos;
using BookAPI.Entities.Livros;
using BookModels.DTOs.Carrinhos;
using BookModels.DTOs.Clientes;
using BookModels.DTOs.Livros;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookAPI.Repositories.Carrinhos
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly BookDbContext _dbContext;

        public CarrinhoRepository(BookDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<bool> AddItemCarrinho(ItensCarrinhoDTO itensCarrinhoDTO, int clienteId, int tipo)
        {
            var carrinho = await _dbContext.Carrinho.Where(c => c.ClienteId == clienteId).FirstOrDefaultAsync();
            var livroAnunciado = await _dbContext.LivrosAnunciados.Where(la => la.Id == itensCarrinhoDTO.LivroAnunciadoDTO.Id).FirstOrDefaultAsync();
            var clienteLivro = await _dbContext.Clientes.Where(c => c.Id == livroAnunciado.ClienteId).FirstOrDefaultAsync();
            var livro = await _dbContext.Livros.Where(l => l.Id == livroAnunciado.LivroId).FirstOrDefaultAsync();
            var itemCarrinhoExistente = await _dbContext.ItemCarrinho.Where(ic => ic.CarrinhoId == carrinho.Id && ic.LivroAnunciadoId == livroAnunciado.Id && livroAnunciado.Tipo == tipo).FirstOrDefaultAsync();

            if (!(itemCarrinhoExistente is null)) return false;

            var itemCarrinho = new ItemCarrinho
            {
                Carrinho = carrinho,
                CarrinhoId = carrinho.Id,
                LivroAnunciado = livroAnunciado,
                LivroAnunciadoId = livroAnunciado.Id
            };

            await _dbContext.ItemCarrinho.AddAsync(itemCarrinho);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task Create(int clienteId)
        {
            var carrinhoExistente = await _dbContext.Carrinho.Where(c => c.ClienteId == clienteId).FirstOrDefaultAsync();

            if (carrinhoExistente is null)
            {
                var cliente = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

                var carrinho = new Carrinho
                {
                    ClienteId = clienteId
                };

                cliente.Carrinho = carrinho;

                await _dbContext.Carrinho.AddAsync(carrinho);
                _dbContext.Clientes.Update(cliente);

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<CarrinhoDTO> GetCarrinhoAsync(int clienteId)
        {
            var carrinho = await _dbContext.Carrinho
                .Include(c => c.Itens)
                    .ThenInclude(ic => ic.LivroAnunciado)
                        .ThenInclude(la => la.Livro)
                .Include(c => c.Itens)
                    .ThenInclude(ic => ic.LivroAnunciado)
                        .ThenInclude(la => la.Cliente)
                .Include(c => c.Itens)
                    .ThenInclude(ic => ic.LivroAnunciado)
                        .ThenInclude(la => la.Livro.FotosLivros)
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId);

            if (carrinho == null)
                return null;

            var itensDTO = carrinho.Itens.Select(ic =>
            {
                var livroAnunciado = ic.LivroAnunciado;
                var livro = livroAnunciado.Livro;
                var cliente = livroAnunciado.Cliente;
                var imagem = livro.FotosLivros.FirstOrDefault();

                return new ItensCarrinhoDTO
                {
                    Id = ic.Id,
                    CarrinhoDTO = new CarrinhoDTO
                    {
                        Id = carrinho.Id
                    },
                    LivroAnunciadoDTO = new LivroAnunciadoDTO
                    {
                        Id = livroAnunciado.Id,
                        Tipo = livroAnunciado.Tipo,
                        ClienteId = livroAnunciado.ClienteId,
                        LivroId = livroAnunciado.LivroId,
                        QuantidadeAnunciado = livroAnunciado.QuantidadeAnunciado,
                        ClienteDTO = new ClienteDTO
                        {
                            Id = cliente.Id,
                            Nome = cliente.Nome
                        },
                        LivroDTO = new LivroDTO
                        {
                            Id = livro.Id,
                            Titulo = livro.Titulo,
                            Valor = livro.Valor,
                            UriImagemLivro = imagem?.UrlImagem
                        }
                    }
                };
            }).Where(i => i.LivroAnunciadoDTO.QuantidadeAnunciado > 0).ToList();

            return new CarrinhoDTO
            {
                ClienteId = clienteId,
                Id = carrinho.Id,
                Itens = itensDTO
            };
        }

        public async Task<bool> RemoveFromCarrinhoAsync(ItensCarrinhoDTO item)
        {
            var itemExistente = await _dbContext.ItemCarrinho.FindAsync(item.Id);

            if (itemExistente == null) return false;

            _dbContext.ItemCarrinho.Remove(itemExistente);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Verificar(int clienteId)
        {
            return await _dbContext.Carrinho.AnyAsync(c => c.ClienteId == clienteId);
        }
    }
}
