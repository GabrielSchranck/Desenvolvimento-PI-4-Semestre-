using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Clientes
{
    public interface IClienteRepository
    {
        void Create(Cliente cliente);
    }
}
