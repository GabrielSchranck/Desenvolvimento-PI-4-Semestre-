import { LivroDTO } from "./Livros";

export class Notificacao {
  id?: number;
  tipo?: number;
  notificado: number = 0;
  visto: number = 0;
  vendedorId?: number;
  compradorId?: number;
  mensagem: string = '';
  livroDTO?: LivroDTO;
  livroId?: number;
}