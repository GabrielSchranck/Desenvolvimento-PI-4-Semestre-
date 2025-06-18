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

        public async Task<bool> CreateUUid(string uuid, int clienteId)
        {
			var cliente = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

			if (cliente == null) return false;

            cliente.uuidMercadoPago = uuid;
            _dbContext.Clientes.Update(cliente);

            await _dbContext.SaveChangesAsync();

			return true;
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

        public async Task<double> GetSaldo(int clienteId)
        {
			var cliente = await _dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

			if (cliente is null) return 0;

			if (cliente.Saldo is null)
			{
				cliente.Saldo = 0;
				_dbContext.Clientes.Update(cliente);
				await _dbContext.SaveChangesAsync();
			}

			return (double)cliente.Saldo;
        }

        public async Task<double> GetSaldoSacado(int clienteId)
        {
			var saldoClienteSacado = await _dbContext.Saques.FindAsync(clienteId);

			if (saldoClienteSacado is null) return 0;

			return (double)saldoClienteSacado.Saldo;
        }

        public async Task<string> GetUUIDMercadoPago(int clienteId)
        {
			var cliente = await this._dbContext.Clientes.Where(c => c.Id == clienteId).FirstOrDefaultAsync();

			return cliente.uuidMercadoPago;
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
