using BookAPI.Entities.CEPs;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Clientes
{
	public class Endereco
	{
		public int ClienteId { get; set; }
		public int CepId { get; set; }

		[Key]
		public int Id { get; set; }
		public int Numero { get; set; }

		[MaxLength(100)]
		public string Logradouro { get; set; } = string.Empty;

		public Cep? Cep { get; set; }
		public Cliente? Cliente { get; set; }
	}

}
