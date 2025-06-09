using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Clientes
{
	public interface ICartaoClienteRepository
	{
		Task<IEnumerable<CartaoCliente>> GetAllAsync(int clienteId);
		Task<CartaoCliente> GetByIdAsync(int cartaoId);
		Task CreateAsync(CartaoCliente cartaoCliente);
		Task UpdateAsync(CartaoCliente cartaoCliente);
		Task DeleteAsync(CartaoCliente cartaoCliente);
		Task<string> GetUUIDMercadoPago(int clienteId);
		Task<bool> CreateUUid(string uuid, int clienteId);
		Task<double> GetSaldo(int clienteId);
	}
}
