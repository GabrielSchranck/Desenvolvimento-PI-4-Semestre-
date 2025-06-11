using BookModels.DTOs.Clientes;
using BookModels.DTOs.Livros;
using Stripe.Checkout;

namespace BookAPI.Services.Vendas
{
    public interface IVendaService
    {
        Task<string> CriarPagamentoAsync(int clientId, decimal valor);
        Task<bool> ChangeStatus(string status, Session session);
        Task<bool> ComprarLivro(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
    }
}
