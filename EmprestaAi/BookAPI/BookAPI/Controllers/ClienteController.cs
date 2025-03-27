using BookAPI.Entities.Clientes;
using BookAPI.mappings;
using BookAPI.Repositories.Clientes;
using BookAPI.Repositories.Enderecos;
using BookAPI.Services.Autenticadores;
using BookAPI.Services.Token;
using BookModels.DTOs.Clientes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public ClienteController(IClienteRepository repository)
        {
            _repository = repository;
            _autenticadorClienteService = new AutenticadorClienteService(this._repository);
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

        [HttpGet("me")]
        public async Task<ActionResult<Cliente>> GetById()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token de autenticação não encontrado.");
            }

            int clienteId = (int)await TokenService.GetClientIdFromToken(token);

            var cliente = await _repository.GetByIdAsync(clienteId);
            if (cliente == null)
            {
                return BadRequest("Dados não encontrados");
            }

            var ceps = await _repository.GetClienteEnderecosAsync(clienteId);

            var enderecosList = ceps.Select(cep => new
            {
                cep.Id,
                cep.CepCod,
                cep.Bairro,
                cep.Cidade,
                cep.Uf,
                EnderecosList = cep.Enderecos.Select(endereco => new
                {
                    endereco.Id,
                    endereco.Logradouro,
                    endereco.Numero,
                }).ToList()
            }).ToList();

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
                    cliente.DataNascimento
                },
                enderecos = enderecosList
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
        public async Task<ActionResult<EnderecoDTO>> CreateEndereco(EnderecoDTO enderecoDTO)
        {
            try
            {
                if (enderecoDTO == null) return BadRequest("Adicone um endereço válido");
                var endereco = enderecoDTO.ConverterEnderecoDTOParaEndereco();
                _enderecoRepository.Create(endereco);

                return Ok();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
