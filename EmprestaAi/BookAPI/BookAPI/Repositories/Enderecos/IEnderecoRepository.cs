using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Enderecos
{
    public interface IEnderecoRepository
    {
        Task CreateAsync(Endereco endereco);
        Task<Endereco> GetByCepAsync(string cep);
        Task CreateEnderecoClienteAsync(EnderecoCliente enderecoCliente);
        Task<bool> FindEnderecoClienteAsync(EnderecoCliente enderecoCliente);
        Task DeleteEnderecoClienteAsync(EnderecoCliente enderecoCliente);
        Task UpdateEnderecoAsync(Endereco endereco, int clienteId);
        Task<bool> CepExiste(string cep);
    }
}
