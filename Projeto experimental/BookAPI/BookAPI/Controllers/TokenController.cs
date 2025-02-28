//using BookAPI.Token;
//using BookModels.DTOs.Clientes;
//using Fluent.Infrastructure.FluentModel;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace BookAPI.Controllers
//{
//	[Route("api/[controller]")]
//	[ApiController]
//	public class TokenController : ControllerBase
//	{
//		private readonly UserManager<ApplicationUser> _userManager;
//		private readonly SignInManager<ApplicationUser> _signInManager;

//		public TokenController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
//		{
//			this._userManager = userManager;
//			this._signInManager = signInManager;
//		}

//		[AllowAnonymous]
//		[Produces("application/json")]
//		[HttpPost("/api/CreateToken")]
//		public async Task<IActionResult> CreateToken([FromBody] InputDTO input)
//		{
//			if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Senha))
//				return Unauthorized();

//			var result = await _signInManager.PasswordSignInAsync(input.Email, input.Senha, false, lockoutOnFailure: false);

//			if (result.Succeeded)
//			{
//				var token = new TokenJWTBuilder().AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
//					.AddSubject("gabriel")
//					.AddIssuer("Teste.Security.Bearer")
//					.AddAudience("Teste.Security.Bearer")
//					.AddClaim("UsuarioAPINumero", "1")
//					.AddExpiry(5)
//					.Builder();

//				return Ok(token.value);
//			}
//			else
//			{
//				return Unauthorized();
//			}
//		}


//	}
//}
