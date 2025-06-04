using BookModels.DTOs.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Livros
{
    public class LivroAnunciadoDTO
    {
        public int? Id { get; set; }
        public int? ClienteId { get; set; }
        public int? LivroId { get; set; }
        public int? QuantidadeAnunciado { get; set; }
        public LivroDTO? LivroDTO { get; set; }
        public ClienteDTO? ClienteDTO { get; set; }
        public int Tipo { get; set; }
    }
}
