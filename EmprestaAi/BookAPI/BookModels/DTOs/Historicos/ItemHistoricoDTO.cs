using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Historicos
{
	public class ItemHistoricoDTO
	{
		public int HistoricoId { get; set; }
		public int Id { get; set; }
		public int LivroId { get; set; }
		public decimal Valor { get; set; }
		public decimal Custo { get; set; }
		public int Quantidade { get; set; }
	}
}
