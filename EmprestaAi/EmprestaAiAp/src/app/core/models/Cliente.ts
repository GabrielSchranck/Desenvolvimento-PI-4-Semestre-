export class Cliente {
    Id?: number;
    Nome?: string;
    Email?: string;
    Cpf?: string;
    Contato?: string;
    DDD?: number;
    Idade?: number;
    DataNascimento?: string;
    Senha?: string;
    Enderecos?: EnderecoCliente[];  
}

export class EnderecoCliente{
    id?: number;
    numero?: number;
    complemento?: string;
    cep?: string;
    rua?: string;
    bairro?: string;
    cidade?: string;
    uf?: string;
}