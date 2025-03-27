using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Livros
{
	public class FotoLivroDTO
	{
		public int LivroId { get; set; }
		public int Id { get; set; }
		public string? UrlImagem { get; set; }
	}
}
