
using BookAPI.Repositories.Carrinhos;
using BookModels.DTOs.Carrinhos;
using BookModels.DTOs.Livros;

namespace BookAPI.Services.Carrinhos
{
    public class CarrinhoService : ICarrinhoService
    {
        private readonly ICarrinhoRepository _carrinhoRepository;

        public CarrinhoService(ICarrinhoRepository carrinhoRepository)
        {
            this._carrinhoRepository = carrinhoRepository;
        }

        public async Task<bool> AddItemCarrinho(LivroAnunciadoDTO livroAnunciadoDTO, int clienteId)
        {
            var itemCarrinho = new ItensCarrinhoDTO
            {
                LivroAnunciadoDTO = livroAnunciadoDTO
            };

            return await _carrinhoRepository.AddItemCarrinho(itemCarrinho, clienteId, livroAnunciadoDTO.Tipo);
        }

        public async Task CreateCarrinhoUser(int clienteId)
        {
            await _carrinhoRepository.Create(clienteId);
        }

        public async Task<CarrinhoDTO> GetCarrinhoAsync(int clienteId)
        {
            return await _carrinhoRepository.GetCarrinhoAsync(clienteId);
        }

        public async Task<bool> VerificarExistenciaAsync(int clienteId)
        {
            return await _carrinhoRepository.Verificar(clienteId);
        }
    }
}
