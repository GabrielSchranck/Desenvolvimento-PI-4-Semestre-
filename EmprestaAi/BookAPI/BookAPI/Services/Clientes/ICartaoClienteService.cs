using BookAPI.Entities.Clientes;
using BookModels.DTOs.Clientes;

namespace BookAPI.Services.Clientes
{
	public interface ICartaoClienteService
	{
		Task<IEnumerable<CartaoCliente>> GetCartoesClienteAsync(int clienteId);
		Task CreateCartaoClienteAsync(CartaoClienteDTO cartaoClienteDTO);
		Task DeleteCartaoClienteAsync(CartaoClienteDTO cartaoClienteDTO);
		Task AlterCartaoClienteAsync(CartaoClienteDTO cartaoClienteDTO);
		Task<string> GetUUID(int clienteId);
		Task<bool> CreateUUId(string uuid, int clienteId);
        Task<double> GetSaldo(int clienteId);
    }
}
