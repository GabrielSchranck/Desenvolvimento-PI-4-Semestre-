using BookAPI.Entities.Clientes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookAPI.Entities.Livros
{
    public class ComentarioLivro
    {
        [Key]
        public int Id { get; set; }
        public int LivroId { get; set; }
        public int ClienteId { get; set; }
        public string Comentario { get; set; }
        public DateTime DataComentario { get; set; }
        [ForeignKey("LivroId")]
        public Livro? Livro { get; set; }
        [ForeignKey("ClienteId")]
        public Cliente? Cliente { get; set; }
    }
}
