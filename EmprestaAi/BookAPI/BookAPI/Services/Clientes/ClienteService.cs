using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Repositories.Clientes;
using BookAPI.Services.Enderecos;
using BookAPI.Services.Token;

namespace BookAPI.Services.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task CreateEnderecoClienteAsync(Endereco endereco, int clienteId)
        {
            var result = endereco.EnderecosCliente.FirstOrDefault();

            var enderecoCliente = new EnderecoCliente
            {
                EnderecoId = endereco.Id,
                ClienteId = clienteId,
                Numero = result.Numero,
                Complemento = result.Complemento

            };

            await _clienteRepository.CreateEnderecoCliente(enderecoCliente);
        }

        public Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId)
        {
            return _clienteRepository.GetClienteEnderecosAsync(clienteId);
        }

        public async Task<int> GetClienteIdByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return 0;
            return (int)await TokenService.GetClientIdFromToken(token);
        }
    }
}
