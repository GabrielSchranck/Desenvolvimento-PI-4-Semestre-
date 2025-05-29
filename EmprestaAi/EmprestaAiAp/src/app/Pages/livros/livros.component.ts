import { Component, OnInit } from '@angular/core';
import { UserInfoComponent } from "../../MainPages/user-info/user-info.component";
import { LivroDTO } from '../../core/models/Livros';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LivroService } from '../../core/services/livro.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-livros',
  imports: [UserInfoComponent],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent implements OnInit {
  livros: LivroDTO[] = [];
  livrosEmprestados: LivroDTO[] = [];
  abrirModal: boolean = false;
  modalAnuciar: boolean = false;
  formLivro!: FormGroup;

  constructor(private router: Router, private formBuilder: FormBuilder, private livroService: LivroService){}
  
  async ngOnInit(): Promise<void> {
    this.buscarLivros();
  }

  public GoToAddLivro(){
    this.router.navigate(['/registraLivro']);
  }

  public async buscarLivros() {
    try {
      const data: LivroDTO[] = await this.livroService.getLivros();
      this.livros = data;
    } catch (error) {
      console.error('Erro ao buscar livros:', error);
    }
  }

  public async deleteLivro(livroId: number): Promise<void> {
    const result = await Swal.fire({
      title: 'Tem certeza que deseja excluir?',
      text: 'Esta ação não pode ser desfeita.',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim, excluir',
      cancelButtonText: 'Cancelar',
      reverseButtons: true,
      focusCancel: true,
      customClass: {
        confirmButton: 'bg-red-600 hover:bg-red-700 text-white font-medium px-4 py-2 rounded-md',
        cancelButton:  'bg-gray-200 hover:bg-gray-300 text-gray-800 font-medium px-4 py-2 rounded-md'
      },
      buttonsStyling: false
    });

    if (result.isConfirmed) {
      try {
        const mensagem = await this.livroService.delete(livroId).toPromise();

        await Swal.fire({
          title: 'Erro',
          text: 'Não foi possível excluir o livro.',
          icon: 'error',
          confirmButtonText: 'Fechar',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });
        
      } 
      catch (err: any) {
        await Swal.fire({
          title: 'Excluído!',
          text: 'O livro foi removido com sucesso.',
          icon: 'success',
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'bg-[#3596D2] hover:bg-[#287bb3] text-white font-medium px-4 py-2 rounded-md'
          },
          buttonsStyling: false
        });

        this.atualizarListaDeLivros();
      }
    }
  }


  private atualizarListaDeLivros(): void {
    this.buscarLivros();
  }
  
}
