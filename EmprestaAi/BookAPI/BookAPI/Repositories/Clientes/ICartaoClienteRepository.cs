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
	}
}
