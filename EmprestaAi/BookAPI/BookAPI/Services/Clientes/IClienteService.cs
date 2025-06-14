using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Entities.Notificacoes;
using BookModels.DTOs.Clientes;

namespace BookAPI.Services.Clientes
{
    public interface IClienteService
    {
        Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId);
        Task<int> GetClienteIdByTokenAsync(string token);
        Task CreateEnderecoClienteAsync(Endereco endereco, int clienteId);
        Task SendEmail(string token, IConfiguration configuration, Cliente cliente);
        Task<Cliente> FindByToken(string token);
        Task<IEnumerable<Notificacao>> GetNotificacoes(int clienteId);
        Task FecharNotificacao(int notificacaoId);
    }
}
