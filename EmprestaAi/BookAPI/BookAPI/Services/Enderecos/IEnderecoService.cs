using BookAPI.Entities.CEPs;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Services.Enderecos
{
    public interface IEnderecoService
    {
        Task<Endereco> GetEnderecoByApi(string cep);
        Task CreateAsync(Endereco endereco);
        Task CreateEnderecoCliente(Endereco endereco, int clienteId);
    }
}
