using BookAPI.Entities.CEPs;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Clientes
{
	public class EnderecoCliente
	{
		public int ClienteId { get; set; }
		public int EnderecoId { get; set; }

		[Key]
		public int Id { get; set; }
		public int Numero { get; set; }

		[MaxLength(100)]
		public string Logradouro { get; set; } = string.Empty;

		public Endereco? Endereco { get; set; }
		public Cliente? Cliente { get; set; }
	}

}
