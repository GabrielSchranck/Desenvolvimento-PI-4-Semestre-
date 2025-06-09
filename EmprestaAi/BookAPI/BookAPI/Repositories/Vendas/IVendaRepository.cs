using BookAPI.Entities.Clientes;
using Stripe.Checkout;


namespace BookAPI.Repositories.Vendas
{
    public interface IVendaRepository
    {
        Task<bool> SavePayment(Pagamento pagamento);
        Task<bool> ChangeStatus(string status, Session session);
        Task ChangeSaldo(Session session);
    }
}
