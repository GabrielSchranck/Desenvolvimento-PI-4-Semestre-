using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Clientes
{
    public class CriarPagamentoDto
    {
        public int ClienteId { get; set; }
        public decimal Valor { get; set; }
    }

}
