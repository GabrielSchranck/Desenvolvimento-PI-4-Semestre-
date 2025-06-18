﻿using BookAPI.Data;
using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;
using BookAPI.Entities.Livros;
using BookAPI.Entities.Notificacoes;
using Microsoft.EntityFrameworkCore;
using System.Management;

namespace BookAPI.Repositories.Clientes
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BookDbContext _context;

        public ClienteRepository(BookDbContext context)
        {
            this._context = context;
        }

        public async Task Create(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
        }
        public async Task CreateEnderecoCliente(EnderecoCliente enderecoCliente)
        {
            await _context.EnderecosClientes.AddAsync(enderecoCliente);
            await _context.SaveChangesAsync();
        }
        public async Task FecharNotificacao(int notificacaoId)
        {
            var notificacao = await _context.Notificacoes.Where(n => n.Id == notificacaoId).FirstOrDefaultAsync();

            notificacao.Visto = 1;

            _context.Notificacoes.Update(notificacao);
            await _context.SaveChangesAsync();
        }
        public async Task<Cliente> FindByTokenAsync(string token)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.TokenConfirmacao == token);

            if (cliente != null)
            {
                cliente.EmailConfirmado = true;
                _context.Entry(cliente).Property(c => c.EmailConfirmado).IsModified = true;
                _context.SaveChanges();
                return cliente;
            }
            return null;
        }
        public async Task<IEnumerable<Cliente>> GetAllClientAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
        public async Task<bool> GetByCpfAsync(string cpf)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Cpf == cpf);

            if (cliente == null)
                return false;

            return true;
        }
        public async Task<bool> GetByEmailAsync(string email)
        {
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);

            if (cliente == null)
                return false;

            return true;
        }
        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Cliente> GetClienteByEmailAsync(string email)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
        }
        public async Task<IEnumerable<Endereco>> GetClienteEnderecosAsync(int clienteId)
        {
            var enderecoIds = await _context.EnderecosClientes
                .Where(ec => ec.ClienteId == clienteId)
                .Select(ec => ec.EnderecoId)
                .ToListAsync();

            var enderecosClientes = await _context.EnderecosClientes.Where(ec => ec.ClienteId == clienteId).ToListAsync();

            var enderecos = await _context.Enderecos
                .Where(e => enderecoIds.Contains(e.Id))
                .ToListAsync();

            return enderecos;
        }
        public async Task<IEnumerable<Notificacao>> GetNotificacao(int clienteId)
        {
            var notificacoes = await _context.Notificacoes.Where(n => n.VendedorId == clienteId && n.Visto == 0).ToListAsync();

            foreach (var notificacao in notificacoes)
            {
                notificacao.Livro = await _context.Livros.Where(l => l.Id == notificacao.LivroId).FirstOrDefaultAsync();
            }

            return notificacoes;
        }
        public async Task<Cliente> Login(string email, string senha)
        {
            return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email && c.Senha == senha);
        }
        public async Task Sacar(int clienteId, decimal saldo)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);
            var saque = await _context.Saques.FindAsync(clienteId);

            if (saque == null)
            {
                saque = new Saque
                {
                    Cliente = cliente,
                    ClienteId = clienteId,
                    Id = 0,
                    Sacado = false,
                    Saldo = saldo,
                    DataSaque = DateTime.Now
                };
                cliente.Saldo -= (double)saldo;

                _context.Clientes.Update(cliente);
                await _context.Saques.AddAsync(saque);
                await _context.SaveChangesAsync();
                return;
            }

            saque.Saldo += saldo;
            cliente.Saldo -= (double)saldo;

            _context.Clientes.Update(cliente);
            _context.Saques.Update(saque);
            await _context.SaveChangesAsync();
        }
        public async Task Update(Cliente cliente)
        {
            var clienteExistente = await _context.Clientes.FindAsync(cliente.Id);

            if (clienteExistente != null)
            {
                clienteExistente.Nome = cliente.Nome;
                clienteExistente.Email = cliente.Email;
                clienteExistente.DataNascimento = cliente.DataNascimento;
                clienteExistente.Contato = cliente.Contato;

                _context.Entry(clienteExistente).Property(c => c.Nome).IsModified = true;
                _context.Entry(clienteExistente).Property(c => c.Email).IsModified = true;
                _context.Entry(clienteExistente).Property(c => c.DataNascimento).IsModified = true;
                _context.Entry(clienteExistente).Property(c => c.Contato).IsModified = true;

                await _context.SaveChangesAsync();
            }
        }
    }
}
