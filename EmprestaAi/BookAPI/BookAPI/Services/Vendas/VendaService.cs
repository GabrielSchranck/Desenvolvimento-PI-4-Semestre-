using BookAPI.Entities.Clientes;
using BookAPI.Entities.Historicos;
using BookAPI.Repositories.Vendas;
using BookModels.DTOs.Clientes;
using BookModels.DTOs.Livros;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace BookAPI.Services.Vendas
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;

        public VendaService(IVendaRepository vendaRepository)
        {
            this._vendaRepository = vendaRepository;
        }

        public async Task<bool> ChangeStatus(string status, Stripe.Checkout.Session session)
        {
            if (await _vendaRepository.ChangeStatus(status, session))
            {
                await UpdateSaldoCliente(session);
            }

            return false;
        }

        private async Task UpdateSaldoCliente(Stripe.Checkout.Session session)
        {
            await _vendaRepository.ChangeSaldo(session);
        }

        public async Task<string> CriarPagamentoAsync(int clientId, decimal valor)
        {
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(valor * 100),
                            Currency = "brl",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Adicionar saldo"
                            },
                        },
                        Quantity = 1,
                    }
                },
                Mode = "payment",
                SuccessUrl = "https://1bcc-177-128-8-241.ngrok-free.app/sucesso?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "https://1bcc-177-128-8-241.ngrok-free.app/erro"
            };

            var service = new Stripe.Checkout.SessionService();
            var session = service.Create(options);

            var pagamento = new Pagamento
            {
                ClienteId = clientId,
                Valor = valor,
                StripePaymentIntentId = "0",
                StripeSessionId = session.Id,
                Status = "pendente",
                DataCriacao = DateTime.UtcNow
            };

            await _vendaRepository.SavePayment(pagamento);

            return session.Url;
        }

        public async Task<bool> ComprarLivro(int clienteId, LivroAnunciadoDTO livroAnunciadoDTO)
        {
            if (livroAnunciadoDTO.Tipo == 0)
            {
                if (await _vendaRepository.OperacaoVenda(clienteId, livroAnunciadoDTO))
                {
                    if(await _vendaRepository.OperacaoLivro(clienteId, livroAnunciadoDTO))
                    {
                        return await SalvarHistorico(livroAnunciadoDTO, clienteId);
                    }

                }
            }

            return await _vendaRepository.OperacaoLivro(clienteId, livroAnunciadoDTO);
        }

        private async Task<bool> SalvarHistorico(LivroAnunciadoDTO livroAnunciadoDTO, int clienteId)
        {
            return await _vendaRepository.SaveHistorico(clienteId, livroAnunciadoDTO);
        }
    }
}
