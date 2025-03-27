using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Livros
{
    public class Categoria
    {
        public int Id { get; set; }

        [MaxLength(30)]
        public string NomeCategoria { get; set; } = string.Empty;

        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }
}
