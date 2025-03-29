using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Repositories.Enderecos;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace BookAPI.Services.Enderecos
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;

        public EnderecoService(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        class DadosRetornados
        {
            public string? Cep { get; set; }
            public string? Logradouro { get; set; }
            public string? Complemento { get; set; }
            public string? Bairro { get; set; }
            public string? Localidade { get; set; }
            public string? Uf { get; set; }
            public string? Unidade { get; set; }
            public string? Ibge { get; set; }
            public string? Gia { get; set; }
        }

        public async Task<Endereco> GetEnderecoByApi(string cep)
        {

            var endereco = await _enderecoRepository.GetByCepAsync(cep);

            if (endereco != null) return endereco;

            endereco = new Endereco();

            string url = $"https://viacep.com.br/ws/{cep}/json/";
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        DadosRetornados? dados = JsonSerializer.Deserialize<DadosRetornados>(content, options);


                        endereco.CodigoCep = cep;
                        endereco.Logradouro = dados.Logradouro;
                        endereco.Cidade = dados.Localidade;
                        endereco.Bairro = dados.Bairro;
                        endereco.Uf = dados.Uf;

                        await CreateAsync(endereco);

                        return endereco;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public async Task CreateAsync(Endereco endereco)
        {
            await _enderecoRepository.CreateAsync(endereco);
        }

        public async Task CreateEnderecoCliente(Endereco endereco, int clienteId)
        {
            var enderecoBanco = await _enderecoRepository.GetByCepAsync(endereco.CodigoCep);

            if (enderecoBanco != null)
            {
                await _enderecoRepository.CreateEnderecoClienteAsync(new EnderecoCliente
                {
                    ClienteId = clienteId,
                    Complemento = endereco.EnderecosCliente.FirstOrDefault(e => e.ClienteId == clienteId).Complemento,
                    Numero = endereco.EnderecosCliente.FirstOrDefault(e => e.ClienteId == clienteId).Numero,
                    EnderecoId = enderecoBanco.Id
                });
            }
            else
            {
                await CreateAsync(endereco);
                await _enderecoRepository.CreateEnderecoClienteAsync(new EnderecoCliente
                {
                    ClienteId = clienteId,
                    Complemento = endereco.EnderecosCliente.FirstOrDefault(e => e.ClienteId == clienteId).Complemento,
                    Numero = endereco.EnderecosCliente.FirstOrDefault(e => e.ClienteId == clienteId).Numero,
                    EnderecoId = endereco.Id
                });
            }
        }
    }
}
