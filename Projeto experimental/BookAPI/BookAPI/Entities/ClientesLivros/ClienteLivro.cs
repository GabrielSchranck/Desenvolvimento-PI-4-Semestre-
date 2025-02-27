using BookAPI.Entities.Clientes;
using BookAPI.Entities.Livros;

namespace BookAPI.Entities.ClientesLivros
{
    public class ClienteLivro
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int LivroId { get; set; }

        public Cliente? Cliente { get; set; }
        public Livro? Livro { get; set; }
    }
}
