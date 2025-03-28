using BookAPI.Entities.CEPs;

namespace BookAPI.Services.Clientes
{
    public interface IClienteService
    {
        Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId);
    }
}
