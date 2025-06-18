using BookAPI.Entities.Clientes;
using BookAPI.mappings;
using BookAPI.Repositories.Clientes;
using BookModels.DTOs.Clientes;

namespace BookAPI.Services.Clientes
{
    public class CartaoClienteService : ICartaoClienteService
	{
		private readonly ICartaoClienteRepository _cartaoClienteRepository;

		public CartaoClienteService(ICartaoClienteRepository cartaoClienteRepository)
		{
			_cartaoClienteRepository = cartaoClienteRepository;
		}

		//Cartões dos clientes
		public async Task AlterCartaoClienteAsync(CartaoClienteDTO cartaoClienteDTO)
		{
			var cartaoCliente = cartaoClienteDTO.ConverterCartaoDTOParaCartaoCliente();

			if (cartaoCliente != null)
			{
				await _cartaoClienteRepository.UpdateAsync(cartaoCliente);
			}
		}
		public async Task CreateCartaoClienteAsync(CartaoClienteDTO cartaoClienteDTO)
		{
			var cartaoCliente = cartaoClienteDTO.ConverterCartaoDTOParaCartaoCliente();

			if (cartaoCliente != null)
			{
				await _cartaoClienteRepository.CreateAsync(cartaoCliente);
			}
		}

        public async Task<bool> CreateUUId(string uuid, int clienteId)
        {
			return await _cartaoClienteRepository.CreateUUid(uuid, clienteId);
        }

        public async Task DeleteCartaoClienteAsync(CartaoClienteDTO cartaoClienteDTO)
		{
			var cartaoCliente = cartaoClienteDTO.ConverterCartaoDTOParaCartaoCliente();

			if(cartaoCliente != null)
			{
				await _cartaoClienteRepository.DeleteAsync(cartaoCliente);
			}
		}
		public async Task<IEnumerable<CartaoCliente>> GetCartoesClienteAsync(int clienteId)
		{
			return await _cartaoClienteRepository.GetAllAsync(clienteId);

		}

        public async Task<double> GetSaldo(int clienteId)
        {
            return await _cartaoClienteRepository.GetSaldo(clienteId);
        }

        public async Task<double> GetSaldoSacado(int clienteId)
        {
			return await _cartaoClienteRepository.GetSaldoSacado(clienteId);
        }

        public async Task<string> GetUUID(int clienteId)
        {
            return await _cartaoClienteRepository.GetUUIDMercadoPago(clienteId);
        }
    }
}
