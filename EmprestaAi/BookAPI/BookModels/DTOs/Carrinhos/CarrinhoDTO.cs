using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Carrinhos
{
    public class CarrinhoDTO
    {
        public int? Id { get; set; } = 0;
        public int? ClienteId { get; set; } = 0;
        public IEnumerable<ItensCarrinhoDTO>? Itens { get; set; } = new List<ItensCarrinhoDTO>();
    }
}
