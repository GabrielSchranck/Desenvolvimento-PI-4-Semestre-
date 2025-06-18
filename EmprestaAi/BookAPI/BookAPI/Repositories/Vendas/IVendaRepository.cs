using BookAPI.Entities.Clientes;
using BookModels.DTOs.Livros;
using Stripe.Checkout;


namespace BookAPI.Repositories.Vendas
{
    public interface IVendaRepository
    {
        Task<bool> SavePayment(Pagamento pagamento);
        Task<bool> ChangeStatus(string status, Session session);
        Task ChangeSaldo(Session session);
        Task<bool> OperacaoVenda(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
        Task<bool> OperacaoEmprestimo(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
        Task<bool> OperacaoDoacao(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
        Task<bool> OperacaoLivro(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
        Task<bool> SaveHistorico(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO);
    }
}
