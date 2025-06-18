﻿using Azure.Core;
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
                await this.SaveImagemLivro(imagemLivro, false);
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
            if (livroDTO.Imagem != null)
            {
                var imagemLivro = new ImagemLivroDTO
                {
                    ClienteId = livroDTO.ClienteId,
                    Imagem = livroDTO.Imagem,
                    LivroId = (int)livroDTO.Id,
                    ImagemUrl = livroDTO.UriImagemLivro ?? ""
                };

                await SaveImagemLivro(imagemLivro, true);
            }

            var livro = livroDTO.ConverteLivroDTOParaLivro();
            await _livroRepository.UpdateAsync(livro);
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

        public async Task SaveImagemLivro(ImagemLivroDTO imagemLivroDTO, bool ehEdicao)
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

            if (!ehEdicao) await _livroImagemRepository.SaveImage(livroFoto);
            else await _livroImagemRepository.Update(livroFoto);
        }

        public async Task AnunciarLivroAsync(LivroAnunciadoDTO livroAnunciadoDTO)
        {
            if(livroAnunciadoDTO != null) await _livroRepository.AnunciarLivroAsync(livroAnunciadoDTO);
        }

        public async Task CancelarAnuncioAsync(LivroAnunciadoDTO livroAnunciadoDTO)
        {
            if(livroAnunciadoDTO != null) await _livroRepository.CancelarAnuncioAsync(livroAnunciadoDTO);
        }

        public async Task<IEnumerable<LivroDTO>> SelecionarAnuncios()
        {
            return await _livroRepository.SelecionarAnuncios();
        }

        public async Task<LivroAnunciadoDTO> GetLivroAnunciadoDTO(int livroId, int tipo)
        {
            return await _livroRepository.GetAnuncioDTO(livroId, tipo);
        }

        public Task<IEnumerable<LivroDTO>> GetAllRelacionados(int categoriaId, int livroId, int tipo)
        {
            return _livroRepository.GetAllByCategoria(categoriaId, livroId, tipo);
        }

        public async Task<IEnumerable<LivroEmprestadoDTO>> GetLivrosEmprestados(int clienteId)
        {
            return await _livroRepository.GetLivrosEmprestados(clienteId);
        }

        public async Task DevolverLivro(int livroId)
        {
            await _livroRepository.DevolverLivro(livroId);
        }

        public async Task AdicionarComentario(ComentarioLivroDTO comentarioLivroDTO)
        {
            await _livroRepository.AdicionarComentario(comentarioLivroDTO);
        }

        public async Task<IEnumerable<ComentarioLivroDTO>> GetComentarioLivroDTO(int livroId)
        {
            return await _livroRepository.GetComentarios(livroId);
        }

        public async Task ExcluirComentario(int comentarioId)
        {
            await _livroRepository.ExcluirComentario(comentarioId);
        }

        public async Task EditarComentario(ComentarioLivroDTO comentarioLivroDTO)
        {
            await _livroRepository.EditarComentario(comentarioLivroDTO);
        }
    }
}
