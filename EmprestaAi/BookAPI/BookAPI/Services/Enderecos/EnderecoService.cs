using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Repositories.Enderecos;
using BookModels.DTOs.Clientes;
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

                        if (endereco.Cidade == null) return null;

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
            var result = await _enderecoRepository.GetByCepAsync(endereco.CodigoCep);

            if (result != null) return;

            await _enderecoRepository.CreateAsync(endereco);
        }

        public async Task CreateEnderecoCliente(Endereco endereco, int clienteId)
        {
            var enderecoBanco = await _enderecoRepository.GetByCepAsync(endereco.CodigoCep);

            if (enderecoBanco != null)
            {
                var enderecoCLiente = new EnderecoCliente
                {
                    ClienteId = clienteId,
                    Complemento = endereco.EnderecosCliente.FirstOrDefault().Complemento,
                    Numero = endereco.EnderecosCliente.FirstOrDefault().Numero,
                    EnderecoId = enderecoBanco.Id
                };

				if (!(await _enderecoRepository.FindEnderecoClienteAsync(enderecoCLiente)))
                {
					await _enderecoRepository.CreateEnderecoClienteAsync(enderecoCLiente);
				}
            }
            else
            {
                await CreateAsync(endereco);

                await _enderecoRepository.CreateEnderecoClienteAsync(new EnderecoCliente
                {
                    ClienteId = clienteId,
                    Complemento = endereco.EnderecosCliente.FirstOrDefault().Complemento,
                    Numero = endereco.EnderecosCliente.FirstOrDefault().Numero,
                    EnderecoId = endereco.Id
                });
            }
        }

		public async Task UpdateEnderecoClienteAsync(Endereco endereco, int clienteId)
		{
            await _enderecoRepository.UpdateEnderecoAsync(endereco, clienteId);
        }

		public async Task DeleteEnderecoClienteAsync(Endereco endereco, int clientId)
		{
			foreach(var enderecoCliente in endereco.EnderecosCliente)
            {
                enderecoCliente.ClienteId = clientId;
				await _enderecoRepository.DeleteEnderecoClienteAsync (enderecoCliente);
			}
		}
	}
}
