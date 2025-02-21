using BookAPI.Entities;
using BookAPI.Entities.Clientes;
using BookAPI.Entities.Historicos;
using BookAPI.Entities.Livros;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Data
{
	public class BookDbContext : DbContext
	{
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options) 
        { 
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<ItemHistorico> ItensHistoricos { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<FotoLivro> FotosLivros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
