using BookAPI.Entities.Historicos;
using System.ComponentModel.DataAnnotations;
using BookAPI.Entities.Livros;
using System.ComponentModel;
using BookAPI.Entities.ClientesLivros;


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

        [MaxLength(3)]
		public int Idade { get; set; }
		public DateTime DataNascimento { get; set; }

        [MaxLength(1)]
		public int Genero { get; set; }

		[MaxLength(9)]
        public string Contato { get; set; } = string.Empty;

        [MaxLength(2)]
        public int DDD { get; set; }

        [PasswordPropertyText]
        public string? Senha { get; set; }

        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<ClienteLivro> ClientesLivros { get; set; } = new List<ClienteLivro>();
        public ICollection<Historico> Historicos { get; set; } = new List<Historico>();
    }
}
