using BookModels.DTOs.Clientes;
using Stripe.Checkout;

namespace BookAPI.Services.Vendas
{
    public interface IVendaService
    {
        Task<string> CriarPagamentoAsync(int clientId, decimal valor);
        Task<bool> ChangeStatus(string status, Session session);
    }
}
