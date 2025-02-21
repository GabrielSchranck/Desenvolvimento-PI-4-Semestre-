using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Livros
{
	public class FotoLivro
	{
        public int LivroId { get; set; }
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string UrlImagem { get; set; } = string.Empty;

        public Livro? Livro { get; set; }
    }
}
