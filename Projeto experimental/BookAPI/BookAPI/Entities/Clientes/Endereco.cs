using System.ComponentModel.DataAnnotations;

namespace BookAPI.Entities.Clientes
{
	public class Endereco
	{
        public int ClienteId { get; set; }
        public int Id { get; set; }

		[MaxLength(8)]
        public string Cep { get; set; } = string.Empty;
        public int Numero { get; set; }

        [MaxLength(100)]
        public string Logradouro { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [MaxLength(2)]
        public string Uf { get; set; } = string.Empty;

        public Cliente? Cliente { get; set; }
    }
}
