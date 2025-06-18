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
  categoria?: string;
}

export class LivroAnunciadoDTO {
  id?: number;
  clienteId?: number;
  LivroId? : number;
  tipo?: number;
  quantidadeAnunciado?: number;
  livroDTO?: LivroDTO;
  clienteDTO?: Cliente;
  valorTaxa?: number;
}

export class LivroEmprestado {
  id?: number;
  livroId?: number;
  vendedorId?: number;
  compradorId?: number;
  dataEmprestimo?: Date;
  dataDevolucao?: Date;
  devolvido?: boolean = false;
  livro?: LivroDTO;
  vendedor?: Cliente;
  comprador?: Cliente;
}

export class ComentarioLivroDTO {
  id?: number;
  clienteId?: number;
  livroId?: number;
  comentario?: string;
  dataComentario?: Date;
  livroDTO?: LivroDTO;
  clienteDTO?: Cliente;
  editar?: boolean = false;
}
