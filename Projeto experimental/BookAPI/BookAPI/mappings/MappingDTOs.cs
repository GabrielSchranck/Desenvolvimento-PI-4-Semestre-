﻿using BookAPI.Entities.Livros;
using BookModels.DTOs.Livros;

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
				ClienteId = livro.ClienteId,
				AutorId = livro.AutorId,
				Titulo = livro.Titulo,
				Valor = livro.Valor,
				Custo = livro.Custo,
				QtdPaginas = livro.QtdPaginas,
				Quantidade = livro.Quantidade
			};
		}
	}
}
