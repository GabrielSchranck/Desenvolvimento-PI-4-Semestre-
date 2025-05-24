using BookAPI.Entities;
using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Entities.ClientesLivros;
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
        public DbSet<EnderecoCliente> EnderecosClientes { get; set; }
        public DbSet<Historico> Historicos { get; set; }
        public DbSet<ItemHistorico> ItensHistoricos { get; set; }
        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<FotoLivro> FotosLivros { get; set; }
        public DbSet<ClienteLivro> ClientesLivros { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
		public DbSet<CartaoCliente> CartoesClientes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ItemHistorico>()
				.HasKey(ih => ih.Id);

			modelBuilder.Entity<ItemHistorico>()
				.HasOne(ih => ih.Historico)
				.WithMany(h => h.ItensHistorico)
				.HasForeignKey(ih => ih.HistoricoId)
				.OnDelete(DeleteBehavior.Cascade); 

			modelBuilder.Entity<ItemHistorico>()
				.HasOne(ih => ih.Livro)
				.WithMany(l => l.ItensHistorico)
				.HasForeignKey(ih => ih.LivroId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<EnderecoCliente>()
				.HasOne(e => e.Endereco)
				.WithMany(c => c.EnderecosCliente)
				.HasForeignKey(e => e.EnderecoId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Cliente>()
				.HasIndex(c => c.Email)
				.IsUnique();

			modelBuilder.Entity<Cliente>()
				.HasIndex(c => c.Cpf)
				.IsUnique();
        }

	}
}
