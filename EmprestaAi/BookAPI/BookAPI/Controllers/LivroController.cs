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
		
	}
}
