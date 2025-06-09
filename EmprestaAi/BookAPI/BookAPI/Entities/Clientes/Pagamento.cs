namespace BookAPI.Entities.Clientes
{
    public class Pagamento
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal Valor { get; set; }
        public string? StripeSessionId { get; set; }
        public string? StripePaymentIntentId { get; set; }
        public string? Status { get; set; } 
        public DateTime DataCriacao { get; set; }

        public Cliente? Cliente { get; set; }
    }

}
