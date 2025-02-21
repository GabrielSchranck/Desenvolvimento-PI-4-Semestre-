using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Entities.Historicos
{
	public class ItemHistorico
	{
        public int HistoricoId { get; set; }
        public int Id { get; set; }
        public int LivroId { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Valor { get; set; }

		[Column(TypeName = "decimal(10,2)")]
		public decimal Custo { get; set; }
        public int Quantidade { get; set; }

        public Historico? Historico { get; set; }
        public Livro? Livro { get; set; }
    }
}
