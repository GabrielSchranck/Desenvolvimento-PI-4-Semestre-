using BookModels.DTOs.Carrinhos;
using BookModels.DTOs.Livros;

namespace BookAPI.Services.Carrinhos
{
    public interface ICarrinhoService
    {
        Task CreateCarrinhoUser(int clienteId);
        Task<bool> VerificarExistenciaAsync(int clienteId);
        Task<CarrinhoDTO> GetCarrinhoAsync(int clienteId);
        Task<bool> AddItemCarrinho(LivroAnunciadoDTO livroAnunciadoDTO, int clienteId);

    }
}
