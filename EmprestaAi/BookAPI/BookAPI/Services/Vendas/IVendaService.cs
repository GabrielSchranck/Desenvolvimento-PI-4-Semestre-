using BookModels.DTOs.Clientes;
using BookModels.DTOs.Livros;
using BookModels.DTOs.Operacoes;
using Stripe.Checkout;

namespace BookAPI.Services.Vendas
{
    public interface IVendaService
    {
        Task<string> CriarPagamentoAsync(int clientId, decimal valor);
        Task<bool> ChangeStatus(string status, Session session);
        Task<bool> ComprarLivro(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
        Task<bool> FinalizarOperacaoLivro(int clienteId, Operacao Operacoes);
        Task<bool> DoarLivro(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
        Task<bool> SolicitarEmprestimo(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
    }
}
