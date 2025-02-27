using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Clientes
{
    public interface IClienteRepository
    {
        Task Create(Cliente cliente);
        Task<IEnumerable<Cliente>> GetAllClientAsync();
        Task<Cliente> Login(string email, string senha);
    }
}
