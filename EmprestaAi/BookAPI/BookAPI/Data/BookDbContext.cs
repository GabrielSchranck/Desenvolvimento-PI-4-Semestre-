using BookAPI.Entities;
using BookAPI.Entities.Carrinhos;
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
        //public DbSet<Autor> Autores { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<FotoLivro> FotosLivros { get; set; }
        public DbSet<ClienteLivro> ClientesLivros { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
		public DbSet<CartaoCliente> CartoesClientes { get; set; }
        public DbSet<LivroAnunciado> LivrosAnunciados { get; set; }
        public DbSet<Carrinho> Carrinho { get; set; }
        public DbSet<ItemCarrinho> ItemCarrinho { get; set; }

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

            modelBuilder.Entity<Categoria>().HasData(
                new Categoria { Id = 1, NomeCategoria = "Ficção" },
                new Categoria { Id = 2, NomeCategoria = "Fantasia" },
                new Categoria { Id = 3, NomeCategoria = "Romance" },
                new Categoria { Id = 4, NomeCategoria = "Terror" },
                new Categoria { Id = 5, NomeCategoria = "Suspense" },
                new Categoria { Id = 6, NomeCategoria = "Mistério" },
                new Categoria { Id = 7, NomeCategoria = "Drama" },
                new Categoria { Id = 8, NomeCategoria = "Aventura" },
                new Categoria { Id = 9, NomeCategoria = "Ficção Científica" },
                new Categoria { Id = 10, NomeCategoria = "Biografia" },
                new Categoria { Id = 11, NomeCategoria = "Autobiografia" },
                new Categoria { Id = 12, NomeCategoria = "História" },
                new Categoria { Id = 13, NomeCategoria = "Religião" },
                new Categoria { Id = 14, NomeCategoria = "Espiritualidade" },
                new Categoria { Id = 15, NomeCategoria = "Autoajuda" },
                new Categoria { Id = 16, NomeCategoria = "Negócios" },
                new Categoria { Id = 17, NomeCategoria = "Finanças" },
                new Categoria { Id = 18, NomeCategoria = "Tecnologia" },
                new Categoria { Id = 19, NomeCategoria = "Programação" },
                new Categoria { Id = 20, NomeCategoria = "Ciência" },
                new Categoria { Id = 21, NomeCategoria = "Filosofia" },
                new Categoria { Id = 22, NomeCategoria = "Psicologia" },
                new Categoria { Id = 23, NomeCategoria = "Educação" },
                new Categoria { Id = 24, NomeCategoria = "Direito" },
                new Categoria { Id = 25, NomeCategoria = "Política" },
                new Categoria { Id = 26, NomeCategoria = "Saúde" },
                new Categoria { Id = 27, NomeCategoria = "Medicina" },
                new Categoria { Id = 28, NomeCategoria = "Esportes" },
                new Categoria { Id = 29, NomeCategoria = "Culinária" },
                new Categoria { Id = 30, NomeCategoria = "Arte" },
                new Categoria { Id = 31, NomeCategoria = "Música" },
                new Categoria { Id = 32, NomeCategoria = "Quadrinhos" },
                new Categoria { Id = 33, NomeCategoria = "Mangá" },
                new Categoria { Id = 34, NomeCategoria = "Infantil" },
                new Categoria { Id = 35, NomeCategoria = "Juvenil" },
                new Categoria { Id = 36, NomeCategoria = "Poesia" },
                new Categoria { Id = 37, NomeCategoria = "Humor" },
                new Categoria { Id = 38, NomeCategoria = "Clássicos" },
                new Categoria { Id = 39, NomeCategoria = "Erótico" },
                new Categoria { Id = 40, NomeCategoria = "Viagem" }
            );

            modelBuilder.Entity<ItemCarrinho>()
            .HasOne(ic => ic.LivroAnunciado)
            .WithMany()
            .HasForeignKey(ic => ic.LivroAnunciadoId)
            .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
