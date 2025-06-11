using BookAPI.Entities.Clientes;
using BookAPI.Entities.Historicos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Entities.Notificacoes
{
    public class Notificacao
    {
        [Key]
        public int Id { get; set; }

        public TipoOperacaoEnum Tipo { get; set; }

        public int Notificado { get; set; } = 0;
        public int Visto { get; set; } = 0;

        public int VendedorId { get; set; }
        public int CompradorId { get; set; }

        [MaxLength(200)]
        public string? Mensagem { get; set; } = string.Empty;

        [ForeignKey("CompradorId")]
        public Cliente? Comprador { get; set; }

        [ForeignKey("VendedorId")]
        public Cliente? Vendedor { get; set; }
    }

}
