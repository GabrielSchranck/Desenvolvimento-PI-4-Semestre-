using BookAPI.Entities.Historicos;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Clientes
{
	public class Cliente
	{
		public int Id { get; set; }

		[MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [MaxLength(11)]
        public string Cpf { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(9)]
        public string Contato { get; set; } = string.Empty;

        [MaxLength(2)]
        public int DDD { get; set; }

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
        public ICollection<Historico> Historicos { get; set; } = new List<Historico>();
    }
}
