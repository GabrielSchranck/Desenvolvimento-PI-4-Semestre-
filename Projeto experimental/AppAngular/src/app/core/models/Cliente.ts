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
    Enderecos?: Endereco[];  
}

export class Endereco{
    Cep?: string;
    Numero?: number;
    Rua?: string;
    Complemento?: string;
    Bairro?: string;
    Cidade?: string;
    Uf?: string;
}