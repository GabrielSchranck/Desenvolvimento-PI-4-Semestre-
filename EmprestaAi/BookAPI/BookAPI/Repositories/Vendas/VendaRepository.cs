using BookAPI.Data;
using BookAPI.Entities.Clientes;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using System;

namespace BookAPI.Repositories.Vendas
{
    public class VendaRepository : IVendaRepository
    {
        private readonly BookDbContext _dbContext;

        public VendaRepository(BookDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task ChangeSaldo(Session session)
        {
            var pagamento = await _dbContext.Pagamentos
            .FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);

            var cliente = await _dbContext.Clientes.Where(c => c.Id == pagamento.ClienteId).FirstOrDefaultAsync();

            cliente.Saldo += (double)pagamento.Valor;

            _dbContext.Clientes.Update(cliente);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ChangeStatus(string status, Session session)
        {
            var pagamento = await _dbContext.Pagamentos
            .FirstOrDefaultAsync(p => p.StripeSessionId == session.Id);

            if(pagamento == null) return false;

            pagamento.Status = status;
            pagamento.StripePaymentIntentId = session.PaymentIntentId;

            _dbContext.Pagamentos.Update(pagamento);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SavePayment(Pagamento pagamento)
        {
            var cliente = await _dbContext.Clientes.Where(c => c.Id == pagamento.ClienteId).FirstOrDefaultAsync();

            pagamento.Cliente = cliente;

            await _dbContext.Pagamentos.AddAsync(pagamento);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
