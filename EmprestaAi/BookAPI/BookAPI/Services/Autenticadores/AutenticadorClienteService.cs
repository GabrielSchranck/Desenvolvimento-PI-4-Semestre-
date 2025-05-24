using BookAPI.Entities.Clientes;
using BookAPI.Repositories.Clientes;

namespace BookAPI.Services.Autenticadores
{
    public class AutenticadorClienteService : IAutenticadorClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        Task<int> IAutenticadorClienteService.GetIdadeAsync(DateTime dataNascimento) => GetIdadeAsync(dataNascimento);
        public AutenticadorClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<IDictionary<string, string>> AutenticarClienteAoCriar(Cliente cliente)
        {
            var erros = new Dictionary<string, string>();

            if (await ExisteEmailAsync(cliente))
                erros.Add("email", "Email já cadastrado");

            if (await ExisteCpfAsync(cliente.Cpf))
                erros.Add("cpf", "CPF já cadastrado");

            return erros;
        }
        private async Task<bool> ExisteEmailAsync(Cliente cliente)
        {
            return await _clienteRepository.GetByEmailAsync(cliente.Email);
        }
        private async Task<bool> ExisteCpfAsync(string cpf)
        {
            return await _clienteRepository.GetByCpfAsync(cpf);
        }
        private async Task<int> GetIdadeAsync(DateTime dataNascimento)
        {
            try
            {
                var idade = DateTime.Now.Year - dataNascimento.Year;
                return idade;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
