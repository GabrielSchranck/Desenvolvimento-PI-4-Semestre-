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
    numero?: number;
    complemento?: string;
}