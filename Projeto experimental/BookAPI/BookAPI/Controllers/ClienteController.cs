using BookAPI.Entities.Clientes;
using BookAPI.Repositories.Clientes;
using Microsoft.AspNetCore.Mvc;

namespace BookAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _repository;

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            try
            {
                if (cliente == null) return BadRequest("Cliente não pode ser nulo");

                _repository.Create(cliente);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados");
            }
        }
    }
}
