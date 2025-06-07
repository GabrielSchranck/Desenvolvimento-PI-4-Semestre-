import { Cliente } from "./Cliente";

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
  livroAnunciadoDTO?: LivroAnunciadoDTO;
  livrosAnunciados?: LivroAnunciadoDTO[];
}

export class LivroAnunciadoDTO {
  id?: number;
  clienteId?: number;
  LivroId? : number;
  tipo?: number;
  quantidadeAnunciado?: number;
  livroDTO?: LivroDTO;
  clienteDTO?: Cliente;
}