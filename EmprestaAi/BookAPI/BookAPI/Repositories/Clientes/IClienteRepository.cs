using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Clientes
{
    public interface IClienteRepository
    {
        Task Create(Cliente cliente);
        Task<IEnumerable<Cliente>> GetAllClientAsync();
        Task<bool> GetByCpfAsync(string cpf);
        Task<bool> GetByEmailAsync(string email);
        Task<Cliente> Login(string email, string senha);
        Task<Cliente> GetByIdAsync(int id);
        Task Update(Cliente cliente);
        Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId);
        Task CreateEnderecoCliente(EnderecoCliente enderecoCliente);
    }
}
