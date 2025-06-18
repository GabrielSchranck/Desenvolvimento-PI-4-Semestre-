using BookAPI.Entities.Clientes;
using BookAPI.Services.Clientes;
using BookAPI.Services.Token;
using BookModels.DTOs.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System;

namespace BookAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {
        private readonly ICartaoClienteService _cartaoClienteService;

		public CartaoController(ICartaoClienteService cartaoClienteService)
		{
			_cartaoClienteService = cartaoClienteService;
		}


		[HttpPost("create")]
		public async Task<ActionResult> Create([FromBody] CartaoClienteDTO cartaoClienteDTO)
		{
			try
			{
				var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
				if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

				int clienteId = (int)await TokenService.GetClientIdFromToken(token);

				cartaoClienteDTO.ClienteId = clienteId;

				await _cartaoClienteService.CreateCartaoClienteAsync(cartaoClienteDTO);

				return Ok(new { mensagem = "Cartão criado com sucesso!" });
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
			}
		}

		[HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CartaoCliente>>> GetCartoesCliente()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);
                var cartoes = await _cartaoClienteService.GetCartoesClienteAsync(clienteId);

                if (cartoes == null) return Ok("Nenhum cartão encontrado");

                return Ok(new
                {
                    cartoes = cartoes
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

		[HttpPut("update")]
		public async Task<ActionResult> UpdateCartao(CartaoClienteDTO cartaoClienteDTO)
		{
			try
			{
				var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
				if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

				int clienteId = (int)await TokenService.GetClientIdFromToken(token);

				cartaoClienteDTO.ClienteId = clienteId;

				await _cartaoClienteService.AlterCartaoClienteAsync(cartaoClienteDTO);

				return Ok(new { mensagem = "Cartão atualizado com sucesso!" });
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
			}
		}

		[HttpDelete("delete")]
		public async Task<ActionResult> DeleteCartao([FromBody] CartaoClienteDTO cartaoClienteDTO)
		{
			try
			{
				var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
				if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

				int clienteId = (int)await TokenService.GetClientIdFromToken(token);

				cartaoClienteDTO.ClienteId = clienteId;

				await _cartaoClienteService.DeleteCartaoClienteAsync(cartaoClienteDTO);

				return Ok(new { mensagem = "Cartão deletado com sucesso!" });
			}
			catch (Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
			}
		}

		[HttpGet("getUuidMP")]
		public async Task<ActionResult> GetUuidMercadoPago()
		{
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

				var uuid = await _cartaoClienteService.GetUUID(clienteId);

                return Ok(new
                {
                    uuid = uuid
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

		[HttpPost("creteUuid")]
		public async Task<ActionResult> CreateUUID([FromBody] string uuid)
		{
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

				if (!(await _cartaoClienteService.CreateUUId(uuid, clienteId))) return BadRequest("Não foi possível cadastrar o uuid");

                return Ok(new
                {
                    result = "UUID cadastrado com sucesso"
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

		[HttpGet("getSaldo")]
		public async Task<ActionResult> GetSaldoCliente()
		{
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var saldo = await _cartaoClienteService.GetSaldo(clienteId);
                var saldoSacado = await _cartaoClienteService.GetSaldoSacado(clienteId);

                return Ok(new{ saldo = saldo, saldoSacado = saldoSacado});
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
