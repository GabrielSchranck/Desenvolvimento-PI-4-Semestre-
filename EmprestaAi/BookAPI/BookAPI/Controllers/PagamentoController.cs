using BookAPI.Entities.Clientes;
using BookAPI.Services.Token;
using BookAPI.Services.Vendas;
using BookModels.DTOs.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace BookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PagamentoController : Controller
    {
        private readonly IVendaService _vendaService;

        public PagamentoController(IVendaService vendaService)
        {
            this._vendaService = vendaService;
        }

        [AllowAnonymous]
        [HttpPost("criar-pagamento/{valor}")]
        public async Task<IActionResult> CriarPagamento([FromRoute] decimal valor)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var result = await _vendaService.CriarPagamentoAsync(clienteId, valor);

                return Ok(new{ result = result });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [AllowAnonymous]
        [HttpGet("/sucesso")]
        public async Task<IActionResult> Sucesso([FromQuery] string session_id)
        {
            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                await _vendaService.ChangeStatus("Pago", session);
                return Ok("Pagamento confirmado com sucesso!");
            }

            return BadRequest("Pagamento não foi confirmado.");
        }

        [AllowAnonymous]
        [HttpGet("/erro")]
        public IActionResult Erro()
        {
            return BadRequest("O pagamento foi cancelado ou falhou.");
        }
    }
}
