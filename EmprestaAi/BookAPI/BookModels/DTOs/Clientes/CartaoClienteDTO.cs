using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Clientes
{
    public class CartaoClienteDTO
    {
		public int Id { get; set; }
		public int ClienteId { get; set; }
		public string? NumeroCartao { get; set; } = string.Empty;
		public string? NomeImpresso { get; set; } = string.Empty;
		public DateTime Validade { get; set; }
		public string? Cvv { get; set; } = string.Empty;
		public string? Bandeira { get; set; } = string.Empty;
	}
}
