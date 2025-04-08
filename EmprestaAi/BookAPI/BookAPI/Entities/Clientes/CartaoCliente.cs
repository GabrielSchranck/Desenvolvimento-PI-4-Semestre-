namespace BookAPI.Entities.Clientes
{
	public class CartaoCliente
	{
		public int Id { get; set; }
		public int ClienteId { get; set; }
		public string? NumeroCartao { get; set; } = string.Empty;
		public string? NomeImpresso { get; set; } = string.Empty;
		public DateTime Validade { get; set; }
		public string? Cvv { get; set; } = string.Empty;
		public string? Bandeira { get; set; } = string.Empty;

		public Cliente? Cliente { get; set; }
	}

}
