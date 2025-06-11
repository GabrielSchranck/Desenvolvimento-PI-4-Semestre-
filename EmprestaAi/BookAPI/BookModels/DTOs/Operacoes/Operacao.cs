using BookModels.DTOs.Livros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Operacoes
{
    public class Operacao
    {
        public LivroAnunciadoDTO? LivroAnunciadoDTO { get; set; }
        public int? enderecoId { get; set; }
        public int? tipo { get; set; }
        public int? Quantidade { get; set; }
    }
}
