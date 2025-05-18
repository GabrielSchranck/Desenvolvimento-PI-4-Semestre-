using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Livros
{
	public class LivroDTO
	{
		public int? Id { get; set; }
		public int? ClienteId { get; set; }
		public int? AutorId { get; set; }
		public string? Titulo { get; set; }
		public decimal? Valor { get; set; }
		public decimal? Custo { get; set; }
		public int? QtdPaginas { get; set; }
		public int? Quantidade { get; set; }
        public string? UriImagemLivro { get; set; }
    }
}
