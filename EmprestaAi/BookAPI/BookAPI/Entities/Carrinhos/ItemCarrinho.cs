using BookAPI.Entities.Livros;

namespace BookAPI.Entities.Carrinhos
{
    public class ItemCarrinho
    {
        public int Id { get; set; }
        public int CarrinhoId { get; set; }
        public int LivroAnunciadoId { get; set; }
        public int Quantidade { get; set; }

        public LivroAnunciado? LivroAnunciado { get; set; }
        public Carrinho? Carrinho { get; set; }
    }
}
