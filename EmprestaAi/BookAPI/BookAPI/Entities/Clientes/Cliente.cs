using BookAPI.Entities.Historicos;
using System.ComponentModel.DataAnnotations;
using BookAPI.Entities.Livros;
using System.ComponentModel;
using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Carrinhos;


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

		[MaxLength(15)]
        public string Contato { get; set; } = string.Empty;

        [MaxLength(2)]
        public int DDD { get; set; }

        [PasswordPropertyText]
        public string? Senha { get; set; }

        public double? Saldo { get; set; } = 0;

        public string? uuidMercadoPago { get; set; }

        public bool EmailConfirmado { get; set; } = false;
        public string? TokenConfirmacao { get; set; }

        public Carrinho? Carrinho { get; set; }
        public ICollection<EnderecoCliente> Enderecos { get; set; } = new List<EnderecoCliente>();
        public ICollection<ClienteLivro> ClientesLivros { get; set; } = new List<ClienteLivro>();
        public ICollection<Historico> Historicos { get; set; } = new List<Historico>();
        public ICollection<CartaoCliente> CartaoClientes { get; set; } = new List<CartaoCliente>();
	}
}
