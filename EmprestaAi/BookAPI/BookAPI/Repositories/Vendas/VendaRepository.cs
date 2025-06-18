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

        public Task<bool> OperacaoDoacao(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> OperacaoEmprestimo(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO)
        {
            var livro = await _dbContext.Livros.FirstOrDefaultAsync(l => l.Id == livroAnunciadoDTO.LivroDTO.Id);
            var comprador = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == clienteId);
            var vendedor = await _dbContext.Clientes.FirstOrDefaultAsync(c => c.Id == livroAnunciadoDTO.ClienteId);
            var livroAnunciado = await _dbContext.LivrosAnunciados.FirstOrDefaultAsync(al => al.Id == livroAnunciadoDTO.Id);
            var clienteLivro = await _dbContext.ClientesLivros.FirstOrDefaultAsync(cl => cl.ClienteId == vendedor.Id && cl.LivroId == livro.Id);
            var fotoLivro = await _dbContext.FotosLivros.FirstOrDefaultAsync(fl => fl.LivroId == livro.Id);
            var notificacoes = await _dbContext.Notificacoes.Where(n => n.LivroId == livro.Id).ToListAsync();

            if (livro == null || comprador == null || vendedor == null || livroAnunciado == null || clienteLivro == null)
                return false;

            livroAnunciado.QuantidadeAnunciado -= 1;
            livro.Emprestado = true;
            livro.DataEmprestimo = DateTime.Now;

            if (livroAnunciadoDTO.Tipo == 1) 
            {
                var livroEmprestado = new LivroEmprestado
                {
                    CompradorId = comprador.Id,
                    VendedorId = vendedor.Id,
                    LivroId = livro.Id,
                    DataEmprestimo = DateTime.Now,
                    DataDevolucao = DateTime.Now.AddDays(30),
                    Devolvido = false
                };

                await _dbContext.LivrosEmprestados.AddAsync(livroEmprestado);
            }
            else 
            {
                var livrosDoComprador = await _dbContext.ClientesLivros
                    .Where(cl => cl.ClienteId == comprador.Id)
                    .ToListAsync();

                Livro livroCompradorExistente = null;

                foreach (var lc in livrosDoComprador)
                {
                    var l = await _dbContext.Livros.FirstOrDefaultAsync(x => x.Id == lc.LivroId);
                    if (l != null && l.Titulo == livro.Titulo)
                    {
                        livroCompradorExistente = l;
                        break;
                    }
                }

                if (livroCompradorExistente != null)
                {
                    livroCompradorExistente.Quantidade += 1;
                    _dbContext.Livros.Update(livroCompradorExistente);
                }
                else
                {
                    var novoLivro = new Livro
                    {
                        Titulo = livro.Titulo,
                        QtdPaginas = livro.QtdPaginas,
                        Quantidade = 1,
                        Valor = livro.Valor,
                        Anunciado = false,
                        CategoriaId = livro.CategoriaId,
                        Custo = livro.Custo
                    };

                    await _dbContext.Livros.AddAsync(novoLivro);
                    await _dbContext.SaveChangesAsync(); 

                    var novoClienteLivro = new ClienteLivro
                    {
                        ClienteId = comprador.Id,
                        LivroId = novoLivro.Id
                    };

                    await _dbContext.ClientesLivros.AddAsync(novoClienteLivro);

                    if (fotoLivro != null)
                    {
                        var novaFoto = new FotoLivro
                        {
                            LivroId = novoLivro.Id,
                            UrlImagem = fotoLivro.UrlImagem
                        };

                        await _dbContext.FotosLivros.AddAsync(novaFoto);
                    }
                }
            }

            foreach(var notificacao in notificacoes)
            {
                notificacao.Notificado = 1;
                notificacao.Visto = 1;

                _dbContext.Notificacoes.Update(notificacao);
            }

            _dbContext.LivrosAnunciados.Update(livroAnunciado);
            _dbContext.Livros.Update(livro);

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
            else if(livroAnunciadoDTO.Tipo == 1)
            {
                var comprador = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

                var notificacao = new Notificacao
                {
                    Id = 0,
                    Comprador = comprador,
                    Vendedor = vendedor,
                    Visto = 0,
                    CompradorId = clienteId,
                    Mensagem = $"O usuário {comprador.Nome} está querendo emprestar seu livro {livro.Titulo}\nDeseja aceitar esse empréstimo?",
                    Notificado = 0,
                    Tipo = TipoOperacaoEnum.Emprestimo,
                    VendedorId = (int)livroAnunciadoDTO.ClienteId,
                    Livro = livro,
                    LivroId = livro.Id
                };

                await _dbContext.Notificacoes.AddAsync(notificacao);
                await _dbContext.SaveChangesAsync();

                await _hubContext.Clients.User(notificacao.VendedorId.ToString()).SendAsync("ReceiveNotification", notificacao.Mensagem);
            }
            else if (livroAnunciadoDTO.Tipo == 2)
            {
                var comprador = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

                var notificacao = new Notificacao
                {
                    Id = 0,
                    Comprador = comprador,
                    Vendedor = vendedor,
                    Visto = 0,
                    CompradorId = clienteId,
                    Mensagem = $"O usuário {comprador.Nome} está querendo seu livro {livro.Titulo}\nDeseja aceitar essa doação?",
                    Notificado = 0,
                    Tipo = TipoOperacaoEnum.Doacao,
                    VendedorId = (int)livroAnunciadoDTO.ClienteId,
                    Livro = livro,
                    LivroId = livro.Id
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
