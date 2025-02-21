using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Historicos
{
	public class HistoricoDTO
	{
		public int ClienteId { get; set; }
		public int Id { get; set; }
		public int TipoOperacao { get; set; }
		public DateTime DataHora { get; set; }
	}
}
