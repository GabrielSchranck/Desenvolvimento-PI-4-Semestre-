using BookAPI.Entities.Clientes;

namespace BookAPI.Entities.Historicos
{
	public class Historico
	{
        public int ClienteId { get; set; }
        public int Id { get; set; }
        public int TipoOperacao { get; set; }
        public DateTime DataHora { get; set; }

        public Cliente? Cliente { get; set; }
        public ICollection<ItemHistorico> ItensHistorico { get; set; } = new List<ItemHistorico>();
    }
}
