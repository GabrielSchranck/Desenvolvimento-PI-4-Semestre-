using BookAPI.Entities.Livros;
using BookAPI.mappings;
using BookAPI.Repositories.Livros;
using BookModels.DTOs.Livros;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

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

            var fotoLivro = await GetImgBook(livro.Titulo);
            fotoLivro.LivroId = livro.Id;
        }

        public async Task<FotoLivro> GetImgBook(string titulo)
        {
            string url = $"https://www.googleapis.com/books/v1/volumes?q={Uri.EscapeDataString(titulo)}";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(json);

                if (result.items != null && result.items.Count > 0)
                {
                    var volumeInfo = result.items[0].volumeInfo;

                    if (volumeInfo.imageLinks != null)
                    {
                        string thumbnail = volumeInfo.imageLinks.thumbnail != null ?
                                           (string)volumeInfo.imageLinks.thumbnail :
                                           (string)volumeInfo.imageLinks.smallThumbnail;

                        if (!string.IsNullOrEmpty(thumbnail))
                        {
                            return new FotoLivro
                            {
                                UrlImagem = thumbnail.Replace("http://", "https://") // às vezes vem com http
                            };
                        }
                    }
                }

                return null;
            }
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
