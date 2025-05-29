using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookModels.DTOs.Livros
{
    public class ImagemLivroDTO
    {
        public int? ClienteId { get; set; }
        public int? LivroId { get; set; }
        public IFormFile? Imagem { get; set; }
        public string? ImagemUrl { get; set; }
        public IEnumerable<string>? ImagensUrl { get; set; }
    }
}
