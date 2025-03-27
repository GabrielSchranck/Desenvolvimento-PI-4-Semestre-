using BookAPI.mappings;
using BookAPI.Repositories.Livros;
using BookModels.DTOs.Livros;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
	[Route("api/[Controller]")]
	[ApiController]
	public class LivroController : ControllerBase
	{
		private readonly ILivroRepository _repository;

		[HttpGet("{id:int}")]
		public async Task<ActionResult<LivroDTO>> GetItem(int id)
		{
			try
			{
				var livro = await _repository.GetItem(id);

				if (livro == null) return NotFound();

				var livroDTO = livro.ConverterLivroParaLivroDTO();

				return Ok(livroDTO);
			}
			catch(Exception)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
			}
		}
	}
}
