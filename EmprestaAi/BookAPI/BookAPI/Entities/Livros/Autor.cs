using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Livros
{
	public class Autor
	{
        public int Id { get; set; }

		[MaxLength(100)]
		public string Name { get; set; } = string.Empty;

		public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }
}
