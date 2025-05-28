using BookAPI.Entities.Livros;
using BookModels.DTOs.Livros;
using Microsoft.Win32.SafeHandles;

namespace BookAPI.Services.Livros
{
	public interface ILivroServices
	{
		Task CadastrarLivroCliente(LivroDTO livroDTO, int clienteId);
		Task<IEnumerable<Livro>> GetAll();
		Task<IEnumerable<LivroDTO>> GetAll(int clienteId);
		Task Update(LivroDTO livroDTO);
		Task Delete(LivroDTO livroDTO);
		Task<FotoLivro> GetImgBook(string titulo);
		Task<IEnumerable<Categoria>> GetCategorias();
		Task SaveImagemLivro(ImagemLivroDTO imagemLivroDTO);
    }
}
