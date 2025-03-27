using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Enderecos
{
    public interface IEnderecoRepository
    {
        Task Create(Cep cep);
    }
}
