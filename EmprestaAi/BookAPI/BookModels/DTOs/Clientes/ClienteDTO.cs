using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Clientes
{
	public class ClienteDTO
	{
		public int Id { get; set; }
		public string? Nome { get; set; }
		public string? Cpf { get; set; }
		public string? Email { get; set; }
		public string? Contato { get; set; }
		public int DDD { get; set; }
		public int Idade { get; set; }
		public DateTime DataNascimento { get; set; }
		public string? Genero { get; set; }
        public string? Senha { get; set; }
        public string? uuidMercadoPago { get; set; }
        public double? Saldo { get; set; }
        //public IEnumerable<EnderecoDTO> EnderecoDTOs { get; set; }
    }
}
