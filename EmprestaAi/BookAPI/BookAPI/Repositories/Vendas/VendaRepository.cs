using BookAPI.Data;
using BookAPI.Entities.Clientes;
using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Historicos;
using BookAPI.Entities.Livros;
using BookAPI.Entities.Notificacoes;
using BookAPI.Services.Vendas;
using BookModels.DTOs.Livros;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;

namespace BookAPI.Repositories.Vendas
{
    public class VendaRepository : IVendaRepository
    {
        private readonly BookDbContext _dbContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public VendaRepository(BookDbContext dbContext, IHubContext<NotificationHub> hubContext)
        {
            this._dbContext = dbContext;
            this._hubContext = hubContext;
        }

        public async Task ChangeSaldo(Session session)
        {
            var pagamento = await _dbContext.Pagamentos
            .FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);

            var cliente = await _dbContext.Clientes.Where(c => c.Id == pagamento.ClienteId).FirstOrDefaultAsync();

            cliente.Saldo += (double)pagamento.Valor;

            _dbContext.Clientes.Update(cliente);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ChangeStatus(string status, Session session)
        {
            var pagamento = await _dbContext.Pagamentos
            .FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);

            if (pagamento == null) return false;

            pagamento.Status = status;
            pagamento.StripePaymentIntentId = session.PaymentIntentId;

