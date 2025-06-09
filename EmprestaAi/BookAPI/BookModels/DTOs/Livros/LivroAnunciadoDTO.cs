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
        public decimal? valorTaxa
        {
            get
            {
                if (Tipo == 0 && LivroDTO?.Valor != null)
                {
                    return LivroDTO.Valor * 15 / 100;
                }

                return 0;
            }
        }

        public int Tipo { get; set; }
    }
}
