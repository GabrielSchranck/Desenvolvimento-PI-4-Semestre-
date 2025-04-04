using BookAPI.Entities.CEPs;
using BookModels.DTOs.Clientes;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Services.Enderecos
{
    public interface IEnderecoService
    {
        Task<Endereco> GetEnderecoByApi(string cep);
        Task CreateAsync(Endereco endereco);
        Task CreateEnderecoCliente(Endereco endereco, int clienteId);
        Task UpdateEnderecoClienteAsync(Endereco endereco, int clienteId);
        Task DeleteEnderecoClienteAsync(Endereco endereco, int clientId);
    }
}
