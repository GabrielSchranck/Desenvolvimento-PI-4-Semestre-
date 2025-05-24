using BookAPI.Entities.Clientes;
using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Historicos;
using BookAPI.Entities.Livros;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Entities.Livros
{
	public class Livro
	{
		public int Id { get; set; }
        public int CategoriaId { get; set; }
        public int AutorId { get; set; }

		[MaxLength(100)]
		public string Titulo { get; set; } = string.Empty;

		[Column(TypeName = "decimal(18,2)")]
		public decimal Valor { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Custo { get; set; }

		public int QtdPaginas { get; set; }
		public int Quantidade { get; set; }

        public Cliente? Cliente { get; set; }
        public Autor? Autor { get; set; }
        public Categoria? Categoria { get; set; }

        public ICollection<FotoLivro> FotosLivros { get; set; } = new List<FotoLivro>();
        public ICollection<ClienteLivro> CLientesLivros { get; set; } = new List<ClienteLivro>();
        public ICollection<ItemHistorico> ItensHistorico { get; set; } = new List<ItemHistorico>();
	}
}
