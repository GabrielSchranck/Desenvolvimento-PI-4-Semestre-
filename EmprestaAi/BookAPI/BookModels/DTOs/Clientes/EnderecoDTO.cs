using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Clientes
{
	public class EnderecoDTO
	{
		public int? ClienteId { get; set; }
		public int? Id { get; set; }
		public int? EnderecoId { get; set; }
		public string? Cep { get; set; }
		public int? Numero { get; set; }
		public string? Complemento { get; set; }
		public string? Rua { get; set; }
		public string? Bairro { get; set; }
		public string? Cidade { get; set; }
		public string? Uf { get; set; }
	}

}
