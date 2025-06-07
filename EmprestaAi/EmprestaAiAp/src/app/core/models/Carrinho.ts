import { LivroAnunciadoDTO } from "./Livros";

export class CarrinhoDTO{
    id?: number;
    clienteId?: number;
    itens: ItemCarrinhoDTO[] = [];
}

export class ItemCarrinhoDTO {
    id?: number;
    carrinhoId?: number;
    livroId?: number;
    quantidade?: number;
    livroAnunciadoDTO?: LivroAnunciadoDTO;
}