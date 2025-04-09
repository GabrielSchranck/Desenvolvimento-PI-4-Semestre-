using BookAPI.Data;
using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Repositories.Clientes
{
	public class CartaoClienteRepository : ICartaoClienteRepository
	{
		private readonly BookDbContext _dbContext;

		public CartaoClienteRepository(BookDbContext dbContext)
		{
			this._dbContext = dbContext;
		}

		public async Task CreateAsync(CartaoCliente cartaoCliente)
		{
			await this._dbContext.CartoesClientes.AddAsync(cartaoCliente);
			await this._dbContext.SaveChangesAsync();
		}

		public async Task DeleteAsync(CartaoCliente cartaoCliente)
		{
			this._dbContext.CartoesClientes.Remove(cartaoCliente);
			await this._dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<CartaoCliente>> GetAllAsync(int clienteId)
		{
			var cartaoCliente =  await this._dbContext.CartoesClientes.Where(c => c.ClienteId == clienteId).ToListAsync();

			if(cartaoCliente.Count != 0)
			{
				return cartaoCliente;
			}

            return null;
        }

		public async Task<CartaoCliente> GetByIdAsync(int cartaoId)
		{
			var existente = await this._dbContext.CartoesClientes.Where(c => c.Id == cartaoId).FirstOrDefaultAsync();
			if(existente != null)
			{
				return existente;
			}

			return null;
		}

		public async Task UpdateAsync(CartaoCliente cartaoCliente)
		{
			var existente = await _dbContext.CartoesClientes.FindAsync(cartaoCliente.Id);
			if (existente != null)
			{
				_dbContext.Entry(existente).CurrentValues.SetValues(cartaoCliente);
				await _dbContext.SaveChangesAsync();
			}
		}

	}
}
