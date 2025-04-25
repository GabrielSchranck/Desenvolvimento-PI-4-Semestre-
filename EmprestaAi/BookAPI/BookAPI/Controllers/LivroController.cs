using BookAPI.mappings;
using BookAPI.Repositories.Livros;
using BookAPI.Services.Livros;
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
		private readonly ILivroServices livroServices;

		//Crud

		
	}
}
