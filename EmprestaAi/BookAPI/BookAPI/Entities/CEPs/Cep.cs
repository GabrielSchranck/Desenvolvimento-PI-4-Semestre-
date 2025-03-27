using BookAPI.Entities.Clientes;
using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.CEPs
{
	public class Cep
	{
        public int Id { get; set; }

		[MaxLength(8)]
		public string CepCod { get; set; } = string.Empty;

		[MaxLength(100)]
		public string Bairro { get; set; } = string.Empty;

		[MaxLength(100)]
		public string Cidade { get; set; } = string.Empty;

		[MaxLength(2)]
		public string Uf { get; set; } = string.Empty;

		public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
    }
}
