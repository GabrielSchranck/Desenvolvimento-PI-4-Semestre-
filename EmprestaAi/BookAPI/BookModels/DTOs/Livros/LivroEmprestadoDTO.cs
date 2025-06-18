using BookModels.DTOs.Clientes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Livros
{
    public class LivroEmprestadoDTO
    {
        public int? Id { get; set; }
        public int? LivroId { get; set; }
        public int? VendedorId { get; set; }
        public int? CompradorId { get; set; }
        public DateTime? DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public bool? Devolvido { get; set; } = false;
        public LivroDTO? Livro { get; set; }
        public ClienteDTO? Vendedor { get; set; }
        public ClienteDTO? Comprador { get; set; }
    }
}
