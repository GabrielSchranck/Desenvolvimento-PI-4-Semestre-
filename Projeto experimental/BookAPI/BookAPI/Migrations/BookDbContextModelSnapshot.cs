﻿// <auto-generated />
using System;
using BookAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookAPI.Migrations
{
    [DbContext(typeof(BookDbContext))]
    partial class BookDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BookAPI.Entities.CEPs.Cep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CepCod")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Uf")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("Id");

                    b.ToTable("Ceps");
                });

            modelBuilder.Entity("BookAPI.Entities.Clientes.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contato")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<int>("DDD")
                        .HasMaxLength(2)
                        .HasColumnType("int");

                    b.Property<DateTime>("DataNascimento")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("EmailConfirmado")
                        .HasColumnType("bit");

                    b.Property<int>("Genero")
                        .HasMaxLength(1)
                        .HasColumnType("int");

                    b.Property<int>("Idade")
                        .HasMaxLength(3)
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Senha")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TokenConfirmacao")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("BookAPI.Entities.Clientes.Endereco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CepId")
                        .HasColumnType("int");

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Numero")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CepId");

                    b.HasIndex("ClienteId");

                    b.ToTable("Enderecos");
                });

            modelBuilder.Entity("BookAPI.Entities.ClientesLivros.ClienteLivro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<int>("LivroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.HasIndex("LivroId");

                    b.ToTable("ClientesLivros");
                });

            modelBuilder.Entity("BookAPI.Entities.Historicos.Historico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ClienteId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataHora")
                        .HasColumnType("datetime2");

                    b.Property<int>("TipoOperacao")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ClienteId");

                    b.ToTable("Historicos");
                });

            modelBuilder.Entity("BookAPI.Entities.Historicos.ItemHistorico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Custo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("HistoricoId")
                        .HasColumnType("int");

                    b.Property<int>("LivroId")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("HistoricoId");

                    b.HasIndex("LivroId");

                    b.ToTable("ItensHistoricos");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NomeCategoria")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.FotoLivro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LivroId")
                        .HasColumnType("int");

                    b.Property<string>("UrlImagem")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("LivroId");

                    b.ToTable("FotosLivros");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Livro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<decimal>("Custo")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("QtdPaginas")
                        .HasColumnType("int");

                    b.Property<int>("Quantidade")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Valor")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("AutorId");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("ClienteId");

                    b.ToTable("Livros");
                });

            modelBuilder.Entity("BookAPI.Entities.Clientes.Endereco", b =>
                {
                    b.HasOne("BookAPI.Entities.CEPs.Cep", "Cep")
                        .WithMany("Enderecos")
                        .HasForeignKey("CepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookAPI.Entities.Clientes.Cliente", "Cliente")
                        .WithMany("Enderecos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cep");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("BookAPI.Entities.ClientesLivros.ClienteLivro", b =>
                {
                    b.HasOne("BookAPI.Entities.Clientes.Cliente", "Cliente")
                        .WithMany("ClientesLivros")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookAPI.Entities.Livros.Livro", "Livro")
                        .WithMany("CLientesLivros")
                        .HasForeignKey("LivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("BookAPI.Entities.Historicos.Historico", b =>
                {
                    b.HasOne("BookAPI.Entities.Clientes.Cliente", "Cliente")
                        .WithMany("Historicos")
                        .HasForeignKey("ClienteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("BookAPI.Entities.Historicos.ItemHistorico", b =>
                {
                    b.HasOne("BookAPI.Entities.Historicos.Historico", "Historico")
                        .WithMany("ItensHistorico")
                        .HasForeignKey("HistoricoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookAPI.Entities.Livros.Livro", "Livro")
                        .WithMany("ItensHistorico")
                        .HasForeignKey("LivroId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Historico");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.FotoLivro", b =>
                {
                    b.HasOne("BookAPI.Entities.Livros.Livro", "Livro")
                        .WithMany("FotosLivros")
                        .HasForeignKey("LivroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Livro", b =>
                {
                    b.HasOne("BookAPI.Entities.Livros.Autor", "Autor")
                        .WithMany("Livros")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookAPI.Entities.Livros.Categoria", "Categoria")
                        .WithMany("Livros")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookAPI.Entities.Clientes.Cliente", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteId");

                    b.Navigation("Autor");

                    b.Navigation("Categoria");

                    b.Navigation("Cliente");
                });

            modelBuilder.Entity("BookAPI.Entities.CEPs.Cep", b =>
                {
                    b.Navigation("Enderecos");
                });

            modelBuilder.Entity("BookAPI.Entities.Clientes.Cliente", b =>
                {
                    b.Navigation("ClientesLivros");

                    b.Navigation("Enderecos");

                    b.Navigation("Historicos");
                });

            modelBuilder.Entity("BookAPI.Entities.Historicos.Historico", b =>
                {
                    b.Navigation("ItensHistorico");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Autor", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Categoria", b =>
                {
                    b.Navigation("Livros");
                });

            modelBuilder.Entity("BookAPI.Entities.Livros.Livro", b =>
                {
                    b.Navigation("CLientesLivros");

                    b.Navigation("FotosLivros");

                    b.Navigation("ItensHistorico");
                });
#pragma warning restore 612, 618
        }
    }
}
