using BookAPI.Entities.ClientesLivros;
using BookAPI.mappings;
using BookAPI.Repositories.Livros;
using BookAPI.Services.Livros;
using BookAPI.Services.Token;
using BookModels.DTOs.Clientes;
using BookModels.DTOs.Livros;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Authorize]
    [Route("api/[Controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly ILivroServices _livroServices;

        public LivroController(ILivroServices livroService)
        {
            this._livroServices = livroService;
        }

        //Crud
        [HttpPost("create")]
        public async Task<ActionResult> Create([FromForm] LivroDTO livroDTO)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);


                await this._livroServices.CadastrarLivroCliente(livroDTO, clienteId);

                return Ok("Livro cadastrado com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpDelete("delete/{livroId}")]
        public async Task<ActionResult> Delete([FromRoute] int livroId)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var clienteLivro = new ClienteLivro
                {
                    ClienteId = clienteId,
                    LivroId = livroId
                };

                await _livroServices.Delete(clienteLivro);

                return Ok("Livro removido com sucesso");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpGet("getLivros")]
        public async Task<ActionResult> GetLivros()
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var livrosDto = await _livroServices.GetAll(clienteId);

                foreach (var livro in livrosDto)
                {
                    if (!string.IsNullOrEmpty(livro.UriImagemLivro) && livro.UriImagemLivro.StartsWith("imagens/"))
                    {
                        livro.UriImagemLivro = $"{Request.Scheme}://{Request.Host}/{livro.UriImagemLivro}";
                    }
                }


                return Ok(new { livros = livrosDto });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no GetLivros: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }


        //Procura imagem na API
        [HttpGet("getIinfoApi")]
        public async Task<ActionResult> GetInfoFromApi([FromQuery] string name)
        {
            try
            {
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");

                int clienteId = (int)await TokenService.GetClientIdFromToken(token);

                var fotoLivro = await _livroServices.GetImgBook(name);

                return Ok(new { urlImagem = fotoLivro.UrlImagem });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        [HttpGet("getCategorias")]
        public async Task<ActionResult> GetCategorias()
        {
            try
            {
                var categorias = await _livroServices.GetCategorias();

                return Ok(new { result = categorias });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }

        //[HttpPost("saveImagem")]
        //public async Task<ActionResult> PostImagens([FromBody] ImagemLivroDTO imagemLivroDTO)
        //{
        //    try
        //    {
        //        var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //        if (string.IsNullOrEmpty(token)) return Unauthorized("Token de autenticação não encontrado.");
        //        int clienteId = (int)await TokenService.GetClientIdFromToken(token);


        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
        //    }
        //}
    }
}
