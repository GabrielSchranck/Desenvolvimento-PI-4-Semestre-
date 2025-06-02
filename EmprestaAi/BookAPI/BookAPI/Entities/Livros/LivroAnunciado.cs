using BookAPI.Entities.Clientes;

namespace BookAPI.Entities.Livros
{
    public class LivroAnunciado
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int LivroId { get; set; }
        public int QuantidadeAnunciado { get; set; }
        public int Tipo { get; set; } // 0 - Venda, 1 - empréstimo, 2 - Doação
        public Cliente? Cliente { get; set; }
        public Livro? Livro { get; set; }
    }
}
