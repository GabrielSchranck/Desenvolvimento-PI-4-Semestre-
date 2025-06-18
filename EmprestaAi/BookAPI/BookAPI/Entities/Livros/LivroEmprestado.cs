using BookAPI.Entities.Clientes;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Entities.Livros
{
    public class LivroEmprestado
    {
        [Key]
        public int Id { get; set; }

        public int LivroId { get; set; }

        public int VendedorId { get; set; }

        public int CompradorId { get; set; }

        public DateTime DataEmprestimo { get; set; }

        public DateTime DataDevolucao { get; set; }

        public bool Devolvido { get; set; } = false;

        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }

        [ForeignKey("VendedorId")]
        public Cliente? Vendedor { get; set; }

        [ForeignKey("CompradorId")]
        public Cliente? Comprador { get; set; }
    }
}
