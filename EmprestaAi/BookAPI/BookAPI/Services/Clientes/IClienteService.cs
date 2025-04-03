using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;

namespace BookAPI.Services.Clientes
{
    public interface IClienteService
    {
        Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId);
        Task<int> GetClienteIdByTokenAsync(string token);
        Task CreateEnderecoClienteAsync(Endereco endereco, int clienteId);
    }
}
