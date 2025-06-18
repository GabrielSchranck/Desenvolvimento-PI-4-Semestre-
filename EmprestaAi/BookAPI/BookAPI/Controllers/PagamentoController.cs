using BookAPI.Entities.Clientes;
using BookAPI.Services.Token;
using BookAPI.Services.Vendas;
using BookModels.DTOs.Clientes;
using BookModels.DTOs.Operacoes;
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
            }

            return Redirect("http://localhost:4200/financeiro");
        }

        [AllowAnonymous]
        [HttpGet("/erro")]
        public IActionResult Erro()
        {
            return BadRequest("O pagamento foi cancelado ou falhou.");
        }

        [HttpPost("comprarLivro")]
        public async Task<ActionResult> Vender([FromBody] List<Operacao> operacoes)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                foreach (var operacao in operacoes)
                {
                    operacao.LivroAnunciadoDTO.qtdOperacao = operacao.Quantidade;
                    await _vendaService.ComprarLivro(clienteId, operacao.LivroAnunciadoDTO);
                }

                return Ok(new { result = "Compras realizadas com sucesso!" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPost("solicitarEmprestimoDoacao")]
        public async Task<ActionResult> Solicitar([FromBody] List<Operacao> operacoes)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                foreach (var operacao in operacoes)
                {
                    operacao.LivroAnunciadoDTO.qtdOperacao = operacao.Quantidade;
                    await _vendaService.SolicitarEmprestimo(clienteId, operacao.LivroAnunciadoDTO);
                }

                return Ok(new { result = "Solicitação realizadas com sucesso!" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPost("finalizarOperacaoEmprestimoDocao")]
        public async Task<ActionResult> FinalizarOperacao([FromBody] List<Operacao> operacoes, int clienteId)
        {
            try
            {
                foreach (var operacao in operacoes)
                {
                    operacao.LivroAnunciadoDTO.qtdOperacao = operacao.Quantidade;
                    await _vendaService.FinalizarOperacaoLivro(clienteId, operacao);
                }

                return Ok(new { result = "Solicitação realizadas com sucesso!" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
