using BookAPI.Entities.Livros;
using BookAPI.Services.Carrinhos;
using BookAPI.Services.Token;
using BookModels.DTOs.Carrinhos;
using BookModels.DTOs.Livros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class CarrinhoController : Controller
    {
        private ICarrinhoService _carrinhoService;

        public CarrinhoController(ICarrinhoService carrinhoService)
        {
            this._carrinhoService = carrinhoService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateCarrinho()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);
                
                await _carrinhoService.CreateCarrinhoUser(clienteId);

                return Ok(new { result = "Carrinho criado com sucesso!" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpGet("getCarrinho")]
        public async Task<ActionResult> GetCarrinho()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");
                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var carrinho = await _carrinhoService.GetCarrinhoAsync(clienteId);

                foreach(var item in carrinho.Itens)
                {
                    if (!string.IsNullOrEmpty(item.LivroAnunciadoDTO.LivroDTO.UriImagemLivro) && item.LivroAnunciadoDTO.LivroDTO.UriImagemLivro.StartsWith("imagens/"))
                    {
                        item.LivroAnunciadoDTO.LivroDTO.UriImagemLivro = $"{Request.Scheme}://{Request.Host}/{item.LivroAnunciadoDTO.LivroDTO.UriImagemLivro}";
                    }
                }

                return Ok(new { result = carrinho });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpGet("verify")]
        public async Task<ActionResult<bool>> VerificaExistenciaCarrinho()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var existe = await _carrinhoService.VerificarExistenciaAsync(clienteId);

                return Ok(new { result = existe });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpPost("addItemCarrinho")]
        public async Task<ActionResult> AdicionarItemAoCarrinho([FromBody] LivroAnunciadoDTO livroAnunciadoDTO)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var result = await _carrinhoService.AddItemCarrinho(livroAnunciadoDTO, clienteId);

                if(!result) return BadRequest("Item já existe no carrinho");

                return Ok(new { result = "Item inserido ao carrinho!" });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
