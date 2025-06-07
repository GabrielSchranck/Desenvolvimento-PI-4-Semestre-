using BookModels.DTOs.Carrinhos;
using BookModels.DTOs.Livros;

namespace BookAPI.Repositories.Carrinhos
{
    public interface ICarrinhoRepository
    {
        Task Create(int clienteId);
        Task<bool> Verificar(int clienteId);
        Task<CarrinhoDTO> GetCarrinhoAsync(int clienteId);
        Task<bool> AddItemCarrinho(ItensCarrinhoDTO itensCarrinhoDTO, int clienteId, int tipo);
    }
}
