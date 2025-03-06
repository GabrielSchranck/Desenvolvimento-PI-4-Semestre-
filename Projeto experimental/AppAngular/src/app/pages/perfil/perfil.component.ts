import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators  } from '@angular/forms';
import { cpf } from 'cpf-cnpj-validator';
import { Cliente } from '../../core/models/Cliente';
import { ClienteService } from '../../core/services/cliente.service';
import { firstValueFrom } from 'rxjs';
import { get } from 'http';

@Component({
  selector: 'app-perfil',
  imports: [ReactiveFormsModule],
  templateUrl: './perfil.component.html',
  styleUrl: './perfil.component.css'
})
export class PerfilComponent implements OnInit {

  formularioPerfil: any;
  clienteDTO: Cliente = new Cliente();

  constructor(private clienteService: ClienteService) { }

  async ngOnInit(): Promise<void> {
    await this.getPerfil().then(() => {
      this.inicializarFormulario();
    });
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
    var user = JSON.parse(localStorage.getItem('authToken') || '{}');
    return user.cliente.id;
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
    }
  }
  

}