            _dbContext.Pagamentos.Update(pagamento);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> OperacaoLivro(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO)
        {
            bool clienteJaPossuiLivro = false;
            Livro livroEncontradoComprador = null;

            var vendedor = await _dbContext.Clientes.Where(c => c.Id == livroAnunciadoDTO.ClienteId).FirstOrDefaultAsync();
            var comprador = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();
            var livro = await _dbContext.Livros.Where(l => l.Id == livroAnunciadoDTO.LivroDTO.Id).FirstOrDefaultAsync();
            var livroVendedor = await _dbContext.ClientesLivros.Where(cl => cl.LivroId == livro.Id && cl.ClienteId == vendedor.Id).FirstOrDefaultAsync();
            var livroComprador = await _dbContext.ClientesLivros.Where(cl => cl.ClienteId == comprador.Id).ToListAsync();
            var livroAnunciado = await _dbContext.LivrosAnunciados.Where(la => la.LivroId == livro.Id).FirstOrDefaultAsync();
            var fotoLivro = await _dbContext.FotosLivros.Where(fl => fl.LivroId == livro.Id).FirstOrDefaultAsync();

            foreach (var livroCliente in livroComprador)
            {
                var livroEncontrado = await _dbContext.Livros.Where(l => l.Id == livroCliente.LivroId).FirstOrDefaultAsync();

                if (livroEncontrado.Titulo == livro.Titulo)
                {
                    clienteJaPossuiLivro = true;
                    livroEncontradoComprador = livroEncontrado;
                }
            }

            livroAnunciado.QuantidadeAnunciado -= (int)livroAnunciadoDTO.qtdOperacao;

            _dbContext.LivrosAnunciados.Update(livroAnunciado);
            await _dbContext.SaveChangesAsync();

            if (vendedor == null || comprador == null) return false;

            if (vendedor.Id == comprador.Id)
            {
                livro.Quantidade += (int)livroAnunciadoDTO.qtdOperacao;

                _dbContext.Livros.Update(livro);
                _dbContext.LivrosAnunciados.Update(livroAnunciado);

                await _dbContext.SaveChangesAsync();

                return false;
            }

            if (!clienteJaPossuiLivro)
            {
                var newLivro = new Livro
                {
                    Id = 0,
                    QtdPaginas = livro.QtdPaginas,
                    Quantidade = (int)livroAnunciadoDTO.qtdOperacao,
                    Titulo = livro.Titulo,
                    Valor = livro.Valor,
                    Anunciado = false,
                    Categoria = livro.Categoria,
                    CategoriaId = livro.CategoriaId,
                    Custo = livro.Custo
                };


                await _dbContext.Livros.AddAsync(newLivro);
                await _dbContext.SaveChangesAsync();

                var clienteLivro = new ClienteLivro
                {
                    Cliente = comprador,
                    ClienteId = comprador.Id,
                    Livro = newLivro,
                    LivroId = newLivro.Id,
                    Id = 0
                };

                await _dbContext.ClientesLivros.AddRangeAsync(clienteLivro);
                await _dbContext.SaveChangesAsync();

                var nfotoLivro = new FotoLivro
                {
                    Id = 0,
                    Livro = newLivro,
                    LivroId = newLivro.Id,
                    UrlImagem = fotoLivro.UrlImagem
                };

                await _dbContext.FotosLivros.AddAsync(nfotoLivro);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                livroEncontradoComprador.Quantidade += (int)livroAnunciadoDTO.qtdOperacao;
                _dbContext.Livros.Update(livroEncontradoComprador);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> OperacaoVenda(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO)
        {
            var vendedor = await _dbContext.Clientes.Where(c => c.Id == livroAnunciadoDTO.ClienteId).FirstOrDefaultAsync();
            var comprador = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

            if (vendedor == null || comprador == null) return false;

            if (vendedor.Id == comprador.Id) return true;

            var valorCompra = (livroAnunciadoDTO.LivroDTO.Valor * livroAnunciadoDTO.qtdOperacao) + (livroAnunciadoDTO.valorTaxa * livroAnunciadoDTO.qtdOperacao);

            if (vendedor.Saldo is null) vendedor.Saldo = 0;
            if (comprador.Saldo is null) comprador.Saldo = 0;

            vendedor.Saldo += (double)valorCompra;
            comprador.Saldo -= (double)valorCompra;

            _dbContext.Clientes.Update(vendedor);
            _dbContext.Clientes.Update(comprador);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SaveHistorico(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO)
        {
            var livro = await _dbContext.Livros.Where(l => l.Id == livroAnunciadoDTO.LivroDTO.Id).FirstOrDefaultAsync();

            //var historico = new Historico
            //{
            //    ClienteId = clienteId,
            //    DataHora = DateTime.UtcNow,
            //    TipoOperacao = livroAnunciadoDTO.Tipo,
            //    Id = 0
            //};

            //await _dbContext.Historicos.AddAsync(historico);
            //await _dbContext.SaveChangesAsync();

            //var itemHistorico = new ItemHistorico
            //{
            //    Id = 0,
            //    LivroId = (int)livroAnunciadoDTO.LivroId,
            //    Quantidade = (int)livroAnunciadoDTO.qtdOperacao,
            //    Valor = (decimal)(livroAnunciadoDTO.valorTaxa + livro.Valor),
            //    HistoricoId = historico.Id,
            //};

            //await _dbContext.ItensHistoricos.AddAsync(itemHistorico);
            //await _dbContext.SaveChangesAsync();

            var vendedor = await _dbContext.Clientes.Where(v => v.Id == livroAnunciadoDTO.ClienteId).FirstOrDefaultAsync();

            if(livroAnunciadoDTO.Tipo == 0)
            {
                var comprador = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

                var notificacao = new Notificacao
                {
                    Id = 0,
                    Comprador = comprador,
                    Vendedor = vendedor,
                    Visto = 0,
                    CompradorId = clienteId,
                    Mensagem = $"O seu livro '{livro.Titulo}' foi comprado por {comprador.Nome}",
                    Notificado = 0,
                    Tipo = TipoOperacaoEnum.Venda,
                    VendedorId = (int)livroAnunciadoDTO.ClienteId
                };

                await _dbContext.Notificacoes.AddAsync(notificacao);
                await _dbContext.SaveChangesAsync();

                await _hubContext.Clients.User(notificacao.VendedorId.ToString()).SendAsync("ReceiveNotification", notificacao.Mensagem);
            }

            return true;
        }

        public async Task<bool> SavePayment(Pagamento pagamento)
        {
            var cliente = await _dbContext.Clientes.Where(c => c.Id == pagamento.ClienteId).FirstOrDefaultAsync();

            pagamento.Cliente = cliente;

            await _dbContext.Pagamentos.AddAsync(pagamento);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
