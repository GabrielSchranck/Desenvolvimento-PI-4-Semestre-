using BookAPI.Entities.CEPs;
using BookAPI.Services.Enderecos;
using BookAPI.Services.Token;
using BookModels.DTOs.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Optivem.Framework.Core.Domain;

namespace BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoService _enderecoService;
        public EnderecoController(IEnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [AllowAnonymous]
        [HttpGet("getViaCep")]
        public async Task<ActionResult<Endereco>> GetEnderecoByCep([FromQuery] string cep)
        {

            try
            {
                var endereco = await _enderecoService.GetEnderecoByApi(cep);

                if (endereco.Cidade == null)
                {
                    return BadRequest("Não foi possível encontrar um endereço");
                }
                return Ok(new
                {
                    cep = endereco.CodigoCep,
                    rua = endereco.Logradouro,
                    bairro = endereco.Bairro,
                    cidade = endereco.Cidade,
                    uf = endereco.Uf
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateEndereco([FromBody] Endereco endereco)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                await _enderecoService.CreateEnderecoCliente(endereco, clienteId);

                return Ok("Endereço cadastrado com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
