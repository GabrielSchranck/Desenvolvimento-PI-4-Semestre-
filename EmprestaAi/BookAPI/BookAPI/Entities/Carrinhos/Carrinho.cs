using BookAPI.Entities.Clientes;

namespace BookAPI.Entities.Carrinhos
{
    public class Carrinho
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }

        public ICollection<ItemCarrinho>? Itens { get; set; } = new List<ItemCarrinho>();

    }
}
