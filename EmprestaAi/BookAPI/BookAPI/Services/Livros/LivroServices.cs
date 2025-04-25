using BookAPI.Entities.Livros;
using BookAPI.mappings;
using BookAPI.Repositories.Livros;
using BookModels.DTOs.Livros;

namespace BookAPI.Services.Livros
{
	public class LivroServices : ILivroServices
	{
		private readonly ILivroRepository? _repository;

		public LivroServices(ILivroRepository livroRepository)
		{
			this._repository = livroRepository;
		}

		public async Task CadastrarLivroCliente(LivroDTO livroDTO)
		{
			//Cadastrar livro do cliente

			var livro = livroDTO.ConverteLivroDTOParaLivro();
			await Create(livro);
		}

		private async Task Create(Livro livro)
		{
			await _repository.CreateAsync(livro);
		}

		public async Task Delete(LivroDTO livroDTO)
		{
			var livro = livroDTO.ConverteLivroDTOParaLivro();

			if (livro == null) _repository.DeleteAsync(livro);
		}

		public async Task Update(LivroDTO livroDTO)
		{
			var livro = livroDTO.ConverteLivroDTOParaLivro();

			if (livro == null) _repository.UpdateAsync(livro);
		}

		public Task<IEnumerable<Livro>> GetAll()
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Livro>> GetAll(int clienteId)
		{
			throw new NotImplementedException();
		}
	}
}
