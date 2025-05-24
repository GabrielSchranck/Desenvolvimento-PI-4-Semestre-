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

export class Cartao{
  id?: number = 0;
  clienteId?: number = 0;
  numeroCartao?: string;
  nomeImpresso?: string;
  validade?: string;
  cvv?: string;
  bandeira?: string;
}
