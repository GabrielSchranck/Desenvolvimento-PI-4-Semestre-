using BookAPI.Entities.Clientes;
using BookAPI.mappings;
using BookAPI.Repositories.Clientes;
using BookAPI.Repositories.Enderecos;
using BookAPI.Services.Autenticadores;
using BookAPI.Services.Clientes;
using BookAPI.Services.Token;
using BookModels.DTOs.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using BookAPI.Entities.CEPs;
using Optivem.Framework.Core.Domain;
using Optivem.Framework.Core.Common.Http;
using Microsoft.EntityFrameworkCore;
using BookModels.DTOs.Notificacoes;

namespace BookAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private IClienteRepository _repository;
        private IAutenticadorClienteService _autenticadorClienteService;
        private IEnderecoRepository _enderecoRepository;
        private IClienteService _clienteService;
        private readonly IConfiguration _configuration;

        public ClienteController(IClienteRepository repository, IClienteService clienteService, IConfiguration configuration)
        {
            _repository = repository;
            _clienteService = clienteService;
            _autenticadorClienteService = new AutenticadorClienteService(this._repository);
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetAllAsync()
        {
            var clientes = await _repository.GetAllClientAsync();


            try
            {
                if (clientes == null) return BadRequest("Não foram encontrados clientes");

                return Ok(clientes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpGet("perfil")]
        public async Task<ActionResult<Cliente>> GetById()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

            int clienteId = (int)await TokenService.GetClientIdFromToken(token);
            var cliente = await _repository.GetByIdAsync(clienteId);

            if (cliente == null) return BadRequest("Dados não encontrados");

            var dataNascimento = cliente.DataNascimento.ToString("yyyy-MM-dd");

            var enderecosList = (await _clienteService.GetClienteEnderecosAsync(clienteId)).ToList();

            var enderecosCliente = new List<EnderecoDTO>();

            foreach (var endereco in enderecosList)
            {
                foreach (var enderecoCliente in endereco.EnderecosCliente)
                {
                    var enderecoDTO = new EnderecoDTO
                    {
                        Id = enderecoCliente.Id,
                        EnderecoId = endereco.Id,
                        Rua = endereco.Logradouro,
                        Bairro = endereco.Bairro,
                        Cidade = endereco.Cidade,
                        Uf = endereco.Uf,
                        Cep = endereco.CodigoCep,
                        Complemento = enderecoCliente.Complemento,
                        Numero = enderecoCliente.Numero
                    };

                    enderecosCliente.Add(enderecoDTO);
                }
            }

            var notificacoes = await _clienteService.GetNotificacoes(clienteId);

            var notificacoesDTO = new List<NotificacaoDTO>();

            foreach(var notificacao in notificacoes)
            {
                var notificacaoDTO = new NotificacaoDTO
                {
                    CompradorId = notificacao.CompradorId,
                    VendedorId = notificacao.VendedorId,
                    Id = notificacao.Id,
                    Mensagem = notificacao.Mensagem,
                    Notificado = notificacao.Notificado,
                    Tipo = (int?)notificacao.Tipo,
                    Visto = notificacao.Visto
                };

                notificacoesDTO.Add(notificacaoDTO);
            }

            return Ok(new
            {
                cliente = new
                {
                    cliente.Id,
                    cliente.Nome,
                    cliente.Email,
                    cliente.Cpf,
                    cliente.Idade,
                    cliente.DDD,
                    cliente.Contato,
                    dataNascimento
                },
                enderecos = enderecosCliente,
                notificacoes = notificacoesDTO
            });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Cliente>> Login([FromBody] ClienteDTO clienteDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(clienteDTO.Email) || string.IsNullOrEmpty(clienteDTO.Senha))
                    return BadRequest("Campos não podem ser nulos");

                var cliente = await _repository.Login(clienteDTO.Email, clienteDTO.Senha);

                if (cliente == null)
                    return BadRequest("Email ou senha inválido");

                var token = TokenService.GenerateToken(cliente);

                return Ok(new
                {
                    token = token.Token
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }



        [AllowAnonymous]
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req)
        {
            if (string.IsNullOrEmpty(req.IdToken))
                return BadRequest("idToken não informado.");

            try
            {

                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Authentication:Google:ClientId"] },
                    IssuedAtClockTolerance = TimeSpan.FromMinutes(2)
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(req.IdToken, settings);

                var cliente = await _repository.GetClienteByEmailAsync(payload.Email);

                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nome = payload.Name,
                        Email = payload.Email,

                    };
                    await _repository.Create(cliente);
                }

                var token = TokenService.GenerateToken(cliente);

                return Ok(new { token = token.Token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }


        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<ClienteDTO>> Create(ClienteDTO clienteDTO)
        {
            try
            {
                var cliente = clienteDTO.ConverterClienteDTOParaCliente();

                if (cliente == null) return BadRequest("Cliente não pode ser nulo");

                var erros = (await _autenticadorClienteService.AutenticarClienteAoCriar(cliente));

                if (erros.Count != 0)
                    return BadRequest(new { errors = erros });

                if (cliente.DataNascimento > DateTime.Now)
                    return BadRequest("A data de nascimento não pode ser maior que a atual");

                else
                    cliente.Idade = await _autenticadorClienteService.GetIdadeAsync(cliente.DataNascimento);

                await _repository.Create(cliente);

                //var verifyToken = Guid.NewGuid().ToString();
                //cliente.EmailConfirmado = false;

                //await _clienteService.SendEmail(verifyToken, _configuration, cliente);

                var token = TokenService.GenerateToken(cliente);

                return Ok(new
                {
                    token = token.Token
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPost("endereco")]
        public async Task<ActionResult<EnderecoDTO>> CreateEnderecoCliente(EnderecoDTO enderecoDTO)
        {
            try
            {
                if (enderecoDTO == null) return BadRequest("Adicone um endereço válido");

                int clienteId = (int)await _clienteService.GetClienteIdByTokenAsync(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last());
                if (clienteId == 0) return BadRequest("Cliente não encontrado");

                var endereco = enderecoDTO.ConverterEnderecoDTOParaEndereco();
                await _enderecoRepository.CreateAsync(endereco);

                await _clienteService.CreateEnderecoClienteAsync(endereco, clienteId);

                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult> Update(ClienteDTO clienteDTO)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var cliente = clienteDTO.ConverterClienteDTOParaCliente();
                cliente.Id = clienteId;

                if (cliente == null) return BadRequest("Cliente não pode ser nulo");

                await _repository.Update(cliente);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPost("verificar-email")]
        public async Task<IActionResult> VerificarEmail([FromBody] string token)
        {
            var cliente = await _clienteService.FindByToken(token);

            if (cliente == null)
                return NotFound("Token inválido");

            return Ok("E-mail verificado com sucesso.");
        }

        [HttpPost("fecharNotificacao/{notificacaoId}")]
        public async Task<ActionResult> FecharNotificacao([FromRoute] int notificacaoId)
        {
            try
            {
               await _clienteService.FecharNotificacao(notificacaoId);

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
