using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Entities.Livros;
using BookModels.DTOs.Clientes;
using BookModels.DTOs.Livros;
using System.Linq.Expressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookAPI.mappings
{
	public static class MappingDTOs
	{

		#region ~Gabriel 21/02/2024
		/*
					Aqui é a classe para converter das entidades para DTOs

					Obs: conheço uma biblioteca que faz isso sem precisar ficar codando
					porém desconheço seu funcionamento


					Explicação de o que são DTOs (Data Transfer Object):
					
					Pensa no DTO como uma "cesta" de dados que você coloca dentro de um objeto. 
					Você escolhe exatamente o que colocar nessa cesta, sem incluir lógicas complexas ou comportamento (métodos). 
					É só para transportar dados.

					Por exemplo: 
					Se você tem uma tabela de clientes no banco de dados, pode criar um DTO que contém apenas o nome e o email do
					cliente, ao invés de enviar todos os detalhes, como endereço e telefone. 
					Isso torna o envio de dados mais leve e focado.
				
			
					Basicamente, a aplicaçãoi será assim:

								  __________________________________
						  DTOs   |                                  |
					SPA <------> | Controller <------> Repositories |  <------> Database   
						  DTOs   |__________________________________|                                  
												API
		 */
		#endregion

		public static LivroDTO ConverterLivroParaLivroDTO(this Livro livro)
		{
			return new LivroDTO
			{
				Id = livro.Id,
				AutorId = livro.AutorId,
				Titulo = livro.Titulo,
				Valor = livro.Valor,
				Custo = livro.Custo,
				QtdPaginas = livro.QtdPaginas,
				Quantidade = livro.Quantidade
			};
		}

		public static ClienteDTO ConverterClienteParaClienteDTO(this Cliente cliente)
		{
			return new ClienteDTO
			{
				Id = cliente.Id,
				Nome = cliente.Nome,
				Cpf = cliente.Cpf,
				Email = cliente.Email,
				Contato = cliente.Contato,
				DDD = cliente.DDD,
				Idade = cliente.Idade,
				DataNascimento = cliente.DataNascimento
			};
		}

		public static Cliente ConverterClienteDTOParaCliente(this ClienteDTO clienteDTO)
		{
			var cliente = new Cliente
			{
				Nome = clienteDTO.Nome,
				Email = clienteDTO.Email,
				Senha = clienteDTO.Senha,
				Cpf = clienteDTO.Cpf,
				DDD = clienteDTO.DDD,
				Contato = clienteDTO.Contato,
				DataNascimento = clienteDTO.DataNascimento
			};

			return cliente;
		}

		public static Endereco ConverterEnderecoDTOParaEndereco(this EnderecoDTO enderecoDTO)
		{
			var cep = new Endereco
			{
				Id = enderecoDTO.EnderecoId,
				CodigoCep = enderecoDTO.Cep,
                Bairro = enderecoDTO.Bairro,
                Cidade = enderecoDTO.Cidade,
                Uf = enderecoDTO.Uf,
                Logradouro = enderecoDTO.Rua,
                EnderecosCliente = new List<EnderecoCliente>
                {
                    new EnderecoCliente
					{
						Numero = enderecoDTO.Numero,
                        ClienteId = enderecoDTO.ClienteId,
                        Complemento = enderecoDTO.Complemento,
						Id = enderecoDTO.Id,
                    }
                }
            };

			return cep;
        }

		public static CartaoCliente ConverterCartaoDTOParaCartaoCliente(this CartaoClienteDTO cartaoClienteDTO)
		{
			return new CartaoCliente
			{
				Id = cartaoClienteDTO.Id,
				ClienteId = cartaoClienteDTO.ClienteId,
				NumeroCartao = cartaoClienteDTO.NumeroCartao,
				NomeImpresso = cartaoClienteDTO.NomeImpresso,
				Validade = cartaoClienteDTO.Validade,
				Cvv = cartaoClienteDTO.Cvv,
				Bandeira = cartaoClienteDTO.Bandeira,
			};
		}
	}
}
