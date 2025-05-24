using BookAPI.Entities.Clientes;

namespace BookAPI.Services.Autenticadores
{
    public interface IAutenticadorClienteService
    {
        Task<IDictionary<string, string>> AutenticarClienteAoCriar(Cliente cliente);
        Task<int> GetIdadeAsync(DateTime dataNascimento);
    }
}
