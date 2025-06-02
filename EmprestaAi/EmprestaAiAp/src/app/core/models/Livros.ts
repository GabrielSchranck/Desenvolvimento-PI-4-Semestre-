export class LivroDTO {
  id?: number;
  clienteId?: number;
  categoriaId?: number;
  titulo?: string;
  valor?: number;
  custo?: number;
  qtdPaginas?: number;
  quantidade?: number;
  uriImagemLivro?: string;
  anunciado?: boolean;
  LivroAnunciadoDTO?: LivroAnunciadoDTO;
  LivrosAnunciados?: LivroAnunciadoDTO[];
}

export class LivroAnunciadoDTO {
  id?: number;
  clienteId?: number;
  LivroId? : number;
  Tipo?: number;
  quantidadeAnunciado?: number;
}