using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookModels.DTOs.Clientes;
using System.ComponentModel;
using BookModels.DTOs.Livros;

namespace BookModels.DTOs.Notificacoes
{
    //public enum TipoOperacao
    //{
    //    [Description("VENDA")]
    //    Venda,

    //    [Description("EMPRESTIMO")]
    //    Emprestimo,

    //    [Description("DOACAO")]
    //    Doacao
    //}

    public class NotificacaoDTO
    {
        public int? Id { get; set; }
        public int? Tipo { get; set; }
        public int? Notificado { get; set; } = 0;
        public int? Visto { get; set; } = 0;
        public int? VendedorId { get; set; }
        public int? CompradorId { get; set; }
        public int? LivroId { get; set; }
        public string? Mensagem { get; set; } = string.Empty;
        public ClienteDTO? Comprador { get; set; }
        public ClienteDTO? Vendedor { get; set; }
        public LivroDTO? LivroDTO { get; set; }
    }
}
