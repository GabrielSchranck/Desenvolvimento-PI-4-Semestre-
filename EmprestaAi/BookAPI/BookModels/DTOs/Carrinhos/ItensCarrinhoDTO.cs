using BookModels.DTOs.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Carrinhos
{
    public class ItensCarrinhoDTO
    {
        public int Id { get; set; }
        public int? Quantidade { get; set; }
        public int? Tipo { get; set; }
        public LivroDTO? Livro { get; set; } = null;
        public CarrinhoDTO? CarrinhoDTO { get; set; }
        public LivroAnunciadoDTO? LivroAnunciadoDTO { get; set; }
    }
}
