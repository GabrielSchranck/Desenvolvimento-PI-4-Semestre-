using BookAPI.Entities.CEPs;
using BookAPI.Repositories.Clientes;

namespace BookAPI.Services.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId)
        {
            return _clienteRepository.GetClienteEnderecosAsync(clienteId);
        }
    }
}
