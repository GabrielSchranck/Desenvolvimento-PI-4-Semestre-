using BookAPI.Entities.ClientesLivros;
using BookAPI.Entities.Livros;
using BookModels.DTOs.Livros;

namespace BookAPI.Repositories.Livros
{
	public interface ILivroRepository
	{
		Task<IEnumerable<LivroDTO>> GetAllAsync(int clientId);
		Task CreateAsync(Livro livro, ClienteLivro clienteLivro);
		Task UpdateAsync(Livro livro);
		Task DeleteAsync(ClienteLivro clienteLivro);
		Task<IEnumerable<Categoria>> GetCategorias();
		Task AnunciarLivroAsync(LivroAnunciadoDTO livroAnunciadoDTO);
		Task CancelarAnuncioAsync(LivroAnunciadoDTO livroAnunciadoDTO);
		Task<IEnumerable<LivroDTO>> SelecionarAnuncios();
		Task<LivroAnunciadoDTO> GetAnuncioDTO(int livroId, int tipo);
		Task<IEnumerable<LivroDTO>> GetAllByCategoria(int categoriaId, int livroId, int tipo);
		Task<IEnumerable<LivroEmprestadoDTO>> GetLivrosEmprestados(int clienteId);
		Task DevolverLivro(int livroId);
		Task AdicionarComentario(ComentarioLivroDTO comentarioLivroDTO);
		Task<IEnumerable<ComentarioLivroDTO>> GetComentarios(int livroId);
		Task ExcluirComentario(int comentarioId);
		Task EditarComentario(ComentarioLivroDTO comentarioLivroDTO);
    }
}
