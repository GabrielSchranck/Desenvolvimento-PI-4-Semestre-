using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Entities.Clientes
{
    public class Saque
    {
        [Key]
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public decimal Saldo { get; set; }
        public bool Sacado { get; set; } = false;
        public DateTime DataSaque { get; set; }

        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
    }
}
