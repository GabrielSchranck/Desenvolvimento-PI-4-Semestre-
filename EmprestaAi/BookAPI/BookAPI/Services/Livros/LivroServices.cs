using BookAPI.Entities.ClientesLivros;
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
        private readonly ILivroRepository? _livroRepository;
        private readonly ILivroImagemRepository _livroImagemRepository;

        public LivroServices(ILivroRepository livroRepository, ILivroImagemRepository livroImagemRepository)
        {
            this._livroRepository = livroRepository;
            _livroImagemRepository = livroImagemRepository;
        }

        public async Task CadastrarLivroCliente(LivroDTO livroDTO, int clienteId)
        {
            var livro = livroDTO.ConverteLivroDTOParaLivro();
            var clienteLivro = new ClienteLivro
            {
                ClienteId = clienteId,
                LivroId = livro.Id
            };

            await Create(livro, clienteLivro);

            if (livroDTO.Imagem == null || livroDTO.Imagem.Length == 0)
            {
                var fotoLivro = new FotoLivro
                {
                    LivroId = livro.Id,
                    UrlImagem = livroDTO.UriImagemLivro
                };

                await this._livroImagemRepository.SaveImage(fotoLivro);
            }
            else
            {
                var imagemLivro = new ImagemLivroDTO
                {
                    Imagem = livroDTO.Imagem,
                    LivroId = livro.Id
                };
                await this.SaveImagemLivro(imagemLivro);
            }
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


        private async Task Create(Livro livro, ClienteLivro clienteLivro)
        {
            await _livroRepository.CreateAsync(livro, clienteLivro);
        }



        public async Task Delete(ClienteLivro clienteLivro)
        {
            await _livroRepository.DeleteAsync(clienteLivro);
        }

        public async Task Update(LivroDTO livroDTO)
        {
            var livro = livroDTO.ConverteLivroDTOParaLivro();

            if (livro == null) _livroRepository.UpdateAsync(livro);
        }

        public Task<IEnumerable<Livro>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LivroDTO>> GetAll(int clienteId)
        {
            return await _livroRepository.GetAllAsync(clienteId);
        }

        public async Task<IEnumerable<Categoria>> GetCategorias()
        {
            return await _livroRepository.GetCategorias();
        }

        public async Task SaveImagemLivro(ImagemLivroDTO imagemLivroDTO)
        {
            if (imagemLivroDTO.Imagem == null || imagemLivroDTO.Imagem.Length == 0)
                throw new ArgumentException("Imagem inválida");

            var nomeArquivo = $"{Guid.NewGuid()}_{imagemLivroDTO.Imagem.FileName}";
            var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await imagemLivroDTO.Imagem.CopyToAsync(stream);
            }

            var urlImagem = Path.Combine("imagens", nomeArquivo).Replace("\\", "/");

            var livroFoto = new FotoLivro
            {
                LivroId = (int)imagemLivroDTO.LivroId,
                UrlImagem = urlImagem
            };

            await _livroImagemRepository.SaveImage(livroFoto);
        }
    }
}
