import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { cpf } from 'cpf-cnpj-validator';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { firstValueFrom } from 'rxjs';
import { get } from 'http';
import { TextInputComponent } from "../../components/inputs/text-input/text-input.component";

@Component({
  selector: 'app-perfil',
  imports: [ReactiveFormsModule, TextInputComponent],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit {

  formularioPerfil: FormGroup = new FormGroup({});
  clienteDTO: Cliente = new Cliente();

  constructor(private clienteService: ClienteService) {}

  async ngOnInit(): Promise<void> {
    await this.getPerfil();
    this.inicializarFormulario();
  }

  private inicializarFormulario(): void {
    try {
      this.formularioPerfil = new FormGroup({
        nome: new FormControl(this.clienteDTO?.Nome || '', [Validators.required]),
        email: new FormControl(this.clienteDTO?.Email || '', [Validators.required, Validators.email]),
        cpf: new FormControl(this.clienteDTO?.Cpf || ''),
        idade: new FormControl(this.clienteDTO?.Idade || ''),
        dataNascimento: new FormControl(this.clienteDTO?.DataNascimento || ''),
        genero: new FormControl(this.clienteDTO?.Genero || ''),
        ddd: new FormControl(this.clienteDTO?.DDD || ''),
        contato: new FormControl(this.clienteDTO?.Contato || ''),
      });
    } catch (error) {
      console.error("Erro ao inicializar o formulário:", error);
    }
  }

  private obterIdDoUsuario(): number | null {
    if (typeof window !== 'undefined' && window.localStorage) {
      const user = JSON.parse(localStorage.getItem('authToken') || '{}');
      console.log(user);
      return user?.cliente?.id || null;
    }
    console.error("Erro: localStorage não está disponível.");
    return null;
  }


  private async getPerfil(): Promise<void> {
    const id = this.obterIdDoUsuario();
    if (!id) {
      console.error("Erro: ID do usuário inválido ou ausente.");
      return;
    }

    try {
      this.clienteDTO = await firstValueFrom(this.clienteService.GetById(id));
    } catch (error) {
      console.error("Erro ao buscar cliente:", error);
      this.clienteDTO = new Cliente();
    }
  }

  get nomeControl(): FormControl {
    return this.formularioPerfil.get('nome') as FormControl;
  }

  get emailControl(): FormControl {
    return this.formularioPerfil.get('email') as FormControl;
  }

  get cpfControl(): FormControl {
    return this.formularioPerfil.get('cpf') as FormControl;
  }

}

