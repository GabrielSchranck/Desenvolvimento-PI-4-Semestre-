using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Repositories.Clientes;
using BookAPI.Services.Enderecos;
using BookAPI.Services.Token;
using BookModels.DTOs.Clientes;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;

namespace BookAPI.Services.Clientes
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;


        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        //Endereços do cliente
        public async Task CreateEnderecoClienteAsync(Endereco endereco, int clienteId)
        {
            var result = endereco.EnderecosCliente.FirstOrDefault();

            var enderecoCliente = new EnderecoCliente
            {
                EnderecoId = endereco.Id,
                ClienteId = clienteId,
                Numero = result.Numero,
                Complemento = result.Complemento

            };

            await _clienteRepository.CreateEnderecoCliente(enderecoCliente);
        }

        public async Task<Cliente> FindByToken(string token)
        {
            return await _clienteRepository.FindByTokenAsync(token);
        }

        public Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId)
        {
            return _clienteRepository.GetClienteEnderecosAsync(clienteId);
        }

        //Cliente
        public async Task<int> GetClienteIdByTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token)) return 0;
            return (int)await TokenService.GetClientIdFromToken(token);
        }

        public async Task SendEmail(string token, IConfiguration configuration, Cliente cliente)
        {
            await Task.Run(() =>
            {
                var link = $"http://localhost:4200/verificar-email?token={token}";

                string email = configuration["SMTP:UserName"];
                string None = configuration["SMTP:Nome"];
                string host = configuration["SMTP:Host"];
                string password = configuration["SMTP:Password"];
                string port = configuration["SMTP:Port"];



                var menssagem = new MailMessage(email, cliente.Email);
                menssagem.Subject = "Verificação de email";
                menssagem.Body = $"Clique no link para verificar seu e-mail: {link}";

                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;

                using var smtp = new SmtpClient("smtp.dominio.com")
                {
                    Port = int.Parse(port),
                    Credentials = new NetworkCredential(email, password),
                    EnableSsl = true,
                };
                smtp.Send(menssagem);
            });
        }
    }
}
