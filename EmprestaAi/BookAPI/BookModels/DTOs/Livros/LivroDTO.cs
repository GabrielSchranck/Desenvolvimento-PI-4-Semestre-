using BookModels.DTOs.Clientes;
using Microsoft.AspNetCore.Http;
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
        public int? CategoriaId { get; set; }
        public string? Titulo { get; set; }
		public decimal? Valor { get; set; }
		public decimal? Custo { get; set; }
		public int? QtdPaginas { get; set; }
		public int? Quantidade { get; set; }
        public string? UriImagemLivro { get; set; }
        public IFormFile? Imagem { get; set; }
		public bool? Anunciado { get; set; }
        public ClienteDTO? ClienteDTO { get; set; }
        //public string? Descricao { get; set; }
        public LivroAnunciadoDTO? LivroAnunciadoDTO { get; set; }
        public List<LivroAnunciadoDTO>? LivrosAnunciados { get; set; }
    }
}
