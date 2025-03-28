﻿using BookAPI.Entities.CEPs;
using BookAPI.Entities.Clientes;

namespace BookAPI.Repositories.Enderecos
{
    public interface IEnderecoRepository
    {
        Task CreateAsync(Endereco endereco);
        Task<Endereco> GetByCepAsync(string cep);
        Task CreateEnderecoClienteAsync(EnderecoCliente enderecoCliente);
    }
}
